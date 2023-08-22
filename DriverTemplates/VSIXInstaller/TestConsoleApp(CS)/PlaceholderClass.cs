// This is a dummy class whose only purpose is to ensure that the Platform code compiles correctly, without errors.
// This class is discarded when the template wizard runs on the user's PC and does not appear in the generated project.

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

        public string DriverInfo { get { return "Template driver information"; } }

        public string DriverVersion { get { return "Template driver version"; } }

        public string InterfaceVersion { get { return "Template interface version"; } }

        public bool Connected { get; set; }

        internal static string Choose(string id)
        {
            return "template.choose";
        }
    }
}
