#ifndef _TheSkyXFacadeForDriversInterface_H
#define _TheSkyXFacadeForDriversInterface_H

#define TheSkyXFacadeForDriversInterface_Name "com.bisque.TheSkyX.Components.TheSkyXFacadeForDriversInterface/1.0"

class BasicStringInterface;

/*!
\brief The TheSkyXFacadeForDriversInterface is a simplified interface to TheSkyX passed to X2 implementors.

\ingroup Tool

The TheSkyXFacadeForDriversInterface provides X2 implementors a way to get sometimes necessary information back from TheSkyX.
Tested and works on Windows, Mac, Ubuntu Linux.

*/

class TheSkyXFacadeForDriversInterface
{
public:

	virtual ~TheSkyXFacadeForDriversInterface(){}

	/*!Software Bisque only.*/
	enum Command
	{
		CURRENT_TARGET	=100,
		GET_X2UI		=101,
		UNGET_X2UI		=102,
	};

//Properties
public:
	/*!Returns the version of TheSkyX as a string.*/
	virtual void version(char* pszOut, const int& nOutMaxSize) const=0;
	/*!Returns the build number of TheSkyX.  With every committed change to TheSkyX the build is incremented by one.*/
	virtual int build() const =0;

	/*!Returns the TheSkyX's latitude.*/
	virtual double latitude() const=0;
	/*!Returns the TheSkyX's longitude.*/
	virtual double longitude() const=0;
	/*!Returns the TheSkyX's time zone.*/
	virtual double timeZone() const=0;
	/*!Returns the TheSkyX's elevation.*/
	virtual double elevation() const=0;

//Methods
	/*!Returns the TheSkyX's julian date.*/
	virtual double julianDate() const =0;
	/*!Returns the TheSkyX's local sidereal time (lst).*/
	virtual double lst() const =0;
	/*!Returns the TheSkyX's hour angle.*/
	virtual double hourAngle(const double& dRAIn) const =0;
	/*!Returns the TheSkyX's local time.*/
	virtual int localDateTime(int& yy, int& mm, int& dd, int& h, int& min, double& sec, int& nIsDST) const =0;

	/*!Returns the TheSkyX's universal time in ISO8601 format.*/
	virtual int utInISO8601(char* pszOut, const int& nOutMaxSize) const=0;
	/*!Returns the TheSkyX's local time as a string.*/
	virtual int localDateTime(char* pszOut, const int& nOutMaxSize) const=0;

	/*!Remove the effects of atmospheric refraction for the given equatorial coordinates.*/
	virtual int removeRefraction(double& dRa, double& dDec) const=0;
	/*!Add in the effects of atmospheric refraction for the given topocentric, equatorial coordinates.*/
	virtual int addRefraction(double& dRa, double& dDec) const=0;

	/*!Convert a topocentric coordinate to equinox 2000.*/
	virtual int EqNowToJ2K(double& dRa, double& dDec) const=0;
	/*!Convert a equatorial coordinate to horizon based coordinate.*/
	virtual int EqToHz(const double& dRa, const double& dDec, double& dAz, double& dAlt) const=0;
	/*!Convert a horizon based coordinate to equatorial coordinate.*/
	virtual int HzToEq(const double& dAz, const double& dAlt, double& dRa, double& dDec) const=0;

	/*!Software Bisque only.*/
	virtual void pathToWriteConfigFilesTo(char* pszOut, const int& nOutMaxSize) const=0;
	/*!Software Bisque only.*/
	virtual int doCommand(const int& command, void* pIn, void* pOut) const=0;
};

#endif