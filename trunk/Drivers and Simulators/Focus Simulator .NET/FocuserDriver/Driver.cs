//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Focuser driver for FocuserSimulator
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
        private bool _Link, _IsMoving;

        //
        // Constructor - Must be public for COM registration!
        //
        public Focuser()
        {
            // TODO Implement your additional construction here
            _Link = false;
            _IsMoving = false;
            Properties.Settings.Default.Reload();
        }

        //
        // PUBLIC COM INTERFACE IFocuser IMPLEMENTATION
        //

        #region IFocuser Members

        public bool Absolute
        {
            get { return Properties.Settings.Default.sAbsolute; }
        }

        public void Halt()
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("Halt");
        }

        public bool IsMoving
        {
            // TODO Replace this with your implementation
            get { return _IsMoving; }
        }

        public bool Link
        {
            get { return _Link; }
            set { _Link = value; }
        }

        public int MaxIncrement
        {
            get { return (int)Properties.Settings.Default.sMaxIncrement; }
        }

        public int MaxStep
        {
            get { return (int)Properties.Settings.Default.sMaxStep; }
        }


        private void Deplace(int pStep)
        {
            _IsMoving = true;
            Thread.Sleep(2);
            _IsMoving = false;
        }

        public void Move(int val)
        {
            // TODO Replace this with your implementation
            if (Properties.Settings.Default.sAbsolute)
            {
                if (val == Properties.Settings.Default.sPosition) return;
                if (val > Properties.Settings.Default.sPosition)
                {
                    for (int i = Properties.Settings.Default.sPosition; i < val; i += (int)Properties.Settings.Default.sStepSize)
                    {
                        Deplace((int)Properties.Settings.Default.sStepSize);
                        Properties.Settings.Default.sPosition += (int)Properties.Settings.Default.sStepSize;
                    }
                }
                else
                {
                    for (int i = Properties.Settings.Default.sPosition; i > val; i -= (int)Properties.Settings.Default.sStepSize)
                    {
                        Deplace((int)Properties.Settings.Default.sStepSize);
                        Properties.Settings.Default.sPosition -= (int)Properties.Settings.Default.sStepSize;
                    }
                }
                
            }
        }

        public int Position
        {
            get 
            {
                if (Properties.Settings.Default.sAbsolute) return Properties.Settings.Default.sPosition;
                else throw new PropertyNotImplementedException("Position", false); 
            }
        }

        public void SetupDialog()
        {
            SetupDialogForm F = new SetupDialogForm();
            F.ShowDialog();
        }

        public double StepSize
        {
            get { return (double)Properties.Settings.Default.sStepSize; }
        }

        public bool TempComp
        {
            get { return Properties.Settings.Default.sTempComp; }
            set 
            { 
                
                if (Properties.Settings.Default.sTempCompAvailable) Properties.Settings.Default.sTempComp = value;
                else throw new MethodNotImplementedException("TempComp");
            }
        }

        public bool TempCompAvailable
        {
            get { return Properties.Settings.Default.sTempCompAvailable; }
        }

        public double Temperature
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("Temperature", false); }
        }

        #endregion
    }
}
