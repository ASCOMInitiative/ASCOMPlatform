echo off
echo Removing PIA Interop.ISimpleCOMObject.dll from the Global Assembly Cache...
gacutil -u Interop.ISimpleCOMObject
echo.
echo Unregistering PIA Interop.ISimpleCOMObject.dll from the Registry
regasm -u Interop.ISimpleCOMObject.dll
echo.
echo Unregistering interface type library for COM from the Registry
"c:\Program Files\Utilities\regtlb" -u ISimpleCOMObject.tlb
