using System;
using System.Collections;
using System.Threading;
using ASCOM.DeviceInterface;
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
						LogActivityLine( msgType, $"{xcp}" );

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
					LogActivityLine( msgType, $"{xcp}" );

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

				ActivityMessageTypes msgType = ActivityMessageTypes.Other;
				LogActivityStart( msgType, "Get Link flag - " );

				if ( CheckDevice( false, false ) )
				{
					try
					{
						retval = Service.Link;
						LogActivityEnd( msgType, retval );
					}
					catch ( Exception xcp )
					{
						LogActivityEnd( msgType, Failed );
						LogActivityLine( msgType, $"{xcp}" );

						throw;
					}
				}

				LogActivityEnd( ActivityMessageTypes.Other, retval );

				return retval;
			}
			set
			{
				ActivityMessageTypes msgType = ActivityMessageTypes.Other;
				LogActivityStart( msgType, "Set Link flag -> {0}", value );

				try
				{
					bool isConnected = Service.Link;

					if ( value != isConnected )
					{
						CheckDevice( false );
						Service.Link = value;

						// Poll the device for up to ten seconds or until it reports that
						// disconnect/connect is complete.

						int i = 0;
						int numTries = 20;
						int msDelay = 500;

						while ( i < numTries )
						{
							Thread.Sleep( msDelay );
							isConnected = Service.Link;

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
					LogActivityLine( msgType, $"{xcp}" );

					throw;
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

		public void Dispose()
		{
			Service.Dispose();
			Service = null;
			FocuserManager.Instance = null;
		}

		public void Halt()
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			string msgEnd = "";
			Exception except = null;

			try
			{
				CheckDevice();
				Service.Halt();
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
				LogActivityLine( msgType, $"Halt - {msgEnd}" );

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}

				Status = new DevHubFocuserStatus( this );
			}
		}

		public void Move( int position )
		{
			ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
			string msgEnd = "";
			Exception except = null;

			try
			{
				CheckDevice();
				Service.Move( position );
				msgEnd = MoveStarted;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msgEnd = $"{Failed}. Details follow:";

				throw;
			}
			finally
			{
				if ( Parameters.Absolute )
				{
					LogActivityLine( msgType, $"Move To Position {position} - {msgEnd}" );
				}
				else
				{
					LogActivityLine( msgType, $"Move By {position} steps - {msgEnd}" );
				}

				if ( except != null )
				{
					LogActivityLine( msgType, $"{except}" );
				}

				Status = new DevHubFocuserStatus( this );
			}
		}

        /// <summary>
        /// This <see langword="method"/> is only included so that all members of the ITelescope interface are implemented. It has no other function.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void SetupDialog()
        {
            throw new InvalidOperationException("TelescopeManagerAccess.SetupDialog - This method is only included to keep the compiler happy! - Use TelescopeDriver.SetupDialog instead.");
        }

        #endregion IFocuserV3 Methods

        #region IFocuserV4 Properties and Methods

        public IStateValueCollection DeviceState => GetServiceProperty<IStateValueCollection>(() => Service.DeviceState, new StateValueCollection());

        public void Connect()
        {
            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            string msgEnd = "";
            Exception except = null;

            try
            {
                CheckDevice();
                Service.Connect();
                msgEnd = Done;
            }
            catch (Exception xcp)
            {
                except = xcp;
                msgEnd = $"{Failed}. Details follow:";

                throw;
            }
            finally
            {
                LogActivityLine(msgType, "Connect - {0}", msgEnd);

                if (except != null)
                {
                    LogActivityLine(msgType, $"{except}");
                }

                Status = new DevHubFocuserStatus(this);
            }
        }

        public void Disconnect()
        {
            ActivityMessageTypes msgType = ActivityMessageTypes.Commands;
            string msgEnd = "";
            Exception except = null;

            try
            {
                CheckDevice();
                Service.Disconnect();
                msgEnd = Done;
            }
            catch (Exception xcp)
            {
                except = xcp;
                msgEnd = $"{Failed}. Details follow:";

                throw;
            }
            finally
            {
                LogActivityLine(msgType, "Disconnect - {0}", msgEnd);

                if (except != null)
                {
                    LogActivityLine(msgType, $"{except}");
                }

                Status = new DevHubFocuserStatus(this);
            }
        }

        public bool Connecting => GetServiceProperty<bool>(() => Service.Connecting, false);

        #endregion


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
					string msg = $"The {DeviceType.GetDisplayName()} object is null.";

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
					string msg = $"There is no connected {DeviceType.GetDisplayName()}.";

					throw new NotConnectedException( msg );
				}

				retval = false;
			}

			return retval;
		}

		#endregion CheckDevice Methods
	}
}
