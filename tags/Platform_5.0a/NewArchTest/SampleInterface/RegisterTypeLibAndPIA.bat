echo off
echo Registering interface type library for COM to the Registry
regtlb IAscomSample.tlb
echo.
echo Registering PIA to the Registry
regasm Interop.IAscomSample.dll
echo.
echo Adding PIA into the Global Assembly Cache
gacutil -i Interop.IAscomSample.dll
