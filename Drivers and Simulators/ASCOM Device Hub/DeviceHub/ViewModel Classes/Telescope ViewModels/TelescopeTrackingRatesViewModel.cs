using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using ASCOM.DeviceInterface;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public class TelescopeTrackingRatesViewModel : DeviceHubViewModelBase
	{
		#region Public Properties

		public TelescopeCapabilities Capabilities { get; set; }
		public TelescopeParameters Parameters { get; set; }
		public DevHubTelescopeStatus Status { get; set; }

		public ITelescopeManager TelescopeManager { get; private set; }

		private string _ratesNote;

		public string RatesNote
		{
			get { return _ratesNote; }
		}

		public string AscomRaUnits => "seconds / sidereal second";
		public string AscomDecUnits => "arc-seconds / SI second";
		public string NasaJplRaUnits => "arc-seconds / hour";
		public string NasaJplDecUnits => "arc-seconds / hour";

		#endregion Public Properties

		#region Constructor

		public TelescopeTrackingRatesViewModel( ITelescopeManager telescopeManager )
		{
			string caller = "TelescopeTrackingRatesViewModel ctor";

			LogAppMessage( "Initializing Instance constructor", caller );

			TelescopeManager = telescopeManager;

			_raRateOffset = null;
			_decRateOffset = null;	
			_newRaOffsetRate = 0.0;
			_newDecOffsetRate = 0.0;
			_ratesNote = "These offsets are only applied when the tracking rate is set to Sidereal.";
			UpdateTrackingRateText();

			LogAppMessage( "Registering message handlers", caller );

			Messenger.Default.Register<TelescopeCapabilitiesUpdatedMessage>( this, ( action ) => UpdateCapabilities( action ) );
			Messenger.Default.Register<TelescopeParametersUpdatedMessage>( this, ( action ) => UpdateParameters( action ) );
			Messenger.Default.Register<TelescopeStatusUpdatedMessage>( this, ( action ) => UpdateStatus( action ) );
			Messenger.Default.Register<DeviceDisconnectedMessage>( this, (action) => InvalidateDeviceValues( action ) );

			LogAppMessage( "Instance constructor initialization complete", caller );
		}

		#endregion Constructor

		#region Change Notification Properties

		private bool _isConnected;

		public bool IsConnected
		{
			get { return _isConnected; }
			set
			{
				if ( value != _isConnected )
				{
					_isConnected = value;
					OnPropertyChanged();
				}
			}
		}

		private double _newRaOffsetRate;

		public double NewRaOffsetRate
		{
			get { return _newRaOffsetRate; }
			set
			{
				if ( value != _newRaOffsetRate )
				{
					_newRaOffsetRate = value;
					OnPropertyChanged();
				}
			}
		}

		private double _newDecOffsetRate;

		public double NewDecOffsetRate
		{
			get { return _newDecOffsetRate; }
			set
			{
				if ( value != _newDecOffsetRate )
				{
					_newDecOffsetRate = value;
					OnPropertyChanged();
				}
			}
		}

		private string _trackingRateText;

		public string TrackingRateText
		{
			get { return _trackingRateText; }
			set
			{
				if ( value != _trackingRateText )
				{
					_trackingRateText = value;
					OnPropertyChanged();
				}
			}
		}

		// The rate offsets are nullable so that no value is displayed when we are not connected to a scope.

		private double? _raRateOffset;

		public double? RaRateOffset
		{
			get { return _raRateOffset; }
			set
			{
				if ( value != _raRateOffset )
				{
					_raRateOffset = value;
					OnPropertyChanged();
				}
			}
		}

		private double? _decRateOffset;

		public double? DecRateOffset
		{
			get { return _decRateOffset; }
			set
			{
				if ( value != _decRateOffset )
				{
					_decRateOffset = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _useNasaJplUnits;

		public bool UseNasaJplUnits
		{
			get { return _useNasaJplUnits; }
			set
			{
				if ( value != _useNasaJplUnits )
				{
					_useNasaJplUnits = value;
					OnPropertyChanged();
					RecalculateNewRates( _useNasaJplUnits );
				}
			}
		}

		private string _newRaOffsetUnits;

		public string NewRaOffsetUnits
		{
			get { return _newRaOffsetUnits; }
			set
			{
				if ( value != _newRaOffsetUnits )
				{
					_newRaOffsetUnits = value;
					OnPropertyChanged();
				}
			}
		}

		private string _newDecOffsetUnits;

		public string NewDecOffsetUnits
		{
			get { return _newDecOffsetUnits; }
			set
			{
				if ( value != _newDecOffsetUnits )
				{
					_newDecOffsetUnits = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region Private Helper Methods

		private void UpdateStatus( TelescopeStatusUpdatedMessage action )
		{
			// This is a registered message handler. It could be called from a worker thread
			// and we need to be sure that the work is done on the U/I thread.

			if ( Capabilities == null )
			{
				return;
			}

			Task.Factory.StartNew( () =>
			{
				Status = action.Status;
				IsConnected = Status.Connected;

				UpdateTrackingRateText();
				RaRateOffset = Status.RightAscensionRate;
				DecRateOffset = Status.DeclinationRate;
			}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void ApplyStandardTrackingRate( DriveRates rate )
		{
			if ( !IsConnected )
			{
				return;
			}

			if ( rate != Status.TrackingRate )
			{
				try
				{
					TelescopeManager.SetTrackingRate( rate );

					Status.TrackingRate = rate;
					UpdateTrackingRateText();
				}
				catch ( Exception xcp )
				{
					string msg = "The telescope driver returned an error when trying to change the tracking rate. "
						+ $"Details follow:\r\n\r\n{xcp}";
					ShowMessage( msg, "Telescope Driver Error" );
				}
			}
		}

		private void UpdateTrackingRateText()
		{
			if ( !IsConnected )
			{
				TrackingRateText = "The tracking rate is not known.";
			}
			else
			{
				DriveRates driveRate = Status.TrackingRate;
				string rate = "Sidereal";

				switch ( driveRate )
				{
					case DriveRates.driveLunar:
						rate = "Lunar";

						break;

					case DriveRates.driveSolar:
						rate = "Solar";

						break;

					case DriveRates.driveKing:
						rate = "King";

						break;
				}

				TrackingRateText = $"Currently tracking at the {rate} rate.";
			}
		}

		private void InvalidateDeviceValues( DeviceDisconnectedMessage action )
		{
			if ( action.DeviceType == DeviceTypeEnum.Telescope )
			{
				RaRateOffset = null;
				DecRateOffset = null;
				SetCapabilities( null );
				SetParameters( null );
			}
		}

		private void UpdateCapabilities( TelescopeCapabilitiesUpdatedMessage action )
		{
			SetCapabilities( action.Capabilities );
		}

		private void SetCapabilities( TelescopeCapabilities capabilities )
		{
			// Make sure that we update the Paremeters on the U/I thread.

			Task.Factory.StartNew( () => Capabilities = capabilities, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void UpdateParameters( TelescopeParametersUpdatedMessage action )
		{
			SetParameters( action.Parameters );
		}

		private void SetParameters( TelescopeParameters parameters )
		{
			// Make sure that we update the Paremeters on the U/I thread.

			Task.Factory.StartNew( () => Parameters = parameters, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		protected override void DoDispose()
		{
			Messenger.Default.Unregister<DeviceDisconnectedMessage>( this );
			Messenger.Default.Unregister<TelescopeCapabilitiesUpdatedMessage>( this );
			Messenger.Default.Unregister<TelescopeParametersUpdatedMessage>( this );
			Messenger.Default.Unregister<TelescopeStatusUpdatedMessage>( this );

			_applySiderealTrackingCommand = null;
			_applyLunarTrackingCommand = null;
			_applySolarTrackingCommand = null;
			_applyKingTrackingCommand = null;
			_commitNewRatesCommand = null;
		}

		private double ConvertTrackingRate( double rate, bool isRaRate, bool toNasaJplUnits )
		{
			double retval = Double.NaN;

			double factor;

			if ( isRaRate )
			{
				factor = Globals.UTC_SECS_PER_SIDEREAL_SEC / ( 15.0 * 3600.0 );
			}
			else // is Dec rate
			{
				factor = 1.0 / 3600.0;
			}

			if ( toNasaJplUnits )
			{
				// Convert a rate from from ASCOM units to NASA JPL units

				retval = rate / factor;
			}
			else
			{
				// Convert a rate from NASA JPL units to ASCOM units.

				retval = rate * factor;
			}

			return retval;
		}

		private void RecalculateNewRates( bool useNasaJplUnits )
		{	
			NewRaOffsetRate = ConvertTrackingRate( NewRaOffsetRate, true, useNasaJplUnits );
			NewDecOffsetRate = ConvertTrackingRate( NewDecOffsetRate, false, useNasaJplUnits );
		}

		#endregion Private Helper Methods

		#region Commands

		#region ApplySiderealTrackingCommand

		private ICommand _applySiderealTrackingCommand;

		public ICommand ApplySiderealTrackingCommand
		{
			get
			{
				if ( _applySiderealTrackingCommand == null )
				{
					_applySiderealTrackingCommand = new RelayCommand(
						param => this.ApplySiderealTracking(),
						param => this.CanApplySiderealTracking() );
				}

				return _applySiderealTrackingCommand;
			}
		}

		private void ApplySiderealTracking()
		{
			ApplyStandardTrackingRate( DriveRates.driveSidereal );
		}

		private bool CanApplySiderealTracking()
		{
			bool retval = IsConnected && Parameters != null && Status != null;

			if ( retval )
			{
				retval = Array.Exists( Parameters.TrackingRates, r => r.Rate == DriveRates.driveSidereal ) && !Status.Slewing;
			}

			return retval;
		}

		#endregion ApplySiderealTrackingCommand

		#region ApplyLunarTrackingCommand

		private ICommand _applyLunarTrackingCommand;

		public ICommand ApplyLunarTrackingCommand
		{
			get
			{
				if ( _applyLunarTrackingCommand == null )
				{
					_applyLunarTrackingCommand = new RelayCommand(
						param => this.ApplyLunarTracking(),
						param => this.CanApplyLunarTracking() );
				}

				return _applyLunarTrackingCommand;
			}
		}

		private void ApplyLunarTracking()
		{
			ApplyStandardTrackingRate( DriveRates.driveLunar );
		}

		private bool CanApplyLunarTracking()
		{
			bool retval = IsConnected && Parameters != null && Status != null;

			if ( retval )
			{
				retval = Array.Exists( Parameters.TrackingRates, r => r.Rate == DriveRates.driveLunar ) && !Status.Slewing;
			}

			return retval;
		}

		#endregion ApplyLunarTrackingCommand

		#region ApplySolarTrackingCommand

		private ICommand _applySolarTrackingCommand;

		public ICommand ApplySolarTrackingCommand
		{
			get
			{
				if ( _applySolarTrackingCommand == null )
				{
					_applySolarTrackingCommand = new RelayCommand(
						param => this.ApplySolarTracking(),
						param => this.CanApplySolarTracking() );
				}

				return _applySolarTrackingCommand;
			}
		}

		private void ApplySolarTracking()
		{
			ApplyStandardTrackingRate( DriveRates.driveSolar );
		}

		private bool CanApplySolarTracking()
		{
			bool retval = IsConnected && Parameters != null && Status != null;
			retval &= Capabilities != null && Capabilities.CanSetRightAscensionRate && Capabilities.CanSetDeclinationRate;

			if ( retval )
			{ 
				retval = Array.Exists( Parameters.TrackingRates, r => r.Rate == DriveRates.driveSolar ) && !Status.Slewing;
			}

			return retval;
		}

		#endregion ApplyOffsetTrackingCommand

		#region ApplyKingTrackingCommand

		private ICommand _applyKingTrackingCommand;

		public ICommand ApplyKingTrackingCommand
		{
			get
			{
				if ( _applyKingTrackingCommand == null )
				{
					_applyKingTrackingCommand = new RelayCommand(
						param => this.ApplyKingTracking(),
						param => this.CanApplyKingTracking() );
				}

				return _applyKingTrackingCommand;
			}
		}

		private void ApplyKingTracking()
		{
			ApplyStandardTrackingRate( DriveRates.driveKing );
		}

		private bool CanApplyKingTracking()
		{
			bool retval = IsConnected && Parameters != null && Status != null;

			if ( retval )
			{
				retval = Array.Exists( Parameters.TrackingRates, r => r.Rate == DriveRates.driveKing ) && !Status.Slewing;
			}

			return retval;
		}

		#endregion ApplyKingTrackingCommand

		#region ChangeRateUnitsCommand

		private ICommand _changeRateUnitsCommand;

		public ICommand ChangeRateUnitsCommand
		{
			get
			{
				if ( _changeRateUnitsCommand == null )
				{
					_changeRateUnitsCommand = new RelayCommand(
						param => this.ChangeRateUnits(),
						param => this.CanChangeRateUnits() );
				}

				return _changeRateUnitsCommand;
			}
		}

		private void ChangeRateUnits()
		{
			if ( UseNasaJplUnits )
			{
				NewRaOffsetUnits = NasaJplRaUnits;
				NewDecOffsetUnits = NasaJplDecUnits;
			}
			else
			{
				NewRaOffsetUnits = AscomRaUnits;
				NewDecOffsetUnits = AscomDecUnits;
			}
		}

		private bool CanChangeRateUnits()
		{
			return true;
		}

		#endregion ChangeRateUnitsCommand


		#region CommitNewRatesCommand

		private ICommand _commitNewRatesCommand;

		public ICommand CommitNewRatesCommand
		{
			get
			{
				if ( _commitNewRatesCommand == null )
				{
					_commitNewRatesCommand = new RelayCommand(
						param => this.CommitNewRates(),
						param => this.CanCommitNewRates() );
				}

				return _commitNewRatesCommand;
			}
		}

		private void CommitNewRates()
		{
			double raOffset = NewRaOffsetRate;
			double decOffset = NewDecOffsetRate;

			if ( UseNasaJplUnits )
			{
				// New values are in NASA JPL units so convert to ASCOM units.

				raOffset = ConvertTrackingRate( raOffset, true, false );
				decOffset = ConvertTrackingRate( decOffset, false, false );
			}

			TelescopeManager.SetRaOffsetTrackingRate( raOffset );
			TelescopeManager.SetDecOffsetTrackingRate( decOffset );
		}

		private bool CanCommitNewRates()
		{
			if ( Capabilities != null )
			{
				return IsConnected == true && Capabilities.CanSetDeclinationRate && Capabilities.CanSetRightAscensionRate;
			}

			return false;
		}

		#endregion CommitNewRatesCommand

		#endregion Commands
	}
}
