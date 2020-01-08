using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.DynamicRemoteClients
{
    enum InstallationState
    {
        Ok,
        NotCOMRegistered, // Not registered for COM
        NotASCOMRegistered, // Not registered in the ASCOM Profile
        MissingDriver, // Driver DLL not found
        NotCompatible, // Failure message generated when tested from compatibility
        BadProfile // Exception when reading a value from the ASCOM Profile
    }
}
