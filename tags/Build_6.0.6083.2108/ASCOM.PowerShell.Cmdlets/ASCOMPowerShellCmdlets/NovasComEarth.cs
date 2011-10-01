using System.Management.Automation;
using ASCOM.Astrometry.NOVASCOM;

namespace ASCOM.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "Earth")]
    public class NovasComEarth : Cmdlet
    {
        #region private fields

        private Earth m_earth;
        private double m_tjd;

        #endregion private fields

        #region parameters

        /// <summary>
        /// Optional Terrestial Julian date to initialise component
        /// </summary>
        [Parameter(Position = 0,
                    Mandatory = false,
                    ValueFromPipeline = true,
                    HelpMessage = "Initialize the Earth object for given terrestrial Julian date")]
        [ValidateNotNullOrEmpty]
        public double tjd
        {
            get { return m_tjd; }
            set { m_tjd = value; }
        }

        #endregion parameters

        #region protected overrides

        protected override void ProcessRecord()
        {
            m_earth = new Earth();
            if (m_tjd != 0)
                m_earth.SetForTime(m_tjd);
            WriteObject(m_earth);
        }

        #endregion protected overrides
    }
}
