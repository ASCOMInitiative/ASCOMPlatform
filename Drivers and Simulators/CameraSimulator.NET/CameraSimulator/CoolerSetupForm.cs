using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ASCOM.Simulator
{
    public partial class CoolerSetupForm : Form
    {
        private const decimal MINIMUM_TEMPERATURE = -273.15m; // Absolute zero as a decimal constant
        private const decimal MAXIMUM_TEMPERATURE = decimal.MaxValue; // A huge upper limit
        private const decimal DELTATMAX_MINIMUM = 1.0m; // Minimum permissible value of maximum delta T - Recommended to be at least 1.0 in order to avoid infinities arising in the cooler constant calculation
        private const decimal FLUCTUATIONS_MINIMUM = 0.0m; // Minimum permissible value for random temperature fluctuations
        private const decimal OVERSHOOT_MINIMUM = 0.0m; // Minimum permissible value for the overshoot temperature increment
        private const decimal TIMETOSETPOINT_MINIMUM = 0.1m; // Minimum permissible value for the time to setpoint
        private const decimal TIMETOSETPOINT_MAXIMUM = decimal.MaxValue; // Maximum permissible value for the time to setpoint
        private const decimal NUMBER_OF_UNDER_DAMPED_CYCLES_MINIMUM = 0.0m; // Minimum number of permissible under damped cycles
        private const decimal NUMBER_OF_UNDER_DAMPED_CYCLES_MAXIMUM = 50.0m; // Maximum number of permissible under damped cycles

        private const int GRAPH_REFRESH_FREQUENCY = 2; // Cooling graph refresh rate (Refreshes per second)

        private Camera camera; // Private variable that holds a reference to the main camera object where data persistence code is located
        private bool coolingConfigurationChanged = false;

        private Timer graphRefreshTimer = new Timer();

        #region Initialiser and Form Load

        /// <summary>
        /// Cooler setup form initialiser
        /// </summary>
        public CoolerSetupForm()
        {
            try
            {
                InitializeComponent();
                Log.LogMessage("CoolerSetupForm", "Starting initialisation.");
                cmbCoolerModes.DrawMode = DrawMode.OwnerDrawFixed; // Add event handler to give the combo box have a white background when in list mode.
                cmbCoolerModes.DrawItem += DropDownListComboBox_DrawItem;
                graphRefreshTimer = new Timer();
                graphRefreshTimer.Tick += new EventHandler(GraphRefreshTimer_Tick);
                Log.LogMessage("CoolerSetupForm", "Completed initialisation.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Initialiser - " + ex.ToString());
                Log.LogMessageCrLf("Initialiser", ex.ToString());
            }
        }

        /// <summary>
        /// Cooler configuration form load event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CoolerSetupForm_Load(object sender, EventArgs e)
        {
            try
            {
                const string DOUBLE_SPACE = "\r\n\r\n";
                const string SINGLE_SPACE = "\r\n";

                #region Descriptive help text for the COOLING CONFIGURATION FORM and controls

                // Set help text for the AMBIENT TEMPERATURE numeric control
                CoolingHelp.SetHelpString(NumAmbientTemperature,
                    "Sets the ambient environment temperature" + DOUBLE_SPACE +
                    "This value is returned through the Camera.HeatSinkTemperature property" + DOUBLE_SPACE +
                    "Must be less than or equal to the [SETPOINT + MAXIMUM_DELTA_T] temperature." + DOUBLE_SPACE +
                    "Must be greater than or equal to the [SETPOINT] temperature." + DOUBLE_SPACE +
                    "NOTES:" + SINGLE_SPACE +
                    "1) The SetCCDTemperature property will throw an invalid value exception if the requested CCD temperature is above the value configured here."
                    );
                CoolingHelp.SetHelpString(LblAmbientTemperature, CoolingHelp.GetHelpString(NumAmbientTemperature)); // Add help text to the control label

                // Set help text for the default SETPOINT temperature numeric control
                CoolingHelp.SetHelpString(NumCCDSetPoint,
                    "Sets the camera's default setpoint temperature." + DOUBLE_SPACE +
                    "This value is set and returned through the Camera.SetCCDTemperature property." + DOUBLE_SPACE +
                    "Must be less than or equal to [AMBIENT] temperature." + DOUBLE_SPACE +
                    "Must be greater than or equal to [LOWEST_SETTABLE]."
                    );
                CoolingHelp.SetHelpString(LblCCDSetPoint, CoolingHelp.GetHelpString(NumCCDSetPoint)); // Add help text to the control label

                // Set help text for the MAXIMUM TEMPERATURE DIFFERENTIAL numeric control
                CoolingHelp.SetHelpString(NumCoolerDeltaTMax,
                    "Sets the maximum temperature differential from ambient that the cooler can maintain." + DOUBLE_SPACE +
                    "Must be less than or equal to the difference between the [AMBIENT] and [SETPOINT] temperatures." + DOUBLE_SPACE +
                    string.Format("Must be greater than or equal to {0:0.0}.", DELTATMAX_MINIMUM) + DOUBLE_SPACE +
                    "NOTES:" + SINGLE_SPACE +
                    "1) Over most of the cooling cycle and at the setpoint, the Camera.CoolerPower property is calculated from the CCD temperature expressed as a fraction of the cooler" + SINGLE_SPACE +
                    "   temperature range: [POWER] = [AMBIENT - CCD_TEMPERATURE] / [MAXIMUM_DELTA_T]." + SINGLE_SPACE +
                    "2) The CoolerPower property will return 100% in the early stages of cooling and 0% in the early stages of warming to simulate the cooler driving the temperature change as fast as possible."
                    );
                CoolingHelp.SetHelpString(LblCoolerDeltaTMax, CoolingHelp.GetHelpString(NumCoolerDeltaTMax)); // Add help text to the control label

                // Set help text for the SETPOINT MINIMUM numeric control
                CoolingHelp.SetHelpString(NumSetpointMinimum,
                    "Sets the minimum temperature to which the cooler can be set without receiving an invalid value exception." + DOUBLE_SPACE +
                    "When the setpoint is within the cooler's achievable temperature range, [AMBIENT] to [AMBIENT - MAXIMUM_DELTA_T], the setpoint will be achieved within the configured time." + DOUBLE_SPACE +
                    "If the setpoint is between [AMBIENT - MAXIMUM_DELTA_T] and the value configured here, the CCD temperature will exhibit the expected behaviour and fall to [AMBIENT - MAXIMUM_DELTA_T] but " + SINGLE_SPACE +
                    "will not go below this value." + DOUBLE_SPACE +
                    "Must be less than or equal to [AMBIENT - MAXIMUM_DELTA_T]." + DOUBLE_SPACE +
                    "NOTES:" + SINGLE_SPACE +
                    "1) The SetCCDTemperature property will throw an invalid value exception if the requested CCD temperature is below the value configured here."
                    );
                CoolingHelp.SetHelpString(LblSetpointMinimum, CoolingHelp.GetHelpString(NumSetpointMinimum)); // Add help text to the control label

                // Set help text for the TIME TO GET TO SETPOINT numeric control
                CoolingHelp.SetHelpString(NumTimeToSetPoint,
                    "Sets the time taken to arrive at the maximum cooling temperature when cooling from ambient." + DOUBLE_SPACE +
                    "Temperature changes of less than the maximum temperature difference will complete in less than the specified time." + DOUBLE_SPACE +
                    string.Format("Must be greater than or equal to {0:0.0}.", TIMETOSETPOINT_MINIMUM) + DOUBLE_SPACE +
                    "NOTES:" + SINGLE_SPACE +
                    "1) Not supported in \"Always at setpoint\" mode." + SINGLE_SPACE +
                    "2) When using the \"Well behaved\" mode and cooling over the full default temperature range of 40C, the CCD will be:" + SINGLE_SPACE +
                    "     a) Within 2.0C of the setpoint after 50% of the cooling time " + SINGLE_SPACE +
                    "     b) Within 1.0C of the setpoint after 60% of the cooling time" + SINGLE_SPACE +
                    "     c) Within 0.5C of the setpoint after 70% of the cooling time" + SINGLE_SPACE +
                    "     d) Within 0.1C of the setpoint after 88% of the cooling time." + SINGLE_SPACE +
                    "     e) At the setpoint after 100% of the cooling time." + SINGLE_SPACE +
                    "3) For smaller temperature changes, the CCD temperature will achieve the setpoint earlier than indicated above."
                  );
                CoolingHelp.SetHelpString(LblTimeToSetPoint, CoolingHelp.GetHelpString(NumTimeToSetPoint)); // Add help text to the control label

                // Set help text for the OVERSHOOT numeric control
                CoolingHelp.SetHelpString(NumOvershoot,
                    "Sets the amount of temperature overshoot that occurs when the CCD set point temperature is changed." + DOUBLE_SPACE +
                    "When overshoot is 0.0, the reported CCD temperature will follow an exponential, Newton's cooling equation, path to the [SETPOINT] temperature." + DOUBLE_SPACE +
                    "In \"Single overshoot\" mode the reported CCD temperature will overshoot by moving smoothly through the setpoint and, half way through the cooling cycle, arriving at the" + SINGLE_SPACE +
                    "[SETPOINT - OVERSHOOT] temperature. It will then undershoot back through the setpoint before joining the Newton's curve and following this to the [SETPOINT] temperature." + DOUBLE_SPACE +
                    "In \"Under damped\" mode , the reported CCD temperature will show a sine wave oscillation whose maximum amplitude is set here and which decreases linearly over the cooling cycle" + DOUBLE_SPACE +
                    string.Format("Must be greater than or equal to {0:0.0}.", OVERSHOOT_MINIMUM) + DOUBLE_SPACE +
                    "NOTES:" + SINGLE_SPACE +
                    "1) Not supported in \"Well behaved\", \"Always at setpoint\" and \"Never gets to setpoint\" modes." + SINGLE_SPACE +
                    "2) When warming, the CCD temperature will show the converse behaviour to that shown when cooling." + SINGLE_SPACE +
                    "3) In \"Single overshoot\" mode the full temperature overshoot will always be exhibited, regardless of the size of [SETPOINT] temperature change." + SINGLE_SPACE +
                    "4) No overshoot will be exhibited when the cooler is turned off and the CCD temperature warms to [AMBIENT] temperature on its own." + SINGLE_SPACE +
                    "5) The reported CCD temperature will be truncated to [AMBIENT] or [AMBIENT - MAXIMUM_DELTA_T] as appropriate, whenever the calculated CCD temperature falls outside this range."
                    );
                CoolingHelp.SetHelpString(LblOvershoot, CoolingHelp.GetHelpString(NumOvershoot)); // Add help text to the control label

                // Set help text for the UNDERDAMPED CYCLES numeric control
                CoolingHelp.SetHelpString(NumUnderDampedCycles,
                    "Sets the number of oscillatory half cycles that will be applied to the cooling curve." + DOUBLE_SPACE +
                    "In \"Under damped\" mode, the CCD temperature will display a sine wave temperature variation, with a maximum amplitude of \"Cooler overshoot\" degrees, " + SINGLE_SPACE +
                    "from the Newton's cooling curve temperature. The number of half cycles (Pi radians) set here will be fitted into the configured time to setpoint." + DOUBLE_SPACE +
                    "The sine wave amplitude reduces linearly during the cooling cycle resulting in a curve that increasing approximates Newton's curve as the cooling cycle progresses." + DOUBLE_SPACE +
                    "Must be greater than or equal to 0." + DOUBLE_SPACE +
                    "NOTES:" + SINGLE_SPACE +
                    "1) Only supported in \"Under damped mode\" ." + SINGLE_SPACE +
                    "2) The maximum overshoot will be less that the value configured in \"Cooler overshoot\"." + SINGLE_SPACE +
                    "3) If the number of cycles is odd, the CCD temperature will approach the setpoint from the overshoot side, if it is even, the CCD temperature will approach from the starting temperature side."
                    );
                CoolingHelp.SetHelpString(LblOvershootCycles, CoolingHelp.GetHelpString(NumUnderDampedCycles)); // Add help text to the control label

                // Set help text for the RANDOM FLUCTUATIONS numeric control
                CoolingHelp.SetHelpString(NumFluctuations,
                    "Sets the size of random temperature fluctuations added to the CCD and heat sink temperatures." + DOUBLE_SPACE +
                    "The returned CCD and heat sink temperatures will exhibit random fluctuations of up to ± this value. Set to 0.00 for fully reproducible behaviour," + DOUBLE_SPACE +
                    string.Format("Must be greater than or equal to {0:0.0}.", FLUCTUATIONS_MINIMUM)
                    );
                CoolingHelp.SetHelpString(LblFluctuations, CoolingHelp.GetHelpString(NumFluctuations)); // Add help text to the control label

                // Set help text for the COOLER MODES drop-down
                CoolingHelp.SetHelpString(cmbCoolerModes,
                    "Selects the cooler's overall behavioural characteristics." + DOUBLE_SPACE +
                    "The \"Well behaved\" mode uses Newton's cooling equation to arrive smoothly at the setpoint in the configured time when cooling from ambient to the maximum cooling temperature." + SINGLE_SPACE +
                    "If the starting temperature is lower than ambient, the cooler will take less time to arrive at the setpoint. This is the default behaviour and is suitable for most purposes." + DOUBLE_SPACE +
                    "The \"Single overshoot\" mode uses Newton's cooling equation as its majority component but also includes an \"overshoot\" component that gradually increases through the first 46% of the cooling cycle" + SINGLE_SPACE +
                    "causing the reported CCD temperature to pass through the setpoint and eventually exceed it by the configured amount. The overshoot contribution then reduces to zero over the next 20% of the cooling cycle" + SINGLE_SPACE +
                    "leaving the remaining 30% of the cycle to follow Newton's curve to the setpoint" + DOUBLE_SPACE +
                    "The \"Under damped\" mode uses Newton's cooling equation as its majority component but also includes an oscillatory \"overshoot\" component that adds a sine wave contribution whose amplitude linearly" + SINGLE_SPACE +
                    "decreases over the cooling cycle. The amplitude of the sine wave and the number of sine wave half cycles can be configured to create a wide variety of behavioural characteristics." + DOUBLE_SPACE +
                    "The \"Always at setpoint\" mode goes immediately to the setpoint when the cooler is turned on or the setpoint is changed." + DOUBLE_SPACE +
                    string.Format("The \"Never gets to setpoint\" mode uses Newton's cooling equation but under reports the CCD temperature by {0:P0} of the difference between ambient and the setpoint.", Camera.COOLER_NEVER_GETS_TO_SETPOINT_REDUCTION_FACTOR) + DOUBLE_SPACE +
                    "NOTES:" + SINGLE_SPACE +
                    "1) Setting the temperature below the maximum achievable temperature but above the lowest settable temperature will result in the CCD temperature stabilising at the maximum achievable temperature." + SINGLE_SPACE +
                    "2) Attempting to set the CCD temperature below the lowest settable temperature will result in an InvalidValueException."
                    );

                // Set help text for the RESET TO AMBIENT checkbox
                CoolingHelp.SetHelpString(ChkResetToAmbientOnConnect,
                    "Sets whether or not to reset the CCD temperature to ambient when the cooler is enabled or the setpoint is changed." + DOUBLE_SPACE +
                    "The cooler will normally warm up or cool down from its current temperature, ambient on connection, but subsequently any temperature within the cooler's range, depending on previous cooling history." + SINGLE_SPACE +
                    "In addition, the camera has a \"warm up\" characteristic when cooling is turned off, which makes the cooler warm from its current temperature to ambient in the specified \"cool down\" time." + DOUBLE_SPACE +
                    "If this checkbox is unchecked the CCD temperature will behave as described above." + DOUBLE_SPACE +
                    "If this checkbox is checked, the CCD temperature will immediately reset to ambient when cooling is turned off; it will also reset to ambient when the setpoint changes, before cooling to the new setpoint." + SINGLE_SPACE +
                    "(This is a driver development aid to ensure that a reproducible, full, cooling curve is delivered every time the cooler is turned on or the setpoint changes.)"
                    );

                // Set help text for the COOLER ENABLED AT POWER UP checkbox
                CoolingHelp.SetHelpString(ChkPowerUpState,
                    "Sets whether the cooler is enabled when the camera is initially connected." + DOUBLE_SPACE +
                    "If checked, the camera will start the cooler when the Connected property is set to True and will immediately commence a cooling cycle to the configured setpoint. The CoolerOn property will immediately return True." + DOUBLE_SPACE +
                    "If unchecked, the cooler will not run until the application sets the CoolerOn property to True."
                    );

                // Set help text for the OK button
                CoolingHelp.SetHelpString(BtnOK,
                    "Closes the form and saves any changes." + DOUBLE_SPACE +
                    "The new configuration will be saved to the ASCOM Profile store."
                    );

                // Set help text for the Cancel button
                CoolingHelp.SetHelpString(BtnCancel,
                    "Closes the form without saving any changes." + DOUBLE_SPACE +
                    "Original configuration values will be retained and all changes discarded."
                    );

                CoolingHelp.SetHelpString(ChkFitCurveToScreen,
                    "Graph temperature range selection." + DOUBLE_SPACE +
                    "Toggles between a graph view focused on the cooling curve and a view covering the full ambient to minimum setpoint temperature range."
                    );

                // This empty picture box is present to provide a control that covers the whole form area. This approach is used because the HelpProvider doesn't work when clicking the form itself.
                CoolingHelp.SetHelpString(BackgroundPictureBox,
                    "Cooler Configuration Form - Sets the cooler's control and behavioural characteristics." + DOUBLE_SPACE +
                    "Configures the Cooler's default control values including ambient temperature, setpoint, time to setpoint and maximum cooling range together with its overall cooling behaviour."
                    );
                CoolingHelp.SetHelpString(LblHelpText, CoolingHelp.GetHelpString(BackgroundPictureBox)); // Add help text to the help instructions label
                CoolingHelp.SetHelpString(LblGraph, CoolingHelp.GetHelpString(BackgroundPictureBox)); // Add help text to the graph descriptive label

                CoolingHelp.SetHelpString(CoolingChart,
                    "Cooling curve graph based on set configuration." + DOUBLE_SPACE +
                    "The graph shows how the CCD temperature varies over time in response to the cooler being turned on or the CCD set temperature being changed. " + DOUBLE_SPACE +
                    "It also shows the temperature range to which the CCD can be set, but that the cooler can never deliver, as well as the temperature below which InvalidValueExceptions will be thrown when setting the CCD temperature."
                    );

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("CoolerSetupForm_Load - " + ex.ToString());
                Log.LogMessageCrLf("CoolerSetupForm_Load", ex.ToString());
            }
        }

        /// <summary>
        /// Initialise the cooler configuration form controls and variables
        /// </summary>
        /// <param name="theCamera">Camera object used to persist changed configuration.</param>
        /// <remarks>This is called from the setup form to initialise the cooler configuration form.</remarks>
        internal void InitProperties(Camera theCamera)
        {
            try
            {
                Log.LogMessage("CoolerForm-InitProperties", "Started initialising properties.");
                camera = theCamera; // Save the supplied Camera object so that it can be used to persist any cooling variable changes

                // Configure the graph refresh timer
                graphRefreshTimer.Interval = 1000 / GRAPH_REFRESH_FREQUENCY;
                graphRefreshTimer.Start();
                coolingConfigurationChanged = false;
                Log.LogMessage("CoolerForm-InitProperties", "Enabled graph refresh timer: {0}, Frequency: {1}, Timer interval: {2}", graphRefreshTimer.Enabled, GRAPH_REFRESH_FREQUENCY, graphRefreshTimer.Interval);

                // Set minimum and maximum values for the numeric controls
                NumAmbientTemperature.Minimum = MINIMUM_TEMPERATURE; NumAmbientTemperature.Maximum = MAXIMUM_TEMPERATURE;
                NumCCDSetPoint.Minimum = MINIMUM_TEMPERATURE; NumCCDSetPoint.Maximum = MAXIMUM_TEMPERATURE;
                NumCoolerDeltaTMax.Minimum = DELTATMAX_MINIMUM; NumCoolerDeltaTMax.Maximum = MAXIMUM_TEMPERATURE;
                NumFluctuations.Minimum = FLUCTUATIONS_MINIMUM; NumFluctuations.Maximum = MAXIMUM_TEMPERATURE;
                NumOvershoot.Minimum = OVERSHOOT_MINIMUM; NumOvershoot.Maximum = MAXIMUM_TEMPERATURE;
                NumTimeToSetPoint.Minimum = TIMETOSETPOINT_MINIMUM; NumTimeToSetPoint.Maximum = TIMETOSETPOINT_MAXIMUM;
                NumUnderDampedCycles.Minimum = NUMBER_OF_UNDER_DAMPED_CYCLES_MINIMUM; NumUnderDampedCycles.Maximum = NUMBER_OF_UNDER_DAMPED_CYCLES_MAXIMUM;
                NumSetpointMinimum.Minimum = MINIMUM_TEMPERATURE; NumSetpointMinimum.Maximum = MAXIMUM_TEMPERATURE;

                // Initialise the controls with current values
                NumAmbientTemperature.Value = (decimal)camera.heatSinkTemperature;
                NumCCDSetPoint.Value = (decimal)camera.setCcdTemperature;
                NumCoolerDeltaTMax.Value = (decimal)camera.coolerDeltaTMax;
                NumTimeToSetPoint.Value = (decimal)camera.coolerTimeToSetPoint;
                NumOvershoot.Value = (decimal)camera.coolerOvershoot;
                NumFluctuations.Value = (decimal)camera.coolerFluctuation;
                NumUnderDampedCycles.Value = (decimal)camera.coolerUnderDampedCycles;
                NumSetpointMinimum.Value = (decimal)camera.coolerSetPointMinimum;
                ChkResetToAmbientOnConnect.Checked = camera.coolerResetToAmbient;
                ChkPowerUpState.Checked = camera.coolerPowerUpState;
                ChkFitCurveToScreen.Checked = camera.coolerGraphRange;

                // Load the cooler modes combo box with options
                foreach (string coolerMode in camera.coolerModes)
                {
                    cmbCoolerModes.Items.Add(coolerMode);
                }

                // Add the camera mode changed event handler here so that the correct options are enabled when the entry is selected
                cmbCoolerModes.SelectedIndexChanged += new EventHandler(CmbCoolerModes_SelectedIndexChanged);
                cmbCoolerModes.SelectedItem = camera.coolerMode;

                // Add event handlers to validate input values - These have to be added after the variables above have been set to avoid inappropriate triggering during initialisation
                NumAmbientTemperature.ValueChanged += new EventHandler(NumAmbientTemperature_ValueChanged);
                NumCCDSetPoint.ValueChanged += new EventHandler(NumCCDSetPoint_ValueChanged);
                NumCoolerDeltaTMax.ValueChanged += new EventHandler(NumCoolerDeltaTMax_ValueChanged);
                NumTimeToSetPoint.ValueChanged += new EventHandler(NumTimeToSetPoint_ValueChanged);
                NumOvershoot.ValueChanged += new EventHandler(NumOvershoot_ValueChanged);
                NumUnderDampedCycles.ValueChanged += new EventHandler(NumUnderDampedCycles_ValueChanged);
                NumSetpointMinimum.ValueChanged += new EventHandler(NumSetpointMinimum_ValueChanged);

                camera.LogCoolerVariables(); // Log the cooler variables

                DrawCoolingCurve(); // Draw the current cooling curve in the chart control

                Log.LogMessage("CoolerForm-InitProperties", "Completed initialising properties.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("CoolerForm-InitProperties - " + ex.ToString());
                Log.LogMessageCrLf("CoolerForm-InitProperties", ex.ToString());
            }
        }

        #endregion

        #region Validation and appearance event handlers

        /// <summary>
        /// Validate ambient temperature changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumAmbientTemperature_ValueChanged(object sender, EventArgs e)
        {
            decimal newAmbientTemperature = ((NumericUpDown)sender).Value;
            if (newAmbientTemperature < NumCCDSetPoint.Value) NumAmbientTemperature.Value = NumCCDSetPoint.Value;
            if ((newAmbientTemperature - NumCCDSetPoint.Value) > NumCoolerDeltaTMax.Value) NumAmbientTemperature.Value = NumCCDSetPoint.Value + NumCoolerDeltaTMax.Value;
            coolingConfigurationChanged = true; // Indicate that the configuration has changed, this will be actioned by the graph redraw timer event handler
        }

        /// <summary>
        /// Validate setpoint changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumCCDSetPoint_ValueChanged(object sender, EventArgs e)
        {
            decimal newCCDSetPoint = ((NumericUpDown)sender).Value;
            if (newCCDSetPoint < NumSetpointMinimum.Value) NumCCDSetPoint.Value = NumSetpointMinimum.Value;
            if (newCCDSetPoint > NumAmbientTemperature.Value) NumCCDSetPoint.Value = NumAmbientTemperature.Value;
            coolingConfigurationChanged = true; // Indicate that the configuration has changed, this will be actioned by the graph redraw timer event handler
        }

        /// <summary>
        /// Validate maximum delta T changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumCoolerDeltaTMax_ValueChanged(object sender, EventArgs e)
        {
            decimal newDeltaTMax = ((NumericUpDown)sender).Value;
            if ((NumAmbientTemperature.Value - NumCCDSetPoint.Value) > newDeltaTMax) NumCoolerDeltaTMax.Value = NumAmbientTemperature.Value - NumCCDSetPoint.Value;
            if ((NumAmbientTemperature.Value - NumCoolerDeltaTMax.Value) < NumSetpointMinimum.Value) NumCoolerDeltaTMax.Value = NumAmbientTemperature.Value - NumSetpointMinimum.Value;
            coolingConfigurationChanged = true; // Indicate that the configuration has changed, this will be actioned by the graph redraw timer event handler
        }

        /// <summary>
        /// Validate minimum setpoint temperature changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumSetpointMinimum_ValueChanged(object sender, EventArgs e)
        {
            decimal newSetPointMinimum = ((NumericUpDown)sender).Value;
            if (newSetPointMinimum > NumAmbientTemperature.Value - NumCoolerDeltaTMax.Value) NumSetpointMinimum.Value = NumAmbientTemperature.Value - NumCoolerDeltaTMax.Value;
            coolingConfigurationChanged = true;
        }

        /// <summary>
        /// Event handler for changes in number of under damped cycles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumUnderDampedCycles_ValueChanged(object sender, EventArgs e)
        {
            coolingConfigurationChanged = true; // Indicate that the configuration has changed, this will be actioned by the graph redraw timer event handler
        }

        /// <summary>
        /// Event handler for changes in the size of the overshoot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumOvershoot_ValueChanged(object sender, EventArgs e)
        {
            coolingConfigurationChanged = true; // Indicate that the configuration has changed, this will be actioned by the graph redraw timer event handler
        }

        /// <summary>
        /// Event handler for changes in the time to setpoint
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumTimeToSetPoint_ValueChanged(object sender, EventArgs e)
        {
            coolingConfigurationChanged = true; // Indicate that the configuration has changed, this will be actioned by the graph redraw timer event handler
        }

        /// <summary>
        /// Event handler for changes in number of under damped cycles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChkGraphFullRange_CheckedChanged(object sender, EventArgs e)
        {
            coolingConfigurationChanged = true; // Indicate that the configuration has changed, this will be actioned by the graph redraw timer event handler
        }

        /// <summary>
        /// Enable relevant controls based on cooler mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbCoolerModes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Color COLOUR_ENABLED = SystemColors.ControlText;
            Color COLOUR_DISABLED = SystemColors.ControlDark;

            ComboBox cmb = (ComboBox)sender; // Turn the sender object into a ComboBox object so we can access its properties

            // Configure which controls are available in each mode
            switch (cmb.SelectedItem.ToString())
            {
                case Camera.COOLERMODE_WELL_BEHAVED:
                    NumOvershoot.Enabled = false; // Not required in this mode
                    NumTimeToSetPoint.Enabled = true;
                    NumUnderDampedCycles.Enabled = false; // Not required in this mode
                    LblOvershoot.ForeColor = COLOUR_DISABLED;
                    LblTimeToSetPoint.ForeColor = COLOUR_ENABLED;
                    LblOvershootCycles.ForeColor = COLOUR_DISABLED;
                    ChkResetToAmbientOnConnect.Enabled = true;
                    break;

                case Camera.COOLERMODE_UNDERDAMPED:
                    NumOvershoot.Enabled = true;
                    NumTimeToSetPoint.Enabled = true;
                    NumUnderDampedCycles.Enabled = true;
                    LblOvershoot.ForeColor = COLOUR_ENABLED;
                    LblTimeToSetPoint.ForeColor = COLOUR_ENABLED;
                    LblOvershootCycles.ForeColor = COLOUR_ENABLED;
                    ChkResetToAmbientOnConnect.Enabled = true;
                    break;

                case Camera.COOLERMODE_SINGLE_OVERSHOOT:
                    NumOvershoot.Enabled = true;
                    NumTimeToSetPoint.Enabled = true;
                    NumUnderDampedCycles.Enabled = false; // Not required in this mode
                    LblOvershoot.ForeColor = COLOUR_ENABLED;
                    LblTimeToSetPoint.ForeColor = COLOUR_ENABLED;
                    LblOvershootCycles.ForeColor = COLOUR_DISABLED;
                    ChkResetToAmbientOnConnect.Enabled = true;
                    break;

                case Camera.COOLERMODE_ALWAYS_AT_SETPOINT:
                    NumOvershoot.Enabled = false; // Not supported in this mode
                    NumTimeToSetPoint.Enabled = false; // No need for this if the cooler is always at the setpoint
                    NumUnderDampedCycles.Enabled = false;
                    LblOvershoot.ForeColor = COLOUR_DISABLED;
                    LblTimeToSetPoint.ForeColor = COLOUR_DISABLED;
                    LblOvershootCycles.ForeColor = COLOUR_DISABLED;
                    ChkResetToAmbientOnConnect.Enabled = false; // Not required in this mode
                    break;

                case Camera.COOLERMODE_NEVER_GETS_TO_SETPOINT:
                    NumOvershoot.Enabled = false; // Not required in this mode
                    NumTimeToSetPoint.Enabled = true;
                    NumUnderDampedCycles.Enabled = false; // Not required in this mode
                    LblOvershoot.ForeColor = COLOUR_DISABLED;
                    LblTimeToSetPoint.ForeColor = COLOUR_ENABLED;
                    LblOvershootCycles.ForeColor = COLOUR_DISABLED;
                    ChkResetToAmbientOnConnect.Enabled = true;
                    break;

                default:
                    break;
            }

            coolingConfigurationChanged = true; // Indicate that the configuration has changed, this will be actioned by the graph redraw timer event handler
        }

        /// <summary>
        /// Event handler to paint the average period combo box in the "DropDown" rather than "DropDownList" style
        /// </summary>
        /// <param name="sender">Device to be painted</param>
        /// <param name="e">Draw event arguments object</param>
        private void DropDownListComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ComboBox comboBox = sender as ComboBox;

            try
            {
                PaintComboBox(comboBox, e, TextRenderingHint.ClearTypeGridFit);
            }
            catch (Exception ex)
            {
                Log.LogMessageCrLf("CmbCoolerMode_DrawItem", "First exception: " + ex.ToString());

                // One more attempt with default text rendering hint in case the clear type request is not supported on this PC
                try
                {
                    PaintComboBox(comboBox, e, TextRenderingHint.SystemDefault);
                }
                catch (Exception ex1)
                {
                    Log.LogMessageCrLf("CmbCoolerMode_DrawItem", "Second exception: " + ex1.ToString());
                }
            }
        }

        /// <summary>
        /// Render the combo box using the supplied text rendering hint
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="e"></param>
        /// <param name="textRenderingHint"></param>
        private void PaintComboBox(ComboBox comboBox, DrawItemEventArgs e, TextRenderingHint textRenderingHint)
        {
            e.Graphics.TextRenderingHint = textRenderingHint; // Use high quality text rendering

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) // Draw the selected item in menu highlight colour
            {
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.MenuHighlight), e.Bounds);
                e.Graphics.DrawString(comboBox.Items[e.Index].ToString(), e.Font, new SolidBrush(SystemColors.HighlightText), new Point(e.Bounds.X, e.Bounds.Y));
            }
            else // Draw the selected item in normal colour
            {
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.Window), e.Bounds);
                e.Graphics.DrawString(comboBox.Items[e.Index].ToString(), e.Font, new SolidBrush(comboBox.ForeColor), new Point(e.Bounds.X, e.Bounds.Y));
            }

            e.DrawFocusRectangle();
        }

        #endregion

        #region Button and graph timer event handlers

        /// <summary>
        /// Cancel any changes made to the cooler configuration by closing the dialogue without saving values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Log.LogMessage("CoolerForm", "Form cancelled, values not saved.");
            graphRefreshTimer.Enabled = false;
            this.Close();
        }

        /// <summary>
        /// Save any changes to the cooler configuration and close the dialogue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            Log.LogMessage("CoolerForm", "OK - Persisting variables to the camera object.");

            // Save the new cooler configuration to internal variables
            camera.heatSinkTemperature = (double)NumAmbientTemperature.Value;
            camera.setCcdTemperature = (double)NumCCDSetPoint.Value;
            camera.coolerDeltaTMax = (double)NumCoolerDeltaTMax.Value;
            camera.coolerTimeToSetPoint = (double)NumTimeToSetPoint.Value;
            camera.coolerResetToAmbient = ChkResetToAmbientOnConnect.Checked;
            camera.coolerFluctuation = (double)NumFluctuations.Value;
            camera.coolerOvershoot = (double)NumOvershoot.Value;
            camera.coolerUnderDampedCycles = (double)NumUnderDampedCycles.Value;
            camera.coolerSetPointMinimum = (double)NumSetpointMinimum.Value;
            camera.coolerPowerUpState = ChkPowerUpState.Checked;
            camera.coolerGraphRange = ChkFitCurveToScreen.Checked;

            if (cmbCoolerModes.SelectedItem != null) camera.coolerMode = cmbCoolerModes.SelectedItem.ToString(); // Ensure we only persist a valid value

            camera.LogCoolerVariables();
            camera.SaveCoolerToProfile(); // Save the revised variables to the Profile store
            graphRefreshTimer.Enabled = false;

            Log.LogMessage("CoolerForm", "Form closed OK, values persisted to the camera object.");
            this.Close();
        }

        /// <summary>
        /// Event handler for the chart refresh timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>This event is called every 500ms to check whether the configuration has been changed, if so it will redraw the cooling graph. This approach decouples the resource intensive
        /// graph drawing process from changes in the configuration controls enabling the controls to be fully responsive and change quickly.</remarks>
        private void GraphRefreshTimer_Tick(object sender, EventArgs e)
        {
            if (coolingConfigurationChanged) // Test whether the cooling configuration has changed
            {
                // Configuration has changed so redraw the cooling graph
                coolingConfigurationChanged = false; // Reset the configuration changed flag
                DrawCoolingCurve(); // Refresh the curve if the configuration has changed
            }
        }

        #endregion

        #region Support code

        /// <summary>
        /// Draw the cooling graph in the chart control
        /// </summary>
        void DrawCoolingCurve()
        {
            const string COOLING_CURVE = "CoolingCurve";
            const int NUMBER_OF_CURVE_POINTS = 201; // Number of data points displayed in the cooling curve. Larger numbers give higher resolution but require more processing resource and execution time. Array indexes are [0] to [NUMBER_OF_CURVE_POINTS - 1]
            const double ROUNDING_FACTOR = 10.0; // Round axis end values to the nearest multiple of this value when in "Full Range" graphing mode
            const double SETPOINT_STRIPLINE_HALF_WIDTH = 0.5; // Half the width of the strip surrounding the setpoint showing when the cooling curve is getting close to the setpoint (degrees C)

            try
            {
                Log.LogMessage("DrawCoolingCurve", "Number of curve points: {0}", NUMBER_OF_CURVE_POINTS);

                // Define arrays to hold the cooler graph X and Y axis data points
                double[] coolerTemperature = new double[NUMBER_OF_CURVE_POINTS]; // Elements will be numbered [0] to [NUMBER_OF_CURVE_POINTS - 1]
                double[] CoolerTime = new double[NUMBER_OF_CURVE_POINTS];

                double coolerConstant = camera.CalculateCoolerConstant((double)NumTimeToSetPoint.Value, (double)NumCoolerDeltaTMax.Value); // Calculate the cooling constant for the current time to setpoint
                double temperatureRange = Math.Min((double)NumAmbientTemperature.Value - (double)NumCCDSetPoint.Value, (double)NumCoolerDeltaTMax.Value); // Ensure that the temperature range can't exceed the maximum delta T
                double overallTimeToSetpoint = Math.Max(0.1, -Math.Log(Camera.COOLER_SETPOINT_REACHED_OFFSET / temperatureRange) / coolerConstant); // Calculate the time required to reach the setpoint from the starting CCD temperature, ensuring that it is always at least 0.1 so that it appears on the graph

                Log.LogMessage("DrawCoolingCurve", "Overall time to setpoint: {0:0.0}, Ambient temperature: {1:0.0}, CCD setpoint: {2:0.0}, Cooler constant: {3}", overallTimeToSetpoint, NumAmbientTemperature.Value, NumCCDSetPoint.Value, camera.coolerConstant);

                // Set the type of chart that will be displayed
                CoolingChart.Series[COOLING_CURVE].ChartType = SeriesChartType.Line;

                // Remove any data already present in the chart control so that only the new configuration data will be shown
                CoolingChart.Series[COOLING_CURVE].Points.Clear();
                CoolingChart.Annotations.Clear();
                CoolingChart.ChartAreas[0].AxisY.StripLines.Clear();

                // Configure the chart axes - X = Cooling time - Y = Temperature (C)
                // Set minimum and maximum axis values for the time (X) axis
                CoolingChart.ChartAreas[0].AxisX.Minimum = 0.0;
                CoolingChart.ChartAreas[0].AxisX.Maximum = (double)NumTimeToSetPoint.Value;

                // Set minimum and maximum axis values for the temperature (Y) axis depending on whether we are focusing on just the cooling curve itself or the whole temperature range
                if (ChkFitCurveToScreen.Checked) // We are focused on the cooling curve so use default scaling based on the data points added below
                {
                    CoolingChart.ChartAreas[0].AxisY.Maximum = double.NaN; // A value of double.NaN indicates that we should use default scaling
                    CoolingChart.ChartAreas[0].AxisY.Minimum = double.NaN;

                    // Fix the axis minimum and maximum for specific cooler modes
                    switch (cmbCoolerModes.SelectedItem.ToString())
                    {
                        case Camera.COOLERMODE_ALWAYS_AT_SETPOINT:
                            double fitToScreenMinimum = RoundDown(Math.Max((double)NumCCDSetPoint.Value - 0.01, (double)NumSetpointMinimum.Value), ROUNDING_FACTOR);
                            double fitToScreenMaximum = RoundUp((double)NumCCDSetPoint.Value + 0.1, ROUNDING_FACTOR);
                            CoolingChart.ChartAreas[0].AxisY.Minimum = fitToScreenMinimum;
                            CoolingChart.ChartAreas[0].AxisY.Maximum = fitToScreenMaximum;
                            Log.LogMessage("DrawCoolingCurve", "Mode: Always at setpoint - Fit to screen axis Y minimum set to: {0:0.0}, Y maximum set to {1:0.0}", fitToScreenMinimum, fitToScreenMaximum);
                            break;

                        case Camera.COOLERMODE_NEVER_GETS_TO_SETPOINT:
                            fitToScreenMinimum = RoundDown(Math.Max((double)NumCCDSetPoint.Value - 0.01, (double)(NumAmbientTemperature.Value - NumCoolerDeltaTMax.Value)), ROUNDING_FACTOR);
                            CoolingChart.ChartAreas[0].AxisY.Minimum = fitToScreenMinimum;
                            Log.LogMessage("DrawCoolingCurve", "Mode: Never gets to setpoint - Fit to screen axis Y minimum set to: {0:0.0}", fitToScreenMinimum);
                            break;
                    }
                }
                else // We are displaying the full temperature range down to the lowest settable temperature
                {
                    double minimumMultiple = RoundDown((double)NumSetpointMinimum.Value, ROUNDING_FACTOR); // Calculate and set a round number for the low end of the temperature axis
                    CoolingChart.ChartAreas[0].AxisY.Minimum = minimumMultiple;

                    double maximumMultiple = RoundUp((double)NumAmbientTemperature.Value, ROUNDING_FACTOR);  // Calculate and set a round number for the high end of the temperature axis
                    CoolingChart.ChartAreas[0].AxisY.Maximum = maximumMultiple;
                }

                Log.LogMessage("DrawCoolingCurve", "Axis X Minimum: {0}, Axis X Maximum: {1}, Axis Y Minimum: {2}, Axis Y Maximum: {3}", CoolingChart.ChartAreas[0].AxisX.Minimum, CoolingChart.ChartAreas[0].AxisX.Maximum, CoolingChart.ChartAreas[0].AxisY.Minimum, CoolingChart.ChartAreas[0].AxisY.Maximum);

                // Set axis titles and tooltips
                CoolingChart.ChartAreas[0].AxisX.Title = "Time (seconds)";
                CoolingChart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
                CoolingChart.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Horizontal;

                CoolingChart.ChartAreas[0].AxisY.Title = "CCD Temperature (C) ";
                CoolingChart.ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Center;
                CoolingChart.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Rotated270;

                CoolingChart.ChartAreas[0].AxisX.ToolTip = "Time through the Cooling cycle";
                CoolingChart.ChartAreas[0].AxisY.ToolTip = "CCD temperature as the cooling cycle progresses";

                // Add a strip line in the vicinity of the setpoint to show when the cooling curve is getting close
                StripLine closeToSetpointStripline = new StripLine(); // Create the strip line instance
                closeToSetpointStripline.Interval = 0; // Indicate that it does not repeat

                // Set the default strip line start and width - Starting half a strip width below the set point and extending half a strip width above the set point giving an overall width of two times the strip half width
                double stripLineStart = (double)NumCCDSetPoint.Value - SETPOINT_STRIPLINE_HALF_WIDTH;
                double stripLineWidth = SETPOINT_STRIPLINE_HALF_WIDTH * 2.0;
                Log.LogMessage("DrawCoolingCurve", "Default strip line start: {0}, Default strip line width: {1}", stripLineStart, stripLineWidth);

                // Handle edge case: When the strip approaches the lowest achievable temperature
                if (stripLineStart < (double)(NumAmbientTemperature.Value - NumCoolerDeltaTMax.Value)) // Part of the strip line will be below the lowest achievable temperature so don't draw this part
                {
                    stripLineWidth = Math.Max(0.0, (double)(NumCCDSetPoint.Value - NumAmbientTemperature.Value + NumCoolerDeltaTMax.Value) + SETPOINT_STRIPLINE_HALF_WIDTH); // Calculate the new strip line width, ensuring that it can never be negative
                    stripLineStart = (double)(NumAmbientTemperature.Value - NumCoolerDeltaTMax.Value); // Set the start of the strip line to the lowest achievable temperature
                    Log.LogMessage("DrawCoolingCurve", "Strip line falls below bottom of chart - Strip line start: {0}, Strip line width: {1}", stripLineStart, stripLineWidth);
                }

                // Handle edge case: When the strip approaches ambient temperature
                if (stripLineStart + stripLineWidth > (double)(NumAmbientTemperature.Value)) // Part of the strip line will be above ambient temperature
                {
                    stripLineWidth = (double)(NumAmbientTemperature.Value - NumCCDSetPoint.Value) + SETPOINT_STRIPLINE_HALF_WIDTH; // Calculate the new strip line width. The strip line start default calculated above will be correct in this case
                    Log.LogMessage("DrawCoolingCurve", "Strip line falls above top of chart - Strip line start: {0}, Strip line width: {1}", stripLineStart, stripLineWidth);
                }

                // Configure strip line properties
                closeToSetpointStripline.IntervalOffset = stripLineStart;
                closeToSetpointStripline.StripWidth = stripLineWidth;
                closeToSetpointStripline.BackColor = Color.LightGreen;
                closeToSetpointStripline.ForeColor = Color.LightGreen;
                closeToSetpointStripline.ToolTip = string.Format("Temperature range ±0.5C of the set CCD temperature ({0:0.0}C)", NumCCDSetPoint.Value);
                if (closeToSetpointStripline.StripWidth > 0.0) CoolingChart.ChartAreas[0].AxisY.StripLines.Add(closeToSetpointStripline); // Only include the strip line if it has greater than zero width

                // Create a strip line to mark the temperature range that is settable but not achievable
                StripLine unachievableTemperatureRangeStripline = new StripLine(); // Create the strip line instance
                unachievableTemperatureRangeStripline.Interval = 0; // Indicate that it does not repeat
                unachievableTemperatureRangeStripline.IntervalOffset = (double)NumSetpointMinimum.Value; // Set the start temperature of the strip line
                unachievableTemperatureRangeStripline.StripWidth = Math.Max(0.0, (double)(NumAmbientTemperature.Value - NumCoolerDeltaTMax.Value - NumSetpointMinimum.Value)); // Calculate the new strip line width, ensuring that it can never be negative
                unachievableTemperatureRangeStripline.BackColor = Color.MistyRose;
                unachievableTemperatureRangeStripline.ForeColor = Color.MistyRose;
                unachievableTemperatureRangeStripline.ToolTip = string.Format("The CCD temperature can be set here ({0:0.0}C - {1:0.0}C) but the camera will never reach this temperature", unachievableTemperatureRangeStripline.IntervalOffset, unachievableTemperatureRangeStripline.IntervalOffset + unachievableTemperatureRangeStripline.StripWidth);
                if (unachievableTemperatureRangeStripline.StripWidth > 0.0) CoolingChart.ChartAreas[0].AxisY.StripLines.Add(unachievableTemperatureRangeStripline); // Only include the strip line if it has greater than zero width

                // Add a horizontal dotted line at the setpoint
                HorizontalLineAnnotation ann = new HorizontalLineAnnotation();
                ann.AxisX = CoolingChart.ChartAreas[0].AxisX;
                ann.AxisY = CoolingChart.ChartAreas[0].AxisY;
                ann.IsSizeAlwaysRelative = false;
                ann.AnchorY = (double)NumCCDSetPoint.Value;
                ann.IsInfinitive = true;
                ann.ClipToChartArea = CoolingChart.ChartAreas[0].Name;
                ann.LineColor = Color.Red;
                ann.LineDashStyle = ChartDashStyle.Dot;
                ann.LineWidth = 1;
                ann.Name = "Setpoint";
                ann.ToolTip = string.Format("Set CCD temperature ({0:0.0}C)", NumCCDSetPoint.Value);
                CoolingChart.Annotations.Add(ann);

                // Add a horizontal line at the lowest achievable temperature
                HorizontalLineAnnotation annMax = new HorizontalLineAnnotation();
                annMax.AxisX = CoolingChart.ChartAreas[0].AxisX;
                annMax.AxisY = CoolingChart.ChartAreas[0].AxisY;
                annMax.IsSizeAlwaysRelative = false;
                annMax.AnchorY = (double)(NumAmbientTemperature.Value - NumCoolerDeltaTMax.Value);
                annMax.IsInfinitive = true;
                annMax.ClipToChartArea = CoolingChart.ChartAreas[0].Name;
                annMax.LineColor = Color.Orange;
                annMax.LineDashStyle = ChartDashStyle.Solid;
                annMax.LineWidth = 2;
                annMax.Name = "LowestAchievableSetpoint";
                annMax.ToolTip = string.Format("Lowest achievable CCD temperature ({0:0.0}C)", NumAmbientTemperature.Value - NumCoolerDeltaTMax.Value);
                CoolingChart.Annotations.Add(annMax);

                // Add a horizontal line at the lowest possible setpoint temperature
                HorizontalLineAnnotation annLowest = new HorizontalLineAnnotation();
                annLowest.AxisX = CoolingChart.ChartAreas[0].AxisX;
                annLowest.AxisY = CoolingChart.ChartAreas[0].AxisY;
                annLowest.IsSizeAlwaysRelative = false;
                annLowest.AnchorY = (double)(NumSetpointMinimum.Value);
                annLowest.IsInfinitive = true;
                annLowest.ClipToChartArea = CoolingChart.ChartAreas[0].Name;
                annLowest.LineColor = Color.Red;
                annLowest.LineDashStyle = ChartDashStyle.Solid;
                annLowest.LineWidth = 2;
                annLowest.Name = "LowestPossibleSetpoint";
                annLowest.ToolTip = string.Format("Lowest settable CCD temperature ({0:0.0}C), beyond this InvalidValueExceptions are thrown", NumSetpointMinimum.Value);
                CoolingChart.Annotations.Add(annLowest);

                // Add a vertical line at the time at which the setpoint is achieved
                VerticalLineAnnotation annV = new VerticalLineAnnotation();
                annV.AxisX = CoolingChart.ChartAreas[0].AxisX;
                annV.AxisY = CoolingChart.ChartAreas[0].AxisY;
                annV.IsSizeAlwaysRelative = false;
                annV.AnchorX = overallTimeToSetpoint; // Set the X coordinate at which the vertical line will appear
                if (cmbCoolerModes.SelectedItem.ToString() == Camera.COOLERMODE_ALWAYS_AT_SETPOINT) annV.AnchorX = 0.1; // Revise the X coordinate to 0.1 if we are in "Always at setpoint" mode
                annV.IsInfinitive = true;
                annV.ClipToChartArea = CoolingChart.ChartAreas[0].Name;
                annV.LineColor = Color.Red;
                annV.LineDashStyle = ChartDashStyle.Dot;
                annV.LineWidth = 1;
                annV.Name = "TimeToTemperature";
                annV.ToolTip = string.Format("Time at which the CCD temperature becomes stable at the expected temperature ({0:0.0} seconds)", annV.AnchorX);
                CoolingChart.Annotations.Add(annV);

                // Calculate the data points and add them to the data arrays
                for (int i = 0; i < NUMBER_OF_CURVE_POINTS; i++)
                {
                    double timeFraction = (double)i / ((double)NUMBER_OF_CURVE_POINTS - 1.0);
                    CoolerTime[i] = overallTimeToSetpoint * timeFraction;
                    camera.CalculateCoolerTemperature(timeFraction, overallTimeToSetpoint, (double)NumAmbientTemperature.Value, Math.Max((double)NumCCDSetPoint.Value, (double)(NumAmbientTemperature.Value - NumCoolerDeltaTMax.Value)), (double)NumUnderDampedCycles.Value, (double)NumOvershoot.Value, (double)NumAmbientTemperature.Value, (double)NumCoolerDeltaTMax.Value, coolerConstant, cmbCoolerModes.SelectedItem.ToString());
                    coolerTemperature[i] = camera.ccdTemperature;
                    Log.LogMessage("DrawCoolingCurve", "Adding point {0} - Time fraction: {1:0.000}, Cooler time: {2:0.000}, CCD Temperature: {3:0.000}", i, timeFraction, CoolerTime[i], coolerTemperature[i]);
                }
                CoolingChart.Series[COOLING_CURVE].Points.DataBindXY(CoolerTime, coolerTemperature); // Add the calculated cooling curve data points to the graph

                CoolingChart.Series[COOLING_CURVE].ToolTip = "Time: #VALX{0.0} - Temperature: #VAL{0.0}"; // Add a tool tip that will display the value of the data point when the mouse hovers over it
                Log.LogMessage("DrawCoolingCurve", "Finished");
            }
            catch (Exception ex)
            {
                Log.LogMessageCrLf("DrawCoolingCurve", ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Round the value down to the nearest whole rounding factor
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="RoundingFactor"></param>
        /// <returns></returns>
        double RoundDown(double Value, double RoundingFactor)
        {
            return Math.Floor(Value / RoundingFactor) * RoundingFactor;
        }

        /// <summary>
        /// Round the value up to the nearest whole rounding factor
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="RoundingFactor"></param>
        /// <returns></returns>
        double RoundUp(double Value, double RoundingFactor)
        {
            return Math.Ceiling(Value / RoundingFactor) * RoundingFactor;
        }

        #endregion

    }
}
