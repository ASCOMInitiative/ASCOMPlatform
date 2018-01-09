#ifndef _UnparkInterface_H
#define _UnparkInterface_H

#define UnparkInterface_Name "com.bisque.TheSkyX.UnparkInterface/1.0"

/*!   
\brief The UnparkInterface allows domes and mounts to be unparked.

\ingroup Interface

At this time TheSkyX only queries domes and mounts for implementation of this interface.
In the future, other devices may be queried for implementation of this interface if and when unparking ever exists on these devices.
This interface is optional.  

\sa ParkInterface
*/

class UnparkInterface 
{
public:

	virtual ~UnparkInterface(){}

public:
	/*!Initiate the park process.*/
	virtual int								startUnpark(void)						= 0;
	/*!Called to monitor the unpark process.  \param bComplete Set to true if the unpark is complete, otherwise set to false.*/
	virtual int								isCompleteUnpark(bool& bComplete) const	= 0;
	/*!Called once the unpark is complete.  This is called once for every corresponding startUnpark() allowing software implementations of unpark.*/
	virtual int								endUnpark(void)							= 0;

};

#endif