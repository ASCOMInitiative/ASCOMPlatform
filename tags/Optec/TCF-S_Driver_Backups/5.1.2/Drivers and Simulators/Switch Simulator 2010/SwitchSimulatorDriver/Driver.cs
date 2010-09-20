using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;


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
    [Guid("221DF9AE-22FD-46C9-A475-59E8EA9393BB")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Switch : ISwitch, IDisposable
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
        /// Driver information
        /// </summary>
        private const string driverInfo = "Switch Simulator Driver and collection of Switch devices";
        
        /// <summary>
        /// Driver interface version
        /// </summary>
        private const short interfaceVersion = 6;

        /// <summary>
        /// Driver version number
        /// </summary>
        private const string driverVersion = "6.0";

        /// <summary>
        /// Gets the last result.
        /// </summary>
        private const string lastResult = "False";

        /// <summary>
        /// Backing store for the private switch collection.
        /// </summary>
        private static readonly ArrayList switches = new ArrayList(numSwitches);
        
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
        /// Sets up the permenant store for saved settings
        /// </summary>
        private static readonly Profile Profile = new Profile();

        /// <summary>
        /// Sets up the permenant store for device names
        /// </summary>
        private static readonly string[] DeviceNames = { "White Lights", "Red Lights", "Telescope Power", "Camera Power", "Focuser Power", "Dew Heaters", "Dome Power", "Self Destruct" };


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
            var f = new SetupDialogForm();
            var result = f.ShowDialog();
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
        /// Gets the last result.
        /// </summary>
        /// <value>
        /// The result of the last executed action, or <see cref="String.Empty"	/>
        /// if no action has yet been executed.
        /// </value>
        public string LastResult
        {
            get { return lastResult; }
        }

        /// <summary>
        /// Yields a collection of string[] objects.
        /// </summary>
        /// <value></value>
        public ArrayList Switches
        {
            get{ return switches;}
        }

	    void ISwitch.Dispose()
	    {
	        Dispose();
	    }

        private static void Dispose()
        {
            throw new System.NotImplementedException();
        }

	    /// <summary>
        /// Flips a switch on or off
        /// </summary>
        /// <value>name of the switch</value>
        public void SetSwitch(string switchName, bool state)
	    {
	        if (switchName == null) throw new ArgumentNullException("switchName");
	        foreach (string[,] sd in switches)
            {
                if (sd[0, 0] == switchName)
                {
                    sd[0, 1] = state.ToString();
                    SaveProfileSetting(switchName, state.ToString());
                }
            }
	    }

	    public string Action(string actionName, string actionParameters)
        {
            throw new MethodNotImplementedException("Action");
        }

        public void CommandBlind(string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandString");
        }

        public string[] SupportedActions
        {
            get { throw new MethodNotImplementedException("SupportedActions");}
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }

        #endregion

        #region Switch Private Members

        /// <summary>
        /// Load the switches and makes sure there is only 8 loaded
        /// </summary>
        private static void LoadSwitchDevices()
        {
            //each new instance load the switches, make sure this doesn't repeat
            if (switches.Count != 8)
            {
                //too many or not enough found, start over
                switches.Clear();
                //load a new set of switch devices
                foreach (string device in DeviceNames)
                {
                    if (device != null) switches.Add(new string[1,2]{{device, "False"}});
                }
            }
        }

        /// <summary>
        /// Saves all settings to the profile for this driver
        /// </summary>
        private static void GetProfileSettings()
        {
             foreach (string[,] sw in switches)
             {
                 bool state = Convert.ToBoolean(GetProfileSetting(sw[0,0], "false"));

                 if (state != Convert.ToBoolean(sw[0,1]))
                {
                    sw[0, 1] = state.ToString();
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
                bool chkRegistered = Profile.IsRegistered(sCsDriverId);
                if (chkRegistered)
                {
                    //check proffile to see if the subkey is loaded 
                    ArrayList pv = Profile.Values(sCsDriverId, "Switches");
                    if (pv.Count == 8)
                    {
                        //check profile to see if key each named device exist
                        // ReSharper disable LoopCanBeConvertedToQuery
                        foreach (string device in DeviceNames)
                        // ReSharper restore LoopCanBeConvertedToQuery
                        {
                            string s = Profile.GetValue(sCsDriverId, device, "Switches");
                            if (s != "True" & s != "False")
                            {
                                //found something wrong, delete evertyhing
                                DeleteProfileSettings();
                                return false;
                            }
                        }
                        return true;
                    }
                    DeleteProfileSettings();
                    return false;
                }
                return false;
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
            foreach (string[,] sd in switches)
            {
                Profile.WriteValue(sCsDriverId, sd[0,0], sd[0,1], "Switches");
                Trace.WriteLine("Save Setting: " + sd[0, 0] + "-" + sd[0, 1]);
            }
        }

        /// <summary>
        /// Loads a specific setting from the profile
        /// </summary>
        private static string GetProfileSetting(string switchName, string defValue)
        {
            if (switchName == null) throw new ArgumentNullException("switchName");
            string s = Profile.GetValue(sCsDriverId, switchName, "Switches", "");
            if (s == "") s = defValue;
            return s;
        }

        /// <summary>
        /// Delete all settings io the profile for this driver ID
        /// </summary>
        private static void DeleteProfileSettings()
        {
            Profile.DeleteSubKey(sCsDriverId, "Switches");
        }

        /// <summary>
        /// Saves specific state setting to the profile a switchdevice
        /// </summary>
        protected internal static void SaveProfileSetting(string switchName, string state)
        {
            Profile.WriteValue(sCsDriverId, switchName, state, "Switches");
            Trace.WriteLine("Save Setting: " + switchName + "-" + state);
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
            var p = new ASCOM.Utilities.Profile {DeviceType = "Switch"};
            if (bRegister)
            {
                p.Register(sCsDriverId, sCsDriverDescription);
            }
            else
            {
                p.Unregister(sCsDriverId);
            }
            p = null;
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
