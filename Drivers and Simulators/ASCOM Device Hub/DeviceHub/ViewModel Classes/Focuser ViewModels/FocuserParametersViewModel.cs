using System.Threading;
using System.Threading.Tasks;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public class FocuserParametersViewModel : DeviceHubViewModelBase
    {
		public FocuserParametersViewModel()
		{
			string caller = "FocuserParametersViewModel ctor";
			LogAppMessage( "Initializing Instance constructor", caller );
			LogAppMessage( "Registering message handlers", caller );

			Messenger.Default.Register<FocuserParametersUpdatedMessage>( this, ( action ) => UpdateParameters( action ) );
			Messenger.Default.Register<DeviceDisconnectedMessage>( this, ( action ) => InvalidateParameters( action ) );

			LogAppMessage( "Initialization complete", caller );
		}

		private FocuserParameters _parameters;

		public FocuserParameters Parameters
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
			if ( action.DeviceType == DeviceTypeEnum.Focuser )
			{
				SetParameters( null );
			}
		}

		private void UpdateParameters( FocuserParametersUpdatedMessage action )
		{
			SetParameters( action.Parameters );
		}

		private void SetParameters( FocuserParameters parameters )
		{
			// Make sure that we update the Paremeters on the U/I thread.

			Task.Factory.StartNew( () => Parameters = parameters, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		protected override void DoDispose()
		{
			Messenger.Default.Unregister<DeviceDisconnectedMessage>( this );
			Messenger.Default.Unregister<FocuserParametersUpdatedMessage>( this );
		}
	}
}
