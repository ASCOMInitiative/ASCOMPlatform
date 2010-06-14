; Gemini Installer

; ***** REQUIRES INNO 5.3.4 OR LATER *****

; Original build
; Setup Build 1 Released

; Changed source directories to new x86 and x64 locations
; Setup Build 2 Released

; Removed redundant registry access from platform install test and corrected platform updater name to 5.5
; Genericised "platform not installed" messages to work with future platforms as well as 5 and 5.5
; Setup Build 3 Released

[Setup]
; SetupVersion is the installer version number and appears as the fourth digit in the installer version number
; Only increment SetupVersion when you make changes to the installer, it does not ned to increment when you make Gemini code changes
#define Public SetupVersion 0; Setup program version number

; Determiine the driver version number from the GeminiTelescope executable (only Major, Minor and Release are used
; as SetupVersion replaces Build)
#define Public Major 0
#define Public Minor 0
#define Public Release 0
#define Public Build 0
#define AppVer ParseVersion("..\GeminiTelescope\bin\x64\Release\ASCOM.GeminiTelescope.exe", Major ,Minor ,Release ,Build) ; define version variables
#define AppVer str(Major) + "." + str(Minor) + "." + str(Release) + "." + str(SetupVersion) ; redefine to include setup version

AppCopyright=Copyright © 2010 ASCOM Gemini Developers Group
;AppID must not change to maintain a consistent uninstall experience although AppName can be changed.
;This value is hard coded in the uninstall code below. If you do change this you must change the corresponding reference in
;the [Code] CurStepChanged section
AppID=ASCOM.GeminiTelescope

#emit "AppName=Gemini Driver Installler " + str(Major) + "." + str(Minor);
#emit "AppVerName=ASCOM Gemini Telescope Driver (" + Appver + ")"
#emit "AppVersion=" + AppVer
AppPublisher=ASCOM Gemini Developers
AppPublisherURL=http://ascom-standards.org/
AppSupportURL=http://tech.groups.yahoo.com/group/ASCOM-Talk/
AppUpdatesURL=http://ascom-standards.org/
MinVersion=0,5.0.2195sp4
DefaultDirName={cf}\ASCOM\Telescope\Gemini
DefaultGroupName=ASCOM Platform\Gemini
DisableDirPage=true
DisableProgramGroupPage=true
OutputDir=.
PrivilegesRequired=admin
#emit "OutputBaseFilename=GeminiTelescopeInstaller(" + AppVer +")"
Compression=lzma
SolidCompression=true
SetupIconFile=GeminiSB32x32.ico
ShowLanguageDialog=auto
WizardImageFile=NewWizardImage.bmp
WizardSmallImageFile=ASCOMLogo.bmp
Uninstallable=true
DirExistsWarning=no
UninstallDisplayIcon={cf}\ASCOM\Telescope\Gemini\GeminiSB32x32.ico
UninstallFilesDir={cf}\ASCOM\Uninstall\Gemini
UsePreviousTasks=true
VersionInfoCompany=ASCOM Gemini Developers
VersionInfoCopyright=ASCOM Gemini Developers
VersionInfoDescription=ASCOM Gemini Telescope Driver
VersionInfoProductName=ASCOM Gemini Telescope Driver
#emit "VersionInfoProductVersion=" + AppVer
#emit "VersionInfoVersion=" + AppVer
ArchitecturesInstallIn64BitMode=x64
SetupLogging=true
WindowVisible=false

[Languages]
Name: english; MessagesFile: compiler:Default.isl

[Dirs]
Name: {cf}\ASCOM\Uninstall\Gemini
Name: {cf}\ASCOM\Telescope\Gemini\GeminiTelescopeServedClasses

[Tasks]
Name: desktopicon; Description: Install a Gemini start icon on your desktop; GroupDescription: """Installation Tasks"""

[Files]
;Install Gemini for 64bit and 32bit systems
Source: ..\GeminiTelescope\bin\x64\Release\ASCOM.GeminiTelescope.exe; DestDir: {cf}\ASCOM\Telescope\Gemini; Check: Is64BitInstallMode; Flags: ignoreversion
Source: ..\GeminiTelescope\bin\x64\Release\ASCOM.GeminiTelescope.pdb; DestDir: {cf}\ASCOM\Telescope\Gemini; Check: Is64BitInstallMode; Flags: ignoreversion
Source: ..\GeminiTelescope\bin\x86\Release\ASCOM.GeminiTelescope.exe; DestDir: {cf}\ASCOM\Telescope\Gemini; Check: NotIs64BitInstallMode; Flags: ignoreversion
Source: ..\GeminiTelescope\bin\x86\Release\ASCOM.GeminiTelescope.pdb; DestDir: {cf}\ASCOM\Telescope\Gemini; Check: NotIs64BitInstallMode; Flags: ignoreversion

Source: ..\GeminiTelescope\bin\x64\Release\Interop.SpeechLib.dll; DestDir: {cf}\ASCOM\Telescope\Gemini; Check: Is64BitInstallMode; Flags: ignoreversion
Source: ..\GeminiTelescope\bin\x86\Release\Interop.SpeechLib.dll; DestDir: {cf}\ASCOM\Telescope\Gemini; Check: NotIs64BitInstallMode; Flags: ignoreversion

Source: ..\Telescope\bin\x64\Release\ASCOM.GeminiTelescope.Telescope.dll; DestDir: {cf}\ASCOM\Telescope\Gemini\GeminiTelescopeServedClasses; Check: Is64BitInstallMode; Flags: ignoreversion; Tasks: ; Languages: 
Source: ..\Telescope\bin\x64\Release\ASCOM.GeminiTelescope.Telescope.pdb; DestDir: {cf}\ASCOM\Telescope\Gemini\GeminiTelescopeServedClasses; Check: Is64BitInstallMode; Flags: ignoreversion
Source: ..\Telescope\bin\x86\Release\ASCOM.GeminiTelescope.Telescope.dll; DestDir: {cf}\ASCOM\Telescope\Gemini\GeminiTelescopeServedClasses; Check: NotIs64BitInstallMode; Flags: ignoreversion
Source: ..\Telescope\bin\x86\Release\ASCOM.GeminiTelescope.Telescope.pdb; DestDir: {cf}\ASCOM\Telescope\Gemini\GeminiTelescopeServedClasses; Check: NotIs64BitInstallMode; Flags: ignoreversion

Source: ..\Focuser\bin\x64\Release\ASCOM.GeminiTelescope.Focuser.dll; DestDir: {cf}\ASCOM\Telescope\Gemini\GeminiTelescopeServedClasses; Check: Is64BitInstallMode; Flags: ignoreversion
Source: ..\Focuser\bin\x64\Release\ASCOM.GeminiTelescope.Focuser.pdb; DestDir: {cf}\ASCOM\Telescope\Gemini\GeminiTelescopeServedClasses; Check: Is64BitInstallMode; Flags: ignoreversion
Source: ..\Focuser\bin\x86\Release\ASCOM.GeminiTelescope.Focuser.dll; DestDir: {cf}\ASCOM\Telescope\Gemini\GeminiTelescopeServedClasses; Check: NotIs64BitInstallMode; Flags: ignoreversion
Source: ..\Focuser\bin\x86\Release\ASCOM.GeminiTelescope.Focuser.pdb; DestDir: {cf}\ASCOM\Telescope\Gemini\GeminiTelescopeServedClasses; Check: NotIs64BitInstallMode; Flags: ignoreversion

; Language resources
Source: ..\GeminiTelescope\bin\x64\Release\en-GB\ASCOM.GeminiTelescope.resources.dll; DestDir: {cf}\ASCOM\Telescope\Gemini\en-GB; Check: Is64BitInstallMode; Flags: ignoreversion
Source: ..\GeminiTelescope\bin\x86\Release\en-GB\ASCOM.GeminiTelescope.resources.dll; DestDir: {cf}\ASCOM\Telescope\Gemini\en-GB; Check: NotIs64BitInstallMode; Flags: ignoreversion

Source: ..\GeminiTelescope\bin\x64\Release\ru\ASCOM.GeminiTelescope.resources.dll; DestDir: {cf}\ASCOM\Telescope\Gemini\ru; Check: Is64BitInstallMode; Flags: ignoreversion
Source: ..\GeminiTelescope\bin\x86\Release\ru\ASCOM.GeminiTelescope.resources.dll; DestDir: {cf}\ASCOM\Telescope\Gemini\ru; Check: NotIs64BitInstallMode; Flags: ignoreversion

Source: ..\GeminiTelescope\bin\x64\Release\fr\ASCOM.GeminiTelescope.resources.dll; DestDir: {cf}\ASCOM\Telescope\Gemini\fr; Check: Is64BitInstallMode; Flags: ignoreversion
Source: ..\GeminiTelescope\bin\x86\Release\fr\ASCOM.GeminiTelescope.resources.dll; DestDir: {cf}\ASCOM\Telescope\Gemini\fr; Check: NotIs64BitInstallMode; Flags: ignoreversion

;Documentation help file
Source: ..\GeminiTelescope\Documentation\Gemini Telescope Net Installation and Operation.pdf; DestDir: {cf}\ASCOM\Telescope\Gemini; Flags: ignoreversion

;Gemini Icon
Source: ..\GeminiTelescope\bin\Release\GeminiSB32x32.ico; DestDir: {cf}\ASCOM\Telescope\Gemini; Flags: ignoreversion

;Catalog files
Source: ..\Catalogs\Arp Peculiar Galaxies.guc; DestDir: {commonappdata}\ASCOM\Gemini Telescope .NET\Catalogs; Flags: ignoreversion
Source: ..\Catalogs\Bright Star List.guc; DestDir: {commonappdata}\ASCOM\Gemini Telescope .NET\Catalogs; Flags: ignoreversion
Source: ..\Catalogs\CED bright diffuse nebulae.guc; DestDir: {commonappdata}\ASCOM\Gemini Telescope .NET\Catalogs; Flags: ignoreversion
Source: ..\Catalogs\Galactic Planetary Objects.guc; DestDir: {commonappdata}\ASCOM\Gemini Telescope .NET\Catalogs; Flags: ignoreversion
Source: ..\Catalogs\IC.guc; DestDir: {commonappdata}\ASCOM\Gemini Telescope .NET\Catalogs; Flags: ignoreversion
Source: ..\Catalogs\Lynds Bright Nebulae.guc; DestDir: {commonappdata}\ASCOM\Gemini Telescope .NET\Catalogs; Flags: ignoreversion
Source: ..\Catalogs\Lynds Dark Nebulae.guc; DestDir: {commonappdata}\ASCOM\Gemini Telescope .NET\Catalogs; Flags: ignoreversion
Source: ..\Catalogs\Messier.guc; DestDir: {commonappdata}\ASCOM\Gemini Telescope .NET\Catalogs; Flags: ignoreversion
Source: ..\Catalogs\NGC.guc; DestDir: {commonappdata}\ASCOM\Gemini Telescope .NET\Catalogs; Flags: ignoreversion
Source: ..\Catalogs\SAO Catalog.guc; DestDir: {commonappdata}\ASCOM\Gemini Telescope .NET\Catalogs; Flags: ignoreversion
Source: ..\Catalogs\Sharpless HII Regions.guc; DestDir: {commonappdata}\ASCOM\Gemini Telescope .NET\Catalogs; Flags: ignoreversion
Source: ..\Catalogs\VDB reflection nebulae.guc; DestDir: {commonappdata}\ASCOM\Gemini Telescope .NET\Catalogs; Flags: ignoreversion
Source: ..\Catalogs\Washington Double Star Catalog.guc; DestDir: {commonappdata}\ASCOM\Gemini Telescope .NET\Catalogs; Flags: ignoreversion


[Icons]
Name: {commondesktop}\Gemini Telescope; Filename: {cf}\ASCOM\Telescope\Gemini\ASCOM.GeminiTelescope.exe; Tasks: desktopicon

[Run]
; Run the driver to Register for COM
Filename: {cf}\ASCOM\Telescope\Gemini\ASCOM.GeminiTelescope.exe; Parameters: /Register; Flags: runhidden; StatusMsg: Registering Gemini for COM

[UninstallRun]
;Run the driver to Unregister from COM
Filename: {cf}\ASCOM\Telescope\Gemini\ASCOM.GeminiTelescope.exe; Parameters: /Unregcom; Flags: runhidden; StatusMsg: Unregistering Gemini for COM

[UninstallDelete]
Type: files; Name: {cf}\ASCOM\Telescope\Gemini\GeminiTelescopeServedClasses\*.*
Type: files; Name: {cf}\ASCOM\Telescope\Gemini\*.*
Type: dirifempty; Name: {cf}\ASCOM\Telescope\Gemini\GeminiTelescopeServedClasses
Type: dirifempty; Name: {cf}\ASCOM\Telescope\Gemini\

[Code]
//This function is called automatically before install starts and will test whether platform 5 or later and the platform update are installed
function InitializeSetup(): Boolean;
  var
	version: cardinal;

begin
  // Initialise return value
  Result:= True;

  // Test for ASCOM platform
  if not FileExists(ExpandConstant('{cf32}\ASCOM\Interface\AscomMasterInterfaces.tlb')) then begin
    MsgBox('ASCOM Platform 5 or later is not installed. You must install the latest ASCOM Platform before installing this update. You can download this from http:\\www.ascom.com\downloads', mbCriticalError, MB_OK);
    Result:= False;
  end;

  if Result then begin // Platform 5 is installed so now test if the 5.5 update or later is installed
    // Test for Platform 5.5 update
    if not FileExists(ExpandConstant('{cf}\ASCOM\.net\ASCOM.Utilities.dll')) then begin
      MsgBox('ASCOM Platform 5.5 Update is not installed. You must install the ASCOM Platform 5.5 updater before installing this driver. You can download this from http:\\www.ascom.com\downloads', mbCriticalError, MB_OK);
      Result:= False;
    end;
  end;

  if Result then // check for .NET 3.5 SP1
  begin
		RegQueryDWordValue(HKLM, 'Software\Microsoft\NET Framework Setup\NDP\v3.5', 'SP', version);
		if version < 1 then
		begin
     MsgBox('Microsoft .NET version 3.5 SP1 is not installed. You must install .NET 3.5 SP1 run-time before installing this driver.', mbCriticalError, MB_OK);
       Result:= False;
    end;
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
  ResultCode: Integer;
  Uninstall: String;
begin
  if (CurStep = ssInstall) then
	begin
      if RegQueryStringValue(HKLM, 'Software\Microsoft\Windows\CurrentVersion\Uninstall\ASCOM.GeminiTelescope_is1', 'UninstallString', Uninstall) then begin
      MsgBox('Setup will now remove the previous version.', mbInformation, MB_OK);
      Exec(RemoveQuotes(Uninstall), ' /SILENT', '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode);
    end;
  end;
end;

function NotIs64BitInstallMode() : Boolean;
begin
    Result := not Is64BitInstallMode;
end;


const
	dotnetfx35sp1_url = 'http://download.microsoft.com/download/0/6/1/061f001c-8752-4600-a198-53214c69b51f/dotnetfx35setup.exe';



[Messages]
WelcomeLabel1=%n%n[name]%n
#emit "WelcomeLabel2=This will update your installation to version: " + AppVer + ".%n%nIt is recommended that you close all other applications before continuing.%n%n"



[CustomMessages]
dotnetfx35sp1_title=.NET Framework 3.5 Service Pack 1


[_ISTool]
LogFile=Setup.log
LogFileAppend=false
