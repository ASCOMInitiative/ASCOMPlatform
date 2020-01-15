using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.DynamicRemoteClients
{
    /// <summary>
    /// Possible installation states for an Alpaca dynamic driver
    /// </summary>
    enum InstallationState
    {
        Ok = 0, // The driver is correctly installed
        NotCOMRegistered = 1, // The driver is not registered for COM
        NotASCOMRegistered = 2, // The driver is not registered in the ASCOM Profile
        MissingDriver = 3, // The driver DLL was not found
        NotCompatible = 4, // A failure message was generated when the Alpaca dynamic driver was tested for compatibility
        BadProfile = 5 // An exception occurred when reading a value from the ASCOM Profile (an integer value may have a non-numeric value or the value may be missing entirely)
    }
}
