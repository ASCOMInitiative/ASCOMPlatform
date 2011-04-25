
Set ASCOM=%CommonProgramFiles%\ASCOM\Telescope\
Set Driver=MeadeLX200GPS Driver.exe
Set Help=MeadeLX200GPSHelp.htm

If Exist "%ASCOM%%Driver%" "%ASCOM%%Driver%" /unregserver

copy "%Driver%" "%ASCOM%"
copy "%Help%" "%ASCOM%"

"%ASCOM%%Driver%" /regserver

"%ASCOM%%Driver%" -r

pause