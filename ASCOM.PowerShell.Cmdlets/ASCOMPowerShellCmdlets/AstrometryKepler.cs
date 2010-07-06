using System.Management.Automation;
using ASCOM.Astrometry.Kepler;

namespace ASCOM.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "Kepler")]
    public class AstrometryKepler : Cmdlet
    {
        #region private fields

        private  Ephemeris m_kepler;

        #endregion private fields

        #region parameters

        #endregion parameters

        #region protected overrides

        protected override void ProcessRecord()
        {
            m_kepler = new Ephemeris();
            WriteObject(m_kepler);
        }

        #endregion protected overrides
    }
}
