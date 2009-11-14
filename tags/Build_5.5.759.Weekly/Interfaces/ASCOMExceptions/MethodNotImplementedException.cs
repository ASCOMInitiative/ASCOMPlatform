//
//	ASCOM Exceptions for drivers
//
// Originally written by Tim Long <tim@tigranetworks.co.uk>
// Adapted for new ASCOM Platform by Bob Denny <rdenny@dc3.com>
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace ASCOM
{

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
