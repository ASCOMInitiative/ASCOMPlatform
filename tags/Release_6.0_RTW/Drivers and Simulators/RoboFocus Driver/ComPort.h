const int ReadIntervalTimeout = 200;			// milliseconds between receive chars
const int ReadTotalTimeoutMultiplier = 4;		// milliseconds per character (min 9600 baud)
const int ReadTotalTimeoutConstant = 100;		// additional milliseconds per message
const int WriteTotalTimeoutMultiplier = 0;		// no write timeouts
const int WriteTotalTimeoutConstant = 0;

class ComPort
{

// Construction
public:
	ComPort();
	~ComPort();

// Implementation
protected:
	HANDLE hCom;		// Handle to com device
	DCB dcb;			// Device control block

public:
	BOOL ReadPortNoWaiting ( unsigned char &Char );
	BOOL OpenPort ( CString COM, int Baud, BOOL EvenParity = FALSE );
	BOOL SetPortParams ( int Baud );
	BOOL SetPortWriteTimeouts( int TotalTimeoutConstant, int TotalTimeoutMultiplier = WriteTotalTimeoutMultiplier );
	void ClosePort();
	BOOL WritePort ( unsigned char *Buf, int NumToWrite );
	BOOL ReadPort ( unsigned char *Buf, int NumToRead );
	BOOL PurgePort();
	BOOL PurgeTx();
	BOOL PurgeRx();
	void SetCTSFlowControl(BOOL CTSOn);
	void SetTimeouts(int NewReadIntervalTimeout, int NewReadTotalTimeoutMultiplier, int NewReadTotalTimeoutConstant, 
						  int NewWriteTotalTimeoutMultiplier, int NewWriteTotalTimeoutConstant );
};