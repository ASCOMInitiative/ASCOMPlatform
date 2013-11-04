using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace ASCOM
{
    /// <summary>
    ///   Exception to report an invalid value supplied to a driver.
    /// </summary>
    /// <remarks>
    ///   The most useful way to use this exception is to inform the user which property/method/parameter
    ///   had the invalid value and also the range of allowed values.
    /// </remarks>
    [Serializable]
    [ComVisible(true)]
    [Guid("939B5C76-A502-4729-8786-0C1600445EFE")]
    public class InvalidValueException : DriverException
    {
        [NonSerialized] const string csMessage = "{0} set - '{1}' is an invalid value. The valid range is: {2}.";
        [NonSerialized] const string csUnspecified = "unspecified";
        [NonSerialized] string invalidValue;
        [NonSerialized] string propertyOrMethod;
        [NonSerialized] string range;

        /// <summary>
        ///   Create a new exception object and identify the specified driver property or method as the source.
        /// </summary>
        /// <param name = "propertyOrMethod">The name of the driver property/accessor or method that caused the exception</param>
        /// <param name = "value">The invalid value that was supplied</param>
        /// <param name = "range">The valid value range</param>
        public InvalidValueException(string propertyOrMethod, string value, string range)
            : base(
                String.Format(CultureInfo.InvariantCulture, csMessage, propertyOrMethod, value, range),
                ErrorCodes.InvalidValue)
        {
            PropertyOrMethod = propertyOrMethod;
            Value = value;
            Range = range;
        }

        /// <summary>
        ///   Create a new exception object and identify the specified driver property as the source,
        ///   and include an inner exception object containing a caught exception.
        /// </summary>
        /// <param name = "propertyOrMethod">The name of the driver property/accessor or method that caused the exception</param>
        /// <param name = "value">The invalid value that was supplied</param>
        /// <param name = "inner">The caught exception</param>
        /// <param name = "range">The valid value range</param>
        public InvalidValueException(string propertyOrMethod, string value, string range, Exception inner)
            : base(
                String.Format(CultureInfo.InvariantCulture, csMessage, propertyOrMethod, value, range),
                ErrorCodes.InvalidValue, inner)
        {
            PropertyOrMethod = propertyOrMethod;
            Value = value;
            Range = range;
        }

        /// <summary>
        ///   Create a new exception
        /// </summary>
        /// <param name = "message">Exception description</param>
        public InvalidValueException(string message)
            : base(message, ErrorCodes.InvalidValue)
        {
        }

        /// <summary>
        ///   Create a new exception
        /// </summary>
        /// <param name = "message">Exception description</param>
        /// <param name = "inner">The underlying exception that caused this exception to be thrown.</param>
        public InvalidValueException(string message, Exception inner)
            : base(message, ErrorCodes.InvalidValue, inner)
        {
        }

        /// <summary>
        ///   Create a new exception object
        /// </summary>
        public InvalidValueException()
            : base(csUnspecified, ErrorCodes.InvalidValue)
        {
        }

        /// <summary>
        ///   Added to keep Code Analysis happy
        /// </summary>
        /// <param name = "info"></param>
        /// <param name = "context"></param>
        protected InvalidValueException(SerializationInfo info,
                                        StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        ///   The property/accessor or method that has an invalid value.
        /// </summary>
        public string PropertyOrMethod
        {
            get { return propertyOrMethod; }
            private set { propertyOrMethod = value; }
        }

        /// <summary>
        ///   The invalid value.
        /// </summary>
        public string Value
        {
            get { return invalidValue; }
            private set { invalidValue = value; }
        }

        /// <summary>
        ///   The valid range for this property.
        /// </summary>
        public string Range
        {
            get { return range; }
            private set { range = value; }
        }
    }
}