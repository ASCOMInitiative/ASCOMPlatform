using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Reflection;
using System.Diagnostics;
namespace ASCOM.OptecTCF_S
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            TCFSettings x = new TCFSettings();
            propertyGrid1.SelectedObject = x;
            
            Type propertygridtype = propertyGrid1.GetType();
            FieldInfo y = propertygridtype.GetField("gridView", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            y.FieldType.GetMethod("MoveSplitterTo",
                BindingFlags.NonPublic | BindingFlags.Instance).Invoke(y.GetValue(propertyGrid1), new object[] { 175 });

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

            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            
        }

        private void Close_BTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
    
    public class TCFSettings
    {
           
        
        [CategoryAttribute("Temperature Compensation")]
        [DescriptionAttribute("This property is automatically set when the probe is removed/connected")]
        [DisplayName("Temp. Probe Enabled")]
        public bool TemperatureProbeEnabled
        {
            get { return DeviceSettings.TempProbePresent; }
           // set {
           //     OptecFocuser.SetTempProbeEnabled(value);
            //    DeviceSettings.TempProbePresent = value; 
           // }
        }

        [CategoryAttribute("Temperature Compensation")]
        [DescriptionAttribute("This property can be used to offset the temperature read by the probe." + 
            " The allowed range of offsets is -5 to 5°.")]
        [DisplayName("Temperature Offset")]
        public double TemperatureOffset
        {
            get { return DeviceSettings.TemperatureOffset; }
            set
            {
                DeviceSettings.TemperatureOffset = value;
            }
        }

        [CategoryAttribute("Temperature Compensation")]
        [DescriptionAttribute("Set which temperature compensation mode is to be used.")]
        [DisplayName("Active Temp Comp Mode")]
        [TypeConverter(typeof(AvailableTempCompModes))]
        public char TempCompMode
        {
            get { return DeviceSettings.ActiveTempCompMode; }
            set { DeviceSettings.ActiveTempCompMode = value; }
        }

        [CategoryAttribute("Temperature Compensation")]
        [DescriptionAttribute("Set a friendly name for Mode A Temparature Compensation.")]
        [DisplayName("Mode A Name")]
        public string ModeAName
        {
            get { return DeviceSettings.ModeA_Name; }
            set { DeviceSettings.ModeA_Name = value; }
        }

        [CategoryAttribute("Temperature Compensation")]
        [DescriptionAttribute("Set a delay for Mode A Temparature Compensation. This determines " +
            "how often the device adjusts the position when temperature compensation is active.")]
        [DisplayName("Mode A Update Delay")]
        public int ModeADelay
        {
            get { return DeviceSettings.ModeA_Delay; }
            set 
            {
                if (!Connect) throw new ASCOM.InvalidOperationException("You must set Connect to True in order to modify this setting.");
                try
                {
                    OptecFocuser.SetFocuserDelay(value, 'A');
                    DeviceSettings.ModeA_Delay = value;
                }
                catch
                {
                    throw new DriverException("Unable to set temp comp delay. Communication with the device failed");
                }
                

            }
        }


        [CategoryAttribute("Temperature Compensation")]
        [DescriptionAttribute("Set a friendly name for Mode B Temparature Compensation.")]
        [DisplayName("Mode B Name")]
        public string ModeBName
        {
            get { return DeviceSettings.ModeB_Name; }
            set { DeviceSettings.ModeB_Name = value; }
        }

        [CategoryAttribute("Temperature Compensation")]
        [DescriptionAttribute("Set a delay for Mode B Temparature Compensation. This determines " +
            "how often the device adjusts the position when temperature compensation is active." )]
        [DisplayName("Mode B Update Delay")]
        public int ModeBDelay
        {
            get { return DeviceSettings.ModeB_Delay; }
            set 
            {
                if (!Connect) throw new ASCOM.InvalidOperationException("You must set Connect to True in order to modify this setting.");
                try
                {
                    OptecFocuser.SetFocuserDelay(value, 'B');
                    DeviceSettings.ModeB_Delay = value;
                }
                catch
                {
                    throw new DriverException("Unable to set temp comp delay. Communication with the device failed");
                }
                

            }
        }

        [CategoryAttribute("Device Properties")]
        [DescriptionAttribute("Set the type of focuser you have.\n"+
            "NOTE: You must Connect to the device before setting this property.\n" +
            "NOTE: You must REBOOT the device after changing device type for change to take effect.")]
        [DisplayName("Device Type")]
        public DeviceSettings.DeviceTypes DeviceType
        {
            get { return DeviceSettings.DeviceType; }
            set
            {
                DialogResult answer = new DialogResult();
                answer = MessageBox.Show("If you change the device type you will have to reboot the device. Are you sure you want to proceed?",
                    "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (answer == DialogResult.No)
                {
                    return;
                }

                if (!Connect) throw new ASCOM.InvalidOperationException("You must set Connect to True in order to modify this setting.");
                if (value == DeviceSettings.DeviceTypes.Unknown) throw new ASCOM.DriverException("Unacceptable Device Type");
                try
                {
                    OptecFocuser.SetDeviceType(value);
                    DeviceSettings.DeviceType = value;
                    MessageBox.Show("Device type has been successfully changed. \nREBOOT YOUR FOCUSER NOW");
                }
                catch
                {
                    throw new DriverException("Unable to set device type. Communication with the device failed");
                }
            }
        }

        [CategoryAttribute("Device Properties")]
        [DescriptionAttribute("The firmware version in the device.")]
        [DisplayName("Firmware Version")]
        public string FirmwareVersion
        {
            get { return DeviceSettings.FirmwareVersion; }
        }

        [CategoryAttribute("Connection Settings")]
        [DisplayName("COM Port")]
        [Description("Select the COM port to use for connecting to your focuser.")]
        [TypeConverter(typeof(AvailableComPorts))]
        public string ComPortName
        {
            get { return DeviceSettings.COMPort; }
            set 
            {
                if (OptecFocuser.ConnectionState != OptecFocuser.ConnectionStates.Disconnected)
                    throw new ASCOM.DriverException("You must disconnect before changing the COM port");
                DeviceSettings.COMPort = value; 
            }
        }

        [CategoryAttribute("Connection Settings")]
        [DisplayName("Connect")]
        [Description("Set True to connect to device. Set False to disconnect")]
        public bool Connect
        {
            get
            {
                if (OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.SerialMode)
                    return true;
                else return false;
            }
            set 
            {
                if (value) OptecFocuser.ConnectAndEnterSerialMode();
                else OptecFocuser.Disconnect();
            }
        }


        [CategoryAttribute("Temperature Coefficients")]
        [DisplayName("Mode 'A' Coefficient")]
        [Description("Enter the temperature coefficient for Mode A. This determines " + 
            "how many steps the focuser moves for every degree change in temperature")]
        public int ModeACoeff
        {
            get 
            {
                if (OptecFocuser.ConnectionState != OptecFocuser.ConnectionStates.SerialMode)
                    throw new DriverException("Must Be Connected");
                try
                {
                    return OptecFocuser.GetLearnedSlope('A');
                }
                catch { throw new ASCOM.DriverException("???"); }
            }
            set { OptecFocuser.SetSlope(value, 'A'); }
        }

        [CategoryAttribute("Temperature Coefficients")]
        [DisplayName("Mode 'B' Coefficient")]
        [Description("Enter the temperature coefficient for Mode B. This determines " +
            "how many steps the focuser moves for every degree change in temperature")]
        public int ModeBCoeff
        {
            get 
            {
                if (OptecFocuser.ConnectionState != OptecFocuser.ConnectionStates.SerialMode)
                    throw new DriverException("Must Be Connected");
                try
                {
                    return OptecFocuser.GetLearnedSlope('B');
                }
                catch { throw new ASCOM.DriverException("???"); }
            }
            set { OptecFocuser.SetSlope(value, 'B'); }
        }
    }

    public class AvailableComPorts : StringConverter
    {
        public override bool  GetStandardValuesSupported(ITypeDescriptorContext context)
        {
             return true;
        }

        public override StandardValuesCollection  GetStandardValues(ITypeDescriptorContext context)
        {
 	         StandardValuesCollection x = new StandardValuesCollection(SerialPort.GetPortNames());
            return x;
        } 
    }

    public class AvailableTempCompModes : CharConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            StandardValuesCollection x = new StandardValuesCollection(new char[] { 'A', 'B' });
            return x;
        }
    }
}
