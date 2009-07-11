; Installer for HelperNet
;
[Setup]
#define AppVer GetFileVersion("..\HelperNET\bin\Release\ASCOM.HelperNET.dll") ; define version variable
AppCopyright=Copyright © 2009 ASCOM Initiative
AppName=ASCOM Helper for .NET
#emit "AppVerName=HelperNET " + AppVer
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
#emit "OutputBaseFilename=HelperNET(" + AppVer +")setup32"
Compression=lzma
SolidCompression=yes
SetupIconFile=..\HelperNET\Resources\ASCOM.ico
ShowLanguageDialog=auto
; Put there by Platform if Driver Installer Support selected
WizardImageFile="C:\Program Files\ASCOM\InstallGen\Resources\WizardImage.bmp"
; {cf}\ASCOM\Uninstall\Camera folder created by Platform, always
Uninstallable=no
DirExistsWarning=no
UninstallFilesDir="{cf}\ASCOM\Uninstall\HelperNET"
VersionInfoCompany=ASCOM Initiative
VersionInfoCopyright=ASCOM Initiative
VersionInfoDescription=ASCOM HelperCOmponent for .NET
VersionInfoProductName=HelperNET
#emit "VersionInfoProductVersion=" + AppVer
#emit "VersionInfoVersion=" + AppVer
;ArchitecturesInstallIn64BitMode=X64

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{cf}\ASCOM\Uninstall\HelperNET"

;  Add an option to install the source files
[Tasks]

[Files]
Source: "..\HelperNET\bin\Release\ASCOM.HelperNET.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\HelperNET\bin\Release\ASCOM.HelperNET.reg"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\HelperNET\bin\Release\Interop.Scripting.dll"; DestDir: "{app}"; Flags: ignoreversion

;Source: "..\HelperNET\bin\Debug\ASCOM.HelperNET.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "..\HelperNET\bin\Debug\ASCOM.HelperNET.pdb"; DestDir: "{app}"; Flags: ignoreversion
;Source: "..\HelperNET\bin\Debug\ASCOM.HelperNET.reg"; DestDir: "{app}"; Flags: ignoreversion
;Source: "..\HelperNET\bin\Debug\Interop.Scripting.dll"; DestDir: "{app}"; Flags: ignoreversion

Source: "..\VB6Helper\Helper.dll"; DestDir: "{cf}\ASCOM"; Flags: ignoreversion
Source: "..\VB6Helper2\Helper2.dll"; DestDir: "{cf}\ASCOM"; Flags: ignoreversion
Source: "..\..\GACInstall\bin\Release\GACInstall.exe"; DestDir: {app}; Flags: ignoreversion
Source: "RegAsmCmd.Cmd"; DestDir: {app}; Flags: ignoreversion
;Chris's serial files
;Source: "..\SerialHelper\bin\Release\ASCOM.SerialHelper.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "..\SerialHelper\bin\Release\ASCOM.SerialHelper.dll.config"; DestDir: "{app}"; Flags: ignoreversion

[Registry]

[Run]
; Install to the GAC and register COM types
Filename: {app}\GACInstall.exe; Parameters: ASCOM.HelperNET.dll; Flags: runhidden; StatusMsg: Installing HelperNET to the assembly cache
Filename: {app}\GACInstall.exe; Parameters: Interop.Scripting.dll; Flags: runhidden; StatusMsg: Installing Interop.Scripting to the assembly cache
Filename: "{win}\Microsoft.NET\Framework\v2.0.50727\regasm.exe"; Parameters: "/TLB ""{app}\ASCOM.HelperNET.dll"""; Flags: runhidden; StatusMsg: Installing HelperNET to the assembly cache;
;Filename: "{win}\Microsoft.NET\Framework\v2.0.50727\regasm.exe"; Parameters: "/TLB ""{app}\ASCOM.SerialHelper.dll"""; Flags: runhidden; StatusMsg: Installing SerialHelper to the assembly cache;
;Filename: {app}\GACInstall.exe; Parameters: ASCOM.SerialHelper.dll; Flags: runhidden; StatusMsg: Installing SerialHelper to the assembly cache
;Filename: {app}\RegAsmCmd.Cmd; Parameters: "/TLB ""{app}\ASCOM.HelperNET.dll"""; StatusMsg: Installing HelperNET to the assembly cache; Flags: waituntilterminated shellexec
;filename: {win}\regedit.exe; Parameters: "/S ""{app}\ASCOM.HelperNET.dll"""; StatusMsg: Running COM registration
;Flags: runhidden;
[CODE]
//
// Before the installer UI appears, verify that the (prerequisite)
// ASCOM Platform 5.x is installed, including both Helper components.
// Helper is required for all types (COM and .NET)!
//
function InitializeSetup(): Boolean;
//var
  // H : Variant;
  // H2 : Variant;
begin
   Result := FALSE;  // Assume failure
   try               // Will catch all errors including missing reg data
//      H := CreateOLEObject('DriverHelper.Util');  // Assure both are available
 //     H2 := CreateOleObject('DriverHelper2.Util');
  //    if ((H2.PlatformVersion >= 5.0) and (H2.PlatformVersion < 6.0)) then
         Result := TRUE;
   except
   end;
   if(not Result) then
 //     MsgBox('The ASCOM Platform 5 is required for this driver. Found version ' + H2.PlatformVersion, mbInformation, MB_OK);
end;

