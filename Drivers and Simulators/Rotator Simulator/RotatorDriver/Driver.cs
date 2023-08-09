//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Rotator driver for RotatorSimulator
//
// Implements:	ASCOM Rotator interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Version	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Rotator Driver template
// --------------------------------------------------------------------------------
//
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;
using System.Collections;
using System;

namespace ASCOM.Simulator
{
    // The Guid attribute sets the CLSID for ASCOM.Simulator.Rotator
    // The ClassInterface/None attribute prevents an empty interface called _Rotator from being created and used as the [default] interface

    [Guid("347B5004-3662-42C0-96B8-3F8F6F0467D2")]
    [ServedClassName("Rotator Simulator .NET")]
    [ProgId("ASCOM.Simulator.Rotator")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Rotator : ReferenceCountedObjectBase, IRotatorV4
    {
        //
        // Constructor - Must be public for COM registration!
        //
        public Rotator()
        {
        }

        //
        // PUBLIC COM INTERFACE IRotator IMPLEMENTATION
        //
        #region IDeviceControl Members
        /// <summary>
        /// Invokes the specified device-specific action.
        /// </summary>
        /// <exception cref="MethodNotImplementedException"></exception>
        public string Action(string actionName, string actionParameters)
        {
            throw new ASCOM.ActionNotImplementedException(actionName);
        }

        public void CommandBlind(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandBlind " + command);
        }

        public bool CommandBool(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandBool " + command);
        }

        public string CommandString(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandString " + command);
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        public ArrayList SupportedActions
        {
            // no supported actions, return empty array
            get { ArrayList sa = new ArrayList(); return sa; }
        }

        #endregion

        #region IRotator Members
        public bool Connected
        {
            get { return RotatorHardware.Connected; }
            set { RotatorHardware.Connected = value; }
        }

        public string Description
        {
            get { return RotatorHardware.Description; }
        }

        public string DriverInfo
        {
            get { return RotatorHardware.DriverInfo; }
        }

        public string DriverVersion
        {
            get { return RotatorHardware.DriverVersion; }
        }

        public short InterfaceVersion
        {
            get { return RotatorHardware.InterfaceVersion; }
        }

        public string Name
        {
            get { return RotatorHardware.RotatorName; }
        }

        public bool CanReverse
        {
            get { return RotatorHardware.CanReverse; }
        }

        public void Halt()
        {
            RotatorHardware.Halt();
        }

        public bool IsMoving
        {
            get { return RotatorHardware.IsMoving; }
        }

        public void Move(float position)
        {
            RotatorHardware.Move(position);
        }

        public void MoveAbsolute(float position)
        {
            RotatorHardware.MoveAbsolute(position);
        }

        public float Position
        {
            get { return RotatorHardware.Position; }
        }

        public bool Reverse
        {
            get { return RotatorHardware.Reverse; }
            set { RotatorHardware.Reverse = value; }
        }

        public void SetupDialog()
        {
            if (RotatorHardware.Connected) throw new DriverException("The rotator is connected, cannot do SetupDialog()", unchecked(ErrorCodes.DriverBase + 4));

            RotatorHardware.SetupDialog();
        }

        public float StepSize
        {
            get { return RotatorHardware.StepSize; }
        }

        public float TargetPosition
        {
            get { return RotatorHardware.TargetPosition; }
        }

        #endregion

        #region IRotatorV3 members

        public float MechanicalPosition
        {
            get { return RotatorHardware.InstrumentalPosition; }
        }

        public void Sync(float position)
        {
            RotatorHardware.Sync(position);
        }

        public void MoveMechanical(float position)
        {
            RotatorHardware.MoveMechanical(position);
        }

        #endregion

        #region IRotatorV4 members

        public void Connect()
        {
            Connected = true;
        }

        public void Disconnect()
        {
            Connected = false;
        }

        public bool Connecting
        {
            get
            {
                return false;
            }
        }

        public ArrayList DeviceState
        {
            get
            {
                ArrayList deviceState = new ArrayList();

                try { deviceState.Add(new StateValue(nameof(IRotatorV4.IsMoving), IsMoving)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IRotatorV4.MechanicalPosition), MechanicalPosition)); } catch { }
                try { deviceState.Add(new StateValue(nameof(IRotatorV4.Position), Position)); } catch { }
                try { deviceState.Add(new StateValue(DateTime.Now)); } catch { }

                return deviceState;
            }
        }

        #endregion

    }
}
