using System.Windows;
using System.Windows.Media.Media3D;

using ASCOM.DeviceHub;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unit_Tests.Dome
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

			DomeSynchronize ds = new DomeSynchronize( _layout, _siteLatitude );
			Point position = ds.DomePosition( scopePosition, hourAngle, isPierWest );

			Assert.AreEqual( 318.26, position.X, 0.5 );
			Assert.AreEqual( 7.40, position.Y, 0.5 );
		}

		[TestMethod]
		public void TestSyncSouthwest()
		{
			Point scopePosition = new Point( 225.12, 5.87 );
			double hourAngle = 7.83812 * 15.0; // in degrees
			bool isPierWest = false;

			DomeSynchronize ds = new DomeSynchronize( _layout, _siteLatitude );
			Point position = ds.DomePosition( scopePosition, hourAngle, isPierWest );

			Assert.AreEqual( 226.46, position.X, 0.5 );
			Assert.AreEqual( 10.61, position.Y, 0.5 );
		}

		[TestMethod]
		public void TestSyncNortheast()
		{
			Point scopePosition = new Point( 47.13, 6.6 );
			double hourAngle = 7.83812 * 15.0; // in degrees
			bool isPierWest = true;

			DomeSynchronize ds = new DomeSynchronize( _layout, _siteLatitude );
			Point position = ds.DomePosition( scopePosition, hourAngle, isPierWest );

			Assert.AreEqual( 45.50, position.X, 0.5 );
			Assert.AreEqual( 0.91, position.Y, 0.5 );
		}

		[TestMethod]
		public void TestSyncSoutheast()
		{
			Point scopePosition = new Point( 137.1, 5.4 );
			double hourAngle = 7.83812 * 15.0; // in degrees
			bool isPierWest = false;

			DomeSynchronize ds = new DomeSynchronize( _layout, _siteLatitude );
			Point position = ds.DomePosition( scopePosition, hourAngle, isPierWest );

			Assert.AreEqual( 136.85, position.X, 0.5 );
			Assert.AreEqual( 10.0, position.Y, 0.5 );
		}
	}
}
