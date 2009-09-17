echo off
gacutil /nologo /uf ASCOM.Interfaces
regasm /nologo /u ASCOM.Interfaces.dll
regtlb -u AscomMasterInterfaces.tlb
