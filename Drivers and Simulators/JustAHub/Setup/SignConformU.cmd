rem @echo off
rem @echo ConformU Requested to sign %1
rem call "C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Auxiliary\Build\vcvarsall.bat" x64
rem start "Sign with SHA256" /wait /b "signtool" sign /a /fd SHA256 /tr http://rfc3161timestamp.globalsign.com/advanced /td SHA256 %1

"C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x86\signtool" sign /a /fd SHA256 /tr http://rfc3161timestamp.globalsign.com/advanced /td SHA256 %1

exit 0