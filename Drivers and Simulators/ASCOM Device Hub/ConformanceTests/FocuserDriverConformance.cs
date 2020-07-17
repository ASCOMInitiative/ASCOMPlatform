using System;
using System.Collections;
using System.Reflection;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ASCOM;
using ASCOM.DriverAccess;

namespace ConformanceTests
{
	/// <summary>
	/// Summary description for FocuserDriverConformance
	/// </summary>
	[TestClass]
	public class FocuserDriverConformance
	{
		#region Test Variables

		private TestContext _testContextInstance;

		public TestContext TestContext
		{
			get => _testContextInstance;
			set => _testContextInstance = value;
		}

		private const string _focuserID = "ASCOM.DeviceHub.Focuser";
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

		private static Focuser Focuser { get; set; }
		
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

			context.WriteLine( "Conformance Test Driver ProgID: {0}", _focuserID );
			context.WriteLine( "" );

			context.WriteLine( "Driver Access Checks" );

			try
			{
				Focuser = new Focuser( _focuserID );
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
					Focuser.Connected = true;
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
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			Focuser.Connected = false;
			Focuser.Dispose();
			Focuser = null;

			if ( _failedConformanceCheck )
			{
				Assert.Fail();
			}

		}

		#endregion Test Setup and Teardown

		#region Test Methods

		[TestMethod]
		public void ConformanceTests()
		{
			if ( !_failedConformanceCheck )
			{
				CommonDriverMethods();

				if ( !_failedSafetyCheck )
				{
					Properties();
					Methods();
				}
			}

			TestContext.WriteLine( "" );

			string msg = "Conformance test complete";

			if ( _failedConformanceCheck )
			{
				msg += "d with errors";
			}

			TestContext.WriteLine( msg );
		}

		private void CommonDriverMethods()
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

			ArrayList actions = Focuser.SupportedActions;

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

		private void Properties()
		{
			if ( _failedSafetyCheck )
			{
				TestContext.WriteLine( "Properties tests not executed due to a failed pre-run safety check." );

				return;
			}

			TestContext.WriteLine( "" );
			TestContext.WriteLine( "" );
			TestContext.WriteLine( "Properties" );

			goto StartHere;
		StartHere:

			bool isAbsolute = ReadProperty<bool>( "Absolute" );
			ReadProperty<bool>( "IsMoving" );
			ReadProperty<int>( "MaxIncrement" );
			ReadProperty<int>( "MaxStep" );
			Type expectedException = ( isAbsolute ) ? null : typeof( TargetInvocationException );
			ReadProperty<int>( "Position", "", "", "", expectedException );
			ReadProperty<double>( "StepSize", "F1" );
			bool canSetTempComp = ReadProperty<bool>( "TempCompAvailable" );
			ReadProperty<bool>( "TempComp", "", "Read" );

			if ( canSetTempComp )
			{
				WriteProperty<bool>( "TempComp", true, "", "Write", "Successfully turned temperature compensation on" );
				WriteProperty<bool>( "TempComp", false, "", "Write", "Successfully turned temperature compensation off" );
			}
			else
			{
				TestContext.WriteLine( _fmt, NowTime(), "TempComp Write", _ok, "Unable to write to TempComp due to TempCompAvailable being false" );
			}

			ReadProperty<double>( "Temperature" );
		}

		private void Methods()
		{
			if ( _failedSafetyCheck )
			{
				TestContext.WriteLine( " Methods tests not executed due to a failed pre-run safety check." );

				return;
			}

			TestContext.WriteLine( "" );
			TestContext.WriteLine( "" );
			TestContext.WriteLine( "Methods" );

			goto StartTest;
		StartTest:

			TestHalt();
			TestMove();
		}

		private void TestMove()
		{
			int steps;

			if ( Focuser.TempCompAvailable )
			{
				Focuser.TempComp = false;
			}

			goto StartTest;
		StartTest:

			steps = TestMove( 5000, true, false );
			TestMove( -steps, false, false );

			if ( Focuser.TempCompAvailable )
			{
				Focuser.TempComp = true;
			}

			steps = TestMove( 5000, true, true );
			TestMove( -steps, false, true );

			if ( Focuser.TempCompAvailable )
			{
				Focuser.TempComp = false;
			}

			if ( Focuser.Absolute )
			{
				TestMoveToLimit( 0 );
				TestMoveToLimit( -10 );
				TestMoveToLimit( Focuser.MaxStep );
				TestMoveToLimit( Focuser.MaxStep + 10 );			
			}
			else
			{
				//TODO: Test moves of > MaxIncrement and < -MaxIncrement
				// This case is not tested by Conform as of 5/31/2019.
			}
		}

		private void TestMoveToLimit( int position )
		{
			string status = _error;
			short interfaceVersion = Focuser.InterfaceVersion;
			int maxStep = Focuser.MaxStep;

			string verSuffix = "";
			string msg = String.Format( "Moving to position: {0}", position );

			if ( position == 0 )
			{
				verSuffix = "To 0";
			}
			else if ( position < 0 )
			{
				verSuffix = "Below 0";
			}
			else if ( position == maxStep )
			{
				verSuffix = "To MaxStep";
			}
			else if ( position > maxStep )
			{
				verSuffix = "Above MaxStep";
			}

			string methodName = String.Format( "Move - {0}", verSuffix );
												
			TestContext.WriteLine( _fmt, NowTime(), methodName, _info, msg );

			try
			{
				bool timeout = false;
				Focuser.Move( position );

				if ( Focuser.IsMoving )
				{
					TestContext.WriteLine( _fmt, NowTime(), methodName, _ok
											, "Asynchronous move found" );

					timeout = WaitForMoveToComplete( 160.0 );

					int finalPosition = Focuser.Position;
					msg = String.Format( "Moved to {0}", finalPosition );

					if ( position == finalPosition )
					{
						status = _ok;
					}
					else if ( position < 0 && finalPosition == 0 )
					{
						status = _ok;
					}
					else if ( position > maxStep && finalPosition == maxStep )
					{
						status = _ok;
					}
					else if ( timeout )
					{
						Focuser.Halt();
						msg = "Focuser did not complete the requested move";
					}
					else
					{
						msg = "Focuser did not move to the expected position";
					}
				}
				else
				{
					msg = "Move appears to be synchronous";
				}
			}
			catch ( Exception xcp )
			{
				status = _error;
				msg = GetExceptionMessage( xcp );
			}

			if ( !String.IsNullOrEmpty( msg ) )
			{
				TestContext.WriteLine( _fmt, NowTime(), methodName, status, msg );
			}

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private int TestMove( int delta, bool initialMove, bool withTempComp )
		{
			int retval;
			string msg;

			if ( Focuser.Absolute )
			{
				// Figure out where we are moving to, but keep it between 0 and MaxStep.

				int startingPosition = Focuser.Position;
				int newPosition = startingPosition + delta;
				newPosition = Math.Max( Math.Min( newPosition, Focuser.MaxStep ), 0 );

				// Set up to return how much we are actually moving by.

				retval = newPosition - startingPosition;

				if ( initialMove )
				{
					msg = String.Format( "Moving to position: {0}", newPosition );
				}
				else
				{
					msg = String.Format( "Returning to original position: {0}", newPosition );
				}

				TestMoveTo( newPosition, msg, initialMove, withTempComp );
			}
			else // Relative focuser
			{
				retval = Math.Min( delta, Focuser.MaxIncrement );

				if ( initialMove )
				{
					msg = String.Format( "Initial move of {0} steps", delta );
				}
				else
				{
					msg = String.Format( "Return move of {0} steps", delta );
				}

				TestMoveBy( retval, msg );
			}

			return retval;
		}

		private void TestMoveTo( int position, string msg, bool initialMove, bool withTempComp )
		{
			string status = _error;
			short interfaceVersion = Focuser.InterfaceVersion;

			string verSuffix = "";

			if ( withTempComp )
			{
				verSuffix = String.Format( " V{0}", interfaceVersion );
			}

			string methodName = String.Format( "Move - TempComp {0}",
												( withTempComp ) ? "True" : "False" );

			TestContext.WriteLine( _fmt, NowTime(), methodName + verSuffix, _info, msg );

			try
			{
				bool timeout = false;
				//Debug.WriteLine( "{0}: Focuser moving from {1} to {2}.", NowTime(), Focuser.Position, position );

				Focuser.Move( position );

				if ( Focuser.IsMoving )
				{
					if ( initialMove )
					{
						TestContext.WriteLine( _fmt, NowTime(), methodName + verSuffix, _ok
												, "Asynchronous move found" );
					}

					timeout = WaitForMoveToComplete( 60.0 );

					if ( timeout )
					{
						Focuser.Halt();
						msg = "Focuser did not complete the requested move";
					}
					else if ( position == Focuser.Position )
					{
						status = _ok;

						if ( initialMove )
						{
							msg = "Absolute move OK";
						}
						else
						{
							msg = "";
						}
					}
					else if ( !withTempComp )
					{
						msg = String.Format( "Did not move to the expected position: Exp = {0}, Act = {1}"
											, position, Focuser.Position );
					}
					else
					{
						status = _info;
						msg = String.Format( "Move was within {0} steps of desired position", Focuser.Position - position );
					}
				}
				else
				{
					msg = "Move appears to be synchronous";
				}
			}
			catch ( Exception xcp )
			{
				status = _error;
				msg = GetExceptionMessage( xcp );
			}

			if ( !String.IsNullOrEmpty( msg ) )
			{
				TestContext.WriteLine( _fmt, NowTime(), methodName + verSuffix, status, msg );
			}

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestMoveBy( int amount, string msg )
		{
			string status = _error;
		
			TestContext.WriteLine( _fmt, NowTime(), "Move", "", msg );

			try
			{
				bool timeout = false;
				Focuser.Move( amount );

				if ( Focuser.IsMoving )
				{
					TestContext.WriteLine( _fmt, NowTime(), "Move", _ok, "Asynchronous move found" );
					timeout = WaitForMoveToComplete( 60.0 );

					if ( timeout )
					{
						Focuser.Halt();
						msg = "Focuser did not complete the requested move";
					}
					else
					{
						status = _ok;
						msg = "Relative move OK";
					}
				}
				else
				{
					msg = "Move appears to be synchronous";
				}
			}
			catch ( Exception xcp )
			{
				status = _error;
				msg = GetExceptionMessage( xcp );
			}

			TestContext.WriteLine( _fmt, NowTime(), "Move", status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestHalt()
		{
			string msg = "";
			string status = _error;

			try
			{
				Focuser.Halt();

				status = _ok;
				msg = "Focuser halted OK";
			}
			catch ( Exception xcp )
			{
				msg = GetExceptionMessage( xcp );
			}

			TestContext.WriteLine( _fmt, NowTime(), "Halt", status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		#endregion Test Methods

		#region Helper Methods

		private static bool WaitForMoveToComplete( double secondsToWait )
		{
			bool retval = false;
			DateTime timeoutTime = DateTime.Now.AddSeconds( secondsToWait );

			while ( Focuser.IsMoving )
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

		private T ReadProperty<T>( string propName, string valueFormat = "", string nameSuffix = ""
									, string customResultMessage = "", Type expectedException=null )
		{
			T retval = default( T );
			string status = _error;
			string exception = "";
			string result = null;
			
			try
			{
				PropertyInfo pi = Focuser.GetType().GetProperty( propName );
				T propValue = (T)pi.GetValue( Focuser );

				if ( valueFormat != null )
				{
					result = String.Format( "{0:" + valueFormat + "}", propValue );
					status = _ok;
				}

				retval = propValue;
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null )
				{
					exception = GetExceptionMessage( xcp );
				}
				else if ( xcp.GetType() == expectedException )
				{
					if ( xcp.InnerException != null && xcp.InnerException.GetType() == typeof( PropertyNotImplementedException ) )
					{
						result = String.Format( "PropertyNotImplementedException generated as expected" );
						status = _ok;
					}
				}
			}

			string msg = "";

			if ( status == _error )
			{
				msg = exception;
				_failedConformanceCheck = true;
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
				PropertyInfo pi = Focuser.GetType().GetProperty( propName );
				pi.SetValue( Focuser, propValue );

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
