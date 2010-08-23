using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.PyxisLE_ASCOM
{
    public static class Logger
    {
        public static ASCOM.Utilities.TraceLogger TLogger =
            new ASCOM.Utilities.TraceLogger(string.Empty, "Rotator - Optec Pyxis LE");

        static Logger()
        {
            TLogger.Enabled = true;
        }

    }
}