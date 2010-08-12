//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Rotator driver for PyxisLE_ASCOM
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Rotator interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Rotator Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Interface;
using PyxisLE_API;
using ASCOM.Utilities;

namespace ASCOM.PyxisLE_ASCOM
{
    //
    // Your driver's ID is ASCOM.PyxisLE_ASCOM.Rotator
    //
    // The Guid attribute sets the CLSID for ASCOM.PyxisLE_ASCOM.Rotator
    // The ClassInterface/None addribute prevents an empty interface called
    // _Rotator from being created and used as the [default] interface
    //
    [Guid("fff6d636-a4e8-4dbd-b190-1729b3c91f24")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Rotator : IRotator
    {
        //
        // Driver ID and descriptive string that shows in the Chooser
        //
        private static string s_csDriverID = "ASCOM.PyxisLE_ASCOM.Rotator";
        // TODO Change the descriptive string for your driver then remove this line
        private static string s_csDriverDescription = "PyxisLE_ASCOM Rotator";

        private Rotators RotatorManager;
        private Profile myProfile;
        private PyxisLE_API.Rotator myRotator;

        //
        // Constructor - Must be public for COM registration!
        //
        public Rotator()
        {
            RotatorManager = new Rotators();
            myProfile = new Profile();
            myProfile.DeviceType = "Rotator";
            myRotator = null;
        }

        #region ASCOM Registration
        //
        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        private static void RegUnregASCOM(bool bRegister)
        {
            Utilities.Profile P = new Utilities.Profile();
            P.DeviceType = "Rotator";					//  Requires Helper 5.0.3 or later
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
        // PUBLIC COM INTERFACE IRotator IMPLEMENTATION
        //

        #region IRotator Members

        public bool CanReverse
        {
            get { return true; }
        }

        public bool Connected
        {
            get 
            {
                if(myRotator == null) return false;        // No device has been set yet...
                if(myRotator.IsAttached == false) return false;    // Device has been unattached from PC
                return true; 
            }

            set 
            {
                if (value)  
                {
                    // CONNECT TO THE DEVICE
                    if(myRotator == null)
                    {
                        try 
                        {
                            myRotator = FindMyRotator();    // If this succeeds we know the device is attached
                        }
                        catch 
                        {
                            myRotator = null; 
                            throw;
                        }  
                    }
                    else
                    {
                        if(!myRotator.IsAttached)
                        {
                            myRotator = null; 
                            throw new ASCOM.NotConnectedException("Rotator Device is not connected to the PC");
                        }
                    }

                }
                else
                {
                    // DISCONNECT FROM THE DEVICE
                    myRotator = null;
                }
            }
        }   

        public void Halt()
        {
            if (!Connected) throw new ASCOM.NotConnectedException("The rotator device is no longer connected");
            myRotator.Halt_Move();
        }

        public bool IsMoving
        {
            get 
            {
                VerifyConnected();
                return (myRotator.IsMoving || myRotator.IsMoving);
            }
        }     

        public void Move(float Position)
        {
            // TODO Replace this with your implementation

            if (!Connected) throw new ASCOM.NotConnectedException("The rotator device is no longer connected");
            throw new MethodNotImplementedException("Move");
        }

        public void MoveAbsolute(float Position)
        {
            if (!Connected) throw new ASCOM.NotConnectedException("The rotator device is no longer connected");
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("MoveAbsolute");
        }

        public float Position
        {
            get 
            {
                if (!Connected) throw new ASCOM.NotConnectedException("The rotator device is no longer connected");
                return (float)myRotator.CurrentPosition; 
            }
        }

        public bool Reverse
        {
            get {
                if (!Connected) throw new ASCOM.NotConnectedException("The rotator device is no longer connected");
                return myRotator.Reverse;
            }
            set { 
                if (!Connected) throw new ASCOM.NotConnectedException("The rotator device is no longer connected");
                myRotator.Reverse = value;
            }
        }

        public void SetupDialog()
        {
            SetupDialogForm F = new SetupDialogForm();
            F.ShowDialog();
        }

        public float StepSize
        {
            get 
            {
                return .05F;
            }
        }

        public float TargetPosition
        {
            get 
            {
                VerifyConnected();
                return (float)myRotator.TargetPosition;
            }
        }

        #endregion

        #region Private methods

        private PyxisLE_API.Rotator FindMyRotator()
        {
            string DesiredSerialNumber = myProfile.GetValue(s_csDriverID, "SelectedSerialNumber", "", "0");

            foreach (PyxisLE_API.Rotator r in RotatorManager.RotatorList)
            {
                if (r.SerialNumber == DesiredSerialNumber)
                {
                    return r;
                }
            }
            throw new ApplicationException("The selected rotator is not connected to the PC");
        }

        private void VerifyConnected()
        {
            if (!Connected) throw new ASCOM.NotConnectedException("The rotator device is no longer connected");
        }

        #endregion
    }
}
