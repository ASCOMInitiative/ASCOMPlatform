using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace ASCOM
{
	/// <summary>
	/// Exception to report an invalid value supplied to a driver.
	/// </summary>
	/// <remarks>
	/// The most useful way to use this exception is to inform the user which property/method/parameter
	/// had the invalid value and also the range of allowed values.
	/// </remarks>
	public class InvalidValueException : DriverException
	{
		private const string csMessage = "{0} set - '{1}' is an invalid value. The valid range is: {2}.";
		private const string csUnspecified = "unspecified";

		/// <summary>
		/// Create a new exception object and identify the specified driver property or method as the source.
		/// </summary>
		/// <param name="strPropertyOrMethod">The name of the driver property/accessor or method that caused the exception</param>
		/// <param name="strValue">The invalid value that was supplied</param>
		/// <param name="strRange">The valid value range</param>
		public InvalidValueException(string strPropertyOrMethod, string strValue, string strRange)
			: base(String.Format(csMessage, strPropertyOrMethod, strValue, strRange), ErrorCodes.InvalidValue)
		{
			PropertyOrMethod = strPropertyOrMethod;
			Value = strValue;
			Range = strRange;
		}
		/// <summary>
		/// Create a new exception object and identify the specified driver property as the source,
		/// and include an inner exception object containing a caught exception.
		/// </summary>
		/// <param name="strPropertyOrMethod">The name of the driver property/accessor or method that caused the exception</param>
		/// <param name="strValue">The invalid value that was supplied</param>
		/// <param name="inner">The caught exception</param>
		/// <param name="strRange">The valid value range</param>
		public InvalidValueException(string strPropertyOrMethod, string strValue, string strRange, System.Exception inner)
			: base(String.Format(csMessage, strPropertyOrMethod, strValue, strRange), ErrorCodes.InvalidValue, inner)
		{
			PropertyOrMethod = strPropertyOrMethod;
			Value = strValue;
			Range = strRange;
		}
         
		/// <summary>
		/// The property/accessor or method that has an invalid value.
		/// </summary>
		public string PropertyOrMethod { get; private set; }
		/// <summary>
		/// The invalid value.
		/// </summary>
		public string Value { get; private set; }
		/// <summary>
		/// The valid range for this property.
		/// </summary>
		public string Range { get; private set; } 
	}
}
