// Setup.cpp : implementation file
//

#include "stdafx.h"
#include "resource.h"
#include "Setup.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// Setup dialog

const char * ROOT_KEY = "Software\\ASCOM\\Focuser Drivers\\RoboFocus.Focuser";


Setup::Setup(CWnd* pParent /*=NULL*/)
	: CDialog(Setup::IDD, pParent)
{
	//{{AFX_DATA_INIT(Setup)
	m_COMPort = -1;
	m_MaxPos = 0;
	//}}AFX_DATA_INIT
	
	// Defaults
	m_COMPort = 0;
	m_MaxPos = 10000;
	
	HKEY Key;
	DWORD lRes;
    LONG lStat = RegCreateKeyEx(HKEY_LOCAL_MACHINE, ROOT_KEY, 0, 0, 0, KEY_READ | KEY_WRITE | KEY_CREATE_SUB_KEY, 0, &Key, &lRes);
	if (lStat == ERROR_SUCCESS) 
	{
		
		char Data[ 256 ];
		strnset ( Data, 0, sizeof ( Data ) );
		DWORD Len = sizeof ( Data );
		lStat = RegQueryValueEx ( Key, "COM", 0, 0, (unsigned char *) Data, &Len);
		
		char MaxPos[ 256 ];
		strnset ( MaxPos, 0, sizeof ( MaxPos ) );
		Len = sizeof ( MaxPos );
		lStat = RegQueryValueEx ( Key, "MaxPos", 0, 0, (unsigned char *) MaxPos, &Len);
		RegCloseKey ( Key );
		
		m_COMPort = atoi ( Data );
		m_MaxPos = atoi ( MaxPos );
	}
	
	if (m_COMPort < 0) m_COMPort = 0;
	if (m_COMPort > 63) m_COMPort = 63;
	
	if (m_MaxPos == 0) m_MaxPos = 10000;  // Default
	if (m_MaxPos < 10) m_MaxPos = 10;
	if (m_MaxPos > 100000) m_MaxPos = 100000;
}


void Setup::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(Setup)
	DDX_Control(pDX, IDC_COMPORT, m_CtrlCOMPort);
	DDX_CBIndex(pDX, IDC_COMPORT, m_COMPort);
	DDX_Text(pDX, IDC_MAX_POS, m_MaxPos);
	DDV_MinMaxInt(pDX, m_MaxPos, 10, 100000);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(Setup, CDialog)
	//{{AFX_MSG_MAP(Setup)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// Setup message handlers

void Setup::OnOK() 
{
	// Save default
	if (!UpdateData()) return;

	HKEY Key;
	DWORD lRes;
    LONG lStat = RegCreateKeyEx(HKEY_LOCAL_MACHINE, ROOT_KEY, 0, 0, 0, KEY_READ | KEY_WRITE | KEY_CREATE_SUB_KEY, 0, &Key, &lRes);
	if (lStat != ERROR_SUCCESS) return;

	// Format a string
	CString St;
	St.Format ( "%i", m_COMPort );
    lStat = RegSetValueEx( Key, "COM", 0, REG_SZ, (unsigned char *) (LPCSTR) St, St.GetLength());

	St.Format ( "%i", m_MaxPos );
    lStat = RegSetValueEx( Key, "MaxPos", 0, REG_SZ, (unsigned char *) (LPCSTR) St, St.GetLength());

	RegCloseKey ( Key );
	
	CDialog::OnOK();
}
