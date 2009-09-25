using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ASCOM.OptecTCF_Driver
{
    class DeviceComm
    {
        private static ASCOM.Utilities.Serial ASerialPort;

        //The Following are used when calling the sendCmd method...
        private const bool ExpectResponse = true;
        private const bool ExpectNoResponse = false;

        enum DeviceModes { Unknown, MainLoop, SerialLoop, AutoModeX, Sleeping }
        private static DeviceModes CurrentMode;

#region DeviceComm Constructor
        static DeviceComm()
        {
            ASerialPort = new ASCOM.Utilities.Serial();
            CurrentMode = DeviceModes.Unknown;
        }
#endregion

#region DeviceComm Properties



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
                throw new ASCOM.DriverException("Connection attempt failed", Ex);
            }
        }

        public static bool GetConnectionState()
        {
            if (CurrentMode == DeviceModes.Unknown) return false;
            else return true;
        }

        internal static void Disconnect()
        {
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
            ASerialPort.Connected = false;
            CurrentMode = DeviceModes.Unknown;
        }

        #endregion

#region DeviceComm Focuser Operations METHODS

        internal static bool CheckIfConnected()
        {
            throw new System.NotImplementedException();
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
                throw new MethodNotImplementedException("MoveFocus");
            }
            catch (Exception Ex)
            {
                throw new ASCOM.DriverException("Failed to move focus.\n" + Ex.ToString(), Ex);
            }
        }
        public static int GetPosition()
        {
            try
            {
                string received = SendCmd( "FPxxxx", 500, ExpectResponse, "P=" );
                int i = received.IndexOf("=") + 1;
                int pos = int.Parse(received.Substring(i,4));
                return pos;
            }
            catch (Exception Ex)
            {
                throw new DriverException("Error retrieving position./n" + Ex.ToString(), Ex);
            }
        }

        public static void MoveToCenter()
        {
            try
            {
                string received = SendCmd("FCxxxx", 500, ExpectResponse, "CENTER");
                
            }
            catch { }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cmd"></param>
        /// <param name="timeout"></param>
        /// <param name="ResponseDesired"></param>
        /// <param name="ResponseContains"></param>
        /// <returns></returns>
        private static string SendCmd(string Cmd, int timeout, bool ResponseDesired, string ResponseContains)
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
                throw new InvalidPrerequisits("Device is not in the correct state to send the command: " + Cmd + 
                    "\nCurrent state is : " + CurrentMode.ToString());
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

                    //char mode = DeviceSettings.GetModeAorB();
                    char mode = 'A';
                    switch (mode)
                    {
                        case 'A':
                            SendCmd("FAxxxx", 500, ExpectResponse, "A");
                            break;
                        case 'B':
                            SendCmd("FBxxxx", 500, ExpectResponse, "B");
                            break;
                        default:
                            throw new ASCOM.InvalidValueException("Temp Comp Mode", mode.ToString(), "A or B");
                    }
                    CurrentMode = DeviceModes.AutoModeX;
                    System.Threading.Thread.Sleep(300);
                    
                }

                else
                {
                    SendCmd("FMxxxx", 500, ExpectResponse, "!");
                    CurrentMode = DeviceModes.SerialLoop;
                }
            }
            catch (Exception Ex)
            {
                string ErrorString = "Failed to change Focus Mode ";
                if (Enter) ErrorString += "while attempting to ENTER Temp Comp Mode.\n";
                else ErrorString += "while attempting to EXIT Temp Comp Mode.\n";
                throw new ASCOM.DriverException(ErrorString + Ex.ToString() + "\n" , Ex);
            }
        }

        

#endregion




        internal static double GetTemperaterature()
        {
            throw new System.NotImplementedException();
        }
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
