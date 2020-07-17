using System;
using ASCOM.DeviceHub;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unit_Tests.Telescope
{
	[TestClass]
	public class AxisConvertersTests
	{
		private decimal _testValue = 65.4321m;
		private int _testDegrees = 65;
		private int _testMinutes = 25;
		private readonly int _testSeconds = 56;
		private string _testString = " 065° 25' 56\"";

		private decimal _testAzimuthValue = 265.4321m;
		private int _testAzimuthDegrees = 265;
		private string _testAzimuthString = " 265° 25' 56\"";

		private decimal _testRaValue = 12.3456m;
		private int _testRaHours = 12;
		private int _testRaMinutes = 20;
		private int _testRaSeconds = 44;
		private string _testRaString = "12:20:44";

		private decimal _testValueRoundUp = 16.6499917m;
		private int _testDegreesRoundUp = 16;
		private int _testMinutesRoundUp = 39;
		private int _testSecondsRoundUp = 0;
		private string _testStringRoundUp = " 016° 39' 00\"";

		[TestMethod]
		public void AltitudeGoodConversions()
		{
			AltitudeConverter cvt;
			cvt = new AltitudeConverter( _testValue );
			Assert.AreEqual( _testDegrees, cvt.Degrees );
			Assert.AreEqual( _testMinutes, cvt.Minutes );
			Assert.AreEqual( _testSeconds, cvt.Seconds );
			string result = cvt.ToString();
			Assert.AreEqual( _testString, result );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void AltitudeRangeHighConversion()
		{
			AltitudeConverter cvt = new AltitudeConverter( 90.1m );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void AltitudeRangeLowConversion()
		{
			AltitudeConverter cvt = new AltitudeConverter( _testValue * -2m );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void AltitudeRangeHighDegrees()
		{
			int temp = _testDegrees + 70;

			AltitudeConverter cvt = new AltitudeConverter( temp, _testMinutes, _testSeconds );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void AltitudeRangeHighMinutes()
		{
			int temp = _testMinutes + 70;

			AltitudeConverter cvt = new AltitudeConverter( _testDegrees, temp, _testSeconds );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void AltitudeRangeHighSeconds()
		{
			int temp = _testSeconds + 70;

			AltitudeConverter cvt = new AltitudeConverter( _testDegrees, _testMinutes, temp );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void AltitudeRangeLowDegrees()
		{
			int temp = ( _testDegrees + 70 ) * -1;

			AltitudeConverter cvt = new AltitudeConverter( temp, _testMinutes, _testSeconds );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void AltitudeRangeLowMinutes()
		{
			int temp = _testMinutes - 70;

			AltitudeConverter cvt = new AltitudeConverter( _testDegrees, temp, _testSeconds );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void AltitudeRangeLowSeconds()
		{
			int temp = _testSeconds - 70;

			AltitudeConverter cvt = new AltitudeConverter( _testDegrees, _testMinutes, temp );
		}

		[TestMethod]
		public void DeclinationGoodConversions()
		{
			DeclinationConverter cvt;
			cvt = new DeclinationConverter( _testValue );
			Assert.AreEqual( _testDegrees, cvt.Degrees );
			Assert.AreEqual( _testMinutes, cvt.Minutes );
			Assert.AreEqual( _testSeconds, cvt.Seconds );
			string result = cvt.ToString();
			Assert.AreEqual( _testString, result );

			cvt = new DeclinationConverter( _testValue * -1m );
			Assert.AreEqual( _testDegrees * -1, cvt.Degrees );
			Assert.AreEqual( 25, cvt.Minutes );
			Assert.AreEqual( 56m, cvt.Seconds );

			cvt = new DeclinationConverter( _testValueRoundUp );
			Assert.AreEqual( _testDegreesRoundUp, cvt.Degrees );
			Assert.AreEqual( _testMinutesRoundUp, cvt.Minutes );
			Assert.AreEqual( _testSecondsRoundUp, cvt.Seconds );
			result = cvt.ToString();
			Assert.AreEqual( _testStringRoundUp, result );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void DeclinationRangeHighConversion()
		{
			DeclinationConverter cvt = new DeclinationConverter( 90.1m );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void DeclinationRangeLowConversion()
		{
			DeclinationConverter cvt = new DeclinationConverter( -90.1m );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void DeclinationRangeHighDegrees()
		{
			int temp = _testDegrees + 70;

			DeclinationConverter cvt = new DeclinationConverter( temp, _testMinutes, _testSeconds );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void DeclinationRangeHighMinutes()
		{
			int temp = _testMinutes + 70;

			DeclinationConverter cvt = new DeclinationConverter( _testDegrees, temp, _testSeconds );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void DeclinationRangeHighSeconds()
		{
			int temp = _testSeconds + 70;

			DeclinationConverter cvt = new DeclinationConverter( _testDegrees, _testMinutes, temp );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void DeclinationRangeLowDegrees()
		{
			int temp = ( _testDegrees + 70 ) * -1;

			DeclinationConverter cvt = new DeclinationConverter( temp, _testMinutes, _testSeconds );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void DeclinationRangeLowMinutes()
		{
			int temp = _testMinutes - 70;

			DeclinationConverter cvt = new DeclinationConverter( _testDegrees, temp, _testSeconds );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void DeclinationRangeLowSeconds()
		{
			int temp = _testSeconds - 70;

			DeclinationConverter cvt = new DeclinationConverter( _testDegrees, _testMinutes, temp );
		}

		[TestMethod]
		public void AzimuthGoodConversions()
		{
			AzimuthConverter cvt;
			cvt = new AzimuthConverter( _testAzimuthValue );
			Assert.AreEqual( _testAzimuthDegrees, cvt.Degrees );
			Assert.AreEqual( _testMinutes, cvt.Minutes );
			Assert.AreEqual( _testSeconds, cvt.Seconds );
			string result = cvt.ToString();
			Assert.AreEqual( _testAzimuthString, result );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void AzimuthRangeHighConversion()
		{
			AzimuthConverter cvt = new AzimuthConverter( 360.1m );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void AzimuthRangeLowConversion()
		{
			AzimuthConverter cvt = new AzimuthConverter( -0.1m );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void AzimuthRangeHighDegrees()
		{
			int temp = _testAzimuthDegrees + 100 ;

			AzimuthConverter cvt = new AzimuthConverter( temp, _testMinutes, _testSeconds );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void AzimuthRangeHighMinutes()
		{
			int temp = _testMinutes + 70;

			AzimuthConverter cvt = new AzimuthConverter( _testDegrees, temp, _testSeconds );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void AzimuthRangeHighSeconds()
		{
			int temp = _testSeconds + 70;

			AzimuthConverter cvt = new AzimuthConverter( _testDegrees, _testMinutes, temp );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void AzimuthRangeLowDegrees()
		{
			int temp = _testAzimuthDegrees * -1;

			AzimuthConverter cvt = new AzimuthConverter( temp, _testMinutes, _testSeconds );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void AzimuthRangeLowMinutes()
		{
			int temp = _testMinutes - 70;

			AzimuthConverter cvt = new AzimuthConverter( _testDegrees, temp, _testSeconds );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void AzimuthRangeLowSeconds()
		{
			int temp = _testSeconds - 70;

			AzimuthConverter cvt = new AzimuthConverter( _testDegrees, _testMinutes, temp );
		}

		[TestMethod]
		public void RaGoodConversions()
		{
			RightAscensionConverter cvt;
			cvt = new RightAscensionConverter( _testRaValue );
			Assert.AreEqual( _testRaHours, cvt.Hours );
			Assert.AreEqual( _testRaMinutes, cvt.Minutes );
			Assert.AreEqual( _testRaSeconds, cvt.Seconds );
			string result = cvt.ToString();
			Assert.AreEqual( _testRaString, result );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void RaRangeHighConversion()
		{
			RightAscensionConverter cvt = new RightAscensionConverter( 24.0m );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void RaRangeLowConversion()
		{
			RightAscensionConverter cvt = new RightAscensionConverter( -0.1m );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void RaRangeHighDegrees()
		{
			int temp = _testRaHours + 20;

			RightAscensionConverter cvt = new RightAscensionConverter( temp, _testMinutes, _testSeconds );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void RaRangeHighMinutes()
		{
			int temp = _testMinutes + 70;

			RightAscensionConverter cvt = new RightAscensionConverter( _testRaHours, temp, _testSeconds );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void RaRangeHighSeconds()
		{
			int temp = _testSeconds + 70;

			RightAscensionConverter cvt = new RightAscensionConverter( _testRaHours, _testMinutes, temp );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void RaRangeLowDegrees()
		{
			int temp = _testRaHours * -1;

			RightAscensionConverter cvt = new RightAscensionConverter( temp, _testMinutes, _testSeconds );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void RaRangeLowMinutes()
		{
			int temp = _testMinutes - 70;

			RightAscensionConverter cvt = new RightAscensionConverter( _testRaHours, temp, _testSeconds );
		}

		[TestMethod]
		[ExpectedException( typeof( ASCOM.InvalidValueException ) )]
		public void RaRangeLowSeconds()
		{
			int temp = _testSeconds - 70;

			RightAscensionConverter cvt = new RightAscensionConverter( _testRaHours, _testMinutes, temp );
		}
	}
}
