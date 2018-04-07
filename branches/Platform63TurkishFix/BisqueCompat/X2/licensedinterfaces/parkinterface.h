#ifndef _ParkInterface_H
#define _ParkInterface_H

#define ParkInterface_Name "com.bisque.TheSkyX.ParkInterface/1.0"

/*!   
\brief The ParkInterface allows domes and mounts to be parked.

\ingroup Interface

This interface is optional.  At this time TheSkyX only queries domes and mounts for implementation of this interface.
In the future, other devices may be queried for implementation of this interface if and when parking ever exists on these devices.

\sa UnparkInterface
*/
class ParkInterface 
{
public:

	virtual ~ParkInterface(){}

public:
	
	/*!Return true if the device is parked.*/
	virtual bool							isParked(void)							{return false;}
	/*!Initiate the park process.*/
	virtual int								startPark(const double& dAz, const double& dAlt)= 0;
	/*!Called to monitor the park process.  \param bComplete Set to true if the park is complete, otherwise set to false.*/
	virtual int								isCompletePark(bool& bComplete) const	= 0;
	/*!Called once the park is complete.  This is called once for every corresponding startPark() allowing software implementations of park.*/
	virtual int								endPark(void)							= 0;

};

#endif