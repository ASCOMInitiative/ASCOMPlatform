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

        /// <summary>
        /// The sensor name must match the name in Hub.Sensors[property]
        /// </summary>
        public string Units {
            get
            {
                return lblUnits.Text;
            }
            set
            {
                lblUnits.Text = value;
            }
        }

        public double LowValue
        {
            get
            {
                return Convert.ToDouble(txtMinValue.Text);
            }
            set
            {
                txtMinValue.Text = value.ToString();
            }
        }

        public double MaxValue
        {
            get
            {
                return Convert.ToDouble(txtMaxValue.Text);
            }
            set
            {
                txtMaxValue.Text = value.ToString();
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
            TL = OCSimulator.TL;
            TL.LogMessage("SensorView", "This.Name: " + this.Name);
        }

        /// <summary>
        /// sets up the combo boxes and selects the current driver and device Id.
        /// </summary>
        public void InitUI()
        {
        }

    }
}
