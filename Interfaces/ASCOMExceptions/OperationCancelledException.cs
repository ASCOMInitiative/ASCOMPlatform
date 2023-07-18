using System;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace ASCOM
{
    /// <summary>
    /// This exception should be used to indicate that an in-progress operation has been cancelled.
    /// </summary>
    /// <remarks>
    /// <para>If you need to throw this error as a COM exception use the error number: 0x80040408.</para>
    /// </remarks>
    [Serializable]
    [ComVisible(true)]
    [Guid("29E601B9-8197-41F1-812D-5956D7FACACC")]
    public class OperationCancelledException : DriverException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "ParkedException" /> class
        /// using default error text and error codes.
        /// </summary>
        public OperationCancelledException() : base("Operation not valid while the device is parked", ErrorCodes.InvalidWhileParked)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ParkedException" /> class
        /// with a caught (inner) exception.
        /// </summary>
        /// <param name = "inner">The inner.</param>
        public OperationCancelledException(Exception inner) : base("Operation not valid while the device is parked", ErrorCodes.InvalidWhileParked, inner)
        {
        }

        /// <summary>
        /// Create a new exception
        /// </summary>
        /// <param name = "message">Exception description</param>
        public OperationCancelledException(string message) : base(message, ErrorCodes.InvalidWhileParked)
        {
        }

        /// <summary>
        /// Create a new exception
        /// </summary>
        /// <param name = "message">Exception description</param>
        /// <param name = "inner">Underlying exception that caused this exception to be thrown.</param>
        public OperationCancelledException(string message, Exception inner) : base(message, ErrorCodes.InvalidWhileParked, inner)
        {
        }

        /// <summary>
        /// Added to keep Code Analysis happy
        /// </summary>
        /// <param name = "info">Serialisation information</param>
        /// <param name = "context">Streaming context.</param>
        protected OperationCancelledException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}