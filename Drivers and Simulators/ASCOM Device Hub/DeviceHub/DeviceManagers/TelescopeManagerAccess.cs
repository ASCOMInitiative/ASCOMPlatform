using System;
using System.Collections;
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
			set
			{
				_service = value;
			}
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
						LogActivityLine( msgType, except.ToString() );
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

		public string Description
		{
			get
			{
				return GetServiceProperty<string>( () => Service.Description, "N/A", ActivityMessageTypes.Parameters );
			}
		}

		public string DriverInfo
		{
			get
			{
				return GetServiceProperty<string>( () => Service.DriverInfo, "N/A", ActivityMessageTypes.Parameters );
			}
		}

		public string DriverVersion
		{
			get
			{
				return GetServiceProperty<string>( () => Service.DriverVersion, "N/A", ActivityMessageTypes.Parameters );
			}
		}

		public short InterfaceVersion
		{
			get
			{
				return  GetServiceProperty<short>( () => Service.InterfaceVersion, 0, ActivityMessageTypes.Parameters );
			}
		}

		public string Name
		{
			get
			{
				return  GetServiceProperty<string>( () => Service.Name, "N/A", ActivityMessageTypes.Other );
			}
		}

		public ArrayList SupportedActions
		{
			get
			{
				ArrayList emptyList = new ArrayList();
				return GetServiceProperty<ArrayList>( () => Service.SupportedActions, emptyList, ActivityMessageTypes.Parameters );
			}
		}

		public AlignmentModes AlignmentMode
		{
			get
			{
				return GetServiceProperty<AlignmentModes>( () => Service.AlignmentMode, AlignmentModes.algAltAz
															, ActivityMessageTypes.Parameters );
			}
		}

		public double Altitude
		{
			get
			{
				return GetServiceProperty<double>( () => Service.Altitude, Double.NaN );
			}
		}

		public double ApertureArea
		{
			get
			{
				return GetServiceProperty<double>( () => Service.ApertureArea, Double.NaN, ActivityMessageTypes.Parameters );
			}
		}

		public double ApertureDiameter
		{
			get
			{
				return GetServiceProperty<double>( () => Service.ApertureDiameter, Double.NaN, ActivityMessageTypes.Parameters );
			}
		}

		public bool AtHome
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.AtHome, false );
			}
		}

		public bool AtPark
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.AtPark, false );
			}
		}

		public double Azimuth
		{
			get
			{
				return GetServiceProperty<double>( () => Service.Azimuth, Double.NaN );
			}
		}

		public bool CanFindHome
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanFindHome, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanPark
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanPark, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanPulseGuide
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanPulseGuide, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanSetDeclinationRate
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanSetDeclinationRate, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanSetGuideRates
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanSetGuideRates, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanSetPark
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanSetPark, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanSetPierSide
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanSetPierSide, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanSetRightAscensionRate
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanSetRightAscensionRate, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanSetTracking
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanSetTracking, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanSlew
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanSlew, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanSlewAltAz
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanSlewAltAz, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanSlewAltAzAsync
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanSlewAltAzAsync, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanSlewAsync
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanSlewAsync, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanSync
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanSync, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanSyncAltAz
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanSyncAltAz, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanUnpark
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanUnpark, false, ActivityMessageTypes.Capabilities );
			}
		}

		public double Declination
		{
			get
			{
				return GetServiceProperty<double>( () => Service.Declination, Double.NaN );
			}
		}
		
		public double DeclinationRate
		{
			get
			{
				return GetServiceProperty<double>( () => Service.DeclinationRate, Double.NaN );
			}
			set
			{
				SetServiceProperty<double>( () =>
				{
					Service.DeclinationRate = value;

					if ( Status != null )
					{
						Status.DeclinationRate = value;
						Status.ClearException();
					}
				}, value );
			}
		}

		public bool DoesRefraction
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.DoesRefraction, false, ActivityMessageTypes.Parameters );

			}
			set
			{
				SetServiceProperty<bool>( () => Service.DoesRefraction = value, value, ActivityMessageTypes.Parameters );
				ForceParameterUpdate = true;
			}
		}

		public EquatorialCoordinateType EquatorialSystem
		{
			get
			{
				return GetServiceProperty<EquatorialCoordinateType>( () => Service.EquatorialSystem
														, EquatorialCoordinateType.equTopocentric, ActivityMessageTypes.Parameters );
			}
		}

		public double FocalLength
		{
			get
			{
				return GetServiceProperty<double>( () => Service.FocalLength, Double.NaN, ActivityMessageTypes.Parameters );
			}
		}

		public double GuideRateDeclination
		{
			get
			{
				return GetServiceProperty<double>( () => Service.GuideRateDeclination, Double.NaN );
			}
			set
			{
				SetServiceProperty<double>( () =>
				{
					Service.GuideRateDeclination = value;

					if ( Status != null )
					{
						Status.GuideRateDeclination = value;
						Status.ClearException();
					}
				}, value );
			}
		}

		public double GuideRateRightAscension
		{
			get
			{
				return GetServiceProperty<double>( () => Service.GuideRateRightAscension, Double.NaN );
			}
			set
			{
				SetServiceProperty<double>( () =>
				{
					Service.GuideRateRightAscension = value;

					if ( Status != null )
					{
						Status.GuideRateRightAscension = value;
						Status.ClearException();
					}
				}, value );
			}
		}

		public bool IsPulseGuiding
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.IsPulseGuiding, false );
			}
		}

		public double RightAscension
		{
			get
			{
				return GetServiceProperty<double>( () => Service.RightAscension, Double.NaN );
			}
		}

		public double RightAscensionRate
		{
			get
			{
				return GetServiceProperty<double>( () => Service.RightAscensionRate, Double.NaN );
			}
			set
			{
				SetServiceProperty<double>( () =>
				{
					Service.RightAscensionRate = value;

					if ( Status != null )
					{
						Status.RightAscensionRate = value;
						Status.ClearException();
					}
				}, value );
			}
		}

		public PierSide SideOfPier
		{
			get
			{
				return GetServiceProperty<PierSide>( () => Service.SideOfPier, PierSide.pierUnknown );
			}
			set
			{
				SetServiceProperty<PierSide>( () =>
				{
					Service.SideOfPier = value;

					if ( Status != null )
					{
						Status.SideOfPier = value;
						Status.ClearException();
					}
				}, value );
			}
		}

		public double SiderealTime
		{
			get
			{
				return GetServiceProperty<double>( () => Service.SiderealTime, Double.NaN );
			}
		}

		public double SiteElevation
		{
			get
			{
				return GetServiceProperty<double>( () => Service.SiteElevation, Double.NaN, ActivityMessageTypes.Parameters );
			}
			set
			{
				SetServiceProperty<double>( () => Service.SiteElevation = value, value, ActivityMessageTypes.Parameters );
				ForceParameterUpdate = true;
			}
		}

		public double SiteLatitude
		{
			get
			{
				return GetServiceProperty<double>( () => Service.SiteLatitude, Double.NaN, ActivityMessageTypes.Parameters );
			}
			set
			{
				SetServiceProperty<double>( () => Service.SiteLatitude = value, value, ActivityMessageTypes.Parameters );
				ForceParameterUpdate = true;
			}
		}

		public double SiteLongitude
		{
			get
			{
				return GetServiceProperty<double>( () => Service.SiteLongitude, Double.NaN, ActivityMessageTypes.Parameters );
			}
			set
			{
				SetServiceProperty<double>( () => Service.SiteLongitude = value, value, ActivityMessageTypes.Parameters );
				ForceParameterUpdate = true;
			}
		}

		public bool Slewing
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.Slewing, false );
			}
		}

		public short SlewSettleTime
		{
			get
			{
				return GetServiceProperty<short>( () => Service.SlewSettleTime, 0, ActivityMessageTypes.Parameters );
			}
			set
			{
				SetServiceProperty<short>( () => Service.SlewSettleTime = value, value, ActivityMessageTypes.Parameters );
				ForceParameterUpdate = true;
			}
		}

		public double TargetDeclination
		{
			get
			{
				return GetServiceProperty<double>( () => Service.TargetDeclination, Double.NaN );
			}
			set
			{
				SetServiceProperty<double>( () =>
				{
					Service.TargetDeclination = value;

					if ( Status != null )
					{
						Status.TargetDeclination = value;
						Status.ClearException();
					}
				}, value );
			}
		}

		public double TargetRightAscension
		{
			get
			{
				return GetServiceProperty<double>( () => Service.TargetRightAscension, Double.NaN );
			}
			set
			{
				SetServiceProperty<double>( () =>
				{
					Service.TargetRightAscension = value;

					if ( Status != null )
					{
						Status.TargetRightAscension = value;
						Status.ClearException();
					}
				}, value );
			}
		}

		public bool Tracking
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.Tracking, false );
			}
			set
			{
				SetServiceProperty<bool>( () =>
				{
					Service.Tracking = value;

					if ( Status != null )
					{
						Status.Tracking = value;
						Status.ClearException();
					}
				}, value );
			}
		}

		public DriveRates TrackingRate
		{
			get
			{
				return GetServiceProperty<DriveRates>( () => Service.TrackingRate, DriveRates.driveSidereal );
			}
			set
			{
				SetServiceProperty<DriveRates>( () =>
				{
					Service.TrackingRate = value;

					if ( Status != null )
					{
						Status.TrackingRate = value;
						Status.ClearException();
					}
				}, value );
			}
		}

		public ITrackingRates TrackingRates
		{
			get
			{
				DriveRates[] driveRates = new DriveRates[] { DriveRates.driveSidereal };
				ScopeTrackingRates defaultRates = new ScopeTrackingRates( driveRates );
				ITrackingRates rates =  GetServiceProperty<ITrackingRates>( () => Service.TrackingRates, defaultRates, ActivityMessageTypes.Parameters );

				return rates;
			}
		}

		public DateTime UTCDate
		{
			get
			{
				return GetServiceProperty<DateTime>( () => Service.UTCDate, DateTime.MinValue );
			}
			set
			{
				SetServiceProperty<DateTime>( () =>
				{
					Service.UTCDate = value;

					if ( Status != null )
					{
						Status.UTCDate = value;
						Status.ClearException();
					}
				}, value );
			}
		}

		#endregion

		#region ITelescopeV3 Methods 

		public string Action( string actionName, string actionParameters )
		{
			LogActivityStart( ActivityMessageTypes.Commands, "Action ({0}):", actionName );
			CheckDevice();
			string retval = Service.Action( actionName, actionParameters );
			LogActivityEnd( ActivityMessageTypes.Commands, "returned {0}", retval );

			return retval;
		}

		public void AbortSlew()
		{
			LogActivityStart( ActivityMessageTypes.Commands, "AbortSlew: " );
			CheckDevice();
			Service.AbortSlew();
			LogActivityEnd( ActivityMessageTypes.Commands, Done );
		}

		public IAxisRates AxisRates( TelescopeAxes axis )
		{
			IAxisRates retval;

			Exception xcp = null;
			ActivityMessageTypes messageType = ActivityMessageTypes.Capabilities;

			string axisName = GetNameFromAxis( axis );
			string msg = String.Format( "Calling AxisRates( {0} )", axisName );

			try
			{
				CheckDevice();
				retval = Service.AxisRates( axis );
				msg += Done;
			}
			catch (Exception ex )
			{
				xcp = ex;
				msg += Failed;

				// Return an empty IAxisRates collection

				IRate[] rateArr = new IRate[0];
				retval = new ScopeAxisRates( rateArr );
			}
			finally
			{
				LogActivityLine( messageType, msg );

				if ( xcp != null )
				{
					LogActivityLine( messageType, "AxisRates( {0} ) Exception: {1}", axisName, xcp.Message );
				}
			}

			return retval;
		}

		public bool CanMoveAxis( TelescopeAxes axis )
		{
			bool retval = false;
			Exception xcp = null;
			ActivityMessageTypes messageType = ActivityMessageTypes.Capabilities;

			string axisName = GetNameFromAxis( axis );
			string msg = String.Format( "Calling CanMoveAxis( {0} )", axisName );

			try
			{
				CheckDevice();
				retval = Service.CanMoveAxis( axis );
				msg += retval.ToString();
			}
			catch ( Exception ex )
			{
				xcp = ex;
				msg += Failed;
				throw;
			}

			finally
			{
				LogActivityLine( messageType, msg );

				if ( xcp != null )
				{
					LogActivityLine( messageType, "CanMoveAxis( {0} ) Exception: {1}", axisName, xcp.Message );
				}
			}

			return retval;
		}

		public void CommandBlind( string command, bool raw = false )
		{
			LogActivityStart( ActivityMessageTypes.Commands, "CommandBlind - {0}", command );
			CheckDevice();
			Service.CommandBlind( command, raw );
			LogActivityEnd( ActivityMessageTypes.Commands, Done );
		}

		public bool CommandBool( string command, bool raw = false )
		{
			LogActivityStart( ActivityMessageTypes.Commands, "CommandBool - {0}", command );
			CheckDevice();
			bool retval = Service.CommandBool( command, raw );
			LogActivityEnd( ActivityMessageTypes.Commands, "returned {0} {1}", retval, Done );

			return retval;
		}

		public string CommandString( string command, bool raw = false )
		{
			LogActivityStart( ActivityMessageTypes.Commands, "CommandString - {0}", command );
			CheckDevice();
			string retval = Service.CommandString( command, raw );
			LogActivityEnd( ActivityMessageTypes.Commands, "returned {0} {1}", retval, Done );

			return retval;
		}

		public PierSide DestinationSideOfPier( double rightAscension, double declination )
		{
			RightAscensionConverter raConverter = new RightAscensionConverter( (decimal)rightAscension );
			DeclinationConverter decConverter = new DeclinationConverter( (decimal)declination );

			LogActivityStart( ActivityMessageTypes.Other, " Get DestinationSideOfPier \r\n   RA {0}\r\n   Dec {1}:", raConverter, decConverter );
			CheckDevice();
			PierSide retval = Service.DestinationSideOfPier( rightAscension, declination );
			string name = GetPierSideName( retval );
			LogActivityEnd( ActivityMessageTypes.Other, "{0} {1}", name, Done );

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
			LogActivityStart( ActivityMessageTypes.Commands, "FindHome:" );
			CheckDevice();
			Service.FindHome();
			LogActivityEnd( ActivityMessageTypes.Commands, Done );
		}

		public void MoveAxis( TelescopeAxes axis, double rate )
		{
			string name = GetNameFromAxis( axis );

			LogActivityStart( ActivityMessageTypes.Commands, "MoveAxis {0} at {1} deg/sec: ", name, rate );
			CheckDevice();
			Service.MoveAxis( axis, rate );
			Status.Slewing = rate != 0.0;
			LogActivityEnd( ActivityMessageTypes.Commands, Done );
		}

		public void Park()
		{
			LogActivityStart( ActivityMessageTypes.Commands, "Park:" );
			CheckDevice();
			Service.Park();
			LogActivityEnd( ActivityMessageTypes.Commands, Done );
		}

		public void PulseGuide( GuideDirections direction, int duration )
		{
			string name = GetGuideDirectionName( direction );
			LogActivityStart( ActivityMessageTypes.Commands, "PulseGuide {0} for {1} ms:", name, duration );
			CheckDevice();
			DateTime startTime = DateTime.Now;
			Service.PulseGuide( direction, duration );

			// Pulse guiding can be synchronous or asynchronous. If it is the former then
			// we will not see IsPulseGuiding set to true.

			if ( Service.IsPulseGuiding )
			{
				Status.IsPulseGuiding = true;
				DateTime endTime = startTime.AddMilliseconds( duration );
				Task.Run( () => MonitorPulseGuidingTask( endTime ) );
			}
			else
			{
				Status.IsPulseGuiding = false;
			}

			LogActivityEnd( ActivityMessageTypes.Commands, Done );
		}

		public void SetPark()
		{
			LogActivityStart( ActivityMessageTypes.Commands, "SetPark:" );
			CheckDevice();
			Service.SetPark();
			LogActivityEnd( ActivityMessageTypes.Commands, Done );
		}

		public void SlewToAltAz( double azimuth, double altitude )
		{
			AzimuthConverter azConverter = new AzimuthConverter( (decimal)azimuth );
			AltitudeConverter altConverter = new AltitudeConverter( (decimal)altitude );

			LogActivityLine( ActivityMessageTypes.Commands, "SlewToAltAz \r\nAz {0}\r\nAlt {1}: {2}"
							, azConverter, altConverter, SlewStarted );
			CheckDevice();
			Status.Slewing = true;
			Service.SlewToAltAz( azimuth, altitude );
			try
			{
				double ra = Service.TargetRightAscension;
				Status.TargetRightAscension = ra;
			}
			catch { }
			try
			{
				double dec = Service.TargetDeclination;
				Status.TargetDeclination = dec;
			}
			catch { }

			Status.RightAscension = Service.RightAscension;
			Status.Declination = Service.Declination;
			Status.Azimuth = Service.Azimuth;
			Status.Altitude = Service.Altitude;
			LogActivityLine( ActivityMessageTypes.Commands, "SlewToAltAz: {0}", SlewComplete );
		}

		public void SlewToAltAzAsync( double azimuth, double altitude )
		{
			AzimuthConverter azConverter = new AzimuthConverter( (decimal)azimuth );
			AltitudeConverter altConverter = new AltitudeConverter( (decimal)altitude );

			LogActivityLine( ActivityMessageTypes.Commands, "SlewToAltAzAsync \r\nAz {0}\r\nAlt {1}: {2}"
							, azConverter, altConverter, SlewStarted );
			CheckDevice();
			Status.Slewing = true;
			Service.SlewToAltAzAsync( azimuth, altitude );
		}

		public void SlewToCoordinates( double rightAscension, double declination )
		{
			RightAscensionConverter raConverter = new RightAscensionConverter( (decimal)rightAscension );
			DeclinationConverter decConverter = new DeclinationConverter( (decimal)declination );

			LogActivityLine( ActivityMessageTypes.Commands, "SlewToCoordinates \r\nRA  {0}\r\nDec {1}: {2}"
							, raConverter, decConverter, SlewStarted );
			CheckDevice();
			Status.Slewing = true;
			Service.SlewToCoordinates( rightAscension, declination );
			Status.TargetRightAscension = Service.TargetRightAscension;
			Status.TargetDeclination = Service.TargetDeclination;
			Status.RightAscension = Service.RightAscension;
			Status.Declination = Service.Declination;
			Status.Azimuth = Service.Azimuth;
			Status.Altitude = Service.Altitude;

			LogActivityLine( ActivityMessageTypes.Commands, "SlewToCoordinates: {0}", SlewComplete );
		}

		public void SlewToCoordinatesAsync( double rightAscension, double declination )
		{
			RightAscensionConverter raConverter = new RightAscensionConverter( (decimal)rightAscension );
			DeclinationConverter decConverter = new DeclinationConverter( (decimal)declination );

			LogActivityLine( ActivityMessageTypes.Commands, "SlewToCoordinatesAsync \r\nRA  {0}\r\nDec {1}: {2}"
							, raConverter, decConverter, SlewStarted );
			CheckDevice();
			Status.Slewing = true;
			Service.SlewToCoordinatesAsync( rightAscension, declination );
		}

		public void SlewToTarget()
		{
			LogActivityStart( ActivityMessageTypes.Commands, "SlewToTarget:" );
			CheckDevice();
			Service.SlewToTarget();
			Status.Slewing = true;
			Status.TargetRightAscension = Service.TargetRightAscension;
			Status.TargetDeclination = Service.TargetDeclination;
			Status.RightAscension = Service.RightAscension;
			Status.Declination = Service.Declination;
			Status.Azimuth = Service.Azimuth;
			Status.Altitude = Service.Altitude;
			LogActivityEnd( ActivityMessageTypes.Commands, SlewComplete );
		}

		public void SlewToTargetAsync()
		{
			LogActivityStart( ActivityMessageTypes.Commands, "SlewToTargetAsync:" );
			CheckDevice();
			Service.SlewToTargetAsync();
			Status.Slewing = true;
			LogActivityEnd( ActivityMessageTypes.Commands, SlewStarted );
		}

		public void SyncToAltAz( double azimuth, double altitude )
		{
			AzimuthConverter azConverter = new AzimuthConverter( (decimal)azimuth );
			AltitudeConverter altConverter = new AltitudeConverter( (decimal)altitude );

			LogActivityLine( ActivityMessageTypes.Commands, "SyncToAltAz \r\nAz {0}\r\nAlt {1}:", azConverter, altConverter );
			CheckDevice();
			Service.SyncToAltAz( azimuth, altitude );
			Status.Slewing = true;
			Status.TargetRightAscension = Service.TargetRightAscension;
			Status.TargetDeclination = Service.TargetDeclination;
			Status.RightAscension = Service.RightAscension;
			Status.Declination = Service.Declination;
			Status.Azimuth = Service.Azimuth;
			Status.Altitude = Service.Altitude;
			LogActivityLine( ActivityMessageTypes.Commands, "SyncToAltAz: {0}", Done );
		}

		public void SyncToCoordinates( double rightAscension, double declination )
		{
			RightAscensionConverter raConverter = new RightAscensionConverter( (decimal)rightAscension );
			DeclinationConverter decConverter = new DeclinationConverter( (decimal)declination );

			LogActivityLine( ActivityMessageTypes.Commands, "SlewToCoordinates \r\nRA  {0}\r\nDec {1}:", raConverter, decConverter );
			CheckDevice();
			Service.SyncToCoordinates( rightAscension, declination );
			Status.Slewing = true;
			Status.TargetRightAscension = Service.TargetRightAscension;
			Status.TargetDeclination = Service.TargetDeclination;
			Status.RightAscension = Service.RightAscension;
			Status.Declination = Service.Declination;
			Status.Azimuth = Service.Azimuth;
			Status.Altitude = Service.Altitude;
			LogActivityLine( ActivityMessageTypes.Commands, "SyncToCoordinates: {0}", Done );
		}

		public void SyncToTarget()
		{
			LogActivityStart( ActivityMessageTypes.Commands, "SyncToTarget:" );
			CheckDevice();
			Service.SyncToTarget();
			Status.TargetRightAscension = Service.TargetRightAscension;
			Status.TargetDeclination = Service.TargetDeclination;
			Status.RightAscension = Service.RightAscension;
			Status.Declination = Service.Declination;
			Status.Azimuth = Service.Azimuth;
			Status.Altitude = Service.Altitude;
			LogActivityEnd( ActivityMessageTypes.Commands, Done );
		}

		public void Unpark()
		{
			LogActivityStart( ActivityMessageTypes.Commands, "Unpark:" );
			CheckDevice();
			Service.Unpark();
			LogActivityEnd( ActivityMessageTypes.Commands, Done );
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
