using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.IO.Ports;
using System.Net.Sockets;
using System.Threading;
using ASCOM.Utilities;

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
        private FocuserCommunicator focuserCommunicator;
        private HubFocuser FocuserInst1 = null;
        private HubFocuser FocuserInst2 = null;
        private List<string> lastStatus = new List<string>();
        private List<string> lastConfig = new List<string>();
        
        #endregion

        public event EventHandler ConnectionStateChanged;
        
        private FocuserManager()
        {
            // Get the connections settings from profile.
            System.Windows.Forms.MessageBox.Show("Creating New Focuser Manager");

            Profile p = new Profile();
            p.DeviceType = "Focuser";
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

        public ConnectionMethods ConnectionMethod
        {
            get { return connectionMethod; }
            set
            {
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
                
            }
        }

        public bool Connected
        {
            get
            {
                if (connectionState == CONNECTION_STATES.LINK_VERIFIED) return true;
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
            focuserCommunicator.ConnectionOpen = false;
            connectionState = CONNECTION_STATES.DISCONNECTED;
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


    public class HubFocuser
    {
        //public delegate void FocuserEventHandler(object sender, FocuserEventArgs e); 
        
        public event EventHandler StatusChanged;
        public event EventHandler ConfigurationChanged;
        private FocuserCommunicator focuserCommunicator;
        private DateTime lastStatusUpdateTime = new DateTime(1, 1, 1);
        private DateTime lastConfigUpdateTime = new DateTime(1, 1, 1);

        #region Constructors / Destructors **************************************************************************************************

        public HubFocuser(FOCUSER_NUMBER n, FocuserCommunicator focComm)
        {
            //this.connectionState = CONNECTION_STATES.DISCONNECTED;
            this.focuserCommunicator = focComm;
            this.focuserNumber = n;
        }

        #endregion

        #region Private Fields of Focuser *************************************************************************************************

        const char newline = '\n';
        private FOCUSER_NUMBER focuserNumber = FOCUSER_NUMBER.ONE;
        // Config values
        private int maxPosition = 0;
        private string nickname = "Unknown";
        private int maxSpeed = 0;
        private int maxPower = 0;
        private string deviceType = "";
        private bool tempCompEnabled = false;
        private int tempCompA, tempCompB, tempCompC, tempCompD, tempCompE;
        private string tempCompMode = "";
        private bool backlashCompEnabled = false;
        private int backlashCompSteps = 0;
        private int ledBrightness = 0;
        private bool isInSleepMode = false;
        // Status values
        private double currentTempC = 0;
        private int currentPosSteps = 0;
        private int targetPosSteps = 0;
        private bool isMoving = false;
        private bool isHoming = false;
        private bool isHomed = false;
        private bool tempProbeAttached = false;
        private bool remoteInOutAttached = false;
        private bool handControlAttached = false;
        private bool hasBacklashComp = false;


        #endregion



        #region Public Properties of Focuser ***************************************************************************************

        #endregion

        #region Public NonStatic Properties of Focuser ************************************************************************************

        public int MaxPosition
        {
            get
            {
                getConfig(); 
                return maxPosition; 
            }
        }

        public int CurrentPositionSteps
        {
            get {
                getStatus();
                return currentPosSteps; 
            }
        }

        public int CurrentPositionMicrons
        {
            get { throw new ApplicationException(); }
        }

        public int TargetPositionSteps
        {
            get
            {
                getStatus(); 
                return targetPosSteps; 
            }
        }

        public int TargetPositionMicrons
        {
            get { throw new ApplicationException(); }
        }

        public double CurrentTempC
        {
            get{ getStatus(); return currentTempC; }
        }

        public double CurrentTempF
        {
            get { throw new ApplicationException(); }
        }

        public double CurrentTempK
        {
            get { throw new ApplicationException(); }
        }

        public bool IsMoving { get { getStatus();  return isMoving; } }

        public bool IsHoming { get { getStatus(); return isHoming; } }

        #endregion

        #region Private Methods of Focuser ************************************************************************************************

        internal void getConfig()
        {
            // 1. Make sure the connection state is correct
            /*
            if (connectionState == CONNECTION_STATES.DISCONNECTED)
            {
                throw new ApplicationException("Attempted to get config info before connecting to device.");
            }
             * */

            // Make sure there has been some delay since the last check...
            if (DateTime.Now.Subtract(lastConfigUpdateTime).TotalMilliseconds < 100)
                return;
            else lastStatusUpdateTime = DateTime.Now;

            // 2. Format the command string
            string cmd = (this.focuserNumber == FOCUSER_NUMBER.ONE) ? "<F1GETCONFIG>" : "<F2GETCONFIG>";

            // 3. Send the command string
            focuserCommunicator.SendString(cmd, 1000);

            // 4. Receive the bang
            if (focuserCommunicator.ReadLine(2000) != "!")
                throw new ApplicationException("Did not receive \"!\" in response to sent command \"" + cmd + "\"");

            // 5. Receive the config data (or error text if there was a problem)
            string resp = focuserCommunicator.ReadLine(2000);
            List<string> ConfigData = new List<string>();
            if (resp.Contains("CONFIG"))
            {
                string r = " ";
                while (true)
                {
                    r = focuserCommunicator.ReadLine(2000);
                    if (r.Contains("END")) break;
                    else ConfigData.Add(r);
                }
            }
            else if (resp.Contains("ER=")) throw new ApplicationException("The device return the error \"" + resp + "\" from command \"" + cmd + "\".");

            // 6. Parse the Config data
            parseConfigData(ConfigData);

        }

        private void parseConfigData(List<string> data)
        {
            int start = 11;

            // 1. Get the device Nickname
            this.nickname = data[0].Substring(start);
            // 2. Get the Max Position
            this.maxPosition = int.Parse(data[1].Substring(start));
            // 3. Get the Max Speed
            this.maxSpeed = int.Parse(data[2].Substring(start));
            // 4. Get the Max Power
            this.maxPower = int.Parse(data[3].Substring(start));
            // 5. Get the Device Type
            this.deviceType = data[4].Substring(start);
            // 6. Get the Temp Comp On flag
            this.tempCompEnabled = (data[5].Substring(start) == "1") ? true : false;
            // 7. Get the Temperature Coefficient for A
            this.tempCompA = int.Parse(data[6].Substring(start));
            // 8. Get the Temperature Coefficient for B
            this.tempCompB = int.Parse(data[7].Substring(start));
            // 9. Get the Temperature Coefficient for C
            this.tempCompC = int.Parse(data[8].Substring(start));
            // 10. Get the Temperature Coefficient for D
            this.tempCompD = int.Parse(data[9].Substring(start));
            // 11. Get the Temperature Coefficient for E
            this.tempCompE = int.Parse(data[10].Substring(start));
            // 12. Get the Temp Comp Mode
            this.tempCompMode = data[11].Substring(start);
            // 13. Get the Backlash Compensation Enabled Flag
            this.backlashCompEnabled = (data[12].Substring(start) == "1") ? true : false;
            // 14 Get the Backlash Compensatino Steps 
            this.backlashCompSteps = int.Parse(data[13].Substring(start));
            // 15. Get the LED brightness setting
            this.ledBrightness = int.Parse(data[14].Substring(start));
            // 16. Get the SleepMode designator
            this.isInSleepMode = (data[15].Substring(start) == "1") ? true : false;


        }

        internal void getStatus()
        {

            // 1. Make sure the connection state is correct
            /*
            if (connectionState == CONNECTION_STATES.DISCONNECTED)
            {
                throw new ApplicationException("Attempted to get status info before connecting to device.");
            }
             */

            // Make sure there has been some delay since the last check...
            if (DateTime.Now.Subtract(lastStatusUpdateTime).TotalMilliseconds < 100)
                return;
            else lastStatusUpdateTime = DateTime.Now;

            // 2. Format the command string
            string cmd = (this.focuserNumber == FOCUSER_NUMBER.ONE) ? "<F1GETSTATUS>" : "<F2GETSTATUS>";

            // 3. Send the command string
            focuserCommunicator.SendString(cmd, 1000);

            // 4. Receive the bang
            if (focuserCommunicator.ReadLine(2000) != "!")
                throw new ApplicationException("Did not receive \"!\" in response to sent command \"" + cmd + "\"");

            // 5. Receive the status data (or error text if there was a problem)
            string resp = focuserCommunicator.ReadLine(2000);
            List<string> StatusData = new List<string>();
            if (resp.Contains("STATUS"))
            {
                string r = " ";
                while (true)
                {
                    r = focuserCommunicator.ReadLine(2000);
                    if (r.Contains("END")) break;
                    else StatusData.Add(r);
                }
            }
            else if (resp.Contains("ER=")) throw new ApplicationException("The device return the error \"" + resp + "\" from command \"" + cmd + "\".");

          
            // 6. Parse the Status data
            parseStatusData(StatusData);

            // 7. Trigger the event if something was different
            /*
            if (!StatusData.Equals(lastStatus))
                TriggerAnEvent(StatusChanged);
             * */

        }

        private void parseStatusData(List<string> data)
        {
            int start = 11;
            int i = 0;
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NegativeSign = "-"; nfi.PositiveSign = "+";
            nfi.NumberDecimalSeparator = "."; nfi.NumberGroupSeparator = ",";

            // 1. Get the Current Temp
            string temp = data[i++].Substring(start);
            this.currentTempC = double.Parse(temp, nfi);
            // 2. Get the Current Position
            this.currentPosSteps = int.Parse(data[i++].Substring(start));
            // 3. Get the Target Position
            this.targetPosSteps = int.Parse(data[i++].Substring(start));
            // 4. Get the isMoving Flag
            this.isMoving = (data[i++].Substring(start) == "1") ? true : false;
            // 5. Get the isHoming Flag
            this.isHoming = (data[i++].Substring(start) == "1") ? true : false;
            // 6. Get the isHomed Flag
            this.isHomed = (data[i++].Substring(start) == "1") ? true : false;
            // 7. Get the TempProbeAttached flag
            this.tempProbeAttached = (data[i++].Substring(start) == "1") ? true : false;
            // 8. Get the Remote In/Out Attached flag
            this.remoteInOutAttached = (data[i++].Substring(start) == "1") ? true : false;
            // 9. Get the HandController Attached flag
            this.handControlAttached = (data[i++].Substring(start) == "1") ? true : false;
        }

        private void TriggerAnEvent(EventHandler EH)
        {
            if (EH == null) return;
            var EventListeners = EH.GetInvocationList();
            if (EventListeners != null)
            {
                for (int index = 0; index < EventListeners.Count(); index++)
                {
                    var methodToInvoke = (EventHandler)EventListeners[index];
                    methodToInvoke.BeginInvoke(this, EventArgs.Empty, EndAsyncEvent, new object[] { });
                }
            }

        }

        private static void EndAsyncEvent(IAsyncResult iar)
        {
            var ar = (System.Runtime.Remoting.Messaging.AsyncResult)iar;
            var invokedMethod = (EventHandler)ar.AsyncDelegate;

            try
            {
                invokedMethod.EndInvoke(iar);
            }
            catch
            {
                // Handle any exceptions that were thrown by the invoked method
                Console.WriteLine("An event listener went kaboom!");
            }
        }


        #endregion

        #region Public Non-Static Methods of Focuser **************************************************************************************

        public void MoveAbsolute(int newPos)
        {
            // 1. Verify the position is valid
            
            if (newPos > MaxPosition) throw new ApplicationException("Invalid Position Request");
            // 2. Format the command string to the form <F1MA12345>
            string cmd = (this.focuserNumber == FOCUSER_NUMBER.ONE) ? "<F1MA" : "<F2MA";
            cmd += (newPos.ToString().PadLeft(5, '0') + ">");
            // 3. Send the command
            focuserCommunicator.SendString(cmd, 1000);
            // 4. Receive the bang
            if (focuserCommunicator.ReadLine(1000) != "!")
                throw new ApplicationException("Did not receive \"!\" in response to sent command \"" + cmd + "\"");
            // 5. Receive the acknowledgment (or error text if there was a problem)
            string resp = focuserCommunicator.ReadLine(1000);
            if (resp == "M")
            {
                return;
            }
            else if (resp.Contains("ER=")) throw new ApplicationException("The device return the error \"" + resp + "\" from command \"" + cmd + "\".");
        }

        public void Home()
        {
            // 1. create the command string
            string cmd = (this.focuserNumber == FOCUSER_NUMBER.ONE) ? "<F1HOME>" : "<F2HOME>";
            // 2. send the command
            focuserCommunicator.SendString(cmd, 1000);
            // 3. Receive the bang
            if (focuserCommunicator.ReadLine(1000) != "!")
                throw new ApplicationException("Did not receive \"!\" in response to sent command \"" + cmd + "\"");
            // 5. Receive the acknowledgment (or error text if there was a problem)
            /*
            string resp = focuserCommunicator.ReadLine(1000);
            if (resp == "H")
            {
                return;
            }
            else if (resp.Contains("ER=")) throw new ApplicationException("The device return the error \"" + resp + "\" from command \"" + cmd + "\".");
            */
        }

        public void RefreshStatus() { this.getStatus(); }

        public void RefreshConfig() { this.getConfig(); }

        #endregion

    }

    public interface FocuserCommunicator
    {
        bool ConnectionOpen { get; set; }
        bool SendString(string cmd, int timeout_ms);
        string ReadLine(int timeout_ms);
        void Dispose();
    }

    public class SerialCommunicator : FocuserCommunicator
    {
        SerialPort mySerialPort = new SerialPort("COM1");
        static object Locker = new object();

        public SerialCommunicator(string serialPortName)
        {
            lock (Locker)
            {
                mySerialPort.PortName = serialPortName;
                mySerialPort.BaudRate = 115200;
                mySerialPort.NewLine = "\n";
            }
        }

        #region FocuserCommunicator Members

        public bool ConnectionOpen
        {
            get
            {
                return mySerialPort.IsOpen;
            }
            set
            {
                lock (Locker)
                {
                    if (value) mySerialPort.Open();
                    else mySerialPort.Close();
                }
            }
        }

        public bool SendString(string cmd, int timeout_ms)
        {
            lock (Locker)
            {
                mySerialPort.WriteTimeout = timeout_ms;
                mySerialPort.Write(cmd);
                return true;
            }
        }

        public string ReadLine(int timeout_ms)
        {
            lock (Locker)
            {
                mySerialPort.ReadTimeout = timeout_ms;
                return mySerialPort.ReadLine();
            }
        }

        public void Dispose()
        {
            lock (Locker)
            {
                if (mySerialPort == null) return;
                if (mySerialPort.IsOpen) mySerialPort.Close();
                mySerialPort.Dispose();
                mySerialPort = null;
            }
        }

        #endregion
    }

    public class EthernetCommunicator : FocuserCommunicator
    {
        Socket MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        System.Net.IPEndPoint MyEndPoint;
        static object Locker = new object();

        public EthernetCommunicator(string ipAddress, string portNum)
        {
            lock (Locker)
            {
                int Port = System.Convert.ToInt16(portNum, 10);
                System.Net.IPAddress IP = System.Net.IPAddress.Parse(ipAddress);
                MyEndPoint = new System.Net.IPEndPoint(IP, Port);
            }

        }

        #region FocuserCommunicator Members

        public bool ConnectionOpen
        {
            get
            {
                if (MySocket == null) return false;
                if (MySocket.Connected) return true;
                else return false;

            }
            set
            {
                if (value)
                {
                    lock (Locker)
                    {
                        if (MySocket == null) MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        MySocket.Connect(MyEndPoint);
                    }
                }
                else
                {
                    lock (Locker)
                    {
                        MySocket.Disconnect(true);
                        MySocket = null;
                    }
                }
            }
        }

        public bool SendString(string cmd, int timeout_ms)
        {
            lock (Locker)
            {
                // Make sure the socket is connected
                if (!MySocket.Connected)
                    throw new ApplicationException("Device must be connected before sending commands");

                // Device IS connected
                byte[] buff = Encoding.ASCII.GetBytes(cmd);
                int startTickCount = Environment.TickCount;
                int sent = 0;  // how many bytes is already sent
                do
                {
                    if (Environment.TickCount > startTickCount + timeout_ms)
                        throw new Exception("Timeout.");
                    try
                    {
                        sent += MySocket.Send(buff, SocketFlags.None);
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode == SocketError.WouldBlock ||
                            ex.SocketErrorCode == SocketError.IOPending ||
                            ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                        {
                            // socket buffer is probably full, wait and try again
                            Thread.Sleep(30);
                        }
                        else
                            throw ex;  // any serious error occurr
                    }
                } while (sent < buff.Length);
                return true;
            }

        }

        public string ReadLine(int timeout_ms)
        {

            int startTickCount = Environment.TickCount;
            int received = 0;  // how many bytes is already received
            byte[] buffer = new byte[1];
            string receivedString = string.Empty;
            bool newlineReceived = false;
            lock (Locker)
            {
                do
                {
                    if (Environment.TickCount > startTickCount + timeout_ms)
                        throw new Exception("Timeout.");
                    try
                    {
                        received += MySocket.Receive(buffer, 1, SocketFlags.None);
                        string r = Encoding.ASCII.GetString(buffer);
                        if (r.Contains("\n")) newlineReceived = true;
                        else receivedString += r;
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode == SocketError.WouldBlock ||
                            ex.SocketErrorCode == SocketError.IOPending ||
                            ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                        {
                            // socket buffer is probably empty, wait and try again
                            Thread.Sleep(30);
                        }
                        else
                            throw ex;  // any serious error occurr
                    }
                } while (!newlineReceived);

                return receivedString;
            }
        }

        public void Dispose()
        {
            lock (Locker)
            {
                if (MySocket == null) return;
                if (MySocket.Connected)
                    MySocket.Disconnect(false);
            }
        }

        #endregion
    }
}
