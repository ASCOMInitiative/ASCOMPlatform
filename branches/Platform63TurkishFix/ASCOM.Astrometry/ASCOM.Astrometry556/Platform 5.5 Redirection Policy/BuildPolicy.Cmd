@Echo off
Call "%VS90COMNTOOLS%vsvars32"
al /link:AstrometryPolicy.xml /out:policy.5.5.ASCOM.Astrometry.dll /keyfile:..\..\..\ASCOM.snk  /version:5.5.6.0 /fileversion:5.5.6.0 /company:"ASCOM Initiative" /product:"ASCOM Platform 6 version of Platform 5.5 Astrometry component"