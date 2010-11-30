using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;


namespace Optec
{
    [RunInstaller(true)]
    public class InstallerCustomActions : System.Configuration.Install.Installer
    {
        public override void Install(IDictionary stateSaver)
        {
            
            string msg = "Installer Custom Action - Event Log Created During Install";
            EventLogger.LoggingLevel = TraceLevel.Verbose;
            EventLogger.LogMessage(msg, TraceLevel.Info);
            Trace.WriteLine(msg);

            // Abort Install if Platform is required and not found

            if (Context.Parameters["Ascom"].ToString().Contains("1"))
            {
                // If the platform is not found
                if (!Context.Parameters["PlatformFound"].ToString().Contains("1"))
                {
                    EventLogger.LogMessage("ASCOM Driver was requested for install but ASCOM Platform was not found. Install Rolling Back...", TraceLevel.Error);
                    throw new InstallException("The required version of the ASCOM Platform was not found on your PC. The install process will now be aborted." +
                        "Please install the ASCOM Platform now before continuing.");
                }
            }
            else 
            {
                EventLogger.LogMessage("ASCOM Driver not requested for Install.", TraceLevel.Info);
            }
            base.Install(stateSaver);
        }
    }
}
