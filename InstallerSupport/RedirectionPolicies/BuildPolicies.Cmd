@Echo off
Call "%VS90COMNTOOLS%vsvars32"
al /link:Policy.1.0.Exceptions.xml /out:policy.1.0.ASCOM.Exceptions.dll /keyfile:..\..\ASCOM.snk  /version:6.0.0.0 /fileversion:6.0.0.0 /company:"ASCOM Initiative" /product:"ASCOM Platform"
al /link:Policy.1.1.Exceptions.xml /out:policy.1.1.ASCOM.Exceptions.dll /keyfile:..\..\ASCOM.snk  /version:6.0.0.0 /fileversion:6.0.0.0 /company:"ASCOM Initiative" /product:"ASCOM Platform"
al /link:Policy.1.0.DriverAccess.xml /out:policy.1.0.ASCOM.DriverAccess.dll /keyfile:..\..\ASCOM.snk  /version:6.0.0.0 /fileversion:6.0.0.0 /company:"ASCOM Initiative" /product:"ASCOM Platform"
al /link:policy.5.5.Astrometry.xml /out:policy.5.5.ASCOM.Astrometry.dll /keyfile:..\..\ASCOM.snk  /version:6.0.0.0 /fileversion:6.0.0.0 /company:"ASCOM Initiative" /product:"ASCOM Platform"
al /link:Policy.5.5.Utilities.xml /out:policy.5.5.ASCOM.Utilities.dll /keyfile:..\..\ASCOM.snk  /version:6.0.0.0 /fileversion:6.0.0.0 /company:"ASCOM Initiative" /product:"ASCOM Platform"
