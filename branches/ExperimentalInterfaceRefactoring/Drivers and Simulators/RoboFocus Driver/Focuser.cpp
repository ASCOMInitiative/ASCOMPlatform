//---------------------------------------------------------------------
// Copyright © 2000 Diffraction Limited, Ottawa, ON K2G 5W3, Canada
//
// Permission is hereby granted to use this Software for any purpose
// including combining with commercial products, creating derivative
// works, and redistribution of source or binary code, without
// limitation or consideration. Any redistributed copies of this
// Software must include the above Copyright Notice.
//
// THIS SOFTWARE IS PROVIDED "AS IS". DIFFRACTION LIMITED MAKES NO
// WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
// SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
//---------------------------------------------------------------------
//
// Written:  01-Sep-01   Douglas B. George <dgeorge@cyanogen.com>
//
// Edits:
//
// When      Who     What
// --------- ---     --------------------------------------------------
// 01-Sep-01 dbg     Initial edit
// 04-Sep-02 dbg     Update header for ASCOM release
// 28-Mar-09 dbg     Increase number of available COM ports
//
//

#include "stdafx.h"
#include "RoboFocus.h"
#include "Focuser.h"
#include "Setup.h"
#include "ComPort.h"

/////////////////////////////////////////////////////////////////////////////
// CFocuser

int MaxPosition = 65535;

STDMETHODIMP CFocuser::InterfaceSupportsErrorInfo(REFIID riid)
{
	static const IID* arr[] = 
	{
		&IID_IFocuser
	};
	for (int i=0; i < sizeof(arr) / sizeof(arr[0]); i++)
	{
		if (InlineIsEqualGUID(*arr[i],riid))
			return S_OK;
	}
	return S_FALSE;
}

STDMETHODIMP CFocuser::SetupDialog()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	Setup SetupDlg;

	SetupDlg.DoModal();

	return S_OK;
}

STDMETHODIMP CFocuser::get_StepSize(float *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	return Error ( "Not supported by focuser", IID_IFocuser );
}

STDMETHODIMP CFocuser::get_MaxStep(long *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	*pVal = MaxPosition;

	return S_OK;
}

STDMETHODIMP CFocuser::get_IsMoving(VARIANT_BOOL *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

		// Default to not moving
	*pVal = VARIANT_FALSE;

	// Check whether a command has gone out
	if (!bMoving)
	{
		return S_OK;
	}

	unsigned char Char;
	while (Port->ReadPortNoWaiting ( Char ))
	{
		if (Char == 'I' || Char == 'O')
		{
			// Still moving; reset timeout
			*pVal = VARIANT_TRUE;
			Timeout = clock() + CLOCKS_PER_SEC;
			continue;
		}
		// If was an end command...
		else if (Char == 'F') 
		{
			// We won't bother checking the message
			unsigned char Buf[ 10 ];
			Port->ReadPort ( Buf, 8 );
			*pVal = VARIANT_FALSE;
			bMoving = FALSE;
			return S_OK;
		}
		
		// We'll assume we just got a bad character and keep going
	}

	// If we got told that we're still waiting; great
	if (*pVal == VARIANT_TRUE) return S_OK;

	// We didn't get a character; check for timeout
	if (clock() < Timeout) 
	{
		// Assume still moving
		*pVal = VARIANT_TRUE;
		return S_OK;
	}
	else
	{
		// Whoops -- didn't get response
		*pVal = VARIANT_FALSE;
		bMoving = FALSE;
		return Error ( "Link fail", IID_IFocuser );
	}

}

STDMETHODIMP CFocuser::Move(long Position)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	if (bTempCompMode) return Error ( "Temperature Compensation On", IID_IFocuser );

	VARIANT_BOOL Moving; // Dummy; sets bMoving
	get_IsMoving ( &Moving );
	if (bMoving) return Error ( "Focuser already in motion", IID_IFocuser );

	if (Position < 0 || Position > MaxPosition) return Error ( "Value out of range", IID_IFocuser );

	long CurrentPos;
	if (!GetPosition( CurrentPos )) return Error ( "Link fail", IID_IFocuser );

	if (!SetPosition ( Position - CurrentPos )) return Error ( "Link fail", IID_IFocuser );

	return S_OK;
}


STDMETHODIMP CFocuser::Stop()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	if (Port == NULL) return false;

	CString St;
	St = "FVXXXXXX";
	CalcChecksum ( (unsigned char *) (LPCSTR) St );

	Port->PurgePort();
	Port->WritePort ( (unsigned char *) (LPCSTR) St, St.GetLength() );
	return S_OK;
}

STDMETHODIMP CFocuser::get_Position(long *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	long Position;
	if (!GetPosition( Position )) return Error ( "Link fail", IID_IFocuser );

	*pVal = Position;
	return S_OK;
}

STDMETHODIMP CFocuser::get_Link(VARIANT_BOOL *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	*pVal = bLinkEstablished ? VARIANT_TRUE : VARIANT_FALSE;

	return S_OK;
}

STDMETHODIMP CFocuser::put_Link(VARIANT_BOOL newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	// Check whether state is actually changing
	if (bLinkEstablished == newVal) return S_OK;

	if (newVal)
	{
		// Reinitialize all variables
		Port = NULL;
		bMoving = FALSE;
		bTempCompMode = FALSE;

		// Initialize link
		Setup SetupDlg;
		MaxPosition = SetupDlg.m_MaxPos;
		CString COMPort;
		COMPort.Format ( "//./COM%i", SetupDlg.m_COMPort + 1 );
		Port = new ComPort;
		if (Port == NULL) return Error ( "Out of memory", IID_IFocuser );
		if (!Port->OpenPort ( COMPort, 9600 ))
		{
			delete Port;
			Port = NULL;
			CString St;
			St.Format ( "Could not open port %s", COMPort );
			return Error ( St, IID_IFocuser );
		}

		// Try to find the focuser
		for (int I = 0; I < 10; I++)
		{
			Port->PurgePort();
			unsigned char Msg[] = "FV000000x";
			CalcChecksum ( Msg );
			Port->WritePort ( Msg, 9 );
			unsigned char Buf[ 10 ];
			BOOL Ok = Port->ReadPort ( Buf, 9 );
			if (Ok && ChecksumOk ( Buf )) 
			{
				bLinkEstablished = TRUE;
				return S_OK;
			}
		}
		Port->ClosePort();
		delete Port;
		Port = NULL;
		return Error ( "Focuser did not respond", IID_IFocuser );
	}
	else
	{
		if (Port != NULL) 
		{
			// Shutdown link
			Port->ClosePort();
		}

		delete Port;
		Port = NULL;
		bLinkEstablished = FALSE;
		return S_OK;
	}
}

STDMETHODIMP CFocuser::get_Absolute(VARIANT_BOOL *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	*pVal = VARIANT_TRUE;

	return S_OK;
}

bool CFocuser::GetPosition(long &Position)
{
	static LastPosition = 0;

	if (Port == NULL) return false;

	if (bMoving && !bTempCompMode)
	{
		VARIANT_BOOL Dummy;
		get_IsMoving(&Dummy);
	}
	if (bMoving || bTempCompMode)
	{
		Position = LastPosition;
		return true;
	}

	// Try three times
	for (int I = 0; I < 3; I++)
	{
		Port->PurgePort();
		unsigned char Msg[] = "FG000000x";
		CalcChecksum ( Msg );
		Port->WritePort ( Msg, 9 );
		unsigned char Buf[ 10 ];
		BOOL Ok = Port->ReadPort ( Buf, 9 );
		if (Ok && ChecksumOk ( Buf )) 
		{
			if (Buf[0] != 'F') continue;
			if (Buf[1] != 'D') continue;
			Buf[8] = 0;
			char *Ptr = (char *) &Buf[2];
			Position = atoi ( Ptr );
			LastPosition = Position;
			return true;
		}
	}

	return false;
}

bool CFocuser::SetPosition(long Delta)
{
	if (Port == NULL) return false;

	CString St;
	if (Delta >= 0)
	{
		St.Format ( "FO%06ix", Delta );
	}
	else
	{
		St.Format ( "FI%06ix", -Delta );
	}

	CalcChecksum ( (unsigned char *) (LPCSTR) St );

	Port->PurgePort();
	Port->WritePort ( (unsigned char *) (LPCSTR) St, St.GetLength() );

	bMoving = TRUE;
	Timeout = clock() + 1 * CLOCKS_PER_SEC;
	return true;
}


STDMETHODIMP CFocuser::get_TempCompAvailable(VARIANT_BOOL *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	*pVal = VARIANT_FALSE;

	return S_OK;
}

STDMETHODIMP CFocuser::get_TempComp(VARIANT_BOOL *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	*pVal = VARIANT_FALSE;

	return S_OK;
}

STDMETHODIMP CFocuser::put_TempComp(VARIANT_BOOL newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	return Error ( "Not Supported", IID_IFocuser );
}

STDMETHODIMP CFocuser::get_Temperature(float *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	// Not available with this focuser
	*pVal = 100.0f;
	return S_OK;
}

STDMETHODIMP CFocuser::get_MaxIncrement(long *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	*pVal = MaxPosition;

	return S_OK;
}

// Length of string must be 9 minimum
void CFocuser::CalcChecksum(unsigned char *Msg)
{
	unsigned char Sum = 0;
	for (int I = 0; I < 8; I++)
	{
		Sum += Msg[ I ];
	}
	Msg[ I ] = Sum;
}

// Length of string must be 9 minimum
bool CFocuser::ChecksumOk(unsigned char *Msg)
{
	unsigned char Sum = 0;
	for (int I = 0; I < 8; I++)
	{
		Sum += Msg[ I ];
	}
	return Msg[ I ] == Sum;
}

STDMETHODIMP CFocuser::Halt()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	if (Port == NULL) return false;

	CString St;
	St = "FVXXXXXX";
	CalcChecksum ( (unsigned char *) (LPCSTR) St );

	Port->PurgePort();
	Port->WritePort ( (unsigned char *) (LPCSTR) St, St.GetLength() );
	return S_OK;
}
