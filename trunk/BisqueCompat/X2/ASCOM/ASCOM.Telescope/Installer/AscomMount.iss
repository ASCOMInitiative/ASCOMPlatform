; Installer for ASCOM Mount driver for X2
;
; ** WARNING **
; THIS USES THE DEBUG BUILD!!!
;
[Setup]
AppName=X2 Mount Driver BETA for ASCOM Telescopes
AppVersion=0.9.4
AppPublisher=ASCOM Initiative
AppPublisherURL=http://ascom-standards.org/
AppSupportURL=http://tech.groups.yahoo.com/group/ASCOM-Talk/
AppUpdatesURL=http://ascom-standards.org/
VersionInfoVersion=1.0.0
MinVersion=0,5.0.2195sp4
CreateAppDir=no
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir=.
OutputBaseFilename=X2AscomTelescope(0.9.4)Setup
Compression=lzma
SolidCompression=yes
WizardImageFile="WizardImage.bmp"
LicenseFile="CreativeCommons.txt"
; {cf}\ASCOM\Uninstall\ folder created by Platform, always
UninstallFilesDir="{cf}\ASCOM\Uninstall\X2\Telescope"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "../Debug/ASCOM.Telescope.dll";   DestDir: "{code:GetDLLPath}";  Flags: 
Source: "../mountlist ASCOM.txt"; DestDir: "{code:GetMiscPath}"; Flags: ignoreversion
Source: "X2-ASCOM-ReadMe.txt";  DestDir: "{code:GetDLLPath}";  Flags: ignoreversion isreadme

[Code]
const
  DLLType = 'Mount';

var
  TSXPath: String;

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
