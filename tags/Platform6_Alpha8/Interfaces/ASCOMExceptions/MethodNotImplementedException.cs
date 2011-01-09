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
using System.Globalization;

namespace ASCOM
{

	/// <summary>
	/// All methods defined by the relevant ASCOM standard interface must exist in each driver. However,
	/// those methods do not all have to be <i>implemented</i>. The minimum requirement
	/// for each defined method is to throw the ASCOM.MethodNotImplementedException. Note
	/// that no default constructor is supplied. Throwing this requires the the method name. 
	/// </summary>
    [Serializable]
	public class MethodNotImplementedException : NotImplementedException
	{
        [NonSerialized]
        private const string csMessage = "Method {0}";
        [NonSerialized]
        private string method = "Unknown";

		/// <summary>
		/// Create a new exception object and identify the specified driver method as the source.
		/// </summary>
		/// <param name="method">The name of the driver method that caused the exception.</param>
		public MethodNotImplementedException(string method)
			: base(String.Format(CultureInfo.InvariantCulture,csMessage, method))
		{
			this.method = method;
		}
		/// <summary>
		/// Create a new exception object and identify the specified driver method as the source,
		/// and include an inner exception object containing a caught exception.
		/// </summary>
		/// <param name="method">The name of the driver method that caused the exception</param>
		/// <param name="inner">The caught exception</param>
		public MethodNotImplementedException(string method, System.Exception inner)
			: base(String.Format(CultureInfo.InvariantCulture, csMessage, method), inner)
		{
			this.method = method;
		}

        /// <summary>
        /// For Code Analysis, please don't use
        /// </summary>
        public MethodNotImplementedException()
            : base("Unknown  Method")
        {
        }

		/// <summary>
		/// The method that is not implemented
		/// </summary>
		public string Method { get { return method; } }

        /// <summary>
        /// Added to keep Code Analysis happy
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected MethodNotImplementedException(SerializationInfo info, 
         StreamingContext context) : base(info, context)
        {
        }

	}
}
