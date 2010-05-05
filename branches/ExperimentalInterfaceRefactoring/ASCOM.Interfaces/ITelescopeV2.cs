using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace ASCOM.Interface
{
    /// <summary>
    /// ITelescopeV2 comment
    /// </summary>
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("452F2D5E-640F-4422-AC85-8BDDD2998B4A")]
    public interface ITelescopeV2 : IAscomDriver, IDeviceControl, ITelescope
    { 
    }
}

