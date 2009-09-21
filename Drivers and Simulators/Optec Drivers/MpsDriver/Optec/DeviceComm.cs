using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ASCOM.Helper;

namespace ASCOM.Optec
{
    [ComVisible(false)]
    class DeviceComm    //this class defines the hardware level methods of the MPS
    {
        private static short d_ComState;
        private static short d_CurrentPosition;
        private static Helper.Serial SerialTools = new ASCOM.Helper.Serial(); 
        private static bool PortSelected;
        private static string MPSErrorMsg;

        static DeviceComm()
        {
            //Initialize global parameters
            SerialTools.Speed = PortSpeed.ps19200;
            if (Properties.Defaults.Default.COMPort > 0)
            {
                PortSelected = true;
                SerialTools.Port = Properties.Defaults.Default.COMPort;
            }
            else { PortSelected = false; }
            d_ComState = 0;
            d_CurrentPosition = 0;
            MPSErrorMsg = "Multi-Port Selector ERROR:\n";
        }
        public static short ComState
        {
            //this property is read only form outside the class
            get { return d_ComState; }
        }
        public static short CurrentPosition //Position Property
        {
            get { return d_CurrentPosition; }
            set { SetPosition(value); }
        }
        private static void SetPosition(short value)
        {

            if (value < 1 || value > 4)
            {
                throw new InvalidOperationException("Can not move to the position " + value + 
                    "/n Position must be between 0 and 4");
                

            }
            if (ComState == 2)
            {
                string cmdstring = "RP000" + value.ToString();

                if (SendCmd(cmdstring, 1000).Contains("!"))
                {
                    d_CurrentPosition = value;
                }
                //else return without changing currentposition
                return;
            }
            else
            {
                //do not change current position
                return;
            }

            ;
;
        }
        private static bool SetComState(short desiredState)
        {
            /*
            Layers or connectivity
            •	COM Port - Open or Closed
                    o	State 0 = Port is Closed, device may or may not be active but no communication is possible
                    o	State 1 = Port is Open, device may or may not be active but no communication has taken place
                                        The device should not be in the serial loop
            •	Device has communicated
                    o	State 2 = Port is Open, Communication has taken place with the device, assume device can accept commands
            •	Device is BUSY
                    o	State 3 = Port is Open, Communication has taken place but the device is busy
            */
            bool returnValue = false;
            try
            {
                if (PortSelected == false)
                {
                    //attempt to get the user to select a COM Port
                    //if they select one continue if they don't fail
                    if (SelectCOMPort() == false) { d_ComState = 0; return false; }
                }
                if (desiredState == 0)
                {
                    
                    if (SerialTools.Connected)
                    {
                        
                        
                        SerialTools.Connected = false; //Close the port
                    
                    }
                    d_ComState = 0;
                    returnValue = true;
                }
                if (desiredState > 0)
                {
                    if (SerialTools.Connected == false) SerialTools.Connected = true;
                }
                if (desiredState == 2)
                {
                    string received = "";
                    SerialTools.Transmit("WSMODE");
                    SerialTools.ReceiveTimeoutMs = 300;
                    SerialTools.ClearBuffers();
                    received = SerialTools.ReceiveTerminated("\n\r");
                    if (!received.Contains("!"))
                    {
                        d_ComState = 1;
                        throw new FormatException("When connecting, response did not contain \"!\"");
                        //return false;
                    }
                    else
                    {
                        d_ComState = 2;
                        returnValue = true;
                    }

                }
                if (desiredState == 3)
                {
                    //this will never be the desired state
                }
            }
            catch (COMException Ex)
            {
                string DescribeError = "A timeout occured while communicating with device.\n";
                string Advice = "Is the correct COM port selected?\n Did serial cable come unplugged?\n";
                MessageBox.Show(MPSErrorMsg + DescribeError + Advice + Ex.ToString());
            }
            catch (FormatException Ex)
            {
                string DescribeError = "Unexpected Device Response.\n";
                string Advice = "Is the correct COM port selected?\n";
                MessageBox.Show(MPSErrorMsg + DescribeError + Advice + Ex.ToString());

            }
            catch (Exception Ex)
            {
                string DescribeError = "An Unexpected Error Occured.\n";
                string Advice = "Please email Jordan@optecinc.com with the following message" +
                " and a description of what led to this problem: \n";
                MessageBox.Show(MPSErrorMsg + DescribeError + Advice + Ex.ToString());
            }
            
            return returnValue;
            
        }
        private static bool SelectCOMPort()
        {
            //Prompt user to select a COM Port 
            COMPortForm CPForm = new COMPortForm();
            CPForm.ShowDialog();
            if (Properties.Defaults.Default.COMPort > 0) return true;
            else return false;
        }
        private static string SendCmd(string CMD, int Timeout )
        {
            string ReturnString = "DEVICE-ERROR";   //guilty until proven innocent

            // SHOULD NOT GET HERE IF ComState IS NOT ALREADY 2 SO NO NEED TO CHECK
            //step 1
            //if (ComState != 2) 
            //{
            //    SetComState(2);
            //}

            //step 2
            SerialTools.ClearBuffers();

            //step 3
            try
            {
                SerialTools.Transmit(CMD);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("An error occured while attempting to send command: " + CMD + 
                    "\n Error Details: " + Ex.ToString());
                return ReturnString;
            }
  
            try
            {
                SerialTools.ReceiveTimeoutMs = Timeout;
                ReturnString = SerialTools.ReceiveTerminated("\n\r");  //line feed, carriage return
            }
            catch(Exception Ex)
            {
                MessageBox.Show("An error occured while attempting to send command: " + CMD +
                    "\n Error Details: " + Ex.ToString()); 
            }

            return ReturnString;
        }
        internal static void Connect()
        {
            //THIS SHOULD GET THE DEVICE IN THE SERIAL LOOP SO WE CAN TALK TO IT
            //AND IT SHOULD GET THE CURRENT POSITION OF THE DEVICE AND STORE IT IN Current_Position
            SetComState(2); //get device communicating (in serial loop)
            //GetPosition();  //this sets d_CurrentPosition to the device current pos
        }
        internal static void Disconnect()
        {
            SetComState(0);
        }
        private static void GetPosition()
        {
            string Pos = "";
            if (ComState == 2)
            {
                Pos = SendCmd("RX0000", 1000);
            }
            else
            {
                return;
            }

            if (Pos.Contains("ERROR")) throw new COMException("SendCmd attempt failed");
            else if (Pos.Length < 1) throw new COMException("SendCmd yeilded no response");
            else d_CurrentPosition = short.Parse(Pos.Substring(3, 1));
        }

    }
}
