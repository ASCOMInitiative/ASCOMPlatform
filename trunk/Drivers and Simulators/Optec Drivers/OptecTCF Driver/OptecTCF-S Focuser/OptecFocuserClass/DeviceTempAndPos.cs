using System.ComponentModel;
using System;
using System.Diagnostics;
using Optec;

namespace ASCOM.OptecTCF_S
{

    partial class OptecFocuser
    {
        
        /// <summary>
        /// Used to prevent two serial communications from occuring at the same time.
        /// </summary>
        private static object LockObject = new object();

        /// <summary>
        /// Used to hold the most recent data received from the focuser
        /// </summary>
        private static string ReceivedData;

        /// <summary>
        /// Property used to determine if the device is currently moving or stationary.
        /// </summary>
        private static bool _IsMoving = false;
        public static bool IsMoving
        {
            get { return _IsMoving; }
        }

        /// <summary>
        /// Property that represents the current temperature as measured by the device
        /// </summary>
        private static double _CurrentTemp;
        public static double CurrentTemp
        {
            get { return _CurrentTemp; }
        }

        /// <summary>
        /// Property that holds the current position of the focuser
        /// </summary>
        private static int _CurrentPosition;
        public static int CurrentPosition
        {
            get { return _CurrentPosition; }
        }

        /// <summary>
        /// Used to determine if the device is in the process of receiving non-requested data from the device
        /// </summary>
        private static bool _ReceivingUnsolicited;
        public static bool ReceivingUnsolicited
        {
            get { return _ReceivingUnsolicited; }
            set
            {
                if (value)          //Enter Temp Comp
                {
                    ReceiveUnsolicitedData();
                    _ReceivingUnsolicited = true;

                }
                else                //Exit Temp Comp
                {
                    StopReceivingUnsolicitedData();
                    _ReceivingUnsolicited = false;
                }
            }
        }
       
        /// <summary>
        /// Add the event handler for the serial port that is used to received the unsolicited data on the port.
        /// </summary>
        private static void ReceiveUnsolicitedData()
        {
            mySerialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(mySerialPort_DataReceived);
        }

        /// <summary>
        /// Removes the eventhandler for the DataReceived event
        /// </summary>
        private static void StopReceivingUnsolicitedData()
        {
            mySerialPort.DataReceived -= mySerialPort_DataReceived;
        }

        /// <summary>
        /// Parse the data received on the serial port.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void mySerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            lock (LockObject)
            {
                try
                {
                    if (ConnectionState != ConnectionStates.TempCompMode)
                    {
                        // This should only happen in Temp Comp mode so throw an exception here
                        throw new ASCOM.InvalidOperationException("Unsolicited Data Received while not in Temp Comp Mode");
                    }

                    ReceivedData += mySerialPort.ReadExisting();
                    if (ReceivedData.Contains("\n\r"))
                    {
                        int newline_index = ReceivedData.IndexOf("\n\r");
                        int pos_index = ReceivedData.IndexOf("P=");
                        int temp_index = ReceivedData.IndexOf("T=");
                        int ER4_index = ReceivedData.IndexOf("ER=4");
                        int ER1_index = ReceivedData.IndexOf("ER=1");

#if DEBUG
                        EventLogger.LogMessage("Unsolicited Data Received = " + ReceivedData, TraceLevel.Verbose);
#endif

                        string value = "";

                        // Attempt to parse the position value
                        if (pos_index != -1)
                        {
                            if (newline_index > pos_index)
                            {
                                value = ReceivedData.Substring(pos_index + 2, 4);
                                _CurrentPosition = Convert.ToInt32(value);
                            }
                        }

                        // Attempt to parse the temperature value
                        if (ER1_index != -1)
                        {
                            throw new ASCOM.DriverException("Device Lost Connection To Temp Probe");
                        }
                        else if (ER4_index != -1)
                        {
                            throw new ASCOM.DriverException("Received ER=4 during");
                        }
                        else if (temp_index != -1)
                        {
                            if (newline_index > temp_index)
                            {
                                value = ReceivedData.Substring(temp_index + 2, 5);
                                _CurrentTemp = Convert.ToDouble(value);
                            }
                        }


                    }
                    else if (ReceivedData.Length > 15)
                    {
                        // Too many characters received without a newline. 
                        throw new ASCOM.DriverException("Too many characters received without a newline @ mySerialPort_DataReceived");
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.LogMessage(ex);
                    throw new ASCOM.DriverException("An error occurred while receiving unsolicited data from the serial port.\n" +
                        "Exception data = " + ex.ToString());
                }
                finally
                {
                    // Get ready for the next transmission of data
                    ReceivedData = "";
                }
            }
        }

        public static double GetTemperature()
        {
            try
            {
               // lock (LockObject)
              //  {

                    if (_ConnectionState == ConnectionStates.Sleeping)
                    {
                        throw new InvalidOperationException("Unable To Get Temperature While In Sleep Mode.");
                    }
                    else if (_ConnectionState == ConnectionStates.Disconnected)
                    {
                        throw new InvalidOperationException("Unable To Get Temperature While Disconnected.");
                    }
                    else if (_ConnectionState == ConnectionStates.TempCompMode)
                    {
                        return _CurrentTemp;
                    }
                    else if (_ConnectionState == ConnectionStates.SerialMode)
                    {
                        string resp = SendCommandGetResponse("FTEMPx", "T=", 310, 3);
                        int i = resp.IndexOf("T=") + 2;
                        _CurrentTemp = double.Parse(resp.Substring(i, 5)) + DeviceSettings.TemperatureOffset;
                        return _CurrentTemp;
                    }
                    else throw new ASCOM.DriverException("Unknown Mode Selected.");
              //  }

            }
            catch (ER1_DeviceException) {
                System.Threading.Thread.Sleep(200);
                throw; 
            }
            
            catch (ER4_DeviceException) {
                System.Threading.Thread.Sleep(200);
                throw;  
            }

            catch (Exception ex)
            {
                throw new ASCOM.DriverException("Error Retrieving Temperature.", ex);
            }
        

        }

        public static void SetTempProbeEnabled(bool enabled)
        {
            try
            {
                // Command FYxxx1 disables the probe
                // Command FYxxx0 enabled the probe
                string val = enabled ? "0" : "1";   // Means if enabled == true, val = 0 else val = 1;
                string cmdToSend = "FYxxx" + val;
                string expResp = "DONE";
                lock (LockObject)
                {
                    SendCommandGetResponse(cmdToSend, expResp, 800, 4);
                }
                Debug.Print("Probe enabled = " + enabled.ToString());
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw;
            }

        }

        public static int GetPosition()
        {
            try
            {
               // lock (LockObject)
               // {
                    if (_ConnectionState == ConnectionStates.Sleeping)
                    {
                        throw new InvalidOperationException("Unable To Get Position While In Sleep Mode.");
                    }
                    else if (_ConnectionState == ConnectionStates.Disconnected)
                    {
                        throw new InvalidOperationException("Unable To Get Position While Disconnected.");
                    }
                    else if (_ConnectionState == ConnectionStates.TempCompMode)
                    {
                        return _CurrentPosition;
                    }
                    else if (_ConnectionState == ConnectionStates.SerialMode)
                    {
                        string resp = SendCommandGetResponse("FPOSxx", "P=", 400, 4);
                        int i = resp.IndexOf("P=") + 2;
                        _CurrentPosition = int.Parse(resp.Substring(i, 4));
                        return _CurrentPosition;
                    }
                    else throw new ASCOM.DriverException("Unknown Mode Selected.");
               // }

            }
            catch (Exception ex)
            {
                throw new ASCOM.DriverException("Error Retrieving Position.", ex);
            }
        }

        public static void MoveFocus(int NewPos)
        {
            try
            {
           //     lock (LockObject)
            //    {
                    if (ConnectionState != ConnectionStates.SerialMode)
                    {
                        throw new InvalidOperationException("Must Be In Serial Mode To Move Focus");
                    }
                    else if (NewPos <= 0)
                    {
                        NewPos = 1;
                    }
                    else if (NewPos > DeviceSettings.MaxStep)
                    {
                        NewPos = DeviceSettings.MaxStep;
                    }

                    // Prepare for Move
                    double diff = Math.Abs(_CurrentPosition - NewPos);
                    double t = ((37 * 1000) / 7000) * diff * 4;
                    int TimeOut = Convert.ToInt32(t);
                    if (TimeOut < 2000) TimeOut = 2000;
                    string cmd = "";

                    // Do the Move
                    if (_CurrentPosition == NewPos)
                    {
                        return;
                    }
                    else if (_CurrentPosition > NewPos)
                    {
                        _IsMoving = true;
                        cmd = "FI" + diff.ToString().PadLeft(4, '0');
                        SendCommandGetResponse(cmd, "*", TimeOut, 1);
                        _IsMoving = false;
                    }
                    else if (_CurrentPosition < NewPos)
                    {
                        _IsMoving = true;
                        cmd = "FO" + diff.ToString().PadLeft(4, '0');
                        SendCommandGetResponse(cmd, "*", TimeOut, 1);
                        _IsMoving = false;
                    }

                    // Verify new position and update Current Position...
                    _CurrentPosition = NewPos;
                //    if (_CurrentPosition != NewPos)
                 //       throw new ASCOM.DriverException("Move Did Not Succeed. Desired Pos = " + NewPos.ToString() +
                  //          "Final Position = " + _CurrentPosition.ToString());

              //  }
            }
            catch (Exception ex)
            {
                throw new ASCOM.DriverException("Error Moving Focus", ex);
            }
            finally
            {
                _IsMoving = false;
            }
        }

        public static void SetFocuserDelay(int delay, char AorB)
        {
            try
            {
                if (delay < 1) throw new InvalidValueException("SetFocuserDelay", "Delay", "001 to 999");
                if (delay > 999) throw new InvalidValueException("SetFocuserDelay", "Delay", "001 to 999");
                if (AorB != 'A' && AorB != 'B') throw new InvalidValueException("SetFocuserDelay", "AorB", "A or B");
                string cmd = delay.ToString().PadLeft(3, '0');
                if (AorB == 'A')
                {
                    cmd = "FDA";
                }
                else if (AorB == 'B')
                {
                    cmd = "FDB";
                }
                lock (LockObject)
                {
                    cmd = cmd + delay.ToString().PadLeft(3, '0');    //cmd = FDAnnn or FDBnnn
                    SendCommandGetResponse(cmd, "DONE", 500, 4);
                }
            }
            catch (Exception Ex)
            {
                throw new DriverException("\nError executing LoadSlope method.\n" + Ex.ToString(), Ex);
            }
        }

        internal static void SetDeviceType(DeviceSettings.DeviceTypes value)
        {
            try
            {
                string CmdToSend = "FE000";
                switch (value)
                {
                    case DeviceSettings.DeviceTypes.TCF_S:
                        CmdToSend += "1";
                        break;
                    case DeviceSettings.DeviceTypes.TCF_Si:
                        CmdToSend += "1";
                        break;
                    case DeviceSettings.DeviceTypes.TCF_S3:
                        CmdToSend += "3";
                        break;
                    case DeviceSettings.DeviceTypes.TCF_S3i:
                        CmdToSend += "3";
                        break;
                    default:
                        throw new ASCOM.InvalidValueException("Device Type to Set", 
                            value.ToString(), "TCF-S, TCF-Si, TCF-S3, TCF-S3i");
                }
                lock (LockObject)
                {
                    SendCommandGetResponse(CmdToSend, "DONE", 500, 4);
                }
            }
            catch (Exception Ex)
            {
                throw new DriverException("\nError Setting Device Type./n" + Ex.ToString(), Ex);
            }
        }

        internal static int GetLearnedSlope(char AorB)
        {
            int slope = 0;
            try
            {
                lock (LockObject)
                {

                    if (AorB == 'A')
                    {
                        string resp = SendCommandGetResponse("FREADA", "A=", 500, 4);
                        int i = resp.IndexOf("=") + 2;
                        slope = int.Parse(resp.Substring(i, 3));
                    }
                    else if (AorB == 'B')
                    {
                        string resp = SendCommandGetResponse("FREADB", "B=", 500, 4);
                        int i = resp.IndexOf("=") + 2;
                        slope = int.Parse(resp.Substring(i, 3));
                    }
                    else
                    {
                        throw new InvalidValueException("GetLearnedSlope", AorB.ToString(), "Must be A or B");
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new DriverException("\nError executing GetLearnedSlope method.\n" + Ex.ToString(), Ex);
            }

            //Now get the sign of the slope

            try
            {
                if (AorB != 'A' && AorB != 'B') throw new InvalidValueException("SetFocuserDelay", "AorB", "A or B");

                string resp = "";
                if (AorB == 'A')
                {
                    resp = SendCommandGetResponse("FtxxxA", "A=", 500, 4);
                }
                else if (AorB == 'B')
                {
                    resp = SendCommandGetResponse("FtxxxB", "B=", 500, 4);
                }
                int i = resp.IndexOf("=") + 1;
                char signChar = resp[i];
                System.Threading.Thread.Sleep(50);
                if (signChar == '0') return slope;
                else if (signChar == '1') return -slope;
                else throw new ASCOM.InvalidValueException("GetSlope", signChar.ToString(), "1 or 0");
            }
            catch (Exception Ex)
            {
                throw new DriverException("\nError executing GetSlopeSign method.\n" + Ex.ToString(), Ex);
            }
        }

        internal static void SetSlope(int SignedSlope, char AorB)
        {
            try
            {
                lock (LockObject)
                {
                    string cmd = "";
                    string slp = "";
                    int UnsignedSlope = Math.Abs(SignedSlope);

                    if (UnsignedSlope > 999) throw new InvalidValueException("Load Slope", "Slope", "000 to 999");
                    if (UnsignedSlope < 0) throw new InvalidValueException("Load Slope", "Slope", "000 to 999");
                    slp = UnsignedSlope.ToString().PadLeft(3, '0');
                    if (AorB == 'A')
                    {
                        cmd = "FLA" + slp;
                    }
                    else if (AorB == 'B')
                    {
                        cmd = "FLB" + slp;
                    }
                    else
                    {
                        throw new InvalidValueException("LoadSlope", AorB.ToString(), "Must be A or B");
                    }
                    SendCommandGetResponse(cmd, "DONE", 500, 4);
                }

            }
            catch (Exception Ex)
            {
                throw new DriverException("\nError executing LoadSlope method.\n" + Ex.ToString(), Ex);
            }


            // Now set the slope sign...

            try
            {
                char sign;
                if (SignedSlope < 0) sign = '-';
                else sign = '+';
                if (sign != '+' && sign != '-') throw new InvalidValueException("SetSlopeSign", "slope", "+ or -");
                if (AorB != 'A' && AorB != 'B') throw new InvalidValueException("SetSlopeSign", "slope", "+ or -");
                string cmd = "";
                if (AorB == 'A')
                {
                    cmd = "FZAxx";
                }
                else if (AorB == 'B')
                {
                    cmd = "FZBxx";
                }
                if (sign == '+')
                {
                    cmd = cmd + "0";
                }
                else if (sign == '-')
                {
                    cmd = cmd + "1";
                }
                SendCommandGetResponse(cmd, "DONE", 500, 4);
                System.Threading.Thread.Sleep(110);
            }
            catch (Exception Ex)
            {
                throw new DriverException("\nError executing SetSlopeSign method./n" + Ex.ToString(), Ex);
            }
        }

        internal static void SetDelay(char AorB, double delay)
        {
            try
            {
                
                if (AorB != 'A' && AorB != 'B') throw new InvalidValueException("SetDelay", AorB.ToString(), "A or B");
                if (delay < 1 || delay > 10.99) throw new InvalidValueException("SetDelay", delay.ToString(), "1 through 10.99");
                lock (LockObject)
                {
                    delay = delay - 1;
                    string cmd;
                    string delaystring = (delay * 100).ToString().PadLeft(3, '0');

                    if (AorB == 'A')
                    {
                        cmd = "FDA" + delaystring;
                    }
                    else
                    {
                        cmd = "FDB" + delaystring;
                    }
                    SendCommandGetResponse(cmd, "DONE", 600, 4);
                }
            }
            catch (Exception Ex)
            {
                throw new DriverException("Error setting delay", Ex);
            }
        }
    }
}