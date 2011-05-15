using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OptecHIDTools
{
    [Guid("82E53E81-97AA-4b2a-9CFD-5BCDDC0BA8E4")]
    public class DeviceListChangedArgs : EventArgs
    {
        private string pid = "";
        private string vid = "";
        private string serialNumber = "";

        public DeviceListChangedArgs(string PID)
        {
            pid = PID;
        }
        public DeviceListChangedArgs(string PID, string VID)
        {
            pid = PID;
            vid = VID;
        }
        public DeviceListChangedArgs(string PID, string VID, string SerialNumber)
        {
            pid = PID;
            vid = VID;
            serialNumber = SerialNumber;
        }

        public string SerialNumber
        {
            get { return serialNumber; }
        }
        public string PID
        {
            get { return pid; }
        }
        public string VID
        {
            get { return vid; }
        }
    }
}
