using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.DynamicRemoteClients
{
    public abstract class RestResponseBase
    {
        private Exception exception;
        private string errorMessage = "";

        public uint ClientTransactionID { get; set; }
        public uint ServerTransactionID { get; set; }
        public int ErrorNumber { get; set; } = 0;

        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                //  Force ErrorMessage to return an empty string even if set to a null value. This prevents having to test ErrorMessage for a null value within the rest of the application
                if (value is null)
                {
                    errorMessage = "";
                }
                else
                {
                    errorMessage = value;
                }
            }
        }

        public Exception DriverException
        {
            get
            {
                return exception;
            }
            set
            {
                exception = value;
                if (exception != null)
                {
                    // Set the error number and message fields from the exception
                    ErrorNumber = exception.HResult;
                    ErrorMessage = exception.Message;

                    // Convert ASCOM exception error numbers (0x80040400 - 0x80040FFF) to equivalent Alpaca error numbers (0x400 to 0xFFF) so that they will be interpreted correctly by native Alpaca clients
                    if ((ErrorNumber >= SharedConstants.ASCOM_ERROR_NUMBER_BASE) && (ErrorNumber <= SharedConstants.ASCOM_ERROR_NUMBER_MAX))
                    {
                        ErrorNumber = ErrorNumber - SharedConstants.ASCOM_ERROR_NUMBER_OFFSET;
                    }
                }
            }
        }

        /// <summary>
        /// Method used by NewtonSoft JSON to determine whether the DriverException field should be included in the serialise JSON response.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeDriverException()
        {
            return SerializeDriverException;
        }

        /// <summary>
        /// Control variable that determines whether the DriverException field will be included in serialised JSON responses
        /// </summary>
        internal bool SerializeDriverException { get; set; } = true;
    }
}
