#if !defined(_X2Rotator_H_)
#define _X2Rotator_H_

#include "../../licensedinterfaces/rotatordriverinterface.h"
#include "../../licensedinterfaces/serialportparams2interface.h"
#include "../../licensedinterfaces/modalsettingsdialoginterface.h"

#define PLUGIN_DISPLAY_NAME "X2 Rotator" 

class SerXInterface;
class TheSkyXFacadeForDriversInterface;
class SleeperInterface;
class BasicIniUtilInterface;
class LoggerInterface;
class MutexInterface;
class SyncMountInterface;
class TickCountInterface;

/*!
\brief The X2Rotator example.

\ingroup Example

Use this example to write an X2Rotator driver.
*/
class X2Rotator : public RotatorDriverInterface , public SerialPortParams2Interface, public ModalSettingsDialogInterface
{
// Construction
public:

	/*!Standard X2 constructor*/
	X2Rotator(const char* pszDriverSelection,
				const int& nInstanceIndex,
						SerXInterface					* pSerX, 
						TheSkyXFacadeForDriversInterface	* pTheSkyX, 
						SleeperInterface					* pSleeper,
						BasicIniUtilInterface			* pIniUtil,
						LoggerInterface					* pLogger,
						MutexInterface					* pIOMutex,
						TickCountInterface				* pTickCount);

	virtual ~X2Rotator();  

public:

// Operations
public:

	/*!\name DriverRootInterface Implementation
	See DriverRootInterface.*/
	//@{ 
	virtual DeviceType							deviceType(void)							  {return DriverRootInterface::DT_ROTATOR;}
	virtual int									queryAbstraction(const char* pszName, void** ppVal);
	//@} 

	/*!\name DriverInfoInterface Implementation
	See DriverInfoInterface.*/
	//@{ 
	virtual void								driverInfoDetailedInfo(BasicStringInterface& str) const;
	virtual double								driverInfoVersion(void) const				;
	//@} 

	/*!\name HardwareInfoInterface Implementation
	See HardwareInfoInterface.*/
	//@{ 
	virtual void deviceInfoNameShort(BasicStringInterface& str) const				;
	virtual void deviceInfoNameLong(BasicStringInterface& str) const				;
	virtual void deviceInfoDetailedDescription(BasicStringInterface& str) const		;
	virtual void deviceInfoFirmwareVersion(BasicStringInterface& str)				;
	virtual void deviceInfoModel(BasicStringInterface& str)							;
	//@} 

	/*!\name LinkInterface Implementation
	See LinkInterface.*/
	//@{ 
	virtual int									establishLink(void)						;
	virtual int									terminateLink(void)						;
	virtual bool								isLinked(void) const					;
	virtual bool								isEstablishLinkAbortable(void) const	{return false;}
	//@} 

	/*!\name RotatorDriverInterface Implementation
	See RotatorDriverInterface.*/
	//@{ 
	virtual int									position(double& dPosition)			;
	virtual int									abort(void)							;

	virtual int									startRotatorGoto(const double& dTargetPosition)	;
	virtual int									isCompleteRotatorGoto(bool& bComplete) const	;
	virtual int									endRotatorGoto(void)							;
	//@} 


/*****************************************************************************************/
// Implementation

	//SerialPortParams2Interface
	virtual void			portName(BasicStringInterface& str) const			;
	virtual void			setPortName(const char* szPort)						;

	virtual unsigned int	baudRate() const			{return 9600;};
	virtual void			setBaudRate(unsigned int)	{};
	virtual bool			isBaudRateFixed() const		{return true;}

	virtual SerXInterface::Parity	parity() const				{return SerXInterface::B_NOPARITY;}
	virtual void					setParity(const SerXInterface::Parity& parity){parity;};
	virtual bool					isParityFixed() const		{return true;}

	//Use default impl of SerialPortParams2Interface for rest

	//
	/*!\name ModalSettingsDialogInterface Implementation
	See ModalSettingsDialogInterface.*/
	//@{ 
	virtual int								initModalSettingsDialog(void){return 0;}
	virtual int								execModalSettingsDialog(void);
	//@} 
private:	

	SerXInterface*							m_pSerX;		
	TheSkyXFacadeForDriversInterface* 		m_pTheSkyXForMounts;
	SleeperInterface*						m_pSleeper;
	BasicIniUtilInterface*				m_pIniUtil;
	LoggerInterface*						m_pLogger;
	MutexInterface*							m_pIOMutex;
	TickCountInterface*						m_pTickCount;

	SerXInterface 							*GetSerX() {return m_pSerX; }		
	TheSkyXFacadeForDriversInterface		*GetTheSkyXFacadeForDrivers() {return m_pTheSkyXForMounts;}
	SleeperInterface						*GetSleeper() {return m_pSleeper; }
	BasicIniUtilInterface					*GetSimpleIniUtil() {return m_pIniUtil; }
	LoggerInterface							*GetLogger() {return m_pLogger; }
	MutexInterface							*GetMutex()  {return m_pIOMutex;}
	TickCountInterface						*GetTickCountInterface() {return m_pTickCount;}

	bool m_bLinked;
	int m_nInstanceIndex;
	double m_dPosition;
	bool m_bDoingGoto;
	double m_dTargetPosition;
	int m_nGotoStartStamp;

	void portNameOnToCharPtr(char* pszPort, const int& nMaxSize) const;


	//Samples for Tom doing IO with SerXInteface and Optec TCFS
	int tcfsGetSerialConnection();
	int tcfsTest();
	int tcfsSendFMMODE();
	int tcfsPositionCore(int* plPos);
};




#endif
