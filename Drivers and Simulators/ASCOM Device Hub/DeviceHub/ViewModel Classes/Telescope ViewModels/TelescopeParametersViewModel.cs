using System.Threading;
using System.Threading.Tasks;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public class TelescopeParametersViewModel : DeviceHubViewModelBase
	{
		public TelescopeParametersViewModel()
		{
			Messenger.Default.Register<TelescopeParametersUpdatedMessage>( this, ( action ) => UpdateParameters( action ) );
			Messenger.Default.Register<DeviceDisconnectedMessage>( this, ( action ) => InvalidateParameters( action ) );
		}

		private TelescopeParameters _parameters;

		public TelescopeParameters Parameters
		{
			get { return _parameters; }
			set
			{
				if ( value != _parameters )
				{
					_parameters = value;
					OnPropertyChanged();
				}
			}
		}

		private void InvalidateParameters( DeviceDisconnectedMessage action )
		{
			if ( action.DeviceType == DeviceTypeEnum.Telescope )
			{
				SetParameters( null );
			}
		}

		private void UpdateParameters( TelescopeParametersUpdatedMessage action )
		{
			SetParameters( action.Parameters );
		}

		private void SetParameters( TelescopeParameters parameters )
		{
			// Make sure that we update the Parameters on the U/I thread.

			Task.Factory.StartNew( () => Parameters = parameters, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		protected override void DoDispose()
		{
			Messenger.Default.Unregister<DeviceDisconnectedMessage>( this );
			Messenger.Default.Unregister<TelescopeParametersUpdatedMessage>( this );
		}
	}
}
