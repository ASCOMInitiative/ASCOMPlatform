; CLW file contains information for the MFC ClassWizard

[General Info]
Version=1
ResourceCount=1
ClassCount=2
Class1=CRoboFocusApp
LastClass=Setup
LastTemplate=CDialog
NewFileInclude1=#include "stdafx.h"
NewFileInclude2=#include "resource.h"
ODLFile=RoboFocus.idl
LastPage=0
Class2=Setup
Resource1=IDD_SETUP


[CLS:CRoboFocusApp]
Type=0
HeaderFile=RoboFocus.h
ImplementationFile=RoboFocus.cpp
Filter=N
BaseClass=CWinApp
VirtualFilter=AC
LastObject=CRoboFocusApp

[DLG:IDD_SETUP]
Type=1
Class=Setup
ControlCount=7
Control1=IDOK,button,1342242817
Control2=IDCANCEL,button,1342242816
Control3=IDC_COMPORT,combobox,1344340227
Control4=IDC_STATIC,static,1342308352
Control5=IDC_STATIC,static,1342308352
Control6=IDC_MAX_POS,edit,1350631552
Control7=IDC_STATIC,static,1342308352

[CLS:Setup]
Type=0
HeaderFile=Setup.h
ImplementationFile=Setup.cpp
BaseClass=CDialog
Filter=D
LastObject=IDC_COMPORT
VirtualFilter=dWC

