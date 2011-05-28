echo off
regtlb NOVAS.tlb
regasm ASCOM.NOVAS.DLL
gacutil -i ASCOM.NOVAS.dll