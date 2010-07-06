using System.Management.Automation;
using ASCOM.DriverAccess;

namespace ASCOM.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "Focuser")]
    public class AscomFocuser : Cmdlet
    {
        #region private fields

        private string m_driverId = null;
        private Focuser m_focuser;

        #endregion private fields

        #region parameters
        /// <summary>
        /// The device driver id to open
        /// </summary>
        [Parameter(Position = 0,
                    Mandatory = true,
                    ValueFromPipeline = true,
                    HelpMessage = "ASCOM driver id, as returned by Show-Chooser")]
        [ValidateNotNullOrEmpty]
        public string DriverId
        {
            get { return m_driverId; }
            set { m_driverId = value; }
        }

        #endregion parameters

        #region protected overrides

        protected override void ProcessRecord()
        {
            m_focuser = new Focuser(m_driverId);
            WriteObject(m_focuser);
        }

        #endregion protected overrides
    }
}
