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

using Optec;
using System.Diagnostics;

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
        internal static string s_csDriverID = "ASCOM.PyxisLE_ASCOM.Rotator";
        // TODO Change the descriptive string for your driver then remove this line
        private static string s_csDriverDescription = "Optec Pyxis LE";

        private Rotators RotatorManager;
        private Profile myProfile;
        private PyxisLE_API.Rotator myRotator;

        //
        // Constructor - Must be public for COM registration!
        //

        public Rotator()
        {
#if DEBUG
            System.Windows.Forms.MessageBox.Show("Constructing Rotator object");
            EventLogger.LoggingLevel = TraceLevel.Info;
#endif
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
#if DEBUG
            System.Windows.Forms.MessageBox.Show("Registering Driver");
#endif
            Logger.TLogger.LogMessage("Installer","Registering driver for ASCOM");
            RegUnregASCOM(true);
        }

        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
#if DEBUG
            System.Windows.Forms.MessageBox.Show("Unregistering Driver");
#endif
            Logger.TLogger.LogMessage("Installer", "Unregistering driver for ASCOM");
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
            if (Connected && myRotator.IsMoving) myRotator.Halt_Move();
        }

        public bool IsMoving
        {
            get 
            {
                VerifyConnected();
                return (myRotator.IsMoving || myRotator.IsHoming);
                
            }
        }     

        public void Move(float Position)
        {

            try
            {
                EventLogger.LogMessage("Relative move to " + Position.ToString() + " requested.", TraceLevel.Info);
                VerifyConnected();
                if (Position > 360) throw new ASCOM.InvalidOperationException("Cannot move to position greater than 360°");
                else if (Position < -360) throw new ASCOM.InvalidOperationException("Cannot move to position less than 0°");
                double NewPosition = myRotator.CurrentSkyPA + Position;
                if (NewPosition > 360) NewPosition = NewPosition - 360;
                else if (NewPosition < 0) NewPosition = NewPosition + 360;
                myRotator.CurrentSkyPA = NewPosition;
                //System.Threading.Thread.Sleep(500);
                EventLogger.LogMessage("Move Started at " + DateTime.Now.ToLongTimeString(), TraceLevel.Info);
                while (this.IsMoving) { System.Threading.Thread.Sleep(50); System.Windows.Forms.Application.DoEvents(); }
                EventLogger.LogMessage("Returning from move method at " + DateTime.Now.ToLongTimeString(), TraceLevel.Info);
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw;
            }
        }

        public void MoveAbsolute(float Position)
        {
            try
            {
                EventLogger.LogMessage("Absolute move to " + Position.ToString(), TraceLevel.Info);
                VerifyConnected();
                if (Position > 360) throw new ASCOM.InvalidOperationException("Cannot move to position greater than 360°");
                else if (Position < 0) throw new ASCOM.InvalidOperationException("Cannot move to position less than 0°");
                EventLogger.LogMessage("Starting Move at " + DateTime.Now.ToLongTimeString(), TraceLevel.Info);
                myRotator.CurrentSkyPA = (double)Position;    
                
                //DateTime start = DateTime.Now;
                //while (myRotator.CurrentSkyPA != Position) { System.Windows.Forms.Application.DoEvents(); }
                 while (myRotator.IsMoving || myRotator.CurrentSkyPA != Position) { System.Windows.Forms.Application.DoEvents(); }
                EventLogger.LogMessage("Returning from move method at " + DateTime.Now.ToLongTimeString() + 
                    ", Current Sky PA = " + myRotator.CurrentSkyPA.ToString(), TraceLevel.Info);
            }
            catch (Exception ex) 
            {
                EventLogger.LogMessage(ex);
                throw;
            }
        }

        public float Position
        {
            get 
            {
                VerifyConnected();
                return (float)myRotator.CurrentSkyPA; 
            }
        }

        public bool Reverse
        {
            get {
                VerifyConnected();
               
                return myRotator.Reverse;
            }
            set {
                try
                {
                    VerifyConnected();
                    while (myRotator.IsHoming || myRotator.IsMoving) { }
                    myRotator.Reverse = value;
                    System.Threading.Thread.Sleep(500);
                    while (myRotator.IsHoming || myRotator.IsMoving) { /* We have to wait here because this method is synchronous*/ }
                    System.Threading.Thread.Sleep(100);
                }
                catch (Exception ex) 
                { 
                    EventLogger.LogMessage(ex);
                    throw;
                }
            }
        }

        public void SetupDialog()
        {
            try
            {
                SetupDialogForm F = new SetupDialogForm();
                F.ShowDialog();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw;
            }
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
                return (float)myRotator.TargetSkyPA;
            }
        }


        #endregion

        #region Private methods

        private PyxisLE_API.Rotator FindMyRotator()
        {
            try
            {
                string DesiredSerialNumber = myProfile.GetValue(s_csDriverID, "SelectedSerialNumber", "", "0");

                if (RotatorManager.RotatorList.Count == 1)
                {
                    PyxisLE_API.Rotator rr = RotatorManager.RotatorList[0] as PyxisLE_API.Rotator;
                    string msg = "Only one rotator attached. Using that rotator: " + rr.SerialNumber + " for ASCOM application.";
                    EventLogger.LogMessage(msg, TraceLevel.Info);
                    return rr;
                }

                foreach (PyxisLE_API.Rotator r in RotatorManager.RotatorList)
                {
                    if (r.SerialNumber == DesiredSerialNumber)
                    {
                        return r;
                    }
                }
                throw new ApplicationException("The selected rotator is not connected to the PC");
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw;
            }

        }

        private void VerifyConnected()
        {
            if (!Connected) throw new ASCOM.NotConnectedException("The rotator device is no longer connected");
            else if (myRotator.ErrorState != 0) throw new ASCOM.NotConnectedException(myRotator.GetErrorMessage(myRotator.ErrorState));
            else if (myRotator.IsHomed == false) throw new ASCOM.NotConnectedException("The Connected Rotator is not homed. You must home the device before the requested operation can be performed.");
        }

        #endregion
    }
}
