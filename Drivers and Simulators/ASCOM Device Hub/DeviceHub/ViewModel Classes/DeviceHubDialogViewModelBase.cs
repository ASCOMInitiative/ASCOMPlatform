using System;

namespace ASCOM.DeviceHub
{
	public class DeviceHubDialogViewModelBase : DeviceHubViewModelBase
    {
		public event EventHandler NotifyClosed = delegate { };

		public DeviceHubDialogViewModelBase( string sourceID )
			: base()
		{
			SourceID = sourceID;
		}

		public string SourceID { get; protected set; }

		public virtual void NotifyClosedSubscribers()
		{
			OnNotifyClosed();
		}

		public void Activate()
		{
			OnActivate();
		}

		public void Deactivate()
		{
			OnDeactivate();
		}

		protected virtual void OnNotifyClosed()
		{
			NotifyClosed( this, EventArgs.Empty );
		}

		protected virtual void OnActivate()
		{}

		protected virtual void OnDeactivate()
		{}
	}
}
