using System.Management.Automation;
using ASCOM.Utilities;


namespace ASCOM.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Show, "Chooser")]
    public class AscomChooser : Cmdlet
    {
        #region private fields

        private string m_deviceType = null;
        private string m_driverId = null; 
        private Chooser m_chooser;
    
        #endregion private fields

        #region Parameters
        /// <summary>
        /// The device type (Telescope, Camera, etc) that the Chooser will display.
        /// Default value = Telescope
        /// </summary>
        [Parameter(Position = 0,
                    Mandatory = false,
                    ValueFromPipeline = true,
                    HelpMessage = "ASCOM device type (Telescope[default], Focuser, Dome, etc)")]
        [ValidateNotNullOrEmpty]
        public string DeviceType
        {
            get { return m_deviceType; }
            set { m_deviceType = value; }
        }

        /// <summary>
        /// The device driver to display in list by default
        /// </summary>
        [Parameter()]
        public string DriverId
        {
            get { return m_driverId; }
            set { m_driverId = value; }
        }

        #endregion parameters

        #region protected overrides

        protected override void BeginProcessing()
        {
            //base.BeginProcessing();
            m_chooser = new Chooser();
        }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();
            //WriteObject("Hello World!");
            if (m_deviceType != null) m_chooser.DeviceType = m_deviceType;
            WriteObject(m_chooser.Choose(m_driverId));

        }

        protected override void EndProcessing()
        {
            //base.EndProcessing();
            m_chooser.Dispose();
            m_chooser = null;
        }

        #endregion protected overrides

    }
}
