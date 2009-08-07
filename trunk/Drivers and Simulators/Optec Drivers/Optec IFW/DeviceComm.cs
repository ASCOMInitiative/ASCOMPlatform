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
        public static char WheelID;

        private static SerialPort Port;
     
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
            // FilterNames = "";
            //TODO: change this so that it uses a variable property of COM3
            Port = new SerialPort("COM3", 19200, Parity.None, 8, StopBits.One);
        }

        public static void ConnectToDevice()
        {
            if (!Port.IsOpen) Port.Open();
            sendCmd("WSMODE", 1000);
            
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
            sendCmd("WHOMEZ", 30000);
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
                WheelID = inputbuff[0];
                return true;
            }
        }
        public static char GetWheelIdentity()
        {
            sendCmd("WIDENT", 1000);
            string inputbuff;
            try { inputbuff = Port.ReadLine(); }
            catch (TimeoutException)
            {
                throw new Exception("Failed to get wheel identity. Response Timeout Occured");
            }
            if (!inputbuff.Contains("y")) throw new Exception("Failed to get wheel identity. Incorrect Response: " + inputbuff);
            return inputbuff[0];
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
        public static string[] ReadAllNames()      //this returns a string of 40 characters that are the stored names for the filters
        {
            sendCmd("WREADZ", 1000);
            string inputbuff;
            string[] NamesToOutput = new string[0];
            try { inputbuff = Port.ReadLine(); }
            catch (TimeoutException)
            {
                throw new Exception("Failed to get current position. Response Timeout Occured");
            }
            //TODO: add the correct length for the string containing all of the names
            if (inputbuff.Length < 5) throw new Exception("Failed to get current position. Incorrect Response: " + inputbuff);
            
            if (inputbuff.Contains("ER=4")) throw new Exception("Failed to Read Names from EEPROM. Input Buffer: " + inputbuff);
            return NamesToOutput;
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
