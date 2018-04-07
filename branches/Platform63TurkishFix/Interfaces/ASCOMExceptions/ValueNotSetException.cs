using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace ASCOM
{
    /// <summary>
    ///   Exception to report that no value has yet been set for this property.
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    [Guid("6B8F457E-29D6-463D-841B-85C85A8E6A1F")]
    public class ValueNotSetException : DriverException
    {
        [NonSerialized] const string csMessage = "{0} get - no value has been set.";
        [NonSerialized] readonly string propertyOrMethod = "Unknown";

        /// <summary>
        ///   Create a new exception object and identify the specified driver property or method as the source.
        /// </summary>
        /// <param name = "propertyOrMethod">The name of the driver property/accessor or method that caused the exception</param>
        public ValueNotSetException(string propertyOrMethod)
            : base(String.Format(CultureInfo.InvariantCulture, csMessage, propertyOrMethod), ErrorCodes.ValueNotSet)
        {
            this.propertyOrMethod = propertyOrMethod;
        }

        /// <summary>
        ///   Create a new exception object and identify the specified driver property as the source,
        ///   and include an inner exception object containing a caught exception.
        /// </summary>
        /// <param name = "propertyOrMethod">The name of the driver property/accessor or method that caused the exception</param>
        /// <param name = "inner">The caught exception</param>
        public ValueNotSetException(string propertyOrMethod, Exception inner)
            : base(
                String.Format(CultureInfo.InvariantCulture, csMessage, propertyOrMethod), ErrorCodes.ValueNotSet, inner)
        {
            this.propertyOrMethod = propertyOrMethod;
        }

        /// <summary>
        ///   Added to keep Code Analysis happy
        /// </summary>
        public ValueNotSetException()
            : this("unspecified value")
        {
        }

        /// <summary>
        ///   Added to keep Code Analysis happy
        /// </summary>
        /// <param name = "info"></param>
        /// <param name = "context"></param>
        protected ValueNotSetException(SerializationInfo info,
                                       StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        ///   The property/accessor or method that has no value
        /// </summary>
        public string PropertyOrMethod
        {
            get { return propertyOrMethod; }
        }
    }
}