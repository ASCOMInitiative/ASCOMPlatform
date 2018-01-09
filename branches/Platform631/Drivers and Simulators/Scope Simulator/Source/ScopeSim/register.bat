
Set ASCOM=%CommonProgramFiles%\ASCOM\Telescope\
Set Driver=ScopeSim.exe
Set Help=ScopeSimHelp.htm

If Exist "%ASCOM%%Driver%" "%ASCOM%%Driver%" /unregserver

copy "%Driver%" "%ASCOM%"
copy "%Help%" "%ASCOM%"

"%ASCOM%%Driver%" /regserver

pause