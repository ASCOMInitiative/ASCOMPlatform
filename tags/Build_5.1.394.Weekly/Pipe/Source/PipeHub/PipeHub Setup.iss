; ASCOM Install Script for Pipe and Hub
; Jon Brewster

[Setup]
AppName=ASCOM Pipe and Hub
AppVerName=PipeHub 5.0.6
AppVersion=5.0.6
AppPublisher=Jon Brewster jon@brewsters.net
AppPublisherURL=http://ascom-standards.org/
AppSupportURL=http://ascom-standards.org/
AppUpdatesURL=http://ascom-standards.org/
MinVersion=0,5.0.2195sp4
DefaultDirName="{cf}\ASCOM\Telescope"
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir="."
OutputBaseFilename="PipeHub 5-0-6 Setup"
Compression=lzma
SolidCompression=yes
; Put there by Platform if Driver Installer Support selected
WizardImageFile="C:\Program Files\ASCOM\InstallGen\Resources\WizardImage.bmp"
LicenseFile="C:\Program Files\ASCOM\InstallGen\Resources\CreativeCommons.txt"
; {cf}\ASCOM\Uninstall\Telescope folder created by Platform, always
UninstallFilesDir="{cf}\ASCOM\Uninstall\Telescope\PipeHub"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{cf}\ASCOM\Uninstall\Telescope\PipeHub"
; TODO: Add subfolders below {app} as needed (e.g. Name: "{app}\MyFolder")

[InstallDelete]
; ditch any old source
Type: filesandordirs; Name: "{app}\Source\PipeHub"
; get rid of old files no longer needed
Type: files; Name: "{app}\PipeHelp.htm"
Type: files; Name: "{app}\HubHelp.htm"

[Tasks]
 Name: SourceCode; Description: "Install source code"; Flags: unchecked
 
[Files]
Source: "Pipe.exe"; DestDir: "{app}"; Flags: ignoreversion; BeforeInstall: Unregister('Pipe.exe')
Source: "Hub.exe"; DestDir: "{app}"; Flags: ignoreversion; BeforeInstall: Unregister('Hub.exe')
; Require a read-me HTML to appear after installation, maybe driver's Help doc
Source: "PipeHubHelp.htm"; DestDir: "{app}"; Flags: ignoreversion isreadme
; Source code
Source: "AxisRates.cls"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "Bug72T-sm.bmp"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "Dome.cls"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "DomeHW.bas"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "ErrorConstants.bas"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "Focuser.cls"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "FocuserHW.bas"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "frmHandBox.frm"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "frmHandBox.frx"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "frmSetup.frm"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "frmSetup.frx"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "hotlink.cur"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "Hub.vbp"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "Hub.vbw"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "Hub-ref.exe"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "IObjectSafety.idl"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "IObjectSafety.tlb"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "ObjectSafety.bas"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "PipeHub Setup.iss"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "Pipe.vbp"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "Pipe.vbw"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "PipeHub.vbg"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "PipeHubHelp.htm"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "Pipe-ref.exe"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "Public.bas"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "Rate.cls"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "ReadMe.txt"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "register.bat"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "saturn.ico"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "saturnc.jpg"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "ScopeHW.bas"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "Startup.bas"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "Telescope.cls"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "TrackingRates.cls"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "unregister.bat"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;
Source: "WindowsAPI.bas"; DestDir: "{app}\Source\PipeHub"; Tasks: SourceCode;

;Only if COM Local Server
[Run]
Filename: "{app}\Pipe.exe"; Parameters: "/regserver"
Filename: "{app}\Pipe.exe"; Parameters: "-r"
Filename: "{app}\Hub.exe"; Parameters: "/regserver"
Filename: "{app}\Hub.exe"; Parameters: "-r"

;Only if COM Local Server
[UninstallRun]
Filename: "{app}\Pipe.exe"; Parameters: "/unregserver"
Filename: "{app}\Hub.exe"; Parameters: "/unregserver"

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
