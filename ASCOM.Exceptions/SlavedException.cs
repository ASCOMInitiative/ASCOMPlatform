using System;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace ASCOM
{
    /// <summary>
    /// This exception should be used to indicate that movement (or other invalid operation) was attempted while the device was in slaved mode. This applies primarily to domes drivers.
    /// </summary>
    /// <remarks>
    /// <para>If you need to throw this error as a COM exception use the error number: 0x80040409.</para>
    /// </remarks>
    [Serializable]
    [ComVisible(true)]
    [Guid("537BF13D-55E0-4C80-98EB-BE270E653E10")]
    public class SlavedException : DriverException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "SlavedException" /> class.
        /// </summary>
        public SlavedException() : base("Operation not valid while the device is in slave mode.", ErrorCodes.InvalidWhileSlaved)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "SlavedException" /> class
        /// with a caught (inner) exception.
        /// </summary>
        /// <param name = "inner">Inner exception</param>
        public SlavedException(Exception inner) : base("Operation not valid while the device is in slave mode.", ErrorCodes.InvalidWhileSlaved, inner)
        {
        }

        /// <summary>
        /// Create a new exception
        /// </summary>
        /// <param name = "message">Exception description</param>
        public SlavedException(string message) : base(message, ErrorCodes.InvalidWhileSlaved)
        {
        }

        /// <summary>
        /// Create a new exception
        /// </summary>
        /// <param name = "message">Exception description</param>
        /// <param name = "inner">Underlying exception that caused this exception to be thrown.</param>
        public SlavedException(string message, Exception inner) : base(message, ErrorCodes.InvalidWhileSlaved, inner)
        {
        }

        /// <summary>
        /// Create a new exception
        /// </summary>
        /// <param name = "info">Information required to serialise the exception</param>
        /// <param name = "context">Information of the serialising stream context.</param>
        protected SlavedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}