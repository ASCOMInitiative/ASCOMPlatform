using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using OptecHID_FilterWheelAPI;
using System.Reflection;
using System.Diagnostics;

namespace ASCOM.HighSpeedFilterWheelUSB
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {

        internal static Utilities.Profile myProfile = new Utilities.Profile();
        internal static OptecHID_FilterWheelAPI.FilterWheels FilterWheelManager = new FilterWheels();

        public SetupDialogForm()
        {
            InitializeComponent();
            myProfile.DeviceType = "FilterWheel"; 
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void BrowseToAscom(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            RefreshAvailableDevicesCB();
            ChooseSelectedSerialNumber();

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
                        info.SetValue(control, 6, null);

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

        private void ChooseSelectedSerialNumber()
        {
            string SN = myProfile.GetValue(FilterWheel.s_csDriverID, 
                ASCOMFilterWheel.ProfileStrings.SelectedSerialNumber.ToString(),
                "", "None Selected");

            if(SN == "None Selected") return;   // As to not waste time...

            foreach (object x in AvailableDevice_CB.Items)
            {
                OptecHID_FilterWheelAPI.FilterWheel fw = (OptecHID_FilterWheelAPI.FilterWheel)x;
                if (fw.SerialNumber == SN)
                {
                    AvailableDevice_CB.SelectedItem = x;
                    PropertyGridClass pgc = new PropertyGridClass(fw);
                    propertyGrid1.SelectedObject = pgc;
                    break;
                }
            }
        }

        private void RefreshAvailableDevicesCB()
        {
            AvailableDevice_CB.Items.Clear();
            AvailableDevice_CB.DisplayMember = "SerialNumber";
            foreach (OptecHID_FilterWheelAPI.FilterWheel fw in FilterWheelManager.FilterWheelList)
            {
                AvailableDevice_CB.Items.Add(fw);
            }
        }

        private void AvailableDevice_CB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox CB = sender as ComboBox;
            OptecHID_FilterWheelAPI.FilterWheel fw = CB.SelectedItem as OptecHID_FilterWheelAPI.FilterWheel;
            myProfile.WriteValue(FilterWheel.s_csDriverID,
                ASCOMFilterWheel.ProfileStrings.SelectedSerialNumber.ToString(), fw.SerialNumber);
            PropertyGridClass pgc = new PropertyGridClass(fw);
            propertyGrid1.SelectedObject = pgc;
        }

        private void Home_Btn_Click(object sender, EventArgs e)
        {
            OptecHID_FilterWheelAPI.FilterWheel fw = this.AvailableDevice_CB.SelectedItem as 
                OptecHID_FilterWheelAPI.FilterWheel;
            if (fw != null)
            {
                fw.HomeDevice();
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

        [TypeConverter(typeof(AvailableWheelIDs))]
        [Category("Properties"), PropertyOrder(1)]
        [DisplayName("1. Wheel ID")]
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

        [Category("Properties"), PropertyOrder(2)]
        [DisplayName("2. Wheel Name")]
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

        [Category("Properties"), PropertyOrder(3)]
        [DisplayName("Centerig Offset")]
        [Description("The Centering Offset can be used to adjust the final position " +
            "of the wheel after each move or home operation. " +
            "Range = -128 to +127")]
        public sbyte CenteringOffset
        {
            get { return Device.CenteringOffset; }
            set { Device.CenteringOffset = value; }
        }

        [Category("Properties"), PropertyOrder(4)]
        [DisplayName("Firmware Version")]
        public string FirmwareVersion
        {
            get { return Device.FirmwareVersion; }
            set { }
        }

        [Category("Properties"), PropertyOrder(5)]
        [DisplayName("Filter 1 Name")]
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

        [Category("Properties"), PropertyOrder(6)]
        [DisplayName("Filter 2 Name")]
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

        [Category("Properties"), PropertyOrder(7)]
        [DisplayName("Filter 3 Name")]
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

        [Category("Properties"), PropertyOrder(8)]
        [DisplayName("Filter 4 Name")]
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

        [Category("Properties"), PropertyOrder(9)]
        [DisplayName("Filter 5 Name")]
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

        [Category("Properties"), PropertyOrder(10)]
        [DisplayName("Filter 6 Name")]
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

        [Category("Properties"), PropertyOrder(11)]
        [DisplayName("Filter 7 Name")]
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

        [Category("Properties"), PropertyOrder(12)]
        [DisplayName("Filter 8 Name")]
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