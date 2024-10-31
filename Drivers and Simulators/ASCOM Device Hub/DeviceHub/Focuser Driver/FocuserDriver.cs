//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Focuser driver for DeviceHub
//
// Description:	This class implements the Focuser object that is exposed by the
//				ASCOM Device Hub application. The properties get values that are
//				cached by the Device Hub Focuser Manager and set values that update the
//				cache and forward them to the true driver. Focuser command methods
//				make calls through the Focuser Manager
//
// Implements:	ASCOM Focuser interface version: 3
// Author:		(RDB) Rick Burke <astroman133@gmail.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 11-Dec-2018	RDB	6.4.0	Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//

using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using ASCOM.DeviceInterface;
using ASCOM.Utilities;

using static System.FormattableString;

namespace ASCOM.DeviceHub
{
    // Your driver's DeviceID is ASCOM.DeviceHub.Focuser

    // The Guid attribute sets the CLSID for ASCOM.DeviceHub.Focuser
    // The ClassInterface/None attribute prevents an empty interface called
    // _DeviceHub from being created and used as the [default] interface

    /// <summary>
    /// ASCOM Focuser Driver for DeviceHub.
    /// </summary>
    [ComVisible(true)]
    [Guid("5eb66c80-1658-4dde-8931-4d5772fa2311")]
    [ProgId("ASCOM.DeviceHub.Focuser")]
    [ServedClassName("Device Hub Focuser")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : DeviceDriverBase, IFocuserV4
    {
        #region Private Fields and Properties

        private FocuserManager FocuserManager => FocuserManager.Instance;

        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        private bool ConnectedState { get; set; }

        private Util Utilities { get; set; }

        // Fields to support the Platform 7 Connect / Disconnect protocol
        private bool connecting = false;
        private Exception connectException = null;
        private readonly object connectedLock = new object();

        #endregion Private Fields and Properties

        #region Instance Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceHub"/> class.
        /// </summary>
        public Focuser()
        {
            _driverID = Marshal.GenerateProgIdForType(this.GetType());
            _driverDescription = GetDriverDescriptionFromAttribute();

            _logger = new TraceLogger("", "DeviceHubFocuser");
            ReadProfile(); // Read device configuration from the ASCOM Profile store

            _logger.LogMessage("Focuser", "Starting initialization");

            ConnectedState = false; // Initialise connected to false
            Utilities = new Util(); //Initialise util object

            // We need this to get the focuser ID.

            AppSettingsManager.LoadAppSettings();
            _logger.LogMessage("Focuser", "Completed initialization");
        }

        #endregion Instance Constructor

        //
        // PUBLIC COM INTERFACE IFocuserV3 IMPLEMENTATION
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
            if (Server.FocusersInUse > 1 || FocuserManager.Instance.IsConnected)
            {
                System.Windows.MessageBox.Show("Unable to change Focuser Properties at this time.");

                return;
            }

            if (ConnectedState)
            {
                System.Windows.MessageBox.Show("Already connected, just press OK");

                return;
            }

            // Launch the setup dialog on the U/I thread.

            Task taskShow = new Task(() =>
            {
                FocuserDriverSetupDialogViewModel vm = new FocuserDriverSetupDialogViewModel();
                FocuserDriverSetupDialogView view = new FocuserDriverSetupDialogView { DataContext = vm };
                vm.RequestClose += view.OnRequestClose;
                vm.InitializeCurrentFocuser(FocuserManager.FocuserID, FocuserManager.FastPollingPeriod);
                vm.IsLoggingEnabled = _logger.Enabled;
                view.ContentRendered += (sender, eventArgs) => view.Activate();

                bool? result = view.ShowDialog();

                if (result.HasValue && result.Value)
                {
                    _logger.Enabled = vm.IsLoggingEnabled;
                    FocuserManager.FocuserID = vm.FocuserSetupVm.FocuserID;
                    FocuserManager.FastPollingPeriod = vm.FocuserSetupVm.FastUpdatePeriod;
                    SaveProfile();
                    AppSettingsManager.SaveAppSettings();

                    view.DataContext = null;
                    view = null;

                    vm.Dispose();
                    vm = null;
                }
            });

            taskShow.Start(Globals.UISyncContext);

            // Wait for the task to be completed and the profile to be updated.

            taskShow.Wait();
        }

        public ArrayList SupportedActions
        {
            get
            {
                ArrayList retval;
                string msg = "";

                try
                {
                    retval = FocuserManager.SupportedActions;
                    msg += $"Returning list from driver{_done}";
                }
                catch (Exception)
                {
                    msg += _failed;

                    throw;
                }
                finally
                {
                    LogMessage("Get SupportedActions:", msg);
                }

                return retval;
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            string retval;

            if (String.IsNullOrEmpty(actionName))
            {
                throw new InvalidValueException("Action method: no actionName was provided.");
            }

            string msg = $"Action {actionName}, parameters {actionParameters}";

            try
            {
                retval = FocuserManager.Action(actionName, actionParameters);
                msg += $", returned {retval}{_done}";
            }
            catch (Exception)
            {
                msg += _failed;

                throw;
            }
            finally
            {
                LogMessage("Action:", msg);
            }

            return retval;
        }

        public void CommandBlind(string command, bool raw)
        {
            string msg = $"Command {command}, Raw {raw}";

            try
            {
                FocuserManager.CommandBlind(command, raw);
                msg += _done;
            }
            catch (Exception)
            {
                msg += _failed;

                throw;
            }
            finally
            {
                LogMessage("CommandBlind:", msg);
            }
        }

        public bool CommandBool(string command, bool raw)
        {
            bool retval;
            string msg = $"Command {command}, Raw {raw}";

            try
            {
                retval = FocuserManager.CommandBool(command, raw);
                msg += $", Returned {retval}{_done}.";
            }
            catch (Exception)
            {
                msg += _failed;

                throw;
            }
            finally
            {
                LogMessage("CommandBool:", msg);
            }

            return retval;
        }

        public string CommandString(string command, bool raw)
        {
            string retval;
            string msg = $"Command {command}, Raw {raw}";

            try
            {
                retval = FocuserManager.CommandString(command, raw);
                msg += $", Returned {retval}{_done}.";
            }
            catch (Exception)
            {
                msg += _failed;

                throw;
            }
            finally
            {
                LogMessage("CommandString:", msg);
            }

            return retval;
        }

        public void Dispose()
        {
            // Clean up the trace logger and util objects
            _logger.Enabled = false;
            _logger.Dispose();
            _logger = null;
            Utilities.Dispose();
            Utilities = null;
        }

        public bool Connected
        {
            get
            {
                LogMessage("Get Connected: ", ConnectedState.ToString());

                return ConnectedState;
            }
            set
            {
                lock (connectedLock)
                {
                    string msg = $"Setting Connected to {value}";
                    LogMessage("Set Connected:", $"Setting Connected to :{value}, current state: {ConnectedState}");

                    try
                    {
                        if (value == ConnectedState) // Short circuit since we are already connected.
                        {
                            msg += " (no change)";
                            return;
                        }

                        // Determine whether we are connecting or disconnecting
                        if (value) // Connecting
                        {
                            msg += $" (connecting to {FocuserManager.FocuserID})";

                            // Do this on the U/I thread.

                            Task task = new Task(() =>
                            {
                                ConnectedState = FocuserManager.ConnectFocuser();
                            });

                            task.Start(Globals.UISyncContext);
                            task.Wait();
                        }
                        else // Disconnecting
                        {
                            ConnectedState = false;
                            LogMessage("Set Connected:", $"Calling DisconnectFocuserIF()");
                            Server.DisconnectFocuserIf();
                            LogMessage("Set Connected:", $"DisconnectFocuserIF() complete.");
                            msg += $" (disconnecting from {FocuserManager.FocuserID}){_done}";
                        }
                    }
                    catch (Exception)
                    {
                        msg += _failed;
                        throw;
                    }
                    finally
                    {
                        LogMessage("Set Connected:", msg);
                    }
                }
            }
        }

        public string Description
        {
            get
            {
                LogMessage("Description Get", _driverDescription);

                return _driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverInfo = Invariant($"DeviceHub focuser driver. Version: {version.Major}.{version.Minor}");

                LogMessage("Get DriverInfo:", driverInfo);

                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = Invariant($"{version.Major}.{version.Minor}");

                LogMessage("Get DriverVersion:", driverVersion);

                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // Set by the driver wizard

            get
            {
                short version = 4;

                LogMessage("Get InterfaceVersion:", version.ToString());

                return version;
            }
        }

        public string Name
        {
            get
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                var attribute = (AssemblyProductAttribute)assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false).FirstOrDefault();
                string name = attribute.Product;

                string downstreamName = FocuserManager.Name;

                if (!String.IsNullOrEmpty(downstreamName))
                {
                    name += $" -> {downstreamName}";
                }

                LogMessage("Get Name", name);

                return name;
            }
        }

        #endregion  Common properties and methods.

        #region IFocuser Implementation

        public bool Absolute
        {
            get
            {
                var retval = false;
                string msg = "";

                try
                {
                    Exception xcp = FocuserManager.Parameters.GetException();

                    if (xcp != null)
                    {
                        throw xcp;
                    }

                    retval = FocuserManager.Parameters.Absolute;
                    msg += $"{retval}{_done}";
                }
                catch (Exception)
                {
                    msg += _failed;

                    throw;
                }
                finally
                {
                    LogMessage("Get Absolute:", msg);
                }

                return retval;
            }
        }

        public void Halt()
        {
            string name = "Halt:";

            CheckConnected(name, ConnectedState);

            string msg = "";

            try
            {
                FocuserManager.Halt();

                msg = _done;
            }
            catch (Exception)
            {
                msg = _failed;

                throw;
            }
            finally
            {
                LogMessage(name, msg);
            }
        }

        public bool IsMoving
        {
            get
            {
                string name = "Get IsMoving:";

                CheckConnected(name, ConnectedState);

                var retval = false;
                string msg = "";

                try
                {
                    retval = FocuserManager.Service.IsMoving;
                    msg += $"{retval}{_done}";
                }
                catch (Exception ex)
                {
                    msg += $"{_failed} - {ex.Message}";
                    throw;
                }
                finally
                {
                    LogMessage(name, msg);
                }

                return retval;
            }
        }

        public bool Link
        {
            get
            {
                LogMessage("Get Link: ", ConnectedState.ToString());

                return ConnectedState;
            }
            set
            {
                string msg = $"Setting Link to {value}";

                try
                {
                    if (value == ConnectedState) // Short circuit since we are already connected.
                    {
                        msg += " (no change)";

                        return;
                    }

                    if (value)
                    {
                        msg += $" (connecting to {FocuserManager.FocuserID})";

                        // Do this on the U/I thread.

                        Task task = new Task(() =>
                        {
                            ConnectedState = FocuserManager.ConnectFocuser();
                        });

                        task.Start(Globals.UISyncContext);
                        task.Wait();
                    }
                    else
                    {
                        FocuserManager.DisconnectFocuser();
                        ConnectedState = false;
                        LogMessage("Set Link:", $"Calling DisconnectFocuserIF()");
                        Server.DisconnectFocuserIf();
                        LogMessage("Set Link:", $"DisconnectFocuserIF() complete.");

                        msg += $" (disconnecting from {FocuserManager.FocuserID}){_done}";
                    }
                }
                catch (Exception)
                {
                    msg += _failed;

                    throw;
                }
                finally
                {
                    LogMessage("Set Connected:", msg);
                }
            }
        }

        public int MaxIncrement
        {
            get
            {
                var retval = Int32.MinValue;
                string msg = "";

                try
                {
                    Exception xcp = FocuserManager.Parameters.GetException();

                    if (xcp != null)
                    {
                        throw xcp;
                    }

                    retval = FocuserManager.Parameters.MaxIncrement;
                    msg += $"{retval}{_done}";
                }
                catch (Exception)
                {
                    msg += _failed;

                    throw;
                }
                finally
                {
                    LogMessage("Get MaxIncrement:", msg);
                }

                return retval;

            }
        }

        public int MaxStep
        {
            get
            {
                var retval = Int32.MinValue;
                string msg = "";

                try
                {
                    Exception xcp = FocuserManager.Parameters.GetException();

                    if (xcp != null)
                    {
                        throw xcp;
                    }

                    retval = FocuserManager.Parameters.MaxStep;
                    msg += $"{retval}{_done}";
                }
                catch (Exception)
                {
                    msg += _failed;

                    throw;
                }
                finally
                {
                    LogMessage("Get MaxStep:", msg);
                }

                return retval;
            }
        }

        public void Move(int newPosition)
        {
            string name = "Move:";

            CheckConnected(name, ConnectedState);

            string msg = $"Position = {newPosition}";

            try
            {
                int amount = newPosition;

                if (FocuserManager.Parameters.Absolute)
                {
                    amount = newPosition - Position;
                }

                FocuserManager.MoveFocuserBy(amount);
                msg += _done;
            }
            catch (Exception)
            {
                msg += _failed;

                throw;
            }
            finally
            {
                LogMessage(name, msg);
            }
        }

        public int Position
        {
            get
            {
                string name = "Get Position:";

                CheckConnected(name, ConnectedState);
                CheckAbsolute(name, FocuserManager.Parameters.Absolute, false);

                var retval = Int32.MinValue;
                string msg = "";

                try
                {
                    retval = FocuserManager.Service.Position;
                    msg += $"{retval}{_done}";
                }
                catch (Exception ex)
                {
                    msg += $"{_failed} - {ex.Message}";
                    throw;
                }
                finally
                {
                    LogMessage(name, msg);
                }

                return retval;
            }
        }

        public double StepSize
        {
            get
            {
                var retval = Double.NaN;
                string msg = "";

                try
                {
                    Exception xcp = FocuserManager.Parameters.GetException();

                    if (xcp != null)
                    {
                        throw xcp;
                    }

                    retval = FocuserManager.Parameters.StepSize;
                    msg += $"{retval}{_done}";
                }
                catch (Exception)
                {
                    msg += _failed;

                    throw;
                }
                finally
                {
                    LogMessage("Get StepSize:", msg);
                }

                return retval;
            }
        }

        public bool TempComp
        {
            get
            {
                string name = "Get TempComp:";

                CheckConnected(name, ConnectedState);

                var retval = false;
                string msg = "";

                try
                {
                    Exception xcp = FocuserManager.Status.GetException();

                    if (xcp != null)
                    {
                        throw xcp;
                    }

                    retval = FocuserManager.Status.TempComp;
                    msg += $"{retval}{_done}";
                }
                catch (Exception)
                {
                    msg += _failed;

                    throw;
                }
                finally
                {
                    LogMessage(name, msg);
                }

                return retval;
            }
            set
            {
                string name = "Set TempComp:";

                CheckConnected(name, ConnectedState);
                CheckTempCompAvailable(name, FocuserManager.Parameters.TempCompAvailable);

                string msg = value.ToString();

                try
                {
                    FocuserManager.Status.TempComp = value;
                    FocuserManager.TempComp = value;
                    msg += _done;
                }
                catch (Exception)
                {
                    msg += _failed;

                    throw;
                }
                finally
                {
                    LogMessage("Set TempComp:", msg);
                }
            }
        }

        public bool TempCompAvailable
        {
            get
            {
                var retval = false;
                string msg = "";

                try
                {
                    Exception xcp = FocuserManager.Parameters.GetException();

                    if (xcp != null)
                    {
                        throw xcp;
                    }

                    retval = FocuserManager.Parameters.TempCompAvailable;
                    msg += $"{retval}{_done}";
                }
                catch (Exception)
                {
                    msg += _failed;

                    throw;
                }
                finally
                {
                    LogMessage("Get TempCompAvailable:", msg);
                }

                return retval;
            }
        }

        public double Temperature
        {
            get
            {
                var retval = Double.NaN;
                string msg = "";

                try
                {
                    retval = FocuserManager.Service.Temperature;
                    msg += $"{retval}{_done}";
                }
                catch (Exception ex)
                {
                    msg += $"{_failed} - {ex.Message}";
                    throw;
                }
                finally
                {
                    LogMessage("Get Temperature:", msg);
                }

                return retval;
            }
        }

        #endregion

        #region IFocuserV4 Properties and Methods

        public IStateValueCollection DeviceState
        {
            get
            {
                IStateValueCollection retval;
                string msg = "DeviceState";

                try
                {
                    Exception xcp = FocuserManager.Parameters.GetException();

                    if (xcp != null)
                    {
                        throw xcp;
                    }

                    retval = FocuserManager.DeviceState;
                    msg += _done;
                }
                catch (Exception)
                {
                    msg += _failed;
                    throw;
                }
                finally
                {
                    LogMessage("DeviceState:", msg);
                }

                return retval;
            }
        }

        public void Connect()
        {
            if (connecting)
            {
                LogMessage("Connect", "Connect or Disconnect already in progress, ignoring Connect() call.");
                return;
            }

            if (ConnectedState)
            {
                LogMessage("Connect", "Already connected, ignoring Connect() call.");
                return;
            }

            // Set Connecting to true and clear any previous exception
            LogMessage("Connect", "Setting connecting TRUE.");
            connecting = true;
            connectException = null;

            // Run a task to set the Connected property to True
            LogMessage("Connect", "Starting connection task...");
            Task.Run(() =>
            {
                // Ensure that no exceptions can escape
                try
                {
                    // Set Connected True
                    LogMessage("ConnectTask", "About to set Connected True");
                    Connected = true;
                    LogMessage("ConnectTask", "Connected Set True OK");
                }
                catch (Exception ex)
                {
                    // Something went wrong so log the issue and save the exception
                    LogMessage("ConnectTask", $"Connected threw an exception: {ex.Message}");
                    connectException = ex;
                }
                // Ensure that Connecting is always set False at the end of the task
                finally
                {
                    LogMessage("ConnectTask", "Setting Connecting to False");
                    connecting = false;
                }
            });

            LogMessage("Connect", "Connection task started OK");
        }

        public void Disconnect()
        {
            if (connecting)
            {
                LogMessage($"Disconnect", $"Connect or Disconnect already in progress, ignoring Disconnect() call.");
                return;
            }

            if (!ConnectedState)
            {
                LogMessage($"Disconnect", $"Already disconnected, ignoring Disconnect() call.");
                return;
            }

            // Set Connecting to true and clear any previous exception
            connecting = true;
            connectException = null;

            // Run a task to set the Connected property to False
            Task.Run(() =>
             {
                 // Ensure that no exceptions can escape
                 try
                 {
                     // Set Connected False
                     LogMessage($"DisconnectTask", "Setting Connected FALSE...");
                     Connected = false;

                     LogMessage($"DisconnectTask", "Connected set FALSE OK");
                 }
                 catch (Exception ex)
                 {
                     // Something went wrong so log the issue and save the exception
                     LogMessage($"DisconnectTask", $"Connected threw an exception: {ex.Message}\r\n{ex}");
                     connectException = ex;
                 }
                 finally
                 {
                     // Ensure that Connecting is always set False at the end of the task
                     LogMessage($"DisconnectTask", "Setting Connecting to FALSE");
                     connecting = false;
                 }

                 // Immediately clean up any disposed objects so that the local server can shut down promptly rather than waiting for its next scheduled 60 second garbage collection
                 for (int loopCount = 1; loopCount <= Globals.GARBAGE_COLLECT_ATTEMPTS; loopCount++)
                 {
                     // Sleep the thread to give some time for COM objects to be invalidated and ready for garbage collection
                     LogMessage($"DisconnectTask", $"Sleeping thread for {Globals.GARBAGE_COLLECT_TIME_BETWEEN_ATTEMPTS} milliseconds");
                     Thread.Sleep(Globals.GARBAGE_COLLECT_TIME_BETWEEN_ATTEMPTS);

                     // Force a garbage collection to remove COM object references and enable the local server to shut down.
                     // If this is not done here the local server will wait until the next scheduled garbage collection (1 minute) before closing down.
                     LogMessage($"DisconnectTask", $"Forcing garbage collection - attempt {loopCount}/{Globals.GARBAGE_COLLECT_ATTEMPTS}, Object count: {Server.ObjectsCount}");
                     Server.ForceGarbageCollection();
                 }

                 LogMessage($"DisconnectTask", "Task finished");
             });

            LogMessage($"Disconnect", "Complete.");
        }

        public bool Connecting
        {
            get
            {
                // If Connect or Disconnect threw an exception, throw it here
                if (!(connectException is null))
                    throw connectException;

                // Connect or Disconnect did not throw an exception so return the current connecting value
                return connecting;
            }
        }

        #endregion

        #region Private properties and methods

        private void CheckTempCompAvailable(string ident, bool isAvailable)
        {
            if (!isAvailable)
            {
                string msg = "The focuser driver does not support setting temperature compensation.";
                LogMessage(ident, msg);

                throw new PropertyNotImplementedException(ident, msg);
            }
        }

        private void CheckAbsolute(string ident, bool absolute, bool isSetter)
        {
            if (!absolute)
            {
                string msg = "Not implemented for a relative positioning focuser.";
                LogMessage(ident, msg);

                throw new PropertyNotImplementedException(ident, isSetter, msg);
            }
        }

        /// <summary>
        /// Helper method to read the driver description from the custom attribute.
        /// </summary>
        /// <returns>the display name for the class</returns>
        private string GetDriverDescriptionFromAttribute()
        {
            string retval = "Unknown driver";

            Type t = this.GetType(); ;
            Type attributeType = typeof(ServedClassNameAttribute);
            object[] customAtts = t.GetCustomAttributes(attributeType, false);

            if (customAtts != null && customAtts.Length == 1)
            {
                if (customAtts[0] is ServedClassNameAttribute attr)
                {
                    retval = attr.DisplayName;
                }
            }

            return retval;
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            FocuserSettings settings = FocuserSettings.FromProfile();
            _logger.Enabled = settings.IsLoggingEnabled;
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void SaveProfile()
        {
            FocuserSettings settings = new FocuserSettings
            {
                FocuserID = FocuserManager.FocuserID,
                TemperatureOffset = Globals.FocuserTemperatureOffset,
                IsLoggingEnabled = _logger.Enabled,
                FastUpdatePeriod = FocuserManager.FastPollingPeriod
            };

            settings.ToProfile();
        }

        #endregion Private properties and methods
    }
}
