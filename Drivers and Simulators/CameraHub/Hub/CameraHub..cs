using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ASCOM.CameraHub.Camera
{
    /// <summary>
    /// ASCOM Camera Hub main functional class shared by all instances of the driver class.
    /// </summary>
    [HardwareClass()] // Attribute to flag this as a device hardware class that needs to be disposed by the local server when it exits.
    internal static class CameraHub
    {
        // Constants used for Profile persistence
        internal const string TRACE_STATE_PROFILE_NAME = "Trace Level"; internal const string TRACE_STATE_DEFAULT = "true";
        internal const string CAMERA_PROGID = "Camera ProgID"; internal const string CAMERA_PROGID_DEFAULT = "ASCOM.Simulator.Camera";
        internal static string cameraProgId;

#if DEBUG
        private static DriverAccess.Camera camera; // Camera device being hosted
#else
        private static dynamic camera; // Camera device being hosted
#endif
        private static readonly string hubProgId = ""; // ASCOM DeviceID (COM ProgID) for this driver, the value is set by the driver's class initialiser.
        private static string hubDescription = ""; // The value is set by the driver's class initialiser.
        private static bool connectedState; // Local server's connected state

        private static List<Guid> uniqueIds = new List<Guid>(); // List of driver instance unique IDs

        private static bool runOnce = false; // Flag to enable "one-off" activities only to run once.
        internal static Util utilities; // ASCOM Utilities object for use as required
        internal static TraceLogger TL; // Local server's trace logger object for diagnostic log with information that you specify

        /// <summary>
        /// Initializes a new instance of the device Hardware class.
        /// </summary>
        static CameraHub()
        {
            try
            {
                // Create the hardware trace logger in the static initialiser.
                // All other initialisation should go in the InitialiseHardware method.
                TL = new TraceLogger("", "CameraHub");

                // DriverProgId has to be set here because it used by ReadProfile to get the TraceState flag.
                hubProgId = Camera.hubProgId; // Get this device's ProgID so that it can be used to read the Profile configuration values

                // ReadProfile has to go here before anything is written to the log because it loads the TraceLogger enable / disable state.
                ReadProfile(); // Read device configuration from the ASCOM Profile store, including the trace state

                LogMessage("CameraHub", $"Static initialiser completed.");
            }
            catch (Exception ex)
            {
                try { LogMessage("CameraHub", $"Initialisation exception: {ex}"); } catch { }
                MessageBox.Show($"{ex.Message}", "Exception creating ASCOM.CameraHub.Camera", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        /// <summary>
        /// Place device initialisation code here
        /// </summary>
        /// <remarks>Called every time a new instance of the driver is created.</remarks>
        internal static void InitialiseHub()
        {
            // This method will be called every time a new ASCOM client loads your driver
            LogMessage("InitialiseHub", $"Start.");

            // Make sure that "one off" activities are only undertaken once
            if (runOnce == false)
            {
                LogMessage("InitialiseHub", $"Starting one-off initialisation.");

                hubDescription = Camera.hubDescription; // Get this device's Chooser description

                LogMessage("InitialiseHub", $"ProgID: {hubProgId}, Description: {hubDescription}");

                connectedState = false; // Initialise connected to false
                utilities = new Util(); //Initialise ASCOM Utilities object

                LogMessage("InitialiseHub", "Completed basic initialisation");

                // Add your own "one off" device initialisation here e.g. validating existence of hardware and setting up communications

                CreateCameraInstance();

                if (string.IsNullOrEmpty(cameraProgId))
                    throw new InvalidValueException("The camera ProgID is null or empty");

                LogMessage("InitialiseHub", $"One-off initialisation complete.");
                runOnce = true; // Set the flag to ensure that this code is not run again
            }
        }

        /// <summary>
        /// Connect to the hardware if not already connected
        /// </summary>
        /// <param name="uniqueId">Unique ID identifying the calling driver instance.</param>
        /// <remarks>
        /// The unique ID is stored to record that the driver instance is connected and to ensure that multiple calls from the same driver are ignored.
        /// If this is the first driver instance to connect, the hardware link to the device is established
        /// </remarks>
        public static void Connect(Guid uniqueId)
        {
            LogMessage("Connect", $"Unique ID: {uniqueId}");

            // Check whether this driver instance has already connected
            if (uniqueIds.Contains(uniqueId)) // Instance already connected
            {
                // Ignore the request, the unique ID is already in the list
                LogMessage("Connect", $"Ignoring request to connect because the device is already connected.");
                return;
            }

            // Driver instance not yet connected

            // Test whether the camera is already connected
            if (!connectedState) // Camera hardware is not connected so connect
            {
                LogMessage("Connect", $"First connection request - Connecting to hardware...");
                camera.Connected = true;
                connectedState = true;
                LogMessage("Connect", $"Camera connected OK.");
            }

            // Add the driver unique ID to the connected list
            uniqueIds.Add(uniqueId);
            LogMessage("Connect", $"Unique id {uniqueId} added to the connection list.");

            // Log the current connected state
            LogMessage("Connect", $"Currently connected driver ids:");
            foreach (Guid id in uniqueIds)
            {
                LogMessage("Connect", $" ID {id} is connected");
            }
        }

        /// <summary>
        /// Disconnect from the hardware if this is the last driver instance that is connected
        /// </summary>
        /// <param name="uniqueId">Unique ID identifying the calling driver instance.</param>
        /// <remarks>
        /// The list of connected driver instance IDs is queried to determine whether this driver instance is connected and, if so, it is removed from the connection list. 
        /// The unique ID ensures that multiple calls from the same driver are ignored.
        /// If this is the last connected driver instance, the link to the device hardware is disconnected.
        /// </remarks>
        public static void Disconnect(Guid uniqueId)
        {
            LogMessage("Disconnect", $"Unique ID: {uniqueId}");

            if (!uniqueIds.Contains(uniqueId)) // Instance already disconnected
            {
                // Ignore the request, the unique ID is absent from the list
                LogMessage("Disconnect", $"Ignoring request to disconnect because the device is already disconnected.");
                return;
            }

            // Driver instance currently connected

            // Remove the driver unique ID from the connected list
            uniqueIds.Remove(uniqueId);
            LogMessage("Disconnect", $"Unique id {uniqueId} removed from the connection list.");

            // Test whether any instances are still connected
            if (uniqueIds.Count == 0) // No instances remain connected so disconnect the camera device
            {
                LogMessage("Disconnect", $"Last disconnection request - Disconnecting hardware...");
                camera.Connected = false;
                connectedState = false;
                LogMessage("Disconnect", $"Camera disconnected OK.");
            }

            // Log the current connected state
            if (uniqueIds.Count > 0)
            {
                LogMessage("Disconnect", $"Remaining connected driver IDs:");
                foreach (Guid id in uniqueIds)
                {
                    LogMessage("Disconnect", $"  ID {id} is connected");
                }
            }
            else
                LogMessage("Disconnect", $"No connected devices.");
        }

        /// <summary>
        /// Test whether a driver instance is connected, identified by its unique ID
        /// </summary>
        /// <param name="uniqueId">The driver's unique ID</param>
        /// <returns>True if the driver instance is connected</returns>
        public static bool IsConnected(Guid uniqueId)
        {
            return uniqueIds.Contains(uniqueId);
        }

        private static void CreateCameraInstance()
        {
            // Remove any current instance and replace with a new one
            if (!(camera is null)) // There is an existing instance
            {
                try { camera.Connected = false; } catch { }

                try { camera.Dispose(); } catch { }

                try
                {
                    int remainingCount;

                    do
                    {
                        remainingCount = Marshal.ReleaseComObject(camera);
                        LogMessage("CreateCameraInstance", $"Released COM object wrapper, remaining count: {remainingCount}.");
                    } while (remainingCount > 0);
                }
                catch { }

                camera = null;

                // ALlow some time to dispose of the driver
                System.Threading.Thread.Sleep(1000);
            }
            try
            {

                // Create an instance of the camera
                try
                {
#if DEBUG
                    LogMessage("CreateCameraInstance", $"Creating DriverAccess Camera device.");
                    camera = new DriverAccess.Camera(cameraProgId);
#else
                    // Get the Type of this ProgID
                    Type cameraType = Type.GetTypeFromProgID(cameraProgId);
                    LogMessage("CreateCameraInstance", $"Created Type for ProgID: {cameraProgId} OK.");
                    camera = Activator.CreateInstance(cameraType);
#endif
                    LogMessage("CreateCameraInstance", $"Created COM object for ProgID: {cameraProgId} OK.");
                }
                catch (Exception ex1)
                {
                    throw new InvalidOperationException($"Unable to create an instance of the camera with ProgID {cameraProgId}: {ex1.Message}");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unable to create Type for ProgID {cameraProgId}: {ex.Message}");
            }
        }

        // PUBLIC COM INTERFACE ICameraV3 IMPLEMENTATION

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialogue form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public static void SetupDialog()
        {
            // Don't permit the setup dialogue if already connected
            if (connectedState)
                MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm(TL))
            {
                var result = F.ShowDialog();
                if (result == DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store

                    // Kill the current instance and create a new once in case the configuration has changed
                    CreateCameraInstance();
                }
            }
        }

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        public static ArrayList SupportedActions
        {
            get
            {
                ArrayList actions = camera.SupportedActions;
                LogMessage("SupportedActions Get", $"Returning ArrayList of length: {actions.Count}");
                return actions;
            }
        }

        /// <summary>Invokes the specified device-specific custom action.</summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.</param>
        /// <returns>A string response. The meaning of returned strings is set by the driver author.
        /// <para>Suppose filter wheels start to appear with automatic wheel changers; new actions could be <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
        /// of wheel names and the second taking a wheel name and making the change, returning appropriate values to indicate success or failure.</para>
        /// </returns>
        public static string Action(string actionName, string actionParameters)
        {
            return camera.Action(actionName, actionParameters);
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
        public static void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            camera.CommandBlind(command, raw);
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
        public static bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            return camera.CommandBool(command, raw);
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
        public static string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            return camera.CommandString(command, raw);
        }

        /// <summary>
        /// Deterministically release both managed and unmanaged resources that are used by this class.
        /// </summary>
        /// <remarks>
        /// TODO: Release any managed or unmanaged resources that are used in this class.
        /// 
        /// Do not call this method from the Dispose method in your driver class.
        ///
        /// This is because this hardware class is decorated with the <see cref="HardwareClassAttribute"/> attribute and this Dispose() method will be called 
        /// automatically by the  local server executable when it is irretrievably shutting down. This gives you the opportunity to release managed and unmanaged 
        /// resources in a timely fashion and avoid any time delay between local server close down and garbage collection by the .NET runtime.
        ///
        /// For the same reason, do not call the SharedResources.Dispose() method from this method. Any resources used in the static shared resources class
        /// itself should be released in the SharedResources.Dispose() method as usual. The SharedResources.Dispose() method will be called automatically 
        /// by the local server just before it shuts down.
        /// 
        /// </remarks>
        public static void Dispose()
        {
            try { LogMessage("CameraHub.Dispose", $"Disposing of assets and closing down."); } catch { }

            if (!(camera is null))
            {
#if DEBUG
                try { camera.Dispose(); } catch (Exception) { }
                try { LogMessage("CameraHub.Dispose", $"Disposed DriverAccess camera object."); } catch { }
                try { camera = null; } catch (Exception) { }
#else
                try { Marshal.ReleaseComObject(camera); } catch (Exception) { }
                try { LogMessage("CameraHub.Dispose", $"Released camera COM object."); } catch { }
                try { camera = null; } catch (Exception) { }
#endif
            }

            try
            {
                // Clean up the trace logger and utility objects
                TL.Enabled = false;
                TL.Dispose();
                TL = null;
            }
            catch { }

            try
            {
                utilities.Dispose();
                utilities = null;
            }
            catch { }

        }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        public static string Description
        {
            get
            {
                string description = camera.Description;
                LogMessage("Description Get", description);
                return description;
            }
        }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        public static string DriverInfo
        {
            get
            {
                string driverInfo = camera.DriverInfo;
                LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        /// <summary>
        /// A string containing only the major and minor version of the driver formatted as 'm.n'.
        /// </summary>
        public static string DriverVersion
        {
            get
            {
                string driverVersion = camera.DriverVersion;
                LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        /// <summary>
        /// The interface version number that this device supports.
        /// </summary>
        public static short InterfaceVersion
        {
            get
            {
                short interfaceVersion = camera.InterfaceVersion;
                LogMessage("InterfaceVersion Get", interfaceVersion.ToString());
                return interfaceVersion;
            }
        }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        public static string Name
        {
            get
            {
                string name = camera.Name;
                LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region ICamera Implementation

        /// <summary>
        /// Aborts the current exposure, if any, and returns the camera to Idle state.
        /// </summary>
        static internal void AbortExposure()
        {
            camera.AbortExposure();
        }

        /// <summary>
        /// Returns the X offset of the Bayer matrix, as defined in <see cref="SensorType" />.
        /// </summary>
        /// <returns>The Bayer colour matrix X offset, as defined in <see cref="SensorType" />.</returns>
        static internal short BayerOffsetX
        {
            get
            {
                return camera.BayerOffsetX;
            }
        }

        /// <summary>
        /// Returns the Y offset of the Bayer matrix, as defined in <see cref="SensorType" />.
        /// </summary>
        /// <returns>The Bayer colour matrix Y offset, as defined in <see cref="SensorType" />.</returns>
        static internal short BayerOffsetY
        {
            get
            {
                return camera.BayerOffsetY;
            }
        }

        /// <summary>
        /// Sets the binning factor for the X axis, also returns the current value.
        /// </summary>
        /// <value>The X binning value</value>
        static internal short BinX
        {
            get
            {
                return camera.BinX;
            }
            set
            {
                camera.BinX = value;
            }
        }

        /// <summary>
        /// Sets the binning factor for the Y axis, also returns the current value.
        /// </summary>
        /// <value>The Y binning value.</value>
        static internal short BinY
        {
            get
            {
                return camera.BinY;
            }
            set
            {
                camera.BinY = value;
            }
        }

        /// <summary>
        /// Returns the current CCD temperature in degrees Celsius.
        /// </summary>
        /// <value>The CCD temperature.</value>
        static internal double CCDTemperature
        {
            get
            {
                return camera.CCDTemperature;
            }
        }

        /// <summary>
        /// Returns the current camera operational state
        /// </summary>
        /// <value>The state of the camera.</value>
        static internal CameraStates CameraState
        {
            get
            {
                return (CameraStates)camera.CameraState;
            }
        }

        /// <summary>
        /// Returns the width of the CCD camera chip in unbinned pixels.
        /// </summary>
        /// <value>The size of the camera X.</value>
        static internal int CameraXSize
        {
            get
            {
                return camera.CameraXSize;
            }
        }

        /// <summary>
        /// Returns the height of the CCD camera chip in unbinned pixels.
        /// </summary>
        /// <value>The size of the camera Y.</value>
        static internal int CameraYSize
        {
            get
            {
                return camera.CameraYSize;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if the camera can abort exposures; <c>false</c> if not.
        /// </summary>
        /// <value>
        static internal bool CanAbortExposure
        {
            get
            {
                return camera.CanAbortExposure;
            }
        }

        /// <summary>
        /// Returns a flag showing whether this camera supports asymmetric binning
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can asymmetric bin; otherwise, <c>false</c>.
        /// </value>
        static internal bool CanAsymmetricBin
        {
            get
            {
                return camera.CanAsymmetricBin;
            }
        }

        /// <summary>
        /// Camera has a fast readout mode
        /// </summary>
        /// <returns><c>true</c> when the camera supports a fast readout mode</returns>
        static internal bool CanFastReadout
        {
            get
            {
                return camera.CanFastReadout;
            }
        }

        /// <summary>
        /// If <c>true</c>, the camera's cooler power setting can be read.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can get cooler power; otherwise, <c>false</c>.
        /// </value>
        static internal bool CanGetCoolerPower
        {
            get
            {
                return camera.CanGetCoolerPower;
            }
        }

        /// <summary>
        /// Returns a flag indicating whether this camera supports pulse guiding
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can pulse guide; otherwise, <c>false</c>.
        /// </value>
        static internal bool CanPulseGuide
        {
            get
            {
                return camera.CanPulseGuide;
            }
        }

        /// <summary>
        /// Returns a flag indicating whether this camera supports setting the CCD temperature
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can set CCD temperature; otherwise, <c>false</c>.
        /// </value>
        static internal bool CanSetCCDTemperature
        {
            get
            {
                return camera.CanSetCCDTemperature;
            }
        }

        /// <summary>
        /// Returns a flag indicating whether this camera can stop an exposure that is in progress
        /// </summary>
        /// <value>
        /// <c>true</c> if the camera can stop the exposure; otherwise, <c>false</c>.
        /// </value>
        static internal bool CanStopExposure
        {
            get
            {
                return camera.CanStopExposure;
            }
        }

        /// <summary>
        /// Turns on and off the camera cooler, and returns the current on/off state.
        /// </summary>
        /// <value><c>true</c> if the cooler is on; otherwise, <c>false</c>.</value>
        static internal bool CoolerOn
        {
            get
            {
                return camera.CoolerOn;
            }
            set
            {
                camera.CoolerOn = value;
            }
        }

        /// <summary>
        /// Returns the present cooler power level, in percent.
        /// </summary>
        /// <value>The cooler power.</value>
        static internal double CoolerPower
        {
            get
            {
                return camera.CoolerPower;
            }
        }

        /// <summary>
        /// Returns the gain of the camera in photoelectrons per A/D unit.
        /// </summary>
        /// <value>The electrons per ADU.</value>
        static internal double ElectronsPerADU
        {
            get
            {
                return camera.ElectronsPerADU;
            }
        }

        /// <summary>
        /// Returns the maximum exposure time supported by <see cref="StartExposure">StartExposure</see>.
        /// </summary>
        /// <returns>The maximum exposure time, in seconds, that the camera supports</returns>
        static internal double ExposureMax
        {
            get
            {
                return camera.ExposureMax;
            }
        }

        /// <summary>
        /// Minimum exposure time
        /// </summary>
        /// <returns>The minimum exposure time, in seconds, that the camera supports through <see cref="StartExposure">StartExposure</see></returns>
        static internal double ExposureMin
        {
            get
            {
                return camera.ExposureMin;
            }
        }

        /// <summary>
        /// Exposure resolution
        /// </summary>
        /// <returns>The smallest increment in exposure time supported by <see cref="StartExposure">StartExposure</see>.</returns>
        static internal double ExposureResolution
        {
            get
            {
                return camera.ExposureResolution;
            }
        }

        /// <summary>
        /// Gets or sets Fast Readout Mode
        /// </summary>
        /// <value><c>true</c> for fast readout mode, <c>false</c> for normal mode</value>
        static internal bool FastReadout
        {
            get
            {
                return camera.FastReadout;
            }
            set
            {
                camera.FastReadout = value;
            }
        }

        /// <summary>
        /// Reports the full well capacity of the camera in electrons, at the current camera settings (binning, SetupDialog settings, etc.)
        /// </summary>
        /// <value>The full well capacity.</value>
        static internal double FullWellCapacity
        {
            get
            {
                return camera.FullWellCapacity;
            }
        }


        /// <summary>
        /// The camera's gain (GAIN VALUE MODE) OR the index of the selected camera gain description in the <see cref="Gains" /> array (GAINS INDEX MODE)
        /// </summary>
        /// <returns><para><b> GAIN VALUE MODE:</b> The current gain value.</para>
        /// <p style="color:red"><b>OR</b></p>
        /// <b>GAINS INDEX MODE:</b> Index into the Gains array for the current camera gain
        /// </returns>
        static internal short Gain
        {
            get
            {
                return camera.Gain;
            }
            set
            {
                camera.Gain = value;
            }
        }

        /// <summary>
        /// Maximum <see cref="Gain" /> value of that this camera supports
        /// </summary>
        /// <returns>The maximum gain value that this camera supports</returns>
        static internal short GainMax
        {
            get
            {
                return camera.GainMax;
            }
        }

        /// <summary>
        /// Minimum <see cref="Gain" /> value of that this camera supports
        /// </summary>
        /// <returns>The minimum gain value that this camera supports</returns>
        static internal short GainMin
        {
            get
            {
                return camera.GainMin;
            }
        }

        /// <summary>
        /// Minimum <see cref="Gain" /> value of that this camera supports
        /// </summary>
        /// <returns>The minimum gain value that this camera supports</returns>
        static internal ArrayList Gains
        {
            get
            {
                return camera.Gains;
            }
        }

        /// <summary>
        /// Returns a flag indicating whether this camera has a mechanical shutter
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has shutter; otherwise, <c>false</c>.
        /// </value>
        static internal bool HasShutter
        {
            get
            {
                return camera.HasShutter;
            }
        }

        /// <summary>
        /// Returns the current heat sink temperature (called "ambient temperature" by some manufacturers) in degrees Celsius.
        /// </summary>
        /// <value>The heat sink temperature.</value>
        static internal double HeatSinkTemperature
        {
            get
            {
                return camera.HeatSinkTemperature;
            }
        }

        /// <summary>
        /// Returns a safearray of integers of size <see cref="NumX" /> * <see cref="NumY" /> containing the pixel values from the last exposure.
        /// </summary>
        /// <value>The image array.</value>
        static internal object ImageArray
        {
            get
            {
                // Maximise available memory
                ReleaseArrayMemory();

                return camera.ImageArray;
            }
        }

        /// <summary>
        /// Returns a safearray of Variant of size <see cref="NumX" /> * <see cref="NumY" /> containing the pixel values from the last exposure.
        /// </summary>
        /// <value>The image array variant.</value>
        static internal object ImageArrayVariant
        {
            get
            {
                // Maximise available memory
                ReleaseArrayMemory();

                return camera.ImageArrayVariant;
            }
        }

        /// <summary>
        /// Returns a flag indicating whether the image is ready to be downloaded from the camera
        /// </summary>
        /// <value><c>true</c> if [image ready]; otherwise, <c>false</c>.</value>
        static internal bool ImageReady
        {
            get
            {
                return camera.ImageReady;
            }
        }

        /// <summary>
        /// Returns a flag indicating whether the camera is currently in a <see cref="PulseGuide">PulseGuide</see> operation.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is pulse guiding; otherwise, <c>false</c>.
        /// </value>
        static internal bool IsPulseGuiding
        {
            get
            {
                return camera.IsPulseGuiding;
            }
        }

        /// <summary>
        /// Reports the actual exposure duration in seconds (i.e. shutter open time).
        /// </summary>
        /// <value>The last duration of the exposure.</value>
        static internal double LastExposureDuration
        {
            get
            {
                return camera.LastExposureDuration;
            }
        }

        /// <summary>
        /// Reports the actual exposure start in the FITS-standard CCYY-MM-DDThh:mm:ss[.sss...] format.
        /// The start time must be UTC.
        /// </summary>
        /// <value>The last exposure start time in UTC.</value>
        static internal string LastExposureStartTime
        {
            get
            {
                return camera.LastExposureStartTime;
            }
        }

        /// <summary>
        /// Reports the maximum ADU value the camera can produce.
        /// </summary>
        /// <value>The maximum ADU.</value>
        static internal int MaxADU
        {
            get
            {
                return camera.MaxADU;
            }
        }

        /// <summary>
        /// Returns the maximum allowed binning for the X camera axis
        /// </summary>
        /// <value>The maximum bin X.</value>
        static internal short MaxBinX
        {
            get
            {
                return camera.MaxBinX;
            }
        }

        /// <summary>
        /// Returns the maximum allowed binning for the Y camera axis
        /// </summary>
        /// <value>The maximum bin Y.</value>
        static internal short MaxBinY
        {
            get
            {
                return camera.MaxBinY;
            }
        }

        /// <summary>
        /// Sets the subframe width. Also returns the current value.
        /// </summary>
        /// <value>The subframe width.</value>
        static internal int NumX
        {
            get
            {
                return camera.NumX;
            }
            set
            {
                camera.NumX = value;
            }
        }

        /// <summary>
        /// Sets the subframe height. Also returns the current value.
        /// </summary>
        /// <value>The subframe height.</value>
        static internal int NumY
        {
            get
            {
                return camera.NumY;
            }
            set
            {
                camera.NumY = value;
            }
        }

        /// <summary>
        /// The camera's offset (OFFSET VALUE MODE) OR the index of the selected camera offset description in the <see cref="Offsets" /> array (OFFSETS INDEX MODE)
        /// </summary>
        /// <returns><para><b> OFFSET VALUE MODE:</b> The current offset value.</para>
        /// <p style="color:red"><b>OR</b></p>
        /// <b>OFFSETS INDEX MODE:</b> Index into the Offsets array for the current camera offset
        /// </returns>
        static internal int Offset
        {
            get
            {
                return camera.Offset;
            }
            set
            {
                camera.Offset = value;
            }
        }

        /// <summary>
        /// Maximum <see cref="Offset" /> value that this camera supports
        /// </summary>
        /// <returns>The maximum offset value that this camera supports</returns>
        static internal int OffsetMax
        {
            get
            {
                return camera.OffsetMax;
            }
        }

        /// <summary>
        /// Minimum <see cref="Offset" /> value that this camera supports
        /// </summary>
        /// <returns>The minimum offset value that this camera supports</returns>
        static internal int OffsetMin
        {
            get
            {
                return camera.OffsetMin;
            }
        }

        /// <summary>
        /// List of Offset names supported by the camera
        /// </summary>
        /// <returns>The list of supported offset names as an ArrayList of strings</returns>
        static internal ArrayList Offsets
        {
            get
            {
                return camera.Offsets;
            }
        }

        /// <summary>
        /// Percent completed, Interface Version 2 and later
        /// </summary>
        /// <returns>A value between 0 and 100% indicating the completeness of this operation</returns>
        static internal short PercentCompleted
        {
            get
            {
                return camera.PercentCompleted;
            }
        }

        /// <summary>
        /// Returns the width of the CCD chip pixels in microns.
        /// </summary>
        /// <value>The pixel size X.</value>
        static internal double PixelSizeX
        {
            get
            {
                return camera.PixelSizeX;
            }
        }

        /// <summary>
        /// Returns the height of the CCD chip pixels in microns.
        /// </summary>
        /// <value>The pixel size Y.</value>
        static internal double PixelSizeY
        {
            get
            {
                return camera.PixelSizeY;
            }
        }

        /// <summary>
        /// Activates the Camera's mount control system to instruct the mount to move in a particular direction for a given period of time
        /// </summary>
        /// <param name="Direction">The direction of movement.</param>
        /// <param name="Duration">The duration of movement in milli-seconds.</param>
        static internal void PulseGuide(GuideDirections Direction, int Duration)
        {
            camera.PulseGuide(Direction, Duration);
        }

        /// <summary>
        /// Readout mode, Interface Version 2 only
        /// </summary>
        /// <value></value>
        /// <returns>Short integer index into the <see cref="ReadoutModes">ReadoutModes</see> array of string readout mode names indicating
        /// the camera's current readout mode.</returns>
        static internal short ReadoutMode
        {
            get
            {
                return camera.ReadoutMode;
            }
            set
            {
                camera.ReadoutMode = value;
            }
        }

        /// <summary>
        /// List of available readout modes, Interface Version 2 only
        /// </summary>
        /// <returns>An ArrayList of readout mode names</returns>
        static internal ArrayList ReadoutModes
        {
            get
            {
                return camera.ReadoutModes;
            }
        }

        /// <summary>
        /// Sensor name, Interface Version 2 and later
        /// </summary>
        /// <returns>The name of the sensor used within the camera.</returns>
        static internal string SensorName
        {
            get
            {
                return camera.SensorName;
            }
        }

        /// <summary>
        /// Type of colour information returned by the camera sensor, Interface Version 2 and later
        /// </summary>
        /// <value>The type of sensor used by the camera.</value>
        internal static SensorType SensorType
        {
            get
            {
                return (SensorType)camera.SensorType;
            }
        }

        /// <summary>
        /// Sets the camera cooler set point in degrees Celsius, and returns the current set point.
        /// </summary>
        /// <value>The set CCD temperature.</value>
        static internal double SetCCDTemperature
        {
            get
            {
                return camera.SetCCDTemperature;
            }
            set
            {
                camera.SetCCDTemperature = value;
            }
        }

        /// <summary>
        /// Starts an exposure. Use <see cref="ImageReady" /> to check when the exposure is complete.
        /// </summary>
        /// <param name="Duration">Duration of exposure in seconds, can be zero if <see cref="StartExposure">Light</see> is <c>false</c></param>
        /// <param name="Light"><c>true</c> for light frame, <c>false</c> for dark frame (ignored if no shutter)</param>
        static internal void StartExposure(double Duration, bool Light)
        {
            camera.StartExposure(Duration, Light);
        }

        /// <summary>
        /// Sets the subframe start position for the X axis (0 based) and returns the current value.
        /// </summary>
        static internal int StartX
        {
            get
            {
                return camera.StartX;
            }
            set
            {
                camera.StartX = value;
            }
        }

        /// <summary>
        /// Sets the subframe start position for the Y axis (0 based). Also returns the current value.
        /// </summary>
        static internal int StartY
        {
            get
            {
                return camera.StartY;
            }
            set
            {
                camera.StartY = value;
            }
        }

        /// <summary>
        /// Stops the current exposure, if any.
        /// </summary>
        static internal void StopExposure()
        {
            camera.StopExposure();
        }

        /// <summary>
        /// Camera's sub-exposure interval
        /// </summary>
        static internal double SubExposureDuration
        {
            get
            {
                return camera.SubExposureDuration;
            }
            set
            {
                camera.SubExposureDuration = value;
            }
        }

        #endregion

        #region Private properties and methods
        // Useful methods that can be used as required to help with driver development

        /// <summary>
        /// Release memory allocated to the large arrays on the large object heap.
        /// </summary>
        private static void ReleaseArrayMemory()
        {
            // Clear out any previous memory allocations
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(2, GCCollectionMode.Forced, true, true);
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private static void CheckConnected(string message)
        {
            if (!connectedState)
            {
                throw new NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal static void ReadProfile()
        {
            using (Profile hubProfile = new Profile())
            {
                hubProfile.DeviceType = "Camera";
                TL.Enabled = Convert.ToBoolean(hubProfile.GetValue(hubProgId, TRACE_STATE_PROFILE_NAME, string.Empty, TRACE_STATE_DEFAULT));
                cameraProgId = hubProfile.GetValue(hubProgId, CAMERA_PROGID, string.Empty, CAMERA_PROGID_DEFAULT);
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal static void WriteProfile()
        {
            using (Profile hubProfile = new Profile())
            {
                hubProfile.DeviceType = "Camera";
                hubProfile.WriteValue(hubProgId, TRACE_STATE_PROFILE_NAME, TL.Enabled.ToString());
                hubProfile.WriteValue(hubProgId, CAMERA_PROGID, cameraProgId);
            }
        }

        /// <summary>
        /// Log helper function that takes identifier and message strings
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        internal static void LogMessage(string identifier, string message)
        {
            TL.LogMessageCrLf(identifier, message);
        }

        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            LogMessage(identifier, msg);
        }
        #endregion
    }
}
