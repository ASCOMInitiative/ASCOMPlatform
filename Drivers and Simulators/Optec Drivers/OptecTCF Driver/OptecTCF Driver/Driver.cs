//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Focuser driver for OptecTCF_Driver
//
// Description:	
//				
//				 
//				 
//				
//
// Implements:	ASCOM Focuser interface version: 5.0
// Author:		Jordan Schaenzle <Jordan@optecinc.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 09-23-2009	JTS	5.0.0	Initial edit, from ASCOM Focuser Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using ASCOM;
using ASCOM.Helper;
using ASCOM.Helper2;
using ASCOM.Interface;


namespace ASCOM.OptecTCF_Driver
{
    //
    // Your driver's ID is ASCOM.OptecTCF_Driver.Focuser
    //
    // The Guid attribute sets the CLSID for ASCOM.OptecTCF_Driver.Focuser
    // The ClassInterface/None addribute prevents an empty interface called
    // _Focuser from being created and used as the [default] interface
    //
    [Guid("d045bc52-e855-4b89-babc-d87c07c8a5a8")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : IFocuser
    {
        //
        // Driver ID and descriptive string that shows in the Chooser
        //
        internal static string s_csDriverID = "ASCOM.OptecTCF_Driver.Focuser";
        private static string s_csDriverDescription = "Optec TCF-S";

        //
        // Constructor - Must be public for COM registration!
        //
        public Focuser()
        {
            // TODO Implement your additional construction here
        }

        #region ASCOM Registration
        //
        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        private static void RegUnregASCOM(bool bRegister)
        {
            Helper.Profile P = new Helper.Profile();
            P.DeviceTypeV = "Focuser";					//  Requires Helper 5.0.3 or later
            if (bRegister)
                P.Register(s_csDriverID, s_csDriverDescription);
            else
                P.Unregister(s_csDriverID);
            try										// In case Helper becomes native .NET
            {
                Marshal.ReleaseComObject(P);
            }
            catch (Exception) { }
            P = null;
        }

        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }
        #endregion

        //
        // PUBLIC COM INTERFACE IFocuser IMPLEMENTATION
        //

        #region IFocuser Members

        public bool Absolute
        {
            get { return true; }
        }  //FINISHED

        public void Halt()
        {
            // This operation is not supported by Optec Drivers
            throw new ASCOM.MethodNotImplementedException("Halt");
        }    //FINISHED

        public bool IsMoving
        {
            //This driver does not allow any other methods to be called
            //while the device is moving. So this is always false.
            get { return false; }
        }  //FINISHED

        public bool Link
        {
            // TODO Replace this with your implementation
            get { return DeviceComm.GetConnectionState(); }
            set
            {
                try
                {
                    if (value)
                    {
                        DeviceComm.Connect();
                    }
                    else
                    {
                        DeviceComm.Disconnect();
                    }
                }
                catch (Exception Ex)
                {

                    throw new ASCOM.DriverException("Device Failed to change Link state.\n" + Ex.ToString(), Ex);
                }
            }
        }      //FINISHED

        public int MaxIncrement
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("MaxIncrement", false); }
        }

        public int MaxStep
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("MaxStep", false); }
        }

        public void Move(int val)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("Move");
        }

        public int Position
        {
            get { return DeviceComm.GetPosition(); }
        }      //FINISHED

        public void SetupDialog()
        {
            SetupDialogForm F = new SetupDialogForm();
            F.ShowDialog();
        }    //FINISHED

        public double StepSize
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("StepSize", false); }
        }

        public bool TempComp
        {
            // TODO Replace this with your implementation
            get { return DeviceComm.InTempCompMode(); }
            set { DeviceComm.EnterTempCompMode(value); }
        }

        public bool TempCompAvailable
        {
            get { return true; }
        }    //FINISHED

        public double Temperature
        {
            get { return DeviceComm.GetTemperaterature(); }
        }        //

        #endregion
    }
}
