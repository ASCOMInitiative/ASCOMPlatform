@Echo off
Call "%VS90COMNTOOLS%vsvars32"
al /link:PublisherPolicy.xml /out:policy.5.5.ASCOM.Utilities.dll /keyfile:..\ASCOM.Utilities\ASCOM.snk  /version:5.5.0.0 /fileversion:5.5.0.0 /company:"ASCOM Initiative" /product:"ASCOM Platform"
al /link:PublisherPolicy.xml /out:policy.5.5.ASCOM.Astrometry.dll /keyfile:..\ASCOM.Utilities\ASCOM.snk  /version:5.5.0.0 /fileversion:5.5.0.0 /company:"ASCOM Initiative" /product:"ASCOM Platform"
