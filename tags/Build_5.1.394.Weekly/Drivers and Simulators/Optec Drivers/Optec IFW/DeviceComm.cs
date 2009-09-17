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
        //private static ASCOM.Helper.Serial SerialTools;
        private static ASCOM.Helper.Profile ProfileTools;
        private static System.IO.Ports.SerialPort Port = new SerialPort();
        

        #endregion

        static DeviceComm()                //Default Constructor
        {
            IsAtHome = false;
            NumOfFilters = 0;
            Port.BaudRate = 19200;
            Port.StopBits = StopBits.One;
            Port.Parity = Parity.None;
            ProfileTools = new ASCOM.Helper.Profile();
            ProfileTools.DeviceTypeV = "FilterWheel";
        }

        public static bool ConnectToDevice()
        {
            if (Port.IsOpen) return true;
            Cursor.Current = Cursors.WaitCursor;
            string PortNum = GetCOMPort();       //Get the Com Port number from the registry
            Port.PortName = "COM" + PortNum;
            try
            {
                Port.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("COM port is not accessable. Did you select the correct COM port? ");
                Port.Close();
                return false;
            }
            Port.ReadTimeout = 800;
            short Attempts = 0;
            string Received = "";
        TryAgain:
            Port.DiscardInBuffer();
            try
            {
                Port.Write("WSMODE");
            }
            catch
            {
                if (Attempts < 3)
                {
                    Attempts += 1;
                    goto TryAgain;
                }
                else 
                MessageBox.Show("Could not connect. Did you select the correct COM Port?");
                Port.Close();
                return false;
            }
            
            try
            {
                Received = Port.ReadLine();
            }
            catch (Exception)
            {
                if (Attempts < 3)
                {
                    Attempts += 1;
                    goto TryAgain;
                }
                else
                {
                    MessageBox.Show("No response received form device. Did you select the correct COM Port?");
                    Port.Close();
                    return false;
                }
            }
            
            if (!Received.Contains("!"))
            {
                Cursor.Current = Cursors.Default;
                Port.Close();
                MessageBox.Show("Could not connect. Did not receive ! as response. Response = " + Received);
                return false;
            }
            GetWheelIdentity();
            GetNumOfPos();
            string ReceivedFWType = ProfileTools.GetValue(FilterWheel.s_csDriverID, RegistryStrings.FW_Type.ToString(), "");
            if (ReceivedFWType.ToString() == "IFW3")
            {
                FilterWheelType = TypesOfFWs.IFW3;
            }
            else if (ReceivedFWType.ToString() == "IFW")
            {
                FilterWheelType = TypesOfFWs.IFW;
            }
            else throw new Exception("You must open up the driver settings before connecting");
            OffsetsRegString = FilterWheelType.ToString() + "-WID:" + WheelID + "Offsets";
            Cursor.Current = Cursors.Default;
            //System.Threading.Thread.Sleep(200);
            return true;
        }

        public static bool CheckForConnection()
        {
            return Port.IsOpen;
        }

        public static void DisconnectDevice()
        {
            if (Port.IsOpen)
            {
                ExitProgramLoop();  //free up control back to the hand control box
                try
                {
                    Port.Close();  //dissconnect the port
                    //System.Threading.Thread.Sleep(200);
                }
                catch(Exception)
                {
                    MessageBox.Show("Dissconnecting Failed");
                }
            } 
        }

        private static string sendCmd(string CmdToSend, int ResponseTime, short CharToRec )
        {
            short Attempts = 0;
            string Received = "";
            //SerialTools.ReceiveTimeoutMs = ResponseTime;
            Port.ReadTimeout = ResponseTime;
        TryAgain:
            Port.DiscardInBuffer();
            try
            {
                Port.Write(CmdToSend);
            }
            catch
            {
                throw new Exception("Error: Trying to send command without opening port first");
            }
            try
            {
                Received = Port.ReadLine();
            }
            catch (Exception)
            {  
                if (Attempts < 3) 
                {
                    Attempts += 1;
                    goto TryAgain;
                }
                else throw new Exception("No response received form device. Command Sent was " + CmdToSend + ". Failed 3 attempts");
            }
            return Received;
        }

        public static string DebugSendCmd(string CmdToSend, int ComPort, int ResponseTime)
        {
            System.IO.Ports.SerialPort Port = new SerialPort();
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Port.BaudRate = 19200;
                Port.DataBits = 8;
                Port.Parity = Parity.None;
                Port.StopBits = StopBits.One;
                Port.PortName = "COM" + ComPort;
                Port.Open();
                Port.DiscardInBuffer();
                Port.Write(CmdToSend);
                Port.ReadTimeout = ResponseTime;
                String Received = Port.ReadLine();

                Port.Close();
                Cursor.Current = Cursors.Default;
                return Received;
            }
            catch (TimeoutException)
            {
                Cursor.Current = Cursors.Default;
                Port.Close();
                return "Error: Response Timeout. Is the Receive Timeout set too short?";     
            }
        }

        public static bool HomeDevice()
        {
            Cursor.Current = Cursors.WaitCursor;
            string Response = sendCmd("WHOMEZ", 20000, 1);
            if (Response.Contains("E"))
            {
                throw new Exception("Home procedure failed. Wrong response returned. Response = " + Response);
            }
            else
            {
                IsAtHome = true;
                WheelID = Response[0];
                Cursor.Current = Cursors.Default;
                return true;
            }
        }

        public static void GetWheelIdentity()
        {
            Cursor.Current = Cursors.WaitCursor;
            string Response = sendCmd("WIDENT", 1000, 1);
            if ((Response.Length < 1)) throw new Exception("Failed to get wheel identity. Incorrect Response: " + Response);
            WheelID = Response[0];
            Cursor.Current = Cursors.Default;
        }

        public static short GetCurrentPos()
        {
            string Response = sendCmd("WFILTR", 1000, 1);
            short Pos = short.Parse(Response[0].ToString());
            if ((Pos < 1) || (Pos > NumOfFilters)) throw new Exception("ERROR in GetCurrentPos. Failed to get current position. Incorrect Response: " + Response);
            Pos--;  //Because Standard says first pos is 0, device says first pos is 1
            return Pos;
        }

        public static void GoToPosition(short Pos)    //Go to a certain filter
        {
            Pos += 1;   //Documentation starts at pos 0, Device firmware starts at pos 1.
            if ((Pos > NumOfFilters) || (Pos < 0)) throw new Exception("Position value is out of reach. Can not go to position " + Pos);
            string Response = sendCmd("WGOTO" + Pos, 120000, 1);
            if (!Response.Contains("*")) throw new Exception("Failed to get current position. Incorrect Response: " + Response);
            if (Response.Contains("E")) throw new Exception("Failed to move to new position. Input Buffer: " + Response);
            Cursor.Current = Cursors.Default;
        }     

        public static string[] ReadAllNames()      //Rreturns a string of 40 characters that are the stored names for the filters
        {
            
            string[] NamesToOutput = new string[NumOfFilters];
            int stringsize = 8 * NumOfFilters;
            short ShortStringSize = short.Parse(stringsize.ToString());
            string Response = sendCmd("WREADZ", 1000, ShortStringSize);
            for (int i = 0; i < NumOfFilters; i++)
            {
                string SingleName = Response.Substring(i * 8, 8);
                SingleName = SingleName.Trim();
                NamesToOutput[i] = SingleName;
            } 
            return NamesToOutput;
        }
       
        public static void ExitProgramLoop()        //Exit the serial loop and return to normal manual operation
        {
            string Response = sendCmd("WEXITS", 3000, 3);
            if (Response.Contains("E") || Response.Contains("N") || Response.Contains("D"))
            {
                //do nothing
            }
            else throw new Exception("Did not receive END response while trying to exit. Response  = " + Response);
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
            Cursor.Current = Cursors.WaitCursor;
            string StringToWrite = "";
            foreach (string i in Names)
            {
                StringToWrite += i.PadLeft(8, char.Parse(" "));
            }
            if (StringToWrite.Length != NumOfFilters * 8) throw new Exception("Error Storing names to Device. Incorrect String Length");
            StringToWrite = "WLOAD" + WheelID.ToString() + "*" + StringToWrite;
            string Response = sendCmd(StringToWrite, 1000, 1);
            System.Threading.Thread.Sleep(2500);    //wait for 2.5 seconds
            Cursor.Current = Cursors.Default;
        }

        public static void StoreCenteringData(int ComPort, int NextValue, int BackValue)
        {
            string NextString = NextValue.ToString().PadLeft(2, char.Parse("0"));
            string BackString = BackValue.ToString().PadLeft(2, char.Parse("0"));
            string CmdToSend = "WC" + BackString + NextString;

            System.IO.Ports.SerialPort Port = new SerialPort();
            try
            {
                Port.BaudRate = 19200;
                Port.DataBits = 8;
                Port.Parity = Parity.None;
                Port.StopBits = StopBits.One;
                Port.PortName = "COM" + ComPort;
                Port.Open();
                Port.DiscardInBuffer();
                Port.Write("WSMODE");
                Port.ReadTimeout = 500;
                String Received = Port.ReadLine();
                if (!Received.Contains("!")) MessageBox.Show
                    ("Could not communicate with device. Is the COM port number set right?" + Received);
                Port.DiscardInBuffer();
                Port.Write(CmdToSend);
                Port.ReadTimeout = 2000;
                Received = "";
                Received = Port.ReadLine();
                if (!Received.Contains("CW")) MessageBox.Show
                    ("Centering Values not saved. Incorrect response from Device. Response = " + Received);
                Port.Close();
                MessageBox.Show("Saved Successfully!");
            }
            
            catch (Exception Ex)
            {

                MessageBox.Show("Could not connect to device to store Centering Values. Did you select the right COM port?");
                MessageBox.Show(Ex.Data + "&&&" + Ex.Message + "&&&" + Ex.Source + "&&&" + Ex.TargetSite);
            }

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
                        throw new Exception("Unacceptable Wheel ID returned. Wheel ID = " + WheelID);
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
                        throw new Exception("Unacceptable Wheel ID returned. Wheel ID = " + WheelID);

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
