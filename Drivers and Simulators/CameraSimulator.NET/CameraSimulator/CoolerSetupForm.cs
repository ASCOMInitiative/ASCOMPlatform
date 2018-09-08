using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.Simulator
{
    public partial class CoolerSetupForm : Form
    {
        const decimal MINIMUM_TEMPERATURE = -273.15m; // Absolute zero as a decimal constant
        const decimal MAXIMUM_TEMPERATURE = decimal.MaxValue; // A huge upper limit
        const decimal DELTATMAX_MINIMUM = 1.0m; // Minimum permissible value of maximum delta T - Recommended to be at least 1.0 in order to avoid infinities arising in the cooler constant calculation
        const decimal FLUCTUATIONS_MINIMUM = 0.0m; // Minimum permissible value for random temperature fluctuations
        const decimal OVERSHOOT_MINIMUM = 0.0m; // Minimum permissible value for the overshoot temperature increment
        const decimal TIMETOSETPOINT_MINIMUM = 1.0m; // Minimum permissible value for the time to setpoint

        private Camera camera; // Private variable that holds a reference to the main camera object where data persistence code is located

        #region Initialiser and Form Load

        /// <summary>
        /// Cooler setup form initialiser
        /// </summary>
        public CoolerSetupForm()
        {
            InitializeComponent();
            cmbCoolerModes.DrawMode = DrawMode.OwnerDrawFixed; // Add event handler to give the combo box have a white background when in list mode.
            cmbCoolerModes.DrawItem += DropDownListComboBox_DrawItem;
        }

        /// <summary>
        /// Cooler configuration form load event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CoolerSetupForm_Load(object sender, EventArgs e)
        {
            const string DOUBLE_SPACE = "\r\n\r\n";
            const string SINGLE_SPACE = "\r\n";

            #region Descriptive help text for the COOLING CONFIGURATION FORM and controls

            // Set help text for the AMBIENT TEMPERATURE numeric control
            CoolingHelp.SetHelpString(NumAmbientTemperature,
                "Sets the ambient environment temperature" + DOUBLE_SPACE +
                "This value is returned through the Camera.HeatSinkTemperature property" + DOUBLE_SPACE +
                "Must be less than or equal to the [SETPOINT + MAXIMUM_DELTA_T] temperature." + DOUBLE_SPACE +
                "Must be greater than or equal to the [SETPOINT] temperature."
                );
            CoolingHelp.SetHelpString(LblAmbientTemperature, CoolingHelp.GetHelpString(NumAmbientTemperature)); // Add help text to the control label

            // Set help text for the default SETPOINT temperature numeric control
            CoolingHelp.SetHelpString(NumCCDSetPoint,
                "Sets the camera's default setpoint temperature." + DOUBLE_SPACE +
                "This value is set and returned through the Camera.SetCCDTemperature property." + DOUBLE_SPACE +
                "Must be less than or equal to [AMBIENT] temperature." + DOUBLE_SPACE +
                "Must be greater than or equal to [AMBIENT - MAXIMUM_DELTA_T]."
                );
            CoolingHelp.SetHelpString(LblCCDSetPoint, CoolingHelp.GetHelpString(NumCCDSetPoint)); // Add help text to the control label

            // Set help text for the MAXIMUM TEMPERATURE DIFFERENTIAL numeric control
            CoolingHelp.SetHelpString(NumCoolerDeltaTMax,
                "Sets the maximum temperature differential from ambient that the cooler can maintain." + DOUBLE_SPACE +
                "Must be less than or equal to the difference between the [AMBIENT] and [SETPOINT] temperatures." + DOUBLE_SPACE +
                string.Format("Must be greater than or equal to {0:0.0}.", DELTATMAX_MINIMUM) + DOUBLE_SPACE +
                "NOTE:" + SINGLE_SPACE +
                "Over most of the cooling cycle and at the setpoint, the Camera.CoolerPower property is calculated from the CCD temperature expressed as a fraction of the cooler temperature range: [POWER] = [AMBIENT - CCD_TEMPERATURE] / [MAXIMUM_DELTA_T]." + SINGLE_SPACE +
                "In addition, the CoolerPower property will return 100% in the early stages of cooling and 0% in the early stages of warming to simulate the cooler driving the temperature change as fast as possible."
                );
            CoolingHelp.SetHelpString(LblCoolerDeltaTMax, CoolingHelp.GetHelpString(NumCoolerDeltaTMax)); // Add help text to the control label

            // Set help text for the TIME TO GET TO SETPOINT numeric control
            CoolingHelp.SetHelpString(NumTimeToSetPoint,
                "Sets the time taken to arrive at the maximum cooling temperature when cooling from ambient." + DOUBLE_SPACE +
                "Temperature changes of less than the maximum temperature difference will complete in less than the specified time." + DOUBLE_SPACE +
                string.Format("Must be greater than or equal to {0:0.0}.", TIMETOSETPOINT_MINIMUM) + DOUBLE_SPACE +
                "NOTES:" + SINGLE_SPACE +
                "1) Not supported in \"Always at setpoint\" mode." + SINGLE_SPACE +
                "2) When cooling over the full default temperature range of 40C, the CCD will be:" + SINGLE_SPACE +
                "   a) Within 2.0C of the setpoint after 50% of the cooling time " + SINGLE_SPACE +
                "   b) Within 1.0C of the setpoint after 60% of the cooling time" + SINGLE_SPACE +
                "   c) Within 0.5C of the setpoint after 70% of the cooling time" + SINGLE_SPACE +
                "   d) Within 0.1C of the setpoint after 88% of the cooling time." + SINGLE_SPACE +
                "   e) At the setpoint after 100% of the cooling time." + SINGLE_SPACE +
                "3) For smaller temperature changes, the CCD temperature will approach the setpoint earlier than indicated above ." + SINGLE_SPACE +
                "4) For larger temperature changes, the CCD temperature will approach the setpoint later than indicated above."
              );
            CoolingHelp.SetHelpString(LblTimeToSetPoint, CoolingHelp.GetHelpString(NumTimeToSetPoint)); // Add help text to the control label

            // Set help text for the OVERSHOOT numeric control
            CoolingHelp.SetHelpString(NumOvershoot,
                "Sets the amount of temperature overshoot that occurs when the CCD set point temperature is changed." + DOUBLE_SPACE +
                "When overshoot is 0.0, the reported CCD temperature will follow an exponential, Newton's cooling equation, path to the [SETPOINT] temperature." + DOUBLE_SPACE +
                "When overshoot is greater than 0.0, the reported CCD temperature will overshoot by moving smoothly through the setpoint and, half way through the cooling cycle, arriving at the [SETPOINT ± OVERSHOOT] temperature." + SINGLE_SPACE +
                "It will then undershoot back through the setpoint before joining the \"zero overshoot\" curve and following this to the [SETPOINT] temperature." + DOUBLE_SPACE +
                string.Format("Must be greater than or equal to {0:0.0}.", OVERSHOOT_MINIMUM) + DOUBLE_SPACE +
                "NOTES:" + SINGLE_SPACE +
                "1) Not supported in \"Always at setpoint\" and \"Never gets to setpoint\" modes." + SINGLE_SPACE +
                "2) When warming, the reported CCD temperature will rise above the setpoint by the configured amount before returning below it to follow the warming curve to the setpoint." + SINGLE_SPACE +
                "3) The full temperature overshoot will always be exhibited, regardless of the size of [SETPOINT] temperature change." + SINGLE_SPACE +
                "4) No overshoot will be exhibited when the cooler is turned off and the CCD temperature warms to [AMBIENT] temperature on its own." + SINGLE_SPACE +
                "5) The reported CCD temperature will be truncated to [AMBIENT] or [AMBIENT - MAXIMUM_DELTA_T] temperature as appropriate, whenever the calculated overshoot exceeds this range during the cooling cycle."
                );
            CoolingHelp.SetHelpString(LblOvershoot, CoolingHelp.GetHelpString(NumOvershoot)); // Add help text to the control label

            // Set help text for the RANDOM FLUCTUATIONS numeric control
            CoolingHelp.SetHelpString(NumFluctuations,
                "Sets the size of random temperature fluctuations added to the CCD and heat sink temperatures." + DOUBLE_SPACE +
                "The returned CCD and heat sink temperatures will exhibit random fluctuations of up to ± this value. Set to 0.00 for fully reproducible behaviour," + DOUBLE_SPACE +
                string.Format("Must be greater than or equal to {0:0.0}.", FLUCTUATIONS_MINIMUM)
                );
            CoolingHelp.SetHelpString(LblFluctuations, CoolingHelp.GetHelpString(NumFluctuations)); // Add help text to the control label

            // Set help text for the COOLER MODES drop-down
            CoolingHelp.SetHelpString(cmbCoolerModes,
                "Selects the cooler's behavioural characteristics." + DOUBLE_SPACE +
                "The \"Always at setpoint\" mode goes immediately to the setpoint when the cooler is turned on or the setpoint is changed." + DOUBLE_SPACE +
                "The \"Damped approach\" mode, with an overshoot of 0.00,  uses Newton's cooling equation to arrive smoothly at the setpoint in the configured time when cooling from ambient to the maximum cooling temperature." + SINGLE_SPACE +
                "If the starting temperature is lower than ambient, the cooler will take less time to arrive at the setpoint." + DOUBLE_SPACE +
                "The \"Damped approach\" mode, with an overshoot greater than 0.00, uses Newton's cooling equation as its majority component but also includes an \"overshoot\" component." + SINGLE_SPACE +
                "The contribution from the overshoot component gradually increases through the first 50% of the cooling cycle causing the reported CCD temperature to pass through the setpoint and eventually exceed it by the specified amount." + SINGLE_SPACE +
                "The overshoot contribution then reduces to zero over the next 20% of the cooling cycle leaving the remaining 30% of the cycle to follow Newton's curve to the setpoint" + DOUBLE_SPACE +
                string.Format("The \"Never gets to setpoint\" mode will miss the setpoint by {0:P0} of the difference between ambient and the setpoint.", Camera.COOLER_NEVER_GETS_TO_SETPOINT_REDUCTION_FACTOR)
                );
            CoolingHelp.SetHelpString(LblCoolerModes, CoolingHelp.GetHelpString(cmbCoolerModes)); // Add help text to the control label

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

            // This empty picture box is present to provide a control that covers the whole form area. This approach is used because the HelpProvider doesn't work when clicking the form itself.
            CoolingHelp.SetHelpString(BackgroundPictureBox,
                "Cooler Configuration Form - Sets the cooler's control and behavioural characteristics." + DOUBLE_SPACE +
                "Configures the Cooler's default control values including ambient temperature, setpoint, time to setpoint and maximum cooling range together with its overall cooling behaviour."
                );
            CoolingHelp.SetHelpString(LblHelpText, CoolingHelp.GetHelpString(BackgroundPictureBox)); // Add help text to the help instructions label

            #endregion

        }

        /// <summary>
        /// Initialise the cooler configuration form controls and variables
        /// </summary>
        /// <param name="theCamera">Camera object used to persist changed configuration.</param>
        /// <remarks>This is called from the setup form to initialise the cooler configuration form.</remarks>
        internal void InitProperties(Camera theCamera)
        {
            Log.LogMessage("CoolerForm", "Started initialising properties.");
            camera = theCamera; // Save the supplied Camera object so that it can be used to persist any cooling variable changes

            // Set minimum and maximum values for the numeric controls
            NumAmbientTemperature.Minimum = MINIMUM_TEMPERATURE; NumAmbientTemperature.Maximum = MAXIMUM_TEMPERATURE;
            NumCCDSetPoint.Minimum = MINIMUM_TEMPERATURE; NumCCDSetPoint.Maximum = MAXIMUM_TEMPERATURE;
            NumCoolerDeltaTMax.Minimum = DELTATMAX_MINIMUM; NumCoolerDeltaTMax.Maximum = MAXIMUM_TEMPERATURE;
            NumFluctuations.Minimum = FLUCTUATIONS_MINIMUM; NumFluctuations.Maximum = MAXIMUM_TEMPERATURE;
            NumOvershoot.Minimum = OVERSHOOT_MINIMUM; NumOvershoot.Maximum = MAXIMUM_TEMPERATURE;
            NumTimeToSetPoint.Minimum = TIMETOSETPOINT_MINIMUM; NumTimeToSetPoint.Maximum = MAXIMUM_TEMPERATURE;

            // Initialise the controls with current values
            NumAmbientTemperature.Value = (decimal)camera.heatSinkTemperature;
            NumCCDSetPoint.Value = (decimal)camera.setCcdTemperature;
            NumCoolerDeltaTMax.Value = (decimal)camera.coolerDeltaTMax;
            NumTimeToSetPoint.Value = (decimal)camera.coolerTimeToSetPoint;
            NumOvershoot.Value = (decimal)camera.coolerOvershoot;
            NumFluctuations.Value = (decimal)camera.coolerFluctuation;
            ChkResetToAmbientOnConnect.Checked = camera.coolerResetToAmbient;
            ChkPowerUpState.Checked = camera.coolerPowerUpState;

            foreach (string coolerMode in camera.coolerModes) // Load the cooler modes combo box with options
            {
                cmbCoolerModes.Items.Add(coolerMode);
            }

            cmbCoolerModes.SelectedIndexChanged += new System.EventHandler(this.CmbCoolerModes_SelectedIndexChanged); // Add the camera mode changed event handler here so that the correct options are enabled
            cmbCoolerModes.SelectedItem = camera.coolerMode;

            // Add event handlers to validate input values - These have to be added after the variables above have been set to avoid inappropriate triggering during initialisation
            NumAmbientTemperature.ValueChanged += new System.EventHandler(this.NumAmbientTemperature_ValueChanged);
            NumCCDSetPoint.ValueChanged += new System.EventHandler(this.NumCCDSetPoint_ValueChanged);
            NumCoolerDeltaTMax.ValueChanged += new System.EventHandler(this.NumCoolerDeltaTMax_ValueChanged);
            NumTimeToSetPoint.ValueChanged += new System.EventHandler(this.NumTimeToSetPoint_ValueChanged);

            camera.LogCoolerVariables();

            Log.LogMessage("CoolerForm", "Completed initialising properties.");
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
        }

        /// <summary>
        /// Validate setpoint changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumCCDSetPoint_ValueChanged(object sender, EventArgs e)
        {
            decimal newCCDSetPoint = ((NumericUpDown)sender).Value;
            if ((NumAmbientTemperature.Value - newCCDSetPoint) > NumCoolerDeltaTMax.Value) NumCCDSetPoint.Value = NumAmbientTemperature.Value - NumCoolerDeltaTMax.Value;
            if (newCCDSetPoint > NumAmbientTemperature.Value) NumCCDSetPoint.Value = NumAmbientTemperature.Value;
        }

        /// <summary>
        /// Validate time to setpoint changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumTimeToSetPoint_ValueChanged(object sender, EventArgs e)
        {
            decimal newTimeToSetPoint = ((NumericUpDown)sender).Value;
            if (newTimeToSetPoint < 1) NumTimeToSetPoint.Value = 1;
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
        }

        /// <summary>
        /// Event handler for a change in cooler behaviour mode
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
                case Camera.COOLERMODE_DAMPED:
                    NumOvershoot.Enabled = true; // Supported in this mode
                    NumTimeToSetPoint.Enabled = true; // Supported in this mode
                    LblOvershoot.ForeColor = COLOUR_ENABLED;
                    LblTimeToSetPoint.ForeColor = COLOUR_ENABLED;
                    break;

                case Camera.COOLERMODE_ALWAYS_AT_SETPOINT:
                    NumOvershoot.Enabled = false; // Not supported in this mode
                    NumTimeToSetPoint.Enabled = false; // No need for this if the cooler is always at the setpoint
                    LblOvershoot.ForeColor = COLOUR_DISABLED;
                    LblTimeToSetPoint.ForeColor = COLOUR_DISABLED;
                    break;

                case Camera.COOLERMODE_NEVER_GETS_TO_SETPOINT:
                    NumOvershoot.Enabled = false; // Not supported in this mode
                    NumTimeToSetPoint.Enabled = true; // Supported in this mode
                    LblOvershoot.ForeColor = COLOUR_DISABLED;
                    LblTimeToSetPoint.ForeColor = COLOUR_ENABLED;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Event handler to paint the average period combo box in the "DropDown" rather than "DropDownList" style
        /// </summary>
        /// <param name="sender">Device to be painted</param>
        /// <param name="e">Draw event arguments object</param>
        private void DropDownListComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ComboBox combo = sender as ComboBox;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) // Draw the selected item in menu highlight colour
            {
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.MenuHighlight), e.Bounds);
                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font, new SolidBrush(SystemColors.HighlightText), new Point(e.Bounds.X, e.Bounds.Y));
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.Window), e.Bounds);
                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font, new SolidBrush(combo.ForeColor), new Point(e.Bounds.X, e.Bounds.Y));
            }

            e.DrawFocusRectangle();
        }

        #endregion

        #region Button event handlers

        /// <summary>
        /// Cancel any changes made to the cooler configuration by closing the dialogue without saving values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Log.LogMessage("CoolerForm", "Form cancelled, values not saved.");
            this.Close();
        }

        /// <summary>
        /// Save any changes to the cooler configuration and close the dialogue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            Log.LogMessage("CoolerForm", "OK - Persisting variables to the Profile store.");

            // Save the new cooler configuration to internal variables
            camera.heatSinkTemperature = (double)NumAmbientTemperature.Value;
            camera.setCcdTemperature = (double)NumCCDSetPoint.Value;
            camera.coolerDeltaTMax = (double)NumCoolerDeltaTMax.Value;
            camera.coolerTimeToSetPoint = (double)NumTimeToSetPoint.Value;
            camera.coolerResetToAmbient = ChkResetToAmbientOnConnect.Checked;
            camera.coolerFluctuation = (double)NumFluctuations.Value;
            camera.coolerOvershoot = (double)NumOvershoot.Value;
            camera.coolerPowerUpState = ChkPowerUpState.Checked;

            if (cmbCoolerModes.SelectedItem != null) camera.coolerMode = cmbCoolerModes.SelectedItem.ToString(); // Ensure we only persist a valid value

            camera.LogCoolerVariables();
            camera.SaveCoolerToProfile(); // Save the revised variables to the Profile store
            Log.LogMessage("CoolerForm", "Form closed OK, values persisted to Profile store.");

            this.Close();
        }

        #endregion

        #region Support code

        #endregion
    }
}
