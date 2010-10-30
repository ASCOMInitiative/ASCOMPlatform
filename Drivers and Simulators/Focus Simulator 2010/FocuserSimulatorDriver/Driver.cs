using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Collections;

namespace ASCOM.Simulator
{
    //
    // Your driver's DeviceID is ASCOM.Simulator.Focuser
    //
    // The Guid attribute sets the CLSID for ASCOM.Simulator.Focuser
    // The ClassInterface/None addribute prevents an empty interface called
    // _Conceptual from being created and used as the [default] interface
    //

    /// <summary>
    /// ASCOM Focuser Driver for a Focuser.
    /// This class is the implementation of the public ASCOM interface.
    /// </summary>
    [Guid("24C040F2-2FA5-4DA4-B87B-6C1101828D2A")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Focuser : IFocuser, IDisposable
    {
        #region Constants

        /// <summary>
        /// Name of the Driver
        /// </summary>
        private const string name = "ASCOM.Simulator.Focuser";

        /// <summary>
        /// Description of the driver
        /// </summary>
        private const string description = "ASCOM Focuser Simulator Driver";

        /// <summary>
        /// Driver information
        /// </summary>
        private const string driverInfo = "Focuser Simulator Driver";

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
        private const string sCsDriverId = "ASCOM.Simulator.Focuser";

        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private const string sCsDriverDescription = "ASCOM Simulator Focuser Driver";

        /// <summary>
        /// Sets up the permenant store for saved settings
        /// </summary>
        private static readonly Profile Profile = new Profile();

        #endregion

        #region Public Focuser ASCOM Members

        /// <summary>
        /// Initializes a new instance of the <see cref="Focuser"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Focuser()
        {
            //check to see if the profile is ok
            if (ValidateProfile())
            {
                if (LoadFocuserKeyValues())
                {
                    if (!LoadStaticFocuserKeyValues())
                    {
                        DeleteProfileSettings();
                        SetDefaultProfileSettings();
                        Connected = false;
                        Link = false;
                    }
                }
                else
                {
                    DeleteProfileSettings();
                    SetDefaultProfileSettings();
                    Connected = false;
                    Link = false;
                }    

            }
            else
            {
                RegisterWithProfile();
            }
        }

        /// <summary>
        /// True if the focuser is capable of absolute position; 
        /// that is, being commanded to a specific step location.
        /// </summary>
        public bool Absolute { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Focuser"/> is connected.
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
        /// True if the focuser is currently moving to a new position. False if the focuser is stationary.
        /// </summary>
        /// <value><c>true</c> if moving; otherwise, <c>false</c>.</value>
        public bool IsMoving { get; set; }

        /// <summary>
        /// State of the connection to the focuser. et True to start the link to the focuser; 
        /// set False to terminate the link. The current link status can also be read 
        /// back as this property. An exception will be raised if the link fails to 
        /// change state for any reason.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Link { get; set; }

        /// <summary>
        /// Maximum increment size allowed by the focuser; i.e. the maximum number 
        /// of steps allowed in one move operation. For most focusers this is the 
        /// same as the MaxStep property. This is normally used to limit the 
        /// Increment display in the host software.
        /// </summary>
        public int MaxIncrement { get; set; }

        /// <summary>
        /// Maximum step position permitted. The focuser can step between 0 and MaxStep. 
        /// If an attempt is made to move the focuser beyond these limits, 
        /// it will automatically stop at the limit.
        /// </summary>
        public int MaxStep { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Current focuser position, in steps. Valid only for absolute positioning 
        /// focusers (see the Absolute property). An exception will be raised for 
        /// relative positioning focusers.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Step size (microns) for the focuser. Raises an exception if the focuser 
        /// does not intrinsically know what the step size is.
        /// </summary>
        public double StepSize { get; set; }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        public ArrayList SupportedActions
        {
            // no supported actions, return empty array
            get { ArrayList sa = new ArrayList(); return sa; }
            //get { throw new MethodNotImplementedException("SupportedActions"); }
        }

        /// <summary>
        /// The state of temperature compensation mode (if available), else always 
        /// False. If the TempCompAvailable property is True, then setting TempComp 
        /// to True puts the focuser into temperature tracking mode. While in 
        /// temperature tracking mode, Move commands will be rejected by the 
        /// focuser. Set to False to turn off temperature tracking. An exception 
        /// will be raised if TempCompAvailable is False and an attempt is made 
        /// to set TempComp to true.
        /// </summary>
        public bool TempComp { get; set; }

        /// <summary>
        /// True if focuser has temperature compensation available. Will be True 
        /// only if the focuser's temperature compensation can be turned on and 
        /// off via the TempComp property.
        /// </summary>
        public bool TempCompAvailable { get; set; }

        /// <summary>
        /// Current ambient temperature as measured by the focuser. Raises an 
        /// exception if ambient temperature is not available. Commonly 
        /// available on focusers with a built-in temperature compensation 
        /// mode.
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it 
        /// via COM if it is a COM object, else if native .NET will just 
        /// dereference it for GC
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private static void Dispose()
        {
            throw new System.NotImplementedException();
        }
        
        /// <summary>
        /// Immediately stop any focuser motion due to a previous Move() 
        /// method call. Some focusers may not support this function, in 
        /// which case an exception will be raised. Recommendation: Host 
        /// software should call this method upon initialization and, if 
        /// it fails, disable the Halt button in the user interface.
        /// </summary>
        public void Halt()
        {
            if (!CanHalt) return;
            IsMoving = false;
            Connected = false;
        }

        /// <summary>
        /// Step size (microns) for the focuser. Raises an exception if 
        /// the focuser does not intrinsically know what the step size is.
        /// </summary>
        public void Move(int val)
        {
            Connected = true;
            Link = true;
            IsMoving = true;
            Position = Position + val;

            if (Position >= MaxStep)
            {
                Position = MaxStep;
            }
            if (Position + val <= 0)
            {
                Position = 0;
            }
            IsMoving = false;
            Connected = false;
            Link = false;


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
        /// Invokes the specified device-specific action.
        /// </summary>
        /// <exception cref="MethodNotImplementedException"></exception>
        public string Action(string actionName, string actionParameters)
        {
            throw new MethodNotImplementedException("Action");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not 
        /// wait for a response. Optionally, protocol framing characters 
        /// may be added to the string before transmission.
        /// mode.
        /// </summary>
        /// <exception cref="MethodNotImplementedException"></exception>
        public void CommandBlind(string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandBlind");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits 
        /// for a boolean response. Optionally, protocol framing 
        /// characters may be added to the string before transmission.
        /// </summary>
        /// <exception cref="MethodNotImplementedException"></exception>
        public bool CommandBool(string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandBool");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits 
        /// for a string response. Optionally, protocol framing 
        /// characters may be added to the string before transmission.
        /// </summary>
        /// <exception cref="MethodNotImplementedException"></exception>
        public string CommandString(string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandString");
        }

        #endregion

        #region explicit Members

        void IFocuser.Dispose()
        {
            Dispose();
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }

        #endregion

        #region Focuser Private Members

        /// <summary>
        /// Validate the profile is in good shape
        /// </summary>
        private static bool ValidateProfile()
        {
            try
            {
                Profile.DeviceType = "Focuser";
                //check profile if the driver id is registered
                return Profile.IsRegistered(sCsDriverId);
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Load the profile values
        /// </summary>
        private bool LoadFocuserKeyValues()
        {
            try
            {
                Absolute = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "Absolute"));
                MaxIncrement = Convert.ToInt32(Profile.GetValue(sCsDriverId, "MaxIncrement"));
                MaxStep = Convert.ToInt32(Profile.GetValue(sCsDriverId, "MaxStep"));
                Position = Convert.ToInt32(Profile.GetValue(sCsDriverId, "Position"));
                StepSize = Convert.ToDouble(Profile.GetValue(sCsDriverId, "StepSize"));
                Temperature = Convert.ToDouble(Profile.GetValue(sCsDriverId, "Temperature"));
                TempComp = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "TempComp"));
                TempCompAvailable = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "TempCompAvailable"));

                return true;

            }
            catch (Exception)
            {
                return false;
            }  
        }

        /// <summary>
        /// Load the profile values
        /// </summary>
        private bool LoadStaticFocuserKeyValues()
        {
            try
            {
                //extended focuser items
                CanHalt = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "CanHalt"));
                TempProbe = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "TempProbe"));
                Synchronus = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "Synchronus"));
                CanStepSize = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "CanStepSize"));
                TempMax = Convert.ToInt32(Profile.GetValue(sCsDriverId, "TempMax"));
                TempMin = Convert.ToInt32(Profile.GetValue(sCsDriverId, "TempMin"));
                TempPeriod = Convert.ToInt32(Profile.GetValue(sCsDriverId, "TempPeriod"));
                TempSteps = Convert.ToInt32(Profile.GetValue(sCsDriverId, "TempSteps"));

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Register the driver to setup a Profile
        /// </summary>
        private static void RegisterWithProfile()
        {
            Profile.Register(sCsDriverId, sCsDriverId);
            if (ValidateProfile())
            {
                Profile.WriteValue(sCsDriverId, "Focuser", "false");
                return;
            }
            return;
        }

        /// <summary>
        /// Delete all settings io the profile for this driver ID
        /// </summary>
        private static void DeleteProfileSettings()
        {
            Profile.DeleteSubKey(sCsDriverId, "Focuser");
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
            var p = new Profile { DeviceType = "Focuser" };
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

        #region Public Focuser Extended Members
        //used to extend the functionallity beyound the ascom interfaces

        public bool CanHalt { get; set; }
        public bool TempProbe { get; set; }
        public bool Synchronus { get; set; }
        public bool CanStepSize { get; set; }
        public int TempMax { get; set; }
        public int TempMin { get; set; }
        public int TempPeriod { get; set; }
        public int TempSteps { get; set; }

        /// <summary>
        /// Save profile values
        /// </summary>
        public void SaveProfileSettings()
        {

            if (Temperature > TempMax)
            {Temperature = TempMax;}
            if (Temperature < TempMin)
            { Temperature = TempMin; }

            if (Position > MaxStep)
            { Position = MaxStep;}

            //ascom items
            Profile.WriteValue(sCsDriverId,"MaxStep", MaxStep.ToString()) ;
            Profile.WriteValue(sCsDriverId, "StepSize", StepSize.ToString());
            Profile.WriteValue(sCsDriverId, "Absolute", Absolute.ToString());
            Profile.WriteValue(sCsDriverId, "MaxIncrement", MaxIncrement.ToString());
            Profile.WriteValue(sCsDriverId, "Position", Position.ToString());
            Profile.WriteValue(sCsDriverId, "Temperature", Temperature.ToString());
            Profile.WriteValue(sCsDriverId, "TempComp", TempComp.ToString());
            Profile.WriteValue(sCsDriverId, "TempCompAvailable", TempCompAvailable.ToString());
        }

        /// <summary>
        /// Save profile values
        /// </summary>
        public void SaveStaticProfileSettings()
        {

            if (TempMin > TempMax)
            { TempMin = TempMax; }
            if (TempMax < TempMin)
            { TempMax = TempMin; }

            //extended items
            Profile.WriteValue(sCsDriverId, "CanHalt", CanHalt.ToString());
            Profile.WriteValue(sCsDriverId, "TempProbe", TempProbe.ToString());
            Profile.WriteValue(sCsDriverId, "Synchronus", Synchronus.ToString());
            Profile.WriteValue(sCsDriverId, "CanStepSize", CanStepSize.ToString());
            Profile.WriteValue(sCsDriverId, "TempMax", TempMax.ToString());
            Profile.WriteValue(sCsDriverId, "TempMin", TempMin.ToString());
            Profile.WriteValue(sCsDriverId, "TempPeriod", TempPeriod.ToString());
            Profile.WriteValue(sCsDriverId, "TempSteps", TempSteps.ToString());
        }

        /// <summary>
        /// Saves specific state setting to the profile a switchdevice
        /// </summary>
        public void SaveProfileSetting(string keyName, string value)
        {
            Profile.WriteValue(sCsDriverId, keyName, value);
        }

        /// <summary>
        /// Default values for the profile
        /// </summary>
        public void SetDefaultProfileSettings()
        {
            Profile.WriteValue(sCsDriverId, "Absolute", "True");
            Profile.WriteValue(sCsDriverId, "MaxIncrement", "50000");
            Profile.WriteValue(sCsDriverId, "MaxStep", "50000");
            Profile.WriteValue(sCsDriverId, "Position", "25000");
            Profile.WriteValue(sCsDriverId, "StepSize", "20");
            Profile.WriteValue(sCsDriverId, "Temperature", "4.96");
            Profile.WriteValue(sCsDriverId, "TempComp", "False");
            Profile.WriteValue(sCsDriverId, "TempCompAvailable", "True");
            Profile.WriteValue(sCsDriverId, "CanHalt", "True");
            Profile.WriteValue(sCsDriverId, "TempProbe", "True");
            Profile.WriteValue(sCsDriverId, "Synchronus", "True");
            Profile.WriteValue(sCsDriverId, "CanStepSize", "True");
            Profile.WriteValue(sCsDriverId, "TempMax", "5");
            Profile.WriteValue(sCsDriverId, "TempMin", "-5");
            Profile.WriteValue(sCsDriverId, "TempPeriod", "3");
            Profile.WriteValue(sCsDriverId, "TempSteps", "10");

        }

        #endregion
    }
}
