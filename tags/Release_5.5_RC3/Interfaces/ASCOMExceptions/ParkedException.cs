using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM
{
	/// <summary>
	/// This exception should be used to indicate that movement (or other invalid operation) was attempted
	/// while the device was in a parked state.
	/// </summary>
	public class ParkedException : DriverException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ParkedException"/> class
		/// using default error text and error codes.
		/// </summary>
		public ParkedException()
			: base("Operation not valid while the device is parked", ErrorCodes.InvalidWhileParked)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="ParkedException"/> class
		/// with a caught (inner) exception.
		/// </summary>
		/// <param name="inner">The inner.</param>
		public ParkedException(Exception inner)
			: base("Operation not valid while the device is parked", ErrorCodes.InvalidWhileParked, inner)
		{
		}
	}
}
