#ifndef _DriverInfoInterface_H
#define _DriverInfoInterface_H

#define DriverInfoInterface_Name "com.bisque.TheSkyX.DriverInfoInterface/1.0"

class BasicStringInterface;

/*!
\brief The DriverInfoInterface provides X2 implementors a standarized way to provide driver specific information.

\ingroup Interface

*/
class DriverInfoInterface
{
public:

	virtual ~DriverInfoInterface(){}

public:
	/*!Return a version number.*/
	virtual double								driverInfoVersion(void) const							=0;
	/*!Return detailed information about the driver.*/
	virtual void								driverInfoDetailedInfo(BasicStringInterface& str) const	=0;

};

#endif