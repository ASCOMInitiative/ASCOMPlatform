namespace ASCOM.DynamicRemoteClients
{
    class DynamicDriverRegistration
    {

        public string ProgId { get; set; }
        public int Number { get; set; }
        public string DeviceType { get; set; }
        public string IPAdrress { get; set; }
        public int PortNumber { get; set; }
        public int RemoteDeviceNumber { get; set; }
        public string UniqueID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public InstallationState InstallState { get; set; }

        /// <summary>
        /// The ToString method is used by the checked list box control to provide the display description 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Description;
        }
    }
}
