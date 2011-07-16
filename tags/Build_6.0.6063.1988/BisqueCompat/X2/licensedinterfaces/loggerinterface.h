#ifndef _LoggerInterface_H_
#define _LoggerInterface_H_

#define LoggerInterface_Name "com.bisque.TheSkyX.Components.LoggerInterface/1.0"

/*!   
\brief The LoggerInterface is a cross-platform logging utility passed to X2 implementors.

\ingroup Tool

This interface is optional.
Tested and works on Windows, Mac, Ubuntu Linux.
*/

class LoggerInterface
{
public:
	virtual ~LoggerInterface(){};
	
public:
	/*! Have a string logged in TheSkyX's Communication Log window.*/
	virtual int out(char* szLogThis)=0;

	/*! Return the number of packets, retries and failures associated with device io if appropriate.*/
	virtual void packetsRetriesFailuresChanged(const int& p, const int& r, const int& f)=0;

};

#endif
//Linux wants a 'newline' at the end of every source file - don't delete the one after this line
