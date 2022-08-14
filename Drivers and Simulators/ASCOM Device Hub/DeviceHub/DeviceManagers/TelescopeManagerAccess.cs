using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DeviceHub
{
	// This portion of the TelescopeManager class exposes pass-thru methods to allow a client app
	// and the DeviceHub telescope driver to issue commands to the connected telescope driver.

	// The methods and properties also provide live activity logging and access the connected telescope
	// via an implementation of the ITelescopeService in order to facilitate unit testing of the class.

	public partial class TelescopeManager
	{
		public static string Choose( string id )
		{
			Chooser chooser = new Chooser
			{
				DeviceType = "Telescope"
			};

			return chooser.Choose( id );
		}

		private ITelescopeService _service;

		private ITelescopeService Service
		{
			get
			{
				if ( _service == null )
				{
					_service = ServiceContainer.Instance.GetService<ITelescopeService>();
				}

				return _service;
			}
			set => _service = value;
		}

		public void InitializeTelescopeService( string id )
		{
			if ( Service.DeviceCreated )
			{
				Service.Dispose();
				Service = null;
			}

			// This creates the service and defines the driver ID.

			Service.CreateDevice( id );
		}

		private void ReleaseTelescopeService()
		{
			_service.Dispose();
			_service = null;
		}

		#region ITelescopeV3 Properties

		public bool Connected
		{
			get
			{
				bool retval = false;
				string msgEnd = "";
				Exception except = null;

				ActivityMessageTypes msgType = ActivityMessageTypes.Other;

				try
				{
					if ( CheckDevice( false, false ) )
					{
						retval = Service.Connected;
						msgEnd = retval.ToString();
					}
				}
				catch ( Exception xcp )
				{
					except = xcp;
					msgEnd = Failed;

					throw;
				}
				finally
				{
					LogActivityLine( msgType, "Get Connected flag - {0}", msgEnd );

					if ( except != null )
					{
						LogActivityLine( msgType, $"{except}" );
					}
				}

				return retval;
			}
			set
			{
				ActivityMessageTypes msgType = ActivityMessageTypes.Other;
				LogActivityStart( msgType, "Set Connected flag -> {0}", value );

				try
				{
					bool isConnected = Service.Connected;

					if ( value != isConnected )
					{
						CheckDevice( false );
						Service.Connected = value;

						// Poll the device for up to ten seconds or until it reports that
						// disconnect/connect is complete.

						int i = 0;
						int numTries = 20;
						int msDelay = 500;

						while ( i < numTries )
						{
							Thread.Sleep( msDelay );
							isConnected = Service.Connected;

							if ( value == isConnected )
							{
								break;
							}

							++i;
						}

						if ( value == isConnected )
						{
							LogActivityEnd( msgType, value ? "(connected)" : "(disconnected)" );
							LogActivityLine( msgType, "{0} took {1} milliseconds."
											, ( value ) ? "Connection" : "Disconnection"
											, i * msDelay );
						}
						else
						{
							LogActivityEnd( msgType, Failed );
							LogActivityLine( msgType, "{0} failed after {1} milliseconds"
											, ( value ) ? "Connection" : "Disconnection"
											, numTries * msDelay );
						}
					}
					else
					{
						LogActivityEnd( msgType, "(no change)" );
					}
				}
				catch ( Exception xcp )
				{
					LogActivityEnd( msgType, Failed );
					LogActivityLine( msgType, xcp.ToString() );

					throw;
				}
			}
		}

		public string Description => GetServiceProperty<string>( () => Service.Description, "N/A", ActivityMessageTypes.Parameters );

		public string DriverInfo => GetServiceProperty<string>( () => Service.DriverInfo, "N/A", ActivityMessageTypes.Parameters );

		public string DriverVersion => GetServiceProperty<string>( () => Service.DriverVersion, "N/A", ActivityMessageTypes.Parameters );

		public short InterfaceVersion => GetServiceProperty<short>( () => Service.InterfaceVersion, 0, ActivityMessageTypes.Parameters );

		public string Name => GetServiceProperty<string>( () => Service.Name, "N/A", ActivityMessageTypes.Other );

		public ArrayList SupportedActions
		{
			get
			{
				ArrayList emptyList = new ArrayList();
				return GetServiceProperty<ArrayList>( () => Service.SupportedActions, emptyList, ActivityMessageTypes.Parameters );
			}
		}

		public AlignmentModes AlignmentMode => GetServiceProperty<AlignmentModes>( () => Service.AlignmentMode, AlignmentModes.algAltAz
															, ActivityMessageTypes.Parameters );

		public double Altitude => GetServiceProperty<double>( () => Service.Altitude, Double.NaN );

		public double ApertureArea => GetServiceProperty<double>( () => Service.ApertureArea, Double.NaN, ActivityMessageTypes.Parameters );

		public double ApertureDiameter => GetServiceProperty<double>( () => Service.ApertureDiameter, Double.NaN, ActivityMessageTypes.Parameters );

		public bool AtHome => GetServiceProperty<bool>( () => Service.AtHome, false );

		public bool AtPark => GetServiceProperty<bool>( () => Service.AtPark, false );

		public double Azimuth => GetServiceProperty<double>( () => Service.Azimuth, Double.NaN );

		public bool CanFindHome => GetServiceProperty<bool>( () => Service.CanFindHome, false, ActivityMessageTypes.Capabilities );

		public bool CanPark => GetServiceProperty<bool>( () => Service.CanPark, false, ActivityMessageTypes.Capabilities );

		public bool CanPulseGuide => GetServiceProperty<bool>( () => Service.CanPulseGuide, false, ActivityMessageTypes.Capabilities );

		public bool CanSetDeclinationRate => GetServiceProperty<bool>( () => Service.CanSetDeclinationRate, false, ActivityMessageTypes.Capabilities );

		public bool CanSetGuideRates => GetServiceProperty<bool>( () => Service.CanSetGuideRates, false, ActivityMessageTypes.Capabilities );

		public bool CanSetPark => GetServiceProperty<bool>( () => Service.CanSetPark, false, ActivityMessageTypes.Capabilities );

		public bool CanSetPierSide => GetServiceProperty<bool>( () => Service.CanSetPierSide, false, ActivityMessageTypes.Capabilities );

		public bool CanSetRightAscensionRate => GetServiceProperty<bool>( () => Service.CanSetRightAscensionRate, false, ActivityMessageTypes.Capabilities );

		public bool CanSetTracking => GetServiceProperty<bool>( () => Service.CanSetTracking, false, ActivityMessageTypes.Capabilities );

		public bool CanSlew => GetServiceProperty<bool>( () => Service.CanSlew, false, ActivityMessageTypes.Capabilities );

		public bool CanSlewAltAz => GetServiceProperty<bool>( () => Service.CanSlewAltAz, false, ActivityMessageTypes.Capabilities );

		public bool CanSlewAltAzAsync => GetServiceProperty<bool>( () => Service.CanSlewAltAzAsync, false, ActivityMessageTypes.Capabilities );

		public bool CanSlewAsync => GetServiceProperty<bool>( () => Service.CanSlewAsync, false, ActivityMessageTypes.Capabilities );

		public bool CanSync => GetServiceProperty<bool>( () => Service.CanSync, false, ActivityMessageTypes.Capabilities );

		public bool CanSyncAltAz => GetServiceProperty<bool>( () => Service.CanSyncAltAz, false, ActivityMessageTypes.Capabilities );

		public bool CanUnpark => GetServiceProperty<bool>( () => Service.CanUnpark, false, ActivityMessageTypes.Capabilities );

		public double Declination => GetServiceProperty<double>( () => Service.Declination, Double.NaN );

		public double DeclinationRate
		{
			get => GetServiceProperty<double>( () => Service.DeclinationRate, Double.NaN );
			set => SetServiceProperty<double>( () =>
				   {
					   Service.DeclinationRate = value;

					   if ( Status != null )
					   {
						   Status.DeclinationRate = value;
						   Status.ClearException();
					   }
				   }, value );
		}

		public bool DoesRefraction
		{
			get => GetServiceProperty<bool>( () => Service.DoesRefraction, false, ActivityMessageTypes.Parameters );
			set
			{
				SetServiceProperty<bool>( () => Service.DoesRefraction = value, value, ActivityMessageTypes.Parameters );
				ForceParameterUpdate = true;
			}
		}

		public EquatorialCoordinateType EquatorialSystem => GetServiceProperty<EquatorialCoordinateType>( () => Service.EquatorialSystem
														, EquatorialCoordinateType.equTopocentric, ActivityMessageTypes.Parameters );

		public double FocalLength => GetServiceProperty<double>( () => Service.FocalLength, Double.NaN, ActivityMessageTypes.Parameters );

		public double GuideRateDeclination
		{
			get => GetServiceProperty<double>( () => Service.GuideRateDeclination, Double.NaN );
			set => SetServiceProperty<double>( () =>
				   {
					   Service.GuideRateDeclination = value;

					   if ( Status != null )
					   {
						   Status.GuideRateDeclination = value;
						   Status.ClearException();
					   }
				   }, value );
		}

		public double GuideRateRightAscension
		{
			get => GetServiceProperty<double>( () => Service.GuideRateRightAscension, Double.NaN );
			set => SetServiceProperty<double>( () =>
				   {
					   Service.GuideRateRightAscension = value;

					   if ( Status != null )
					   {
						   Status.GuideRateRightAscension = value;
						   Status.ClearException();
					   }
				   }, value );
		}

		public bool IsPulseGuiding => GetServiceProperty<bool>( () => Service.IsPulseGuiding, false );

		public double RightAscension => GetServiceProperty<double>( () => Service.RightAscension, Double.NaN );

		public double RightAscensionRate
		{
			get => GetServiceProperty<double>( () => Service.RightAscensionRate, Double.NaN );
			set => SetServiceProperty<double>( () =>
				   {
					   Service.RightAscensionRate = value;

					   if ( Status != null )
					   {
						   Status.RightAscensionRate = value;
						   Status.ClearException();
					   }
				   }, value );
		}

		public PierSide SideOfPier
		{
			get => GetServiceProperty<PierSide>( () => Service.SideOfPier, PierSide.pierUnknown );
			set => SetServiceProperty<PierSide>( () =>
				   {
					   Service.SideOfPier = value;

					   if ( Status != null )
					   {
						   Status.SideOfPier = value;
						   Status.ClearException();
					   }
				   }, value );
		}

		public double SiderealTime => GetServiceProperty<double>( () => Service.SiderealTime, Double.NaN );

		public double SiteElevation
		{
			get => GetServiceProperty<double>( () => Service.SiteElevation, Double.NaN, ActivityMessageTypes.Parameters );
			set
			{
				SetServiceProperty<double>( () => Service.SiteElevation = value, value, ActivityMessageTypes.Parameters );
				ForceParameterUpdate = true;
			}
		}

		public double SiteLatitude
		{
			get => GetServiceProperty<double>( () => Service.SiteLatitude, Double.NaN, ActivityMessageTypes.Parameters );
			set
			{
				SetServiceProperty<double>( () => Service.SiteLatitude = value, value, ActivityMessageTypes.Parameters );
				ForceParameterUpdate = true;
			}
		}

		public double SiteLongitude
		{
			get => GetServiceProperty<double>( () => Service.SiteLongitude, Double.NaN, ActivityMessageTypes.Parameters );
			set
			{
				SetServiceProperty<double>( () => Service.SiteLongitude = value, value, ActivityMessageTypes.Parameters );
				ForceParameterUpdate = true;
			}
		}

		public bool Slewing => GetServiceProperty<bool>( () => Service.Slewing, false );

		public short SlewSettleTime
		{
			get => GetServiceProperty<short>( () => Service.SlewSettleTime, 0, ActivityMessageTypes.Parameters );
			set
			{
				SetServiceProperty<short>( () => Service.SlewSettleTime = value, value, ActivityMessageTypes.Parameters );
				ForceParameterUpdate = true;
			}
		}

		public double TargetDeclination
		{
			get => GetServiceProperty<double>( () => Service.TargetDeclination, Double.NaN );
			set => SetServiceProperty<double>( () =>
				   {
					   Service.TargetDeclination = value;

					   if ( Status != null )
					   {
						   Status.TargetDeclination = value;
						   Status.ClearException();
					   }
				   }, value );
		}

		public double TargetRightAscension
		{
			get => GetServiceProperty<double>( () => Service.TargetRightAscension, Double.NaN );
			set => SetServiceProperty<double>( () =>
				   {
					   Service.TargetRightAscension = value;

					   if ( Status != null )
					   {
						   Status.TargetRightAscension = value;
						   Status.ClearException();
					   }
				   }, value );
		}

		public bool Tracking
		{
			get => GetServiceProperty<bool>( () => Service.Tracking, false );
			set => SetServiceProperty<bool>( () =>
				   {
					   Service.Tracking = value;

					   if ( Status != null )
					   {
						   Status.Tracking = value;
						   Status.ClearException();
					   }
				   }, value );
		}

		public DriveRates TrackingRate
		{
			get => GetServiceProperty<DriveRates>( () => Service.TrackingRate, DriveRates.driveSidereal );
			set => SetServiceProperty<DriveRates>( () =>
				   {
					   Service.TrackingRate = value;

					   if ( Status != null )
					   {
						   Status.TrackingRate = value;
						   Status.ClearException();
					   }
				   }, value );
		}

		public ITrackingRates TrackingRates
		{
			get
			{
				DriveRates[] driveRates = new DriveRates[] { DriveRates.driveSidereal };
				ScopeTrackingRates defaultRates = new ScopeTrackingRates( driveRates );
				ITrackingRates rates =  GetServiceProperty<ITrackingRates>( () => Service.TrackingRates, (ITrackingRates)defaultRates, ActivityMessageTypes.Parameters );

				return rates;
			}
		}

		public DateTime UTCDate
		{
			get => GetServiceProperty<DateTime>( () => Service.UTCDate, DateTime.MinValue );
			set => SetServiceProperty<DateTime>( () =>
				   {
					   Service.UTCDate = value;

					   if ( Status != null )
					   {
						   Status.UTCDate = value;
						   Status.ClearException();
					   }
				   }, value );
		}

		#endregion

		#region ITelescopeV3 Methods 

		public string Action( string actionName, string actionParameters )
		{
			string retval = "";

			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			Exception except = null;
			string msgEnd = "";

			try
			{
				CheckDevice();
				retval = Service.Action( actionName, actionParameters );
				msgEnd = $" returned {retval}";
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = "{Failed}. Details follow:";

				throw;
			}
			finally
			{
				LogActivityLine( msgType, $"Action ({actionName}) {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}
			}

			return retval;
		}

		public void AbortSlew()
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			string msgEnd = "";
			Exception except = null;

			try
			{
				CheckDevice();
				Service.AbortSlew();
				msgEnd = Done;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = $"{Failed}. Details follow:";

				throw;
			}
			finally
			{
				LogActivityLine( msgType, "AbortSlew - {0}", msgEnd );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );	
				}

				Status = new DevHubTelescopeStatus( this );
			}
		}

		public IAxisRates AxisRates( TelescopeAxes axis )
		{
			IAxisRates retval;

			Exception except = null;
			ActivityMessageTypes messageType = ActivityMessageTypes.Capabilities;

			string axisName = GetNameFromAxis( axis );
			string msg = $"Calling AxisRates( {axisName} )";

			try
			{
				CheckDevice();

				if ( Service.CanMoveAxis( axis ))
				{
					retval = Service.AxisRates( axis );
				}
				else
				{
					IRate[] rateArr = new IRate[0];
					retval = new ScopeAxisRates( rateArr );
				}

				msg += Done;
			}
			catch (Exception xcp )
			{
				except = xcp;
				msg += Failed;

				// Return an empty IAxisRates collection

				IRate[] rateArr = new IRate[0];
				retval = new ScopeAxisRates( rateArr );
			}
			finally
			{
				LogActivityLine( messageType, msg );

				if ( except != null )
				{
					LogActivityLine( messageType, "AxisRates( {0} ) Exception: {1}", axisName, except.Message );
				}
			}

			return retval;
		}

		public bool CanMoveAxis( TelescopeAxes axis )
		{
			bool retval = false;
			Exception except = null;
			ActivityMessageTypes messageType = ActivityMessageTypes.Capabilities;

			string msg = $"Calling CanMoveAxis( {GetNameFromAxis( axis )} ) - ";

			try
			{
				CheckDevice();
				retval = Service.CanMoveAxis( axis );
				msg += $"returned {retval}.";
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msg += $"{Failed}. Details follow:";

				throw;
			}
			finally
			{
				LogActivityLine( messageType, msg );

				if ( except != null )
				{
					LogActivityLine( messageType, $"{except}" );
				}
			}

			return retval;
		}

		public void CommandBlind( string command, bool raw = false )
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			Exception except = null;
			string msgEnd = "";

			try
			{
				CheckDevice();
				Service.CommandBlind( command, raw );
				msgEnd = Done;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = $"{Failed}. Details Follow:";

				throw;
			}
			finally
			{
				LogActivityLine( msgType, $"CommandBlind( {command} ) - {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}
			}
		}

		public bool CommandBool( string command, bool raw = false )
		{
			bool retval;

			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			Exception except = null;
			string msgEnd = "";

			try
			{
				CheckDevice();
				retval = Service.CommandBool( command, raw );
				msgEnd = $"returned {retval}.";
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = $"{Failed}. Details Follow:";

				throw;
			}
			finally
			{
				LogActivityLine( msgType, $"CommandBool( {command} ) - {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}
			}

			return retval;
		}

		public string CommandString( string command, bool raw = false )
		{
			string retval;

			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			Exception except = null;
			string msgEnd = "";

			try
			{
				CheckDevice();
				retval = Service.CommandString( command, raw );
				msgEnd = $"returned {retval}.";
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = $"{Failed}. Details Follow:";

				throw;
			}
			finally
			{
				LogActivityLine( msgType, $"CommandString( {command} ) - {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}
			}

			return retval;
		}

		public PierSide DestinationSideOfPier( double rightAscension, double declination )
		{
			PierSide retval = PierSide.pierUnknown;

			ActivityMessageTypes msgType = ActivityMessageTypes.Other;
			Exception except = null;
			string msgEnd = "";

			try
			{
				CheckDevice();
				retval = Service.DestinationSideOfPier( rightAscension, declination );
				string name = GetPierSideName( retval );
				msgEnd = $"returned {name}.";
			} 
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = $"{Failed}. Details follow:";
			}
			finally 
			{
				RightAscensionConverter raConverter = new RightAscensionConverter( (decimal)rightAscension );
				DeclinationConverter decConverter = new DeclinationConverter( (decimal)declination );

				LogActivityLine( msgType, $"DestinationSideOfPier RA = {raConverter}, Dec = {decConverter}: {msgEnd}." );

				if ( except != null)
				{
					LogActivityLine( msgType, "${except}" );
				}
			}

			return retval;
		}

		public void Dispose()
		{
			Service.Dispose();
			Service = null;
			TelescopeManager.Instance = null;
		}

		public void FindHome()
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			Exception except = null;
			string msgEnd = "";

			try
			{
				CheckDevice();
				Status.Slewing = true;
				Service.FindHome();
				msgEnd = Done;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = Failed;

				throw;
			}
			finally
			{
				LogActivityLine( msgType, $"FindHome - {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}

				Status = new DevHubTelescopeStatus( this );
			}
		}

		public void MoveAxis( TelescopeAxes axis, double rate )
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			Exception except = null;
			string msgEnd = "";

			try
			{
				CheckDevice();
				Service.MoveAxis( axis, rate );
				msgEnd = Done;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = Failed;

				throw;
			}
			finally 
			{
				string name = GetNameFromAxis( axis );
				LogActivityLine( msgType, $"MoveAxis {name} at {rate} deg/sec: {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}

				Status = new DevHubTelescopeStatus( this );
			}
		}

		public void Park()
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			Exception except = null;
			string msgEnd = "";

			try
			{
				CheckDevice();
				Service.Park();
				msgEnd = Done;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = $"{Failed}. Details follow:";

				throw;
			}
			finally
			{
				LogActivityLine( msgType, $"Park - {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}

				Status = new DevHubTelescopeStatus( this );
			}
		}

		public void PulseGuide( GuideDirections direction, int duration )
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			Exception except = null;
			string msgEnd = "";
			string name = GetGuideDirectionName( direction );

			try
			{
				CheckDevice();
				DateTime startTime = DateTime.Now;
				Service.PulseGuide( direction, duration );

				// Pulse guiding can be synchronous or asynchronous. If it is the former then
				// we will not see IsPulseGuiding set to true.

				if ( Service.IsPulseGuiding )
				{
					Status.IsPulseGuiding = true;
					Status.Slewing = Service.Slewing;
					DateTime endTime = startTime.AddMilliseconds( duration );
					Task.Run( () => MonitorPulseGuidingTask( endTime ) );
				}
				else
				{
					Status.IsPulseGuiding = false;
				}

				msgEnd = Done;
			}
			catch ( Exception xcp)
			{
				except = xcp;
				msgEnd = Failed;

				throw;
			}
			finally
			{
				LogActivityLine( msgType, $"PulseGuide {name} for {duration} ms: {msgEnd}." );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}");
				}
			}
		}

		public void SetPark()
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			string msgEnd = "";
			Exception except = null;

			try
			{
				CheckDevice();
				Service.SetPark();
				msgEnd = Done;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = $"{Failed}. Details follow:";

				throw;
			}
			finally
			{
				LogActivityLine( msgType, $"SetPark: {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}
			}
		}

		public void SlewToAltAz( double azimuth, double altitude )
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			Exception except = null;
			string msgEnd = "";

			try
			{
				CheckDevice();
				Status.Slewing = true;
				Service.SlewToAltAz( azimuth, altitude );
				msgEnd = Done;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = $"{Failed}. Details follow:";

				throw;
			}
			finally
			{
				AzimuthConverter azConverter = new AzimuthConverter( (decimal)azimuth );
				AltitudeConverter altConverter = new AltitudeConverter( (decimal)altitude );

				LogActivityLine( msgType, "SlewToAltAz - Az = {azConverter}, Alt = {altConverter}: {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}

				Status = new DevHubTelescopeStatus( this );
			}
		}

		public void SlewToAltAzAsync( double azimuth, double altitude )
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			Exception except = null;
			string msgEnd = "";

			try
			{
				CheckDevice();
				Service.SlewToAltAzAsync( azimuth, altitude );
				msgEnd = SlewStarted;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = $"{Failed}. Details follow";

				throw;
			}
			finally
			{
				AzimuthConverter azConverter = new AzimuthConverter( (decimal)azimuth );
				AltitudeConverter altConverter = new AltitudeConverter( (decimal)altitude );
				
				LogActivityLine( msgType, $"SlewToAltAzAsync - Az = {azConverter}, Alt = {altConverter}: {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}

				Status = new DevHubTelescopeStatus( this );
			}
		}

		public void SlewToCoordinates( double rightAscension, double declination )
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			Exception except = null;
			string msgEnd = "";

			try
			{
				CheckDevice();
				Status.Slewing = true;
				Service.SlewToCoordinates( rightAscension, declination );
				msgEnd = Done;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = $"{Failed}. Details follow";

				throw;
			}
			finally
			{
				RightAscensionConverter raConverter = new RightAscensionConverter( (decimal)rightAscension );
				DeclinationConverter decConverter = new DeclinationConverter( (decimal)declination );

				LogActivityLine( msgType, $"SlewToCoordinates - RA = {raConverter}, Dec = {decConverter}: {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}

				Status = new DevHubTelescopeStatus( this );
			}
		}

		public void SlewToCoordinatesAsync( double rightAscension, double declination )
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			Exception except = null;
			string msgEnd = "";

			try
			{
				CheckDevice();
				Service.SlewToCoordinatesAsync( rightAscension, declination );
				msgEnd = SlewStarted;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = $"{Failed}. Details follow";

				throw;
			}
			finally
			{
				RightAscensionConverter raConverter = new RightAscensionConverter( (decimal)rightAscension );
				DeclinationConverter decConverter = new DeclinationConverter( (decimal)declination );

				LogActivityLine( msgType, $"SlewToCoordinatesAsync - RA = {raConverter}, Dec {decConverter}: {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}

				Status = new DevHubTelescopeStatus( this );
			}
		}

		public void SlewToTarget()
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			Exception except = null;
			string msgEnd = "";

			try
			{
				CheckDevice();
				Status.Slewing = true;
				Service.SlewToTarget();
				msgEnd = Done;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = $"{Failed}. Details follow";

				throw;
			}
			finally
			{
				LogActivityLine( msgType, $"SlewToTarget: {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}

				Status = new DevHubTelescopeStatus( this );
			}
		}

		public void SlewToTargetAsync()
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			Exception except = null;
			string msgEnd = "";

			try
			{
				CheckDevice();
				Service.SlewToTargetAsync();
				msgEnd = SlewStarted;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = $"{Failed}. Details follow";

				throw;
			}
			finally
			{
				LogActivityLine( msgType, "SlewToTargetAsync: {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}

				Status = new DevHubTelescopeStatus( this );
			}
		}

		public void SyncToAltAz( double azimuth, double altitude )
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			Exception except = null;
			string msgEnd = "";

			try
			{
				CheckDevice();
				Service.SyncToAltAz( azimuth, altitude );
				msgEnd = Done;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = $"{Failed}. Details follow";

				throw;
			}
			finally
			{
				AzimuthConverter azConverter = new AzimuthConverter( (decimal)azimuth );
				AltitudeConverter altConverter = new AltitudeConverter( (decimal)altitude );

				LogActivityLine( msgType, "SyncToAltAz - Az = {azConverter}, Alt = {altConverter}: {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}

				Status = new DevHubTelescopeStatus( this );
			}
		}

		public void SyncToCoordinates( double rightAscension, double declination )
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			Exception except = null;
			string msgEnd = "";

			try
			{
				CheckDevice();
				Service.SyncToCoordinates( rightAscension, declination );
				msgEnd = Done;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = $"{Failed}. Details follow:";
			}
			finally
			{
				RightAscensionConverter raConverter = new RightAscensionConverter( (decimal)rightAscension );
				DeclinationConverter decConverter = new DeclinationConverter( (decimal)declination );

				LogActivityLine( msgType, $"SyncToCoordinates - RA = {raConverter}, Dec = {decConverter} {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}

				Status = new DevHubTelescopeStatus( this );


			}
		}

		public void SyncToTarget()
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			Exception except = null;
			string msgEnd = "";

			try
			{
				CheckDevice();
				Service.SyncToTarget( );
				msgEnd = Done;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = $"{Failed}. Details follow:";
			}
			finally
			{
				LogActivityLine( msgType, $"SyncToTarget: {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}

				Status = new DevHubTelescopeStatus( this );
			}

		}

		public void Unpark()
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			Exception except = null;
			string msgEnd = "";

			try
			{
				CheckDevice();
				Service.Unpark();
				msgEnd = Done;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = $"{Failed}. Details follow:";
			}
			finally
			{
				LogActivityLine( msgType, $"Unpark: {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}

				Status = new DevHubTelescopeStatus( this );
			}
		}

		#endregion

		#region Helper Methods

		protected override void CheckDevice()
		{
			CheckDevice( true, true );
		}

		private void CheckDevice( bool testConnected )
		{
			CheckDevice( testConnected, true );
		}

		private bool CheckDevice( bool testConnected, bool throwException )
		{
			bool retval;

			if ( Service == null || !Service.DeviceCreated )
			{
				if ( throwException )
				{
					throw new NullReferenceException( "The Telescope object is null." );
				}

				retval = false;
			}
			else
			{
				retval = true;
			}

			if ( retval && testConnected && !Service.Connected )
			{
				if ( throwException )
				{
					throw new NotConnectedException( "There is no connected telescope." );
				}

				retval = false;
			}

			return retval;
		}

		#endregion Helper Methods
	}
}
