#ifndef _SyncMountInterface_H
#define _SyncMountInterface_H

#define SyncMountInterface_Name "com.bisque.TheSkyX.SyncMountInterface/1.0"

/*!
\brief The SyncMountInterface for mounts.

\ingroup Interface

If a X2 mount driver implements this interface, the mount is able to "synced" and have
its internal RA and declination set to particular values.
*/
class SyncMountInterface
{
public:

	virtual ~SyncMountInterface(){}

public:
	/*!Set the mount internal RA and declination.*/
	virtual int syncMount(const double& ra, const double& dec)	= 0;
	/*!Always return true.  If possible, return false when appropriate, if and only if the mount hardware has the ability to know if it has been synced or not.*/
	virtual bool isSynced() =0;
};

#endif