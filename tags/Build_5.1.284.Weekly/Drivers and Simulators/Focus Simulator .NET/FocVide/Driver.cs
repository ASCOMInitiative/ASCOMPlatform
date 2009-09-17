//tabs=4
// --------------------------------------------------------------------------------
// ASCOM Focuser driver for .NET
//
// Description:	ASCOM Focuser driver for .NET
//
// Implements:	ASCOM Focuser interface version: 1.0
// Author:		Christophe Gerbier (ch.gerbier@strulik.fr)
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 12-07-2009	ChG	1.0.0	Initial beta 1release
// 29-07-2009   ChG 1.1.0   Beta 2 release.
//                          Removed all the server parts so that it is a single DLL now
//                          Leave the driver.cs call implementations in FocuserHardware.cs
//                          Reworked temperature compensation implementation
//                          Corrected bug in log trace
//                          Changes in the GUI
// --------------------------------------------------------------------------------
//
using System;
using System.Runtime.InteropServices;

using ASCOM.Interface;

namespace ASCOM.FocVide
{
    [Guid("2e21ae40-2f82-41f7-902a-45a48c7dce78")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : IFocuser
    {
        private static string s_csDriverID = "ASCOM.FocVide.Focuser";
        private static string s_csDriverDescription = "ASCOM .NET Focuser Simulator";

        public Focuser()
        {
            FocuserHardware.Gui.Show();
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
