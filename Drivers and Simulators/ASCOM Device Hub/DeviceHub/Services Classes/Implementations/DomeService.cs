using ASCOM.Astrometry;
using ASCOM.DeviceHub.MvvmMessenger;
using ASCOM.DeviceInterface;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace ASCOM.DeviceHub
{
    public class DomeService : IDomeService
    {
        private DateTime lastSlewCompleted;
        private bool lastSlewingState = false;

        private string DeviceID { get; set; }
        private ASCOM.DriverAccess.Dome Dome { get; set; }

        public bool DeviceCreated => Initialized && Dome != null;
        public bool DeviceAvailable => DeviceCreated && Connected;
        public bool Initialized { get; private set; }

        public void CreateDevice(string id)
        {
            if (Initialized)
            {
                throw new Exception("The dome service attempted to re-initialize the dome.");
            }

            if (Dome == null)
            {
                if (String.IsNullOrEmpty(id))
                {
                    throw new Exception("The dome service is unable to create a dome until a Dome has been chosen.");
                }

                DeviceID = id;

                try
                {
                    Dome = new ASCOM.DriverAccess.Dome(DeviceID);
                    Initialized = true;
                }
                catch (Exception xcp)
                {
                    throw new Exception("Unable to create the Dome object", xcp);
                }
            }
        }

        #region IDomeV2 Properties

        public bool Connected
        {
            get
            {
                bool retval = false; // Defaulted to false

                try
                {
                    if (!(Dome is null))
                        retval = Dome.Connected;
                }
                catch (DriverException)
                {
                    throw;
                }
                catch (Exception xcp)
                {
                    throw new DriverException("Unable to read Dome.Connected property", xcp);
                }

                return retval;
            }
            set
            {
                try
                {
                    Dome.Connected = value;
                }
                catch (DriverException)
                {
                    throw;
                }
                catch (Exception xcp)
                {
                    throw new DriverException("Unable to write Dome.Connected property", xcp);
                }
            }
        }

        public string Description => Dome.Description;
        public string DriverInfo => Dome.DriverInfo;
        public string DriverVersion => Dome.DriverVersion;
        public short InterfaceVersion => Dome.InterfaceVersion;
        public string Name => Dome.Name;

        public ArrayList SupportedActions => Dome.SupportedActions;
        public double Altitude => Dome.Altitude;
        public bool AtHome => Dome.AtHome;
        public bool AtPark => Dome.AtPark;
        public double Azimuth => Dome.Azimuth;
        public bool CanFindHome => Dome.CanFindHome;
        public bool CanPark => Dome.CanPark;
        public bool CanSetAltitude => Dome.CanSetAltitude;
        public bool CanSetAzimuth => Dome.CanSetAzimuth;
        public bool CanSetPark => Dome.CanSetPark;
        public bool CanSetShutter => Dome.CanSetShutter;
        public bool CanSlave => Dome.CanSlave;
        public bool CanSyncAzimuth => Dome.CanSyncAzimuth;
        public ShutterState ShutterStatus => Dome.ShutterStatus;
        public bool Slaved
        {
            get => Dome.Slaved;
            set => Dome.Slaved = value;
        }
        public bool Slewing
        {
            get
            {
                // Retrieve the dome slewing state
                bool retval = Dome.Slewing;
                LogMessage($"Dome.Slewing returned {retval}, Last slewing state: {lastSlewingState}, Insert slew delay: {DelaySlewToAzimuth}");

                // Check whether the dome has just transitioned from slewing to stationary
                if (lastSlewingState & !retval) // Dome was slewing but is now reporting not slewing
                {
                    DelaySlewToAzimuth = true; // Flag that we may need to insert a delay on the next SlewToAzimuth call
                    lastSlewCompleted = DateTime.UtcNow; // Save the time at which the end of slew was recorded
                    LogMessage("Dome has just stopped slewing, will insert delay on next SlewToAzimuth command if necessary.");
                }

                // Save the current slewing state as the last seen slewing state
                lastSlewingState = retval;

                return retval;
            }
        }

        #endregion IDomeV2 Properties

        #region IDomeV2 Methods

        public void AbortSlew()
        {
            Dome.AbortSlew();
        }

        public string Action(string actionName, string actionParameters)
        {
            return Dome.Action(actionName, actionParameters);
        }

        public void CloseShutter()
        {
            Dome.CloseShutter();
        }

        public void CommandBlind(string command, bool raw = false)
        {
            Dome.CommandBlind(command, raw);
        }

        public bool CommandBool(string command, bool raw = false)
        {
            return Dome.CommandBool(command, raw);
        }

        public string CommandString(string command, bool raw = false)
        {
            return Dome.CommandString(command, raw);
        }

        public void Dispose()
        {
            Dome.Dispose();
            Dome = null;
            DeviceID = null;
            Initialized = false;
        }

        public void FindHome()
        {
            Dome.FindHome();
        }

        public void OpenShutter()
        {
            Dome.OpenShutter();
        }

        public void Park()
        {
            Dome.Park();
        }

        public void SetPark()
        {
            Dome.SetPark();
        }

        public void SetupDialog()
        {
            // This method is here to satisfy the IDomeV2 interface and should never be called.

            throw new System.NotImplementedException();
        }

        public void SlewToAltitude(double altitude)
        {
            Dome.SlewToAltitude(altitude);
        }

        public void SlewToAzimuth(double azimuth)
        {
            LogMessage($"SlewToAzimuth - Received SlewToAzimuth command.");
            // Check whether we need to insert a delay before issuing the slew command
            if (DelaySlewToAzimuth) // We do need to insert a delay
            {
                // Reset the insert delay flag
                DelaySlewToAzimuth = false;

                // Check whether enough time has passed in order not to wait
                TimeSpan timeSinceLastSlew = DateTime.UtcNow - lastSlewCompleted;

                if (timeSinceLastSlew.TotalSeconds >= Globals.SlewDelay) // Sufficient time has passed so no delay required
                {
                    LogMessage($"SlewToAzimuth - {timeSinceLastSlew.TotalSeconds:0.0} seconds have passed since the last slew (minimum: {Globals.SlewDelay} seconds), so no need to delay sending the SlewToAzimuth command.");
                }
                else // Insufficient time has passed, so we need to wait before sending the SlewToAzimuth command.
                {
                    // Calculate the remaining time to wait
                    double waitTime = Globals.SlewDelay - timeSinceLastSlew.TotalSeconds;

                    // Wait for the remaining time to complete the required delay
                    LogMessage($"SlewToAzimuth - The minimum time to pass ({Globals.SlewDelay} seconds) has not been exceeded, waiting {waitTime:0.0} seconds before issuing SlewToAzimuth command...");
                    Task.Delay(TimeSpan.FromSeconds(waitTime)).Wait();
                    LogMessage("SlewToAzimuth - Completed wait before SlewToAzimuth command.");
                }
            }
            else
            {
                LogMessage("SlewToAzimuth - No wait required before SlewToAzimuth command.");
            }

            LogMessage($"SlewToAzimuth - Sending SlewToAzimuth command to Dome.");
            Dome.SlewToAzimuth(azimuth);
            LogMessage($"SlewToAzimuth - Received control back from SlewToAzimuth command.");
        }

        public void SyncToAzimuth(double azimuth)
        {
            Dome.SyncToAzimuth(azimuth);
        }

        #endregion IDomeV2 Methods

        #region IDomeV3 Properties and Methods

        public IStateValueCollection DeviceState => Dome.DeviceState;

        public void Connect()
        {
            Dome.Connect();
        }

        public void Disconnect()
        {
            Dome.Disconnect();
        }

        public bool Connecting => Dome.Connecting;

        #endregion

        #region Private Methods

        private void LogMessage(string message)
        {
            Messenger.Default.Send(new ActivityMessage(DeviceTypeEnum.Dome, ActivityMessageTypes.Other, $"{DateTime.Now:HH:mm:ss.fff}: Dome - Others:       DomeService - {message}\r\n"));
        }

        private bool DelaySlewToAzimuth { get; set; } = false;

        #endregion
    }
}
