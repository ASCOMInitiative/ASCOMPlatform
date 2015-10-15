using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.Simulator
{
    public partial class OverrideView : UserControl
    {
        public OverrideView()
        {
            InitializeComponent();
            trkOverride.Enabled = chkOverride.Checked;
            chkOverride.CheckedChanged += ChkOverride_CheckedChanged;
            trkOverride.ValueChanged += TrkOverride_ValueChanged;
            lblFrom.Text = "0.0";
            lblTo.Text = "0.0";
        }

        private void TrkOverride_ValueChanged(object sender, EventArgs e)
        {
            TrackBar tb = (TrackBar)sender;
            txtValue.Text = tb.Value.ToString();

            SetEnabledButton();
        }

        private void ChkOverride_CheckedChanged(object sender, EventArgs e)
        {
            trkOverride.Enabled = chkOverride.Checked;

            SetEnabledButton();
        }

        public void InitUI(string property)
        {
            double fromValue = OCSimulator.OverrideFromValues[property];
            double toValue = OCSimulator.OverrideToValues[property];
            lblFrom.Text = fromValue.ToString();
            Point trkOverideLocation = trkOverride.Location;
            Point lblFromLocation = lblFrom.Location;
            lblFromLocation.X = trkOverideLocation.X - lblFrom.Width;
            lblFrom.Location = lblFromLocation;

            lblTo.Text = toValue.ToString();
            trkOverride.SetRange((int)fromValue, (int)toValue);
            chkOverride.Checked = OCSimulator.Sensors[property].Override;
            trkOverride.Value = (int)OCSimulator.Sensors[property].OverrideValue;
            txtValue.Text = OCSimulator.Sensors[property].OverrideValue.ToString();
            txtValue.BackColor = Color.White;
        }

        public void SaveUI(string property)
        {
            OCSimulator.TL.LogMessage("SaveUI", "property: " + property + " " + chkOverride.Name + " " + chkOverride.Checked + " " + trkOverride.Value.ToString());
            OCSimulator.Sensors[property].Override = chkOverride.Checked;
            OCSimulator.Sensors[property].OverrideValue = (double)trkOverride.Value;
        }

        private void SetEnabledButton()
        {
            frmMain mainForm = (frmMain)trkOverride.Parent.Parent; // Get a handle to the frmMain form
            mainForm.btnEnable.Enabled = true; // Set the enabled button to true
            mainForm.btnEnable.ForeColor = Color.Red;
        }
    }
}
