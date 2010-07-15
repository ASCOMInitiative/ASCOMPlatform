using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.OptecTCF_S
{
    public static class Logger
    {
        public static ASCOM.Utilities.TraceLogger TLogger =
            new ASCOM.Utilities.TraceLogger(string.Empty, "Focuser-Optec TCF-S");

        static Logger()
        {
            TLogger.Enabled = true;
        }
        
    }
}
