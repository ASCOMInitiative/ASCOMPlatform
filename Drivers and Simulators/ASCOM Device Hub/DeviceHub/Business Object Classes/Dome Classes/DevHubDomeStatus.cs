using System;
using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	public class DevHubDomeStatus : AscomDomeStatus
    {
		public static DevHubDomeStatus GetEmptyStatus()
		{
			DevHubDomeStatus status = new DevHubDomeStatus();
			status.Clean();

			return status;
		}

		public DevHubDomeStatus()
			: base()
		{}

		public DevHubDomeStatus( DomeManager mgr )
			: base( mgr )
		{
			if ( this.ParkingState != ParkingStateEnum.ParkInProgress )
			{
				ParkingState = ( AtPark ) ? ParkingStateEnum.IsAtPark : ParkingStateEnum.Unparked;
			}
			else
			{
				ParkingState = ParkingStateEnum.ParkInProgress;
			}

			HomingState = this.HomingState;

			ShutterPosition = FormatShutterPosition( mgr );
		}

		#region Change Notification Properties

		private ParkingStateEnum _parkingState;

		public ParkingStateEnum ParkingState
		{
			get { return _parkingState; }
			set
			{
				if ( value != _parkingState )
				{
					_parkingState = value;
					OnPropertyChanged();
				}
			}
		}
		
		private HomingStateEnum _homingState;

		public HomingStateEnum HomingState
		{
			get { return _homingState; }
			set
			{
				if ( value != _homingState )
				{
					_homingState = value;
					OnPropertyChanged();
				}
			}
		}

		private string _shutterPosition;

		public string ShutterPosition
		{
			get { return _shutterPosition; }
			set
			{
				if ( value != _shutterPosition )
				{
					_shutterPosition = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region Helper Methods

		private string FormatShutterPosition( DomeManager mgr )
		{
			string position = "Unknown";
			ShutterState shutterStatus = this.ShutterStatus;
			double fractionOpen = ( mgr.CanSetAltitude ) ? this.Altitude / 90.0 : Double.NaN;
			bool useFraction = ( !Double.IsNaN( fractionOpen ) );

			switch ( shutterStatus )
			{
				case ShutterState.shutterClosed:
					position = "Closed";
					break;

				case ShutterState.shutterClosing:
					position = "Closing" + ( ( useFraction ) ? String.Format( " at {0:P0}", fractionOpen ) : "" );
					break;

				case ShutterState.shutterError:
					position = "Unknown";
					break;

				case ShutterState.shutterOpen:
					position = "Open" + ( ( useFraction ) ? String.Format( " at {0:P0}", fractionOpen ) : "" );
					break;

				case ShutterState.shutterOpening:
					position = "Opening" + ( ( useFraction ) ? String.Format( " at {0:P0}", fractionOpen ) : "" );

					break;
			}

			return position;
		}

		protected override void Clean()
		{
			base.Clean();

			ParkingState = ParkingStateEnum.Unparked;
			HomingState = HomingStateEnum.NotAtHome;
		}

		#endregion Helper Methods
	}
}
