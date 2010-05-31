//-----------------------------------------------------------------------
// <summary>Defines the MemberFactory class.</summary>
//-----------------------------------------------------------------------
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using System.Reflection;
using System.Runtime.InteropServices;

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
        private ExceptionLogger objLogger;

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
            catch(Exception e)
            {
                Dispose();
                string error = "ASCOM: cannot create driver instance of progID: " + strProgID;
                objLogger.LogException(e);
                throw new Exception(error);
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
                            string error = "Check Driver: Failed call to driver property " + memberName + " in " + strProgID;
                            LogError(error, propertyGetInfo, null);
                            throw new Exception(error);
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
                            propertyGetInfo = null;
                            string error = "Check Driver: Failed call to driver property " + memberName + " in " + strProgID;
                            LogError(error, propertySetInfo, null);
                            throw new Exception(error);
                        }
                    case 3:
                        MethodInfo methodInfo = objType.GetMethod(memberName, parameterTypes);
                        if (methodInfo != null)
                        {
                            return methodInfo.Invoke(objLateBound, parms);
                        }
                        else
                        {
                            string error = "Check Driver: Failed call to driver method " + memberName + " in " + strProgID;
                            LogError(error, null, methodInfo);
                            methodInfo = null;
                            throw new Exception(error);
                        }
                    default:
                        return null;
                }
        }

        /// <summary>
        /// Logs the error to the eventviewer
        /// </summary> 
        /// <returns>nothing</returns>
        internal void LogError(string error, PropertyInfo propertyInfo, MethodInfo methodInfo)
        {
            ExceptionLogger objLogger = new ExceptionLogger();
            objLogger.LogError(error, strProgID, propertyInfo, methodInfo);
            objLogger = null;
            throw new Exception(error);
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
                objType = null;
                objLogger = null;
            }
        }

        #endregion  
        
        #endregion
    }
}
