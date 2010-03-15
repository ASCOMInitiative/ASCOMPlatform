using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ASCOM.Internal
{
	public static class IntExtensions
	{
		/// <summary>
		/// Defines a lookup table of bit masks, for a fast method of determining
		/// a mask for any given bit position.
		/// N.B. It might just be quicker to raise to a power of two,
		/// the compiler might be smart enough to optimize that.
		/// </summary>
		private static readonly uint[] bitMask =
		{
			0x00000001,0x00000002,0x00000004,0x00000008,0x00000010,0x00000020,0x00000040,0x00000080,
			0x00000100,0x00000200,0x00000400,0x00000800,0x00001000,0x00002000,0x00004000,0x00008000,
			0x00010000,0x00020000,0x00040000,0x00080000,0x00100000,0x00200000,0x00400000,0x00800000,
			0x01000000,0x02000000,0x04000000,0x08000000,0x10000000,0x20000000,0x40000000,0x80000000,
		};

		/// <summary>
		/// Returns a boolean value corresponding to the value at the specified bit position.
		/// </summary>
		/// <param name="register">The register, an unsigned integer, containing bit values.</param>
		/// <param name="bitPosition">The bit position to be tested, where bit 0 is the least significant bit.</param>
		/// <returns>A boolean value corresponding to the bit at the specified bit position.</returns>
		public static bool Bit(this uint register, int bitPosition)
		{
			if (bitPosition < 0 || bitPosition > 31)
			{
				throw new ArgumentOutOfRangeException("bitPosition", "Valid bit positions are 0..31");
			}
			uint mask = bitMask[bitPosition];
			uint result = register & mask;
			return (result > 0 ? true : false);
		}

		/// <summary>
		/// Returns a boolean value corresponding to the value at the specified bit position.
		/// </summary>
		/// <param name="register">The register, an integer, containing bit values.</param>
		/// <param name="bitPosition">The bit position to be tested, where bit 0 is the least significant bit.</param>
		/// <returns>A boolean value corresponding to the bit at the specified bit position.</returns>
		public static bool Bit(this int register, int bitPosition)
		{
			return ((uint)register).Bit(bitPosition);
		}
	}
}
