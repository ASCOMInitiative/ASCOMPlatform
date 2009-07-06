; ASCOM Install Script for SwitchSim
; Jon Brewster

[Setup]
AppName=ASCOM Switch Simulator
AppVerName=SwitchSim 5.0.2
AppVersion=5.0.2
AppPublisher=Jon Brewster jon@brewsters.net
AppPublisherURL=http://ascom-standards.org/
AppSupportURL=http://ascom-standards.org/
AppUpdatesURL=http://ascom-standards.org/
MinVersion=0,5.0.2195sp4
DefaultDirName="{cf}\ASCOM\Switch"
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir="."
OutputBaseFilename="SwitchSim 5-0-2 Setup"
Compression=lzma
SolidCompression=yes
; Put there by Platform if Driver Installer Support selected
WizardImageFile="C:\Program Files\ASCOM\InstallGen\Resources\WizardImage.bmp"
LicenseFile="C:\Program Files\ASCOM\InstallGen\Resources\CreativeCommons.txt"
; {cf}\ASCOM\Uninstall\Switch folder created by Platform, always
UninstallFilesDir="{cf}\ASCOM\Uninstall\Switch\SwitchSim"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{cf}\ASCOM\Uninstall\Switcher\SwitchSim"
; TODO: Add subfolders below {app} as needed (e.g. Name: "{app}\MyFolder")

[InstallDelete]
; ditch any old source
Type: filesandordirs; Name: "{app}\Source\Switch Simulator"
Type: filesandordirs; Name: "{app}\Source\SwitchSim"

[Tasks]
 Name: SourceCode; Description: "Install source code"; Flags: unchecked
 
[Files]
Source: "SwitchSim.exe"; DestDir: "{app}"; Flags: ignoreversion; BeforeInstall: Unregister('SwitchSim.exe')
; Require a read-me HTML to appear after installation, maybe driver's Help doc
; Source: "SwitchSimHelp.htm"; DestDir: "{app}"; Flags: ignoreversion isreadme
; Source code
Source: "Bug72T-sm.bmp"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "ErrorConstants.bas"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "frmHandBox.frm"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "frmHandBox.frx"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "frmSetup.frm"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "frmSetup.frx"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "frmShow.frm"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "frmShow.frx"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "hotlink.cur"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "ReadMe.txt"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "register.bat"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "saturn.ico"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "saturnc.jpg"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "Startup.bas"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "Switch.cls"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "SwitchSim Setup.iss"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "SwitchSim.vbp"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "SwitchSim.vbw"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "SwitchSim-ref.exe"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "unregister.bat"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;
Source: "WindowsAPI.bas"; DestDir: "{app}\Source\SwitchSim"; Tasks: SourceCode;

;Only if COM Local Server
[Run]
Filename: "{app}\SwitchSim.exe"; Parameters: "/regserver"
Filename: "{app}\SwitchSim.exe"; Parameters: "-r"

;Only if COM Local Server
[UninstallRun]
Filename: "{app}\SwitchSim.exe"; Parameters: "/unregserver"

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
