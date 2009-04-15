; ASCOM Install Script for POTH
; Jon Brewster

[Setup]
AppName=ASCOM POTH
AppVerName=POTH 5.0.4
AppVersion=5.0.4
AppPublisher=Jon Brewster jon@brewsters.net
AppPublisherURL=http://ascom-standards.org/
AppSupportURL=http://ascom-standards.org/
AppUpdatesURL=http://ascom-standards.org/
MinVersion=0,5.0.2195sp4
DefaultDirName="{cf}\ASCOM\Telescope"
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir="."
OutputBaseFilename="POTH 5-0-4 Setup"
Compression=lzma
SolidCompression=yes
; Put there by Platform if Driver Installer Support selected
WizardImageFile="C:\Program Files\ASCOM\InstallGen\Resources\WizardImage.bmp"
LicenseFile="C:\Program Files\ASCOM\InstallGen\Resources\CreativeCommons.txt"
; {cf}\ASCOM\Uninstall\Telescope folder created by Platform, always
UninstallFilesDir="{cf}\ASCOM\Uninstall\Telescope\POTH"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{cf}\ASCOM\Uninstall\Telescope\POTH"
; TODO: Add subfolders below {app} as needed (e.g. Name: "{app}\MyFolder")

[InstallDelete]
; ditch any old source
Type: filesandordirs; Name: "{app}\Source\POTH"

[Tasks]
 Name: SourceCode; Description: "Install source code"; Flags: unchecked
 
[Files]
Source: "POTH.exe"; DestDir: "{app}"; Flags: ignoreversion; BeforeInstall: Unregister('POTH.exe')
; Require a read-me HTML to appear after installation, maybe driver's Help doc
Source: "POTHHelp.htm"; DestDir: "{app}"; Flags: ignoreversion isreadme
; Source code
Source: "astro32.bas"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "astro32.dll"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "AxisRates.cls"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "Bug72T-sm.bmp"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "Dome.cls"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "DomeHW.bas"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "Dsync.bas"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "ErrorConstants.bas"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "Focuser.cls"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "FocuserHW.bas"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "frmHandBox.frm"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "frmHandBox.frx"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "frmSetup.frm"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "frmSetup.frx"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "frmShow.frm"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "frmShow.frx"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "hotlink.cur"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "IObjectSafety.idl"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "IObjectSafety.tlb"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "ObjectSafety.bas"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "POTH Setup.iss"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "POTH.vbp"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "POTH.vbw"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "POTHHelp.htm"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "POTH-ref.exe"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "Public.bas"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "Rate.cls"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "ReadMe.txt"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "register.bat"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "saturn.ico"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "saturnc.jpg"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "ScopeHW.bas"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "Startup.bas"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "Telescope.cls"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "TrackingRates.cls"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "unregister.bat"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;
Source: "WindowsAPI.bas"; DestDir: "{app}\Source\POTH"; Tasks: SourceCode;

;Only if COM Local Server
[Run]
Filename: "{app}\POTH.exe"; Parameters: "/regserver"
Filename: "{app}\POTH.exe"; Parameters: "-r"

;Only if COM Local Server
[UninstallRun]
Filename: "{app}\POTH.exe"; Parameters: "/unregserver"

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
