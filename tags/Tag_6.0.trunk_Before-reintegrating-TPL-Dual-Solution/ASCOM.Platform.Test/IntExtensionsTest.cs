using ASCOM.Internal;
using Xunit;

namespace ASCOM.Platform.Test
{
    
    
    /// <summary>
    ///This is a test class for IntExtensionsTest and is intended
    ///to contain all IntExtensionsTest Unit Tests
    ///</summary>
	public class IntExtensionsTest
	{
		[Fact]
		public void IntBit_LsbSet_BitPosition0_ShouldBeTrue()
		{
			uint register = 0x00000001;
			int bitPosition = 0;
			bool expected = true;
			bool actual;
			actual = IntExtensions.Bit(register, bitPosition);
			Assert.Equal(expected, actual);
		}
		[Fact]
		public void IntBit_LsbSet_BitPosition1_ShouldBeFalse()
		{
			uint register = 0x00000001;
			int bitPosition = 1;
			bool expected = false;
			bool actual;
			actual = IntExtensions.Bit(register, bitPosition);
			Assert.Equal(expected, actual);
		}
		[Fact]
		public void IntBit_LsbSet_BitPosition31_ShouldBeFalse()
		{
			uint register = 0x00000001;
			int bitPosition = 31;
			bool expected = false;
			bool actual;
			actual = IntExtensions.Bit(register, bitPosition);
			Assert.Equal(expected, actual);
		}
		[Fact]
		public void IntBit_LsbSet_BitPosition32_ShouldThrow_ArgumentOutOfRange()
		{
			uint register = 0x00000001;
			int bitPosition = 32;
			Assert.Throws<System.ArgumentOutOfRangeException>(
				delegate { IntExtensions.Bit(register, bitPosition); }
				);
		}
		[Fact]
		public void IntBit_MsbSet_BitPosition0_ShouldBeFalse()
		{
			uint register = 0x80000000U;
			int bitPosition = 0;
			bool expected = false;
			bool actual;
			actual = IntExtensions.Bit(register, bitPosition);
			Assert.Equal(expected, actual);
		}
		[Fact]
		public void IntBit_MsbSet_BitPosition30_ShouldBeFalse()
		{
			uint register = 0x80000000U;
			int bitPosition = 30;
			bool expected = false;
			bool actual;
			actual = IntExtensions.Bit(register, bitPosition);
			Assert.Equal(expected, actual);
		}
		[Fact]
		public void IntBit_MsbSet_BitPosition31_ShouldBeTrue()
		{
			uint register = 0x80000000U;
			int bitPosition = 31;
			bool expected = true;
			bool actual;
			actual = IntExtensions.Bit(register, bitPosition);
			Assert.Equal(expected, actual);
		}
	}
}
