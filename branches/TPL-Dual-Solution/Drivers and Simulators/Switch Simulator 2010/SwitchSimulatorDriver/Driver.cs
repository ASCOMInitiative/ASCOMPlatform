using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
    [Guid("dc17f874-056a-41c2-b7aa-7cd6df8ecf63")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Switch : ISwitch
    {
        #region Constants
        /// <summary>
        /// Stores the state of the connection
        /// </summary>
        private static bool connected = false;

        /// <summary>
        /// Name of the Driver
        /// </summary>
        private static string name = "ASCOM.Simulator.Switch";

        /// <summary>
        /// Description of the driver
        /// </summary>
        private static string description = "ASCOM Switch Simulator Driver";

        /// <summary>
        /// Driver information
        /// </summary>
        private static string driverInfo = "Switch Simulator Driver and collection of Switch devices";
        
        /// <summary>
        /// Driver interface version
        /// </summary>
        private static short interfaceVersion = 1;

        /// <summary>
        /// Driver version number
        /// </summary>
        private static string driverVersion = "6.0";

        /// <summary>
        /// Backing store for the private switch collection.
        /// </summary>
        private static ArrayList switchDevices = new ArrayList(numSwitches);

        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        private static string s_csDriverID = "ASCOM.Simulator.Switch";

        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string s_csDriverDescription = "ASCOM Simulator Switch Driver";

        /// <summary>
        /// The number of physical switches that this device has.
        /// </summary>
        private const int numSwitches = 8;

        /// <summary>
        /// Sets up the permenant store for saved settings
        /// </summary>
        private static Utilities.Profile Profile = new Profile();

        /// <summary>
        /// Sets up the permenant store for device names
        /// </summary>
        private static string[] deviceNames = { "White Lights", "Red Lights", "Telescope Power", "Camera Power", "Focuser Power", "Dew Heaters", "Dome Power", "Self Destruct" };


        #endregion

        #region ISwitch Public Members

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
            LoadSwitchDevices();
            //check to see if the profile is ok
            if (ValidateProfile())
            {
                //load profile settings
                GetProfileSettings();
            }
            else
            {
                //attempt to save a new profile
                SaveProfileSettings();
            }
        }

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// </summary>
        public void SetupDialog()
        {
            SetupDialogForm F = new SetupDialogForm();
            var result = F.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.Save();
                return;
            }
            Properties.Settings.Default.Reload();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Switch"/> is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Connected
        {
            get
            {
                return connected;
            }
            set
            {
                connected = value;
            }
        }

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
            get
            {
                return driverVersion;
            }
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
        /// Yields a collection of ISwitchDevice objects.
        /// </summary>
        /// <value></value>
        public System.Collections.ArrayList SwitchDevices
        {
            get
            {
                return switchDevices;
            }
        }

        #endregion

        #region Switch Private Members

        /// <summary>
        /// Load the switches and makes sure there is only 8 loaded
        /// </summary>
        private static void LoadSwitchDevices()
        {
            //each new instance load the switches, make sure this doesn't repeat
            if (switchDevices.Count != 8)
            {
                //too many or not enough found, start over
                switchDevices.Clear();
                //load a new set of switch devices
                foreach (string device in deviceNames)
                {
                    switchDevices.Add(new SwitchDevice(device.ToString()));
                }
            }
        }

        /// <summary>
        /// Saves all settings to the profile for this driver
        /// </summary>
        private static void GetProfileSettings()
        {
            foreach (SwitchDevice sw in switchDevices)
            {
                bool state = System.Convert.ToBoolean(GetProfileSetting(sw.Name, "false"));
                if (state != sw.State)
                {
                    sw.State = state;
                }
            }
         }

        /// <summary>
        /// Validate the profile is in good shape
        /// </summary>
        private static bool ValidateProfile()
        {
            try
            {
                Profile.DeviceType = "Switch";
                //check profile if the driver id is registered
                bool chkRegistered = Profile.IsRegistered(s_csDriverID);
                if (chkRegistered)
                {
                    //check proffile to see if the subkey is loaded 
                    ArrayList pv = Profile.Values(s_csDriverID, "SwitchDevices");
                    if (pv.Count == 8)
                    {
                        //check profile to see if key each named device exist
                        foreach (string device in deviceNames)
                        {
                            string s = Profile.GetValue(s_csDriverID, device.ToString(), "SwitchDevices");
                            if (s == "")
                            {
                                //found something wrong, delete evertyhing
                                DeleteProfileSettings();
                                return false;
                            }
                        }
                        return true;
                    }
                    else
                    {
                        DeleteProfileSettings();
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch(System.IO.DirectoryNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Saves all settings to the profile for this driver
        /// </summary>
        private static void SaveProfileSettings()
        {
            foreach (SwitchDevice sd in switchDevices)
            {
                Profile.WriteValue(s_csDriverID,sd.Name, sd.State.ToString(), "SwitchDevices");
            }
        }

        /// <summary>
        /// Loads a specific setting from the profile
        /// </summary>
        private static string GetProfileSetting(string Name, string DefValue)
        {
            string s = Profile.GetValue(s_csDriverID, Name, "SwitchDevices", "");
            if (s == "") s = DefValue;
            return s;
        }

        /// <summary>
        /// Delete all settings io the profile for this driver ID
        /// </summary>
        private static void DeleteProfileSettings()
        {
            Profile.DeleteSubKey(s_csDriverID, "SwitchDevices");
        }

        /// <summary>
        /// Saves specific state setting to the profile a switchdevice
        /// </summary>
        protected internal static void SaveProfileSetting(string Name, bool State)
        {
            Profile.WriteValue(s_csDriverID, Name, State.ToString(), "SwitchDevices");
        }
        #endregion

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
            var P = new ASCOM.Utilities.Profile();
            P.DeviceType = "Switch";
            if (bRegister)
            {
                P.Register(s_csDriverID, s_csDriverDescription);
            }
            else
            {
                P.Unregister(s_csDriverID);
            }
            P = null;
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
            Trace.WriteLine("Registering -> {0} with ASCOM Profile", s_csDriverID);
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
            Trace.WriteLine("Unregistering -> {0} with ASCOM Profile", s_csDriverID);
            RegUnregASCOM(false);
        }
        #endregion
    }
}
