; ASCOM Install Script for FocusSim
; Jon Brewster

[Setup]
AppName=ASCOM Focuser Simulator
AppVerName=FocusSim 5.0.5
AppVersion=5.0.5
AppPublisher=Jon Brewster jon@brewsters.net
AppPublisherURL=http://ascom-standards.org/
AppSupportURL=http://ascom-standards.org/
AppUpdatesURL=http://ascom-standards.org/
MinVersion=0,5.0.2195sp4
DefaultDirName="{cf}\ASCOM\Focuser"
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir="."
OutputBaseFilename="FocusSim 5-0-5 Setup"
Compression=lzma
SolidCompression=yes
; Put there by Platform if Driver Installer Support selected
WizardImageFile="C:\Program Files\ASCOM\InstallGen\Resources\WizardImage.bmp"
LicenseFile="C:\Program Files\ASCOM\InstallGen\Resources\CreativeCommons.txt"
; {cf}\ASCOM\Uninstall\Focuser folder created by Platform, always
UninstallFilesDir="{cf}\ASCOM\Uninstall\Focuser\FocusSim"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{cf}\ASCOM\Uninstall\Focuser\FocusSim"
; TODO: Add subfolders below {app} as needed (e.g. Name: "{app}\MyFolder")

[InstallDelete]
; ditch any old source
Type: filesandordirs; Name: "{app}\Source\Focuser Simulator"
Type: filesandordirs; Name: "{app}\Source\FocusSim"

[Tasks]
 Name: SourceCode; Description: "Install source code"; Flags: unchecked
 
[Files]
Source: "FocusSim.exe"; DestDir: "{app}"; Flags: ignoreversion; BeforeInstall: Unregister('FocusSim.exe')
; Require a read-me HTML to appear after installation, maybe driver's Help doc
; Source: "FocusSimHelp.htm"; DestDir: "{app}"; Flags: ignoreversion isreadme
; Source code
Source: "Bug72T-sm.bmp"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "ErrorConstants.bas"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "Focus.cls"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "FocusSim Setup.iss"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "FocusSim.vbp"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "FocusSim.vbw"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "FocusSim-ref.exe"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "frmHandBox.frm"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "frmHandBox.frx"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "frmSetup.frm"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "frmSetup.frx"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "frmShow.frm"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "frmShow.frx"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "hotlink.cur"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "ReadMe.txt"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "register.bat"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "saturnc.jpg"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "Startup.bas"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "unregister.bat"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;
Source: "WindowsAPI.bas"; DestDir: "{app}\Source\FocusSim"; Tasks: SourceCode;

;Only if COM Local Server
[Run]
Filename: "{app}\FocusSim.exe"; Parameters: "/regserver"

;Only if COM Local Server
[UninstallRun]
Filename: "{app}\FocusSim.exe"; Parameters: "/unregserver"

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
