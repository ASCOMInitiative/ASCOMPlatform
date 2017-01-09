using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.Globalization;

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
    public class Focuser : IFocuserV2,IDisposable
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
        private const short interfaceVersion = 2;

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
         
        internal TraceLogger TL ;// Shared tracelogger between this instances classes

        #region local parameters

        private bool _isConnected;
        private System.Timers.Timer _moveTimer; // drives the position and temperature changers
        private int _position;
        internal int Target;
        private double _lastTemp;
        private FocuserHandboxForm Handbox;
        private  DateTime lastTempUpdate;
        private Random RandomGenerator;
        internal double stepSize;
        internal bool tempComp;
        #endregion

        #region Constructor and dispose

        /// <summary>
        /// Initializes a new instance of the <see cref="Focuser"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Focuser()
        {
            try
            {
                TL = new TraceLogger("","FocusSimulator");
                TL.Enabled = RegistryCommonCode.GetBool(GlobalConstants.SIMULATOR_TRACE, GlobalConstants.SIMULATOR_TRACE_DEFAULT);
                TL.LogMessage("New", "Started");

                //check to see if the profile is ok
                if (ValidateProfile())
                {
                    TL.LogMessage("New", "Validated OK");
                    KeepMoving = false;
                    LastOffset = 0;
                    RateOfChange = 1;
                    MouseDownTime = DateTime.MaxValue; //Initialise to "don't accelerate" value
                    RandomGenerator=new Random(); //Temperature fluctuation random generator
                    LoadFocuserKeyValues();
                    TL.LogMessage("New", "Loaded Key Values");
                    Handbox = new FocuserHandboxForm(this);
                    Handbox.Hide();
                    TL.LogMessage("FocusSettingsForm", "Created Handbox");

                    // start a timer that monitors and moves the focuser
                    _moveTimer = new System.Timers.Timer();
                    _moveTimer.Elapsed += new System.Timers.ElapsedEventHandler(MoveTimer_Tick);
                    _moveTimer.Interval = 100;
                    _moveTimer.Enabled = true;
                    _lastTemp = Temperature;
                    Target = _position;

                    TL.LogMessage("New", "Started Simulation");
                }
                else
                {
                    TL.LogMessage("New", "Registering Profile");
                    RegisterWithProfile();
                    TL.LogMessage("New", "Registered OK");
                }

                TL.LogMessage("New", "Completed");
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("New Exception",ex.ToString());
                MessageBox.Show(@"Focuser: " + ex);
            }
        }

        public void Dispose()
        {
            try { Handbox.Close(); } catch { }
            try { Handbox.Dispose(); } catch { }
            try { _moveTimer.Stop(); } catch { }
            try { _moveTimer.Close(); } catch { }
            try { _moveTimer.Dispose(); } catch { }
            try { TL.Enabled=false; } catch { }
            try { TL.Dispose(); } catch { }
        }

        #endregion

        #region Private Properties

        internal bool CanHalt { get; set; }
        internal bool TempProbe { get; set; }
        internal bool Synchronous { get; set; }
        internal bool CanStepSize { get; set; }
        internal bool KeepMoving { get; set; }
        internal int LastOffset { get; set; }
        internal double TempMax { get; set; }
        internal double TempMin { get; set; }
        internal double TempPeriod { get; set; }
        internal int TempSteps { get; set; }
        internal int RateOfChange { get; set; }
        internal DateTime MouseDownTime { get; set; }

        #endregion

        #region IFocuserV2 Members

        /// <summary>
        /// True if the focuser is capable of absolute position; 
        /// that is, being commanded to a specific step location.
        /// </summary>
        public bool Absolute { get; set; }

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

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Focuser"/> is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Connected
        {
            get { return _isConnected; }
            set
            {
                if (_isConnected == value)
                    return;
                if (value)
                {
                    if (_moveTimer == null)
                        _moveTimer = new System.Timers.Timer();
                    _moveTimer.Elapsed += new System.Timers.ElapsedEventHandler(MoveTimer_Tick);
                    _moveTimer.Interval = 100;
                    _moveTimer.Enabled = true;
                    Handbox.Show();
                }
                else
                {
                    _moveTimer.Enabled = false;
                    _moveTimer.Elapsed -= MoveTimer_Tick;
                    Handbox.Hide();
                }
                _isConnected = value;
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
            get { return driverVersion; }
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
            if (!CanHalt)
                throw new MethodNotImplementedException("Halt");

            CheckConnected("Halt");
            if (Absolute)
            {
                Target = _position;
            }
            else
            {
                _position = 0;
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
        public bool IsMoving
        {
            get { return (_position != Target); }
        }

        /// <summary>
        /// State of the connection to the focuser. et True to start the link to the focuser; 
        /// set False to terminate the link. The current link status can also be read 
        /// back as this property. An exception will be raised if the link fails to 
        /// change state for any reason.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Link
        {
            get { return Connected; }
            set { Connected = value; }
        }

        /// <summary>
        /// Maximum increment size allowed by the focuser; i.e. the maximum number 
        /// of steps allowed in one move operation. For most focusers this is the 
        /// same as the MaxStep property. This is normally used to limit the 
        /// Increment display in the host software.
        /// </summary>
        public int MaxIncrement { get; internal set; }

        /// <summary>
        /// Maximum step position permitted. The focuser can step between 0 and MaxStep. 
        /// If an attempt is made to move the focuser beyond these limits, 
        /// it will automatically stop at the limit.
        /// </summary>
        public int MaxStep { get; internal set; }

        /// <summary>
        /// Step size (microns) for the focuser. Raises an exception if 
        /// the focuser does not intrinsically know what the step size is.
        /// </summary>
        public void Move(int value)
        {
            CheckConnected("Move");
            if (tempComp)
                throw new InvalidOperationException("Move not allowed when temperature compensation is active");
            if (Absolute)
            {
                TL.LogMessage("Move Absolute", value.ToString());
                Target = Truncate(0, value, MaxStep);
                RateOfChange = 40;
            }
            else
            {
                TL.LogMessage("Move Relative", value.ToString());
                Target = 0;
                _position = Truncate(-MaxStep, value, MaxStep);
                RateOfChange = 40;
            }
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
        /// Current focuser position, in steps. Valid only for absolute positioning 
        /// focusers (see the Absolute property). An exception will be raised for 
        /// relative positioning focusers.
        /// </summary>
        public int Position
        {
            get
            {
                return _position;
            }
        }

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// </summary>
        public void SetupDialog()
        {
            TL.LogMessage("SetupDialog", "Started");
            var f = new FocuserSettingsForm(this);
            TL.LogMessage("SetupDialog", "Created HandboxForm");
            f.ShowDialog();
            TL.LogMessage("SetupDialog", "Finshed");
        }

        /// <summary>
        /// Step size (microns) for the focuser. Raises an exception if the focuser 
        /// does not intrinsically know what the step size is.
        /// </summary>
        public double StepSize
        {
            get
            {
                if (CanStepSize)
                {
                    return stepSize;
                }
                throw new PropertyNotImplementedException("Property StepSize is not implemented");
            }
            internal set
            {
                stepSize = value;
            }
        }

        //public double StepSize { get; internal set; }

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
        public bool TempComp 
        {
            get { return tempComp; }
            set
            {
                if (!TempCompAvailable)
                    throw new PropertyNotImplementedException("TempComp");
                tempComp = value;
            }
        }

        /// <summary>
        /// True if focuser has temperature compensation available. Will be True 
        /// only if the focuser's temperature compensation can be turned on and 
        /// off via the TempComp property.
        /// </summary>
        public bool TempCompAvailable { get; internal set; }

        /// <summary>
        /// Current ambient temperature as measured by the focuser. Raises an 
        /// exception if ambient temperature is not available. Commonly 
        /// available on focusers with a built-in temperature compensation 
        /// mode.
        /// </summary>
        public double Temperature { get; internal set; }

        #endregion

        #region Private Members

        /// <summary>
        /// Ticks 10 times a second, updating the focuser position and IsMoving properties
        /// </summary>
        private void MoveTimer_Tick(object source, System.Timers.ElapsedEventArgs e)
        {
            //Create random temperature change
            if (DateTime.Now.Subtract(lastTempUpdate).TotalSeconds > TempPeriod)
            {
                lastTempUpdate = DateTime.Now;
                // apply a random change to the temperature
                double tempOffset = (RandomGenerator.NextDouble() - 0.5);// / 10.0;
                Temperature = Math.Round(Temperature + tempOffset, 2);

                // move the focuser target to track the temperature if required
                if (tempComp)
                {
                    var dt = (int)((Temperature - _lastTemp) * TempSteps);
                    if (dt != 0)// return;
                    {
                        Target += dt;
                        _lastTemp = Temperature;
                    }
                }
            }
            if (Target > MaxStep) Target = MaxStep; // Condition target within the acceptable range
            if (Target < 0) Target = 0;

            if (_position != Target) //Actually move the focuse if necessary
            {
                 TL.LogMessage("Moving", "LastOffset, Position, Target RateOfChange " + LastOffset + " " + _position + " " + Target + " " + RateOfChange);

                 if (Math.Abs(_position - Target) <= RateOfChange)
                 {
                     _position = Target;
                     TL.LogMessage("Moving", "  Set position = target");

                 }
                 else
                 {
                     _position += (_position > Target) ? -RateOfChange : RateOfChange;
                     TL.LogMessage("Moving", "  Updated position = " + _position);
                 }
                 TL.LogMessage("Moving", "  New position = " + _position);
            }
            if (KeepMoving & (DateTime.Now.Subtract(MouseDownTime).TotalSeconds > 0.5))
            {
                Target = (Math.Sign(LastOffset) > 0 ) ?  MaxStep : 0;
                MouseDownTime = DateTime.Now;
                if (RateOfChange < 100)
                {
                    RateOfChange = (int) Math.Ceiling((double) RateOfChange  * 1.2);
                }
                TL.LogMessage("KeepMoving", "LastOffset, Position, Target, RateOfChange MouseDownTime " + LastOffset + " " + _position + " " + Target + " " + RateOfChange + " " + MouseDownTime.ToLongTimeString());
            }
        }

        private void CheckConnected(string property)
        {
            if (!_isConnected)
                throw new NotConnectedException(property);
        }

        /// <summary>
        /// Truncate val to be between min and max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="val"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static int Truncate(int min, int val, int max)
        {
            return Math.Max(Math.Min(max, val), min);
        }

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
            catch (DirectoryNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Load the profile values
        /// </summary>
        private void LoadFocuserKeyValues()
        {
            Absolute = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "Absolute", string.Empty, "true"), CultureInfo.InvariantCulture);
            MaxIncrement = Convert.ToInt32(Profile.GetValue(sCsDriverId, "MaxIncrement", string.Empty, "50000"), CultureInfo.InvariantCulture);
            MaxStep = Convert.ToInt32(Profile.GetValue(sCsDriverId, "MaxStep", string.Empty, "50000"), CultureInfo.InvariantCulture);
            _position = Convert.ToInt32(Profile.GetValue(sCsDriverId, "Position", string.Empty, "25000"), CultureInfo.InvariantCulture);
            stepSize = Convert.ToDouble(Profile.GetValue(sCsDriverId, "StepSize", string.Empty, "20"), CultureInfo.InvariantCulture);
            tempComp = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "TempComp", string.Empty, "false"), CultureInfo.InvariantCulture);
            TempCompAvailable = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "TempCompAvailable", string.Empty, "true"), CultureInfo.InvariantCulture);
            Temperature = Convert.ToDouble(Profile.GetValue(sCsDriverId, "Temperature", string.Empty, "5"), CultureInfo.InvariantCulture);
            //extended focuser items
            CanHalt = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "CanHalt", string.Empty, "true"), CultureInfo.InvariantCulture);
            CanStepSize = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "CanStepSize", string.Empty, "true"), CultureInfo.InvariantCulture); 
            Synchronous = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "Synchronous", string.Empty, "true"), CultureInfo.InvariantCulture);
            TempMax = Convert.ToDouble(Profile.GetValue(sCsDriverId, "TempMax", string.Empty, "50"), CultureInfo.InvariantCulture);
            TempMin = Convert.ToDouble(Profile.GetValue(sCsDriverId, "TempMin", string.Empty, "-50"), CultureInfo.InvariantCulture);
            TempPeriod = Convert.ToDouble(Profile.GetValue(sCsDriverId, "TempPeriod", string.Empty, "3"), CultureInfo.InvariantCulture);
            TempProbe = Convert.ToBoolean(Profile.GetValue(sCsDriverId, "TempProbe", string.Empty, "true"), CultureInfo.InvariantCulture);
            TempSteps = Convert.ToInt32(Profile.GetValue(sCsDriverId, "TempSteps", string.Empty, "10"), CultureInfo.InvariantCulture);
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
        /// Save profile values
        /// </summary>
        public void SaveProfileSettings()
        {
            if (Temperature > TempMax) Temperature = TempMax;
            if (Temperature < TempMin) Temperature = TempMin;
            if (_position > MaxStep) _position = MaxStep;

            //ascom items
            Profile.WriteValue(sCsDriverId, "Absolute", Absolute.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "MaxIncrement", MaxIncrement.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "MaxStep", MaxStep.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "Position", _position.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "StepSize", stepSize.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "TempComp", tempComp.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "TempCompAvailable", TempCompAvailable.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "Temperature", Temperature.ToString(CultureInfo.InvariantCulture));
            //extended focuser items
            Profile.WriteValue(sCsDriverId, "CanHalt", CanHalt.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "CanStepSize", CanStepSize.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "Synchronous", Synchronous.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "TempMax", TempMax.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "TempMin", TempMin.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "TempPeriod", TempPeriod.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "TempProbe", TempProbe.ToString(CultureInfo.InvariantCulture));
            Profile.WriteValue(sCsDriverId, "TempSteps", TempSteps.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Saves specific state setting to the profile a switchdevice
        /// </summary>
        public static void SaveProfileSetting(string keyName, string value)
        {
            Profile.WriteValue(sCsDriverId, keyName, value);
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
            using (var p = new Profile())
            {
                p.DeviceType = "Focuser";
                if (bRegister)
                {
                    p.Register(sCsDriverId, sCsDriverDescription);
                }
                else
                {
                    p.Unregister(sCsDriverId);
                }
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
            RegUnregASCOM(false);
        }

        #endregion
    }
}