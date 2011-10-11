@Echo off
Call "%VS90COMNTOOLS%vsvars32"
al /link:ExceptionsPolicy.xml /out:policy.1.0.ASCOM.Exceptions.dll /keyfile:..\ASCOM.snk  /version:6.0.0.0 /fileversion:6.0.0.0 /company:"ASCOM Initiative" /product:"ASCOM Platform"
al /link:ExceptionsPolicy.xml /out:policy.1.1.ASCOM.Exceptions.dll /keyfile:..\ASCOM.snk  /version:6.0.0.0 /fileversion:6.0.0.0 /company:"ASCOM Initiative" /product:"ASCOM Platform"
