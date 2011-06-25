using System.Management.Automation;
using ASCOM.Astrometry;

namespace ASCOM.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "AstrometryStruct")]
    public class AstrometryStruct : Cmdlet
    {
        #region private fields

        private string m_type;
        
        #endregion private fields

        #region parameters

        /// <summary>
        /// The structure type to return
        /// </summary>
        [Parameter(Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            HelpMessage = "ASCOM Astrometry structure type to return")]
        [ValidateNotNullOrEmpty]
        public string type
        {
            get { return m_type; }
            set { m_type = value; }
        }
        #endregion parameters

        #region protected overrides

        protected override void ProcessRecord()
        {
            switch (m_type.ToLower())
            {
                case "bodydescription":
                    WriteObject(new BodyDescription());
                    break;
                case "catentry":
                    WriteObject(new CatEntry());
                    break;
                case "fundamentalargs":
                    WriteObject(new FundamentalArgs());
                    break;
                case "posvector":
                    WriteObject(new PosVector());
                    break;
                case "siteinfo":
                    WriteObject(new SiteInfo());
                    break;
                case "velvector":
                    WriteObject(new VelVector());
                    break;
                default:
                    WriteObject(null);
                    break;
            }
        }

        #endregion protected overrides
    }
}
