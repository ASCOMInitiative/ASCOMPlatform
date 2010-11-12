using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using Optec;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Globalization;

namespace Pyxis_Rotator_Control
{
    class OptecPyxis
    {

        public enum DeviceStates { Disconnected, Connected, Sleep }
        public enum SteppingModes { HalfStep, FullStep }
        public enum DeviceTypes { TwoInch, ThreeInch }
        private static DeviceStates currentDeviceState;
        private static DeviceTypes currentDeviceType = DeviceTypes.TwoInch;

        public static event EventHandler ConnectionEstablished;
        public static event EventHandler ConnectionTerminated;
        public static event EventHandler MotionStarted;
        public static event EventHandler MotionCompleted;
        public static event EventHandler MotionHalted;
        public static event EventHandler ErrorOccurred;

        private static DateTime moveStartTime;
        private static TimeSpan moveTotalTime;
        private static int targetDevicePosition = 0;
        private static int currentDevicePosition = 0;
        private static string firmwareVersion = "";
        private static int currentDirection = 0;
        private static bool canHalt = false;
        private static bool HaltNOW = false;
        private static Thread MotionMonitorThread;
        private static int StepsMoved = 0;

        private static SerialPort mySerialPort;
        private static int mGuessDirection = 0;
        private static volatile bool isHoming = false;
        private static volatile bool isMoving = false;

        private static object ComLock = new object();

        public const int CCW = 1;
        public const int CW = 0;

        /// <summary>
        ///  STATIC CONTRUCTOR FOR OptecPyxis CLASS
        /// </summary>
        static OptecPyxis()
        {
            currentDeviceState = DeviceStates.Disconnected;
#if DEBUG
            //  EventLogger.LoggingLevel = TraceLevel.Info;
#endif
        }

        #region Private methods for OptecPyxic class

        /// <summary>
        /// This method will attempt to get a response from the device.
        /// It will set the state based on the response and also return the current state.
        /// </summary>
        /// <returns></returns>
        private static DeviceStates checkDeviceState()
        {
            if (mySerialPort == null) return DeviceStates.Disconnected;
            if (!mySerialPort.IsOpen)
            {
                currentDeviceState = DeviceStates.Disconnected;
                return DeviceStates.Disconnected;
            }
            else
            {
                // 1. Check if it's already moving.
                if (isMoving || isHoming)
                {
                    return DeviceStates.Connected;
                }

                // 2. Check if it responds
                try
                {
                    mySerialPort.DiscardInBuffer();
                    mySerialPort.DiscardOutBuffer();
                    mySerialPort.ReadTimeout = 1000;
                    mySerialPort.Write("CCLINK");
                    string resp = mySerialPort.ReadLine();
                    if (resp.Contains("!"))
                    {
                        currentDeviceState = DeviceStates.Connected;
                        return DeviceStates.Connected;
                    }
                    else if (resp.Contains("S"))
                    {
                        currentDeviceState = DeviceStates.Sleep;
                        return DeviceStates.Sleep;
                    }
                    else
                    {
                        throw new ApplicationException("Unexpected response from Pyxis. Response was " + resp);
                    }


                }
                catch (System.TimeoutException)
                {
                    Disconnect();   // Close the port
                    currentDeviceState = DeviceStates.Disconnected;
                    return DeviceStates.Disconnected;
                }

            }
        }

        /// <summary>
        /// Checks if the device is in motion.
        /// Make sure the port is open before calling this method.
        /// </summary>
        /// <returns></returns>
        private static bool checkIfInMotion()
        {

#if DEBUG
            // 1. Make sure the Port is open
            Debug.WriteLine("Checking if in motion");
            if (!mySerialPort.IsOpen) throw new ApplicationException("The Serial port must be opened before calling CheckIfInMotion");
#endif
            // 2. Clear the receive buffer
            mySerialPort.DiscardInBuffer();
            mySerialPort.DiscardOutBuffer();
            System.Threading.Thread.Sleep(50);
            try
            {
                if (mySerialPort.BytesToRead > 0)
                {
                    return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Creates (if necessary) and opens(if necessary) an instance of mySerialPort based on the COM port name setting in the xml file.
        /// </summary>
        private static void openSerialPort()
        {
            // If the port is already exists, make sure it's open
            if (mySerialPort == null)
            {
                mySerialPort = new SerialPort(XMLSettings.SavedSerialPortName, 19200, Parity.None, 8, StopBits.One);
                mySerialPort.NewLine = "\n\r";
            }

            if (!mySerialPort.IsOpen)
            {
                mySerialPort.Open();
            }

        }

        /// <summary>
        /// Sends the CVxxxx command to the device to get the firmware version
        /// </summary>
        private static void getFirmwareVersion()
        {

#if DEBUG
            // 1. Check for connected
            if (currentDeviceState != DeviceStates.Connected)
            {
                throw new ApplicationException("Device must be connected in order to change PA");
            }

            // 2. Check if Moving
            if (isMoving || isHoming)
            {
                throw new ApplicationException("Attempted to get firmware version while device is moving or Homing!");
            }
#endif
            // 3. Send the command to move
            string cmd = "CVxxxx";
            mySerialPort.DiscardInBuffer();
            mySerialPort.DiscardOutBuffer();
            mySerialPort.Write(cmd);
            mySerialPort.ReadTimeout = 500;
            try
            {
                string resp = mySerialPort.ReadLine();
                if (resp.Contains(".")) firmwareVersion = resp;
                else firmwareVersion = "V.old";
            }
            catch (TimeoutException)
            {
                firmwareVersion = "V.old";
            }

        }


        /// <summary>
        /// Call this when Disconnecting to close the port and free up the resources it uses.
        /// </summary>
        private static void closeSerialPortandDispose()
        {
            if (mySerialPort == null) return;
            else
            {
                if (mySerialPort.IsOpen)
                    mySerialPort.Close();
                mySerialPort.Dispose();
                mySerialPort = null;
            }
        }

        /// <summary>
        /// Send the CGETPA command to the device to get the current device position angle
        /// </summary>
        /// <returns></returns>
        private static int refreshDevicePosition()
        {
#if DEBUG
            // 1. Make sure the Port is open
            if (!mySerialPort.IsOpen) throw new ApplicationException("The Serial port must be opened before calling CheckIfInMotion");
#endif
            // 2. Make sure the device is in the connected state
            if (currentDeviceState != DeviceStates.Connected)
            {
                throw new ApplicationException("The device must be in the Connected state in order to get the current position angle.");
            }

            // 3. Finally, try to get the position
            try
            {
                mySerialPort.DiscardInBuffer();
                mySerialPort.DiscardOutBuffer();
                mySerialPort.Write("CGETPA");
                string resp = mySerialPort.ReadLine();
                if (resp.Length != 3)
                    throw new ApplicationException("Incorrect response received from device");
                else
                {
                    // 4.0 Parse the response into a position
                    int pos = int.Parse(resp, CultureInfo.InvariantCulture);
                    if (pos < 0 || pos > 359) throw new ApplicationException("Incorrect position received from device. " +
                         "Received Position = " + pos.ToString() + ", but position must be from 0 to 259.");

                    Debug.WriteLine("Setting currentDevicePostion to " + pos.ToString());
                    currentDevicePosition = pos;
                    return pos;
                }
            }
            catch
            {

                throw;
            }

        }

        /// <summary>
        /// Send the CWAKEx command to wake the device from sleep mode
        /// </summary>
        private static void wakeDevice()
        {
#if DEBUG
            // 1. Make sure the Port is open
            if (!mySerialPort.IsOpen) throw new ApplicationException("The Serial port must be opened before calling CheckIfInMotion");
#endif
            // 2. Send the command to wake up
            mySerialPort.DiscardInBuffer();
            mySerialPort.DiscardOutBuffer();
            mySerialPort.Write("CWAKUP");
            mySerialPort.ReadTimeout = 1000;

            try
            {
                // 3. Check that the correct response was received
                string resp = mySerialPort.ReadLine();
                if (resp.Contains("!"))
                {
                    currentDeviceState = DeviceStates.Connected;
                    return;
                }
                else throw new ApplicationException("Device did not wake up properly.");
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw;
            }
        }

        /// <summary>
        /// Check the firmware version to see if the rotator is capable of halting during a move.
        /// Versions less than 1.42 Can NOT Halt
        /// Versions 1.45 through 2.99 Can Halt
        /// Versions 3.0 through 3.2 can NOT Halt
        /// Versions 3.3 and greater can Halt
        /// </summary>
        private static void checkIfCanHalt()
        {
            double firmwareDouble = 0;
            bool FirmwareIsDouble = double.TryParse(firmwareVersion,NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out firmwareDouble);
            if (FirmwareIsDouble)
            {
                if (firmwareDouble <= 1.42) canHalt = false;
                else if (firmwareDouble > 1.42 && firmwareDouble < 3.0) canHalt = true;
                else if (firmwareDouble >= 3.0 && firmwareDouble <= 3.2) canHalt = false;
                else if (firmwareDouble > 3.2) canHalt = true;
                else
                {
                    EventLogger.LogMessage("Unrecognized firmware version detected while attempting to check if device can halt.",
                        TraceLevel.Warning);
                    canHalt = false;
                }
            }
            else canHalt = false;
        }

        /// <summary>
        /// Determine the device type based on the firmware verison
        /// Versions less than 2.99 are 2"
        /// Versions 3.0 and greater are 3"
        /// </summary>
        private static void determineDeviceType()
        {
            double firmwareDouble = 0;
            bool FirmwareIsDouble = double.TryParse(firmwareVersion, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out firmwareDouble);
            if (FirmwareIsDouble)
            {
                if (firmwareDouble < 3) currentDeviceType = DeviceTypes.TwoInch;
                else currentDeviceType = DeviceTypes.ThreeInch;
            }
            else currentDeviceType = DeviceTypes.TwoInch;
        }

        /// <summary>
        /// Sets the move direction used for guessing the current position during a move procedure.
        /// </summary>
        /// <param name="tp">The target device position</param>
        /// <param name="cp">The current device position as last read from the device</param>
        public static void setMoveDirection(int tp, int cp)
        {
            if (cp < tp)
            {
                if (cp <= 180 && tp >= 181)
                {
                    mGuessDirection = OptecPyxis.CW;
                }
                else mGuessDirection = mGuessDirection = OptecPyxis.CCW;
            }
            else if (tp < cp)
            {
                if (tp <= 180 && cp >= 181)
                {
                    mGuessDirection = OptecPyxis.CCW;
                }
                else mGuessDirection = mGuessDirection = OptecPyxis.CW;
            }
            else throw new ApplicationException("tp and cp are the same");
        }

        /// <summary>
        /// Call to move rotator to a specific Position angle
        /// Range: 0 to 359
        /// </summary>
        /// <param name="PA"></param>
        private static void goToPA(int PA)
        {
            // 1. Check for appropriate PA
            if (PA < 0 || PA > 359)
                throw new ArgumentOutOfRangeException("Cannot move to a PA less than zero or greater than 359. Requested PA = " + PA.ToString());

            //1.5 Check if the device is already moving...
            if (isMoving || isHoming) throw new ApplicationException("Device is already moving or homing.");

            // 2. Check for connected
            if (currentDeviceState != DeviceStates.Connected)
            {
                throw new ApplicationException("Device must be connected in order to change PA");
            }

            // 2.2 Make sure we are not already at the desired position
            if (PA == currentDevicePosition) return;

            // 3. Send the command to move
            string cmd = "CPA" + PA.ToString("000");
            mySerialPort.DiscardInBuffer();
            mySerialPort.DiscardOutBuffer();
            mySerialPort.Write(cmd);
            //mySerialPort.ReadTimeout = 500;
            System.Threading.Thread.Sleep(50);
            try
            {
                string resp = mySerialPort.ReadExisting();
                if (resp.Contains("!")) { }
                else if (resp.Contains("5"))
                {
                    TriggerAnEvent(ErrorOccurred, "The Device MUST be Homed");
                    return;
                }
                targetDevicePosition = PA;
                setMoveDirection(targetDevicePosition, currentDevicePosition);
                moveStartTime = DateTime.Now;
                moveTotalTime = new TimeSpan(0, 0, 0, 0, Math.Abs(currentDevicePosition - targetDevicePosition) * 1000 / 8);
                isMoving = true;

                // 5. Start the motion monitor thread.
                ThreadStart ts2 = delegate { MotionMonitor(resp, EventArgs.Empty); };
                // ThreadStart ts = new ThreadStart(MotionMonitor);
                MotionMonitorThread = new Thread(ts2);
                MotionMonitorThread.Start();

                // 6. Trigger the MotionStarted Event
                TriggerAnEvent(MotionStarted, null);
            }
            catch (TimeoutException)
            {
                throw new ApplicationException("Device did not start move");
            }



        }

        /// <summary>
        /// Request the default move direction from the device.
        /// </summary>
        /// <returns></returns>
        private static int getDirectionFlag()
        {
            // 1. Check for connected
            if (currentDeviceState != DeviceStates.Connected)
            {
                throw new ApplicationException("Device must be connected in order to getDirectionFlag");
            }

            // 1.5 Check if is moving
            if (isMoving || isHoming)
            {
                throw new ApplicationException("Cannot get Direction flag while device is moving/homing");
            }

            // 2. Send the getFlag command
            string cmd = "CMREAD";
            mySerialPort.DiscardInBuffer();
            mySerialPort.DiscardOutBuffer();
            mySerialPort.Write(cmd);
            mySerialPort.ReadTimeout = 800;

            // 3. Check that the correct response was received
            try
            {
                string resp = mySerialPort.ReadLine();
                if (resp.Contains("1"))
                {
                    currentDeviceState = DeviceStates.Connected;
                    return 1;
                }
                else if (resp.Contains("0")) return 0;
                else throw new ApplicationException("Device did not respond properly to getDirectionFlag command.");
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw;
            }


        }

        /// <summary>
        /// Sets the default direction of travel for a positive increase in PA.
        /// The default is CCW - positive moves
        /// </summary>
        /// <param name="dir"></param>
        private static void setCurrentDirection(int dir)
        {
            // 1. Check for appropriate direction value
            if (dir < 0 || dir > 1)
                throw new ApplicationException("The direction value must be set to 0 or 1. A request has been made to set the direction to " + dir.ToString());

            // 2. Check for correct connection state
            if (checkDeviceState() != DeviceStates.Connected)
            {
                throw new ApplicationException("Device must be connected in order to set the direction flag.");
            }

            // 3. Send the command to change direction flag
            string cmd = (dir == 0) ? "CD0xxx" : "CD1xxx";
            mySerialPort.DiscardInBuffer();
            mySerialPort.DiscardOutBuffer();
            mySerialPort.Write(cmd);
            mySerialPort.ReadTimeout = 800;

            // 4. This command does not solicit a response, so we pause biefly and request the direction to make sure it matches.
            System.Threading.Thread.Sleep(500);
            if (getDirectionFlag() != dir)
            {
                throw new ApplicationException("The device did not set the direction flag properly.");
            }
            else
            {
                currentDirection = dir;
                return;
            }

        }

        /// <summary>
        /// Sends the CZxxxx command to the device to set the stepping mode of the stepper motor. 
        /// </summary>
        /// <param name="mode"></param>
        private static void setHalfStep(SteppingModes mode)
        {

            // 1. Make sure the device is connected...
            if (CurrentDeviceState != DeviceStates.Connected)
            {
                throw new ApplicationException("The device must be connected to set the half step property");
            }

            // 2. Send the command
            string cmd = (mode == SteppingModes.HalfStep) ? "CZ1111" : "CZ0000";
            mySerialPort.DiscardInBuffer();
            mySerialPort.DiscardOutBuffer();
            mySerialPort.Write(cmd);
            System.Threading.Thread.Sleep(200); // Pause just to allow the device to process the command.
            // This command does not return anything so we have to assume it worked!

            // 3. Update the xml file with the new value.
            XMLSettings.SteppingMode = mode;
        }

        /// <summary>
        /// Send the CJxxxx command to the device to set the HomeOnStart property in the firmware.
        /// </summary>
        /// <param name="homeOnStart"></param>
        private static void setHomeOnStart(bool homeOnStart)
        {
            // 1. Make sure the device is connected...
            if (CurrentDeviceState != DeviceStates.Connected)
                throw new ApplicationException("The device must be connected to set the HomeOnStart property");

            // 1.2 Make sure the device type is a 3" rotator
            if (DeviceType != DeviceTypes.ThreeInch)
                throw new ApplicationException("The HomeOnStart property can only be changed for 3 inch Pyxis Rotators.");


            // 2. Send the command
            string cmd = (homeOnStart) ? "CJ0000" : "CJ1111";
            mySerialPort.DiscardInBuffer();
            mySerialPort.DiscardOutBuffer();
            mySerialPort.Write(cmd);
            System.Threading.Thread.Sleep(200); // Pause just to allow the device to process the command.
            // This command does not return anything so we have to assume it worked!

            // 3. Update the xml file with the new value.
            XMLSettings.HomeOnStart = homeOnStart;
        }

        /// <summary>
        /// This method is started when a move is initiated. It monitors the serial port
        /// and detects when the move or home routine is finished. The current position 
        /// will be updated and the MotionCompleted event will be triggered when the move
        /// is complete. The thread will automatically exit if no data is received on the 
        /// serial line for 100ms.
        /// </summary>
        private static void MotionMonitor(object sender, EventArgs e)
        {
            string buffer = sender.ToString();
            int LastBufferCount = 1;
            int ThisBufferCount = 0;
            DateTime startTime = new DateTime();
            try
            {
                lock (ComLock)
                {
                    if (!mySerialPort.IsOpen) mySerialPort.Open();
                    //System.Threading.Thread.Sleep(100);
                    // 2.0 Start the loop
                    startTime = DateTime.Now;
                    while (!HaltNOW)
                    {
                        ThisBufferCount = mySerialPort.BytesToRead;

                        if (ThisBufferCount > 10)
                        {
                            buffer += mySerialPort.ReadExisting();
                            startTime = DateTime.Now;
                        }
                        else
                        {
                            if (ThisBufferCount != LastBufferCount)
                            {
                                //Debug.Print(" i = " + DateTime.Now.Subtract(startTime).TotalMilliseconds.ToString());

                                LastBufferCount = ThisBufferCount;
                                StepsMoved = ThisBufferCount + buffer.Length;
                                startTime = DateTime.Now;
                            }
                            else
                            {
                                // If it has been more than 100 ms since the last buffer change then the move is likely finished.
                                if (DateTime.Now.Subtract(startTime).TotalMilliseconds > 200)
                                {
                                    buffer += mySerialPort.ReadExisting();
                                    if (buffer.Contains("F"))
                                    {
                                        EventLogger.LogMessage("Move Finished as Expected!", TraceLevel.Info);
                                        break;
                                    }
                                    else
                                        EventLogger.LogMessage("Device finished move but F character was never received.", TraceLevel.Warning);
                                }

                            }
                        }
                    }
                    // Check if Halt is requested...
                    if (HaltNOW)
                    {
                        HaltNOW = false;    // reset the flag

                        TimeSpan Timeout = new TimeSpan(0, 0, 2);
                        DateTime st = DateTime.Now;
                        bool success = false;
                        int bufferSizeAtHalt = buffer.Length;

                        while (DateTime.Now.Subtract(st) <= Timeout)
                        {
                            mySerialPort.Write("QQ");
                            buffer += mySerialPort.ReadExisting();
                            if (buffer.Contains("Q"))
                            {
                                success = true;
                                Debug.Print("Contains Q");
                                break;
                            }
                            else if (buffer.Contains("F"))
                            {
                                success = true;
                                Debug.Print("Contains F");
                                break;
                            }
                        }

                        if (success) TriggerAnEvent(MotionHalted, null);
                        else
                        {
                            // If the buffer size hasn't increased more than 20 motion has been stopped
                            TriggerAnEvent(MotionHalted, null);
                            if (buffer.Length - bufferSizeAtHalt <= 20)
                            {
                                // do nothing the move has stopped
                            }
                            else
                            {
                                TriggerAnEvent(ErrorOccurred, "The device did not respond to halt request");
                            }

                        }

                        //mySerialPort.ReadTimeout = 700;
                        //mySerialPort.DiscardInBuffer();
                        //mySerialPort.Write("QQQQQQ");
                        //System.Threading.Thread.Sleep(10);
                        //mySerialPort.Write("QQQQQQ");
                        //System.Threading.Thread.Sleep(100);
                        //try
                        //{
                        //    string r = mySerialPort.ReadTo("QF");
                        //    TriggerAnEvent(MotionHalted, null);
                        //}
                        //catch (TimeoutException)
                        //{
                        //    EventLogger.LogMessage("Device did not respond to Halt Command", TraceLevel.Error);
                        //}
                    }
                    else
                    {
                        // The move has finished, finally update the current position.
                        refreshDevicePosition();
                        TriggerAnEvent(MotionCompleted, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
            }
            finally
            {
                // If we don't change these here they will be stuck forever!
                isMoving = false;
                isHoming = false;
                buffer = null;

            }

        }

        /// <summary>
        /// Call this method internally to trigger an event.
        /// </summary>
        /// <param name="EH"></param>
        /// <param name="e"></param>
        private static void TriggerAnEvent(EventHandler EH, object e)
        {
            if (EH == null) return;
            var EventListeners = EH.GetInvocationList();
            if (EventListeners != null)
            {
                for (int index = 0; index < EventListeners.Count(); index++)
                {

                    var methodToInvoke = (EventHandler)EventListeners[index];
                    methodToInvoke.BeginInvoke(e, EventArgs.Empty, EndAsyncEvent, new object[] { });

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

        #region Public Methods of OptecPyxis

        /// <summary>
        /// Attempt to establish a connection to a Pyxis Rotator.
        /// </summary>
        public static void Connect()
        {
            // 1. Make sure that the Port is Open
            openSerialPort();

            //TODO 2.0 Handle the situation where the device is already moving or homing here
            while (checkIfInMotion())
            {
                System.Threading.Thread.Sleep(500);
            }


            // 3. Attempt to "Link" to the device.
            if (checkDeviceState() != DeviceStates.Connected)
            {
                if (currentDeviceState == DeviceStates.Sleep)
                {
                    wakeDevice();
                }
                else throw new ApplicationException("Communication with Pyxis has failed. Connection could not be established.");
            }

            currentDevicePosition = refreshDevicePosition();
            Debug.WriteLine("Refreshed CurrentDevicePossition - " + currentDevicePosition.ToString());
            targetDevicePosition = currentDevicePosition;
            getFirmwareVersion();
            currentDirection = getDirectionFlag();
            checkIfCanHalt();
            determineDeviceType();

            // 6. Trigger the Connection established event
            TriggerAnEvent(ConnectionEstablished, null);
        }

        /// <summary>
        /// Disconnect from the Pyxis Rotator device
        /// </summary>
        public static void Disconnect()
        {
            try
            {
                switch (currentDeviceState)
                {
                    case DeviceStates.Disconnected:
                        break;
                    case DeviceStates.Connected:
                        break;
                    case DeviceStates.Sleep:
                        wakeDevice();
                        break;
                }

                closeSerialPortandDispose();

            }
            catch
            {

            }
            finally
            {
                currentDeviceState = DeviceStates.Disconnected;
                TriggerAnEvent(ConnectionTerminated, null);
            }
        }

        /// <summary>
        /// Stop the device during a home or move procedure. 
        /// </summary>
        public static void HaltMove()
        {
            HaltNOW = true;

        }

        /// <summary>
        /// Starts a process to home the device
        /// Note: Check IsMoving to see when the process is complete
        /// </summary>
        public static void Home()
        {

            // 1. Check for connected
            if (checkDeviceState() != DeviceStates.Connected)
            {
                throw new ApplicationException("Device must be connected in home the device");
            }

            // 1.5 Check if moving
            if (isMoving)
            {
                throw new ApplicationException("Cannot home the device while it is already moving.");
            }

            // 2. Send the command to home
            string cmd = "CHOMEx";
            mySerialPort.DiscardInBuffer();
            mySerialPort.DiscardOutBuffer();
            mySerialPort.Write(cmd);
            mySerialPort.ReadTimeout = 500;
            try
            {
                mySerialPort.ReadTo("!");
            }
            catch (TimeoutException)
            {
                throw new ApplicationException("Device did not start home.");
            }

            // 3. Change the state to reflect moving...
            isHoming = true;

            // 4. Start the motion monitor
            ThreadStart ts = delegate { MotionMonitor("", EventArgs.Empty); };
            MotionMonitorThread = new Thread(ts);
            MotionMonitorThread.Start();

            // 5. Trigger the Motion Started Eventhandler
            TriggerAnEvent(MotionStarted, null);
        }

        /// <summary>
        /// Puts the device into  a low power sleep state. Call the wake up method to wake the device.
        /// </summary>
        public static void PutToSleep()
        {
            // 1. Check for connected
            if (checkDeviceState() != DeviceStates.Connected)
            {
                throw new ApplicationException("Device must be connected in order to enter sleep mode.");
            }

            // 2. Send the command to go to sleep
            string cmd = "CSLEEP";
            mySerialPort.DiscardInBuffer();
            mySerialPort.DiscardOutBuffer();
            mySerialPort.Write(cmd);
            mySerialPort.Write(cmd);
            mySerialPort.Write(cmd);
            mySerialPort.ReadTimeout = 500;
            try
            {
                string resp = mySerialPort.ReadLine();
                if (resp.Contains("S"))
                    currentDeviceState = DeviceStates.Sleep;
                else throw new TimeoutException();
            }
            catch (TimeoutException)
            {
                throw new ApplicationException("Device did not enter sleep mode.");
            }

        }

        /// <summary>
        /// Sets the delay between each step for the rotator in milliseconds.
        /// The default rate is 8.
        /// </summary>
        /// <param name="r"></param>
        public static void SetStepTime(int r)
        {
            // 1. Check for appropriate rate
            if (r < 2 || r > 12)
                throw new ArgumentOutOfRangeException("Step rate must be in the range of 2 through 12. Requested Rate = " + r.ToString());

            // 2. Check for connected
            if (checkDeviceState() != DeviceStates.Connected)
            {
                throw new ApplicationException("Device must be connected in order to change step rate");
            }

            // 3. Send the command to set step rate
            string cmd = "CTxx" + r.ToString("00");
            mySerialPort.DiscardInBuffer();
            mySerialPort.DiscardOutBuffer();
            mySerialPort.Write(cmd);
            mySerialPort.ReadTimeout = 800;

            try
            {
                // 4. Check that the correct response was received
                string resp = mySerialPort.ReadLine();
                if (resp.Contains("!"))
                {
                    XMLSettings.StepTime = r;
                    return;
                }
                else throw new ApplicationException("Device did not respond to command to set step rate");

            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw;
            }

        }

        /// <summary>
        /// WARNING: This not update the CurrentPosition of the rotator and rotator will be out of home.
        /// This moves the rotator one step in the specified direction
        /// </summary>
        public static void Derotate1Step()
        {
            // TODO: Make sure this always moves in the correct direction based on the device type
        }

        /// <summary>
        /// This will cause the rotator to travel to the device angle specified by the ParkPosition property.
        /// </summary>
        public static void ParkRotator()
        {
            goToPA(ParkPosition);
        }

        /// <summary>
        /// Sets the current rotators position as the new 0° position. 
        /// </summary>
        [Obsolete("This should no longer be used now that the SkyPA Offset is available")]
        public static void RedefineHome()
        {
            // 1. Make sure the device is connected...
            if (CurrentDeviceState != DeviceStates.Connected)
                throw new ApplicationException("The device must be connected to set the half step property");

            // 2. Verify that we are within the correct range of +/- 15°
            if (currentDevicePosition > 15 && currentDevicePosition < 345)
                throw new ApplicationException("Current Position must be in the range of +/-15 from original home to set a new home position.");

            // 3. Send the command to reset home
            string cmd = "CNxxxx";
            mySerialPort.DiscardInBuffer();
            mySerialPort.DiscardOutBuffer();
            mySerialPort.Write(cmd);
            mySerialPort.ReadTimeout = 500;
            try
            {
                string resp = mySerialPort.ReadLine();
                if (resp.Contains("F"))
                {
                    refreshDevicePosition();
                    return;
                }
                else throw new ApplicationException("Incorrect response received from New Home Request. Expected 'F', received '" + resp + "'.");
            }
            catch (TimeoutException)
            {
                throw new ApplicationException("The device did not respond to request to set new home position. Do you have the latest firmware revision?");
            }

        }

        /// <summary>
        /// Restores the stored device properties back to zero and sets the Sky PA Offset to zero
        /// </summary>
        public static void RestoreDeviceDefaults()
        {
            // 1. Make sure the device is connected...
            if (currentDeviceState != DeviceStates.Connected)
                throw new ApplicationException("The device must be connected to set the half step property");

            // 2. Send the command to reset home
            string cmd = "CRxxxx";
            mySerialPort.DiscardInBuffer();
            mySerialPort.DiscardOutBuffer();
            mySerialPort.Write(cmd);
           // XMLSettings.StepTime = 
            System.Threading.Thread.Sleep(50);
            // There is no response from this command so we can't verify its success.

        }

        /// <summary>
        /// This will set a new value for the SkyPAOffset based on the desired current sky pa and the current device pa.
        /// </summary>
        /// <param name="desiredCurrentPosition">
        /// The value of the new current position to calculate the new sky pa value from.
        /// </param>
        public static void RedefineSkyPAOffset(double desiredCurrentPosition)
        {
            //Determine the offset
            double offset = desiredCurrentPosition - refreshDevicePosition();
            if (offset > 180) offset = 360 - offset;
            else if (offset < -180) offset = 360 + offset;

            // Set the new value in the XML file.
            XMLSettings.SkyPAOffset = (int)offset;
        }


        #endregion

        #region Public Properties of OptecPyxis

        /// <summary>
        /// Gets or Sets the current default direction of travel for the Pyxis Rotator
        /// </summary>
        public static bool Reverse
        {
            get
            {
                if (DeviceType == DeviceTypes.TwoInch)
                {
                    if (currentDirection == 1) return false;
                    else return true;
                }
                else if (DeviceType == DeviceTypes.ThreeInch)
                {
                    if (currentDirection == 1) return true;
                    else return false;
                }
                else throw new ApplicationException("Invalid Device type in Reverse.Get");
            }
            set
            {
                if (currentDevicePosition != 0)
                {
                    goToPA(0);
                    System.Threading.Thread.Sleep(250);
                }
                while (isMoving) { }

                if (DeviceType == DeviceTypes.TwoInch)
                {
                    if (value)
                        setCurrentDirection(0);
                    else
                        setCurrentDirection(1);
                }
                else if (DeviceType == DeviceTypes.ThreeInch)
                {
                    if (value)
                        setCurrentDirection(1);
                    else
                        setCurrentDirection(0);
                }
                else throw new ApplicationException("Invalid Device type in Reverse.Set");
 
            }
        }

        /// <summary>
        /// This represent the position that the focuser travels to when the park method is called.
        /// </summary>
        public static int ParkPosition
        {
            get { return XMLSettings.ParkPosition; }
            set
            {
                if (value < 0 || value >= 360)
                    throw new ArgumentOutOfRangeException("Park position must be between 0 and 359 degrees");
                XMLSettings.ParkPosition = value;
            }
        }

        /// <summary>
        /// Gets the firmware version of the currently connected rotator device.
        /// </summary>
        public static string FirmwareVersion
        {
            get { return firmwareVersion; }
        }

        /// <summary>
        /// Get or set value which indicates whether the device will automatically home when power is first supplied or reset.
        /// Note: This must be set to TRUE for 2" Pyxii.
        /// </summary>
        public static bool HomeOnStart
        {
            get
            {
                if (DeviceType == DeviceTypes.ThreeInch)
                    return XMLSettings.HomeOnStart;
                else return true;
            }
            set
            {
                setHomeOnStart(value);
            }
        }

        /// <summary>
        ///  Get or set the stepping mode of the connected Rotator as HalfStepping or FullStepping.
        /// </summary>
        public static SteppingModes SteppingMode
        {
            get { return XMLSettings.SteppingMode; }
            set
            {
                setHalfStep(value);
            }
        }

        /// <summary>
        /// Indicates whether the connected rotator is capable of halting during a move.
        /// </summary>
        public static bool CanHalt
        {
            get { return canHalt; }
        }

        /// <summary>
        ///  Represents the type of Pyxis connected; either 2 inch or 3 inch
        /// </summary>
        public static DeviceTypes DeviceType
        {
            get { return currentDeviceType; }        
        }

        /// <summary>
        /// The COM port used to communicate with the Pyxis. Note: The connectionState must be Disconnected to change this property.
        /// This will store the new value in the XML file when updating.
        /// </summary>
        public static string PortName
        {
            get
            {
                return XMLSettings.SavedSerialPortName;
            }
            set
            {
                if (currentDeviceState != DeviceStates.Disconnected)
                    throw new ApplicationException("You must disconnect before attempting to change the serial port name.");
                XMLSettings.SavedSerialPortName = value;
            }
        }

        /// <summary>
        /// This number of steps per revolution for the current connected Pyxis. This is determined by the device type.
        /// </summary>
        public static int Resolution
        {
            get
            {
                if (DeviceType == DeviceTypes.TwoInch)
                    return 5280;
                else return 46080;
            }
        }

        /// <summary>
        /// The number of degrees traveled per second for the connected Pyxis when in motion. This is dependant on The Resolution and StepTime properties.
        /// </summary>
        public static double SlewRate
        {
            get
            {
                double r = 1 / ((double)Resolution * (double)XMLSettings.StepTime / (360D * 1000D));
                return Math.Round(r, 3);
            }
        }

        /// <summary>
        /// The target position of the current move adjusted by the SkyPA Offset.
        /// </summary>
        public static int AdjustedTargetPosition
        {
            get
            {
                int ap = targetDevicePosition + XMLSettings.SkyPAOffset;
                if (ap < 0)
                {
                    return 360 + ap;
                }
                else if (ap >= 360)
                {
                    return ap - 360;
                }
                else return ap;
            }
        }

        /// <summary>
        /// Indicates whether the connected rotator is currently moving.
        /// </summary>
        public static bool IsMoving
        {
            get
            {
                return isMoving;
            }
        }

        /// <summary>
        /// Indicates whether the connected rotator is currently homing.
        /// </summary>
        public static bool IsHoming
        {
            get { return isHoming; }
        }

        /// <summary>
        /// The current state of the connected Pyxis rotator
        /// </summary>
        public static DeviceStates CurrentDeviceState
        {
            get
            {
                //checkDeviceState();
                return currentDeviceState;
            }
        }

        /// <summary>
        /// The current position angle of the connected rotator adjusted by the Sky PA Offset
        /// </summary>
        public static int CurrentAdjustedPA
        {
            get
            {
                int mGuessPosition = 0;
                if (isHoming)       // Return 0 when homing
                {
                    return 0;
                }
                else if (isMoving)  // Guess the position as somewhere between current position and target position.
                {
                    // double elap = DateTime.Now.Subtract(moveStartTime).TotalSeconds;
                    // elap *= SlewRate;
                    //  elap *= .95;
                    // Debug.WriteLine("elap = " + elap.ToString());
                   
                    double elap = StepsMoved * 360 / Resolution;
                    Debug.Print("CDP = " + currentDevicePosition.ToString());
                    if (mGuessDirection == OptecPyxis.CCW)
                        mGuessPosition = currentDevicePosition + XMLSettings.SkyPAOffset + (int)elap;
                    else mGuessPosition = currentDevicePosition + XMLSettings.SkyPAOffset - (int)elap;

                    if (mGuessPosition >= 360)
                        return (mGuessPosition) % 360;
                    else if (mGuessPosition < 0)
                        return mGuessPosition + 360;
                    else return mGuessPosition;
                }
                else
                {
                    // The device is not moving...
                    int ap = currentDevicePosition + XMLSettings.SkyPAOffset;
                    if (ap < 0)
                    {
                        ap = 360 + ap;
                    }
                    else if (ap >= 360)
                    {
                        ap = ap - 360;
                    }
                    return ap;
                }
            }
            set
            {
                double tp = value - XMLSettings.SkyPAOffset;
                if (tp < 0)
                {
                    tp = 360 + tp;
                }
                else if (tp >= 360)
                {
                    tp = tp - 360;
                }
                goToPA((int)tp);
            }
        }

        #endregion
    }
}
