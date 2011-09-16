using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PyxisLE_API;
using System.Reflection;

namespace PyxisLE_Control
{
    public partial class AdvancedForm : Form
    {
        private Rotator myRotator;

        public AdvancedForm(Rotator r)
        {
            InitializeComponent();
            myRotator = r;
        }

        private void AdvancedForm_Load(object sender, EventArgs e)
        {
            RotatorAdvancedSettingsUI UIClass = new RotatorAdvancedSettingsUI(myRotator);
            this.propertyGrid1.SelectedObject = UIClass;

            Type propertygridtype = propertyGrid1.GetType();
            FieldInfo y = propertygridtype.GetField("gridView",
                BindingFlags.NonPublic | BindingFlags.Instance);
            y.FieldType.GetMethod("MoveSplitterTo",
                BindingFlags.NonPublic | BindingFlags.Instance).Invoke(y.GetValue(propertyGrid1), new object[] { 177 });

            try
            {
                var info = propertyGrid1.GetType().GetProperty("Controls");
                var collection = (Control.ControlCollection)info.GetValue(propertyGrid1, null);

                foreach (var control in collection)
                {
                    var type = control.GetType();

                    if ("DocComment" == type.Name)
                    {
                        const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;
                        var field = type.BaseType.GetField("userSized", Flags);
                        field.SetValue(control, true);

                        info = type.GetProperty("Lines");
                        info.SetValue(control, 8, null);

                        propertyGrid1.HelpVisible = true;
                        break;
                    }
                }
            }
            catch{}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string msg = String.Empty;
            msg = "The following properties will be restored as shown..." + Environment.NewLine + Environment.NewLine;
            msg += "Reverse = FALSE"+ Environment.NewLine;
            msg += "Backlash = Enabled" + Environment.NewLine;
            msg += "Backlash Steps = 20" + Environment.NewLine;
            msg += "Zero Offset = 0" + Environment.NewLine;
            msg += "Sky PA Offset = 0" + Environment.NewLine;
            msg += "AND THE DEVICE WILL BE FORCED TO HOME." + Environment.NewLine + Environment.NewLine;
            msg+= "Are you sure you want to proceed with restore?";

            DialogResult result = MessageBox.Show(
                msg, 
                "Restore Default Settings?", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                myRotator.RestoreDefaults();
            }
             
        }
    }

    class RotatorAdvancedSettingsUI
    {
        private Rotator myRotator;

        public RotatorAdvancedSettingsUI(Rotator r)
        {
            myRotator = r;
        }

        [Category("Application Settings")]
        [DisplayName("Check for Updates")]
        [Description("Configure whether the application automatically checks for updates on startup or not.")]
        public bool CheckForUpdates
        {
            get { return XmlSettings.CheckForUpdates; }
            set
            {
                XmlSettings.CheckForUpdates = value;
            }
        }

        [Category("Device Settings")]
        [DisplayName("Zero Offset (°)")]
        [Description("Adjust the rotators zero point in units of degrees. This is used when homing the device." + 
            " Changing this value will force the device to home.")]
        public double ZeroOffset
        {
            get {
                int steps = myRotator.ZeroOffset;
                int deg = steps / (myRotator.StepsPerRev / 360);

                return (short)deg;
            }
            set 
            {

                string msg = "CAUTION: Changing this property may cause the rotator to travel more than 360°. " + Environment.NewLine +
                     "The device will automatically home after the Zero Offset is changed. Changing the Zero Offset essentially changes the" + Environment.NewLine +
                     " point that the rotator 'thinks' is zero degrees PA. During a home the rotator may travel up to (360 + Zero Offset) degrees." + Environment.NewLine +
                     "Are you sure you want to continue?";
                DialogResult r = MessageBox.Show(msg, "CAUTION", MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    // convert the degree value to steps
                    double StepsPerDegree =   myRotator.StepsPerRev / 360;

                    double newvalue = (value * StepsPerDegree);
                    // Set the value in the rotator
                    myRotator.ZeroOffset = (short)newvalue;
                }
            }
        }

        [Category("Device Settings")]
        [Description("Use this property to reverse the direction of travel for move commands. " + 
            "The False setting will result in counter-clockwise positive moves. The True setting " + 
            "will result in clockwise positive moves.")]
        public bool Reverse
        {
            get { return myRotator.Reverse; }
            set {
                string msg = "ATTENTION: Changing the reverse property will cause the device to re-home. " + Environment.NewLine + 
                    "The reverse property should be used to switch the direction that the rotator will travel when given move commands. " + 
                    "This essentially flips the x-axis. " + Environment.NewLine + "Would you like to continue?";
               DialogResult r = MessageBox.Show(msg, "Continue?", MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);
                if(r == DialogResult.Yes) myRotator.Reverse = value; 
            }
        }

        [Category("Device Settings")]
        [DisplayName("Backlash Comp. Enabled")]
        public bool BacklashComp
        {
            get
            {
                return myRotator.BacklashEnabled;
            }
            set
            {
                myRotator.BacklashEnabled = value;
            }
        }

        [Category("Device Settings")]
        [DisplayName("Backlash Steps")]
        public short bklsteps
        {
            get{ return myRotator.BacklashSteps;}
            set{ myRotator.BacklashSteps = value;}
        }

        [Category("Device Description")]
        [Description("The unique serial number for the connected device.")]
        [DisplayName("SerialNumber")]
        public string SerialNumber
        {
            get { return myRotator.SerialNumber; }
            set { /* Read Only*/}
        }

        [Category("Device Description")]
        [DisplayName("Resolution (Steps per Rev)")]
        [Description("The number of stepper motor steps per revolution for the connected device.")]
        public string Resolution
        {
            get { return myRotator.StepsPerRev.ToString();}
            set { /* Read Only*/}
        }

        [Category("Device Description")]
        [Description("The revision of the firmware programmed in the device.")]
        [DisplayName("Firmware Version")]
        public string FirmwareVersion
        {
            get { return myRotator.FirmwareVersion; }
            set { /* Read Only*/}
        }

        [Category("Device Settings")]
        [Description("The position angle that the device travels to during a park routine. This value " +
            "is relative to the devices position angle. It is NOT affected by changes to Sky PA. Changes to " +
            "the Zero Offset WILL affect the park position.")]
        [DisplayName("Park Position(°)")]
        public float ParkPosition
        {
            get { return XmlSettings.ParkPosition; }
            set { XmlSettings.ParkPosition = value; }
        }

    }
}
