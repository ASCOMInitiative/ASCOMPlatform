using System;
using System.Collections;
using ASCOM.DriverAccess;

namespace ASCOM.DeviceHub
{
	public class FocuserService : IFocuserService
	{
		private string DeviceID { get; set; }
		private Focuser Focuser { get; set; }
		public bool DeviceCreated => Initialized && Focuser != null;
		public bool DeviceAvailable => DeviceCreated && Connected;
		public bool Initialized { get; private set; }

		public void CreateDevice( string id )
		{
			if ( Initialized )
			{
				throw new Exception( "The focuser service attempted to re-initialize the focuser." );
			}

			if ( Focuser == null )
			{
				if ( String.IsNullOrEmpty( id ) )
				{
					throw new Exception( "The focuser service is unable to create a focuser until a Focuser has been chosen." );
				}

				DeviceID = id;

				try
				{
					Focuser = new Focuser( DeviceID );
					Initialized = true;
				}
				catch ( Exception xcp )
				{
					throw new Exception( "Unable to create the Focuser object", xcp );
				}
			}
		}

		#region IFocuserV3 Properties

		public bool Connected
		{
			get
			{
				bool retval = false;

				try
				{
					retval = Focuser.Connected;
				}
				catch ( DriverException )
				{
					throw;
				}
				catch ( Exception xcp )
				{
					throw new DriverException( "Unable to read Focuser.Connected property", xcp );
				}

				return retval;
			}
			set
			{
				try
				{
					Focuser.Connected = value;
				}
				catch ( DriverException )
				{
					throw;
				}
				catch ( Exception xcp )
				{
					throw new DriverException( "Unable to write Focuser.Connected property", xcp );
				}
			}
		}

		public string Description => Focuser.Description;
		public string DriverInfo => Focuser.DriverInfo;
		public string DriverVersion => Focuser.DriverVersion;
		public short InterfaceVersion => Focuser.InterfaceVersion;
		public string Name => Focuser.Name;

		public ArrayList SupportedActions => Focuser.SupportedActions;
		public bool Absolute => Focuser.Absolute;
		public bool IsMoving => Focuser.IsMoving;
		public bool Link { get => Focuser.Link; set => Focuser.Link = value; }
		public int MaxIncrement => Focuser.MaxIncrement;
		public int MaxStep => Focuser.MaxStep;
		public int Position => Focuser.Position;
		public double StepSize => Focuser.StepSize;
		public bool TempComp { get => Focuser.TempComp; set => Focuser.TempComp = value; }
		public bool TempCompAvailable => Focuser.TempCompAvailable;
		public double Temperature => Focuser.Temperature;

		#endregion IFocuserV3 Properties

		#region IFocuserV3 Methods

		public string Action( string actionName, string actionParameters )
		{
			return Focuser.Action( actionName, actionParameters );
		}

		public void CommandBlind( string command, bool raw = false )
		{
			Focuser.CommandBlind( command, raw );
		}

		public bool CommandBool( string command, bool raw = false )
		{
			return Focuser.CommandBool( command, raw );
		}

		public string CommandString( string command, bool raw = false )
		{
			return Focuser.CommandString( command, raw );
		}

		public void Dispose()
		{
			Focuser.Dispose();
			Focuser = null;
			DeviceID = null;
			Initialized = false;
		}

		public void Halt()
		{
			Focuser.Halt();
		}

		public void Move( int position )
		{
			Focuser.Move( position );
		}

		public void SetupDialog()
		{
			// This method is here to satisfy the IFocuserV3 interface and should never be called.

			throw new System.NotImplementedException();
		}

		#endregion IFocuserV3 Methods
	}
}
