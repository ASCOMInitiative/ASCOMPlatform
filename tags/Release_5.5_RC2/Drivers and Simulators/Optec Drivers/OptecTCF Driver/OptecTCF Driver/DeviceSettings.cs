using System;
using System.Collections.Generic;
using System.Text;
using ASCOM.Utilities;

namespace ASCOM.OptecTCF_Driver
{
    class DeviceSettings
    {
        private static ASCOM.Utilities.Profile Prof;
        private const string ProfileString_LastPort = "LastUsedCOMPort";
        private const string ProfileString_AutoMode = "AutoMode";

        
        static DeviceSettings()
        {
            try
            {
                t_PortSelected = false;
                Prof = new ASCOM.Utilities.Profile(true);
                Prof.DeviceType = "Focuser";

                ///Check if a port has been selected in the past
                string PortToUse;
                PortToUse = Prof.GetValue(Focuser.s_csDriverID, ProfileString_LastPort, "", "");
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
               return Prof.GetValue(Focuser.s_csDriverID, ProfileString_LastPort);
            }
        }

        #endregion

        #region DeviceSettings Methods

        internal static void SetCOMPort(string p)
        {
            if (p == "") PortSelected = false;
            else
            {
                Prof.WriteValue(Focuser.s_csDriverID, ProfileString_LastPort, p, "");
                PortSelected = true;
            }
        }

        internal static char GetModeAorB()
        {
            throw new System.NotImplementedException();
        }


        internal static string GetComPort()
        {
            t_LastPort = Prof.GetValue(Focuser.s_csDriverID, ProfileString_LastPort, "", "");
            return t_LastPort;
        }

        #endregion




    }
}
