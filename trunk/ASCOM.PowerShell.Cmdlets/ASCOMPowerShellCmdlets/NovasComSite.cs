using System.Management.Automation;
using ASCOM.Astrometry.NOVASCOM;

namespace ASCOM.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "Site")]
    public class NovasComSite : Cmdlet
    {
        #region private fields
        
        private Site m_site;
        private double m_latitude;
        private double m_longitude;
        private double m_height;

        #endregion private fields

        #region parameters
        /// <summary>
        /// Site latitude (double)
        /// </summary>
        [Parameter(Position = 0,
                    ValueFromPipeline = true,
                    HelpMessage = "Site latitude as a double")]
        [ValidateNotNullOrEmpty]
        public double latitude
        {
            get { return m_latitude; }
            set { m_latitude = value; }
        }
        /// <summary>
        /// Site longitude (double)
        /// </summary>
        [Parameter(Position = 1,
                    ValueFromPipeline = true,
                    HelpMessage = "Site longitude as a double")]
        [ValidateNotNullOrEmpty]
        public double longitude
        {
            get { return m_longitude; }
            set { m_longitude = value; }
        }
        /// <summary>
        /// Site longitude (double)
        /// </summary>
        [Parameter(Position = 2,
                    ValueFromPipeline = true,
                    HelpMessage = "Site height as a double")]
        [ValidateNotNullOrEmpty]
        public double height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        #endregion parameters

        #region protected overrides

        protected override void ProcessRecord()
        {
            m_site = new Site();
            if (m_latitude!=0 && m_longitude!=0 && m_height!=0)
                m_site.Set(m_latitude,m_longitude,m_height);

             WriteObject(m_site);
        }

        #endregion protected overrides
    }
}
