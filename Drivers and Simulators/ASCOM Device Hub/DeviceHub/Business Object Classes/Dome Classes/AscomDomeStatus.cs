using System;
using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	public class AscomDomeStatus : DevicePropertiesBase
    {
		#region Instance Constructor

		public AscomDomeStatus()
		{ }

		public AscomDomeStatus( DomeManager mgr )
		{
			double newAltitude = Double.NaN;

			ShutterStatus = mgr.ShutterStatus;

			if ( !mgr.CanSetAltitude )
			{
				// Driver doesn't support altitude control.

				// Return NaN
			}
			else if ( ShutterStatus != ShutterState.shutterOpen )
			{
				// Can't get altitude unless the shutter is open.
				// Use the previous value or NaN, if none.

				double previousAltitude = Double.NaN;

				if ( mgr.Status != null )
				{
					previousAltitude = mgr.Status.Altitude;
				}

				newAltitude = previousAltitude;
			}
			else
			{
				newAltitude = mgr.Altitude;
			}

			Altitude = newAltitude;

			AtHome = mgr.AtHome;
			AtPark = mgr.AtPark;
			Azimuth = mgr.Azimuth;
			Connected = mgr.Connected;
			Slewing = mgr.Slewing;
		}

		#endregion Instance Constructor

		#region Change Notification Properties

		private double _altitude;

		public double Altitude
		{
			get { return _altitude; }
			set
			{
				if ( value != _altitude )
				{
					_altitude = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _atHome;

		public bool AtHome
		{
			get { return _atHome; }
			set
			{
				if ( value != _atHome )
				{
					_atHome = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _atPark;

		public bool AtPark
		{
			get { return _atPark; }
			set
			{
				if ( value != _atPark )
				{
					_atPark = value;
					OnPropertyChanged();
				}
			}
		}

		private double _azimuth;

		public double Azimuth
		{
			get { return _azimuth; }
			set
			{
				if ( value != _azimuth )
				{
					_azimuth = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _connected;

		public bool Connected
		{
			get { return _connected; }
			set
			{
				if ( value != _connected )
				{
					_connected = value;
					OnPropertyChanged();
				}
			}
		}

		private ShutterState _shutterStatus;

		public ShutterState ShutterStatus
		{
			get { return _shutterStatus; }
			set
			{
				if ( value != _shutterStatus )
				{
					_shutterStatus = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _slewing;

		public bool Slewing
		{
			get { return _slewing; }
			set
			{
				if ( value != _slewing )
				{
					_slewing = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region Protected Methods

		protected virtual void Clean()
		{
			Altitude = Double.NaN;
			AtHome = false;
			AtPark = false;
			Azimuth = Double.NaN;
			Connected = false;
			ShutterStatus = ShutterState.shutterClosed;
			Slewing = false;
		}

		#endregion
	}
}
