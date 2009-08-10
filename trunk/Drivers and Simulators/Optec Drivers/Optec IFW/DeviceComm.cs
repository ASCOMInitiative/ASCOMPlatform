using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;

namespace ASCOM.Optec_IFW
{
    class DeviceComm
    {
        
        public static bool IsAtHome;
        public static int NumOfFilters;
        public static char WheelID;

        //private static SerialPort Port;
        private static ASCOM.Helper.Serial SerialTools;
        private static ASCOM.Helper.Profile ProfileTools;

        
     
        #region   //Filter Configuration Variables        
        
        public string[] FilterNames = new string[9];
        private static float[] FilterOffsets = new float[9];

        private static string FilterName1;
        private static float FilterOffset1;

        private static string FilterName2;
        private static float FilterOffset2;

        private static string FilterName3;
        private static float FilterOffset3;

        private static string FilterName4;
        private static float FilterOffset4;

        private static string FilterName5;
        private static float FilterOffset5;

        private static string FilterName6;
        private static float FilterOffset6;

        private static string FilterName7;
        private static float FilterOffset7;

        private static string FilterName8;
        private static float FilterOffset8;

        private static string FilterName9;
        private static float FilterOffset9;
        #endregion

        static DeviceComm()                //Default Constructor
        {
            IsAtHome = false;
            NumOfFilters = 0;
            
            SerialTools = new ASCOM.Helper.Serial();
            SerialTools.Speed = ASCOM.Helper.PortSpeed.ps19200;
            ProfileTools = new ASCOM.Helper.Profile();
            ProfileTools.DeviceTypeV = "FilterWheel";

        }

        public static void ConnectToDevice()
        {
            SerialTools.Port = short.Parse(GetCOMPort());           //Get the Com Port number from the registry 
            //MessageBox.Show("Connecting");
            SerialTools.Connected = true;
            sendCmd("WSMODE", 500);
            if (!SerialTools.Receive().Contains("!")) throw new Exception("Did not connect");

        }
        public static bool CheckForConnection()
        {
            return SerialTools.Connected;
        }
        public static void DisconnectDevice()
        {
            if (SerialTools.Connected) SerialTools.Connected = false;   //dissconnect the port
        }

        private static void sendCmd(string CmdToSend, int ResponseTime)
        {
            SerialTools.ReceiveTimeoutMs = ResponseTime;
            SerialTools.ClearBuffers();
            SerialTools.Transmit(CmdToSend);
        }
        public static bool HomeDevice()
        {
            sendCmd("WHOMEZ", 20000);
            string inputbuff;
            try { inputbuff = SerialTools.Receive(); }
            catch (TimeoutException)
            {
                throw new Exception("Homing Failed. Response Timeout.");
            }
            if (inputbuff.Contains("ER="))
            {
                throw new Exception("Home procedure failed. Error = " + inputbuff);
            }

            else
            {
                WheelID = inputbuff[0];
                return true;
            }

        }
        public static char GetWheelIdentity()
        {
            sendCmd("WIDENT", 1000);
            string inputbuff;
            try { inputbuff = SerialTools.Receive(); }
            catch (TimeoutException)
            {
                throw new Exception("Failed to get wheel identity. Response Timeout Occured");
            }
            if ((inputbuff.Length < 1)) throw new Exception("Failed to get wheel identity. Incorrect Response: " + inputbuff);
            return inputbuff[0];
        }
        public static string GetCurrentPos()
        {
            sendCmd("WFILTR", 1000);
            string inputbuff;
            try { inputbuff = SerialTools.Receive(); }
            catch (TimeoutException)
            {
                throw new Exception("Failed to get current position. Response Timeout Occured");
            }
            if (!inputbuff.Contains("x")) throw new Exception("Failed to get current position. Incorrect Response: " + inputbuff);
            return inputbuff;
        }
        public static void GoToPosition(int Pos)
        {
            if ((Pos > 8) || (Pos < 0)) throw new Exception("Position value is out of reach. Can not go to position " + Pos);
            sendCmd("WGOTO" + Pos, 180000);
            string inputbuff;
            try { inputbuff = SerialTools.Receive(); }
            catch (TimeoutException)
            {
                throw new Exception("Failed to get current position. Response Timeout Occured");
            }
            if (!inputbuff.Contains("*")) throw new Exception("Failed to get current position. Incorrect Response: " + inputbuff);
            //return inputbuff;
            if (inputbuff.Contains("ER=4")) throw new Exception("Failed to move to new position. Input Buffer: " + inputbuff);

        }
        public static string[] ReadAllNames(int Pos)      //this returns a string of 40 characters that are the stored names for the filters
        {
            sendCmd("WREADZ", 1000);
            string inputbuff;
            string[] NamesToOutput = new string[Pos];
            try { inputbuff = SerialTools.ReceiveCounted(40); }
            catch (TimeoutException)
            {
                throw new Exception("Failed to get current position. Response Timeout Occured");
            }
            //TODO: add the correct length for the string containing all of the names
            if (inputbuff.Length < 10) throw new Exception("Failed to get current position. Incorrect Response: " + inputbuff);
            
            if (inputbuff.Contains("ER=4")) throw new Exception("Failed to Read Names from EEPROM. Input Buffer: " + inputbuff);
            MessageBox.Show(inputbuff);
            for (int i = 0; i < Pos; i++)
            {
                NamesToOutput[i] = inputbuff.Substring(i * 8, 8);
            }
            
            
            return NamesToOutput;

        }

        public static void LoadNames(string Names)            //
        {

        }

        public static void ExitProgramLoop()        //Exit the serial loop and return to normal manual operation
        {
            sendCmd("WEXITS", 1000);
            string inputbuff;
            try { inputbuff = SerialTools.Receive(); }
            catch (TimeoutException)
            {
                throw new Exception("Failed to execute EXIT command. Response Timeout Occured");
            }
            if (!inputbuff.Contains("END")) throw new Exception("Failed execute EXIT command. Incorrect Response: " + inputbuff);
        }

        public static void SavePortNumber(string pn)
        {
            //this method saves the port number that has been selected to the registry
            ProfileTools.WriteValue(FilterWheel.s_csDriverID, "PortNumber", pn, "");
        }

        public static void StoreFilterOffsets(float[] filteroffsets)
        {
            //throw new System.NotImplementedException();
        }

        public static string GetCOMPort()
        {
                string cp = ProfileTools.GetValue(FilterWheel.s_csDriverID, "PortNumber", "");
                if (cp != "")
                {
                    return cp;
                }
                else throw new Exception("You must open up the driver settings and select a COM port first");    
        }
        public static string TryGetCOMPort()
        {
            string cp = ProfileTools.GetValue(FilterWheel.s_csDriverID, "PortNumber", "");
            if (cp != "")
            {
                return cp;
            }
            else return "000";
        }

        public static int GetNumOfPos(bool IFW_RB_State)
        {
            int Pos;
            if (IFW_RB_State)
            {
                #region Wheel ID and Focuser Model Switch Statement
                switch (WheelID)
                {
                    case 'A':
                        Pos = 5;
                        break;
                    case 'B':
                        Pos = 5;
                        break;
                    case 'C':
                        Pos = 5;
                        break;
                    case 'D':
                        Pos = 5;
                        break;
                    case 'E':
                        Pos = 5;
                        break;
                    case 'F':
                        Pos = 8;
                        break;
                    case 'G':
                        Pos = 8;
                        break;
                    case 'H':
                        Pos = 8;
                        break;
                    default:
                        throw new Exception("Unacceptable Wheel ID returned");
                }
            }
            else                         //These are for the IFW 3 Focuser wheels
            {
                switch (WheelID)
                {
                    case 'A':
                        Pos = 9;
                        break;
                    case 'B':
                        Pos = 9;
                        break;
                    case 'C':
                        Pos = 6;
                        break;
                    case 'D':
                        Pos = 6;
                        break;
                    case 'E':
                        Pos = 6;
                        break;
                    case 'F':
                        Pos = 5;
                        break;
                    case 'G':
                        Pos = 5;
                        break;
                    case 'H':
                        Pos = 5;
                        break;
                    default:
                        throw new Exception("Unacceptable Wheel ID returned");

                } 
                #endregion
            }
            return Pos;
        }
    }
}
