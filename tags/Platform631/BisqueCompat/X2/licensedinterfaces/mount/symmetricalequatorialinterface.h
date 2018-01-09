#ifndef _SymmetricalEquatorialInterface_H
#define _SymmetricalEquatorialInterface_H
// 
// Bob Denny - Modified 26-Apr-2011 for changes to kit, H files. Maybe not needed.
//
#include "mounttypeinterface.h"

#define SymmetricalEquatorialInterface_Name "com.bisque.TheSkyX.SymmetricalEquatorialInterface/1.0"

/*!
\brief The SymmetricalEquatorialInterface for equtorial mounts.

\ingroup Interface

This is the default mount type, no implementation is necessary.
\sa AsymmetricalEquatorialInterface 
*/

class SymmetricalEquatorialInterface 
{
public:

	virtual ~SymmetricalEquatorialInterface(){}

	/*!The default implemtnation returns the appropriate type of mount.*/
	MountTypeInterface::Type mountType(){return MountTypeInterface::Symmetrical_Equatorial;}

};

#endif