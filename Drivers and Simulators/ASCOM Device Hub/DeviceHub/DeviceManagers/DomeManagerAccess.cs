using System;
using System.Collections;

using ASCOM.DeviceInterface;
using ASCOM.Utilities;

using ASCOM.DeviceHub.MvvmMessenger;
using System.Threading;

namespace ASCOM.DeviceHub
{
	public partial class DomeManager
	{
		public static string Choose( string id )
		{
			Chooser chooser = new Chooser
			{
				DeviceType = "Dome"
			};

			return chooser.Choose( id );
		}

		private IDomeService _service;

		private IDomeService Service
		{
			get
			{
				if ( _service == null )
				{
					_service = ServiceContainer.Instance.GetService<IDomeService>();
				}

				return _service;
			}
			set
			{
				_service = value;
			}
		}

		public void InitializeDomeService( string id )
		{
			if ( Service.DeviceCreated )
			{
				Service.Dispose();
				Service = null;
			}

			// This creates the service and defines the driver ID.

			Service.CreateDevice( id );
		}

		private void ReleaseDomeService()
		{
			_service.Dispose();
			_service = null;
		}

		#region IDomeV2 Properties

		public bool Connected
		{
			get
			{
				bool retval = false;

				ActivityMessageTypes msgType = ActivityMessageTypes.Other;

				LogActivityStart( msgType, "Get Connected flag - " );

				if ( CheckDevice( false, false ) )
				{
					try
					{
						retval = Service.Connected;
						LogActivityEnd( msgType, retval );
					}
					catch ( Exception xcp )
					{
						LogActivityEnd( msgType, Failed );
						LogActivityLine( msgType, xcp.ToString() );

						throw;
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
				return GetServiceProperty<short>( () => Service.InterfaceVersion, 0, ActivityMessageTypes.Parameters );
			}
		}

		public string Name
		{
			get
			{
				return GetServiceProperty<string>( () => Service.Name, "N/A", ActivityMessageTypes.Other );
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

		public double Altitude
		{
			get
			{
				return GetServiceProperty<double>( () => Service.Altitude, Double.NaN );
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

		public bool CanSetAltitude
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanSetAltitude, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanSetAzimuth
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanSetAzimuth, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanSetPark
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanSetPark, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanSetShutter
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanSetShutter, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanSlave
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanSlave, false, ActivityMessageTypes.Capabilities );
			}
		}

		public bool CanSyncAzimuth
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.CanSyncAzimuth, false, ActivityMessageTypes.Capabilities );
			}
		}

		public ShutterState ShutterStatus
		{
			get
			{
				return GetServiceProperty<ShutterState>( () => Service.ShutterStatus, ShutterState.shutterError );
			}

		}

		public bool Slaved
		{
			get
			{
				LogActivityStart( ActivityMessageTypes.Status, "Get Slaved:" );
				CheckDevice();
				LogActivityEnd( ActivityMessageTypes.Status, Failed );

				// Since we are controlling dome slaving, prevent reading the Slaved status
				// in the driver.

				throw new PropertyNotImplementedException();
			}
			set
			{
				LogActivityStart( ActivityMessageTypes.Status, "Set Slaved -> {0}", value );
				CheckDevice();
				LogActivityEnd( ActivityMessageTypes.Status, Failed );

				// Since we are controlling dome slaving, prevent changing the Slaved status
				// in the driver.

				throw new PropertyNotImplementedException();
			}
		}

		public bool Slewing
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.Slewing, false );
			}
		}

		#endregion IDomeV2 Properties

		#region IDomeV2 Methods

		public void AbortSlew()
		{
			LogActivityStart( ActivityMessageTypes.Commands, "AbortSlew: " );
			CheckDevice();
			Service.AbortSlew();
			LogActivityEnd( ActivityMessageTypes.Commands, Done );
		}

		public string Action( string actionName, string actionParameters )
		{
			LogActivityStart( ActivityMessageTypes.Commands, "Action ({0}):" );
			CheckDevice();
			string retval = Service.Action( actionName, actionParameters );
			LogActivityEnd( ActivityMessageTypes.Commands, "returned {0}", retval );

			return retval;
		}

		public void CloseShutter()
		{
			LogActivityStart( ActivityMessageTypes.Commands, "CloseShutter:" );
			CheckDevice();
			Status.Slewing = true;
			Service.CloseShutter();
			LogActivityEnd( ActivityMessageTypes.Commands, Done );
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
			LogActivityStart( ActivityMessageTypes.Commands, "CommandBool - {0}: ", command );
			CheckDevice();
			bool retval = Service.CommandBool( command, raw );
			LogActivityEnd( ActivityMessageTypes.Commands, "returned {0} {1}", retval, Done );

			return retval;
		}

		public string CommandString( string command, bool raw = false )
		{
			LogActivityStart( ActivityMessageTypes.Commands, "CommandString - {0}: ", command );
			CheckDevice();
			string retval = Service.CommandString( command, raw );
			LogActivityEnd( ActivityMessageTypes.Commands, "returned {0} {1}", retval, Done );

			return retval;
		}

		public void Dispose()
		{
			Messenger.Default.Unregister<TelescopeCapabilitiesUpdatedMessage>( this );
			Messenger.Default.Unregister<TelescopeParametersUpdatedMessage>( this );
			Messenger.Default.Unregister<TelescopeStatusUpdatedMessage>( this );

			if ( Service != null )
			{
				Service.Dispose();
				Service = null;
			}

			DomeManager.Instance = null;
		}

		public void FindHome()
		{
			LogActivityStart( ActivityMessageTypes.Commands, "FindHome:" );
			CheckDevice();
			Status.Slewing = true;
			Service.FindHome();
			LogActivityEnd( ActivityMessageTypes.Commands, Done );
		}

		public void OpenShutter()
		{
			LogActivityStart( ActivityMessageTypes.Commands, "OpenShutter:" );
			CheckDevice();
			Status.Slewing = true;
			Service.OpenShutter();
			LogActivityEnd( ActivityMessageTypes.Commands, Done );
		}

		public void Park()
		{
			LogActivityStart( ActivityMessageTypes.Commands, "Park:" );
			CheckDevice();
			Status.Slewing = true;
			Service.Park();
			LogActivityEnd( ActivityMessageTypes.Commands, Done );
		}

		public void SetPark()
		{
			LogActivityStart( ActivityMessageTypes.Commands, "SetPark:" );
			CheckDevice();
			Service.SetPark();
			Status.AtPark = Service.AtPark;
			LogActivityEnd( ActivityMessageTypes.Commands, Done );
		}

		public void SlewToAltitude( double altitude )
		{
			LogActivityLine( ActivityMessageTypes.Commands, "SlewToAltitude ({0:f5}°): {1}", altitude, SlewStarted );
			CheckDevice();
			Status.Slewing = true;
			Service.SlewToAltitude( altitude );
		}

		public void SlewToAzimuth( double azimuth )
		{
			LogActivityLine( ActivityMessageTypes.Commands, "SlewToAzimuth ({0:f5}°): {1}", azimuth, SlewStarted );
			CheckDevice();
			Status.Slewing = true;
			Service.SlewToAzimuth( azimuth );
		}

		public void SyncToAzimuth( double azimuth )
		{
			LogActivityLine( ActivityMessageTypes.Other, "Azimuth before sync call is {0}.", Azimuth );
			LogActivityLine( ActivityMessageTypes.Commands, "SyncToAzimuth \r\nAz {0:f2}.", azimuth );
			CheckDevice();
			Service.SyncToAzimuth( azimuth );

			// An application will expect to see the adjusted azimuth immediately after this
			// method returns. So we force it into the Status object.

			Status.Azimuth = Service.Azimuth;
			LogActivityLine( ActivityMessageTypes.Commands, "SyncToAzimuth: {0}", Done );
			LogActivityLine( ActivityMessageTypes.Other, "Azimuth after sync call is {0:f2}.", Azimuth );
		}

		#endregion IDomeV2 Methods

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
					throw new NullReferenceException( "The Dome object is null." );
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
					throw new NotConnectedException( "There is no connected dome." );
				}

				retval = false;
			}

			return retval;
		}

		#endregion Helper Methods
	}
}
