@Echo off
Call "%VS90COMNTOOLS%vsvars32"
al /link:PublisherPolicy.xml /out:policy.5.3.ASCOM.Utilities.dll /keyfile:..\Utilities\ASCOM.snk  /version:5.3.0.0 /fileversion:5.3.0.0 /company:"ASCOM Initiative" /product:ASCOM.Utilities
al /link:PublisherPolicy.xml /out:policy.5.3.ASCOM.Astrometry.dll /keyfile:..\Utilities\ASCOM.snk  /version:5.3.0.0 /fileversion:5.3.0.0 /company:"ASCOM Initiative" /product:ASCOM.Utilities
