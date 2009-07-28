//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Focuser driver for FocVide
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Focuser interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Focuser Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.HelperNET;
using ASCOM.Interface;

namespace ASCOM.FocVide
{
    //
    // Your driver's ID is ASCOM.FocVide.Focuser
    //
    // The Guid attribute sets the CLSID for ASCOM.FocVide.Focuser
    // The ClassInterface/None addribute prevents an empty interface called
    // _Focuser from being created and used as the [default] interface
    //
    [Guid("2e21ae40-2f82-41f7-902a-45a48c7dce78")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : IFocuser
    {
        //
        // Driver ID and descriptive string that shows in the Chooser
        //
        private static string s_csDriverID = "ASCOM.FocVide.Focuser";
        // TODO Change the descriptive string for your driver then remove this line
        private static string s_csDriverDescription = "ASCOM .NET Focuser Simulator";

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
            HelperNET.Profile P = new HelperNET.Profile();
            P.DeviceType = "Focuser";					//  Requires Helper 5.0.3 or later
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
            get { return FocuserHardware.Absolute; }
        }

        public void Halt()
        {
            FocuserHardware.Halt();
        }

        public bool IsMoving
        {
            get { return FocuserHardware.IsMoving; }
        }

        public bool Link
        {
            get { return FocuserHardware.Link; }
            set { FocuserHardware.Link = value; }
        }

        public int MaxIncrement
        {
            get { return FocuserHardware.MaxIncrement; }
        }

        public int MaxStep
        {
            get { return FocuserHardware.MaxStep; }
        }


        public void Move(int val)
        {
            FocuserHardware.Move(val);
        }

        public int Position
        {
            get { return FocuserHardware.Position; }
        }

        public void SetupDialog()
        {
            FocuserHardware.DoSetup();
        }

        public double StepSize
        {
            get { return FocuserHardware.StepSize; }
        }

        public bool TempComp
        {
            get { return FocuserHardware.TempComp; }
            set { FocuserHardware.TempComp = value; }
        }

        public bool TempCompAvailable
        {
            get { return FocuserHardware.TempCompAvailable; }
        }

        public double Temperature
        {
            get { return FocuserHardware.Temperature; }
        }
        #endregion
    }
}
