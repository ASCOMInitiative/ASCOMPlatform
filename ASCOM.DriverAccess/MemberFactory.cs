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

        private TraceLogger _tl;
        private readonly String _strProgId;

        /// <summary>
        /// Constructor, creates an instance of the of the ASCOM driver
        /// 
        /// </summary> 
        /// <param name="progId">The program ID of the driver</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        internal MemberFactory(string progId)
        {
            _tl = new TraceLogger("", "MemberFactory");
            _tl.Enabled = true;
            _tl.LogMessage("ProgID", progId);

            _strProgId = progId;
            GetInterfaces = new List<Type>();

            // Get Type Information 
            GetObjType = Type.GetTypeFromProgID(progId);

            //check to see if it found the type information
            if (GetObjType == null)
            {
                //no type information found throw error
                throw new  ASCOM.Utilities.Exceptions.HelperException("Check Driver: cannot create object type of progID: " + _strProgId);
            }

            //setup the property
            IsComObject = GetObjType.IsCOMObject;
            _tl.LogMessage("IsComObject", GetObjType.IsCOMObject.ToString());

            // Create an instance of the object
            GetLateBoundObject = Activator.CreateInstance(GetObjType);

            // Get list of interfaces
            var objInterfaces = GetObjType.GetInterfaces();

            foreach (Type objInterface in objInterfaces)
            {
                GetInterfaces.Add(objInterface);
                _tl.LogMessage("Interface", objInterface.AssemblyQualifiedName);
            }

            MemberInfo[] members = GetObjType.GetMembers();
            foreach (MemberInfo mi in members)
            {
                _tl.LogMessage("Member", Enum.GetName(typeof (MemberTypes), mi.MemberType) + " " + mi.Name);
                if (mi.MemberType == MemberTypes.Method)
                {
                    foreach (ParameterInfo pi in ((MethodInfo) mi).GetParameters())
                    {
                        _tl.LogMessage("Parameter",
                                       " " + pi.Name + 
                                       " " + pi.ParameterType.Name + 
                                       " " + pi.ParameterType.AssemblyQualifiedName);
                    }
                }
            }

            //no instance found throw error
            if (GetLateBoundObject == null)
            {
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
                            _tl.LogMessageCrLf("Dispose COM", "This is a COM object so attempting to call its Dispose method");
                            GetObjType.InvokeMember("Dispose",
                                                    BindingFlags.Default | BindingFlags.InvokeMethod,
                                                    null,
                                                    GetLateBoundObject,
                                                    new object[] { },
                                                    CultureInfo.InvariantCulture);
                            _tl.LogMessageCrLf("Dispose COM", "Successfully called its Dispose method");
                        }
                        catch (Exception ex)
                        {
                            _tl.LogMessageCrLf("Dispose COM", "Exception " + ex.ToString());
                        }

                        _tl.LogMessageCrLf("Dispose COM", "This is a COM object so attempting to release it");
                        var releaseComObject = Marshal.ReleaseComObject(GetLateBoundObject);
                        if (releaseComObject == 0) GetLateBoundObject = null;
                        _tl.LogMessageCrLf("Dispose COM", "Object Count is now: " + releaseComObject);
                    }
                    else // Should be a .NET object so lets dispose of it
                    {
                        _tl.LogMessageCrLf("Dispose .NET", "This is a .NET object so attempting to call its Dispose method");
                        var methodInfo = GetObjType.GetMethod("Dispose");

                        if (methodInfo != null)
                        {
                            _tl.LogMessage("Dispose .NET", "  Got Dispose Method Info, Calling Dispose");
                            object result = methodInfo.Invoke(GetLateBoundObject, new object[] { });
                            _tl.LogMessage("Dispose .NET", "  Successfully called Dispose method");
                        }
                        else
                        {
                            _tl.LogMessage("Dispose .NET", "  No Dispose Method Info so ignoring the call to Dispose");
                        }
                    }
                }
                catch (Exception ex)
                
                {
                    try
                    {
                        _tl.LogMessageCrLf("Dispose", "Exception " + ex.ToString());
                    }
                    catch { }
                } // Ignore any errors here as we are disposing


                if (_tl != null) _tl.Dispose();
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
            _tl.BlankLine();
            switch (memberCode)
            {
                case 1: // Property Read
                    _tl.LogMessage(memberName + " Get", "GET " + memberName);

                    PropertyInfo propertyGetInfo = GetObjType.GetProperty(memberName);
                    if (propertyGetInfo != null) // We have a .NET object
                    {
                        _tl.LogMessage(memberName + " Get", "  This is a .NET object");
                        try
                        {
                            //run the .net object
                            return propertyGetInfo.GetValue(GetLateBoundObject, null);
                        }
                        catch (TargetInvocationException e)
                        {
                            GetTargetInvocationExceptionHandler(memberName, e);
                        }
                        catch (Exception e)
                        {
                            _tl.LogMessageCrLf("Exception", e.ToString());
                            throw;
                        }
                    }
                    _tl.LogMessage(memberName + " Get", "  This is not a .NET object");

                    //check the type to see if it's a COM object
                    if (IsComObject) // We have a COM object
                    {
                        _tl.LogMessage(memberName + " Get", "  This is a COM Object");

                        try
                        {
                            //run the COM object property
                            return
                                (GetObjType.InvokeMember(memberName, 
                                                         BindingFlags.Default | BindingFlags.GetProperty,
                                                         null, 
                                                         GetLateBoundObject, 
                                                         new object[] { }, 
                                                         CultureInfo.InvariantCulture));
                        }
                        catch (COMException e)
                        {
                            _tl.LogMessageCrLf("COMException", e.ToString());
                            if (e.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber, CultureInfo.InvariantCulture))
                            {
                                _tl.LogMessageCrLf(memberName + " Get", "  Throwing PropertyNotImplementedException: " + _strProgId + " " + memberName);
                                throw new PropertyNotImplementedException(_strProgId + " " + memberName, false);
                            }
                            _tl.LogMessageCrLf(memberName + " Get", "Re-throwing exception");
                            throw;
                        }
                        catch (TargetInvocationException e)
                        {
                            _tl.LogMessageCrLf("TargetInvocationException", e.ToString());
                            if (e.InnerException is COMException)
                            {
                                string message = e.InnerException.Message;
                                int errorcode = ((COMException)e.InnerException).ErrorCode;
                                _tl.LogMessageCrLf(memberName + " Get", "COM Exception so throwing inner exception: '" + message + "' '" + String.Format("{0:x8}", errorcode) + "'");
                                throw new DriverAccessCOMException(message, errorcode, e.InnerException);
                            }

                            GetTargetInvocationExceptionHandler(memberName, e);
                        }

                        catch (Exception e)
                        {
                            _tl.LogMessageCrLf("Exception", e.ToString());
                            throw;
                        }
                    }

                    //evertyhing failed so throw an exception
                    _tl.LogMessage(memberName + " Get", "  The object is neither a .NET object nor a COM object!");
                    throw new PropertyNotImplementedException(_strProgId + " " + memberName, false);

                case 2: // Property Write
                    _tl.LogMessage(memberName + " Set", "SET " + memberName);

                    PropertyInfo propertySetInfo = GetObjType.GetProperty(memberName);
                    if (propertySetInfo != null) // We have a .NET object
                    {
                        _tl.LogMessage(memberName + " Set", "  This is a .NET object");
                        try
                        {
                            propertySetInfo.SetValue(GetLateBoundObject, parms[0], null);
                            return null;
                        }
                        catch (TargetInvocationException e)
                        {
                            SetTargetInvocationExceptionHandler(memberName, e);
                        }
                        catch (Exception e)
                        {
                            _tl.LogMessageCrLf("Exception", e.ToString());
                            throw;
                        }
                    }
                    _tl.LogMessage(memberName + " Set", "  This is not a .NET object");

                    //check the type to see if it's a COM object
                    if (IsComObject)
                    {
                        _tl.LogMessage(memberName + " Set", "  This is a COM Object");

                        try
                        {
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
                            _tl.LogMessageCrLf("COMException", e.ToString());
                            if (e.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber, CultureInfo.InvariantCulture))
                            {
                                _tl.LogMessageCrLf(memberName + " Set", "  Throwing PropertyNotImplementedException: " + _strProgId + " " + memberName);
                                throw new PropertyNotImplementedException(_strProgId + " " + memberName, true);
                            }
                            _tl.LogMessageCrLf(memberName + " Set", "  Re-throwing exception");
                            throw;
                        }
                        catch (TargetInvocationException e)
                        {
                            _tl.LogMessageCrLf("TargetInvocationException", e.ToString());

                            if (e.InnerException is COMException)
                            {
                                string message = e.InnerException.Message;
                                int errorcode = ((COMException)e.InnerException).ErrorCode;
                                _tl.LogMessageCrLf(memberName + " Set", "COM Exception so throwing inner exception: '" + message + "' '" + String.Format("{0:x8}", errorcode) + "'");
                                throw new DriverAccessCOMException(message, errorcode, e.InnerException);
                            }

                            SetTargetInvocationExceptionHandler(memberName, e);

                        }
                        catch (Exception e)
                        {
                            _tl.LogMessageCrLf("Exception", e.ToString());
                            throw;
                        }
                    }

                    //evertyhing failed so throw an exception
                    _tl.LogMessage("PropertySet", "  The object is neither a .NET object nor a COM object!");
                    throw new PropertyNotImplementedException(_strProgId + " " + memberName, true);

                case 3: // Method
                    _tl.LogMessage(memberName, "Start");
                    foreach (Type t in parameterTypes)
                    {
                        _tl.LogMessage(memberName, "  Parameter: " + t.FullName);
                    }

                    var methodInfo = GetObjType.GetMethod(memberName);
                    //, parameterTypes); //Peter: Had to take parameterTypes out to get CanMoveAxis to work with .NET drivers
                    if (methodInfo != null)
                    {
                        _tl.LogMessage(memberName, "  Got MethodInfo");

                        ParameterInfo[] pars = methodInfo.GetParameters();
                        foreach (ParameterInfo p in pars)
                        {
                            _tl.LogMessage(memberName, "  Parameter: " + p.ParameterType);
                            _tl.LogMessage(memberName, "    AssemblyQualifiedName: " + p.ParameterType.AssemblyQualifiedName);
                            _tl.LogMessage(memberName, "    AssemblyQualifiedName: " + parameterTypes[0].AssemblyQualifiedName);
                            _tl.LogMessage(memberName, "    FullName: " + p.ParameterType.FullName);
                            _tl.LogMessage(memberName, "    FullName: " + parameterTypes[0].FullName);
                            _tl.LogMessage(memberName, "    AssemblyFullName: " + p.ParameterType.Assembly.FullName);
                            _tl.LogMessage(memberName, "    AssemblyFullName: " + parameterTypes[0].Assembly.FullName);
                            _tl.LogMessage(memberName, "    AssemblyCodeBase: " + p.ParameterType.Assembly.CodeBase);
                            _tl.LogMessage(memberName, "    AssemblyCodeBase: " + parameterTypes[0].Assembly.CodeBase);
                            _tl.LogMessage(memberName, "    AssemblyLocation: " + p.ParameterType.Assembly.Location);
                            _tl.LogMessage(memberName, "    AssemblyLocation: " + parameterTypes[0].Assembly.Location);
                            _tl.LogMessage(memberName, "    AssemblyGlobalAssemblyCache: " + p.ParameterType.Assembly.GlobalAssemblyCache);
                            _tl.LogMessage(memberName, "    AssemblyGlobalAssemblyCache: " + parameterTypes[0].Assembly.GlobalAssemblyCache);
                        }

                        try
                        {
                            _tl.LogMessage(memberName, "  Calling " + memberName);
                            object result = methodInfo.Invoke(GetLateBoundObject, parms);
                            _tl.LogMessage(memberName, "  Successfully called method");
                            return result;
                        }
                        catch (TargetInvocationException e)
                        {
                            MethodTargetInvocationExceptionHandler(memberName, e);
                        }

                        // Unexpected exception so throw it all to the client
                        catch (Exception e)
                        {
                            _tl.LogMessageCrLf("Exception", e.ToString());
                            throw;
                        }
                    }
                    _tl.LogMessage(memberName, "  Didn't Get MethodInfo");
                    //check the type to see if it's a COM object
                    if (IsComObject)
                    {
                        _tl.LogMessage(memberName, "  It is a COM object");
                        try
                        {
                            //run the COM object method
                            return GetObjType.InvokeMember(memberName, 
                                                           BindingFlags.Default | BindingFlags.InvokeMethod,
                                                           null, 
                                                           GetLateBoundObject, 
                                                           parms, 
                                                           CultureInfo.InvariantCulture);
                        }
                        catch (COMException e)
                        {
                            _tl.LogMessageCrLf("COMException", e.ToString());
                            if (e.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber, CultureInfo.InvariantCulture))
                            {
                                _tl.LogMessageCrLf(memberName, "  Throwing MethodNotImplementedException: " + _strProgId + " " + memberName);
                                throw new MethodNotImplementedException(_strProgId + " " + memberName);
                            }
                            _tl.LogMessageCrLf(memberName, "Re-throwing exception");
                            throw;
                        }
                        catch (TargetInvocationException e)
                        {
                            _tl.LogMessageCrLf("TargetInvocationException", e.ToString());
                            if (e.InnerException is COMException)
                            {
                                string message = e.InnerException.Message;
                                int errorcode = ((COMException)e.InnerException).ErrorCode;
                                _tl.LogMessageCrLf(memberName, "COM Exception so throwing inner exception: '" + message + "' '" + String.Format("{0:x8}", errorcode) + "'");
                                throw new DriverAccessCOMException(message, errorcode, e.InnerException); 
                            }

                            MethodTargetInvocationExceptionHandler(memberName, e);
                        }
                        catch (Exception e)
                        {
                            _tl.LogMessageCrLf("Exception", e.ToString());
                            throw;
                        }
                    }

                    _tl.LogMessage(memberName, "  is neither a .NET object nor a COM object!");
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
            //Throw the appropriate exception based on the inner exception of the TargetInvocationException
            if (e.InnerException is InvalidOperationException) 
            {
                string message = e.InnerException.Message;

                _tl.LogMessageCrLf(memberName, "  Throwing InvalidOperationException: '" + message + "'");
                throw new InvalidOperationException(message, e.InnerException); 
            }

            if (e.InnerException is InvalidValueException)
            {
                string member = ((InvalidValueException)e.InnerException).PropertyOrMethod;
                string value = ((InvalidValueException)e.InnerException).Value;
                string range = ((InvalidValueException)e.InnerException).Range;

                _tl.LogMessageCrLf(memberName, "  Throwing InvalidValueException: '" + member + "' '" + value + "' '" + range + "'");
                throw new InvalidValueException(member,value,range,e.InnerException);
            }
            if (e.InnerException is NotConnectedException) 
            {
                string message = e.InnerException.Message;

                _tl.LogMessageCrLf(memberName, "  Throwing NotConnectedException: '" + message + "'");
                throw new NotConnectedException(message, e.InnerException); 
            }

            if (e.InnerException is NotImplementedException)
            {
                string member = ((NotImplementedException)e.InnerException).PropertyOrMethod;

                _tl.LogMessageCrLf(memberName, "  Throwing NotImplementedException: '" + member + "'");
                throw new NotImplementedException(member, e.InnerException);
            }

            if (e.InnerException is ParkedException) 
            {
                string message = e.InnerException.Message;

                _tl.LogMessageCrLf(memberName, "  Throwing ParkedException: '" + message + "'");
                throw new ParkedException(message, e.InnerException); 
            }

            if (e.InnerException is SlavedException) 
            {
                string message = e.InnerException.Message;

                _tl.LogMessageCrLf(memberName, "  Throwing SlavedException: '" + message + "'");
                throw new SlavedException(message, e.InnerException); 
            }

            if (e.InnerException is ValueNotSetException)
            {
                string member = ((NotImplementedException)e.InnerException).PropertyOrMethod;

                _tl.LogMessageCrLf(memberName, "  Throwing ValueNotSetException: '" + member + "'");
                throw new ValueNotSetException(member, e.InnerException);
            }
            if (e.InnerException is DriverException)
            {
                string message = e.InnerException.Message;
                int number = ((DriverException)e.InnerException).Number;

                _tl.LogMessageCrLf(memberName, "  Throwing DriverException: '" + message + "' '" + number + "'");
                throw new DriverException(message, number, e.InnerException);
            }

            // Default behaviour if its not one of the exceptions above
            string defaultmessage = "CheckDotNetExceptions " +  _strProgId + " " + memberName + " " +e.InnerException.ToString() + " (See Inner Exception for details)";

            _tl.LogMessageCrLf(memberName, "  Throwing Default DriverException: '" + defaultmessage + "'");
            throw new DriverException(defaultmessage, e.InnerException);
        }

        private void SetTargetInvocationExceptionHandler(string memberName, Exception e)
        {
            // Check unique exceptions for this case
            _tl.LogMessageCrLf("TargetInvocationException", e.ToString());
            if (e.InnerException is PropertyNotImplementedException)
            {
                string method = ((PropertyNotImplementedException)e.InnerException).PropertyOrMethod;
                bool accessorset = ((PropertyNotImplementedException)e.InnerException).AccessorSet;
                _tl.LogMessageCrLf(memberName + " Set", "  Throwing PropertyNotImplementedException: '" + method + "' '" + accessorset + "'");
                throw new PropertyNotImplementedException(method, accessorset, e.InnerException);
            }
            CheckDotNetExceptions(memberName + "Set", e); // Common handling for ASCOM exceptions so they only have to be coded in one place
        }

        private void GetTargetInvocationExceptionHandler(string memberName, Exception e)
        {                            
            // Check unique exceptions for this case
            _tl.LogMessageCrLf("TargetInvocationException", e.ToString());
            if (e.InnerException is PropertyNotImplementedException)
            {
                string method = ((PropertyNotImplementedException)e.InnerException).PropertyOrMethod;
                bool accessorset = ((PropertyNotImplementedException)e.InnerException).AccessorSet;
                _tl.LogMessageCrLf(memberName + " Get", "  Throwing PropertyNotImplementedException: '" + method + "' '" + accessorset + "'");
                throw new PropertyNotImplementedException(method, accessorset, e.InnerException);
            }
            CheckDotNetExceptions(memberName + " Get", e); // Common handling for ASCOM exceptions so they only have to be coded in one place
        }

        private void MethodTargetInvocationExceptionHandler(string memberName, Exception e)
        {
            _tl.LogMessageCrLf("TargetInvocationException", e.ToString());

            // Check unique exceptions for this case
            if (e.InnerException is MethodNotImplementedException)
            {
                string method = ((MethodNotImplementedException)e.InnerException).Method;
                _tl.LogMessageCrLf(memberName, "  Throwing MethodNotImplementedException :'" + method + "'");
                throw new MethodNotImplementedException(method, e.InnerException);
            }
            CheckDotNetExceptions(memberName, e); // Common handling for ASCOM exceptions so they only have to be coded in one place
        }

        #endregion
    }
    
}