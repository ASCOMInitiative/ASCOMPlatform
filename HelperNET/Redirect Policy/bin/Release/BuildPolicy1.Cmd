@Echo off
Call "%VS90COMNTOOLS%vsvars32"
al /link:PublisherPolicy.xml /out:policy.5.1.ASCOM.HelperNET.dll /keyfile:..\..\ASCOM.snk  /version:5.1.0.0 /fileversion:5.1.0.0 /company:"ASCOM Initiative" /product:ASCOM.HelperNET
