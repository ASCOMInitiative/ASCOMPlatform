
Set ASCOM=C:\Program Files\Common Files\ASCOM\Telescope\
Set Driver=MeadeEx Driver.exe
Set Help=MeadeHelp.htm

If Exist "%ASCOM%%Driver%" "%ASCOM%%Driver%" /unregserver

copy "%Driver%" "%ASCOM%"
copy "%Help%" "%ASCOM%"

"%ASCOM%%Driver%" /regserver

"%ASCOM%%Driver%" -r

pause