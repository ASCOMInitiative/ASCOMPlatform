; ASCOM Install Script for RoboFocus Control Program
; Jon Brewster
; Bob Denny (09-Nov-09) Fix for ASCOM Platform >5, fix uninstall for case
; where user did not select extra instances. Fix for 64-bit systems.

[Setup]
#define Ver "5-2-0"

AppName=ASCOM RoboFocus Control Program
AppVerName=RoboFocus {#Ver}
AppVersion={#Ver}
AppPublisher=Jon Brewster <jon@brewsters.net>
AppPublisherURL=mailto:jon@brewsters.net
AppSupportURL=http://tech.groups.yahoo.com/group/ASCOM-Talk/
AppUpdatesURL=http://ascom-standards.org/
MinVersion=0,5.0.2195sp4
DefaultDirName="{cf}\ASCOM\Focuser"
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir="."
OutputBaseFilename="RoboFocus {#Ver} Setup"
Compression=lzma
SolidCompression=yes
; Put there by Platform if Driver Installer Support selected
WizardImageFile="C:\Program Files (x86)\ASCOM\InstallGen\Resources\WizardImage.bmp"
LicenseFile="C:\Program Files (x86)\ASCOM\InstallGen\Resources\CreativeCommons.txt"
; {cf}\ASCOM\Uninstall\Focuser folder created by Platform, always
UninstallFilesDir="{cf}\ASCOM\Uninstall\Focuser\RoboFocus CP"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{cf}\ASCOM\Uninstall\Focuser\RoboFocus CP"
; TODO: Add subfolders below {app} as needed (e.g. Name: "{app}\MyFolder")

[InstallDelete]
; ditch any old source (including obsolete versions)
Type: filesandordirs; Name: "{app}\Source\RoboFocus Server"
Type: filesandordirs; Name: "{app}\Source\RoboFocus CP"

[Icons]
Name: "{userdesktop}\RoboFocus"; Filename: "{app}\RoboFocusServer.exe"
Name: "{userdesktop}\RoboFocus 2"; Filename: "{app}\RoboFocusServer2.exe"; Flags: createonlyiffileexists
Name: "{userdesktop}\RoboFocus 3"; Filename: "{app}\RoboFocusServer3.exe"; Flags: createonlyiffileexists
Name: "{userdesktop}\Uninstall RoboFocus"; Filename: "{uninstallexe}"

[Tasks]
 Name: SourceCode; Description: "Install source code"; Flags: unchecked
 Name: Second; Description: "Install for 2nd focuser"; Flags: unchecked
 Name: Third; Description: "Install for 3rd focuser"; Flags: unchecked
 
[Files]
Source: "RoboFocusServer.exe"; DestDir: "{app}"; Flags: ignoreversion; BeforeInstall: Unregister('RoboFocusServer.exe')
Source: "RoboFocusServer2.exe"; DestDir: "{app}"; Flags: ignoreversion; BeforeInstall: Unregister('RoboFocusServer2.exe'); Tasks: Second;
Source: "RoboFocusServer3.exe"; DestDir: "{app}"; Flags: ignoreversion; BeforeInstall: Unregister('RoboFocusServer3.exe'); Tasks: Third;
Source: "robofocusins3.doc"; DestDir: "{app}"; Flags: ignoreversion
; Source code
Source: "Docs\RoboFocus ActiveX Functions.doc"; DestDir: "{app}\Source\RoboFocus CP\Docs"; Tasks: SourceCode;
Source: "Docs\RoboFocus ActiveX.doc"; DestDir: "{app}\Source\RoboFocus CP\Docs"; Tasks: SourceCode;
Source: "102701.tcd"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "AslashR.bmp"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "CommonModule.bas"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "ErrorConstants.bas"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "Focus.cls"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "FocusControl.cls"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmAbout.frm"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmAbout.frx"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmAbsRel.frm"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmAbsRel.frx"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmCal.frm"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmComm2.frm"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmComm2.frx"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmConfig.frm"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmConfig.frx"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmHelp.frm"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmHelp.frx"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmMain.frm"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmMain.frx"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmMessage.frm"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmMessage.frx"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmSetupList.frm"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmSetupList.frx"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmShow.frm"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmShow.frx"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmTemp.frm"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmTemp.frx"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmTrain.frm"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmTrain.frx"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmWarnBeforeLoad.frm"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "Module1.bas"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "ReadMe.txt"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "register.bat"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "rfscript.vbs"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RFTrack.log"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocus3.CWS"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocus3.DEP"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocus3.SQS"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocus3.TLB"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocus3.VBR"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocus Setup.iss"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocus.ico"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocus.ini"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocus.PDM"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocus.TLB"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocus.vbp"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocus.VBR"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocus.vbw"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocus2.vbw"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocus3.vbw"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocus.vbg"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "robofocusins3.doc"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocusServer-ref.exe"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocusServer2-ref.exe"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "RoboFocusServer3-ref.exe"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "Switch.cls"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "tempcomp ActiveX.doc"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "temperature1.jpg"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "test1.tcd"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "test1aspread.xls"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "testtv2.tcd"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "testTV 705.tcd"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "testTV.tcd"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "unregister.bat"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "Weds Test 2.tcd"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "Weds Test.tcd"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;
Source: "frmTemp.log"; DestDir: "{app}\Source\RoboFocus CP"; Tasks: SourceCode;

;Only if COM Local Server
[Run]
Filename: "{app}\RoboFocusServer.exe"; Parameters: "/regserver"
Filename: "{app}\RoboFocusServer.exe"; Parameters: "-r"
Filename: "{app}\RoboFocusServer2.exe"; Parameters: "/regserver"; Flags: skipifdoesntexist
Filename: "{app}\RoboFocusServer2.exe"; Parameters: "-r"; Flags: skipifdoesntexist
Filename: "{app}\RoboFocusServer3.exe"; Parameters: "/regserver"; Flags: skipifdoesntexist
Filename: "{app}\RoboFocusServer3.exe"; Parameters: "-r"; Flags: skipifdoesntexist

;Only if COM Local Server
[UninstallRun]
Filename: "{app}\RoboFocusServer.exe"; Parameters: "/unregserver"
Filename: "{app}\RoboFocusServer2.exe"; Parameters: "/unregserver"; Flags: skipifdoesntexist
Filename: "{app}\RoboFocusServer3.exe"; Parameters: "/unregserver"; Flags: skipifdoesntexist

[Code]
procedure Unregister(FileName: String);
var
  ResultCode: Integer;
begin
  // if component already exists, then unregister it
  if FileExists(ExpandConstant('{app}\' + FileName)) then
  begin
    Exec(ExpandConstant('{app}\' + FileName), '/unregserver', '', 0,
     ewWaitUntilTerminated, ResultCode)
  end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
   Helper: Variant;
begin
   if CurUninstallStep = usUninstall then
   begin
     Helper := CreateOleObject('DriverHelper.Profile');
     Helper.DeviceType := 'Focuser';
     Try
       Helper.Unregister('RoboFocusServer.Focuser');
       Helper.Unregister('RoboFocusServer2.Focuser');
       Helper.Unregister('RoboFocusServer3.Focuser');
     Except
       ;
     end;
     Helper.DeviceType := 'Switch';
     Try
       Helper.Unregister('RoboFocusServer.Switch');
       Helper.Unregister('RoboFocusServer2.Switch');
       Helper.Unregister('RoboFocusServer3.Switch');
     Except
       ;
     end;
  end;
end;
