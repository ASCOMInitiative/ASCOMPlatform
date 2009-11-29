
Set ASCOM=%CommonProgramFiles%\ASCOM\Focuser\
Set Driver=RoboFocusServer.exe
Set Help=robofocusins3.doc

If Exist "%ASCOM%%Driver%" "%ASCOM%%Driver%" /unregserver

copy "%Driver%" "%ASCOM%"
copy "%Help%" "%ASCOM%"

"%ASCOM%%Driver%" /regserver

"%ASCOM%%Driver%" -r

pause