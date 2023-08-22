using System;
using System.Windows.Forms;

namespace ASCOM.Simulator
{
    /// <summary>
    /// Handles selecting the Switch or ObservingConditions driver and, for the switch,
    /// selecting the switch id either using a combo of switch names or a spin button to
    /// select the switch id.
    /// Also allows calling the setup dialogue for the selected driver.
    /// </summary>
    public partial class SensorView : UserControl
    {
        TraceLoggerPlus TL;
        /// <summary>
        /// The sensor name must match the name in Hub.Sensors[property]
        /// </summary>
        public string SensorName { get; set; }

        public double NotReadyDelay
        {
            get
            {
                if (DesignMode) return 0.0;
                else return Convert.ToDouble(numDelay.Value);
            }
            set { numDelay.Value = Convert.ToDecimal(value); }
        }

        public bool SensorEnabled
        {
            get
            {
                if (DesignMode) return true;
                else return chkEnabled.Checked;
            }
            set { chkEnabled.Checked = value; }
        }

        public double MinValue
        {
            get
            {
                if (DesignMode) return 0.0;
                else return Convert.ToDouble(txtMinValue.Text);
            }
            set { txtMinValue.Text = value.ToString(); }
        }

        public double ValueCycleTime
        {
            get
            {
                if (DesignMode) return 0.0;
                else return Convert.ToDouble(numValueCycleTime.Value);
            }
            set { numValueCycleTime.Value = Convert.ToDecimal(value); }
        }

        public double MaxValue
        {
            get
            {
                if (DesignMode) return 0.0;
                else return Convert.ToDouble(txtMaxValue.Text);
            }
            set { txtMaxValue.Text = value.ToString(); }
        }

        public bool EnabledCheckboxVisible
        {
            get
            {
                if (DesignMode) return true;
                else return chkEnabled.Visible;
            }
            set { chkEnabled.Visible = value; }
        }

        public bool NotReadyControlsEnabled
        {
            get
            {
                if (DesignMode) return true;
                else return txtMinValue.Enabled;
            }
            set
            {
                numDelay.Enabled = value;
                numValueCycleTime.Enabled = value;
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
            if (!DesignMode)
            {
                CheckBox chk = (CheckBox)sender;
                if (OCSimulator.setupForm != null)
                {
                    SensorView svThisSensor = (SensorView)OCSimulator.setupForm.Controls[this.Name];

                    TL.LogMessage("ChkEnabled_CheckedChanged", "Checked changed: " + chk.Name + " " + this.Name);
                    svThisSensor.NotReadyControlsEnabled = chk.Checked;
                }
                else
                {
                    TL.LogMessage("ChkEnabled_CheckedChanged", "setupForm variable is null!");
                }
            }
        }
    }
}
