REM Sign the MSI and LIB file in the top level build folder
C:\BuildSupport\CodeSign.exe forcesign -l "%~1\Release\Single\*.msi"
C:\BuildSupport\CodeSign.exe forcesign -l "%~1\Release\Single\*.lib"

REM Sign the MSI in the data folder under the top level build folder
C:\BuildSupport\CodeSign.exe forcesign -l "%~1\Release\Single\data\*.msi"
echo "%~1\FilesToSign.txt" > signmsi.txt
C:\BuildSupport\CodeSign.exe forcesignfiles -l "%~1\FilesToSign.txt"
