@echo off
rem Batch command to copy 'content' files to the installer project folder (these could be executables)
rem This command is intended to be used in post compilation build events

@echo Incoming Solution folder: %1
 rem There must be no spaces after the set command below
set solutionFolder=%~1
echo Solution folder: %solutionFolder%

rem Clean the incoming solutiuon folder of hash and backslash characters
for /l %%a in (1,1,3) do (
	if "%solutionFolder:~0,1%"==" " set solutionFolder=%solutionFolder:~1%
	if "%solutionFolder:~-1%"==" " set solutionFolder=%solutionFolder:~0,-1%
	if "%solutionFolder:~-1%"=="\" set solutionFolder=%solutionFolder:~0,-1%
)
echo Cleaned solution folder: %solutionFolder%
echo Cleaned solution folder: %solutionFolder% > %solutionFolder%\xcopy.txt

rem Clean the incoming source files parameter of hash and backslash characters - there must be no spaces after the set command below
set sourceFiles=%~2 
echo Incoming source files: %2

for /l %%a in (1,1,3) do (
	if "%sourceFiles:~0,1%"==" " set sourceFiles=%sourceFiles:~1%
	if "%sourceFiles:~-1%"==" " set sourceFiles=%sourceFiles:~0,-1%
	if "%sourceFiles:~-1%"=="\" set sourceFiles=%sourceFiles:~0,-1%
)
echo Cleaned source files: %sourceFiles%
echo Cleaned source files: %sourceFiles% >> %solutionFolder%\xcopy.txt

rem Find the location of the InstallAware project file
where /R "%solutionFolder%" "ascom platform 6.mpr">%solutionFolder%\where.txt
set /P installerProjectFile=<%solutionFolder%\where.txt
echo Installer project file: %installerProjectFile%
echo Installer project file: %installerProjectFile% >> %solutionFolder%\xcopy.txt

rem Extract the path to the InstallAware folder from the project file full path.
for %%i in ("%installerProjectFile%") do (
 	set installerFolder=%%~di%%~pi
)

echo Installer folder: %installerFolder%
echo Installer folder: %installerFolder% >> %solutionFolder%\xcopy.txt

rem Copy the source files to the installer folder
echo xcopy /y /q "%sourceFiles%" "%installerFolder%"
echo XCOPY command: xcopy /y /q "%sourceFiles%" "%installerFolder%" >> %solutionFolder%\xcopy.txt
xcopy /y /q "%sourceFiles%" "%installerFolder%"
return