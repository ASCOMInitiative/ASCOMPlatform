//tabs=4
// --------------------------------------------------------------------------------
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
//
//
// -----------	---	-----	-------------------------------------------------------
// 01-27-2010   JTS Beta3   Swapped Temperature and Position DRO's on Setup Dialog
//                          Setting Device Type now changes the Firmware device type
//                          Made Device Type Combo Box disabled while disconnected.
// -----------	---	-----	-------------------------------------------------------
// 01-22-2010   JTS Beta2   Fixed Exception thrown when move is attempted in AutoFocus mode
// -----------	---	-----	-------------------------------------------------------
// 09-23-2009	JTS	Beta1	Initial edit, from ASCOM Focuser Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using ASCOM;
using ASCOM.Interface;
using System.Diagnostics;


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
        private static string s_csDriverDescription = "Optec TCF-S BETA3";

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
            Trace.WriteLine("ASCOM Registration Started");
            Utilities.Profile P = new Utilities.Profile();
            P.DeviceType = "Focuser";
            if (bRegister)
            {
                P.Register(s_csDriverID, s_csDriverDescription);
                Trace.WriteLine("ASCOM Registration Complete");
                Debug.WriteLine("ASCOM Registration Complete");
            }
            else
            {
                P.Unregister(s_csDriverID);
                Trace.WriteLine("ASCOM UNRegistration Complete");
                Debug.WriteLine("ASCOM UNRegistration Complete");
            }
            try										
            {
                Marshal.ReleaseComObject(P);
            }
            catch (Exception) { }
            P = null;
            
        }

        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            Debug.WriteLine("Registration Started");
            Trace.WriteLine("COMRegistration Entered.");
            RegUnregASCOM(true);
            Trace.WriteLine("Registration Finished.");
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
        }             //FINISHED

        public void Halt()
        {
            // This operation is not supported by Optec Drivers
            throw new ASCOM.MethodNotImplementedException("Halt");
        }               //FINISHED

        public bool IsMoving
        {
            //This driver does not allow any other methods to be called
            //while the device is moving. So this is always false.
            get { return false; }
        }             //FINISHED

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
        }                 //FINISHED

        public int MaxIncrement
        {
            //Max Increment is the same as Max Step in our case so use the same method...
            get { return DeviceSettings.GetMaxStep(); }
        }          //FINISHED

        public int MaxStep
        {
            get { return DeviceSettings.GetMaxStep(); }
        }               //FINISHED

        public void Move(int val)
        {
            // TODO Replace this with your implementation
            if (DeviceComm.InTempCompMode())
            {
                throw new ASCOM.InvalidValueException("Attempted to move while in TempComp Mode", "", "");
               
            }
            DeviceComm.MoveFocus(val);
        }        //FINISHED

        public int Position
        {
            get { return DeviceComm.Position; }
        }              //FINISHED

        public void SetupDialog()
        {
            SetupDialogForm F = new SetupDialogForm();
            F.ShowDialog();
        }        //FINISHED

        public double StepSize
        {
            get { return DeviceSettings.GetMaxStep(); }
        }           //FINISHED

        public bool TempComp
        {
            get { return DeviceComm.InTempCompMode(); }
            set { DeviceComm.EnterTempCompMode(value); }
        }             //FINISHED

        public bool TempCompAvailable
        {
            get { return true; }
        }    //FINISHED

        public double Temperature
        {
            get { return DeviceComm.Temperature; }
        }        //FINISHED

        #endregion
    }
}
