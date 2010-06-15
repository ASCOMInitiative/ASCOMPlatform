//-----------------------------------------------------------------------
// <summary>Defines the MemberFactory class.</summary>
//-----------------------------------------------------------------------
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Globalization;
using ASCOM;

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


        /// <summary>
        /// Constructor, creates an instance of the of the ASCOM driver
        /// 
        /// </summary> 
        /// <param name="progID">The program ID of the driver</param>
        internal MemberFactory(string progID)
        {
            strProgID = progID;

            // Get Type Information 
            objType = Type.GetTypeFromProgID(progID);

            //check to see if it found the type information
            if (objType != null)
            {
                //setup the property
                isCOMObject = objType.IsCOMObject;

                // Create an instance of the object
                objLateBound = Activator.CreateInstance(objType);

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
                        PropertyInfo propertyGetInfo = objType.GetProperty(memberName);
                        if (propertyGetInfo != null)
                         {
                             try
                             {
                                 //run the .net object
                                 return propertyGetInfo.GetValue(objLateBound, null);
                             }
                             catch (Exception e)
                             {
                                 throw e.InnerException;
                             }
                        }
                        else
                        {
                            //check the type to see if it's a COM object
                            if (isCOMObject)
                            {
                                try
                                {
                                    //run the COM object property
                                    return (objType.InvokeMember(memberName, BindingFlags.Default | BindingFlags.GetProperty, null, objLateBound, new object[] { }));
                                }
                                catch (System.Runtime.InteropServices.COMException e)
                                {
                                    propertyGetInfo = null;
                                    if (e.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber)) throw new ASCOM.PropertyNotImplementedException(strProgID + " " + memberName, false);
                                    else throw;
                                }
                                catch (Exception e)
                                {
                                    throw e.InnerException;
                                }
                            }
                            else
                            {
                                //evertyhing failed so throw an exception
                                propertyGetInfo = null;
                                throw new ASCOM.PropertyNotImplementedException(strProgID + " " + memberName, false);
                            }
                        }
                    case 2:
                        PropertyInfo propertySetInfo = objType.GetProperty(memberName);
                        if (propertySetInfo != null)
                        {
                            try
                            {
                                propertySetInfo.SetValue(objLateBound, parms[0], null);
                                return null;
                            }
                            catch (Exception e)
                            {
                                throw e.InnerException;
                            }
                        }
                        else
                        {
                            //check the type to see if it's a COM object
                            if (isCOMObject)
                            {
                                try
                                {
                                    //run the COM object property
                                    objType.InvokeMember(memberName, BindingFlags.Default | BindingFlags.SetProperty, null, objLateBound, parms);
                                    return null;
                                }
                                catch (System.Runtime.InteropServices.COMException e)
                                {
                                    propertySetInfo = null;
                                    if (e.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber)) throw new ASCOM.PropertyNotImplementedException(strProgID + " " + memberName, true);
                                    else throw;
                                }
                                catch (Exception e)
                                {
                                    throw e.InnerException;
                                }
                            }
                            else
                            {
                                propertySetInfo = null;
                                throw new ASCOM.PropertyNotImplementedException(strProgID + " " + memberName, true);
                            }
                        }
                    case 3:
                        MethodInfo methodInfo = objType.GetMethod(memberName, parameterTypes);
                        if (methodInfo != null)
                        {
                            try
                            {
                                return methodInfo.Invoke(objLateBound, parms);
                            }
                            catch (Exception e)
                            {
                                throw e.InnerException;
                            }
                        }
                        else
                        {
                            //check the type to see if it's a COM object
                            if (isCOMObject)
                            {
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
                                methodInfo = null;
                                throw new ASCOM.MethodNotImplementedException(strProgID + " " + memberName);
                            }
                        }
                    default:
                        return null;
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
        /// Returns the driver type
        /// </summary> 
        /// <returns>type</returns>
        internal Type GetObjType
        {
            get { return this.objType; }
        }       
        
        /// <summary>
        /// Returns true if the interface name is found in the driver type object
        /// </summary> 
        /// <param name="interfaceName">interface name to search</param>
        /// <returns>bool</returns>
        internal bool HasInterface(string interfaceName)
        {
                return objType.GetInterface(interfaceName,true)!= null ;
        }        
        
        /// <summary>
        /// Returns true is the driver is COM based
        /// </summary> 
        /// <returns>object</returns>
        internal bool IsCOMObject
        {
            get { return this.objType.IsCOMObject; }
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
