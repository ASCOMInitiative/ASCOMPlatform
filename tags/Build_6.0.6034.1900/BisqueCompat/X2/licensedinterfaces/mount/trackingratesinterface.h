#ifndef _TrackingRatesInterface_H
#define _TrackingRatesInterface_H

#define TrackingRatesInterface_Name "com.bisque.TheSkyX.TrackingRatesInterface/1.0"

/*!
\brief The TrackingRatesInterface allows X2 mounts to support variable tracking rates. 

\ingroup Interface

*/
class TrackingRatesInterface 
{
public:

	virtual ~TrackingRatesInterface(){}

public:

	/*!Set the tracking rates.*/
	virtual int setTrackingRates( const bool& bTrackingOn, const bool& bIgnoreRates, const double& dRaRateArcSecPerSec, const double& dDecRateArcSecPerSec)=0;

	/*!Turn off tracking.  Provided for convenience, merely calls setTrackingRates() function.*/
	virtual int trackingOff()
	{
		return setTrackingRates( false, true, 0.0, 0.0);
	}
	/*!Turn on sidereal tracking.  Provided for convenience, merely calls setTrackingRates() function.*/
	virtual int siderealTrackingOn()
	{
		return setTrackingRates( true, true, 0.0, 0.0);
	}

	/*!Return the current tracking rates.  A special case for mounts that can set rates, but not read them...
	So the TheSkyX's user interface can know this, set bTrackSidereal=false and both rates to -1000.0*/
	virtual int trackingRates( bool& bTrackingOn, double& dRaRateArcSecPerSec, double& dDecRateArcSecPerSec)=0;
};

#endif