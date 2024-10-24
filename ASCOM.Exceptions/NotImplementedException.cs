using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace ASCOM
{
    /// <summary>
    /// All properties and methods defined by the relevant ASCOM standard interface must exist in each driver. However,
    /// those properties and methods do not all have to be <i>implemented</i>. This exception is a base class for
    /// PropertyNotImplementedException and MethodNotImplementedException, which drivers should use for throwing
    /// the relevant exception(s). This class is intended to be used by clients who wish to catch either of
    /// the two specific exceptions in a single catch() clause.
    /// </summary>
    /// <remarks>
    /// <para>If you need to throw this error as a COM exception use the error number: 0x80040400.</para>
    /// </remarks>
    [Serializable]
    [ComVisible(true)]
    [Guid("46584278-AC16-4CFC-8878-09CA960AEABE")]
    public class NotImplementedException : DriverException
    {
        /// <summary>
        /// A format string used to create the exception's human-readable message.
        /// </summary>
        [NonSerialized] const string csMessage = "{0} is not implemented in this driver.";
        [NonSerialized] string propertyOrMethod;

        /// <summary>
        /// Create a new exception object and identify the specified driver property or method as the source.
        /// </summary>
        /// <param name = "propertyOrMethod">The name of the driver property/accessor or method that caused the exception</param>
        public NotImplementedException(string propertyOrMethod) : base(String.Format(CultureInfo.InvariantCulture, csMessage, propertyOrMethod), ErrorCodes.NotImplemented)
        {
            PropertyOrMethod = propertyOrMethod;
        }

        /// <summary>
        /// Create a new exception object and identify the specified driver property as the source,
        /// and include an inner exception object containing a caught exception.
        /// </summary>
        /// <param name = "propertyOrMethod">The name of the driver property/accessor or method that caused the exception</param>
        /// <param name = "inner">The caught exception</param>
        public NotImplementedException(string propertyOrMethod, Exception inner) : base(String.Format(CultureInfo.InvariantCulture, csMessage, propertyOrMethod), ErrorCodes.NotImplemented, inner)
        {
            PropertyOrMethod = propertyOrMethod;
        }

        /// <summary>
        /// Create a not implemented exception using a provided member name, error message and inner exception
        /// </summary>
        /// <param name="memberName">The member name that threw the exception</param>
        /// <param name="errorMessage">The error message</param>
        /// <param name="innerException">An optional inner exception or null.</param>
        public NotImplementedException(string memberName, string errorMessage, Exception innerException) : base(errorMessage, ErrorCodes.NotImplemented, innerException)
        {
            propertyOrMethod = memberName;
        }

        /// <summary>
        /// Added to keep Code analysis happy, please don't use it.
        /// </summary>
        public NotImplementedException() : base("Unspecified", ErrorCodes.NotImplemented)
        {
        }

        /// <summary>
        /// Added to keep Code Analysis happy
        /// </summary>
        /// <param name = "info">Serialisation information</param>
        /// <param name = "context">Streaming context.</param>
        protected NotImplementedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Create an exception using the supplied message text
        /// </summary>
        /// <param name="propertyOrMethod">Property or method name</param>
        /// <param name="message">Exception message</param>
        /// <remarks>
        /// Sets the member name and forms a not implemented exception with the supplied message.
        /// </remarks>
        protected NotImplementedException(string propertyOrMethod, string message) : base(message, ErrorCodes.NotImplemented)
        {
            PropertyOrMethod = propertyOrMethod;
        }

        /// <summary>
        /// The property/accessor or method that is not implemented
        /// </summary>
        public string PropertyOrMethod
        {
            get { return propertyOrMethod; }
            private set { propertyOrMethod = value; }
        }
    }
}