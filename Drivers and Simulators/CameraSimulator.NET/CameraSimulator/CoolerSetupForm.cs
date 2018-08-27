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
        internal bool EnableValidation = false;

        public CoolerSetupForm()
        {
            InitializeComponent();
            cmbCoolerModes.DrawMode = DrawMode.OwnerDrawFixed;
            cmbCoolerModes.DrawItem += DropDownListComboBox_DrawItem;
        }

        private void CheckBounds(object sender, CancelEventArgs e)
        {
            if ((NumAmbientTemperature.Value - NumCCDSetPoint.Value) > NumCoolerDeltaTMax.Value) e.Cancel = true;
            MessageBox.Show(string.Format("CheckBounds {0} - Ambient: {1}, SetPoint: {2}, DeltaTMax: {3}", ((NumericUpDown)sender).Name, NumAmbientTemperature.Value, NumCCDSetPoint.Value, NumCoolerDeltaTMax.Value));
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// Event handler to paint the average period combo box in the "DropDown" rather than "DropDownList" style
        /// </summary>
        /// <param name="sender">Device to be painted</param>
        /// <param name="e">Draw event arguments object</param>
        void DropDownListComboBox_DrawItem(object sender, DrawItemEventArgs e)
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

        private void NumAmbientTemperature_ValueChanged(object sender, EventArgs e)
        {
            if (EnableValidation)
            {
                decimal newAmbientTemperature = ((NumericUpDown)sender).Value;
                if (newAmbientTemperature < NumCCDSetPoint.Value) NumAmbientTemperature.Value = NumCCDSetPoint.Value;
                if ((newAmbientTemperature - NumCCDSetPoint.Value) > NumCoolerDeltaTMax.Value) NumAmbientTemperature.Value = NumCCDSetPoint.Value + NumCoolerDeltaTMax.Value;
            }
        }

        private void NumCCDSetPoint_ValueChanged(object sender, EventArgs e)
        {
            if (EnableValidation)
            {
                decimal newCCDSetPoint = ((NumericUpDown)sender).Value;
                if ((NumAmbientTemperature.Value - newCCDSetPoint) > NumCoolerDeltaTMax.Value) NumCCDSetPoint.Value = NumAmbientTemperature.Value - NumCoolerDeltaTMax.Value;
                if (newCCDSetPoint > NumAmbientTemperature.Value) NumCCDSetPoint.Value = NumAmbientTemperature.Value;
            }
        }

        private void NumTimeToSetPoint_ValueChanged(object sender, EventArgs e)
        {
            if (EnableValidation)
            {
                decimal newTimeToSetPoint = ((NumericUpDown)sender).Value;
                if (newTimeToSetPoint < 1) NumTimeToSetPoint.Value = 1;
            }
        }

        private void NumCoolerDeltaTMax_ValueChanged(object sender, EventArgs e)
        {
            if (EnableValidation)
            {
                decimal newDeltaTMax = ((NumericUpDown)sender).Value;
                if ((NumAmbientTemperature.Value - NumCCDSetPoint.Value) > newDeltaTMax) NumCoolerDeltaTMax.Value = NumAmbientTemperature.Value - NumCCDSetPoint.Value;
            }
        }

        private void CoolerSetupForm_Load(object sender, EventArgs e)
        {
            const string DOUBLE_SPACE = "\r\n\r\n";
            const string SINGLE_SPACE = "\r\n";

            // Set help text for the cooling configuration form - this picturebox has to be used because the HelpProvider doesn't work when clicking the form itself.
            CoolingHelp.SetHelpString(BackgroundPictureBox,
                "Cooler Configuration Form - Sets the cooler's control and behavioural characteristics." + DOUBLE_SPACE +
                "Configures the Cooler's default control values including, ambient temperature, setpoint, time to setpoint and maximum cooling range together with its cooling behaviour."
                );

            // Set help text for the cooler modes dropdown
            CoolingHelp.SetHelpString(cmbCoolerModes,
                "Selects the cooler's behavioural characteristics." + DOUBLE_SPACE +
                "The \"Always at setpoint\" mode goes immediately to the setpoint when the cooler is turned on or the setpoint is changed." + DOUBLE_SPACE +
                "The \"Damped approach\" mode uses Newton's cooling equation to arrive smoothly at the setpoint in the configured time when cooling from ambient to the setpoint." + SINGLE_SPACE +
                "If the starting temnperature is lower than ambient, the cooler will take less time to arrive at the setpoint." + DOUBLE_SPACE +
                "The \"Under damped\" mode will exceed the setpoint (on both cooling and warming) by 20% of the difference between ambient and the setpoint, subject to a minimum of 2C and a maximum of 5C." + DOUBLE_SPACE +
                "The \"Never gets to setpoint\" mode will miss the setpoint by 10% of the difference between ambient and the setpoint."
                );
            CoolingHelp.SetHelpString(LblCoolerModes, CoolingHelp.GetHelpString(cmbCoolerModes));

            // Set help text for the ambient temperature numeric control
            CoolingHelp.SetHelpString(NumAmbientTemperature,
                "Sets the ambient environment temperature" + DOUBLE_SPACE +
                "This value is returned through the Camera.HeatSinkTemperature property" + DOUBLE_SPACE +
                "Must be above the setpoint temperature."
                );
            CoolingHelp.SetHelpString(LblAmbientTemperature, CoolingHelp.GetHelpString(NumAmbientTemperature));

            // Set help text for the default setpoint temperature numeric control
            CoolingHelp.SetHelpString(NumCCDSetPoint,
                "Sets the camera's default setpoint temperature." + DOUBLE_SPACE +
                "This value is set and returned the Camera.SetCCDTemperature property." + DOUBLE_SPACE +
                "Must be less than or equal to the ambient temperature."
                );
            CoolingHelp.SetHelpString(LblCCDSetPoint, CoolingHelp.GetHelpString(NumCCDSetPoint));

            // Set help text for the maxium temperature differential numeric control
            CoolingHelp.SetHelpString(NumCoolerDeltaTMax,
                "Sets the maximum temperature differential that the cooler can maintain from ambient." + DOUBLE_SPACE +
                "The Camera.CoolerPower property will report 100% in the early stages of cooling. It can also report 0% in the early stages of warming." + SINGLE_SPACE +
                "Later in the cooling cycle, the cooler power value is scaled linearly between 0% when the CCD temperature is at ambient and 100% when it is at ambient minus the maximum temperature differential." + DOUBLE_SPACE +
                "Must be greater than or equal to the difference between the ambient and setpoint temperatures."
                );
            CoolingHelp.SetHelpString(LblCoolerDeltaTMax, CoolingHelp.GetHelpString(NumCoolerDeltaTMax));

            // Set help text for the time to get to setpoint numeric control
            CoolingHelp.SetHelpString(NumTimeToSetPoint,
                "Sets the time taken to get to the setpoint when cooling from ambient." + DOUBLE_SPACE +
                "If the setpoint is changed while the cooler is already cooling, the new setpoint may be reached in less than the specified time." + DOUBLE_SPACE +
                "Must be greater than 1."
                );
            CoolingHelp.SetHelpString(LblTimeToSetPoint, CoolingHelp.GetHelpString(NumTimeToSetPoint));

            // Set help text for the reset to ambient checkbox
            CoolingHelp.SetHelpString(ChkResetToAmbientOnConnect,
                "Sets whether or not to reset the CCD temperature to ambient when cooling is enabled." + DOUBLE_SPACE +
                "When cooling is turned off, the CCD will normally warm up to ambient temperature in the configured cooling time." + DOUBLE_SPACE +
                "If this checkbox is unchecked and cooling is re-enabled during the warm-up time, the partially warmed up CCD temperature will be used as the starting point." + DOUBLE_SPACE +
                "If checked, ambient temperature will be used as the starting point instead."
                );

            // Set help text for the close button
            CoolingHelp.SetHelpString(BtnClose,
                "Closes this form." + DOUBLE_SPACE +
                "Any changes will NOT be saved until you press the \"OK\" button on the Simulator Setup form from which you accessed this form."
                );
        }
    }
}
