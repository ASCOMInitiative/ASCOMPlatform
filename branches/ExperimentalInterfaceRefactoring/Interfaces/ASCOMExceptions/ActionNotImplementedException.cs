using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace ASCOM
{
    /// <summary>
    /// Exception to be thrown if a device receives an IASCOMDriver.Action command that it does not support
    /// </summary>
    public class ActionNotImplementedException : DriverException
    {
        private const string csMessage = "Action '{0}' is not supported by this device";
        private string m_strAction = "";					// Should not need initialization (typ.)
        
        /// <summary>
        /// Create a new exception object and identify the specified device control action as the reason.
        /// </summary>
        /// <param name="strAction">The name of the driver property that caused the exception.</param>
        public ActionNotImplementedException(string strAction)
            : base(String.Format(csMessage, strAction),ErrorCodes.ActionNotImplementedException)
        {
            this.m_strAction = strAction;
        }
        /// <summary>
        /// The action that is not implemented
        /// </summary>
        public string Action { get { return m_strAction; } }
    }
}

