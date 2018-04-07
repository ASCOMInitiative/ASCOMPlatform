#ifndef _NeedsRefractionInterface_H
#define _NeedsRefractionInterface_H


#define NeedsRefractionInterface_Name "com.bisque.TheSkyX.NeedsRefractionInterface/1.0"

/*!
\brief The NeedsRefractionInterface for mounts.

\ingroup Interface

Mounts that need refraction adjustments means coordinates returned by a 
mount are tainted by refraction and the control system 
does not do its own internal refraction adjustments.
For example, the Paramount and the Bisque TCS, return true
from this interface so TheSkyX can act accordingly. 

If a mount does its own, internal,refraction, this means
the coordinates returned from a mount have the effects of refraction removed.  An example of such a mount is the Meade LX200.

What this means under the hood, if an X2 mount object returns true to needsRefactionAdjustments()
1)Upon reading raDec, TheSkyX removes the effects of refraction so the "real" object's location is lower than where the mount is currently, physically pointing to in altitude through the atmosphere.
2)Before syncing a mount to a cataloged coordinate, (setting raDec) TheSkyX adds in the effects of refraction to the cataloged coordinate.  This is opposite of #1 - in other words, on sync, intitialize the 
control system so that when we read raDec and subsequently remove the effects of refraction, the coordinates are the "real" topocententric coordinates.

Definintion - refraction is zero at zenith, max positive at horizon (a positive value)

*/
class NeedsRefractionInterface 
{
public:

	virtual ~NeedsRefractionInterface(){}

public:
	/*!Return true to have TheSkyX handle refraction, otherwise return false.*/
	virtual bool							needsRefactionAdjustments(void)	{return true;}


};

#endif