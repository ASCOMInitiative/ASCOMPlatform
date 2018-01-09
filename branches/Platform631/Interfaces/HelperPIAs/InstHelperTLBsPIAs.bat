echo off
regtlb Helper.tlb
regasm ASCOM.Helper.DLL
gacutil -i ASCOM.Helper.dll
regtlb Helper2.tlb
regasm ASCOM.Helper2.dll
gacutil -i ASCOM.Helper2.dll
