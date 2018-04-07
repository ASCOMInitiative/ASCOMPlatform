#if !defined(AFX_SETUP_H__5085CBE5_95C0_45C5_A496_18E0BD1DDD08__INCLUDED_)
#define AFX_SETUP_H__5085CBE5_95C0_45C5_A496_18E0BD1DDD08__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Setup.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// Setup dialog

class Setup : public CDialog
{
// Construction
public:
	Setup(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(Setup)
	enum { IDD = IDD_SETUP };
	CComboBox	m_CtrlCOMPort;
	int		m_COMPort;
	int		m_MaxPos;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(Setup)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(Setup)
	virtual void OnOK();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SETUP_H__5085CBE5_95C0_45C5_A496_18E0BD1DDD08__INCLUDED_)
