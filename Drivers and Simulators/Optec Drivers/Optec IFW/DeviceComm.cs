using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;

namespace ASCOM.Optec_IFW
{
    class DeviceComm
    {
        #region Declaratoins

        //Device Variables
        internal static bool IsAtHome;
        internal static int NumOfFilters;
        internal static char WheelID;
        internal static TypesOfFWs FilterWheelType;
        private static string OffsetsRegString;

        //Registry Read and Write Enums
        internal enum TypesOfFWs : short { IFW, IFW3 }
        internal enum RegistryStrings : short { PortNumber, FW_Type }

        //define ASCOM tools;
        private static ASCOM.Helper.Serial SerialTools;
        private static ASCOM.Helper.Profile ProfileTools; 


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
            HomeDevice();
            GetNumOfPos();
            OffsetsRegString = FilterWheelType.ToString() + "-WID:" + WheelID + "Offsets";
        }

        public static bool CheckForConnection()
        {
            return SerialTools.Connected;
        }

        public static void DisconnectDevice()
        {
            if (SerialTools.Connected)
            {
                ExitProgramLoop();  //free up control back to the hand control box
                SerialTools.Connected = false;   //dissconnect the port
            } 
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
                IsAtHome = true;
                WheelID = inputbuff[0];
                return true;
            }

        }

        // !!!!!!!Do not need because this is done in the homing procedure!!!!!!!!!!!!!!
        //public static char GetWheelIdentity()
        //{
        //    sendCmd("WIDENT", 1000);
        //    string inputbuff;
        //    try { inputbuff = SerialTools.Receive(); }
        //    catch (TimeoutException)
        //    {
        //        throw new Exception("Failed to get wheel identity. Response Timeout Occured");
        //    }
        //    if ((inputbuff.Length < 1)) throw new Exception("Failed to get wheel identity. Incorrect Response: " + inputbuff);
        //    return inputbuff[0];
        //}

        public static short GetCurrentPos()
        {
            sendCmd("WFILTR", 1000);
            string inputbuff;
            try { inputbuff = SerialTools.Receive(); }
            catch (TimeoutException)
            {
                throw new Exception("Failed to get current position. Response Timeout Occured");
            }
            short Pos = short.Parse(inputbuff[0].ToString());
            if ((Pos == 0) || (Pos > NumOfFilters)) throw new Exception("Failed to get current position. Incorrect Response: " + inputbuff);
            
            return Pos;
        }

        public static void GoToPosition(short Pos)    //Go to a certain filter
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

        public static string[] ReadAllNames()      //Rreturns a string of 40 characters that are the stored names for the filters
        {
            sendCmd("WREADZ", 1000);
            string inputbuff;
            string[] NamesToOutput = new string[NumOfFilters];
            try { inputbuff = SerialTools.ReceiveCounted(40); }
            #region Handle Errors
		            catch (TimeoutException)
            {
                throw new Exception("Failed to get current position. Response Timeout Occured");
            }
            if (inputbuff.Length < 10) throw new Exception("Failed to get current position. Incorrect Response: " + inputbuff);
             
	        if (inputbuff.Contains("ER=4")) throw new Exception("Failed to Read Names from EEPROM. Input Buffer: " + inputbuff);
           #endregion
            for (int i = 0; i < NumOfFilters; i++)
            {
                string SingleName = inputbuff.Substring(i * 8, 8);
                SingleName = SingleName.Trim();
                NamesToOutput[i] = SingleName;
            } 
            return NamesToOutput;
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
        
        public static void SavePortNumber(string pn)    //this method saves the port number that has been selected to the registry
        {
            
            ProfileTools.WriteValue(FilterWheel.s_csDriverID, RegistryStrings.PortNumber.ToString(), pn, "");
        } 
        
        public static void SaveFilterWheelType(string ft)
        {
            ProfileTools.WriteValue(FilterWheel.s_csDriverID, RegistryStrings.FW_Type.ToString(), ft, "");
        }    
        
        public static void StoreNames(string[] Names)            //Stores the filter names on the device memory
        {
            string StringToWrite = "";
            foreach (string i in Names)
            {
                StringToWrite += i.PadLeft(8, char.Parse(" "));
            }
            if (StringToWrite.Length != NumOfFilters * 8) throw new Exception("Error Storing names to Device. Incorrect String Length");
            StringToWrite = "WLOAD" + WheelID.ToString() + "*" + StringToWrite;
            sendCmd(StringToWrite, 1000);
            string inputbuffer = SerialTools.Receive();
            if (!inputbuffer.Contains("!")) throw new Exception("Error writing names to EEPROM. Input Buffer read: " + inputbuffer);


        
        }
        
        public static void StoreFilterOffsets(int[] filteroffsets)
        {
            string OffsetsToWrite = "";
            for (int i = 0; i < NumOfFilters; i++)
            {
                OffsetsToWrite += filteroffsets[i].ToString().PadLeft(8, char.Parse(" "));
            }
            ProfileTools.WriteValue(FilterWheel.s_csDriverID, OffsetsRegString, OffsetsToWrite,"");         
        }

        public static string[] TryGetOffsets()    //Try to get offsets from the registry
        {  
            string OffsetString = ProfileTools.GetValue(FilterWheel.s_csDriverID,OffsetsRegString, "");
            string[] Offsets = new string[NumOfFilters];
            if (OffsetString != "")
            {   
                for (int i = 0; i < NumOfFilters; i++)
                {
                    string j = OffsetString.Substring(i * 8, 8);
                    j = j.Trim();    //get rid of the spaces
                    Offsets[i] = j;
                } 
            }
             return Offsets;
        }
        
        public static string TryGetCOMPort()
        {
            string cp = ProfileTools.GetValue(FilterWheel.s_csDriverID, RegistryStrings.PortNumber.ToString(), "");
            if (cp == "")
            {
                return "000";
            }
            else return cp;
        }
        
        public static string GetCOMPort()
        {    
                string cp = ProfileTools.GetValue(FilterWheel.s_csDriverID, RegistryStrings.PortNumber.ToString(), "");
                if (cp == "")
                {
                    throw new Exception("You must open up the driver settings and select a COM port first");
                }
                else return cp;    
        }
        
        public static string TryGetFilterWheelType()    //"Try" because if it may not be stored in registry yet
        {
            string ft = ProfileTools.GetValue(FilterWheel.s_csDriverID, RegistryStrings.FW_Type.ToString(), "");
            return ft;
        }
        
        public static void GetNumOfPos()
        {
            string FilterWheelType = ProfileTools.GetValue(FilterWheel.s_csDriverID, RegistryStrings.FW_Type.ToString(), "");
            if ((FilterWheelType != "IFW") && (FilterWheelType != "IFW3"))
            {
                throw new Exception("You must run the driver setup before attempting to use the Filter Wheel");
            }
            int Pos;
            if (FilterWheelType == TypesOfFWs.IFW.ToString())
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
            NumOfFilters = Pos;
        }

        public static int[] GetOffsets()
        {
            string OffsetString = ProfileTools.GetValue(FilterWheel.s_csDriverID, OffsetsRegString, "");
            int[] Offsets = new int[NumOfFilters];
            if (OffsetString != "")
            {
                for (int i = 0; i < NumOfFilters; i++)
                {
                    string j = OffsetString.Substring(i * 8, 8);
                    int k = int.Parse(j);
                    Offsets[i] = k;
                }
                return Offsets;
            }
            else throw new Exception("Failed to retrieve Offsets from Registry. Data Received = " + OffsetString);
            
        }
    }

}
