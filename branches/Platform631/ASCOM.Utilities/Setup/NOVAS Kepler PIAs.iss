; Installer for NOVAS Kepler PIAs

; ***** REQUIRES INNO 5.3.4 OR LATER *****

; Initial release
; Setup Build 1

[Setup]
#define Public SetupVersion 2; Setup program version number

#define Public Major 1
#define Public Minor 0
#define Public Release 0
#define Public Build 0
#define AppVer str(Major) + "." + str(Minor) + "." + str(Release) + "." + str(SetupVersion) ; redefine to include setup version

AppCopyright=Copyright © 2009 ASCOM Initiative
;AppID must not change to maintain a consistent uninstall experience although AppName can be changed.
;This value is hard coded in the uninstall code below. If you do change this you must change the corresponding reference in
;the [Code] CurStepChanged section
AppID=ASCOM.NOVAS.Kepler.PIAs
AppName=ASCOM NOVAS Kepler PIAs
#emit "AppVerName=ASCOM NOVAS Kepler PIAs (" + Appver + ")"
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
#emit "OutputBaseFilename=ASCOM NOVAS Kepler PIAs v" + AppVer +")"
Compression=lzma
SolidCompression=true
SetupIconFile=..\ASCOM.Utilities\Resources\ASCOM.ico
ShowLanguageDialog=auto
WizardImageFile=NewWizardImage.bmp
WizardSmallImageFile=ASCOMLogo.bmp
Uninstallable=true
DirExistsWarning=no
UninstallDisplayIcon={app}\ASCOM.ico
UninstallFilesDir={cf32}\ASCOM\Uninstall\Utilities
UsePreviousTasks=false
VersionInfoCompany=ASCOM Initiative
VersionInfoCopyright=ASCOM Initiative
VersionInfoDescription=ASCOM NOVAS Kepler PIAs
VersionInfoProductName=ASCOM NOVAS Kepler PIAs
#emit "VersionInfoProductVersion=" + AppVer
#emit "VersionInfoVersion=" + AppVer
ArchitecturesInstallIn64BitMode=X64
SetupLogging=true
WindowVisible=false

[Languages]
Name: english; MessagesFile: compiler:Default.isl

[Files]
;NOVAS and Kepler PIAs and TLBs- optional through task checkbox
Source: ..\..\Interfaces\NOVAS PIAs\ASCOM.NOVAS.DLL; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: ..\..\Interfaces\Kepler PIAs\ASCOM.Kepler.DLL; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion
Source: ..\..\Interfaces\NOVAS PIAs\NOVAS.tlb; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion regtypelib 32bit
Source: ..\..\Interfaces\Kepler PIAs\Kepler.tlb; DestDir: {cf32}\ASCOM\.net; Flags: ignoreversion regtypelib 32bit

;Tool to install into GAC
Source: ..\..\GACInstall\bin\Release\GACInstall.exe; DestDir: {cf32}\ASCOM\.net; Flags: sharedfile

[Run]
;NOVAS and Kepler 32 bit interface components
Filename: {cf32}\ASCOM\.net\GACInstall.exe; Parameters: ASCOM.NOVAS.dll; Flags: runhidden; StatusMsg: Installing NOVAS 2 to the assembly cache
Filename: {cf32}\ASCOM\.net\GACInstall.exe; Parameters: ASCOM.Kepler.dll; Flags: runhidden; StatusMsg: Installing Kepler to the assembly cache
Filename: {dotnet2032}\regasm.exe; Parameters: """{cf32}\ASCOM\.net\ASCOM.NOVAS.dll"""; Flags: runhidden; StatusMsg: Registering NOVAS for COM
Filename: {dotnet2032}\regasm.exe; Parameters: """{cf32}\ASCOM\.net\ASCOM.Kepler.dll"""; Flags: runhidden; StatusMsg: Registering Kepler for COM

[UninstallRun]
;NOVAS and Kepler 32 bit interfaces
Filename: {cf32}\ASCOM\.net\GACInstall.exe; Parameters: "/U ""ASCOM.NOVAS"""; Flags: runhidden; StatusMsg: Uninstalling NOVAS 2 from the assembly cache
Filename: {cf32}\ASCOM\.net\GACInstall.exe; Parameters: "/U ""ASCOM.Kepler"""; Flags: runhidden; StatusMsg: Uninstalling Kepler from the assembly cache
Filename: {dotnet2032}\regasm.exe; Parameters: "/Unregister /TLB ""{cf32}\ASCOM\.net\ASCOM.NOVAS.dll"""; Flags: runhidden; StatusMsg: Unregistering NOVAS for COM
Filename: {dotnet2032}\regasm.exe; Parameters: "/Unregister /TLB ""{cf32}\ASCOM\.net\ASCOM.Kepler.dll"""; Flags: runhidden; StatusMsg: Unregistering Kepler for COM

[Code]
//This funciton is called automatically before install starts and will test whether platform 5 is installed
function InitializeSetup(): Boolean;
begin
  // Initialise return value
  Result:= True;

  // Test for platform 5
  if not FileExists(ExpandConstant('{cf32}\ASCOM\Interface\AscomMasterInterfaces.tlb')) then begin
    MsgBox('ASCOM Platform 5 is not installed. You must install ASCOM Platform 5a before installing this update. You can download this from http:\\www.ascom.com\downloads', mbCriticalError, MB_OK);
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
      if RegQueryStringValue(HKLM, 'Software\Microsoft\Windows\CurrentVersion\Uninstall\ASCOM.NOVAS.Kepler.PIAs_is1', 'UninstallString', Uninstall) then begin
      MsgBox('Setup will now remove the previous version.', mbInformation, MB_OK);
      Exec(RemoveQuotes(Uninstall), ' /SILENT', '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode);
    end;
  end;
end;

[Messages]
WelcomeLabel1=%n%n[name]%n
#emit "WelcomeLabel2=This will update your computer to version: " + AppVer + ".%n%nIt is recommended that you close all other applications before continuing.%n%n"

