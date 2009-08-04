echo off
echo Compiling ASCOM master interface type library...
midl /nologo /tlb AscomMasterInterfaces.tlb AscomMasterInterfaces.idl
del *.c
del *.h
echo Making ASCOM master interface PIA...
tlbimp AscomMasterInterfaces.tlb /nologo /out:ASCOM.Interfaces.dll /asmversion:5.0.0.0 /keyfile:ASCOM.snk /primary
