using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using Optec;


namespace PyxisLE_Control
{
    [RunInstaller(true)]
    public partial class InstallerCustomActions : System.Configuration.Install.Installer
    {
        public override void Install(IDictionary stateSaver)
        {
            // This ensures that the log gets created during the install process.
            EventLogger.LogMessage("Running Pyxis LE Control Installer Custom Action.", System.Diagnostics.TraceLevel.Error);
            base.Install(stateSaver);
        }
    }
}
