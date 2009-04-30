echo off
cd LocalServerCOM\LocalServerCOM\bin\Debug
cd
LocalServerCOM.exe -unregister
echo LocalServerCOM unregistered
cd ..\..\..\..\InProcServerCOM\C#.NET\bin\Debug
cd
regasm -u InprocServerCSharp.dll
cd ..\..\..\VB.Net\bin\Debug
cd
regasm -u InprocServerVB.dll
cd ..\..\..\VB6
cd
regsvr32 /u /s InprocServerVB6.dll
echo InprocServerVB6 unregistered
cd ..\..\SampleInterface
cd
call UnregisterTypeLibAndPIA.bat
cd ..
cd
echo Remove completed - check for errors!
