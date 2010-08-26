using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using OptecHIDTools;
using System.Windows.Forms;
using System.Threading;
using OptecLogging;

namespace PyxisLE_API
{
    [Guid("570703DB-20B5-4563-84E3-CF8C4E2574B5")]
    public class Rotators
    {
        
        private ArrayList DetectedRotators = new ArrayList();

        public event EventHandler RotatorAttached;
        public event EventHandler RotatorRemoved;

        internal const string ROTATOR_PID = "85B6";
        internal const string OPTEC_VID = "10C4";

        internal const byte REPORTID_OUTPUT_CLEAR_ERROR = 1;
        internal const byte REPORTID_INPUT_DEVICE_DESC = 11;
        internal const byte REPORTID_INPUT_DEVICE_STATUS = 10;
        internal const byte REPORTID_FEATURE_DO_MOTION = 20;

        internal const byte MOTION_OPCODE_DOHOME = 1;
        internal const byte MOTION_OPCODE_DOMOVE = 2;
        internal const byte MOTION_OPCODE_HALT = 3;
        internal const byte MOTION_OPCODE_SET_REVERSE = 4;
        internal const byte MOTION_OPCODE_SET_NEW_SKYPA = 5;
        internal const byte MOTION_OPCODE_SET_RETURNTOLAST = 6;
        internal const byte MOTION_OPCODE_SET_DEVICETYPE = 7;
        internal const byte MOTION_OPCODE_SET_ZOFF = 8;
        
        internal const byte REPORT_TRUE = 255;
        internal const byte REPORT_FALSE = 0;

        private Thread RefreshRotatorListThread;

        public Rotators()
        {
            OptecLogger.LogMessage("*******************************************************************");
            OptecLogger.LogMessage("**************Rotator API Constructer Called***********************");
            HIDMonitor.HIDAttached += new EventHandler(HIDMonitor_HIDAttached);
            HIDMonitor.HIDRemoved += new EventHandler(HIDMonitor_HIDRemoved);

            StartRefreshingRotatorList();
        }

        void HIDMonitor_HIDRemoved(object sender, EventArgs e)
        {
            if (RefreshRotatorListThread.IsAlive)
            {
                RefreshRotatorListThread.Join();
            }
            else StartRefreshingRotatorList();

            DeviceListChangedArgs deviceInfo = (DeviceListChangedArgs)e;

            if ((deviceInfo.PID == ROTATOR_PID) && (deviceInfo.VID == OPTEC_VID))
            {
                OptecLogger.LogMessage("Rotator Removed - Serial Number = " + deviceInfo.SerialNumber);
                TriggerAnEvent(RotatorRemoved);
            }
        }

        void HIDMonitor_HIDAttached(object sender, EventArgs e)
        {
            if (RefreshRotatorListThread.IsAlive)
            {
                RefreshRotatorListThread.Join();
            }
            else StartRefreshingRotatorList();

            DeviceListChangedArgs deviceInfo = (DeviceListChangedArgs)e;
            if ((deviceInfo.PID == ROTATOR_PID) && (deviceInfo.VID == OPTEC_VID))
            {
                TriggerAnEvent(RotatorAttached);
                OptecLogger.LogMessage("Rotator Attached - Serial Number = " + deviceInfo.SerialNumber);
            }
        }

        private void StartRefreshingRotatorList()
        {
            ThreadStart ts = new ThreadStart(RefreshRotatorList);
            RefreshRotatorListThread = new Thread(ts);
            RefreshRotatorListThread.Start();
        }

        private void RefreshRotatorList()
        {
            try
            {
                DetectedRotators.Clear();
                List<HID> detectedHIDs = HIDMonitor.DetectedHIDs;
                detectedHIDs = detectedHIDs.Where(d => d.PID_Hex == ROTATOR_PID).ToList();
                detectedHIDs = detectedHIDs.Where(d => d.DeviceIsAttached == true).ToList();
                foreach (HID x in detectedHIDs)
                {

                    DetectedRotators.Add(new Rotator(x));
                }
            }
            catch (Exception)
            { 
                throw;
            }
        }

        public ArrayList RotatorList
        {
            get
            {
                if (RefreshRotatorListThread.IsAlive) RefreshRotatorListThread.Join();
                return DetectedRotators;
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
                OptecLogger.LogMessage("EndAsyncEvent Method called");
                invokedMethod.EndInvoke(iar);
            }
            catch
            {
                // Handle any exceptions that were thrown by the invoked method
                OptecLogger.LogMessage("An event listener went kaboom!");
                Console.WriteLine("An event listener went kaboom!");
            }
        }
    }

 

}
