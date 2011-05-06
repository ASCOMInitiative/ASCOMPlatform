using System.Management.Automation;
using ASCOM.Astrometry.NOVASCOM;

namespace ASCOM.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "Site")]
    public class NovasComSite : Cmdlet
    {
        #region private fields
        
        private Site m_site;
        private double m_latitude = -999;
        private double m_longitude = -999;
        private double m_height = -999;
        private double m_pressure = -999;
        private double m_temperature = -999;

        #endregion private fields

        #region parameters
        /// <summary>
        /// Site latitude (double)
        /// </summary>
        [Parameter(Position = 0,
                    ValueFromPipeline = true,
                    HelpMessage = "Site latitude (deg) as a double")]
        [Alias("lat")]
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
                    HelpMessage = "Site longitude (deg) as a double")]
        [Alias("lon")]
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
                    HelpMessage = "Site height (m) as a double")]
        [Alias("ht")]
        [ValidateNotNullOrEmpty]
        public double height
        {
            get { return m_height; }
            set { m_height = value; }
        }
        /// <summary>
        /// Site pressure (double)
        /// </summary>
        [Parameter(Position = 3,
                    ValueFromPipeline = true,
                    HelpMessage = "Site atmospheric pressure (hPa) as a double")]
        [Alias("press")]
        [ValidateNotNullOrEmpty]
        public double pressure
        {
            get { return m_pressure; }
            set { m_pressure = value; }
        }
        /// <summary>
        /// Site temperature (double)
        /// </summary>
        [Parameter(Position = 3,
                    ValueFromPipeline = true,
                    HelpMessage = "Site temperature (C) as a double")]
        [Alias("temp")]
        [ValidateNotNullOrEmpty]
        public double temperature
        {
            get { return m_temperature; }
            set { m_temperature = value; }
        }

        #endregion parameters

        #region protected overrides

        protected override void ProcessRecord()
        {
            m_site = new Site();
            if (m_latitude != -999) m_site.Latitude = m_latitude;
            if (m_longitude != -999) m_site.Longitude = m_longitude;
            if (m_height != -999) m_site.Height = m_height;
            if (m_pressure != -999) m_site.Pressure = m_pressure;
            if (m_temperature != -999) m_site.Temperature = m_temperature;

            WriteObject(m_site);
        }

        #endregion protected overrides
    }
}
