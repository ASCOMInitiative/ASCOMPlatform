
Set ASCOM=%CommonProgramFiles%\ASCOM\Telescope\
Set Driver=POTH.exe
Set Help=POTHHelp.htm

If Exist "%ASCOM%%Driver%" "%ASCOM%%Driver%" /unregserver

copy "%Driver%" "%ASCOM%"
copy "%Help%" "%ASCOM%"

"%ASCOM%%Driver%" /regserver

pause