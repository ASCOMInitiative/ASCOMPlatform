using System;
using System.Collections;
using System.Reflection;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.NOVAS;
using ASCOM.Astrometry.Transform;
using ASCOM.DeviceInterface;
using ASCOM.DriverAccess;
using ASCOM.Utilities;
using System.Windows;

namespace ConformanceTests
{
	[TestClass]
	public class TelescopeDriverConformance
	{
		#region Test Variables

		private TestContext _testContextInstance;

		public TestContext TestContext
		{
			get => _testContextInstance;
			set => _testContextInstance = value;
		}

		private const string _telescopeID = "ASCOM.DeviceHub.Telescope";
		private const string _fmt = "{0} {1,-47}{2,-9} {3}";
		private const string _ok = "OK";
		private const string _info = "INFO";
		private const string _issue = "ISSUE";
		private const string _error = "ERROR";
		private static int _notImplementedError;
		private static int _invalidValue1;
		private static int _invalidValue2;
		private static int _valueNotSet1;
		private static int _valueNotSet2;
		private static double _saneAltitude = 50.0;
		private static double _saneAzimuth = 135.0;
		private static double _tolerance = 0.02;

		private static Telescope Telescope { get; set; }
		private static Util Util { get; set; }

		private static bool _failedConformanceCheck = false;
		private static bool _failedSafetyCheck = false;
		private static string[] _axisNames = new string[] { "Primary", "Secondary", "Tertiary" };

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

			context.WriteLine( "Conformance Test Driver ProgID: {0}", _telescopeID );
			context.WriteLine( "" );

			context.WriteLine( "Driver Access Checks" );

			try
			{
				Telescope = new Telescope( _telescopeID );
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
					Telescope.Connected = true;
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
			Telescope.Connected = false;
			Telescope.Dispose();
			Telescope = null;
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

			ArrayList actions = Telescope.SupportedActions;

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

		private void CanProperties()
		{
			TestContext.WriteLine( "" );
			TestContext.WriteLine( "" );
			TestContext.WriteLine( "Can Properties" );
			ReadProperty<bool>( "CanFindHome" );
			ReadProperty<bool>( "CanPark" );
			ReadProperty<bool>( "CanPulseGuide" );
			ReadProperty<bool>( "CanSetDeclinationRate" );
			ReadProperty<bool>( "CanSetGuideRates" );
			ReadProperty<bool>( "CanSetPark" );
			ReadProperty<bool>( "CanSetPierSide" );
			ReadProperty<bool>( "CanSetRightAscensionRate" );
			ReadProperty<bool>( "CanSetTracking" );
			ReadProperty<bool>( "CanSlew" );
			ReadProperty<bool>( "CanSlewAltAz" );
			ReadProperty<bool>( "CanSlewAltAzAsync" );
			ReadProperty<bool>( "CanSlewAsync" );
			ReadProperty<bool>( "CanSync" );
			ReadProperty<bool>( "CanSyncAltAz" );
			ReadProperty<bool>( "CanUnpark" );
		}

		private void PreRunChecks()
		{
			TestContext.WriteLine( "" );
			TestContext.WriteLine( "" );
			TestContext.WriteLine( "Pre-run Checks" );

			if ( Telescope.AtPark )
			{
				_failedSafetyCheck = true;
				Assert.Fail( "Telescope is at park!" );
			}

			TestContext.WriteLine( _fmt, NowTime(), "Mount Safety", _info, "Scope is not parked, continuing testing" );

			if ( !Telescope.Tracking )
			{
				Telescope.Tracking = true;
			}

			TestContext.WriteLine( _fmt, NowTime(), "Mount Safety", _info, "Scope tracking has been enabled" );

			TimeZoneInfo tzi = TimeZoneInfo.Local;
			string name = tzi.StandardName;
			double offset = tzi.GetUtcOffset( DateTime.Now ).TotalHours;
			string msg = String.Format( "PC Time Zone:    {0}, offset {1:f1} hours.", name, offset );
			TestContext.WriteLine( _fmt, NowTime(), "Time Check", _info, msg );

			msg = String.Format( "PC UTCDate:    {0}", DateTime.Now.ToUniversalTime().ToString( "dd-MMM-yyyy HH:mm:ss.fff" ) );
			TestContext.WriteLine( _fmt, NowTime(), "Time Check", _info, msg );

			msg = String.Format( "Mount UTCDate: {0}", Telescope.UTCDate.ToString( "dd-MMM-yyyy HH:mm:ss.fff" ) );
			TestContext.WriteLine( _fmt, NowTime(), "Time Check", _info, msg );
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

			ReadProperty<AlignmentModes>( "AlignmentMode" );
			ReadProperty<double>( "Altitude", "F2" );
			ReadProperty<double>( "ApertureArea", "F5" );
			ReadProperty<double>( "ApertureDiameter", "F2" );
			ReadProperty<bool>( "AtHome" );
			ReadProperty<bool>( "AtPark" );
			ReadProperty<double>( "Azimuth", "F2" );
			ReadDegreeProperty( "Declination" );
			double dVal = ReadProperty<double>( "DeclinationRate", "F2", "Read" );
			WriteProperty<double>( "DeclinationRate", dVal, "F2", "Write" );
			ReadProperty<bool>( "DoesRefraction", "", "Read" );
			WriteProperty<bool>( "DoesRefraction", true, "", "Write", "Can set DoesRefraction to True" );
			ReadProperty<EquatorialCoordinateType>( "EquatorialSystem" );
			ReadProperty<double>( "FocalLength", "F3" );
			dVal = ReadProperty<double>( "GuideRateDeclination", "F2", "Read" );
			WriteProperty<double>( "GuideRateDeclination", dVal, "", "Write", "Can set Declination Guide Rate OK" );
			dVal = ReadProperty<double>( "GuideRateRightAscension", "F2", "Read" );
			WriteProperty<double>( "GuideRateRightAscension", dVal, "", "Write", "Can set Right Ascension Guide OK" );
			ReadProperty<bool>( "IsPulseGuiding" );
			ReadHourProperty( "RightAscension" );
			dVal = ReadProperty<double>( "RightAscensionRate", "F2", "Read" );
			WriteProperty<double>( "RightAscensionRate", dVal, "F2", "Write" );
			dVal = ReadProperty<double>( "SiteElevation", "F0", "Read" );
			WriteProperty<double>( "SiteElevation", -301, "F0", "Write"
									, "Invalid value exception generated as expected on set site elevation < -300m"
									, typeof( TargetInvocationException ) );
			WriteProperty<double>( "SiteElevation", 10001, "F0", "Write"
									, "Invalid value exception generated as expected on set site elevation > 10,000m"
									, typeof( TargetInvocationException ) );
			string customMsg = String.Format( "Legal value {0:F0}m written successfully", dVal );
			WriteProperty<double>( "SiteElevation", dVal, "", "Write", customMsg );
			dVal = ReadDegreeProperty( "SiteLatitude" );
			WriteProperty<double>( "SiteLatitude", -90.1, "", "Write"
									, "Invalid value exception generated as expected on set site latitude < -90 degrees"
									, typeof( TargetInvocationException ) );
			WriteProperty<double>( "SiteLatitude", 90.1, "", "Write"
									, "Invalid value exception generated as expected on set site latitude > 90 degrees"
									, typeof( TargetInvocationException ) );
			customMsg = String.Format( "Legal value {0} degrees written successfully"
											, Util.DegreesToDMS( dVal, ":", ":", "", 2 ) );
			WriteProperty<double>( "SiteLatitude", dVal, "", "Write", customMsg );

			dVal = ReadDegreeProperty( "SiteLongitude" );
			WriteProperty<double>( "SiteLongitude", -180.1, "", "Write"
									, "Invalid value exception generated as expected on set site longitude < -180 degrees"
									, typeof( TargetInvocationException ) );
			WriteProperty<double>( "SiteLongitude", 180.1, "", "Write"
									, "Invalid value exception generated as expected on set site longitude > 180 degrees"
									, typeof( TargetInvocationException ) );
			customMsg = String.Format( "Legal value {0} degrees written successfully"
											, Util.DegreesToDMS( dVal, ":", ":", "", 2 ) );
			WriteProperty<double>( "SiteLongitude", dVal, "", "Write", customMsg );
			ReadProperty<bool>( "Slewing" );
			ReadProperty<short>( "SlewSettleTime", "", "Read" );
			WriteProperty<short>( "SlewSettleTime", -1, "", "Write"
								, "Invalid value exception generated as expected on set slew settle time < 0"
								, typeof( TargetInvocationException ) );
			ReadProperty<PierSide>( "SideOfPier", "", "Read" );
			ReadAndTestSiderealTime();

			ReadDegreeProperty( "TargetDeclination", "Read", typeof( TargetInvocationException ) );
			WriteProperty<double>( "TargetDeclination", -91.0, "", "Write"
									, "Invalid value exception generated as expected on set TargetDeclination < -90 degrees"
									, typeof( TargetInvocationException ) );
			WriteProperty<double>( "TargetDeclination", 91.0, "", "Write"
									, "Invalid value exception generated as expected on set TargetDeclination > 90 degrees"
									, typeof( TargetInvocationException ) );
			dVal = 1.0;
			string dValFormatted = Util.DegreesToDMS( dVal, ":", ":", "", 2 );
			string msg = String.Format( "Legal Value {0} DD:MM:SS written successfully.", dValFormatted );
			WriteProperty<double>( "TargetDeclination", dVal, "", "Write", msg );
			ReadDegreeProperty( "TargetDeclination", "Read" );

			ReadHourProperty( "TargetRightAscension", "Read", typeof( TargetInvocationException ) );
			WriteProperty<double>( "TargetRightAscension", -181.0, "", "Write"
									, "Invalid value exception generated as expected on set TargetRightAscension < -180 degrees"
									, typeof( TargetInvocationException ) );
			WriteProperty<double>( "TargetRightAscension", 181.0, "", "Write"
									, "Invalid value exception generated as expected on set TargetRightAscension > 180 degrees"
									, typeof( TargetInvocationException ) );
			dVal = 16.1679027;
			dValFormatted = Util.HoursToHMS( dVal, ":", ":", "", 2 );
			msg = String.Format( "Legal Value {0} HH:MM:SS written successfully.", dValFormatted );
			WriteProperty<double>( "TargetRightAscension", dVal, "", "Write", msg );
			ReadHourProperty( "TargetRightAscension", "Read" );

			ReadProperty<bool>( "Tracking", "", "Read" );
			WriteProperty<bool>( "Tracking", false, "", "Write" );

			ITrackingRates trackingRates = ReadAndReportTrackingRates();
			ReadProperty<DriveRates>( "TrackingRate", "", "Read" );
			WriteAndReportTrackingRates( trackingRates );

			DateTime utcDate = ReadProperty<DateTime>( "UTCDate", "MM-dd-yyyy H:mm:ss.fff", "Read" );
			msg = String.Format( "New UTCDate written successfully: {0:G}", utcDate );
			WriteProperty<DateTime>( "UTCDate", utcDate, "", "Write", msg );

			return;
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

			GetCanMoveAxes();
			TestParkUnpark();
			TestAbortSlewNotSlewing();
			TestAxisRates();
			TestFindHome();
			TestMoveAxes();
			TestPulseGuide();

			TestCoordinateSlew();
			TestCoordinateSlewAsync();
			TestTargetSlew();
			TestTargetAsyncSlew();
			TestDestinationSideOfPier();
			TestAltAzSlew();
			TestAltAzAsyncSlew();
			TestSyncToCoordinates();
			TestSyncToTarget();
			TestSyncToAltAz();

			return;
		}

		private void TestSyncToAltAz()
		{
			TestSyncToAltAz_Valid();
			TestSyncToAltAz_BadValue( true, -100.0, "(Bad L)" );
			TestSyncToAltAz_BadValue( false, -10.0, "(Bad L)" );
			TestSyncToAltAz_BadValue( true, 100.0, "(Bad H)" );
			TestSyncToAltAz_BadValue( false, 370.0, "(Bad H)" );
		}

		private void TestSyncToAltAz_BadValue( bool isAlt, double badValue, string suffix )
		{
			string status = _error;
			string msg = null;

			Type expectedException = ( Telescope.CanSync )
									? typeof( DriverAccessCOMException )
									: typeof( MethodNotImplementedException );
			double targetAz;
			double targetAlt;
			string badValueFormatted;

			if ( isAlt )
			{
				targetAz = Telescope.Azimuth;
				targetAlt = badValue;
				badValueFormatted = Util.DegreesToDMS( targetAlt, ":", ":", "", 2 );
			}
			else
			{
				targetAz = badValue;
				targetAlt = Telescope.Altitude;
				badValueFormatted = Util.DegreesToDMS( targetAz, ":", ":", "", 2 );
			}

			try
			{
				Telescope.SyncToAltAz( targetAz, targetAlt );

				msg = String.Format( "Incorrectly accepted bad {0} coordinate: {1}"
									, isAlt ? "Alt" : "Az", badValueFormatted );
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					msg = GetExceptionMessage( xcp ) + " when attempting to SyncToAltAz.";
				}
				else
				{
					status = _ok;
					msg = String.Format( "Correctly rejected bad {0} coordinate: {1}"
									, isAlt ? "Alt" : "Az", badValueFormatted );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SyncToAltAz " + suffix, status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestSyncToAltAz_Valid()
		{
			string status = _ok;
			string msg1 = null;
			string msg2 = null;
			Type expectedException = null;
			Vector target;

			expectedException = ( Telescope.CanSyncAltAz )
									? null
									: typeof( MethodNotImplementedException );

			target = new Vector( 135.0, 50.0 );

			bool trackingFlag = Telescope.Tracking;
			Telescope.Tracking = false;

			if ( !PositionTelescopeToAltAz( target, "SyncToAltAz", false ) )
			{
				return;
			}

			try
			{
				// Reduce the Azimuth by 4 minutes.
				double trueAz = target.X - ( 4.0 / 60.0 );

				// Reduce the Altitude by 1 degree;
				// Given the sanity of the target, subtracting 1 degree should still be valid.
				double trueAlt = target.Y - 1.0;

				Telescope.SyncToAltAz( trueAz, trueAlt );
				//Thread.Sleep( 5000 );

				double actualAz = Telescope.Azimuth;
				double actualAlt = Telescope.Altitude;

				if ( Math.Abs( trueAz - actualAz ) > _tolerance )
				{
					status = _error;
					msg1 = String.Format( "Sync completed, but Azimuth does not match - Expected = {0}, Actual = {1}"
										, trueAz, actualAz );
				}
				else if ( Math.Abs( trueAlt - actualAlt ) > _tolerance )
				{
					status = _error;
					msg1 = String.Format( "Sync completed, but Altitude does not match - Expected = {0}, Actual = {1}"
										, trueAlt, actualAlt );
				}
				else
				{
					string text = Util.DegreesToDMS( trueAz, ":", ":", "", 2 );
					msg1 = String.Format( "Synced to sync position OK.   AZ:    {0}", text );
					text = Util.DegreesToDMS( trueAlt, ":", ":", "", 2 );
					msg2 = String.Format( "Synced to sync position OK.   ALT:   {0}", text );
				}
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					status = _error;
					msg1 = GetExceptionMessage( xcp ) + " when attempting to SyncToAltAz.";
				}
				else
				{
					msg1 = String.Format( "CanSyncAltAz is False and a {0} exception was generated as expected", expectedException.Name );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SyncToAltAz", status, msg1 );

			if ( !String.IsNullOrEmpty( msg2 ) )
			{
				TestContext.WriteLine( _fmt, NowTime(), "SyncToAltAz", status, msg2 );
			}

			if ( status != _ok )
			{
				return;
			}

			if ( !PositionTelescopeToAltAz( target, "SyncToAltAz", true ) )
			{
				return;
			}

			try
			{
				// Increase the Azimuth by 4 minutes, but make sure we don't go beyond 24 hrs.
				double trueAz = target.X + ( 4.0 / 60.0 );

				// Increase the Dec by 1 degree;
				// Given the sanity of the target, adding 1 degree should still be valid.
				double trueAlt = target.Y + 1.0;

				Telescope.SyncToAltAz( trueAz, trueAlt );
				//Thread.Sleep( 5000 );

				double targetAz = Telescope.Azimuth;
				double targetAlt = Telescope.Altitude;

				if ( Math.Abs( trueAz - targetAz ) > _tolerance )
				{
					status = _error;
					msg1 = String.Format( "Sync to reversed sync position completed, but Azimuth does not match - Expected = {0}, Actual = {1}"
										, trueAz, targetAz );
				}
				else if ( Math.Abs( trueAlt - targetAlt ) > _tolerance )
				{
					status = _error;
					msg1 = String.Format( "Sync to reversed sync position completed, but Altitude does not match - Expected = {0}, Actual = {1}"
										, trueAlt, targetAlt );
				}
				else
				{
					string text = Util.DegreesToDMS( trueAz, ":", ":", "", 2 );
					msg1 = String.Format( "Synced to reversed sync position OK.   Az:    {0}", text );
					text = Util.DegreesToDMS( trueAlt, ":", ":", "", 2 );
					msg2 = String.Format( "Synced to reversed sync position OK.   Alt:   {0}", text );
				}
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					status = _error;
					msg1 = GetExceptionMessage( xcp ) + " when attempting to SyncToAltAz.";
				}
				else
				{
					msg1 = String.Format( "CanSync is False and a {0} exception was generated as expected", expectedException.Name );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SyncToAltAz", status, msg1 );

			if ( !String.IsNullOrEmpty( msg2 ) )
			{
				TestContext.WriteLine( _fmt, NowTime(), "SyncToAltAz", status, msg2 );
			}

			if ( status == _ok )
			{
				PositionTelescopeToAltAz( target, "SyncToAltAz", true );
			}
		}

		private bool PositionTelescopeToAltAz( Vector target, string methodName, bool isReturnSlew )
		{
			bool retval = true;
			string status = _ok;
			string msg1 = null;
			string msg2 = null;

			try
			{
				Telescope.SlewToAltAz( target.X, target.Y );
				//Thread.Sleep( 5000 );

				double finalAz = Telescope.Azimuth;
				double finalAlt = Telescope.Altitude;

				string text = Util.DegreesToDMS( finalAz, ":", ":", "", 2 );
				msg1 = String.Format( "Slewed {0}to start position OK.  AZ:    {1}"
									, isReturnSlew ? "back " : "", text );

				text = Util.DegreesToDMS( finalAlt, ":", ":", "", 2 );
				msg2 = String.Format( "Slewed {0}to start position OK.  ALT:   {1}"
									, isReturnSlew ? "back " : "", text );
			}
			catch ( Exception )
			{
				status = _error;
				msg1 = String.Format( "Unable to slew {0}to start position"
									, isReturnSlew ? "back " : "" );
				retval = false;
			}

			TestContext.WriteLine( _fmt, NowTime(), methodName, status, msg1 );

			if ( !String.IsNullOrEmpty( msg2 ) )
			{
				TestContext.WriteLine( _fmt, NowTime(), methodName, status, msg2 );
			}

			return retval;
		}

		private void TestSyncToTarget()
		{

			Telescope.Tracking = true;
			TestSyncToTarget_Valid();
			TestSyncToTarget_BadValue( true, -1.0, "(Bad L)" );
			TestSyncToTarget_BadValue( false, -100.0, "(Bad L)" );
			TestSyncToTarget_BadValue( true, 25.0, "(Bad H)" );
			TestSyncToTarget_BadValue( false, 100.0, "(Bad H)" );
		}

		private void TestSyncToTarget_BadValue( bool isRA, double badValue, string suffix )
		{
			string status = _error;
			string msg = null;

			Type expectedException = ( Telescope.CanSync )
									? typeof( DriverAccessCOMException )
									: typeof( MethodNotImplementedException );
			double targetRA;
			double targetDec;
			string badValueFormatted;

			if ( isRA )
			{
				targetRA = badValue;
				targetDec = Telescope.Declination;
				badValueFormatted = Util.HoursToHMS( targetRA, ":", ":", "", 2 );
			}
			else
			{
				targetRA = Telescope.RightAscension;
				targetDec = badValue;
				badValueFormatted = Util.DegreesToDMS( targetDec, ":", ":", "", 2 );
			}

			try
			{
				Telescope.TargetRightAscension = targetRA;
				Telescope.TargetDeclination = targetDec;
				Telescope.SyncToTarget();

				msg = String.Format( "Incorrectly accepted bad {0} coordinate: {1}"
									, isRA ? "RA" : "Dec", badValueFormatted );
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					msg = GetExceptionMessage( xcp ) + " when attempting to SyncToTarget.";
				}
				else
				{
					status = _ok;
					msg = String.Format( "Correctly rejected bad {0} coordinate: {1}"
									, isRA ? "RA" : "Dec", badValueFormatted );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SyncToTarget " + suffix, status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestSyncToTarget_Valid()
		{
			string status = _ok;
			string msg1 = null;
			string msg2 = null;
			Type expectedException = null;
			Vector target;

			expectedException = ( Telescope.CanSync )
									? null
									: typeof( MethodNotImplementedException );

			target = GetSaneRaDec();

			if ( !PositionTelescopeToCoordinates( target, "SyncToTarget", false ) )
			{
				return;
			}

			try
			{
				// Reduce the RA by 4 minutes, but make sure we don't go negative.
				double trueRa = target.X - ( 4.0 / 60.0 );
				trueRa = ( trueRa + 24.0 ) % 24.0;

				// Reduce the Dec by 1 degree;
				// Given the sanity of the target, subtracting 1 degree should still be valid.
				double trueDec = target.Y - 1.0;

				Telescope.TargetRightAscension = trueRa;
				Telescope.TargetDeclination = trueDec;

				Telescope.SyncToTarget();
				//Thread.Sleep( 5000 );

				double targetRa = Telescope.TargetRightAscension;
				double targetDec = Telescope.Declination;

				if ( Math.Abs( trueRa - targetRa ) > _tolerance )
				{
					status = _error;
					msg1 = String.Format( "Sync completed, but Right Ascension does not match - Expected = {0}, Actual = {1}"
										, trueRa, targetRa );
				}
				else if ( Math.Abs( trueDec - targetDec ) > _tolerance )
				{
					status = _error;
					msg1 = String.Format( "Sync completed, but Declination does not match - Expected = {0}, Actual = {1}"
										, trueDec, targetDec );
				}
				else
				{
					string text = Util.HoursToHMS( trueRa, ":", ":", "", 2 );
					msg1 = String.Format( "Synced to sync position OK.   RA:    {0}", text );
					text = Util.DegreesToDMS( trueDec, ":", ":", "", 2 );
					msg2 = String.Format( "Synced to sync position OK.   DEC:   {0}", text );
				}
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					status = _error;
					msg1 = GetExceptionMessage( xcp ) + " when attempting to SyncToTarget.";
				}
				else
				{
					msg1 = String.Format( "CanSync is False and a {0} exception was generated as expected", expectedException.Name );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SyncToTarget", status, msg1 );

			if ( !String.IsNullOrEmpty( msg2 ) )
			{
				TestContext.WriteLine( _fmt, NowTime(), "SyncToTarget", status, msg2 );
			}

			if ( status != _ok )
			{
				return;
			}

			if ( !PositionTelescopeToCoordinates( target, "SyncToTarget", true ) )
			{
				return;
			}

			try
			{
				// Increase the RA by 4 minutes, but make sure we don't go beyond 24 hrs.
				double trueRa = target.X + ( 4.0 / 60.0 );
				trueRa %= 24.0;

				// Increase the Dec by 1 degree;
				// Given the sanity of the target, adding 1 degree should still be valid.
				double trueDec = target.Y + 1.0;

				Telescope.TargetRightAscension = trueRa;
				Telescope.TargetDeclination = trueDec;
				Telescope.SyncToTarget();
				//Thread.Sleep( 5000 );

				double targetRa = Telescope.TargetRightAscension;
				double targetDec = Telescope.Declination;

				if ( Math.Abs( trueRa - targetRa ) > _tolerance )
				{
					status = _error;
					msg1 = String.Format( "Sync to reversed sync position completed, but Right Ascension does not match - Expected = {0}, Actual = {1}"
										, trueRa, targetRa );
				}
				else if ( Math.Abs( trueDec - targetDec ) > _tolerance )
				{
					status = _error;
					msg1 = String.Format( "Sync to reversed sync position completed, but Declination does not match - Expected = {0}, Actual = {1}"
										, trueDec, targetDec );
				}
				else
				{
					string text = Util.HoursToHMS( trueRa, ":", ":", "", 2 );
					msg1 = String.Format( "Synced to reversed sync position OK.   RA:    {0}", text );
					text = Util.DegreesToDMS( trueDec, ":", ":", "", 2 );
					msg2 = String.Format( "Synced to reversed sync position OK.   DEC:   {0}", text );
				}
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					status = _error;
					msg1 = GetExceptionMessage( xcp ) + " when attempting to SyncToTarget.";
				}
				else
				{
					msg1 = String.Format( "CanSync is False and a {0} exception was generated as expected", expectedException.Name );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SyncToTarget", status, msg1 );

			if ( !String.IsNullOrEmpty( msg2 ) )
			{
				TestContext.WriteLine( _fmt, NowTime(), "SyncToTarget", status, msg2 );
			}

			if ( status == _ok )
			{
				PositionTelescopeToCoordinates( target, "SyncToTarget", true );
			}

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestSyncToCoordinates()
		{

			Telescope.Tracking = true;
			TestSyncToCoordinates_Valid();
			TestSyncToCoordinates_BadValue( true, -1.0, "(Bad L)" );
			TestSyncToCoordinates_BadValue( false, -100.0, "(Bad L)" );
			TestSyncToCoordinates_BadValue( true, 25.0, "(Bad H)" );
			TestSyncToCoordinates_BadValue( false, 100.0, "(Bad H)" );
		}

		private void TestSyncToCoordinates_BadValue( bool isRA, double badValue, string suffix )
		{
			string status = _error;
			string msg = null;

			Type expectedException = ( Telescope.CanSync )
									? typeof( DriverAccessCOMException )
									: typeof( MethodNotImplementedException );
			double targetRA;
			double targetDec;
			string badValueFormatted;

			if ( isRA )
			{
				targetRA = badValue;
				targetDec = Telescope.Declination;
				badValueFormatted = Util.HoursToHMS( targetRA, ":", ":", "", 2 );
			}
			else
			{
				targetRA = Telescope.RightAscension;
				targetDec = badValue;
				badValueFormatted = Util.DegreesToDMS( targetDec, ":", ":", "", 2 );
			}

			try
			{
				Telescope.SyncToCoordinates( targetRA, targetDec );

				msg = String.Format( "Incorrectly accepted bad {0} coordinate: {1}"
									, isRA ? "RA" : "Dec", badValueFormatted );
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					msg = GetExceptionMessage( xcp ) + " when attempting to SyncToCoordinates.";
				}
				else
				{
					status = _ok;
					msg = String.Format( "Correctly rejected bad {0} coordinate: {1}"
									, isRA ? "RA" : "Dec", badValueFormatted );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SyncToCoordinates " + suffix, status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestSyncToCoordinates_Valid()
		{
			string status = _ok;
			string msg1 = null;
			string msg2 = null;
			Type expectedException = null;
			Vector target;

			expectedException = ( Telescope.CanSync )
									? null
									: typeof( MethodNotImplementedException );

			target = GetSaneRaDec();

			if ( !PositionTelescopeToCoordinates( target, "SyncToCoordinates", false ) )
			{
				return;
			}

			try
			{
				// Reduce the RA by 4 minutes, but make sure we don't go negative.
				double trueRa = target.X - ( 4.0 / 60.0 );
				trueRa = ( trueRa + 24.0 ) % 24.0;

				// Reduce the Dec by 1 degree;
				// Given the sanity of the target, subtracting 1 degree should still be valid.
				double trueDec = target.Y - 1.0;

				Telescope.SyncToCoordinates( trueRa, trueDec );
				//Thread.Sleep( 5000 );

				double targetRa = Telescope.TargetRightAscension;
				double targetDec = Telescope.Declination;

				if ( Math.Abs( trueRa - targetRa ) > _tolerance )
				{
					status = _error;
					msg1 = String.Format( "Sync completed, but Right Ascension does not match - Expected = {0}, Actual = {1}"
										, trueRa, targetRa );
				}
				else if ( Math.Abs( trueDec - targetDec ) > _tolerance )
				{
					status = _error;
					msg1 = String.Format( "Sync completed, but Declination does not match - Expected = {0}, Actual = {1}"
										, trueDec, targetDec );
				}
				else
				{
					string text = Util.HoursToHMS( trueRa, ":", ":", "", 2 );
					msg1 = String.Format( "Synced to sync position OK.   RA:    {0}", text );
					text = Util.DegreesToDMS( trueDec, ":", ":", "", 2 );
					msg2 = String.Format( "Synced to sync position OK.   DEC:   {0}", text );
				}
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					status = _error;
					msg1 = GetExceptionMessage( xcp ) + " when attempting to SyncToCoordinates.";
				}
				else
				{
					msg1 = String.Format( "CanSync is False and a {0} exception was generated as expected", expectedException.Name );
				}
			}

			TestContext.WriteLine(_fmt, NowTime(), "SyncToCoordinates", status, msg1 );

			if ( !String.IsNullOrEmpty(msg2 ) )
			{
				TestContext.WriteLine(_fmt, NowTime(), "SyncToCoordinates", status, msg2 );
			}

			if ( status != _ok )
			{
				return;
			}

			if ( !PositionTelescopeToCoordinates( target, "SyncToCoordinates", true ) )
			{
				return;
			}

			try
			{
				// Increase the RA by 4 minutes, but make sure we don't go beyond 24 hrs.
				double trueRa = target.X + ( 4.0 / 60.0 );
				trueRa %= 24.0;

				// Increase the Dec by 1 degree;
				// Given the sanity of the target, adding 1 degree should still be valid.
				double trueDec = target.Y + 1.0;

				Telescope.SyncToCoordinates( trueRa, trueDec );
				//Thread.Sleep( 5000 );

				double targetRa = Telescope.TargetRightAscension;
				double targetDec = Telescope.Declination;

				if ( Math.Abs( trueRa - targetRa ) > _tolerance )
				{
					status = _error;
					msg1 = String.Format( "Sync to reversed sync position completed, but Right Ascension does not match - Expected = {0}, Actual = {1}"
										, trueRa, targetRa );
				}
				else if ( Math.Abs( trueDec - targetDec ) > _tolerance )
				{
					status = _error;
					msg1 = String.Format( "Sync to reversed sync position completed, but Declination does not match - Expected = {0}, Actual = {1}"
										, trueDec, targetDec );
				}
				else
				{
					string text = Util.HoursToHMS( trueRa, ":", ":", "", 2 );
					msg1 = String.Format( "Synced to reversed sync position OK.   RA:    {0}", text );
					text = Util.DegreesToDMS( trueDec, ":", ":", "", 2 );
					msg2 = String.Format( "Synced to reversed sync position OK.   DEC:   {0}", text );
				}
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					status = _error;
					msg1 = GetExceptionMessage( xcp ) + " when attempting to SyncToCoordinates.";
				}
				else
				{
					msg1 = String.Format( "CanSync is False and a {0} exception was generated as expected", expectedException.Name );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SyncToCoordinates", status, msg1 );

			if ( !String.IsNullOrEmpty( msg2 ) )
			{
				TestContext.WriteLine( _fmt, NowTime(), "SyncToCoordinates", status, msg2 );
			}

			if ( status == _ok )
			{
				PositionTelescopeToCoordinates( target, "SyncToCoordinates", true ); 
			}

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private bool PositionTelescopeToCoordinates( Vector target, string methodName, bool isReturnSlew )
		{
			bool retval = true;
			string status = _ok;
			string msg1 = null;
			string msg2 = null;

			try
			{
				Telescope.SlewToCoordinates( target.X, target.Y );
				//Thread.Sleep( 5000 );

				double finalRA = Telescope.RightAscension;
				double finalDec = Telescope.Declination;

				string text = Util.HoursToHMS( finalRA, ":", ":", "", 2 );
				msg1 = String.Format( "Slewed {0}to start position OK.  RA:    {1}"
									, isReturnSlew ? "back " : "", text );

				text = Util.DegreesToDMS( finalDec, ":", ":", "", 2 );
				msg2 = String.Format( "Slewed {0}to start position OK.  DEC:   {1}"
									, isReturnSlew ? "back " : "", text );
			}
			catch ( Exception )
			{
				status = _error;
				msg1 = String.Format( "Unable to slew {0}to start position"
									, isReturnSlew ? "back " : "" );
				retval = false;
			}

			TestContext.WriteLine( _fmt, NowTime(), methodName, status, msg1 );

			if ( !String.IsNullOrEmpty( msg2 ) )
			{
				TestContext.WriteLine( _fmt, NowTime(), methodName, status, msg2 );
			}

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}

			return retval;
		}

		private void TestAltAzAsyncSlew()
		{
			// Make sure that we restore tracking.

			bool trackingFlag = Telescope.Tracking;
			Telescope.Tracking = false;

			try
			{
				TestAltAzAsyncSlew_Valid();
				TestAltAzAsyncSlew_BadValue( true, -100.0, "(Bad L)" );
				TestAltAzAsyncSlew_BadValue( false, -10.0, "(Bad L)" );
				TestAltAzAsyncSlew_BadValue( true, 100.0, "(Bad H)" );
				TestAltAzAsyncSlew_BadValue( false, 370.0, "(Bad H)" );
			}
			catch ( Exception xcp )
			{
				throw xcp;
			}
			finally
			{
				Telescope.Tracking = trackingFlag;
			}
		}

		private void TestAltAzAsyncSlew_BadValue( bool isAlt, double badValue, string suffix )
		{
			string status = _error;
			string msg = null;

			Type expectedException = ( Telescope.CanSlewAltAz && Telescope.CanSlewAltAzAsync )
									? typeof( DriverAccessCOMException )
									: typeof( MethodNotImplementedException );
			double targetAz;
			double targetAlt;
			string badValueFormatted;

			if ( isAlt )
			{
				targetAlt = badValue;
				targetAz = Telescope.Azimuth;
				badValueFormatted = Util.DegreesToDMS( targetAlt, ":", ":", "", 2 );
			}
			else
			{
				targetAlt = Telescope.Altitude;
				targetAz = badValue;
				badValueFormatted = Util.DegreesToDMS( targetAz, ":", ":", "", 2 );
			}

			try
			{
				Telescope.SlewToAltAz( targetAz, targetAlt );

				bool timedOut = WaitForSlewToComplete( 60.0 );
				string msgEnd = "";

				if ( timedOut )
				{
					Telescope.AbortSlew();
					msgEnd = "and slew did not complete.";
				}

				msg = String.Format( "Incorrectly accepted bad {0} coordinate: {1} {2}"
										, isAlt ? "Alt" : "Az", badValueFormatted, msgEnd );
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					msg = GetExceptionMessage( xcp ) + " when attempting to SlewToAltAzAsync.";
				}
				else
				{
					status = _ok;
					msg = String.Format( "Correctly rejected bad {0} coordinate: {1}"
									, isAlt ? "Alt" : "Az", badValueFormatted );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SlewToAltAzAsync " + suffix, status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestAltAzAsyncSlew_Valid()
		{
			string status = _ok;
			string msg1 = null;
			string msg2 = null;
			bool timedOut = false;
			Type expectedException = null;

			Vector target = new Vector( 135.0, 50.0 );

			expectedException = ( Telescope.CanSlewAltAz && Telescope.CanSlewAltAzAsync )
									? null
									: typeof( MethodNotImplementedException );

			try
			{
				Telescope.SlewToAltAz( target.X, target.Y );
				timedOut = WaitForSlewToComplete( 60.0 );

				if ( timedOut )
				{
					Telescope.AbortSlew();
					status = _error;
					msg1 = "Slew did not complete within a reasonable time.";
				}
				else
				{
					double finalAz = Telescope.Azimuth;
					double finalAlt = Telescope.Altitude;

					if ( Math.Abs( target.X - finalAz ) > _tolerance )
					{
						status = _error;
						msg1 = String.Format( "Slew completed, but Azimuth does not match - Expected = {0}, Actual = {1}"
											, target.X, finalAz );
					}
					else if ( Math.Abs( target.Y - finalAlt ) > _tolerance )
					{
						status = _error;
						msg1 = String.Format( "Slew completed, but Altitude does not match - Expected = {0}, Actual = {1}"
											, target.Y, finalAlt );
					}
					else
					{
						string text = Util.HoursToHMS( finalAz, ":", ":", "", 2 );
						msg1 = String.Format( "Slewed to target Azimuth OK: {0}", text );
						text = Util.DegreesToDMS( finalAlt, ":", ":", "", 2 );
						msg2 = String.Format( "Slewed to target Altitude OK: {0}", text );
					}
				}
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					status = _error;
					msg1 = GetExceptionMessage( xcp ) + " when attempting to SlewToAltAz.";
				}
				else
				{
					msg1 = String.Format( "CanSlewAltAz is False and a {0} exception was generated as expected", expectedException.Name );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SlewToAltAz", status, msg1 );

			if ( !String.IsNullOrEmpty( msg2 ) )
			{
				TestContext.WriteLine( _fmt, NowTime(), "SlewToAltAz", status, msg2 );
			}

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestAltAzSlew()
		{
			// Make sure that we restore tracking.

			bool trackingFlag = Telescope.Tracking;
			Telescope.Tracking = false;

			try
			{
				TestAltAzSlew_Valid();
				TestAltAzSlew_BadValue( true, -100.0, "(Bad L)" );
				TestAltAzSlew_BadValue( false, -10.0, "(Bad L)" );
				TestAltAzSlew_BadValue( true, 100.0, "(Bad H)" );
				TestAltAzSlew_BadValue( false, 370.0, "(Bad H)" );
			}
			catch ( Exception xcp )
			{
				throw xcp;
			}
			finally
			{
				Telescope.Tracking = trackingFlag;
			}
		}

		private void TestAltAzSlew_BadValue( bool isAlt, double badValue, string suffix )
		{
			string status = _error;
			string msg = null;

			Type expectedException = ( Telescope.CanSlewAltAz )
									? typeof( DriverAccessCOMException )
									: typeof( MethodNotImplementedException );
			double targetAz;
			double targetAlt;
			string badValueFormatted;

			if ( isAlt )
			{
				targetAlt = badValue;
				targetAz = Telescope.Azimuth;
				badValueFormatted = Util.DegreesToDMS( targetAlt, ":", ":", "", 2 );
			}
			else
			{
				targetAlt = Telescope.Altitude;
				targetAz = badValue;
				badValueFormatted = Util.DegreesToDMS( targetAz, ":", ":", "", 2 );
			}

			try
			{
				Telescope.SlewToAltAz( targetAz, targetAlt );

				msg = String.Format( "Incorrectly accepted bad {0} coordinate: {1}"
										, isAlt ? "Alt" : "Az", badValueFormatted );
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					msg = GetExceptionMessage( xcp ) + " when attempting to SlewToAltAz.";
				}
				else
				{
					status = _ok;
					msg = String.Format( "Correctly rejected bad {0} coordinate: {1}"
									, isAlt ? "Alt" : "Az", badValueFormatted );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SlewToAltAz " + suffix, status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestAltAzSlew_Valid()
		{
			string status = _ok;
			string msg1 = null;
			string msg2 = null;

			Type expectedException = null;
			Vector target = new Vector( 135.0, 50.0 );

			expectedException = ( Telescope.CanSlewAltAz )
									? null
									: typeof( MethodNotImplementedException );

			try
			{
				Telescope.SlewToAltAz( target.X, target.Y );

				double finalAz = Telescope.Azimuth;
				double finalAlt = Telescope.Altitude;

				if ( Math.Abs( target.X - finalAz ) > _tolerance )
				{
					status = _error;
					msg1 = String.Format( "Slew completed, but Azimuth does not match - Expected = {0}, Actual = {1}"
										, target.X, finalAz );
				}
				else if ( Math.Abs( target.Y - finalAlt ) > _tolerance )
				{
					status = _error;
					msg1 = String.Format( "Slew completed, but Altitude does not match - Expected = {0}, Actual = {1}"
										, target.Y, finalAlt );
				}
				else
				{
					string text = Util.HoursToHMS( finalAz, ":", ":", "", 2 );
					msg1 = String.Format( "Slewed to target Azimuth OK: {0}", text );
					text = Util.DegreesToDMS( finalAlt, ":", ":", "", 2 );
					msg2 = String.Format( "Slewed to target Altitude OK: {0}", text );
				}
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					status = _error;
					msg1 = GetExceptionMessage( xcp ) + " when attempting to SlewToAltAz.";
				}
				else
				{
					msg1 = String.Format( "CanSlewAltAz is False and a {0} exception was generated as expected", expectedException.Name );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SlewToAltAz", status, msg1 );

			if ( !String.IsNullOrEmpty( msg2 ) )
			{
				TestContext.WriteLine( _fmt, NowTime(), "SlewToAltAz", status, msg2 );
			}

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestDestinationSideOfPier()
		{
			string status = _error;
			string msg = "DestinationSideOfPier does not change as expected";

			// Move to sane pier west position and get the destination side-of-pier 
			// value for a pier east position.

			Vector targetPierWest = GetSaneRaDec();
			Vector targetPierEast = GetSaneRaDec( -6, 0 );

			bool trackingFlag = Telescope.Tracking;
			Telescope.Tracking = true;

			try
			{
				Telescope.SlewToCoordinates( targetPierWest.X, targetPierWest.Y );

				PierSide currentPierSide = Telescope.SideOfPier;
				PierSide targetPierSide = Telescope.DestinationSideOfPier( targetPierEast.X, targetPierEast.Y );

				if ( currentPierSide == PierSide.pierWest && targetPierSide != currentPierSide )
				{
					Telescope.SlewToCoordinates( targetPierEast.X, targetPierEast.Y );
					//Thread.Sleep( 5000 );

					currentPierSide = Telescope.SideOfPier;
					targetPierSide = Telescope.DestinationSideOfPier( targetPierWest.X, targetPierWest.Y );

					if ( currentPierSide == PierSide.pierEast && targetPierSide != currentPierSide )
					{
						status = _ok;
						msg = "DestinationSideOfPier is different on either side of the meridian";
					}
				}
			}
			catch ( Exception xcp )
			{
				msg = GetExceptionMessage( xcp )
						+ " when attempting to determine DestinationSideOfPier.";
			}

			TestContext.WriteLine( _fmt, NowTime(), "DestinationSideOfPier", status, msg );

			Telescope.Tracking = trackingFlag;

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestTargetAsyncSlew()
		{
			bool trackingFlag = Telescope.Tracking;
			Telescope.Tracking = true;

			goto StartTest;
			StartTest:

			TestTargetSlewAsync_Valid();
			TestTargetSlewAsync_BadValue( true, -1.0, "(Bad L)" );
			TestTargetSlewAsync_BadValue( false, -100.0, "(Bad L)" );
			TestTargetSlewAsync_BadValue( true, 25.0, "(Bad H)" );
			TestTargetSlewAsync_BadValue( false, 100.0, "(Bad H)" );

			Telescope.Tracking = trackingFlag;
		}

		private void TestTargetSlewAsync_BadValue( bool isRA, double badValue, string suffix )
		{
			string status = _error;
			string msg = null;

			Type expectedException = ( Telescope.CanSlew && Telescope.CanSlewAsync )
									? typeof( DriverAccessCOMException )
									: typeof( MethodNotImplementedException );
			double targetRA;
			double targetDec;
			string badValueFormatted;

			if ( isRA )
			{
				targetRA = badValue;
				targetDec = Telescope.Declination;
				badValueFormatted = Util.HoursToHMS( targetRA, ":", ":", "", 2 );
			}
			else
			{
				targetRA = Telescope.RightAscension;
				targetDec = badValue;
				badValueFormatted = Util.DegreesToDMS( targetDec, ":", ":", "", 2 );
			}

			try
			{
				Telescope.TargetRightAscension = targetRA;
				Telescope.TargetDeclination = targetDec;

				Telescope.SlewToTargetAsync();

				bool timedOut = WaitForSlewToComplete( 60.0 );
				string msgEnd = "";

				if ( timedOut )
				{
					Telescope.AbortSlew();
					msgEnd = "and slew did not complete.";
				}

				msg = String.Format( "Incorrectly accepted bad {0} coordinate: {1} {2}"
										, isRA ? "RA" : "Dec", badValueFormatted, msgEnd );
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					msg = GetExceptionMessage( xcp ) + " when attempting to SlewToTargetAsync.";
				}
				else
				{
					status = _ok;
					msg = String.Format( "Correctly rejected bad {0} coordinate: {1}"
									, isRA ? "RA" : "Dec", badValueFormatted );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SlewToTargetAsync " + suffix, status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestTargetSlewAsync_Valid()
		{
			string status = _ok;
			string msg1 = null;
			string msg2 = null;
			bool timedOut = false;
			Type expectedException = null;
			Vector target;

			expectedException = ( Telescope.CanSlew && Telescope.CanSlewAsync )
									? null
									: typeof( MethodNotImplementedException );

			try
			{
				target = GetSaneRaDec( -2.0, 2.0);

				Telescope.TargetRightAscension = target.X;
				Telescope.TargetDeclination = target.Y;

				Telescope.SlewToTargetAsync();
				timedOut = WaitForSlewToComplete( 60.0 );

				if ( timedOut )
				{
					Telescope.AbortSlew();
					status = _error;
					msg1 = "Slew did not complete within a reasonable time.";
				}
				else
				{
					double finalRA = Telescope.RightAscension;
					double finalDec = Telescope.Declination;

					if ( Math.Abs( target.X - finalRA ) > _tolerance )
					{
						status = _error;
						msg1 = String.Format( "Slew completed, but RA does not match - Expected = {0}, Actual = {1}"
											, target.X, finalRA );
					}
					else if ( Math.Abs( target.Y - finalDec ) > _tolerance )
					{
						status = _error;
						msg1 = String.Format( "Slew completed, but Dec does not match - Expected = {0}, Actual = {1}"
											, target.Y, finalDec );
					}
					else
					{
						string text = Util.HoursToHMS( finalRA, ":", ":", "", 2 );
						msg1 = String.Format( "Slewed OK.   RA:    {0}", text );
						text = Util.DegreesToDMS( finalDec, ":", ":", "", 2 );
						msg2 = String.Format( "Slewed OK.   DEC:   {0}", text );
					}
				}
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					status = _error;
					msg1 = GetExceptionMessage( xcp ) + " when attempting to SlewToTargetAsync.";
				}
				else
				{
					msg1 = String.Format( "CanSlew is False and a {0} exception was generated as expected", expectedException.Name );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SlewToTargetAsync", status, msg1 );

			if ( !String.IsNullOrEmpty( msg2 ) )
			{
				TestContext.WriteLine( _fmt, NowTime(), "SlewToTargetAsync", status, msg2 );
			}

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestTargetSlew()
		{
			bool trackingFlag = Telescope.Tracking;
			Telescope.Tracking = true;

			goto StartTest;
			StartTest:

			TestTargetSlew_Valid();
			TestTargetSlew_BadValue( true, -1.0, "(Bad L)" );
			TestTargetSlew_BadValue( false, -100.0, "(Bad L)" );
			TestTargetSlew_BadValue( true, 25.0, "(Bad H)" );
			TestTargetSlew_BadValue( false, 100.0, "(Bad H)" );

			Telescope.Tracking = trackingFlag;
		}

		private void TestTargetSlew_BadValue( bool isRA, double badValue, string suffix )
		{
			string status = _error;
			string msg = null;

			Type expectedException = ( Telescope.CanSlew )
									? typeof( DriverAccessCOMException )
									: typeof( MethodNotImplementedException );
			double targetRA;
			double targetDec;
			string badValueFormatted;

			if ( isRA )
			{
				targetRA = badValue;
				targetDec = Telescope.Declination;
				badValueFormatted = Util.HoursToHMS( targetRA, ":", ":", "", 2 );
			}
			else
			{
				targetRA = Telescope.RightAscension;
				targetDec = badValue;
				badValueFormatted = Util.DegreesToDMS( targetDec, ":", ":", "", 2 );
			}

			try
			{
				Telescope.TargetRightAscension = targetRA;
				Telescope.TargetDeclination = targetDec;

				Telescope.SlewToTarget();

				msg = String.Format( "Incorrectly accepted bad {0} coordinate: {1}"
										, isRA ? "RA" : "Dec", badValueFormatted );
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					msg = GetExceptionMessage( xcp ) + " when attempting to SlewToTarget.";
				}
				else
				{
					status = _ok;
					msg = String.Format( "Correctly rejected bad {0} coordinate: {1}"
									, isRA ? "RA" : "Dec", badValueFormatted );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SlewToTarget " + suffix, status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestTargetSlew_Valid()
		{
			string status = _ok;
			string msg1 = null;
			string msg2 = null;

			Type expectedException = null;
			Vector target;

			expectedException = ( Telescope.CanSlew )
									? null
									: typeof( MethodNotImplementedException );

			try
			{
				target = GetSaneRaDec();

				Telescope.TargetRightAscension = target.X;
				Telescope.TargetDeclination = target.Y;
				Telescope.SlewToTarget();

				double finalRA = Telescope.RightAscension;
				double finalDec = Telescope.Declination;

				if ( Math.Abs( target.X - finalRA ) > _tolerance )
				{
					status = _error;
					msg1 = String.Format( "Slew completed, but RA does not match - Expected = {0}, Actual = {1}"
										, target.X, finalRA );
				}
				else if ( Math.Abs( target.Y - finalDec ) > _tolerance )
				{
					status = _error;
					msg1 = String.Format( "Slew completed, but Dec does not match - Expected = {0}, Actual = {1}"
										, target.Y, finalDec );
				}
				else
				{
					string text = Util.HoursToHMS( finalRA, ":", ":", "", 2 );
					msg1 = String.Format( "Slewed OK.   RA:    {0}", text );
					text = Util.DegreesToDMS( finalDec, ":", ":", "", 2 );
					msg2 = String.Format( "Slewed OK.   DEC:   {0}", text );
				}
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					status = _error;
					msg1 = GetExceptionMessage( xcp ) + " when attempting to SlewToTarget.";
				}
				else
				{
					msg1 = String.Format( "CanSlew is False and a {0} exception was generated as expected", expectedException.Name );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SlewToTarget", status, msg1 );

			if ( !String.IsNullOrEmpty( msg2 ) )
			{
				TestContext.WriteLine( _fmt, NowTime(), "SlewToTarget", status, msg2 );
			}

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestCoordinateSlewAsync()
		{
			bool trackingFlag = Telescope.Tracking;
			Telescope.Tracking = true;

			TestCoordinateSlewAsync_Valid();
			TestCoordinateSlewAsync_BadValue( true, -1.0, "(Bad L)" );
			TestCoordinateSlewAsync_BadValue( false, -100.0, "(Bad L)" );
			TestCoordinateSlewAsync_BadValue( true, 25.0, "(Bad H)" );
			TestCoordinateSlewAsync_BadValue( false, 100.0, "(Bad H)" );

			Telescope.Tracking = trackingFlag;
		}

		private void TestCoordinateSlewAsync_BadValue( bool isRA, double badValue, string suffix )
		{
			string status = _error;
			string msg = null;

			Type expectedException = ( Telescope.CanSlew && Telescope.CanSlewAsync )
									? typeof( DriverAccessCOMException )
									: typeof( MethodNotImplementedException );
			double targetRA;
			double targetDec;
			string badValueFormatted;

			if ( isRA )
			{
				targetRA = badValue;
				targetDec = Telescope.Declination;
				badValueFormatted = Util.HoursToHMS( targetRA, ":", ":", "", 2 );
			}
			else
			{
				targetRA = Telescope.RightAscension;
				targetDec = badValue;
				badValueFormatted = Util.DegreesToDMS( targetDec, ":", ":", "", 2 );
			}

			try
			{
				Telescope.SlewToCoordinatesAsync( targetRA, targetDec );

				bool timedOut = WaitForSlewToComplete( 60.0 );
				string msgEnd = "";

				if ( timedOut )
				{
					Telescope.AbortSlew();
					msgEnd = "and slew did not complete.";
				}

				msg = String.Format( "Incorrectly accepted bad {0} coordinate: {1} {2}"
										, isRA ? "RA" : "Dec", badValueFormatted, msgEnd );
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					msg = GetExceptionMessage( xcp ) + " when attempting to SlewToCoordinatesAsync.";
				}
				else
				{
					status = _ok;
					msg = String.Format( "Correctly rejected bad {0} coordinate: {1}"
									, isRA ? "RA" : "Dec", badValueFormatted );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SlewToCoordinatesAsync " + suffix, status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestCoordinateSlewAsync_Valid()
		{
			string status = _ok;
			string msg1 = null;
			string msg2 = null;
			bool timedOut = false;
			Type expectedException = null;
			Vector target;

			expectedException = ( Telescope.CanSlew )
									? null
									: typeof( MethodNotImplementedException );
			try
			{
				target = GetSaneRaDec( -1.0, 1.0 );

				Telescope.SlewToCoordinatesAsync( target.X, target.Y );
				timedOut = WaitForSlewToComplete( 60.0 );

				if ( timedOut )
				{
					Telescope.AbortSlew();
					status = _error;
					msg1 = "Slew did not complete within a reasonable time.";
				}
				else
				{
					double finalRA = Telescope.RightAscension;
					double finalDec = Telescope.Declination;

					if ( Math.Abs( target.X - finalRA ) > _tolerance )
					{
						status = _error;
						msg1 = String.Format( "Slew completed, but RA does not match - Expected = {0}, Actual = {1}"
											, target.X, finalRA );
					}
					else if ( Math.Abs( target.Y - finalDec ) > _tolerance )
					{
						status = _error;
						msg1 = String.Format( "Slew completed, but Dec does not match - Expected = {0}, Actual = {1}"
											, target.Y, finalDec );
					}
					else
					{
						string text = Util.HoursToHMS( finalRA, ":", ":", "", 2 );
						msg1 = String.Format( "Slewed OK.   RA:    {0}", text );
						text = Util.DegreesToDMS( finalDec, ":", ":", "", 2 );
						msg2 = String.Format( "Slewed OK.   DEC:   {0}", text );
					}
				}
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					status = _error;
					msg1 = GetExceptionMessage( xcp ) + " when attempting to SlewToCoordinatesAsync.";
				}
				else
				{
					msg1 = String.Format( "CanSlew is False and a {0} exception was generated as expected", expectedException.Name );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SlewToCoordinatesAsync", status, msg1 );

			if ( !String.IsNullOrEmpty( msg2 ) )
			{
				TestContext.WriteLine( _fmt, NowTime(), "SlewToCoordinatesAsync", status, msg2 );
			}

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private static bool WaitForSlewToComplete( double secondsToWait )
		{
			bool retval = false;
			DateTime timeoutTime = DateTime.Now.AddSeconds( secondsToWait );

			while ( Telescope.Slewing )
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

		private void TestCoordinateSlew()
		{
			bool trackingFlag = Telescope.Tracking;

			Telescope.Tracking = true;

			TestCoordinateSlew_Valid();
			TestCoordinateSlew_BadValue( true, -1.0, "(Bad L)" );
			TestCoordinateSlew_BadValue( false, -100.0, "(Bad L)" );
			TestCoordinateSlew_BadValue( true, 25.0, "(Bad H)" );
			TestCoordinateSlew_BadValue( false, 100.0, "(Bad H)" );

			Telescope.Tracking = trackingFlag;
		}

		private void TestCoordinateSlew_BadValue( bool isRA, double badValue, string suffix )
		{
			string status = _error;
			string msg = null;

			Type expectedException = ( Telescope.CanSlew )
									? typeof( DriverAccessCOMException )
									: typeof( MethodNotImplementedException );
			double targetRA;
			double targetDec;
			string badValueFormatted;

			if ( isRA )
			{
				targetRA = badValue;
				targetDec = Telescope.Declination;
				badValueFormatted = Util.HoursToHMS( targetRA, ":", ":", "", 2 );
			}
			else
			{
				targetRA = Telescope.RightAscension;
				targetDec = badValue;
				badValueFormatted = Util.DegreesToDMS( targetDec, ":", ":", "", 2 );
			}

			try
			{
				Telescope.SlewToCoordinates( targetRA, targetDec );

				msg = String.Format( "Incorrectly accepted bad {0} coordinate: {1}"
									, isRA ? "RA" : "Dec", badValueFormatted );
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					msg = GetExceptionMessage( xcp ) + " when attempting to SlewToCoordinates.";
				}
				else
				{
					status = _ok;
					msg = String.Format( "Correctly rejected bad {0} coordinate: {1}"
									, isRA ? "RA" : "Dec", badValueFormatted );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SlewToCoordinates " + suffix, status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestCoordinateSlew_Valid()
		{
			string status = _ok;
			string msg1 = null;
			string msg2 = null;

			Type expectedException = null;
			Vector target;

			expectedException = ( Telescope.CanSlew ) 
									? typeof( MethodNotImplementedException )
									: null;

			try
			{
				target = GetSaneRaDec();

				Telescope.SlewToCoordinates( target.X, target.Y );
				Thread.Sleep( 5000 );

				double finalRA = Telescope.RightAscension;
				double finalDec = Telescope.Declination;

				double diff = Math.Abs( target.X - finalRA );
				if ( Math.Abs( target.X - finalRA ) > _tolerance )
				{
					status = _error;
					msg1 = String.Format( "Slew completed, but RA does not match - Expected = {0}, Actual = {1}"
										, target.X, finalRA );
				}
				else if ( Math.Abs( target.Y - finalDec ) > _tolerance )
				{
					status = _error;
					msg1 = String.Format ("Slew completed, but Dec does not match - Expected = {0}, Actual = {1}"
										, target.Y, finalDec );
				}
				else
				{
					string text = Util.HoursToHMS( finalRA, ":", ":", "", 2 );
					msg1 = String.Format( "Slewed OK.   RA:    {0}", text );
					text = Util.DegreesToDMS( finalDec, ":", ":", "", 2 );
					msg2 = String.Format( "Slewed OK.   DEC:   {0}", text );
				}
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					status = _error;
					msg1 = GetExceptionMessage( xcp ) + " when attempting to SlewToCoordinates.";
				}
				else
				{
					msg1 = String.Format( "CanSlew is False and a {0} exception was generated as expected", expectedException.Name );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "SlewToCoordinates", status, msg1 );

			if ( !String.IsNullOrEmpty( msg2 ) )
			{
				TestContext.WriteLine( _fmt, NowTime(), "SlewToCoordinates", status, msg2 );
			}

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private Vector GetSaneRaDec()
		{
			return GetSaneRaDec( 0.0, 0.0 );
		}

		private Vector GetSaneRaDec( double deltaRA, double deltaDec )
		{

			// Return a sane slew target.

			double targetRightAscension = Double.NaN;
			double targetDeclination = Double.NaN;

			try
			{
				Transform xform = new Transform
				{
					SiteElevation = Telescope.SiteElevation,
					SiteLatitude = Telescope.SiteLatitude,
					SiteLongitude = Telescope.SiteLongitude
				};

				xform.SetAzimuthElevation( _saneAzimuth, _saneAltitude );

				targetRightAscension = xform.RATopocentric;
				targetDeclination = xform.DECTopocentric;
			}
			catch ( Exception )
			{ }

			targetRightAscension += deltaRA;
			targetRightAscension = ( targetRightAscension + 24.0 ) % 24.0;
			targetDeclination += deltaDec;
			targetDeclination =  ( targetDeclination + 90.0 ) % 90.0;

			return new Vector( targetRightAscension, targetDeclination );
		}

		private void TestPulseGuide()
		{
			string status = _ok;
			string msg;
			Type expectedException = null;
			
			try
			{
				if ( !Telescope.CanPulseGuide )
				{
					expectedException = typeof( MethodNotImplementedException );
				}

				bool isSynchronous = IsSynchronousPulseGuiding();

				Telescope.PulseGuide( GuideDirections.guideWest, 1500 );

				if ( isSynchronous )
				{
					if ( Telescope.IsPulseGuiding )
					{
						status = _issue;
						msg = "Synchronous pulseguide expected but IsPulseGuiding has returned TRUE";
					}
					else
					{
						msg = "Synchronous pulseguide found OK";
					}
				}
				else
				{
					msg = "Asynchronous pulseguide found OK";

				}
			}
			catch ( Exception xcp )
			{
				if ( expectedException == null || xcp.GetType() != expectedException )
				{
					status = _error;
					msg = GetExceptionMessage( xcp ) + " when attempting to PulseGuide.";
				}
				else
				{
					msg = String.Format( "CanPulseGuide is False and a {0} exception was generated as expected", expectedException.Name );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "PulseGuide", status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private bool IsSynchronousPulseGuiding()
		{
			// Issue a PulseGuide command and measure how long it takes to return. If it returns
			// before the command has finished then it was asynchronous.

			int pulseDuration = 500;

			DateTime startTime = DateTime.Now;
			Telescope.PulseGuide( GuideDirections.guideWest, pulseDuration );
			DateTime returnTime = DateTime.Now;
			Thread.Sleep( pulseDuration );

			TimeSpan span = returnTime - startTime;

			bool retval = span.TotalMilliseconds >= pulseDuration;

			return retval;
		}

		private void TestMoveAxes()
		{
			TelescopeAxes[] axes = (TelescopeAxes[])Enum.GetValues( typeof( TelescopeAxes ) );

			foreach ( TelescopeAxes axis in axes )
			{
				TestMoveAxis( axis );
			}
		}

		private void TestMoveAxis( TelescopeAxes axis )
		{
			string status;
			string msg;
			string axisName = _axisNames[(int)axis];

			if ( Telescope.CanMoveAxis( axis ) )
			{
				double moveRate = 0.0;
				TestMoveAxisValidRate( axis, moveRate
					, "Can successfully set a movement rate of zero" );

				double minMoveRate = Double.MinValue;
				Double maxMoveRate = Double.MaxValue;

				GetHighLowAxisRates( axis, ref minMoveRate, ref maxMoveRate );

				moveRate = maxMoveRate + 1.0;
				string message = String.Format( " below lowest rate ({0:F0})", -moveRate );
				TestMoveAxisInvalidRate( axis, -moveRate, message, typeof( DriverAccessCOMException ) );

				message = String.Format( " above highest rate ({0:F0})", moveRate );
				TestMoveAxisInvalidRate( axis, moveRate, message, typeof( DriverAccessCOMException ) );

				moveRate = minMoveRate;
				message = String.Format( "Successfully moved axis at minimum rate: {0}", moveRate );
				bool trackingRestored = TestMoveAxisValidRate( axis, moveRate, message );

				moveRate = maxMoveRate;
				message = String.Format( "Successfully move axis at maximum rate: {0}", moveRate );
				trackingRestored = trackingRestored & TestMoveAxisValidRate( axis, moveRate, message );

				status = trackingRestored ? _ok : _error;
				msg = trackingRestored ? "Tracking state correctly retained for both tracking states" 
							: "Tracking state not correctly retained after MoveAxis calls";
				TestContext.WriteLine( _fmt, NowTime(), "MoveAxis " + axisName, status, msg );

				if ( status == _error )
				{
					_failedConformanceCheck = true;
				}
			}
			else
			{
				double moveRate = 0.0;
				msg = String.Format( "CanMoveAxis {0} is False and a MethodNotImplementedException was generated as expected", axisName );
				TestMoveAxisInvalidRate( axis, moveRate, msg, typeof( MethodNotImplementedException ) );
			}
		}

		private void GetHighLowAxisRates( TelescopeAxes axis, ref double minRate, ref double maxRate )
		{
			double minimum = Double.MaxValue;
			double maximum = 0.0;

			IAxisRates axisRates = Telescope.AxisRates( axis );

			for ( int i = 1; i <= axisRates.Count; ++i )
			{
				IRate axisRate = axisRates[i];
				minimum = Math.Min( minimum, axisRate.Minimum );
				maximum = Math.Max( maximum, axisRate.Maximum );
			}

			minRate = minimum;
			maxRate = maximum;
		}

		// More complex than I thought....finish later 
		private bool TestMoveAxisValidRate( TelescopeAxes axis, double moveRate, string message )
		{
			bool trackingRestored = false;
			bool initialTrackingFlag = Telescope.Tracking;

			string msg = "";
			string status = _error;
			try
			{
				Telescope.MoveAxis( axis, moveRate );

				if ( moveRate == 0.0 )
				{
					msg = message;
					trackingRestored = true;
				}
				else
				{
					Telescope.MoveAxis( axis, 0.0 );
					trackingRestored = initialTrackingFlag == Telescope.Tracking;
				}

				msg = message;
				status = _ok;
			}
			catch ( Exception xcp )
			{
				msg = GetExceptionMessage( xcp ) + " when movement rate set to zero";
			}

			string axisName = _axisNames[(int)axis];
			TestContext.WriteLine( _fmt, NowTime(), "MoveAxis " + axisName, status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}

			return trackingRestored;
		}

		private void TestMoveAxisInvalidRate( TelescopeAxes axis, double moveRate, string messageSuffix, Type expectedException = null )
		{
			string msg = "";
			string status = _error;
			try
			{
				Telescope.MoveAxis( axis, moveRate );
				msg = String.Format( "Invalid movement rate ({0}) accepted.", moveRate );
				status = _error;
			}
			catch ( Exception xcp )
			{
				msg = GetExceptionMessage( xcp );

				if ( msg.Contains( "invalid value" ) && expectedException == typeof( DriverAccessCOMException ) )
				{
					msg = "Exception correctly generated when move rate set " + messageSuffix;
					status = _ok;
				}
				else if ( msg.Contains( "not implemented" ) && expectedException == typeof( MethodNotImplementedException ) )
				{
					msg = messageSuffix;
					status = _ok;
				}
			}

			string axisName = _axisNames[(int)axis];
			TestContext.WriteLine( _fmt, NowTime(), "MoveAxis " + axisName, status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestFindHome()
		{
			string status = _error;
			string msg = "Telescope did not reach its home position";

			try
			{
				Telescope.FindHome();

				DateTime abortTime = DateTime.Now + TimeSpan.FromSeconds( 90.0 );

				while ( DateTime.Now < abortTime )
				{
					if ( Telescope.AtHome )
					{
						status = _ok;
						msg = "Found home OK";

						break;
					}

					Thread.Sleep( 1000 );
				}
			}
			catch ( Exception xcp )
			{
				msg = GetExceptionMessage( xcp );
			}

			TestContext.WriteLine(_fmt, NowTime(), "FindHome", status, msg);

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestAxisRates()
		{
			TelescopeAxes[] axes = (TelescopeAxes[])Enum.GetValues( typeof( TelescopeAxes ) );

			foreach ( TelescopeAxes axis in axes )
			{
				string axisName = _axisNames[(int)axis];
				TestAxisRates( axis, axisName );
			}
		}

		private void TestAxisRates( TelescopeAxes axis, string axisName )
		{
			string msg = "";
			string status = _error;
			Exception except;

			try
			{
				IAxisRates axisRates = Telescope.AxisRates( axis );
				status = _ok;

				if ( !Telescope.CanMoveAxis( axis ) && axisRates.Count == 0 )
				{
					TestContext.WriteLine( _fmt, NowTime(), "AxisRate:" + axisName, status, "Empty axis rate returned" );

					return;
				}

				double minimum;
				double maximum;

				for ( int i = 1; i <= axisRates.Count; ++i )
				{
					IRate axisRate = axisRates[i];
					minimum = axisRate.Minimum;
					maximum = axisRate.Maximum;

					msg = String.Format( "Axis rate minimum: {0:F5} Axis rate maximum: {1:F5}", minimum, maximum );

					TestContext.WriteLine( _fmt, NowTime(), "AxisRate:" + axisName, status, msg );
				}

				for ( int i = 1; i < axisRates.Count; ++i )
				{
					bool overlappingRates = false;
					bool duplicateRates = false;

					IRate axisRate = axisRates[i];
					minimum = axisRate.Minimum;
					maximum = axisRate.Maximum;

					for ( int j = 2; j <= axisRates.Count; ++j )
					{
						overlappingRates = AreOverlapping( minimum, maximum, axisRates[j].Minimum, axisRates[j].Maximum );

						if ( overlappingRates )
						{
							break;
						}
					}

					if ( overlappingRates )
					{
						msg = "Two or more axis rates overlap";
						status = _error;
					}
					else
					{
						msg = "No overlapping axis rates found";
						status = _ok;
					}
					TestContext.WriteLine( _fmt, NowTime(), "AxisRate:" + axisName, status, msg );

					for ( int j = 2; j <= axisRates.Count; ++j )
					{
						duplicateRates = AreSameRates( minimum, maximum, axisRates[j].Minimum, axisRates[j].Maximum );

						if ( duplicateRates )
						{
							break;
						}
					}

					if ( duplicateRates )
					{
						msg = "Two or more axis rates are duplicates";
						status = _error;
					}
					else
					{
						msg = "No duplicate axis rates found";
						status = _ok;
					}

					TestContext.WriteLine( _fmt, NowTime(), "AxisRate:" + axisName, status, msg );
				}
			}
			catch ( Exception xcp )
			{
				except = xcp;
				msg = GetExceptionMessage( xcp );
			}

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private bool AreOverlapping( double min1, double max1, double min2, double max2 )
		{
			return Math.Max( min1, min2 ) <= Math.Min( max1, max2 );
		}

		private bool AreSameRates( double min1, double max1, double min2, double max2)
		{
			double tolerance = 0.000001;

			return Math.Abs( min1 - min2 ) < tolerance && Math.Abs( max1 - max2 ) < tolerance;
		}

		private void TestAbortSlewNotSlewing()
		{
			string msg;
			string status = _error;
			Exception except;

			if ( Telescope.Slewing )
			{
				msg = "Unable to test because the telescope is unexpectedly slewing";
			}
			else
			{
				try
				{
					Telescope.AbortSlew();
					status = _ok;
					msg = "AbortSlew Ok when not slewing";
				}
				catch ( Exception xcp )
				{
					except = xcp;
					msg = GetExceptionMessage( xcp );
				}
			}

			TestContext.WriteLine( _fmt, NowTime(), "AbortSlew", status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}
		}

		private void TestParkUnpark()
		{
			TestContext.WriteLine( _fmt, NowTime(), "Park/Unpark", _info, "Tests skipped" );
		}

		private bool[] GetCanMoveAxes()
		{
			bool[] retvals = new bool[] { false, false, false };
			Exception except = null;
			string exceptMsg = "";

			TelescopeAxes[] axes = (TelescopeAxes[])Enum.GetValues( typeof( TelescopeAxes ) );
			
			for ( int i = 0; i < 3; ++i )
			{
				TelescopeAxes axis = axes[i];
				string axisName = _axisNames[i];
				string status = _error;
				string result = "?";

				try
				{
					retvals[i] = Telescope.CanMoveAxis( axis );
					result = retvals[i].ToString();

					status = _ok;
				}
				catch ( Exception xcp )
				{
					except = xcp;
					exceptMsg = GetExceptionMessage( xcp );
				}

				string msg = "";

				if ( status == _error )
				{
					msg = exceptMsg;
				}
				else if ( result != null )
				{
					msg = "CanMoveAxis:" + axisName + " " + result;
				}

				TestContext.WriteLine( _fmt, NowTime(), "CanMoveAxis:" + axisName, status, msg );

				if ( status == _error )
				{
					_failedConformanceCheck = true;
				}
			}

			return retvals;
		}


		#endregion Test Methods 

		#region Helper Methods

		private void WriteAndReportTrackingRates( ITrackingRates supportedRates )
		{
			Type expectedException;
			string fmt = "Successfully set drive rate: {0}";

			DriveRates[] allRates = (DriveRates[])Enum.GetValues( typeof( DriveRates ) );

			foreach ( DriveRates rate in allRates )
			{
				string msg = String.Format( fmt, rate );
				expectedException = null;

				if ( !IsRateSupported( rate, supportedRates ) )
				{
					expectedException = typeof( TargetInvocationException );
					msg = String.Format(
						"Invalid value exception generated as expected on set TrackingRate = {0}",
						rate );
				}

				WriteProperty<DriveRates>( "TrackingRate", rate, "", "Write"
											, msg, expectedException );
			}

			expectedException = typeof( TargetInvocationException );
			WriteProperty<int>( "TrackingRate", 5, "", "Write"
								, "Error generated as expected when TrackingRate is set to an invalid value (5)"
								, expectedException );
			WriteProperty<int>( "TrackingRate", -1, "", "Write"
								, "Error generated as expected when TrackingRate is set to an invalid value (-1)"
								, expectedException );
		}

		private bool IsRateSupported( DriveRates rate, ITrackingRates supportedRates )
		{
			foreach ( DriveRates supportedRate in supportedRates )
			{
				if ( rate == supportedRate )
				{
					return true;
				}
			}

			return false;
		}

		private ITrackingRates ReadAndReportTrackingRates()
		{
			ITrackingRates trackingRates = null;
			string msg;

			trackingRates = ReadProperty<ITrackingRates>( "TrackingRates", "", "", "Drive rates read OK" );

			if ( trackingRates != null )
			{
				foreach ( DriveRates trackingRate in trackingRates )
				{
					msg = String.Format( "Found drive rate: {0}", trackingRate );
					TestContext.WriteLine( _fmt, NowTime(), "TrackingRates", "", msg );
				}
			}

			return trackingRates;
		}

		private T ReadProperty<T>( string propName, string valueFormat="", string nameSuffix=""
									, string customResultMessage = "" )
		{
			T retval = default(T);
			string status = _error;
			string exception = "";
			string result = null;

			try
			{
				PropertyInfo pi = Telescope.GetType().GetProperty( propName );
				T propValue = (T)pi.GetValue( Telescope );

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
									, string nameSuffix="", string customResultFormat=""
									, Type expectedException=null  )
		{
			string status = _error;
			string exception = "";
			string msg = null;
			Exception except = null;

			try
			{
				PropertyInfo pi = Telescope.GetType().GetProperty( propName );
				pi.SetValue( Telescope, propValue );

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
			}
			else if ( except == null && expectedException != null )
			{
				status = _issue;
				msg = String.Format( "The expected error was not generated when {0} was set to {1}.", propName, propValue );
			}

			if ( status == _error )
			{
				msg = exception;
				_failedConformanceCheck = true;
			}
			else if ( status == _ok )
			{
				string resultFormat = !String.IsNullOrEmpty( customResultFormat ) ? customResultFormat : "{0:" + valueFormat + "}";
				msg = String.Format( resultFormat, propValue );
			}

			string suffix = String.IsNullOrEmpty( nameSuffix ) ? "" : " " + nameSuffix;
			TestContext.WriteLine( _fmt, NowTime(), propName+suffix, status, msg );
		}

		private double ReadDegreeProperty( string propName, string nameSuffix = "", Type expectedException=null )
		{
			double retval = Double.NaN;

			string status = _error;
			Exception except = null;
			string exception = "";
			string result = "?";

			try
			{
				PropertyInfo pi = Telescope.GetType().GetProperty( propName );
				double propValue = (double)pi.GetValue( Telescope );

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
			TestContext.WriteLine( _fmt, NowTime(), propName+suffix, status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}

			return retval;
		}

		private double ReadHourProperty( string propName, string nameSuffix="", Type expectedException = null )
		{
			double retval = Double.NaN;

			string status = _error;
			Exception except = null;
			string exception = "";
			string result = "?";

			try
			{
				PropertyInfo pi = Telescope.GetType().GetProperty( propName );
				double propValue = (double)pi.GetValue( Telescope );

				result = Util.HoursToHMS( propValue, ":", ":", "", 2 );
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
				except = null;
				status = _ok;
			}

			string msg = !String.IsNullOrEmpty( exception ) ? exception : result;
			string suffix = String.IsNullOrEmpty( nameSuffix ) ? "" : " " + nameSuffix;
			TestContext.WriteLine( _fmt, NowTime(), propName+suffix, status, msg );

			if ( status == _error )
			{
				_failedConformanceCheck = true;
			}

			return retval;
		}

		private void ReadAndTestSiderealTime()
		{
			double dVal = ReadHourProperty( "SiderealTime" );
			double siderealTime = GetSystemSiderealTime();
			string msg;

			if ( Double.IsNaN( siderealTime ) )
			{
				msg = "Unable to calculate the local apparent sidereal time";
				TestContext.WriteLine( _fmt, NowTime(), "SiderealTime", _error, msg );
				_failedConformanceCheck = true;
			}
			else
			{
				double diff = Math.Abs( dVal - siderealTime );

				if ( diff < 1.0 )
				{
					string scopeHours = Util.HoursToHMS( dVal, ":", ":", "", 2 );
					string systemHours = Util.HoursToHMS( siderealTime, ":", ":", "", 2 );

					msg = String.Format( "Scope and ASCOM sidereal times agree to better than 1 second, "
						+ "Scope: {0}, ASCOM: {1}", scopeHours, systemHours );
					TestContext.WriteLine( _fmt, NowTime(), "SiderealTime", _ok, msg );
				}
			}
		}

		private double GetSystemSiderealTime()
		{
			double nowJulian = Util.DateUTCToJulian( DateTime.UtcNow );
			double jdHigh = Math.Truncate( nowJulian );
			double jdLow = nowJulian - jdHigh;

			double gast = 0.0;

			// Use NOVAS 3.1 to get the GreenwichApparentSiderealTime (GAST).

			using ( var novas = new NOVAS31() )
			{
				novas.SiderealTime( jdHigh, jdLow, novas.DeltaT( nowJulian ),
					GstType.GreenwichApparentSiderealTime,
					Method.EquinoxBased,
					Accuracy.Full, ref gast );
			}

			// Adjust for the longitude.

			double last = gast + Telescope.SiteLongitude / 360.0 * 24.0;

			// Normalize the time to the range 0 to 24 hours.

			last += 24.0; // Make sure it is not negative.
			last = last % 24.0;

			return last;
		}

		private static string GetExceptionMessage ( Exception xcp )
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

		private static string NowTime()
		{
			return DateTime.Now.ToString( "hh:mm:ss.fff" );
		}

		#endregion Helper Methods
	}
}
