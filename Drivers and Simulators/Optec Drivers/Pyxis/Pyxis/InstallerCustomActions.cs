using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using Optec;
using System;

namespace ASCOM.Pyxis
{
    /// <summary>
    /// Custom install actions that must be carried out during product installation.
    /// </summary>
    [RunInstaller(true)]
    public class ComRegistration : Installer
    {
        /// <summary>
        /// Custom Install Action that regsiters the driver with COM Interop.
        /// Note that this will in turn trigger an methods with the [ComRegisterFunction()] attribute
        /// such as those in Driver.cs that perform ASCOM registration.
        /// </summary>
        /// <param name="stateSaver">Not used.<see cref="Installer"/></param>
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            string msg = "Install custom action - Starting registration for COM Interop";
            Trace.WriteLine(msg);
            
#if DEBUG
            MessageBox.Show("Attach debugger to this process now, if required", "Custom Action Debug", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

#endif
            base.Install(stateSaver);

            RegistrationServices regsrv = new RegistrationServices();
            if (!regsrv.RegisterAssembly(this.GetType().Assembly, AssemblyRegistrationFlags.SetCodeBase))
            {
                Trace.WriteLine("COM registration failed");
    
                throw new InstallException("Failed To Register driver for COM Interop");
            }
            Trace.WriteLine("Completed registration for COM Interop");
        }

        /// <summary>
        /// Custom Install Action that removes the COM Interop component registrations.
        /// Note that this will in turn trigger any methods with the [ComUnregisterFunction()] attribute
        /// such as those in Driver.cs that remove the ASCOM registration.
        /// </summary>
        /// <param name="savedState">Not used.<see cref="Installer"/></param>
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            Trace.WriteLine("Uninstall custom action - unregistering from COM Interop");
            try
            {
                base.Uninstall(savedState);
                RegistrationServices regsrv = new RegistrationServices();
                if (!regsrv.UnregisterAssembly(this.GetType().Assembly))
                {
                    Trace.WriteLine("COM Interop deregistration failed");
                    throw new InstallException("Failed To Unregister from COM Interop");
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }   
            finally
            {
                Trace.WriteLine("Completed uninstall custom action");
            }
        }

        protected override void OnBeforeInstall(System.Collections.IDictionary savedState)
        {
            // Try to call get
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles), "\\ASCOM\\.net\\ASCOM.Utilities.dll")))
            {
                Trace.WriteLine("ASCOM.Utilities.dll Found! Platform 5.5 is installed!");
            }
            else
            {
                Trace.WriteLine("ASCOM.Utilities.dll NOT Found! Platform 5.5 is NOT installed!");
                throw new InstallException("The ASCOM Platform 5.5 is required for this ASCOM driver however the platform can not be found on this PC. " +
                    "The install process will now be cancelled. Please install the ASCOM Platform before continuing.");
            }
            base.OnBeforeInstall(savedState);
        }
    }
}