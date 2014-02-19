using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace ASCOM
{
    /// <summary>
    ///   All methods defined by the relevant ASCOM standard interface must exist in each driver. However,
    ///   those methods do not all have to be <i>implemented</i>. The minimum requirement
    ///   for each defined method is to throw the ASCOM.MethodNotImplementedException. Note
    ///   that no default constructor is supplied. Throwing this requires the the method name.
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    [Guid("BBED286E-5814-4467-9471-A499DED13452")]
    public class MethodNotImplementedException : NotImplementedException
    {
        [NonSerialized] const string csMessage = "Method {0}";
        [NonSerialized] readonly string method = "Unknown";

        /// <summary>
        ///   Create a new exception object and identify the specified driver method as the source.
        /// </summary>
        /// <param name = "method">The name of the driver method that caused the exception.</param>
        public MethodNotImplementedException(string method)
            : base(String.Format(CultureInfo.InvariantCulture, csMessage, method))
        {
            this.method = method;
        }

        /// <summary>
        ///   Create a new exception object and identify the specified driver method as the source,
        ///   and include an inner exception object containing a caught exception.
        /// </summary>
        /// <param name = "method">The name of the driver method that caused the exception</param>
        /// <param name = "inner">The caught exception</param>
        public MethodNotImplementedException(string method, Exception inner)
            : base(String.Format(CultureInfo.InvariantCulture, csMessage, method), inner)
        {
            this.method = method;
        }

        /// <summary>
        ///   For Code Analysis, please don't use
        /// </summary>
        public MethodNotImplementedException()
            : base("Unknown  Method")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodNotImplementedException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected MethodNotImplementedException(SerializationInfo info,
                                                StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        ///   The method that is not implemented
        /// </summary>
        public string Method
        {
            get { return method; }
        }
    }
}