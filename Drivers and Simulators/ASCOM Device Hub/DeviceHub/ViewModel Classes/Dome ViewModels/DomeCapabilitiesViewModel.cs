using System.Threading;
using System.Threading.Tasks;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public class DomeCapabilitiesViewModel : DeviceHubViewModelBase
	{
		public DomeCapabilitiesViewModel()
		{
			Messenger.Default.Register<DomeCapabilitiesUpdatedMessage>( this, ( action ) => UpdateCapabilities( action ) );
			Messenger.Default.Register<DeviceDisconnectedMessage>( this, ( action ) => InvalidateCapabilities( action ) );
		}

		private DomeCapabilities _capabilities;

		public DomeCapabilities Capabilities
		{
			get { return _capabilities; }
			set
			{
				if ( value != _capabilities )
				{
					_capabilities = value;
					OnPropertyChanged();
				}
			}
		}

		private void UpdateCapabilities( DomeCapabilitiesUpdatedMessage action )
		{
			SetCapabilities( action.Capabilities );
		}

		private void InvalidateCapabilities( DeviceDisconnectedMessage action )
		{
			if ( action.DeviceType == DeviceTypeEnum.Dome )
			{
				SetCapabilities( null );
			}
		}

		private void SetCapabilities( DomeCapabilities capabilities )
		{
			// Make sure that we update the Parameters on the U/I thread.

			Task.Factory.StartNew( () => Capabilities = capabilities, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		protected override void DoDispose()
		{
			Messenger.Default.Unregister<DeviceDisconnectedMessage>( this );
			Messenger.Default.Unregister<DomeCapabilitiesUpdatedMessage>( this );
		}
	}
}
