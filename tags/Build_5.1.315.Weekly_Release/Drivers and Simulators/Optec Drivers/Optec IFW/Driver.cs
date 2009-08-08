//tabs=4
// --------------------------------------------------------------------------------

//
// ASCOM FilterWheel driver for Optec_IFW
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
using ASCOM.Helper;
using ASCOM.Helper2;
using ASCOM.Interface;
using System.IO.Ports;


namespace ASCOM.Optec_IFW
{
    //
    // Your driver's ID is ASCOM.Optec_IFW.FilterWheel
    //
    // The Guid attribute sets the CLSID for ASCOM.Optec_IFW.FilterWheel
    // The ClassInterface/None addribute prevents an empty interface called
    // _FilterWheel from being created and used as the [default] interface
    //
    [Guid("81903010-4642-4afd-bfe7-9ac28ea27056")]
    [ClassInterface(ClassInterfaceType.None)]
    public class FilterWheel : IFilterWheel
    {
        //
        // Driver ID and descriptive string that shows in the Chooser
        //
        private static string s_csDriverID = "ASCOM.Optec_IFW.FilterWheel";
        private static string s_csDriverDescription = "Driver for Optec IFW";
        //
        // Constructor - Must be public for COM registration!
        //
        private static object CommLock = new object();

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
            Helper.Profile P = new Helper.Profile();
            P.DeviceTypeV = "FilterWheel";					//  Requires Helper 5.0.3 or later
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
            // TODO Replace this with your implementation
            get
            {
                lock(CommLock)
                {
                    return DeviceComm.CheckForConnection();
                }
            }
            set 
            {
                lock (CommLock)
                {
                    if (value)
                    {
                        DeviceComm.ConnectToDevice();
                        if (DeviceComm.CheckForConnection())
                        {
                            Connected = true;
                        }
                        else
                        {
                            Connected = false;
                            throw new Exception("Connection to the device has failed");
                        }
                    }
                    else
                    {
                        DeviceComm.DisconnectDevice();

                    }
                }
            }
        }

        public short Position
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("Position", false); }
            set { throw new PropertyNotImplementedException("Position", true); }
        }

        public int[] FocusOffsets
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("FocusOfsets", false); }
        }

        public string[] Names
        {
            // TODO Replace this with your implementation
            get
            {
                return DeviceComm.ReadAllNames();

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
