using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace ASCOM
{
	/// <summary>
	/// Exception to report that no value has yet been set for this property.
	/// </summary>
	public class ValueNotSetException : DriverException
	{
		private const string csMessage = "{0} get - no value has been set.";
		private string m_strPropertyOrMethod = "Unknown";

		/// <summary>
		/// Create a new exception object and identify the specified driver property or method as the source.
		/// </summary>
		/// <param name="strPropertyOrMethod">The name of the driver property/accessor or method that caused the exception</param>
		public ValueNotSetException(string strPropertyOrMethod)
			: base(String.Format(csMessage, strPropertyOrMethod), ErrorCodes.ValueNotSet)
		{
			m_strPropertyOrMethod = strPropertyOrMethod;
		}
		/// <summary>
		/// Create a new exception object and identify the specified driver property as the source,
		/// and include an inner exception object containing a caught exception.
		/// </summary>
		/// <param name="strPropertyOrMethod">The name of the driver property/accessor or method that caused the exception</param>
		/// <param name="inner">The caught exception</param>
		public ValueNotSetException(string strPropertyOrMethod, System.Exception inner)
			: base(String.Format(csMessage, strPropertyOrMethod), ErrorCodes.ValueNotSet, inner)
		{
			m_strPropertyOrMethod = strPropertyOrMethod;
		}
		/// <summary>
		/// The property/accessor or method that has no value
		/// </summary>
		public string PropertyOrMethod { get { return m_strPropertyOrMethod; } }
	}
}
