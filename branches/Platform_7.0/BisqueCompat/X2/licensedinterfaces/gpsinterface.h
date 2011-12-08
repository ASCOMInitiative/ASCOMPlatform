#ifndef _GPSInterface_H
#define _GPSInterface_H

#define GPSInterface_Name "com.bisque.TheSkyX.GPSInterface/1.0"

/*!   
\brief The GPSInterface allows communcation with a GPS device.

\ingroup Interface

This interface is optional.  At this time TheSkyX only queries mounts for implementation of this interface.
In the future, other devices may be queried for implementation of this interface if and when GPS services ever exist on these devices.
*/

class GPSInterface
{
public:

	virtual ~GPSInterface(){}

public:
	//GPSInterface
	/*!Return true if the GPS exists and is present.*/
	virtual bool gpsExists(void)=0;
	/*!Return true if the GPS is connected (linked).*/
	virtual int isGPSLinked(bool&)=0;
	/*!Return the longitude in degrees.*/
	virtual	int  gpsLongitude( double& dLong )=0;
	/*!Return the latitude in degrees.*/
	virtual	int  gpsLatitude( double& dLat )=0;
	/*!Return the date.*/
	virtual	int	 gpsDate( int& mm, int& dd )=0;
	/*!Return the time.*/
	virtual	int	 gpsTime( int& hh, int& min, double& s)=0;
	/*!Return the year.*/
	virtual int  gpsYear(int& yy)=0;
	/*!Return the timezone.*/
	virtual int  gpsTimeZone(int& tz)=0;
};

#endif