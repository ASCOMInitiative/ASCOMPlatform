//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM FilterWheel driver for HighSpeedFilterWheelUSB
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM FilterWheel interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM FilterWheel Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Utilities;
using ASCOM.Interface;

namespace ASCOM.HighSpeedFilterWheelUSB
{
    //
    // Your driver's ID is ASCOM.HighSpeedFilterWheelUSB.FilterWheel
    //
    // The Guid attribute sets the CLSID for ASCOM.HighSpeedFilterWheelUSB.FilterWheel
    // The ClassInterface/None addribute prevents an empty interface called
    // _FilterWheel from being created and used as the [default] interface
    //
    [Guid("62621d38-9db0-4a6f-947d-a2032205ada5")]
    [ClassInterface(ClassInterfaceType.None)]
    public class FilterWheel : IFilterWheel
    {
        //
        // Driver ID and descriptive string that shows in the Chooser
        //
        internal static string s_csDriverID = "ASCOM.HighSpeedFilterWheelUSB.FilterWheel";
        // TODO Change the descriptive string for your driver then remove this line
        private static string s_csDriverDescription = "HighSpeedFilterWheelUSB FilterWheel";

        //
        // Constructor - Must be public for COM registration!
        //

        public FilterWheel()
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
            Profile P = new Profile();
            P.DeviceType = "FilterWheel";					//  Requires Helper 5.0.3 or later
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
        // PUBLIC COM INTERFACE IFilterWheel IMPLEMENTATION
        //

        #region IFilterWheel Members

        public bool Connected
        {
            get
            {
                try
                {
                    return ASCOMFilterWheel.Connected;
                }
                catch { throw; }
            }
            set
            {
                try
                {
                    ASCOMFilterWheel.Connected = value;
                }
                catch
                {
                    throw;
                }
            }
        }

        public short Position
        {
            get 
            {
                try
                {
                    if (!ASCOMFilterWheel.Connected)
                    {
                        throw new ASCOM.NotConnectedException();
                    }
                    else
                    {
                        return ASCOMFilterWheel.myFilterWheel.CurrentPosition;
                    }
                }
                catch { throw; }
            }
            set
            {
                try
                {
                    if (!ASCOMFilterWheel.Connected)
                    {
                        throw new ASCOM.NotConnectedException();
                    }
                    else
                    {
                        ASCOMFilterWheel.myFilterWheel.CurrentPosition = value ;
                    }
                }
                catch { throw; }
            }
        }

        public int[] FocusOffsets
        {
            get { throw new PropertyNotImplementedException("FocusOfsets", false); }
        }

        public string[] Names
        {
            get 
            {
                try
                {
                    if (!ASCOMFilterWheel.Connected)
                    {
                        throw new ASCOM.NotConnectedException();
                    }
                    else
                    {
                        return ASCOMFilterWheel.myFilterWheel.GetFilterNames(ASCOMFilterWheel.myFilterWheel.WheelID);
                    }
                }
                catch { throw; }
            }
        }

        public void SetupDialog()
        {
            SetupDialogForm F = new SetupDialogForm();
            F.ShowDialog();
        }

        #endregion
    }
}
