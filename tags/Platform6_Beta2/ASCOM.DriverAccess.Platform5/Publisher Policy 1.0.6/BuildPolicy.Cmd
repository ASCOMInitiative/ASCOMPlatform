@echo off
call "%VS90COMNTOOLS%vsvars32"
al /link:DriverAccess.config /out:policy.1.0.ASCOM.DriverAccess.dll /keyfile:ASCOM.snk /version:1.0.6.0 /fileversion:1.0.6.0 /company:"ASCOM Initiative" /product:"ASCOM.Interfaces Assembly"
pause