echo off
regtlb Kepler.tlb
regasm ASCOM.Kepler.DLL
gacutil -i ASCOM.Kepler.dll
