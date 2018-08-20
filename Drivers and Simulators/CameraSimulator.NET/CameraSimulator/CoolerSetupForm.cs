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
                if (newTimeToSetPoint < 1)  NumTimeToSetPoint.Value = 1;
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
    }
}
