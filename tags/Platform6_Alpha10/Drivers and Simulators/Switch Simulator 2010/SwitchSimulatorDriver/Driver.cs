using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ASCOM.DeviceInterface;
using ASCOM.DriverAccess;
using ASCOM.Utilities;

namespace ASCOM.Simulator
{
    //
    // Your driver's DeviceID is ASCOM.Simulator.Switch
    //
    // The Guid attribute sets the CLSID for ASCOM.Simulator.Switch
    // The ClassInterface/None addribute prevents an empty interface called
    // _Conceptual from being created and used as the [default] interface
    //

    /// <summary>
    /// ASCOM Switch Driver for a conceptual switch (proof of concept).
    /// This class is the implementation of the public ASCOM interface.
    /// </summary>
    [Guid("1F07419A-0C9E-4B90-8B62-FC8053E89EE2")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Switch : ISwitchV2, IDisposable
    {
        #region Constants

        /// <summary>
        /// Name of the Driver
        /// </summary>
        private const string name = "ASCOM.Simulator.Switch";

        /// <summary>
        /// Description of the driver
        /// </summary>
        private const string description = "ASCOM Switch Simulator Driver";

        /// <summary>
        /// Device type
        /// </summary>
        private const string deviceType = "Switch";
        
        /// <summary>
        /// Driver information
        /// </summary>
        private const string driverInfo = "Switch Simulator Driver and collection of Switch devices";

        /// <summary>
        /// Driver interface version
        /// </summary>
        private const short interfaceVersion = 2;

        /// <summary>
        /// Driver version number
        /// </summary>
        private const string driverVersion = "6.0";

        /// <summary>
        /// Gets the last result.
        /// </summary>
        private const string lastResult = "False";

        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        private const string sCsDriverId = "ASCOM.Simulator.Switch";

        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private const string sCsDriverDescription = "ASCOM Simulator Switch Driver";

        /// <summary>
        /// The number of physical switches that this device has.
        /// </summary>
        private const int numSwitches = 7;

        /// <summary>
        /// Type of switches for this driver
        /// </summary>
        private const string switchType = "ToggleSwitch";

        /// <summary>
        /// Backing store for the private switch collection.
        /// </summary>
        private static readonly ArrayList SwitchList = new ArrayList(numSwitches);

        /// <summary>
        /// Sets up the permenant store for saved settings
        /// </summary>
        private static readonly Profile Profile = new Profile();

        /// <summary>
        /// Sets up the permenant store for device names
        /// </summary>
        private static readonly string[] DeviceNames = {
                                                           "White Lights", "Red Lights", "Telescope Power",
                                                           "Camera Power",
                                                           "Focuser Power", "Dew Heaters", "Dome Power", "Self Destruct"
                                                       };

        #endregion

        #region Constructors

        //
        // PUBLIC COM INTERFACE ISwitch IMPLEMENTATION
        //

        /// <summary>
        /// Initializes a new instance of the <see cref="Switch"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Switch()
        {
            //new instance so load switches
            Profile.DeviceType = deviceType;
            LoadSwitchDevices();
            SaveProfileSettings();
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Dispose();
        }

        #endregion

        #region ISwitchV2 Members

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// </summary>
        public void SetupDialog()
        {
            var f = new SetupDialogForm();
            DialogResult dialogResult = f.ShowDialog();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Switch"/> is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Connected { get; set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return description; }
        }

        /// <summary>
        /// Gets the driver info.
        /// </summary>
        /// <value>The driver info.</value>
        public string DriverInfo
        {
            get { return driverInfo; }
        }

        /// <summary>
        /// Gets the driver version.
        /// </summary>
        /// <value>The driver version.</value>
        public string DriverVersion
        {
            get { return driverVersion; }
        }

        /// <summary>
        /// Gets the interface version.
        /// </summary>
        /// <value>The interface version.</value>
        public short InterfaceVersion
        {
            get { return interfaceVersion; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Yields a collection of string[] objects.
        /// </summary>
        /// <value></value>
        public ArrayList Switches
        {
            get { return SwitchList; }
        }

        void ISwitchV2.Dispose()
        {
            Dispose();
        }

        /// <summary>
        /// return a specific switch
        /// </summary>
        /// <value>name of the switch</value>
        public object GetSwitch(string switchName)
        {
            if (switchName == null) throw new ArgumentNullException("switchName");
            return SwitchList.Cast<ToggleSwitch>().FirstOrDefault(t => t.Name == switchName);
        }

        /// <summary>
        /// Flips a switch on or off
        /// </summary>
        /// <value>name of the switch</value>
        public void SetSwitch(string switchName, string[] state)
        {
            if (switchName == null) throw new ArgumentNullException("switchName");
            foreach (ToggleSwitch t in from ToggleSwitch t in SwitchList where t.Name == switchName select t)
            {
                t.State = state;
            }
            SaveProfileSettings();
        }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        public string Action(string actionName, string actionParameters)
        {
            return null;
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not 
        /// wait for a response. Optionally, protocol framing 
        /// characters may be added to the string before transmission.
        /// </summary>
        public void CommandBlind(string command, bool raw)
        {
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits 
        /// for a boolean response. Optionally, protocol framing 
        /// characters may be added to the string before transmission.
        /// </summary>
        public bool CommandBool(string command, bool raw)
        {
            return false;
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits 
        /// for a string response. Optionally, protocol framing 
        /// characters may be added to the string before transmission.
        /// </summary>
        public string CommandString(string command, bool raw)
        {
            return null;
        }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        public ArrayList SupportedActions
        {
            // no supported actions, return empty array
            get
            {
                var sa = new ArrayList();
                return sa;
            }
        }

        /// <summary>
        /// Gets the driver version.
        /// </summary>
        /// <value>The driver version.</value>
        public string SwitchType
        {
            get { return switchType; }
        }

        #endregion

        private static void Dispose()
        {
        }

        /// <summary>
        /// Simulate reading the hardware devices
        /// </summary>
        private static void LoadSwitchDevices()
        {
            foreach (string deviceName in DeviceNames)
            {
                string v = Profile.GetValue(sCsDriverId, deviceName, "Switches");
                if ((v != "On") && (v != "Off"))
                {
                    v = "Off";
                }
                var toggleSwitch = new ToggleSwitch
                                       {
                                           Name = deviceName,
                                           DeviceType = switchType,
                                           State = new[] {v}
                                       };
                int add = SwitchList.Add(toggleSwitch);
            }
        }

        /// <summary>
        /// Saves all settings to the profile for this driver
        /// </summary>
        private static void SaveProfileSettings()
        {
            foreach (ToggleSwitch t in SwitchList)
            {
                if (Profile != null) Profile.WriteValue(sCsDriverId, t.Name, t.State[0], "Switches");
            }
        }

        #region ASCOM Registration

        //
        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            var p = new Profile {DeviceType = "Switch"};
            if (bRegister)
            {
                p.Register(sCsDriverId, sCsDriverDescription);
                p.CreateSubKey(sCsDriverId, "Switches"); //Driver instantiation fails if this subkey is not present
            }
            else
            {
                p.Unregister(sCsDriverId);
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            Trace.WriteLine("Registering -> {0} with ASCOM Profile", sCsDriverId);
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            Trace.WriteLine("Unregistering -> {0} with ASCOM Profile", sCsDriverId);
            RegUnregASCOM(false);
        }

        #endregion
    }
}