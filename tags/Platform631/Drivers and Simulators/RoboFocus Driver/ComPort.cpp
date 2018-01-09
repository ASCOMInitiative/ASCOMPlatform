//---------------------------------------------------------------------
// Copyright © 1998 Diffraction Limited, Ottawa, ON K2G 5W3, Canada
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
// Purpose:   Simple serial port class wrapper 
//
// Written:  01-Jan-98   Douglas B. George <dgeorge@cyanogen.com>
//
// Edits:
//
// When      Who     What
// --------- ---     --------------------------------------------------
// 01-Jan-98 dbg     Initial edit
// 04-Sep-02 dbg     Update header for ASCOM release
// 05-Jan-04 rbd     Per dbg, add extended port syntax in OpenPort()
//

#include "stdafx.h"
#include "ComPort.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//-------------------------------------------------------------------------------
//
//	Name:		Constructor
//	Purpose:	Creates object
//
//-------------------------------------------------------------------------------
ComPort::ComPort()
{
	hCom = INVALID_HANDLE_VALUE;		// Handle to com device
}

//-------------------------------------------------------------------------------
//
//	Name:		Destructor
//	Purpose:	Deletes object
//
//-------------------------------------------------------------------------------
ComPort::~ComPort()
{
	ClosePort();
}

//-------------------------------------------------------------------------------
//
//	Name:		OpenPort
//	Purpose:	Opens a COM port
//
//-------------------------------------------------------------------------------
BOOL ComPort::OpenPort ( CString COM, int Baud, BOOL EvenParity /*=FALSE*/ )
{
	if (COM[0] != '/') COM = "//./" + COM;								// Use extended port name for high port numbers

	// Access the port
	hCom = CreateFile ( COM, GENERIC_READ | GENERIC_WRITE, 0, NULL, OPEN_EXISTING, 0, NULL );
	if (hCom == INVALID_HANDLE_VALUE) return FALSE;

	// Get a device record
	if (GetCommState(hCom, &dcb) == 0) 
	{
		CloseHandle ( hCom );
		return FALSE;
	}

	// Set comport timeouts
	COMMTIMEOUTS Timeouts;
	Timeouts.ReadIntervalTimeout = ReadIntervalTimeout;					// milliseconds between receive chars
	Timeouts.ReadTotalTimeoutMultiplier = ReadTotalTimeoutMultiplier;	// milliseconds per character (min 9600 baud)
	Timeouts.ReadTotalTimeoutConstant = ReadTotalTimeoutConstant;		// additional milliseconds per message
	Timeouts.WriteTotalTimeoutMultiplier = WriteTotalTimeoutMultiplier; // no write timeouts
	Timeouts.WriteTotalTimeoutConstant = WriteTotalTimeoutConstant;
	if (!SetCommTimeouts ( hCom, &Timeouts ))
	{
		CloseHandle ( hCom );
		return FALSE;
	}


	// Set default baud rate
	dcb.BaudRate = Baud;
	dcb.ByteSize = 8;
	dcb.Parity = EvenParity ? EVENPARITY : NOPARITY;
	dcb.StopBits = ONESTOPBIT;
	
	return SetCommState(hCom, &dcb) != 0;
}

//-------------------------------------------------------------------------------
//
//	Name:		SetPortWriteTimeouts
//	Purpose:	Sets up the port write timeouts
//
//-------------------------------------------------------------------------------
BOOL ComPort::SetPortWriteTimeouts( int TotalTimeoutConstant, int TotalTimeoutMultiplier )
{
	// Set comport timeouts
	COMMTIMEOUTS Timeouts;
	Timeouts.ReadIntervalTimeout = ReadIntervalTimeout;					// milliseconds between receive chars
	Timeouts.ReadTotalTimeoutMultiplier = ReadTotalTimeoutMultiplier;	// milliseconds per character (min 9600 baud)
	Timeouts.ReadTotalTimeoutConstant = ReadTotalTimeoutConstant;		// additional milliseconds per message
	Timeouts.WriteTotalTimeoutMultiplier = TotalTimeoutMultiplier;
	Timeouts.WriteTotalTimeoutConstant = TotalTimeoutConstant;
	return SetCommTimeouts ( hCom, &Timeouts );
}

//-------------------------------------------------------------------------------
//
//	Name:		SetPortParams
//	Purpose:	Sets up the port parameters
//
//-------------------------------------------------------------------------------
BOOL ComPort::SetPortParams ( int Baud )
{
	// Set new baud rate
	dcb.BaudRate = Baud;
	
	return SetCommState(hCom, &dcb) == 0;
}

//-------------------------------------------------------------------------------
//
//	Name:		ClosePort
//	Purpose:	Closes the com port
//
//-------------------------------------------------------------------------------
void ComPort::ClosePort()
{
	if (hCom == INVALID_HANDLE_VALUE) return;
	CloseHandle ( hCom );
	hCom = INVALID_HANDLE_VALUE;
}

//-------------------------------------------------------------------------------
//
//	Name:		WritePort
//	Purpose:	Writes to the COM port
//
//-------------------------------------------------------------------------------
BOOL ComPort::WritePort ( unsigned char *Buf, int NumToWrite )
{
	DWORD NumWritten;
	BOOL Ok = WriteFile( hCom, Buf, NumToWrite, &NumWritten, NULL );
	Ok &= NumToWrite == int ( NumWritten );
	return Ok;
}

//-------------------------------------------------------------------------------
//
//	Name:		ReadPort
//	Purpose:	Writes to the COM port
//
//-------------------------------------------------------------------------------
BOOL ComPort::ReadPort ( unsigned char *Buf, int NumToRead )
{
	DWORD NumRead;
	BOOL Ok = ReadFile ( hCom, Buf, NumToRead, &NumRead, NULL );
	Ok &= int ( NumRead ) == NumToRead;
	return Ok;
}

//-------------------------------------------------------------------------------
//
//	Name:		PurgePort
//	Purpose:	Purges the transmit and receive buffers
//
//-------------------------------------------------------------------------------
BOOL ComPort::PurgePort()
{
	return PurgeComm ( hCom, PURGE_TXCLEAR | PURGE_RXCLEAR | PURGE_RXABORT );
}

//-------------------------------------------------------------------------------
//
//	Name:		PurgeTx
//	Purpose:	Purges the transmit buffer only
//
//-------------------------------------------------------------------------------
BOOL ComPort::PurgeTx()
{
	return PurgeComm ( hCom, PURGE_TXCLEAR );
}

//-------------------------------------------------------------------------------
//
//	Name:		PurgeRx
//	Purpose:	Purges the receive buffer only
//
//-------------------------------------------------------------------------------
BOOL ComPort::PurgeRx()
{
	return PurgeComm ( hCom, PURGE_RXCLEAR | PURGE_RXABORT );
}

//-------------------------------------------------------------------------------
//
//	Name:		ReadPortNoWaiting
//	Purpose:	Checks if character available and reads if so
//
//-------------------------------------------------------------------------------
BOOL ComPort::ReadPortNoWaiting ( unsigned char &Char )
{
	// Change timeouts
	COMMTIMEOUTS Timeouts;
	Timeouts.ReadIntervalTimeout = 1;			// Minimal timeouts
	Timeouts.ReadTotalTimeoutMultiplier = 1;
	Timeouts.ReadTotalTimeoutConstant = 1;
	Timeouts.WriteTotalTimeoutMultiplier = WriteTotalTimeoutMultiplier; // Don't change write
	Timeouts.WriteTotalTimeoutConstant = WriteTotalTimeoutConstant;
	if (!SetCommTimeouts ( hCom, &Timeouts )) return FALSE;

	BOOL CharReceived = ReadPort ( &Char, 1 );

	// Restore timeouts
	Timeouts.ReadIntervalTimeout = ReadIntervalTimeout;					// milliseconds between receive chars
	Timeouts.ReadTotalTimeoutMultiplier = ReadTotalTimeoutMultiplier;	// milliseconds per character (min 9600 baud)
	Timeouts.ReadTotalTimeoutConstant = ReadTotalTimeoutConstant;		// additional milliseconds per message
	Timeouts.WriteTotalTimeoutMultiplier = WriteTotalTimeoutMultiplier; // no write timeouts
	Timeouts.WriteTotalTimeoutConstant = WriteTotalTimeoutConstant;
	SetCommTimeouts ( hCom, &Timeouts );

	return CharReceived;
}

//-------------------------------------------------------------------------------
//
//	Name:		SetCTSFlowControl
//	Purpose:	Turns on/off CTS flow control
//  WARNING:    Turn on write timeouts if you are going to use this!
//
//-------------------------------------------------------------------------------
void ComPort::SetCTSFlowControl(BOOL CTSOn)
{
	// Set CTS state
	dcb.fOutxCtsFlow = CTSOn;
	
	SetCommState(hCom, &dcb);
}

//-------------------------------------------------------------------------------
//
//	Name:		SetTimeouts
//	Purpose:	Changes com port timeout settings
//	Notes:		Set to -1 to skip
//
//-------------------------------------------------------------------------------
void ComPort::SetTimeouts(int NewReadIntervalTimeout, int NewReadTotalTimeoutMultiplier, int NewReadTotalTimeoutConstant, 
						  int NewWriteTotalTimeoutMultiplier, int NewWriteTotalTimeoutConstant )
{
	// Get current timeouts
	COMMTIMEOUTS Timeouts;
	if (!GetCommTimeouts ( hCom, &Timeouts ))
	{
		// Oops -- Set defaults
		Timeouts.ReadIntervalTimeout = ReadIntervalTimeout;					// milliseconds between receive chars
		Timeouts.ReadTotalTimeoutMultiplier = ReadTotalTimeoutMultiplier;	// milliseconds per character (min 9600 baud)
		Timeouts.ReadTotalTimeoutConstant = ReadTotalTimeoutConstant;		// additional milliseconds per message
		Timeouts.WriteTotalTimeoutMultiplier = WriteTotalTimeoutMultiplier; // no write timeouts
		Timeouts.WriteTotalTimeoutConstant = WriteTotalTimeoutConstant;
	}
 
	if (NewReadIntervalTimeout >= 0) Timeouts.ReadIntervalTimeout = NewReadIntervalTimeout;					// milliseconds between receive chars
	if (NewReadTotalTimeoutMultiplier >= 0) Timeouts.ReadTotalTimeoutMultiplier = NewReadTotalTimeoutMultiplier;	// milliseconds per character (min 9600 baud)
	if (NewReadTotalTimeoutConstant >= 0) Timeouts.ReadTotalTimeoutConstant = NewReadTotalTimeoutConstant;		// additional milliseconds per message
	if (NewWriteTotalTimeoutMultiplier >= 0) Timeouts.WriteTotalTimeoutMultiplier = NewWriteTotalTimeoutMultiplier; // no write timeouts
	if (NewWriteTotalTimeoutConstant >= 0) Timeouts.WriteTotalTimeoutConstant = NewWriteTotalTimeoutConstant;

	SetCommTimeouts ( hCom, &Timeouts );
}

