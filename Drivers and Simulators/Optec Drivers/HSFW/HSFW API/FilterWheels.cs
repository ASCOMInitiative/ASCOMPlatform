using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OptecHIDTools;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections;
using System.Threading;

namespace OptecHID_FilterWheelAPI
{
    [Guid("FA9E7329-1F0C-4176-996B-9E2EEABAFD85")]
    public class FilterWheels
    {
        private ArrayList DetectedFilterWheels = new ArrayList { };

        public Dictionary<char, short> FiltersPerWheel = new Dictionary<char, short>();
        private int attachedDeviceCount = 0;
        private Thread ListRefreshThread;
        public event EventHandler FilterWheelRemoved;
        public event EventHandler FilterWheelAttached;

        internal const string FILTER_WHEEL_PID                      = "82CD";
        internal const string OPTEC_VID                             = "10C4";

        internal const byte REPORTID_OUTPUT_RESET = 1;
        internal const byte REPORTID_OUTPUT_CLEARERROR = 2;
        internal const byte REPORTID_INPUT_STATUS = 10;
        internal const byte REPORTID_INPUT_DESCRIPTION = 11;
        internal const byte REPORTID_FEATURE_DOMOVE = 20;
        internal const byte REPORTID_FEATURE_DOHOME = 21;
        internal const byte REPOTRID_FEATURE_FLASHOPS = 22;
        internal const byte REPORT_TRUE = 255;
        internal const byte REPORT_FALSE = 0;

        internal const byte FLASH_OPCODE_SET_DEFAULT_NAMES          = 1;
        internal const byte FLASH_OPCODE_UPDATE_FILTER_NAME         = 2;
        internal const byte FLASH_OPCODE_READ_FILTER_NAME           = 3;
        internal const byte FLASH_OPCODE_UPDATE_WHEEL_NAME          = 4;
        internal const byte FLASH_OPCODE_READ_WHEEL_NAME            = 5;
        internal const byte FLASH_OPCODE_UPDATE_CENTERING_OFFSET    = 6;

        public FilterWheels()
        {
#if DEBUG
            System.Windows.Forms.MessageBox.Show("Creating FilterWheels");
#endif
            Trace.WriteLine("*******************************************************************");
            Trace.WriteLine("**************FilterWheel API IS IN USE****************************");
            HIDMonitor.HIDAttached += new EventHandler(HIDMonitor_HIDAttached);
            HIDMonitor.HIDRemoved += new EventHandler(HIDMonitor_HIDRemoved);
            StartRefreshingFilterWheelList();
        }
          
        // This is the event handler that is triggered when any HID
        // device is removed from the PC. When this occurs, FilterWheels
        // needs to check if the device was a filter wheel and refresh the
        // device list accordingly.
        void HIDMonitor_HIDRemoved(object sender, EventArgs e)
        {
            if (ListRefreshThread.IsAlive)
            {
                ListRefreshThread.Join();
            }
            else StartRefreshingFilterWheelList();

            DeviceListChangedArgs deviceInfo = (DeviceListChangedArgs)e;
            if ((deviceInfo.PID == FILTER_WHEEL_PID) && (deviceInfo.VID == OPTEC_VID))
            {
                TriggerAnEvent(FilterWheelRemoved);
            }
        }

        // This is the event handler that is triggered when any HID
        // device is removed from the PC. When this occurs, FilterWheels
        // needs to check if the device was a filter wheel and refresh the
        // device list accordingly.
        void HIDMonitor_HIDAttached(object sender, EventArgs e)
        {
            if (ListRefreshThread.IsAlive)
            {
                ListRefreshThread.Join();
            }
            else StartRefreshingFilterWheelList();

            DeviceListChangedArgs deviceInfo = (DeviceListChangedArgs)e;
            if ((deviceInfo.PID == FILTER_WHEEL_PID) && (deviceInfo.VID == OPTEC_VID))
            {
               TriggerAnEvent(FilterWheelAttached);
            }
        }

        // This method is used to trigger an event handeler and pass
        // execution of that handler to a new thread. This ensures that 
        // if the handler executs a long running operation, the class can 
        // still continue to function normally.
        private void TriggerAnEvent(EventHandler EH)
        {
            if (EH == null) return;
            var EventListeners = EH.GetInvocationList();
            if (EventListeners != null)
            {
                for (int index = 0; index < EventListeners.Count(); index++)
                {
                    var methodToInvoke = (EventHandler)EventListeners[index];
                    methodToInvoke.BeginInvoke(null, EventArgs.Empty, EndAsyncEvent, new object[] { });
                }
            }

        }

        // Needed for triggering events on separate threads
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

        // This method creates a new thread and calls RefreshFilterWheelList
        // to begin running on it.
        private void StartRefreshingFilterWheelList()
        {
            ThreadStart ts = new ThreadStart(RefreshFilterWheelList);
            ListRefreshThread = new Thread(ts);
            ListRefreshThread.Start();
        }

        // This method iterates through all of the HID objects that have been
        // connected since the start of execution of assembly. It then builds a 
        // list of all the HID objects that have a PID that matches that of an
        // Optec filter wheel.
        private void RefreshFilterWheelList()
        {
            
            try
            {
                DetectedFilterWheels.Clear();
                List<HID> detectedHIDs = HIDMonitor.DetectedHIDs;
                
                // First make sure VID's match Optec's VID
                detectedHIDs = detectedHIDs.Where(d => d.VID_Hex == OPTEC_VID).ToList();
                // Next make sure the PID's match that of a Filter Wheel
                detectedHIDs = detectedHIDs.Where(d => d.PID_Hex == FILTER_WHEEL_PID).ToList();
                int i = 0;
                foreach (HID x in detectedHIDs)
                {
                    DetectedFilterWheels.Add(new FilterWheel(x));
                    if(x.DeviceIsAttached) i++;
                    
                }
                attachedDeviceCount = i;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public string GetErrorMessage(short ErrorState)
        {
            string ErrorName = "ErrorState_" + ErrorState.ToString();
            string x = Resource1.ResourceManager.GetString(ErrorName);
            object y = Resource1.ResourceManager.GetObject(ErrorName);
            if (x == null) x = "No Error Message Available for: " + ErrorName;
            return x;
        }

        public int AttachedDeviceCount
        {
            get 
            {
                if (ListRefreshThread.IsAlive)
                {
                    ListRefreshThread.Join();
                }
                return attachedDeviceCount;
            }
        }

        public ArrayList FilterWheelList
        {
            get 
            {
                if (ListRefreshThread.IsAlive)
                    ListRefreshThread.Join(); ;
                return DetectedFilterWheels; 
            }
        }


    }

    
}
