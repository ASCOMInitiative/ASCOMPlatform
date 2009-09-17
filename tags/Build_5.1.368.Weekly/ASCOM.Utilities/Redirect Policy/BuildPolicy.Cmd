@echo off
call "%VS90COMNTOOLS%vsvars32"
call "LinkStep.Cmd"
REM al /link:PublisherPolicy.xml /out:policy.5.0.ASCOM.HelperNET.dll /keyfile:..\HelperNET\ASCOM.snk /version:5.0.0.0 /fileversion:5.0.0.0 /company:"ASCOM Initiative" /product:ASCOM.HelperNET
pause