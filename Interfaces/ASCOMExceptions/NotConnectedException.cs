using System;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace ASCOM
{
    /// <summary>
    ///   This exception should be raised when an operation is attempted that requires communication with the
    ///   device, but the device is disconnected.
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    [Guid("C22D9A81-63FA-4AC3-AD64-77B77D3CEB2B")]
    public class NotConnectedException : DriverException
    {
        [NonSerialized] const string csDefaultMessage = "Device is not connected";

        /// <summary>
        ///   Default public constructor for NotConnectedException takes no parameters.
        /// </summary>
        public NotConnectedException()
            : base(csDefaultMessage, ErrorCodes.NotConnected)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "NotConnectedException" /> class
        ///   from another exception.
        /// </summary>
        /// <param name = "innerException">The inner exception.</param>
        public NotConnectedException(Exception innerException)
            : base(csDefaultMessage, ErrorCodes.NotConnected, innerException)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "NotConnectedException" /> class
        ///   with a non-default error message.
        /// </summary>
        /// <param name = "message">A descriptive human-readable message.</param>
        public NotConnectedException(string message)
            : base(message, ErrorCodes.NotConnected)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "NotConnectedException" /> class
        ///   based on another exception.
        /// </summary>
        /// <param name = "message">Descriptive text documenting the cause or source of the error.</param>
        /// <param name = "innerException">The inner exception the led to the throwing of this exception.</param>
        public NotConnectedException(string message, Exception innerException)
            : base(message, ErrorCodes.NotConnected, innerException)
        {
        }

        /// <summary>
        ///   Added to keep Code Analysis happy
        /// </summary>
        /// <param name = "info"></param>
        /// <param name = "context"></param>
        protected NotConnectedException(SerializationInfo info,
                                        StreamingContext context) : base(info, context)
        {
        }
    }
}