using System.Threading;
using System.Threading.Tasks;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public class DomeParametersViewModel : DeviceHubViewModelBase
	{
		public DomeParametersViewModel()
		{
			Messenger.Default.Register<DomeParametersUpdatedMessage>( this, ( action ) => UpdateParameters( action ) );
			Messenger.Default.Register<DeviceDisconnectedMessage>( this, ( action ) => InvalidateParameters( action ) );
		}

		private DomeParameters _parameters;

		public DomeParameters Parameters
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
			if ( action.DeviceType == DeviceTypeEnum.Dome )
			{
				SetParameters( null );
			}
		}

		private void UpdateParameters( DomeParametersUpdatedMessage action )
		{
			SetParameters( action.Parameters );
		}

		private void SetParameters( DomeParameters parameters )
		{
			// Make sure that we update the Parameters on the U/I thread.

			Task.Factory.StartNew( () => Parameters = parameters, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		protected override void DoDispose()
		{
			Messenger.Default.Unregister<DeviceDisconnectedMessage>( this );
			Messenger.Default.Unregister<DomeParametersUpdatedMessage>( this );
		}
	}
}
