echo off
gacutil -u ASCOM.Helper
regasm -u ASCOM.Helper.dll
regtlb -u Helper.tlb
gacutil -u ASCOM.Helper2
regasm -u ASCOM.Helper2.dll
regtlb -u Helper2.tlb
