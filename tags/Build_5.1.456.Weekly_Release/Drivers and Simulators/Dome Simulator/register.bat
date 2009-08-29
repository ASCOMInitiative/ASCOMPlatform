
Set ASCOM=%CommonProgramFiles%\ASCOM\Dome\
Set Driver=DomeSim.exe

If Exist "%ASCOM%%Driver%" "%ASCOM%%Driver%" /unregserver

copy "%Driver%" "%ASCOM%"

"%ASCOM%%Driver%" /regserver

pause