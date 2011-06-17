using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using Optec;

namespace Optec_TCF_S_Focuser
{
    public sealed class SerialCommunicator
    {
        private static readonly SerialCommunicator _instance = new SerialCommunicator();
        private SerialPort mySerialPort;
        private static object serialPortLock = new object();
        private int ConsecutiveLineReads = 0;
        private string portName = "COM1";
        private bool PortNameValid = false;

        private SerialCommunicator()
        {
            Debug.Print("Initializing Serial Communicator");
            mySerialPort = new SerialPort();
            mySerialPort.BaudRate = 19200;
            mySerialPort.NewLine = "\n\r";
            mySerialPort.PortName = portName;
        }

        public string PortName
        {
            get
            {
                return PortName;
            }
            set
            {
                bool match = false;
                foreach (string n in SerialPort.GetPortNames())
                {
                    if (n == value)
                    {
                        match = true;
                        break;
                    }
                }
                if (match == false)
                {
                    PortNameValid = false;
                    throw new InvalidOperationException("The selected COM port name does not exist or is not available. Please select a different port.");
                }
                lock (serialPortLock)
                {
                    if (mySerialPort.IsOpen) mySerialPort.Close();
                    mySerialPort.PortName = value;
                    portName = value;
                    PortNameValid = true;
                }
            }
        }

        public static SerialCommunicator Instance
        {
            get
            {
                return _instance;
            }
        }

        public string SendCommand(string cmd, int timeout)
        {
            lock (serialPortLock)
            {
                try
                {
                    if (!PortNameValid) throw new InvalidOperationException("The selected COM port name is not valid. Please select a different port.");
                    mySerialPort.ReadTimeout = timeout;
                    if (!mySerialPort.IsOpen) mySerialPort.Open();
                    mySerialPort.DiscardInBuffer();
                    mySerialPort.DiscardOutBuffer();
                    EventLogger.LogMessage("Sending (SerialCommander): " + cmd, TraceLevel.Verbose);
                    mySerialPort.Write(cmd);
                    string r = mySerialPort.ReadLine();
                    EventLogger.LogMessage("SendCommand() received: " + r, TraceLevel.Verbose);
                    return r;
                }

                catch (Exception ex)
                {
                    EventLogger.LogMessage(ex);
                    throw;
                }

            }
        }

        public string ReadALine(int timeout)
        {
            lock (serialPortLock)
            {
                if (!PortNameValid) throw new InvalidOperationException("The selected COM port name is not valid. Please select a different port.");
                try
                {
                    mySerialPort.ReadTimeout = timeout;
                    if (!mySerialPort.IsOpen) mySerialPort.Open();
                    if (ConsecutiveLineReads < 10)
                    {
                        string s = mySerialPort.ReadLine();
                        ConsecutiveLineReads++;
                        return s;
                    }
                    else
                    {
                        mySerialPort.DiscardInBuffer();
                        ConsecutiveLineReads = 0;
                        return "CLEARING INPUT BUFFER";
                    }
                }
                catch (TimeoutException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    EventLogger.LogMessage(ex);
                    throw;
                }
            }
        }

        public void CloseThePort()
        {
            lock (serialPortLock)
            {
                mySerialPort.Close();
            }
        }

        internal void ClearBuffers()
        {
            lock (serialPortLock)
            {
                if (!PortNameValid) throw new InvalidOperationException("The selected COM port name is not valid. Please select a different port.");
                if (!mySerialPort.IsOpen) mySerialPort.Open();
                mySerialPort.DiscardInBuffer();
                mySerialPort.DiscardOutBuffer();
                mySerialPort.Close();
            }
        }
    }
}
