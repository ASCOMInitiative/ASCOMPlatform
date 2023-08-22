﻿using System;

using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
    public class DevHubDomeStatus : AscomDomeStatus, ICloneable
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

		public DevHubDomeStatus( DevHubDomeStatus other )
			: base( other )
		{
			this.HomingState = other.HomingState;
			this.ParkingState = other.ParkingState;
			this.ShutterPosition = other.ShutterPosition;
			this.LastUpdateTime = other.LastUpdateTime;
		}

		public DevHubDomeStatus( DomeManager mgr )
			: base( mgr )
		{
			if ( mgr.HomingState != HomingStateEnum.HomingInProgress || !Slewing )
			{
				HomingState = ( AtHome ) ? HomingStateEnum.IsAtHome : HomingStateEnum.NotAtHome;
			}

			if ( mgr.ParkingState != ParkingStateEnum.ParkInProgress || !Slewing )
			{
				ParkingState = ( AtPark ) ? ParkingStateEnum.IsAtPark : ParkingStateEnum.Unparked;
			}

			ShutterPosition = FormatShutterPosition( mgr );

			LastUpdateTime = DateTime.Now;
		}

		public DateTime LastUpdateTime { get; private set; }

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

			double fractionOpen = ( mgr.Capabilities.CanSetAltitude ) ? this.Altitude / 90.0 : Double.NaN;
			bool useFraction = ( !Double.IsNaN( fractionOpen ) );

			switch ( shutterStatus )
			{
				case ShutterState.shutterClosed:
					position = "Closed";
					break;

				case ShutterState.shutterClosing:
					position = "Closing";

					break;

				case ShutterState.shutterError:
					position = "Unknown";

					break;

				case ShutterState.shutterOpen:
					position = "Open";
					position += (useFraction) ? $" at {fractionOpen:P0}" : "";

					break;

				case ShutterState.shutterOpening:
					position = "Opening";

					break;
			}

			return position;
		}

		protected override void Clean()
		{
			base.Clean();

			ParkingState = ParkingStateEnum.Unparked;
			HomingState = HomingStateEnum.NotAtHome;

			LastUpdateTime = DateTime.Now;
		}

		object ICloneable.Clone()
		{
			return new DevHubDomeStatus( this );
		}

		public new DevHubDomeStatus Clone()
		{
			return new DevHubDomeStatus( this );
		}

		#endregion Helper Methods
	}
}
