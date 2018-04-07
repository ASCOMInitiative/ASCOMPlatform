using System.Management.Automation;
using ASCOM.Astrometry.NOVASCOM;

namespace ASCOM.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "VelocityVector")]
    public class NovsaComVelocityVector : Cmdlet
    {
        #region private fields

        private VelocityVector m_velvec;

        #endregion private fields

        #region parameters

        #endregion parameters

        #region protected overrides

        protected override void ProcessRecord()
        {
            m_velvec = new VelocityVector();
            WriteObject(m_velvec);
        }

        #endregion protected overrides
    }
}
