using System;
using System.Collections.Generic;
using ASCOM.Utilities;
using System.ComponentModel;

namespace ASCOM.OptecFocuserHubTools
{

    public enum ConnectionMethods { Serial, WiredEthernet, WiFi }
    public enum FOCUSER_NUMBER { ONE, TWO }
    public enum CONNECTION_STATES { DISCONNECTED, PORT_OPEN, LINK_VERIFIED }


    public class FocuserManager
    {

        #region Private Fields of Focuser **************************************************************************************************
        private const string pDriverID = "ASCOM.OptecFocuserHub.Focuser";
        private const string pConnMeth = "ConnectionMethod";
        private const string pCOMPortName = "COM Port Name";
        private const string pIPAddress = "IP Address";
        private const string pTcpipPort = "TCP/IP Port";
        private static FocuserManager focuserManager;
        private HubFocuser myFocuser1;
        private HubFocuser myFocuser2;
        private ConnectionMethods connectionMethod;
        private CONNECTION_STATES connectionState = CONNECTION_STATES.DISCONNECTED;
        private string comPortName;
        private string tcpipPortNumber;
        private string ethIPAddress;
        private static FocuserCommunicator focuserCommunicator;
        private List<string> lastStatus = new List<string>();
        private List<string> lastConfig = new List<string>();
        private Dictionary<string, char> focuserTypes = new Dictionary<string, char>();
        #endregion
        
        private FocuserManager()
        {
            // Get the connections settings from profile.
            //System.Windows.Forms.MessageBox.Show("Creating New Focuser Manager");

            Profile p = new Profile();
            p.DeviceType = "Focuser";

            focuserTypes.Add("Starlight Focuser", 'A');
            focuserTypes.Add("Optec TCF-S 2\"", 'B');
            focuserTypes.Add("Optec TCF-S 3\"", 'C');
            focuserTypes.Add("Optec TCF-S 2\" Ext. Travel", 'D');
            focuserTypes.Add("Optec 8\" Edge-HD Focuser", 'E');
            focuserTypes.Add("Optec 11\" Edge-HD Focuser", 'F');
            focuserTypes.Add("Optec Thin Focuser", 'G');

            connectionMethod = (ConnectionMethods)Enum.Parse(typeof(ConnectionMethods),
                p.GetValue(pDriverID, pConnMeth, "", ConnectionMethods.Serial.ToString()));
            comPortName = p.GetValue(pDriverID, pCOMPortName, "", "COM1");
            ethIPAddress = p.GetValue(pDriverID, pIPAddress, "", "192.168.1.107");
            tcpipPortNumber = p.GetValue(pDriverID, pTcpipPort, "", "9760");

            switch (connectionMethod)
            {
                default:
                case ConnectionMethods.Serial:
                   
                    focuserCommunicator = new SerialCommunicator(comPortName);
                    break;
                
                case ConnectionMethods.WiredEthernet:
                    
                    focuserCommunicator = new EthernetCommunicator(ethIPAddress, tcpipPortNumber);
                    break;
                case ConnectionMethods.WiFi:
                    break;
            }

            myFocuser1 = new HubFocuser(FOCUSER_NUMBER.ONE, focuserCommunicator);
            myFocuser2 = new HubFocuser(FOCUSER_NUMBER.TWO, focuserCommunicator);

           
        }


        public static FocuserManager GetInstance()
        {
            if(focuserManager == null)
            {
                focuserManager = new FocuserManager();
            }
            return focuserManager;
        }

        public Dictionary<string, char> FocuserTypes { get { return focuserTypes; } }

        public ConnectionMethods ConnectionMethod
        {
            get { return connectionMethod; }
            set
            {
                // Make sure the new method is different that than the current method.
                if (value == connectionMethod) return;
                // Make sure the device is disconnected
                if (connectionState != CONNECTION_STATES.DISCONNECTED)
                    throw new ApplicationException("Focuser hub must be disconnected to changed connection method.");

                // Close the current connection if one exists
                if (focuserCommunicator != null)
                    focuserCommunicator.Dispose();
                connectionMethod = value;
                // Store the connection method in the profile store.
                Utilities.Profile p = new Profile();
                p.DeviceType = "Focuser";
                p.WriteValue(pDriverID, pConnMeth, value.ToString());
                
                switch (value)
                {
                    case ConnectionMethods.Serial:
                        focuserCommunicator = new SerialCommunicator(comPortName);
                        break;
                    case ConnectionMethods.WiredEthernet:
                        focuserCommunicator = new EthernetCommunicator(ethIPAddress, tcpipPortNumber);
                        break;
                }
                myFocuser1.SetFocuserCommunicator(focuserCommunicator);
                myFocuser2.SetFocuserCommunicator(focuserCommunicator);
                connectionMethod = value;
            }
        }

        public bool Connected
        {
            get
            {
                if (connectionState == CONNECTION_STATES.LINK_VERIFIED)
                {
                    if (focuserCommunicator.ConnectionOpen) return true;
                    else
                    {
                        connectionState = CONNECTION_STATES.DISCONNECTED;
                        return false;
                    }
                }
                else return false;
            }
            set
            {
                if (value)
                {
                    //System.Windows.Forms.MessageBox.Show("Connecting");
                    connect();
                }
                else
                {
                    disconnect();
                }
            }
        }

        public string IPAddress
        {
            get { return ethIPAddress; }
            set
            {               
                ethIPAddress = value;
                Utilities.Profile p = new Utilities.Profile();
                p.DeviceType = "Focuser";
                p.WriteValue(pDriverID, pIPAddress, value); 
            }
        }

        public string TCPIPPort
        {
            get { return tcpipPortNumber; }
            set 
            { 
                tcpipPortNumber = value;
                Utilities.Profile p = new Utilities.Profile();
                p.DeviceType = "Focuser";
                p.WriteValue(pDriverID, pTcpipPort, value);
            }
        }

        public string COMPortName
        {
            get { return comPortName; }
            set
            {
                comPortName = value;
                Utilities.Profile p = new Utilities.Profile();
                p.DeviceType = "Focuser";
                p.WriteValue(pDriverID, pCOMPortName, value);
            }
        }

        /// <summary>
        /// Connects to the focuser using the selected connection type.
        /// Sets MaxPosition
        /// </summary>
        private void connect()
        {
            // 0. Check if it is already connected
            if (connectionState == CONNECTION_STATES.LINK_VERIFIED)
            {
                return;
            }

            // 1. Make sure a focuser communicator exists.
            if (focuserCommunicator == null)
            {
                switch (connectionMethod)
                {
                    case ConnectionMethods.Serial:
                        focuserCommunicator = new SerialCommunicator(comPortName);
                        break;
                    case ConnectionMethods.WiredEthernet:
                        focuserCommunicator = new EthernetCommunicator(ethIPAddress, tcpipPortNumber);
                        break;
                    case ConnectionMethods.WiFi:
                        //TODO: Finish implementing this...
                        // //focuserCommunicator = new WiFICommunicator(WiFiInfo);
                        break;
                }
                myFocuser1.SetFocuserCommunicator(focuserCommunicator);
                myFocuser2.SetFocuserCommunicator(focuserCommunicator);
            }
            else
            {
                // Make sure the type didn't change
                switch (connectionMethod)
                {
                    case ConnectionMethods.Serial:
                        if (focuserCommunicator.GetType() != typeof(SerialCommunicator))
                        {
                            focuserCommunicator.Dispose();
                            focuserCommunicator = null;
                            connect();
                            return;
                        }
                        break;
                    case ConnectionMethods.WiredEthernet:
                        if (focuserCommunicator.GetType() != typeof(EthernetCommunicator))
                        {
                            focuserCommunicator.Dispose();
                            focuserCommunicator = null;
                            connect();
                            return;
                        }
                        // TODO: Implement this...
                        break;
                    case ConnectionMethods.WiFi:
                        // TODO: Implement this...
                        break;
                }
            }

            // 3. Try to open the conneciton...            
            if (!focuserCommunicator.ConnectionOpen)
            {
                try
                {

                    focuserCommunicator.ConnectionOpen = true;
                    connectionState = CONNECTION_STATES.PORT_OPEN;

                }
                catch
                {
                    focuserCommunicator.Dispose();
                    connectionState = CONNECTION_STATES.DISCONNECTED;
                    throw;
                }

            }

            //4 Try to get the Config and status
            Focuser1.getConfig();
            Focuser1.getStatus();
            Focuser2.getConfig();
            Focuser2.getStatus();

            // 5. Set the connection state
            connectionState = CONNECTION_STATES.LINK_VERIFIED;

        }

        /// <summary>
        /// Disconnects from a focuser
        /// </summary>
        private void disconnect()
        {
            try
            {
                focuserCommunicator.ConnectionOpen = false;
                connectionState = CONNECTION_STATES.DISCONNECTED;
            }
            catch
            {
                // Do nothing here... just disconect
            }
        }

        public HubFocuser Focuser1
        {
            get { return myFocuser1; }
        }

        public HubFocuser Focuser2
        {
            get { return myFocuser2; }
        }
    }

}
