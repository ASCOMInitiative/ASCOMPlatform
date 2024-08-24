﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.DeviceInterface
{
    // Dummy class to remove compile warnings for the .NET Standard implementation. Not required for .NET Framework 3.5 or 4.7.2.
#if NETSTANDARD2_0_OR_GREATER

    /// <summary>
    /// Dummy class to remove compile warnings for the .NET Standard implementation
    /// </summary>
    static class Util
    {
        public static void ConvertUnits() { }
        public static void DewPoint2Humidity() { }
        public static void Humidity2DewPoint() { }
        public static void ConvertPressure() { }
    }

#endif

}
