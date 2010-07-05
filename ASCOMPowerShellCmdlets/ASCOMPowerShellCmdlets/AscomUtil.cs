using System.Management.Automation;
using ASCOM.Utilities;

namespace ASCOMPowerShellCmdlets
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
            try
            {
                m_util = new Util();
                WriteObject(m_util);
            }
            catch (System.Exception) { }
        }

        #endregion protected overrides
    }
}
