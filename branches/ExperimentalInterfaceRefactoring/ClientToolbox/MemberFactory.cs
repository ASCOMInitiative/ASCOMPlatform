//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Member factory for all drivers
//
// Description:	With the progID, creates an instance, seraches for members, invokes members
//
// Implements:	IDisposable
// Author:		Rob Morgan
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 27-May-2010	rbd	6.0.0	Initial edit
// 
// --------------------------------------------------------------------------------
//
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ASCOM.DriverAccess
{
    #region MemberFactory
    /// <summary>
    /// A factory class to access any registered driver members
    /// </summary>
    public class MemberFactory: IDisposable
    {
        private object objLateBound;
        private Type objType;
        private String strProgID;

        /// <summary>
        /// Constructor, creates an instance of the of the ASCOM driver
        /// 
        /// </summary> 
        /// <param name="progID">The program ID of the driver</param>  
        internal MemberFactory(string progID)
        {
            strProgID = progID;
            try
            {
                // Get Type Information 
                objType = Type.GetTypeFromProgID(progID);

                // Create an instance of the object
                objLateBound = Activator.CreateInstance(objType);
            }
            catch
            {
                Dispose();
                throw new Exception("ASCOM: cannot create driver instance of progID: " + strProgID);
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
                        return propertyGetInfo.GetValue(objLateBound, null);
                    }
                    else
                    {
                        propertyGetInfo = null;
                        throw new Exception( "Check Driver: Failed call to driver property " + memberName + " from " + strProgID);
                    }
                case 2:
                    PropertyInfo propertySetInfo = objType.GetProperty(memberName);
                    if (propertySetInfo != null)
                    {
                        propertySetInfo.SetValue(objLateBound, parms[0], null);
                        return null;
                    }
                    else
                    {
                        propertySetInfo = null;
                        throw new Exception("Check Driver: Failed call to driver property " + memberName + " from " + strProgID);
                    }
                case 3:
                    MethodInfo methodInfo = objType.GetMethod(memberName, parameterTypes);
                    if (methodInfo != null)
                    {
                        return methodInfo.Invoke(objLateBound, parms);
                    }
                    else
                    {
                        methodInfo = null;
                        throw new Exception("Check Driver: Failed call to driver method " + memberName + " from " + strProgID);
                    }
                default:
                    return null;
            }

        }
    #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
		/// if it is a COM object, else if native .NET will just dereference it
		/// for GC.
        /// </summary>
        public void Dispose()
        {
            if (objLateBound != null)
            { 
				try { Marshal.ReleaseComObject(objLateBound); }
				catch (Exception) { }
				objLateBound = null;
                objType = null;
            }
        }
    #endregion
    }
}
