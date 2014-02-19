//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Switch driver for Simulator
//
// Description:	ASCOM Switch V2 Simulator.
//
// Implements:	ASCOM Switch interface version: 2
// Author:		(CDR) Chris Rowland <chris.rowland@cherryfield.me.uk>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 25-Sep-2013	CDR	6.0.0	Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.Diagnostics;

namespace ASCOM.Simulator
{
    //
    // Your driver's DeviceID is ASCOM.Simulator.Switch
    //
    // The Guid attribute sets the CLSID for ASCOM.Simulator.Switch
    // The ClassInterface/None addribute prevents an empty interface called
    // _Simulator from being created and used as the [default] interface
    //

    /// <summary>
    /// ASCOM Switch Driver for Simulator.
    /// </summary>
    [Guid("602b2780-d8fe-438b-a11a-e45a8df6e7c8")]
    [ProgId("ASCOM.Simulator.Switch")]
    [ServedClassName("ASCOM SwitchV2 Simulator Driver")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Switch : ReferenceCountedObjectBase, ISwitchV2
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.Simulator.Switch";
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "ASCOM SwitchV2 Simulator Driver.";

        internal static string traceStateProfileName = "Trace Level";
        internal static string traceStateDefault = "false";

        internal static bool traceState;

        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        private bool connectedState;

        /// <summary>
        /// Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        private TraceLogger tl;

        /// <summary>
        /// Initializes a new instance of the <see cref="Simulator"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Switch()
        {
            driverID = Marshal.GenerateProgIdForType(this.GetType());
            ReadProfile(); // Read device configuration from the ASCOM Profile store
            tl = new TraceLogger("", "Simulator");
            tl.Enabled = traceState;
            tl.LogMessage("Switch", "Starting initialisation");

            connectedState = false; // Initialise connected to false
            //TODO: Implement your additional construction here

            tl.LogMessage("Switch", "Completed initialisation");
        }

        //
        // PUBLIC COM INTERFACE ISwitchV2 IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            if (IsConnected)
                System.Windows.Forms.MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm())
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                tl.LogMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public void CommandBlind(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            // Clean up the tracelogger and util objects
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
        }

        public bool Connected
        {
            get
            {
                tl.LogMessage("Connected Get", IsConnected.ToString());
                return IsConnected;
            }
            set
            {
                tl.LogMessage("Connected Set", value.ToString());
                if (value == IsConnected)
                    return;

                if (value)
                {
                    connectedState = true;
                    // TODO connect to the device
                }
                else
                {
                    connectedState = false;
                    // TODO disconnect from the device
                }
            }
        }

        public string Description
        {
            // TODO customise this device description
            get
            {
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                FileVersionInfo FV = Process.GetCurrentProcess().MainModule.FileVersionInfo; //Get the name of the executable without path or file extension
                string driverInfo = "Switch V2 Simulator, version: " + FV.FileVersion;
                tl.LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                FileVersionInfo FV = Process.GetCurrentProcess().MainModule.FileVersionInfo; //Get the name of the executable without path or file extension
                string driverVersion = FV.FileMajorPart.ToString() + "." + FV.FileMinorPart.ToString();
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                tl.LogMessage("InterfaceVersion Get", "2");
                return Convert.ToInt16("2");
            }
        }

        public string Name
        {
            get
            {
                string name = "ASCOM Switch V2 Simulator";
                tl.LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region ISwitchV2 Implementation

        /// <summary>
        /// list of switches used for simulation
        /// </summary>
        internal static List<LocalSwitch> switches;

        /// <summary>
        /// The number of switches managed by this driver
        /// </summary>
        public short MaxSwitch
        {
            get
            {
                CheckConnected("MaxSwitch");
                tl.LogMessage("MaxSwitch Get", switches.Count.ToString());
                return (short)switches.Count;
            }
        }

        /// <summary>
        /// Return the name of switch n
        /// </summary>
        /// <param name="id">The switch number to return</param>
        /// <returns>
        /// The name of the switch
        /// </returns>
        public string GetSwitchName(short id)
        {
            Validate("GetSwitchName", id);
            return switches[id].Name;
        }

        /// <summary>
        /// Sets a switch name to a specified value
        /// </summary>
        /// <param name="id">The number of the switch whose name is to be set</param>
        /// <param name="name">The name of the switch</param>
        public void SetSwitchName(short id, string name)
        {
            // not sure if this should be set or not
            //throw new MethodNotImplementedException("SetSwitchName");
            Validate("SetSwitchName", id);
            switches[id].Name = name;
            using (Profile p = new Profile { DeviceType = "Switch" })
            {
                switches[id].Save(p, driverID, id);
            }
        }

        /// <summary>
        /// Gets the description of the specified switch. This is to allow a fuller description of
        /// the switch to be returned, for example for a tool tip.
        /// </summary>
        /// <param name="id">The number of the switch whose description is to be returned</param>
        /// <returns></returns>
        /// <exception cref="T:ASCOM.MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
        public string GetSwitchDescription(short id)
        {
            Validate("GetSwitchDescription", id);
            return switches[id].Description;
        }

        public bool CanWrite(short id)
        {
            Validate("CanWrite", id);
            return switches[id].CanWrite;
        }

        #region boolean switch members

        /// <summary>
        /// Return the state of switch n
        /// an analogue switch will throw a notImplemented exception
        /// </summary>
        /// <param name="id">The switch number to return</param>
        /// <returns>
        /// True or false
        /// </returns>
        public bool GetSwitch(short id)
        {
            Validate("GetSwitch", id, true);
            LocalSwitch sw = switches[id];
            if (!sw.CanRead)
            {
                throw new MethodNotImplementedException(string.Format("device {0} cannot be read and", id));
            }
            // returns true if the value is closer to the maximum than the minimum
            return sw.Maximum - sw.Value <= sw.Value - sw.Minimum;
        }

        /// <summary>
        /// Sets a switch to the specified state
        /// If the switch cannot be set then throws a MethodNotImplementedException.
        /// An analogue switch will throw a not implemented exception
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        public void SetSwitch(short id, bool state)
        {
            Validate("SetSwitch", id, true);
            LocalSwitch sw = switches[id];
            sw.SetValue(state ? sw.Maximum : sw.Minimum, "SetSwitch");
        }

        #endregion boolean switch members

        #region analogue switch members

        /// <summary>
        /// returns the maximum value for this switch
        /// boolean switches must return 1.0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double MaxSwitchValue(short id)
        {
            Validate("MaxSwitchValue", id);
            return switches[id].Maximum;
        }

        /// <summary>
        /// returns the minimum value for this switch
        /// boolean switches must return 0.0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double MinSwitchValue(short id)
        {
            Validate("MinSwitchValue", id);
            return switches[id].Minimum;
        }

        /// <summary>
        /// returns the step size that this switch supports. This gives the difference between
        /// successive values of the switch.
        /// The number of values is ((MaxSwitchValue - MinSwitchValue) / SwitchStep) + 1
        /// boolean switches must return 1.0, giving two states.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double SwitchStep(short id)
        {
            Validate("SwitchStep", id);
            return switches[id].StepSize;
        }

        /// <summary>
        /// returns the analogue switch value for switch id
        /// boolean switches will return 1.0 or 0.0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double GetSwitchValue(short id)
        {
            Validate("GetSwitchValue", id, false);
            if (!switches[id].CanRead)
            {
                throw new MethodNotImplementedException(string.Format("device {0} cannot be read and", id));
            }
            return switches[id].Value;
        }

        /// <summary>
        /// set the analogue value for this switch.
        /// If the switch cannot be set then throws a MethodNotImplementedException.
        /// If the value is not between the maximum and minimum then throws an InvalidValueException
        /// boolean switches will be set to true if the value is closer to the maximum than the minimum.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void SetSwitchValue(short id, double value)
        {
            Validate("SetSwitchValue", id, value);
            switches[id].SetValue(value, "SetSwitchValue");
        }

        #endregion analogue members
        #endregion ISwitchV2 Implementation

        #region Private properties and methods

        /// <summary>
        /// Checks that we are connected and the switch id is in range and throws an InvalidValueException if it isn't
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="id">The id.</param>
        private void Validate(string message, short id)
        {
            CheckConnected(message);
            if (id < 0 || id >= switches.Count)
            {
                tl.LogMessage(message, string.Format("Switch {0} not available, range is 0 to {1}", id, switches.Count - 1));
                throw new InvalidValueException(message, id.ToString(), string.Format("0 to {0}", switches.Count - 1));
            }
        }

        /// <summary>
        /// Check we are connected, that the switch id is valid and that the number of states is valid: 
        /// 2 for a boolean and >2 for a multi-value switch
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <param name="expectBoolean"></param>
        private void Validate(string message, short id, bool expectBoolean)
        {
            Validate(message, id);
            var ns = NumStates(id);
            if (ns < 2)
            {
                tl.LogMessage(message, string.Format("Switch {0} has the wrong number of states: {1}", id, ns));
                throw new InvalidValueException(message, id.ToString(), string.Format("{0} states", ns));
            }
            if ((expectBoolean && ns != 2))
            {
                tl.LogMessage(message, string.Format("Boolean Switch {0} has the wrong number of states: {1}", id, ns));
                throw new MethodNotImplementedException(string.Format("{0}({1}): switch is not Boolean and", message, id));
            }
            if (!expectBoolean && ns <= 2)
            {
                tl.LogMessage(message, string.Format("Multi-value Switch {0} has too few states {1}", id, ns));
                throw new MethodNotImplementedException((string.Format("{0}({1}): switch is not multi-value and", message, id)));
            }
        }

        /// <summary>
        /// Check we are connected, the switch id is valid, that the switch is a multi-value switch and that the value is in range.
        /// Throw exceptions if this is incorrect
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <param name="value"></param>
        private void Validate(string message, short id, double value)
        {
            Validate(message, id, false);
            var sw = switches[id];
            if (value < sw.Minimum || value > sw.Maximum)
            {
                tl.LogMessage(message, string.Format("Switch {0} value {1} out of range {2} to {3}", id, value, sw.Minimum, sw.Maximum));
                throw new InvalidValueException(message, id.ToString(), (string.Format("{0} to {1}", sw.Minimum, sw.Maximum)));
            }
        }

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                // simulator has no hardware
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
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Returns the number of states the switch can have, boolean switches
        /// must have 2, multi-value switches more than 2
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int NumStates(short id)
        {
            var s = switches[id];
            return (int)((s.Maximum - s.Minimum) / s.StepSize + 1);
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            using (Profile driverProfile = new Profile() { DeviceType = "Switch" })
            {
                traceState = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));

                switches = new List<LocalSwitch>();
                int numSwitch;
                if (int.TryParse(driverProfile.GetValue(driverID, "NumSwitches"), out numSwitch))
                {
                    for (short i = 0; i < numSwitch; i++)
                    {
                        switches.Add(new LocalSwitch(driverProfile, driverID, i));
                    }
                }
                else
                {
                    LoadDefaultSwitches();
                }
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            using (Profile driverProfile = new Profile() { DeviceType = "Switch" })
            {
                driverProfile.WriteValue(driverID, traceStateProfileName, traceState.ToString());
                driverProfile.WriteValue(driverID, "NumSwitches", switches.Count.ToString());
                int i = 0;
                foreach (var item in switches)
                {
                    item.Save(driverProfile, driverID, i++);
                }
            }
        }

        /// <summary>
        /// Loads a default set of switches.
        /// </summary>
        private void LoadDefaultSwitches()
        {
            switches.Add(new LocalSwitch("Power1") { Description = "Generic power switch" });
            switches.Add(new LocalSwitch("Power2") { Description = "Generic Power switch", CanRead = false });
            switches.Add(new LocalSwitch("Light Box", 100, 0, 10, 0) { Description = "Light box , 0 to 100%" });
            switches.Add(new LocalSwitch("Flat Panel", 255, 0, 1, 0) { Description = "Flat panel , 0 to 255" });
            switches.Add(new LocalSwitch("Scope Cover") { Description = "Scope cover control true is closed, false is open" });
            switches.Add(new LocalSwitch("Scope Parked") { Description = "Scope parked switch, true if parked", CanWrite = false });
            switches.Add(new LocalSwitch("Cloudy", 2, 0, 1, 0, false) { Description = "Cloud monitor: 0=clear, 1=light cloud, 2= heavy cloud" });
            switches.Add(new LocalSwitch("Temperature", 30, -20, 0.1, 12, false) { Description = "Temperature in deg C" });
            switches.Add(new LocalSwitch("Humidity", 100, 0, 1, 50, false) { Description = "Relative humidity %" });
            switches.Add(new LocalSwitch("Raining") { Description = "Rain monitor, true if raining", CanWrite = false });
        }

        #endregion

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        //private static void RegUnregASCOM(bool bRegister)
        //{
        //    using (var P = new ASCOM.Utilities.Profile())
        //    {
        //        P.DeviceType = "Switch";
        //        if (bRegister)
        //        {
        //            P.Register(driverID, driverDescription);
        //        }
        //        else
        //        {
        //            P.Unregister(driverID);
        //        }
        //    }
        //}

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
        //[ComRegisterFunction]
        //public static void RegisterASCOM(Type t)
        //{
        //    RegUnregASCOM(true);
        //}

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
        //[ComUnregisterFunction]
        //public static void UnregisterASCOM(Type t)
        //{
        //    RegUnregASCOM(false);
        //}

        #endregion
    }
}
