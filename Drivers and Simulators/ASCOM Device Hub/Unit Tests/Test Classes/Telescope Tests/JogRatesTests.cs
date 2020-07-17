using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ASCOM.DeviceInterface;
using ASCOM.DeviceHub;


namespace Unit_Tests.Telescope
{
	[TestClass]
	public class JogRatesTests
	{
		private readonly double _siderealRate = 0.004178;

		[TestMethod]
		public void DefaultJogRatesTest()
		{
			JogRates defaults = JogRates.DefaultJogRates();

			Assert.AreEqual<int>( 3, defaults.Count );
			Assert.AreEqual( 16 * _siderealRate, defaults[0].Rate, 0.00001 );			
			Assert.AreEqual( 64 * _siderealRate, defaults[1].Rate, 0.00001 );
			Assert.AreEqual( 2.0, defaults[2].Rate );
		}

		[TestMethod]
		public void FromAxisRatesTest()
		{
			// Test that the default rates are used when they are in the min-max range.

			JogRates expected = JogRates.DefaultJogRates();

			List<AxisRate> rateList = new List<AxisRate> { new AxisRate( 0.0, 5.0 ) };
			IRate[] axisRates = rateList.ToArray<IRate>();

			JogRates jogRates = JogRates.FromAxisRates( axisRates );
			Assert.AreEqual<int>( expected.Count, jogRates.Count );

			for ( int n = 0; n < expected.Count; ++n )
			{
				Assert.AreEqual( expected[n].Rate, jogRates[n].Rate );
			}

			// Test multiple ranges like the simulator. 
			// Jog rates should be in the middle of each range.

			rateList = new List<AxisRate> {  new AxisRate( 0.0, 1.67 )
											, new AxisRate( 2.5, 5.0 )};
			axisRates = rateList.ToArray<IRate>();

			jogRates = JogRates.FromAxisRates( axisRates );

			Assert.AreEqual<int>( 2, jogRates.Count );
			Assert.AreEqual<double>( 0.835, jogRates[0].Rate );
			Assert.AreEqual<double>( 3.75, jogRates[1].Rate );

			// Test a set of discrete axis rates (where the min and max for each rate are the same).

			rateList = new List<AxisRate> {  new AxisRate( 0.25, 0.25 )
											, new AxisRate( 0.75, 0.75 )
											, new AxisRate( 1.5, 1.5 )
											, new AxisRate( 3.0, 3.0 ) };
			axisRates = rateList.ToArray<IRate>();

			jogRates = JogRates.FromAxisRates( axisRates );

			Assert.AreEqual<int>( 4, jogRates.Count );
			Assert.AreEqual<double>( 0.25, jogRates[0].Rate );
			Assert.AreEqual<double>( 0.75, jogRates[1].Rate );
			Assert.AreEqual<double>( 1.5, jogRates[2].Rate );
			Assert.AreEqual<double>( 3.0, jogRates[3].Rate );
		}
	}
}