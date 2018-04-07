#ifndef _LinkInterface_H
#define _LinkInterface_H

#define LinkInterface_Name "com.bisque.TheSkyX.LinkInterface/1.0"

/*!   
\brief The LinkInterface allows connect and realated device operations.

\ingroup Tool

The word Link is used to describe the connection state to a device simply because the
word Connect (and disconnet) are used frequently in source code for other things.
*/

class LinkInterface
{
public:

	virtual ~LinkInterface(){}

public:
	//LinkInterface
	/*! Connect (link) to the device.*/
	virtual int									establishLink(void)						= 0;
	/*! Disconnect from the device.*/
	virtual int									terminateLink(void)						= 0;
	/*! Return true if there is a connection, otherwise return false.*/
	virtual bool								isLinked(void) const					= 0;

	/*!
	Software Bisque implementations only.
	For those devices where the above establishLink can take more than say 2 seconds,
	its nice to allow user's the ability to abort the operation, especially if/when 
	there is no actual device connected and establishLink has to time out.
	*/
	virtual bool								isEstablishLinkAbortable(void){return false;}

};

#endif