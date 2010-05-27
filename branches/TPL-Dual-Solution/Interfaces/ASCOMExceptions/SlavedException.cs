using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM
{
	/// <summary>
	/// This exception should be used to indicate that movement (or other invalid operation) was attempted
	/// while the device was in slaved mode. This applies primarily to domes drivers.
	/// </summary>
	public class SlavedException : DriverException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SlavedException"/> class.
		/// </summary>
		public SlavedException()
			: base("Operation not valid while the device is in slave mode.", ErrorCodes.InvalidWhileSlaved)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="SlavedException"/> class
		/// with a caught (inner) exception.
		/// </summary>
        /// <param name="inner">Inner exception</param>
		public SlavedException(Exception inner)
			: base("Operation not valid while the device is in slave mode.", ErrorCodes.InvalidWhileSlaved, inner)
		{
		}
	}
}
