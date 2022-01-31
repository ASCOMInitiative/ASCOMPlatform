using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.DriverAccess
{
    class TEMPLATEDEVICECLASS
    {
        private string id;

        public TEMPLATEDEVICECLASS(string id)
        {
            // TODO: Complete member initialization
            this.id = id;
        }

        public string Name { get { return "Template name"; } }

        public string Description { get { return "Template description"; } }

        public string DriverInfo { get { return "Template driverinfo"; } }

        public string DriverVersion { get { return "Template driver version"; } }

        public bool Connected { get; set; }

        internal static string Choose(string id)
        {
            return "template.choose";
        }
    }
}
