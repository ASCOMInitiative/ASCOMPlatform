using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM
{
	/// <summary>
	/// Exception thrown when a value is outside the allowed range.
	/// This would typically be used to range-check method arguments.
	/// </summary>
	public class OutOfRangeException : DriverException
	{
		/// <summary>
		/// Gets the name of the value that is out of range. This may be shown to the user.
		/// </summary>
		/// <value>The name of the value.</value>
		public string ValueName { get; private set; }
		/// <summary>
		/// Gets a description of the range of valid values. This may be shown to the user.
		/// </summary>
		/// <value>The expected range.</value>
		public string ExpectedRange { get; private set; }
		/// <summary>
		/// Initializes a new instance of the <see cref="OutOfRangeException"/> class.
		/// </summary>
		public OutOfRangeException()
			: this("The supplied value")
		{ }
		/// <summary>
		/// Initializes a new instance of the <see cref="OutOfRangeException"/> class
		/// with a caught (inner) exception.
		/// </summary>
		/// <param name="innerException">The inner exception.</param>
		public OutOfRangeException(Exception innerException)
			: this("The supplied value", innerException)
		{ }
		/// <summary>
		/// Initializes a new instance of the <see cref="OutOfRangeException"/> class
		/// specifying the name of the value causing the exception.
		/// </summary>
		/// <param name="valueName">Name of the value that caused the exception.</param>
		public OutOfRangeException(string valueName)
			: this(valueName, "(Unspecified)")
		{ }
		/// <summary>
		/// Initializes a new instance of the <see cref="OutOfRangeException"/> class
		/// with the name of the value that caused the exception and a caught (inner) exception.
		/// </summary>
		/// <param name="valueName">Name of the value that caused the exception.</param>
		/// <param name="innerException">The inner exception.</param>
		public OutOfRangeException(string valueName, Exception innerException)
			: this(valueName, "(Unspecified)", innerException)
		{ }
		/// <summary>
		/// Initializes a new instance of the <see cref="OutOfRangeException"/> class.
		/// </summary>
		/// <param name="valueName">Name of the value that caused the exception.</param>
		/// <param name="rangeDescription">A description of the acceptable range of values.</param>
		public OutOfRangeException(string valueName, string rangeDescription)
			: base(String.Format("{0} was outside the expected range of {1}.", valueName, rangeDescription),
			ErrorCodes.InvalidValue)
		{
			this.ValueName = valueName;
			this.ExpectedRange = rangeDescription;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="OutOfRangeException"/> class
		/// with the name of the value that caused the exception,
		/// A description of the allowed range of values
		/// and a caught (inner) exception.
		/// </summary>
		/// <param name="valueName">Name of the value that caused the exception.</param>
		/// <param name="rangeDescription">The description of the acceptable range of values.</param>
		/// <param name="innerException">The inner exception.</param>
		public OutOfRangeException(string valueName, string rangeDescription, Exception innerException)
			: base(String.Format("{0} was outside the expected range of {1}.", valueName, rangeDescription),
			ErrorCodes.InvalidValue, innerException)
		{
			this.ValueName = valueName;
			this.ExpectedRange = rangeDescription;
		}
	}
}
