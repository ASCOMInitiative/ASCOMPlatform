using System;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [Guid("2F0BD8E6-5FF5-48FA-BAB3-59D006638544")]
    public class OperationCompleteArgs : EventArgs
    {
        string errorMessage;

        /// <summary>
        /// Initialise with default values.
        /// </summary>
        public OperationCompleteArgs()
        {
            Operation = Operation.Uninitialised;
            ErrorNumber = 0;
            ErrorMessage = "";
        }

        /// <summary>
        /// Initialise with operation name.
        /// </summary>
        /// <param name="operation">Operation that completed.</param>
        public OperationCompleteArgs(Operation operation)
        {
            Operation = operation;
            ErrorNumber = 0;
            ErrorMessage = "";
        }

        /// <summary>
        /// Initialise with operation name, error number and error message.
        /// </summary>
        /// <param name="operation">Operation that completed.</param>
        /// <param name="errorNumber">Completion error number - 0 for success.E</param>
        /// <param name="errorMessage">Completion error message - Empty string for success.</param>
        public OperationCompleteArgs(Operation operation,int errorNumber,string errorMessage)
        {
            Operation = operation;
            ErrorNumber = errorNumber;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// The device operation that completed
        /// </summary>
        public Operation Operation { get; set; }

        /// <summary>
        /// Operation error status - 0 for success.
        /// </summary>
        public int ErrorNumber { get; set; }

        /// <summary>
        /// Operation error message - Use an empty string to denote success
        /// </summary>
        /// <exception cref="InvalidValueException">When a null value is set.</exception>
        public string ErrorMessage {
            get
            {
                return errorMessage;
            }
            set
            {
                // Validate the supplied string
                if (value is null) 
                    throw new InvalidValueException($"OperationCompleteArgs - The error message must not be null, use an empty string to denote a successful outcome.");

                errorMessage = value;
            }
        }
    }
}
