;
; Installer for ASCOM Mount driver for X2
; (No longer uses the debug build of the DLL)
; 17/08/12 pwgs Added ignoreversion to ASCOM.Telescope.dll source line to ensure that update is effected
;               when a previous version is installed even if the version number hasn't changed
; 17/08/12 pwgs Changed TSXPath to AnsiString type for compatibility with Unicode Inno setup (may need to 
;               go back to String for non-unicode Inno versions)
; 17/08/12 pwgs Updated to version 1.0.3
;
[Setup]
AppName=X2 Mount Driver for ASCOM Telescopes
AppVersion=1.0.3
AppPublisher=ASCOM Initiative
AppPublisherURL=http://ascom-standards.org/
AppSupportURL=http://tech.groups.yahoo.com/group/ASCOM-Talk/
AppUpdatesURL=http://ascom-standards.org/
VersionInfoVersion=1.0.3
MinVersion=0,5.0.2195sp4
CreateAppDir=no
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir=.
OutputBaseFilename=X2AscomTelescope(1.0.3)Setup
Compression=lzma
SolidCompression=yes
WizardImageFile="WizardImage.bmp"
LicenseFile="CreativeCommons.txt"
; {cf}\ASCOM\Uninstall\ folder created by Platform, always
UninstallFilesDir="{cf}\ASCOM\Uninstall\X2\Telescope"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "../Release/ASCOM.Telescope.dll";   DestDir: "{code:GetDLLPath}";  Flags: ignoreversion
Source: "../mountlist ASCOM.txt"; DestDir: "{code:GetMiscPath}"; Flags: ignoreversion
Source: "X2-ASCOM-ReadMe.txt";  DestDir: "{code:GetDLLPath}";  Flags: ignoreversion isreadme

[Code]
const
  DLLType = 'Mount';

var
  TSXPath: AnsiString;

function GetTSXPath: Boolean;
var
  DocPath: String;
begin
  DocPath := ExpandConstant('{userdocs}\Software Bisque\TheSkyX Professional Edition\TheSkyXInstallPath.txt')
  if (LoadStringFromFile(DocPath,TSXPath)) then
  begin
    Result := True;
  end else
  begin
    DocPath := ExpandConstant('{userdocs}\Software Bisque\TheSkyX Serious Astronomer Edition\TheSkyXInstallPath.txt')
    if (LoadStringFromFile(DocPath,TSXPath)) then
    begin
      Result := True;
    end else
    begin
      DocPath := ExpandConstant('{userdocs}\Software Bisque\TheSkyX Theater Edition\TheSkyXInstallPath.txt')
      if (LoadStringFromFile(DocPath,TSXPath)) then
      begin
        Result := True;
      end else
      begin
        Result := False;
      end;
    end;
  end;
end;

function GetDLLPath(Default : String): String;
begin
    Result := TSXPath + '\Resources\Common\Plugins\' + DLLType + 'Plugins'
end;

function GetMiscPath(Default : String): String;
begin
    Result := TSXPath + '\Resources\Common\Miscellaneous Files'
end;

function InitializeSetup(): Boolean;
begin
  Result := GetTSXPath();
  if not Result then
  begin
     MsgBox('TheSkyX must be run on this system at least once before this plugin can be installed.  ' +
            'Setup cannot continue.', mbError, mb_Ok);
  end;
end;
