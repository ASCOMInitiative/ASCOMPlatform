using System;
using System.Collections;

using ASCOM.DeviceInterface;
using ASCOM.DriverAccess;

namespace ASCOM.DeviceHub
{
	public class TelescopeService : ITelescopeService
	{
		private string DeviceID { get; set; }
		private Telescope Telescope { get; set; }

		public bool DeviceCreated => Initialized && Telescope != null;
		public bool DeviceAvailable => DeviceCreated && Connected;
        public bool Initialized { get; private set; }

        public void CreateDevice( string id )
		{
			if ( Initialized )
			{
				throw new Exception( "The telescope service attempted to re-initialize the telescope." );
			}

			if ( Telescope == null )
			{
				if ( String.IsNullOrEmpty( id ) )
				{
					throw new Exception( "The telescope service is unable to create a telescope until a Telescope has been chosen." );
				}

				DeviceID = id;

				try
				{
					Telescope = new Telescope( DeviceID );
					Initialized = true;
				}
				catch ( Exception xcp )
				{
					throw new Exception( "Unable to create the Telescope object", xcp );
				}
			}
		}

		#region ITelescopeV3 Properties

		public bool Connected
		{
			get
			{
				bool retval = false;

				try
				{
					retval = Telescope.Connected;
				}
				catch (DriverException )
				{
					throw;
				}
				catch ( Exception xcp )
				{
					throw new DriverException( "Unable to read Telescope.Connected property", xcp );
				}

				return retval;
			}
			set
			{
				try
				{
					Telescope.Connected = value;
				}
				catch ( DriverException )
				{
					throw;
				}
				catch ( Exception xcp )
				{
					throw new DriverException( "Unable to write Telescope.Connected property", xcp );
				}
			}
		}

		public string Description => Telescope.Description;
		public string DriverInfo => Telescope.DriverInfo;
		public string DriverVersion => Telescope.DriverVersion;
		public short InterfaceVersion => Telescope.InterfaceVersion;
		public string Name => Telescope.Name;

		public ArrayList SupportedActions => Telescope.SupportedActions;
		public AlignmentModes AlignmentMode => Telescope.AlignmentMode;
		public double Altitude => Telescope.Altitude;
		public double ApertureArea => Telescope.ApertureArea;
		public double ApertureDiameter => Telescope.ApertureDiameter;
		public bool AtHome => Telescope.AtHome;
		public bool AtPark => Telescope.AtPark;
		public double Azimuth => Telescope.Azimuth;
		public bool CanFindHome => Telescope.CanFindHome;
		public bool CanPark => Telescope.CanPark;
		public bool CanPulseGuide => Telescope.CanPulseGuide;
		public bool CanSetDeclinationRate => Telescope.CanSetDeclinationRate;
		public bool CanSetGuideRates => Telescope.CanSetGuideRates;
		public bool CanSetPark => Telescope.CanSetPark;
		public bool CanSetPierSide => Telescope.CanSetPierSide;
		public bool CanSetRightAscensionRate => Telescope.CanSetRightAscensionRate;
		public bool CanSetTracking =>Telescope.CanSetTracking;
		public bool CanSlew => Telescope.CanSlew;
		public bool CanSlewAltAz => Telescope.CanSlewAltAz;
		public bool CanSlewAltAzAsync => Telescope.CanSlewAltAzAsync;
		public bool CanSlewAsync => Telescope.CanSlewAsync;
		public bool CanSync => Telescope.CanSync;
		public bool CanSyncAltAz => Telescope.CanSyncAltAz;
		public bool CanUnpark => Telescope.CanUnpark;
		public double Declination => Telescope.Declination;
		public double DeclinationRate
		{
			get => Telescope.DeclinationRate;
			set => Telescope.DeclinationRate = value;
		}
		public bool DoesRefraction
		{
			get => Telescope.DoesRefraction;
			set => Telescope.DoesRefraction = value;
		}
		public EquatorialCoordinateType EquatorialSystem => Telescope.EquatorialSystem;
		public double FocalLength => Telescope.FocalLength;
		public double GuideRateDeclination
		{
			get => Telescope.GuideRateDeclination;
			set => Telescope.GuideRateDeclination = value;
		}
		public double GuideRateRightAscension
		{
			get => Telescope.GuideRateRightAscension;
			set => Telescope.GuideRateRightAscension = value;
		}
		public bool IsPulseGuiding => Telescope.IsPulseGuiding;
		public double RightAscension => Telescope.RightAscension;
		public double RightAscensionRate
		{
			get => Telescope.RightAscensionRate;
			set => Telescope.RightAscensionRate = value;
		}

		public PierSide SideOfPier
		{
			get => Telescope.SideOfPier;
			set => Telescope.SideOfPier = value;
		}
		public double SiderealTime => Telescope.SiderealTime;
		public double SiteElevation
		{
			get => Telescope.SiteElevation;
			set => Telescope.SiteElevation = value;
		}
		public double SiteLatitude
		{
			get => Telescope.SiteLatitude;
			set => Telescope.SiteLatitude = value;
		}
		public double SiteLongitude
		{
			get => Telescope.SiteLongitude;
			set => Telescope.SiteLongitude = value;
		}
		public bool Slewing => Telescope.Slewing;
		public short SlewSettleTime
		{
			get => Telescope.SlewSettleTime;
			set => Telescope.SlewSettleTime = value;
		}
		public double TargetDeclination
		{
			get => Telescope.TargetDeclination;
			set => Telescope.TargetDeclination = value;
		}
		public double TargetRightAscension
		{
			get => Telescope.TargetRightAscension;
			set => Telescope.TargetRightAscension = value;
		}
		public bool Tracking
		{
			get => Telescope.Tracking;
			set => Telescope.Tracking = value;
		}
		public DriveRates TrackingRate
		{
			get => Telescope.TrackingRate;
			set => Telescope.TrackingRate = value;
		}
		public ITrackingRates TrackingRates => Telescope.TrackingRates;
		public DateTime UTCDate
		{
			get => Telescope.UTCDate;
			set => Telescope.UTCDate = value;
		}

		#endregion

		#region ITelescopeV3 Methods 

		public string Action( string actionName, string actionParameters )
		{
			return Telescope.Action( actionName, actionParameters );
		}

		public void AbortSlew()
		{
			Telescope.AbortSlew();
		}

		public IAxisRates AxisRates( TelescopeAxes axis )
		{
			return Telescope.AxisRates( axis );
		}

		public bool CanMoveAxis( TelescopeAxes axis )
		{
			return Telescope.CanMoveAxis( axis );
		}

		public void CommandBlind( string command, bool raw = false )
		{
			Telescope.CommandBlind( command, raw );
		}

		public bool CommandBool( string command, bool raw = false )
		{
			return Telescope.CommandBool( command, raw );
		}

		public string CommandString( string command, bool raw = false )
		{
			return Telescope.CommandString( command, raw );
		}

		public PierSide DestinationSideOfPier( double rightAscension, double declination )
		{
			return Telescope.DestinationSideOfPier( rightAscension, declination );
		}

		public void Dispose()
		{
			Telescope.Dispose();
			Telescope = null;
			DeviceID = null;
			Initialized = false;
		}

		public void FindHome()
		{
			Telescope.FindHome();
		}

		public void MoveAxis( TelescopeAxes axis, double rate )
		{
			Telescope.MoveAxis( axis, rate );
		}

		public void Park()
		{
			Telescope.Park();
		}

		public void PulseGuide( GuideDirections direction, int duration )
		{
			Telescope.PulseGuide( direction, duration );
		}

		public void SetPark()
		{
			Telescope.SetPark();
		}

		public void SetupDialog()
		{
			// This method is here to satisfy the ITelescopeV3 interface and should never be called.

			throw new NotImplementedException();
		}

		public void SlewToAltAz( double azimuth, double altitude )
		{
			Telescope.SlewToAltAz( azimuth, altitude );
		}

		public void SlewToAltAzAsync( double azimuth, double altitude )
		{
			Telescope.SlewToAltAzAsync( azimuth, altitude );
		}

		public void SlewToCoordinates( double rightAscension, double declination )
		{
			Telescope.SlewToCoordinates( rightAscension, declination );
		}

		public void SlewToCoordinatesAsync( double rightAscension, double declination )
		{
			Telescope.SlewToCoordinatesAsync( rightAscension, declination );
		}

		public void SlewToTarget()
		{
			Telescope.SlewToTarget();
		}

		public void SlewToTargetAsync()
		{
			Telescope.SlewToTargetAsync();
		}

		public void SyncToAltAz( double azimuth, double altitude )
		{
			Telescope.SyncToAltAz( azimuth, altitude );
		}

		public void SyncToCoordinates( double rightAscension, double declination )
		{
			Telescope.SyncToCoordinates( rightAscension, declination );
		}

		public void SyncToTarget()
		{
			Telescope.SyncToTarget();
		}

		public void Unpark()
		{
			Telescope.Unpark();
		}

		#endregion
	}
}
