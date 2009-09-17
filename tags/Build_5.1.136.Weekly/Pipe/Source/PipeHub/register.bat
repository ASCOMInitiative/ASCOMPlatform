
Set ASCOM=%CommonProgramFiles%\ASCOM\Telescope\
Set Driver1=Pipe.exe
Set Driver2=Hub.exe
Set Help=PipeHubHelp.htm

If Exist "%ASCOM%%Driver1%" "%ASCOM%%Driver1%" /unregserver
If Exist "%ASCOM%%Driver2%" "%ASCOM%%Driver2%" /unregserver

copy "%Driver1%" "%ASCOM%"
copy "%Driver2%" "%ASCOM%"
copy "%Help%" "%ASCOM%"

"%ASCOM%%Driver1%" /regserver
"%ASCOM%%Driver2%" /regserver

"%ASCOM%%Driver1%" -r
"%ASCOM%%Driver2%" -r

pause