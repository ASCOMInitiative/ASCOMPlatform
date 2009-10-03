; Installer for Utilities

; ***** REQUIRES INNO 5.3.4 OR LATER *****

; 5.0.1.0 Removed register from the VB6 helper dlls
; 5.0.2.0 Added version 5 redirection policy install and XML intellisense file
; 5.1.3.0 Added profile Explorer
; 5.2.5.0 Split ASCOM.Astrometrics from ASCOM.HelperNET
; 5.3.0.0 Adapted for use in the ASCOM SVN repository where everything is built in debug mode
; Added ASCOM.Exceptions install
; Moved IConform from Utiities to own assembly
; Removed Introp Scripting
; Migrated to Inno 5.3.4
; Switched to using {dotnet} constants
; Added 32bit registration on 64bit systems
; Added registration of COM NOVAS and Kepler Interface DLLs
; Added unregister for 32bit registrations on 64bit systems
; Deleted unwanted commented out items
; Added ASCOM WizardImage file
; Fixed PlatformHelp name incorrect in Help link
; Added ASCOM.Interfaces 5.0.0.0 and ASCOM.Attributes for Gemini drivers
; Removed ASCOM Interfaces 5.0.0.0 as we will now use the version 1.000 installed by platform 5a
; Converted to use Release versions
; Setup Build 0 Released

; Added astrometry pia task i.e. removed 32bit astrometry pias from normal install
; Installed new exceptions version 1.1 rather than overwriting 1.0
; Installed client toolkit 1.0.5 but I won't uninstall it.
; Fixed issue with uninstalling Astrometry and Utilities policies
; Setup Build 1 Released

; Added extra debug information on running assemblies in trace logs
; Setup Build 2 Released

; Removed extra {} from a number of file copies
; Setup build 3 Released

; Added MigrateProfile and desktop icon option
; Changed setup name to ASCOM Platform Major.Minor Updater (vMajor.Minor.Release.Build)
; Setup build 4 Released

; Changed AppVer to access Release code rather than Debug code
; Added pre-requisite test for Platform 5 as that must be present for the update to work
; Changed Ad/Remove programs title to ASCOM Platform 5.5 Update (x.x.x.x)
; Made erase profile run minimised
; Fixed the application name to be ASCOM Platform 5.5 update
; Removed ISTool Setup log file reference
; Setup build 5 Released

; Removed NOVAS and Kepler PIA install task and moved to a separate installer as this is for testing only and not for release
; Make GACInstall a shared file
; Changed client toolkit files to the original versions from the 1.0.5a installer rather than from the new SVN build
; Added client toolkit symbols to the symbols directory
; Utilities uplevelled to 5.5.0.0
; Setup Build 6 Released
; Corrected ASCOM download url given when platform 5 is not installed
; Changed location of Platform 5a Architecture pdf to make it more easily found.
; Setup Build 7 Released

; Added extra delete files and delete directories commands to clean up tlbs and directories left over after uninstall
; Setup Build 8 Released

; Removed directory delete commands that caused an access issue on Tim's PC.
; Setup Build 9 Released

[Setup]
; Setup program version number - change this each time you change this setup script
#define Public SetupVersion 9

#define Public Major 0
#define Public Minor 0
#define Public Release 0
#define Public Build 0
#define AppVer ParseVersion("..\Utilities\bin\Release\ASCOM.Utilities.dll", Major ,Minor ,Release ,Build) ; define version variable
#define AppVer str(Major) + "." + str(Minor) + "." + str(Release) + "." + str(SetupVersion) ; redefine to include setup version

AppCopyright=Copyright � 2009 ASCOM Initiative
;AppID must not change to maintain a consistent uninstall experience although AppName can be changed.
;This value is hard coded in the uninstall code below. If you do change this you must change the corresponding reference in
;the [Code] CurStepChanged section
AppID=ASCOM.Platform.NET.Components
;#emit "AppName=ASCOM Platform " + str(Major) + "." + str(Minor) + " Update" ;
AppName=ASCOM Platform 5.5 Update
#emit "AppVerName=ASCOM Platform 5.5 Update (" + Appver + ")"
#emit "AppVersion=" + AppVer
AppPublisher=ASCOM Initiative
AppPublisherURL=http://ascom-standards.org/
AppSupportURL=http://tech.groups.yahoo.com/group/ASCOM-Talk/
AppUpdatesURL=http://ascom-standards.org/
MinVersion=0,5.0.2195sp4
DefaultDirName={cf}\ASCOM\.net
DefaultGroupName=ASCOM Platform\Docs
DisableDirPage=true
DisableProgramGroupPage=true
OutputDir=.
PrivilegesRequired=admin
#emit "OutputBaseFilename=ASCOM Platform " + str(Major) +"." + str(Minor) + " Updater (v" + AppVer +")"
Compression=lzma
SolidCompression=true
SetupIconFile=..\Utilities\Resources\ASCOM.ico
ShowLanguageDialog=auto
WizardImageFile=NewWizardImage.bmp
WizardSmallImageFile=ASCOMLogo.bmp
Uninstallable=true
DirExistsWarning=no
UninstallDisplayIcon={app}\ASCOM.ico
UninstallFilesDir={cf}\ASCOM\Uninstall\Utilities
UsePreviousTasks=false
VersionInfoCompany=ASCOM Initiative
VersionInfoCopyright=ASCOM Initiative
VersionInfoDescription=ASCOM Platform .NET Components 5.0
VersionInfoProductName=ASCOM Platform .NET Components
#emit "VersionInfoProductVersion=" + AppVer
#emit "VersionInfoVersion=" + AppVer
ArchitecturesInstallIn64BitMode=X64
SetupLogging=true
WindowVisible=false

[Languages]
Name: english; MessagesFile: compiler:Default.isl

[Dirs]
Name: {cf}\ASCOM\Uninstall\Utilities

;  Add an option to erase the HelperNET profile
[Tasks]
Name: cleanprofile; Description: Erase Utilities profile store (leaves registry profile intact); GroupDescription: Release Candidate Testing; Flags: unchecked
Name: desktopicons; Description: Install EraseProfile and MigrateProfile desktop icons; GroupDescription: Release Candidate Testing

[Files]
;Install the ASCOM.Utilities code
Source: ..\Utilities\bin\Release\ASCOM.Utilities.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\Utilities\bin\Release\ASCOM.Utilities.pdb; DestDir: {app}; Flags: ignoreversion
Source: ..\Utilities\bin\Release\ASCOM.Utilities.xml; DestDir: {app}; Flags: ignoreversion
;Debug symbols to the symbols directory
Source: ..\Utilities\bin\Release\ASCOM.Utilities.pdb; DestDir: {win}\Symbols\dll; Flags: ignoreversion
;Install to 32bit directory as well on 64bit systems so that 32bit apps will find Utilities in the place they expect on a 64bit system
Source: ..\Utilities\bin\Release\ASCOM.Utilities.dll; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: ..\Utilities\bin\Release\ASCOM.Utilities.pdb; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: ..\Utilities\bin\Release\ASCOM.Utilities.xml; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion

;Install the Astrometry code
Source: ..\..\ASCOM.Astrometry\Astrometry\bin\Release\ASCOM.Astrometry.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\..\ASCOM.Astrometry\Astrometry\bin\Release\ASCOM.Astrometry.pdb; DestDir: {app}; Flags: ignoreversion
Source: ..\..\ASCOM.Astrometry\Astrometry\bin\Release\ASCOM.Astrometry.xml; DestDir: {app}; Flags: ignoreversion
;Debug symbols to the symbols directory
Source: ..\..\ASCOM.Astrometry\Astrometry\bin\Release\ASCOM.Astrometry.pdb; DestDir: {win}\Symbols\dll; Flags: ignoreversion
;Install to 32bit directory as well on 64bit systems so that 32bit apps will find Utilities in the place they expect on a 64bit system
Source: ..\..\ASCOM.Astrometry\Astrometry\bin\Release\ASCOM.Astrometry.dll; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: ..\..\ASCOM.Astrometry\Astrometry\bin\Release\ASCOM.Astrometry.pdb; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: ..\..\ASCOM.Astrometry\Astrometry\bin\Release\ASCOM.Astrometry.xml; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion

;Install the IConform interface and classes
Source: ..\..\ASCOM.IConform\ASCOM.IConform\bin\Release\ASCOM.IConform.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\..\ASCOM.IConform\ASCOM.IConform\bin\Release\ASCOM.IConform.pdb; DestDir: {app}; Flags: ignoreversion
Source: ..\..\ASCOM.IConform\ASCOM.IConform\bin\Release\ASCOM.IConform.xml; DestDir: {app}; Flags: ignoreversion
;Debug symbols to the symbols directory
Source: ..\..\ASCOM.IConform\ASCOM.IConform\bin\Release\ASCOM.IConform.pdb; DestDir: {win}\Symbols\dll; Flags: ignoreversion
;Install to 32bit directory as well on 64bit systems so that 32bit apps will find Utilities in the place they expect on a 64bit system
Source: ..\..\ASCOM.IConform\ASCOM.IConform\bin\Release\ASCOM.IConform.dll; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: ..\..\ASCOM.IConform\ASCOM.IConform\bin\Release\ASCOM.IConform.pdb; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: ..\..\ASCOM.IConform\ASCOM.IConform\bin\Release\ASCOM.IConform.xml; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion

;ASCOM Attributes
Source: ..\..\ASCOM.Attributes\bin\Release\ASCOM.Attributes.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\..\ASCOM.Attributes\bin\Release\ASCOM.Attributes.pdb; DestDir: {app}; Flags: ignoreversion
;Debug symbols to the symbols directory
Source: ..\..\ASCOM.Attributes\bin\Release\ASCOM.Attributes.pdb; DestDir: {win}\Symbols\dll; Flags: ignoreversion
;Install to 32bit directory as well on 64bit systems so that 32bit apps will find Utilities in the place they expect on a 64bit system
Source: ..\..\ASCOM.Attributes\bin\Release\ASCOM.Attributes.dll; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: ..\..\ASCOM.Attributes\bin\Release\ASCOM.Attributes.pdb; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion

;VB6 Helpers
Source: ..\VB6Helper\Helper.dll; DestDir: {cf32}\ASCOM; Flags: ignoreversion uninsneveruninstall 32bit
Source: ..\VB6Helper2\Helper2.dll; DestDir: {cf32}\ASCOM; Flags: ignoreversion uninsneveruninstall 32bit

;Copy the policy files
Source: PublisherPolicy.xml; DestDir: {app}; Flags: ignoreversion
#emit "Source: ""policy." + str(Major) + "." + str(Minor) + ".ASCOM.Utilities.dll""; DestDir: ""{app}""; Flags: ignoreversion"
#emit "Source: ""policy." + str(Major) + "." + str(Minor) + ".ASCOM.Astrometry.dll""; DestDir: ""{app}""; Flags: ignoreversion"

;Make sure we have backout copies of the original helpers
Source: ..\OriginalHelpers\Helper.dll; DestDir: {cf32}\ASCOM\Utilities; Flags: ignoreversion
Source: ..\OriginalHelpers\Helper2.dll; DestDir: {cf32}\ASCOM\Utilities; Flags: ignoreversion
Source: ..\OriginalHelpers\RestoreOriginalHelpers.cmd; DestDir: {cf32}\ASCOM\Utilities; Flags: ignoreversion

;ASCOM Platform .NET Help files
Source: ..\Help\Help\PlatformHelp.chm; DestDir: {cf}\ASCOM\Doc; Flags: ignoreversion
Source: ..\Help\Platform 5.5 Architecture\Platform 5.5.pdf; DestDir: {cf}\ASCOM\Doc; Flags: ignoreversion
Source: ReadMe55.txt; DestDir: {app}; Flags: ignoreversion

;Profile Explorer
Source: ..\Profile Explorer\bin\Release\ProfileExplorer.exe; DestDir: {pf}\ASCOM\Profile Explorer; Flags: ignoreversion
Source: ..\Profile Explorer\bin\Release\ProfileExplorer.pdb; DestDir: {pf}\ASCOM\Profile Explorer; Flags: ignoreversion

;Tools to erase and migrate profile
Source: ..\EraseProfile\bin\Release\EraseProfile.exe; DestDir: {cf32}\ASCOM\Utilities; Flags: ignoreversion
Source: ..\EraseProfile\bin\Release\EraseProfile.pdb; DestDir: {cf32}\ASCOM\Utilities; Flags: ignoreversion
Source: ..\MigrateProfile\bin\Release\MigrateProfile.exe; DestDir: {cf32}\ASCOM\Utilities; Flags: ignoreversion
Source: ..\MigrateProfile\bin\Release\MigrateProfile.pdb; DestDir: {cf32}\ASCOM\Utilities; Flags: ignoreversion

;Tool to install into GAC
Source: ..\..\GACInstall\bin\Release\GACInstall.exe; DestDir: {app}; Flags: ignoreversion sharedfile

;ASCOM Icon
Source: ..\Utilities\Resources\ASCOM.ico; DestDir: {app}; Flags: ignoreversion

;ASCOM Exceptions - Removed Interfaces as we will use the platform 5a version already installed
Source: ..\..\Interfaces\ASCOMExceptions\bin\Release\ASCOM.Exceptions.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\..\Interfaces\ASCOMExceptions\bin\Release\ASCOM.Exceptions.XML; DestDir: {app}; Flags: ignoreversion
;...and for 32bit directories on a 64bit system
Source: ..\..\Interfaces\ASCOMExceptions\bin\Release\ASCOM.Exceptions.dll; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: ..\..\Interfaces\ASCOMExceptions\bin\Release\ASCOM.Exceptions.XML; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion

;Client toolbox 1.0.5, in case it isn't already installed
Source: Client Toolkit 1.0.5 Executables\ASCOM.DriverAccess.dll; DestDir: {app}; Flags: ignoreversion
Source: Client Toolkit 1.0.5 Executables\ASCOM.DriverAccess.pdb; DestDir: {app}; Flags: ignoreversion
Source: Client Toolkit 1.0.5 Executables\ASCOM.DriverAccess.XML; DestDir: {app}; Flags: ignoreversion
;Debug symbols to the symbols directory
Source: Client Toolkit 1.0.5 Executables\ASCOM.DriverAccess.pdb; DestDir: {win}\Symbols\dll; Flags: ignoreversion
;32bit directories
Source: Client Toolkit 1.0.5 Executables\ASCOM.DriverAccess.dll; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: Client Toolkit 1.0.5 Executables\ASCOM.DriverAccess.pdb; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: Client Toolkit 1.0.5 Executables\ASCOM.DriverAccess.XML; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion

;Policy file to redirect to 1.0.5
Source: ..\..\ClientToolbox\SimpsonBitsPolicyStuff\PolicyInstaller\policy.1.0.ASCOM.DriverAccess.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\..\ClientToolbox\SimpsonBitsPolicyStuff\PolicyInstaller\driveraccess.config; DestDir: {app}; Flags: ignoreversion

;Debug symbols directory
Source: ..\..\ClientToolbox\bin\Release\ASCOM.DriverAccess.pdb; DestDir: {win}\Symbols\dll; Flags: ignoreversion

;NOVAS C DLLs
Source: ..\NOVAS-C x86-x64\Release\NOVAS-C.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\NOVAS-C x86-x64\Release\NOVAS-C.pdb; DestDir: {app}; Flags: ignoreversion
Source: ..\NOVAS-C x86-x64\x64\Release\NOVAS-C64.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\NOVAS-C x86-x64\x64\Release\NOVAS-C64.pdb; DestDir: {app}; Flags: ignoreversion
;Install to 32bit directory as well on 64bit systems so that 32bit apps will find NOVAS DLLs in the place they expect on a 64bit system
Source: ..\NOVAS-C x86-x64\Release\NOVAS-C.dll; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: ..\NOVAS-C x86-x64\Release\NOVAS-C.pdb; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: ..\NOVAS-C x86-x64\x64\Release\NOVAS-C64.dll; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: ..\NOVAS-C x86-x64\x64\Release\NOVAS-C64.pdb; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion

[Registry]
Root: HKLM64; Subkey: SOFTWARE\Microsoft\.NETFramework\v2.0.50727\AssemblyFoldersEx\ASCOM64; ValueType: string; ValueName: ; ValueData: {cf}\ASCOM\.net; Flags: uninsdeletekey; Check: IsWin64
Root: HKLM32; Subkey: SOFTWARE\Microsoft\.NETFramework\v2.0.50727\AssemblyFoldersEx\ASCOM64; ValueType: string; ValueName: ; ValueData: {cf64}\ASCOM\.net; Flags: uninsdeletekey; Check: IsWin64

[Icons]
Name: {commonprograms}\ASCOM Platform\Docs\ASCOM Platform Update 5.5; Filename: {cf}\ASCOM\Doc\PlatformHelp.chm
Name: {commonprograms}\ASCOM Platform\Docs\ASCOM Platform Architecture; Filename: {cf}\ASCOM\Doc\Platform 5.5.pdf
Name: {commonprograms}\ASCOM Platform\Tools\Profile Explorer; Filename: {pf}\ASCOM\Profile Explorer\ProfileExplorer.exe
Name: {commondesktop}\Migrate Profile; Filename: {cf32}\ASCOM\Utilities\MigrateProfile.exe; Tasks: desktopicons
Name: {commondesktop}\Erase Profile; Filename: {cf32}\ASCOM\Utilities\EraseProfile.exe; Tasks: desktopicons

[Run]
; Install Utilties, Astrometry and IConform to the GAC and register COM types for 32and 64bit COM
Filename: {app}\GACInstall.exe; Parameters: ASCOM.Utilities.dll; Flags: runhidden; StatusMsg: Installing Utilities to the assembly cache
Filename: {app}\GACInstall.exe; Parameters: ASCOM.Astrometry.dll; Flags: runhidden; StatusMsg: Installing ASCOM.Astrometry to the assembly cache
Filename: {app}\GACInstall.exe; Parameters: ASCOM.IConform.dll; Flags: runhidden; StatusMsg: Installing ASCOM.IConform to the assembly cache
Filename: {app}\GACInstall.exe; Parameters: ASCOM.Attributes.dll; Flags: runhidden; StatusMsg: Installing ASCOM.Attributes to the assembly cache
Filename: {dotnet20}\regasm.exe; Parameters: "/TLB ""{app}\ASCOM.Utilities.dll"""; Flags: runhidden; StatusMsg: Registering Utilities for COM
Filename: {dotnet20}\regasm.exe; Parameters: "/TLB ""{app}\ASCOM.Astrometry.dll"""; Flags: runhidden; StatusMsg: Registering ASCOM.Astrometry for COM
Filename: {dotnet20}\regasm.exe; Parameters: "/TLB ""{app}\ASCOM.IConform.dll"""; Flags: runhidden; StatusMsg: Registering ASCOM.IConform for COM
Filename: {dotnet2032}\regasm.exe; Parameters: "/TLB ""{cf32}\ASCOM\.net\ASCOM.Utilities.dll"""; Flags: runhidden; Check: IsWin64; StatusMsg: Registering Utilities for 32bit COM
Filename: {dotnet2032}\regasm.exe; Parameters: "/TLB ""{cf32}\ASCOM\.net\ASCOM.Astrometry.dll"""; Flags: runhidden; Check: IsWin64; StatusMsg: Registering ASCOM.Astrometry for 32bit COM
Filename: {dotnet2032}\regasm.exe; Parameters: "/TLB ""{cf32}\ASCOM\.net\ASCOM.IConform.dll"""; Flags: runhidden; Check: IsWin64; StatusMsg: Registering ASCOM.IConform for 32bit COM

;Erase and migrate the profile if needed
Filename: {cf32}\ASCOM\Utilities\EraseProfile.exe; Tasks: cleanprofile; Flags: runminimized; statusMsg: Erasing Profile
Filename: {cf32}\ASCOM\Utilities\MigrateProfile.exe; Parameters: /MIGRATEIFNEEDED; Flags: runminimized; statusMsg: Migrating Profile if necessary

;ASCOM Exceptions
Filename: {app}\GACInstall.exe; Parameters: ASCOM.Exceptions.dll; Flags: runhidden; StatusMsg: Instlling ASCOM.Exceptions to the assembly cache

; ASCOM Client Toolkit 1.0.5
Filename: {app}\GACInstall.exe; Parameters: ASCOM.DriverAccess.dll; Flags: runhidden; StatusMsg: Installing Client Access Toolkit to the assembly cache
Filename: {app}\GACInstall.exe; Parameters: """{app}\policy.1.0.ASCOM.DriverAccess.dll"""; Flags: runhidden; StatusMsg: Installing Client Access Toolkit Policy to the assembly cache

;Publisher policies for Astrometry and Utilities
#emit "Filename: {app}\GACInstall.exe; Parameters: policy." + str(Major) + "." + str(Minor) + ".ASCOM.Utilities.dll; Flags: runhidden; StatusMsg: Installing ASCOM Utilities policy to the assembly cache"
#emit "Filename: {app}\GACInstall.exe; Parameters: policy." + str(Major) + "." + str(Minor) + ".ASCOM.Astrometry.dll; Flags: runhidden; StatusMsg: Installing ASCOM Astrometry policy to the assembly cache"
Filename: {app}\ReadMe55.txt; Description: ReadMe file; StatusMsg: Displaying ReadMe file; Flags: shellexec skipifdoesntexist postinstall skipifsilent

[UninstallRun]
; uninstall Utilties, Astrometry and IConform from the GAC and unregister COM types for 32and 64bit COM
Filename: {app}\GACInstall.exe; Parameters: "/U ""ASCOM.Astrometry"""; Flags: runhidden; StatusMsg: Uninstalling ASCOM.Astrometry from the assembly cache
Filename: {app}\GACInstall.exe; Parameters: "/U ""ASCOM.Utilities"""; Flags: runhidden; StatusMsg: Uninstalling Utilities from the assembly cache
Filename: {app}\GACInstall.exe; Parameters: "/U ""ASCOM.IConform"""; Flags: runhidden; StatusMsg: Uninstalling ASCOM.IConform from the assembly cache
Filename: {app}\GACInstall.exe; Parameters: "/U ""ASCOM.Attributes"""; Flags: runhidden; StatusMsg: Uninstalling ASCOM.Attributes from the assembly cache
Filename: {dotnet20}\regasm.exe; Parameters: "/Unregister /TLB ""{app}\ASCOM.Astrometry.dll"""; Flags: runhidden; StatusMsg: Unregistering ASCOM.Astrometry for COM
Filename: {dotnet20}\regasm.exe; Parameters: "/Unregister /TLB ""{app}\ASCOM.Utilities.dll"""; Flags: runhidden; StatusMsg: Unregistering ASCOM.Utilities for COM
Filename: {dotnet20}\regasm.exe; Parameters: "/Unregister /TLB ""{app}\ASCOM.IConform.dll"""; Flags: runhidden; StatusMsg: Unregistering ASCOM.IConform for COM
Filename: {dotnet2032}\regasm.exe; Parameters: "/Unregister /TLB ""{cf32}\ASCOM\.net\ASCOM.IConform.dll"""; Flags: runhidden; Check: IsWin64; StatusMsg: Registering ASCOM.IConform for COM
Filename: {dotnet2032}\regasm.exe; Parameters: "/Unregister /TLB ""{cf32}\ASCOM\.net\ASCOM.Astrometry.dll"""; Flags: runhidden; Check: IsWin64; StatusMsg: Registering ASCOM.Astrometry for COM
Filename: {dotnet2032}\regasm.exe; Parameters: "/Unregister /TLB ""{cf32}\ASCOM\.net\ASCOM.Utilities.dll"""; Flags: runhidden; Check: IsWin64; StatusMsg: Registering Utilities for COM

Filename: {cf32}\ASCOM\Utilities\RestoreOriginalHelpers.cmd; Parameters: """{cf32}\ASCOM\Utilities\*.dll"" ""{cf32}\ASCOM"""; StatusMsg: Restoring helper dlls; Flags: runhidden

;ASCOM Exceptions
Filename: {app}\GACInstall.exe; Parameters: "/U ""ASCOM.Exceptions"""; Flags: runhidden; StatusMsg: Uninstalling ASCOM.Exceptions from the assembly cache

;ASCOM Client Toolkit
;I'm deliberately leaving this installed as it is a prerequisite rather than part of this update

;Publisher policy
#emit "Filename: {app}\GACInstall.exe; Parameters: ""/U """"policy." + str(Major) + "." + str(Minor) + ".ASCOM.Utilities""""""; Flags: runhidden; StatusMsg: Uninstalling ASCOM Utilities policy from the assembly cache"
#emit "Filename: {app}\GACInstall.exe; Parameters: ""/U """"policy." + str(Major) + "." + str(Minor) + ".ASCOM.Astrometry""""""; Flags: runhidden; StatusMsg: Uninstalling ASCOM Astrometry policy from the assembly cache"

[UninstallDelete]
Type: files; Name: {cf32}\ASCOM\Utilities\*.*
Type: dirifempty; Name: {cf32}\ASCOM\Utilities
Type: files; Name: {cf32}\ASCOM\.net\ASCOM.Utilities.tlb
Type: files; Name: {cf32}\ASCOM\.net\ASCOM.Astrometry.tlb
Type: files; Name: {cf32}\ASCOM\.net\ASCOM.IConform.tlb
Type: dirifempty; Name: {cf32}\ASCOM\Uninstall\Utilities

Type: files; Name: {cf}\ASCOM\Utilities\*.*
Type: dirifempty; Name: {cf}\ASCOM\Utilities
Type: files; Name: {app}\ASCOM.Utilities.tlb
Type: files; Name: {app}\ASCOM.Astrometry.tlb
Type: files; Name: {app}\ASCOM.IConform.tlb
Type: dirifempty; Name: {cf}\ASCOM\Uninstall\Utilities
;Type: dirifempty; Name: {cf}\ASCOM\Uninstall
Type: dirifempty; Name: {app}
;Type: dirifempty; Name: {cf}\ASCOM

[Code]
//This funciton is called automatically before install starts and will test whether platform 5 is installed
function InitializeSetup(): Boolean;
begin
  // Initialise return value
  Result:= True;

  // Test for platform 5
  if not FileExists(ExpandConstant('{cf32}\ASCOM\Interface\AscomMasterInterfaces.tlb')) then begin
    MsgBox('ASCOM Platform 5 is not installed. You must install ASCOM Platform 5a before installing this update. You can download this from http://www.ascom-standards.org', mbCriticalError, MB_OK);
    Result:= False;
  end;

end;

procedure CurStepChanged(CurStep: TSetupStep);
var
  ResultCode: Integer;
  Uninstall: String;
begin
  if (CurStep = ssInstall) then
	begin
      if RegQueryStringValue(HKLM, 'Software\Microsoft\Windows\CurrentVersion\Uninstall\ASCOM.Platform.NET.Components_is1', 'UninstallString', Uninstall) then begin
      MsgBox('Setup will now remove the previous version.', mbInformation, MB_OK);
      Exec(RemoveQuotes(Uninstall), ' /SILENT', '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode);
    end;
  end;
end;

[Messages]
WelcomeLabel1=%n%n[name]%n
#emit "WelcomeLabel2=This will update your computer to version: " + AppVer + ".%n%nIt is recommended that you close all other applications before continuing.%n%n"
[_ISToolPreCompile]
Name: ..\..\ASCOM Redirection Policies\ASCOM Redirection Policies\bin\Release\ASCOM Redirection Policies.exe; Parameters: ; Flags: runminimized abortonerror
