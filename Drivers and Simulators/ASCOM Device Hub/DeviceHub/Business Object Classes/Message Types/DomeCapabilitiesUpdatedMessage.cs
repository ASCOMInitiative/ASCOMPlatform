namespace ASCOM.DeviceHub
{
	class DomeCapabilitiesUpdatedMessage
    {
		public DomeCapabilitiesUpdatedMessage( DomeCapabilities capabilities )
		{
			Capabilities = capabilities;
		}

		public DomeCapabilities Capabilities { get; private set; }
    }
}
