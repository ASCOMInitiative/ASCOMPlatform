using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ASCOM.OptecTCF_Driver
{
    class DeviceComm
    {
        private static ASCOM.Utilities.Serial ASerialPort =  new ASCOM.Utilities.Serial();
        private static System.Threading.Thread AutoModeThread;
        private const bool ExpectResponse = true;
        private const bool ExpectNoResponse = false;
        private static int CurrentPosition = 0;
        private static double CurrentTemp = 0;
        enum DeviceModes { Unknown, SerialLoop, AutoModeX, Sleeping }        
        private static DeviceModes CurrentMode = DeviceModes.Unknown;

        internal static double Temperature
        {
            get 
            {
                if (CurrentMode == DeviceModes.AutoModeX)
                {
                    return CurrentTemp;
                }
                else
                {
                    return GetTemperaterature();
                }
 
            }
        }
        internal static int Position
        {
            get
            {
                if (CurrentMode == DeviceModes.AutoModeX)
                {               
                    return CurrentPosition;   
                }
                else
                {
                    return GetPosition();
                }
            }
        }

#region DeviceComm Learn Related Methods

        internal static int GetLearnedSlope(char AorB)
        {
            try 
	        {	        
		        if (AorB == 'A')
                {
                    string resp = SendCmd("FREADA",500,ExpectResponse, "A=");
                    int i = resp.IndexOf("=") + 2;
                    int slope = int.Parse(resp.Substring(i,3));
                    return slope;

                }
                else if (AorB == 'B')
                {
                    string resp = SendCmd("FREADB",500,ExpectResponse, "B=");
                    int i = resp.IndexOf("=") + 2;
                    int slope = int.Parse(resp.Substring(i,3));
                    return slope;
                }
                else 
                {
                    throw new InvalidValueException("GetLearnedSlope", AorB.ToString(), "Must be A or B");
                }
	        }
	        catch (Exception Ex)
	        {        		
		        throw new DriverException("\nError executing GetLearnedSlope method.\n" + Ex.ToString(), Ex);
	        }
        }

        internal static void SetSlope(int slope, char AorB)
        {
            try
            {
                string cmd = "";
                string slp = "";
                if (slope > 999) throw new InvalidValueException("Load Slope", "Slope", "000 to 999");
                if (slope < 0) throw new InvalidValueException("Load Slope", "Slope", "000 to 999");
                slp = slope.ToString().PadLeft(3, '0');
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
                
                SendCmd(cmd, 500, ExpectResponse, "DONE");

            }
            catch (Exception Ex)
            {
                throw new DriverException("\nError executing LoadSlope method.\n" + Ex.ToString(), Ex);
            }
        }

        internal static void SetFocuserDelay(int delay, char AorB)
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
                cmd = cmd + delay.ToString().PadLeft(3, '0');    //cmd = FDAnnn or FDBnnn
                SendCmd(cmd, 500, ExpectResponse, "DONE");
            }
            catch (Exception Ex)
            {
                throw new DriverException("\nError executing LoadSlope method.\n" + Ex.ToString(), Ex);
            }
        }

        internal static char GetSlopeSign(char AorB)
        {
            try
            {
                if (AorB != 'A' && AorB != 'B') throw new InvalidValueException("SetFocuserDelay", "AorB", "A or B");
                
                string resp = "";
                if (AorB == 'A')
                {
                    resp = SendCmd("FtxxxA", 500, ExpectResponse, "A=");
                }
                else if (AorB == 'B')
                {
                    resp = SendCmd("FtxxxB", 500, ExpectResponse, "B=");
                }
                int i = resp.IndexOf("=") + 1;
                char signChar = resp[i];

                if (signChar == '0') return '+';
                else if (signChar == '1') return '-';
                else throw new InvalidResponse("\n Expected slope sign 1 or 0.\n Received: " + signChar.ToString() + "\n");
            }
            catch (Exception Ex)
            {
                throw new DriverException("\nError executing GetSlopeSign method.\n" + Ex.ToString(), Ex);
            }

        }

        internal static void SetSlopeSign(char sign, char AorB)
        {
            try
            {
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
                SendCmd(cmd, 500, ExpectResponse, "DONE");
            }
            catch (Exception Ex)
            {    
                throw new DriverException("\nError executing SetSlopeSign method./n" + Ex.ToString(), Ex);
            }
        }

#endregion

#region DeviceComm Connection METHODS

        internal static void Connect()
        {
            //This should connect and verify that connection is established.
            //If connection is not established it should throw an exception and NOT catch it.

            try
            {
                if (!DeviceSettings.PortSelected)
                {
                    throw new PortNotSelectedException();
                }
                else
                {
                    //get the COM port that has been used in the past
                    ASerialPort.PortName = DeviceSettings.PortName;
                    ASerialPort.Speed = ASCOM.Utilities.SerialSpeed.ps19200;
                    ASerialPort.Connected = true;

                    string received = SendCmd("FMxxxx", 500, ExpectResponse, "!");
                    CurrentMode = DeviceModes.SerialLoop;
                }
            }

            catch (Exception Ex)  //thrown if the receive times out
            {
                CurrentMode = DeviceModes.Unknown;
                throw new ASCOM.DriverException("\nConnection attempt failed", Ex);
            }
        }

        public static bool GetConnectionState()
        {
            if (CurrentMode == DeviceModes.Unknown) return false;
            else return true;
        }

        internal static void Disconnect()
        {
            try
            {
                System.Threading.Thread.Sleep(500);
                if (CurrentMode == DeviceModes.SerialLoop)
                {
                    SendCmd("FFxxxx", 1000, ExpectResponse, "END");
                }
                else if (CurrentMode == DeviceModes.AutoModeX)
                {
                    SendCmd("FMxxxx", 1000, ExpectResponse, "END");
                    SendCmd("FFxxxx", 1000, ExpectResponse, "!");
                }
                else if (CurrentMode == DeviceModes.Sleeping)
                {
                    SendCmd("FWxxxx", 1000, ExpectResponse, "WAKE");
                    SendCmd("FFxxxx", 1000, ExpectResponse, "END");
                }
            }
            catch
            {
                //if this fails that means we arn't communicating with the device so it is already disconnected
                //Do nothing here...
            }
            ASerialPort.Connected = false;
            CurrentMode = DeviceModes.Unknown;
        }

        #endregion

#region DeviceComm Focuser Operations METHODS

        internal static string[] GetFVandDT()   //Firmware Version & Device Type
        {
            try
            {
                string resp = "";
                resp = SendCmd("FVxxxx", 500, ExpectResponse, ".");
                int i = resp.IndexOf(".");
                string version = resp.Substring(i - 1, 4);
                string type = "";
                if (resp[i + 4] == '1')
                {
                    type = "TCF-S";
                }
                else if (resp[i + 4] == '3')
                {
                    type = "TCF-S3";
                }
                else
                {
                    throw new InvalidResponse("\n Failed to get device type. Device type was not a 1 or a 3.\n" + 
                    "Response from device was: " + resp + "\n");
                }
                string[] ReturnVals = {version, type};
                return ReturnVals;

            }
            catch (LostCommWithDevice Ex)
            {
                throw new DriverException("No response from device", Ex);
                //string[] ReturnVals = {"Unable To Determine Version", "Unable To Determine Type"};
                //return ReturnVals;
            }
            catch (Exception Ex)
            {   
                throw new DriverException("Error retrieving firmware version and device type", Ex);
            }

        }

        internal static bool InTempCompMode()
        {
            if (CurrentMode == DeviceModes.AutoModeX) return true;
            else return false;
        }

        internal static void MoveFocus(int pos)
        {
            try
            {
                int CurrentPos = GetPosition();
                int diff = Math.Abs(CurrentPos - pos);
                string cmd = "";
                int TimeOut = 35 * 1000;    //35 Second timeout

                if (CurrentPos == pos)
                {
                    return;
                }
                else if (CurrentPos > pos)
                {
                    cmd = "FI" + diff.ToString().PadLeft(4, '0');
                    SendCmd(cmd, TimeOut, ExpectResponse, "*");
                }
                else if (CurrentPos < pos)
                {
                    cmd = "FO" + diff.ToString().PadLeft(4, '0');
                    SendCmd(cmd, TimeOut, ExpectResponse, "*" ); 
                }
            }
            catch (Exception Ex)
            {
                //throw new ASCOM.DriverException("\nFailed to move focus.\n" + Ex.ToString(), Ex); 
                throw new ASCOM.DriverException("\nFailed to move focuser.\n" + Ex.ToString(), unchecked ((int)0x80040403));  
            }
        }

        private static int GetPosition()
        {
            try
            {
                string received = SendCmd("FPxxxx", 500, ExpectResponse, "P=");
                int i = received.IndexOf("=") + 1;
                int pos = int.Parse(received.Substring(i, 4));
                return pos;
            }
            catch (Exception Ex)
            {
                throw new DriverException("\nError retrieving position./n" + Ex.ToString(), Ex);
            }
        }

        private static double GetTemperaterature()
        {
            try
            {
                string resp = SendCmd("FTMPRO", 2000, ExpectResponse, "T=");
                int i = resp.IndexOf("=") + 1;
                string t = resp.Substring(i, 5);
                double temp = double.Parse(t);
                return temp;
            }
            catch (Exception Ex)
            {
                throw new DriverException("\n Unexpcted Error in GetTemperature method.\n" +
                    Ex.ToString(), Ex);
            }
            
        }

        internal static void MoveToCenter()
        {
            try
            {
                string received = SendCmd("FCxxxx", 500, ExpectResponse, "CENTER");
                
            }
            catch { }

        }

        private static string SendCmd(string Cmd, int timeout, bool ResponseDesired, 
            string ResponseContains)
        {
            bool BadResponse = false;
            if (CurrentMode == DeviceModes.Sleeping && Cmd.Contains("FW"))
            {
                  //okay to send the command
            }
            else if (CurrentMode == DeviceModes.AutoModeX && Cmd.Contains("FM"))
            {
                  //okay to send the command
            }
            else if (CurrentMode == DeviceModes.Unknown && Cmd.Contains("FM"))
            {
                  //okay to send the command
            }
            else if (CurrentMode != DeviceModes.SerialLoop)
            {
                        //NOT ok to send command
                throw new InvalidPrerequisits("\nDevice is not in the correct state to send the command: " + Cmd + 
                    "\nCurrent state is : " + CurrentMode.ToString() + "\n" );
            }

            //SEND THE COMMAND//
            string received = "";
            try
            {
                ASerialPort.ReceiveTimeoutMs = timeout;
                if (Cmd.Contains("FM"))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        ASerialPort.ClearBuffers();
                        ASerialPort.Transmit(Cmd);
                        System.Threading.Thread.Sleep(timeout / 3);
                    }
                }
                else
                {
                    ASerialPort.ClearBuffers();
                    ASerialPort.Transmit(Cmd);
                }
                
                if (!ResponseDesired) return "";
                ReadAgain:            
                
                received = received + ASerialPort.ReceiveTerminated("\n\r");
                
                if (received.Contains(ResponseContains))
                {
                    return received;
                }
                else
                {
                    BadResponse = true;
                    goto ReadAgain;
                }
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                if (BadResponse == true)
                {
                    throw new InvalidResponse("Method was looking for " + ResponseContains +
                            "Response received was: " + received + "\n");
                }
                else
                {
                    throw new LostCommWithDevice("No response back from the following command: " + Cmd + "\n");
                }
            }
            catch (Exception Ex)
            {
                throw new InvalidPrerequisits("Error sending command: " + Cmd + "\n", Ex);
            }
        }

        internal static void EnterTempCompMode(bool Enter)
        {
            try
            {
                if (Enter)
                {
                    char mode = DeviceSettings.GetActiveMode();                
                    switch (mode)
                    {
                        case 'A':
                            SendCmd("FAxxxx", 500, ExpectResponse, "A");
                            break;
                        case 'B':
                            SendCmd("FBxxxx", 500, ExpectResponse, "B");
                            break;
                        default:
                            throw new ASCOM.InvalidValueException("\nTemp Comp Mode", mode.ToString(), "A or B");
                    }
                    CurrentMode = DeviceModes.AutoModeX;
                    CurrentTemp = 0;
                    CurrentPosition = 0;
                    ThreadStart ts = new ThreadStart(ReceiveUnsolicited);
                    AutoModeThread = new Thread(ts);
                    AutoModeThread.Priority = ThreadPriority.AboveNormal;
                    AutoModeThread.Start();

                    //Wait for five seconds for a temp and position then throw exception
                    TimeSpan FiveSec = new TimeSpan(0, 0, 5);
                    DateTime Start = DateTime.Now;
                    while (CurrentPosition == 0 || CurrentTemp == 0)
                    {
                        if (DateTime.Now - Start > FiveSec)
                        {
                            CurrentMode = DeviceModes.Unknown;
                            //throw new DriverException("Timed out while waiting for data");
                        }
                    } 
                }
                else
                {
                    CurrentMode = DeviceModes.SerialLoop;
                    if (AutoModeThread != null)
                    {
                        if (AutoModeThread.ThreadState == ThreadState.Running)
                        {
                            AutoModeThread.Abort();
                            System.Threading.Thread.Sleep(300);
                        }
                    }
                    SendCmd("FMxxxx", 500, ExpectResponse, "!");
                    
                }
            }
            catch (Exception Ex)
            {
                string ErrorString = "\nFailed to change Auto Focus Mode ";
                if (Enter) ErrorString += "while attempting to ENTER Temp Comp Mode.\n";
                else ErrorString += "while attempting to EXIT Temp Comp Mode.\n";
                ErrorString += Ex.ToString();
                throw new ASCOM.DriverException(ErrorString + Ex.ToString() + "\n" , Ex);
            }
        }

        

        //internal static int GetMaxStep()
        //{
        //    //try
        //    //{
        //    //    string DevType = "";
        //    //    DevType = DeviceSettings.GetDeviceType();
        //    //    if (DevType == "?")
        //    //    {
        //    //        string[] FV = GetFVandDT();
        //    //        DevType = FV[1];
        //    //    }

        //    //    if (DevType == "TCF-S3" || DevType == "TCF-S3i")
        //    //        return 10000;
        //    //    else if (DevType == "TCF-S" || DevType == "TCF-Si")
        //    //    {
        //    //        return 7000;
        //    //    }
        //    //    else
        //    //    {
        //    //        throw new InvalidResponse("\nGetMaxStep command did not receive a valid response.\n" +
        //    //             "Response was: " + DevType + ".\n");
        //    //    }
        //    //}
        //    //catch (Exception Ex)
        //    //{
        //    //    throw new DriverException("/nAn Unexpected error occured in GetMaxStep.\n" + Ex.ToString());
        //    //}
        //}

        internal static double GetStepSize()
        {
            try
            {
                string[] FV = GetFVandDT();
                string DevType = FV[1];
                if (DevType == "TCF-S3" || DevType == "TCF-S3i")
                    return 2.032;
                else if (DevType == "TCF-S" || DevType == "TCF-Si")
                {
                    return 2.54;
                }
                else
                {
                    throw new InvalidResponse("\nGetStepSize command did not receive a valid response.\n" +
                         "Response was: " + DevType + ".\n");
                }
            }
            catch (Exception Ex)
            {
                throw new DriverException("/nAn Unexpected error occured in GetStepSize.\n" + Ex.ToString());
            }
        }

        internal static void QuietModeOn(bool state)
        {
            try
            {
                if (state == true)  //enable output of data every se
                {
                    SendCmd("FQUIT0", 500, ExpectResponse, "DONE");
                }
                else                 //disable data output
                {
                    SendCmd("FQUIT1", 500, ExpectResponse, "DONE");
                }
            }
            catch (Exception Ex)
            {       
                throw new DriverException("\n An error occured while setting QuietMode.\n" 
                    + Ex.ToString(), Ex);
            }
        }

        internal static void SetDelay(char AorB, double delay)
        {
            try
            {
                if (AorB != 'A' && AorB != 'B') throw new InvalidValueException("SetDelay", AorB.ToString(), "A or B");
                if (delay < 1 || delay > 10.99) throw new InvalidValueException("SetDelay", delay.ToString(), "1 through 10.99");

                delay = delay - 1;
                string cmd;
                string delaystring = (delay*100).ToString().PadLeft(3,'0');
                
                if (AorB == 'A')
                {
                    cmd = "FDA" + delaystring;
                }
                else 
                {
                    cmd = "FDB" + delaystring;
                }

                SendCmd(cmd, 600, ExpectResponse, "DONE");
            }
            catch (Exception Ex)
            {
                throw new DriverException("Error setting delay", Ex);
            }
        }

        private static void ReceiveUnsolicited()
        {
            int pos = 0;
            string value, rec;
            ASerialPort.ReceiveTimeout = 5;
            while (CurrentMode == DeviceModes.AutoModeX)
            {
                try
                {
                    rec = ASerialPort.ReceiveTerminated("\n");
                    if (rec.Contains("P="))
                    {
                        pos = rec.IndexOf("=") + 1;
                        value = rec.Substring(pos, rec.Length - 3);
                        CurrentPosition = Convert.ToInt32(value);
                    }
                    else if (rec.Contains("T="))
                    {
                        pos = rec.IndexOf("=") + 1;
                        value = rec.Substring(pos, rec.Length - 3);
                        CurrentTemp = Convert.ToDouble(value);
                    }
                    else continue;
                }
                catch(Exception Ex)
                {
                    if (CurrentMode == DeviceModes.AutoModeX)
                    {
                        MessageBox.Show("Error in ReceiveUnsolicited" + "\n\r" + Ex.ToString());
                    }
                } 
            }
        }
#endregion

    }   //end of the DeviceComm class


#region Exception Related Classes

    public class LostCommWithDevice : ASCOM.DriverException
    {

        /// <summary>

        /// Default public constructor for LostCommWithDevice takes no parameters.
        /// This exception should be thrown if an error occurs that is device related 
        /// (Not for driver or user errors)

        /// </summary>

        public LostCommWithDevice(string message) 
            : base(message, ErrorCodes.LostComm){}
        public LostCommWithDevice(string message, System.Exception inner) 
            : base(message, ErrorCodes.LostComm, inner) { }
    }

    public class InvalidPrerequisits : ASCOM.DriverException
    {
       /// <summary>
       /// Constructor for exception used in the SendCmd class
       /// Throw this exception when the device is in the wrong mode
       /// </summary>
       /// <param name="message"></param>
   
        public InvalidPrerequisits(string message)
            :base(message, ErrorCodes.InvalidPrereqs) 
        { }
        public InvalidPrerequisits(string message, System.Exception inner) 
            :base(message, ErrorCodes.InvalidPrereqs, inner)
        { }
    }

    public class InvalidResponse : ASCOM.DriverException
    {
        /// <summary>
        /// Constructor for exception used in the SendCmd class
        /// Throw this exception when the wrong response is received
        /// </summary>
        /// <param name="message"></param>

        public InvalidResponse(string message)
            : base(message, ErrorCodes.InvalidResponse)
        { }
        public InvalidResponse(string message, System.Exception inner)
            : base(message, ErrorCodes.InvalidResponse, inner)
        { }
    }

    public class PortNotSelectedException : ASCOM.DriverException
    {
        /// <summary>
        /// Constructor for exception used in the SendCmd class
        /// Throw this exception when the wrong response is received
        /// </summary>
        /// <param name="message"></param>
        public PortNotSelectedException()
            : base("Tried to connect without selecting a serial port")
        { }
        public PortNotSelectedException(string message)
            : base(message, ErrorCodes.PortNotSelectedException)
        { }
        public PortNotSelectedException(string message, System.Exception inner)
            : base(message, ErrorCodes.PortNotSelectedException, inner)
        { }
    }

    public class ErrorCodes
    {
        public const int LostComm = unchecked((int)0x80040407); 
        public const int InvalidPrereqs = unchecked((int)0x80040408);
        public const int PortNotSelectedException = unchecked((int)0x80040409);
        public const int InvalidResponse = unchecked((int)0x80040408);
    }

#endregion
}   //end of OptecTCF_Driver Namespace
