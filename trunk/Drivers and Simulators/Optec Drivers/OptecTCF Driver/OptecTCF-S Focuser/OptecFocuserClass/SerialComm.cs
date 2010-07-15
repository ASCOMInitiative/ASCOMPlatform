using System;
using System.IO.Ports;
using System.Diagnostics;


namespace ASCOM.OptecTCF_S
{

    partial class OptecFocuser
    {
        // This part of the class handles the serial communication between the device and PC
        public static SerialPort mySerialPort;
        
        private static void PreparePort(string CmdToSend, int Timeout, int Attempts)
        {
            // Verify the port is open
            try
            {
                if (!mySerialPort.IsOpen) throw new InvalidOperationException("Attempted To Send Command With Port Closed");

                // Verify the input paramaters are acceptable
                if (CmdToSend.Length != 6) throw new ASCOM.InvalidValueException("CmdToSend", CmdToSend, "Must Be 6 Characters In Length");
                if (Timeout < 0) throw new ASCOM.InvalidValueException("Timeout", Timeout.ToString(), "Must Be Greater Than -1");
                if (Attempts < 1) throw new ASCOM.InvalidValueException("Attempts", Attempts.ToString(), "Must Be At Least 1");

                // Prepare For Send and Receive
                mySerialPort.ReadTimeout = Timeout;
                mySerialPort.DiscardOutBuffer();
                mySerialPort.DiscardInBuffer();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string SendCommandGetResponse(string CmdToSend, string ExpectedResponse, int Timeout, int Attempts)
        {
            lock (LockObject)
            {
                //Prepare the port and verify correct paramaters
                try
                {
                    Debug.WriteLine("Entered SendCommandGetResponse");
                    PreparePort(CmdToSend, Timeout, Attempts);
                }
                catch (Exception ex)
                {
                    throw ex;
                }


                string Received = "";


                bool success = false;


                for (int i = 0; i < Attempts; i++)
                {
                    // Send the command
                    try
                    {
                        mySerialPort.Write(CmdToSend);
                    }
                    catch (Exception ex)
                    {
                        throw new ASCOM.DriverException("Unexpected Problem Sending Data", ex);
                    }

                    // Read the Response
                    try
                    {
                        Debug.WriteLine("Read Attempt: " + i.ToString());
                        try
                        {
                            Received = mySerialPort.ReadLine();
                            if (!Received.Contains(ExpectedResponse))
                            {
                                if (Received.Contains("ER=1"))
                                {
                                    throw new ER1_DeviceException("ER=1");
                                }
                                else
                                {
                                    throw new TimeoutException();
                                }
                            }
                            else
                            {
                                success = true;
                                break;
                            }
                        }
                        catch (TimeoutException) {/*Do nothing here*/ }
                        catch (ER1_DeviceException)
                        {
                            throw new ER1_DeviceException("ER=1");
                        }
                    }
                    catch (ER1_DeviceException ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        // An unknown problem occured
                        throw new ASCOM.DriverException("An Unknown Problem Occured While Waiting For Reponse", ex);
                    }
                }
                if (!success)
                {
                    throw new TimeoutException("Completed " + Attempts + " attempts without success in 'SendCommandGetResponse'");
                }
                else return Received; 
            }
        }

        private static string SendCommandGetResponse(string CmdToSend, int Timeout, int Attempts)
        {

            lock (LockObject)
            {
                //Prepare the port and verify correct paramaters
                try
                {
                    Debug.WriteLine("Entered SendCommandGetResponse");
                    PreparePort(CmdToSend, Timeout, Attempts);
                }
                catch (Exception ex)
                {
                    throw ex;
                }


                string Received = "";


                bool success = false;


                for (int i = 0; i < Attempts; i++)
                {
                    // Send the command
                    try
                    {
                        mySerialPort.Write(CmdToSend);
                    }
                    catch (Exception ex)
                    {
                        throw new ASCOM.DriverException("Unexpected Problem Sending Data", ex);
                    }


                    // Read the Response
                    try
                    {
                        Debug.WriteLine("Read Attempt: " + i.ToString());
                        try
                        {
                            Received = mySerialPort.ReadLine();
                            success = true;
                            break;
                        }
                        catch (TimeoutException) {/*Do nothing here*/ }
                    }
                    catch (Exception ex)
                    {
                        // An unknown problem occured
                        throw new ASCOM.DriverException("An Unknown Problem Occured While Waiting For Reponse", ex);
                    }
                }
                if (!success)
                {
                    throw new TimeoutException("Completed " + Attempts + " attempts without success in 'SendCommandGetResponse'");
                }
                else return Received;
            }

            
        }

    }

    public class ER1_DeviceException : ASCOM.DriverException
    {

        /// <summary>

        /// Default public constructor for exception to be thrown when the driver receives ER1 
        /// from the device
        /// </summary>

        public ER1_DeviceException(string message)
            : base(message, ErrorCodes.NoTempProbe) { }
        public ER1_DeviceException(string message, System.Exception inner)
            : base(message, ErrorCodes.NoTempProbe, inner) { }
    }
    public class ErrorCodes
    {
        public const int NoTempProbe = unchecked((int)0x80040407);
       // public const int InvalidPrereqs = unchecked((int)0x80040408);
       // public const int PortNotSelectedException = unchecked((int)0x80040409);
       // public const int InvalidResponse = unchecked((int)0x80040408);
    }
}