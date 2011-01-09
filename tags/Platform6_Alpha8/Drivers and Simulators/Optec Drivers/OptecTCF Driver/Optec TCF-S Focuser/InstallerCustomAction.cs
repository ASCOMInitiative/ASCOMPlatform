using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using Optec;


namespace Optec_TCF_S_Focuser
{
    [RunInstaller(true)]
    public partial class InstallerCustomAction : System.Configuration.Install.Installer
    {
        public override void Install(IDictionary stateSaver)
        {
            // This is required because the installer runs with elevated privileges
            //  and if an EventLog needs to be created it must be done when provileges are
            //  elevated!
            string msg = "Running TCF-S Control custom action";
            EventLogger.LogMessage(msg, System.Diagnostics.TraceLevel.Warning);

            base.Install(stateSaver);
        }
    }
}
