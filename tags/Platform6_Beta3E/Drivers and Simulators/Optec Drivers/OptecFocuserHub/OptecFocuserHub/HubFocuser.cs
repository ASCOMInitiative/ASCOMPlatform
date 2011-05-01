using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using System.Threading;

namespace ASCOM.OptecFocuserHubTools
{
    public class HubFocuser
    {
        //public delegate void FocuserEventHandler(object sender, FocuserEventArgs e); 

        private static FocuserCommunicator focuserCommunicator;     // Static because both of the focusers share the same device.
        private DateTime lastStatusUpdateTime = new DateTime(1, 1, 1);
        private DateTime lastConfigUpdateTime = new DateTime(1, 1, 1);


        #region Constructors / Destructors **************************************************************************************************

        public HubFocuser(FOCUSER_NUMBER n, FocuserCommunicator focComm)
        {
            //this.connectionState = CONNECTION_STATES.DISCONNECTED;
            Debug.Print("Creating Hub Focuser");
            focuserCommunicator = focComm;
            this.focuserNumber = n;
        }

        internal void SetFocuserCommunicator(FocuserCommunicator newFc)
        {
            focuserCommunicator = newFc;
        }

        #endregion

        #region Private Fields of Focuser *************************************************************************************************

        const char newline = '\n';
        private FOCUSER_NUMBER focuserNumber = FOCUSER_NUMBER.ONE;
        // Config values
        private int maxPosition = 0;
        private string nickname = "Unknown";
        private int maxSpeed = 0;
        private int maxPower = 0;
        private char deviceType = 'A';
        private bool tempCompEnabled = false;
        private int tempCompA, tempCompB, tempCompC, tempCompD, tempCompE;
        private string tempCompMode = "";
        private bool backlashCompEnabled = false;
        private int backlashCompSteps = 0;
        private int ledBrightness = 0;
        private bool isInSleepMode = false;
        // Status values
        private double currentTempC = 0;
        private int currentPosSteps = 0;
        private int targetPosSteps = 0;
        private bool isMoving = false;
        private bool isHoming = false;
        private bool isHomed = false;
        private bool tempProbeAttached = false;
        private bool remoteInOutAttached = false;
        private bool handControlAttached = false;

        #endregion



        #region Public Properties of Focuser ***************************************************************************************

        #endregion

        #region Public NonStatic Properties of Focuser ************************************************************************************

        public int MaxPosition
        {
            get
            {
                getConfig();
                return maxPosition;
            }
        }

        public int CurrentPositionSteps
        {
            get
            {
                getStatus();
                return currentPosSteps;
            }
        }

        public int CurrentPositionMicrons
        {
            get { throw new ApplicationException(); }
        }

        public int TargetPositionSteps
        {
            get
            {
                getStatus();
                return targetPosSteps;
            }
        }

        public int TargetPositionMicrons
        {
            get { throw new ApplicationException(); }
        }

        public double CurrentTempC
        {
            get { getStatus(); return currentTempC; }
        }

        public double CurrentTempF
        {
            get { throw new ApplicationException(); }
        }

        public double CurrentTempK
        {
            get { throw new ApplicationException(); }
        }

        public bool IsMoving { get { getStatus(); return isMoving; } }

        public bool IsHoming { get { getStatus(); return isHoming; } }

        public string Nickname { get { getConfig(); return nickname; } set { setDeviceNickname(value); } }

        public char DeviceType { get { getConfig();  return deviceType; } set { setDeviceType(value); } }

        public int LEDBrightness { get { getConfig(); return ledBrightness;} set { setLEDBrightness(value);}}

        public bool BacklashCompEnabled { get { getConfig(); return backlashCompEnabled; } set { setBacklashCompEnabled(value); } }

        public int BacklashCompSteps { get { getConfig(); return backlashCompSteps; } set { setBacklashCompSteps(value); } }

        public bool TempCompEnabled { get { getConfig(); return tempCompEnabled; } set { setTempCompEnabled(value); } }

        public bool TempProbeAttached { get { getStatus(); return tempProbeAttached; } }

        #endregion

        #region Private Methods of Focuser ************************************************************************************************

        internal void getConfig()
        {

#if DEBUG
            Debug.Print("Getting Config");
#endif

            // 1. Make sure there has been some delay since the last check...
            if (DateTime.Now.Subtract(lastConfigUpdateTime).TotalMilliseconds < 300)
                return;
            else lastConfigUpdateTime = DateTime.Now;

            // 2. Format the command string
            string cmd = (this.focuserNumber == FOCUSER_NUMBER.ONE) ? "<F1GETCONFIG>" : "<F2GETCONFIG>";

            
            List<string> ConfigData = new List<string>();
            try
            {
                // 3. Send the command string and get response
                ConfigData = focuserCommunicator.SendStringReadLines(cmd, "END", 1000);
                ClearTimeoutCount();        // Because the operation did not time out
            }
            catch
            {
                HandleTimeoutEvent();
            }

            try
            {
                // 4. Try to verify good response data...
                if (ConfigData[0] == "!")
                    ConfigData.RemoveAt(0);
                else
                    throw new ApplicationException();
                if (ConfigData[0] == "CONFIG")
                    ConfigData.RemoveAt(0);
                else
                    throw new ApplicationException();

                // 5. Parse the Config data
                parseConfigData(ConfigData);

                // 6. Clear the bad response count because we interpreted good data
                ClearBadResponseCount();
            }
            catch
            {
                HandleBadResponse();
                return;
            }
          
#if DEBUG
            Debug.Print("Finished getting Config");
#endif

        }

        private void parseConfigData(List<string> data)
        {
            //
            // Parse the received configuration string
            // No Try/Catch block because we want exceptions to be handled by the caller.
            //
            int start = 11;

            // 1. Get the device Nickname
            this.nickname = data[0].Substring(start);
            // 2. Get the Max Position
            this.maxPosition = int.Parse(data[1].Substring(start));
            // 3. Get the Max Speed
            this.maxSpeed = int.Parse(data[2].Substring(start));
            // 4. Get the Max Power
            this.maxPower = int.Parse(data[3].Substring(start));
            // 5. Get the Device Type
            this.deviceType = data[4].Substring(start)[0];
            // 6. Get the Temp Comp On flag
            this.tempCompEnabled = (data[5].Substring(start) == "1") ? true : false;
            // 7. Get the Temperature Coefficient for A
            this.tempCompA = int.Parse(data[6].Substring(start));
            // 8. Get the Temperature Coefficient for B
            this.tempCompB = int.Parse(data[7].Substring(start));
            // 9. Get the Temperature Coefficient for C
            this.tempCompC = int.Parse(data[8].Substring(start));
            // 10. Get the Temperature Coefficient for D
            this.tempCompD = int.Parse(data[9].Substring(start));
            // 11. Get the Temperature Coefficient for E
            this.tempCompE = int.Parse(data[10].Substring(start));
            // 12. Get the Temp Comp Mode
            this.tempCompMode = data[11].Substring(start);
            // 13. Get the Backlash Compensation Enabled Flag
            this.backlashCompEnabled = (data[12].Substring(start) == "1") ? true : false;
            // 14 Get the Backlash Compensatino Steps 
            this.backlashCompSteps = int.Parse(data[13].Substring(start));
            // 15. Get the LED brightness setting
            this.ledBrightness = int.Parse(data[14].Substring(start));
            // 16. Get the SleepMode designator
            this.isInSleepMode = (data[15].Substring(start) == "1") ? true : false;
        }

        internal void getStatus()
        {
            Debug.Print("Getting Status"); 
            // 1. Make sure there has been some delay since the last check...
            if (DateTime.Now.Subtract(lastStatusUpdateTime).TotalMilliseconds < 300)
            {
                Debug.Print("not enough time - Satus" + " - Focuser: " + focuserNumber.ToString());
                return;
            }
            else
            {
                lastStatusUpdateTime = DateTime.Now;
            }
            Debug.Print("Updating Status - " + focuserNumber.ToString() + " " +  DateTime.Now.ToString());

            // 2. Format the command string
            string cmd = (this.focuserNumber == FOCUSER_NUMBER.ONE) ? "<F1GETSTATUS>" : "<F2GETSTATUS>";
            List<string> StatusData = new List<string>();
            try
            {
                // 3. Send the command string and get responses
                StatusData = focuserCommunicator.SendStringReadLines(cmd, "END", 1000);
                ClearTimeoutCount();    // Because the communication did not time out.
            }
            catch
            {
                HandleTimeoutEvent();
                return;
            }
           
            try
            {
                // 4. Verify correct responses
                if (StatusData[0] == "!")
                    StatusData.RemoveAt(0);
                else
                {
                    HandleBadResponse();
                    return;
                }
                if (StatusData[0] == "STATUS")
                    StatusData.RemoveAt(0);
                else
                {
                    HandleBadResponse();
                    return;
                }
                // 5. Parse the Status data
                parseStatusData(StatusData);

                // 6. Clear the bad response count since the data was good.
                ClearBadResponseCount();
            }
            catch
            {
                HandleBadResponse();
                return;
            }
            Debug.Print("Finished getting Status");

        }

        private void parseStatusData(List<string> data)
        {
            //
            // Parse the received status strings
            // No Try/Catch block because we want exceptions to be handled by the caller.
            //   
            int start = 11;
            int i = 0;
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NegativeSign = "-"; nfi.PositiveSign = "+";
            nfi.NumberDecimalSeparator = "."; nfi.NumberGroupSeparator = ",";

            // 1. Get the Current Temp
            string temp = data[i++].Substring(start);
            this.currentTempC = double.Parse(temp, nfi);
            // 2. Get the Current Position
            this.currentPosSteps = int.Parse(data[i++].Substring(start));
            // 3. Get the Target Position
            this.targetPosSteps = int.Parse(data[i++].Substring(start));
            // 4. Get the isMoving Flag
            this.isMoving = (data[i++].Substring(start) == "1") ? true : false;
            // 5. Get the isHoming Flag
            this.isHoming = (data[i++].Substring(start) == "1") ? true : false;
            // 6. Get the isHomed Flag
            this.isHomed = (data[i++].Substring(start) == "1") ? true : false;
            // 7. Get the TempProbeAttached flag
            this.tempProbeAttached = (data[i++].Substring(start) == "1") ? true : false;
            // 8. Get the Remote In/Out Attached flag
            this.remoteInOutAttached = (data[i++].Substring(start) == "1") ? true : false;
            // 9. Get the HandController Attached flag
            this.handControlAttached = (data[i++].Substring(start) == "1") ? true : false;
        }

        private void setDeviceNickname(string newName)
        {
            // Make sure the name is 16 or less characters
            if (newName.Length > 16)
                throw new ApplicationException("Nickname must be less than or equal to 16 characters in length.");
            if (newName.Length < 1)
                throw new ApplicationException("Nickname mus tbe at least one character in length.");
            // Make sure it contains acceptable characters.
            if(newName.Contains("<") || newName.Contains(">"))
                throw new ApplicationException("Nickname cannot conatin the characters '<' or '>'.");
            string cmd = (focuserNumber == FOCUSER_NUMBER.ONE) ? ("<F1SCNN") : ("<F2SCNN");
            cmd += newName + ">";
            List<string> resp = new List<string>();
            try
            {
                resp = focuserCommunicator.SendStringReadLines(cmd, 2, 1000);
                ClearTimeoutCount();
            }
            catch
            {
                HandleTimeoutEvent();
                return;
            }
            // Check for the bang
            try
            {
                if (resp[0] != "!")
                    throw new ApplicationException("Did not receive ! in response to setting device Nickname. Response = " + resp[0]);
                // Check for "SET"
                if (resp[1] != "SET")
                    throw new ApplicationException("Did not receive SET in response to setting device Nickname. Response = " + resp[1]);
                // Clear bad response count because we got good data
                ClearBadResponseCount();
            }
            catch
            {
                HandleBadResponse();
            }
        }

        private void setDeviceType(char newType)
        {

            // Make sure it contains acceptable characters.
            if (newType < 'A' || newType > 'G')
                throw new ApplicationException("New device type must be 'A' through 'G'");
            string cmd = (focuserNumber == FOCUSER_NUMBER.ONE) ? ("<F1SCDT") : ("<F2SCDT");
            cmd += newType + ">";
            List<string> resp = new List<string>();
            try
            {
                resp = focuserCommunicator.SendStringReadLines(cmd, 2, 1000);
                ClearTimeoutCount();    // Because it didn't timeout
            }
            catch
            {
                HandleTimeoutEvent();
            }
            try
            {
                // Check for the bang
                if (resp[0] != "!")
                    throw new ApplicationException("Did not receive ! in response to setting device type. Response = " + resp[0]);
                // Check for "SET"
                if (resp[1] != "SET")
                    throw new ApplicationException("Did not receive SET in response to setting device type. Response = " + resp[1]);
                ClearBadResponseCount();
            }
            catch
            {
                HandleBadResponse();
            }
            try
            {
                // The device will automatically reset now... Disconnect from the device. The user will have to reconnect.
                focuserCommunicator.ConnectionOpen = false;
            }
            catch { }
            
        }

        private void setLEDBrightness(int newVal)
        {
            // Check if new value is in acceptable range
            if (newVal < 0)
            {
                throw new ApplicationException("LED Brightness must be greater than zero");
            }
            if (newVal > 100)
            {
                throw new ApplicationException("LED Brightness must be less than or equal to 100");
            }
            if (newVal == ledBrightness) 
                return;

            string cmd = "<F1SCLB" + newVal.ToString("000") + ">";
            List<string> resp = new List<string>();
            try
            {
                // Set the new value
                resp = focuserCommunicator.SendStringReadLines(cmd, 2, 1000);
                ClearTimeoutCount();
            }
            catch
            {
                HandleTimeoutEvent();
            }
            try
            {
                // Check for the bang
                if (resp[0] != "!")
                    throw new ApplicationException("Did not receive ! in response to setting device LED Brightness. Response = " + resp[0]);
                // Check for "SET"
                if (resp[1] != "SET")
                    throw new ApplicationException("Did not receive SET in response to setting device LED Brightness. Response = " + resp[1]);
                ClearBadResponseCount();
            }
            catch
            {
                HandleBadResponse();
            }
        }

        private void setBacklashCompEnabled(bool enable)
        {
            if (enable == backlashCompEnabled) return;

            string cmd = (focuserNumber == FOCUSER_NUMBER.ONE) ? ("<F1SCBE") : ("<F2SCBE");
            if (enable) cmd += "1>";
            else cmd += "0>";
            List<string> resp = new List<string>();
            try
            {
                resp = focuserCommunicator.SendStringReadLines(cmd, 2, 1000);
                ClearTimeoutCount();
            }
            catch
            {
                HandleTimeoutEvent();
            }
            try
            {
                // Check for the bang
                if (resp[0] != "!")
                    throw new ApplicationException("Did not receive ! in response to setting backlash compensation. Response = " + resp[0]);
                // Check for "SET"
                if (resp[1] != "SET")
                    throw new ApplicationException("Did not receive SET in response to setting device backlash compensation. Response = " + resp[1]);
                ClearBadResponseCount();
            }
            catch
            {
                HandleBadResponse();
            }
        }

        private void setBacklashCompSteps(int newValue)
        {
            //System.Windows.Forms.MessageBox.Show("attch");
            if (newValue == backlashCompSteps) return;
            if (newValue < -99 || newValue > 99) throw new ApplicationException("Backlash comp steps must be between -99 and 99");

            string cmd = (focuserNumber == FOCUSER_NUMBER.ONE) ? ("<F1SCBS") : ("<F2SCBS");
            if (newValue >= 0) cmd += "+";
            else cmd += "-";
            cmd += newValue.ToString("00") + ">";
            List<string> resp = new List<string>();
            try
            {
                resp = focuserCommunicator.SendStringReadLines(cmd, 2, 1000);
                ClearTimeoutCount();
            }
            catch
            {
                HandleTimeoutEvent();
            }

            try
            {
                // Check for the bang
                if (resp[0] != "!")
                    throw new ApplicationException("Did not receive ! in response to setting backlash compensation steps. Response = " + resp[0]);
                // Check for "SET"
                if (resp[1] != "SET")
                    throw new ApplicationException("Did not receive SET in response to setting device backlash compensation steps. Response = " + resp[1]);
                ClearBadResponseCount();
            }
            catch
            {
                HandleBadResponse();
            }
        }

        private void setTempCompEnabled(bool enabled)
        {
            // Prepare the command to enable/disable temp comp
            string cmd = (focuserNumber == FOCUSER_NUMBER.ONE) ? "<F1SCTE" : "<F2SCTE";
            if (enabled) cmd += "1>";
            else cmd += "0>";
            // Send the command and get response
            List<string> resp = new List<string>();
            try
            {
                resp = focuserCommunicator.SendStringReadLines(cmd, 2, 1000);
                ClearTimeoutCount();
            }
            catch
            {
                HandleTimeoutEvent();
            }

            try
            {
                // Verify success
                if (resp[0] != "!")
                    throw new ApplicationException("Did not receive ! in response to setting temp comp enabled flag. Response = " + resp[0]);
                if (resp[1] != "SET")
                    throw new ApplicationException("Did not receive SET in response to setting device backlash compensation steps. Response = " + resp[1]);
                ClearBadResponseCount();
            }
            catch
            {
                HandleBadResponse();
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

        #region Public Non-Static Methods of Focuser **************************************************************************************

        public void MoveAbsolute(int newPos)
        {
            // 1. Verify the position is valid
            if (newPos > MaxPosition) throw new ApplicationException("Invalid Position Request");
            // 2. Format the command string to the form <F1MA12345>
            string cmd = (this.focuserNumber == FOCUSER_NUMBER.ONE) ? "<F1MA" : "<F2MA";
            cmd += (newPos.ToString().PadLeft(5, '0') + ">");
            // 3. Send the command and get responses
            List<string> respToMoveCmd = new List<string>();
            try
            {
                respToMoveCmd = focuserCommunicator.SendStringReadLines(cmd, 2, 1000);
                ClearTimeoutCount();
            }
            catch
            {
                HandleTimeoutEvent();
            }
            try
            {
                // 4. Check for the bang
                if (respToMoveCmd[0] != "!")
                    throw new ApplicationException("Did not receive \"!\" in response to sent command \"" + cmd + "\"");
                // 5. Check for the acknowledgment (or error text if there was a problem)
                if (respToMoveCmd[1] != "M")
                    throw new ApplicationException("The device return the error \"" + respToMoveCmd[1] + "\" from command \"" + cmd + "\".");
                ClearBadResponseCount();
            }
            catch
            {
                HandleBadResponse();
            }
        }

        public void Home()
        {
            // 1. create the command string
            string cmd = (this.focuserNumber == FOCUSER_NUMBER.ONE) ? "<F1HOME>" : "<F2HOME>";
            // 2. send the command and get responses
            List<string> respToHomeCmd = new List<string>();
            try
            {
                respToHomeCmd = focuserCommunicator.SendStringReadLines(cmd, 2, 1000);
                ClearTimeoutCount();
            }
            catch 
            {
                HandleTimeoutEvent();
            }
            try
            {
                // 3. Check for the bang
                if (respToHomeCmd[0] != "!")
                    throw new ApplicationException("Did not receive \"!\" in response to sent command \"" + cmd + "\"");
                // 4. Check the acknowledgment (or error text if there was a problem)
                if (respToHomeCmd[1] != "H")
                    throw new ApplicationException("The device return the error \"" + respToHomeCmd[1] + "\" from command \"" + cmd + "\".");
                // 5. Clear crap
                ClearBadResponseCount();
            }
            catch 
            {
                HandleBadResponse();
            }
        }

        public void Halt() // This should send the halt command to the focuser regardless of whether the focuser is actually moving or not.
        {
            // Prepare the command to send
            string cmd = (focuserNumber == FOCUSER_NUMBER.ONE) ? "<F1HALT>" : "<F2HALT>";
            // Send the command
            List<string> resp = new List<string>();
            try
            {
                resp = focuserCommunicator.SendStringReadLines(cmd, 2, 1000);
                ClearTimeoutCount();
            }
            catch
            {
                HandleTimeoutEvent();
                throw;
            }

            try
            {
                // Check for acknowledgment of response
                if (resp[0] != "!")
                    throw new ApplicationException("Did not receive \"!\" in response to sent command \"" + cmd + "\"");
                if (resp[1] != "HALTED")
                    throw new ApplicationException("The device returned the error \"" + resp[1] + "\" from command \"" + cmd + "\".");
                ClearBadResponseCount();
            }
            catch
            {
                HandleBadResponse();
            }

        }

        #endregion

        private static int timeoutCounter = 0;

        private static int badResponseCounter = 0;

        private static void HandleTimeoutEvent()
        {
            //
            // A timeout could occur as a fluke because the firmware was busy
            // This method keeps track of consecutive timeout events. If the event count gets too high 
            // this method will throw an exception.
            //
            const int MaxTimeouts = 5;
            if (timeoutCounter++ >= MaxTimeouts)
            {
                // Set the focuserCommunicator disconnected
                focuserCommunicator.ConnectionOpen = false;
                string msg = "It seems as though communication with the device has been broken. The device has exceeded the maximum " +
                    "number of allowed consecutive timeouts. Please check your cables to ensure a good clean connection. If the problem " +
                    "continues please contact Optec Technical Support.";
                throw new ApplicationException(msg);
            }
            
        }

        private static void HandleBadResponse()
        {
            //
            // A bad response received could occur because of noise in the data lines.
            // This method keeps track of consecutive timeout events. If the event count gets too high 
            // this method will throw an exception.
            //
            const int MaxBadResponses = 5;
            if (badResponseCounter++ >= MaxBadResponses)
            {
                // Set the focuserCommunicator disconnected
                focuserCommunicator.ConnectionOpen = false;
                string msg = "The application is having trouble reading proper data from the device. The device has exceeded the maximum " +
                    "number of allowed consecutive incorrect responses. Please check your cables to ensure a good clean connection. If the problem " +
                    "continues please contact Optec Technical Support.";
                throw new ApplicationException(msg);
            }  
        }

        private static void ClearBadResponseCount()
        {
            badResponseCounter = 0;
        }

        private static void ClearTimeoutCount()
        {
            timeoutCounter = 0;
        }


    }
}
