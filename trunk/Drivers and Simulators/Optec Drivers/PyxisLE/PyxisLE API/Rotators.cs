using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using OptecHIDTools;
using System.Windows.Forms;

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

        public Rotators()
        {
           // MessageBox.Show("Creating");
            Trace.WriteLine("*******************************************************************");
            Trace.WriteLine("**************FilterWheel API IS IN USE****************************");
            HIDMonitor.HIDAttached += new EventHandler(HIDMonitor_HIDAttached);
            HIDMonitor.HIDRemoved += new EventHandler(HIDMonitor_HIDRemoved);
        }

        void HIDMonitor_HIDRemoved(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void HIDMonitor_HIDAttached(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void RefreshRotatorList()
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

        public ArrayList RotatorList
        {
            get
            {
                RefreshRotatorList();
                return DetectedRotators;
            }
        }
    }

 

}
