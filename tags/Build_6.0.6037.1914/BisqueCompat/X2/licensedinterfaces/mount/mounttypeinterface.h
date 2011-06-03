#ifndef _MountTypeInterface_H
#define _MountTypeInterface_H

#define MountTypeInterface_Name "com.bisque.TheSkyX.MountTypeInterface/1.0"

/*!
\brief The MountTypeInterface covers all possible mount types that X2 drivers can implement

\ingroup Tool

A Naïveté, object oriented architecture might insist upon a property common to all mounts, 
namely mountType() (and even worse yet, setMountType() where
implementation might be difficult to dyanmically change from one type  of mount to another.
Such a property leads to confusion for all methods and properties related
to the specific type of mount, and which ones apply and don't apply, etc.
In the X2 architecture, the related methods and properties are contained
in optional interfaces that each mount can support as needed.
In other words, X2 doesn't riddle the global scope with n number of stubs
for methods/properties that don't apply.

Below are the related, mutually exclusive, interfaces when implemented dictate
the type of mount.
\sa SymmetricalEquatorialInterface
\sa AsymmetricalEquatorialInterface
*/

class MountTypeInterface 
{
public:

	virtual ~MountTypeInterface(){}

	/*! Mount Type */
	enum Type
	{
		Symmetrical_Equatorial,	/**<  e.g. fork, horseshoe or yoke */
		Asymmetrical_Equatorial,/**<  e.g. GEM or cross-axis */
		AltAz,					/**<  Not yet natively supported */
		Unknown,				/**<  Unknown mount type */
	};

};

#endif