using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace ASCOM
{
    /// <summary>
    /// Exception thrown by a driver when it receives an unknown command through the Action method. 
    /// </summary>
    /// <remarks>
    /// If you need to throw this error as a COM exception use the error number: 0x8004040C.
    /// </remarks>
    [Serializable]
    [ComVisible(true)]
    [Guid("6D6475A7-A6E0-4983-A4A8-EF7A8BCFFF1E")]
    public class ActionNotImplementedException : NotImplementedException
    {
        [NonSerialized] const string csMessage = "Action {0} is not implemented in this driver";
        [NonSerialized] readonly string action = "Unknown";

        /// <summary>
        ///   Create a new exception object and identify the specified driver method as the source.
        /// </summary>
        /// <param name = "Action">The name of the action that caused the exception.</param>
        public ActionNotImplementedException(string Action) : base(String.Format(CultureInfo.InvariantCulture, csMessage, Action))
        {
            this.action = Action;
            this.HResult = ErrorCodes.ActionNotImplementedException;
        }

        /// <summary>
        ///   Create a new exception object and identify the specified driver method as the source,
        ///   and include an inner exception object containing a caught exception.
        /// </summary>
        /// <param name = "Action">The name of the driver method that caused the exception</param>
        /// <param name = "inner">The caught exception</param>
        public ActionNotImplementedException(string Action, Exception inner) : base(String.Format(CultureInfo.InvariantCulture, csMessage, Action), inner)
        {
            this.action = Action;
            this.HResult = ErrorCodes.ActionNotImplementedException;
        }

        /// <summary>
        ///   For Code Analysis, please don't use
        /// </summary>
        public ActionNotImplementedException() : base("Unknown  Action")
        {
            this.HResult = ErrorCodes.ActionNotImplementedException;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionNotImplementedException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected ActionNotImplementedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        ///   The method that is not implemented
        /// </summary>
        public string Action
        {
            get { return action; }
        }
    }
}
