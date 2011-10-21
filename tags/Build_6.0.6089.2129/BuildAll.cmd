MSBuild "ASCOM Platform VS2010.sln" /t:rebuild /p:Configuration=Release
MSBuild "c:\ascom trunk\help\ascomplatform.shfbproj" /p:configuration=release
MSBuild "c:\ascom trunk\help\ascomdeveloper.shfbproj" /p:configuration=release
MiaBuild "C:\ASCOM Trunk\Releases\ASCOM 6\Developer\Installer Project\ASCOM Platform 6 Developer.mpr" /r
MiaBuild "C:\ASCOM Trunk\Releases\ASCOM 6\Platform\Installer Project\ASCOM Platform 6.mpr" /r