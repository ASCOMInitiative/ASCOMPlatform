echo off
echo Registering interface type library for COM to the Registry
"c:\Program Files\Utilities\regtlb" ISimpleCOMObject.tlb
echo.
echo Generating Primary Interop Assembly for ISimpleCOMObject.tlb ...
tlbimp ISimpleCOMObject.tlb /out:Interop.ISimpleCOMObject.dll /keyfile:SimpleCOMObject.snk /primary
echo.
echo Registering PIA Interop.ISimpleCOMObject.dll to the Registry
regasm Interop.ISimpleCOMObject.dll
echo.
echo Registering PIA Interop.ISimpleCOMObject.dll into the Global Assembly Cache...
gacutil -i Interop.ISimpleCOMObject.dll
