using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ASCOM.FocuserSimulator
{
    public class FocuserHardware
    {
        public enum eLogKind { LogMove, LogTemp, LogIsMoving, LogOther };

        public static bool IsTempCompMove;
        private static bool _Link;
        public static bool HaltRequested;
        public static bool _LogHaltMove, _LogIsMoving, _LogTempRelated, _LogOthers;

        #region Focuser.Constructor
        static FocuserHardware()
		{
            Properties.Settings.Default.Reload();
            _Link = false;
            IsTempCompMove = false;
            HaltRequested = false;
            Properties.Settings.Default.IsMoving = false;
            _LogHaltMove = false;
            _LogIsMoving = false;
            _LogTempRelated = false;
            _LogOthers = false;
        }
        #endregion

        #region Focuser.SetupDialog() method
        public static void DoSetup()
        {
            SetupDialogForm F = new SetupDialogForm();
            F.ShowDialog();
        }
        #endregion

        #region Focuser.Link() method
        public static bool Link
        {
            get { return _Link; }
            set { _Link = value; }
        }
        #endregion

        #region Focuser.MaxIncrement property
        public static int MaxIncrement
        {
            get { return (int)Properties.Settings.Default.sMaxIncrement; }
        }
        #endregion

        #region Focuser.MaxStep property
        public static int MaxStep
        {
            get { return (int)Properties.Settings.Default.sMaxStep; }
        }
        #endregion

        #region Focuser.Position property
        public static int Position
        {
            get
            {
                if (Properties.Settings.Default.sAbsolute) return (int)Properties.Settings.Default.sPosition;
                else throw new PropertyNotImplementedException("Position", false);
            }
        }
        #endregion

        #region Focuser.StepSize property
        public static double StepSize
        {
            get 
            {
                if (!Properties.Settings.Default.sIsStepSize) { throw new PropertyNotImplementedException("StepSize", false); }
                else { return (double)Properties.Settings.Default.sStepSize; }
            }
        }
        #endregion

        #region Focuser.Absolute property
        public static bool Absolute
        {
            get { return Properties.Settings.Default.sAbsolute; }
        }
        #endregion

        #region Focuser.Halt() method
        public static void Halt()
        {
            if (!Properties.Settings.Default.sEnableHalt) { throw new MethodNotImplementedException("Halt"); }
            else { HaltRequested = true; }
        }
        #endregion

        #region Focuser.TempComp property
        public static bool TempComp
        {
            get 
            {
                MyLog(eLogKind.LogTemp, "Retrieving TempComp property");
                if (!Properties.Settings.Default.sTempCompAvailable) { return false; }
                else return Properties.Settings.Default.sTempComp; 
            }
            set
            {
                if (Properties.Settings.Default.sTempCompAvailable)
                {
                    MyLog(eLogKind.LogTemp, "Setting temperature compensation "+(value ? "ON" : "OFF"));
                    Properties.Settings.Default.sTempComp = value;
                }
                else throw new MethodNotImplementedException("TempComp");
            }
        }
        #endregion

        #region Focuser.TempCompAvailable property
        public static bool TempCompAvailable
        {
            get { return Properties.Settings.Default.sTempCompAvailable; }
        }
        #endregion

        #region Focuser.Temperature property
        public static double Temperature
        {
            get 
            {
                if (Properties.Settings.Default.sIsTemperature) { return (double)17.5; }
                else throw new PropertyNotImplementedException("Temperature", false);
            }
        }
        #endregion

        #region Focuser.IsMoving property
        public static bool IsMoving
        {
            get { return Properties.Settings.Default.IsMoving; }
        }
        #endregion

        #region Focuser.Move() method
        public static void Move(int val)
        {
            int DestPosition = 0;

            if (!IsTempCompMove)
            {
                if (Properties.Settings.Default.sTempComp) // No moves when in t° compensation mode
                {
                    MyLog(eLogKind.LogOther, "No move allowed because temperature compensation is ON");
                    return;
                }
                else
                {
                    MyLog(eLogKind.LogMove, "Move from " + Properties.Settings.Default.sPosition.ToString() + " to " + val.ToString());
                }
            }
            else
            {
                MyLog(eLogKind.LogMove, "Temperature compensation move : " + Properties.Settings.Default.sStepPerDeg.ToString() + " steps");
                val = (int)Properties.Settings.Default.sPosition + (int)Properties.Settings.Default.sStepPerDeg;
            }
            HaltRequested = false;
            if (Properties.Settings.Default.sAbsolute)
            {
                if (val == Properties.Settings.Default.sPosition) return;
                if (val < 0)
                {
                    MyLog(eLogKind.LogOther, "Error : no move because destination is < 0");
                    return;
                }
                DestPosition = (val > (int)Properties.Settings.Default.sMaxStep ? (int)Properties.Settings.Default.sMaxStep : val);
                Properties.Settings.Default.IsMoving = true;
                MyLog(eLogKind.LogIsMoving, "Focuser is moving");
                if (val > Properties.Settings.Default.sPosition)  
                {
                    for (int i = (int)Properties.Settings.Default.sPosition; i < val; i ++)
                    {
                        if (HaltRequested) 
                        { 
                            Properties.Settings.Default.IsMoving = false; 
                            HaltRequested = false;
                            MyLog(eLogKind.LogMove, "HALT requested, focuser has stopped");
                            return; 
                        }
                        Deplace(1);
                        Properties.Settings.Default.sPosition ++;
                    }
                }
                else
                {
                    for (int i = (int)Properties.Settings.Default.sPosition; i > val; i --)
                    {
                        if (HaltRequested) 
                        { 
                            Properties.Settings.Default.IsMoving = false; 
                            HaltRequested = false;
                            MyLog(eLogKind.LogMove, "HALT requested, focuser has stopped");
                            return; 
                        }
                        Deplace(1);
                        Properties.Settings.Default.sPosition --;
                    }
                }
                Properties.Settings.Default.IsMoving = false;
                MyLog(eLogKind.LogIsMoving, "Move done");
            }
            else  // Relative focuser move
            {
                Properties.Settings.Default.IsMoving = true;
                
                for (int i = 0; i < Math.Abs(val); i ++)
                {
                    // Focuser.Halt() was called
                    if (HaltRequested) 
                    { 
                        Properties.Settings.Default.IsMoving = false; 
                        HaltRequested = false; 
                        return; 
                    }
                    Deplace(1);  // Fake move
                }
                Properties.Settings.Default.IsMoving = false;
            }
        }
        #endregion


        #region private methods

        private static void Deplace(int pStep)
        {
            Thread.Sleep(1);
        }
        private static void MyLog(eLogKind Kind, string Texte)
        {
            if ((Properties.Settings.Default.LogHaltMove && Kind == eLogKind.LogMove) ||
                (Properties.Settings.Default.LogIsMoving && Kind == eLogKind.LogIsMoving) ||
                (Properties.Settings.Default.LogTempRelated && Kind == eLogKind.LogTemp) ||
                (Properties.Settings.Default.LogOther && Kind == eLogKind.LogOther))
            {
                Properties.Settings.Default.LogTxt += Texte + Environment.NewLine;
            }
        }

        #endregion
    }
}
