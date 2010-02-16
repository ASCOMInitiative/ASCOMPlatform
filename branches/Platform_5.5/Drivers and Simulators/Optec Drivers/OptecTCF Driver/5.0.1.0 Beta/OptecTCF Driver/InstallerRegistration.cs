using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.OptecTCF_Driver
{
    [RunInstaller(true)]
    public class InstallerRegistration : Installer
    {

        public override void Install(IDictionary stateSaver)
        {
            Focuser.RegUnregASCOM(true);
            base.Install(stateSaver);
        }

        public override void Uninstall(IDictionary savedState)
        {

            Focuser.RegUnregASCOM(false);
            base.Uninstall(savedState);
        }

    }
}
