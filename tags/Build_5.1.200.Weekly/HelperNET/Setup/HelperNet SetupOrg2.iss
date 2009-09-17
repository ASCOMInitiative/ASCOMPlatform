; Installer for HelperNet
;
[Setup]
#define AppVer GetFileVersion("..\HelperNET\bin\Release\ASCOM.HelperNET.dll") ; define version variable
AppCopyright=Copyright © 2009 ASCOM Initiative
AppName="ASCOM Platform .NET Components"
; #emit "AppVerName=ASCOM HelperNET " + AppVer
AppVerName="ASCOM Platform .NET Components 5.0"
#emit "AppVersion=" + AppVer
AppPublisher=ASCOM Initiative
AppPublisherURL=http://ascom-standards.org/
AppSupportURL=http://tech.groups.yahoo.com/group/ASCOM-Talk/
AppUpdatesURL=http://ascom-standards.org/
MinVersion=0,5.0.2195sp4
DefaultDirName="{cf}\ASCOM\.net"
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir="."
PrivilegesRequired=admin
#emit "OutputBaseFilename=HelperNET(" + AppVer +")setup"
Compression=lzma
SolidCompression=yes
SetupIconFile=..\HelperNET\Resources\ASCOM.ico
ShowLanguageDialog=auto
; Put there by Platform if Driver Installer Support selected
WizardImageFile="C:\Program Files\ASCOM\InstallGen\Resources\WizardImage.bmp"
Uninstallable=yes
DirExistsWarning=no
UninstallDisplayIcon={cf32}\ASCOM\HelperNET\EraseProfile.exe
UninstallFilesDir="{cf}\ASCOM\Uninstall\HelperNET"
UsePreviousTasks=no
VersionInfoCompany=ASCOM Initiative
VersionInfoCopyright=ASCOM Initiative
VersionInfoDescription=ASCOM Platform .NET Components 5.0
VersionInfoProductName=ASCOM Platform .NET Components
#emit "VersionInfoProductVersion=" + AppVer
#emit "VersionInfoVersion=" + AppVer
ArchitecturesInstallIn64BitMode=X64

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{cf}\ASCOM\Uninstall\HelperNET"

;  Add an option to erase the HelperNET profile
[Tasks]
Name: cleanprofile; Description: "Erase HelperNET profile store (leaves registry profile intact)"; GroupDescription: "Installation Tasks"   ; Flags: unchecked

[Files]
;Install the new code
Source: "..\HelperNET\bin\Release\ASCOM.HelperNET.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\HelperNET\bin\Release\Interop.Scripting.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\VB6Helper\Helper.dll"; DestDir: "{cf32}\ASCOM"; Flags: ignoreversion uninsneveruninstall regserver 32bit
Source: "..\VB6Helper2\Helper2.dll"; DestDir: "{cf32}\ASCOM"; Flags: ignoreversion uninsneveruninstall regserver 32bit

;Make sure we have backout copies of the original helpers
Source: "..\OriginalHelpers\Helper.dll"; DestDir: "{cf32}\ASCOM\HelperNET"; Flags: ignoreversion
Source: "..\OriginalHelpers\Helper2.dll"; DestDir: "{cf32}\ASCOM\HelperNET"; Flags: ignoreversion
Source: "..\OriginalHelpers\RestoreOriginalHelpers.cmd"; DestDir: "{cf32}\ASCOM\HelperNET"; Flags: ignoreversion

;Tool to erase profile
Source: "..\EraseProfile\bin\Release\EraseProfile.exe"; DestDir: "{cf32}\ASCOM\HelperNET"; Flags: ignoreversion

;Tool to install into GAC
Source: "..\..\GACInstall\bin\Release\GACInstall.exe"; DestDir: {app}; Flags: ignoreversion

[Registry]
Root: HKLM64; Subkey: "SOFTWARE\Microsoft\.NETFramework\v2.0.50727\AssemblyFoldersEx\ASCOM64"; ValueType: string;  ValueName: ""; ValueData: "{cf}\ASCOM\.net";Flags: uninsdeletekey; Check: IsWin64

[Run]
; Install to the GAC and register COM types
Filename: "{app}\GACInstall.exe"; Parameters: ASCOM.HelperNET.dll; Flags: runhidden; StatusMsg: Installing HelperNET to the assembly cache
Filename: "{app}\GACInstall.exe"; Parameters: Interop.Scripting.dll; Flags: runhidden; StatusMsg: Installing Interop.Scripting to the assembly cache
Filename: "{win}\Microsoft.NET\Framework\v2.0.50727\regasm.exe"; Parameters: "/TLB ""{app}\ASCOM.HelperNET.dll"""; Flags: runhidden; StatusMsg: Registering HelperNET for COM
Filename: "{cf32}\ASCOM\HelperNET\EraseProfile.exe"; Tasks: cleanprofile

[UninstallRun]
Filename: "{app}\GACInstall.exe"; Parameters: "/U ""ASCOM.HelperNET"""; Flags: runhidden; StatusMsg: Uninstalling HelperNET from the assembly cache
Filename: "{app}\GACInstall.exe"; Parameters: "/U ""Interop.Scripting"""; Flags: runhidden; StatusMsg: Uninstalling Interop.Scripting from the assembly cache
Filename: "{win}\Microsoft.NET\Framework\v2.0.50727\regasm.exe"; Parameters: "/Unregister /TLB ""{app}\ASCOM.HelperNET.dll"""; Flags: runhidden; StatusMsg: Unregistering HelperNET for COM
Filename: "{cf32}\ASCOM\HelperNET\RestoreOriginalHelpers.cmd"; Parameters: """{cf32}\ASCOM\HelperNET\*.dll"" ""{cf32}\ASCOM"""; StatusMsg: Restoring helper dlls; Flags: runhidden

[UninstallDelete]
Type: files; Name: "{cf32}\ASCOM\HelperNET\*.*"
Type: dirifempty; Name: "{cf32}\ASCOM\HelperNET"
Type: files; Name: "{app}\ASCOM.HelperNET.tlb"
Type: dirifempty; Name: "{app}"
[CODE]
//
// Before the installer UI appears, verify that the (prerequisite)
// ASCOM Platform 5.x is installed, including both Helper components.
// Helper is required for all types (COM and .NET)!
//
//function InitializeSetup(): Boolean;
//var
//   H : Variant;
//   H2 : Variant;
//begin
//   Result := FALSE;  // Assume failure
//   try               // Will catch all errors including missing reg data
//      H := CreateOLEObject('DriverHelper.Util');  // Assure both are available
//      H2 := CreateOleObject('DriverHelper2.Util');
//      if ((H2.PlatformVersion >= 5.0) and (H2.PlatformVersion < 6.0)) then Result := TRUE;
//   except
//   end;
//   CoFreeUnusedLibraries;
   
   
//   if(not Result) then
//      MsgBox('The ASCOM Platform 5 is required for this driver. Found version ' + H2.PlatformVersion, mbInformation, MB_OK);
//end;

