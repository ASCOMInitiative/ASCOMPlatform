;
; Scriopt for updating a Platform 7 Device Hub installation
; This will only run on a single Platform version to prevent it over-wrting later Device Hub versions
;

#define MyAppName "ASCOM.DeviceHub"
#define MyAppVersion "7.0.0.1"
#define MyDestSubdirName "DeviceHub"

[Setup]
AppID={{94c74c75-8747-48e2-9100-736565caf056}
AppCopyright=Copyright © 2024 ASCOM Initiative
AppName={#MyAppName}
AppVersion={#MyAppVersion}
#emit "AppVerName=ASCOM Device Hub " + MyAppVersion
AppPublisher=ASCOM Initiative
AppPublisherURL=https://ascom-standards.org
AppSupportURL=https://ascomtalk.groups.io
AppUpdatesURL=https://ascom-standards.org/
VersionInfoVersion={#MyAppVersion}
MinVersion=10.0.17763
DefaultDirName="{commoncf}\ASCOM\DeviceHub"
DefaultGroupName=ASCOM Platform
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir=.\Builds
OutputBaseFilename={#MyAppName}({#MyAppVersion})_setup
Compression=lzma
MissingRunOnceIdsWarning=no
SetupIconFile=ASCOM.ico
SetupLogging=true
SignTool = SignDeviceHub
SolidCompression=yes
UninstallDisplayIcon={app}\ASCOM.DeviceHub.exe
WizardImageFile=NewWizardImage.bmp
WizardSmallImageFile=ASCOMLogo.bmp
UninstallFilesDir="{commoncf}\ASCOM\Uninstall\{#MyDestSubdirName}"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{commoncf}\ASCOM\Uninstall\{#MyDestSubdirName}"
Name: "{commoncf}\ASCOM\{#MyDestSubdirName}"

[Files]
Source: "..\DeviceHub\bin\release\ASCOM.DeviceHub.exe";            DestDir: "{app}"; Flags: ignoreversion
Source: "..\DeviceHub\bin\release\ASCOM.DeviceHub.exe.config";     DestDir: "{app}"; Flags: ignoreversion
Source: "..\DeviceHub\bin\release\ASCOM.DeviceHub.pdb";            DestDir: "{app}"; Flags: ignoreversion
Source: "..\DeviceHub\bin\release\MvvmMessenger.dll";              DestDir: "{app}"; Flags: ignoreversion
Source: "..\DeviceHub\bin\release\MvvmMessenger.pdb";              DestDir: "{app}"; Flags: ignoreversion
Source: "..\DeviceHub\bin\release\PresentationFramework.Aero.dll"; DestDir: "{app}"; Flags: ignoreversion

[InstallDelete]
Type: files; Name: "{app}\*.*"

[Icons]
Name: "{commonprograms}\ASCOM Platform\ASCOM Device Hub"; Filename: "{app}\ASCOM.DeviceHub.exe"; WorkingDir: "{app}"

; Only if driver is .NET
[Run]

; Only for .NET local-server drivers
Filename: "{app}\ASCOM.DeviceHub.exe"; Parameters: "/register"

; Only if driver is .NET
[UninstallRun]
; Only for .NET local-server drivers
Filename: "{app}\ASCOM.DeviceHub.exe"; Parameters: "/unregister"

[Code]
const
   REQUIRED_PLATFORM_VERSION = '7.0.0.4619';    // Set this to the specific ASCOM Platform version required for this application
   REQUIRED_DOTNET_VERSION = 4.8;

//
// Function to return the ASCOM Platform's version number as a double.
//
function PlatformVersion(): Double;
var
   PlatVerString : String;
begin
   Result := 0.0;  // Initialise the return value in case we can't read the registry
   try
      if RegQueryStringValue(HKEY_LOCAL_MACHINE_32, 'Software\ASCOM','PlatformVersion', PlatVerString) then 
      begin // Successfully read the value from the registry
         Result := StrToFloat(PlatVerString); // Create a double from the X.Y Platform version string
      end;
   except                                                                   
      ShowExceptionMessage;
      Result:= -1.0; // Indicate in the return value that an exception was generated
   end;
end;

//
// Function to return the ASCOM Platform's version number as a double.
//
function PlatformVersionString(): String;
var
   PlatVerString : String;
begin
   Result := '0.0';  // Initialise the return value in case we can't read the registry
   try
      if RegQueryStringValue(HKEY_LOCAL_MACHINE_32, 'Software\ASCOM\Platform','Platform Version', PlatVerString) then 
      begin // Successfully read the value from the registry
         Result := PlatVerString; // Return the value found
      end;
   except                                                                   
      ShowExceptionMessage;
      Result:= '-1.0'; // Indicate in the return value that an exception was generated
   end;
end;

function IsDotNetDetected(version: string; service: cardinal): boolean;
// Indicates whether the specified version and service pack of the .NET Framework is installed.
//
// version -- Specify one of these strings for the required .NET Framework version:
//    'v1.1'          .NET Framework 1.1
//    'v2.0'          .NET Framework 2.0
//    'v3.0'          .NET Framework 3.0
//    'v3.5'          .NET Framework 3.5
//    'v4\Client'     .NET Framework 4.0 Client Profile
//    'v4\Full'       .NET Framework 4.0 Full Installation
//    'v4.5'          .NET Framework 4.5
//    'v4.5.1'        .NET Framework 4.5.1
//    'v4.5.2'        .NET Framework 4.5.2
//    'v4.6'          .NET Framework 4.6
//    'v4.6.1'        .NET Framework 4.6.1
//    'v4.6.2'        .NET Framework 4.6.2
//    'v4.7'          .NET Framework 4.7
//    'v4.7.1'        .NET Framework 4.7.1
//    'v4.7.2'        .NET Framework 4.7.2
//    'v4.8'          .NET Framework 4.8
//
// service -- Specify any non-negative integer for the required service pack level:
//    0               No service packs required
//    1, 2, etc.      Service pack 1, 2, etc. required
var
    key, versionKey: string;
    install, release, serviceCount, versionRelease: cardinal;
    success: boolean;
begin
    versionKey := version;
    versionRelease := 0;

    // .NET 1.1 and 2.0 embed release number in version key
    if version = 'v1.1' then begin
        versionKey := 'v1.1.4322';
    end else if version = 'v2.0' then begin
        versionKey := 'v2.0.50727';
    end

    // .NET 4.5 and newer install as update to .NET 4.0 Full
    else if Pos('v4.', version) = 1 then begin
        versionKey := 'v4\Full';
        case version of
          'v4.5':   versionRelease := 378389;
          'v4.5.1': versionRelease := 378675; // 378758 on Windows 8 and older
          'v4.5.2': versionRelease := 379893;
          'v4.6':   versionRelease := 393295; // 393297 on Windows 8.1 and older
          'v4.6.1': versionRelease := 394254; // 394271 before Win10 November Update
          'v4.6.2': versionRelease := 394802; // 394806 before Win10 Anniversary Update
          'v4.7':   versionRelease := 460798; // 460805 before Win10 Creators Update
          'v4.7.1': versionRelease := 461308; // 461310 before Win10 Fall Creators Update
          'v4.7.2': versionRelease := 461808; // 461814 before Win10 April 2018 Update
          'v4.8':   versionRelease := 528040; // 528049 before Win10 May 2019 Update
        end;
    end;

    // installation key group for all .NET versions
    key := 'SOFTWARE\Microsoft\NET Framework Setup\NDP\' + versionKey;

    // .NET 3.0 uses value InstallSuccess in subkey Setup
    if Pos('v3.0', version) = 1 then begin
        success := RegQueryDWordValue(HKLM, key + '\Setup', 'InstallSuccess', install);
    end else begin
        success := RegQueryDWordValue(HKLM, key, 'Install', install);
    end;

    // .NET 4.0 and newer use value Servicing instead of SP
    if Pos('v4', version) = 1 then begin
        success := success and RegQueryDWordValue(HKLM, key, 'Servicing', serviceCount);
    end else begin
        success := success and RegQueryDWordValue(HKLM, key, 'SP', serviceCount);
    end;

    // .NET 4.5 and newer use additional value Release
    if versionRelease > 0 then begin
        success := success and RegQueryDWordValue(HKLM, key, 'Release', release);
        success := success and (release >= versionRelease);
    end;

    result := success and (install = 1) and (serviceCount >= service);
end;

//
// Before the installer UI appears, verify that the required ASCOM Platform version is installed.
//
function InitializeSetup(): Boolean;
var
   PlatformVersionNumber : string;
   RequiredPlatformVersion : string;
 begin
   RequiredPlatformVersion := REQUIRED_PLATFORM_VERSION;
   result := FALSE;  // Assume failure

   PlatformVersionNumber := PlatformVersionString(); // Get the installed Platform version as a double
   If PlatformVersionNumber = RequiredPlatformVersion then	// Check whether we have the required Platform version (not earlier or later to avoid over-writing later versions)
      result := TRUE
   else
   begin
     if PlatformVersionNumber = '-1.0' then
       MsgBox('Unable to determine with current ASCOM Platform version from the registry. Please report this on the ASCOM support forum: https://ascomtalk.groups.io/g/Help', mbCriticalError, MB_OK)
     else 
     begin
       if PlatformVersionNumber = '0.0' then
          MsgBox('No ASCOM Platform is installed. Please install Platform ' + REQUIRED_PLATFORM_VERSION + ' or later from http://www.ascom-standards.org', mbCriticalError, MB_OK)
       else 
          MsgBox('This installer is specific to ASCOM Platform ' + REQUIRED_PLATFORM_VERSION + ' and is not required on other Platform versions.' #13#10#13#10 \
            'Platform '+ PlatformVersionNumber + ' is currently installed on this PC.' #13#10#13#10 \
            'The latest Platform can be found at ascom-standards.org', mbCriticalError, MB_OK);
     end;
   end;
end;

// Code to enable the installer to uninstall previous versions of itself when a new version is installed
procedure CurStepChanged(CurStep: TSetupStep);
var
  ResultCode: Integer;
  UninstallExe: String;
  UninstallRegistry: String;
begin
  if (CurStep = ssInstall) then // Install step has started
	begin
      // Create the correct registry location name, which is based on the AppId
      UninstallRegistry := ExpandConstant('Software\Microsoft\Windows\CurrentVersion\Uninstall\{#SetupSetting("AppId")}' + '_is1');
      // Check whether an extry exists
      if RegQueryStringValue(HKLM, UninstallRegistry, 'UninstallString', UninstallExe) then
        begin // Entry exists and previous version is installed so run its uninstaller quietly after informing the user
          MsgBox('Setup will now remove the previous version.', mbInformation, MB_OK);
          Exec(RemoveQuotes(UninstallExe), ' /SILENT', '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode);
          sleep(1000);    //Give enough time for the install screen to be repainted before continuing
        end;
  end;
end;

