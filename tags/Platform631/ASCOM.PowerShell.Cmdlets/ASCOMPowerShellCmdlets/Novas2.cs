using System.Management.Automation;
using ASCOM.Astrometry.NOVAS;

namespace ASCOM.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "NOVAS2")]
    public class AstrometryNovas2 : Cmdlet
    {
        #region private fields

        private NOVAS2COM m_novas2;

        #endregion private fields

        #region parameters

        #endregion parameters

        #region protected overrides

        protected override void ProcessRecord()
        {
            m_novas2 = new  NOVAS2COM();
            WriteObject(m_novas2);
        }

        #endregion protected overrides
    }
}
