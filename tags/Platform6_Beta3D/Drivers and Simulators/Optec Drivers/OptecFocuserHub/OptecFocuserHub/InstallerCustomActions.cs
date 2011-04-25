using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Diagnostics;


namespace ASCOM.OptecFocuserHub
{
    [RunInstaller(true)]
    public partial class InstallerCustomActions : System.Configuration.Install.Installer
    {
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            string cmd = System.Reflection.Assembly.GetExecutingAssembly().Location; 
            //System.Windows.Forms.MessageBox.Show(cmd, "/register");
            Process.Start(cmd, "/register");
        }

        public override void Uninstall(IDictionary savedState)
        {
            string cmd = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //System.Windows.Forms.MessageBox.Show(cmd, "/unregister");
            Process.Start(cmd, "/unregister");
            // Pause to give the unregister command time to execute
            System.Threading.Thread.Sleep(3000);
            base.Uninstall(savedState);
        }
    }
}
