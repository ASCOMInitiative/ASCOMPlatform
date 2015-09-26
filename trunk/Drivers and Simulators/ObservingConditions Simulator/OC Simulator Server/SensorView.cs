using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.DriverAccess;

namespace ASCOM.Simulator
{
    /// <summary>
    /// Handles selecting the Switch or ObservingConditions driver and, for the switch,
    /// selecting the swtch id either using a combo of switch names or a spin button to
    /// select the switch id.
    /// Also allows calling the setup dialog for the selected driver.
    /// </summary>
    public partial class SensorView : UserControl
    {
        TraceLoggerPlus TL;
        /// <summary>
        /// The sensor name must match the name in Hub.Sensors[property]
        /// </summary>
        public string SensorName { get; set; }

        public bool ShowNotReady
        {
            get { return chkNotReady.Checked; }
            set { chkNotReady.Checked = value; }
        }

        public double NotReadyDelay
        {
            get { return Convert.ToDouble(numDelay.Value); }
            set { numDelay.Value = Convert.ToDecimal(value); }
        }

        public bool SensorEnabled
        {
            get { return chkEnabled.Checked; }
            set { chkEnabled.Checked = value; }
        }

        public double MinValue
        {
            get { return Convert.ToDouble(txtMinValue.Text); }
            set { txtMinValue.Text = value.ToString(); }
        }

        public double MaxValue
        {
            get { return Convert.ToDouble(txtMaxValue.Text); }
            set { txtMaxValue.Text = value.ToString(); }
        }

        public bool EnabledCheckboxVisible
        {
            get { return chkEnabled.Visible; }
            set { chkEnabled.Visible = value; }
        }

        public bool NotReadyControlsEnabled
        {
            get { return chkNotReady.Enabled; }
            set
            {
                chkNotReady.Enabled = value;
                numDelay.Enabled = value;
                txtMinValue.Enabled = value;
                txtMaxValue.Enabled = value;
            }
        }



        /// <summary>
        /// should be done with an event, at present it's assumed that this is set
        /// before InitUi is called.
        /// Or could be in this control so each device can have it's own state.
        /// *** currently not used, we attempt to connect and set the UI depending on what can be read.
        /// </summary>
        public bool ConnectToDriver { get; set; }

        public SensorView()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                TL = OCSimulator.TL;
                TL.LogMessage("SensorView", "This.Name: " + this.Name);
                chkEnabled.CheckedChanged += ChkEnabled_CheckedChanged;
            }
        }

        private void ChkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (OCSimulator.setupForm != null)
            {
                SensorView svHumidity = (SensorView)OCSimulator.setupForm.Controls["sensorViewHumidity"];
                SensorView svThisSensor = (SensorView)OCSimulator.setupForm.Controls[this.Name];

                TL.LogMessage("ChkEnabled_CheckedChanged", "Checked changed: " + chk.Name + " " + this.Name);
                if (this.Name.Contains("DewPoint"))
                {
                    TL.LogMessage("ChkEnabled_CheckedChanged", "Found DewPoint");
                    if (svHumidity != null)
                    {
                        if (chk.Checked != svHumidity.SensorEnabled)
                        {
                            TL.LogMessage("ChkEnabled_CheckedChanged", "Humidity is not the same as Dewpoint, alignining Humidity to DewPoint");
                            svHumidity.SensorEnabled = chk.Checked;
                        }
                        else
                        {
                            TL.LogMessage("ChkEnabled_CheckedChanged", "Humidity is the same as Dewpoint, no action required");
                        }
                    }
                    else TL.LogMessage("ChkEnabled_CheckedChanged", "Sensorview variable is null!");
                    svHumidity.NotReadyControlsEnabled = chk.Checked;
                }
                svThisSensor.NotReadyControlsEnabled = chk.Checked;
            }
            else
            {
                TL.LogMessage("ChkEnabled_CheckedChanged", "setupForm variable is null!");
            }
        }

        /// <summary>
        /// sets up the combo boxes and selects the current driver and device Id.
        /// </summary>
        public void InitUI()
        {
        }

    }
}
