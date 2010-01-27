using System;
using System.Collections.Generic;
using System.Text;
using ASCOM.Utilities;

namespace ASCOM.OptecTCF_Driver
{
    class DeviceSettings
    {
        private static ASCOM.Utilities.Profile Prof;

        private enum ProfileStrings
        {
            LastUsedCOMPort,
            AutoMode,
            DeviceType,
            FirmwareVersion,

            StartPtDateTime,
            StartPtTemp,
            StartPtPos,

            EndPtDateTimeA,
            EndPtTempA,
            EndPtPosA,

            EndPtDateTimeB,
            EndPtTempB,
            EndPtPosB,

            ModeAName,
            ModeBName,

            DelayA,
            DelayB
        }

        static DeviceSettings() //Constructor
        {
            try
            {
                t_PortSelected = false;
                t_TempProbePresent = true;
                Prof = new ASCOM.Utilities.Profile(true);
                Prof.DeviceType = "Focuser";

                ///Check if a port has been selected in the past
                string PortToUse;
                PortToUse = Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.LastUsedCOMPort.ToString(), "", "");
                if (PortToUse == "") t_PortSelected = false;
                else if (PortToUse.Substring(0, 3) == "COM" && PortToUse.Length > 3)
                {
                    t_PortSelected = true;
                    t_LastPort = PortToUse;
                }
                else
                {
                    throw new ApplicationException("The COM Port string stored in the config does not " +
                        "contain COMnn.\n The method which stores the COM string contains an error");
                }

                
            }
            catch (Exception Ex)
            {
                throw new DriverException("\nError initializing device settings. \n " + Ex.ToString(), Ex);
            }
        }

        #region Device Properties

        private static bool t_PortSelected;
        private static string t_LastPort;
        private static bool t_TempProbePresent;

        internal static bool TempProbePresent
        {
            get { return t_TempProbePresent; }
            set { t_TempProbePresent = value; }
        }

        internal static bool PortSelected
        {
            get { return t_PortSelected; }
            set { t_PortSelected = value; }
        }

        internal static string PortName
        {
            get
            {
               return Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.LastUsedCOMPort.ToString());
            }
        }

        #endregion

        #region DeviceSettings Methods

        internal static void SetCOMPort(string p)
        {
            if (p == "") PortSelected = false;
            else
            {
                Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.LastUsedCOMPort.ToString(), p, "");
                PortSelected = true;
            }
        }

        internal static char GetActiveMode()
        {
            string AorB = Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.AutoMode.ToString(), "", "A");
            if (char.Parse(AorB) != 'A' && char.Parse(AorB) != 'B')
                throw new InvalidValueException("AorB", AorB.ToString(), "A or B");
            return char.Parse(AorB);
        }

        internal static void SetActiveMode(char AorB)
        {
            if (AorB != 'A' && AorB != 'B')
                throw new InvalidValueException("AorB", AorB.ToString(), "A or B");
            else
            {
                Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.AutoMode.ToString(), AorB.ToString());
            }

        }

        internal static string GetComPort()
        {
            t_LastPort = Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.LastUsedCOMPort.ToString(), "", "");
            return t_LastPort;
        }

        internal static SlopePoint GetStartPoint()
        {    
            SlopePoint tempPt = new SlopePoint();
            
            tempPt.Position = int.Parse(Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.StartPtPos.ToString(), "", "11111"));
            tempPt.Temperature = double.Parse(Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.StartPtTemp.ToString(), "", "11111"));
            
            tempPt.DateAndTime = DateTime.Parse(Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.StartPtDateTime.ToString(), "", "7/17/1985"));
            return tempPt; 
        }

        internal static void SetStartPoint(SlopePoint StPt)
        {
            string Pos = ""; string Temp = ""; string DateTime = "";
            Pos = StPt.Position.ToString();
            Temp = StPt.Temperature.ToString();
            DateTime = StPt.DateAndTime.ToString();
            Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.StartPtPos.ToString(), Pos, "");
            Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.StartPtTemp.ToString(), Temp, "");
            Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.StartPtDateTime.ToString(), DateTime, "");      
        }

        internal static string CalculateSlope(int Pos1, double Temp1, int Pos2, double Temp2)
        {
            if (Temp1 == Temp2) throw new InvalidOperationException("Temperature1 and Temperature2 can not be equal.\n");
            //if (Pos1 == Pos2) throw new InvalidOperationException("Position1 and Position2 can not be equal.\n");

            //Calculate Slope - Slope = Steps per Degree
            int slope = (Pos1 - Pos2) / (Convert.ToInt32(Temp2) - Convert.ToInt32(Temp1));
            char SlopeSign;
            if (slope < 0) SlopeSign = '-';
            else SlopeSign = '+';

            slope = Math.Abs(slope);

            if (slope < 0 || slope > 999)
            {
                //MessageBox.Show("Could not save time constant to device. TC must be between 2 and 99.\n"
                //    + "Calculated TC = " + slope.ToString() + ".\n");
                throw new InvalidOperationException("ABS(Slope) must be beween 000 and 999");
            }
            return SlopeSign.ToString() + slope.ToString().PadLeft(3, '0');
        }

        internal static void SetModeNames(string A_Name, string B_Name)
        {
            Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.ModeAName.ToString(), A_Name);
            Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.ModeBName.ToString(), B_Name);
        }

        internal static string GetModeName(char AorB)
        {
            if (AorB != 'A' && AorB != 'B') throw new InvalidValueException("AorB", AorB.ToString(), "A or B");
            else if (AorB == 'A')
            {
                return Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.ModeAName.ToString(), "", "Temp Comp Mode A");
            }
            else
            {
                return Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.ModeBName.ToString(), "", "Temp Comp Mode B");
            }
        }

        internal static double GetDelayFromConfig(char AorB)
        {
            if (AorB != 'A' && AorB != 'B') throw new InvalidValueException("AorB", AorB.ToString(), "A or B");
            else if (AorB == 'A')
            {
                string d = Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.DelayA.ToString(), "", "1.00");
                return double.Parse(d);
            }
            else
            {
                string d = Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.DelayB.ToString(), "", "1.00");
                return double.Parse(d);
            }
        }

        internal static void SetDelayToConfig(char AorB, double delay)
        {
            if (AorB != 'A' && AorB != 'B') throw new InvalidValueException("AorB", AorB.ToString(), "A or B");
            else if (AorB == 'A')
            {
                Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.DelayA.ToString(), delay.ToString());
            }
            else
            {
                Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.DelayB.ToString(), delay.ToString());
            }
        }

        internal static string GetDeviceType()
        { 
            return Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.DeviceType.ToString(), "", "?");
        }

        internal static void SetDeviceType(string type)
        {
            Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.DeviceType.ToString(), type);
        }

        internal static string GetFirmwareVersion()
        {
            return Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.FirmwareVersion.ToString(), "", "?");
        }

        internal static void SetFirmwareVersion(string version)
        {
            Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.FirmwareVersion.ToString(), version);
        }

        internal static int GetMaxStep()
        {
            string DevType = "";
            DevType = GetDeviceType();

            if (DevType == "?")
            {
                throw new DriverException("A Device Type has not been selected in the SetupDialog");
            }

            if (DevType == "TCF-S3" || DevType == "TCF-S3i")
                return 10000;
            else if (DevType == "TCF-S" || DevType == "TCF-Si")
            {
                return 7000;
            }
            else
            {
                throw new InvalidResponse("\nUnacceptable Device Type. Expected TCF-S, TCF-S3, TCF-Si or TCF-S3i.\n" +
                     "Stored type is: " + DevType + ".\n");
            }
            
        }

        //internal static SlopePoint GetEndPoint(char AorB)
        //{
        //    if (AorB != 'A' && AorB != 'B') throw new InvalidValueException("AorB", AorB.ToString(), "A or B");
        //    else if (AorB == 'A')
        //    {
        //        SlopePoint tempPt = new SlopePoint();
        //        tempPt.Position = int.Parse(Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.EndPtPosA.ToString(), "", "11111"));
        //        tempPt.Temperature = int.Parse(Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.EndPtPosA.ToString(), "", "11111"));
        //        tempPt.DateAndTime = DateTime.Parse(Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.EndPtDateTimeA.ToString(), "", "7/17/1985"));
        //        return tempPt;
        //    }
        //    else
        //    {
        //        SlopePoint tempPt = new SlopePoint();
        //        tempPt.Position = int.Parse(Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.EndPtPosB.ToString(), "", "11111"));
        //        tempPt.Temperature = int.Parse(Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.EndPtPosB.ToString(), "", "11111"));
        //        tempPt.DateAndTime = DateTime.Parse(Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.EndPtDateTimeB.ToString(), "", "7/17/1985"));
        //        return tempPt;
        //    }
        //}

        //internal static void SetEndPoint(SlopePoint EndPt, char AorB)
        //{
        //    if (AorB != 'A' && AorB != 'B') throw new InvalidValueException("AorB", AorB.ToString(), "A or B");
        //    else if (AorB == 'A')
        //    {
        //        string Pos = ""; string Temp = ""; string DateTime = "";
        //        Pos = EndPt.DateAndTime.ToString();
        //        Temp = EndPt.Temperature.ToString();
        //        DateTime = EndPt.Temperature.ToString();
        //        Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.EndPtPosA.ToString(), Pos);
        //        Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.EndPtTempA.ToString(), Temp);
        //        Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.EndPtDateTimeA.ToString(), DateTime);
        //    }
        //    else
        //    {
        //        string Pos = ""; string Temp = ""; string DateTime = "";
        //        Pos = EndPt.DateAndTime.ToString();
        //        Temp = EndPt.Temperature.ToString();
        //        DateTime = EndPt.Temperature.ToString();
        //        Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.EndPtPosB.ToString(), Pos);
        //        Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.EndPtTempB.ToString(), Temp);
        //        Prof.WriteValue(Focuser.s_csDriverID, ProfileStrings.EndPtDateTimeB.ToString(), DateTime);
        //    }
        //}

        #endregion
    }

    class SlopePoint
    {
        int s_pos;
        double s_temp;
        DateTime s_dateTime;

        internal SlopePoint()
        {
            s_pos = 0;
            s_temp = 0;
            s_dateTime = DateTime.Now;
        }

        internal int Position
        {
            get { return s_pos; }
            set{ s_pos = value; }
        }

        internal double Temperature
        {
            get { return s_temp; }
            set { s_temp = value; }
        }

        internal DateTime DateAndTime
        {
            get { return s_dateTime; }
            set { s_dateTime = value; }
        }
        
    }

}
