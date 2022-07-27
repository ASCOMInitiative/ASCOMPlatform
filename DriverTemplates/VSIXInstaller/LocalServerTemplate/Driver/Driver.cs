//tabs=4
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
    // Your driver's DeviceID is TEMPLATEDEVICEID
    //
    // The Guid attribute sets the CLSID for TEMPLATEDEVICEID
    // The ClassInterface/None attribute prevents an empty interface called
    // _TEMPLATEDEVICENAME from being created and used as the [default] interface
    //
    // TODO Replace the not implemented exceptions with code to implement the function or
    // throw the appropriate ASCOM exception.
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
        // Constants used for Profile persistence
        internal const string comPortProfileName = "COM Port";
        internal const string comPortDefault = "COM1";
        internal const string traceStateProfileName = "Trace Level";
        internal const string traceStateDefault = "true";

        internal static string driverID; // ASCOM DeviceID (COM ProgID) for this driver, the value is retrieved from the ServedClassName attribute in the class initialiser.
        internal static string driverDescription; // The value is retrieved from the ServedClassName attribute in the class initialiser.
        internal static string comPort; // Variable to hold the COM port if required
        internal static bool connectedState; // variable to hold the connected state
        internal static Util utilities; // Private variable to hold an ASCOM Utilities object
        internal static AstroUtils astroUtilities; // Variable to hold an ASCOM AstroUtilities object to provide the Range method
        internal static TraceLogger tl; // Variable to hold the trace logger object (creates a diagnostic log file with information that you specify)

        /// <summary>
        /// Initializes a new instance of the <see cref="TEMPLATEDEVICENAME"/> class. Must be public to successfully register for COM.
        /// </summary>
        public TEMPLATEDEVICECLASS()
        {
            try
            {
                // Pull the ProgID from the ProgID class attribute.
                Attribute attr = Attribute.GetCustomAttribute(this.GetType(), typeof(ProgIdAttribute));
                driverID = ((ProgIdAttribute)attr).Value ?? "PROGID NOT SET!";  // Get the driver ProgIDfrom the ProgID attribute.

                // Pull the display name from the ServedClassName class attribute.
                attr = Attribute.GetCustomAttribute(this.GetType(), typeof(ServedClassNameAttribute));
                driverDescription = ((ServedClassNameAttribute)attr).DisplayName ?? "DISPLAY NAME NOT SET!";  // Get the driver description that displays in the ASCOM Chooser from the ServedClassName attribute.

                tl = new TraceLogger("", "TEMPLATEDEVICEID");
                ReadProfile(); // Read device configuration from the ASCOM Profile store, including the trace state

                tl.LogMessage("TEMPLATEDEVICECLASS", "Starting initialisation");
                tl.LogMessage("TEMPLATEDEVICECLASS", $"ProgID: {driverID}, Description: {driverDescription}");

                connectedState = false; // Initialise connected to false
                utilities = new Util(); //Initialise util object
                astroUtilities = new AstroUtils(); // Initialise astro-utilities object

                //TODO: Implement your additional construction here

                tl.LogMessage("TEMPLATEDEVICECLASS", "Completed initialisation");
            }
            catch (Exception ex)
            {
                tl.LogMessageCrLf("TEMPLATEDEVICECLASS", $"Initialisation exception: {ex}");
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
            // consider only showing the setup dialogue if not connected
            // or call a different dialogue if connected
            if (IsConnected)
                MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm(tl))
            {
                var result = F.ShowDialog();
                if (result == DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

		/// <summary>Returns the list of custom action names supported by this driver.</summary>
		/// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
		public ArrayList SupportedActions
        {
            get
            {
                tl.LogMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList();
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
            LogMessage("", "Action {0}, parameters {1} not implemented", actionName, actionParameters);
            throw new ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
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
            CheckConnected("CommandBlind");
            // TODO The optional CommandBlind method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBlind must send the supplied command to the mount and return immediately without waiting for a response

            throw new MethodNotImplementedException("CommandBlind");
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
            CheckConnected("CommandBool");
            // TODO The optional CommandBool method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBool must send the supplied command to the mount, wait for a response and parse this to return a True or False value

            // string retString = CommandString(command, raw); // Send the command and wait for the response
            // bool retBool = XXXXXXXXXXXXX; // Parse the returned string and create a boolean True / False value
            // return retBool; // Return the boolean value to the client

            throw new MethodNotImplementedException("CommandBool");
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
            CheckConnected("CommandString");
            // TODO The optional CommandString method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandString must send the supplied command to the mount and wait for a response before returning this to the client

            throw new MethodNotImplementedException("CommandString");
        }

		/// <summary>
		/// Dispose the late-bound interface, if needed. Will release it via COM
		/// if it is a COM object, else if native .NET will just dereference it
		/// for GC.
		/// </summary>
		public void Dispose()
        {
            // Clean up the trace logger and util objects
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
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
                LogMessage("Connected", "Get {0}", IsConnected);
                return IsConnected;
            }
            set
            {
                tl.LogMessage("Connected", "Set {0}", value);
                if (value == IsConnected)
                    return;

                if (value)
                {
                    connectedState = true;
                    LogMessage("Connected Set", "Connecting to port {0}", comPort);
                    // TODO connect to the device
                }
                else
                {
                    connectedState = false;
                    LogMessage("Connected Set", "Disconnecting from port {0}", comPort);
                    // TODO disconnect from the device
                }
            }
        }

		/// <summary>
		/// Returns a description of the device, such as manufacturer and modelnumber. Any ASCII characters may be used.
		/// </summary>
		/// <value>The description.</value>
		public string Description
        {
            // TODO customise this device description
            get
            {
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

		/// <summary>
		/// Descriptive and version information about this ASCOM driver.
		/// </summary>
		public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // TODO customise this driver description
                string driverInfo = "Information about the driver itself. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverInfo Get", driverInfo);
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
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

		/// <summary>
		/// The interface version number that this device supports.
		/// </summary>
		public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "TEMPLATEINTERFACEVERSION");
                return Convert.ToInt16("TEMPLATEINTERFACEVERSION");
            }
        }

		/// <summary>
		/// The short name of the driver, for display purposes
		/// </summary>
		public string Name
        {
            get
            {
                string name = "Short driver name - please customise";
                tl.LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        //INTERFACECODEINSERTIONPOINT
        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "TEMPLATEDEVICECLASS";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));
                comPort = driverProfile.GetValue(driverID, comPortProfileName, string.Empty, comPortDefault);
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "TEMPLATEDEVICECLASS";
                driverProfile.WriteValue(driverID, traceStateProfileName, tl.Enabled.ToString());
                driverProfile.WriteValue(driverID, comPortProfileName, comPort.ToString());
            }
        }

        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            tl.LogMessage(identifier, msg);
        }
        #endregion
    }
}
