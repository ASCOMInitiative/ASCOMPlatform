//-----------------------------------------------------------------------
// <summary>Defines the ExceptionLogger class.</summary>
//-----------------------------------------------------------------------
using System;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Globalization;
using System.Runtime.Remoting.Messaging;


namespace ASCOM.DriverAccess
{
    internal class ExceptionLogger
    {
        Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
        const string logName = "ASCOM";

        /// <summary>
        /// Logs exceptions for the DriverAccess asembly
        /// </summary> 
        public ExceptionLogger()
        {
            // Create the source, if it does not already exist.
            if (!EventLog.SourceExists(a.FullName))
            {
                //An event log source should not be created and immediately used.
                //There is a latency time to enable the source, it should be created
                //prior to executing the application that uses the source.
                //Execute this sample a second time to use the new source.
                EventLog.CreateEventSource(a.FullName, logName);
                // The source is created.  Exit the application to allow it to be registered.
                return;
            }
        }

        /// <summary>
        /// Logs error from the memberfactory calls to driver properties and methods
        /// in just about all cases the propertyinfo and methodinfo should be null
        /// meaning it was not found in the driver. 
        /// </summary> 
        public void LogError(string strError, string strProgID, PropertyInfo propertyInfo, MethodInfo methodInfo)
        {
            EventLog log = new EventLog(logName);
            log.Source = a.FullName;
            strError = BuildError(strError, strProgID, propertyInfo, methodInfo);
            log.WriteEntry(strError, EventLogEntryType.Warning);
        }

        /// <summary>
        /// Logs the exception in the event log
        /// </summary> 
        public void LogException(Exception exception)
        {
            EventLog log = new EventLog(logName);
            log.Source = a.FullName;

            StringBuilder error = new StringBuilder();
            error.AppendLine("Exception classes:   ");
            error.Append(GetExceptionTypeStack(exception));
            error.AppendLine("");
            error.AppendLine("Exception messages: ");
            error.Append(GetExceptionMessageStack(exception));
            error.AppendLine("");
            error.AppendLine("Stack Traces:");
            error.Append(GetExceptionCallStack(exception));
            error.AppendLine("");
            log.WriteEntry(error.ToString(), EventLogEntryType.Error);
        }

        /// <summary>
        /// Buils the string for the error
        /// </summary> 
        public string BuildError(string strError, string strProgID, PropertyInfo propertyInfo, MethodInfo methodInfo)
        {
            StringBuilder error = new StringBuilder();

            error.AppendLine("Error:             " + strError);
            error.AppendLine("ProgID:            " + strProgID);
            error.AppendLine("Date:              " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            error.AppendLine("OS:                " + Environment.OSVersion.ToString());
            error.AppendLine("Culture:           " + CultureInfo.CurrentCulture.Name);
 
            if (propertyInfo != null)
            {
                error.AppendLine("Property Info       ");
                error.AppendLine("Name:               " + propertyInfo.Name);
                error.AppendLine("Can Read:           " + propertyInfo.CanRead.ToString());
                error.AppendLine("Can Write:          " + propertyInfo.CanWrite);
            }
            if (methodInfo != null)
            {
                error.AppendLine("Method Info         ");
                error.AppendLine("Name:               " + methodInfo.Name);
                error.AppendLine("Is Public:          " + methodInfo.IsPublic.ToString());
                error.AppendLine("Is Static:          " + methodInfo.IsStatic.ToString());
            }
            return error.ToString();
        }

        /// <summary>
        /// Buils the string for the error type stack
        /// </summary> 
        private string GetExceptionTypeStack(Exception e)
        {
            if (e.InnerException != null)
            {
                StringBuilder message = new StringBuilder();
                message.AppendLine(GetExceptionTypeStack(e.InnerException));
                message.AppendLine("   " + e.GetType().ToString());
                return (message.ToString());
            }
            else
            {
                return "   " + e.GetType().ToString();
            }
        }

        /// <summary>
        /// Buils the string for the message stack
        /// </summary> 
        private string GetExceptionMessageStack(Exception e)
        {
            if (e.InnerException != null)
            {
                StringBuilder message = new StringBuilder();
                message.AppendLine(GetExceptionMessageStack(e.InnerException));
                message.AppendLine("   " + e.Message);
                return (message.ToString());
            }
            else
            {
                return "   " + e.Message;
            }
        }

        /// <summary>
        /// Buils the string for the caall stack
        /// </summary> 
        private string GetExceptionCallStack(Exception e)
        {
            if (e.InnerException != null)
            {
                StringBuilder message = new StringBuilder();
                message.AppendLine(GetExceptionCallStack(e.InnerException));
                message.AppendLine("--- Next Call Stack:");
                message.AppendLine(e.StackTrace);
                return (message.ToString());
            }
            else
            {
                return e.StackTrace;
            }
        }
    }
}
