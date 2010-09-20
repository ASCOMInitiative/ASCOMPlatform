using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OptecHIDTools;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace OptecHID_FilterWheelAPI
{
    [Guid("D1347EAF-1DA3-414b-81F6-D3EE36FEE7AF")]
    public class FilterWheel
    {
        #region Fields

        // Events
        public event EventHandler HomingCompleted;
        public event EventHandler HomingStarted;
        public event EventHandler MoveComplete;
        public event EventHandler MoveStarted;
        public event EventHandler DeviceRemoved;
        // Property Fields
        private string name;
        private string serialNumber;
        private string manufacturer;
        private HID selectedDevice = null;
        private bool isHomed = false;
        private bool isHoming = false;
        private bool isMoving = false;
        private short currentPosition = 0;
        private short centeringOffset = 0;
        private short errorState = 0;
        private string firmwareVersion = "";
        private short numberOfFilters = 0;
        private char wheelID = '?';

        #endregion

        #region Constructor

        internal FilterWheel(HID Device)
        {
            this.selectedDevice = Device;
            this.Manufacturer = Device.Manufacturer;
            this.Name = Device.ProductDescription;
            this.SerialNumber = Device.SerialNumber;
            this.selectedDevice.DeviceRemoved += new EventHandler(TriggerFilterWheelUnplugged);
            
            TimeSpan MaxHomeTime = new TimeSpan(0, 0, 3);
            DateTime FirstAttempt = DateTime.Now;

            TryAgain:
            if (Device.DeviceIsAttached)
            {
   
                RefreshDeviceStatus();
            
                if (this.isHoming == true)
                {
                    System.Threading.Thread.Sleep(500);
                    goto TryAgain;
                }
                else if (this.ErrorState == 0)
                {
                    // This is where we read the number of filters and such so
                    // we need to make sure the device is already homed bewf
                    RefershDeviceDescription();
                }
            }


        }

        #endregion 

        #region Properties

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool IsAttached
        {
            get { return this.selectedDevice.DeviceIsAttached; }
        }

        public short CurrentPosition
        {
            get
            {
                return currentPosition;
            }
            set
            {
                ChangePosition(value);
                currentPosition = value;
            }
        }

        public bool IsHomed
        {
            get
            {
                return isHomed;
            }
            internal set
            {
                isHomed = value;
            }
        }

        public bool IsHoming
        {
            get
            {
                return isHoming;
            }
            set
            {
                isHoming = value;
            }
        }

        public bool IsMoving
        {
            get
            {
                return isMoving;
            }
            set
            {
                isMoving = value;
            }
        }

        public short ErrorState
        {
            get {
                RefreshDeviceStatus();
                return errorState; 
            }
            set { errorState = value; }
        }

        public short CenteringOffset
        {
            get { return centeringOffset; }
            set 
            {
                if (value > 127) value = 127;
                if (value < -128) value = -128;
                UpdateCenteringOffset(value);
                centeringOffset = value;
                
            }
        }

        public string SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; }
        }

        public string Manufacturer
        {
            get { return manufacturer; }
            set { manufacturer = value; }
        }

        public char WheelID
        {
            get { return wheelID; }
            set { wheelID = value; }
        }

        #endregion

        #region Methods

        public string FirmwareVersion
        {
            get { return firmwareVersion; }
            set { firmwareVersion = value; }
        }

        public short NumberOfFilters
        {
            get { return numberOfFilters; }
            set { numberOfFilters = value; }
        }
     
        private void RefreshDeviceStatus()
        {
            try
            {
                InputReport StatusReport = new InputReport(FilterWheels.REPORTID_INPUT_STATUS);
                this.selectedDevice.RequestInputReport_Control(StatusReport);
                ParseDeviceStatus(StatusReport);
            }
            catch (Exception ex)
            {

                Trace.WriteLine("EXCEPTION THROWN in FilterWheel.RefreshDeviceStatus: \n" + ex.ToString());
                throw;
            }

        }

        private void ParseDeviceStatus(InputReport reveicedReport)
        {
            try
            {
                // ***************Parse The Received Data*****************////
                short rReportID = 0;
                short rIsHomed = 0;
                short rIsHoming = 0;
                short rIsMoving = 0;
                short rCurrentPosition = 0;
                short rErrorState = 0;

                // Check that at least one byte of data was received.
                if (reveicedReport.ReceivedData.Length < 6) throw new ApplicationException("Device Status Not Received");

                // Convert the received bytes to shorts
                rReportID = Convert.ToInt16(reveicedReport.ReceivedData[0]);
                rIsHomed = Convert.ToInt16(reveicedReport.ReceivedData[1]);
                rIsHoming = Convert.ToInt16(reveicedReport.ReceivedData[2]);
                rIsMoving = Convert.ToInt16(reveicedReport.ReceivedData[3]);
                rCurrentPosition = Convert.ToInt16(reveicedReport.ReceivedData[4]);
                rErrorState = Convert.ToInt16(reveicedReport.ReceivedData[5]);

                // Verify the ReportID is correct
                if (rReportID != FilterWheels.REPORTID_INPUT_STATUS)
                {
                    throw new ApplicationException("Wrong ReportID returned from device status request.");
                }

                // Extract the DeviceIsHomedFlag
                if (rIsHomed == FilterWheels.REPORT_TRUE)
                {
                    this.IsHomed = true;
                }
                else if (rIsHomed == FilterWheels.REPORT_FALSE)
                {
                    this.IsHomed = false;
                }
                else throw new ApplicationException("Invalid data received for IsHomed value");

                // Extract the DeviceIsHomingFlag
                if (rIsHoming == FilterWheels.REPORT_TRUE)
                {
                    this.IsHoming = true;
                }
                else if (rIsHoming == FilterWheels.REPORT_FALSE)
                {
                    this.IsHoming = false;
                }
                else throw new ApplicationException("Invalid data received for IsHoming value");

                // Extract the DeviceIsMovingFlag
                if (rIsMoving == FilterWheels.REPORT_TRUE)
                {
                    this.IsMoving = true;
                }
                else if (rIsMoving == FilterWheels.REPORT_FALSE)
                {
                    this.IsMoving = false;
                }
                else throw new ApplicationException("Invalid data received for IsMoving value");

                // Extract the Current Position
                if ((rCurrentPosition < 10) && (rCurrentPosition > 0))
                {
                    this.currentPosition = rCurrentPosition;
                }

                // Extracth the ErrorState
                this.ErrorState = rErrorState;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EXCEPTION THROWN in FilterWheel.ParseDeviceStatus: \n" + ex.ToString());
                throw;
            }

        }

        private void RefershDeviceDescription()
        {
            try
            {
                InputReport DescriptionReport = new InputReport(FilterWheels.REPORTID_INPUT_DESCRIPTION);
                this.selectedDevice.RequestInputReport_Control(DescriptionReport);
                ParseDeviceDescription(DescriptionReport);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EXCEPTION THROWN in FilterWheel.RefershDeviceDescription: \n" + ex.ToString());
                throw; 
            }
        }

        private void ParseDeviceDescription(InputReport reveicedReport)
        {
            try
            {
                // ***************Parse The Received Data*****************////
                short rReportID = 0;
                short rFirmwareVerMajor = 0;
                short rFirmwareVerMinor = 0;
                short rFirmwareVerRevision = 0;
                short rNumberFilters = 0;
                byte rWheelID = 0;
                short rCenteringOffset = 0;

                // Check that at least one byte of data was received.
                if (reveicedReport.ReceivedData.Length < 6) throw new ApplicationException("Device Status Not Received");

                // Convert the received bytes to shorts
                rReportID = Convert.ToInt16(reveicedReport.ReceivedData[0]);
                rFirmwareVerMajor = Convert.ToInt16(reveicedReport.ReceivedData[1]);
                rFirmwareVerMinor = Convert.ToInt16(reveicedReport.ReceivedData[2]);
                rFirmwareVerRevision = Convert.ToInt16(reveicedReport.ReceivedData[3]);
                rNumberFilters = Convert.ToInt16(reveicedReport.ReceivedData[4]);
                rWheelID = reveicedReport.ReceivedData[5];
                rCenteringOffset = (short)reveicedReport.ReceivedData[6];

                // Verify the ReportID is correct
                if (rReportID != FilterWheels.REPORTID_INPUT_DESCRIPTION)
                {
                    throw new ApplicationException("Wrong ReportID returned from device status request.");
                }

                // Extract the Firmware Version
                string tempFV = "V" + rFirmwareVerMajor.ToString();
                tempFV += "." + rFirmwareVerMinor.ToString();
                tempFV += "." + rFirmwareVerRevision.ToString();
                this.FirmwareVersion = tempFV;

                // Extract the Number of Filters
                if ((rNumberFilters < 5) || (rNumberFilters > 9))
                {
                    throw new ApplicationException("Incorrect number of filters");
                }
                else this.NumberOfFilters = rNumberFilters;

                // Extract the WheelID
                this.WheelID = Convert.ToChar(rWheelID);

                // Extract the Centering Offset
                this.centeringOffset = rCenteringOffset;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EXCEPTION THROWN in FilterWheel.ParseDeviceDescription: \n" + ex.ToString());
                throw;
            }

        }

        private void UpdateCenteringOffset(short NewValue)
        {
            try
            {
                byte[] DataToSend = new byte[5];
                byte[] ExpectedResponses = new byte[5];
                ExpectedResponses[0] = FilterWheels.FLASH_OPCODE_UPDATE_CENTERING_OFFSET;
                ExpectedResponses[1] = 0;   // Error State
                ExpectedResponses[2] = 0;   // Wheel ID, Not used
                ExpectedResponses[3] = 0;   // Filter Number, Not used
                ExpectedResponses[4] = (byte)NewValue;
                DataToSend[0] = FilterWheels.FLASH_OPCODE_UPDATE_CENTERING_OFFSET;
                DataToSend[1] = (byte)NewValue;
                // Create the Feature Report
                FeatureReport UpdateCentOffReport = new FeatureReport(FilterWheels.REPOTRID_FEATURE_FLASHOPS, DataToSend);
                // Send the Feature Report
                this.selectedDevice.ProcessFeatureReport(UpdateCentOffReport);
                // Verify the correct data was received
                for (int j = 0; j < ExpectedResponses.Length; j++)
                {
                    if (ExpectedResponses[j] != UpdateCentOffReport.Response1[j])
                    {
                        string msg = "ExpectedResponse did not " +
                            "match ReceivedResponse1 for byte number " + j.ToString();
                        throw new ApplicationException(msg);
                    }
                    else if (ExpectedResponses[j] != UpdateCentOffReport.Response2[j])
                    {
                        string msg = "ExpectedResponse did not " +
                            "match ReceivedResponse2 for byte number " + j.ToString();
                        throw new ApplicationException(msg);
                    }
                }
                Trace.WriteLine("Centering Offset for Wheel successfully updated to " + NewValue.ToString());
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EXCEPTION THROWN in FilterWheel.UpdateCenteringOffset: \n" + ex.ToString());
                throw;
            }
        }
        
        public void HomeDevice()
        {
            try
            {
                byte[] datatosend = new byte[] { };

                // Create the report to send
                FeatureReport HomeReport = new FeatureReport(
                    FilterWheels.REPORTID_FEATURE_DOHOME, datatosend);
                // Feature Reports Must have at least one data item so we put a zero in it.
                HomeReport.DataToSend = new byte[] { 0 };
                
                // Send the Report
                this.selectedDevice.ProcessFeatureReport(HomeReport);

                // Verify the the first response was correct
                if (HomeReport.Response1[0] != FilterWheels.REPORT_TRUE)
                {
                    throw new ApplicationException("The device never started homing according to response from feature report.");
                }

                //Verify the second response, the error state, is zero;
                if (HomeReport.Response2[0] != FilterWheels.REPORT_FALSE)
                {
                    short errorstate = Convert.ToInt16(HomeReport.Response2[0]);
                    string msg = GetErrorMessage(errorstate);
                    throw new FirmwareException("An Error State Has Been Set In the Device Firmware. \n" +
                        msg);
                }

                this.RefreshDeviceStatus();
                TriggerHomingStarted();

                DateTime start = DateTime.Now;
                TimeSpan elapsedTime = DateTime.Now.Subtract(start);
                while (elapsedTime.Seconds < 4)
                {
                    RefreshDeviceStatus();
                    if (this.IsHomed == true)
                    {
                        // Home Finished
                        Trace.WriteLine("Home Completed in " + elapsedTime.ToString());
                        TriggerHomingComplete();
                        return;
                    }
                    else if (ErrorState != 0)
                    {
                        string errormsg = GetErrorMessage(ErrorState);
                        Trace.WriteLine(errormsg);
                        throw new FirmwareException("An Error State Has Been Set In the Device Firmware. \n" +
                        errormsg);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(100);
                        elapsedTime = DateTime.Now.Subtract(start);
                    }
                }
                throw new System.ApplicationException("Home Timed Out");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                throw;
            }
        }

        public void HomeDevice_ASync()
        {
            if (this.isHoming || this.IsMoving) return;

            try
            {
                byte[] datatosend = new byte[] { };

                // Create the report to send
                FeatureReport HomeReport = new FeatureReport(
                    FilterWheels.REPORTID_FEATURE_DOHOME, datatosend);
                // Feature Reports Must have at least one data item so we put a zero in it.
                HomeReport.DataToSend = new byte[] { 0 };

                // Send the Report
                this.selectedDevice.ProcessFeatureReport(HomeReport);

                // Verify the the first response was correct
                if (HomeReport.Response1[0] != FilterWheels.REPORT_TRUE)
                {
                    throw new ApplicationException("The first feature report received did not contain true.");
                }

                //Verify the second response, the error state, is zero;
                if (HomeReport.Response2[0] != FilterWheels.REPORT_FALSE)
                {
                    short errorstate = Convert.ToInt16(HomeReport.Response2[0]);
                    string msg = GetErrorMessage(errorstate);
                    throw new FirmwareException("An Error State Has Been Set In the Device Firmware. \n" +
                        msg);
                }

                this.RefreshDeviceStatus();
                TriggerHomingStarted();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                throw;
            }
        }

        private void ChangePosition(short NewPos)
        {

            try
            {
                FeatureReport MoveReport = new FeatureReport(
                        FilterWheels.REPORTID_FEATURE_DOMOVE,
                        new byte[] { Convert.ToByte(NewPos) });

                this.selectedDevice.ProcessFeatureReport(MoveReport);
                this.RefreshDeviceStatus();
                TriggerMoveStarted();

                DateTime start = DateTime.Now;
                TimeSpan elapsedTime = DateTime.Now.Subtract(start);
                while (elapsedTime.Seconds < 10)
                {

                    RefreshDeviceStatus();
                    if (this.CurrentPosition == NewPos)
                    {
                        // Move Finished
                        Trace.WriteLine("Move Completed in " + elapsedTime.ToString());
                        System.Threading.Thread.Sleep(100);
                        RefreshDeviceStatus();
                        //Debug.WriteLine("Moving" + this.IsMoving.ToString());
                        TriggerMoveComplete();
                        return;
                    }
                    else if (ErrorState != 0)
                    {
                        string errormsg = GetErrorMessage(ErrorState);
                        Trace.WriteLine(errormsg);
                        throw new FirmwareException("An Error State Has Been Set In the Device Firmware. \n" +
                        errormsg);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(100);
                        elapsedTime = DateTime.Now.Subtract(start);
                    }
                }
                throw new System.ApplicationException("Move Timed Out");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EXCEPTION THROWN in ChangePosition: " + ex.ToString());
                throw;
            }
        }

        public void ClearErrorState()
        {
            try
            {
                string OldErrorState = this.ErrorState.ToString();
                OutputReport ClearErrorReport = new OutputReport(
                    FilterWheels.REPORTID_OUTPUT_CLEARERROR,
                    new byte[] { });
                this.selectedDevice.SendReport_Control(ClearErrorReport);
                Trace.WriteLine("Error State Cleared. Was set to " + OldErrorState);
                
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EXCEPTION THROWN in FilterWheel.ClearErrorState: " + ex.ToString());
                throw;
            }

        }

        public void RestoreDefaultNames()
        {
            try
            {
                // Create Feature Report
                FeatureReport StoreDefaultsReport = new FeatureReport(22, new byte[] { 1 });
                // Create the Expected Response Buffer
                Byte[] ExpectedResponses = new Byte[2];
                ExpectedResponses[0] = FilterWheels.FLASH_OPCODE_SET_DEFAULT_NAMES;
                ExpectedResponses[1] = FilterWheels.REPORT_FALSE; // Error Code
                // Send the Report
                this.selectedDevice.ProcessFeatureReport(StoreDefaultsReport);
                // Verify that an error code wasn't set
                for (int j = 0; j < ExpectedResponses.Length; j++)
                {
                    if (ExpectedResponses[j] != StoreDefaultsReport.Response1[j])
                    {
                        string msg = "ExpectedResponse did not " +
                            "match ReceivedResponse1 for byte number " + j.ToString();
                        throw new ApplicationException(msg);
                    }
                    else if (ExpectedResponses[j] != StoreDefaultsReport.Response2[j])
                    {
                        string msg = "ExpectedResponse did not " +
                            "match ReceivedResponse2 for byte number " + j.ToString();
                        throw new ApplicationException(msg);
                    }
                }
                Trace.WriteLine("Default Filter and Wheel Names successfully stored in device");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EXCEPTION THROWN in FilterWheel.StoreDefaultNames: " + ex.ToString());
                throw;
            }
        }

        public string[] GetFilterNames(char pWheelID)
        {
            try
            {
                string[] ReceivedNames = null;
                if (pWheelID < 'A' || pWheelID > 'H')
                {
                    throw new ArgumentOutOfRangeException("WheelID", "Must be between A and H");
                }
                else
                {

                    byte[] sendData = new byte[3];
                    byte[] ExpectedResponse = new byte[4];
                    // Set the Flash OpCode now becaues it won't change
                    sendData[0] = FilterWheels.FLASH_OPCODE_READ_FILTER_NAME;
                    // Set the Wheel ID
                    sendData[1] = Convert.ToByte(pWheelID);
                    // Resize the array to hold the correct number of filters for this wheel.
                    if (pWheelID <= 'E') Array.Resize(ref ReceivedNames, 5);
                    else Array.Resize(ref ReceivedNames, 8);
                    // Setup the expected Response
    
                    ExpectedResponse[0] = FilterWheels.FLASH_OPCODE_READ_FILTER_NAME;
                    ExpectedResponse[1] = FilterWheels.REPORT_FALSE;    // Byte 2 = ErrorState
                    ExpectedResponse[2] = Convert.ToByte(pWheelID);
                    // Get the names
                    for (int i = 0; i < ReceivedNames.Length; i++)
                    {
                        // Set the Filter Number in the outgoing data
                        sendData[2] = Convert.ToByte(i + 1);
                        // Set the Filter number in the expected response
                        ExpectedResponse[3] = Convert.ToByte(i + 1);
                        // Generate the report
                        FeatureReport GetFiltNamesReport = new FeatureReport(FilterWheels.REPOTRID_FEATURE_FLASHOPS,
                            sendData);
                        // Process the report
                        this.selectedDevice.ProcessFeatureReport(GetFiltNamesReport);
                        // Compare the responses here...
                        for (int j=0; j < ExpectedResponse.Length; j++)
                        {
                            if (ExpectedResponse[j] != GetFiltNamesReport.Response1[j])
                            {
                                string msg = "ExpectedResponse did not " +
                                    "match ReceivedResponse1 for byte number " + j.ToString();
                                throw new ApplicationException(msg);
                            }
                            else if (ExpectedResponse[j] != GetFiltNamesReport.Response2[j])
                            {
                                string msg = "ExpectedResponse did not " +
                                    "match ReceivedResponse2 for byte number " + j.ToString();
                                throw new ApplicationException(msg);
                            }
                        }
                        // Not get the name out of the report, it is in bytes 6-14
                        for (int b = 5; b < 13; b++)
                        {
                            ReceivedNames[i] += (char)GetFiltNamesReport.Response2[b];
                        }
                    }
                }
                return ReceivedNames;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EXCEPTION THROWN in FilterWheel.GetFilterWheelNames: \n" + ex.ToString());
                throw;
            }
        }

        public string[] GetWheelNames()
        {
            try
            {
                string[] ReceivedNames = new string[8];
                byte[] sendData = new byte[3];
                byte[] ExpectedResponse = new byte[4];

                // Set the Flash OpCode now becaues it won't change
                sendData[0] = FilterWheels.FLASH_OPCODE_READ_WHEEL_NAME;
                sendData[2] = 0;
                // Setup the expected Response
                ExpectedResponse[0] = FilterWheels.FLASH_OPCODE_READ_WHEEL_NAME;
                ExpectedResponse[1] = FilterWheels.REPORT_FALSE;    // Byte 2 = ErrorState
                ExpectedResponse[3] = 0;
                // Get the names
                for (int i = 0; i < 8; i++)
                {
                    // Set the WheelID in the outgoing data
                    sendData[1] = Convert.ToByte((char)i + 'A');
                    // Set the WheelID number in the expected response
                    ExpectedResponse[2] = Convert.ToByte((char)i + 'A');
                    // Generate the report
                    FeatureReport GetWheelNamesReport = new FeatureReport(FilterWheels.REPOTRID_FEATURE_FLASHOPS,
                        sendData);
                    // Process the report
                    this.selectedDevice.ProcessFeatureReport(GetWheelNamesReport);

                    for (int j = 0; j < ExpectedResponse.Length; j++)
                    {
                        if (ExpectedResponse[j] != GetWheelNamesReport.Response1[j])
                        {
                            string msg = "ExpectedResponse did not " +
                                "match ReceivedResponse1 for byte number " + j.ToString();
                            throw new ApplicationException(msg);
                        }
                        else if (ExpectedResponse[j] != GetWheelNamesReport.Response2[j])
                        {
                            string msg = "ExpectedResponse did not " +
                                "match ReceivedResponse2 for byte number " + j.ToString();
                            throw new ApplicationException(msg);
                        }
                    }


                    // Not get the name out of the report, it is in bytes 6-14
                    for (int b = 5; b < 13; b++)
                    {
                        ReceivedNames[i] += (char)GetWheelNamesReport.Response2[b];
                    }
                }

                return ReceivedNames;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EXCEPTION THROWN in FilterWHeel.GetWheelNames: " +  ex.ToString());
                throw;
            }
        }

        public void UpdateFilterName(char WheelID, short FilterNumber, string NewName)
        {
            try
            {
                // Prepare the Report Data buffers
                byte[] DataToSend = new byte[11];
                byte[] ExpectedResponses = new byte[13];
                //Set OpCode
                DataToSend[0] = FilterWheels.FLASH_OPCODE_UPDATE_FILTER_NAME;
                ExpectedResponses[0] = FilterWheels.FLASH_OPCODE_UPDATE_FILTER_NAME;
                // Set Expected Error Response to 0
                ExpectedResponses[1] = FilterWheels.REPORT_FALSE;
                // Set Wheel ID
                DataToSend[1] = Convert.ToByte(WheelID);
                ExpectedResponses[2] = Convert.ToByte(WheelID);
                // Set the Filter Number
                DataToSend[2] = Convert.ToByte(FilterNumber);
                ExpectedResponses[3] = Convert.ToByte(FilterNumber);
                ExpectedResponses[4] = Convert.ToByte(this.CenteringOffset);
                NewName = NewName.PadRight(8, ' ');
                for (int i = 0; i < 8; i++)
                {
                    DataToSend[i + 3] = Convert.ToByte(NewName[i]);
                    ExpectedResponses[i + 5] = Convert.ToByte(NewName[i]);
                }
                // Create the Feature Report
                FeatureReport UpdateNameReport = new FeatureReport(FilterWheels.REPOTRID_FEATURE_FLASHOPS,
                    DataToSend);
                // Process the feature report
                this.selectedDevice.ProcessFeatureReport(UpdateNameReport);
                // Verify the correct data was received
                for (int j = 0; j < ExpectedResponses.Length; j++)
                {
                    if (ExpectedResponses[j] != UpdateNameReport.Response1[j])
                    {
                        string msg = "ExpectedResponse did not " +
                            "match ReceivedResponse1 for byte number " + j.ToString();
                        throw new ApplicationException(msg);
                    }
                    else if (ExpectedResponses[j] != UpdateNameReport.Response2[j])
                    {
                        string msg = "ExpectedResponse did not " +
                            "match ReceivedResponse2 for byte number " + j.ToString();
                        throw new ApplicationException(msg);
                    }
                }
                Trace.WriteLine("Filter Name for " + WheelID.ToString() + "-" + FilterNumber.ToString() +
                    " successfully updated to " + NewName);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EXCEPTION THROWN in FilterWhel.UpdateFilterName:" + ex.ToString());
                throw;
            }
        }

        public void UpdateWheelName(char WheelID, string NewName)
        {
            try
            {
                byte[] DataToSend = new byte[11];
                byte[] ExpectedResponses = new byte[13];
                // Set the Opcode 
                DataToSend[0] = FilterWheels.FLASH_OPCODE_UPDATE_WHEEL_NAME;
                ExpectedResponses[0] = FilterWheels.FLASH_OPCODE_UPDATE_WHEEL_NAME;
                ExpectedResponses[1] = FilterWheels.REPORT_FALSE;
                ExpectedResponses[2] = Convert.ToByte(WheelID);
                ExpectedResponses[3] = 0;
                ExpectedResponses[4] = Convert.ToByte(this.CenteringOffset);
                DataToSend[1] = Convert.ToByte(WheelID);
                NewName = NewName.PadRight(8,' ');
                for (int i = 0; i < 8; i++)
                {
                    DataToSend[i+3] = Convert.ToByte(NewName[i]);
                    ExpectedResponses[i + 5] = Convert.ToByte(NewName[i]);
                }
                FeatureReport UpdateWheelNameReport = new FeatureReport(FilterWheels.REPOTRID_FEATURE_FLASHOPS,
                    DataToSend);
                this.selectedDevice.ProcessFeatureReport(UpdateWheelNameReport);
                for (int j = 0; j < ExpectedResponses.Length; j++)
                {
                    if (ExpectedResponses[j] != UpdateWheelNameReport.Response1[j])
                    {
                        string msg = "ExpectedResponse did not " +
                            "match ReceivedResponse1 for byte number " + j.ToString();
                        throw new ApplicationException(msg);
                    }
                    else if (ExpectedResponses[j] != UpdateWheelNameReport.Response2[j])
                    {
                        string msg = "ExpectedResponse did not " +
                            "match ReceivedResponse2 for byte number " + j.ToString();
                        throw new ApplicationException(msg);
                    }
                }
                Trace.WriteLine("Wheel Name for Wheel" + WheelID.ToString() +
                    " successfully updated to " + NewName);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EXCEPTION THROWN in FilterWhel.UpdateWheelName:" + ex.ToString());
                throw;
            }
        }    

        //public string ReadFilterName(char WheelID, short FilterNumber)
        //{
        //    try
        //    {
        //        byte[] DataToSend = new byte[11];
        //        byte[] ExpectedResponses = new byte[5];
        //        DataToSend[0] = FilterWheels.FLASH_OPCODE_READ_FILTER_NAME;
        //        DataToSend[1] = Convert.ToByte(WheelID);
        //        DataToSend[2] = Convert.ToByte(FilterNumber);
        //        ExpectedResponses[0] = FilterWheels.FLASH_OPCODE_READ_FILTER_NAME;
        //        ExpectedResponses[1] = FilterWheels.REPORT_FALSE;
        //        ExpectedResponses[2] = Convert.ToByte(WheelID);
        //        ExpectedResponses[3] = Convert.ToByte(FilterNumber);
        //        ExpectedResponses[4] = Convert.ToByte(this.CenteringOffset);
        //        FeatureReport ReadNameReport = new FeatureReport(22, DataToSend);
        //        this.selectedDevice.ProcessFeatureReport(ReadNameReport);

        //        // Verify all the received data
        //        for (int j = 0; j < ExpectedResponses.Length; j++)
        //        {
        //            if (ExpectedResponses[j] != ReadNameReport.Response1[j])
        //            {
        //                string msg = "ExpectedResponse did not " +
        //                    "match ReceivedResponse1 for byte number " + j.ToString();
        //                throw new ApplicationException(msg);
        //            }
        //            else if (ExpectedResponses[j] != ReadNameReport.Response2[j])
        //            {
        //                string msg = "ExpectedResponse did not " +
        //                    "match ReceivedResponse2 for byte number " + j.ToString();
        //                throw new ApplicationException(msg);
        //            }
        //        }
        //        byte[] name = new byte[8];
        //        Array.Copy(ReadNameReport.Response1, 3, name,0,8);
        //        string s = System.Text.Encoding.ASCII.GetString(name);
        //        return s;
        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.WriteLine("EXCEPTION THROWN in FilterWheel.ReadFilterName: \n" + ex.ToString());
        //        throw;
        //    }

        //}

        public string GetErrorMessage(short ErrorState)
        {
            try
            {
                string ErrorName = "ErrorState_" + ErrorState.ToString();
                string x = Resource1.ResourceManager.GetString(ErrorName);
               
                if (x == null) x = "No Error Message Available for: " + ErrorName;
                return x;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("An erorr occurred while trying to retrieve the proper error message from the Resource Manager. \n" + 
                    "Exception Data = \n" + ex.ToString() + "\n");
                return "No Error Message Available";
            }
        }

        #endregion

        #region Event Triggering

        private void TriggerHomingStarted()
        {
            TriggerAnEvent(this.HomingStarted);
        }        

        private void TriggerHomingComplete()
        {
            TriggerAnEvent(this.HomingCompleted);
        }       

        private void TriggerMoveStarted()
        {
            TriggerAnEvent(this.MoveStarted);
        }      

        private void TriggerMoveComplete()
        {
            TriggerAnEvent(MoveComplete);
        }
        
        private void TriggerFilterWheelUnplugged(Object sender, EventArgs e)
        {
            TriggerAnEvent(DeviceRemoved);
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

        #endregion
    }
}
