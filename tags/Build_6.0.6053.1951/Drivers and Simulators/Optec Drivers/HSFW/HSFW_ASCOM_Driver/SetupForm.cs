using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OptecHID_FilterWheelAPI;
using System.Collections;
using System.Reflection;
using System.Diagnostics;


namespace ASCOM.HSFW_ASCOM_Driver
{

    public partial class SetupForm : Form
    {
        OptecHID_FilterWheelAPI.FilterWheel Device;
        PropertyGridClass pgc;

        public SetupForm(OptecHID_FilterWheelAPI.FilterWheel fw)
        {
            InitializeComponent();
            Device = fw;
        }

        private void OK_Btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetupForm_Load(object sender, EventArgs e)
        {
            pgc = new PropertyGridClass(Device);
            propertyGrid1.SelectedObject = pgc;

            // This adjusts the position of the horizontal splitter on the property grid.
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
                        info.SetValue(control, 7, null);

                        propertyGrid1.HelpVisible = true;
                        break;
                    }
                }
            }

            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        private void RestDefaults_Btn_Click(object sender, EventArgs e)
        {
            string msgtext = "Are you sure you want to restore all of the filter names " +
                "and wheel names back to their default values? You will lose all custom names you have stored.";
            string cap = "Resore Default Names?";
            DialogResult Answer = new DialogResult();
            Answer = MessageBox.Show(msgtext, cap, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Answer == DialogResult.Yes)
            {
                Device.RestoreDefaultNames();
                pgc = null;
                pgc = new PropertyGridClass(Device);
                propertyGrid1.SelectedObject = null;
                propertyGrid1.SelectedObject = pgc;
            }
        }

        


    }

    [TypeConverter(typeof(PropertySorter))]
    [DefaultProperty("WheelOfInterest")]
    class PropertyGridClass
    {
        private OptecHID_FilterWheelAPI.FilterWheel Device;
        char WheelOfInterest;
        string[] FilterNames;
        string[] WheelNames;

        public PropertyGridClass(OptecHID_FilterWheelAPI.FilterWheel fw)
        {
            Device = fw;
            WheelOfInterest = Device.WheelID;
            FilterNames = Device.GetFilterNames(WheelOfInterest);
            WheelNames = Device.GetWheelNames();
        }

        [Category("Device Properties"), PropertyOrder(0)]
        [DisplayName("Serial Number")]
        [Description("This is the serial number of the device assigned by Optec Inc. This number is non-volatile " +
            "and cannot be changed (Read-Only).")]
        public string SerialNumber
        {
            get
            {
                return Device.SerialNumber;

            }
            set { }
        }

        [TypeConverter(typeof(AvailableWheelIDs))]
        [Category("Wheel Info"), PropertyOrder(2)]
        [DisplayName("Wheel ID")]
        [Description("The Wheel Identifier. This is physically set on each wheel by the location of the magnet.")]
        public char WheelID
        {
            get { return WheelOfInterest; }
            set
            {
                {
                    WheelOfInterest = value;
                    FilterNames = Device.GetFilterNames(WheelOfInterest);
                }
            }
        }

        [Category("Wheel Info"), PropertyOrder(3)]
        [DisplayName("Wheel Name")]
        [Description("A Nick-Name for the wheel. Enter your own value for this property to help you " +
        "remember what this wheel is used for. " + "This name will get stored in the the devices non-volatile " +
            "memory and associated with the selected Wheel ID (A, B, C, etc)")]
        public string WheelName
        {
            get
            {
                int index = WheelOfInterest - 'A';
                return WheelNames[index];
            }
            set
            {
                int index = WheelOfInterest - 'A';
                Device.UpdateWheelName(WheelOfInterest, value);
                WheelNames[index] = value;
            }
        }


        [Category("Device Properties"), PropertyOrder(1)]
        [DisplayName("Centerig Offset")]
        [Description("The Centering Offset can be used to adjust the final position " +
            "of the wheel after each move or home. Setting the offset more positive will " +
            "cause the wheel to travel further clockwise. Setting the value more negative " +
            "will cause the wheel to travel further counter-clockwise.\n" +
            "Minimum value: -128\n" +
            " Maximum value: 127")]
        public short CenteringOffset
        {
            get { return Device.CenteringOffset; }
            set { Device.CenteringOffset = value; }
        }

        [Category("Device Properties"), PropertyOrder(4)]
        [DisplayName("Firmware Version")]
        [Description("This value is programed into the device during the manufacturing process. It represents the" +
            " version of the current firmware programmed into this specific device.")]
        public string FirmwareVersion
        {
            get { return Device.FirmwareVersion; }
            set { }
        }

        [Category("Wheel/Filter Properties"), PropertyOrder(5)]
        [DisplayName("Filter 1 Name")]
        [Description("A Nick-Name for Position 1 on the selected wheel")]
        public string Filter1Name
        {
            get
            {
                try
                {
                    return FilterNames[0];
                }
                catch
                {
                    throw new Exception("Not Available");
                }
            }
            set
            {
                Device.UpdateFilterName(WheelOfInterest, 1, value);
                FilterNames[0] = value;
            }
        }

        [Category("Wheel/Filter Properties"), PropertyOrder(5)]
        [DisplayName("Filter 1 Focus Offset")]
        [Description("A Focus Offset for the specified filter")]
        public int FocusOffset1
        {
            get
            {
                return HSFW_Handler.GetFocusOffset(Device.SerialNumber, WheelOfInterest, 1);
            }
            set
            {
                HSFW_Handler.SetFocusOffset(Device.SerialNumber, WheelOfInterest, 1, value);
            }
        }

        [Category("Wheel/Filter Properties"), PropertyOrder(6)]
        [DisplayName("Filter 2 Name")]
        [Description("A Nick-Name for Position 2 on the selected wheel")]
        public string Filter2Name
        {
            get
            {
                try
                {
                    return FilterNames[1];
                }
                catch
                {
                    throw new Exception("Not Available");
                }
            }
            set
            {
                Device.UpdateFilterName(WheelOfInterest, 2, value);
                FilterNames[1] = value;
            }
        }

        [Category("Wheel/Filter Properties"), PropertyOrder(6)]
        [DisplayName("Filter 2 Focus Offset")]
        [Description("A Focus Offset for the specified filter")]
        public int FocusOffset2
        {
            get
            {
                return HSFW_Handler.GetFocusOffset(Device.SerialNumber, WheelOfInterest, 2);
            }
            set
            {
                HSFW_Handler.SetFocusOffset(Device.SerialNumber, WheelOfInterest, 2, value);
            }
        }

        [Category("Wheel/Filter Properties"), PropertyOrder(7)]
        [DisplayName("Filter 3 Name")]
        [Description("A Nick-Name for Position 3 on the selected wheel")]
        public string Filter3Name
        {
            get
            {
                try
                {
                    return FilterNames[2];
                }
                catch
                {
                    throw new Exception("Not Available");
                }
            }
            set
            {
                Device.UpdateFilterName(WheelOfInterest, 3, value);
                FilterNames[2] = value;
            }
        }

        [Category("Wheel/Filter Properties"), PropertyOrder(7)]
        [DisplayName("Filter 3 Focus Offset")]
        [Description("A Focus Offset for the specified filter")]
        public int FocusOffset3
        {
            get
            {
                return HSFW_Handler.GetFocusOffset(Device.SerialNumber, WheelOfInterest, 3);
            }
            set
            {
                HSFW_Handler.SetFocusOffset(Device.SerialNumber, WheelOfInterest, 3, value);
            }
        }

        [Category("Wheel/Filter Properties"), PropertyOrder(8)]
        [DisplayName("Filter 4 Name")]
        [Description("A Nick-Name for Position 5 on the selected wheel")]
        public string Filter4Name
        {
            get
            {
                try
                {
                    return FilterNames[3];
                }
                catch
                {
                    throw new Exception("Not Available");
                }
            }
            set
            {
                Device.UpdateFilterName(WheelOfInterest, 4, value);
                FilterNames[3] = value;
            }
        }

        [Category("Wheel/Filter Properties"), PropertyOrder(8)]
        [DisplayName("Filter 4 Focus Offset")]
        [Description("A Focus Offset for the specified filter")]
        public int FocusOffset4
        {
            get
            {
                return HSFW_Handler.GetFocusOffset(Device.SerialNumber, WheelOfInterest, 4);
            }
            set
            {
                HSFW_Handler.SetFocusOffset(Device.SerialNumber, WheelOfInterest, 4, value);
            }
        }

        [Category("Wheel/Filter Properties"), PropertyOrder(9)]
        [DisplayName("Filter 5 Name")]
        [Description("A Nick-Name for Position 5 on the selected wheel")]
        public string Filter5Name
        {
            get
            {
                try
                {
                    return FilterNames[4];
                }
                catch
                {
                    throw new Exception("Not Available");
                }
            }
            set
            {
                Device.UpdateFilterName(WheelOfInterest, 5, value);
                FilterNames[4] = value;
            }
        }

        [Category("Wheel/Filter Properties"), PropertyOrder(9)]
        [DisplayName("Filter 5 Focus Offset")]
        [Description("A Focus Offset for the specified filter")]
        public int FocusOffset5
        {
            get
            {
                return HSFW_Handler.GetFocusOffset(Device.SerialNumber, WheelOfInterest, 5);
            }
            set
            {
                HSFW_Handler.SetFocusOffset(Device.SerialNumber, WheelOfInterest, 5, value);
            }
        }


        [Category("Wheel/Filter Properties"), PropertyOrder(10)]
        [DisplayName("Filter 6 Name")]
        [Description("A Nick-Name for Position 6 on the selected wheel")]
        public string Filter6Name
        {
            get
            {
                try
                {
                    return FilterNames[5];
                }
                catch
                {
                    throw new Exception("Not Available");
                }
            }
            set
            {
                Device.UpdateFilterName(WheelOfInterest, 6, value);
                FilterNames[5] = value;
            }
        }

        [Category("Wheel/Filter Properties"), PropertyOrder(10)]
        [DisplayName("Filter 6 Focus Offset")]
        [Description("A Focus Offset for the specified filter")]
        public int FocusOffset6
        {
            get
            {
                return HSFW_Handler.GetFocusOffset(Device.SerialNumber, WheelOfInterest, 6);
            }
            set
            {
                HSFW_Handler.SetFocusOffset(Device.SerialNumber, WheelOfInterest, 6, value);
            }
        }

        [Category("Wheel/Filter Properties"), PropertyOrder(11)]
        [DisplayName("Filter 7 Name")]
        [Description("A Nick-Name for Position 7 on the selected wheel")]
        public string Filter7Name
        {
            get
            {
                try
                {
                    return FilterNames[6];
                }
                catch
                {
                    throw new Exception("Not Available");
                }
            }
            set
            {
                Device.UpdateFilterName(WheelOfInterest, 7, value);
                FilterNames[6] = value;
            }
        }

        [Category("Wheel/Filter Properties"), PropertyOrder(11)]
        [DisplayName("Filter 7 Focus Offset")]
        [Description("A Focus Offset for the specified filter")]
        public int FocusOffset7
        {
            get
            {
                return HSFW_Handler.GetFocusOffset(Device.SerialNumber, WheelOfInterest, 7);
            }
            set
            {
                HSFW_Handler.SetFocusOffset(Device.SerialNumber, WheelOfInterest, 7, value);
            }
        }

        [Category("Wheel/Filter Properties"), PropertyOrder(12)]
        [DisplayName("Filter 8 Name")]
        [Description("A Nick-Name for Position 8 on the selected wheel")]
        public string Filter8Name
        {

            get
            {
                try
                {
                    return FilterNames[7];
                }
                catch
                {
                    throw new Exception("Not Available");
                }
            }
            set
            {
                Device.UpdateFilterName(WheelOfInterest, 8, value);
                FilterNames[7] = value;
            }
        }

        [Category("Wheel/Filter Properties"), PropertyOrder(12)]
        [DisplayName("Filter 8 Focus Offset")]
        [Description("A Focus Offset for the specified filter")]
        public int FocusOffset8
        {
            get
            {
                return HSFW_Handler.GetFocusOffset(Device.SerialNumber, WheelOfInterest, 8);
            }
            set
            {
                HSFW_Handler.SetFocusOffset(Device.SerialNumber, WheelOfInterest, 8, value);
            }
        }
    }

    public class AvailableWheelIDs : CharConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            StandardValuesCollection x = new StandardValuesCollection(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' });
            return x;
        }
    }
}
