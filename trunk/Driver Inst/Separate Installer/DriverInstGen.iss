; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "InstallGen"
#define MyAppVerName "InstallGen V1.3"
#define MyAppPublisher "ASCOM Initiative"
#define MyAppURL "http://ascom-standards.org/"
#define MyAppExeName "InstallerGen.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{222C1C34-A114-4DBA-8D08-864F6E46FD64}
AppName={#MyAppName}
AppVerName={#MyAppVerName}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf32}\ASCOM\InstallGen
DisableDirPage=yes
DefaultGroupName=ASCOM Platform\Tools
LicenseFile=D:\dev\astro\ascom\Driver Inst\InstallerGen\License\CreativeCommons.txt
OutputDir=D:\dev\astro\ascom\Driver Inst\Separate Installer
OutputBaseFilename=InstallGen-1.3
Compression=lzma
SolidCompression=yes
SourceDir="D:\dev\astro\ascom\Driver Inst\InstallerGen"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "bin\Release\InstallerGen.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\Microsoft.Samples.WinForms.Extras.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "Template\TemplateSubstitutionParameters.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "Template\DriverInstallTemplate.iss"; DestDir: "{app}\Resources"; Flags: ignoreversion
Source: "Graphics\WizardImage.bmp"; DestDir: "{app}\Resources"; Flags: ignoreversion
Source: "License\CreativeCommons.txt"; DestDir: "{app}\Resources"; Flags: ignoreversion

[Icons]
Name: "{group}\Driver Install Script Maker"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\Driver Install Script Maker"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon


