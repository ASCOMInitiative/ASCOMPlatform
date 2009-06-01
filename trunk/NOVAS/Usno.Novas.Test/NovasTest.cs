using Usno;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TiGra.Astronomy;

namespace Usno.Test
{
    
    
    /// <summary>
    ///This is a test class for NovasTest and is intended
    ///to contain all NovasTest Unit Tests
    ///</summary>
	[TestClass()]
	public class NovasTest
		{
		/// <summary>
		/// Required accuracy for floating point equality tests.
		/// </summary>
		const double delta = 0.000001;
		/// <summary>
		/// The tolerance threshold for <see cref="DateTime"/> objects,
		/// specified in 100 nanosecond ticks.
		/// </summary>
		const long deltaDateTime = 100000;
		/// <summary>
		/// Microsoft didn;t think to document this, unfortunately.
		/// So it's probably needed, for something.
		/// </summary>
		private TestContext testContextInstance;
		/// <summary>
		/// Location of Brynllefrith Observatory, IAU #J58
		/// </summary>
		private static GeographicCoordinates BrynllefrithCoordinates = new GeographicCoordinates(51.55, -3.3);
		/// <summary>
		/// Observatory site information for Brynllefrith Observatory, IAU #J58
		/// </summary>
		private static Observatory BrynllefrithObservatory = new Observatory() { Location = BrynllefrithCoordinates, Height = 99, Pressure = 1000, Temperature = 12 };
		/// <summary>
		/// A theoretical standard observing site at sea level, on the equator and at the prim meridian,
		/// with 'standard' Temperature and Pressure.
		/// </summary>
		private Observatory StandardObservatory = Observatory.Default;


		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
			{
			get
				{
				return testContextInstance;
				}
			set
				{
				testContextInstance = value;
				}
			}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion



		/// <summary>
		/// A test for Novas Constructor
		/// </summary>
		[TestMethod()]
		public void NovasConstructorTest()
			{
			Novas target = new Novas();
			}

		/// <summary>
		/// Validation data from http://ssd.jpl.nasa.gov/tc.cgi#top
		/// Input Time Zone: UT
		/// -------------------------------------------------------
		/// A.D. 2009-May-19 01:21:12.01 = A.D. 2009-May-19.0563889
		/// A.D.  2009-05-19 01:21:12.01 = A.D.  2009-05-19.0563889
		/// A.D.   2009--139 01:21:12.01 = A.D.   2009--139.0563889
		/// Day-of-Week: Tuesday
		/// Julian Date: 2454970.5563889 UT
		///</summary>
		[TestMethod()]
		public void ConvertJulianDateToCalendarDate()
			{
			double tjd = 2454970.5563889;
			DateTime expected = new DateTime(2009, 5, 19, 01, 21, 12, 10, DateTimeKind.Utc);
			DateTime actual;
			actual = Novas.cal_date(tjd);
			Assert.IsTrue(actual.IsWithin(expected, deltaDateTime));
			}
		/// <summary>
		/// Validation data from http://ssd.jpl.nasa.gov/tc.cgi#top
		/// Input Time Zone: UT
		/// -------------------------------------------------------
		/// A.D. 2009-May-19 01:21:12.01 = A.D. 2009-May-19.0563889
		/// A.D.  2009-05-19 01:21:12.01 = A.D.  2009-05-19.0563889
		/// A.D.   2009--139 01:21:12.01 = A.D.   2009--139.0563889
		/// Day-of-Week: Tuesday
		/// Julian Date: 2454970.5563889 UT
		///</summary>
		[TestMethod()]
		public void ConvertCalendarDateToJulianDate()
			{
			DateTime testDate = new DateTime(2009, 5, 19, 01, 21, 12, 10, DateTimeKind.Utc);
			double expected = 2454970.5563889;
			double actual = Novas.julian_date(testDate);
			Assert.AreEqual(expected, actual, delta);
			}

		[TestMethod()]
		public void CalendarDateShouldSurviveJulianRoundtrip()
			{
			DateTime expected = DateTime.UtcNow;
			double julian = Novas.julian_date(expected);
			DateTime actual = Novas.cal_date(julian);
			Assert.IsTrue(actual.IsWithin(expected, deltaDateTime));
			}

		[TestMethod]
		public void JulianDateShouldSurviveCalendarRoundtrip()
			{
			double expected = 2454970.5563889; // A.D. 2009-May-19 01:21:12.01
			DateTime dt = Novas.cal_date(expected);
			double actual = Novas.julian_date(dt);
			Assert.AreEqual(actual, expected, delta);
			}

		/// <summary>
		/// There should be no refraction at the zenith.
		///</summary>
		[TestMethod()]
		[DeploymentItem("Usno.Novas.dll")]
		public void RefractionAtZenithShouldBeZero()
			{
			double zd_obs = 0F;
			double expected = 0F;
			double actual = Novas.refract(zd_obs, BrynllefrithObservatory);
			Assert.AreEqual(expected, actual);
			}
		/// <summary>
		/// There should be no refraction above 91.0 degrees.
		///</summary>
		[TestMethod()]
		[DeploymentItem("Usno.Novas.dll")]
		public void RefractionAbove91DegreesShouldBeZero()
			{
			double zd_obs = 91F + delta;
			double expected = 0F;
			double actual = Novas.refract(zd_obs, BrynllefrithObservatory);
			Assert.AreEqual(expected, actual);
			}
		/// <summary>
		/// Tests that refractions are non-zero at +/-91 degrees and 0 +/- delta.
		/// </summary>
		[TestMethod]
		[DeploymentItem("Usno.Novas.dll")]
		public void RefractionBetweenZenithAnd91DegreesShouldBeNonZero()
			{
			double zd_obs = 91F;
			double fail = 0F;
			double actual;
			actual = Novas.refract(zd_obs, BrynllefrithObservatory);
			Assert.AreNotEqual(fail, actual, "Should be nonzero at +91 degrees");
			//zenithDistanceObserved = -91F;
			//actual = Novas.refract(location, ref_option, zenithDistanceObserved);
			//Assert.AreNotEqual(fail, actual, "Should be nonzero at -91 degrees");
			zd_obs = 0.1;
			actual = Novas.refract(zd_obs, BrynllefrithObservatory);
			Assert.AreNotEqual(fail, actual, "Should be nonzero at plus delta");
			//zenithDistanceObserved = 0 - delta;
			//actual = Novas.refract(location, ref_option, zenithDistanceObserved);
			//Assert.AreNotEqual(fail, actual, "Should be nonzero at minus delta");
			}
		}
}
