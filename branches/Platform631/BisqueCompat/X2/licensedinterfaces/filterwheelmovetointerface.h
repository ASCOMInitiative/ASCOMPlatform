#ifndef _FilterWheelMoveToInterface_H
#define _FilterWheelMoveToInterface_H

#define FilterWheelMoveToInterface_Name "com.bisque.TheSkyX.FilterWheelMoveToInterface/1.0"

class BasicStringInterface;

/*!
\brief The FilterWheelMoveToInterface allows moving to a specific filter.

\ingroup Interface

This interface is used by TheSkyX to move a filter wheel to a specific filter.
By nature this interface supports moving to a filter asyncrhonously, but can also
be used to move to a filter synchronously.

*/
class FilterWheelMoveToInterface 
{
public:

	virtual ~FilterWheelMoveToInterface(){}

public:

	/*!Return the total number of available filters.*/
	virtual int									filterCount(int& nCount)							= 0;
	
	/*!Start the move to operation.*/
	virtual int									startFilterWheelMoveTo(const int& nTargetPosition)	= 0;
	/*!Sets bComplete to non zero when the move to is complete.*/
	virtual int									isCompleteFilterWheelMoveTo(bool& bComplete) const	= 0;
	/*!End the move to.  This function is always called for every corresponding startFilterWheelMoveTo(), allowing software implementations of move to.*/
	virtual int									endFilterWheelMoveTo(void)							= 0;
	/*!Abort any move to operation in progress.*/
	virtual int									abortFilterWheelMoveTo(void)						= 0;

	/*!Return a default name of the filter associated with nIndex (in the range 0 to nCount-1 returned by filterCount(nCount)).  
	This is optional and gives X2 drivers a way to provide a default filter name if so desired.  The default implemenation does nothing,
	in which case TheSky will provide a default name for each filter.  TheSkyX provides a means for users to edit filter names as well.*/
	virtual int									defaultFilterName(const int& nIndex, BasicStringInterface& strFilterNameOut){nIndex; return 0;};

	/*! \page embeddeddevices Embedded Devices

	In an effort to lessen the burden in developing device drivers for TheSkyX, X2 specifically addresses the concept of an embedded device.
	The X2 architecture by default keeps in line with object oriented programming techniques which strive to keep objects independent of one another, 
	for example a camera and a filter wheel are independent.  This is fine when physically the devices are separate, say from two manufacturers and 
	they communicate on two different ports and they are independent.  In practice, many cameras have a built in filter wheel, 
	and both camera and filter wheel communicate over the same port and a (plug in) driver model that treats them independently can place a burden 
	on the developer to solve how to get the two independent drivers hosted in two shared libraries (dlls) to communicate over the same port.

	TheSkyX has the means to allow a camera to have an embedded filter wheel (a mount having an embedded focuser might be next
	but TheSkyX already has native drivers for most popular mounts with embedded focusers).
	
	The following are required for a camera to have a embedded filter wheel from TheSkyX's perspective:

	-# 1) The x2 camera driver must implement the FilterWheelMoveToInterface.
	-# 2) The hardwarelist.txt for the filter wheel must have its "MapsTo" field set to "Camera's Filter Wheel". 

	With this combination, TheSkyX will simply delegate the filter wheel calls to the camera's implementation of the FilterWheelMoveToInterface,
	thus "easily" allowing a camera driver to be created that has an embedded filter wheel while at the same time not duplicating interfaces to address
	an independent filter wheel vs. an dependent (embedded) filter wheel.

  	*/


	/*!The default implementation of this function does nothing which is correct for most filter wheels.  Only consider this function if you are 
	implementing a camera's embedded filter wheel. 

	If a CameraDriverInterface implements the FilterWheelMoveToInterface (for an embedded filter wheel), TheSkyX calls embeddedFilterWheelInit 
	passing along the name of the filter wheel selection just prior to establishLink for the camera.
	This serves to provide the camera sufficient information to be prepared for filter wheel control. 
	X2 implementors can give many names to an embedded filter wheel(s) through their hardwarelist.txt, 
	in case that differentiation helps in implementation or there are n filter wheel models to choose from.
	For more information, see the \ref embeddeddevices page.*/
	virtual void								embeddedFilterWheelInit(const char* psFilterWheelSelection){psFilterWheelSelection;}

};

#endif