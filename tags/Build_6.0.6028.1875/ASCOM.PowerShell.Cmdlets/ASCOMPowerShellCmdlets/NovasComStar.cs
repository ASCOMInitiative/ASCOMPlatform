using System;
using System.Management.Automation;
using ASCOM.Astrometry.NOVASCOM;

namespace ASCOM.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "Star")]
    public class NovasComStar : Cmdlet
    {
        #region private fields

        private Star m_star;
        private double[] m_init;

        #endregion private fields

        #region parameters

        [Parameter(Position = 0,
            ValueFromPipeline = true,
            HelpMessage = "Optionally initialize the Star object with an array of parameters (RA,DEC,propmRA,propmDEC,parallax,radVel)")]
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
            
            m_star = new Star();
            if (m_init != null)
            {
                if (m_init.Length == 6)
                {
                    m_star.Set(m_init[0], m_init[1], m_init[2], m_init[3], m_init[4], m_init[5]);
                }
                else
                {
                    ThrowTerminatingError(new ErrorRecord(new Exception("Invalid number of arguments, -init parameter requires an array of 6 arguments"), "", ErrorCategory.InvalidArgument, m_init));
                }
            }
            WriteObject(m_star);
        }

        #endregion protected overrides
    }
}
