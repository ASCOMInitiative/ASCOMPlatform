; ASCOM Install Script for Meade LX200GPS/R
; Jon Brewster

[Setup]
AppName=ASCOM Meade LX200GPS/R Telescope Driver
AppVerName=MeadeLX200GPS 5.0.0
AppVersion=5.0.0
AppPublisher=Jon Brewster jon@brewsters.net
AppPublisherURL=http://ascom-standards.org/
AppSupportURL=http://ascom-standards.org/
AppUpdatesURL=http://ascom-standards.org/
MinVersion=0,5.0.2195sp4
DefaultDirName="{cf}\ASCOM\Telescope"
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir="."
OutputBaseFilename="MeadeLX200GPS 5.0.0 Setup"
Compression=lzma
SolidCompression=yes
; Put there by Platform if Driver Installer Support selected
WizardImageFile="C:\Program Files\ASCOM\InstallGen\Resources\WizardImage.bmp"
LicenseFile="C:\Program Files\ASCOM\InstallGen\Resources\CreativeCommons.txt"
; {cf}\ASCOM\Uninstall\Telescope folder created by Platform, always
UninstallFilesDir="{cf}\ASCOM\Uninstall\Telescope\Meade LX200GPS"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{cf}\ASCOM\Uninstall\Telescope\Meade LX200GPS"
; TODO: Add subfolders below {app} as needed (e.g. Name: "{app}\MyFolder")

[InstallDelete]
; ditch any old source (nameing changed acrossed versions)
Type: filesandordirs; Name: "{app}\Source\Meade LX200GPS and R"
Type: filesandordirs; Name: "{app}\Source\Meade LX200GPS"

[Tasks]
 Name: SourceCode; Description: "Install source code"; Flags: unchecked
 Name: OldMeade; Description: "Delete Meade LX200 and Autostar I drivers. (The LX200GPS/R driver used to be integrated in this common driver.)"
 
[Files]
Source: "MeadeLX200GPS Driver.exe"; DestDir: "{app}"; Flags: ignoreversion; BeforeInstall: Unregister('MeadeLX200GPS Driver.exe')
; Require a read-me HTML to appear after installation, maybe driver's Help doc
Source: "MeadeLX200GPSHelp.htm"; DestDir: "{app}"; Flags: ignoreversion isreadme
; Source code
Source: "astro32.bas"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "astro32.dll"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "AxisRates.cls"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "Bug72T-sm.bmp"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "Common.bas"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "ErrorConstants.bas"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "Focuser.cls"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "frmSetup.frm"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "frmSetup.frx"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "hotlink.cur"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "info.ico"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "Meade LX200GPS Setup.iss"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "Meade LX200GPS.vbp"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "Meade LX200GPS.vbw"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "MeadeLX200GPS Driver-ref.exe"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "MeadeLX200GPSHelp.htm"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "Rate.cls"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "ReadMe.txt"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "register.bat"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "Telescope.cls"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "TrackingRates.cls"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;
Source: "unregister.bat"; DestDir: "{app}\Source\Meade LX200GPS"; Tasks: SourceCode;

;Only if COM Local Server
[Run]
Filename: "{app}\MeadeLX200GPS Driver.exe"; Parameters: "/regserver"
Filename: "{app}\MeadeLX200GPS Driver.exe"; Parameters: "-r"
Filename: "{app}\MeadeLX200GPS Driver.exe"; Parameters: "-do"; Tasks: OldMeade; AfterInstall: DeleteOld()

;Only if COM Local Server
[UninstallRun]
Filename: "{app}\MeadeLX200GPS Driver.exe"; Parameters: "-ur"
Filename: "{app}\MeadeLX200GPS Driver.exe"; Parameters: "/unregserver"

[Code]
// version 1.0 of code from Jon Brewster

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

procedure UnregisterDLL(FileName: String);
var
  ResultCode: Integer;
begin
  // if component already exists, then unregister it
  if FileExists(ExpandConstant('{app}\' + FileName)) then
  begin
    Exec(ExpandConstant('{win}\system32\regsvr32.exe'), ExpandConstant('/u /s "{app}\' + FileName + '"'), '', 0,
     ewWaitUntilTerminated, ResultCode)
  end;
end;

procedure DeleteEXE(FileName: String);
begin
  if FileExists(ExpandConstant('{app}\' + FileName)) then
  begin
      Unregister(FileName)
      DeleteFile(ExpandConstant('{app}\' + FileName))
  end;
end;

procedure DeleteDLL(FileName: String);
begin
  if FileExists(ExpandConstant('{app}\' + FileName)) then
  begin
      UnregisterDLL(FileName)
      DeleteFile(ExpandConstant('{app}\' + FileName))
  end;
end;

procedure DeleteOld();
begin
  DeleteDLL('Meade Driver.dll')
  DeleteEXE('MeadeEx Driver.exe')
  if FileExists(ExpandConstant('{app}\MeadeHelp.htm')) then
  begin
      DeleteFile(ExpandConstant('{app}\MeadeHelp.htm'))
  end;
end;
