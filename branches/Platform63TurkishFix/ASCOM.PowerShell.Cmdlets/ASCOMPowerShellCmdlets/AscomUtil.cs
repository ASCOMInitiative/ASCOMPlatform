using System.Management.Automation;
using ASCOM.Utilities;

namespace ASCOM.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "Util")]
    public class AscomUtil : Cmdlet
    {

        #region private fields

        private Util m_util;

        #endregion private fields

        #region parameters

        #endregion parameters

        #region protected overrides

        protected override void ProcessRecord()
        {
            m_util = new Util();
            WriteObject(m_util);
        }

        #endregion protected overrides
    }
}
