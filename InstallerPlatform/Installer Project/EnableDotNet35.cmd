@echo off
color 1e
@echo INSTALLING .NET 3.5
@echo.
@echo Please wait for the update process to complete and the "Press any key to continue . . ." message to appear.
@echo.
rem
DISM /Online /Enable-Feature /FeatureName:NetFx3 /All /NoRestart
rem
@echo.
@echo DISM exit code: %errorlevel%
@echo.
pause
exit /b %errorlevel%