#ifndef _BasicIniUtilInterface_H_
#define _BasicIniUtilInterface_H_

#define BasicIniUtilInterface_Name "com.bisque.TheSkyX.Components.BasicIniUtilInterface/1.0"

/*!
\brief The BasicIniUtilInterface is used to make properties persistent.

\ingroup Tool

The BasicIniUtilInterface is a cross-platform utility making it easy for X2 implementors to make properties persistent.
X2 implementors are purposefully hidden from any path, filename, instance specifics which is handled
by TheSkyX's implementation of this interface.

The Reads never fail because a default value is passed.
The Write might fail and an error code is returned, but this rarely happens.
Tested and works on Windows, Mac, Ubuntu Linux.
*/

class BasicIniUtilInterface
{
public:
	virtual ~BasicIniUtilInterface(){};

public:

	/*!Read an integer from a persistent state.*/
	virtual int readInt(const char* szParentKey, const char* szChildKey, const int& nDefault)=0;
	/*!Write an integer to a persistent state.*/
	virtual int writeInt(const char* szParentKey, const char* szChildKey, const int& nValue)=0;

	/*!Read a double from a persistent state.*/
	virtual double readDouble(const char* szParentKey, const char* szChildKey, const double& dDefault)=0;
	/*!Write a double to a persistent state.*/
	virtual int writeDouble(const char* szParentKey, const char* szChildKey, const double& dValue)=0;

	/*!Read a string from a persistent state.*/
	virtual void readString(const char* szParentKey, const char* szChildKey, const char* szDefault, char* szResult, int nMaxSizeOfResultIn)=0;
	/*!Write a string to a persistent state.*/
	virtual int writeString(const char* szParentKey, const char* szChildKey, const char* szValue)=0;

};

#endif
//Linux wants a 'newline' at the end of every source file - don't delete the one after this line
