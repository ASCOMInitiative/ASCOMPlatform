//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM FilterWheel driver for HSFW_ASCOM_Driver
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM FilterWheel interface version: 1.0
// Author:		Jordan T. Schaenzle <jordan@optecinc.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 07-22-2010	JTS	1.0.0	Initial edit, from ASCOM FilterWheel Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Interface;

namespace ASCOM.HSFW_ASCOM_Driver
{
    //
    // Your driver's ID is ASCOM.HSFW_ASCOM_Driver.FilterWheel
    //
    // The Guid attribute sets the CLSID for ASCOM.HSFW_ASCOM_Driver.FilterWheel
    // The ClassInterface/None addribute prevents an empty interface called
    // _FilterWheel from being created and used as the [default] interface
    //
    [Guid("8efbd2ff-5dea-4e04-9d82-2be2846e48f4")]
    [ClassInterface(ClassInterfaceType.None)]
    public class FilterWheel : IFilterWheel
    {
        //
        // Driver ID and descriptive string that shows in the Chooser
        //
        internal static string s_csDriverID = "ASCOM.HSFW_ASCOM_Driver.FilterWheel";

        private static string s_csDriverDescription = "Optec High Speed Filter Wheel";
        private HSFW_Handler myHandler;
        private bool connected;

        //
        // Constructor - Must be public for COM registration!
        //
        public FilterWheel()
        {
#if DEBUG
            System.Windows.Forms.MessageBox.Show("Attach Process Here if Necessary");
#endif
            myHandler = HSFW_Handler.GetInstance();
            connected = false;
        }

        #region ASCOM Registration
        //
        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        private static void RegUnregASCOM(bool bRegister)
        {
            Utilities.Profile P = new Utilities.Profile();
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
                return connected;
            }
            set 
            {
                if (!value) connected = false;
                else
                {
                    if (myHandler.myDevice.IsAttached)
                    {
                        connected = true;
                    }
                    else throw new ApplicationException("The specifed device is not attached.");
                }
            }
        }

        public short Position
        {
            get { return myHandler.myDevice.CurrentPosition; }
            set { myHandler.myDevice.CurrentPosition = value; }
        }

        public int[] FocusOffsets
        {
            get 
            {
                int[] offsets = new int[8];
                for (int i = 0; i < 8; i++)
                {
                    offsets[i] = HSFW_Handler.GetFocusOffset(myHandler.myDevice.SerialNumber,
                        myHandler.myDevice.WheelID, (short)(i + 1));
                }
                return offsets;    
            }
        }

        public string[] Names
        {
            get { return myHandler.myDevice.GetFilterNames(myHandler.myDevice.WheelID); }

        }

        public void SetupDialog()
        {
            SetupDialogForm F = new SetupDialogForm();
            F.ShowDialog();
        }

        #endregion
    }
}
