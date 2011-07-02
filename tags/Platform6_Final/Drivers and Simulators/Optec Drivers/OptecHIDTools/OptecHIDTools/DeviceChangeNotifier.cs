using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel;
using Optec;
namespace OptecHIDTools
{
    [Guid("53D8BD52-86FC-4b93-8917-555788FC13D8")]
    public class DeviceChangeNotifier : Form
    {

        #region Constants
        private const Int32 DBT_DEVICEARRIVAL = 0X8000;
        private const Int32 DBT_DEVICEREMOVECOMPLETE = 0X8004;
        #endregion Constants

        public delegate void DeviceNotifyDelegate(Message msg);
        private static DeviceChangeNotifier mInstance;
        private static System.Guid HIDGuid;
        private static IntPtr DeviceNotificationHandle;
        private static bool _Registered = false;
        private static int InstanceCounter = 0;



        static DeviceChangeNotifier()
        {
            EventLogger.LogMessage("Creating instance of DeviceChangeNotifier. Instance Number " +
                InstanceCounter.ToString(), TraceLevel.Info);
            Interlocked.Increment(ref InstanceCounter);
        }
 
        internal static bool Registered
        {
            get
            {
                return _Registered;
            }
            set
            {
                _Registered = value;
            }
        }


        internal static void Start(System.Guid guid)
        {
            try
            {
                EventLogger.LogMessage("Starting DeviceChangeNotifier Thread", TraceLevel.Info);
                if (mInstance != null) return;
                HIDGuid = guid;
                Thread t = new Thread(runForm);
                t.SetApartmentState(ApartmentState.STA);
                t.IsBackground = true;
                t.Start();
                while (mInstance == null)
                {
                    Application.DoEvents();
                }
                EventLogger.LogMessage("DeviceChangeNotifier Thread Started. Instance of notifier form created successfully!", TraceLevel.Info);
            }
            catch (Exception ex)
            {
                _Registered = false;
                EventLogger.LogMessage(ex);
                throw;
            }
        }

        internal static void runForm()
        {
            Application.Run(new DeviceChangeNotifier());
            mInstance = null;
        }

        internal static void RegisterForNotifications()
        {
            try
            {
                if (_Registered == false)
                {
                    _Registered = true;
                    mInstance.Invoke(new MethodInvoker(mInstance.Register));
                }
                else return;
            }
            catch (Exception ex)
            {
                _Registered = false;
                EventLogger.LogMessage(ex);
                throw new ApplicationException("Error Registering For Notifications", ex);
            }
        }

        internal static void UnregisterForNotifications()
        {
            EventLogger.LogMessage("Unregistering for Device Change Notifications", TraceLevel.Info);
            Setup_API_Wrappers.UnregisterDeviceNotification(DeviceNotificationHandle);
            EventLogger.LogMessage("Unregisteration complete!", TraceLevel.Info);
        }

        private void Register()
        {
            try
            {
                EventLogger.LogMessage("Registering for Device Change Notifications", TraceLevel.Info);
                Setup_API_Wrappers.DeviceBroadcastInterface devBDI = new Setup_API_Wrappers.DeviceBroadcastInterface();
                IntPtr devBDI_Buffer;

                int size = 0;
                size = Marshal.SizeOf(devBDI);
                devBDI.Size = size;
                devBDI.DeviceType = Setup_API_Wrappers.DBT_DEVTYP_DEVICEINTERFACE;
                devBDI.Reserved = 0;
                devBDI.ClassGuid = HIDGuid;

                devBDI_Buffer = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(devBDI, devBDI_Buffer, true);
                DeviceNotificationHandle = Setup_API_Wrappers.RegisterDeviceNotification(this.Handle, devBDI_Buffer, 0);
                
                Marshal.FreeHGlobal(devBDI_Buffer);
                EventLogger.LogMessage("Registeration complete!", TraceLevel.Info);
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw;
            }
        }

        internal static void Stop()
        {
            if (mInstance == null) return;
            mInstance.Invoke(new MethodInvoker(mInstance.endForm));
        }
 
        private void endForm()
        {
            EventLogger.LogMessage("Closing DeviceChangeNotification Form", TraceLevel.Info);
            this.Close();
        }

        protected override void SetVisibleCore(bool value)
        {
            // Prevent window getting visible
            if (mInstance == null) CreateHandle();
            mInstance = this;
            value = false;
            base.SetVisibleCore(value);
        }

        protected override void WndProc(ref Message m)
        {
            // Handle WM_DEVICECHANGE
            if (m.Msg == 0x219)
            {
                if(mInstance != null) mInstance.OnDeviceChange(m);
            }
            base.WndProc(ref m);
        }

        private void OnDeviceChange(Message message)
        {
            if (message.WParam.ToInt32() == DBT_DEVICEARRIVAL)
            {
                
                // A device has been attached
                EventLogger.LogMessage("WndProc message received - DEVICE ARRIVAL", TraceLevel.Info);
                string path = GetDevicePath(message);
                HIDMonitor.AttachDevice(path);
                //OptecHID.RefreshDeviceList(true);
            }
            else if (message.WParam.ToInt32() == DBT_DEVICEREMOVECOMPLETE)
            {
                // A device has been removed
                EventLogger.LogMessage("WndProc message received - DEVICE REMOVAL", TraceLevel.Info);
                string path = GetDevicePath(message);
                if (path != "") HIDMonitor.SetDeviceDisconnected(path); 
            }
        }

        private string GetDevicePath(Message m)
        {
            //if (WatchList.Count == 0) return false;
            Int32 stringSize;
            HID_API_Wrapers.DEV_BROADCAST_DEVICEINTERFACE_1 devBroadcastDeviceInterface = new HID_API_Wrapers.DEV_BROADCAST_DEVICEINTERFACE_1();
            HID_API_Wrapers.DEV_BROADCAST_HDR devBroadcastHeader = new HID_API_Wrapers.DEV_BROADCAST_HDR();

            // The LParam parameter of Message is a pointer to a DEV_BROADCAST_HDR structure.

            Marshal.PtrToStructure(m.LParam, devBroadcastHeader);

            if ((devBroadcastHeader.dbch_devicetype == Setup_API_Wrappers.DBT_DEVTYP_DEVICEINTERFACE))
            {
                // The dbch_devicetype parameter indicates that the event applies to a device interface.
                // So the structure in LParam is actually a DEV_BROADCAST_INTERFACE structure, 
                // which begins with a DEV_BROADCAST_HDR.

                // Obtain the number of characters in dbch_name by subtracting the 32 bytes
                // in the strucutre that are not part of dbch_name and dividing by 2 because there are 
                // 2 bytes per character.

                stringSize = System.Convert.ToInt32((devBroadcastHeader.dbch_size - 32) / 2);

                // The dbcc_name parameter of devBroadcastDeviceInterface contains the device name. 
                // Trim dbcc_name to match the size of the String.         

                devBroadcastDeviceInterface.dbcc_name = new Char[stringSize + 1];

                // Marshal data from the unmanaged block pointed to by m.LParam 
                // to the managed object devBroadcastDeviceInterface.

                Marshal.PtrToStructure(m.LParam, devBroadcastDeviceInterface);

                // Store the device name in a String.

                String DeviceNameString = new String(devBroadcastDeviceInterface.dbcc_name, 0, stringSize);
                return DeviceNameString;
            }
            return "";
        }

    }
    
}
