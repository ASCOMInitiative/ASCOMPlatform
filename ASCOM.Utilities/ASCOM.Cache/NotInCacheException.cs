using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
namespace ASCOM.Utilities.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested item is not present in the ASCOM cache.
    /// </summary>
    /// <remarks>When returned through COM, the exception number is hex 0x8004040D.</remarks>
    [Serializable]
    [ComVisible(true)]
    [Guid("37B88116-99E8-4ED1-B580-5F71184EE9A4")]
    public class NotInCacheException : Exception
    {
        /// <summary>
        /// Create a new ASCOM NotInCacheException exception using the specified text message
        /// </summary>
        /// <param name = "message">Descriptive text describing the cause of the exception</param>
        public NotInCacheException(string message) : base(message)
        {
            HResult = ErrorCodes.NotInCacheException;
        }

        /// <summary>
        /// Create a new ASCOM NotInCacheException exception using the specified text message and inner exception.
        /// </summary>
        /// <param name = "message">Descriptive text describing the cause of the exception</param>
        /// <param name = "inner">The inner exception that led to throwing this exception</param>
        public NotInCacheException(string message, Exception inner) : base(message, inner)
        {
            HResult = ErrorCodes.NotInCacheException;
        }

        /// <summary>
        /// Initializes a new instance of the ASCOM NotInCacheException exception with no message.
        /// Sets the COM HResult to <see cref = "ErrorCodes.UnspecifiedError" />.
        /// </summary>
        public NotInCacheException() : base()
        {
            HResult = ErrorCodes.NotInCacheException;
        }

        /// <summary>
        /// Initializes a new instance of the ASCOM NotInCacheException exception.
        /// </summary>
        /// <param name = "info">The <see cref = "T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name = "context">The <see cref = "T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref = "T:System.ArgumentNullException">
        /// The <paramref name = "info" /> parameter is null.
        /// </exception>
        /// <exception cref = "T:System.Runtime.Serialization.SerializationException">
        ///   The class name is null or <see cref = "P:System.Exception.HResult" /> is zero (0).
        /// </exception>
        protected NotInCacheException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            HResult = ErrorCodes.NotInCacheException;
        }

        /// <summary>
        /// The COM error code for this exception (hex 0x8004040D)
        /// </summary>
        public int Number
        {
            get { return HResult; }
        }
    }
}