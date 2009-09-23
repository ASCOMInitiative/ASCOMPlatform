using System;
using System.Collections.Generic;
using System.Text;
using ASCOM.Utilities;

namespace ASCOM.OptecTCF_Driver
{
    class DeviceSettings
    {
        private static ASCOM.Utilities.Profile Prof;
        const string ProfileString_LastPort = "LastUsedCOMPort";
        private static bool PortSelected;
        private static string PortToUse;

        static DeviceSettings()
        {
            PortSelected = false;
            PortToUse = "";

            Prof = new ASCOM.Utilities.Profile();
            Prof.DeviceType = "Focuser";

            PortToUse = Prof.GetValue(Focuser.s_csDriverID, ProfileString_LastPort, "", "");
            if (PortToUse == "") PortSelected = true;
        }



    }
}
