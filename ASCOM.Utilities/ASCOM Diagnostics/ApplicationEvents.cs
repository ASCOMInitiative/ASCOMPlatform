using System.Windows.Forms;

namespace ASCOM.Utilities.My
{

    // The following events are available for MyApplication:
    // 
    // Startup: Raised when the application starts, before the startup form is created.
    // Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    // UnhandledException: Raised if the application encounters an unhandled exception.
    // StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    // NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    internal partial class MyApplication
    {
        // Catch an unhandled exception.
        private void MyApplication_UnhandledException(object sender, Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs e)
        {
            // If the user clicks No, then exit.
            e.ExitApplication = MessageBox.Show(e.Exception.ToString() + "\r\n" + "Continue?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No;
        }
    }


}