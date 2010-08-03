//-----------------------------------------------------------------------
// <summary>Defines the MemberFactory class.</summary>
//-----------------------------------------------------------------------
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Globalization;
using ASCOM;
using ASCOM.Utilities;
using System.Collections.Generic;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// A factory class to access any registered driver members
    /// </summary>
    public class MemberFactory: IDisposable
    {
        #region MemberFactory

        private object objLateBound;
        private Type objType;
        private String strProgID;
        private bool isCOMObject;
        private TraceLogger TL ;
        private List<Type> objInterfaceList;


        /// <summary>
        /// Constructor, creates an instance of the of the ASCOM driver
        /// 
        /// </summary> 
        /// <param name="progID">The program ID of the driver</param>
        internal MemberFactory(string progID)
        {
            strProgID = progID;
            objInterfaceList=new List<Type>();

            // Get Type Information 
            objType = Type.GetTypeFromProgID(progID);

            //check to see if it found the type information
            if (objType != null)
            {
                TL = new TraceLogger("","MemberFactory");
                TL.Enabled=true;

                //setup the property
                isCOMObject = objType.IsCOMObject;

                // Create an instance of the object
                objLateBound = Activator.CreateInstance(objType);

                // Get list of interfaces
                Type[] objInterfaces = objType.GetInterfaces();

                foreach (Type objInterface in objInterfaces)
                {
                        objInterfaceList.Add(objInterface);
                        TL.LogMessage("Interface", objInterface.AssemblyQualifiedName.ToString());
                }

                MemberInfo[] Members = objType.GetMembers();
                foreach (MemberInfo mi in Members)
                {
                    TL.LogMessage("Member", Enum.GetName(typeof(MemberTypes), mi.MemberType) + " " + mi.Name);
                    if (mi.MemberType == MemberTypes.Method)
                    {
                        foreach (ParameterInfo pi in ((MethodInfo) mi).GetParameters())
                        {
                            TL.LogMessage("Parameter", "  " + pi.Name + " " + pi.ParameterType.Name + " " + pi.ParameterType.AssemblyQualifiedName );
                        }
                    }

                }
                
                //no instance found throw error
                if (objLateBound == null)
                {
                    throw new Exception("Check Driver: cannot create driver isntance of progID: " + strProgID);
                }
            }
            else
            {
                //no type information found throw error
                throw new Exception("Check Driver: cannot create object type of progID: " + strProgID);
            } 
        }

        /// <summary>
        /// Returns the instance of the driver
        /// </summary> 
        /// <returns>object</returns>
        internal object GetLateBoundObject
        {
            get { return this.objLateBound ;}
        }

        /// <summary>
        /// Returns true is the driver is COM based
        /// </summary> 
        /// <returns>object</returns>
        internal bool IsCOMObject
        {
            get { return isCOMObject; }
        }

        /// <summary>
        /// Returns the driver type
        /// </summary> 
        /// <returns>type</returns>
        internal Type GetObjType
        {
            get { return this.objType; }
        }

        internal List<Type> GetInterfaces
        {
            get { return this.objInterfaceList; }
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
        /// <returns>object</returns>
        internal object CallMember(int memberCode, string memberName, Type[] parameterTypes, params object[] parms)
        {
                switch (memberCode)
                {
                    case 1:
                        TL.LogMessage("PropertyGet", memberName);

                        PropertyInfo propertyGetInfo = objType.GetProperty(memberName);
                        if (propertyGetInfo != null)
                         {
                             TL.LogMessage("PropertyGet", "propertyGetInfo is not null");
                             try
                             {
                                 //run the .net object
                                 return propertyGetInfo.GetValue(objLateBound, null);
                             }
                             catch (TargetInvocationException e)
                             {
                                 TL.LogMessage("PropertyGetEx1", e.ToString());
                                 if (e.InnerException.GetType() == typeof(ASCOM.PropertyNotImplementedException))
                                 {
                                     throw new ASCOM.PropertyNotImplementedException(memberName + " is not implemented in this driver", true, e.InnerException);
                                 }
                                 else if (e.InnerException.GetType() == typeof(ASCOM.InvalidValueException))
                                 {
                                     throw new ASCOM.InvalidValueException(e.InnerException.Message,"","",e.InnerException);
                                 }
                                 else if (e.InnerException.GetType() == typeof(ASCOM.DriverException))
                                 {
                                     throw new ASCOM.DriverException(e.InnerException.Message, e.InnerException);
                                 }
                                 else
                                 {
                                     throw e.InnerException;
                                 }
                             }
                             catch (Exception e)
                             {
                                 TL.LogMessage("PropertyGetEx2", e.ToString());
                                 throw e;
                             }
                        }
                        else
                        {
                            TL.LogMessage("PropertyGet", "propertyGetInfo is null");
                            //check the type to see if it's a COM object
                            if (isCOMObject)
                            {
                                TL.LogMessage("PropertyGet", "propertyGetInfo is COM Object");

                                try
                                {
                                    //run the COM object property
                                    return (objType.InvokeMember(memberName, BindingFlags.Default | BindingFlags.GetProperty, null, objLateBound, new object[] { }));
                                }
                                catch (System.Runtime.InteropServices.COMException e)
                                {
                                    TL.LogMessage("PropertyGetEx3", e.ToString());
                                    propertyGetInfo = null;
                                    if (e.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber)) throw new ASCOM.PropertyNotImplementedException(strProgID + " " + memberName, false);
                                    else throw;
                                }
                                catch (Exception e)
                                {
                                    TL.LogMessage("PropertyGetEx4", e.ToString());

                                    throw e.InnerException;
                                }
                            }
                            else
                            {
                                //evertyhing failed so throw an exception
                                propertyGetInfo = null;
                                TL.LogMessage("PropertyGet", "propertyGetInfo is .NET object");
                                throw new ASCOM.PropertyNotImplementedException(strProgID + " " + memberName, false);
                            }
                        }
                    case 2:
                        TL.LogMessage("PropertySet", memberName);
                        PropertyInfo propertySetInfo = objType.GetProperty(memberName);
                        if (propertySetInfo != null)
                        {
                            TL.LogMessage("PropertySet", "propertyGetInfo is not null");
                            try
                            {
                                propertySetInfo.SetValue(objLateBound, parms[0], null);
                                return null;
                            }
                            catch (TargetInvocationException e)
                            {
                                TL.LogMessage("PropertySetEx1", e.ToString());
                                if (e.InnerException.GetType() == typeof(ASCOM.PropertyNotImplementedException))
                                {
                                    throw new ASCOM.PropertyNotImplementedException(memberName + " is not implemented in this driver", true, e.InnerException);
                                }
                                else if (e.InnerException.GetType() == typeof(ASCOM.DriverException))
                                {
                                    throw new ASCOM.DriverException(e.InnerException.Message, e.InnerException);
                                }
                                else if (e.InnerException.GetType() == typeof(ASCOM.InvalidValueException))
                                {
                                    throw e.InnerException;
                                }
                                else
                                {
                                    throw;
                                }




                            }
                            catch (Exception e)
                            {
                                TL.LogMessage("PropertySetEx1", e.ToString());
                                throw e;
                            }
                        }
                        else
                        {
                            TL.LogMessage("PropertySet", "propertyGetInfo is null");
                            //check the type to see if it's a COM object
                            if (isCOMObject)
                            {
                                TL.LogMessage("PropertySet", "propertyGetInfo is COM Object");
                                try
                                {
                                    //run the COM object property
                                    objType.InvokeMember(memberName, BindingFlags.Default | BindingFlags.SetProperty, null, objLateBound, parms);
                                    return null;
                                }
                                catch (System.Runtime.InteropServices.COMException e)
                                {
                                    TL.LogMessage("PropertySetEx3", e.ToString());
                                    propertySetInfo = null;
                                    if (e.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber)) throw new ASCOM.PropertyNotImplementedException(strProgID + " " + memberName, true);
                                    else throw;
                                }
                                catch (Exception e)
                                {
                                    TL.LogMessage("PropertySetEx4", e.ToString());
                                    throw e.InnerException;
                                }
                            }
                            else
                            {
                                propertySetInfo = null;
                                TL.LogMessage("PropertySet", "propertyGetInfo is .NET object");
                                throw new ASCOM.PropertyNotImplementedException(strProgID + " " + memberName, true);
                            }
                        }
                    case 3:
                        TL.LogMessage(memberName, "Start");
                        foreach (Type t in parameterTypes)
                        {
                            TL.LogMessage(memberName, "  Parameter: " + t.FullName);
                        }


                        MethodInfo methodInfo = objType.GetMethod(memberName);//, parameterTypes); //Peter: Had to take parameterTypes out to get CanMoveAxis to work with .NET drivers
                        if (methodInfo != null)
                        {
                            TL.LogMessage(memberName, "  Got MethodInfo");

                            ParameterInfo[] pars = methodInfo.GetParameters();
                            foreach (ParameterInfo p in pars)
                            {
                                TL.LogMessage(memberName, "  Parameter: " + p.ParameterType.ToString());
                                TL.LogMessage(memberName, "    AssemblyQualifiedName: " + p.ParameterType.AssemblyQualifiedName);
                                TL.LogMessage(memberName, "    AssemblyQualifiedName: " + parameterTypes[0].AssemblyQualifiedName);
                                TL.LogMessage(memberName, "    FullName: " + p.ParameterType.FullName);
                                TL.LogMessage(memberName, "    FullName: " + parameterTypes[0].FullName);
                                TL.LogMessage(memberName, "    AssemblyFullName: " + p.ParameterType.Assembly.FullName);
                                TL.LogMessage(memberName, "    AssemblyFullName: " + parameterTypes[0].Assembly.FullName);
                                TL.LogMessage(memberName, "    AssemblyCodeBase: " + p.ParameterType.Assembly.CodeBase);
                                TL.LogMessage(memberName, "    AssemblyCodeBase: " + parameterTypes[0].Assembly.CodeBase);
                                TL.LogMessage(memberName, "    AssemblyLocation: " + p.ParameterType.Assembly.Location);
                                TL.LogMessage(memberName, "    AssemblyLocation: " + parameterTypes[0].Assembly.Location);
                                TL.LogMessage(memberName, "    AssemblyGlobalAssemblyCache: " + p.ParameterType.Assembly.GlobalAssemblyCache.ToString());
                                TL.LogMessage(memberName, "    AssemblyGlobalAssemblyCache: " + parameterTypes[0].Assembly.GlobalAssemblyCache.ToString());
                            }
 



                            try
                            {
                                object result = methodInfo.Invoke(objLateBound, parms);
                                TL.LogMessage(memberName, "  Successfully called method");
                                return result;
                            }
                            catch (TargetInvocationException e)
                            {
                                TL.LogMessage(memberName, "  ***** TargetInvocationException: " + e.ToString());
                                if (e.InnerException is ASCOM.DriverException)
                                {
                                    throw e.InnerException;
                                }
                                else throw new ASCOM.MethodNotImplementedException(strProgID + " " + memberName, e.InnerException);
                            }
                            catch (Exception e)
                            {
                                TL.LogMessage(memberName, "  ***** Exception: " + e.ToString());
                                 throw e;
                            }
                        }
                        else
                        {
                            TL.LogMessage(memberName, "  Didn't Get MethodInfo");
                            //check the type to see if it's a COM object
                            if (isCOMObject)
                            {
                                TL.LogMessage(memberName, "  It is a COM object");
                                try
                                {
                                    //run the COM object method
                                    return objType.InvokeMember(memberName, BindingFlags.Default | BindingFlags.InvokeMethod, null, objLateBound, parms);
                                }
                                catch (System.Runtime.InteropServices.COMException e)
                                {
                                    propertyGetInfo = null;
                                    if (e.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber)) throw new ASCOM.MethodNotImplementedException(strProgID + " " + memberName);
                                    else throw;
                                }
                                catch (Exception e)
                                {
                                    throw e.InnerException;
                                }
                            }
                            else
                            {
                                TL.LogMessage(memberName, "  It is NOT a COM object");
                                methodInfo = null;
                                throw new ASCOM.MethodNotImplementedException(strProgID + " " + memberName);
                            }
                        }
                    default:
                        return null;
                }
        }

        #region IDisposable Members

        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
		/// if it is a COM object, else if native .NET will just dereference it
		/// for GC.
        /// </summary>
        /// <returns>nothing</returns>
        public void Dispose()
        {
            if (objLateBound != null)
            { 
				try { Marshal.ReleaseComObject(objLateBound); }
				catch (Exception) { }
				objLateBound = null;
            }
        }

        #endregion  
        
        #endregion
    }
}
