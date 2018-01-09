
Set ASCOM=%CommonProgramFiles%\ASCOM\Focuser\
Set Driver=FocusSim.exe

If Exist "%ASCOM%%Driver%" "%ASCOM%%Driver%" /unregserver

copy "%Driver%" "%ASCOM%"

"%ASCOM%%Driver%" /regserver

pause