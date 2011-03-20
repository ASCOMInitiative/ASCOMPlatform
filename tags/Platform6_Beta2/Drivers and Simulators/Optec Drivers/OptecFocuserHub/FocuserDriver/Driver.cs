//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Focuser driver for OptecFocuserHub
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
using ASCOM.Utilities;
using ASCOM.Interface;
using ASCOM.OptecFocuserHubTools;

namespace ASCOM.OptecFocuserHub
{
    //
    // Your driver's ID is ASCOM.OptecFocuserHub.Focuser
    //
    // The Guid attribute sets the CLSID for ASCOM.OptecFocuserHub.Focuser
    // The ClassInterface/None addribute prevents an empty interface called
    // _Focuser from being created and used as the [default] interface
    //
    [Guid("e1028dba-d482-4620-8142-dd1385d0661a")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : ReferenceCountedObjectBase, ASCOM.Interface.IFocuser
    {
        private FocuserManager myFocuserManager;
        private HubFocuser myFocuser;
        //
        // Constructor - Must be public for COM registration!
        //
        public Focuser()
        {
            myFocuserManager = ASCOM.OptecFocuserHub.SharedResources.SharedFocuserManager;
            myFocuser = SharedResources.SharedFocuserManager.Focuser1;
        }



        //
        // PUBLIC COM INTERFACE IFocuser IMPLEMENTATION
        //

        #region IFocuser Members

        public bool Absolute
        {
            // TODO Replace this with your implementation
            get { return true; }
        }

        public void Halt()
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("Halt");
        }

        public bool IsMoving
        {
            // TODO Replace this with your implementation
            get { return myFocuser.IsMoving || myFocuser.IsHoming; }
        }

       
        public bool Link
        {
            // TODO Replace this with your implementation

            get { return myFocuserManager.Connected; }
            set { myFocuserManager.Connected = value; }
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
            myFocuser.MoveAbsolute(val);
        }

        public int Position
        {
            // TODO Replace this with your implementation
            get { return myFocuser.CurrentPositionSteps; }
        }

        public void SetupDialog()
        {
            SetupDialogForm sdf = new SetupDialogForm();
            sdf.ShowDialog();
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
            get
            {
                myFocuser.RefreshStatus();
                return myFocuser.CurrentTempC; }
        }

        #endregion
    }
}
