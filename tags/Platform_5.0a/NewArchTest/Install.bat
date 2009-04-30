echo off
cd SampleInterface
cd
call RegisterTypeLibAndPIA.bat
cd ..\InprocServerCOM\VB6
cd
regsvr32 /s InprocServerVB6.dll
echo InprocServerVB6 registered
cd ..\C#.NET\bin\Debug
cd
regasm InprocServerCSharp.dll
cd ..\..\..\VB.Net\bin\Debug
cd
regasm InprocServerVB.dll
cd ..\..\..\..\LocalServerCOM\LocalServerCOM\bin\Debug
cd
LocalServerCOM.exe -register
echo LocalServerCOM registered
cd ..\..\..
cd
echo Install completed - check for errors!
