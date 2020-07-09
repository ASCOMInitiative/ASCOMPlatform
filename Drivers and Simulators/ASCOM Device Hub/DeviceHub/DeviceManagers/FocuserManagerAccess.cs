using System;
using System.Collections;
using System.Threading;
using ASCOM.Utilities;

namespace ASCOM.DeviceHub
{
	public partial class FocuserManager
 	{
		public static string Choose( string id )
		{
			Chooser chooser = new Chooser
			{
				DeviceType = "Focuser"
			};

			return chooser.Choose( id );
		}

		private IFocuserService _service;

		private IFocuserService Service
		{
			get
			{
				if ( _service == null )
				{
					_service = ServiceContainer.Instance.GetService<IFocuserService>();
				}

				return _service;
			}
			set
			{
				_service = value;
			}
		}

		#region CheckDevice Methods

		protected override void CheckDevice()
		{
			CheckDevice( true, true );
		}

		protected void CheckDevice( bool testConnected )
		{
			CheckDevice( testConnected, true );
		}

		protected bool CheckDevice( bool testConnected, bool throwException )
		{
			bool retval;

			if ( Service == null || !Service.DeviceCreated )
			{
				if ( throwException )
				{
					string msg = String.Format( "The {0} object is null.", DeviceType );
					throw new NullReferenceException( msg );
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
					string msg = String.Format( "There is no connected {0}.", DeviceType );
					throw new NotConnectedException( msg );
				}

				retval = false;
			}

			return retval;
		}

		#endregion CheckDevice Methods

		public void InitializeFocuserService( string id )
		{
			if ( Service.DeviceCreated )
			{
				Service.Dispose();
				Service = null;
			}

			// This creates the service and defines the driver ID.

			Service.CreateDevice( id );
		}

		private void ReleaseFocuserService()
		{
			_service.Dispose();
			_service = null;
		}

		#region IFocuserV3 Properties

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

		public bool Absolute
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.Absolute, false, ActivityMessageTypes.Parameters );
			}
		}

		public bool IsMoving
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.IsMoving, false );
			}
		}

		public bool Link
		{
			get
			{
				bool retval = false;
				LogActivityStart( ActivityMessageTypes.Other, "Get Link flag - " );

				if ( CheckDevice( false, false ) )
				{
					retval = Service.Link;
				}

				LogActivityEnd( ActivityMessageTypes.Other, retval );

				return retval;
			}
			set
			{
				LogActivityStart( ActivityMessageTypes.Other, "Set Link flag -> {0}", value );

				if ( value != Service.Link )
				{
					CheckDevice( false );
					Service.Link = value;

					LogActivityEnd( ActivityMessageTypes.Other, value ? "(connected)" : "(disconnected)" );
				}
				else
				{
					LogActivityEnd( ActivityMessageTypes.Other, "(no change)" );
				}
			}
		}

		public int MaxIncrement
		{
			get
			{
				return GetServiceProperty<int>( () => Service.MaxIncrement, Int32.MinValue, ActivityMessageTypes.Parameters );
			}
		}

		public int MaxStep
		{
			get
			{
				return GetServiceProperty<int>( () => Service.MaxStep, Int32.MinValue, ActivityMessageTypes.Parameters );
			}
		}

		public int Position
		{
			get
			{
				return GetServiceProperty<int>( () => Service.Position, Int32.MinValue );
			}
		}

		public double StepSize
		{
			get
			{
				return GetServiceProperty<double>( () => Service.StepSize, Double.NaN, ActivityMessageTypes.Parameters );
			}
		}

		public bool TempComp
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.TempComp, false );
			}
			set
			{
				SetServiceProperty<bool>( () =>
				{
					Service.TempComp = value;

					if ( Status != null )
					{
						Status.TempComp = value;
						Status.ClearException();
					}
				}, value );
			}
		}

		public bool TempCompAvailable
		{
			get
			{
				return GetServiceProperty<bool>( () => Service.TempCompAvailable, false, ActivityMessageTypes.Parameters );
			}
		}

		public double Temperature
		{
			get
			{
				return GetServiceProperty<double>( () => Service.Temperature, Double.NaN );
			}
		}

		#endregion IFocuserV3 Properties

		#region IFocuserV3 Methods

		public string Action( string actionName, string actionParameters )
		{
			LogActivityStart( ActivityMessageTypes.Commands, "Action ({0}):" );
			CheckDevice();
			string retval = Service.Action( actionName, actionParameters );
			LogActivityEnd( ActivityMessageTypes.Commands, "returned {0}", retval );

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

		public void Dispose()
		{
			Service.Dispose();
			Service = null;
			FocuserManager.Instance = null;
		}

		public void Halt()
		{
			LogActivityStart( ActivityMessageTypes.Commands, "Halt: " );
			CheckDevice();
			Service.Halt();
			LogActivityEnd( ActivityMessageTypes.Commands, Done );
		}

		public void Move( int position )
		{
			LogActivityStart( ActivityMessageTypes.Commands, "Move: \r\nPosition {0}: ", position );
			CheckDevice();
			Service.Move( position );
			LogActivityEnd( ActivityMessageTypes.Commands, Done );
		}

		#endregion IFocuserV3 Methods
	}
}
