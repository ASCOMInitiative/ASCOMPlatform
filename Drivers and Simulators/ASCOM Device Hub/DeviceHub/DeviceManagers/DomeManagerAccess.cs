using System;
using System.Collections;

using ASCOM.DeviceInterface;
using ASCOM.Utilities;

using ASCOM.DeviceHub.MvvmMessenger;
using System.Threading;

namespace ASCOM.DeviceHub
{
    public partial class DomeManager
    {
        public static string Choose(string id)
        {
            Chooser chooser = new Chooser
            {
                DeviceType = "Dome"
            };

            return chooser.Choose(id);
        }

        private IDomeService _service;

        public IDomeService Service
        {
            get
            {
                if (_service == null)
                {
                    _service = ServiceContainer.Instance.GetService<IDomeService>();
                }

                return _service;
            }
            private set
            {
                _service = value;
            }
        }

        public void InitializeDomeService(string id)
        {
            if (Service.DeviceCreated)
            {
                Service.Dispose();
                Service = null;
            }

            // This creates the service and defines the driver ID.

            Service.CreateDevice(id);
        }

        private void ReleaseDomeService()
        {
            _service.Dispose();
            _service = null;
        }

        #region IDomeV2 Properties

        public bool Connected
        {
            get
            {
                bool retval = false;

                ActivityMessageTypes msgType = ActivityMessageTypes.Other;

                if (CheckDevice(false, false))
                {
                    try
                    {
                        retval = Service.Connected;
                        LogActivityLine(msgType, $"Get Connected flag - {retval}");
                    }
                    catch (Exception xcp)
                    {
                        LogActivityLine(msgType, $"Get Connected flag - {Failed}");
                        LogActivityLine(msgType, $"{xcp}");

                        throw;
                    }
                }

                return retval;
            }
            set
            {
                ActivityMessageTypes msgType = ActivityMessageTypes.Other;

                try
                {
                    bool isConnected = Service.Connected;

                    if (value != isConnected)
                    {
                        CheckDevice(false);
                        Service.Connected = value;

                        // Poll the device for up to ten seconds or until it reports that
                        // disconnect/connect is complete.

                        int i = 0;
                        int numTries = 20;
                        int msDelay = 500;

                        while (i < numTries)
                        {
                            Thread.Sleep(msDelay);
                            isConnected = Service.Connected;

                            if (value == isConnected)
                            {
                                break;
                            }

                            ++i;
                        }

                        if (value == isConnected)
                        {
                            LogActivityLine(msgType, $"Set Connected flag -> {value} {(value ? "(connected)" : "(disconnected)")}");
                            LogActivityLine(msgType, "{0} took {1} milliseconds."
                                            , (value) ? "Connection" : "Disconnection"
                                            , i * msDelay);
                        }
                        else
                        {
                            LogActivityLine(msgType, $"Set Connected flag -> {value} {Failed}");
                            LogActivityLine(msgType, "{0} failed after {1} milliseconds"
                                            , (value) ? "Connection" : "Disconnection"
                                            , numTries * msDelay);
                        }
                    }
                    else
                    {
                        LogActivityLine(msgType, $"Set Connected flag -> {value} (no change)");
                    }
                }
                catch (Exception xcp)
                {
                    LogActivityLine(msgType, $"Set Connected flag -> {value} {Failed}");
                    LogActivityLine(msgType, xcp.ToString());

                    throw;
                }
            }
        }

        public string Description
        {
            get
            {
                return GetServiceProperty<string>(() => Service.Description, "N/A", ActivityMessageTypes.Parameters);
            }
        }

        public string DriverInfo
        {
            get
            {
                return GetServiceProperty<string>(() => Service.DriverInfo, "N/A", ActivityMessageTypes.Parameters);
            }
        }

        public string DriverVersion
        {
            get
            {
                return GetServiceProperty<string>(() => Service.DriverVersion, "N/A", ActivityMessageTypes.Parameters);
            }
        }

        public short InterfaceVersion
        {
            get
            {
                return GetServiceProperty<short>(() => Service.InterfaceVersion, 0, ActivityMessageTypes.Parameters);
            }
        }

        public string Name
        {
            get
            {
                return GetServiceProperty<string>(() => Service.Name, "N/A", ActivityMessageTypes.Other);
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                ArrayList emptyList = new ArrayList();
                return GetServiceProperty<ArrayList>(() => Service.SupportedActions, emptyList, ActivityMessageTypes.Parameters);
            }
        }

        public double Altitude
        {
            get
            {
                return GetServiceProperty<double>(() => Service.Altitude, Double.NaN);
            }
        }

        public bool AtHome
        {
            get
            {
                return GetServiceProperty<bool>(() => Service.AtHome, false);
            }
        }

        public bool AtPark
        {
            get
            {
                return GetServiceProperty<bool>(() => Service.AtPark, false);
            }
        }

        public double Azimuth
        {
            get
            {
                return GetServiceProperty<double>(() => Service.Azimuth, Double.NaN);
            }
        }

        public bool CanFindHome
        {
            get
            {
                return GetServiceProperty<bool>(() => Service.CanFindHome, false, ActivityMessageTypes.Capabilities);
            }
        }

        public bool CanPark
        {
            get
            {
                return GetServiceProperty<bool>(() => Service.CanPark, false, ActivityMessageTypes.Capabilities);
            }
        }

        public bool CanSetAltitude
        {
            get
            {
                return GetServiceProperty<bool>(() => Service.CanSetAltitude, false, ActivityMessageTypes.Capabilities);
            }
        }

        public bool CanSetAzimuth
        {
            get
            {
                return GetServiceProperty<bool>(() => Service.CanSetAzimuth, false, ActivityMessageTypes.Capabilities);
            }
        }

        public bool CanSetPark
        {
            get
            {
                return GetServiceProperty<bool>(() => Service.CanSetPark, false, ActivityMessageTypes.Capabilities);
            }
        }

        public bool CanSetShutter
        {
            get
            {
                return GetServiceProperty<bool>(() => Service.CanSetShutter, false, ActivityMessageTypes.Capabilities);
            }
        }

        public bool CanSlave
        {
            get
            {
                return GetServiceProperty<bool>(() => Service.CanSlave, false, ActivityMessageTypes.Capabilities);
            }
        }

        public bool CanSyncAzimuth
        {
            get
            {
                return GetServiceProperty<bool>(() => Service.CanSyncAzimuth, false, ActivityMessageTypes.Capabilities);
            }
        }

        public ShutterState ShutterStatus
        {
            get
            {
                return GetServiceProperty<ShutterState>(() => Service.ShutterStatus, ShutterState.shutterError);
            }

        }

        public bool Slaved
        {
            get
            {
                string ident = "Get Slaved";
                LogActivityStart(ActivityMessageTypes.Status, ident + ": ");
                CheckDevice();
                LogActivityEnd(ActivityMessageTypes.Status, Failed);

                // Since we are controlling dome slaving, prevent reading the Slaved status
                // in the driver.

                throw new PropertyNotImplementedException(ident, "Dome slaving is implemented in Device Hub, not the driver.");
            }
            set
            {
                string ident = "Set Slaved";
                string msg = "Setting Slaved to {value} {Failed}: DeviceHub does not use driver-implemented slaving.";
                CheckDevice();
                LogActivityLine(ActivityMessageTypes.Status, msg);

                // Since we are controlling dome slaving, prevent changing the Slaved status
                // in the driver.

                throw new PropertyNotImplementedException(ident, msg);
            }
        }

        public bool Slewing
        {
            get
            {
                return GetServiceProperty<bool>(() => Service.Slewing, false);
            }
        }

        #endregion IDomeV2 Properties

        #region IDomeV2 Methods

        public void AbortSlew()
        {
            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            Exception except = null;
            string msgEnd = "";

            try
            {
                CheckDevice();
                Service.AbortSlew();
                msgEnd = Done;
            }
            catch (Exception xcp)
            {
                except = xcp;
                msgEnd = $"{Failed}. Details follow:";

                throw;
            }
            finally
            {
                LogActivityLine(msgType, msgEnd);

                if (except != null)
                {
                    LogActivityLine(msgType, $"{except}");
                }

                DomeStatus = new DevHubDomeStatus(this);
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            string retval = null;
            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            Exception except = null;
            string msgEnd = "";

            try
            {
                CheckDevice();
                retval = Service.Action(actionName, actionParameters);
            }
            catch (Exception xcp)
            {
                except = xcp;
                msgEnd = $"{Failed}. Details follow:";

                throw;
            }
            finally
            {
                if (except == null)
                {
                    LogActivityLine(msgType, $"Action ({actionName}) returned '{retval}'.");
                }
                else
                {
                    LogActivityLine(msgType, $"Action ({actionName}) {msgEnd}");
                    LogActivityLine(msgType, $"{except}");
                }
            }

            return retval;
        }

        public void CloseShutter()
        {
            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            Exception except = null;
            string msgEnd = "";

            try
            {
                CheckDevice();
                Service.CloseShutter();
                msgEnd = Done;
            }
            catch (Exception xcp)
            {
                except = xcp;
                msgEnd = $"{Failed}. Details follow:";

                throw;
            }
            finally
            {
                LogActivityLine(msgType, $"CloseShutter: {msgEnd}");

                if (except != null)
                {
                    LogActivityLine(msgType, $"{except}");
                }

                DomeStatus = new DevHubDomeStatus(this);
            }
        }

        public void CommandBlind(string command, bool raw = false)
        {
            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            Exception except = null;
            string msgEnd = "";

            try
            {
                CheckDevice();
                Service.CommandBlind(command, raw);
                msgEnd = Done;
            }
            catch (Exception xcp)
            {
                except = xcp;
                msgEnd = $"{Failed}. Details follow:";

                throw;
            }
            finally
            {
                LogActivityLine(msgType, $"CommandBlind( {command} ) - {msgEnd}");

                if (except != null)
                {
                    LogActivityEnd(msgType, $"{except}");
                }
            }
        }

        public bool CommandBool(string command, bool raw = false)
        {
            bool retval;

            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            Exception except = null;
            string msgEnd = "";

            try
            {
                CheckDevice();
                retval = Service.CommandBool(command, raw);
                msgEnd = $"returned {retval}.";
            }
            catch (Exception xcp)
            {
                except = xcp;
                msgEnd = $"{Failed}. Details Follow:";

                throw;
            }
            finally
            {
                LogActivityLine(msgType, $"CommandBool( {command} ) - {msgEnd}");

                if (except != null)
                {
                    LogActivityLine(msgType, $"{except}");
                }
            }

            return retval;
        }

        public string CommandString(string command, bool raw = false)
        {
            string retval;

            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            Exception except = null;
            string msgEnd = "";

            try
            {
                CheckDevice();
                retval = Service.CommandString(command, raw);
                msgEnd = $"returned {retval}.";
            }
            catch (Exception xcp)
            {
                except = xcp;
                msgEnd = $"{Failed}. Details Follow:";

                throw;
            }
            finally
            {
                LogActivityLine(msgType, $"CommandString( {command} ) - {msgEnd}");

                if (except != null)
                {
                    LogActivityLine(msgType, $"{except}");
                }
            }

            return retval;
        }

        public void Dispose()
        {
            Messenger.Default.Unregister<TelescopeCapabilitiesUpdatedMessage>(this);
            Messenger.Default.Unregister<TelescopeParametersUpdatedMessage>(this);
            Messenger.Default.Unregister<TelescopeStatusUpdatedMessage>(this);

            if (Service != null)
            {
                Service.Dispose();
                Service = null;
            }

            DomeManager.Instance = null;
        }

        public void FindHome()
        {
            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            Exception except = null;
            string msgEnd = "";

            try
            {
                CheckDevice();
                DomeStatus.Slewing = true;
                Service.FindHome();
                msgEnd = Done;
            }
            catch (Exception xcp)
            {
                except = xcp;
                msgEnd = $"{Failed}. Details follow:";

                throw;
            }
            finally
            {
                LogActivityLine(msgType, "FindHome - {0}", msgEnd);

                if (except != null)
                {
                    LogActivityLine(msgType, $"{except}");
                }

                DomeStatus = new DevHubDomeStatus(this);
            }
        }

        public void OpenShutter()
        {
            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            Exception except = null;
            string msgEnd = "";

            try
            {
                CheckDevice();
                Service.OpenShutter();
                msgEnd = SlewStarted;
            }
            catch (Exception xcp)
            {
                except = xcp;
                msgEnd = $"{Failed}. Details follow:";

                throw;
            }
            finally
            {
                LogActivityLine(msgType, $"OpenShutter: {msgEnd}");

                if (except != null)
                {
                    LogActivityLine(msgType, $"{except}");
                }

                DomeStatus = new DevHubDomeStatus(this);
            }
        }

        public void Park()
        {
            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            Exception except = null;
            string msgEnd = "";

            try
            {
                CheckDevice();
                Service.Park();
                msgEnd = Done;
            }
            catch (Exception xcp)
            {
                except = xcp;
                msgEnd = $"{Failed}. Details follow:";

                throw;
            }
            finally
            {
                LogActivityLine(msgType, $"Park - {msgEnd}");

                if (except != null)
                {
                    LogActivityLine(msgType, $"{except}");
                }

                DomeStatus = new DevHubDomeStatus(this);
            }
        }

        public void SetPark()
        {
            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            string msgEnd = "";
            Exception except = null;

            try
            {
                CheckDevice();
                Service.SetPark();
                msgEnd = Done;
            }
            catch (Exception xcp)
            {
                except = xcp;
                msgEnd = Failed;

                throw;
            }
            finally
            {
                LogActivityLine(msgType, $"SetPark: {msgEnd}");

                if (except != null)
                {
                    LogActivityLine(msgType, except.ToString());
                }
            }
        }

        /// <summary>
        /// This <see langword="method"/> is only included so that all members of the ITelescope interface are implemented. It has no other function.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void SetupDialog()
        {
            throw new InvalidOperationException("TelescopeManagerAccess.SetupDialog - This method is only included to keep the compiler happy! - Use TelescopeDriver.SetupDialog instead.");
        }

        public void SlewToAltitude(double altitude)
        {
            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            Exception except = null;
            string msgEnd = "";

            try
            {
                CheckDevice();
                Service.SlewToAltitude(altitude);
                msgEnd = SlewStarted;
            }
            catch (Exception xcp)
            {
                except = xcp;
                msgEnd = $"{Failed}. Details follow:";

                throw;
            }
            finally
            {
                LogActivityLine(msgType, $"SlewToAltitude ({altitude:f5}°): {msgEnd}");

                if (except != null)
                {
                    LogActivityLine(msgType, $"{except}");
                }

                DomeStatus = new DevHubDomeStatus(this);
            }
        }

        public void SlewToAzimuth(double azimuth)
        {
            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            Exception except = null;
            string msgEnd = "";

            try
            {
                CheckDevice();
                Service.SlewToAzimuth(azimuth);
                msgEnd = SlewStarted;
            }
            catch (Exception xcp)
            {
                except = xcp;
                msgEnd = $"{Failed}. Details follow:";

                throw;
            }
            finally
            {
                LogActivityLine(ActivityMessageTypes.Commands, $"SlewToAzimuth ({azimuth:f5}°): {msgEnd}");

                if (except != null)
                {
                    LogActivityLine(msgType, $"{except}");
                }

                DomeStatus = new DevHubDomeStatus(this);
            }
        }

        public void SyncToAzimuth(double azimuth)
        {
            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            Exception except = null;
            string msgEnd = "";

            try
            {
                CheckDevice();
                Service.SyncToAzimuth(azimuth);
                msgEnd = Done;
            }
            catch (Exception xcp)
            {
                except = xcp;
                msgEnd = $"{Failed}. Details follow:";

                throw;
            }
            finally
            {
                LogActivityLine(ActivityMessageTypes.Commands, $"SyncToAzimuth: {msgEnd}");

                if (except != null)
                {
                    LogActivityLine(msgType, $"{except}");
                }

                DomeStatus = new DevHubDomeStatus(this);
            }
        }

        #endregion IDomeV2 Methods

        #region IDomeV3 Properties and Methods

        public IStateValueCollection DeviceState => GetServiceProperty<IStateValueCollection>(() => Service.DeviceState, new StateValueCollection());

        public void Connect()
        {
            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            string msgEnd = "";
            Exception except = null;

            try
            {
                CheckDevice();
                Service.Connect();
                msgEnd = Done;
            }
            catch (Exception xcp)
            {
                except = xcp;
                msgEnd = $"{Failed}. Details follow:";

                throw;
            }
            finally
            {
                LogActivityLine(msgType, "Connect - {0}", msgEnd);

                if (except != null)
                {
                    LogActivityLine(msgType, $"{except}");
                }

                DomeStatus = new DevHubDomeStatus(this);
            }

        }

        public void Disconnect()
        {
            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            string msgEnd = "";
            Exception except = null;

            try
            {
                CheckDevice();
                Service.Disconnect();
                msgEnd = Done;
            }
            catch (Exception xcp)
            {
                except = xcp;
                msgEnd = $"{Failed}. Details follow:";

                throw;
            }
            finally
            {
                LogActivityLine(msgType, "Disconnect - {0}", msgEnd);

                if (except != null)
                {
                    LogActivityLine(msgType, $"{except}");
                }

                DomeStatus = new DevHubDomeStatus(this);
            }
        }

        public bool Connecting => GetServiceProperty<bool>(() => Service.Connecting, false);

        #endregion

        #region Helper Methods

        protected override void CheckDevice()
        {
            CheckDevice(true, true);
        }

        private void CheckDevice(bool testConnected)
        {
            CheckDevice(testConnected, true);
        }

        private bool CheckDevice(bool testConnected, bool throwException)
        {
            bool retval;

            if (Service == null || !Service.DeviceCreated)
            {
                if (throwException)
                {
                    throw new NullReferenceException("The Dome object is null.");
                }

                retval = false;
            }
            else
            {
                retval = true;
            }

            if (retval && testConnected && !Service.Connected)
            {
                if (throwException)
                {
                    throw new NotConnectedException("There is no connected dome.");
                }

                retval = false;
            }

            return retval;
        }

        #endregion Helper Methods

    }
}
