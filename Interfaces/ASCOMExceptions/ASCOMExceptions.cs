//
//	ASCOM Exceptions for drivers
//
// Originally written by Tim Long <tim@tigranetworks.co.uk>
// Adapted for new ASCOM Platform by Bob Denny <rdenny@dc3.com>
//
using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM
{
	//
	// Cannot be an enum, as this would be really messy, requiring casts
	// for error codes not reserved.
	//
	/// <summary>
	/// Error numbers for use by drivers.
	/// </summary>
	/// <remarks>
	/// The range of permitted values falls within the class FACILTY_ITF as 
	/// defined by the operating system and COM. These values will never clash with 
	/// COM, RPC, or OS error codes.
	/// </remarks>
	public static class ErrorCodes
	{
		/// <summary>
		/// Reserved error number for property or method not implemented.
		/// </summary>
		/// <remarks>
		/// See ASCOM.Exception.NotImplementedException.
		/// </remarks>
		public static int NotImplemented = unchecked((int)0x80040400);
		/// <summary>
		/// The starting value for driver-specific error numbers. 
		/// </summary>
		/// <remarks>
		/// Drivers are free to choose their own numbers starting with
		/// DriverBase, up to and including DriverMax.
		/// </remarks>
		public static int DriverBase = unchecked((int)0x80040500);
		/// <summary>
		/// Maximum value for driver-specific error numbers. 
		/// </summary>
		/// <remarks>
		/// Drivers are free to choose their own numbers starting with
		/// DriverBase, up to and including DriverMax.
		/// </remarks>
		public static int DriverMax = unchecked((int)0x80040FFF);
	}

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
		/// The error code for this exception (hex 80040400 - 800404FF)
		/// </summary>
		public int Number { get { return this.HResult; } }

	}

	/// <summary>
	/// All properties and methods defined by the relevant ASCOM standard interface must exist in each driver. However,
	/// those properties and methods do not all have to be <i>implemented</i>. This exception is a base class for
	/// PropertyNotImplementedException and MethodNotImplementedException, which drivers should use for throwing
	/// the relevant exception(s). This class is intended to be used by clients who wich to catch either of
	/// the two specific exceptions in a single catch() clause.
	/// </summary>
	public class NotImplementedException : DriverException
	{
		private const string csMessage = "{0} is not implemented in this driver.";
		private string m_strPropertyOrMethod = "Unknown";

		/// <summary>
		/// Create a new exception object and identify the specified driver property or method as the source.
		/// </summary>
		/// <param name="strPropertyOrMethod">The name of the driver property/accessor or method that caused the exception</param>
		public NotImplementedException(string strPropertyOrMethod)
			: base(String.Format(csMessage, strPropertyOrMethod), ErrorCodes.NotImplemented)
		{
			m_strPropertyOrMethod = strPropertyOrMethod;
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
			m_strPropertyOrMethod = strPropertyOrMethod;
		}
		/// <summary>
		/// The property/accessor or method that is not implemented
		/// </summary>
		public string PropertyOrMethod { get { return m_strPropertyOrMethod; } }
	}

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
		public string Property 	{ get { return m_strProperty; }	}
		/// <summary>
		/// True if the 'set' accessor is not implemented, else false
		/// </summary>
		public bool AccessorSet { get { return m_bAccessorSet; } }
	}

	/// <summary>
	/// All methods defined by the relevant ASCOM standard interface must exist in each driver. However,
	/// those methods do not all have to be <i>implemented</i>. The minimum requirement
	/// for each defined method is to throw the ASCOM.MethodNotImplementedException. Note
	/// that no default constructor is supplied. Throwing this requires the the method name. 
	/// </summary>
	public class MethodNotImplementedException : NotImplementedException
	{
		private const string csMessage = "Method {0}";
		private string m_strMethod = "Unknown";

		/// <summary>
		/// Create a new exception object and identify the specified driver method as the source.
		/// </summary>
		/// <param name="strMethod">The name of the driver method that caused the exception.</param>
		public MethodNotImplementedException(string strMethod)
			: base(String.Format(csMessage, strMethod))
		{
			this.m_strMethod = strMethod;
		}
		/// <summary>
		/// Create a new exception object and identify the specified driver method as the source,
		/// and include an inner exception object containing a caught exception.
		/// </summary>
		/// <param name="strMethod">The name of the driver method that caused the exception</param>
		/// <param name="inner">The caught exception</param>
		public MethodNotImplementedException(string strMethod, System.Exception inner)
			: base(String.Format(csMessage, strMethod), inner)
		{
			this.m_strMethod = strMethod;
		}
		/// <summary>
		/// The method that is not implemented
		/// </summary>
		public string Method { get { return m_strMethod; } }
	}
}
