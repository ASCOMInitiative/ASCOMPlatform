@echo off
if not defined VSCMD_VER (
  call "C:\Program Files\Microsoft Visual Studio\18\Community\Common7\Tools\VsDevCmd.bat"
)

REM Sign the MSI in the top level build folder
signtool sign /v /tr http://timestamp.acs.microsoft.com/ /td sha256 /fd sha256 /n "Peter Simpson" "%1\Release\Single\*.msi"

REM Sign the MSI in the data folder under the top level build folder
signtool sign /v /tr http://timestamp.acs.microsoft.com/ /td sha256 /fd sha256 /n "Peter Simpson" "%1\Release\Single\data\*.msi"
