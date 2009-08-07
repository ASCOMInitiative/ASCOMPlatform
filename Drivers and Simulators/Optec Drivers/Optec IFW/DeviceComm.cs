using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace ASCOM.Optec_IFW
{
    class DeviceComm
    {
        public static bool IsAtHome;
        public static int NumOfFilters;
        private static char WheelID;
        //public string[] FilterNames;
        //public string SerialPortName;
        private static SerialPort Port;


        static DeviceComm()                //Default Constructor
        {
            IsAtHome = false;
            NumOfFilters = 0;
            // FilterNames = "";
            //TODO: change this so that it uses a variable property of COM3
            Port = new SerialPort("COM3", 19200, Parity.None, 8, StopBits.One);
        }

        public static void ConnectToDevice()
        {
            if (!Port.IsOpen) Port.Open();
            sendCmd("FMMODE", 1000);
            
        }
        public static bool CheckForConnection()
        {
            if (!Port.IsOpen) return false;
            sendCmd("WSMODE", 500);
            string inputbuff;
            try { inputbuff = Port.ReadLine(); }
            catch (TimeoutException)
            {
                throw new Exception("Connecting to device failed. No Response Received");
            }
            if (!inputbuff.Contains("!"))
            {
                throw new Exception("Connecting to device failed. Wrong Response Received. Input Buffer = " + inputbuff);
            }
            return true;

        }
        public static void DisconnectDevice()
        {
            if (Port.IsOpen) Port.Close();
        }

        private static void sendCmd(string CmdToSend, int ResponseTime)
        {
            if (!Port.IsOpen) throw new Exception("Tried to send command while port is closed");
            Port.ReadTimeout = ResponseTime;
            Port.DiscardInBuffer();
            Port.Write(CmdToSend);
        }
        public static bool HomeDevice()
        {
            sendCmd("WHOME", 30000);
            string inputbuff;
            try { inputbuff = Port.ReadLine(); }
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
                WheelID = inputbuff[1];
                return true;
            }
        }
        public static string GetWheelIdentity()
        {
            sendCmd("WIDENT", 1000);
            string inputbuff;
            try { inputbuff = Port.ReadLine(); }
            catch (TimeoutException)
            {
                throw new Exception("Failed to get wheel identity. Response Timeout Occured");
            }
            if (!inputbuff.Contains("y")) throw new Exception("Failed to get wheel identity. Incorrect Response: " + inputbuff);
            return inputbuff;
        }
        public static string GetCurrentPos()
        {
            sendCmd("WFILTR", 1000);
            string inputbuff;
            try { inputbuff = Port.ReadLine(); }
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
            try { inputbuff = Port.ReadLine(); }
            catch (TimeoutException)
            {
                throw new Exception("Failed to get current position. Response Timeout Occured");
            }
            if (!inputbuff.Contains("*")) throw new Exception("Failed to get current position. Incorrect Response: " + inputbuff);
            //return inputbuff;
            if (inputbuff.Contains("ER=4")) throw new Exception("Failed to move to new position. Input Buffer: " + inputbuff);

        }
        public static void ReadAllNames(int Pos)      //this returns a string of 40 characters that are the stored names for the filters
        {
            sendCmd("WREAD" + Pos, 1000);
            string inputbuff;
            try { inputbuff = Port.ReadLine(); }
            catch (TimeoutException)
            {
                throw new Exception("Failed to get current position. Response Timeout Occured");
            }
            if (!inputbuff.Contains("n")) throw new Exception("Failed to get current position. Incorrect Response: " + inputbuff);
            //return inputbuff;
            if (inputbuff.Contains("ER=4")) throw new Exception("Failed to Read Names from EEPROM. Input Buffer: " + inputbuff);
        }

        public static void LoadNames(string Names)            //
        {

        }

        public static void ExitProgramLoop()        //Exit the serial loop and return to normal manual operation
        {
            sendCmd("WEXITS", 1000);
            string inputbuff;
            try { inputbuff = Port.ReadLine(); }
            catch (TimeoutException)
            {
                throw new Exception("Failed to execute EXIT command. Response Timeout Occured");
            }
            if (!inputbuff.Contains("END")) throw new Exception("Failed execute EXIT command. Incorrect Response: " + inputbuff);
        }



    }
}
