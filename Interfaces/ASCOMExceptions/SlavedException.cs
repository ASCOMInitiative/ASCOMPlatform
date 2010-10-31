using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace ASCOM
{
	/// <summary>
	/// This exception should be used to indicate that movement (or other invalid operation) was attempted
	/// while the device was in slaved mode. This applies primarily to domes drivers.
	/// </summary>
    [Serializable]
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

        /// <summary>
        /// Added to keep Code Analysis happy
        /// </summary>
        /// <param name="message"></param>
        public SlavedException(string message)
            : base(message, ErrorCodes.InvalidWhileSlaved)
        {
        }

        /// <summary>
        /// Added to keep Code Analysis happy
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public SlavedException(string message, Exception inner)
            : base(message, ErrorCodes.InvalidWhileSlaved, inner)
        {
        }

        /// <summary>
        /// Added to keep Code Analysis happy
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected SlavedException(SerializationInfo info, 
         StreamingContext context) : base(info, context)
        {
        }

	}
}
