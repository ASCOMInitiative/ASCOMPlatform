using System.Windows;
using System.Windows.Media.Media3D;

using ASCOM.DeviceHub;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unit_Tests.Dome
{
	[TestClass]
	public class DomeControlTests
	{
		[TestClass]
		public class DomeSynchronizationTests
		{
			private const double _siteLatitude = 31.484;
			private DomeLayoutSettings _layout = new DomeLayoutSettings
			{
				DomeScopeOffset = new Point3D( 101.0, 190.0, 0.0 ),
				DomeRadius = 2740,
				AzimuthAccuracy = 2,
				GemAxisOffset = 300,
				SlaveInterval = 30
			};

			private DomeManager _mgr;

			[TestInitialize]
			public void TestInit()
			{
				_mgr = DomeManager.Instance;
			}

			[TestCleanup]
			public void TestCleanup()
			{
				_mgr.Dispose();
			}

			[TestMethod]
			public void TestSyncNorthwest()
			{
				Point scopePosition = new Point( 318.0, 2.6 );
				double hourAngle = 7.83812 * 15.0; // in degrees
				bool isPierWest = false;

				DomeControl dc = new DomeControl( _layout, _siteLatitude );
				Point position = dc.DomePosition( scopePosition, hourAngle, isPierWest );

				Assert.AreEqual( 315.0, position.X, 0.5 );
				Assert.AreEqual( -2.33, position.Y, 0.5 );
			}

			[TestMethod]
			public void TestSyncSouthwest()
			{
				Point scopePosition = new Point( 225.12, 5.87 );
				double hourAngle = 7.83812 * 15.0; // in degrees
				bool isPierWest = false;

				DomeControl dc = new DomeControl( _layout, _siteLatitude );
				Point position = dc.DomePosition( scopePosition, hourAngle, isPierWest );

				Assert.AreEqual( 229.42, position.X, 0.5 );
				Assert.AreEqual( .84, position.Y, 0.5 );
			}

			[TestMethod]
			public void TestSyncNortheast()
			{
				Point scopePosition = new Point( 47.13, 6.6 );
				double hourAngle = 7.83812 * 15.0; // in degrees
				bool isPierWest = true;

				DomeControl dc = new DomeControl( _layout, _siteLatitude );
				Point position = dc.DomePosition( scopePosition, hourAngle, isPierWest );

				Assert.AreEqual( 42.54, position.X, 0.5 );
				Assert.AreEqual( 10.69, position.Y, 0.5 );
			}

			[TestMethod]
			public void TestSyncSoutheast()
			{
				Point scopePosition = new Point( 137.1, 5.4 );
				double hourAngle = 7.83812 * 15.0; // in degrees
				bool isPierWest = false;

				DomeControl dc = new DomeControl( _layout, _siteLatitude );
				Point position = dc.DomePosition( scopePosition, hourAngle, isPierWest );

				Assert.AreEqual( 140.05, position.X, 0.5 );
				Assert.AreEqual( 1.06, position.Y, 0.5 );
			}
		}
	}
}
