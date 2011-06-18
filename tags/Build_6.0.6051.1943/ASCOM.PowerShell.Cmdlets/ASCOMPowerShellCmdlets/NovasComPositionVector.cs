using System;
using System.Management.Automation;
using ASCOM.Astrometry.NOVASCOM;

namespace ASCOM.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "PositionVector")]
    public class NovasComPositionVector : Cmdlet
    {
        #region private fields

        private PositionVector m_posv;
        private double[] m_init;

        #endregion private fields

        #region parameters

        [Parameter(Position = 0,
                    ValueFromPipeline = true,
                    HelpMessage = "Optionally initialize the Position Vector object with an array of parameters (x,y,z,RA,DEC,Distance,Light,Az,Alt)")]
        [ValidateNotNullOrEmpty]
        public double[] init
        {
            get { return m_init; }
            set { m_init = value; }
        }
        
        #endregion parameters

        #region protected overrides

        protected override void ProcessRecord()
        {
            if (m_init == null)
                m_posv = new PositionVector();
            else if (m_init.Length == 9)
                m_posv = new PositionVector(m_init[0], m_init[1], m_init[2], m_init[3], m_init[4],
                                            m_init[5], m_init[6], m_init[7], m_init[8]);
            else
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Invalid number of arguments, -init parameter requires an array of 9 arguments"), "", ErrorCategory.InvalidArgument, m_init));
            }
            WriteObject(m_posv);
        }

        #endregion protected overrides
    }
}
