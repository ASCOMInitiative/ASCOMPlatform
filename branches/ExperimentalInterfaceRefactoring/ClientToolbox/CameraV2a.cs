using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using ASCOM.Interface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Implements a camera class to access any registered ASCOM Camera
    /// </summary>
    public class CameraV2a : Camera
    {
        protected object objCameraLateBound;
        protected ICameraV2 iCamera;
        protected Type objTypeCamera;

        #region Camera new
        /// <summary>
        /// Initializes a new instance of the <see cref="CameraV2"/> class.
        /// </summary>
        /// <param name="cameraID">The COM ProgID of the underlying driver.</param>
        public CameraV2a(string cameraID) : base(cameraID)

        {
             /*       // Get Type Information 
            objTypeCamera = Type.GetTypeFromProgID(cameraID);

            // Create an instance of the camera object
            objCameraLateBound = Activator.CreateInstance(objTypeCamera);

            // Try to see if this driver has an ASCOM.Camera interface
            try
            {
                iCamera = (ICameraV2)objCameraLateBound;
            }
            catch (Exception)
            {
                iCamera = null;
            }*/
        }
        #endregion

        #region IAscomDriver Members
        private int? interfaceVersion;

        private int CheckInterfaceVersion()
        {
            if (this.interfaceVersion != null)
                return (int)interfaceVersion;
            try
            {
                this.interfaceVersion = Convert.ToInt16(objTypeCamera.InvokeMember("InterfaceVersion",
                    BindingFlags.Default | BindingFlags.GetProperty,
                    null, objCameraLateBound, new object[] { }));
            }

            catch (System.Runtime.InteropServices.COMException ex)
            {
                this.interfaceVersion = 1;
                if (ex.ErrorCode == -2147352570) //0x80020006
                {
                    this.interfaceVersion = 1;
                }
                else
                {
                    throw;
                }
            }

            catch (System.MissingFieldException)
            {
                this.interfaceVersion = 1;
                global::System.Windows.Forms.MessageBox.Show("Got a MissingFieldException");
            }

            return (int)this.interfaceVersion;
        }

        public string DriverVersion
        {
            get { throw new ASCOM.PropertyNotImplementedException("DriverVersion", false); }
        }

        public string Action(string ActionName, string ActionParameters)
        {
            throw new PropertyNotImplementedException("Action", false);
        }

        /// <summary>
        /// The version of this interface. Will return 2 for this version.
        /// Clients can detect legacy V1 drivers by trying to read ths property.
        /// If the driver raises an error, it is a V1 driver. V1 did not specify this property. A driver may also return a value of 1.
        /// In other words, a raised error or a return value of 1 indicates that the driver is a V1 driver.
        /// </summary>
        /// <value>The interface version.</value>
        public short InterfaceVersion
        {
            get
            {
                /*return Convert.ToInt16(objTypeCamera.InvokeMember("InterfaceVersion",
                    BindingFlags.Default | BindingFlags.GetProperty,
                    null, objCameraLateBound, new object[] { }));
                 */
                return (short)CheckInterfaceVersion();
            }
        }

        public string LastResult
        {
            get { throw new ASCOM.InvalidOperationException("LastResult has been called before an Action"); }
        }

        public string[] SupportedActions
        {
            get { throw new PropertyNotImplementedException("SupportedActions", false); }
        }

        public void CommandBlind(string Command, bool Raw)
        {
            throw new MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string Command, bool Raw)
        {
            throw new MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string Command, bool Raw)
        {
            throw new MethodNotImplementedException("CommandString");
        }

        string Config = "Default";
        public string[] SupportedConfigurations
        {
            get
            {
                return new string[] { "Default" };
            }
        }

        public string Configuration
        {
            get
            {
                return Config;
            }

            set
            {
                Config = value;
            }
        }
        #endregion

    }
}