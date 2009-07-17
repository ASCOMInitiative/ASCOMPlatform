//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Focuser driver for Gemini
//
// Description:	Gemini controlled focuser
//
// Implements:	ASCOM Focuser interface version: 1.0
// Author:		(rbt) Robert Turner <robert@robertturnerastro.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 15-JUL-2009	rbt	1.0.0	Initial edit, from ASCOM Focuser Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.HelperNET;
using ASCOM.Interface;

namespace ASCOM.GeminiTelescope
{
    //
    // Your driver's ID is ASCOM.Focuser.Focuser
    //
    // The Guid attribute sets the CLSID for ASCOM.Focuser.Focuser
    // The ClassInterface/None addribute prevents an empty interface called
    // _Focuser from being created and used as the [default] interface
    //
    [Guid("3a22c443-4e46-4504-8cef-731095e51e1f")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : ReferenceCountedObjectBase, IFocuser
    {

        //
        // Constructor - Must be public for COM registration!
        //
        public Focuser()
        {
            // TODO Implement your additional construction here
        }


        //
        // PUBLIC COM INTERFACE IFocuser IMPLEMENTATION
        //

        #region IFocuser Members

        public bool Absolute
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("Absolute", false); }
        }

        public void Halt()
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("Halt");
        }

        public bool IsMoving
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("IsMoving", false); }
        }

        public bool Link
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("Link", false); }
            set { throw new PropertyNotImplementedException("Link", true); }
        }

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
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("Position", false); }
        }

        public void SetupDialog()
        {
            if (GeminiHardware.Connected)
            {
                throw new DriverException("The hardware is connected, cannot do SetupDialog()",
                                    unchecked(ErrorCodes.DriverBase + 4));
            }
            GeminiTelescope.m_MainForm.DoTelescopeSetupDialog();
        }

        public double StepSize
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("StepSize", false); }
        }

        public bool TempComp
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("TempComp", false); }
            set { throw new PropertyNotImplementedException("TempComp", true); }
        }

        public bool TempCompAvailable
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("TempCompAvailable", false); }
        }

        public double Temperature
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("Temperature", false); }
        }

        #endregion
    }
}
