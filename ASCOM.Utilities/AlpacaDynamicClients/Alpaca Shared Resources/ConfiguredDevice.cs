using System.Collections.Concurrent;
using System.Threading;

namespace ASCOM.DynamicRemoteClients
{
    /// <summary>
    /// Class to hold COM activation and access information for a served device
    /// </summary>
    public class ConfiguredDevice
    {
        public ConfiguredDevice(string deviceType, string progID, string description, int deviceNumber, bool allowConnectedSetFalse, bool allowConnectedSetTrue, bool allowConcurrentAccess, string uniqueId)
        {
            DeviceType = deviceType;
            ProgID = progID;
            DeviceNumber = deviceNumber;
            Description = description;
            AllowConnectedSetFalse = allowConnectedSetFalse;
            AllowConnectedSetTrue = allowConnectedSetTrue;
            AllowConcurrentAccess = allowConcurrentAccess;
            UniqueID = uniqueId;
        }

        public string DeviceType { get; set; }
        public string ProgID { get; set; }
        public string Description { get; set; }
        public int DeviceNumber { get; set; }
        public bool AllowConnectedSetFalse { get; set; }
        public bool AllowConnectedSetTrue { get; set; }
        public bool AllowConcurrentAccess { get; set; }
        public string UniqueID { get; set; }

        /// <summary>
        /// Return a unique key for this device based on its device type and device number
        /// </summary>
        public string DeviceKey
        {
            get
            {
                return string.Format("{0}/{1}", DeviceType.ToLowerInvariant(), DeviceNumber); // Create the device key
            }
        }
    }
}
