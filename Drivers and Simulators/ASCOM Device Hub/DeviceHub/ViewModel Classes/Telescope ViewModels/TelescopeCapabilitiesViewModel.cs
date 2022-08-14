using System.Threading;
using System.Threading.Tasks;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public class TelescopeCapabilitiesViewModel : DeviceHubViewModelBase
	{
		public TelescopeCapabilitiesViewModel()
		{
			string caller = "TelescopeCapabilitiesViewModel ctor";
			LogAppMessage( "Initializing Instance constructor", caller );
			LogAppMessage( "Registering message handlers" , caller );

			Messenger.Default.Register<TelescopeCapabilitiesUpdatedMessage>( this, ( action ) => UpdateCapabilities( action ) );
			Messenger.Default.Register<DeviceDisconnectedMessage>( this, ( action ) => InvalidateCapabilities( action ) );

			LogAppMessage( "Instance constructor initialization complete.", caller );
		}

		private TelescopeCapabilities _capabilities;

		public TelescopeCapabilities Capabilities
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

		private void UpdateCapabilities( TelescopeCapabilitiesUpdatedMessage action )
		{
			SetCapabilities( action.Capabilities );
		}

		private void InvalidateCapabilities( DeviceDisconnectedMessage action )
		{
			if ( action.DeviceType == DeviceTypeEnum.Telescope )
			{
				SetCapabilities( null );
			}
		}

		private void SetCapabilities( TelescopeCapabilities capabilities )
		{
			// Make sure that we update the Capabilities on the U/I thread.

			Task.Factory.StartNew( () => Capabilities = capabilities, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		protected override void DoDispose()
		{
			Messenger.Default.Unregister<DeviceDisconnectedMessage>( this );
			Messenger.Default.Unregister<TelescopeCapabilitiesUpdatedMessage>( this );
		}
	}
}
