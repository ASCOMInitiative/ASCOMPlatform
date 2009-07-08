using System;
using System.Threading;
using System.Diagnostics;

namespace ASCOM.FocuserSimulator
{
    public class FocuserHardware
    {
        #region Public variables
        public enum eLogKind { LogMove, LogTemp, LogIsMoving, LogOther };
        public static bool IsTempCompMove;
        public static bool HaltRequested;
        public static bool _LogHaltMove, _LogIsMoving, _LogTempRelated, _LogOthers;
        #endregion

        private static bool _Link;


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
            MyLog(eLogKind.LogOther, "Calling SetupDialog()");
            SetupDialogForm F = new SetupDialogForm();
            F.ShowDialog();
        }
        #endregion

        #region Focuser.Link() method
        public static bool Link
        {
            get { MyLog(eLogKind.LogOther, "Link property request"); return _Link; }
            set { MyLog(eLogKind.LogOther, "Set Link property to " + value.ToString()); _Link = value; }
        }
        #endregion

        #region Focuser.MaxIncrement property
        public static int MaxIncrement
        {
            get 
            {
                MyLog(eLogKind.LogOther, "MaxIncrement property request");
                return (int)Properties.Settings.Default.sMaxIncrement; 
            }
        }
        #endregion

        #region Focuser.MaxStep property
        public static int MaxStep
        {
            get 
            {
                MyLog(eLogKind.LogOther, "MaxStep property request");
                return (int)Properties.Settings.Default.sMaxStep; 
            }
        }
        #endregion

        #region Focuser.Position property
        public static int Position
        {
            get
            {
                MyLog(eLogKind.LogOther, "Position request");
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
                MyLog(eLogKind.LogOther, "StepSize property request");
                if (!Properties.Settings.Default.sIsStepSize) { throw new PropertyNotImplementedException("StepSize", false); }
                else { return (double)Properties.Settings.Default.sStepSize; }
            }
        }
        #endregion

        #region Focuser.Absolute property
        public static bool Absolute
        {
            get 
            {
                MyLog(eLogKind.LogOther, "Absolute property request");
                return Properties.Settings.Default.sAbsolute; 
            }
        }
        #endregion

        #region Focuser.Halt() method
        public static void Halt()
        {
            MyLog(eLogKind.LogMove, "HALT requested");
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
                    Properties.Settings.Default.Save();
                }
                else throw new MethodNotImplementedException("TempComp");
            }
        }
        #endregion

        #region Focuser.TempCompAvailable property
        public static bool TempCompAvailable
        {
            get 
            {
                MyLog(eLogKind.LogTemp, "TempCompAvailable request");
                return Properties.Settings.Default.sTempCompAvailable; 
            }
        }
        #endregion

        #region Focuser.Temperature property
        public static double Temperature
        {
            get 
            {
                MyLog(eLogKind.LogTemp, "Temperature request");
                if (Properties.Settings.Default.sIsTemperature) 
                {
                    return (double)17.5; 
                }
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

            //Properties.Settings.Default.Reload();
            if (!IsTempCompMove)
            {
                if (Properties.Settings.Default.sTempComp) // No moves when in t° compensation mode
                {
                    MyLog(eLogKind.LogOther, "No move because T° compensation is ON");
                    return;
                }
                else
                {
                    MyLog(eLogKind.LogMove, "Requested move from " + Properties.Settings.Default.sPosition.ToString() + " to " + val.ToString());
                }
            }
            else
            {
                MyLog(eLogKind.LogTemp, "T° compensation move");
            }
            HaltRequested = false;
            if (Properties.Settings.Default.sAbsolute)
            {
                if (val == Properties.Settings.Default.sPosition)
                {
                    MyLog(eLogKind.LogOther, "No move because destination = current");
                    return;
                }
                if (val < 0)
                {
                    MyLog(eLogKind.LogOther, "No move because destination is < 0");
                    return;
                }
                DestPosition = (val > (int)Properties.Settings.Default.sMaxStep ? (int)Properties.Settings.Default.sMaxStep : val);
                Properties.Settings.Default.IsMoving = true;
                int Start = (int)Properties.Settings.Default.sPosition;
                MyLog(eLogKind.LogIsMoving, "Moving from "+Start.ToString()+" to "+val.ToString());
                if (val > Properties.Settings.Default.sPosition)  
                {
                    for (int i = Start; i < DestPosition; i++)
                    {
                        if (HaltRequested) 
                        { 
                            Properties.Settings.Default.IsMoving = false; 
                            HaltRequested = false;
                            MyLog(eLogKind.LogMove, "Focuser stopped");
                            Properties.Settings.Default.Save();
                            return; 
                        }
                        FakeMove(1);
                        Properties.Settings.Default.sPosition ++;
                    }
                    
                }
                else
                {
                    for (int i = Start; i > val; i--)
                    {
                        if (HaltRequested) 
                        { 
                            Properties.Settings.Default.IsMoving = false; 
                            HaltRequested = false;
                            MyLog(eLogKind.LogMove, "Focuser stopped");
                            Properties.Settings.Default.Save();
                            return; 
                        }
                        FakeMove(-1);
                        Properties.Settings.Default.sPosition --;
                    }
                }
                
                Properties.Settings.Default.IsMoving = false;
                MyLog(eLogKind.LogIsMoving, "Move done");
            }
            else  // Relative focuser move
            {
                MyLog(eLogKind.LogMove,"Requested relative move "+val.ToString()+" steps");
                Properties.Settings.Default.IsMoving = true;
                MyLog(eLogKind.LogIsMoving, "Moving "+Math.Abs(val)+" steps "+(val >= 0 ? "forward" : "backward"));
                for (int i = 0; i < Math.Abs(val); i ++)
                {
                    // Focuser.Halt() was called
                    if (HaltRequested) 
                    { 
                        Properties.Settings.Default.IsMoving = false; 
                        HaltRequested = false;
                        MyLog(eLogKind.LogMove, "Focuser stopped");
                        Properties.Settings.Default.Save();
                        return; 
                    }
                    FakeMove(1);  // Fake move
                }
                Properties.Settings.Default.IsMoving = false;
                MyLog(eLogKind.LogIsMoving, "Relative move done");
            }
            Properties.Settings.Default.Save();
        }
        #endregion


        #region private methods

        /// <summary>
        /// Fake move. Use to add some delays between each motor step
        /// </summary>
        /// <param name="pStep">The number of steps.</param>
        private static void FakeMove(int pStep)
        {
            Thread.Sleep(1);
        }


        /// <summary>
        /// Logs events in the traffic window
        /// </summary>
        /// <param name="Kind">Kind of event.</param>
        /// <param name="Texte">Text describing the event.</param>
        public static void MyLog(eLogKind Kind, string Texte)
        {
            if (((Properties.Settings.Default.LogHaltMove) && (Kind == eLogKind.LogMove)) ||
               ((Properties.Settings.Default.LogIsMoving) && (Kind == eLogKind.LogIsMoving)) ||
               ((Properties.Settings.Default.LogTempRelated) && (Kind == eLogKind.LogTemp)) ||
               ((Properties.Settings.Default.LogOther) && (Kind == eLogKind.LogOther)))
            {
                //Trace.WriteLine(Texte, Kind.ToString());
                Trace.WriteLine(Texte);
            }
        }

        #endregion
    }


}
