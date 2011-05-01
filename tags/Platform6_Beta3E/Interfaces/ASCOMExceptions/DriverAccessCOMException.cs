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
        public DriverAccessCOMException(string Message, int ErrorCode, Exception InnerException)
            : base(Message, InnerException)
        {
            HResult = ErrorCode;
        }
    }
}
