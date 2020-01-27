using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ASCOM;
using ASCOM.DeviceInterface;
using ASCOM.DriverAccess;
using ASCOM.Utilities;

namespace ConformanceTests
{
	[TestClass]
	public class DomeDriverConformance
	{
		#region Test Variables

		private TestContext _testContextInstance;

		public TestContext TestContext
		{
			get => _testContextInstance;
			set => _testContextInstance = value;
		}

		private const string _domeID = "ASCOM.DeviceHub.Dome";
		private const string _fmt = "{0} {1,-47}{2,-9} {3}";
		private const string _ok = "OK";
		private const string _info = "INFO";
		private const string _issue = "ISSUE";
		private const string _error = "ERROR";
		private const string _timeout = "TIMEOUT";
		private static int _notImplementedError;
		private static int _invalidValue1;
		private static int _invalidValue2;
		private static int _valueNotSet1;
		private static int _valueNotSet2;

		private static Dome Dome { get; set; }
		private static Util Util { get; set; }

		private static bool _failedConformanceCheck = false;
		private static bool _failedSafetyCheck = false;

		#endregion Test Variables

		#region Test Setup and Teardown

		[ClassInitialize]
		public static void ClassInit( TestContext context )
		{
			unchecked
			{
				_notImplementedError = (int)0x80040400;
				_invalidValue1 = (int)0x80040401;
				_invalidValue2 = (int)0x80040405;
				_valueNotSet1 = (int)0x80040402;
				_valueNotSet2 = (int)0x80040403;
			}

			string msg;
			string status = _ok;

			context.WriteLine( "Conformance Test Driver ProgID: {0}", _domeID );
			context.WriteLine( "" );

			context.WriteLine( "Driver Access Checks" );

			try
			{
				Dome = new Dome( _domeID );
				msg = "Driver instance created successfully";
			}
			catch ( Exception xcp )
			{
				status = _error;
				msg = GetExceptionMessage( xcp );
				_failedConformanceCheck = true;
			}

			context.WriteLine( _fmt, NowTime(), "ConformanceCheck", status, msg );

			if ( !_failedConformanceCheck )
			{
				try
				{
					Dome.Connected = true;
					status = _ok;
					msg = "Connected OK";
				}
				catch ( Exception xcp )
				{
					status = _error;
					msg = GetExceptionMessage( xcp );
					_failedConformanceCheck = true;
				}

				context.WriteLine( _fmt, NowTime(), "ConformanceCheck", status, msg );
			}

			Util = new Util();
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			Dome.Connected = false;
			Dome.Dispose();
			Dome = null;
		}

		#endregion Test Setup and Teardown

		#region Test Methods 

		[TestMethod]
		public void ConformanceTests()
		{
			if ( !_failedConformanceCheck )
			{
				CommonDriverMethods();
				CanProperties();
				PreRunChecks();

				if ( !_failedSafetyCheck )
				{
					Properties();
					Methods();
					PostRunChecks();
				}
			}

			TestContext.WriteLine( "" );

			string msg = "Conformance test complete";

			if ( _failedConformanceCheck )
			{
				msg += "ed with errors";
			}

			TestContext.WriteLine( msg );

			if ( _failedConformanceCheck )
			{
				Assert.Fail();
			}
		}

		public void CommonDriverMethods()
		{
			TestContext.WriteLine( "" );
			TestContext.WriteLine( "" );
			TestContext.WriteLine( "Common Driver Methods" );
			ReadProperty<short>( "InterfaceVersion" );
			ReadProperty<bool>( "Connected" );
			ReadProperty<string>( "Description" );
			ReadProperty<string>( "DriverInfo" );
			ReadProperty<string>( "DriverVersion" );
			ReadProperty<string>( "Name" );
			TestContext.WriteLine( _fmt, NowTime(), "CommandString", _info, "Cannot test the CommandString method" );
			TestContext.WriteLine( _fmt, NowTime(), "CommandBlind", _info, "Cannot test the CommandBlind method" );
			TestContext.WriteLine( _fmt, NowTime(), "CommandBool", _info, "Cannot test the CommandBool method" );
			TestContext.WriteLine( _fmt, NowTime(), "Action", _info, "Cannot test the Action method" );

			ArrayList actions = Dome.SupportedActions;

			if ( actions.Count == 0 )
			{
				TestContext.WriteLine( _fmt, NowTime(), "Supported Actions", _ok, "Driver returned an empty action list" );
			}
			else
			{
				foreach ( object action in actions )
				{
					TestContext.WriteLine( _fmt, NowTime(), "Supported Actions", _ok, action );
				}
			}

		}

		public void CanProperties()
		{
			TestContext.WriteLine( "" );
			TestContext.WriteLine( "" );
			TestContext.WriteLine( "Can Properties" );
			ReadProperty<bool>( "CanFindHome" );
			ReadProperty<bool>( "CanPark" );
			ReadProperty<bool>( "CanSetAltitude" );
			ReadProperty<bool>( "CanSetAzimuth" );
			ReadProperty<bool>( "CanSetPark" );
			ReadProperty<bool>( "CanSetShutter" );
			ReadProperty<bool>( "CanSlave" );
			ReadProperty<bool>( "CanSyncAzimuth" );
		}

		public void PreRunChecks()
		{
			TestContext.WriteLine( "" );
			TestContext.WriteLine( "" );
			TestContext.WriteLine( "Pre-run Checks" );

			string status = "";
			_failedSafetyCheck = false;

			TestContext.WriteLine( _fmt, NowTime(), "DomeSafety" , ""
								 , "Attempting to open shutter as some tests may fail if it is closed..." );

			string msg = OpenDomeShutter( ref status );

			TestContext.WriteLine( _fmt, NowTime(), "DomeSafety", status, msg );

			if ( status == _error )
			{
				_failedSafetyCheck = true;
			}
		}
				 
		public void Properties()
		{
			if ( _failedSafetyCheck )
			{
				TestContext.WriteLine( " Properties tests not executed due to a failed pre-run safety check." );

				return;
			}

			TestContext.WriteLine( "" );
			TestContext.WriteLine( "" );
			TestContext.WriteLine( "Properties" );

		//	goto StartHere;
		//StartHere:

			ReadProperty<double>( "Altitude" );
			ReadProperty<bool>( "AtHome" );
			ReadProperty<bool>( "AtPark" );
			ReadProperty<double>( "Azimuth" );
			ReadProperty<ShutterState>( "ShutterStatus" );
			ReadProperty<bool>( "Slaved", "", "Read" );
			WriteProperty<bool>( "Slaved", true, "", "Write", "", typeof( TargetInvocationException ), true );
			ReadProperty<bool>( "Slewing" );
		}

		public void Methods()
		{
			if ( _failedSafetyCheck )
			{
				TestContext.WriteLine( " Methods tests not executed due to a failed pre-run safety check." );

				return;
			}

			TestContext.WriteLine( "" );
			TestContext.WriteLine( "" );
			TestContext.WriteLine( "Methods" );

		//	goto StartTest;
		//StartTest:

			TestAbortSlew();
			TestSlewToAltitude();
			TestSlewToAzimuth();
			TestSyncToAzimuth();
			TestCloseShutter();
			TestOpenShutter();
			TestFindHome();
			TestPark();
			TestFindHome();
			TestSetPark();
		}

		public void PostRunChecks()
		{
			if ( _failedSafetyCheck )
			{
				TestContext.WriteLine( " Post-run checks not executed due to a failed pre-run safety check." );

				return;
			}

			TestContext.WriteLine( "" );
			TestContext.WriteLine( "" );
			TestContext.WriteLine( "Post-run Checks" );

			TestContext.WriteLine( _fmt, NowTime(), "DomeSafety", _info
								 , "Attempting to close shutter..." );

			PostCloseDomeShutter();

			TestContext.WriteLine( _fmt, NowTime(), "DomeSafety", _info
								 , "Attempting to park dome..." );

			PostParkDome();
		}

		private void PostParkDome()
		{
			string msg = "An unexpected error occurred";
			string status = _error;

			string exceptMsg = ParkTheDome( ref status );

			switch ( status )
			{
				case _ok:
					if ( Dome.AtPark )
					{
						msg = "Dome successfully parked";
					}
					else
					{
						msg = "Dome did not go to the park position";
					}
					break;

				case _info:
					msg = "CanPark is False...Skipping this check.";
					break;

				case _timeout:
					status = _error;
					msg = "Park command did not complete within a reasonable time.";
					break;

				case _error:
					if ( !String.IsNullOrEmpty( exceptMsg ) )
					{
						msg = exceptMsg;
						_failedConformanceCheck = true;
					}
					break;
			}

			TestContext.WriteLine( _fmt, NowTime(), "DomeSafety", status, msg );
		}

		private void PostCloseDomeShutter()
		{
			string msg = null;
			string status = _info;

			if ( !Dome.CanSetShutter )
			{
				msg = "Driver does not support shutter control.";
			}
			else if ( Dome.ShutterStatus != ShutterState.shutterClosed )
			{
				string exceptMsg = CloseTheShutter( ref status );

				switch ( status )
				{
					case _ok:
						msg = "Shutter closed successfully";
						break;

					case _info:
						msg = "CanSetShutter is False...Skipping this test.";
						break;

					case _timeout:
						msg = "Shutter Close did not complete within a reasonable time.";
						break;

					case _error:
						if ( !String.IsNullOrEmpty( exceptMsg ) )
						{
							msg = exceptMsg;
						}
						else
						{
							msg = "Unexpected error occurred!";
						}

						_failedConformanceCheck = true;

						break;
				}
			}
			else
			{
				status = _ok;
				msg = "The shutter was already closed.";
			}

			TestContext.WriteLine( _fmt, NowTime(), "DomeSafety", status, msg );
		}

		private void TestSetPark()
		{
			string msg;
			string status = _error;
			bool expectAtParkAfterSetPark = false; // this may need to be customized!

			try
			{
				if ( Dome.CanSetPark )
				{
					Dome.SetPark();

					if ( Dome.AtPark || !expectAtParkAfterSetPark )
					{
						status = _ok;
						msg = "SetPark issued OK";
					}
					else 
					{
						status = _error;
						msg = "AtPark is False after SetPark command.";
					}
				}
				else
				{
					msg = "CanSetPark is False...Skipping this test.";
				}
			}
			catch ( Exception xcp )
			{
				msg = GetExceptionMessage( xcp );
			}

			TestContext.WriteLine( _fmt, NowTime(), "SetPark", status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestPark()
		{
			string msg = "An unexpected error occurred";
			string status = _error;

			string exceptMsg = ParkTheDome( ref status );

			switch ( status )
			{
				case _ok:
					if ( Dome.AtPark )
					{
						msg = "Dome parked successfully";
					}
					else
					{
						msg = "Dome did not go to the park position";
					}
					break;

				case _info:
					msg = "CanPark is False...Skipping this test.";
					break;

				case _timeout:
					status = _error;
					msg = "Park command did not complete within a reasonable time.";
					break;

				case _error:
					if ( !String.IsNullOrEmpty( exceptMsg ) )
					{
						msg = exceptMsg;
					}
					break;
			}

			TestContext.WriteLine( _fmt, NowTime(), "Park", status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private string ParkTheDome( ref string status )
		{
			string exceptMsg = null;
			bool timedOut;

			try
			{
				if ( Dome.CanPark )
				{
					Dome.Park();
					timedOut = WaitForSlewToComplete( 60.0 );

					if ( timedOut )
					{
						Dome.AbortSlew();
						status = _timeout;
					}
					else
					{
						status = _ok;
					}
				}
				else
				{
					status = _info;
				}
			}
			catch ( Exception xcp )
			{
				exceptMsg = GetExceptionMessage( xcp );
			}

			return exceptMsg;
		}

		private void TestFindHome()
		{
			string msg;
			string status = _error;
			bool timedOut;
			Exception except;

			try
			{
				if ( Dome.CanFindHome )
				{
					Dome.FindHome();
					timedOut = WaitForSlewToComplete( 60.0 );

					if ( timedOut )
					{
						Dome.AbortSlew();
						status = _error;
						msg = "FindHome command did not complete within a reasonable time.";
					}
					else if ( Dome.AtHome )
					{
						status = _ok;
						msg = "Dome homed successfully";
					}
					else
					{
						msg = "Dome did not go to the home position";
					}
				}
				else
				{
					msg = "CanFindHome is False...Skipping this test.";
				}
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msg = GetExceptionMessage( xcp );
			}

			TestContext.WriteLine( _fmt, NowTime(), "FindHome", status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestOpenShutter()
		{
			string msg;
			string status = _error;
			bool timedOut;

			try
			{
				if ( Dome.CanSetShutter )
				{
					Dome.OpenShutter();
					timedOut = WaitForSlewToComplete( 60.0 );

					if ( timedOut )
					{
						Dome.AbortSlew();
						status = _error;
						msg = "Shutter Open did not complete within a reasonable time.";
					}
					else
					{
						status = _ok;
						msg = "Shutter opened successfully";
					}
				}
				else
				{
					status = _info;
					msg = "CanSetShutter is False...Skipping this test.";
				}
			}
			catch ( Exception xcp )
			{
				msg = GetExceptionMessage( xcp );
			}

			TestContext.WriteLine( _fmt, NowTime(), "OpenShutter", status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestCloseShutter()
		{
			string msg = "";
			string status = _error;

			string exceptMsg = CloseTheShutter( ref status );

			switch ( status )
			{
				case _ok:
					msg = "Shutter closed successfully";
					break;

				case _info:
					msg = "CanSetShutter is False...Skipping this test.";
					break;

				case _timeout:
					msg = "Shutter Close did not complete within a reasonable time.";
					break;

				case _error:
					if ( !String.IsNullOrEmpty( exceptMsg ) )
					{
						msg = exceptMsg;
					}
					else
					{
						msg = "Unexpected error occurred!";
					}

					_failedConformanceCheck = true;

					break;
			}						

			TestContext.WriteLine( _fmt, NowTime(), "CloseShutter", status, msg );
		}

		private string CloseTheShutter( ref string status )
		{
			string msg = null;

			try
			{
				if ( Dome.CanSetShutter )
				{
					Dome.CloseShutter();
					bool timedOut = WaitForSlewToComplete( 60.0 );

					if ( timedOut )
					{
						Dome.AbortSlew();
						status = _timeout;
					}
					else
					{
						status = _ok;
					}
				}
				else
				{
					status = _info;
				}
			}
			catch ( Exception xcp )
			{
				msg = GetExceptionMessage( xcp );
			}

			return msg;
		}

		private void TestSyncToAzimuth()
		{
			double az = Dome.Azimuth + 2.0;
			az = Math.Min( az, 359.9 );

			TestSyncToAzimuth( az );
			TestSyncToAzimuth( -10.0 );
			TestSyncToAzimuth( 370.0 );
		}

		private void TestSyncToAzimuth( double azimuth )
		{
			string msg = "";
			string status = _error;
			double minValid = 0.0;
			double maxValid = 359.99;
			Exception except;

			try
			{
				if ( Dome.CanSyncAzimuth )
				{
					Dome.SyncToAzimuth( azimuth );

					double finalAzimuth = Dome.Azimuth;

					if ( Math.Abs( azimuth - finalAzimuth ) <= 1.0 )
					{
						status = _ok;
						msg = "Dome sync'd OK to within +/- 1 degree";
					}
					else
					{
						status = _error;
						msg = "Dome sync error greater than +/- 1 degree";
					}
				}
				else
				{
					status = _info;
					msg = "CanSyncAzimuth is False...Skipping this test.";
				}
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msg = GetExceptionMessage( xcp );

				if ( msg.Contains( "invalid value" ) && ( azimuth < minValid || azimuth > maxValid ) )
				{
					status = _ok;
					msg = String.Format( "COM invalid value exception correctly raised for sync to {0} degrees", azimuth );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SyncToAzimuth", status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestSlewToAzimuth()
		{
			goto StartHere;
		StartHere:
			TestSlewToAzimuth( 0 );
			TestSlewToAzimuth( 45 );
			TestSlewToAzimuth( 90 );
			TestSlewToAzimuth( 135 );
			TestSlewToAzimuth( 180 );
			TestSlewToAzimuth( 225 );
			TestSlewToAzimuth( 270 );
			TestSlewToAzimuth( 315 );
			TestSlewToAzimuth( -10 );
			TestSlewToAzimuth( 370 );
		}

		private void TestSlewToAzimuth( int target )
		{
			string msg = "";
			string status = _error;
			bool timedOut = false;
			int minValid = 0;
			int maxValid = 359;
			Exception except;

			try
			{
				if ( Dome.CanSetAzimuth )
				{
					double azimuth = (double)target;
					Dome.SlewToAzimuth( azimuth );
					timedOut = WaitForSlewToComplete( 60.0 );

					if ( timedOut )
					{
						Dome.AbortSlew();
						status = _error;
						msg = "Slew did not complete within a reasonable time.";
					}
					else
					{
						status = _ok;
						msg = "Asynchronous slew OK";
					}
				}
				else
				{
					msg = "CanSetAzimuth is False...Skipping this test.";
				}
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msg = GetExceptionMessage( xcp );

				if ( msg.Contains( "invalid value" ) && ( target < minValid || target > maxValid ) )
				{
					status = _ok;
					msg = String.Format( "COM invalid value exception correctly raised for slew to {0} degrees", target );
				}
			}

			string suffix = ( target >= minValid && target <= maxValid ) ? target.ToString() : "";
			TestContext.WriteLine( _fmt, NowTime(), "SlewToAzimuth " + suffix, status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestSlewToAltitude()
		{
			goto StartHere;
		StartHere:
			TestSlewToAltitude( 0 );
			TestSlewToAltitude( 15 );
			TestSlewToAltitude( 30 );
			TestSlewToAltitude( 45 );
			TestSlewToAltitude( 60 );
			TestSlewToAltitude( 75 );
			TestSlewToAltitude( 90 );
			TestSlewToAltitude( -10 );
			TestSlewToAltitude( 100 );
		}

		private void TestSlewToAltitude( int target )
		{
			string msg = "";
			string status = _error;
			bool timedOut = false;
			int minValid = 0;
			int maxValid = 90;
			Exception except;

			try
			{
				if ( Dome.CanSetAltitude )
				{
					double altitude = (double)target;
					Dome.SlewToAltitude( altitude );
					timedOut = WaitForSlewToComplete( 60.0 );

					if ( timedOut )
					{
						Dome.AbortSlew();
						status = _error;
						msg = "Slew did not complete within a reasonable time.";
					}
					else
					{
						status = _ok;
						msg = "Asynchronous slew OK";
					}
				}
				else
				{
					msg = "CanSetAltitude is False...Skipping this test.";
				}
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msg = GetExceptionMessage( xcp );

				if ( msg.Contains( "invalid value") && ( target < minValid || target > maxValid ) )
				{
					status = _ok;
					msg = String.Format( "COM invalid value exception correctly raised for slew to {0} degrees", target );
				}
			}

			string suffix = ( target >= minValid && target <= maxValid ) ? target.ToString() : "";
			TestContext.WriteLine( _fmt, NowTime(), "SlewToAltitude "+ suffix, status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private static bool WaitForSlewToComplete( double secondsToWait )
		{
			bool retval = false;
			DateTime timeoutTime = DateTime.Now.AddSeconds( secondsToWait );

			while ( Dome.Slewing )
			{
				Thread.Sleep( 1000 );

				if ( DateTime.Now > timeoutTime )
				{
					retval = true;
					break;
				}
			}

			return retval;
		}

		private void TestAbortSlew()
		{
			string msg = "";
			string status = _error;
			Exception except;

			try
			{
				Dome.AbortSlew();

				status = _ok;
				msg = "AbortSlew command issued successfully";
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msg = GetExceptionMessage( xcp );
			}

			TestContext.WriteLine( _fmt, NowTime(), "AbortSlew", status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		#endregion Test Methods

		#region Helper Methods

		private string OpenDomeShutter( ref string status )
		{
			string retval = "";
			int pollingInterval = 400;

			// The name of this method must be different from the ASCOM method name.

			if ( !Dome.CanSetShutter )
			{
				retval = "Driver does not support shutter control. This may cause some tests to fail.";
			}
			else if ( Dome.ShutterStatus != ShutterState.shutterOpen )
			{
				CancellationTokenSource cts = new CancellationTokenSource( 90000 );

				Task task = Task.Run( () =>
				{
					Dome.OpenShutter();
				}, cts.Token );

				// Loop here until the ShutterStatus is open, timeout or exception.

				bool more = true;

				while ( more )
				{
					ShutterState shutterState = Dome.ShutterStatus;

					if ( shutterState == ShutterState.shutterOpen || shutterState == ShutterState.shutterError )
					{
						more = false;
						retval = "ShutterStatus: " + shutterState.ToString();
						status = ( shutterState == ShutterState.shutterOpen ) ? _ok : _error;
					}
					else if ( task.IsCanceled ) // not parked in 90 seconds causes cancellation.
					{
						more = false;
						retval = "The shutter did not open - timeout after 90 seconds";
						status = _error;
					}
					else if ( task.IsFaulted )
					{
						more = false;
						retval = GetExceptionMessage( task.Exception );
						status = _error;
					}

					if ( more )
					{
						Thread.Sleep( pollingInterval );
					}
				}

				return retval;
			}

			return retval;
		}

		private T ReadProperty<T>( string propName, string valueFormat = "", string nameSuffix = ""
									, string customResultMessage = "" )
		{
			T retval = default( T );
			string status = _error;
			string exception = "";
			string result = null;

			try
			{
				PropertyInfo pi = Dome.GetType().GetProperty( propName );
				T propValue = (T)pi.GetValue( Dome );

				if ( valueFormat != null )
				{
					result = String.Format( "{0:" + valueFormat + "}", propValue );
					status = _ok;
				}

				retval = propValue;
			}
			catch ( Exception xcp )
			{
				exception = GetExceptionMessage( xcp );
			}

			string msg = "";

			if ( status == _error )
			{
				msg = exception;
			}
			else if ( !String.IsNullOrEmpty( customResultMessage ) )
			{
				msg = customResultMessage;
			}
			else if ( result != null )
			{
				msg = result;
			}

			string suffix = String.IsNullOrEmpty( nameSuffix ) ? "" : " " + nameSuffix;
			TestContext.WriteLine( _fmt, NowTime(), propName + suffix, status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}

			return retval;
		}

		private void WriteProperty<T>( string propName, T propValue, string valueFormat = ""
									, string nameSuffix = "", string customResultFormat = ""
									, Type expectedException = null
									, bool isOptional = false )
		{
			string status = _error;
			string exception = "";
			string result = null;
			Exception except = null;

			try
			{
				PropertyInfo pi = Dome.GetType().GetProperty( propName );
				pi.SetValue( Dome, propValue );

				status = _ok;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				exception = GetExceptionMessage( xcp );
			}

			if ( except != null && expectedException != null && except.GetType() == expectedException )
			{
				except = null;
				status = _ok;

				if ( isOptional )
				{
					result = "Optional member threw a PropertyNotImplementedException exception.";
				}
			}

			if ( String.IsNullOrEmpty( result ) )
			{
				string resultFormat = !String.IsNullOrEmpty( customResultFormat ) ? customResultFormat : "{0:" + valueFormat + "}";
				result = String.Format( resultFormat, propValue );
			}

			string msg = ( status == _error ) ? exception : result;
			string suffix = String.IsNullOrEmpty( nameSuffix ) ? "" : " " + nameSuffix;
			TestContext.WriteLine( _fmt, NowTime(), propName + suffix, status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private double ReadDegreeProperty( string propName, string nameSuffix = "", Type expectedException = null )
		{
			double retval = Double.NaN;

			string status = _error;
			Exception except = null;
			string exception = "";
			string result = "?";

			try
			{
				PropertyInfo pi = Dome.GetType().GetProperty( propName );
				double propValue = (double)pi.GetValue( Dome );

				result = Util.DegreesToDMS( propValue, ":", ":", "", 2 );
				status = _ok;
				retval = propValue;
			}
			catch ( Exception xcp )
			{
				except = xcp;
				exception = GetExceptionMessage( xcp );
			}

			if ( except != null && expectedException != null && except.GetType() == expectedException )
			{
				if ( except.GetType() == typeof( TargetInvocationException ) )
				{
					result = exception;
				}

				except = null;
				status = _ok;
			}

			string msg = !String.IsNullOrEmpty( exception ) ? exception : result;
			string suffix = String.IsNullOrEmpty( nameSuffix ) ? "" : " " + nameSuffix;
			TestContext.WriteLine( _fmt, NowTime(), propName + suffix, status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}

			return retval;
		}

		private static string InterpretHResult( int hResult )
		{
			string retval = "Target invocation exception was generated";

			if ( hResult == _invalidValue1 || hResult == _invalidValue2 )
			{
				retval = "Invalid value exception generated";
			}
			else if ( hResult == _valueNotSet1 || hResult == _valueNotSet2 )
			{
				retval = "COM not Set exception generated";
			}
			else if ( hResult == _notImplementedError )
			{
				retval = "Property not implemented exception generated";
			}

			return retval;
		}

		private static string GetExceptionMessage( Exception xcp )
		{
			string retval = "";

			if ( xcp is null )
			{
				return retval;
			}
			else if ( xcp is NotConnectedException )
			{
				retval = "Not connected exception was generated";
			}
			else if ( xcp is MethodNotImplementedException )
			{
				retval = "Method not implemented exception was generated";
			}
			else if ( xcp is TargetInvocationException )
			{
				retval = InterpretHResult( xcp.InnerException.HResult );

				if ( retval.Contains( "not Set" ) )
				{
					retval += " on read before write";
				}
			}
			else if ( xcp is DriverAccessCOMException )
			{
				retval = xcp.Message;
			}
			else
			{
				string xcpName = xcp.GetType().Name;
				retval = "Unexpected exception of type " + xcpName + " was generated";
			}

			return retval;
		}

		private static string NowTime()
		{
			return DateTime.Now.ToString( "hh:mm:ss.fff" );
		}

		#endregion Helper Methods
	}
}
