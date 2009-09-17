echo off
gacutil -u ASCOM.NOVAS
regasm -u ASCOM.NOVAS.dll
regtlb -u NOVAS.tlb