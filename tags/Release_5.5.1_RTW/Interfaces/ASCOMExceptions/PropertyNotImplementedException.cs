using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace ASCOM
{
	/// <summary>
	/// All properties defined by the relevant ASCOM standard interface must exist in each driver. However,
	/// those properties do not all have to be <i>implemented</i>. The minimum requirement
	/// for each defined property is to throw the ASCOM.PropertyNotImplementedException for each
	/// of its accessors. Note that no default constructor is supplied. Throwing this requires both the 
	/// property name and unimplemented accessor type to be supplied.
	/// </summary>
	public class PropertyNotImplementedException : NotImplementedException
	{
		private const string csMessage = "Property {0} {1}";
		private string m_strProperty = "Unknown";					// Should not need initialization (typ.)
		private bool m_bAccessorSet = false;

		/// <summary>
		/// Create a new exception object and identify the specified driver property and accessor as the source.
		/// </summary>
		/// <param name="strProperty">The name of the driver property that caused the exception.</param>
		/// <param name="bAccessorSet">True if the exception is being thrown for the 'set' accessor, else false</param>
		public PropertyNotImplementedException(string strProperty, bool bAccessorSet)
			: base(String.Format(csMessage, (bAccessorSet ? "write" : "read"), strProperty))
		{
			this.m_strProperty = strProperty;
			this.m_bAccessorSet = bAccessorSet;
		}
		/// <summary>
		/// Create a new exception object and identify the specified driver property as the source,
		/// and include an inner exception object containing a caught exception.
		/// </summary>
		/// <param name="strProperty">The name of the driver property that caused the exception</param>
		/// <param name="bAccessorSet">True if the exception is being thrown for the 'set' accessor, else false</param>
		/// <param name="inner">The caught exception</param>
		public PropertyNotImplementedException(string strProperty, bool bAccessorSet, System.Exception inner)
			: base(String.Format(csMessage, (bAccessorSet ? "write" : "read"), strProperty), inner)
		{
			this.m_strProperty = strProperty;
			this.m_bAccessorSet = bAccessorSet;
		}
		/// <summary>
		/// The property that is not implemented
		/// </summary>
		public string Property { get { return m_strProperty; } }
		/// <summary>
		/// True if the 'set' accessor is not implemented, else false
		/// </summary>
		public bool AccessorSet { get { return m_bAccessorSet; } }
	}
}
