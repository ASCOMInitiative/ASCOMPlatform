#include <math.h>
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <time.h>
#include "ASCOM.Rotator.h"
#include "../../licensedinterfaces/sberrorx.h"

#include "../../licensedinterfaces/serxinterface.h"
#include "../../licensedinterfaces/theskyxfacadefordriversinterface.h"
#include "../../licensedinterfaces/sleeperinterface.h"
#include "../../licensedinterfaces/loggerinterface.h"
#include "../../licensedinterfaces/basiciniutilinterface.h"
#include "../../licensedinterfaces/mutexinterface.h"
#include "../../licensedinterfaces/basicstringinterface.h"
#include "../../licensedinterfaces/tickcountinterface.h"
#include "../../licensedinterfaces/x2guiinterface.h"

#if defined(SB_WIN_BUILD)
	#define DEF_PORT_NAME					"COM1"
#elif defined(SB_MAC_BUILD)
	#define DEF_PORT_NAME					"/dev/cu.KeySerial1"
#elif defined(SB_LINUX_BUILD)
	#define DEF_PORT_NAME					"/dev/COM0"
#endif

#define LOCK_IO_PORT X2MutexLocker Lock(m_pIOMutex); 

#define TCFSSLEEP(ms)  if (m_pSleeper) m_pSleeper->sleep(ms)


X2Rotator::X2Rotator(const char* pszDriverSelection,
						const int& nInstanceIndex,
						SerXInterface					* pSerX, 
						TheSkyXFacadeForDriversInterface	* pTheSkyX, 
						SleeperInterface					* pSleeper,
						BasicIniUtilInterface			* pIniUtil,
						LoggerInterface					* pLogger,
						MutexInterface					* pIOMutex,
						TickCountInterface				* pTickCount)
{
	m_nInstanceIndex				= nInstanceIndex;
	m_pSerX							= pSerX;		
	m_pTheSkyXForMounts				= pTheSkyX;
	m_pSleeper						= pSleeper;
	m_pIniUtil						= pIniUtil;
	m_pLogger						= pLogger;	
	m_pIOMutex						= pIOMutex;
	m_pTickCount					= pTickCount;

	m_nInstanceIndex = nInstanceIndex;
	m_bLinked = false;
	m_dPosition = 0;
	m_bDoingGoto = false;
	m_dTargetPosition = 0;
	m_nGotoStartStamp = 0;

	char m_pszDriverSelection[DRIVER_MAX_STRING];
	sprintf(m_pszDriverSelection, pszDriverSelection);
	
}

X2Rotator::~X2Rotator()
{
	if (GetSerX())
		delete GetSerX();
	if (GetTheSkyXFacadeForDrivers())
		delete GetTheSkyXFacadeForDrivers();
	if (GetSleeper())
		delete GetSleeper();
	if (GetSimpleIniUtil())
		delete GetSimpleIniUtil();
	if (GetLogger())
		delete GetLogger();
	if (GetMutex())
		delete GetMutex();

}

int	X2Rotator::queryAbstraction(const char* pszName, void** ppVal) 
{
	*ppVal = NULL;

	if (!strcmp(pszName, SerialPortParams2Interface_Name))
		*ppVal = dynamic_cast<SerialPortParams2Interface*>(this);
	else
		if (!strcmp(pszName, ModalSettingsDialogInterface_Name))
			*ppVal = dynamic_cast<ModalSettingsDialogInterface*>(this);
	

	return SB_OK;
}

/********************************************** TCFS Sample Code**********************************************************************************/
#define UNKNOWNPOSITION -1

int X2Rotator::tcfsPositionCore(int* plPos)
{
	if (m_bDoingGoto)
	{
		*plPos = (long)m_dPosition;
		return SB_OK;
	}

	LOCK_IO_PORT 
	int nErr = SB_OK;
	char buf[255];
	unsigned long rote=0;

	int nCount = 0;
	int nTimes = 3;
	int nDelay = 100;
	
	*plPos = UNKNOWNPOSITION;

	do
	{
		if (GetSerX())
			GetSerX()->purgeTxRx();

		sprintf(buf,"FPOSRO");
		if (nErr = m_pSerX->writeFile(&buf, strlen(buf), rote))
			goto exit;

		if (GetSerX())
			GetSerX()->flushTx();

		if (nCount)
			TCFSSLEEP(nDelay);

		memset(buf,0,255);
		rote = 0;
		if (nErr = m_pSerX->readFile(&buf, 8, rote))
			goto exit;

		if (buf[0] == 'P'  &&
			buf[1] == '=')
		{
			*plPos = atoi(&buf[2]);
			nErr = SB_OK;
			goto exit;
		}

		++nCount;

	}while (!nErr && nCount < nTimes);

	if (nCount >= nTimes)
	{
		nErr = ERR_NORESPONSE;
		goto exit;
	}

exit:

	if (SB_OK != nErr)
	{
		int j;
		j=1;

	}
	return nErr;
}
int X2Rotator::tcfsSendFMMODE()
{
	int nErr = SB_OK;
	char buf[255];
	unsigned long rote;
	
	//Always send the command 
	//to put it into serial mode
	//in case it lost power
	sprintf(buf,"FMMODE");

	if (nErr = m_pSerX->writeFile(&buf, strlen(buf), rote))
		 goto exit;
	m_pSerX->flushTx();

exit:
	return nErr;
}
int X2Rotator::tcfsGetSerialConnection()
{
	int nErr = SB_OK;
	int nCount = 0;
	char buf[255];
	unsigned long red;
	unsigned long dwInt = 100;

	LOCK_IO_PORT//Block for this entire call

	//Try for one seocond at 100 ms intervals
	int nTimes = 20;

	if (GetSerX())
		GetSerX()->purgeTxRx();//If in A or B, stuff streaming in so we need clear in buffer

	do
	{
		int dwStart = 0;

		if (GetTickCountInterface())
			dwStart = GetTickCountInterface()->elapsed();

		if (nErr = tcfsSendFMMODE())
			goto exit;

		int nWaitErr;
		unsigned long dwDur;

		do
		{
			nWaitErr = m_pSerX->waitForBytesRx(1, 10);

			if (SB_OK == nWaitErr)
			{
				memset(buf,0,255);red = 0;
				m_pSerX->readFile(&buf, 1, red);

				if (buf[0] == '!')//Go it!
				{
					TCFSSLEEP(400);
					nErr = SB_OK;
					goto exit;
				}
			}

			if (GetTickCountInterface())
				dwDur = GetTickCountInterface()->elapsed() - dwStart;

		} while(dwDur<dwInt);

		dwDur = GetTickCountInterface()->elapsed() - dwStart;
	
		if (dwDur<dwInt)
		{
			do
			{
				TCFSSLEEP(1);
				dwDur = 0;
			}while (dwDur<dwInt);
		}
		

		++nCount;

	}while (!nErr && nCount < nTimes);

	if (nCount >= nTimes)
	{
		nErr = ERR_NORESPONSE;
		goto exit;
	}
exit:

	return nErr;
}

int X2Rotator::tcfsTest()
{
	int FocPos = 0;
	int nErr = SB_OK;
	char szPort[DRIVER_MAX_STRING];
	

	portNameOnToCharPtr(szPort,DRIVER_MAX_STRING);

	//See if we are already connected
	if (!GetSerX()->isConnected())
	{
		if (nErr = m_pSerX->open(szPort, 19200))
			goto exit;	
	}

	if (nErr = tcfsGetSerialConnection())
		goto exit;

	if (nErr = tcfsPositionCore(&FocPos))
		goto exit;

exit:

	if (GetSerX())
		if (GetSerX()->isConnected())
			GetSerX()->close();

	return nErr;
}

int	X2Rotator::establishLink(void)						
{
	m_bLinked = true;
	return SB_OK;
}
int	X2Rotator::terminateLink(void)						
{
	m_bLinked = false;
	return SB_OK;
}
bool X2Rotator::isLinked(void) const					
{
	return m_bLinked;
}

void X2Rotator::deviceInfoNameShort(BasicStringInterface& str) const				
{
	str = PLUGIN_DISPLAY_NAME;
}
void X2Rotator::deviceInfoNameLong(BasicStringInterface& str) const				
{
	str = PLUGIN_DISPLAY_NAME;
}
void X2Rotator::deviceInfoDetailedDescription(BasicStringInterface& str) const		
{
	str = PLUGIN_DISPLAY_NAME;
}
void X2Rotator::deviceInfoFirmwareVersion(BasicStringInterface& str)				
{
	str = PLUGIN_DISPLAY_NAME;
}
void X2Rotator::deviceInfoModel(BasicStringInterface& str)							
{
	str = PLUGIN_DISPLAY_NAME;
}

void X2Rotator::driverInfoDetailedInfo(BasicStringInterface& str) const
{
	str = PLUGIN_DISPLAY_NAME;
}
double X2Rotator::driverInfoVersion(void) const				
{
	return 1.0;
}

int	X2Rotator::position(double& dPosition)			
{
	dPosition  = m_dPosition;
	return SB_OK;
}
int	X2Rotator::abort(void)							
{
	if (m_bDoingGoto)
	{
		m_nGotoStartStamp = 0;
	}

	return SB_OK;
}

int	X2Rotator::startRotatorGoto(const double& dTargetPosition)	
{
	if (m_bDoingGoto)
	{
		return ERR_COMMANDINPROGRESS;
	}
	m_dTargetPosition = dTargetPosition;
	m_bDoingGoto = true;

	if (GetTickCountInterface())
		m_nGotoStartStamp = GetTickCountInterface()->elapsed();

	return SB_OK;
}
int	X2Rotator::isCompleteRotatorGoto(bool& bComplete) const	
{
	int nWait = 3000;
	int nNow = nWait +1;
	X2Rotator* pMe = (X2Rotator*)this;

	if (pMe->GetTickCountInterface())
		nNow = pMe->GetTickCountInterface()->elapsed();

	if (nNow-m_nGotoStartStamp>nWait)
	{
		pMe->m_dPosition = m_dTargetPosition; 
		bComplete = true;
	}
	else
		bComplete = !m_bDoingGoto;

	return SB_OK;
}
int	X2Rotator::endRotatorGoto(void)							
{
	m_bDoingGoto = false;

	return SB_OK;
}

#define PARENT_KEY			"X2Rotator"
#define CHILD_KEY_PORTNAME	"PortName"

void X2Rotator::portNameOnToCharPtr(char* pszPort, const int& nMaxSize) const			
{
	if (NULL == pszPort)
		return;

	sprintf(pszPort, DEF_PORT_NAME);	

	if (m_pIniUtil)
		m_pIniUtil->readString(PARENT_KEY, CHILD_KEY_PORTNAME, pszPort, pszPort, nMaxSize);

}

void X2Rotator::portName(BasicStringInterface& str) const			
{
	char szPortName[DRIVER_MAX_STRING];

	portNameOnToCharPtr(szPortName, DRIVER_MAX_STRING);

	str = szPortName;

}

void X2Rotator::setPortName(const char* szPort)						
{
	if (m_pIniUtil)
		m_pIniUtil->writeString(PARENT_KEY, CHILD_KEY_PORTNAME, szPort);

}

int X2Rotator::execModalSettingsDialog()
{
	int nErr = SB_OK;
	X2ModalUIUtil uiutil(this, GetTheSkyXFacadeForDrivers());
	X2GUIInterface*					ui = uiutil.X2UI();
	X2GUIExchangeInterface*			dx = NULL;//Comes after ui is loaded
	bool bPressedOK = false;

	if (NULL == ui)
		return ERR_POINTER;

	if (nErr = ui->loadUserInterface("x2rotator.ui", deviceType(), m_nInstanceIndex))
		return nErr;

	if (NULL == (dx = uiutil.X2DX()))
		return ERR_POINTER;

	//See the X2Camera example for details on user interface, this ui is to test
	//The "More Settings" button on TheSkyX's general serial port settings dialog.

	//Intialize the user interface


	//Display the user interface
	if (nErr = ui->exec(bPressedOK))
		return nErr;

	//Retreive values from the user interface
	if (bPressedOK)
	{

	}

	return nErr;
}