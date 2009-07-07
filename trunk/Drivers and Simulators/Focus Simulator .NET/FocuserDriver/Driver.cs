//tabs=4
// --------------------------------------------------------------------------------
// ASCOM Focuser driver for .NET
//
// Description:	ASCOM Focuser simulator for .NET Framework
//
// Implements:	ASCOM Focuser interface version: 1.0
// Author:		Christophe Gerbier (becafuel@lsp-fr.com)
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
using System.Threading;

using ASCOM;
using ASCOM.HelperNET;
using ASCOM.Interface;

namespace ASCOM.FocuserSimulator
{
    //
    // Your driver's ID is ASCOM.FocuserSimulator.Focuser
    //
    // The Guid attribute sets the CLSID for ASCOM.FocuserSimulator.Focuser
    // The ClassInterface/None addribute prevents an empty interface called
    // _Focuser from being created and used as the [default] interface
    //
    [Guid("bee80414-0281-4f85-836b-9f34c0c14d10")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : ReferenceCountedObjectBase,IFocuser
    {
        //
        // Constructor
        //
        public Focuser()
        {
        }

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
