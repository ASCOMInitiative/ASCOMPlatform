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

[Setup]
#define Public Major 0
#define Public Minor 0
#define Public Release 0
#define Public Build 0
#define AppVer ParseVersion("..\Utilities\bin\Debug\ASCOM.Utilities.dll", Major ,Minor ,Release ,Build) ; define version variable
AppCopyright=Copyright © 2009 ASCOM Initiative
;AppID must not change to maintain a consistent uninstall experience although AppName can be changed.
;This value is hard coded in the uninstall code below. If you do change this you must change the corresponding reference in
;the [Code] CurStepChanged section
AppID=ASCOM.Platform.NET.Components
#emit "AppName=ASCOM Platform " + str(Major) + "." + str(Minor) + " Update" ;
#emit "AppVerName=ASCOM Platform 5 Update " + Appver
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
#emit "OutputBaseFilename=Utilities(" + AppVer +")setup"
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
Name: cleanprofile; Description: Erase Utilities profile store (leaves registry profile intact); GroupDescription: """Installation Tasks"""; Flags: unchecked

[Files]
;Install the ASCOM.Utilities code
Source: ..\Utilities\bin\Debug\ASCOM.Utilities.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\Utilities\bin\Debug\ASCOM.Utilities.pdb; DestDir: {app}; Flags: ignoreversion
Source: ..\Utilities\bin\Debug\ASCOM.Utilities.xml; DestDir: {app}; Flags: ignoreversion
;Debug symbols to the symbols directory
Source: ..\Utilities\bin\Debug\ASCOM.Utilities.pdb; DestDir: {win}\Symbols\dll; Flags: ignoreversion
;Install to 32bit directory as well on 64bit systems so that 32bit apps will find Utilities in the place they expect on a 64bit system
Source: ..\Utilities\bin\Debug\ASCOM.Utilities.dll; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: ..\Utilities\bin\Debug\ASCOM.Utilities.pdb; DestDir: {{cf32}\ASCOM\.net}; Flags: ignoreversion
Source: ..\Utilities\bin\Debug\ASCOM.Utilities.xml; DestDir: {{cf32}\ASCOM\.net}; Flags: ignoreversion

;Install the Astrometry code
Source: ..\..\ASCOM.Astrometry\Astrometry\bin\Debug\ASCOM.Astrometry.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\..\ASCOM.Astrometry\Astrometry\bin\Debug\ASCOM.Astrometry.pdb; DestDir: {app}; Flags: ignoreversion
Source: ..\..\ASCOM.Astrometry\Astrometry\bin\Debug\ASCOM.Astrometry.xml; DestDir: {app}; Flags: ignoreversion
;Debug symbols to the symbols directory
Source: ..\..\ASCOM.Astrometry\Astrometry\bin\Debug\ASCOM.Astrometry.pdb; DestDir: {win}\Symbols\dll; Flags: ignoreversion
;Install to 32bit directory as well on 64bit systems so that 32bit apps will find Utilities in the place they expect on a 64bit system
Source: ..\..\ASCOM.Astrometry\Astrometry\bin\Debug\ASCOM.Astrometry.dll; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: ..\..\ASCOM.Astrometry\Astrometry\bin\Debug\ASCOM.Astrometry.pdb; DestDir: {{cf32}\ASCOM\.net}; Flags: ignoreversion
Source: ..\..\ASCOM.Astrometry\Astrometry\bin\Debug\ASCOM.Astrometry.xml; DestDir: {{cf32}\ASCOM\.net}; Flags: ignoreversion

;Install the IConform interface and classes
Source: ..\..\ASCOM.IConform\ASCOM.IConform\bin\Debug\ASCOM.IConform.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\..\ASCOM.IConform\ASCOM.IConform\bin\Debug\ASCOM.IConform.pdb; DestDir: {app}; Flags: ignoreversion
Source: ..\..\ASCOM.IConform\ASCOM.IConform\bin\Debug\ASCOM.IConform.xml; DestDir: {app}; Flags: ignoreversion
;Debug symbols to the symbols directory
Source: ..\..\ASCOM.IConform\ASCOM.IConform\bin\Debug\ASCOM.IConform.pdb; DestDir: {win}\Symbols\dll; Flags: ignoreversion
;Install to 32bit directory as well on 64bit systems so that 32bit apps will find Utilities in the place they expect on a 64bit system
Source: ..\..\ASCOM.IConform\ASCOM.IConform\bin\Debug\ASCOM.IConform.dll; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: ..\..\ASCOM.IConform\ASCOM.IConform\bin\Debug\ASCOM.IConform.pdb; DestDir: {{cf32}\ASCOM\.net}; Flags: ignoreversion
Source: ..\..\ASCOM.IConform\ASCOM.IConform\bin\Debug\ASCOM.IConform.xml; DestDir: {{cf32}\ASCOM\.net}; Flags: ignoreversion

;ASCOM Attributes
Source: ..\..\ASCOM.Attributes\bin\Debug\ASCOM.Attributes.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\..\ASCOM.Attributes\bin\Debug\ASCOM.Attributes.pdb; DestDir: {app}; Flags: ignoreversion
;Debug symbols to the symbols directory
Source: ..\..\ASCOM.Attributes\bin\Debug\ASCOM.Attributes.pdb; DestDir: {win}\Symbols\dll; Flags: ignoreversion
;Install to 32bit directory as well on 64bit systems so that 32bit apps will find Utilities in the place they expect on a 64bit system
Source: ..\..\ASCOM.Attributes\bin\Debug\ASCOM.Attributes.dll; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: ..\..\ASCOM.Attributes\bin\Debug\ASCOM.Attributes.pdb; DestDir: {{cf32}\ASCOM\.net}; Flags: ignoreversion

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
Source: ..\Help\Help\HelperNET.pdf; DestDir: {cf}\ASCOM\Doc; Flags: ignoreversion
Source: ReadMe55.txt; DestDir: {app}; Flags: ignoreversion

;Profile Explorer
Source: ..\Profile Explorer\bin\Debug\ProfileExplorer.exe; DestDir: {pf}\ASCOM\Profile Explorer; Flags: ignoreversion
Source: ..\Profile Explorer\bin\Debug\ProfileExplorer.pdb; DestDir: {pf}\ASCOM\Profile Explorer; Flags: ignoreversion

;Tool to erase profile
Source: ..\EraseProfile\bin\Debug\EraseProfile.exe; DestDir: {cf32}\ASCOM\Utilities; Flags: ignoreversion

;Tool to install into GAC
Source: ..\..\GACInstall\bin\Debug\GACInstall.exe; DestDir: {app}; Flags: ignoreversion

;ASCOM Icon
Source: ..\Utilities\Resources\ASCOM.ico; DestDir: {app}; Flags: ignoreversion

;NOVAS and Kepler PIAs and TLBs - Removed these as we now have .NET versions of Transform / NOVAS2 Kepler etc.
;Source: ..\..\Interfaces\NOVAS PIAs\ASCOM.NOVAS.DLL; DestDir: {app}; Flags: ignoreversion
;Source: ..\..\Interfaces\Kepler PIAs\ASCOM.Kepler.DLL; DestDir: {app}; Flags: ignoreversion
;Source: ..\..\Interfaces\NOVAS PIAs\NOVAS.tlb; DestDir: {app}; Flags: ignoreversion regtypelib 32bit
;Source: ..\..\Interfaces\Kepler PIAs\Kepler.tlb; DestDir: {app}; Flags: ignoreversion regtypelib 32bit

;ASCOM Interfaces and Exceptions - Removed Interfaces as we will use the platform 5a version already installed
;Source: ..\..\Interfaces\Master Interfaces\ASCOM.Interfaces.dll; DestDir: {app}; Flags: ignoreversion
;Source: ..\..\Interfaces\Master Interfaces\AscomMasterInterfaces.tlb; DestDir: {app}; Flags: ignoreversion regtypelib
Source: ..\..\Interfaces\ASCOMExceptions\bin\Debug\ASCOM.Exceptions.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\..\Interfaces\ASCOMExceptions\bin\Debug\ASCOM.Exceptions.XML; DestDir: {app}; Flags: ignoreversion
;...and for 32bit directories on a 64bit system
Source: ..\..\Interfaces\ASCOMExceptions\bin\Debug\ASCOM.Exceptions.dll; DestDir: {{cf32}\ASCOM\.net}; Flags: ignoreversion
Source: ..\..\Interfaces\ASCOMExceptions\bin\Debug\ASCOM.Exceptions.XML; DestDir: {{cf32}\ASCOM\.net}; Flags: ignoreversion

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
Name: {commonprograms}\ASCOM Platform\Docs\ASCOM Platform Architecture; Filename: {cf}\ASCOM\Doc\HelperNET.pdf
Name: {commonprograms}\ASCOM Platform\Tools\Profile Explorer; Filename: {pf}\ASCOM\Profile Explorer\ProfileExplorer.exe

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

Filename: {cf32}\ASCOM\Utilities\EraseProfile.exe; Tasks: cleanprofile

;NOVAS and Kepler 32 bit interface components
;Filename: {app}\GACInstall.exe; Parameters: ASCOM.NOVAS.dll; Flags: runhidden; StatusMsg: Installing NOVAS 2 to the assembly cache
;Filename: {app}\GACInstall.exe; Parameters: ASCOM.Kepler.dll; Flags: runhidden; StatusMsg: Installing Kepler to the assembly cache
;Filename: {dotnet2032}\regasm.exe; Parameters: """{cf32}\ASCOM\.net\ASCOM.NOVAS.dll"""; Flags: runhidden; StatusMsg: Registering NOVAS for COM
;Filename: {dotnet2032}\regasm.exe; Parameters: """{cf32}\ASCOM\.net\ASCOM.Kepler.dll"""; Flags: runhidden; StatusMsg: Registering Kepler for COM

;ASCOM Interfaces and Exceptions
; Filename: {app}\GACInstall.exe; Parameters: ASCOM.Interfaces.dll; Flags: runhidden; StatusMsg: Installing ASCOM.Interfaces to the assembly cache
Filename: {app}\GACInstall.exe; Parameters: ASCOM.Exceptions.dll; Flags: runhidden; StatusMsg: Installing ASCOM.Exceptions to the assembly cache

;Publisher policy
#emit "Filename: {app}\GACInstall.exe; Parameters: policy." + str(Major) + "." + str(Minor) + ".ASCOM.Utilities.dll; Flags: runhidden; StatusMsg: Installing ASCOM Utilities policy to the assembly cache"
#emit "Filename: {app}\GACInstall.exe; Parameters: policy." + str(Major) + "." + str(Minor) + ".ASCOM.Astrometry.dll; Flags: runhidden; StatusMsg: Installing ASCOM Astrometry policy to the assembly cache"
Filename: {app}\ReadMe55.txt; Description: ReadMe file; StatusMsg: Displaying ReadMe file; Flags: shellexec skipifdoesntexist postinstall skipifsilent unchecked

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

;NOVAS and Kepler 32 bit interfaces
;Filename: {app}\GACInstall.exe; Parameters: "/U ""ASCOM.NOVAS"""; Flags: runhidden; StatusMsg: Uninstalling NOVAS 2 from the assembly cache
;Filename: {app}\GACInstall.exe; Parameters: "/U ""ASCOM.Kepler"""; Flags: runhidden; StatusMsg: Uninstalling Kepler from the assembly cache
;Filename: {dotnet2032}\regasm.exe; Parameters: "/Unregister /TLB ""{cf32}\ASCOM\.net\ASCOM.NOVAS.dll"""; Flags: runhidden; StatusMsg: Unregistering NOVAS for COM
;Filename: {dotnet2032}\regasm.exe; Parameters: "/Unregister /TLB ""{cf32}\ASCOM\.net\ASCOM.Kepler.dll"""; Flags: runhidden; StatusMsg: Unregistering Kepler for COM

;ASCOM Interfaces and Exceptions
;Filename: {app}\GACInstall.exe; Parameters: "/U ""ASCOM.Interfaces"""; Flags: runhidden; StatusMsg: Uninstalling ASCOM.Interfaces from the assembly cache
Filename: {app}\GACInstall.exe; Parameters: "/U ""ASCOM.Exceptions"""; Flags: runhidden; StatusMsg: Uninstalling ASCOM.Exceptions from the assembly cache

;Publisher policy
#emit "Filename: {app}\GACInstall.exe; Parameters: ""/U """"policy." + str(Major) + "." + str(Minor) + ".ASCOM.Utilities.dll""""""; Flags: runhidden; StatusMsg: Uninstalling ASCOM Utilities policy from the assembly cache"
#emit "Filename: {app}\GACInstall.exe; Parameters: ""/U """"policy." + str(Major) + "." + str(Minor) + ".ASCOM.Astrometry.dll""""""; Flags: runhidden; StatusMsg: Uninstalling ASCOM Astrometry policy from the assembly cache"

[UninstallDelete]
Type: files; Name: {cf32}\ASCOM\Utilities\*.*
Type: dirifempty; Name: {cf32}\ASCOM\Utilities
Type: files; Name: {app}\ASCOM.Utilities.tlb
Type: dirifempty; Name: {app}

[Code]
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
Name: ASCOM Redirection Policies.exe; Parameters: ; Flags: runminimized abortonerror
[_ISTool]
LogFile=C:\Documents and Settings\Peter\My Documents\ASCOM Source\ASCOM.Utilities\Setup\Setup
LogFileAppend=false
