using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Collections;

namespace ASCOM.Simulator
{
    //
    // Your driver's DeviceID is ASCOM.Simulator.SafetyMonitor
    //
    // The Guid attribute sets the CLSID for ASCOM.Simulator.SafetyMonitor
    // The ClassInterface/None addribute prevents an empty interface called
    // _Conceptual from being created and used as the [default] interface
    //

    /// <summary>
    /// ASCOM SafetyMonitor Driver for a SafetyMonitor.
    /// This class is the implementation of the public ASCOM interface.
    /// </summary>
    [Guid("0EF59E5C-2715-4E91-8A5E-38FE388B4F00")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class SafetyMonitor : ISafetyMonitor, IDisposable
    {
        #region Constants

        /// <summary>
        /// Name of the Driver
        /// </summary>
        private const string name = "ASCOM.Simulator.SafetyMonitor";

        /// <summary>
        /// Description of the driver
        /// </summary>
        private const string description = "ASCOM SafetyMonitor Simulator Driver";

        /// <summary>
        /// Driver information
        /// </summary>
        private const string driverInfo = "SafetyMonitor Simulator Drivers";

        /// <summary>
        /// Driver interface version
        /// </summary>
        private const short interfaceVersion = 6;

        /// <summary>
        /// Driver version number
        /// </summary>
        private const string driverVersion = "6.0";

        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        private const string sCsDriverId = "ASCOM.Simulator.SafetyMonitor";

        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private const string sCsDriverDescription = "ASCOM Simulator SafetyMonitor Driver";

        /// <summary>
        /// Sets up the permenant store for saved settings
        /// </summary>
        private static readonly Profile Profile = new Profile();

        private static bool _isSafe;

        #endregion

        #region ISafetyMonitor Public Members

        //
        // PUBLIC COM INTERFACE ISafetyMonitor IMPLEMENTATION
        //

        /// <summary>
        /// Initializes a new instance of the <see cref="SafetyMonitor"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public SafetyMonitor()
        {
            //check to see if the profile is ok
            if (ValidateProfile())
            {
                if (CheckSafetyMonitorKeyValue()) 
                //load profile settings
                GetProfileSetting();
            }
            else
            {
                RegisterWithProfile();
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
            f.ShowDialog();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SafetyMonitor"/> is connected.
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
            get { throw new MethodNotImplementedException("LastResult"); }
        }

        void ISafetyMonitor.Dispose()
        {
            Dispose();
        }

        private static void Dispose()
        {
            throw new System.NotImplementedException();
        }
        
        void IDisposable.Dispose()
        {
            Dispose();
        }

        /// <summary>
        /// Return the condition of the SafetyMonitor
        /// </summary>
        /// <value>State of the Monitor</value>
        public bool IsSafe 
        {
            get { return _isSafe; }
        }

        /// <summary>
        /// Invokes the specified device-specific action.
        /// </summary>
        public string Action(string actionName, string actionParameters)
        {
            throw new MethodNotImplementedException("Action");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does 
        /// not wait for a response. Optionally, protocol framing 
        /// characters may be added to the string before transmission.
        /// </summary>
        public void CommandBlind(string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandBlind");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits 
        /// for a boolean response. Optionally, protocol framing 
        /// characters may be added to the string before transmission.
        /// </summary>
        public bool CommandBool(string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandBool");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits 
        /// for a string response. Optionally, protocol framing 
        /// characters may be added to the string before transmission.
        /// </summary>
        public string CommandString(string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandString");
        }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        public ArrayList SupportedActions
        {
            // no supported actions, return empty array
            get { ArrayList sa = new ArrayList(); return sa; }
        }

        #endregion

        #region SafetyMonitor Private Members

        /// <summary>
        /// Validate the profile is in good shape
        /// </summary>
        private static bool ValidateProfile()
        {
            try
            {
                Profile.DeviceType = "SafetyMonitor";
                //check profile if the driver id is registered
                return Profile.IsRegistered(sCsDriverId);
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Validates the profile key and the value
        /// </summary>
        private static bool CheckSafetyMonitorKeyValue()
        {
            string s = Profile.GetValue(sCsDriverId, "SafetyMonitor").ToUpper();
            if (s != "TRUE" & s != "FALSE")
            {
                //found something wrong, delete evertyhing
                DeleteProfileSettings();
                return RegisterWithProfile();
            }
            return true;
        }

        /// <summary>
        /// Registers the driver with the profile
        /// </summary>
        private static bool RegisterWithProfile()
        {
            Profile.Register(sCsDriverId, sCsDriverId);
            if (ValidateProfile())
            {
                Profile.WriteValue(sCsDriverId, "SafetyMonitor", "false");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Loads a specific setting from the profile
        /// </summary>
        private static void GetProfileSetting()
        {
            string s = Profile.GetValue(sCsDriverId, "SafetyMonitor");
            _isSafe = Convert.ToBoolean(s);
        }

        /// <summary>
        /// Delete all settings io the profile for this driver ID
        /// </summary>
        private static void DeleteProfileSettings()
        {
            Profile.DeleteSubKey(sCsDriverId, "SafetyMonitor");
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
            var p = new Profile { DeviceType = "SafetyMonitor" };
            if (bRegister)
            {
                p.Register(sCsDriverId, sCsDriverDescription);
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
