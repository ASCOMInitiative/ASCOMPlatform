@echo off
if not defined VSCMD_VER (
  call "C:\Program Files\Microsoft Visual Studio\18\Community\Common7\Tools\VsDevCmd.bat"
)

signtool sign /v /tr http://timestamp.acs.microsoft.com/ /td sha256 /fd sha256 /n "Peter Simpson" "%1\Release\Single\AscomPlatformInstaller.msi"
