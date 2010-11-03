using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using Optec;
using System.ComponentModel;

namespace ASCOM.Pyxis
{
    class  OptecPyxis
    {

        public enum DeviceStates { Disconnected, Connected, Sleep, InMotion }
        public enum DeviceTypes { TwoInch, ThreeInch }
        private static DeviceStates currentDeviceState;

        public static int targetDevicePosition = 0;
        public static int currentDevicePosition = 0;
        private static string firmwareVersion = "";

        private static SerialPort mySerialPort;
        private static string portName = "COM1";
        private static bool PositionReadRequired = true;

        public const int CCW = 0;
        public const int CW = 1;

        public OptecPyxis()
        {
        }
        
        static OptecPyxis()
        {
            currentDeviceState = DeviceStates.Disconnected;
        }

        /// <summary>
        /// This method will attempt to get a response from the device.
        /// It will set the state based on the response and also return the current state.
        /// </summary>
        /// <returns></returns>
        private static DeviceStates checkDeviceState()
        {
            if (!mySerialPort.IsOpen)
            {
                currentDeviceState = DeviceStates.Disconnected;
                return DeviceStates.Disconnected;
            }
            else
            {
                // 1. Check if it's already moving.
                if (checkIfInMotion())
                {
                    currentDeviceState = DeviceStates.InMotion;
                    return DeviceStates.InMotion;
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
            if (!mySerialPort.IsOpen) throw new ApplicationException("The Serial port must be opened before calling CheckIfInMotion");
#endif
            // 2. Clear the receive buffer
            mySerialPort.DiscardInBuffer();
            mySerialPort.DiscardOutBuffer();
            System.Threading.Thread.Sleep(250);
            try
            {
                if (mySerialPort.BytesToRead > 0)
                    return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }

        private static void openSerialPort()
        {
            // If the port is already exists, make sure it's open
            if (mySerialPort == null)
            {
                mySerialPort = new SerialPort(portName, 19200, Parity.None, 8, StopBits.One);
                mySerialPort.NewLine = "\n\r";
            }

            if (!mySerialPort.IsOpen)
            {
                mySerialPort.Open();
            }

        }

        private static void getFirmwareVersion()
        {
#if DEBUG 
            // 1. Check for connected
            if (checkDeviceState() != DeviceStates.Connected)
            {
                throw new ApplicationException("Device must be connected in order to change PA");
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

        private static int getDevicePosition()
        {
#if DEBUG
            // 1. Make sure the Port is open
            if (!mySerialPort.IsOpen) throw new ApplicationException("The Serial port must be opened before calling CheckIfInMotion");
#endif
            // 2. Make sure the device is in the connected state
            if (checkDeviceState() != DeviceStates.Connected)
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
                    int pos = int.Parse(resp);
                    if(pos < 0 || pos > 359) throw new ApplicationException("Incorrect position received from device. " + 
                        "Received Position = " + pos.ToString() + ", but position must be from 0 to 259.");
                    currentDevicePosition = pos;
                    PositionReadRequired = false;
                    return pos;
                }
            }
            catch
            {
                throw;
            }
                
        }

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

        public static string PortName
        {
            get 
            {
                return portName; 
            }
            set 
            {
                if (currentDeviceState != DeviceStates.Disconnected)
                    throw new ApplicationException("You must disconnect before attempting to change the serial port name.");
                portName = value; 
            }
        }

        public static void Connect()
        {
            // 1. Make sure that the Port is Open
            openSerialPort();

            if (checkIfInMotion())
            {
                currentDeviceState = DeviceStates.InMotion;
                return;
            }


            // 3. Attempt to "Link" to the device.
            if (checkDeviceState() != DeviceStates.Connected)
            {
                if (currentDeviceState == DeviceStates.Sleep)
                {
                    wakeDevice();
                }
                else throw new NotConnectedException("Communication with Pyxis has failed. Connection could not be established.");
            }
            
            currentDevicePosition = getDevicePosition();
            targetDevicePosition = currentDevicePosition;
            getFirmwareVersion();
        }

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
                    case DeviceStates.InMotion:
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
            }
        }

        public static DeviceStates CurrentDeviceState
        {
            get { return currentDeviceState; }
        }

        public static int CurrentDevicePosition
        {
            set 
            {
                goToPA(value);
            }
            get 
            {
                if (PositionReadRequired) return getDevicePosition();
                else return currentDevicePosition;
            }
        }

        public static int CurrentAdjustedPA
        {
            get 
            {
                int ap = CurrentDevicePosition + XMLSettings.SkyPAOffset;
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

        public static int TargetDevicePosition
        {
            get { return targetDevicePosition; }
        }

        public static int AdjustedTargetPosition
        {
            get
            {
                double ap = targetDevicePosition + XMLSettings.SkyPAOffset;
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
        /// Call to move rotator to a specific Position angle
        /// Range: 0 to 359
        /// </summary>
        /// <param name="PA"></param>
        private static void goToPA(int PA)
        {
            // 1. Check for appropriate PA
            if (PA < 0 || PA > 359)
                throw new ArgumentOutOfRangeException("Cannot move to a PA less than zero or greater than 359. Requested PA = " + PA.ToString());
            
            // 2. Check for connected
            if (checkDeviceState() != DeviceStates.Connected)
            {
                throw new ApplicationException("Device must be connected in order to change PA");
            }

            // 3. Send the command to move
            string cmd = "CPA" + PA.ToString("000");
            mySerialPort.DiscardInBuffer();
            mySerialPort.DiscardOutBuffer();
            mySerialPort.Write(cmd);
            mySerialPort.ReadTimeout = 500;
            try
            {
                mySerialPort.ReadTo("!");
                PositionReadRequired = true;
            }
            catch (TimeoutException)
            {
                throw new ApplicationException("Device did not start move");
            }

            // 4. Change the state to reflect moving...
            checkDeviceState();

        }

        public static bool IsMoving
        {
            get
            {
                if (checkDeviceState() == DeviceStates.InMotion) return true;
                else return false;
            }
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
            checkDeviceState();
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

            // 2. Send the command to home
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

            // 3. Change the state to reflect moving...
            checkDeviceState();
        }

        /// <summary>
        /// Sets the step rate for the rotator device.
        /// The default rate is 8.
        /// </summary>
        /// <param name="r"></param>
        public static void SetStepRate(int r)
        {
            // 1. Check for appropriate rate
            if (r < 4 || r > 12)
                throw new ArgumentOutOfRangeException("Step rate must be in the range of 4 through 12. Requested Rate = " + r.ToString());

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
                    currentDeviceState = DeviceStates.Connected;
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
        /// This will 
        /// </summary>
        public static void RotateXxSteps(int steps, int dir)
        {
            //// 1. Check for appropriate PA
            //if (steps < 1 || steps > 9)
            //    throw new ArgumentOutOfRangeException("Cannot move to a PA less than zero or greater than 359. Requested PA = " + PA.ToString());

            //// 2. Check for connected
            //if (checkDeviceState() != DeviceStates.Connected)
            //{
            //    throw new ApplicationException("Device must be connected in order to change PA");
            //}

            //// 3. Send the command to move
            //string cmd = "CPA" + PA.ToString("000");
            //mySerialPort.DiscardInBuffer();
            //mySerialPort.DiscardOutBuffer();
            //mySerialPort.Write(cmd);
            //mySerialPort.ReadTimeout = 500;
            //try
            //{
            //    mySerialPort.ReadTo("!");
            //}
            //catch (TimeoutException)
            //{
            //    throw new ApplicationException("Device did not start move");
            //}

            //// 4. Change the state to reflect moving...
            //checkDeviceState();
        }

        /// <summary>
        /// Request the default move direction from the device.
        /// </summary>
        /// <returns></returns>
        public static int getDirectionFlag()
        {
            // 1. Check for connected
            if (checkDeviceState() != DeviceStates.Connected)
            {
                throw new ApplicationException("Device must be connected in order to getDirectionFlag");
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
        public static void setDefaultDirection(int dir)
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
            else return;

        }

        public static string FirmwareVersion
        {
            get { return firmwareVersion; }
        }

        public static bool HalfStep
        {
            get { return XMLSettings.HalfStep; }
            set
            {
                setHalfStep(value);
            }
        }

        private static void setHalfStep(bool halfStep)
        {

            // 1. Make sure the device is connected...
            if (CurrentDeviceState != DeviceStates.Connected)
            {
                throw new ApplicationException("The device must be connected to set the half step property");
            }

            // 2. Send the command
            string cmd = (halfStep) ? "CZ1111" : "CZ0000";
            mySerialPort.DiscardInBuffer();
            mySerialPort.DiscardOutBuffer();
            mySerialPort.Write(cmd);
            System.Threading.Thread.Sleep(200); // Pause just to allow the device to process the command.
            // This command does not return anything so we have to assume it worked!
            
            // 3. Update the xml file with the new value.
            XMLSettings.HalfStep = halfStep;
        }

        [Browsable(true)]
        public static bool HomeOnStart
        {
            get { return XMLSettings.HomeOnStart; }
            set
            {
                setHomeOnStart(value);
            }
        }

        private static void setHomeOnStart(bool homeOnStart)
        {
            // 1. Make sure the device is connected...
            if (CurrentDeviceState != DeviceStates.Connected)
            {
                throw new ApplicationException("The device must be connected to set the HomeOnStart property");
            }

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
                    getDevicePosition();
                    return;
                }  
                else throw new ApplicationException("Incorrect response received from New Home Request. Expected 'F', received '" + resp + "'.");
            }
            catch (TimeoutException)
            {
                throw new ApplicationException("The device did not respond to request to set new home position. Do you have the latest firmware revision?");
            }

        }
    }
}
