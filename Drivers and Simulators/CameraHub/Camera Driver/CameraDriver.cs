using ASCOM.DeviceInterface;
using ASCOM.LocalServer;
using ASCOM.Utilities;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ASCOM.CameraHub
{
    /// <summary>
    /// ASCOM Driver for the Camera Hub.
    /// </summary>
    [ComVisible(true)]
    [Guid("c60093ec-65a2-4604-91df-998e208d6b14")]
    [ProgId("ASCOM.CameraHub.Camera")]
    [ServedClassName("ASCOM Camera Hub Camera")] // Driver description that appears in the Chooser, customise as required
    [ClassInterface(ClassInterfaceType.None)]
    public class Camera : ReferenceCountedObjectBase, ICameraV4, IDisposable
    {
        internal static string cameraDescription; // Chooser descriptive text. The value is retrieved from the ServedClassName attribute in the class initialiser.

        private Guid uniqueId; // A unique ID for this instance of the driver

        internal TraceLogger tl; // Trace logger object to hold diagnostic information just for this instance of the driver, as opposed to the local server's log, which includes activity from all driver instances.
        private bool disposedValue;

        #region Initialisation and Dispose

        static Camera()
        {
            // Pull the ProgID from the ProgID class attribute.
            Attribute attr = Attribute.GetCustomAttribute(typeof(Camera), typeof(ProgIdAttribute));
            CameraProgId = ((ProgIdAttribute)attr).Value ?? "PROGID NOT SET!";  // Get the driver ProgIDfrom the ProgID attribute.
                                                                                // Pull the display name from the ServedClassName class attribute.
            attr = Attribute.GetCustomAttribute(typeof(Camera), typeof(ServedClassNameAttribute));
            cameraDescription = ((ServedClassNameAttribute)attr).DisplayName ?? "DISPLAY NAME NOT SET!";  // Get the driver description that displays in the ASCOM Chooser from the ServedClassName attribute.

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ASCOM.Camera"/> class. Must be public to successfully register for COM.
        /// </summary>
        public Camera()
        {
            try
            {

                // LOGGING CONFIGURATION
                // By default all driver logging will appear in Hardware log file
                // If you would like each instance of the driver to have its own log file as well, uncomment the lines below

                tl = new TraceLogger("", "CameraHub.Camera.Driver"); // Remove the leading ASCOM. from the ProgId because this will be added back by TraceLogger.
                SetTraceState();

                // Initialise the hardware if required
                CameraHardware.InitialiseCamera();

                LogMessage("Camera", "Starting driver initialisation");
                LogMessage("Camera", $"ProgID: {CameraProgId}, Description: {cameraDescription}");

                // Create a unique ID to identify this driver instance
                uniqueId = Guid.NewGuid();

                LogMessage("Camera", "Completed initialisation");
            }
            catch (Exception ex)
            {
                LogMessage("Camera", $"Initialisation exception: {ex}");
                MessageBox.Show($"{ex.Message}", "Exception creating ASCOM.CameraHub.Camera", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Class destructor called automatically by the .NET runtime when the object is finalised in order to release resources that are NOT managed by the .NET runtime.
        /// </summary>
        /// <remarks>See the Dispose(bool disposing) remarks for further information.</remarks>
        ~Camera()
        {
            // Please do not change this code.
            // The Dispose(false) method is called here just to release unmanaged resources. Managed resources will be dealt with automatically by the .NET runtime.

            Dispose(false);
        }

        /// <summary>
        /// Deterministically dispose of any managed and unmanaged resources used in this instance of the driver.
        /// </summary>
        /// <remarks>
        /// Do not dispose of items in this method, put clean-up code in the 'Dispose(bool disposing)' method instead.
        /// </remarks>
        public void Dispose()
        {
            // Please do not change the code in this method.

            // Release resources now.
            Dispose(disposing: true);

            // Do not add GC.SuppressFinalize(this); here because it breaks the ReferenceCountedObjectBase COM connection counting mechanic
        }

        /// <summary>
        /// Dispose of large or scarce resources created or used within this driver file
        /// </summary>
        /// <remarks>
        /// The purpose of this method is to enable you to release finite system resources back to the operating system as soon as possible, so that other applications work as effectively as possible.
        ///
        /// NOTES
        /// 1) Do not call the CameraHardware.Dispose() method from this method. Any resources used in the static CameraHardware class itself, 
        ///    which is shared between all instances of the driver, should be released in the CameraHardware.Dispose() method as usual. 
        ///    The CameraHardware.Dispose() method will be called automatically by the local server just before it shuts down.
        /// 2) You do not need to release every .NET resource you use in your driver because the .NET runtime is very effective at reclaiming these resources. 
        /// 3) Strong candidates for release here are:
        ///     a) Objects that have a large memory footprint (> 1Mb) such as images
        ///     b) Objects that consume finite OS resources such as file handles, synchronisation object handles, memory allocations requested directly from the operating system (NativeMemory methods) etc.
        /// 4) Please ensure that you do not return exceptions from this method
        /// 5) Be aware that Dispose() can be called more than once:
        ///     a) By the client application
        ///     b) Automatically, by the .NET runtime during finalisation
        /// 6) Because of 5) above, you should make sure that your code is tolerant of multiple calls.    
        /// </remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        // Dispose of managed objects here

                        // Clean up the trace logger object
                        if (!(tl is null))
                        {
                            tl.Enabled = false;
                            tl.Dispose();
                            tl = null;
                        }
                    }
                    catch (Exception)
                    {
                        // Any exception is not re-thrown because Microsoft's best practice says not to return exceptions from the Dispose method. 
                    }
                }

                try
                {
                    // Dispose of unmanaged objects, if any, here (OS handles etc.)
                }
                catch (Exception)
                {
                    // Any exception is not re-thrown because Microsoft's best practice says not to return exceptions from the Dispose method. 
                }

                // Flag that Dispose() has already run and disposed of all resources
                disposedValue = true;
            }
        }

        #endregion

        // PUBLIC COM INTERFACE ICameraV3 IMPLEMENTATION

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialogue form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            Server.SetupDialog();
        }

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        public ArrayList SupportedActions
        {
            get
            {
                try
                {
                    CheckConnected($"SupportedActions");
                    ArrayList actions = CameraHardware.SupportedActions;
                    LogMessage("SupportedActions", $"Returning {actions.Count} actions.");
                    return actions;
                }
                catch (Exception ex)
                {
                    LogMessage("SupportedActions", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>Invokes the specified device-specific custom action.</summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.</param>
        /// <returns>A string response. The meaning of returned strings is set by the driver author.
        /// <para>Suppose filter wheels start to appear with automatic wheel changers; new actions could be <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
        /// of wheel names and the second taking a wheel name and making the change, returning appropriate values to indicate success or failure.</para>
        /// </returns>
        public string Action(string actionName, string actionParameters)
        {
            try
            {
                CheckConnected($"Action {actionName} - {actionParameters}");
                LogMessage("", $"Calling Action: {actionName} with parameters: {actionParameters}");
                string actionResponse = CameraHardware.Action(actionName, actionParameters);
                LogMessage("Action", $"Completed.");
                return actionResponse;
            }
            catch (Exception ex)
            {
                LogMessage("Action", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not wait for a response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        public void CommandBlind(string command, bool raw)
        {
            try
            {
                CheckConnected($"CommandBlind: {command}, Raw: {raw}");
                LogMessage("CommandBlind", $"Calling method - Command: {command}, Raw: {raw}");
                CameraHardware.CommandBlind(command, raw);
                LogMessage("CommandBlind", $"Completed.");
            }
            catch (Exception ex)
            {
                LogMessage("CommandBlind", $"Command: {command}, Raw: {raw} threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a boolean response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the interpreted boolean response received from the device.
        /// </returns>
        public bool CommandBool(string command, bool raw)
        {
            try
            {
                CheckConnected($"CommandBool: {command}, Raw: {raw}");
                LogMessage("CommandBlind", $"Calling method - Command: {command}, Raw: {raw}");
                bool commandBoolResponse = CameraHardware.CommandBool(command, raw);
                LogMessage("CommandBlind", $"Returning: {commandBoolResponse}.");
                return commandBoolResponse;
            }
            catch (Exception ex)
            {
                LogMessage("CommandBool", $"Command: {command}, Raw: {raw} threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a string response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the string response received from the device.
        /// </returns>
        public string CommandString(string command, bool raw)
        {
            try
            {
                CheckConnected($"CommandString: {command}, Raw: {raw}");
                LogMessage("CommandString", $"Calling method - Command: {command}, Raw: {raw}");
                string commandStringResponse = CameraHardware.CommandString(command, raw);
                LogMessage("CommandString", $"Returning: {commandStringResponse}.");
                return commandStringResponse;
            }
            catch (Exception ex)
            {
                LogMessage("CommandString", $"Command: {command}, Raw: {raw} threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Set True to connect to the device hardware. Set False to disconnect from the device hardware.
        /// You can also read the property to check whether it is connected. This reports the current hardware state.
        /// </summary>
        /// <value><c>true</c> if connected to the hardware; otherwise, <c>false</c>.</value>
        public bool Connected
        {
            get
            {
                try
                {
                    // Return the driver's connection state which could be different to the hardware connected state, because there may be other client connections still active.
                    bool connectedState = CameraHardware.IsConnected(uniqueId);
                    LogMessage("Connected Get", connectedState.ToString());
                    return connectedState;
                }
                catch (Exception ex)
                {
                    LogMessage("Connected Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    // Handle connect and disconnect requests
                    if (value) // Request to connect
                    {
                        LogMessage("Connected Set", "Connecting to device");
                        CameraHardware.Connect(uniqueId, CameraHardware.ConnectType.Connected);
                    }
                    else // Request to disconnect
                    {
                        LogMessage("Connected Set", "Disconnecting from device");
                        CameraHardware.Disconnect(uniqueId, CameraHardware.ConnectType.Connected);
                    }
                }
                catch (Exception ex)
                {
                    LogMessage("Connected Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                try
                {
                    CheckConnected($"Description");
                    string description = CameraHardware.Description;
                    LogMessage("Description", description);
                    return description;
                }
                catch (Exception ex)
                {
                    LogMessage("Description", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        public string DriverInfo
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    string driverInfo = CameraHardware.DriverInfo;
                    LogMessage("DriverInfo", driverInfo);
                    return driverInfo;
                }
                catch (Exception ex)
                {
                    LogMessage("DriverInfo", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// A string containing only the major and minor version of the driver formatted as 'm.n'.
        /// </summary>
        public string DriverVersion
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    string driverVersion = CameraHardware.DriverVersion;
                    LogMessage("DriverVersion", driverVersion);
                    return driverVersion;
                }
                catch (Exception ex)
                {
                    LogMessage("DriverVersion", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// The interface version number that this device supports.
        /// </summary>
        public short InterfaceVersion
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    short interfaceVersion = CameraHardware.InterfaceVersion;
                    LogMessage("InterfaceVersion", interfaceVersion.ToString());
                    return interfaceVersion;
                }
                catch (Exception ex)
                {
                    LogMessage("InterfaceVersion", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        public string Name
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    string name = CameraHardware.Name;
                    LogMessage("Name Get", name);
                    return name;
                }
                catch (Exception ex)
                {
                    LogMessage("Name", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        #endregion

        #region ICamera Implementation

        /// <summary>
        /// Aborts the current exposure, if any, and returns the camera to Idle state.
        /// </summary>
        public void AbortExposure()
        {
            try
            {
                CheckConnected("AbortExposure");
                LogMessage("AbortExposure", $"Calling method.");
                CameraHardware.AbortExposure();
                LogMessage("AbortExposure", $"Completed.");
            }
            catch (Exception ex)
            {
                LogMessage("AbortExposure", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Returns the X offset of the Bayer matrix, as defined in <see cref="SensorType" />.
        /// </summary>
        /// <returns>The Bayer colour matrix X offset, as defined in <see cref="SensorType" />.</returns>
        public short BayerOffsetX
        {
            get
            {
                try
                {
                    CheckConnected("AbortExposure");
                    short bayerOffsetX = CameraHardware.BayerOffsetX;
                    LogMessage("AbortExposure", bayerOffsetX.ToString());
                    return bayerOffsetX;
                }
                catch (Exception ex)
                {
                    LogMessage("BayerOffsetX", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns the Y offset of the Bayer matrix, as defined in <see cref="SensorType" />.
        /// </summary>
        /// <returns>The Bayer colour matrix Y offset, as defined in <see cref="SensorType" />.</returns>
        public short BayerOffsetY
        {
            get
            {
                try
                {
                    CheckConnected("BayerOffsetY");
                    short bayerOffsetY = CameraHardware.BayerOffsetY;
                    LogMessage("BayerOffsetY", bayerOffsetY.ToString());
                    return bayerOffsetY;
                }
                catch (Exception ex)
                {
                    LogMessage("BayerOffsetY", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the binning factor for the X axis, also returns the current value.
        /// </summary>
        /// <value>The X binning value</value>
        public short BinX
        {
            get
            {
                try
                {
                    CheckConnected("BinX Get");
                    short binX = CameraHardware.BinX;
                    LogMessage("BinX Get", binX.ToString());
                    return binX;
                }
                catch (Exception ex)
                {
                    LogMessage("BinX Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    CheckConnected("BinX Set");
                    LogMessage("BinX Set", value.ToString());
                    CameraHardware.BinX = value;
                }
                catch (Exception ex)
                {
                    LogMessage("BinX Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the binning factor for the Y axis, also returns the current value.
        /// </summary>
        /// <value>The Y binning value.</value>
        public short BinY
        {
            get
            {
                try
                {
                    CheckConnected("BinY Get");
                    short binY = CameraHardware.BinY;
                    LogMessage("BinY Get", binY.ToString());
                    return binY;
                }
                catch (Exception ex)
                {
                    LogMessage("BinY Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    CheckConnected("BinY Set");
                    LogMessage("BinY Set", value.ToString());
                    CameraHardware.BinY = value;
                }
                catch (Exception ex)
                {
                    LogMessage("BinY Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns the current CCD temperature in degrees Celsius.
        /// </summary>
        /// <value>The CCD temperature.</value>
        public double CCDTemperature
        {
            get
            {
                try
                {
                    CheckConnected("CCDTemperature");
                    double ccdTemperature = CameraHardware.CCDTemperature;
                    LogMessage("CCDTemperature", ccdTemperature.ToString());
                    return ccdTemperature;
                }
                catch (Exception ex)
                {
                    LogMessage("CCDTemperature", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns the current camera operational state
        /// </summary>
        /// <value>The state of the camera.</value>
        public CameraStates CameraState
        {
            get
            {
                try
                {
                    CheckConnected("CameraState");
                    CameraStates cameraState = CameraHardware.CameraState;
                    LogMessage("CameraState", cameraState.ToString());
                    return cameraState;
                }
                catch (Exception ex)
                {
                    LogMessage("r", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns the width of the CCD camera chip in unbinned pixels.
        /// </summary>
        /// <value>The size of the camera X.</value>
        public int CameraXSize
        {
            get
            {
                try
                {
                    CheckConnected("CameraXSize");
                    int cameraXSize = CameraHardware.CameraXSize;
                    LogMessage("CameraXSize", cameraXSize.ToString());
                    return cameraXSize;
                }
                catch (Exception ex)
                {
                    LogMessage("CameraXSize", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns the height of the CCD camera chip in unbinned pixels.
        /// </summary>
        /// <value>The size of the camera Y.</value>
        public int CameraYSize
        {
            get
            {
                try
                {
                    CheckConnected("CameraYSize");
                    int cameraYSize = CameraHardware.CameraYSize;
                    LogMessage("CameraYSize", cameraYSize.ToString());
                    return cameraYSize;
                }
                catch (Exception ex)
                {
                    LogMessage("CameraYSize", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns <c>true</c> if the camera can abort exposures; <c>false</c> if not.
        /// </summary>
        /// <value>
        public bool CanAbortExposure
        {
            get
            {
                try
                {
                    CheckConnected("CanAbortExposure");
                    bool canAbortExposure = CameraHardware.CanAbortExposure;
                    LogMessage("CanAbortExposure", canAbortExposure.ToString());
                    return canAbortExposure;
                }
                catch (Exception ex)
                {
                    LogMessage("CanAbortExposure", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns a flag showing whether this camera supports asymmetric binning
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can asymmetric bin; otherwise, <c>false</c>.
        /// </value>
        public bool CanAsymmetricBin
        {
            get
            {
                try
                {
                    CheckConnected("CanAsymmetricBin");
                    bool canAsymmetricBin = CameraHardware.CanAsymmetricBin;
                    LogMessage("CanAsymmetricBin", canAsymmetricBin.ToString());
                    return canAsymmetricBin;
                }
                catch (Exception ex)
                {
                    LogMessage("CanAsymmetricBin", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Camera has a fast readout mode
        /// </summary>
        /// <returns><c>true</c> when the camera supports a fast readout mode</returns>
        public bool CanFastReadout
        {
            get
            {
                {
                    try
                    {
                        CheckConnected("CanFastReadout");
                        bool canFastReadout = CameraHardware.CanFastReadout;
                        LogMessage("CanFastReadout", canFastReadout.ToString());
                        return canFastReadout;
                    }
                    catch (Exception ex)
                    {
                        LogMessage("CanFastReadout", $"Threw an exception: \r\n{ex}");
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// If <c>true</c>, the camera's cooler power setting can be read.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can get cooler power; otherwise, <c>false</c>.
        /// </value>
        public bool CanGetCoolerPower
        {
            get
            {
                try
                {
                    CheckConnected("CanGetCoolerPower");
                    bool canGetCoolerPower = CameraHardware.CanGetCoolerPower;
                    LogMessage("CanGetCoolerPower", canGetCoolerPower.ToString());
                    return canGetCoolerPower;
                }
                catch (Exception ex)
                {
                    LogMessage("CanGetCoolerPower", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns a flag indicating whether this camera supports pulse guiding
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can pulse guide; otherwise, <c>false</c>.
        /// </value>
        public bool CanPulseGuide
        {
            get
            {
                try
                {
                    CheckConnected("CanPulseGuide");
                    bool canPulseGuide = CameraHardware.CanPulseGuide;
                    LogMessage("CanPulseGuide", canPulseGuide.ToString());
                    return canPulseGuide;
                }
                catch (Exception ex)
                {
                    LogMessage("CanPulseGuide", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns a flag indicating whether this camera supports setting the CCD temperature
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can set CCD temperature; otherwise, <c>false</c>.
        /// </value>
        public bool CanSetCCDTemperature
        {
            get
            {
                try
                {
                    CheckConnected("CanSetCCDTemperature");
                    bool canSetCCDTemperature = CameraHardware.CanSetCCDTemperature;
                    LogMessage("CanSetCCDTemperature", canSetCCDTemperature.ToString());
                    return canSetCCDTemperature;
                }
                catch (Exception ex)
                {
                    LogMessage("CanSetCCDTemperature", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns a flag indicating whether this camera can stop an exposure that is in progress
        /// </summary>
        /// <value>
        /// <c>true</c> if the camera can stop the exposure; otherwise, <c>false</c>.
        /// </value>
        public bool CanStopExposure
        {
            get
            {
                try
                {
                    CheckConnected("CanStopExposure");
                    bool canStopExposure = CameraHardware.CanStopExposure;
                    LogMessage("CanStopExposure", canStopExposure.ToString());
                    return canStopExposure;
                }
                catch (Exception ex)
                {
                    LogMessage("CanStopExposure", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Turns on and off the camera cooler, and returns the current on/off state.
        /// </summary>
        /// <value><c>true</c> if the cooler is on; otherwise, <c>false</c>.</value>
        public bool CoolerOn
        {
            get
            {
                try
                {
                    CheckConnected("CoolerOn Get");
                    bool coolerOn = CameraHardware.CoolerOn;
                    LogMessage("CoolerOn Get", coolerOn.ToString());
                    return coolerOn;
                }
                catch (Exception ex)
                {
                    LogMessage("CoolerOn Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    CheckConnected("CoolerOn Set");
                    LogMessage("CoolerOn Set", value.ToString());
                    CameraHardware.CoolerOn = value;
                }
                catch (Exception ex)
                {
                    LogMessage("CoolerOn Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns the present cooler power level, in percent.
        /// </summary>
        /// <value>The cooler power.</value>
        public double CoolerPower
        {
            get
            {
                try
                {
                    CheckConnected("CoolerPower");
                    double coolerPower = CameraHardware.CoolerPower;
                    LogMessage("CoolerPower", coolerPower.ToString());
                    return coolerPower;
                }
                catch (Exception ex)
                {
                    LogMessage("CoolerPower", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns the gain of the camera in photoelectrons per A/D unit.
        /// </summary>
        /// <value>The electrons per ADU.</value>
        public double ElectronsPerADU
        {
            get
            {
                try
                {
                    CheckConnected("ElectronsPerADU");
                    double electronsPerAdu = CameraHardware.ElectronsPerADU;
                    LogMessage("ElectronsPerADU", electronsPerAdu.ToString());
                    return electronsPerAdu;
                }
                catch (Exception ex)
                {
                    LogMessage("ElectronsPerADU", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns the maximum exposure time supported by <see cref="StartExposure">StartExposure</see>.
        /// </summary>
        /// <returns>The maximum exposure time, in seconds, that the camera supports</returns>
        public double ExposureMax
        {
            get
            {
                try
                {
                    CheckConnected("ExposureMax");
                    double exposureMax = CameraHardware.ExposureMax;
                    LogMessage("ExposureMax", exposureMax.ToString());
                    return exposureMax;
                }
                catch (Exception ex)
                {
                    LogMessage("ExposureMax", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Minimum exposure time
        /// </summary>
        /// <returns>The minimum exposure time, in seconds, that the camera supports through <see cref="StartExposure">StartExposure</see></returns>
        public double ExposureMin
        {
            get
            {
                try
                {
                    CheckConnected("ExposureMin");
                    double exposureMin = CameraHardware.ExposureMin;
                    LogMessage("ExposureMin", exposureMin.ToString());
                    return exposureMin;
                }
                catch (Exception ex)
                {
                    LogMessage("ExposureMin", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Exposure resolution
        /// </summary>
        /// <returns>The smallest increment in exposure time supported by <see cref="StartExposure">StartExposure</see>.</returns>
        public double ExposureResolution
        {
            get
            {
                try
                {
                    CheckConnected("ExposureResolution");
                    double exposureResolution = CameraHardware.ExposureResolution;
                    LogMessage("ExposureResolution", exposureResolution.ToString());
                    return exposureResolution;
                }
                catch (Exception ex)
                {
                    LogMessage("ExposureResolution", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets or sets Fast Readout Mode
        /// </summary>
        /// <value><c>true</c> for fast readout mode, <c>false</c> for normal mode</value>
        public bool FastReadout
        {
            get
            {
                try
                {
                    CheckConnected("FastReadout Get");
                    bool fastreadout = CameraHardware.FastReadout;
                    LogMessage("FastReadout Get", fastreadout.ToString());
                    return fastreadout;
                }
                catch (Exception ex)
                {
                    LogMessage("FastReadout Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    CheckConnected("FastReadout Set");
                    LogMessage("FastReadout Set", value.ToString());
                    CameraHardware.FastReadout = value;
                }
                catch (Exception ex)
                {
                    LogMessage("FastReadout Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Reports the full well capacity of the camera in electrons, at the current camera settings (binning, SetupDialog settings, etc.)
        /// </summary>
        /// <value>The full well capacity.</value>
        public double FullWellCapacity
        {
            get
            {
                try
                {
                    CheckConnected("FullWellCapacity");
                    double fullWellCapacity = CameraHardware.FullWellCapacity;
                    LogMessage("FullWellCapacity", fullWellCapacity.ToString());
                    return fullWellCapacity;
                }
                catch (Exception ex)
                {
                    LogMessage("FullWellCapacity", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// The camera's gain (GAIN VALUE MODE) OR the index of the selected camera gain description in the <see cref="Gains" /> array (GAINS INDEX MODE)
        /// </summary>
        /// <returns><para><b> GAIN VALUE MODE:</b> The current gain value.</para>
        /// <p style="color:red"><b>OR</b></p>
        /// <b>GAINS INDEX MODE:</b> Index into the Gains array for the current camera gain
        /// </returns>
        public short Gain
        {
            get
            {
                try
                {
                    CheckConnected("Gain Get");
                    short gain = CameraHardware.Gain;
                    LogMessage("Gain Get", gain.ToString());
                    return gain;
                }
                catch (Exception ex)
                {
                    LogMessage("Gain Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                LogMessage("Gain Set", value.ToString());
                CameraHardware.Gain = value;
                try
                {
                    CheckConnected("Gain Set");
                }
                catch (Exception ex)
                {
                    LogMessage("Gain Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Maximum <see cref="Gain" /> value of that this camera supports
        /// </summary>
        /// <returns>The maximum gain value that this camera supports</returns>
        public short GainMax
        {
            get
            {
                try
                {
                    CheckConnected("GainMax");
                    short gainMax = CameraHardware.GainMax;
                    LogMessage("GainMax", gainMax.ToString());
                    return gainMax;
                }
                catch (Exception ex)
                {
                    LogMessage("GainMax", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Minimum <see cref="Gain" /> value of that this camera supports
        /// </summary>
        /// <returns>The minimum gain value that this camera supports</returns>
        public short GainMin
        {
            get
            {
                try
                {
                    CheckConnected("GainMin");
                    short gainMin = CameraHardware.GainMin;
                    LogMessage("GainMin", gainMin.ToString());
                    return gainMin;
                }
                catch (Exception ex)
                {
                    LogMessage("GainMin", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// List of Gain names supported by the camera
        /// </summary>
        /// <returns>The list of supported gain names as an ArrayList of strings</returns>
        public ArrayList Gains
        {
            get
            {
                try
                {
                    CheckConnected("Gains");
                    ArrayList gains = CameraHardware.Gains;
                    LogMessage("Gains", $"Received {gains.Count} gain values.");
                    return gains;
                }
                catch (Exception ex)
                {
                    LogMessage("Gains", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns a flag indicating whether this camera has a mechanical shutter
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has shutter; otherwise, <c>false</c>.
        /// </value>
        public bool HasShutter
        {
            get
            {
                try
                {
                    CheckConnected("HasShutter");
                    bool hasShutter = CameraHardware.HasShutter;
                    LogMessage("HasShutter", hasShutter.ToString());
                    return hasShutter;
                }
                catch (Exception ex)
                {
                    LogMessage("HasShutter", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns the current heat sink temperature (called"ambient temperature" by some manufacturers) in degrees Celsius.
        /// </summary>
        /// <value>The heat sink temperature.</value>
        public double HeatSinkTemperature
        {
            get
            {
                try
                {
                    CheckConnected("HeatSinkTemperature");
                    double heatSinkTemperature = CameraHardware.HeatSinkTemperature;
                    LogMessage("HeatSinkTemperature", heatSinkTemperature.ToString());
                    return heatSinkTemperature;
                }
                catch (Exception ex)
                {
                    LogMessage("HeatSinkTemperature", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns a safearray of integer of size <see cref="NumX" /> * <see cref="NumY" /> containing the pixel values from the last exposure.
        /// </summary>
        /// <value>The image array.</value>
        public object ImageArray
        {
            get
            {
                try
                {
                    CheckConnected("ImageArray");
                    LogMessage("ImageArray", $"Retrieving image array");
                    return CameraHardware.ImageArray;
                }
                catch (Exception ex)
                {
                    LogMessage("ImageArray", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns a safearray of Variant of size <see cref="NumX" /> * <see cref="NumY" /> containing the pixel values from the last exposure.
        /// </summary>
        /// <value>The image array variant.</value>
        public object ImageArrayVariant
        {
            get
            {
                try
                {
                    CheckConnected("ImageArrayVariant");
                    LogMessage("ImageArrayVariant", $"Retrieving image array");
                    return CameraHardware.ImageArrayVariant;
                }
                catch (Exception ex)
                {
                    LogMessage("ImageArrayVariant", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns a flag indicating whether the image is ready to be downloaded from the camera
        /// </summary>
        /// <value><c>true</c> if [image ready]; otherwise, <c>false</c>.</value>
        public bool ImageReady
        {
            get
            {
                try
                {
                    CheckConnected("ImageReady");
                    bool imageReady = CameraHardware.ImageReady;
                    LogMessage("ImageReady", imageReady.ToString());
                    return imageReady;
                }
                catch (Exception ex)
                {
                    LogMessage("ImageReady", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns a flag indicating whether the camera is currently in a <see cref="PulseGuide">PulseGuide</see> operation.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is pulse guiding; otherwise, <c>false</c>.
        /// </value>
        public bool IsPulseGuiding
        {
            get
            {
                try
                {
                    CheckConnected("IsPulseGuiding");
                    bool isPulseGuiding = CameraHardware.IsPulseGuiding;
                    LogMessage("IsPulseGuiding", isPulseGuiding.ToString());
                    return isPulseGuiding;
                }
                catch (Exception ex)
                {
                    LogMessage("IsPulseGuiding", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Reports the actual exposure duration in seconds (i.e. shutter open time).
        /// </summary>
        /// <value>The last duration of the exposure.</value>
        public double LastExposureDuration
        {
            get
            {
                try
                {
                    CheckConnected("LastExposureDuration");
                    double lastExposureDuration = CameraHardware.LastExposureDuration;
                    LogMessage("LastExposureDuration", lastExposureDuration.ToString());
                    return lastExposureDuration;
                }
                catch (Exception ex)
                {
                    LogMessage("LastExposureDuration", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Reports the actual exposure start in the FITS-standard CCYY-MM-DDThh:mm:ss[.sss...] format.
        /// The start time must be UTC.
        /// </summary>
        /// <value>The last exposure start time in UTC.</value>
        public string LastExposureStartTime
        {
            get
            {
                try
                {
                    CheckConnected("LastExposureStartTime");
                    string lastExposureStartTime = CameraHardware.LastExposureStartTime;
                    LogMessage("LastExposureStartTime", lastExposureStartTime.ToString());
                    return lastExposureStartTime;
                }
                catch (Exception ex)
                {
                    LogMessage("LastExposureStartTime", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Reports the maximum ADU value the camera can produce.
        /// </summary>
        /// <value>The maximum ADU.</value>
        public int MaxADU
        {
            get
            {
                try
                {
                    CheckConnected("MaxADU");
                    int maxAdu = CameraHardware.MaxADU;
                    LogMessage("MaxADU", maxAdu.ToString());
                    return maxAdu;
                }
                catch (Exception ex)
                {
                    LogMessage("MaxADU", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns the maximum allowed binning for the X camera axis
        /// </summary>
        /// <value>The maximum width of the image.</value>
        public short MaxBinX
        {
            get
            {
                try
                {
                    CheckConnected("MaxBinX");
                    short maxBinX = CameraHardware.MaxBinX;
                    LogMessage("MaxBinX", maxBinX.ToString());
                    return maxBinX;
                }
                catch (Exception ex)
                {
                    LogMessage("MaxBinX", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns the maximum allowed binning for the Y camera axis
        /// </summary>
        /// <value>The maximum height of the image.</value>
        public short MaxBinY
        {
            get
            {
                try
                {
                    CheckConnected("MaxBinY");
                    short maxBinY = CameraHardware.MaxBinY;
                    LogMessage("MaxBinY", maxBinY.ToString());
                    return maxBinY;
                }
                catch (Exception ex)
                {
                    LogMessage("MaxBinY", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the subframe width. Also returns the current value.
        /// </summary>
        /// <value>The width of the image.</value>
        public int NumX
        {
            get
            {
                try
                {
                    CheckConnected("NumX Get");
                    int numX = CameraHardware.NumX;
                    LogMessage("NumX Get", numX.ToString());
                    return numX;
                }
                catch (Exception ex)
                {
                    LogMessage("NumX Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    CheckConnected("NumX Set");
                    LogMessage("NumX Set", value.ToString());
                    CameraHardware.NumX = value;
                }
                catch (Exception ex)
                {
                    LogMessage("NumX Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the subframe height. Also returns the current value.
        /// </summary>
        /// <value>The height of the image.</value>
        public int NumY
        {
            get
            {
                try
                {
                    CheckConnected("NumY Get");
                    int numY = CameraHardware.NumY;
                    LogMessage("NumY Get", numY.ToString());
                    return numY;
                }
                catch (Exception ex)
                {
                    LogMessage("NumY Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                LogMessage("NumY Set", value.ToString());
                CameraHardware.NumY = value;
                try
                {
                    CheckConnected("NumY Set");
                }
                catch (Exception ex)
                {
                    LogMessage("NumY Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// The camera's offset (OFFSET VALUE MODE) OR the index of the selected camera offset description in the <see cref="Offsets" /> array (OFFSETS INDEX MODE)
        /// </summary>
        /// <returns><para><b> OFFSET VALUE MODE:</b> The current offset value.</para>
        /// <p style="color:red"><b>OR</b></p>
        /// <b>OFFSETS INDEX MODE:</b> Index into the Offsets array for the current camera offset
        /// </returns>
        public int Offset
        {
            get
            {
                try
                {
                    CheckConnected("Offset Get");
                    int offset = CameraHardware.Offset;
                    LogMessage("Offset Get", offset.ToString());
                    return offset;
                }
                catch (Exception ex)
                {
                    LogMessage("Offset Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    CheckConnected("Offset Set");
                    LogMessage("Offset Set", value.ToString());
                    CameraHardware.Offset = value;
                }
                catch (Exception ex)
                {
                    LogMessage("Offset Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Maximum <see cref="Off" /> value that this camera supports
        /// </summary>
        /// <returns>The maximum offset value that this camera supports</returns>
        public int OffsetMax
        {
            get
            {
                try
                {
                    CheckConnected("OffsetMax");
                    int offsetMax = CameraHardware.OffsetMax;
                    LogMessage("OffsetMax", offsetMax.ToString());
                    return offsetMax;
                }
                catch (Exception ex)
                {
                    LogMessage("OffsetMax", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Minimum <see cref="Off" /> value that this camera supports
        /// </summary>
        /// <returns>The minimum offset value that this camera supports</returns>
        public int OffsetMin
        {
            get
            {
                try
                {
                    CheckConnected("OffsetMin");
                    int offsetMin = CameraHardware.OffsetMin;
                    LogMessage("OffsetMin", offsetMin.ToString());
                    return offsetMin;
                }
                catch (Exception ex)
                {
                    LogMessage("OffsetMin", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// List of Offset names supported by the camera
        /// </summary>
        /// <returns>The list of supported offset names as an ArrayList of strings</returns>
        public ArrayList Offsets
        {
            get
            {
                try
                {
                    CheckConnected("Offsets");
                    ArrayList offsets = CameraHardware.Offsets;
                    LogMessage("Offsets", $"{offsets.Count} offsets were returned.");
                    return offsets;
                }
                catch (Exception ex)
                {
                    LogMessage("Offsets", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Percent completed, Interface Version 2 and later
        /// </summary>
        /// <returns>A value between 0 and 100% indicating the completeness of this operation</returns>
        public short PercentCompleted
        {
            get
            {
                try
                {
                    CheckConnected("PercentCompleted");
                    short percentCompleted = CameraHardware.PercentCompleted;
                    LogMessage("PercentCompleted", percentCompleted.ToString());
                    return percentCompleted;
                }
                catch (Exception ex)
                {
                    LogMessage("PercentCompleted", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns the width of the CCD chip pixels in microns.
        /// </summary>
        /// <value>The pixel size X.</value>
        public double PixelSizeX
        {
            get
            {
                try
                {
                    CheckConnected("PixelSizeX");
                    double pixelSizeX = CameraHardware.PixelSizeX;
                    LogMessage("PixelSizeX", pixelSizeX.ToString());
                    return pixelSizeX;
                }
                catch (Exception ex)
                {
                    LogMessage("PixelSizeX", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns the height of the CCD chip pixels in microns.
        /// </summary>
        /// <value>The pixel size Y.</value>
        public double PixelSizeY
        {
            get
            {
                try
                {
                    CheckConnected("PixelSizeY");
                    double pixelSizeY = CameraHardware.PixelSizeY;
                    LogMessage("PixelSizeY", pixelSizeY.ToString());
                    return pixelSizeY;
                }
                catch (Exception ex)
                {
                    LogMessage("PixelSizeY", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Activates the Camera's mount control system to instruct the mount to move in a particular direction for a given period of time
        /// </summary>
        /// <param name="direction">The direction of movement.</param>
        /// <param name="duration">The duration of movement in milli-seconds.</param>
        public void PulseGuide(GuideDirections direction, int duration)
        {
            try
            {
                CheckConnected("PulseGuide");
                LogMessage("PulseGuide", $"Direction: {direction}, Duration: {duration}");
                CameraHardware.PulseGuide(direction, duration);
                LogMessage("PulseGuide", $"Completed.");
            }
            catch (Exception ex)
            {
                LogMessage("PulseGuide", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Readout mode, Interface Version 2 and later
        /// </summary>
        /// <value></value>
        /// <returns>Short integer index into the <see cref="ReadoutModes">ReadoutModes</see> array of string readout mode names indicating
        /// the camera's current readout mode.</returns>
        public short ReadoutMode
        {
            get
            {
                try
                {
                    CheckConnected("ReadoutMode Get");
                    short readoutMode = CameraHardware.ReadoutMode;
                    LogMessage("ReadoutMode Get", readoutMode.ToString());
                    return readoutMode;
                }
                catch (Exception ex)
                {
                    LogMessage("ReadoutMode Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    CheckConnected("ReadoutMode Set");
                    LogMessage("ReadoutMode Set", value.ToString());
                    CameraHardware.ReadoutMode = value;
                }
                catch (Exception ex)
                {
                    LogMessage("ReadoutMode Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// List of available readout modes, Interface Version 2 and later
        /// </summary>
        /// <returns>An ArrayList of readout mode names</returns>
        public ArrayList ReadoutModes
        {
            get
            {
                try
                {
                    CheckConnected("ReadoutModes");
                    ArrayList readoutMode = CameraHardware.ReadoutModes;
                    LogMessage("ReadoutModes", $"Received {readoutMode.Count} readout modes.");
                    return readoutMode;
                }
                catch (Exception ex)
                {
                    LogMessage("ReadoutModes", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Sensor name, Interface Version 2 and later
        /// </summary>
        /// <returns>The name of the sensor used within the camera.</returns>
        public string SensorName
        {
            get
            {
                try
                {
                    CheckConnected("SensorName");
                    string sensorName = CameraHardware.SensorName;
                    LogMessage("SensorName", sensorName.ToString());
                    return sensorName;
                }
                catch (Exception ex)
                {
                    LogMessage("SensorName", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Type of colour information returned by the camera sensor, Interface Version 2 and later
        /// </summary>
        /// <value>The type of sensor used by the camera.</value>
        public SensorType SensorType
        {
            get
            {
                try
                {
                    CheckConnected("SensorType");
                    SensorType sensorType = CameraHardware.SensorType;
                    LogMessage("SensorType", sensorType.ToString());
                    return sensorType;
                }
                catch (Exception ex)
                {
                    LogMessage("SensorType", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the camera cooler setpoint in degrees Celsius, and returns the current setpoint.
        /// </summary>
        /// <value>The set CCD temperature.</value>
        public double SetCCDTemperature
        {
            get
            {
                try
                {
                    CheckConnected("SetCCDTemperature Get");
                    double setCcdTemperature = CameraHardware.SetCCDTemperature;
                    LogMessage("SetCCDTemperature Get", setCcdTemperature.ToString());
                    return setCcdTemperature;
                }
                catch (Exception ex)
                {
                    LogMessage("SetCCDTemperature Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    CheckConnected("SetCCDTemperature Set");
                    LogMessage("SetCCDTemperature Set", value.ToString());
                    CameraHardware.SetCCDTemperature = value;
                }
                catch (Exception ex)
                {
                    LogMessage("SetCCDTemperature Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Starts an exposure. Use <see cref="ImageReady" /> to check when the exposure is complete.
        /// </summary>
        /// <param name="duration">Duration of exposure in seconds, can be zero if <see cref="StartExposure">Light</see> is <c>false</c></param>
        /// <param name="light"><c>true</c> for light frame, <c>false</c> for dark frame (ignored if no shutter)</param>
        public void StartExposure(double duration, bool light)
        {
            try
            {
                CheckConnected("StartExposure");
                LogMessage("StartExposure", $"Duration: {duration}, Light: {light}");
                CameraHardware.StartExposure(duration, light);
                LogMessage("StartExposure", $"Completed.");
            }
            catch (Exception ex)
            {
                LogMessage("StartExposure", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Sets the subframe start position for the X axis (0 based) and returns the current value.
        /// </summary>
        public int StartX
        {
            get
            {
                try
                {
                    CheckConnected("tartX Get");
                    int setCcdTemperature = CameraHardware.StartX;
                    LogMessage("StartX Get", setCcdTemperature.ToString());
                    return setCcdTemperature;
                }
                catch (Exception ex)
                {
                    LogMessage("StartX Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    CheckConnected("StartX Set");
                    LogMessage("StartX Set", value.ToString());
                    CameraHardware.StartX = value;
                }
                catch (Exception ex)
                {
                    LogMessage("StartX Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the subframe start position for the Y axis (0 based). Also returns the current value.
        /// </summary>
        public int StartY
        {
            get
            {
                try
                {
                    CheckConnected("StartY Get");
                    int setCcdTemperature = CameraHardware.StartY;
                    LogMessage("StartY Get", setCcdTemperature.ToString());
                    return setCcdTemperature;
                }
                catch (Exception ex)
                {
                    LogMessage("StartY Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    CheckConnected("StartY Set");
                    LogMessage("StartY Set", value.ToString());
                    CameraHardware.StartY = value;
                }
                catch (Exception ex)
                {
                    LogMessage("StartY Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Stops the current exposure, if any.
        /// </summary>
        public void StopExposure()
        {
            try
            {
                CheckConnected("StopExposure");
                LogMessage("StopExposure", $"Method called");
                CameraHardware.StopExposure();
                LogMessage("StopExposure", $"Completed.");
            }
            catch (Exception ex)
            {
                LogMessage("StopExposure", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Camera's sub-exposure interval
        /// </summary>
        public double SubExposureDuration
        {
            get
            {
                try
                {
                    CheckConnected("SubExposureDuration Get");
                    double subExposureDuration = CameraHardware.SubExposureDuration;
                    LogMessage("SubExposureDuration Get", subExposureDuration.ToString());
                    return subExposureDuration;
                }
                catch (Exception ex)
                {
                    LogMessage("SubExposureDuration Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    CheckConnected("SubExposureDuration Set");
                    LogMessage("SubExposureDuration Set", value.ToString());
                    CameraHardware.SubExposureDuration = value;
                }
                catch (Exception ex)
                {
                    LogMessage("SubExposureDuration Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        #endregion

        #region Private properties and methods

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!CameraHardware.IsConnected(uniqueId))
            {
                throw new NotConnectedException($"{cameraDescription} ({CameraProgId}) is not connected: {message}");
            }
        }

        /// <summary>
        /// Log helper function that writes to the driver or local server loggers as required
        /// </summary>
        /// <param name="identifier">Identifier such as method name</param>
        /// <param name="message">Message to be logged.</param>
        private void LogMessage(string identifier, string message)
        {
            // This code is currently set to write messages to an individual driver log AND to the shared hardware log.

            // Write to the individual log for this specific instance (if enabled by the driver having a TraceLogger instance)
            if (tl != null)
            {
                tl.LogMessageCrLf(identifier, message); // Write to the individual driver log
            }

            // Write to the common hardware log shared by all running instances of the driver.
            CameraHardware.LogMessage(identifier, message); // Write to the local server logger
        }

        /// <summary>
        /// Read the trace state from the driver's Profile and enable / disable the trace log accordingly.
        /// </summary>
        private void SetTraceState()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Camera";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(CameraProgId, CameraHardware.TRACE_STATE_PROFILE_NAME, string.Empty, CameraHardware.TRACE_STATE_DEFAULT));
            }
        }

        #endregion

        #region Connect / Disconnect and DeviceState members

        /// <summary>
        /// Connect to the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>This is a mandatory method and must not throw a <see cref="MethodNotImplementedException"/>.</b></p></remarks>
        public void Connect()
        {
            // Check whether the connected device is Platform 7 or later
            if (Common.DeviceInterfaces.DeviceCapabilities.HasConnectAndDeviceState(Common.DeviceTypes.Camera, CameraHardware.GetInterfaceVersion())) // Platform 7 or later device so Connect
            {
                LogMessage("Connect", "Issuing Connect command");
                CameraHardware.Connect(uniqueId, CameraHardware.ConnectType.Connect_Disconnect);
                return;
            }

            // Platform 6 or earlier so reject the method call
            LogMessage("Connect", "Throwing a MethodNotImplementedException because the connected camera is a Platform 6 or earlier device.");
            throw new MethodNotImplementedException($"Connect is not implemented in this ICameraV{CameraHardware.GetInterfaceVersion()} device.");
        }

        /// <summary>
        /// Disconnect from the device asynchronously
        /// </summary>
        public void Disconnect()
        {
            // Check whether the connected device is Platform 7 or later
            if (Common.DeviceInterfaces.DeviceCapabilities.HasConnectAndDeviceState(Common.DeviceTypes.Camera, CameraHardware.GetInterfaceVersion())) // Platform 7 or later device so Disconnect
            {
                LogMessage("Disconnect", "Issuing Disconnect command");
                CameraHardware.Disconnect(uniqueId, CameraHardware.ConnectType.Connect_Disconnect);
                return;
            }

            // Platform 6 or earlier so reject the method call
            LogMessage("Disconnect", "Throwing a MethodNotImplementedException because the connected camera is a Platform 6 or earlier device.");
            throw new MethodNotImplementedException($"Disconnect is not implemented in this ICameraV{CameraHardware.GetInterfaceVersion()} device.");
        }

        /// <summary>
        /// Returns True while the device is undertaking an asynchronous connect or disconnect operation.
        /// </summary>
        public bool Connecting
        {
            get
            {
                // Check whether the connected device is Platform 7 or later
                if (Common.DeviceInterfaces.DeviceCapabilities.HasConnectAndDeviceState(Common.DeviceTypes.Camera, CameraHardware.GetInterfaceVersion())) // Platform 7 or later device so call Connecting
                {
                    LogMessage("Connecting", "Calling Connecting property");
                    return CameraHardware.Connecting;
                }

                // Platform 6 or earlier so reject the method call
                LogMessage("Connecting", "Throwing a PropertyNotImplementedException because the connected camera is a Platform 6 or earlier device.");
                throw new PropertyNotImplementedException($"Connecting is not implemented in this ICameraV{CameraHardware.GetInterfaceVersion()} device.");
            }
        }

        /// <summary>
        /// Returns the device's operational state in a single call.
        /// </summary>
        public IStateValueCollection DeviceState
        {
            get
            {
                CheckConnected("DeviceState");

                // Check whether the connected device is Platform 7 or later
                if (Common.DeviceInterfaces.DeviceCapabilities.HasConnectAndDeviceState(Common.DeviceTypes.Camera, CameraHardware.GetInterfaceVersion())) // Platform 7 or later device so call Connecting
                {
                    LogMessage("DeviceState", "Calling DeviceState property");
                    return CameraHardware.DeviceState;
                }

                // Platform 6 or earlier so return an empty ArrayList because this feature isn't supported
                LogMessage("DeviceState", "Returning an empty ArrayList because this feature isn't supported.");
                return new StateValueCollection();
            }
        }

        #endregion

        #region Internal members

        internal static string CameraProgId { get; private set; }

        #endregion
    }
}
