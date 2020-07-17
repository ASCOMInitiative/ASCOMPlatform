namespace ASCOM.DeviceHub
{
	public class DevHubFocuserStatus : AscomFocuserStatus
	{
		#region Static Methods

		public static DevHubFocuserStatus GetEmptyStatus()
		{
			DevHubFocuserStatus status = new DevHubFocuserStatus();
			status.Clean();

			return status;
		}

		#endregion Static Methods

		#region Instance Constructors

		public DevHubFocuserStatus()
			: base()
		{}

		public DevHubFocuserStatus( FocuserManager mgr )
			: base( mgr )
		{}

		#endregion Instance Constructors

		// At least initially, there are no additional properties here, but
		// the class is here in case a need arises.

		#region Helper Methods 

		protected override void Clean()
		{
			base.Clean();

			// Add initialization of any new properties here.
		}

		#endregion Helper Methods 
	}
}
