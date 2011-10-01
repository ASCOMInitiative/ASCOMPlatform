//-----------------------------------------------------------------------
// <summary>Defines the MemberFactory class.</summary>
//-----------------------------------------------------------------------
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// A factory class to access any registered driver members
    /// </summary>
    internal class MemberFactory : IDisposable
    {
        #region MemberFactory

        private TraceLogger TL;
        private readonly String _strProgId;

        /// <summary>
        /// Constructor, creates an instance of the of the ASCOM driver using the given TraceLogger
        /// 
        /// </summary> 
        /// <param name="progId">The program ID of the driver</param>
        /// <param name="ascomDriverTraceLogger">The supplied TraceLogger instance in which to log activity</param>

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        internal MemberFactory(string progId, TraceLogger ascomDriverTraceLogger)
        {
            //_tl = new TraceLogger("", "MemberFactory");
            //_tl.Enabled = RegistryCommonCode.GetBool(GlobalConstants.DRIVERACCESS_TRACE, GlobalConstants.DRIVERACCESS_TRACE_DEFAULT);
            TL = ascomDriverTraceLogger; // Save the supplied TraceLogger object for use in method calls
            TL.LogMessage("ProgID", progId);

            _strProgId = progId;
            GetInterfaces = new List<Type>();

            // Get Type Information 
            GetObjType = Type.GetTypeFromProgID(progId);

            //check to see if it found the type information
            if (GetObjType == null)
            {
                //no type information found throw error
                throw new ASCOM.Utilities.Exceptions.HelperException("Check Driver: cannot create object type of progID: " + _strProgId);
            }

            //setup the property
            IsComObject = GetObjType.IsCOMObject;
            TL.LogMessage("IsComObject", GetObjType.IsCOMObject.ToString());

            // Create an instance of the object
            GetLateBoundObject = Activator.CreateInstance(GetObjType);

            // Get list of interfaces but don't throw an exception if this fails
            try
            {
                var objInterfaces = GetObjType.GetInterfaces();

                foreach (Type objInterface in objInterfaces)
                {
                    GetInterfaces.Add(objInterface);
                    TL.LogMessage("GetInterfaces", "Found interface: " + objInterface.AssemblyQualifiedName);
                }
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("GetInterfaces", "Exception: " + ex.ToString());
            }

            /*MemberInfo[] members = GetObjType.GetMembers();
            foreach (MemberInfo mi in members)
            {
                TL.LogMessage("Member", Enum.GetName(typeof(MemberTypes), mi.MemberType) + " " + mi.Name);
                if (mi.MemberType == MemberTypes.Method)
                {
                    foreach (ParameterInfo pi in ((MethodInfo)mi).GetParameters())
                    {
                        TL.LogMessage("Parameter",
                                       " " + pi.Name +
                                       " " + pi.ParameterType.Name +
                                       " " + pi.ParameterType.AssemblyQualifiedName);
                    }
                }
            }*/

            //no instance found throw error
            if (GetLateBoundObject == null)
            {
                TL.LogMessage("Exception", "GetLateBoudObject is null, throwing HelperException");
                throw new ASCOM.Utilities.Exceptions.HelperException("Check Driver: cannot create driver instance of progID: " + _strProgId);
            }
        }

        /// <summary>
        /// Returns the instance of the driver
        /// </summary> 
        /// <returns>object</returns>
        internal object GetLateBoundObject { get; private set; }

        /// <summary>
        /// Returns true is the driver is COM based
        /// </summary> 
        /// <returns>object</returns>
        internal bool IsComObject { get; private set; }

        /// <summary>
        /// Returns the driver type
        /// </summary> 
        /// <returns>type</returns>
        internal Type GetObjType { get; private set; }

        /// <summary>
        /// Returns a list of supported interfaces
        /// </summary> 
        /// <returns>type</returns>
        internal List<Type> GetInterfaces { get; private set; }

        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
        /// if it is a COM object, else if native .NET will just dereference it
        /// for GC.
        /// </summary>
        /// <returns>nothing</returns>
        public void Dispose()
        {
            if (GetLateBoundObject != null)
            {
                try
                {
                    if (IsComObject)
                    {
                        // Attempt to call Dispose first...

                        //run the COM object method
                        try
                        {
                            TL.LogMessageCrLf("Dispose COM", "This is a COM object, attempting to call its Dispose method");
                            GetObjType.InvokeMember("Dispose",
                                                    BindingFlags.Default | BindingFlags.InvokeMethod,
                                                    null,
                                                    GetLateBoundObject,
                                                    new object[] { },
                                                    CultureInfo.InvariantCulture);
                            TL.LogMessageCrLf("Dispose COM", "Successfully called its Dispose method");
                        }
                        catch (COMException ex)
                        {
                            if (ex.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber, CultureInfo.InvariantCulture))
                            {
                                TL.LogMessageCrLf("Dispose COM", "Driver does not have a Dispose method");
                            }
                        }
                        catch (Exception ex)
                        {
                            TL.LogMessageCrLf("Dispose COM", "Exception " + ex.ToString());
                        }

                        TL.LogMessageCrLf("Dispose COM", "This is a COM object so attempting to release it");
                        var releaseComObject = Marshal.ReleaseComObject(GetLateBoundObject);
                        if (releaseComObject == 0) GetLateBoundObject = null;
                        TL.LogMessageCrLf("Dispose COM", "Object Count is now: " + releaseComObject);
                    }
                    else // Should be a .NET object so lets dispose of it
                    {
                        TL.LogMessageCrLf("Dispose .NET", "This is a .NET object, attempting to call its Dispose method");
                        var methodInfo = GetObjType.GetMethod("Dispose");

                        if (methodInfo != null)
                        {
                            TL.LogMessage("Dispose .NET", "  Got Dispose Method Info, Calling Dispose");
                            object result = methodInfo.Invoke(GetLateBoundObject, new object[] { });
                            TL.LogMessage("Dispose .NET", "  Successfully called Dispose method");
                        }
                        else
                        {
                            TL.LogMessage("Dispose .NET", "  No Dispose Method Info so ignoring the call to Dispose");
                        }
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        TL.LogMessageCrLf("Dispose", "Exception " + ex.ToString());
                    }
                    catch { }
                } // Ignore any errors here as we are disposing


                //if (TL != null) TL.Dispose(); no longer need to dispose of this as its now handled by AscomDriver
            }
        }

        /// <summary>
        /// Calls a method on an object dynamically. 
        /// 
        /// parameterTypes must match the parameters and in the same order.
        /// </summary> 
        /// <param name="memberCode">1-GetProperty, 2-SetProperty, 3-Method</param>
        /// <param name="memberName">The member name to call as a string</param>
        /// <param name="parameterTypes">Array of paramerter types in order</param> 
        /// <param name="parms">Array of parameters in order</param>
        /// <exception cref="PropertyNotImplementedException"></exception>
        /// <exception cref="MethodNotImplementedException"></exception>
        /// <returns>object</returns>
        internal object CallMember(int memberCode, string memberName, Type[] parameterTypes, params object[] parms)
        {
            TL.BlankLine();
            switch (memberCode)
            {
                case 1: // Property Read
                    PropertyInfo propertyGetInfo = GetObjType.GetProperty(memberName);
                    if (propertyGetInfo != null) // We have a .NET object
                    {
                        TL.LogMessage(memberName + " Get", "GET " + memberName + " - .NET");
                        try
                        {
                            //run the .net object
                            object result = propertyGetInfo.GetValue(GetLateBoundObject, null);
                            TL.LogMessage(memberName + " Get", "  " + result.ToString());
                            return result;
                        }
                        catch (TargetInvocationException e)
                        {
                            GetTargetInvocationExceptionHandler(memberName, e);
                        }
                        catch (Exception e)
                        {
                            TL.LogMessageCrLf("Exception", e.ToString());
                            throw;
                        }
                    }

                    //check the type to see if it's a COM object
                    if (IsComObject) // We have a COM object
                    {
                        TL.LogMessage(memberName + " Get", "GET " + memberName + " - COM");

                        try
                        {
                            //run the COM object property
                            object result = GetObjType.InvokeMember(memberName,
                                                         BindingFlags.Default | BindingFlags.GetProperty,
                                                         null,
                                                         GetLateBoundObject,
                                                         new object[] { },
                                                         CultureInfo.InvariantCulture);
                            TL.LogMessage(memberName + " Get", "  " + result.ToString());
                            return result;
                        }
                        catch (COMException e)
                        {
                            TL.LogMessageCrLf("COMException", e.ToString());
                            if (e.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber, CultureInfo.InvariantCulture))
                            {
                                TL.LogMessageCrLf(memberName + " Get", "  Throwing PropertyNotImplementedException: " + _strProgId + " " + memberName);
                                throw new PropertyNotImplementedException(_strProgId + " " + memberName, false, e);
                            }
                            TL.LogMessageCrLf(memberName + " Get", "Re-throwing exception");
                            throw;
                        }
                        catch (TargetInvocationException e)
                        {
                            TL.LogMessageCrLf("TargetInvocationException", e.ToString());
                            if (e.InnerException is COMException)
                            {
                                string message = e.InnerException.Message;
                                int errorcode = ((COMException)e.InnerException).ErrorCode;
                                // Telescope simulator in V1 mode throws not implemented exceptions rather than some kind of missing member exception, so test for this!
                                if (errorcode == int.Parse("80040400", NumberStyles.HexNumber, CultureInfo.InvariantCulture))
                                {
                                    TL.LogMessageCrLf(memberName + " Get", "  Translating COM not implemented exception to PropertyNotImplementedException: " + _strProgId + " " + memberName);
                                    throw new PropertyNotImplementedException(_strProgId + " " + memberName, false, e);
                                }
                                else // Throw a new COM exception that looks like the original exception, containing the original inner COM exception for reference
                                {
                                    TL.LogMessageCrLf(memberName + " Get", "COM Exception so throwing inner exception: '" + message + "' '0x" + String.Format("{0:x8}", errorcode) + "'");
                                    throw new DriverAccessCOMException(message, errorcode, e);
                                }
                            }

                            GetTargetInvocationExceptionHandler(memberName, e);
                        }

                        catch (Exception e)
                        {
                            TL.LogMessageCrLf("Exception", e.ToString());
                            throw;
                        }
                    }

                    //evertyhing failed so throw an exception
                    TL.LogMessage(memberName + " Get", "  The object is neither a .NET object nor a COM object!");
                    throw new PropertyNotImplementedException(_strProgId + " " + memberName, false);

                case 2: // Property Write
                    PropertyInfo propertySetInfo = GetObjType.GetProperty(memberName);
                    if (propertySetInfo != null) // We have a .NET object
                    {
                        TL.LogMessage(memberName + " Set", "SET " + memberName + " - .NET");
                        try
                        {
                            TL.LogMessage(memberName + " Set", "  " + parms[0].ToString());
                            propertySetInfo.SetValue(GetLateBoundObject, parms[0], null);
                            return null;
                        }
                        catch (TargetInvocationException e)
                        {
                            SetTargetInvocationExceptionHandler(memberName, e);
                        }
                        catch (Exception e)
                        {
                            TL.LogMessageCrLf("Exception", e.ToString());
                            throw;
                        }
                    }

                    //check the type to see if it's a COM object
                    if (IsComObject)
                    {
                        TL.LogMessage(memberName + " Set", "SET " + memberName + " - COM");

                        try
                        {
                            TL.LogMessage(memberName + " Set", "  " + parms[0].ToString());
                            //run the COM object property
                            GetObjType.InvokeMember(memberName,
                                                    BindingFlags.Default | BindingFlags.SetProperty,
                                                    null,
                                                    GetLateBoundObject,
                                                    parms,
                                                    CultureInfo.InvariantCulture);
                            return null;
                        }
                        catch (COMException e)
                        {
                            TL.LogMessageCrLf("COMException", e.ToString());
                            if (e.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber, CultureInfo.InvariantCulture))
                            {
                                TL.LogMessageCrLf(memberName + " Set", "  Throwing PropertyNotImplementedException: " + _strProgId + " " + memberName);
                                throw new PropertyNotImplementedException(_strProgId + " " + memberName, true, e);
                            }
                            TL.LogMessageCrLf(memberName + " Set", "  Re-throwing exception");
                            throw;
                        }
                        catch (TargetInvocationException e)
                        {
                            TL.LogMessageCrLf("TargetInvocationException", e.ToString());

                            if (e.InnerException is COMException)
                            {
                                string message = e.InnerException.Message;
                                int errorcode = ((COMException)e.InnerException).ErrorCode;
                                // Telescope simulator in V1 mode throws not implemented exceptions rather than some kind of missing member exception, so test for this!
                                if (errorcode == int.Parse("80040400", NumberStyles.HexNumber, CultureInfo.InvariantCulture))
                                {
                                    TL.LogMessageCrLf(memberName + " Set", "  Translating COM not implemented exception to PropertyNotImplementedException: " + _strProgId + " " + memberName);
                                    throw new PropertyNotImplementedException(_strProgId + " " + memberName, true, e);
                                }
                                else // Throw a new COM exception that looks like the original exception, containing the original inner COM exception for reference
                                {
                                    TL.LogMessageCrLf(memberName + " Set", "COM Exception so throwing inner exception: '" + message + "' '0x" + String.Format("{0:x8}", errorcode) + "'");
                                    throw new DriverAccessCOMException(message, errorcode, e);
                                }
                            }

                            SetTargetInvocationExceptionHandler(memberName, e);

                        }
                        catch (Exception e)
                        {
                            TL.LogMessageCrLf("Exception", e.ToString());
                            throw;
                        }
                    }

                    //evertyhing failed so throw an exception
                    TL.LogMessage("PropertySet", "  The object is neither a .NET object nor a COM object!");
                    throw new PropertyNotImplementedException(_strProgId + " " + memberName, true);

                case 3: // Method
                    TL.LogMessage(memberName, "Start");
                    /*foreach (Type t in parameterTypes)
                    {
                        TL.LogMessage(memberName, "  Parameter: " + t.FullName);
                    }*/

                    var methodInfo = GetObjType.GetMethod(memberName);
                    //, parameterTypes); //Peter: Had to take parameterTypes out to get CanMoveAxis to work with .NET drivers
                    if (methodInfo != null)
                    {
                        //TL.LogMessage(memberName, "  Got MethodInfo");

                        //ParameterInfo[] pars = methodInfo.GetParameters();
                        /*foreach (ParameterInfo p in pars)
                        {
                            TL.LogMessage(memberName, "    Parameter: " + p.ParameterType);
                        } */

                        try
                        {
                            foreach (object parm in parms)
                            {
                                TL.LogMessage(memberName, "  Parameter: " + parm.ToString());
                            }
                            TL.LogMessage(memberName, "  Calling " + memberName);
                            object result = methodInfo.Invoke(GetLateBoundObject, parms);

                            if (result == null) TL.LogMessage(memberName, "  Successfully called method, no return value");
                            else TL.LogMessage(memberName, "  " + result.ToString());

                            return result;
                        }
                        catch (TargetInvocationException e)
                        {
                            MethodTargetInvocationExceptionHandler(memberName, e);
                        }

                        // Unexpected exception so throw it all to the client
                        catch (Exception e)
                        {
                            TL.LogMessageCrLf("Exception", e.ToString());
                            throw;
                        }
                    }

                    //check the type to see if it's a COM object
                    if (IsComObject)
                    {
                        try
                        {
                            //run the COM object method
                            foreach (object parm in parms)
                            {
                                TL.LogMessage(memberName, "  Parameter: " + parm.ToString());
                            }
                            TL.LogMessage(memberName, "  Calling " + memberName + " - it is a COM object");
                            object result = GetObjType.InvokeMember(memberName,
                                                           BindingFlags.Default | BindingFlags.InvokeMethod,
                                                           null,
                                                           GetLateBoundObject,
                                                           parms,
                                                           CultureInfo.InvariantCulture);

                            if (result == null) TL.LogMessage(memberName, "  Successfully called method, no return value");
                            else TL.LogMessage(memberName, "  " + result.ToString());

                            return result;
                        }
                        catch (COMException e)
                        {
                            TL.LogMessageCrLf("COMException", e.ToString());
                            if (e.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber, CultureInfo.InvariantCulture))
                            {
                                TL.LogMessageCrLf(memberName, "  Throwing MethodNotImplementedException: " + _strProgId + " " + memberName);
                                throw new MethodNotImplementedException(_strProgId + " " + memberName);
                            }
                            TL.LogMessageCrLf(memberName, "Re-throwing exception");
                            throw;
                        }
                        catch (TargetInvocationException e)
                        {
                            TL.LogMessageCrLf("TargetInvocationException", e.ToString());
                            if (e.InnerException is COMException)
                            {
                                string message = e.InnerException.Message;
                                int errorcode = ((COMException)e.InnerException).ErrorCode;
                                // Telescope simulator in V1 mode throws not implemented exceptions rather than some kind of missing member exception, so test for this!
                                if (errorcode == int.Parse("80040400", NumberStyles.HexNumber, CultureInfo.InvariantCulture))
                                {
                                    TL.LogMessageCrLf(memberName, "  Translating COM not implemented exception to MethodNotImplementedException: " + _strProgId + " " + memberName);
                                    throw new MethodNotImplementedException(_strProgId + " " + memberName, e);
                                }
                                else // Throw a new COM exception that looks like the original exception, containing the original inner COM exception for reference
                                {
                                    TL.LogMessageCrLf(memberName, "  COM Exception so throwing inner exception: '" + message + "' '0x" + String.Format("{0:x8}", errorcode) + "'");
                                    throw new DriverAccessCOMException(message, errorcode, e);
                                }
                            }

                            MethodTargetInvocationExceptionHandler(memberName, e);
                        }
                        catch (Exception e)
                        {
                            TL.LogMessageCrLf("Exception", e.ToString());
                            throw;
                        }
                    }

                    TL.LogMessage(memberName, "  is neither a .NET object nor a COM object!");
                    throw new MethodNotImplementedException(_strProgId + " " + memberName);

                default:
                    return null;
            }
        }

        /// <summary>
        /// Checks for ASCOM exceptions returned as inner exceptions of TargetInvocationException. When new ASCOM exceptions are created 
        /// they must be added to this method. They will then be used in all three cases of Property Get, Property Set and Method call. 
        /// </summary>
        /// <param name="memberName">The name of the invoked member</param>
        /// <param name="e">The thrown TargetInvocationException including the inner exception</param>
        private void CheckDotNetExceptions(string memberName, Exception e)
        {
            string FullName = e.InnerException.GetType().FullName;
            string message = "";
            string member;
            string value;
            string range;

            //Throw the appropriate exception based on the inner exception of the TargetInvocationException
                if(e.InnerException is InvalidOperationException)
                {
                    message = e.InnerException.Message;
                    TL.LogMessageCrLf(memberName, "  Throwing InvalidOperationException: '" + message + "'");
                    throw new InvalidOperationException(message, e.InnerException);
                }

                if (e.InnerException is InvalidValueException)
                {
                    member = (string)e.InnerException.GetType().InvokeMember("PropertyOrMethod", BindingFlags.Default | BindingFlags.GetProperty, null, e.InnerException, new object[] { }, CultureInfo.InvariantCulture);
                    value = (string)e.InnerException.GetType().InvokeMember("Value", BindingFlags.Default | BindingFlags.GetProperty, null, e.InnerException, new object[] { }, CultureInfo.InvariantCulture);
                    range = (string)e.InnerException.GetType().InvokeMember("Range", BindingFlags.Default | BindingFlags.GetProperty, null, e.InnerException, new object[] { }, CultureInfo.InvariantCulture);

                    TL.LogMessageCrLf(memberName, "  Throwing InvalidValueException: '" + member + "' '" + value + "' '" + range + "'");
                    throw new InvalidValueException(member, value, range, e.InnerException);
                }

                if (e.InnerException is NotConnectedException)
                {
                    message = e.InnerException.Message;
                    TL.LogMessageCrLf(memberName, "  Throwing NotConnectedException: '" + message + "'");
                    throw new NotConnectedException(message, e.InnerException);
                }

                if (e.InnerException is ASCOM.NotImplementedException)
                {
                    member = (string)e.InnerException.GetType().InvokeMember("PropertyOrMethod", BindingFlags.Default | BindingFlags.GetProperty, null, e.InnerException, new object[] { }, CultureInfo.InvariantCulture);
                    TL.LogMessageCrLf(memberName, "  Throwing NotImplementedException: '" + member + "'");
                    throw new NotImplementedException(member, e.InnerException);
                }

                if (e.InnerException is ParkedException)
                {
                    message = e.InnerException.Message;

                    TL.LogMessageCrLf(memberName, "  Throwing ParkedException: '" + message + "'");
                    throw new ParkedException(message, e.InnerException);
                }

                if (e.InnerException is SlavedException)
            {
                    message = e.InnerException.Message;

                    TL.LogMessageCrLf(memberName, "  Throwing SlavedException: '" + message + "'");
                    throw new SlavedException(message, e.InnerException);
            }

                if (e.InnerException is ValueNotSetException)
            {
                    member = (string)e.InnerException.GetType().InvokeMember("PropertyOrMethod", BindingFlags.Default | BindingFlags.GetProperty, null, e.InnerException, new object[] { }, CultureInfo.InvariantCulture);

                    TL.LogMessageCrLf(memberName, "  Throwing ValueNotSetException: '" + member + "'");
                    throw new ValueNotSetException(member, e.InnerException);
            }

                if (e.InnerException is DriverException)
            {
                    message = e.InnerException.Message;
                    int number = (int)e.InnerException.GetType().InvokeMember("Number", BindingFlags.Default | BindingFlags.GetProperty, null, e.InnerException, new object[] { }, CultureInfo.InvariantCulture);

                    TL.LogMessageCrLf(memberName, "  Throwing DriverException: '" + message + "' '" + number + "'");
                    throw new DriverException(message, number, e.InnerException);
            }

                // Default behaviour if its not one of the exceptions above
            string defaultmessage = "CheckDotNetExceptions " + _strProgId + " " + memberName + " " + e.InnerException.ToString() + " (See Inner Exception for details)";

            TL.LogMessageCrLf(memberName, "  Throwing Default DriverException: '" + defaultmessage + "'");
            throw new DriverException(defaultmessage, e.InnerException);
        }

        private void SetTargetInvocationExceptionHandler(string memberName, Exception e)
        {
            // Check unique exceptions for this case
            TL.LogMessageCrLf("TargetInvocationException", "Set " + memberName + " " + e.ToString());

            if (e.InnerException.GetType().FullName == "ASCOM.PropertyNotImplementedException")
            {
                TL.LogMessageCrLf("TargetInvocationException", "Set " + memberName + " Found PropertyNotImplementedException");
                string method = (string)e.InnerException.GetType().InvokeMember("Property", BindingFlags.Default | BindingFlags.GetProperty, null, e.InnerException, new object[] { }, CultureInfo.InvariantCulture);
                bool accessorset = (bool)e.InnerException.GetType().InvokeMember("AccessorSet", BindingFlags.Default | BindingFlags.GetProperty, null, e.InnerException, new object[] { }, CultureInfo.InvariantCulture);
                TL.LogMessageCrLf(memberName + " Set", "  Throwing PropertyNotImplementedException: '" + method + "' '" + accessorset + "'");
                throw new PropertyNotImplementedException(method, accessorset, e.InnerException);
            }

            CheckDotNetExceptions(memberName + "Set", e); // Common handling for ASCOM exceptions so they only have to be coded in one place
        }

        private void GetTargetInvocationExceptionHandler(string memberName, Exception e)
        {
            // Check unique exceptions for this case
            TL.LogMessageCrLf("TargetInvocationException", "Get " + e.ToString());

            if (e.InnerException.GetType().FullName == "ASCOM.PropertyNotImplementedException")
            {
                TL.LogMessageCrLf("TargetInvocationException", "Get " + memberName + " Found PropertyNotImplementedException");
                string method = (string)e.InnerException.GetType().InvokeMember("Property", BindingFlags.Default | BindingFlags.GetProperty, null, e.InnerException, new object[] { }, CultureInfo.InvariantCulture);
                bool accessorset = (bool)e.InnerException.GetType().InvokeMember("AccessorSet", BindingFlags.Default | BindingFlags.GetProperty, null, e.InnerException, new object[] { }, CultureInfo.InvariantCulture);
                TL.LogMessageCrLf(memberName + " Get", "  Throwing PropertyNotImplementedException: '" + method + "' '" + accessorset + "'");
                throw new PropertyNotImplementedException(method, accessorset, e.InnerException);
            }

            CheckDotNetExceptions(memberName + " Get", e); // Common handling for ASCOM exceptions so they only have to be coded in one place
        }

        private void MethodTargetInvocationExceptionHandler(string memberName, Exception e)
        {
            TL.LogMessageCrLf("TargetInvocationException", e.ToString());

            // Check unique exceptions for this case
            if (e.InnerException.GetType().FullName == "ASCOM.MethodNotImplementedException")
            {
                string method = (string)e.InnerException.GetType().InvokeMember("Method", BindingFlags.Default | BindingFlags.GetProperty, null, e.InnerException, new object[] { }, CultureInfo.InvariantCulture);
                TL.LogMessageCrLf(memberName, "  Throwing MethodNotImplementedException :'" + method + "'");
                throw new MethodNotImplementedException(method, e.InnerException);
            }
            CheckDotNetExceptions(memberName, e); // Common handling for ASCOM exceptions so they only have to be coded in one place
        }

        #endregion
    }

}