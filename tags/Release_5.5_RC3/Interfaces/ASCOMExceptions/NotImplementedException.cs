using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace ASCOM
{
	/// <summary>
	/// All properties and methods defined by the relevant ASCOM standard interface must exist in each driver. However,
	/// those properties and methods do not all have to be <i>implemented</i>. This exception is a base class for
	/// PropertyNotImplementedException and MethodNotImplementedException, which drivers should use for throwing
	/// the relevant exception(s). This class is intended to be used by clients who wish to catch either of
	/// the two specific exceptions in a single catch() clause.
	/// </summary>
	public class NotImplementedException : DriverException
	{
		/// <summary>
		/// A format string used to create the exception's human-readable message.
		/// </summary>
		private const string csMessage = "{0} is not implemented in this driver.";

		/// <summary>
		/// The property/accessor or method that is not implemented
		/// </summary>
		public string PropertyOrMethod   {get; private set;}

		/// <summary>
		/// Create a new exception object and identify the specified driver property or method as the source.
		/// </summary>
		/// <param name="strPropertyOrMethod">The name of the driver property/accessor or method that caused the exception</param>
		public NotImplementedException(string strPropertyOrMethod)
			: base(String.Format(csMessage, strPropertyOrMethod), ErrorCodes.NotImplemented)
		{
			PropertyOrMethod = strPropertyOrMethod;
		}
		/// <summary>
		/// Create a new exception object and identify the specified driver property as the source,
		/// and include an inner exception object containing a caught exception.
		/// </summary>
		/// <param name="strPropertyOrMethod">The name of the driver property/accessor or method that caused the exception</param>
		/// <param name="inner">The caught exception</param>
		public NotImplementedException(string strPropertyOrMethod, System.Exception inner)
			: base(String.Format(csMessage, strPropertyOrMethod), ErrorCodes.NotImplemented, inner)
		{
			PropertyOrMethod = strPropertyOrMethod;
		}
	}
}
