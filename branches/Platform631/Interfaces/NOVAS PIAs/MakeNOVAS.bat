echo off
echo Compiling NOVAS type library...
midl /tlb NOVAS.tlb NOVAS.idl
echo Making NOVAS PIA...
tlbimp NOVAS.tlb /out:ASCOM.NOVAS.dll /asmversion:2.0.0.0 /keyfile:ASCOM.snk /primary
del *.h
del *.c
