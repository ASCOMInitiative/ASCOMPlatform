using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace ASCOM
{
    /// <summary>
    ///   All properties defined by the relevant ASCOM standard interface must exist in each driver. However,
    ///   those properties do not all have to be <i>implemented</i>. The minimum requirement
    ///   for each defined property is to throw the ASCOM.PropertyNotImplementedException for each
    ///   of its accessors. Note that no default constructor is supplied. Throwing this requires both the 
    ///   property name and unimplemented accessor type to be supplied.
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    [Guid("EA016028-4929-4962-B768-3A4F33FC36A8")]
    public class PropertyNotImplementedException : NotImplementedException
    {
        [NonSerialized] const string csMessage = "Property {0} {1}";
        [NonSerialized] readonly bool accessorSet;
        [NonSerialized] readonly string property = "Unknown"; // Should not need initialization (typ.)

        /// <summary>
        ///   Create a new exception object and identify the specified driver property and accessor as the source.
        /// </summary>
        /// <param name = "property">The name of the driver property that caused the exception.</param>
        /// <param name = "accessorSet">True if the exception is being thrown for the 'set' accessor, else false</param>
        public PropertyNotImplementedException(string property, bool accessorSet)
            : base(String.Format(CultureInfo.InvariantCulture, csMessage, (accessorSet ? "write" : "read"), property))
        {
            this.property = property;
            this.accessorSet = accessorSet;
        }

        /// <summary>
        ///   Create a new exception object and identify the specified driver property as the source,
        ///   and include an inner exception object containing a caught exception.
        /// </summary>
        /// <param name = "property">The name of the driver property that caused the exception</param>
        /// <param name = "accessorSet">True if the exception is being thrown for the 'set' accessor, else false</param>
        /// <param name = "inner">The caught exception</param>
        public PropertyNotImplementedException(string property, bool accessorSet, Exception inner)
            : base(
                String.Format(CultureInfo.InvariantCulture, csMessage, (accessorSet ? "write" : "read"), property),
                inner)
        {
            this.property = property;
            this.accessorSet = accessorSet;
        }

        /// <summary>
        ///   Create a new exception
        /// </summary>
        /// <param name = "message">Exception description</param>
        /// <param name = "inner">Underlying exception that caused this exception to be thrown.</param>
        public PropertyNotImplementedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        ///   Create a new exception
        /// </summary>
        public PropertyNotImplementedException()
            : base(String.Format(CultureInfo.InvariantCulture, csMessage, "", "Unknown"))
        {
        }

        /// <summary>
        ///   Create a new exception
        /// </summary>
        /// <param name = "message">Exception description</param>
        public PropertyNotImplementedException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PropertyNotImplementedException" /> class.
        /// </summary>
        /// <param name = "info">The <see cref = "T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name = "context">The <see cref = "T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref = "T:System.ArgumentNullException">
        ///   The <paramref name = "info" /> parameter is null.
        /// </exception>
        /// <exception cref = "T:System.Runtime.Serialization.SerializationException">
        ///   The class name is null or <see cref = "P:System.Exception.HResult" /> is zero (0).
        /// </exception>
        protected PropertyNotImplementedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///   The property that is not implemented
        /// </summary>
        public string Property
        {
            get { return property; }
        }

        /// <summary>
        ///   True if the 'set' accessor is not implemented, else false
        /// </summary>
        public bool AccessorSet
        {
            get { return accessorSet; }
        }
    }
}