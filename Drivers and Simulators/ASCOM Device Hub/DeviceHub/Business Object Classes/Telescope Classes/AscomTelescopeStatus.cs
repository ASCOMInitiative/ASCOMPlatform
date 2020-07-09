using System;
using System.Collections.Generic;
using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	public class AscomTelescopeStatus : DevicePropertiesBase
	{
		public AscomTelescopeStatus()
		{}

		public AscomTelescopeStatus( TelescopeManager mgr )
		{
			DeviceManager = mgr;

			Altitude = mgr.Altitude;
			AtHome = mgr.AtHome;

			if ( mgr.Parameters.InterfaceVersion > 1 )
			{
				AtPark = mgr.AtPark;
			}
			else
			{
				AtPark = false;
			}

			Azimuth = mgr.Azimuth;
			Connected = mgr.Connected;
			Declination = mgr.Declination;
			DeclinationRate = mgr.DeclinationRate;
			GuideRateDeclination = mgr.GuideRateDeclination;
			GuideRateRightAscension = mgr.GuideRateRightAscension;
			IsPulseGuiding = mgr.IsPulseGuiding;
			RightAscension = mgr.RightAscension;
			RightAscensionRate = mgr.RightAscensionRate;
			SideOfPier = mgr.SideOfPier;
			SiderealTime = mgr.SiderealTime;
			Slewing = mgr.Capabilities.CanSlew ? mgr.Slewing : false;
			TargetDeclination = mgr.TargetDeclination;
			TargetRightAscension = mgr.TargetRightAscension;
			Tracking = mgr.Tracking;
			TrackingRate = mgr.TrackingRate;

			try
			{
				UTCDate = mgr.UTCDate;
			}
			catch ( Exception )
			{
				UTCDate = DateTime.Now.ToUniversalTime();
			}
		}

		#region Change Notification Properties

		private double _altitude;

		public double Altitude
		{
			get { return _altitude; }
			set
			{
				if ( value != _altitude )
				{
					GetExceptionFromManager();
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
					GetExceptionFromManager();
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
					GetExceptionFromManager();
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
					GetExceptionFromManager();
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
					GetExceptionFromManager();
					_connected = value;
					OnPropertyChanged();
				}
			}
		}

		private double _declination;

		public double Declination
		{
			get { return _declination; }
			set
			{
				if ( value != _declination )
				{
					GetExceptionFromManager();
					_declination = value;
					OnPropertyChanged();
				}
			}
		}

		private double _declinationRate;

		public double DeclinationRate
		{
			get { return _declinationRate; }
			set
			{
				if ( value != _declinationRate )
				{
					GetExceptionFromManager();
					_declinationRate = value;
					OnPropertyChanged();
				}
			}
		}

		private double _guideRateDeclination;

		public double GuideRateDeclination
		{
			get { return _guideRateDeclination; }
			set
			{
				if ( value != _guideRateDeclination )
				{
					GetExceptionFromManager();
					_guideRateDeclination = value;
					OnPropertyChanged();
				}
			}
		}

		private double _guideRateRightAscension;

		public double GuideRateRightAscension
		{
			get { return _guideRateRightAscension; }
			set
			{
				if ( value != _guideRateRightAscension )
				{
					GetExceptionFromManager();
					_guideRateRightAscension = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isPulseGuiding;

		public bool IsPulseGuiding
		{
			get { return _isPulseGuiding; }
			set
			{
				if ( value != _isPulseGuiding )
				{
					GetExceptionFromManager();
					_isPulseGuiding = value;
					OnPropertyChanged();
				}
			}
		}

		private double _rightAscension;

		public double RightAscension
		{
			get { return _rightAscension; }
			set
			{
				if ( value != _rightAscension )
				{
					GetExceptionFromManager();
					_rightAscension = value;
					OnPropertyChanged();
				}
			}
		}

		private double _rightAscensionRate;

		public double RightAscensionRate
		{
			get { return _rightAscensionRate; }
			set
			{
				if ( value != _rightAscensionRate )
				{
					GetExceptionFromManager();
					_rightAscensionRate = value;
					OnPropertyChanged();
				}
			}
		}

		private PierSide _sideOfPier;

		public PierSide SideOfPier
		{
			get { return _sideOfPier; }
			set
			{
				if ( value != _sideOfPier )
				{
					GetExceptionFromManager();
					_sideOfPier = value;
					OnPropertyChanged();
				}
			}
		}

		private double _siderealTime;

		public double SiderealTime
		{
			get { return _siderealTime; }
			set
			{
				GetExceptionFromManager();
				if ( value != _siderealTime )
				{
					_siderealTime = value;
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
					GetExceptionFromManager();
					_slewing = value;
					OnPropertyChanged();
				}
			}
		}

		private double _targetDeclination;

		public double TargetDeclination
		{
			get
			{
				//if ( Double.IsNaN( _targetDeclination ))
				//{
				//	throw new DriverAccessCOMException( "TargetDeclination attempted read before write", unchecked((int)0x80040402), null );
				//}

				return _targetDeclination;
			}

			set
			{
				if ( value != _targetDeclination )
				{
					GetExceptionFromManager();
					_targetDeclination = value;
					OnPropertyChanged();
				}
			}
		}

		private double _targetRightAscension;

		public double TargetRightAscension
		{
			get
			{
				//if ( Double.IsNaN( _targetRightAscension) )
				//{
				//	throw new DriverAccessCOMException( "TargetDeclination attempted read before write", unchecked((int)0x80040402), null );
				//}

				return _targetRightAscension;
			}
			set
			{
				if ( value != _targetRightAscension )
				{
					GetExceptionFromManager();
					_targetRightAscension = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _tracking;

		public bool Tracking
		{
			get { return _tracking; }
			set
			{
				if ( value != _tracking )
				{
					GetExceptionFromManager();
					_tracking = value;
					OnPropertyChanged();
				}
			}
		}

		private DriveRates _trackingRate;

		public DriveRates TrackingRate
		{
			get { return _trackingRate; }
			set
			{
				if ( value != _trackingRate )
				{
					GetExceptionFromManager();
					_trackingRate = value;
					OnPropertyChanged();
				}
			}
		}

		private DateTime _utcDate;

		public DateTime UTCDate
		{
			get { return _utcDate; }
			set
			{
				if ( value != _utcDate )
				{
					GetExceptionFromManager();
					_utcDate = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion

		#region Protected Methods

		public virtual void Clean()
		{
			Altitude = Double.NaN;
			AtHome = false;
			AtPark = false;
			Azimuth = Double.NaN;
			Connected = false;
			Declination = Double.NaN;
			DeclinationRate = Double.NaN;
			GuideRateDeclination = Double.NaN;
			GuideRateRightAscension = Double.NaN;
			IsPulseGuiding = false;
			RightAscension = Double.NaN;
			RightAscensionRate = Double.NaN;
			SideOfPier = PierSide.pierUnknown;
			SiderealTime = Double.NaN;
			Slewing = false;
			TargetDeclination = Double.NaN;
			TargetRightAscension = Double.NaN;
			Tracking = false;
			TrackingRate = DriveRates.driveSidereal;
			UTCDate = DateTime.Now.ToUniversalTime();
		}

		#endregion Protected Methods
	}
}
