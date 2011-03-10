using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;
using System.Drawing;

namespace Optec
{
    public class NewVersionChecker
    {
        public event EventHandler NewVersionDetected;
        private string productName = String.Empty;
        private Version asmVersion = new Version(0, 0);
        private bool showResultsIfUpToDate = false;
        private string url = string.Empty;
        private Version latestVersionNumber = new Version(0, 0);
        private Thread versionCheckerThread;
        private Icon iconImg;

        public void CheckForNewVersion(Assembly ExecutingAssembly, string ProductName, bool ShowResultsIfUpToDate, Icon IconImg)
        {
            try
            {
                this.iconImg = IconImg;
                AssemblyName asmName = ExecutingAssembly.GetName();
                this.asmVersion = asmName.Version;
                this.productName = ProductName;
                showResultsIfUpToDate = ShowResultsIfUpToDate;
                if (showResultsIfUpToDate)
                    checkForNewerVersion();
                else
                {
                    ThreadStart ts = new ThreadStart(checkForNewerVersion);
                    versionCheckerThread = new Thread(ts);
                    versionCheckerThread.Start();
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                throw;
            }
            
        }

        private void checkForNewerVersion()
        {
            if (tryToGetLatestVersionFromServer())
            {
                if (asmVersion.CompareTo(latestVersionNumber) < 0)
                {
                    // A newer version is available
                    NewVersionFrm nvfrm = new NewVersionFrm(this.asmVersion.ToString(), this.latestVersionNumber.ToString(), url, this.iconImg);
                    TriggerAnEvent(NewVersionDetected, nvfrm);
                }
                else
                {
                    // The program is up to date!
                    if (showResultsIfUpToDate)
                    {
                        UpToDateForm utdfrm = new UpToDateForm(this.iconImg);
                        TriggerAnEvent(NewVersionDetected, utdfrm);
                    }
                }
            }
            else
            {
                // Unable to find latest verison
                if (showResultsIfUpToDate)
                {
                    throw new ApplicationException("Unable to connect to Optec Server. Is your PC connected to the internet?");
                }
            }
        }

        private bool tryToGetLatestVersionFromServer()
        {
            try
            {
                string xmlURL = "http://www.optecinc.com/astronomy/software/download/ascom/CurrentAppVersion.xml";
                XDocument xdoc = XDocument.Load(xmlURL);
                XElement v = xdoc.Descendants("versionGetter").Descendants(productName).Descendants().Single(i => i.Name == "version");
                XElement u = xdoc.Descendants("versionGetter").Descendants(productName).Descendants().Single(i => i.Name == "url");
                latestVersionNumber = new Version(v.Value);
                url = u.Value;
                return true;
            }
            catch 
            {
                return false;
            }
        }

        private void TriggerAnEvent(EventHandler EH, object e)
        {
            if (EH == null) return;
            var EventListeners = EH.GetInvocationList();
            if (EventListeners != null)
            {
                for (int index = 0; index < EventListeners.Count(); index++)
                {

                    var methodToInvoke = (EventHandler)EventListeners[index];
                    methodToInvoke.BeginInvoke(e, EventArgs.Empty, EndAsyncEvent, new object[] { });

                }
            }

        }

        private void EndAsyncEvent(IAsyncResult iar)
        {
            var ar = (System.Runtime.Remoting.Messaging.AsyncResult)iar;
            var invokedMethod = (EventHandler)ar.AsyncDelegate;

            try
            {
                invokedMethod.EndInvoke(iar);
            }
            catch
            {
                // Handle any exceptions that were thrown by the invoked method
                Console.WriteLine("An event listener went kaboom!");
            }
        }
    }
}
