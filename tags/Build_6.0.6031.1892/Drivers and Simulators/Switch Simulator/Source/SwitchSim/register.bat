
Set ASCOM=%CommonProgramFiles%\ASCOM\Switch\
Set Driver=SwitchSim.exe

If Not Exist "%ASCOM%" mkdir "%ASCOM%
If Exist "%ASCOM%%Driver%" "%ASCOM%%Driver%" /unregserver

copy "%Driver%" "%ASCOM%"

"%ASCOM%%Driver%" /regserver

"%ASCOM%%Driver%" -r

pause