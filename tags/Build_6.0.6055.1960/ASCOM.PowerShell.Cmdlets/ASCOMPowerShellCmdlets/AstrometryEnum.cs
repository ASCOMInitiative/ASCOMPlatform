using System;
using System.Management.Automation;
using ASCOM.Astrometry;

namespace ASCOM.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "AstrometryEnum")]
    public class AstrometryEnum : Cmdlet
    {

        private static int StringToEnum<T>(string name)
        {
            return (int)Enum.Parse(typeof(T), name, true);
        }
        
        #region private fields

        private string m_type;
        private string m_value;
        
        #endregion private fields

        #region parameters

        /// <summary>
        /// The enumeration type to return
        /// </summary>
        [Parameter(Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            HelpMessage = "ASCOM Astrometry enumeration type to return")]
        [ValidateNotNullOrEmpty]
        public string type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// The enumeration value to return
        /// </summary>
        [Parameter(Position = 1,
            Mandatory = true,
            ValueFromPipeline = true,
            HelpMessage = "ASCOM Astrometry enumeration value to return")]
        [ValidateNotNullOrEmpty]
        public string value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        #endregion parameters

        #region protected overrides

        protected override void ProcessRecord()
        {
            switch (m_type.ToLower())
            {
                case "body":
                    WriteObject(StringToEnum<Body>(m_value));
                    break;
                case "bodytype":
                    WriteObject(StringToEnum<BodyType>(m_value));
                    break;
                case "nutationdirection":
                    WriteObject(StringToEnum<NutationDirection>(m_value));
                    break;
                case "origin":
                    WriteObject(StringToEnum<Origin>(m_value));
                    break;
                case "refractionoption":
                    WriteObject(StringToEnum<RefractionOption>(m_value));
                    break;
                case "transformationoption":
                    WriteObject(StringToEnum<TransformationOption>(m_value));
                    break;
                default:
                    WriteObject(null);
                    break;
            }
        }

        #endregion protected overrides
    }
}
