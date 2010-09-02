using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.ComponentModel;
using Optec;
using System.Diagnostics;


namespace ASCOM.OptecTCF_S
{

    partial class OptecFocuser
    {
        // 
        
        //Enumerations and Properties 


        public enum ConnectionStates
        {
            Disconnected,
            SerialMode,
            TempCompMode,
            Sleeping
        }

        /// <summary>
        /// Represents the current connection state of the focuser
        /// </summary>
        private static ConnectionStates _ConnectionState;
        public static ConnectionStates ConnectionState
        {
            get { return _ConnectionState; }
        }

        /// <summary>
        /// Static constructor for the OptecFocuser class.
        /// Creates an instance of a SerialPort object and sets the baudrate to 19200
        /// Defaults the connection state to Disconnected
        /// </summary>
        static OptecFocuser()
        {
            mySerialPort = new SerialPort();
            _ConnectionState = ConnectionStates.Disconnected;
            mySerialPort.BaudRate = 19200;

        }

        /// <summary>
        /// Open the serial port then send the serial command to enter serial mode "FMMODE"
        /// Expects the response "!" back from the device.
        /// </summary>
        public static void ConnectAndEnterSerialMode()
        {
            //*** Open up the COM Port **********************************************
            try
            {
                EventLogger.LogMessage("Attempting to connect and enter serial mode", TraceLevel.Info);

                if (!mySerialPort.IsOpen)
                {
                    mySerialPort.PortName = DeviceSettings.COMPort;
                    mySerialPort.Open();
                }
            }
            catch (InvalidOperationException ex)
            {
                EventLogger.LogMessage(ex);
                mySerialPort.Close();
                throw new InvalidOperationException("The Port Is Already Open");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                EventLogger.LogMessage(ex);
                mySerialPort.Close();
                throw new InvalidOperationException("Port Properties Are Not Configured Properly");
            }
            catch (System.Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw;
            }


            //*** Send Command To Enter Serial Mode**************************************
            try
            {
                SendCommandGetResponse("FMMODE", "!", 500, 4);    //Send FMMODE to Enter Serial
                _ConnectionState = ConnectionStates.SerialMode;
                EventLogger.LogMessage("Successfully entered serial mode", TraceLevel.Info);

            }
            catch (TimeoutException ex)
            {
                EventLogger.LogMessage(ex);
                mySerialPort.Close();
                _ConnectionState = ConnectionStates.Disconnected;
                throw new TimeoutException("Did Not Receive A Response Back From Command: FMMODE");
            }

            try
            {
                string[] FVandDT = GetFVandDT();
                DeviceSettings.FirmwareVersion = FVandDT[0];

                // Only set the device type if the user has not yet set it manually.

                if (DeviceSettings.DeviceType == DeviceSettings.DeviceTypes.Unknown)
                {
                    if (FVandDT[1].Contains("3"))
                    {
                        DeviceSettings.DeviceType = DeviceSettings.DeviceTypes.TCF_S3;
                    }
                    else
                    {
                        DeviceSettings.DeviceType = DeviceSettings.DeviceTypes.TCF_S;
                    }
                }
            }
            catch
            {
                // We get here when the firmware is too old to support getting the firmware version
                DeviceSettings.FirmwareVersion = "2.??";
                if (DeviceSettings.DeviceType == DeviceSettings.DeviceTypes.Unknown)
                {
                    DeviceSettings.DeviceType = DeviceSettings.DeviceTypes.TCF_S;
                }
            }


        }

        /// <summary>
        /// Attempt to Exit Temperature Compensation Mode, Exit Sleep Mode, Exit Serial Mode, and finally close the port
        /// </summary>
        public static void Disconnect()
        {
            try
            {
                EventLogger.LogMessage("Disconnect from device", TraceLevel.Info);
                // Exit Temp Comp mode
                if (ConnectionState == ConnectionStates.TempCompMode)
                {
                    ExitTempCompMode();
                }

                // Exit Sleep Mode
                if (ConnectionState == ConnectionStates.Sleeping)
                {
                    WakeUpDevice();
                }

                // Exit Serial Mode
                if (ConnectionState == ConnectionStates.SerialMode)
                {
                    ExitSerialMode();
                }

                // Close the port
                if (mySerialPort.IsOpen) mySerialPort.Close();

                EventLogger.LogMessage("Disconnected successfully", TraceLevel.Info);
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                _ConnectionState = ConnectionStates.Disconnected;
                //throw new ASCOM.DriverException("Error Disconnecting Device", ex);
            }
            finally
            {
                //Do nothing here. This means communication to the device is broken
            }

        }

        // Attempt to leave 'Serial Mode' by sending the command "FFxxxx".
        // To confirm that the device left 'Serial Mode' we must receive the response "END"
        private static void ExitSerialMode()
        {
            try
            {
                SendCommandGetResponse("FFxxxx", "END", 500, 5);
                _ConnectionState = ConnectionStates.Disconnected;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error Waking Device From Sleep Mode", ex);
            }
        }

        /// <summary>
        /// Puts the device into sleep mode
        /// </summary>
        public static void EnterSleepMode()
        {
            try
            {
                if (_ConnectionState != ConnectionStates.SerialMode)
                {
                    string txt = "Cannot put device in Sleep Mode from " + _ConnectionState.ToString() + " mode.\n" +
                        "Device must be in Serial Mode first.";

                    throw new ASCOM.InvalidOperationException(txt);
                }
                else
                {
                    SendCommandGetResponse("FSLEEP", "ZZZ", 750, 4);
                    _ConnectionState = ConnectionStates.Sleeping;
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw new ASCOM.DriverException("Unable To Enter Sleep Mode.", ex);
            }
        }


        /// <summary>
        /// Wake the device up from sleep mode.
        /// </summary>
        public static void WakeUpDevice()
        {
            try
            {
                SendCommandGetResponse("FWAKEx", "WAKE", 750, 4);
                _ConnectionState = ConnectionStates.SerialMode;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error exiting Sleep mode.", ex);
            }
        }

        /// <summary>
        /// Enter Temperature Compensation Mode A or B based on which is selected by the 
        /// ActiveTempCompMode property.
        /// </summary>
        public static void EnterTempCompMode()
        {
            char mode = DeviceSettings.ActiveTempCompMode;
            switch (mode)
            {
                case 'A':
                    SendCommandGetResponse("FAxxxx", "A", 500, 5);
                    break;
                case 'B':
                    SendCommandGetResponse("FBxxxx", "B", 500, 5);
                    break;
                default:
                    throw new ASCOM.InvalidValueException("\nTemp Comp Mode", mode.ToString(), "A or B");
            }
            _ConnectionState = ConnectionStates.TempCompMode;
            ReceivingUnsolicited = true;            
            _CurrentPosition = 0;
            _CurrentTemp = 0;

            //Wait for five seconds for a temp and position then throw exception
            TimeSpan FiveSec = new TimeSpan(0, 0, 5);
            DateTime Start = DateTime.Now;
            if (DeviceSettings.TempProbePresent)
            {
                while (CurrentPosition == 0 || CurrentTemp == 0)
                {
                    if (DateTime.Now - Start > FiveSec)
                    {
                        ReceivingUnsolicited = false;
                        _ConnectionState = ConnectionStates.SerialMode;
                        throw new ASCOM.DriverException("Device Did Not Transmit Temp and Pos Data In Temp Comp Mode");
                    }
                }
            }
            else
            {
                while (CurrentPosition == 0)
                {
                    if (DateTime.Now - Start > FiveSec)
                    {
                        ReceivingUnsolicited = false;
                        _ConnectionState = ConnectionStates.SerialMode;
                        throw new ASCOM.DriverException("Device Did Not Transmit Temp and Pos Data In Temp Comp Mode");
                    }
                }
            }
        }

        /// <summary>
        /// Exit Temperature Compensation Mode
        /// </summary>
        public static void ExitTempCompMode()
        {
            ReceivingUnsolicited = false;
            ConnectAndEnterSerialMode();        //This will set the connection state properly                                
        }

        /// <summary>
        /// Get the Firmware Version and Device Type from the connected device
        /// </summary>
        /// <returns> Returns a String array[FirmVersion, DeviceType] </returns>
        public static string[] GetFVandDT()   //Firmware Version & Device Type
        {
            try
            {
                string resp = "";
                //resp = SendCmd("FVxxxx", 500, ExpectResponse, ".");
                resp = SendCommandGetResponse("FVxxxx", ".", 700, 2);
                int i = resp.IndexOf(".");
                string version = resp.Substring(i - 1, 4);
                string type = "";
                if (resp[i + 4] == '1')
                {
                    type = "TCF-S";
                }
                else if (resp[i + 4] == '3')
                {
                    type = "TCF-S3";
                }
                else
                {
                    throw new ASCOM.DriverException("\n Failed to get device type. Device type was not a 1 or a 3.\n" +
                    "Response from device was: " + resp + "\n");
                }
                string[] ReturnVals = { version, type };
                return ReturnVals;

            }
            catch (Exception Ex)
            {
                throw new DriverException("Error retrieving firmware version and device type", Ex);
            }

        }

    }   
}
