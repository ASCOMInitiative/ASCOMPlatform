using System;
using System.Collections;
using ASCOM.DeviceInterface;
using ASCOM.DriverAccess;

namespace ASCOM.DeviceHub
{
	public class DomeService : IDomeService
	{
		private string DeviceID { get; set; }
		private Dome Dome { get; set; }

		public bool DeviceCreated => Initialized && Dome != null;
		public bool DeviceAvailable => DeviceCreated && Connected;
		public bool Initialized { get; private set; }

		public void CreateDevice( string id )
		{
			if ( Initialized )
			{
				throw new Exception( "The dome service attempted to re-initialize the dome." );
			}

			if ( Dome == null )
			{
				if ( String.IsNullOrEmpty( id ) )
				{
					throw new Exception( "The dome service is unable to create a dome until a Dome has been chosen." );
				}

				DeviceID = id;

				try
				{
					Dome = new Dome( DeviceID );
					Initialized = true;
				}
				catch ( Exception xcp )
				{
					throw new Exception( "Unable to create the Dome object", xcp );
				}
			}
		}

		#region IDomeV2 Properties

		public bool Connected
		{
			get
			{
				bool retval = false;

				try
				{
					retval = Dome.Connected;
				}
				catch ( DriverException )
				{
					throw;
				}
				catch (Exception xcp )
				{
					throw new DriverException( "Unable to read Dome.Connected property", xcp );
				}

				return retval;
			}
			set
			{
				try
				{
					Dome.Connected = value;
				}
				catch ( DriverException )
				{
					throw;
				}
				catch ( Exception xcp )
				{
					throw new DriverException( "Unable to write Dome.Connected property", xcp );
				}
			}
		}

		public string Description => Dome.Description;
		public string DriverInfo => Dome.DriverInfo;
		public string DriverVersion => Dome.DriverVersion;
		public short InterfaceVersion => Dome.InterfaceVersion;
		public string Name => Dome.Name;

		public ArrayList SupportedActions => Dome.SupportedActions;
		public double Altitude => Dome.Altitude;
		public bool AtHome => Dome.AtHome;
		public bool AtPark => Dome.AtPark;
		public double Azimuth => Dome.Azimuth;
		public bool CanFindHome => Dome.CanFindHome;
		public bool CanPark => Dome.CanPark;
		public bool CanSetAltitude => Dome.CanSetAltitude;
		public bool CanSetAzimuth => Dome.CanSetAzimuth;
		public bool CanSetPark => Dome.CanSetPark;
		public bool CanSetShutter => Dome.CanSetShutter;
		public bool CanSlave => Dome.CanSlave;
		public bool CanSyncAzimuth => Dome.CanSyncAzimuth;
		public ShutterState ShutterStatus => Dome.ShutterStatus;
		public bool Slaved
		{
			get => Dome.Slaved;
			set => Dome.Slaved = value;
		}
		public bool Slewing => Dome.Slewing;

		#endregion IDomeV2 Properties

		#region IDomeV2 Methods

		public void AbortSlew()
		{
			Dome.AbortSlew();
		}

		public string Action( string actionName, string actionParameters )
		{
			return Dome.Action( actionName, actionParameters );
		}

		public void CloseShutter()
		{
			Dome.CloseShutter();
		}

		public void CommandBlind( string command, bool raw = false )
		{
			Dome.CommandBlind( command, raw );
		}

		public bool CommandBool( string command, bool raw = false )
		{
			return Dome.CommandBool( command, raw );
		}

		public string CommandString( string command, bool raw = false )
		{
			return Dome.CommandString( command, raw );
		}

		public void Dispose()
		{
			Dome.Dispose();
			Dome = null;
			DeviceID = null;
			Initialized = false;
		}

		public void FindHome()
		{
			Dome.FindHome();
		}

		public void OpenShutter()
		{
			Dome.OpenShutter();
		}

		public void Park()
		{
			Dome.Park();
		}

		public void SetPark()
		{
			Dome.SetPark();
		}

		public void SetupDialog()
		{
			// This method is here to satisfy the IDomeV2 interface and should never be called.

			throw new System.NotImplementedException();
		}

		public void SlewToAltitude( double altitude )
		{
			Dome.SlewToAltitude( altitude );
		}

		public void SlewToAzimuth( double azimuth )
		{
			Dome.SlewToAzimuth( azimuth );
		}

		public void SyncToAzimuth( double azimuth )
		{
			Dome.SyncToAzimuth( azimuth );
		}

		#endregion IDomeV2 Methods
	}
}
