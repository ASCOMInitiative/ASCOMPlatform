using System.Management.Automation;
using ASCOM.DriverAccess;

namespace ASCOM.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "Dome")]
    public class AscomDome : Cmdlet
    {

        #region private fields

        private string m_deviceId = null;
        private Dome m_dome;

        #endregion private fields

        #region parameters
        /// <summary>
        /// The device driver id to open
        /// </summary>
        [Parameter(Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            HelpMessage = "ASCOM device id, as returned by Show-Chooser")]
        [ValidateNotNullOrEmpty]
        public string DeviceId
        {
            get { return m_deviceId; }
            set { m_deviceId = value; }
        }

        #endregion parameters

        #region protected overrides

        protected override void ProcessRecord()
        {
            m_dome = new Dome(m_deviceId);
            WriteObject(m_dome);
        }

        #endregion protected overrides
    }
}
