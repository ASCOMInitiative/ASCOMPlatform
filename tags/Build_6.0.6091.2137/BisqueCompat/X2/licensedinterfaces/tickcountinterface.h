#ifndef _TickCountInterface_H_
#define _TickCountInterface_H_

#define TickCountInterface_Name "com.bisque.TheSkyX.Components.TickCountInterface/1.0"

/*!
\brief The TickCountInterface is a cross-platform way to measure relative timing. 

\ingroup Tool

The TickCountInterface is a cross-platform timing interface passed to X2 implementors.
Useful for measuring relative timing.
Tested and works on Windows, Mac, Ubuntu Linux.
*/

class TickCountInterface
{
public:
	virtual ~TickCountInterface(){};

public:

	/*!Returns the number of milliseconds that have elapsed since TheSkyX started.*/
	virtual int elapsed()=0;

};

#endif

//Linux wants a 'newline' at the end of every source file - don't delete the one after this line
