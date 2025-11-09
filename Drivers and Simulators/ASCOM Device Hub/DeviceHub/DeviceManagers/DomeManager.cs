using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using ASCOM.DeviceInterface;
using ASCOM.Astrometry.Exceptions;
using ASCOM.Astrometry.Transform;

using ASCOM.DeviceHub.MvvmMessenger;
using System.Diagnostics;

namespace ASCOM.DeviceHub
{
    public partial class DomeManager : DeviceManagerBase, IDomeManager, IDomeV3, IDisposable
    {
        #region Static Constructor, Properties, Fields, and Methods

        private const int POLLING_INTERVAL_NORMAL = 5000;   // once every 5 seconds

        public static string DomeID { get; set; }

        private static DomeManager _instance = null;

        public static DomeManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DomeManager();
                }

                return _instance;
            }

            private set => _instance = value;
        }

        static DomeManager()
        {
            string caller = "DomeManager static ctor";

            LogAppMessage("Initialization started", caller);

            DomeID = "";
        }

        public static void SetDomeID(string id)
        {
            DomeID = id;
            Messenger.Default.Send(new DomeIDChangedMessage(id));
        }

        #endregion Static Constructor, Properties, Fields, and Methods

        #region Fields

        private bool ForceSlavedUpdate;

        #endregion

        #region Private Properties

        private bool DeviceCreated => Service.DeviceCreated;
        private bool DeviceAvailable => Service.DeviceAvailable;

        private CancellationTokenSource PollingTokenSource { get; set; }
        private Task PollingTask { get; set; }

        private bool IsPolling { get; set; }
        private bool StatusUpdated { get; set; }

        // We need telescope info in order to slave our position with the telescope.

        private DevHubTelescopeStatus TelescopeStatus { get; set; }
        private TelescopeParameters TelescopeParameters { get; set; }
        private SlewInProgressMessage TelescopeSlewState { get; set; }

        #endregion Private Properties

        #region Instance Constructor

        private DomeManager()
            : base(DeviceTypeEnum.Dome)
        {
            string caller = "DomeManager instance ctor";

            LogAppMessage("Initializing Instance constructor", caller);

            IsConnected = false;
            Capabilities = null;
            Parameters = null;
            DomeStatus = null;
            PollingTask = null;
            ParkingState = ParkingStateEnum.Unparked;
            HomingState = HomingStateEnum.NotAtHome;
            TelescopeParameters = null;
            TelescopeStatus = null;
            TelescopeSlewState = new SlewInProgressMessage(false);
            ForceSlavedUpdate = false;

            PollingPeriod = POLLING_INTERVAL_NORMAL;
            PollingChange = new ManualResetEvent(false);

            LogAppMessage("Registering message handlers.", caller);

            Messenger.Default.Register<TelescopeParametersUpdatedMessage>(this, (action) => UpdateTelescopeParameters(action));
            Messenger.Default.Register<TelescopeStatusUpdatedMessage>(this, (action) => UpdateTelescopeStatus(action));
            Messenger.Default.Register<DeviceDisconnectedMessage>(this, (action) => ClearTelescopeStatus(action));
            Messenger.Default.Register<SlewInProgressMessage>(this, (action) => InitiateSlavedSlew(action));

            LogAppMessage("Instance constructor initialization complete.", caller);
        }

        #endregion Instance Constructor

        #region Public Properties

        public bool IsConnected { get; private set; }
        public bool IsInteractivelyConnected { get; private set; }
        public string ConnectError { get; protected set; }
        public Exception ConnectException { get; protected set; }
        public ParkingStateEnum ParkingState { get; private set; }
        public HomingStateEnum HomingState { get; private set; }

        public DomeCapabilities Capabilities { get; private set; }
        public DomeParameters Parameters { get; private set; }
        public DevHubDomeStatus DomeStatus { get; private set; }

        public bool IsScopeReadyToSlave
        {
            get
            {
                bool retval = false;

                if (TelescopeStatus != null && TelescopeStatus.Connected)
                {
                    if (!Double.IsNaN(TelescopeStatus.Altitude) && !Double.IsNaN(TelescopeStatus.Azimuth))
                    {
                        retval = true;
                    }
                }
                return retval;
            }
        }

        public bool IsDomeReadyToSlave
        {
            get
            {
                bool retval = false;

                if (DomeStatus != null && DomeStatus.Connected)
                {
                    if (Capabilities != null && (Capabilities.CanSetAltitude || Capabilities.CanSetAzimuth))
                    {
                        retval = true;
                    }
                }

                return retval;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public bool ConnectDome()
        {
            // This is only called by the dome driver.

            return ConnectDome(DomeID, false);
        }

        public bool ConnectDome(string domeID, bool interactiveConnect = true)
        {
            ConnectError = "";
            ConnectException = null;

            // Don't re-create the service if it is already created for the same device.

            if (Service == null || !Service.DeviceCreated || domeID != DomeID)
            {
                try
                {
                    InitializeDomeService(domeID);
                    SetDomeID(domeID);

                    if (interactiveConnect)
                    {
                        IsInteractivelyConnected = interactiveConnect;
                    }
                }
                catch (Exception xcp)
                {
                    ConnectError = "Unable to create the dome object.";
                    ConnectException = xcp;
                    ReleaseDomeService();

                    return false;
                }
            }

            // If another client app is active, then the dome may already be connected!!

            Exception tempXcp = null;

            try
            {
                // Are we already connected?

                if (!Connected)
                {
                    // Not connected, so set connected to true.

                    Connected = true;
                }

                IsConnected = Connected;
            }
            catch (Exception xcp)
            {
                IsConnected = false;
                tempXcp = xcp;
            }

            if (!IsConnected)
            {
                ConnectError = "Unable to connect to the dome!";
                ConnectException = tempXcp;
                ReleaseDomeService();

                return false;
            }

            // According to the ASCOM docs, IsConnected should never be false it this point.  If there was
            // a problem connecting an exception should have been thrown!

            bool retval = false;

            if (IsConnected)
            {
                try
                {
                    Task task = Task.Run(() =>
                    {
                        ReadInitialDomeDataTask();
                    });

                    task.Wait();  // This will propagate any unhandled exception
                }
                catch (AggregateException xcp)
                {
                    LogActivityLine(ActivityMessageTypes.Other, "Attempting to initialize dome operation caught an unhandled exception. Details follow:");
                    LogActivityLine(ActivityMessageTypes.Other, xcp.InnerException.ToString());

                    Connected = false;
                    IsConnected = false;
                    ReleaseDomeService();
                }

                if (Connected)
                {
                    StartDevicePolling();
                }

                retval = Connected;
            }

            return retval;
        }

        public void DisconnectDome(bool interactiveDisconnect = false)
        {
            if (!DeviceCreated)
            {
                return;
            }

            if (IsConnected)
            {
                try
                {
                    if ((Server.DomesInUse == 1 && !IsInteractivelyConnected) ||
                         (Server.DomesInUse == 0 && IsInteractivelyConnected && interactiveDisconnect))
                    {
                        if (interactiveDisconnect)
                        {
                            IsInteractivelyConnected = false;
                        }

                        StopDevicePolling();
                        Connected = false;
                    }
                }
                catch (Exception)
                { }
                finally
                {
                    if (!IsPolling)
                    {
                        IsConnected = false;
                        Messenger.Default.Send(new DomeSlavedChangedMessage(false));
                        Messenger.Default.Send(new DeviceDisconnectedMessage(DeviceTypeEnum.Dome));
                        ReleaseDomeService();
                        Globals.LatestRawDomeStatus = null;
                    }
                }
            }
        }

        public void SetFastUpdatePeriod(double period)
        {
            FastPollingPeriod = period;
        }

        public void OpenDomeShutter()
        {
            // The name of this method must be different from the ASCOM method name.

            if (Connected && Capabilities != null && DomeStatus != null
                && Capabilities.CanSetShutter && DomeStatus.ShutterStatus != ShutterState.shutterOpen)
            {
                OpenShutter();
                DomeStatus = new DevHubDomeStatus(this);

                SetFastPolling();
            }
        }

        public void CloseDomeShutter()
        {
            // The name of this method must be different from the ASCOM method name.

            if (Connected && Capabilities != null && DomeStatus != null
                && Capabilities.CanSetShutter && DomeStatus.ShutterStatus != ShutterState.shutterClosed)
            {
                CloseShutter();
                DomeStatus = new DevHubDomeStatus(this);

                SetFastPolling();
            }
        }

        public void ParkTheDome()
        {
            if (Connected && Capabilities != null && Capabilities.CanPark && !Slewing)
            {
                ParkingState = ParkingStateEnum.ParkInProgress;

                Park();
                DomeStatus = new DevHubDomeStatus(this);

                SetFastPolling();
            }
        }

        public void FindHomePosition()
        {
            if (Connected && Capabilities != null && Capabilities.CanFindHome && !Slewing)
            {
                FindHome();
                DomeStatus = new DevHubDomeStatus(this);

                SetFastPolling();
            }
        }

        public void SlewDomeShutter(double targetAltitude)
        {
            ShutterState status = ShutterStatus;

            if (Connected && !Slewing && Capabilities.CanSetAltitude
                && (status != ShutterState.shutterClosed && status != ShutterState.shutterError))
            {
                SlewToAltitude(targetAltitude);
                DomeStatus = new DevHubDomeStatus(this);

                SetFastPolling();
            }
        }

        public void SetSlavedState(bool state)
        {
            if (state != Globals.IsDomeSlaved)
            {
                Messenger.Default.Send(new DomeSlavedChangedMessage(state));
            }
        }

        public void SlewDomeToAzimuth(double targetAzimuth)
        {
            if (Connected && !DomeStatus.Slewing && Capabilities.CanSetAzimuth)
            {
                SlewToAzimuth(targetAzimuth);
                DomeStatus = new DevHubDomeStatus(this);

                // Put the polling task into high gear.

                SetFastPolling();
            }
        }

        public void StopDomeMotion()
        {
            if (Connected)
            {
                AbortSlew();
                SetSlavedState(false);
                DomeStatus = new DevHubDomeStatus(this);
            }
        }

        public void SyncDomeToAzimuth(double azimuth)
        {
            if (Connected && !Slewing && Capabilities.CanSyncAzimuth)
            {
                SyncToAzimuth(azimuth);
            }
        }

        #endregion Public Methods

        #region Helper Methods

        private void StartDevicePolling()
        {
            if (!DeviceAvailable)
            {
                return;
            }

            if (!IsPolling)
            {
                PollingTokenSource = new CancellationTokenSource();
                PollingTask = Task.Factory.StartNew(() => PollDomeTask(PollingTokenSource.Token)
                                                , PollingTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        private void PollDomeTask(CancellationToken token)
        {
            IsPolling = true;
            bool taskCancelled = token.IsCancellationRequested;
            DateTime nextSlaveAdjustmentTime = DateTime.MinValue;
            WaitHandle[] waitHandles = new WaitHandle[] { token.WaitHandle, PollingChange };
            Stopwatch watch = new Stopwatch();
            double overhead = 0.0;

            TimeSpan fastPollExtension = new TimeSpan(0, 0, 3); //Wait 3 seconds after movement stops to return to normal polling.
            bool previousDomeMovingStatus = false;
            DateTime returnToNormalPollingTime = DateTime.MinValue;
            int previousPollingPeriod;

            while (!taskCancelled)
            {
                try
                {
                    // LogActivityLine(ActivityMessageTypes.Commands, $"* Start of PollDomeTask, polling period: {PollingPeriod}");
                    DateTime wakeupTime = DateTime.Now;

                    previousPollingPeriod = PollingPeriod;
                    PollingPeriod = POLLING_INTERVAL_NORMAL;
                    int fastPollingMilliseconds = Convert.ToInt32(FastPollingPeriod * 1000.0);

                    if (Service.DeviceAvailable)
                    {
                        UpdateDomeStatusTask();

                        bool domeSlewing = Service.Slewing;

                        if (!domeSlewing | ForceSlavedUpdate)
                        {
                            if (SlewTheSlavedDome(ref nextSlaveAdjustmentTime))
                            {
                                UpdateDomeStatusTask();
                            }
                        }

                        // Determine whether the dome is moving, allowing for the possibility that the property is not implemented. In this case report an error state to indicate that the shutter is not moving.
                        ShutterState shutterStatus = ShutterState.shutterError;
                        try
                        {
                            shutterStatus = Service.ShutterStatus;
                        }
                        catch { }

                        bool domeIsMoving = domeSlewing || (shutterStatus == ShutterState.shutterOpening) || (shutterStatus == ShutterState.shutterClosing);
                        LogActivityLine(ActivityMessageTypes.Commands, $"* PollDomeTask - Slewing: {TelescopeSlewState.IsSlewInProgress}, DomeIsMoving: {domeIsMoving}, DomeIsSlewing: {domeSlewing}, Shutter: {shutterStatus}, Target RA: {TelescopeSlewState.RightAscension.ToHMS()}, Declination: {TelescopeSlewState.Declination.ToDMS()}");

                        if (domeIsMoving)
                        {
                            // We are moving, so use the fast polling rate.

                            PollingPeriod = fastPollingMilliseconds;
                        }
                        else if (previousDomeMovingStatus)
                        {
                            // We stopped moving, so start the timer to return to normal polling.

                            returnToNormalPollingTime = DateTime.Now + fastPollExtension;
                            PollingPeriod = fastPollingMilliseconds;
                        }
                        else if (DateTime.Now < returnToNormalPollingTime)
                        {
                            // Continue fast polling.

                            PollingPeriod = fastPollingMilliseconds;
                        }
                        else
                        {
                            // Return to normal polling.

                            returnToNormalPollingTime = DateTime.MinValue;
                        }

                        // Remember our state for the next time through this loop.

                        previousDomeMovingStatus = domeIsMoving;

                        if (PollingPeriod == POLLING_INTERVAL_NORMAL && previousPollingPeriod != POLLING_INTERVAL_NORMAL)
                        {
                            LogActivityLine(ActivityMessageTypes.Commands, $"* Returning to normal polling every {PollingPeriod} ms.");
                        }
                    }

                    TimeSpan waitInterval = wakeupTime.AddMilliseconds((double)PollingPeriod) - DateTime.Now;
                    waitInterval -= TimeSpan.FromMilliseconds(overhead);

                    if (waitInterval.TotalMilliseconds < 0)
                    {
                        waitInterval = TimeSpan.FromMilliseconds(0);
                    }

                    // Wait until the polling interval has expired, we have been cancelled, or we have been
                    // awakened early because the polling interval has been changed.

                    watch.Start();
                    int index = WaitHandle.WaitAny(waitHandles, waitInterval);
                    watch.Stop();

                    // overhead is how much time it took us to get control back, in excess of the waitInterval.

                    overhead = Convert.ToDouble(watch.ElapsedMilliseconds) - waitInterval.TotalMilliseconds;
                    watch.Reset();

                    if (index == 0)
                    {
                        taskCancelled = true;

                    }
                    else if (index == 1)
                    {
                        // We have been awakened externally; presumably to change the polling interval or in response
                        // to the scope being slewed.

                        PollingChange.Reset();

                        // Reset the overhead value since we were awakened early.

                        overhead = 0.0;
                    }
                    else if (index == WaitHandle.WaitTimeout)
                    {
                        // The polling interval has expired.
                    }
                }
                catch (Exception ex)
                {
                    LogActivityLine(ActivityMessageTypes.Commands, $"* Unhandled exception in PollDomeTask - Please report this on the groups.io ASCOM Talk Forum: {ex.Message}\r\n{ex}");
                }
            }

            IsPolling = false;
        }

        private bool SlewTheSlavedDome(ref DateTime nextAdjustmentTime)
        {
            bool retval = false;
            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            DateTime returnTime = nextAdjustmentTime;

            LogActivityLine(msgType, $"SlewTheSlavedDome - Is dome slaved: {Globals.IsDomeSlaved}");

            // Check whether the done is slaved to the telescope
            if (Globals.IsDomeSlaved) // Dome is slaved to the telescope
            {
                // Whenever the scope is slewed, we get a message with the target RA, DEC and side-of-pier.
                // We need to slew there immediately. We will get another message when the slew has finished, but
                // until then we need to suspend normal slaved adjustments.

                SlewInProgressMessage telescopeSlewState = TelescopeSlewState;

                LogActivityLine(msgType, $"SlewTheSlavedDome - telescopeSlewState {(telescopeSlewState is null ? "is null" : "has been set")}, Telescope slew in progress: {telescopeSlewState?.IsSlewInProgress}");

                // Check whether the telescope is currently slewing
                if (telescopeSlewState != null && telescopeSlewState.IsSlewInProgress) // The telescope is slewing
                {
                    // Check whether the dome is slewing
                    bool slewing = Slewing;
                    LogActivityLine(ActivityMessageTypes.Status, $"Get Slewing: {slewing} (SlewTheSlavedDome)");

                    LogActivityLine(msgType, $"SlewTheSlavedDome - ForceSlavedUpdate: {ForceSlavedUpdate}, Dome is slewing: {slewing}, Use composite slewing flag: {Globals.UseCompositeSlewingFlag}");
                    if (ForceSlavedUpdate | !slewing | Globals.UseCompositeSlewingFlag) // The dome is not slewing or the telescope has just been slewed to new coordinates
                    {
                        // The telescope is slewing and we are slaved but not slewing, so calculate the current Alt/Az coordinates corresponding to the scope's target RA/Dec and slew the dome to the Alt/Az position if required.

                        LogActivityLine(msgType, $"Re-calculating Dome position due to telescope slew-in-progress. Forced update: {ForceSlavedUpdate}, Slewing: {slewing}, Composite slewing flag: {Globals.UseCompositeSlewingFlag}");

                        // Clear the forced update flag if set
                        if (ForceSlavedUpdate)
                        {
                            LogActivityLine(ActivityMessageTypes.Other, $"Resetting ForceSlavedUpdate to False");
                            ForceSlavedUpdate = false;
                        }

                        // Create a Transform object and set the site parameters
                        Transform xform = new Transform
                        {
                            SiteElevation = TelescopeParameters.SiteElevation,
                            SiteLongitude = TelescopeParameters.SiteLongitude,
                            SiteLatitude = TelescopeParameters.SiteLatitude
                        };

                        // Get the telescope equatorial system type
                        EquatorialCoordinateType coordinateType = TelescopeParameters.EquatorialSystem;

                        // Calculate the current Alt/Az corresponding to the scope target RA/Dec
                        try
                        {
                            // Set the Transform RA/Dec coordinates
                            if (coordinateType == EquatorialCoordinateType.equJ2000) // RA/Dec are J2000 coordinates
                            {
                                xform.SetJ2000(telescopeSlewState.RightAscension, telescopeSlewState.Declination);
                            }
                            else // Assume JNOW
                            {
                                xform.SetTopocentric(telescopeSlewState.RightAscension, telescopeSlewState.Declination);
                            }

                            // Get the Alt/Az corresponding to the telescope target RA/Dec
                            Point scopeTargetPosition = new Point(xform.AzimuthTopocentric, xform.ElevationTopocentric);

                            // Get the hour angle of the telescope's destination position.
                            double localHourAngle = TelescopeStatus.CalculateHourAngle(telescopeSlewState.RightAscension);

                            // Slave the dome to the target Alt/Az. The dome may or may not actually move depending on how far the new dome Alt/Az is from the current dome Alt/Az
                            SlaveDomePointing(scopeTargetPosition, localHourAngle, telescopeSlewState.SideOfPier);
                            retval = true;
                        }
                        catch (TransformUninitialisedException xcp)
                        {
                            LogActivityLine(msgType, "Attempting to calculate the slaved dome azimuth. Details follow:");
                            LogActivityLine(msgType, xcp.Message);
                            LogActivityLine(msgType, $"Transform Error site location:     latitude = {xform.SiteLatitude.ToDMS()}, longitude = {xform.SiteLongitude.ToDMS()}, elevation = {xform.SiteElevation:F0}");
                            LogActivityLine(msgType, $"Transform Error coordinate system: coordinateType = {coordinateType}");
                            LogActivityLine(msgType, $"Transform Error target position:   RA = {telescopeSlewState.RightAscension.ToHMS()}, Dec = {telescopeSlewState.Declination.ToDMS()}");
                        }
                        catch (Exception xcp)
                        {
                            LogActivityLine(msgType, "Attempting to calculate a new dome slave position due to telescope slew caught an exception. Details follow:");
                            LogActivityLine(msgType, xcp.Message);
                        }
                    }
                    else // The dome is slewing
                    {
                        // If we get here we need to slew to a new target because of a telescope slew, but the dome is already slewing.
                        // We want to test again quickly and slew the dome again as soon as it has stopped from the current slew.

                        returnTime = DateTime.Now.AddMilliseconds(250.0);
                    }
                }
                else // The telescope is not slewing
                {
                    if (DateTime.Now > nextAdjustmentTime)
                    {
                        if (!DomeStatus.Slewing || (TelescopeStatus.Slewing && Globals.UseCompositeSlewingFlag))
                        {
                            // Here is where we re-slew the dome to adjust for non-slew scope movement such as parking, tracking or jogging.

                            LogActivityLine(ActivityMessageTypes.Commands, "Dome position recalculation due to periodic timer expiration.");

                            try
                            {
                                SlaveDomePointing(new Point(TelescopeStatus.Azimuth, TelescopeStatus.Altitude), TelescopeStatus.LocalHourAngle, TelescopeStatus.SideOfPier);
                                retval = true;
                            }
                            catch (Exception xcp)
                            {
                                LogActivityLine(ActivityMessageTypes.Commands, "Attempting to calculate a new dome slave position caught an exception. Details follow:");
                                LogActivityLine(ActivityMessageTypes.Commands, xcp.Message);
                            }
                        }

                        TimeSpan syncSpan = new TimeSpan(0, 0, Globals.DomeLayoutSettings.SlaveInterval);
                        returnTime = DateTime.Now + syncSpan;
                    }
                }
            }
            else // Dome is not slaved to the telescope
            {
                // Set the time to min value to force us to sync the dome
                // as soon as the user enables slaving.

                returnTime = DateTime.MinValue;
            }

            nextAdjustmentTime = returnTime;

            return retval;
        }

        /// <summary>
        /// Synchronizes the dome's position with the telescope's pointing direction.
        /// </summary>
        /// <remarks>This method calculates the appropriate dome azimuth and altitude to align with the
        /// telescope's pointing direction and moves the dome as needed.  If the calculated azimuth or altitude values
        /// are invalid (e.g., out of range or <see cref="double.NaN"/>), the method will log an error and abort the
        /// operation.  The dome's azimuth and altitude are adjusted only if they deviate from the calculated target
        /// values by more than the configured accuracy thresholds.  If the dome is moved, the dome status is updated,
        /// and fast polling is enabled to monitor the movement.</remarks>
        /// <param name="scopePosition">The current position of the telescope in horizontal coordinates, where <c>X</c> represents azimuth and
        /// <c>Y</c> represents altitude.</param>
        /// <param name="localHourAngle">The local hour angle of the telescope, in hours. Negative values indicate positions east of the meridian,
        /// and positive values indicate positions west of the meridian.</param>
        /// <param name="sideOfPier">The telescope's reported pointing state. If <see cref="PierSide.pierUnknown"/>
        /// is provided, the method will attempt to calculate the side of the pier based on the local hour angle.</param>
        private void SlaveDomePointing(Point scopePosition, double localHourAngle, PierSide sideOfPier)
        {
            if (sideOfPier == PierSide.pierUnknown)
            {
                // We don't have side-of-pier from the telescope, so calculate it using the ASCOM convention that targets in the east are pierWest and targets in the West are pierEast.
                sideOfPier = (localHourAngle < 0.0) ? PierSide.pierWest : PierSide.pierEast; // Negative hour angle indicates that the target is in the east so pointing state is pierWest, otherwise pierEast.

                LogActivityLine(ActivityMessageTypes.Other, $"Dome Slaving calculated pier side as {sideOfPier}");
            }

            if (sideOfPier == PierSide.pierUnknown)
            {
                LogActivityLine(ActivityMessageTypes.Other, "Unable to slave the dome; pier side is unknown.");

                return;
            }

            LogActivityLine(ActivityMessageTypes.Other, $"Slaving the dome to telescope Az: {scopePosition.X.ToDMS()}, El: {scopePosition.Y.ToDMS()}, HA: {localHourAngle.ToHMS()}, SOP: {sideOfPier}.");

            // Calculate the dome target Az/El coordinates given the telescope Az/El coordinates, current hour angle and side of pier
            Point domeAltAz = GetDomeCoord(scopePosition, localHourAngle, sideOfPier);

            // Validate the calculated azimuth appears sane.
            // If the scope position is NaN or any other input to the slaving calculation is NaN then
            // the resultant targetAlt and targetAz could also be NaN. We do not want to send NaN to
            // the dome, even though it should reject it.

            double targetAzimuth = domeAltAz.X;
            double targetAltitude = domeAltAz.Y;
            bool targetsValid = true;

            if (Double.IsNaN(targetAzimuth) || targetAzimuth < 0.0 || targetAzimuth > 360.0)
            {
                targetsValid = false;
                LogActivityLine(ActivityMessageTypes.Commands, $"An invalid azimuth value ({targetAzimuth.ToDMS()}) was calculated...short circuiting the slew");
            }

            // Validate the calculated altitude appears sane.
            // Using 180 degrees for the upper limit for a clamshell type of dome that may be able to open to 180.

            if (Double.IsNaN(targetAltitude) || targetAltitude < 0.0 || targetAltitude > 180.0)
            {
                // Don't report a bad altitude if we are not going to set it anyway.

                if (Capabilities.CanSetAltitude)
                {
                    targetsValid = false;
                    LogActivityLine(ActivityMessageTypes.Commands, $"An invalid altitude value ({targetAltitude.ToDMS()}) was calculated...short circuiting the slew");
                }
            }

            // If either the target azimuth or altitude are invalid, return without slewing the dome!!!

            if (!targetsValid)
            {
                return;
            }

            bool moving = false;

            // Change the dome azimuth if necessary
            if (!IsInRange(targetAzimuth, DomeStatus.Azimuth, Globals.DomeLayoutSettings.AzimuthAccuracy)) // Change required
            {
                LogActivityLine(ActivityMessageTypes.Commands, $"The dome azimuth {DomeStatus.Azimuth.ToDMS()} is away from the target azimuth {targetAzimuth.ToDMS()} - slaving dome to the telescope azimuth.");

                SlewToAzimuth(targetAzimuth);
                moving = true;
            }
            else // No change required
                LogActivityLine(ActivityMessageTypes.Commands, $"The dome azimuth {DomeStatus.Azimuth.ToDMS()} is close to the target azimuth {targetAzimuth.ToDMS()} - no slew required.");

            // Change the dome altitude if necessary
            if (Capabilities.CanSetAltitude && DomeStatus.Altitude < targetAltitude) // Change required
            {
                LogActivityLine(ActivityMessageTypes.Commands, $"The dome altitude {DomeStatus.Altitude.ToDMS()} is below the target altitude {targetAltitude.ToDMS()} - opening the shutter further.");

                SlewToAltitude(targetAltitude);
                moving = true;
            }
            else // No change required
                LogActivityLine(ActivityMessageTypes.Commands, $"The dome altitude {DomeStatus.Altitude.ToDMS()} is above the target altitude {targetAltitude.ToDMS()} - no movement required.");

            // Update the dome status and set fast polling if the dome is changing azimuth or altitude 
            if (moving) // Part of the dome is moving
            {
                DomeStatus = new DevHubDomeStatus(this);
                SetFastPolling();
            }
        }

        /// <summary>
        /// Tests whether the target azimuth value is within the range of the current
        /// value +/- the accuracy. Also handles wraparound where the target and current values
        /// are on opposite sides of 0 degrees.
        /// </summary>
        /// <param name="targetValue">the target azimuth value</param>
        /// <param name="currentValue">the current azimuth value</param>
        /// <param name="accuracy">the range threshold</param>
        /// <returns> true if the target is in the range of current +/- accuracy</returns>
        private bool IsInRange(double targetValue, double currentValue, double accuracy)
        {
            bool retval;

            // First handle wraparound.

            if (currentValue >= targetValue)
            {
                while (currentValue - targetValue > 180.0)
                {
                    currentValue -= 360.0;
                }
            }
            else
            {
                while (targetValue - currentValue > 180.0)
                {
                    currentValue += 360.0;
                }
            }

            retval = (Math.Abs(targetValue - currentValue) <= accuracy);

            return retval;
        }

        private void UpdateDomeStatusTask()
        {
            DevHubDomeStatus sts = null;
            Exception except = null;

            try
            {
                sts = new DevHubDomeStatus(this);
            }
            catch (Exception xcp)
            {
                except = xcp;
            }

            if (sts == null)
            {
                LogActivityLine(ActivityMessageTypes.Status, "Attempting to update the dome status caught an exception. Details follow:");
                LogActivityLine(ActivityMessageTypes.Status, except.Message);
            }
            else
            {
                Globals.LatestRawDomeStatus = sts.Clone();

                AdjustForCompositeSlewing(sts);
                DomeStatus = sts;

                StatusUpdated = true;
                Messenger.Default.Send(new DomeStatusUpdatedMessage(sts));
            }
        }

        private void AdjustForCompositeSlewing(DevHubDomeStatus sts)
        {
            if (Globals.LatestRawTelescopeStatus != null && Globals.IsDomeSlaved && Globals.UseCompositeSlewingFlag)
            {
                if (Globals.LatestRawTelescopeStatus.Slewing)
                {
                    sts.Slewing = true;
                }
            }
        }

        private void StopDevicePolling()
        {
            if (!Service.DeviceAvailable || !Connected)
            {
                return;
            }

            if (IsPolling)
            {
                PollingTokenSource.Cancel();
                PollingTask.Wait();
                PollingTokenSource.Dispose();
                PollingTokenSource = null;
            }
        }

        private void ReadInitialDomeDataTask()
        {
            // This task talks to the dome from a worker thread, but updates the U/I on the main thread.

            // Wait a second for the dome to settle before reading the data.

            Thread.Sleep(1000);

            Capabilities = new DomeCapabilities();
            Capabilities.InitializeFromManager(this);

            Parameters = new DomeParameters();
            Parameters.InitializeFromManager(this);

            DevHubDomeStatus status = new DevHubDomeStatus(this);

            Messenger.Default.Send(new DomeCapabilitiesUpdatedMessage(Capabilities.Clone()));
            Messenger.Default.Send(new DomeParametersUpdatedMessage(Parameters.Clone()));
            DomeStatus = status;
            Messenger.Default.Send(new DomeStatusUpdatedMessage(DomeStatus));
        }

        /// <summary>
        /// Calculate the pointing position of the dome given the pointing position of the telescope, 
        /// the hour angle in hours, and the side-of-pier
        /// </summary>
        /// <param name="scopePosition">the azimuth and altitude of the telescope</param>
        /// <param name="hourAngle">the hour angle in decimal hours</param>
        /// <param name="sideOfPier">the side of pier of the telescope</param>
        /// <returns>Point struct containing the azimuth and altitude of the dome</returns>
        private Point GetDomeCoord(Point scopePosition, double hourAngle, PierSide sideOfPier)
        {
            Point domePoth = new Point(0, 0), domeHub = new Point(0, 0), domeRevised = new Point(0, 0), domePosition = new Point(0, 0);

            LogActivityLine(ActivityMessageTypes.Other, $"  Use POTH: {Globals.UsePOTHDomeSlaveCalculation}, Use Revised: {Globals.UseRevisedDomeSlaveCalculation}");
            try
            {
                // Calculate the dome position using the POTH method.
                DomeControl dc = new DomeControl(Globals.DomeLayoutSettings, TelescopeParameters.SiteLatitude);
                domePoth = dc.DomePosition(scopePosition, hourAngle * Globals.HRS_TO_DEG, sideOfPier == PierSide.pierWest);
            }
            catch (Exception ex)
            {
                LogActivityLine(ActivityMessageTypes.Other, $"***** GetDomeCoord POTH Error - {ex.Message}");
            }

            try
            {
                // Calculate the dome position using the original Device Hub method.
                DomeSynchronize dsync = new DomeSynchronize(Globals.DomeLayoutSettings, TelescopeParameters.SiteLatitude);
                domeHub = dsync.DomePosition(scopePosition, hourAngle * Globals.HRS_TO_DEG, sideOfPier == PierSide.pierWest);
            }
            catch (Exception ex)
            {
                LogActivityLine(ActivityMessageTypes.Other, $"***** GetDomeCoord Hub Error - {ex.Message}");
            }

            try
            {
                // Calculate the dome position using the new Device Hub method.
                domeRevised = DomePosition(scopePosition, hourAngle, sideOfPier);
            }
            catch (Exception ex)
            {
                LogActivityLine(ActivityMessageTypes.Other, $"***** GetDomeCoord Hub2 Error - {ex.Message}");
            }

            // Compare results from the three calculation methods
            LogActivityLine(ActivityMessageTypes.Other, $"  Dome Position - POTH:  {domePoth.X.ToDMS()}, {domePoth.Y.ToDMS()} ({domePoth.X:0.0}, {domePoth.Y:0.0})");
            LogActivityLine(ActivityMessageTypes.Other, $"  Dome Position - Hub:    {domeHub.X.ToDMS()}, {domeHub.Y.ToDMS()} ({domeHub.X:0.0}, {domeHub.Y:0.0})");
            LogActivityLine(ActivityMessageTypes.Other, $"  Dome Position - Hub 2: {domeRevised.X.ToDMS()}, {domeRevised.Y.ToDMS()} ({(domeHub.X - domeRevised.X).ToDMS()} {(domeHub.Y - domeRevised.Y).ToDMS()}) ({domeRevised.X:0.0}, {domeRevised.Y:0.0})");

            // Select the appropriate dome position based on the configuration setting
            if (Globals.UseRevisedDomeSlaveCalculation) // Revised calculation
            {
                domePosition = domeRevised;
            }
            else if (Globals.UsePOTHDomeSlaveCalculation) // POTH calculation
            {
                domePosition = domePoth;
            }
            else // Original Device Hub calculation
            {
                domePosition = domeHub;
            }

            LogActivityLine(ActivityMessageTypes.Other, $"  Dome Position - Using:  {domePosition.X.ToDMS()}, {domePosition.Y.ToDMS()} ({domePosition.X:0.0}, {domePosition.Y:0.0})");

            // Apply any configured azimuth correction and ensure the resulting value is between 0.0 and 359.999...
            domePosition.X += Globals.DomeAzimuthAdjustment;
            domePosition.X = Globals.Condition0To359(domePosition.X);

            return domePosition;
        }

        /// <summary>
        /// Calculates the dome's position using the new algorithm based on the telescope's position, hour angle, and pier side.
        /// </summary>
        /// <remarks>This method determines the dome's position by synchronizing it with the telescope's
        /// alignment mode and position.  It supports both Alt/Az and equatorial alignment modes, including German
        /// equatorial mounts. The calculation accounts  for the telescope's mechanical offsets and the dome's physical
        /// layout, such as radius and axis offsets. 
        /// <para> If an error occurs during the calculation, the method returns a default position of (0.0, 0.0) and sets a synchronization error flag. </para>
        /// </remarks>
        /// <param name="scopePosition">The current position of the telescope, where <c>X</c> represents the azimuth or right ascension, and <c>Y</c> represents the altitude or declination.</param>
        /// <param name="hourAngle">The hour angle of the telescope, in hours.</param>
        /// <param name="sideOfPier">The pointing state of the telescope.</param>
        /// <returns>A <see cref="Point"/> representing the calculated dome position, where <c>X</c> is the azimuth and <c>Y</c> is the altitude.</returns>
        private Point DomePosition(Point scopePosition, double hourAngle, PierSide sideOfPier)
        {
            Point domeCoordinates = new Point(0.0, 0.0);

            try
            {
                // NOTE: The ROLL axis is the Right ascension / Azimuth axis, and the PITCH axis is the Declination / Altitude axis.

                double PHI, scopeRollAngle, scopePitchAngle;
                switch (TelescopeParameters.AlignmentMode)
                {
                    case AlignmentModes.algAltAz: // Altitude/Azimuth alignment

                        // Set the value of PHI to 90 for Alt/Az
                        PHI = 90.0;

                        // Determine the roll and pitch angles of the scope
                        scopeRollAngle = 180.0 - scopePosition.X; // Azimuth
                        scopeRollAngle = Globals.Condition0To359(scopeRollAngle);
                        scopePitchAngle = scopePosition.Y; // Altitude

                        LogActivityLine(ActivityMessageTypes.Other, $"  Alignment Mode: {TelescopeParameters.AlignmentMode}, HA {hourAngle.ToHMS()}, SideOfPier: {sideOfPier}, ScopePosition X: {scopePosition.X} ({(scopePosition.X).ToDMS()}), ScopePositionX: {scopeRollAngle} ({(scopeRollAngle).ToDMS()})");
                        LogActivityLine(ActivityMessageTypes.Other, $"  ScopePosition.Y: {scopePosition.Y} ({scopePosition.Y.ToDMS()}), ScopePositionY: {scopePitchAngle} ({scopePitchAngle.ToDMS()})");

                        // Convert angles to radians
                        scopeRollAngle *= Globals.DEG_TO_RAD;
                        scopePitchAngle *= Globals.DEG_TO_RAD;
                        break;

                    case AlignmentModes.algPolar: // Equatorial alignment
                    case AlignmentModes.algGermanPolar: // German equatorial alignment
                                                        // Set PHI to the site latitude
                        PHI = TelescopeParameters.SiteLatitude;

                        // Determine the mechanical hour angle (both hemispheres)
                        double mechanicalHourAngle = sideOfPier == PierSide.pierEast ? hourAngle : hourAngle + 12.0;

                        // Determine the mechanical declination, which varies by hemisphere
                        double mechanicalDeclination;
                        if (PHI >= 0.0) // Northern hemisphere
                        {
                            mechanicalDeclination = sideOfPier == PierSide.pierEast ? TelescopeStatus.Declination : 180.0 - TelescopeStatus.Declination;
                        }
                        else // Southern hemisphere
                        {
                            mechanicalDeclination = sideOfPier == PierSide.pierEast ? TelescopeStatus.Declination : -180.0 - TelescopeStatus.Declination;
                        }

                        // Determine the roll and pitch angles of the scope
                        scopeRollAngle = -mechanicalHourAngle;
                        scopePitchAngle = mechanicalDeclination;

                        LogActivityLine(ActivityMessageTypes.Other, $"  Alignment Mode: {TelescopeParameters.AlignmentMode}, Hour Angle: {hourAngle.ToHMS()}, Mechanical hour angle: {mechanicalHourAngle.ToHMS()}");
                        LogActivityLine(ActivityMessageTypes.Other, $"  Declination: {TelescopeStatus.Declination.ToDMS()}, Mechanical declination: {mechanicalDeclination.ToDMS()}");

                        LogActivityLine(ActivityMessageTypes.Other, $"  SideOfPier: {sideOfPier}, Roll angle (hours): {scopeRollAngle} ({scopeRollAngle.ToHMS()})");
                        LogActivityLine(ActivityMessageTypes.Other, $"  Pitch angle (degrees): {scopePitchAngle} ({scopePitchAngle.ToDMS()})");

                        // Convert the scope's mechanical hour angle and declination to radians
                        scopeRollAngle *= Globals.HRS_TO_RAD;
                        scopePitchAngle *= Globals.DEG_TO_RAD;
                        break;

                    default: // All other alignments
                        throw new DriverException($"Device Hub GetDomeCoord - Unsupported telescope alignment type: {TelescopeParameters.AlignmentMode}");
                }

                // Calculate the dome position using the DomeSyncWallace class
                domeCoordinates = DomeSynchromise2.DomePosition(
                    PHI * Globals.DegRad, // PHI - Site latitude for all equatorial mount types, 90 degrees for alt/az mounts
                    Globals.DomeLayoutSettings.DomeRadius, // Dome radius (mm)
                    Globals.DomeLayoutSettings.DomeScopeOffset.X, // xm - East /west offset of the axis intersection from the dome centre (mm)
                    Globals.DomeLayoutSettings.DomeScopeOffset.Y, // ym - North/south offset of the axis intersection from the dome centre (mm)
                    Globals.DomeLayoutSettings.DomeScopeOffset.Z, // zm - Up / down offset of the axis intersection from the dome centre (mm)
                    Globals.DomeLayoutSettings.GemAxisOffset,     // xt - Offset of the optical axis from the roll axis (right ascension or azimuth axis) (mm) 
                    0.0,                                  // yt - Offset of the pitch (dec/alt) axis from the roll (RA/az) axis. Usually zero in amateur mounts, can be non-zero in some horseshoe designs (mm)
                    Globals.DomeLayoutSettings.OpticalOffset,     // yo - Offset of the optical axis from the nearest point of approach of the declination / altitude axis (mm)
                    scopeRollAngle,                       // roll axis angle (ha/az) (radians)
                    scopePitchAngle);                     // pitch axis angle (dec/alt) (radians)

                SendSyncErrorState(false); // Set the sync error flag to false if the calculation is successful

                LogActivityLine(ActivityMessageTypes.Other, $"  PHI: {PHI}, Dome radius: {Globals.DomeLayoutSettings.DomeRadius}");
                LogActivityLine(ActivityMessageTypes.Other, $"  X offset: {Globals.DomeLayoutSettings.DomeScopeOffset.X}, Y offset: {Globals.DomeLayoutSettings.DomeScopeOffset.Y}, Z offset: {Globals.DomeLayoutSettings.DomeScopeOffset.Z}");
                LogActivityLine(ActivityMessageTypes.Other, $"  GEM axis offset: {Globals.DomeLayoutSettings.GemAxisOffset}, Optical offset: {Globals.DomeLayoutSettings.OpticalOffset}");
            }
            catch (Exception ex)
            {
                LogActivityLine(ActivityMessageTypes.Other, $"***** DomePosition Error - {ex.Message}");
                domeCoordinates.X = 0.0;
                domeCoordinates.Y = 0.0;
                SendSyncErrorState(true);  // Set the sync error flag to true if we have an error calculating the dome position
            }

            return domeCoordinates;
        }

        /// <summary>
        /// Send a update notification message if the dome sync status has changed.
        /// </summary>
        /// <param name="state"></param>
        private void SendSyncErrorState(bool state)
        {
            LogActivityLine(ActivityMessageTypes.Other, $"  SendSyncErrorState - New: {state}, Current: {Globals.DomeSyncError} ");

            if (state != Globals.DomeSyncError)
            {
                Globals.DomeSyncError = state; // Update the sync error state in the dome status

                Messenger.Default.Send(new DomeSyncErrorStateMessage(state));
                LogActivityLine(ActivityMessageTypes.Other, $"  SendSyncErrorState - Sent DomeSyncErrorStateMessage({state}");
            }
        }

        private void UpdateTelescopeParameters(TelescopeParametersUpdatedMessage action)
        {
            // Make sure that we update the Parameters on the U/I thread.

            Task.Factory.StartNew(() => TelescopeParameters = action.Parameters, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext);
        }

        private void UpdateTelescopeStatus(TelescopeStatusUpdatedMessage action)
        {
            TelescopeStatus = action.Status;
        }

        private void ClearTelescopeStatus(DeviceDisconnectedMessage action)
        {
            if (action.DeviceType == DeviceTypeEnum.Telescope)
            {
                TelescopeStatus = null;
            }
        }

        /// <summary>
        /// Here we are notified when a telescope slew is initiated.
        /// </summary>
        /// <param name="action"></param>
        private void InitiateSlavedSlew(SlewInProgressMessage action)
        {
            // Save the message data and wake up the polling loop.
            LogActivityLine(ActivityMessageTypes.Other, $"Received a telescope slew in progress message - RA: {action.RightAscension.ToHMS()}, Declination: {action.Declination.ToDMS()}, Side of pier: {action.SideOfPier}, Is slew in progress: {action.IsSlewInProgress}");
            TelescopeSlewState = action;
            SetFastPolling();

            // Ensure that the next dome slaved update operates on the coordinates received from the telescope regardless of whether or not the dome is already moving
            if (!(action is null))
            {
                if (Globals.IsDomeSlaved & action.IsSlewInProgress)
                {
                    LogActivityLine(ActivityMessageTypes.Other, $"Setting ForceSlavedUpdate to True");
                    ForceSlavedUpdate = true;
                }
            }
        }

        #endregion Helper Methods
    }
}
