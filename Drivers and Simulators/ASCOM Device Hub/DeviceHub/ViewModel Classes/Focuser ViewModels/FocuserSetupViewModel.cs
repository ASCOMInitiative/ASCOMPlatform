using System;
using System.Windows.Input;

namespace ASCOM.DeviceHub
{
	public class FocuserSetupViewModel : DeviceHubViewModelBase
    {
		public FocuserSetupViewModel()
		{
			_temperatureOffset = Globals.FocuserTemperatureOffset;
			_fastUpdateMinimum = Globals.FOCUSER_FAST_UPDATE_MIN;
			_fastUpdateMaximum = Globals.FOCUSER_FAST_UPDATE_MAX;
		}

		#region Change Notification Properties

		private string _focuserID;

		public string FocuserID
		{
			get { return _focuserID; }
			set
			{
				if ( value != _focuserID )
				{
					_focuserID = value;
					OnPropertyChanged();
				}
			}
		}

		private double _temperatureOffset;

		public double TemperatureOffset
		{
			get { return _temperatureOffset; }
			set
			{
				if ( value != _temperatureOffset )
				{
					_temperatureOffset = value;
					OnPropertyChanged();
				}
			}
		}

		private double _fastUpdatePeriod;

		public double FastUpdatePeriod
		{
			get { return _fastUpdatePeriod; }
			set
			{
				if ( value != _fastUpdatePeriod )
				{
					_fastUpdatePeriod = value;
					OnPropertyChanged();
				}
			}
		}

		private double _fastUpdateMinimum;

		public double FastUpdateMinimum
		{
			get { return _fastUpdateMinimum; }
			set
			{
				if ( value != _fastUpdateMinimum )
				{
					_fastUpdateMinimum = value;
					OnPropertyChanged();
				}
			}
		}

		private double _fastUpdateMaximum;

		public double FastUpdateMaximum
		{
			get { return _fastUpdateMaximum; }
			set
			{
				if ( value != _fastUpdateMaximum )
				{
					_fastUpdateMaximum = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region Public Methods

		public void Initialize( double temperatureOffset )
		{
			_temperatureOffset = temperatureOffset;
		}

		#endregion Public methods

		#region Helper Methods

		protected override void DoDispose()
		{
			_chooseFocuserCommand = null;
		}

		#endregion Helper Methods

		#region Relay Commands

		#region ChooseFocuserCommand

		private ICommand _chooseFocuserCommand;

		public ICommand ChooseFocuserCommand
		{
			get
			{
				if ( _chooseFocuserCommand == null )
				{
					_chooseFocuserCommand = new RelayCommand(
						param => this.ChooseFocuser() );
				}

				return _chooseFocuserCommand;
			}
		}

		private void ChooseFocuser()
		{
			string oldID = FocuserID;
			string newID = FocuserManager.Choose( oldID );

			if ( String.IsNullOrEmpty( newID ) )
			{
				return;
			}

			// Prevent us from choosing ourselves as our Focuser.

			if ( newID == Globals.DevHubFocuserID )
			{
				string msg = $"{Globals.DevHubFocuserID} cannot be chosen as the focuser!";
				ShowMessage( msg, "Invalid Focuser Selected" );

				return;
			}

			if ( newID != oldID )
			{
				FocuserID = newID;
			}
		}

		#endregion ChooseFocuserCommand

		#endregion Relay Commands
	}
}
