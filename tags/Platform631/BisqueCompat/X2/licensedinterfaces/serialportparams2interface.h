#ifndef _SerialPortParams2Interface_H
#define _SerialPortParams2Interface_H

#ifdef THESKYX_FOLDER_TREE
#include "staticlibs/serx/serxinterface.h"
#include "components/basicstring/basicstringinterface.h"
#else
#include "../../licensedinterfaces/serxinterface.h"
#include "../../licensedinterfaces/basicstringinterface.h"
#endif

#define SerialPortParams2Interface_Name "com.bisque.TheSkyX.SerialPortParams2Interface/1.0"

/*!
\brief The SerialPortParams2Interface is a cross-platform interface to common serial port parameters. 

\ingroup Interface

For serial devices, implementing this interface causes TheSkyX to display a general serial port 
settings user interface for setting serial port parameters.
This does not encompass all serial port parameters for all operating systems, but does cover the
most common serial port settings applied to astronomical hardware.

New to TheSkyX Version 10.1.10 (build 4443 and later) a "More Settings" button appears on TheSkyX's 
general "Serial Port Settings" dialog if the underlying X2 driver also implements the ModalSettingsDialogInterface.
This allows X2 drivers to leverage TheSkyX's general "Serial Port Settings" dialog while also having
a custom user interface that is displayed when the "More Settings" button is pressed.  
In prior TheSkyX builds, the ModalSettingsDialogInterface and SerialPortParams2Interface
where mutually exclusive but that is no longer the case and X2 implementor don't need to duplicate
the serial port settings in their custom user interface.
*/

class SerialPortParams2Interface 
{
public:

	virtual ~SerialPortParams2Interface(){}

public:
	/*!Return serial port name as a string.*/
	virtual void			portName(BasicStringInterface& str) const			= 0;
	/*!Set the serial port name as a string.*/
	virtual void			setPortName(const char* szPort)	= 0;

	/*!Return the buad rate.*/
	virtual unsigned int	baudRate() const			= 0;
	/*!Set the baud rate.*/
	virtual void			setBaudRate(unsigned int)	= 0;
	/*!Return if the parameter is fixed or not.  The general user interface will hide this parameter if it is fixed.*/
	virtual bool			isBaudRateFixed() const		= 0;

	/*!Return the parity.*/
	virtual SerXInterface::Parity	parity() const				= 0;
	/*!Set the parity.*/
	virtual void					setParity(const SerXInterface::Parity& parity)= 0;
	/*!Return if the parameter is fixed or not.  The general user interface will hide this parameter if it is fixed.*/
	virtual bool					isParityFixed() const		= 0;//Generic serial port ui will hide if fixed

	/*!Return the number of data bits.*/
	virtual int				dataBits() const {return 8;}
	/*!Set the number of data bits.*/
	virtual void			setDataBits(const int& nValue){nValue;}
	/*!Return if the parameter is fixed or not.  The general user interface will hide this parameter if it is fixed.*/
	virtual bool			isDataBitsFixed(){return true;}

	/*!Return the number of stop bits.*/
	virtual int				stopBits() const {return 1;}
	/*!Set the number of stop bits.*/
	virtual void			setStopBits(const int& nValue){nValue;}
	/*!Return if the parameter is fixed or not.  The general user interface will hide this parameter if it is fixed.*/
	virtual bool			isStopBitsFixed(){return true;}

	/*!Return the flow control. Zero means no flow control.*/
	virtual int				flowControl() const {return 0;}
	/*!Set the flow control.*/
	virtual void			setFlowControl(const int& nValue){nValue;}
	/*!Return if the parameter is fixed or not.  The general user interface will hide this parameter if it is fixed.*/
	virtual bool			isFlowControlFixed(){return true;}
};

#endif