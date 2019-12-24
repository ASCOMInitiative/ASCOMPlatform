namespace ASCOM.DynamicRemoteClients
{
    class DriverRegistration
    {
        string progId;
        string type;
        int number;

        public string ProgId { get => progId; set => progId = value; }
        public int Number { get => number; set => number = value; }
        public string DeviceType { get => type; set => type = value; }
    }
}
