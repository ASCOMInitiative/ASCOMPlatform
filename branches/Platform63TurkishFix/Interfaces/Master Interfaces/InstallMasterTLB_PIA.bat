echo off
regtlb AscomMasterInterfaces.tlb
regasm /nologo ASCOM.Interfaces.dll
gacutil /nologo /if ASCOM.Interfaces.dll
