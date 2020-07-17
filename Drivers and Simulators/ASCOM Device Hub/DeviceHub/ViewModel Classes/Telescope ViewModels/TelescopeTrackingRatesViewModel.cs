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
		public TelescopeCapabilities Capabilities { get; set; }
		public TelescopeParameters Parameters { get; set; }
		public DevHubTelescopeStatus Status { get; set; }

		public ITelescopeManager TelescopeManager { get; private set; }

		public TelescopeTrackingRatesViewModel( ITelescopeManager telescopeManager )
		{
			TelescopeManager = telescopeManager;

			_raOffsetRate = 0.0;
			_decOffsetRate = 0.0;
			_canChangeOffsetRate = true;
			UpdateTrackingRateText();

			Messenger.Default.Register<TelescopeCapabilitiesUpdatedMessage>( this, ( action ) => UpdateCapabilities( action ) );
			Messenger.Default.Register<TelescopeParametersUpdatedMessage>( this, ( action ) => UpdateParameters( action ) );
			Messenger.Default.Register<TelescopeStatusUpdatedMessage>( this, ( action ) => UpdateStatus( action ) );
			Messenger.Default.Register<DeviceDisconnectedMessage>( this, (action) => InvalidateDeviceValues( action ) );
		}

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

		private double _raOffsetRate;

		public double RaOffsetRate
		{
			get { return _raOffsetRate; }
			set
			{
				if ( value != _raOffsetRate )
				{
					_raOffsetRate = value;
					OnPropertyChanged();
				}
			}
		}

		private double _decOffsetRate;

		public double DecOffsetRate
		{
			get { return _decOffsetRate; }
			set
			{
				if ( value != _decOffsetRate )
				{
					_decOffsetRate = value;
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

		private bool _canChangeOffsetRate;

		public bool CanChangeOffsetRate
		{
			get { return _canChangeOffsetRate; }
			set
			{
				if ( value != _canChangeOffsetRate )
				{
					_canChangeOffsetRate = value;
					OnPropertyChanged();
				}
			}
		}

		private void UpdateStatus( TelescopeStatusUpdatedMessage action )
		{
			// This is a registered message handler. It could be called from a worker thread
			// and we need to be sure that the work is done on the U/I thread.

			Task.Factory.StartNew( () =>
			{
				Status = action.Status;
				IsConnected = Status.Connected;

				UpdateTrackingRateText();

				if ( !Capabilities.CanSetDeclinationRate || !Capabilities.CanSetRightAscensionRate )
				{
					CanChangeOffsetRate = false;
				}
				else if ( Status.TrackingRate != DriveRates.driveSidereal )
				{
					CanChangeOffsetRate = true;
				}
				else if ( Status.RightAscensionRate == 0.0 && Status.DeclinationRate == 0.0 )
				{
					CanChangeOffsetRate = true;
				}
				else
				{
					CanChangeOffsetRate = false;
				}

			}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void ApplyStandardTrackingRate( DriveRates rate )
		{
			ApplyOffsetTrackingRate( rate, 0.0, 0.0 );
		}

		private void ApplyOffsetTrackingRate( DriveRates rate, double raOffset, double decOffset )
		{
			if ( !IsConnected )
			{
				return;
			}

			if ( Capabilities.CanSetRightAscensionRate )
			{
				TelescopeManager.SetRaOffsetTrackingRate( raOffset );
			}

			if ( Capabilities.CanSetDeclinationRate )
			{
				TelescopeManager.SetDecOffsetTrackingRate( decOffset );
			}

			if ( rate != Status.TrackingRate )
			{
				TelescopeManager.SetTrackingRate( rate );

				Status.TrackingRate = rate;
				UpdateTrackingRateText();
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
					case DriveRates.driveSidereal:
						if ( Status.RightAscensionRate != 0.0 || Status.DeclinationRate != 0.0 )
						{
							rate = "Offset";
						}

						break;

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

				TrackingRateText = String.Format( "Currently tracking at the {0} rate.", rate );
			}
		}

		private void InvalidateDeviceValues( DeviceDisconnectedMessage action )
		{
			if ( action.DeviceType == DeviceTypeEnum.Telescope )
			{
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
			_applyOffsetTrackingCommand = null;
		}

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
			CanChangeOffsetRate = true;
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
			CanChangeOffsetRate = true;
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

		#region ApplyOffsetTrackingCommand

		private ICommand _applyOffsetTrackingCommand;

		public ICommand ApplyOffsetTrackingCommand
		{
			get
			{
				if ( _applyOffsetTrackingCommand == null )
				{
					_applyOffsetTrackingCommand = new RelayCommand(
						param => this.ApplyOffsetTracking(),
						param => this.CanApplyOffsetTracking() );
				}

				return _applyOffsetTrackingCommand;
			}
		}

		private void ApplyOffsetTracking()
		{
			double raOffset = RaOffsetRate / ( 15.0 * 3600.0 ); // Convert from arc-sec/hr to seconds of RA / second.
			raOffset *= Globals.UTC_SECS_PER_SIDEREAL_SEC; // Convert time base from UTC seconds to sidereal seconds.

			double decOffset = DecOffsetRate / 3600.0;

			ApplyOffsetTrackingRate( DriveRates.driveSidereal, raOffset, decOffset );
			CanChangeOffsetRate = false;
		}

		private bool CanApplyOffsetTracking()
		{
			bool retval = IsConnected && Parameters != null && Status != null;
			retval &= Capabilities != null && Capabilities.CanSetRightAscensionRate && Capabilities.CanSetDeclinationRate;

			if ( retval )
			{ 
				retval = Array.Exists( Parameters.TrackingRates, r => r.Rate == DriveRates.driveSidereal ) && !Status.Slewing;
			}

			return retval;
		}

		#endregion ApplyOffsetTrackingCommand
	}
}
