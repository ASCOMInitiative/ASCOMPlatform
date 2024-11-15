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
            bool previousMoveStatus = false;
            DateTime returnToNormalPollingTime = DateTime.MinValue;
            int previousPollingPeriod;

            while (!taskCancelled)
            {
                DateTime wakeupTime = DateTime.Now;
                //Debug.WriteLine( $"Awakened @ {wakeupTime:hh:mm:ss.fff}." );
                previousPollingPeriod = PollingPeriod;
                PollingPeriod = POLLING_INTERVAL_NORMAL;
                int fastPollingMs = Convert.ToInt32(FastPollingPeriod * 1000.0);

                if (Service.DeviceAvailable)
                {
                    UpdateDomeStatusTask();

                    if (!DomeStatus.Slewing)
                    {
                        if (SlewTheSlavedDome(ref nextSlaveAdjustmentTime))
                        {
                            UpdateDomeStatusTask();
                        }
                    }


                    bool isMoving = DomeStatus.Slewing
                                || DomeStatus.ShutterStatus == ShutterState.shutterOpening
                                || DomeStatus.ShutterStatus == ShutterState.shutterClosing;

                    LogAppMessage($"SlavedSlewState.IsSlewInProgress: {TelescopeSlewState.IsSlewInProgress}, SlavedSlewState.RA: {TelescopeSlewState.RightAscension}, SlavedSlewState.Declination: {TelescopeSlewState.Declination}, isMoving: {isMoving}, Slewing: {DomeStatus.Slewing}, ShutterStatus{DomeStatus.ShutterStatus}", "PollDomeTask");

                    if (isMoving)
                    {
                        // We are moving, so use the fast polling rate.

                        PollingPeriod = fastPollingMs;
                    }
                    else if (previousMoveStatus)
                    {
                        // We stopped moving, so start the timer to return to normal polling.

                        returnToNormalPollingTime = DateTime.Now + fastPollExtension;
                        PollingPeriod = fastPollingMs;
                    }
                    else if (DateTime.Now < returnToNormalPollingTime)
                    {
                        // Continue fast polling.

                        PollingPeriod = fastPollingMs;
                    }
                    else
                    {
                        // Return to normal polling.

                        returnToNormalPollingTime = DateTime.MinValue;
                    }

                    // Remember our state for the next time through this loop.

                    previousMoveStatus = isMoving;

                    if (PollingPeriod == POLLING_INTERVAL_NORMAL && previousPollingPeriod != POLLING_INTERVAL_NORMAL)
                    {
                        LogActivityLine(ActivityMessageTypes.Commands, $"Returning to normal polling every {PollingPeriod} ms.");
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

            IsPolling = false;
        }

        private bool SlewTheSlavedDome(ref DateTime nextAdjustmentTime)
        {
            bool retval = false;
            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            DateTime returnTime = nextAdjustmentTime;

            // Check whether the done is slaved to the telescope
            if (Globals.IsDomeSlaved) // Dome is slaved to the telescope
            {
                // Whenever the scope is slewed, we get a message with the target RA, DEC and side-of-pier.
                // We need to slew there immediately. We will get another message when the slew has finished, but
                // until then we need to suspend normal slaved adjustments.

                SlewInProgressMessage telescopeSlewState = TelescopeSlewState;

                // Check whether the telescope is currently slewing
                if (telescopeSlewState != null && telescopeSlewState.IsSlewInProgress) // The telescope is slewing
                {
                    // Check whether the dome is slewing
                    if (!DomeStatus.Slewing || Globals.UseCompositeSlewingFlag) // The dome is not slewing
                    {
                        // The telescope is slewing and we are slaved but not slewing, so calculate the current Alt/Az coordinates corresponding to the scope's target RA/Dec and slew the dome to the Alt/Az position if required.

                        LogActivityLine(msgType, "Dome position recalculation due to telescope slew-in-progress.");

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
                            double localHourAngle = TelescopeStatus.CalculateHourAngle(TelescopeSlewState.RightAscension);

                            // Slave the dome to the target Alt/Az. The dome may or may not actually move depending on how far the new dome Alt/Az is from the current dome Alt/Az
                            SlaveDomePointing(scopeTargetPosition, localHourAngle, TelescopeSlewState.SideOfPier);
                            retval = true;
                        }
                        catch (TransformUninitialisedException xcp)
                        {
                            LogActivityLine(msgType, "Attempting to calculate the slaved dome azimuth. Details follow:");
                            LogActivityLine(msgType, xcp.Message);
                            LogActivityLine(msgType, $"Transform Error site location:     latitude = {xform.SiteLatitude:F5}, longitude = {xform.SiteLongitude:F5}, elevation = {xform.SiteElevation:F0}");
                            LogActivityLine(msgType, $"Transform Error coordinate system: coordinateType = {coordinateType}");
                            LogActivityLine(msgType, $"Transform Error target position:   RA = {telescopeSlewState.RightAscension}, Dec = {telescopeSlewState.Declination}");
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
                            // Here is where we re-slew the dome to adjust for non-slew scope movement such as parking,
                            // tracking, or jogging.

                            LogActivityLine(ActivityMessageTypes.Commands, "Dome position recalculation due to periodic timer expiration.");

                            Point scopePosition = new Point(TelescopeStatus.Azimuth, TelescopeStatus.Altitude);

                            try
                            {
                                SlaveDomePointing(scopePosition, TelescopeStatus.LocalHourAngle, TelescopeStatus.SideOfPier);
                                retval = true;
                            }
                            catch (Exception xcp)
                            {
                                LogActivityLine(ActivityMessageTypes.Commands, "Attempting to calculate a new dome slave position caught an exception. Details follow:");
                                LogActivityLine(ActivityMessageTypes.Commands, xcp.Message);
                            }
                        }

                        TimeSpan syncSpan = new TimeSpan(0, 0, Globals.DomeLayout.SlaveInterval);
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

        private void SlaveDomePointing(Point scopePosition, double localHourAngle, PierSide sideOfPier)
        {
            if (sideOfPier == PierSide.pierUnknown)
            {
                // We don't have side-of-pier from the telescope, so calculate it.

                sideOfPier = (localHourAngle < 0.0) ? PierSide.pierWest : PierSide.pierEast;

                LogActivityLine(ActivityMessageTypes.Commands, "Dome Slaving calculated pier side as {0}", sideOfPier);
            }

            if (sideOfPier == PierSide.pierUnknown)
            {
                LogActivityLine(ActivityMessageTypes.Commands, "Unable to slave the dome; pier side is unknown.");

                return;
            }

            LogActivityLine(ActivityMessageTypes.Commands, "Slaving the dome to telescope Az: {0:f2}, Alt: {1:f2}, HA: {2:f5}, SOP: {3}.", scopePosition.X, scopePosition.Y, localHourAngle, sideOfPier);

            Point domeAltAz = GetDomeCoord(scopePosition, localHourAngle, sideOfPier);
            double targetAzimuth = domeAltAz.X;
            double targetAltitude = domeAltAz.Y;

            bool targetsValid = true;

            // Validate the calculated azimuth appears sane.
            // If the scope position is NaN or any other input to the slaving calculation is NaN then
            // the resultant targetAlt and targetAz could also be NaN. We do not want to send NaN to
            // the dome, even though it should reject it.

            if (Double.IsNaN(targetAzimuth) || targetAzimuth < 0.0 || targetAzimuth > 360.0)
            {
                targetsValid = false;
                LogActivityLine(ActivityMessageTypes.Commands, "An invalid azimuth value ({0:f2}) was calculated...short circuiting the slew"
                                , targetAzimuth);
            }

            // Validate the calculated altitude appears sane.
            // Using 180 degrees for the upper limit for a clamshell type of dome that may be able to open to 180.

            if (Double.IsNaN(targetAltitude) || targetAltitude < 0.0 || targetAltitude > 180.0)
            {
                // Don't report a bad altitude if we are not going to set it anyway.

                if (Capabilities.CanSetAltitude)
                {
                    targetsValid = false;
                    LogActivityLine(ActivityMessageTypes.Commands, "An invalid altitude value ({0:f2}) was calculated...short circuiting the slew", targetAltitude);
                }
            }

            // If either the target azimuth or altitude are invalid, return without slewing the dome!!!

            if (!targetsValid)
            {
                return;
            }

            LogActivityLine(ActivityMessageTypes.Commands, "The calculated dome position is Az: {0:f2}, Alt: {1:f2}.", domeAltAz.X, domeAltAz.Y);

            bool moving = false;

            if (IsInRange(targetAzimuth, DomeStatus.Azimuth, Globals.DomeLayout.AzimuthAccuracy))
            {
                LogActivityLine(ActivityMessageTypes.Commands, "The dome azimuth is close to the target azimuth...no slew is needed.");
            }
            else
            {
                LogActivityLine(ActivityMessageTypes.Commands, "Slaving dome to target azimuth of {0:f2} degrees.", targetAzimuth);

                //Task task = Task.Run( () =>
                //{
                //	SlewToAzimuth( targetAzimuth );
                //	Status.Slewing = true;
                //	SetFastPolling();
                //}, CancellationToken.None );
                SlewToAzimuth(targetAzimuth);
                moving = true;
            }

            if (Capabilities.CanSetAltitude && DomeStatus.Altitude < targetAltitude)
            {
                LogActivityLine(ActivityMessageTypes.Commands, "Synchronizing dome to target altitude of {0:f2} degrees.", targetAltitude);

                //Task task = Task.Run( () =>
                //{
                //	SlewToAltitude( targetAltitude );
                //}, CancellationToken.None );
                SlewToAltitude(targetAltitude);
                moving = true;
            }

            if (moving)
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
                string msg = xcp.ToString();
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
            Point domePosition = new Point(0, 0);

            if (Globals.UsePOTHDomeSlaveCalculation)
            {
                DomeControl dc = new DomeControl(Globals.DomeLayout, TelescopeParameters.SiteLatitude);
                domePosition = dc.DomePosition(scopePosition, hourAngle * Globals.HRS_TO_DEG, sideOfPier == PierSide.pierWest);
            }
            else
            {
                DomeSynchronize dsync = new DomeSynchronize(Globals.DomeLayout, TelescopeParameters.SiteLatitude);
                domePosition = dsync.DomePosition(scopePosition, hourAngle * Globals.HRS_TO_DEG, sideOfPier == PierSide.pierWest);
            }

            domePosition.X += Globals.DomeAzimuthAdjustment;

            return domePosition;
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

        private void InitiateSlavedSlew(SlewInProgressMessage action)
        {
            // Here we are notified when a telescope slew is initiated.
            // Save the message data and wake up the polling loop.

            TelescopeSlewState = action;
            SetFastPolling();
        }

        #endregion Helper Methods
    }
}
