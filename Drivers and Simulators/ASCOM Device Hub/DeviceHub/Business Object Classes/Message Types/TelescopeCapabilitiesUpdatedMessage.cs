namespace ASCOM.DeviceHub
{
	public class TelescopeCapabilitiesUpdatedMessage
    {
		public TelescopeCapabilitiesUpdatedMessage( TelescopeCapabilities capabilities)
		{
			Capabilities = capabilities;
		}

		public TelescopeCapabilities Capabilities { get; private set; }
	}
}
