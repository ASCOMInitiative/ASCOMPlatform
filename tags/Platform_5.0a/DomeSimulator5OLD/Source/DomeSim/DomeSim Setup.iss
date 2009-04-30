; ASCOM Install Script for DomeSim
; Jon Brewster

[Setup]
AppName=ASCOM Dome Simulator
AppVerName=DomeSim 5.0.6
AppVersion=5.0.6
AppPublisher=Jon Brewster jon@brewsters.net
AppPublisherURL=http://ascom-standards.org/
AppSupportURL=http://ascom-standards.org/
AppUpdatesURL=http://ascom-standards.org/
MinVersion=0,5.0.2195sp4
DefaultDirName="{cf}\ASCOM\Dome"
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir="."
OutputBaseFilename="DomeSim 5-0-6 Setup"
Compression=lzma
SolidCompression=yes
; Put there by Platform if Driver Installer Support selected
WizardImageFile="C:\Program Files\ASCOM\InstallGen\Resources\WizardImage.bmp"
LicenseFile="C:\Program Files\ASCOM\InstallGen\Resources\CreativeCommons.txt"
; {cf}\ASCOM\Uninstall\Dome folder created by Platform, always
UninstallFilesDir="{cf}\ASCOM\Uninstall\Dome\DomeSim"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{cf}\ASCOM\Uninstall\Dome\DomeSim"
; TODO: Add subfolders below {app} as needed (e.g. Name: "{app}\MyFolder")

[InstallDelete]
; ditch any old source
Type: filesandordirs; Name: "{app}\Source\Dome Simulator"
Type: filesandordirs; Name: "{app}\Source\DomeSim"

[Tasks]
 Name: SourceCode; Description: "Install source code"; Flags: unchecked
 
[Files]
Source: "DomeSim.exe"; DestDir: "{app}"; Flags: ignoreversion; BeforeInstall: Unregister('DomeSim.exe')
; Require a read-me HTML to appear after installation, maybe driver's Help doc
; Source: "DomeSimHelp.htm"; DestDir: "{app}"; Flags: ignoreversion isreadme
; Source code
Source: "Bug72T-sm.bmp"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "Dome.cls"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "DomeSim Setup.iss"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "DomeSim.vbp"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "DomeSim.vbw"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "DomeSim-ref.exe"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "ErrorConstants.bas"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "frmHandBox.frm"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "frmHandBox.frx"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "frmSetup.frm"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "frmSetup.frx"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "frmShow.frm"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "frmShow.frx"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "Hardware.bas"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "hotlink.cur"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "ReadMe.txt"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "register.bat"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "saturnc.jpg"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "Startup.bas"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "unregister.bat"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;
Source: "WindowsAPI.bas"; DestDir: "{app}\Source\DomeSim"; Tasks: SourceCode;

;Only if COM Local Server
[Run]
Filename: "{app}\DomeSim.exe"; Parameters: "/regserver"

;Only if COM Local Server
[UninstallRun]
Filename: "{app}\DomeSim.exe"; Parameters: "/unregserver"

[Code]
procedure Unregister(FileName: String);
var
  ResultCode: Integer;
begin
  // if component already exists, then unregister it
  if FileExists(ExpandConstant('{app}\' + FileName)) then
  begin
    Exec(ExpandConstant('{app}\' + FileName), '/unregserver', '', 0,
     ewWaitUntilTerminated, ResultCode)
  end;
end;
