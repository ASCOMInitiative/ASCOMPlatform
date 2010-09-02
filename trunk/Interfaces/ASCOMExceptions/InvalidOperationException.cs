using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM
{
    /// <summary>
    /// This exception should be raised by the driver to reject a command from the client. It is intended to be
    /// used for "logical" errors e.g. trying to use a command when the current configuration of the device does
    /// not allow it rather than for device errors such as a communications error.
    /// <para>Its the error to use when the client attempts something, which at another time would be sensible,
    /// but which is not sensible right now. If you expect the condition causing the issue to be short
    /// lived, you may choose to stall the request until the condition is cleared rather than throwing this exception.
    /// Clearly, that is a judgement that you can only make given a specific scenario.</para>
    /// </summary>
    public class InvalidOperationException : ASCOM.DriverException
    {
        private const string csDefaultMessage = "The requested operation is not permitted at this time";
        /// <summary>
        /// Default public constructor for NotConnectedException takes no parameters.
        /// </summary>
        public InvalidOperationException()
            : base(csDefaultMessage, ErrorCodes.InvalidOperationException)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidOperationException"/> class
        /// from another exception.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public InvalidOperationException(Exception innerException)
            : base(csDefaultMessage, ErrorCodes.InvalidOperationException, innerException)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidOperationException"/> class
        /// with a non-default error message.
        /// </summary>
        /// <param name="message">A descriptive human-readable message.</param>
        public InvalidOperationException(string message)
            : base(message, ErrorCodes.InvalidOperationException)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidOperationException"/> class
        /// based on another exception.
        /// </summary>
        /// <param name="message">Descriptive text documenting the cause or source of the error.</param>
        /// <param name="innerException">The inner exception the led to the throwing of this exception.</param>
        public InvalidOperationException(string message, Exception innerException)
            : base(message, ErrorCodes.InvalidOperationException, innerException)
        {
        }
    }

}
