REM Sign the MSI and LIB file in the top level build folder
C:\BuildSupport\CodeSign.exe forcesign -l "%~1\Release\Single\*.msi"
C:\BuildSupport\CodeSign.exe forcesign -l "%~1\Release\Single\*.lib"

REM Sign the MSI in the data folder under the top level build folder
C:\BuildSupport\CodeSign.exe forcesign -l "%~1\Release\Single\data\*.msi"
echo "%~1\FilesToSign.txt" > signmsi1.txt

for %%A in ("%~1") do set LONG=%%~fA
echo "%LONG%\FilesToSign.txt" > signmsi2.txt
echo "Running files to sign..." ? signmsi3.txt
C:\BuildSupport\CodeSign.exe forcesignfiles -l "%LONG%\FilesToSign.txt"
echo "Completed files to sign" > signmsi4.txt