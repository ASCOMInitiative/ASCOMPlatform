;;    Remarks for the template are prefixed like this
;;
;;    2007-sep-08  cdr  Add registry code for DCOM
;;                      Add RegASCOM if COM dll or exe
;;
;;    2007-Sep-09  rbd  Comment for CLSID, make more obvious :-)
;;                      Add dual-interface support, AppId uninstall
;;                      Add driver source code install
;;                      Many small adjustments
;;
;;    2007-Dec-19  cdr  Add /codebase switch to regasm install command
;;                      for .NET assembly driver. Needed for driver to 
;;                      reside outside GAC.
;
; Script generated by the ASCOM Driver Installer Script Generator %gver%
; Generated by %devn% on %date% (UTC)
;
[Setup]
AppName=ASCOM %name% %type% Driver
AppVerName=ASCOM %name% %type% Driver %vers%
AppVersion=%vers%
AppPublisher=%devn% <%deve%>
AppPublisherURL=mailto:%deve%
AppSupportURL=http://tech.groups.yahoo.com/group/ASCOM-Talk/
AppUpdatesURL=http://ascom-standards.org/
MinVersion=0,5.0.2195sp4
DefaultDirName="{cf}\ASCOM\%type%"
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir="."
OutputBaseFilename="%name% Setup"
Compression=lzma
SolidCompression=yes
; Put there by Platform if Driver Installer Support selected
WizardImageFile="%rscf%\WizardImage.bmp"
LicenseFile="%rscf%\CreativeCommons.txt"
; {cf}\ASCOM\Uninstall\%type% folder created by Platform, always
UninstallFilesDir="{cf}\ASCOM\Uninstall\%type%\%name%"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{cf}\ASCOM\Uninstall\%type%\%name%"
%nlcs%; TODO: Add ServedClasses folder for driver assemblies
; TODO: Add subfolders below {app} as needed (e.g. Name: "{app}\MyFolder")
%srce%
%srce%;  Add an option to install the source files
%srce%[Tasks]
%srce%Name: source; Description: Install the Source files; Flags: unchecked

[Files]
%cdll%; regserver flag only if native COM, not .NET
%coms%Source: "%srcp%\%file%"; DestDir: "{app}" %rs32%
%nbth%Source: "%srcp%\bin\Release\%file%"; DestDir: "{app}"
%nlcs%; TODO: Add driver assemblies into the ServedClasses folder
; Require a read-me HTML to appear after installation, maybe driver's Help doc
Source: "%srcp%\%rdmf%"; DestDir: "{app}"; Flags: isreadme
%srce%; Optional source files (COM and .NET aware)
%srce%Source: "%srcp%\*"; Excludes: *.zip,*.exe,*.dll, \bin\*, \obj\*; DestDir: "{app}\Source\%name% Driver"; Tasks: source; Flags: recursesubdirs
; TODO: Add other files needed by your driver here (add subfolders above)
%cexe%
%cexe%;Only if COM Local Server
%cexe%[Run]
%cexe%Filename: "{app}\%file%"; Parameters: "/regserver"
%cexe%
%nbth%
%nbth%; Only if driver is .NET
%nbth%[Run]
%nasm%; Only for .NET assembly/in-proc drivers
%nasm%Filename: "%rgsm%"; Parameters: "/codebase ""{app}\%file%"""; Flags: runhidden
%nlcs%; Only for .NET local-server drivers
%nlcs%Filename: "{app}\%file%"; Parameters: "/register"
%cexe%;Only if COM Local Server
%cexe%[UninstallRun]
%cexe%Filename: "{app}\%file%"; Parameters: "/unregserver"
%nbth%
%nbth%; Only if driver is .NET
%nbth%[UninstallRun]
%nasm%; Only for .NET assembly/in-proc drivers
%nasm%Filename: "%rgsm%"; Parameters: "-u ""{app}\%file%"""; Flags: runhidden
%nlcs%; Only for .NET local-server drivers
%nlcs%Filename: "{app}\%file%"; Parameters: "/unregister"

%cexe%;  DCOM setup for COM local Server, needed for TheSky
%cexe%[Registry]
%cexe%; TODO: If needed set this value to the %type% CLSID of your driver (mind the leading/extra '{')
%cexe%#define AppClsid "{%cid1%"
%cex2%; TODO: If needed set this value to the %typ2% CLSID of your driver (mind the leading/extra '{')
%cex2%#define AppClsid2 "{%cid2%"

%cexe%; set the DCOM access control for TheSky on the %type% interface
%cexe%Root: HKCR; Subkey: CLSID\{#AppClsid}; ValueType: string; ValueName: AppID; ValueData: {#AppClsid}
%cexe%Root: HKCR; Subkey: AppId\{#AppClsid}; ValueType: string; ValueData: "ASCOM %name% %type% Driver"
%cexe%Root: HKCR; Subkey: AppId\{#AppClsid}; ValueType: string; ValueName: AppID; ValueData: {#AppClsid}
%cexe%Root: HKCR; Subkey: AppId\{#AppClsid}; ValueType: dword; ValueName: AuthenticationLevel; ValueData: 1
%cex2%; set the DCOM access control for TheSky on the %typ2% interface
%cex2%Root: HKCR; Subkey: CLSID\{#AppClsid2}; ValueType: string; ValueName: AppID; ValueData: {#AppClsid2}
%cex2%Root: HKCR; Subkey: AppId\{#AppClsid2}; ValueType: string; ValueData: "ASCOM %name% %type% Driver"
%cex2%Root: HKCR; Subkey: AppId\{#AppClsid2}; ValueType: string; ValueName: AppID; ValueData: {#AppClsid2}
%cex2%Root: HKCR; Subkey: AppId\{#AppClsid2}; ValueType: dword; ValueName: AuthenticationLevel; ValueData: 1
%cexe%; CAUTION! DO NOT EDIT - DELETING ENTIRE APPID TREE WILL BREAK WINDOWS!
%cexe%Root: HKCR; Subkey: AppId\{#AppClsid}; Flags: uninsdeletekey
%cex2%Root: HKCR; Subkey: AppId\{#AppClsid2}; Flags: uninsdeletekey

%coms%; code to register and unregister the driver with the chooser
%coms%; modified by CR to use with all COM servers (dll and exe) 
%coms%[Code]
%coms%procedure RegASCOM();
%coms%var
%coms%   Helper: Variant;
%coms%begin
%coms%   Helper := CreateOleObject('DriverHelper.Profile');
%coms%   Helper.DeviceType := '%type%';
%coms%   Helper.Register('%name%.%type%', '%fnam%');
%cex2%   Helper.DeviceType := '%typ2%';
%cex2%   Helper.Register('%name%.%typ2%', '%fnam%');
%coms%end;
%coms%
%coms%procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
%coms%var
%coms%   Helper: Variant;
%coms%begin
%coms%   if CurUninstallStep = usUninstall then
%coms%   begin
%coms%     Helper := CreateOleObject('DriverHelper.Profile');
%coms%     Helper.DeviceType := '%type%';
%coms%     Helper.Unregister('%name%.%type%');
%cex2%     Helper.DeviceType := '%typ2%';
%cex2%     Helper.Unregister('%name%.%typ2%');
%coms%  end;
%coms%end;
