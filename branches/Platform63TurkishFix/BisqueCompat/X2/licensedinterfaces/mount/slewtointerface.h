#ifndef _SlewToInterface_H
#define _SlewToInterface_H

#define SlewToInterface_Name "com.bisque.TheSkyX.SlewToInterface/1.0"

/*!
\brief The SlewToInterface for mounts.

\ingroup Interface

If a X2 mount driver implements this interface, the mount is able to slew to a given RA, dec.
*/
class SlewToInterface
{
public:

	virtual ~SlewToInterface(){}

public:
	/*!Initiate the slew.*/
	virtual int								startSlewTo(const double& dRa, const double& dDec) = 0;
	/*!Called to monitor the slew process. \param bComplete Set to true if the slew is complete, otherwise return false.*/
	virtual int								isCompleteSlewTo(bool& bComplete) const	= 0;
	/*!Called once the slew is complete. This is called once for every corresponding startSlewTo() allowing software implementations of gotos.*/
	virtual int								endSlewTo(void)							= 0;

};

#endif