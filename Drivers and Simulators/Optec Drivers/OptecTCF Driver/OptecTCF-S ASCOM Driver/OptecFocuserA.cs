using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.ComponentModel;
using System.Diagnostics;
using Optec;
using System.Threading;
using System.Globalization;

namespace Optec_TCF_S_Focuser
{
    public sealed class OptecFocuser
    {

        private static readonly OptecFocuser myFocuser = new OptecFocuser();
        private SerialCommunicator mySerialCommunicator;
        //System.Windows.Forms.Timer RefreshTimer = new System.Windows.Forms.Timer();
        BackgroundWorker RefreshBGWorker = new BackgroundWorker();
        private bool RefreshDone;
        //bool pauseRefresh = false;
        private bool isMoving = false;
        public event EventHandler DeviceStatusChanged;
        public event EventHandler ErrorOccurred;
        private int ConsecutivePosTimeouts_AutoMode = 0;
        //   private int ConsecutiveTempTimeouts_AutoMode = 0;
        private int ConsecutiveTimeouts_SerialMode = 0;


        private const double FIRST_TCFS_WITH_BACKLASH = 4.11;
        private const double FIRST_TCFSI_WITH_BACKLASH = 3.11;
        private const double FIRST_TCFS_WITH_DISPLAY_ADJUST = 4.00;
        private const double FIRST_TCFS_CAN_REPORT_BRIGHTNESS = 4.01;
        private const double FIRST_TCFSI_CAN_REPORT_BRIGHTNESS = 3.10;

        private const double STEP_SIZE_MICRONS_TCF_S = 2.177;
        private const double STEP_SIZE_MICROS_TCF_S3 = 2.54;

#if DEBUG
        private const int SERIAL_MODE_REFRESH_DELAY = 500;
#else
        private const int SERIAL_MODE_REFRESH_DELAY = 200;
#endif

        public const int TEMP_WHEN_PROBE_DISABLED = -274;


        private OptecFocuser()
        {
            // First set the desired trace level.
            RefreshDone = true;
            EventLogger.LoggingLevel = XMLSettings.LoggerTraceLevel;
            EventLogger.LogMessage("Creating Instance of Optec Focuser", TraceLevel.Info);
            connectionState = ConnectionStates.Disconnected;
            mySerialCommunicator = SerialCommunicator.Instance;

            //RefreshTimer.Interval = SERIAL_MODE_REFRESH_DELAY;
            // RefreshTimer. = false;
            //RefreshTimer.Tick += new EventHandler(RefreshTimer_Elapsed);
            RefreshBGWorker.WorkerSupportsCancellation = true;
            RefreshBGWorker.DoWork += new DoWorkEventHandler(RefreshBGWorker_DoWork);
            RefreshBGWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RefreshBGWorker_RunWorkerCompleted);

            // Get the settings stored in the xml file.
            this.displayBrightness = XMLSettings.DisplayBrightness;
            this.tempCompMode = XMLSettings.TempCompMode;
            this.temperatureOffsetC = XMLSettings.TemperatureOffsetC;
            this.displayPositionUnits = XMLSettings.DisplayPositionUnits;
            this.displayTempUnits = XMLSettings.DisplayTempUnits;
            this.autoADelay = XMLSettings.AutoADelay;
            this.autoBDelay = XMLSettings.AutoBDelay;
            this.savedSerialPortName = XMLSettings.SavedSerialPortName;
            try
            {
                mySerialCommunicator.PortName = savedSerialPortName;
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
            }
        }

        void RefreshBGWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    TriggerAnEvent(ErrorOccurred, e.Error);

                    if (e.Error.GetType() == typeof(ER1_Exception))
                    {
                        RefreshBGWorker.RunWorkerAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
            }

        }

        void RefreshBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            RefreshDone = false;
            while (true)
            {
                try
                {

                    if (RefreshBGWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    // Do the refresh
                    DoRefresh();

                    // Sleep for a brief time before refreshing
                    System.Threading.Thread.Sleep(SERIAL_MODE_REFRESH_DELAY);
                    if (RefreshBGWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    System.Windows.Forms.Application.DoEvents();
                }
                catch (Exception ex)
                {
                    EventLogger.LogMessage(ex);
                    RefreshDone = true;
                    throw ex;
                }
            }

            RefreshDone = true;
        }

        private void DoRefresh()
        {
            mySerialCommunicator = SerialCommunicator.Instance;
            if (connectionState == ConnectionStates.SerialMode)
            {
                RefreshDeviceStatus();
            }
            else if (connectionState == ConnectionStates.TempCompMode)
            {
                RefreshDeviceStatusAutoMode();
            }
        }

        //void RefreshTimer_Elapsed(object sender, EventArgs e)
        //{
        //    //if (RefreshBGWkr.IsBusy) return;
        //    // else RefreshBGWkr.RunWorkerAsync();
        //    Trace.WriteLine("Refresh Timer Elapsed, starting refresh");
        //    StartRefresh();
        //    if (!pauseRefresh)
        //    {
        //        Trace.WriteLine("Resetting timer in RefreshTimer_Elapsed");
        //        RefreshTimer.Start();
        //    }
        //}

        [DisplayName("Step Size (µm)")]
        [Category("Device Status")]
        [Description("The amount of travel the focuser moves per step in Microns")]
        public double StepSize
        {
            get
            {
                switch (deviceType)
                {
                    case DeviceTypes.TCF_S:
                        return STEP_SIZE_MICRONS_TCF_S;
                    case DeviceTypes.TCF_Si:
                        return STEP_SIZE_MICRONS_TCF_S;
                    case DeviceTypes.TCF_S3:
                        return STEP_SIZE_MICROS_TCF_S3;
                    case DeviceTypes.TCF_S3i:
                        return STEP_SIZE_MICROS_TCF_S3;
                    default:
                        throw new InvalidEnumArgumentException("Invalid deviceType received in StepSize property");
                }
            }
        }

        private double currentPosition = 0;
        [DisplayName("Current Position")]
        [Category("Device Status")]
        [Description("The current position, in steps, of the connected focuser.")]
        public double CurrentPosition
        {
            get
            {
                if (connectionState == ConnectionStates.Disconnected) return 0;
                else return currentPosition;
            }
        }
        [DisplayName("Current Pos. for Display")]
        [Category("Device Status")]
        [Description("Set the units for the position in the application readout (NOT the device readout). If you are using the TCF-S ASCOM driver, this will not affect the driver functionality.")]
#if ASCOM
        [Browsable(false)]
#else
        [Browsable(true)]
#endif
        public string CurrentPositionForDisplay
        {
            get
            {
                if (connectionState == ConnectionStates.Disconnected) return "------";
                string pos = "-----";
                switch (displayPositionUnits)
                {
                    case PositionUnits.Microns:
                        // Convert the current position to Microns
                        double posMic = currentPosition * StepSize;
                        pos = posMic.ToString("0000");
                        break;
                    case PositionUnits.Steps:
                        pos = currentPosition.ToString("0000");
                        break;
                    default:
                        throw new InvalidEnumArgumentException("Invalid value for displayPositionUnits");
                }
                return pos;
            }
        }

        private int targetPosition = 0;
        [Browsable(false)]  // No need to show this to the user
        public int TargetPosition
        {
            get
            {
                if (connectionState == ConnectionStates.Disconnected)
                {
                    EventLogger.LogMessage("Attempted to get Target Postion while not connected", TraceLevel.Warning);
                    return 0;
                }
                return targetPosition;
            }
            set
            {
                try
                {
                    EventLogger.LogMessage("Setting Target Position to " + value.ToString(), TraceLevel.Info);
                    if (connectionState != ConnectionStates.SerialMode)
                        throw new InvalidOperationException("The device must be in Serial Mode to perform this operation.");
                    if (value > MaxSteps) targetPosition = MaxSteps;
                    else if (value < 1) targetPosition = 1;
                    else if (value == currentPosition) return;
                    else targetPosition = value;
                    EventLogger.LogMessage("Target Position set to " + targetPosition.ToString(), TraceLevel.Verbose);
                }
                catch (Exception ex)
                {
                    EventLogger.LogMessage(ex);
                    throw;
                }
            }
        }

        private double currentTemperature = 0;
        [DisplayName("Current Temp (°­C)")]
        [Category("Device Status")]
        [Description("The current temperature reported by the focuser and adjusted by the Temperature Offset constant.")]
        public double CurrentTemperature
        {
            get
            {
                if (connectionState == ConnectionStates.Disconnected) return 0;
                if (tempProbeDisabled)
                {
                    currentTemperature = TEMP_WHEN_PROBE_DISABLED;
                    return currentTemperature;
                }
                else return currentTemperature + temperatureOffsetC;

            }
        }
        [DisplayName("Current Temp. for Display")]
        [Category("Device Status")]
        [Description("The current temperature adjusted for the selected units for the display")]
#if ASCOM
        [Browsable(false)]
#else
        [Browsable(true)]
#endif
        public string CurrentTempForDisplay
        {
            get
            {
                string t = "------";
                if (connectionState == ConnectionStates.Disconnected) return t;
                if (currentTemperature == TEMP_WHEN_PROBE_DISABLED) return t;
                switch (displayTempUnits)
                {
                    case TemperatureUnits.Celsius:
                        t = CurrentTemperature.ToString("00.0°C");
                        break;
                    case TemperatureUnits.Fahrenheit:
                        double tempF = CurrentTemperature * (double)9 / (double)5 + 32;
                        t = tempF.ToString("00.0°F");
                        break;
                    case TemperatureUnits.Kelvin:
                        double tempK = CurrentTemperature + 273;
                        t = tempK.ToString("00.0 K");
                        break;
                    default:
                        throw new InvalidEnumArgumentException("Invalid value for DisplayTempUnits");
                }
                return t;
            }
        }

        public static OptecFocuser Instance
        {
            get { return myFocuser; }
        }

        public enum ConnectionStates
        {
            Disconnected,
            SerialMode,
            TempCompMode,
            Sleep
        }

        private ConnectionStates connectionState;
        [Browsable(false)]
        public ConnectionStates ConnectionState
        {
            get
            {
                return connectionState;
            }
            set
            {
                ChangeConnectionState(value);
                TriggerAnEvent(DeviceStatusChanged);
            }
        }

        private void ChangeConnectionState(ConnectionStates value)
        {
            switch (value)
            {
                #region Disconnected
                case ConnectionStates.Disconnected:
                    try
                    {
                        switch (connectionState)
                        {
                            case ConnectionStates.Disconnected:
                                // The port is already closed, just set the mode

                                break;
                            case ConnectionStates.SerialMode:
                                // Release the device
                                ReleaseDevice();
                                break;
                            case ConnectionStates.Sleep:
                                // Wake up the device, (into serial mode)
                                WakeDevice();
                                // Release the device
                                ReleaseDevice();
                                break;
                            case ConnectionStates.TempCompMode:
                                // Get the device back into serial mode
                                try
                                {
                                    ExitTempCompMode();
                                    ReleaseDevice();
                                }
                                catch
                                {
                                }

                                break;
                            default:
                                // Should never get here. Something is wrong...
                                throw new ApplicationException("Tried to disconnect from unknown mode");
                        }
                        //RefreshTimer.Stop();

                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        // Close the port
                        mySerialCommunicator.CloseThePort();
                        // Set the mode
                        connectionState = ConnectionStates.Disconnected;
                    }
                    break;
                #endregion

                #region Serial Mode
                case ConnectionStates.SerialMode:
                    try
                    {
                        switch (connectionState)
                        {
                            case ConnectionStates.Disconnected:
                                EnterSerialMode();
                                break;
                            case ConnectionStates.SerialMode:
                                return;

                            case ConnectionStates.Sleep:
                                // Wake up the device
                                WakeDevice();
                                // Then enter serial mode
                                break;
                            case ConnectionStates.TempCompMode:
                                // Exit temp comp mode enters into serial mode
                                ExitTempCompMode();
                                break;
                            default:
                                throw new ApplicationException("Tried to disconnect from unknown mode");
                        }
                    }
                    catch
                    {
                        throw;
                    }

                    break;
                #endregion

                #region TempCompMode
                case ConnectionStates.TempCompMode:
                    // Check what the current state is...
                    switch (connectionState)
                    {
                        case ConnectionStates.Disconnected:
                            throw new ApplicationException("You must connect to device before you can enter temp comp mode");
                        case ConnectionStates.SerialMode:
                            EnterTempCompMode();
                            break;
                        case ConnectionStates.Sleep:
                            // Wake up the device, and go into serial mode...
                            WakeDevice();
                            // Enter temp comp mode...
                            EnterTempCompMode();
                            break;
                        case ConnectionStates.TempCompMode:
                            return;
                    }
                    break;
                #endregion

                #region Sleep

                case ConnectionStates.Sleep:
                    switch (connectionState)
                    {
                        case ConnectionStates.Disconnected:
                            throw new ApplicationException("You must connect to device before you can put the device to sleep.");
                        case ConnectionStates.SerialMode:
                            EnterSleepMode();
                            break;
                        case ConnectionStates.Sleep:
                            return;
                        case ConnectionStates.TempCompMode:
                            ExitTempCompMode();
                            EnterSleepMode();
                            break;
                    }
                    break;
                #endregion
            }
        }

        private string savedSerialPortName = "COM1";
        [DisplayName("Serial Port Name")]
        [Category("Application Configuration")]
        [ReadOnly(true)]
#if ASCOM
        [Description("Displays the selected serial port name. To change use the drop down box above labeled 'COM Port'.")]
#else
        [Description("To change close the settings form and select 'Change COM Port Name' form the main form " +
            "Settings menu")]
#endif
        public string SavedSerialPortName
        {
            get
            {
                return savedSerialPortName;
            }
            set
            {
                mySerialCommunicator.PortName = value;
                XMLSettings.SavedSerialPortName = value;
                savedSerialPortName = value;
            }
        }

        private void EnterSerialMode()
        {
            // Send FMMODE to first to possible get device out of another mode
            EventLogger.LogMessage("Entering Serial Mode", TraceLevel.Verbose);
            //RefreshTimer.Stop();
            PauseRefresh();
            DateTime start = DateTime.Now;
            string response = "";
            bool success = false;

            while (DateTime.Now.Subtract(start).TotalSeconds < 5)
            {
                try
                {
                    response = mySerialCommunicator.SendCommand("FMMODE", 1000);
                    if (response == "!")
                    {
                        success = true;
                        break;
                    }
                    else System.Threading.Thread.Sleep(100);
                }
                catch (TimeoutException ex)
                {
					mySerialCommunicator.ClearBuffers();
                    EventLogger.LogMessage(ex);
                }
                System.Windows.Forms.Application.DoEvents();
            }

            if (success) connectionState = ConnectionStates.SerialMode;
			else
            {
                mySerialCommunicator.ClearBuffers();
                throw new ApplicationException("Device failed to enter serial mode");
            }

            // Entered Serial Mode
            // Next try to get firmware version
            try
            {
                mySerialCommunicator.ClearBuffers();
                response = mySerialCommunicator.SendCommand("FVxxxx", 1000);
                // Firmware version should look like 4.01.3 or 4.0.1 or 4.01.255 or 
                if (response.Length < 6)
                {
                    Debug.Print("Bad Firmware Version: " + response);
                    throw new ApplicationException("Bad firmware version");
                }

                string FirmVerString = "";
                double FirmVerNum = 0;

                int DecimalIndex = response.IndexOf(".");
                FirmVerString = response.Substring(0, DecimalIndex + 1);
                response = response.Substring(DecimalIndex + 1, (response.Length - 1 - DecimalIndex));
                // response now looks like 01.3
                DecimalIndex = response.IndexOf(".");
                FirmVerString += response.Substring(0, DecimalIndex);
                response = response.Substring(DecimalIndex + 1, response.Length - 1 - DecimalIndex);

                FirmVerNum = double.Parse(FirmVerString, CultureInfo.InvariantCulture);
                firmwareVersion = "V" + FirmVerString;
                string DevTypeString = response;
                int DevTypeNum = int.Parse(response, CultureInfo.InvariantCulture);

                // Finished parsing the firmware version and device type
                // Now set the device properties accordingly.

                // Only set the device type if it has never been set before...

                switch (DevTypeNum)
                {
                    case 1:
                        if (FirmVerNum >= 3 && FirmVerNum < 4) deviceType = DeviceTypes.TCF_Si;
                        else deviceType = DeviceTypes.TCF_S;
                        break;
                    case 2:
                        deviceType = DeviceTypes.TCF_Si;
                        break;
                    case 3:
                        if (FirmVerNum >= 3 && FirmVerNum < 4) deviceType = DeviceTypes.TCF_S3i;
                        else deviceType = DeviceTypes.TCF_S3;
                        break;
                    case 4:
                        deviceType = DeviceTypes.TCF_S3i;
                        break;
                    default:
                        // This will happen if the device type has not been selected in firmware but
                        // the firmware defaults to the 2" focuser.
                        if (FirmVerNum >= 3 && FirmVerNum < 4) deviceType = DeviceTypes.TCF_Si;
                        else deviceType = DeviceTypes.TCF_S;
                        break;
                }

                // Determine if it has Backlash Compensation
                if (DeviceIs_i())
                {
                    if (FirmVerNum >= FIRST_TCFSI_WITH_BACKLASH)
                    {
                        hasBacklashCompensation = true;
                    }
                    else hasBacklashCompensation = false;
                }
                else
                {
                    if (FirmVerNum >= FIRST_TCFS_WITH_BACKLASH)
                    {
                        hasBacklashCompensation = true;
                    }
                    else hasBacklashCompensation = false;
                }

                // Determine if backlash comp is turned on or off.
                backlashCompEnabled = CheckIfBCEnabled();

                // Determine if it can adjust the display brightness
                if (DeviceIs_i())
                {
                    // All the TCF-Si's can adjust the brightness
                    canAdjustBrightness = true;
                }
                else
                {
                    if (FirmVerNum >= FIRST_TCFS_WITH_DISPLAY_ADJUST) canAdjustBrightness = true;
                    else canAdjustBrightness = false;
                }

                // Determine if it can report the brightness
                if (canAdjustBrightness)
                {
                    if (DeviceIs_i())
                    {
                        if (FirmVerNum >= FIRST_TCFSI_CAN_REPORT_BRIGHTNESS) canReportBrightness = true;
                        else canReportBrightness = false;
                    }
                    else
                    {
                        if (FirmVerNum >= FIRST_TCFS_CAN_REPORT_BRIGHTNESS) canReportBrightness = true;
                        else canReportBrightness = false;
                    }
                }

                // Determine the DisplayBrightnessValue
                displayBrightness = GetDisplayBrightness();


                // Set the class property according to whether the probe is disabled or enabled.
                tempProbeDisabled = CheckIfTempProbeDisabled();

                // Set the current and target positions initially to the same value.
                GetPos();
                targetPosition = (int)currentPosition;


            }
            catch (TimeoutException)
            {
                // Firmware is too old to report version info.
                firmwareVersion = OLD_FIRMWARE_STRING;
                canAdjustBrightness = false;
                tempProbeDisabled = XMLSettings.TemperatureProbeDisabled;
                // We know it is not a TCF-Si
                if (!XMLSettings.DeviceTypeManuallySet)
                {
                    deviceType = DeviceTypes.TCF_S;
                }

                // Set the current and target positions initially to the same value.
                GetPos();
                targetPosition = (int)currentPosition;

            }

            tempCoefficientA = GetTempCoefficient('A');
            tempCoefficientB = GetTempCoefficient('B');

            RefreshDeviceStatus();
            ResumeRefresh();

        }

        private void EnterTempCompMode()
        {
            // first check that the probe is enabled
            if (tempProbeDisabled) throw new ApplicationException("Cannot use temperature compensation when the temperature probe is disabled. Enable the probe and try again.");
            Debug.Print("Entering Temp Comp Mode");
            try
            {
                // Stop the Timer...
                PauseRefresh();
                // Timer is stopped, it is safe to enter serial mode
                string r = mySerialCommunicator.SendCommand("F" + tempCompMode + "MODE", 2000);
                if (r.Contains(tempCompMode.ToString())) connectionState = ConnectionStates.TempCompMode;
                ResumeRefresh();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PauseRefresh()
        {
            Trace.WriteLine("Pausing Refresh Timer. Waiting for RefreshDone to be true...");
            DateTime StartTime = DateTime.Now;
            //pauseRefresh = true;
            RefreshBGWorker.CancelAsync();
            //System.Threading.Thread.Sleep(100);
            while (RefreshDone == false)
            {
                if (DateTime.Now.Subtract(StartTime).TotalSeconds >= 3) return;
                System.Windows.Forms.Application.DoEvents();
            }
            if (RefreshBGWorker.IsBusy)
            {
                Trace.WriteLine("Waiting for BGWorker to be not busy");
                while (RefreshBGWorker.IsBusy) { System.Windows.Forms.Application.DoEvents(); }
                Trace.WriteLine("BGWorker finished");
            }

            Trace.WriteLine("RefreshDone == true. Pause was successful;");
        }

        private void ResumeRefresh()
        {
            Trace.WriteLine("Resuming Timer");
            // pauseRefresh = false;
            if (RefreshBGWorker.IsBusy)
            {
                EventLogger.LogMessage("Attempted to resume refresh while BGWorker IsBusy. RefreshDone = " +
                    RefreshDone.ToString(), TraceLevel.Warning);
                return;
            }
            else RefreshBGWorker.RunWorkerAsync();
            //RefreshTimer.Start();
            Trace.WriteLine("Timer Resumed");
        }

        private void ExitTempCompMode()
        {
            // Stop the Timer...
            Debug.Print("Exiting Temp Comp mode");
            //PauseRefresh();
            // Now go into serial mode
            EnterSerialMode();
        }

        private void EnterSleepMode()
        {
            string r = mySerialCommunicator.SendCommand("FSLEEP", 2000);
            if (r.Contains("ZZ"))
            {
                connectionState = ConnectionStates.Sleep;
            }
            else throw new ApplicationException("Device did not confirm that it entered sleep mode.");
        }

        private void WakeDevice()
        {
            string r = "";
            DateTime Start = DateTime.Now;
            try
            {
                while (DateTime.Now.Subtract(Start).TotalSeconds < 3)
                {
                    r = mySerialCommunicator.SendCommand("FWAKEx", 700);
                    if (r.Contains("WAKE"))
                    {
                        break;
                    }
                    System.Windows.Forms.Application.DoEvents();
                }
            }
            catch (TimeoutException)
            {
                // Timeout exception could mean the device is alredy awake.
                // Don't catch any other exceptions because we don't know the cause of them
            }
            finally
            {
                // Try to enter seial mode
                EnterSerialMode();
            }
        }

        private void ReleaseDevice()
        {
            if (connectionState != ConnectionStates.SerialMode)
                throw new ApplicationException("Can not release device when not in serial mode");

            // Stop the Timer...
            try
            {
                PauseRefresh();
            }
            catch
            {
            }

            DateTime Start = DateTime.Now;
            string r = "";
            try
            {
                while (DateTime.Now.Subtract(Start).TotalSeconds < 1.5)
                {
                    r = mySerialCommunicator.SendCommand("FFxxxx", 700);
                    if (r.Contains("END"))
                    {
                        break;
                    }
                    System.Windows.Forms.Application.DoEvents();
                }
            }
            catch (TimeoutException)
            {
                // Timeout exception could mean the device is alredy released
                // Don't catch any other exceptions because we don't know the cause of them
            }
            catch (Exception)
            {

            }
            finally
            {
                connectionState = ConnectionStates.Disconnected;
            }
        }

        public enum DeviceTypes { TCF_S, TCF_Si, TCF_S3, TCF_S3i }

        private static DeviceTypes deviceType = DeviceTypes.TCF_S;

        [Category("Device Configuration")]
        [DisplayName("Device Type")]
        [Description("REQUIRES REBOOT - Use this property to set the device type of your focuser in firmware. This may change the amount of travel that your focuser can move.")]
        public DeviceTypes DeviceType
        {
            get
            {
                return deviceType;
            }
            set
            {
                if (myFocuser.connectionState != ConnectionStates.SerialMode)
                {
                    throw new ApplicationException("You must connect to the focuser to perform this" +
                        " operation. Also, the focuser can not be in Sleep or Temp. Comp. mode while setting this property.");
                }
                ChangeDeviceType(value);

            }
        }

        [DisplayName("Maximum Step")]
        [Category("Device Status")]
        [Description("The maxmimum position the focuser can move to. This is determined based on the set Device Type.")]
        public int MaxSteps
        {
            get
            {
                if (deviceType == DeviceTypes.TCF_S || deviceType == DeviceTypes.TCF_Si) return 7000;
                else return 10000;
            }
        }

        private void ChangeDeviceType(DeviceTypes value)
        {
            // XMLDeviceType is used because the device type stored in XML should never be a type i becuase
            // all type i focusers are capable of reporting their device types.
            DeviceTypes XMLDeviceType = value;

            char setChar;
            if (value == DeviceTypes.TCF_S || value == DeviceTypes.TCF_Si)
            {
                XMLDeviceType = DeviceTypes.TCF_S;
                setChar = '1';
            }
            else
            {
                XMLDeviceType = DeviceTypes.TCF_S3;
                setChar = '3';
            }
            try
            {
                string resp = mySerialCommunicator.SendCommand("FExxx" + setChar, 2000);
                if (resp.Contains("DONE"))
                {
                    deviceType = XMLDeviceType;
                    XMLSettings.DeviceType = XMLDeviceType;
                    XMLSettings.DeviceTypeManuallySet = true;
                    EnterSerialMode();
                }
                else throw new ApplicationException("An error occurred while attempting to set the device type. Incorrect response received from the device.");
            }
            catch (TimeoutException)
            {
                throw new InvalidOperationException("Unable to set the device type. It is likely that your devices firmware is very old and does not support " +
                    "changing the device type.");
            }
        }

        private static bool DeviceIs_i()
        {
            if (deviceType == DeviceTypes.TCF_Si || deviceType == DeviceTypes.TCF_S3i) return true;
            else return false;
        }

        private bool CheckIfTempProbeDisabled()
        {
            return XMLSettings.TemperatureProbeDisabled;
        }

        [Browsable(false)]
        public bool IsMoving
        {
            get { return isMoving; }

            private set
            {
                isMoving = value;
                EventLogger.LogMessage("isMoving set to " + value.ToString(), TraceLevel.Verbose);
                TriggerAnEvent(DeviceStatusChanged);
            }
        }

        private void RefreshDeviceStatus()
        {

            DateTime start = DateTime.Now;
            try
            {
                if (targetPosition != CurrentPosition)
                {
                    IsMoving = true;
                    ChangeFocus(targetPosition);
                    IsMoving = false;
                }
            }
            catch
            {
                IsMoving = false;
                throw;
            }

            try
            {
                if (!TempProbeDisabled)
                {
                    GetTemp();
                    TriggerAnEvent(DeviceStatusChanged);
                }
            }
            catch (ER1_Exception)
            {
                // Pause for the ER=1 crap.
                System.Threading.Thread.Sleep(1500);

                try
                {
                    TempProbeDisabled = true;
                    TriggerAnEvent(DeviceStatusChanged);
                }
                catch
                {
                    ConnectionState = ConnectionStates.Disconnected;
                    throw new ApplicationException("Communication with the device has been lost. The device state will now be set to Disconnected.");
                }
                throw new ER1_Exception("The device lost communication with the temperature probe. The probe will now be disabled. " +
                  "To continue using the temperature probe, resolve the connection problem and then re-enable the probe.");
            }
            catch (TimeoutException)
            {

                if (ConsecutiveTimeouts_SerialMode++ >= 3)
                {
                    ConnectionState = ConnectionStates.Disconnected;
                    throw new ApplicationException("Communication with the device has been lost. The device state will now be set to Disconnected.");
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            try
            {
                GetPos();
                ConsecutiveTimeouts_SerialMode = 0;
                TriggerAnEvent(DeviceStatusChanged);
            }
            catch (TimeoutException)
            {

                if (ConsecutiveTimeouts_SerialMode++ >= 3)
                {
                    ConnectionState = ConnectionStates.Disconnected;

                    throw new ApplicationException("Communication with the device has been lost. The device state will now be set to Disconnected.");
                }
            }
            catch
            {
            }
            // Debug.Print("refreshed in " + DateTime.Now.Subtract(start).TotalMilliseconds);


        }

        private void RefreshDeviceStatusAutoMode()
        {
            try
            {
                string received = mySerialCommunicator.ReadALine(1200);
                Debug.Print(received);
                if (received.Contains("ER=1"))
                {
                    throw new ER1_Exception("The device lost communication with the temperature probe. The probe will now be disabled and " +
                    "the device must exit Temperature Compensation mode. " +
                    "To continue using the temperature probe, resolve the connection problem and then re-enable the probe.");
                }

                else if (received.Contains("P=") && received.Length == 6)
                {
                    ConsecutivePosTimeouts_AutoMode = 0;
                    // parse the position
                    int pos = int.Parse(received.Substring(2), CultureInfo.InvariantCulture);
                    if (pos != currentPosition)
                    {
                        currentPosition = pos;
                        TriggerAnEvent(DeviceStatusChanged);
                    }
                }
                else if (received.Contains("T=") && received.Length == 7)
                {
                    ConsecutivePosTimeouts_AutoMode = 0;
                    // parse the temperature
                    double temp = double.Parse(received.Substring(2), CultureInfo.InvariantCulture);
                    if (temp != currentTemperature)
                    {
                        currentTemperature = temp;
                        TriggerAnEvent(DeviceStatusChanged);
                    }
                }
            }
            catch (ER1_Exception)
            {
                // Try to exit temp comp mode...
                PauseRefresh();
                // Timer is stopped, it is safe to enter serial mode
                mySerialCommunicator.SendCommand("FMMODE", 1000);
                mySerialCommunicator.SendCommand("FMMODE", 1000);
                mySerialCommunicator.SendCommand("FMMODE", 1000);
                // Set the state
                connectionState = ConnectionStates.SerialMode;
                //Disable the probe
                TempProbeDisabled = true;
                TriggerAnEvent(DeviceStatusChanged);
                // Start the timer again
                ResumeRefresh();
                throw;
            }
            catch (Exception)
            {
                Debug.Print("Consecutive Timeouts = " + ConsecutivePosTimeouts_AutoMode.ToString());
                if (ConsecutivePosTimeouts_AutoMode++ > 3)
                {
                    ConnectionState = ConnectionStates.Disconnected;
                    // TriggerAnEvent(DeviceStatusChanged);
                    throw new ApplicationException("Lost communication with device while in Temp Comp mode. " +
                    "More than 5 consecutive serial timeouts have occurred.");
                }
            }
        }

        private void GetTemp()
        {
            // Wait for the port to become available
            // Debug.Print("GetTemp is continuing to use Port now");

            // Now try to get the temperature
            string t = mySerialCommunicator.SendCommand("FTEMPz", 1000);
            if (t.Contains("ER=1")) throw new ER1_Exception();
            int i = t.IndexOf("T=") + 2;
            if (i == -1) throw new ApplicationException("No Temp Received");
            string t2 = t.Substring(i, 5);
            currentTemperature = double.Parse(t2, CultureInfo.InvariantCulture);
        }

        public enum TemperatureUnits { Celsius, Fahrenheit, Kelvin }

        private TemperatureUnits displayTempUnits = TemperatureUnits.Celsius;

        [Category("Display Configuration")]
        [DisplayName("Display Temp. Units")]
        [Description("Set the units of temperature displayed in the application readout. This will not affect the " +
            "ASCOM driver if used.")]
#if ASCOM
        [Browsable(false)]
#else
        [Browsable(true)]
#endif
        public TemperatureUnits DisplayTempUnits
        {
            get
            {
                return displayTempUnits;
            }
            set
            {
                XMLSettings.DisplayTempUnits = value;
                displayTempUnits = value;
            }
        }

        private void GetPos()
        {
            // Now try to get the temperature
            string p = mySerialCommunicator.SendCommand("FPOSxx", 1000);
            int i = p.IndexOf("P=") + 2;
            if (i == -1) throw new ApplicationException("No Temp Received");
            currentPosition = double.Parse(p.Substring(i, 4), CultureInfo.InvariantCulture);
        }
        public enum PositionUnits { Steps, Microns }
        private PositionUnits displayPositionUnits = PositionUnits.Steps;
        [Category("Display Configuration")]
        [DisplayName("Display Position Units")]
        [Description("Set the units of posiiton displayed in the application readout. This will not affect the device readout " +
            "or the functionality of the ASCOM driver if used.")]
#if ASCOM
        [Browsable(false)]
#else
        [Browsable(true)]
#endif
        public PositionUnits DisplayPositionUnits
        {
            get { return displayPositionUnits; }
            set
            {
                XMLSettings.DisplayPositionUnits = value;
                displayPositionUnits = value;
            }
        }

        private void ChangeFocus(int NewPos)
        {
            try
            {
                EventLogger.LogMessage("Attempting to change focus to " + NewPos.ToString(), TraceLevel.Verbose);
                if (NewPos == currentPosition) return;
                int diff = (int)Math.Abs((double)currentPosition - (double)NewPos);
                int timeout = 1000 + (diff * 6);
                string c = NewPos < currentPosition ? "FI" : "FO";
                c += diff.ToString("0000");
                string r = mySerialCommunicator.SendCommand(c, timeout);
                if (r.Contains("*"))
                {
                    currentPosition = targetPosition;
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw;
            }
            finally
            {
            }

        }

        private DisplayBrightnessValues GetDisplayBrightness()
        {
            if (canReportBrightness)
            {
                // Get the brightness from the device;
                try
                {
                    string r = mySerialCommunicator.SendCommand("Fjxxxx", 1000);
                    if (r.Contains("LED=0")) return DisplayBrightnessValues.OFF;
                    else if (DeviceIs_i() && !r.Contains("LED=0")) return DisplayBrightnessValues.ON;
                    else if (r.Contains("LED=1")) return DisplayBrightnessValues.LOW;
                    else if (r.Contains("LED=2")) return DisplayBrightnessValues.MEDIUM;
                    else if (r.Contains("LED=3")) return DisplayBrightnessValues.HIGH;
                    else throw new ApplicationException("Invalid LED Brightness received");
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                // Get the brightness from the XML file
                return XMLSettings.DisplayBrightness;
            }
        }

        private void SetDisplayBrightness(DisplayBrightnessValues newValue)
        {
            if (!canAdjustBrightness) throw new InvalidOperationException("This focuser does not have the ability to adjust the display brightness. " +
                 "Please contact Optec if you are interested in an upgrade conversion kit.");
            char setChar;

            bool DeviceIsI = DeviceIs_i();

            switch (newValue)
            {
                case DisplayBrightnessValues.ON:
                    setChar = DeviceIsI ? '1' : '3';
                    break;
                case DisplayBrightnessValues.HIGH:
                    setChar = DeviceIsI ? '1' : '3';
                    break;
                case DisplayBrightnessValues.MEDIUM:
                    setChar = DeviceIsI ? '1' : '2';
                    break;
                case DisplayBrightnessValues.LOW:
                    setChar = DeviceIsI ? '1' : '1';
                    break;
                case DisplayBrightnessValues.OFF:
                    setChar = '0';
                    break;
                default: throw new InvalidEnumArgumentException("Invalid value for DisplayBrightess received in SetDisplayBrightness()");
            }

            string received = mySerialCommunicator.SendCommand("FJxxx" + setChar, 1000);
            if (received.Contains("LED=" + setChar))
            {
                displayBrightness = newValue;
                if (!canReportBrightness) XMLSettings.DisplayBrightness = newValue;
            }
            else if (received.Contains("DONE"))
            {
                if (newValue == DisplayBrightnessValues.OFF) displayBrightness = newValue;
                else displayBrightness = DisplayBrightnessValues.ON;
            }
        }

        private bool canAdjustBrightness = false;
        [Browsable(false)]
        public bool CanAdjustBrightness
        {
            get { return canAdjustBrightness; }
        }
        public enum DisplayBrightnessValues
        {
            ON,
            HIGH,
            MEDIUM,
            LOW,
            OFF
        }
        private DisplayBrightnessValues displayBrightness = DisplayBrightnessValues.MEDIUM;
        [Category("Device Configuration")]
        [DisplayName("LED Brightness")]
        [Description("Set the focuer LED brightness. HIGH, MEDIUM and LOW are not available on TCF-Si focuers." +
            " Focusers with firmware versions prior to 3.0 are not capable of adjusting the LED brightness.")]
        public DisplayBrightnessValues DisplayBrightness
        {
            get { return displayBrightness; }
            set
            {
                if (myFocuser.connectionState != ConnectionStates.SerialMode)
                {
                    throw new ApplicationException("You must connect to the focuser to perform this" +
                        " operation. Also, the focuser can not be in Sleep or Temp. Comp. mode while setting this property.");
                }
                SetDisplayBrightness(value);

            }
        }
        private bool canReportBrightness = false;
        [Browsable(false)]
        public bool CanReportBrightness
        {
            get { return canReportBrightness; }
        }

        /// <summary>
        /// Used for disabling the temperature probe.
        /// Probe can not be disabled in firmware
        /// </summary>
        private bool tempProbeDisabled = false;
        [Category("Device Configuration")]
        [DisplayName("Disable Temp. Probe")]
        [Description("Enable or disable the temperature probe with this property.")]
        public bool TempProbeDisabled
        {
            get { return tempProbeDisabled; }
            set
            {
                // just set it in the XML file and the in-memory property.
                XMLSettings.TemperatureProbeDisabled = value;
                tempProbeDisabled = value;
            }
        }

        public const string OLD_FIRMWARE_STRING = "V.old";
        private string firmwareVersion = "";
        [DisplayName("Firmware Version")]
        [Category("Device Status")]
        [Description("The current firmware version as reported by the device. Old firmware" +
            " versions were not capable of reporting the version number thus V.old is displayed.")]
        public string FirmwareVersion
        {
            get { return firmwareVersion; }
        }

        private bool hasBacklashCompensation = false;
        private bool backlashCompEnabled = true;

        [DisplayName("Backlash Comp. Available")]
        [Category("Device Status")]
        [Description("Indicates whether the device is capable of backlash compensation.")]
        public bool HasBacklashCompensation
        {
            get
            {
                if (FirmwareVersion == "V3.10" || FirmwareVersion == "V4.10") return true;
                else return hasBacklashCompensation;
            }
        }

        private bool CheckIfBCEnabled()
        {
            if (FirmwareVersion == "V3.10" || FirmwareVersion == "V4.10") return true;

            if (!hasBacklashCompensation) return false;
            else
            {
                try
                {
                    string cmd = "BKxxxx";
                    string received = mySerialCommunicator.SendCommand(cmd, 1000);
                    if (received.Contains("=0")) return false;
                    else if (received.Contains("=1")) return true;
                    else return false;
                }
                catch
                {
                    return false;
                }
            }
        }
        private void SetBacklash(bool e)
        {
            // Verify that it can set blkcomp
            if (!hasBacklashCompensation)
                throw new ApplicationException("This device does not support Backlash Compensation");
            string cmd = e ? "FKxxx1" : "FKxxx0";
            string received = mySerialCommunicator.SendCommand(cmd, 1000);
            string resp = e ? "=1" : "=0";
            if (received.Contains(resp))
            {
                backlashCompEnabled = e;
                return;
            }
            else throw new ApplicationException("No response from device while attempting to set backlash.");

        }

        [DisplayName("Backlash Compensation Enabled")]
        [Category("Device Configuration")]
        [Description("Select whether backlash compensation should be used or not. NOTE: Firmware versions 4.10 and 3.10 " +
            "have backlash compensation but are incapable of disabling it.")]
        public bool BacklashCompEnabled
        {
            get
            {
                if (FirmwareVersion == "V3.10" || FirmwareVersion == "V4.10") return true;
                else return backlashCompEnabled;
            }
            set
            {
                if (FirmwareVersion == "V3.10" || FirmwareVersion == "V4.10")
                    throw new ApplicationException("This firmware version is not capable of disabling backlash compensation");
                SetBacklash(value);
            }
        }

        public enum TempCompModes { A, B }
        private TempCompModes tempCompMode = TempCompModes.A;
        [Category("Device Configuration")]
        [DisplayName("Temp. Comp. Mode")]
        [Description("Selects which temperature coefficient (A or B) to use when the device" +
            " enters Temperature Compensation Mode.")]
        public TempCompModes TempCompMode
        {
            get { return tempCompMode; }
            set
            {
                XMLSettings.TempCompMode = value;
                tempCompMode = value;
            }
        }

        private int autoADelay = 0;
        private int autoBDelay = 0;
        [Category("Device Configuration")]
        [DisplayName("Temp. Mode. A Delay")]
        [Description("The time delay between position adjustments when in Temp. Comp. Mode in 5ms increments. " +
            "The default and minimum value for this property is 1.")]
        public int AutoADelay
        {
            get { return autoADelay; }
            set
            {
                if (myFocuser.connectionState != ConnectionStates.SerialMode)
                {
                    throw new ApplicationException("You must connect to the focuser to perform this" +
                        " operation. Also, the focuser can not be in Sleep or Temp. Comp. mode while setting this property.");
                }
                SetAutoDelay('A', value);
            }
        }
        [Category("Device Configuration")]
        [DisplayName("Temp Mode B Delay")]
        [Description("The time delay between position adjustments when in Temp. Comp. Mode in 5ms increments. " +
            "The default and minimum value for this property is 1.")]
        public int AutoBDelay
        {
            get { return autoBDelay; }
            set
            {
                if (myFocuser.connectionState != ConnectionStates.SerialMode)
                {
                    throw new ApplicationException("You must connect to the focuser to perform this" +
                        " operation. Also, the focuser can not be in Sleep or Temp. Comp. mode while setting this property.");
                }
                SetAutoDelay('B', value);
            }
        }

        private void SetAutoDelay(char AorB, double d)
        {
            try
            {
                // First try to st the delay in the device

                if (AorB != 'A' && AorB != 'B') throw new InvalidOperationException("SetDelay must be A or B. Received: " + AorB.ToString());
                if (d < 0 || d > 999) throw new InvalidOperationException("SetDelay must be between 0 through 999");
                int delay = (int)d;
                string delaystring = (delay).ToString().PadLeft(3, '0');
                string response = mySerialCommunicator.SendCommand("FD" + AorB + delaystring, 1500);
                if (!response.Contains("DONE")) throw new ApplicationException("Invalid response from device while trying to set auto mode delay.");

                // Next set the delay in the xml file and class property
                if (AorB == 'A')
                {
                    XMLSettings.AutoADelay = delay;
                    autoADelay = delay;
                }
                else if (AorB == 'B')
                {
                    XMLSettings.AutoBDelay = delay;
                    autoBDelay = delay;
                }
                else throw new ApplicationException("Attempted to set delay for invalid Auto mode.");
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw ex;
            }
        }


        private double temperatureOffsetC = 0;
        [DisplayName("Temperature Offset(°C)")]
        [Category("Device Configuration")]
        public double TemperatureOffsetC
        {
            get { return temperatureOffsetC; }
            set
            {
                XMLSettings.TemperatureOffsetC = value;
                temperatureOffsetC = value;
            }
        }

        private int tempCoefficientA = 86;
        [Category("Device Configuration")]
        [DisplayName("Mode A Temp. Coefficient")]
        [Description("The number of steps the focuser will move per degree Celsius of temperature change when in Temperature Compensation Mode A.")]
        public int TempCoefficientA
        {
            get
            {
                return tempCoefficientA;
            }
            set
            {
                if (myFocuser.connectionState != ConnectionStates.SerialMode)
                {
                    throw new ApplicationException("You must connect to the focuser to perform this" +
                        " operation. Also, the focuser can not be in Sleep or Temp. Comp. mode while setting this property.");
                }
                SetTempCoefficient('A', value);
            }
        }

        private int tempCoefficientB = 86;
        [Category("Device Configuration")]
        [DisplayName("Mode B Temp. Coefficient")]
        [Description("The number of steps the focuser will move per degree Celsius of temperature change when in Temperature Compensation Mode A.")]
        public int TempCoefficientB
        {
            get
            {
                return tempCoefficientB;
            }
            set
            {
                if (myFocuser.connectionState != ConnectionStates.SerialMode)
                {
                    throw new ApplicationException("You must connect to the focuser to perform this" +
                        " operation. Also, the focuser can not be in Sleep or Temp. Comp. mode while setting this property.");
                }
                SetTempCoefficient('B', value);
            }
        }
        private int GetTempCoefficient(char AorB)
        {
            // First try to get the slope
            int slope = 0;
            int sign = 1;
            int BadResponseErrors = 0;

            try
            {
                string resp = mySerialCommunicator.SendCommand("FREAD" + AorB, 2000);
                if (resp.Contains(AorB + "=") && resp.Length >= 5)
                {
                    int i = resp.IndexOf("=") + 2;
                    slope = int.Parse(resp.Substring(i, 3), CultureInfo.InvariantCulture);
                }
            }
            catch
            {
                // Every device should be able to report the slope so throw exception if it occurs here.
                throw;
            }

            // Next try to get the sign
            while (true)
            {
                try
                {

                    string resp = mySerialCommunicator.SendCommand("Ftxxx" + AorB, 1000);
                    if (resp.Contains(AorB + "=") && resp.Length >= 3)
                    {
                        int i = resp.IndexOf("=") + 1;
                        sign = (resp[i] == '0') ? 1 : -1;   // if response is 1, sign is negative
                        break;
                    }
                    else throw new ApplicationException();
                }
                catch (ApplicationException)
                {
                    Debug.Print("Bad Responses while getting slope = " + BadResponseErrors);
                    if (BadResponseErrors++ >= 2) throw new
                        ApplicationException("Unable to get temperature coefficient.");
                }
                catch (TimeoutException) { break; }
                catch
                {
                    throw;
                }
                System.Windows.Forms.Application.DoEvents();
            }
            return sign * slope;

        }

        private void SetTempCoefficient(char AorB, int coeff)
        {
            string cmd = "";
            string slp = "";
            int UnsignedCoeff = Math.Abs(coeff);
            if (UnsignedCoeff > 999) throw new InvalidOperationException("Coefficient must be between -999 and 999");
            if (UnsignedCoeff < 0) throw new InvalidOperationException("Coefficient must be between -999 and 999");
            // Set the sign first 
            char sign = (coeff < 0) ? '1' : '0';
            cmd = "FZ" + AorB + "xx" + sign;
            try
            {
                string response = mySerialCommunicator.SendCommand(cmd, 1100);
                if (!response.Contains("DONE")) throw new ApplicationException("Unacceptable response received while attempting to set the temperature coefficient slope. " +
                     "Response = " + response);
            }
            catch (TimeoutException)
            {
                if (sign == '-')
                    throw new InvalidOperationException("The device firmware version is not capable of setting a coefficient with a negative slope. " +
                        "Later revisions of the TCF firmware have made this possible. Please contact Optec for an upgrade.");
            }
            // Next set the coefficient value
            try
            {
                slp = UnsignedCoeff.ToString().PadLeft(3, '0');
                cmd = "FL" + AorB + slp;
                string response = mySerialCommunicator.SendCommand(cmd, 1100);
                if (!response.Contains("DONE")) throw new ApplicationException("Unacceptable response received while attempting to set the temperature coefficient slope. " +
                     "Response = " + response);
                if (AorB == 'A') tempCoefficientA = coeff;
                else tempCoefficientB = coeff;
            }
            catch
            {
                throw;
            }

        }

        [Browsable(false)]
        public List<FocusOffset> FocusOffsets
        {
            get
            {
                //List<FocusOffset> list = new List<FocusOffset>();
                //list.Add(new FocusOffset("No Filter", 0));
                //list.AddRange(XMLSettings.SavedFocusOffsets);
                return XMLSettings.SavedFocusOffsets;
            }
        }

        [Browsable(false)]
        public List<FocusOffset> AbsolutePresets
        {
            get
            {
                //List<FocusOffset> list = new List<FocusOffset>();
                //list.Add(new FocusOffset("No Filter", 0));
                //list.AddRange(XMLSettings.SavedAbsolutePresets);
                return XMLSettings.SavedAbsolutePresets;
            }
        }

        private void TriggerAnEvent(EventHandler EH)
        {
            if (EH == null) return;
            var EventListeners = EH.GetInvocationList();
            if (EventListeners != null)
            {
                for (int index = 0; index < EventListeners.Count(); index++)
                {
                    var methodToInvoke = (EventHandler)EventListeners[index];
                    methodToInvoke.BeginInvoke(this, EventArgs.Empty, EndAsyncEvent, new object[] { });
                }
            }

        }

        private void TriggerAnEvent(EventHandler EH, object e)
        {
            if (EH == null) return;
            var EventListeners = EH.GetInvocationList();
            if (EventListeners != null)
            {
                for (int index = 0; index < EventListeners.Count(); index++)
                {

                    var methodToInvoke = (EventHandler)EventListeners[index];
                    methodToInvoke.BeginInvoke(e, EventArgs.Empty, EndAsyncEvent, new object[] { });

                }
            }

        }

        private static void EndAsyncEvent(IAsyncResult iar)
        {
            var ar = (System.Runtime.Remoting.Messaging.AsyncResult)iar;
            var invokedMethod = (EventHandler)ar.AsyncDelegate;

            try
            {
                invokedMethod.EndInvoke(iar);
            }
            catch
            {
                // Handle any exceptions that were thrown by the invoked method
                Console.WriteLine("An event listener went kaboom!");
            }
        }
    }

    public class ER1_Exception : Exception
    {
        public ER1_Exception() { }
        public ER1_Exception(string message)
            : base(message)
        {

        }
    }

    public class FocusOffset
    {
        public string OffsetName;
        public int OffsetSteps;

        public FocusOffset()
        {
            OffsetName = "";
            OffsetSteps = 0;
        }

        public FocusOffset(string Name, int offsetSteps)
        {
            OffsetName = Name;
            OffsetSteps = offsetSteps;
        }
    }
}
