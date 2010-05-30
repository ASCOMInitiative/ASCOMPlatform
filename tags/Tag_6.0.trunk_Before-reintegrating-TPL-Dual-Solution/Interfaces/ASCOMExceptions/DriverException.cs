using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace ASCOM
{
	/// <summary>
	/// This is the generic driver exception. Drivers are permitted to directly throw this
	/// exception as well as any derived exceptions. Note that the Message property is 
	/// a member of ApplicationException, the base class of DriverException. The HResult
	/// property of ApplicationException is simply renamed to Number.
	/// </summary>
	public class DriverException : ApplicationException
	{
		/// <summary>
		/// Create a new ASCOM exception using the specified text message and error code.
		/// </summary>
		/// <param name="message">Descriptive text describing the cause of the exception</param>
		/// <param name="number">Error code for the exception (80040400 - 80040FFF).</param>
		public DriverException(string message, int number)
			: base(message)
		{
			this.HResult = number;
		}
		/// <summary>
		/// Create a new ASCOM exception based on another exception plus additional descriptive text and error code. This member is 
		/// required for a well-behaved exception class. For example, if a driver receives an exception
		/// (perhaps a COMException) from some other component yet it wants to report some meaningful
		/// error that <i>resulted</i> from the other error, it can package the original error in the
		/// InnerException member of the exception <i>it</i> generates.
		/// </summary>
		/// <param name="message">Descriptive text describing the cause of the exception</param>
		/// <param name="number">Error code for the exception (80040400 - 80040FFF).</param>
		/// <param name="inner">The inner exception that led to throwing this exception</param>
		public DriverException(string message, int number, System.Exception inner)
			: base(message, inner)
		{
			this.HResult = number;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="DriverException"/> class.
		/// Sets the COM HResult to <see cref="ErrorCodes.UnspecifiedError"/>.
		/// </summary>
		public DriverException()
		{
			this.HResult = ErrorCodes.UnspecifiedError;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="DriverException"/> class
		/// with a human-readable descriptive message.
		/// </summary>
		/// <param name="message">The human-readable description of the problem.</param>
		public DriverException(string message)
			: base(message)
		{

		}
		/// <summary>
		/// Initializes a new instance of the <see cref="DriverException"/> class from
		/// another caught exception and a human-readable descriptinve message.
		/// </summary>
		/// <param name="message">The human-readable description of the problem.</param>
		/// <param name="innerException">The caught (inner) exception.</param>
		public DriverException(string message, Exception innerException)
			: base(message, innerException)
		{

		}

		/// <summary>
		/// The error code for this exception (hex 80040400 - 800404FF)
		/// </summary>
		public int Number { get { return this.HResult; } }

	}
}
