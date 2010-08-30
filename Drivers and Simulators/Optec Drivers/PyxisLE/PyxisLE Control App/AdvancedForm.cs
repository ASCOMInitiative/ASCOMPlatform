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
            get { return Properties.Settings.Default.CheckForUpdates; }
            set
            {
                Properties.Settings.Default.CheckForUpdates = value;
                Properties.Settings.Default.Save();
            }
        }

        [Category("Settings")]
        [DisplayName("Zero Offset")]
        [Description("Adjust the rotators zero point. This is used when homing the device. Changing this value will force the device to home.")]
        public short ZeroOffset
        {
            get { return myRotator.ZeroOffset; }
            set 
            {

                string msg = "ATTENTION: Changing the reverse property will cause the device to re-home. " + Environment.NewLine +
                     "The Zero-Offset property is used to change the point that the rotator 'thinks' is zero degrees PA. The units for " + 
                     "this property are in stepper motor steps. Positive and negative values are allowed.";
                DialogResult r = MessageBox.Show(msg, "Continue?", MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question);
                if (r == DialogResult.Yes) myRotator.ZeroOffset = value; 
            }
        }

        [Category("Settings")]
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

        //[Category("Settings")]
        //[DisplayName("Return to Last")]
        //public bool ReturnToLast
        //{
        //    get { return myRotator.ReturnToLastOnHome; }
        //    set { myRotator.ReturnToLastOnHome = value; }
        //}

        [Category("Device Description")]
        [Description("The unique serial number for the connected device.")]
        [DisplayName("SerialNumber")]
        public string SerialNumber
        {
            get { return myRotator.SerialNumber; }
            set { /* Read Only*/}
        }

        [Category("Device Description")]
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

    }
}
