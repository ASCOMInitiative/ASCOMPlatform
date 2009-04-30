echo off
echo Removing PIA from the Global Assembly Cache
gacutil -u Interop.IAscomSample
echo.
echo Unregistering PIA from the Registry
regasm -u Interop.IAscomSample.dll
echo.
echo Unregistering interface type library for COM from the Registry
regtlb -u IAscomSample.tlb
