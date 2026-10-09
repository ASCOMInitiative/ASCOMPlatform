REM Sign the MSI in the top level build folder
C:\BuildSupport\CodeSign.exe sign "%1\Release\Single\*.msi"

REM Sign the MSI in the data folder under the top level build folder
C:\BuildSupport\CodeSign.exe sign "%1\Release\Single\data\*.msi"