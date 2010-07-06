using System.Management.Automation;
using ASCOM.Astrometry.NOVASCOM;

namespace ASCOM.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "Planet")]
    public class NovasComPlanet : Cmdlet
    {
        #region private fields

        private Planet m_planet;

        #endregion private fields

        #region parameters

        #endregion parameters

        #region protected overrides

        protected override void ProcessRecord()
        {
            m_planet = new Planet();
            WriteObject(m_planet);
        }

        #endregion protected overrides
    }
}
