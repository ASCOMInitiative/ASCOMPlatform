; Installer for HelperNet
; 5.0.1.0 Removed register from the VB6 helper dlls
; 5.0.2.0 Added version 5 redirection policy install and XML intellisense file
; 5.1.3.0 Added profile Explorer
[Setup]
#define AppVer GetFileVersion("..\HelperNET\bin\Release\ASCOM.HelperNET.dll") ; define version variable
AppCopyright=Copyright © 2009 ASCOM Initiative
;AppID must not change to maintain a consistent uninstall experience although AppName can be changed.
;This value is hard coded in the uninstall code below. If you do change this you must change the corresponding reference in
;the [Code] CurStepChanged section
AppID="ASCOM.Platform.NET.Components"
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
DefaultGroupName="ASCOM Platform\Docs\Helper Components"
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
WizardSmallImageFile=ASCOMLogo.bmp
Uninstallable=yes
DirExistsWarning=no
;UninstallDisplayIcon={cf32}\ASCOM\HelperNET\EraseProfile.exe,1
UninstallDisplayIcon={app}\ASCOM.ico,4
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
Source: "..\HelperNET\bin\Release\ASCOM.HelperNET.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\HelperNET\bin\Release\ASCOM.HelperNET.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\HelperNET\bin\Release\Interop.Scripting.dll"; DestDir: "{app}"; Flags: ignoreversion

Source: "..\VB6Helper\Helper.dll"; DestDir: "{cf32}\ASCOM"; Flags: ignoreversion uninsneveruninstall 32bit
Source: "..\VB6Helper2\Helper2.dll"; DestDir: "{cf32}\ASCOM"; Flags: ignoreversion uninsneveruninstall 32bit

;Install the ASCOM Master Interface type library introduced at 5.1.0.0
;Source: "..\..\ASCOM Master Interfaces\ASCOM Master Interfaces\bin\Release\ASCOM.MasterInterfaces.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "..\..\ASCOM Master Interfaces\ASCOM Master Interfaces\bin\Release\ASCOM.MasterInterfaces.pdb"; DestDir: "{app}"; Flags: ignoreversion
;Source: "..\..\ASCOM Master Interfaces\ASCOM Master Interfaces\bin\Release\ASCOM.MasterInterfaces.xml"; DestDir: "{app}"; Flags: ignoreversion

;Copy the policy files
Source: "..\Redirect Policy\bin\release\PublisherPolicy.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Redirect Policy\bin\release\policy.5.2.ASCOM.HelperNET.dll"; DestDir: "{app}"; Flags: ignoreversion

;Make sure we have backout copies of the original helpers
Source: "..\OriginalHelpers\Helper.dll"; DestDir: "{cf32}\ASCOM\HelperNET"; Flags: ignoreversion
Source: "..\OriginalHelpers\Helper2.dll"; DestDir: "{cf32}\ASCOM\HelperNET"; Flags: ignoreversion
Source: "..\OriginalHelpers\RestoreOriginalHelpers.cmd"; DestDir: "{cf32}\ASCOM\HelperNET"; Flags: ignoreversion

;ASCOM Platform .NET Help files
Source: "..\Help\Help\HelperNET.chm"; DestDir: "{cf}\ASCOM\Doc"; Flags: ignoreversion
Source: "..\Help\Help\HelperNET.pdf"; DestDir: "{cf}\ASCOM\Doc"; Flags: ignoreversion

;Profile Explorer
Source: "..\Profile Explorer\bin\Release\ProfileExplorer.exe"; DestDir: "{pf}\ASCOM\Profile Explorer"; Flags: ignoreversion
Source: "..\Profile Explorer\bin\Release\ProfileExplorer.pdb"; DestDir: "{pf}\ASCOM\Profile Explorer"; Flags: ignoreversion

;Tool to erase profile
Source: "..\EraseProfile\bin\Release\EraseProfile.exe"; DestDir: "{cf32}\ASCOM\HelperNET"; Flags: ignoreversion

;Tool to install into GAC
Source: "..\..\GACInstall\bin\Release\GACInstall.exe"; DestDir: {app}; Flags: ignoreversion

;ASCOM Icon
Source: "..\HelperNET\Resources\ASCOM.ico"; DestDir: "{app}"; Flags: ignoreversion

[Registry]
Root: HKLM64; Subkey: "SOFTWARE\Microsoft\.NETFramework\v2.0.50727\AssemblyFoldersEx\ASCOM64"; ValueType: string;  ValueName: ""; ValueData: "{cf}\ASCOM\.net";Flags: uninsdeletekey; Check: IsWin64
Root: HKLM32; Subkey: "SOFTWARE\Microsoft\.NETFramework\v2.0.50727\AssemblyFoldersEx\ASCOM64"; ValueType: string;  ValueName: ""; ValueData: "{cf64}\ASCOM\.net";Flags: uninsdeletekey; Check: IsWin64

[Icons]
Name: "{group}\ASCOM Platform for .NET"; Filename: {cf}\ASCOM\Doc\HelperNET.chm
Name: "{group}\ASCOM HelperNET Architecture"; Filename: {cf}\ASCOM\Doc\HelperNET.pdf
Name: "{commonprograms}\ASCOM Platform\Tools\Profile Explorer"; Filename: {pf}\ASCOM\Profile Explorer\ProfileExplorer.exe

[Run]
; Install to the GAC and register COM types
Filename: "{app}\GACInstall.exe"; Parameters: ASCOM.HelperNET.dll; Flags: runhidden; StatusMsg: Installing HelperNET to the assembly cache
Filename: "{app}\GACInstall.exe"; Parameters: Interop.Scripting.dll; Flags: runhidden; StatusMsg: Installing Interop.Scripting to the assembly cache
Filename: "{win}\Microsoft.NET\Framework\v2.0.50727\regasm.exe"; Parameters: "/TLB ""{app}\ASCOM.HelperNET.dll"""; Flags: runhidden; StatusMsg: Registering HelperNET for COM
Filename: "{app}\GACInstall.exe"; Parameters: policy.5.2.ASCOM.HelperNET.dll; Flags: runhidden; StatusMsg: Installing HelperNET redirection policy to the assembly cache
Filename: "{cf32}\ASCOM\HelperNET\EraseProfile.exe"; Tasks: cleanprofile
#emit 'Filename: "XCopy"; Parameters: """{app}\ASCOM.HelperNET.pdb"" ""{win}\assembly\GAC_MSIL\ASCOM.HelperNET\' + AppVer + '__565de7938946fba7""";StatusMsg: Installing debug symbols into the GAC;Flags: runhidden waituntilterminated'

;Master interfaces install
;Filename: "{app}\GACInstall.exe"; Parameters: ASCOM.MasterInterfaces.dll; Flags: runhidden; StatusMsg: Installing ASCOM Master Interfaces to the assembly cache
;Filename: "{win}\Microsoft.NET\Framework\v2.0.50727\regasm.exe"; Parameters: "/TLB ""{app}\ASCOM.MasterInterfaces.dll"""; Flags: runhidden; StatusMsg: Registering ASCOM Master Interfaces for COM

[UninstallRun]
Filename: "{app}\GACInstall.exe"; Parameters: "/U ""policy.5.2.ASCOM.HelperNET"""; Flags: runhidden; StatusMsg: Uninstalling HelperNET redirection policy from the assembly cache
Filename: "{app}\GACInstall.exe"; Parameters: "/U ""ASCOM.HelperNET"""; Flags: runhidden; StatusMsg: Uninstalling HelperNET from the assembly cache
Filename: "{app}\GACInstall.exe"; Parameters: "/U ""Interop.Scripting"""; Flags: runhidden; StatusMsg: Uninstalling Interop.Scripting from the assembly cache
Filename: "{win}\Microsoft.NET\Framework\v2.0.50727\regasm.exe"; Parameters: "/Unregister /TLB ""{app}\ASCOM.HelperNET.dll"""; Flags: runhidden; StatusMsg: Unregistering HelperNET for COM
Filename: "{cf32}\ASCOM\HelperNET\RestoreOriginalHelpers.cmd"; Parameters: """{cf32}\ASCOM\HelperNET\*.dll"" ""{cf32}\ASCOM"""; StatusMsg: Restoring helper dlls; Flags: runhidden

;Master interfaces uninstall
;Filename: "{app}\GACInstall.exe"; Parameters: "/U ""ASCOM.MasterInterfaces.dll"""; Flags: runhidden; StatusMsg: Uninstalling ASCOM Master Interfaces from the assembly cache
;Filename: "{win}\Microsoft.NET\Framework\v2.0.50727\regasm.exe"; Parameters: "/Unregister /TLB ""{app}\ASCOM.MasterInterfaces.dll"""; Flags: runhidden; StatusMsg: Unregistering ASCOM Master Interfaces for COM

[UninstallDelete]
Type: files; Name: "{cf32}\ASCOM\HelperNET\*.*"
Type: dirifempty; Name: "{cf32}\ASCOM\HelperNET"
Type: files; Name: "{app}\ASCOM.HelperNET.tlb"
Type: dirifempty; Name: "{app}"

[CODE]
procedure CurStepChanged(CurStep: TSetupStep);
var
  ResultCode: Integer;
  Uninstall: String;
begin
  if (CurStep = ssInstall) then begin
      if RegQueryStringValue(HKLM, 'Software\Microsoft\Windows\CurrentVersion\Uninstall\ASCOM.Platform.NET.Components_is1', 'UninstallString', Uninstall) then begin
      MsgBox('Setup will now remove the previous version.', mbInformation, MB_OK);
      Exec(RemoveQuotes(Uninstall), ' /SILENT', '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode);
    end;
  end;
end;

