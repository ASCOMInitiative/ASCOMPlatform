using System;
using System.Windows.Input;

namespace ASCOM.DeviceHub
{
	public class TelescopeSetupViewModel : DeviceHubViewModelBase
	{
		public TelescopeSetupViewModel()
		{}

		#region Change Notification Properties

		private string _telescopeID;

		public string TelescopeID
		{
			get { return _telescopeID; }
			set
			{
				if ( value != _telescopeID )
				{
					_telescopeID = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region Helper Methods

		protected override void DoDispose()
		{
			_chooseScopeCommand = null;
		}

		#endregion Helper Methods

		#region Relay Commands

		#region ChooseScopeCommand

		private ICommand _chooseScopeCommand;

		public ICommand ChooseScopeCommand
		{
			get
			{
				if ( _chooseScopeCommand == null )
				{
					_chooseScopeCommand = new RelayCommand(
						param => this.ChooseScope() );
				}

				return _chooseScopeCommand;
			}
		}

		private void ChooseScope()
		{
			string oldID = TelescopeID;
			string newID = TelescopeManager.Choose( oldID );

			if ( String.IsNullOrEmpty( newID ) )
			{
				return;
			}

			// Prevent us from choosing ourselves as our telescope.

			if ( newID == Globals.DevHubTelescopeID )
			{
				string msg = Globals.DevHubTelescopeID + " cannot be chosen as the telescope!";
				ShowMessage( msg, "Invalid Telescope Selected" );

				return;
			}

			if ( newID != oldID )
			{
				TelescopeID = newID;
			}
		}

		#endregion ChooseScopeCommand

		#endregion Relay Commands
	}
}
