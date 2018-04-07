; ASCOM Install Script for ScopeSim
; Jon Brewster

[Setup]
AppName=ASCOM Telescope Simulator
AppVerName=ScopeSim 5.0.9
AppVersion=5.0.9
AppPublisher=Jon Brewster jon@brewsters.net
AppPublisherURL=http://ascom-standards.org/
AppSupportURL=http://ascom-standards.org/
AppUpdatesURL=http://ascom-standards.org/
MinVersion=0,5.0.2195sp4
DefaultDirName="{cf}\ASCOM\Telescope"
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir="."
OutputBaseFilename="ScopeSim 5-0-9 Setup"
Compression=lzma
SolidCompression=yes
; Put there by Platform if Driver Installer Support selected
WizardImageFile="C:\Program Files\ASCOM\InstallGen\Resources\WizardImage.bmp"
LicenseFile="C:\Program Files\ASCOM\InstallGen\Resources\CreativeCommons.txt"
; {cf}\ASCOM\Uninstall\Telescope folder created by Platform, always
UninstallFilesDir="{cf}\ASCOM\Uninstall\Telescope\ScopeSim"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{cf}\ASCOM\Uninstall\Telescope\ScopeSim"
; TODO: Add subfolders below {app} as needed (e.g. Name: "{app}\MyFolder")

[InstallDelete]
; ditch any old source
Type: filesandordirs; Name: "{app}\Source\Simulator"
Type: filesandordirs; Name: "{app}\Source\ScopeSim"

[Tasks]
 Name: SourceCode; Description: "Install source code"; Flags: unchecked
 
[Files]
Source: "ScopeSim.exe"; DestDir: "{app}"; Flags: ignoreversion; BeforeInstall: Unregister('ScopeSim.exe')
; Require a read-me HTML to appear after installation, maybe driver's Help doc
Source: "ScopeSimHelp.htm"; DestDir: "{app}"; Flags: ignoreversion isreadme
; Source code
Source: "astro32.bas"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "astro32.dll"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "AxisRates.cls"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "Bug72T-sm.bmp"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "Defines.bas"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "ErrorConstants.bas"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "frmHandBox.frm"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "frmHandBox.frx"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "frmSetup.frm"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "frmSetup.frx"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "frmShow.frm"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "frmShow.frx"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "Globals.bas"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "hotlink.cur"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "im_spacecom.gif"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "im_spacecom-sm.gif"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "Rate.cls"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "ReadMe.txt"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "register.bat"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "ScopeSim Setup.iss"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "ScopeSim.vbp"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "ScopeSim.vbw"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "ScopeSimHelp.htm"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "ScopeSim-ref.exe"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "Startup.bas"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "Telescope.cls"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "TrackingRates.cls"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "unregister.bat"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "Util.bas"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;
Source: "WindowsAPI.bas"; DestDir: "{app}\Source\ScopeSim"; Tasks: SourceCode;

;Only if COM Local Server
[Run]
Filename: "{app}\ScopeSim.exe"; Parameters: "/regserver"

;Only if COM Local Server
[UninstallRun]
Filename: "{app}\ScopeSim.exe"; Parameters: "/unregserver"

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
