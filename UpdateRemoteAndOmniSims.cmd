call "C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Auxiliary\Build\vcvarsall.bat" x64
rem msbuild UpdateRemoteAndOmniSims.msbuild > buildlog.txt

xcopy /s/y J:\Ascom.Alpaca.Simulators J:\ASCOMPlatform\OmniSimulators

cd \ASCOMPlatform\OmniSimulators\ASCOM.Alpaca.Simulators

dotnet publish -c Release -r win-x86 --self-contained true /p:PublishTrimmed=false -p DefineConstants=ASCOM_COM -p:Platform=x86 -o ..\publish\x86

dotnet publish -c Release -r win-x64 --self-contained true /p:PublishTrimmed=false -p DefineConstants=ASCOM_COM -o ..\publish\x64 

pause