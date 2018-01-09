echo off
gacutil -u ASCOM.Kepler
regasm -u ASCOM.Kepler.dll
regtlb -u Kepler.tlb
