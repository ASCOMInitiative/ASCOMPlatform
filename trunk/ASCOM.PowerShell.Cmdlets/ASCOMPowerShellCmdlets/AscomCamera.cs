using System.Management.Automation;
using ASCOM.DriverAccess;

namespace ASCOM.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "Camera")]
    public class AscomCamera : Cmdlet
    {
        #region private fields

        private string m_driverId = null;
        private Camera m_camera;

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
            m_camera = new Camera(m_driverId);
            WriteObject(m_camera);
        }

        #endregion protected overrides
    }
}
