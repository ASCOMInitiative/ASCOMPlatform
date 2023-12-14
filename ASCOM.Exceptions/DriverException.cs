using System;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace ASCOM
{
    /// <summary>
    ///   This is the generic driver exception. Drivers are permitted to directly throw this
    ///   exception as well as any derived exceptions. Note that the Message property is 
    ///   a member of <see cref = "Exception" />, the base class of DriverException. The <see cref = "Exception.HResult" />
    ///   property of <see cref = "Exception" /> is simply renamed to Number.
    ///   <para>This exception should only be thrown if there is no other more appropriate exception already defined, e.g. PropertyNotImplemented,
    ///     InvalidOperationException, InvalidValueException, NotConnectedException etc. These specific exceptions should be thrown where appropriate
    ///     rather than using the more generic DriverException. Conform will not accept DriverExceptions where more appropriate exceptions 
    ///     are already defined.</para>
    ///   <para>As good programming practice, the Message property should not be empty, so that users understand why the exception was thrown.</para>
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    [Guid("B6EE3D18-CF56-42D3-AED5-B97ABF36B4EE")]
    public class DriverException : Exception
    {
        /// <summary>
        /// Create a new ASCOM exception using the specified text message and error code.
        /// </summary>
        /// <param name = "message">Descriptive text describing the cause of the exception</param>
        /// <param name = "number">Error code for the exception (80040400 - 80040FFF).</param>
        public DriverException(string message, int number)
            : base(message)
        {
            HResult = number;
        }

        /// <summary>
        /// Create a new ASCOM exception based on another exception plus additional descriptive text and error code. This member is 
        /// required for a well-behaved exception class. For example, if a driver receives an exception
        /// (perhaps a COMException) from some other component yet it wants to report some meaningful
        /// error that <i>resulted</i> from the other error, it can package the original error in the
        /// InnerException member of the exception <i>it</i> generates.
        /// </summary>
        /// <param name = "message">Descriptive text describing the cause of the exception</param>
        /// <param name = "number">Error code for the exception (80040400 - 80040FFF).</param>
        /// <param name = "inner">The inner exception that led to throwing this exception</param>
        public DriverException(string message, int number, Exception inner)
            : base(message, inner)
        {
            HResult = number;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "DriverException" /> class that will return the 'unspecified error' number: 0x800404FF.
        /// Sets the COM HResult to <see cref = "ErrorCodes.UnspecifiedError" />.
        /// </summary>
        public DriverException()
        {
            HResult = ErrorCodes.UnspecifiedError;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "DriverException" /> class
        /// with a human-readable descriptive message.
        /// </summary>
        /// <param name = "message">The human-readable description of the problem.</param>
        public DriverException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "DriverException" /> class from another caught exception and a human-readable descriptinve message.
        /// </summary>
        /// <param name = "message">The human-readable description of the problem.</param>
        /// <param name = "innerException">The caught (inner) exception.</param>
        public DriverException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "DriverException" /> class.
        /// </summary>
        /// <param name = "info">The <see cref = "T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name = "context">The <see cref = "T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref = "T:System.ArgumentNullException">
        /// The <paramref name = "info" /> parameter is null.
        /// </exception>
        /// <exception cref = "T:System.Runtime.Serialization.SerializationException">
        ///   The class name is null or <see cref = "P:System.Exception.HResult" /> is zero (0).
        /// </exception>
        protected DriverException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// The COM error code for this exception (hex 80040400 - 800404FF)
        /// </summary>
        public int Number
        {
            get { return HResult; }
        }
    }
}