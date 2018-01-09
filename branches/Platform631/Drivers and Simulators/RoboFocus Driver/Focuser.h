// Focuser.h : Declaration of the CFocuser

#ifndef __FOCUSER_H_
#define __FOCUSER_H_

#include "resource.h"       // main symbols

class ComPort;

/////////////////////////////////////////////////////////////////////////////
// CFocuser
class ATL_NO_VTABLE CFocuser : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CFocuser, &CLSID_Focuser>,
	public ISupportErrorInfo,
	public IDispatchImpl<IFocuser, &IID_IFocuser, &LIBID_RoboFocusLib>
{
public:
	CFocuser()
	{
		bLinkEstablished = FALSE;
		Port = NULL;
		bMoving = FALSE;
		bTempCompMode = FALSE;
	}
	~CFocuser()
	{
		if (Port != NULL) put_Link ( FALSE );
	}

DECLARE_REGISTRY_RESOURCEID(IDR_FOCUSER)

DECLARE_PROTECT_FINAL_CONSTRUCT()

BEGIN_COM_MAP(CFocuser)
	COM_INTERFACE_ENTRY(IFocuser)
	COM_INTERFACE_ENTRY(IDispatch)
	COM_INTERFACE_ENTRY(ISupportErrorInfo)
END_COM_MAP()

// ISupportsErrorInfo
	STDMETHOD(InterfaceSupportsErrorInfo)(REFIID riid);

protected:
	bool ChecksumOk ( unsigned char *Msg );
	void CalcChecksum ( unsigned char * Msg );
	bool SetPosition ( long Delta );
	bool GetPosition ( long &Position );
	BOOL bLinkEstablished;
	BOOL bMoving;
	int Timeout;
	ComPort *Port;
	BOOL bTempCompMode;

// IFocuser
public:
	STDMETHOD(Halt)();
	STDMETHOD(Stop)();
	STDMETHOD(get_MaxIncrement)(/*[out, retval]*/ long *pVal);
	STDMETHOD(get_Temperature)(/*[out, retval]*/ float *pVal);
	STDMETHOD(get_TempComp)(/*[out, retval]*/ VARIANT_BOOL *pVal);
	STDMETHOD(put_TempComp)(/*[in]*/ VARIANT_BOOL newVal);
	STDMETHOD(get_TempCompAvailable)(/*[out, retval]*/ VARIANT_BOOL *pVal);
	STDMETHOD(get_Absolute)(/*[out, retval]*/ VARIANT_BOOL *pVal);
	STDMETHOD(get_Link)(/*[out, retval]*/ VARIANT_BOOL *pVal);
	STDMETHOD(put_Link)(/*[in]*/ VARIANT_BOOL newVal);
	STDMETHOD(get_Position)(/*[out, retval]*/ long *pVal);
	STDMETHOD(Move)(long Position);
	STDMETHOD(get_IsMoving)(/*[out, retval]*/ VARIANT_BOOL *pVal);
	STDMETHOD(get_StepSize)(/*[out, retval]*/ float *pVal);
	STDMETHOD(get_MaxStep)(/*[out, retval]*/ long *pVal);
	STDMETHOD(SetupDialog)();
};

#endif //__FOCUSER_H_
