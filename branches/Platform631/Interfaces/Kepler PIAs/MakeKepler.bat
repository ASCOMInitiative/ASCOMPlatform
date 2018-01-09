echo off
echo Compiling Kepler type library...
midl /tlb Kepler.tlb Kepler.idl
echo Making Kepler PIA...
tlbimp Kepler.tlb /out:ASCOM.Kepler.dll /asmversion:1.0.0.0 /keyfile:ASCOM.snk /primary
del *.h
del *.c
