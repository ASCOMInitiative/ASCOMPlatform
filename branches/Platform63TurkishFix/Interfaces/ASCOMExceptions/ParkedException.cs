using System;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace ASCOM
{
    /// <summary>
    ///   This exception should be used to indicate that movement (or other invalid operation) was attempted
    ///   while the device was in a parked state.
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    [Guid("89EA7E2A-7C74-461C-ABD5-75EE3D46DA13")]
    public class ParkedException : DriverException
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ParkedException" /> class
        ///   using default error text and error codes.
        /// </summary>
        public ParkedException()
            : base("Operation not valid while the device is parked", ErrorCodes.InvalidWhileParked)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ParkedException" /> class
        ///   with a caught (inner) exception.
        /// </summary>
        /// <param name = "inner">The inner.</param>
        public ParkedException(Exception inner)
            : base("Operation not valid while the device is parked", ErrorCodes.InvalidWhileParked, inner)
        {
        }

        /// <summary>
        ///   Create a new exception
        /// </summary>
        /// <param name = "message">Exception description</param>
        public ParkedException(string message)
            : base(message, ErrorCodes.InvalidWhileParked)
        {
        }

        /// <summary>
        ///   Create a new exception
        /// </summary>
        /// <param name = "message">Exception description</param>
        /// <param name = "inner">Underlying exception that caused this exception to be thrown.</param>
        public ParkedException(string message, Exception inner)
            : base(message, ErrorCodes.InvalidWhileParked, inner)
        {
        }

        /// <summary>
        ///   Added to keep Code Analysis happy
        /// </summary>
        /// <param name = "info"></param>
        /// <param name = "context"></param>
        protected ParkedException(SerializationInfo info,
                                  StreamingContext context) : base(info, context)
        {
        }
    }
}