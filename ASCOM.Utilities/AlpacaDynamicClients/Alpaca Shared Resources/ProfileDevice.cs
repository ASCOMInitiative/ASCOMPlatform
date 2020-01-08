namespace ASCOM.DynamicRemoteClients
{
    public class ProfileDevice
    {
        public ProfileDevice(string deviceType, string progID, string description)
        {
            DeviceType = deviceType;
            ProgID = progID;
            Description = description;
        }

        public string DeviceType { get; set; }
        public string ProgID { get; set; }
        public string Description { get; set; }
    }
}
