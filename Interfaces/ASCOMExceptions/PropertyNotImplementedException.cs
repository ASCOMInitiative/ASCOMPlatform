using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Globalization;

namespace ASCOM
{
	/// <summary>
	/// All properties defined by the relevant ASCOM standard interface must exist in each driver. However,
	/// those properties do not all have to be <i>implemented</i>. The minimum requirement
	/// for each defined property is to throw the ASCOM.PropertyNotImplementedException for each
	/// of its accessors. Note that no default constructor is supplied. Throwing this requires both the 
	/// property name and unimplemented accessor type to be supplied.
	/// </summary>
    [Serializable]
	public class PropertyNotImplementedException : NotImplementedException
	{
        [NonSerialized]
		private const string csMessage = "Property {0} {1}";
        [NonSerialized]
		private string property = "Unknown";					// Should not need initialization (typ.)
        [NonSerialized]
		private bool accessorSet = false;

		/// <summary>
		/// Create a new exception object and identify the specified driver property and accessor as the source.
		/// </summary>
		/// <param name="property">The name of the driver property that caused the exception.</param>
		/// <param name="accessorSet">True if the exception is being thrown for the 'set' accessor, else false</param>
		public PropertyNotImplementedException(string property, bool accessorSet)
			: base(String.Format(CultureInfo.InvariantCulture, csMessage, (accessorSet ? "write" : "read"), property))
		{
			this.property = property;
			this.accessorSet = accessorSet;
		}
		/// <summary>
		/// Create a new exception object and identify the specified driver property as the source,
		/// and include an inner exception object containing a caught exception.
		/// </summary>
		/// <param name="property">The name of the driver property that caused the exception</param>
		/// <param name="accessorSet">True if the exception is being thrown for the 'set' accessor, else false</param>
		/// <param name="inner">The caught exception</param>
		public PropertyNotImplementedException(string property, bool accessorSet, System.Exception inner)
			: base(String.Format(CultureInfo.InvariantCulture, csMessage, (accessorSet ? "write" : "read"), property), inner)
		{
			this.property = property;
			this.accessorSet = accessorSet;
		}

        /// <summary>
        /// Create a new exception
        /// </summary>
        /// <param name="message">Exception description</param>
        /// <param name="inner">Underlying exception that caused this exception to be thrown.</param>
        public PropertyNotImplementedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Create a new exception
        /// </summary>
        public PropertyNotImplementedException()
            : base(String.Format(CultureInfo.InvariantCulture, csMessage, "", "Unknown"))
        {
        }

        /// <summary>
        /// Create a new exception
        /// </summary>
        /// <param name="message">Exception description</param>
        public PropertyNotImplementedException(string message)
            : base(message)
        {
        }

		/// <summary>
		/// The property that is not implemented
		/// </summary>
		public string Property { get { return property; } }
		/// <summary>
		/// True if the 'set' accessor is not implemented, else false
		/// </summary>
		public bool AccessorSet { get { return this.accessorSet; } }

        /// <summary>
        /// Added to keep Code Analysis happy
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected PropertyNotImplementedException(SerializationInfo info, 
         StreamingContext context) : base(info, context)
        {
        }

	}
}
