Set ASCOM=%CommonProgramFiles%\ASCOM\Telescope\
Set Driver=POTH.exe

If Exist "%ASCOM%%Driver%" "%ASCOM%%Driver%" /unregserver

pause