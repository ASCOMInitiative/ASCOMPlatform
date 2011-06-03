using System;
using System.Text;
using System.IO.Ports;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

namespace ASCOM.OptecFocuserHubTools
{
    public interface FocuserCommunicator
    {
        bool ConnectionOpen { get; set; }
        string SendStringReadLine(string cmd, int timeout_ms);
        List<string> SendStringReadLines(string cmd, string endString, int timeout_ms);
        List<string> SendStringReadLines(string cmd, int lines, int timeout_ms);
        void Dispose();
    }

    public class SerialCommunicator : FocuserCommunicator
    {
        static SerialPort mySerialPort = new SerialPort("COM1");
        static object Locker = new object();

        public SerialCommunicator(string serialPortName)
        {
            lock (Locker)
            {
                mySerialPort = new SerialPort();
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

        public string SendStringReadLine(string cmd, int timeout_ms)
        {
            lock (Locker)
            {
                mySerialPort.WriteTimeout = timeout_ms;
                mySerialPort.Write(cmd);
                mySerialPort.ReadTimeout = timeout_ms;
                return mySerialPort.ReadLine();
            }
        }

        public List<string> SendStringReadLines(string cmd, string endString, int timeout_ms)
        {
            lock (Locker)
            {
                mySerialPort.WriteTimeout = timeout_ms;
                mySerialPort.Write(cmd);
                List<string> resp = new List<string>();
                mySerialPort.ReadTimeout = timeout_ms;
                string r = "";
                while (true)
                {
                    r = mySerialPort.ReadLine();
                    resp.Add(r);
                    if (r == endString)
                        break;
                }
                return resp;
            }
        }

        public List<string> SendStringReadLines(string cmd, int lines, int timeout_ms)
        {
            lock (Locker)
            {
                mySerialPort.WriteTimeout = timeout_ms;
                mySerialPort.Write(cmd);
                List<string> resp = new List<string>();
                mySerialPort.ReadTimeout = timeout_ms;
                string r = "";
                for(int i = 0; i < lines; i++)
                {
                    r = mySerialPort.ReadLine();
                    resp.Add(r);
                }
                return resp;
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
        static object Locker_low = new object();
        static object Locker_high = new object();

        public EthernetCommunicator(string ipAddress, string portNum)
        {
            lock (Locker_low)
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
                    //System.Windows.Forms.MessageBox.Show("Opening Connection");
                    lock (Locker_low)
                    {
                        if (MySocket == null) MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                      
                        MySocket.Blocking = false;
                        try
                        {
                            MySocket.Connect(MyEndPoint);
                        }
                        catch { }
                        DateTime startTime = DateTime.Now;
                        while (DateTime.Now.Subtract(startTime).TotalSeconds < 1.5)
                        {
                            if (MySocket.Connected)
                                break;
                            System.Windows.Forms.Application.DoEvents();
                        }
                        if (!MySocket.Connected)
                        {
                            MySocket.Close();
                            throw new ApplicationException("Unable to connect to Focuser Hub. Are you certain the cables are connected and that you have selected the correct " + 
                            "IP address and Port number?");
                        }
                        else MySocket.Blocking = true;
                    }
                }
                else
                {
                    lock (Locker_low)
                    {
                        MySocket.Disconnect(true);
                        MySocket = null;
                    }
                }
            }
        }

        private bool SendString(string cmd, int timeout_ms)
        {
            lock (Locker_low)
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

        private string ReadLine(int timeout_ms)
        {

            int startTickCount = Environment.TickCount;
            int received = 0;  // how many bytes is already received
            byte[] buffer = new byte[1];
            string receivedString = string.Empty;
            bool newlineReceived = false;
            lock (Locker_low)
            {
                do
                {
                    MySocket.ReceiveTimeout = timeout_ms;
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
                        {
                            MySocket.Close();
                            MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            throw ex;  // any serious error occurr
                        }
                    }
                } while (!newlineReceived);

                return receivedString;
            }
        }

        public string SendStringReadLine(string cmd, int timeout_ms)
        {
            lock (Locker_high)
            {
                SendString(cmd, timeout_ms);
                return ReadLine(timeout_ms);
            }
        }

        public List<string> SendStringReadLines(string cmd, string endString, int timeout_ms)
        {
            lock (Locker_high)
            {
                SendString(cmd, timeout_ms);
                List<string> resp = new List<string>();
                string r = "";
                while (true)
                {
                    r = ReadLine(timeout_ms);
                    resp.Add(r);
                    if (r == endString)
                        break;
                }
                return resp;
            }
        }

        public List<string> SendStringReadLines(string cmd, int lines, int timeout_ms)
        {
            lock (Locker_high)
            {
                SendString(cmd, timeout_ms);
                List<string> resp = new List<string>();
                string r = "";
                for (int i = 0; i < lines; i++)
                {
                    r = ReadLine(timeout_ms);
                    resp.Add(r);
                }
                return resp;
            }
        }

        public void Dispose()
        {
            lock (Locker_low)
            {
                if (MySocket == null) return;
                MySocket.Close();
                //MyEndPoint = null;
                MySocket = null;
            }
        }

        #endregion
    }
}
