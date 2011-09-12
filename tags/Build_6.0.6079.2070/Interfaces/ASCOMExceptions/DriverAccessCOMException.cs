using System;
using System.Runtime.InteropServices;

namespace ASCOM
{
    /// <summary>
    /// Exception thrown by DriverAccess to return a driver COM error to the client. This exception appears as a COMException 
    /// to the client having the original exception's description and error number as well as the original exception as
    /// the inner exception.
    /// </summary>
    [Serializable]
    public class DriverAccessCOMException : COMException
    {
        /// <summary>
        /// Creates a new DriverAccessCOException
        /// </summary>
        /// <param name="Message">The error message to display</param>
        /// <param name="ErrorCode">The COM error code to attach to this exception</param>
        /// <param name="InnerException">Any inner exception that is to be attached to the exception, or null if there is no inner exception</param>
        public DriverAccessCOMException(string Message, int ErrorCode, Exception InnerException)
            : base(Message, InnerException)
        {
            HResult = ErrorCode;
        }
    }
}
