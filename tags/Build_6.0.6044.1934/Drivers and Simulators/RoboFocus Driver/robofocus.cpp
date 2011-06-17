// RoboFocus.cpp : Implementation of DLL Exports.


// Note: Proxy/Stub Information
//      To build a separate proxy/stub DLL, 
//      run nmake -f RoboFocusps.mk in the project directory.

#include "stdafx.h"
#include "resource.h"
#include <initguid.h>
#include "RoboFocus.h"

#include "RoboFocus_i.c"
#include "Focuser.h"


CComModule _Module;

BEGIN_OBJECT_MAP(ObjectMap)
OBJECT_ENTRY(CLSID_Focuser, CFocuser)
END_OBJECT_MAP()

class CRoboFocusApp : public CWinApp
{
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CRoboFocusApp)
	public:
    virtual BOOL InitInstance();
    virtual int ExitInstance();
	//}}AFX_VIRTUAL

	//{{AFX_MSG(CRoboFocusApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

BEGIN_MESSAGE_MAP(CRoboFocusApp, CWinApp)
	//{{AFX_MSG_MAP(CRoboFocusApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

CRoboFocusApp theApp;

BOOL CRoboFocusApp::InitInstance()
{
    _Module.Init(ObjectMap, m_hInstance, &LIBID_RoboFocusLib);
    return CWinApp::InitInstance();
}

int CRoboFocusApp::ExitInstance()
{
    _Module.Term();
    return CWinApp::ExitInstance();
}

/////////////////////////////////////////////////////////////////////////////
// Used to determine whether the DLL can be unloaded by OLE

STDAPI DllCanUnloadNow(void)
{
    AFX_MANAGE_STATE(AfxGetStaticModuleState());
    return (AfxDllCanUnloadNow()==S_OK && _Module.GetLockCount()==0) ? S_OK : S_FALSE;
}

/////////////////////////////////////////////////////////////////////////////
// Returns a class factory to create an object of the requested type

STDAPI DllGetClassObject(REFCLSID rclsid, REFIID riid, LPVOID* ppv)
{
    return _Module.GetClassObject(rclsid, riid, ppv);
}

/////////////////////////////////////////////////////////////////////////////
// DllRegisterServer - Adds entries to the system registry

STDAPI DllRegisterServer(void)
{
    // registers object, typelib and all interfaces in typelib
    HRESULT Result = _Module.RegisterServer(TRUE);

	// Add in "pretty name" for focuser
	HKEY hKey = 0;
	long Res = RegOpenKeyEx ( HKEY_LOCAL_MACHINE, "software\\ascom\\focuser drivers\\RoboFocus.Focuser", 0, KEY_READ|KEY_WRITE|KEY_QUERY_VALUE, &hKey);
	if (Res != 0) return Result;
	
	Res = RegSetValueEx ( hKey, 0, 0, REG_SZ, (unsigned char *) "RoboFocus", sizeof ( "RoboFocus" ) );

	return Result;
}

/////////////////////////////////////////////////////////////////////////////
// DllUnregisterServer - Removes entries from the system registry

STDAPI DllUnregisterServer(void)
{
    return _Module.UnregisterServer(TRUE);
}


