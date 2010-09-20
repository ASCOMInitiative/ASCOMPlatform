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
        internal MemberFactory(string progId)
        {
            _strProgId = progId;
            GetInterfaces = new List<Type>();

            // Get Type Information 
            GetObjType = Type.GetTypeFromProgID(progId);

            //check to see if it found the type information
            if (GetObjType == null)
            {
                //no type information found throw error
                throw new Exception("Check Driver: cannot create object type of progID: " + _strProgId);
            }
            _tl = new TraceLogger("", "MemberFactory") {Enabled = true};

            //setup the property
            IsComObject = GetObjType.IsCOMObject;

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
                                       "  " + pi.Name + " " + pi.ParameterType.Name + " " +
                                       pi.ParameterType.AssemblyQualifiedName);
                    }
                }
            }

            //no instance found throw error
            if (GetLateBoundObject == null)
            {
                throw new Exception("Check Driver: cannot create driver isntance of progID: " + _strProgId);
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
            if (GetLateBoundObject == null) return;
            var releaseComObject = Marshal.ReleaseComObject(GetLateBoundObject);
            if (releaseComObject == 0 )GetLateBoundObject = null;
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
            switch (memberCode)
            {
                case 1:
                    _tl.LogMessage("PropertyGet", memberName);

                    PropertyInfo propertyGetInfo = GetObjType.GetProperty(memberName);
                    if (propertyGetInfo != null)
                    {
                        _tl.LogMessage("PropertyGet", "propertyGetInfo is not null");
                        try
                        {
                            //run the .net object
                            return propertyGetInfo.GetValue(GetLateBoundObject, null);
                        }
                        catch (TargetInvocationException e)
                        {
                            _tl.LogMessage("PropertyGetEx1", e.ToString());
                            if (e.InnerException.GetType() == typeof (PropertyNotImplementedException))
                            {
                                throw new PropertyNotImplementedException(
                                    memberName + " is not implemented in this driver", true, e.InnerException);
                            }
                            if (e.InnerException.GetType() == typeof (InvalidValueException))
                            {
                                throw new InvalidValueException(e.InnerException.Message, "", "", e.InnerException);
                            }
                            if (e.InnerException.GetType() == typeof (DriverException))
                            {
                                throw new DriverException(e.InnerException.Message, ((DriverException)e.InnerException).Number, e.InnerException);
                            }
                            throw e.InnerException;
                        }
                        catch (Exception e)
                        {
                            _tl.LogMessage("PropertyGetEx2", e.ToString());
                            throw;
                        }
                    }
                    _tl.LogMessage("PropertyGet", "propertyGetInfo is null");
                    //check the type to see if it's a COM object
                    if (IsComObject)
                    {
                        _tl.LogMessage("PropertyGet", "propertyGetInfo is COM Object");

                        try
                        {
                            //run the COM object property
                            return
                                (GetObjType.InvokeMember(memberName, BindingFlags.Default | BindingFlags.GetProperty,
                                                       null, GetLateBoundObject, new object[] {}));
                        }
                        catch (COMException e)
                        {
                            _tl.LogMessage("PropertyGetEx3", e.ToString());
                            if (e.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber))
                                throw new PropertyNotImplementedException(_strProgId + " " + memberName, false);
                            throw;
                        }
                        catch (Exception e)
                        {
                            _tl.LogMessage("PropertyGetEx4", e.ToString());

                            throw e.InnerException;
                        }
                    }
                    //evertyhing failed so throw an exception
                    _tl.LogMessage("PropertyGet", "propertyGetInfo is .NET object");
                    throw new PropertyNotImplementedException(_strProgId + " " + memberName, false);
                case 2:
                    _tl.LogMessage("PropertySet", memberName);
                    PropertyInfo propertySetInfo = GetObjType.GetProperty(memberName);
                    if (propertySetInfo != null)
                    {
                        _tl.LogMessage("PropertySet", "propertyGetInfo is not null");
                        try
                        {
                            propertySetInfo.SetValue(GetLateBoundObject, parms[0], null);
                            return null;
                        }
                        catch (TargetInvocationException e)
                        {
                            _tl.LogMessage("PropertySetEx1", e.ToString());
                            if (e.InnerException.GetType() == typeof (PropertyNotImplementedException))
                            {
                                throw new PropertyNotImplementedException(
                                    memberName + " is not implemented in this driver", true, e.InnerException);
                            }
                            if (e.InnerException.GetType() == typeof (DriverException))
                            {
                                throw new DriverException(e.InnerException.Message, ((DriverException)e.InnerException).Number, e.InnerException);
                            }
                            if (e.InnerException.GetType() == typeof (InvalidValueException))
                            {
                                throw e.InnerException;
                            }
                            throw;
                        }
                        catch (Exception e)
                        {
                            _tl.LogMessage("PropertySetEx1", e.ToString());
                            throw;
                        }
                    }
                    _tl.LogMessage("PropertySet", "propertyGetInfo is null");
                    //check the type to see if it's a COM object
                    if (IsComObject)
                    {
                        _tl.LogMessage("PropertySet", "propertyGetInfo is COM Object");
                        try
                        {
                            //run the COM object property
                            GetObjType.InvokeMember(memberName, BindingFlags.Default | BindingFlags.SetProperty, null,
                                                  GetLateBoundObject, parms);
                            return null;
                        }
                        catch (COMException e)
                        {
                            _tl.LogMessage("PropertySetEx3", e.ToString());
                            if (e.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber))
                                throw new PropertyNotImplementedException(_strProgId + " " + memberName, true);
                            throw;
                        }
                        catch (Exception e)
                        {
                            _tl.LogMessage("PropertySetEx4", e.ToString());
                            throw e.InnerException;
                        }
                    }
                    _tl.LogMessage("PropertySet", "propertyGetInfo is .NET object");
                    throw new PropertyNotImplementedException(_strProgId + " " + memberName, true);
                case 3:
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
                            _tl.LogMessage(memberName,
                                          "    AssemblyQualifiedName: " + p.ParameterType.AssemblyQualifiedName);
                            _tl.LogMessage(memberName,
                                          "    AssemblyQualifiedName: " + parameterTypes[0].AssemblyQualifiedName);
                            _tl.LogMessage(memberName, "    FullName: " + p.ParameterType.FullName);
                            _tl.LogMessage(memberName, "    FullName: " + parameterTypes[0].FullName);
                            _tl.LogMessage(memberName, "    AssemblyFullName: " + p.ParameterType.Assembly.FullName);
                            _tl.LogMessage(memberName, "    AssemblyFullName: " + parameterTypes[0].Assembly.FullName);
                            _tl.LogMessage(memberName, "    AssemblyCodeBase: " + p.ParameterType.Assembly.CodeBase);
                            _tl.LogMessage(memberName, "    AssemblyCodeBase: " + parameterTypes[0].Assembly.CodeBase);
                            _tl.LogMessage(memberName, "    AssemblyLocation: " + p.ParameterType.Assembly.Location);
                            _tl.LogMessage(memberName, "    AssemblyLocation: " + parameterTypes[0].Assembly.Location);
                            _tl.LogMessage(memberName,
                                          "    AssemblyGlobalAssemblyCache: " +
                                          p.ParameterType.Assembly.GlobalAssemblyCache);
                            _tl.LogMessage(memberName,
                                          "    AssemblyGlobalAssemblyCache: " +
                                          parameterTypes[0].Assembly.GlobalAssemblyCache);
                        }


                        try
                        {
                            object result = methodInfo.Invoke(GetLateBoundObject, parms);
                            _tl.LogMessage(memberName, "  Successfully called method");
                            return result;
                        }
                        catch (TargetInvocationException e)
                        {
                            _tl.LogMessage(memberName, "  ***** TargetInvocationException: " + e);
                            if (e.InnerException is DriverException)
                            {
                                throw e.InnerException;
                            }
                            throw new MethodNotImplementedException(_strProgId + " " + memberName,
                                                                    e.InnerException);
                        }
                        catch (Exception e)
                        {
                            _tl.LogMessage(memberName, "  ***** Exception: " + e);
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
                            return GetObjType.InvokeMember(memberName, BindingFlags.Default | BindingFlags.InvokeMethod,
                                                         null, GetLateBoundObject, parms);
                        }
                        catch (COMException e)
                        {
                            if (e.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber))
                                throw new MethodNotImplementedException(_strProgId + " " + memberName);
                            throw;
                        }
                        catch (Exception e)
                        {
                            throw e.InnerException;
                        }
                    }
                    _tl.LogMessage(memberName, "  It is NOT a COM object");
                    throw new MethodNotImplementedException(_strProgId + " " + memberName);
                default:
                    return null;
            }
        }

        #endregion
    }
}