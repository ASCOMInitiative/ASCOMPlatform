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

            StartPtDateTime,
            StartPtTemp,
            StartPtPos,

            EndPtDateTimeA,
            EndPtTempA,
            EndPtPosA,

            EndPtDateTimeB,
            EndPtTempB,
            EndPtPosB
        }

        static DeviceSettings()
        {
            try
            {
                t_PortSelected = false;
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

        internal static char GetModeAorB()
        {
            throw new System.NotImplementedException();
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
            tempPt.Temperature = int.Parse(Prof.GetValue(Focuser.s_csDriverID, ProfileStrings.StartPtTemp.ToString(), "", "11111"));
            
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
