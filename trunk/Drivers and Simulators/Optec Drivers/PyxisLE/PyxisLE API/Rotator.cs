using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OptecHIDTools;
using System.Diagnostics;
using System.Threading;


namespace PyxisLE_API
{
    
    public class Rotator
    {
        private string name;
        private string serialNumber;
        private string manufacturer;
        private HID selectedDevice = null;
        private bool isHomed = false;
        private bool isHoming = false;
        private bool isMoving = false;
        private Int32 zeroOffset = 0;
        private double targetPosition = 0;
        private double currentPosition = 0;
        private Int32 stepsPerRev = 0;
        private short errorState = 0;
        private string firmwareVersion = "";
        private char deviceType = '?';
        private bool reverseProperty = false;
        private bool returnToLast = false;

        private Thread HomeThread;
        private Thread MoveThread;

        public event EventHandler HomeFinished;
        public event EventHandler MoveFinished;
        public event EventHandler DeviceUnplugged;

        public Rotator(HID Device)
        {
            this.selectedDevice = Device;
            this.manufacturer = Device.Manufacturer;
            this.name = Device.ProductDescription;
            this.serialNumber = Device.SerialNumber;
            this.selectedDevice.DeviceRemoved += new EventHandler(TriggerRotatorUnplugged);

            TimeSpan MaxHomeTime = new TimeSpan(0, 0, 60);
            DateTime FirstAttempt = DateTime.Now;
            RefershDeviceDescription();
            // Keep checking device status until the device is homed...
            TryAgain:
            RefreshDeviceStatus();
            if (DateTime.Now.Subtract(FirstAttempt) > MaxHomeTime)
                throw new System.ApplicationException("Home Procedure took too long");
            else if (this.isHoming == true)
            {
                System.Threading.Thread.Sleep(500);
                goto TryAgain;
            }
            else if (this.ErrorState == 0)
            {
                RefershDeviceDescription();
            }
        }

        private void RefershDeviceDescription()
        {
            try
            {
                InputReport DescriptionReport = new InputReport(Rotators.REPORTID_INPUT_DEVICE_DESC);
                this.selectedDevice.RequestInputReport_Control(DescriptionReport);
                ParseDeviceDescription(DescriptionReport);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EXCEPTION THROWN in Rotator.RefershDeviceDescription: \n" + ex.ToString());
                throw;
            }
        }

        private void ParseDeviceDescription(InputReport DescriptionReport)
        {
            try
            {
                // ***************Parse The Received Data*****************////
                short rReportID = 0;
                short rFirmwareVerMajor = 0;
                short rFirmwareVerMinor = 0;
                short rFirmwareVerRevision = 0;
                char rDeviceType = '?';
                Int32 rZeroOffset = 0;
                Int32 rStepsPerRev = 0;
                short rreverseProperty = 0;
                short rreturnToLast = 0;
                
                // Check that at least one byte of data was received.
                if (DescriptionReport.ReceivedData.Length < 6) throw new ApplicationException("Device Status Not Received");

                // Convert the received bytes to usable types.
                rReportID = Convert.ToInt16(DescriptionReport.ReceivedData[0]);
                rFirmwareVerMajor = Convert.ToInt16(DescriptionReport.ReceivedData[1]);
                rFirmwareVerMinor = Convert.ToInt16(DescriptionReport.ReceivedData[2]);
                rFirmwareVerRevision = Convert.ToInt16(DescriptionReport.ReceivedData[3]);
                rDeviceType = Convert.ToChar(DescriptionReport.ReceivedData[4]);
                rreverseProperty = Convert.ToInt16(DescriptionReport.ReceivedData[13]);
                rreturnToLast = Convert.ToInt16(DescriptionReport.ReceivedData[14]);

                // Verify the ReportID is correct
                if (rReportID != Rotators.REPORTID_INPUT_DEVICE_DESC)
                {
                    throw new ApplicationException("Wrong ReportID returned from device status request.");
                }

                // Extract the Firmware Version
                string tempFV = "V" + rFirmwareVerMajor.ToString();
                tempFV += "." + rFirmwareVerMinor.ToString();
                tempFV += "." + rFirmwareVerRevision.ToString();
                this.firmwareVersion = tempFV;

                // Extract the Device Type
                if (rDeviceType < 'A' || rDeviceType > 'Z') throw new ArgumentException("Device type was not A through Z");
                else this.deviceType = Convert.ToChar(rDeviceType);

                // Extract the Zero Offset
                rZeroOffset = BitConverter.ToInt32(
                    new byte[] {
                        DescriptionReport.ReceivedData[5],
                        DescriptionReport.ReceivedData[6],
                        DescriptionReport.ReceivedData[7],
                        DescriptionReport.ReceivedData[8]}, 0);
                this.zeroOffset = rZeroOffset;
                // Extract the StepsPerRev
                rStepsPerRev = BitConverter.ToInt32(
                    new byte[] {
                        DescriptionReport.ReceivedData[9],
                        DescriptionReport.ReceivedData[10],
                        DescriptionReport.ReceivedData[11],
                        DescriptionReport.ReceivedData[12]}, 0);
                stepsPerRev = rStepsPerRev;
                // Extract the Reverse Property
                if (rreverseProperty == Rotators.REPORT_TRUE) this.reverseProperty = true;
                else if (rreverseProperty == Rotators.REPORT_FALSE) this.reverseProperty = false;
                else throw new ApplicationException("Invalid data received for Reverse property");
                // Extract the ReturnToLast Property
                if (rreturnToLast == Rotators.REPORT_TRUE) this.returnToLast = true;
                else if (rreturnToLast == Rotators.REPORT_FALSE) this.returnToLast = false;
                else throw new ApplicationException("Invalid data received for ReturnToLast property");


            }
            catch (Exception ex)
            {
                Trace.WriteLine("EXCEPTION THROWN in Rotator.ParseDeviceDescription: \n" + ex.ToString());
                throw;
            }
        }

        private void RefreshDeviceStatus()
        {
            try
            {
                InputReport StatusReport = new InputReport(Rotators.REPORTID_INPUT_DEVICE_STATUS);
                this.selectedDevice.RequestInputReport_Control(StatusReport);
                ParseDeviceStatus(StatusReport);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EXCEPTION THROWN in Rotator.RefreshDeviceStatus: \n" + ex.ToString());
                throw;
            }
        }

        private void ParseDeviceStatus(InputReport StatusReport)
        {
            short rReportID = 0;
            short rIsHomed = 0;
            short rIsHoming = 0;
            short rIsMoving = 0;
            double rCurrentPosition = 0;
            double rTargetPosition = 0;
            short rErrorState = 0;

            //TODO: Enter the correct number of bytes here.
            //Check that enough bytes were received
            if (StatusReport.ReceivedData.Length < 18) throw new ApplicationException("Device Status Not Received!");

            //Check if an ErrorState is set


            // Convert the received bytes to usable types
            rReportID = Convert.ToInt16(StatusReport.ReceivedData[0]);

            // NOTE: When converting to Ints I reverse the bytes because the MCU is LittleEndian.
            rCurrentPosition = BitConverter.ToInt32(
                new byte[] { 
                    StatusReport.ReceivedData[1],
                    StatusReport.ReceivedData[2],
                    StatusReport.ReceivedData[3],
                    StatusReport.ReceivedData[4]}, 0);
            // NOTE: When converting to Ints I reverse the bytes because the MCU is LittleEndian.
            rTargetPosition = BitConverter.ToInt32(
                new byte[] {
                    StatusReport.ReceivedData[5],
                    StatusReport.ReceivedData[6],
                    StatusReport.ReceivedData[7],
                    StatusReport.ReceivedData[8]}, 0);

            rIsHomed = Convert.ToInt16(StatusReport.ReceivedData[9]);
            rIsHoming = Convert.ToInt16(StatusReport.ReceivedData[10]);
            rIsMoving = Convert.ToInt16(StatusReport.ReceivedData[11]);
            rErrorState = Convert.ToInt16(StatusReport.ReceivedData[13]);


            // Verify the ReportID is correct
            if (rReportID != Rotators.REPORTID_INPUT_DEVICE_STATUS)
            {
                throw new ApplicationException("Wrong ReportID returned from device status request.");
            }
            // Extract the DeviceIsHomedFlag
            if (rIsHomed == Rotators.REPORT_TRUE)
            {
                this.isHomed = true;
            }
            else if (rIsHomed == Rotators.REPORT_FALSE)
            {
                this.isHomed = false;
            }
            else throw new ApplicationException("Invalid data received for IsHomed value");

            // Extract the DeviceIsHomingFlag
            if (rIsHoming == Rotators.REPORT_TRUE)
            {
                this.isHoming = true;
            }
            else if (rIsHoming == Rotators.REPORT_FALSE)
            {
                this.isHoming = false;
            }
            else throw new ApplicationException("Invalid data received for IsHoming value");

            // Extract the DeviceIsMovingFlag
            if (rIsMoving == Rotators.REPORT_TRUE)
            {
                this.isMoving = true;
            }
            else if (rIsMoving == Rotators.REPORT_FALSE)
            {
                this.isMoving = false;
            }
            else throw new ApplicationException("Invalid data received for IsMoving value");

            // Extract the Current Position
            rCurrentPosition = (rCurrentPosition/(StepsPerRev/360));
            this.currentPosition = rCurrentPosition;

            // Extract the Target Position
            rTargetPosition = (rTargetPosition / (StepsPerRev / 360));
            this.targetPosition = rTargetPosition;
 
        }

        private void TriggerRotatorUnplugged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        // ***** Public Properties*********************

        public bool IsHomed
        {
            get { return isHomed; }
        }
        public bool IsHoming
        {
            get { return isHoming; }
        }
        public bool IsMoving
        {
            get { return isMoving; }
        }

        public short ErrorState
        {
            get { return errorState; }
        }

        public string FirmwareVersion
        {
            get { return firmwareVersion; }
        }

        public double CurrentPosition
        {
            get {
                RefreshDeviceStatus();
                return currentPosition; }
        }

        public double TargetPosition
        {
            get { return targetPosition; }
        }

        public Int32 StepsPerRev
        {
            get { return stepsPerRev; }
        }

        public char DeviceType
        {
            get { return deviceType; }
        }

        public Int32 ZeroOffset
        {
            get { return zeroOffset; }
        }

        public string SerialNumber
        {
            get { return this.selectedDevice.SerialNumber; }
        }

        public bool Reverse
        {
            get 
            { 
                RefershDeviceDescription();
                return reverseProperty;
            }
            set 
            {
                byte[] datatosend = new byte[] { };
                // Create the report to send
                FeatureReport ReverseReport = new FeatureReport(
                    Rotators.REPORTID_FEATURE_DO_MOTION, datatosend);
                // Feature Reports Must have at least one data item so we put a zero in it.
                // Create a byte to hold the new value
                byte NewValue = 0;
                if (value) NewValue = Rotators.REPORT_TRUE;
                else NewValue = Rotators.REPORT_FALSE;
                // Set the opcode byte and the new value byte
                ReverseReport.DataToSend = new byte[] { Rotators.MOTION_OPCODE_SET_REVERSE, NewValue };
                // Send the Report
                this.selectedDevice.ProcessFeatureReport(ReverseReport);
                // Update the property value by reading it from the device.
                RefreshDeviceStatus();
            }
        }

        public bool ReturnToLastOnHome
        {
            get
            {
                RefershDeviceDescription();
                return returnToLast;
            }
            set
            {
                byte[] datatosend = new byte[] { };
                // Create the report to send
                FeatureReport ReturnToHomeReport = new FeatureReport(
                    Rotators.REPORTID_FEATURE_DO_MOTION, datatosend);
                // Feature Reports Must have at least one data item so we put a zero in it.
                // Create a byte to hold the new value
                byte NewValue = 0;
                if (value) NewValue = Rotators.REPORT_TRUE;
                else NewValue = Rotators.REPORT_FALSE;
                // Set the opcode byte and the new value byte
                ReturnToHomeReport.DataToSend = new byte[] { Rotators.MOTION_OPCODE_SET_RETURNTOLAST, NewValue };
                // Send the Report
                this.selectedDevice.ProcessFeatureReport(ReturnToHomeReport);
                // Update the property value by reading it from the device.
                RefreshDeviceStatus();
            }
        }

        // ******* Private Methods ***************************************

        private void MoveMonitor()
        {
            if (isMoving)
            {
                RefreshDeviceStatus();
                while (isMoving)
                {
                    RefreshDeviceStatus();
                    if (this.errorState != 0)
                    {
                        string errormsg = GetErrorMessage(ErrorState);
                        Trace.WriteLine(errormsg);
                        throw new ApplicationException("An Error State Has Been Set In the Device Firmware. \n" +
                        errormsg);
                    }
                    System.Threading.Thread.Sleep(250);
                }
                TriggerMoveComplete();
            }
        }

        private void HomeMonitor()
        {
            RefreshDeviceStatus();

            while (isHoming)
            {
                RefreshDeviceStatus();
                if (this.errorState != 0)
                {
                    string errormsg = GetErrorMessage(ErrorState);
                    Trace.WriteLine(errormsg);
                    throw new ApplicationException("An Error State Has Been Set In the Device Firmware. \n" +
                    errormsg);
                }
                System.Threading.Thread.Sleep(250);
            }
            TriggerHomingComplete();
            if (returnToLast)
            {
                MoveMonitor();
            }
            
        }

        private void TriggerHomingComplete()
        {
            TriggerAnEvent(this.HomeFinished);
        }

        private void TriggerMoveComplete()
        {
            TriggerAnEvent(this.MoveFinished);
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


        // ******* Public Methods ***************************************
        public void Home()
        {
            byte[] datatosend = new byte[] { };

            // Create the report to send
            FeatureReport HomeReport = new FeatureReport(
                Rotators.REPORTID_FEATURE_DO_MOTION, datatosend);
            // Feature Reports Must have at least one data item so we put a zero in it.
            HomeReport.DataToSend = new byte[] { Rotators.MOTION_OPCODE_DOHOME };
            
            // Send the Report
            this.selectedDevice.ProcessFeatureReport(HomeReport);

            // Verify the first response was correct (Verify device is homing)
            if (HomeReport.Response1[0] != Rotators.REPORT_TRUE)
            {
                throw new ApplicationException("The device never started homing according to response1 from feature report");
            }
            // Verify the second response does not have error code set
            if (HomeReport.Response2[3] != 0)
            {
                errorState = Convert.ToInt16(HomeReport.Response2[3]);
                string msg = GetErrorMessage(errorState);
            }

            // Start the HomeMonitor thread
            ThreadStart ts = new ThreadStart(this.HomeMonitor);
            HomeThread = new Thread(ts);
            HomeThread.Start();
        }

        private string GetErrorMessage(short errorState)
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
                Trace.WriteLine("An erorr occurred while trying to retrieve the correct error message from the Resource Manager. \n" +
                    "Exception Data = \n" + ex.ToString() + "\n");
                return "No Error Message Available";
            }
        }

        public void ChangePosition(double NewPos)
        {
            // First check that the new pos is in the range of 0-359.9999999
          //  if ((NewPos < 0) || NewPos > 360) throw new ApplicationException("New Position is outside the acceptable range.");
        
            UInt32 NewPosInt = (uint)Math.Round((NewPos * (double) this.StepsPerRev / (double)360)); 
            byte[] datatosend = new byte[] { };

            // Create the report to send
            FeatureReport MoveReport = new FeatureReport(
                Rotators.REPORTID_FEATURE_DO_MOTION, datatosend);
            // Feature Reports Must have at least one data item so we put a zero in it.
            byte[] x = BitConverter.GetBytes(NewPosInt);
            MoveReport.DataToSend = new byte[] { Rotators.MOTION_OPCODE_DOMOVE,
                x[0], x[1], x[2], x[3] };

            // Send the Report
            this.selectedDevice.ProcessFeatureReport(MoveReport);
            RefreshDeviceStatus();

            // Check that no error codes were set
            if (MoveReport.Response2[3] != 0)
            {
                errorState = Convert.ToInt16(MoveReport.Response2[3]);
                string msg = GetErrorMessage(errorState);
            }

            // Start the MoveMonitor thread
            ThreadStart ts = new ThreadStart(this.MoveMonitor);
            MoveThread = new Thread(ts);
            MoveThread.Start();
        }

        public void ChangePosition_Relative(double Degrees)
        {
            double CP = this.CurrentPosition;
            double NewAbsPos = 0;
            if ((Degrees < -360) || Degrees > 360) throw new ApplicationException("Offset is outside the acceptable range.");
            if (Degrees > 0)
            {
                if ((Degrees + CP) > 360)
                {
                    NewAbsPos = Degrees - (360 - CP);
                }
                else NewAbsPos =CP + Degrees;
            }
            else
            {
                if (Degrees + CP < 0)
                {
                    NewAbsPos = 360 + (Degrees + CP);
                }
                else NewAbsPos = CP + Degrees;
            }
            // Convert the new position to a step count.
            Int32 NewPosInt = (int)(NewAbsPos * (double)this.StepsPerRev / (double)360);
            byte[] datatosend = new byte[] { };
            // Create the report to send
            FeatureReport MoveReport = new FeatureReport(
                Rotators.REPORTID_FEATURE_DO_MOTION, datatosend);
            // Feature Reports Must have at least one data item so we put a zero in it.
            byte[] x = BitConverter.GetBytes(NewPosInt);
            MoveReport.DataToSend = new byte[] { Rotators.MOTION_OPCODE_DOMOVE,
                x[0], x[1], x[2], x[3] };

            // Send the Report
            this.selectedDevice.ProcessFeatureReport(MoveReport);
            RefreshDeviceStatus();
            // Start the MoveMonitor thread
            ThreadStart ts = new ThreadStart(this.MoveMonitor);
            MoveThread = new Thread(ts);
            MoveThread.Start();
        }

        public void Halt_Move()
        {
            if (IsHoming) throw new ApplicationException("You can not halt device while homing");
            else if (true)
            {
                // Prepart the feature report to send.
                byte[] datatosend = new byte[] { Rotators.MOTION_OPCODE_HALT };
                FeatureReport HaltReport = new FeatureReport(Rotators.REPORTID_FEATURE_DO_MOTION, datatosend);
                // Send the report to request the halt
                this.selectedDevice.ProcessFeatureReport(HaltReport);
                // Check the IsMoving bit
                if (HaltReport.Response1[2] == Rotators.REPORT_TRUE) isMoving = true;
                else if (HaltReport.Response1[2] == Rotators.REPORT_FALSE) isMoving = false;
                // Alert if device is not halted.
                if (IsMoving) throw new ApplicationException("Device did not respond to halt request");


            }
        }
    }
}
