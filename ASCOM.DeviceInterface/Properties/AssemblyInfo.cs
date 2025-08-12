using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]

#if NET481
// This assembly version is included alongside 6.0.0.0 in ASCOM Platform 7.1 release onwards.
[assembly: Guid("5849AF17-3590-42ff-900E-7544D45C9825")]
[assembly: AssemblyVersion("6.0.0.0")]
[assembly: AssemblyTitle("ASCOM Device Interfaces (.NET 4.x)")]
#elif NET472
[assembly: Guid("5849AF17-3590-42ff-900E-7544D45C9825")]
[assembly: AssemblyVersion("6.0.0.0")]
[assembly: AssemblyTitle("ASCOM Device Interfaces (.NET 4.7.2)")]
#elif NETSTANDARD2_0
[assembly: Guid("5849AF17-3590-42ff-900E-7544D45C9825")]
[assembly: AssemblyVersion("6.0.0.0")]
[assembly: AssemblyTitle("ASCOM Device Interfaces (.NET Standard)")]
#else
// This assembly version is included in ASCOM Platform 7.0 and earlier releases.
[assembly: Guid("5849AF17-3590-42ff-900E-7544D45C9825")]
[assembly: AssemblyVersion("6.0.0.0")]
[assembly: AssemblyTitle("ASCOM Device Interfaces (.NET 2.0 / 3.5)")]
#endif

[assembly: AssemblyCompany("ASCOM Initiative")]
[assembly: AssemblyCopyright("Copyright © ASCOM Initiative 2025")]
[assembly: AssemblyDescription("ASCOM device interfaces for Platform 7")]
[assembly: AssemblyProduct("ASCOM.DeviceInterfaces")]