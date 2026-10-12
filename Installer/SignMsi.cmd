REM Sign the EXE, MSI and LIB file in the top level build folder
C:\BuildSupport\CodeSign.exe forcesignfiles -l "%~1\FilesToSign.txt"