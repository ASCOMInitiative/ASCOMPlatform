using System;

namespace ASCOM.DynamicRemoteClients
{
    public class AlpacaConfiguredDevice
    {
        public AlpacaConfiguredDevice() { }

        public AlpacaConfiguredDevice(string deviceName,string deviceType,int deviceNumber,string uniqueID)
        {
            DeviceName = deviceName;
            DeviceType = deviceType;
            DeviceNumber = deviceNumber;
            UniqueID = uniqueID;
        }

        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public int DeviceNumber { get; set; }
        public string UniqueID { get; set; }
    }
}
