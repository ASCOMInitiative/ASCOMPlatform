using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.OptecTCF_Driver
{
    class DeviceComm
    {
        #region DeviceComm Properties

        internal static bool t_Connected;
        internal static bool Connected
        {
            get { return t_Connected; }
            set { t_Connected = value; }
        }

        #endregion

        #region DeviceComm Connection METHODS

        internal static void Connect()
        {
            throw new System.NotImplementedException();
        }

        internal static void Disconnect()
        {
            
        }

        #endregion

        #region DeviceComm Movement METHODS


        #endregion

    }
}
