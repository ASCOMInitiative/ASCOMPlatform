using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Threading;
using System.Diagnostics;
using Optec;


namespace OptecHIDTools
{
    [Guid("F9300FC5-0C77-4496-BD43-27483A85ACC1")]
    public class HID
    {   
        private string _VID_Hex = "10C4";
        private string _PID_Hex = "";
        private string _pathString = "";
        private SafeFileHandle DeviceHandle = null;
        private int _OutputReportSize = 0;
        private int _InputReportSize = 0;
        private int _FeatureReportSize = 0;
        private string _SerialNumber = "";
        private string _Manufacturer = "";
        private string _Product = "";
        private bool _DeviceIsAttached = false;
        private static int CurrentInstance = 0;
        internal int InstanceID;

        public event EventHandler DeviceRemoved;
        public event EventHandler DeviceAttached;

        internal HID()
        {
        }

        public HID( string vid, string pid, string serialNumber)
        {
            this.PID_Hex = pid;
            this.VID_Hex = vid;
            this.SerialNumber = serialNumber;
            InstanceID = CurrentInstance;
            Interlocked.Increment(ref CurrentInstance);
            EventLogger.LogMessage("Creating HID object instance. Instance number = "
                + InstanceID.ToString(), TraceLevel.Info);
        }

        internal void TriggerDeviceRemoved()
        {
            triggerAnEvent(ref this.DeviceRemoved);
            EventLogger.LogMessage( "Triggering Device Removed Event", TraceLevel.Info);
        }

        internal void TriggerDeviceAttached()
        {
            triggerAnEvent( ref this.DeviceAttached);
            EventLogger.LogMessage("Triggering Device Attached Event", TraceLevel.Info);
        }

        private void triggerAnEvent(ref EventHandler EH)
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
                EventLogger.LogMessage("An exception was caught in EndAsyncEvent method", TraceLevel.Info);
            }
        }
  
        public string VID_Hex
        {
            get { return _VID_Hex; }
            set { _VID_Hex = value; }
        }

        public string PID_Hex
        {
            get { return _PID_Hex; }
            set { _PID_Hex = value; }
        }

        public bool DeviceIsAttached
        {
            get { return _DeviceIsAttached; }
            internal set { _DeviceIsAttached = value; }
        }

        public string SerialNumber
        {
            get { return _SerialNumber; }
            set { _SerialNumber = value; }
        }

        public string Manufacturer
        {
            get
            {
                return _Manufacturer;
            }
            set
            {
                _Manufacturer = value;
            }
        }

        public string ProductDescription
        {
            get
            {
                return _Product;
            }
            set
            {
                _Product = value;
            }
        }

        internal string Path
        {
            get { return _pathString; }
            set { _pathString = value; }
        }

        internal int OutputReportSize
        {
            get { return _OutputReportSize; }
            set { _OutputReportSize = value; }
        }

        internal int InputReportSize
        {
            get { return _InputReportSize; }
            set { _InputReportSize = value; }
        }

        internal int FeatureReportSize
        {
            get { return _FeatureReportSize; }
            set { _FeatureReportSize = value; }
        }

        private SafeFileHandle OpenHandle()
        {
            SafeFileHandle pHandle = ReadWrite_API_Wrappers.CreateFile(
                _pathString,
                (ReadWrite_API_Wrappers.GENERIC_READ | ReadWrite_API_Wrappers.GENERIC_WRITE),
                ReadWrite_API_Wrappers.FILE_SHARE_READ | ReadWrite_API_Wrappers.FILE_SHARE_WRITE,
                IntPtr.Zero,
                ReadWrite_API_Wrappers.OPEN_EXISTING,
                ReadWrite_API_Wrappers.FILE_ATTRIBUTE_NORMAL | ReadWrite_API_Wrappers.FILE_FLAG_OVERLAPPED,
                0);
            EventLogger.LogMessage("A SafeFileHandle was opened to path " + _pathString, TraceLevel.Info);
            return pHandle;
        }

        public bool SendReport_Interrupt(OutputReport ReportToSend)
        {
            try
            {
                if (!IsConnected()) Connect();

                Int32 NumberOfBytesWritten = 0;
                Byte[] outputReportBuffer = null;
                Boolean Success = false;

                Array.Resize(ref outputReportBuffer, _OutputReportSize);

                if (outputReportBuffer.Length < (ReportToSend.DataToSend.Length + 1))
                    throw new ApplicationException("Data to send is larger than Output report buffer size");
                outputReportBuffer[0] = Convert.ToByte(ReportToSend.ReportID);

                int i = 1;
                foreach (Byte x in ReportToSend.DataToSend)
                {
                    outputReportBuffer[i] = x;
                    i++;
                }

                Success = ReadWrite_API_Wrappers.WriteFile(
                    DeviceHandle,
                    outputReportBuffer,
                    outputReportBuffer.Length,
                    ref NumberOfBytesWritten,
                    IntPtr.Zero);

                if (Success == false)
                {
                    string resultString = ParseWin32Error("WriteFile Error in SendReport_Interrupt");
                    EventLogger.LogMessage(resultString, TraceLevel.Warning);
                }
                return Success;
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw; 
            }
            finally
            {
                Disconnect(); ;
            }
        }

        public void ProcessFeatureReport(FeatureReport ReportToProcess)
        {
            
            try
            {
                EventLogger.LogMessage("Attempting to process Feature report", TraceLevel.Verbose);
                // Verify that the Report is not empty
                if (ReportToProcess == null)
                {
                    throw new ArgumentNullException("Feature Report: ReportToSend in ProcessFeatureReport");
                }
                // verify the data is present
                else if (ReportToProcess.DataToSend == null || ReportToProcess.DataToSend.Length == 0)
                {
                    throw new ArgumentOutOfRangeException("FeatureReport.DataToSend", "Must contain at least one value");
                }
                //Open the handle if not already opened.
                if (!IsConnected()) Connect();
                // Declare the buffers empty
                Byte[] FeatureDataBuffer = null;
                Byte[] ReceivedResponse1 = null;
                Byte[] ReceivedResponse2 = null;
                // Set the sizes of the arrays
                Array.Resize(ref FeatureDataBuffer, FeatureReportSize);
                // Set the Report ID byte
                FeatureDataBuffer[0] = Convert.ToByte(ReportToProcess.ReportID);
                // Fill in the rest of the report data
                int i = 0;
                foreach (Byte x in ReportToProcess.DataToSend)
                {
                    FeatureDataBuffer[i + 1] = x;
                    i++;
                }
                // Send the initial report
                bool success = ReadWrite_API_Wrappers.HidD_SetFeature(
                    DeviceHandle,
                    FeatureDataBuffer,
                    FeatureDataBuffer.Length);
                if (!success)   // verify that it was successful
                {
                    string eText = "Error Setting the Feature Report. " + ParseWin32Error("ProcessFeatureReport");
                    throw new ApplicationException(eText);
                }
                else
                {
                    // Retrieve the first response
                    success = ReadWrite_API_Wrappers.HidD_GetFeature(
                        DeviceHandle,
                        FeatureDataBuffer,
                        FeatureDataBuffer.Length);
                    if (!success)   //verify that it was successful
                    {
                        string eText = "Error Getting the Feature Report(Attempt 1). " + ParseWin32Error("ProcessFeatureReport");
                        throw new ApplicationException(eText);
                    }
                    else
                    {
                        // Verify that the report ID of the requested and received data match
                        if (FeatureDataBuffer[0] != Convert.ToByte(ReportToProcess.ReportID))
                        {
                            string msg = "THROWING EXCEPTION in ProcessFeatureReport: ReportID in Response1(" +
                                FeatureDataBuffer[0].ToString() + ") does not match the requested ReportID(" +
                                ReportToProcess.ReportID.ToString() + ")\n";
                            throw new ApplicationException(msg);
                        }
                        // Store the data from the first response
                        Array.Resize(ref ReceivedResponse1, FeatureDataBuffer.Length - 1);
                        Array.Copy(FeatureDataBuffer, 1, ReceivedResponse1, 0, FeatureDataBuffer.Length - 1);
                        // Retrieve the second response
                        success = ReadWrite_API_Wrappers.HidD_GetFeature(
                            DeviceHandle,
                            FeatureDataBuffer,
                            FeatureDataBuffer.Length);
                        if (!success)   //verify that it was successful
                        {
                            string eText = "THROWING EXCEPTION Error Getting the Feature Report(Attempt 2). " + ParseWin32Error("ProcessFeatureReport");
                            throw new ApplicationException(eText);
                        }
                        else
                        {
                            // Verify that the report ID of the requested and received data match
                            if (FeatureDataBuffer[0] != Convert.ToByte(ReportToProcess.ReportID))
                            {
                                string msg = "THROWING EXCEPTION in ProcessFeatureReport: ReportID in Response2(" +
                                    FeatureDataBuffer[0].ToString() + ") does not match the requested ReportID(" +
                                    ReportToProcess.ReportID.ToString() + ")\n";
                                throw new ApplicationException(msg);
                            }
                            // Store the data from the second response
                            Array.Resize(ref ReceivedResponse2, FeatureDataBuffer.Length - 1);
                            Array.Copy(FeatureDataBuffer, 1, ReceivedResponse2, 0, FeatureDataBuffer.Length - 1);
                        }
                    }
                    // Update the feature report with the received data
                    ReportToProcess.Response1 = ReceivedResponse1;
                    ReportToProcess.Response2 = ReceivedResponse2;
                    EventLogger.LogMessage("Feature Report Processed successfully", TraceLevel.Verbose);
                }               
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);    
                throw;
            }
            finally
            {
                // Close the handle used to connect
                Disconnect();
            }
            
        }

        public void SendReport_Control(OutputReport ReportToSend)
        {
            try
            {
                EventLogger.LogMessage("Attempting to send Output Report via Control Transfer", TraceLevel.Verbose);
                // Open the handle if it's not already opened
                if (!IsConnected()) Connect();  
                // Prepare the buffers and success bit
                Byte[] outputReportBuffer = null;
                Boolean Success = false;
                Array.Resize(ref outputReportBuffer, _OutputReportSize);
                // Check that we are not trying to send too much data
                if (outputReportBuffer.Length < (ReportToSend.DataToSend.Length + 1))
                    throw new ApplicationException("Data to send is larger than Output report buffer size");
                // Fill in the outputReportBuffer with the data to send
                int i = 1;
                foreach (Byte x in ReportToSend.DataToSend)
                {
                    outputReportBuffer[i] = x;
                    i++;
                } 
                // Fill in the Report ID
                outputReportBuffer[0] = Convert.ToByte(ReportToSend.ReportID);
                Success = ReadWrite_API_Wrappers.HidD_SetOutputReport(
                    DeviceHandle,
                    outputReportBuffer,
                    outputReportBuffer.Length);
                // Check the that data was sent successfully
                if (!Success)
                {
                    string resultString = ParseWin32Error("SendReport_Control");
                    throw new ApplicationException("An error occurred while sending output report.\n " + resultString);
                }
                EventLogger.LogMessage("Output Report Sent Successfully!", TraceLevel.Verbose);
            }
            catch (Exception ex) 
            {
                EventLogger.LogMessage(ex);
           
                throw; 
            }
            finally { Disconnect(); }
        }

        public byte[] RequestInputReport_Interrupt(short ReportID)
        {
            IntPtr eventObject = IntPtr.Zero;
            NativeOverlapped HidOverlapped = new NativeOverlapped();
            Byte[] inputReportBuffer = null;
            Int32 numberOfBytesRead = 0;
            Int32 result = 0;
            Boolean Success = false;
            IntPtr unManagedBuffer = IntPtr.Zero;
            IntPtr unManagedOverlapped = IntPtr.Zero;

            Array.Resize(ref inputReportBuffer, _InputReportSize);
            inputReportBuffer[0] = Convert.ToByte(ReportID);
            eventObject = ReadWrite_API_Wrappers.CreateEvent(
                IntPtr.Zero,
                false,
                false,
                String.Empty);

            HidOverlapped.OffsetLow = 0;
            HidOverlapped.OffsetHigh = 0;
            HidOverlapped.EventHandle = eventObject;

            unManagedBuffer = Marshal.AllocHGlobal(inputReportBuffer.Length);
            unManagedOverlapped = Marshal.AllocHGlobal(Marshal.SizeOf(HidOverlapped));
            Marshal.StructureToPtr(HidOverlapped, unManagedOverlapped, false);

            //SafeFileHandle readHandle = OpenHandle();
            SafeFileHandle readHandle = ReadWrite_API_Wrappers.CreateFile(
                _pathString,
                ReadWrite_API_Wrappers.GENERIC_READ,
                ReadWrite_API_Wrappers.FILE_SHARE_READ | ReadWrite_API_Wrappers.FILE_SHARE_WRITE,
                IntPtr.Zero,
                ReadWrite_API_Wrappers.OPEN_EXISTING,
                ReadWrite_API_Wrappers.FILE_FLAG_OVERLAPPED,
                0);

            Success = ReadWrite_API_Wrappers.ReadFile(
                readHandle,
                unManagedBuffer,
                inputReportBuffer.Length,
                ref numberOfBytesRead,
                unManagedOverlapped);

            if (Success)
            {
                // Response was received right away.
                return inputReportBuffer;
            }
            else
            {
                EventLogger.LogMessage(ParseWin32Error("Read File"), TraceLevel.Warning);
                //no response so now we have to wait...
                result = ReadWrite_API_Wrappers.WaitForSingleObject(eventObject, 3000);

                switch (result)
                {
                    case ReadWrite_API_Wrappers.WAIT_OBJECT_0:
                        Success = true;
                        ReadWrite_API_Wrappers.GetOverlappedResult(
                            readHandle,
                            unManagedOverlapped,
                            ref numberOfBytesRead,
                            false);
                        EventLogger.LogMessage( "\nOverlapped Result Returned\n", TraceLevel.Info);
                        break;
                    
                    case ReadWrite_API_Wrappers.WAIT_TIMEOUT:
                        inputReportBuffer = null;
                        ReadWrite_API_Wrappers.CancelIo(readHandle);
                        EventLogger.LogMessage("Timeout occured during ReadFile. " + ParseWin32Error("Read File"), TraceLevel.Info);
                        break;

                    default:
                        inputReportBuffer = null;
                        EventLogger.LogMessage("Error occured during ReadFile. " + ParseWin32Error("Read File"), TraceLevel.Info);
                        ReadWrite_API_Wrappers.CancelIo(readHandle);
                        break;

                }
                return inputReportBuffer;

            }

        }

        public void RequestInputReport_Control(InputReport Report)
        {
            try
            {
                EventLogger.LogMessage( "Requesting Input Report via Control Transfer", TraceLevel.Verbose);
                //Open the handle it is not already open
                if (!IsConnected()) Connect();  
                // Prepare the input report buffer and success bit
                Byte[] inputReportBuffer = null;
                bool Success = false;
                Array.Resize(ref inputReportBuffer, _InputReportSize + 1);
                // Set the report ID to request the correct data from the device
                inputReportBuffer[0] = Convert.ToByte(Report.ReportID);
                // Request the data
                Success = ReadWrite_API_Wrappers.HidD_GetInputReport(
                    DeviceHandle,
                    inputReportBuffer,
                    inputReportBuffer.Length + 1);
                if (Success)
                {
                    // Response was received right away.
                    Report.ReceivedData = inputReportBuffer;
                }
                else
                {
                    string resultString = ParseWin32Error("ReadInputReport_Control");
                    throw new ApplicationException("Error Receiving Input Report.\n" + resultString);
                }

                // Finally verify that the data received has the same Report ID as what we requested
                if (Report.ReceivedData[0] != Convert.ToByte(Report.ReportID))
                {
                    string msg = "Received ReportID(" +
                        Report.ReceivedData[0].ToString() +
                        ") does not match Requested Report ID(" +
                        Report.ReportID.ToString() + ")";
                    
                    throw new ApplicationException(msg);
                }
                else
                {
                    EventLogger.LogMessage("Input Report Received", TraceLevel.Verbose);
                    return;
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw;
            }

        }
        
        internal static string ParseWin32Error(string MethodName)
        {
            Int32 bytes = 0; 
            Int32 resultCode = 0; 
            String resultString = ""; 
            
            resultString = new String(Convert.ToChar( 0 ), 129 ); 
            
            // Returns the result code for the last API call.
            
            resultCode = System.Runtime.InteropServices.Marshal.GetLastWin32Error(); 
            
            // Get the result message that corresponds to the code.

            Int64 temp = 0;          
            bytes = Win32Errors.FormatMessage(Win32Errors.FORMAT_MESSAGE_FROM_SYSTEM, ref temp, resultCode, 0, resultString, 128, 0); 
            
            // Subtract two characters from the message to strip the CR and LF.
            
            if ( bytes > 2 ) 
            { 
                resultString = resultString.Remove( bytes - 2, 2 ); 
            }             
            // Create the String to return.
            
            resultString = "\r\n" + "An Error Occured In: " + MethodName + "\r\n" + "Result = " + resultString + "\r\n"; 
            
            return resultString; 
        }

        private bool IsConnected()
        {
            if ((DeviceHandle == null) ||
                    (DeviceHandle.IsClosed == true) ||
                    (DeviceHandle.IsInvalid == true))
            {
                return false;
            }
            else return true;

        }

        private void Connect()
        {
            DeviceHandle = OpenHandle();
            
        }

        private void Disconnect()
        {
            DeviceHandle.Close();
            DeviceHandle = null;
            EventLogger.LogMessage("SafeFileHandle Closed", TraceLevel.Verbose);
            //Debug.WriteLine("Handle Closed!");
        }
        
    }

    

}
