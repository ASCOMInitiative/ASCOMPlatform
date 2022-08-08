// TODO fill in this information for your driver, then remove this line!
//
// ASCOM TEMPLATEDEVICECLASS driver for TEMPLATEDEVICENAME
//
// Description:	 <To be completed by driver developer>
//
// Implements:	ASCOM TEMPLATEDEVICECLASS interface version: <To be completed by driver developer>
// Author:		(XXX) Your N. Here <your@email.here>
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using ASCOM.LocalServer;
using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Astrometry.NOVAS;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;
using System.Windows.Forms;

namespace TEMPLATENAMESPACE
{
    //
    // This code is mostly a presentation layer for the functionality in the TEMPLATEHARDWARECLASS class. You should not need to change the contents of this file very much, if at all.
    // Most customisation will be in the TEMPLATEHARDWARECLASS class, which is shared by all instances of the driver, and which must handle all aspects of communicating with your device.
    //
    // Your driver's DeviceID is TEMPLATEDEVICEID
    //
    // The COM Guid attribute sets the CLSID for TEMPLATEDEVICEID
    // The COM ClassInterface/None attribute prevents an empty interface called _TEMPLATEDEVICENAME from being created and used as the [default] interface
    //

    /// <summary>
    /// ASCOM TEMPLATEDEVICECLASS Driver for TEMPLATEDEVICENAME.
    /// </summary>
    [ComVisible(true)]
    [Guid("3A02C211-FA08-4747-B0BD-4B00EB159297")]
    [ProgId("TEMPLATEDEVICEID")]
    [ServedClassName("ASCOM TEMPLATEDEVICECLASS Driver for TEMPLATEDEVICENAME")] // Driver description that appears in the Chooser, customise as required
    [ClassInterface(ClassInterfaceType.None)]
    public class TEMPLATEDEVICECLASS : ReferenceCountedObjectBase, ITEMPLATEDEVICEINTERFACE
    {
        internal static string DriverProgId; // ASCOM DeviceID (COM ProgID) for this driver, the value is retrieved from the ServedClassName attribute in the class initialiser.
        internal static string DriverDescription; // The value is retrieved from the ServedClassName attribute in the class initialiser.

        // connectedState holds the connection state from this driver's perspective, as opposed to the local server's perspective, which may be different because of other client connections.
        internal bool connectedState; // The connected state from this driver's perspective)
        internal TraceLogger tl; // Trace logger object to hold diagnostic information just for this instance of the driver, as opposed to the local server's log, which includes activity from all driver instances.

        /// <summary>
        /// Initializes a new instance of the <see cref="TEMPLATEDEVICENAME"/> class. Must be public to successfully register for COM.
        /// </summary>
        public TEMPLATEDEVICECLASS()
        {
            try
            {
                // Pull the ProgID from the ProgID class attribute.
                Attribute attr = Attribute.GetCustomAttribute(this.GetType(), typeof(ProgIdAttribute));
                DriverProgId = ((ProgIdAttribute)attr).Value ?? "PROGID NOT SET!";  // Get the driver ProgIDfrom the ProgID attribute.

                // Pull the display name from the ServedClassName class attribute.
                attr = Attribute.GetCustomAttribute(this.GetType(), typeof(ServedClassNameAttribute));
                DriverDescription = ((ServedClassNameAttribute)attr).DisplayName ?? "DISPLAY NAME NOT SET!";  // Get the driver description that displays in the ASCOM Chooser from the ServedClassName attribute.

                // LOGGING CONFIGURATION
                // By default all driver logging will appear in Hardware log file
                // If you would like each instance of the driver to have its own log file as well, uncomment the lines below

                tl = new TraceLogger("", "TEMPLATEDEVICENAME.Driver"); // Remove the leading ASCOM. from the ProgId because this will be added back by TraceLogger.
                SetTraceState();

                LogMessage("TEMPLATEDEVICECLASS", "Starting driver initialisation");
                LogMessage("TEMPLATEDEVICECLASS", $"ProgID: {DriverProgId}, Description: {DriverDescription}");

                connectedState = false; // Initialise connected to false

                // Initialise the hardware if required
                TEMPLATEHARDWARECLASS.InitialiseHardware();

                LogMessage("TEMPLATEDEVICECLASS", "Completed initialisation");
            }
            catch (Exception ex)
            {
                LogMessage("TEMPLATEDEVICECLASS", $"Initialisation exception: {ex}");
                MessageBox.Show($"{ex.Message}", "Exception creating TEMPLATEDEVICEID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // PUBLIC COM INTERFACE ITEMPLATEDEVICEINTERFACE IMPLEMENTATION

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialogue form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            if (connectedState) // Don't show if already connected
            {
                MessageBox.Show("Already connected, just press OK");
            }
            else // Show dialogue
            {
                TEMPLATEHARDWARECLASS.SetupDialog();
            }
        }

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        public ArrayList SupportedActions
        {
            get
            {
                ArrayList actions = TEMPLATEHARDWARECLASS.SupportedActions;
                LogMessage("SupportedActions Get", $"Returning {actions.Count} actions.");
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
        public string Action(string actionName, string actionParameters)
        {
            CheckConnected($"Action {actionName} - {actionParameters}");
            LogMessage("", $"Calling Action: {actionName} with parameters: {actionParameters}");
            return TEMPLATEHARDWARECLASS.Action(actionName, actionParameters);
        }

        //STARTOFCOMMANDXXXMETHODS - This line will be deleted by the template wizard.
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
            CheckConnected($"CommandBlind: {command}, Raw: {raw}");
            TEMPLATEHARDWARECLASS.CommandBlind(command, raw);
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
            CheckConnected($"CommandBool: {command}, Raw: {raw}");
            return TEMPLATEHARDWARECLASS.CommandBool(command, raw);
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
            CheckConnected($"CommandString: {command}, Raw: {raw}");
            return TEMPLATEHARDWARECLASS.CommandString(command, raw);
        }

        //ENDOFCOMMANDXXXMETHODS - This line will be deleted by the template wizard.
        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
        /// if it is a COM object, else if native .NET will just dereference it
        /// for GC.
        /// </summary>
        public void Dispose()
        {
            // Clean up the trace logger object
            if (!(tl is null))
            {
                tl.Enabled = false;
                tl.Dispose();
                tl = null;
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
                // Returns the driver connection state rather than the local server's connected state, which could be different because there may be other client connections still active.
                LogMessage("Connected Get", connectedState.ToString());
                return connectedState;
            }
            set
            {
                if (value == connectedState)
                {
                    LogMessage("Connected Set", "Device already connected, ignoring Connected Set = true");
                    return;
                }

                if (value)
                {
                    connectedState = true;
                    LogMessage("Connected Set", "Connecting to device");
                    TEMPLATEHARDWARECLASS.Connected = true;
                }
                else
                {
                    connectedState = false;
                    LogMessage("Connected Set", "Disconnecting from device");
                    TEMPLATEHARDWARECLASS.Connected = false;
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
                CheckConnected($"Description Get");
                string description = DriverDescription;
                LogMessage("Description Get", description);
                return description;
            }
        }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        public string DriverInfo
        {
            get
            {
                string driverInfo = TEMPLATEHARDWARECLASS.DriverInfo;
                LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        /// <summary>
        /// A string containing only the major and minor version of the driver formatted as 'm.n'.
        /// </summary>
        public string DriverVersion
        {
            get
            {
                string driverVersion = TEMPLATEHARDWARECLASS.DriverVersion;
                LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        /// <summary>
        /// The interface version number that this device supports.
        /// </summary>
        public short InterfaceVersion
        {
            get
            {
                short interfaceVersion = TEMPLATEHARDWARECLASS.InterfaceVersion;
                LogMessage("InterfaceVersion Get", interfaceVersion.ToString());
                return interfaceVersion;
            }
        }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        public string Name
        {
            get
            {
                string name = TEMPLATEHARDWARECLASS.Name;
                LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        //INTERFACECODEINSERTIONPOINT
        #region Private properties and methods
        // Useful properties and methods that can be used as required to help with driver development

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!connectedState)
            {
                throw new NotConnectedException($"{DriverDescription} ({DriverProgId}) is not connected: {message}");
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
            TEMPLATEHARDWARECLASS.LogMessage(identifier, message); // Write to the local server logger
        }

        /// <summary>
        /// Read the trace state from the driver's Profile and enable / disable the trace log accordingly.
        /// </summary>
        private void SetTraceState()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "TEMPLATEDEVICECLASS";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(DriverProgId, TEMPLATEHARDWARECLASS.traceStateProfileName, string.Empty, TEMPLATEHARDWARECLASS.traceStateDefault));
            }
        }

        #endregion
    }
}
