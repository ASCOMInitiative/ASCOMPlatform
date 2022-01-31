// this class is included so that the use of TEMPLATEDEVICENAME in the template sources is satisfied.
// It is removed by the wizard when the project is created from the template

namespace ASCOM.DriverAccess
{
    internal class TEMPLATEDEVICECLASS
    {
        internal static string Choose(string id)
        {
            return "ASCOM.TEMPLATEDEVICECLASS.Any";
        }

        internal TEMPLATEDEVICECLASS(string id)
        {
        }

        internal bool Connected { get; set; }

    }
}
