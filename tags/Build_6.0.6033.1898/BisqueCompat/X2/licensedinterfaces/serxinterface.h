#ifndef _SerXInterface_
#define _SerXInterface_

#define SerXInterface_Name "com.bisque.TheSkyX.staticlibs.serx.SerXInterface/1.0"

/*!   
\brief The SerXInterface is a cross-plaform serial port.

\ingroup Tool

The SerXInterface represents a cross-platform serial port interface passed to X2 implementors.
It provides X2 implementors an operating system agnostic way of using a serial port to hopefully make it easy to write X2 drivers for serial devices.
Tested and works on Windows, Mac, Ubuntu Linux.
Copyright (c) 2005 Software Bisque 
*/

class SerXInterface
{
public:
	SerXInterface(){setAbortTimeout(false);}
	virtual ~SerXInterface(){}

	/*! Parity */
	enum Parity  
	{
		B_NOPARITY,		/**<  No parity */
		B_ODDPARITY,	/**<  Odd parity */
		B_EVENPARITY,	/**<  Even parity */
		B_MARKPARITY,	/**<  Mark parity */
		B_SPACEPARITY	/**<  Space parity */
	};

public:
	/*! Open the port. 
	\param pszPort is a string specifiing the name of the port to open.
	\param dwBaudRate is optional baud rate that defaults to 9600.
	\param parity is the optional parity that defaults to no parity.
	\param pszSession can be used to set the data bits to something other than the default, 8 data bits. This is new to TheSkyX 10.1.11 (technically, build 4635 and later). 
	For example, if pszSession = "-Databits 7", data bits will be set to 7 on the serial port.
	*/
	virtual int open(const char          * pszPort, 
                   const unsigned long & dwBaudRate = 9600, 
                   const Parity        & parity = B_NOPARITY, 
                   const char          * pszSession = 0) = 0;

	/*! Close the port.*/
	virtual int close() = 0;

	/*! Returns non zero if the port is connected (open) or zero if not connected. */
	virtual bool isConnected(void) const = 0;

	/*! Force the OS to push the transmit packet out the port
	in case the operating system buffer's writes.*/
	virtual int flushTx(void) = 0;

	/*! Purge both send and receive queues.*/
	virtual int purgeTxRx(void) = 0;

	/*! Wait for nNumber of bytes to appear in the receive port or timeout.*/
	virtual int waitForBytesRx(const int& nNumber, 
                             const int& nTimeOutMilli) = 0;

	/*! Read dwTot bytes from the receive port, or timeout.*/
	virtual int readFile(void* lpBuf, 
                       const unsigned long dwTot, 
                       unsigned long& Red, 
								       const unsigned long& dwTimeOut = 1000) = 0;

	/*! Write dwTot bytes out the transmit port. */
	virtual int writeFile(void* lpBuf,
                        const unsigned long& dwTot, 
                        unsigned long& Rote) = 0;

	/*! Returns the number bytes in the receive port. */
	virtual int bytesWaitingRx(int &nBytesWaiting) = 0;

	/*!   
	Software Bisque only.  For operations that may time out (WaitForBytesRx and ReadFile)
	calling abortTimeout will cause these operations to quickly return ERR_ABORTEDPROCESS
	instead of having to wait for them to time out.
	Implementation of timeout operations intially set this flag to false so clients don't have that responsibility
	*/
	virtual void abortTimeout(){setAbortTimeout(true);}
	/*!   
	Software Bisque only.  
	*/
	virtual bool didAbortTimeout() const {return m_bAbortTimeout;}
	/*!   
	Software Bisque only.  
	*/
	virtual void setAbortTimeout(const bool& bYes) {m_bAbortTimeout=bYes;}

private:
	bool m_bAbortTimeout;
};

#endif // _SerXInterface_

//Linux wants a 'newline' at the end of every source file - don't delete the one after this line