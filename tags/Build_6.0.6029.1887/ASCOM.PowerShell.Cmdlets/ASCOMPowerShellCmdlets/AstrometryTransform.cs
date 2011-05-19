using System.Management.Automation;
using ASCOM.Astrometry.Transform;

namespace ASCOM.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "Transform")]
    public class AstrometryTransform : Cmdlet
    {
        #region private fields

        private Transform m_transform;

        #endregion private fields

        #region parameters

        #endregion parameters

        #region protected overrides

        protected override void ProcessRecord()
        {
            m_transform = new Transform();
            WriteObject(m_transform);
        }

        #endregion protected overrides
    }
}
