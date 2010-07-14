@echo off
call "%VS90COMNTOOLS%vsvars32"
al /link:InterfaceRedirect.config /out:policy.1.0.ASCOM.Interfaces.dll /keyfile:ASCOM.snk /version:1.0.0.2 /fileversion:1.0.0.2 /company:"ASCOM Initiative" /product:"ASCOM.Interfaces Assembly"
pause