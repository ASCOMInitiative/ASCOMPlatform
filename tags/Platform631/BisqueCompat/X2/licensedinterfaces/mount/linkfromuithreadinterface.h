#ifndef _LinkFromUIThreadInterface_H
#define _LinkFromUIThreadInterface_H

#define LinkFromUIThreadInterface_Name "com.bisque.TheSkyX.LinkFromUIThreadInterface/1.0"

/*!   
\brief The LinkFromUIThreadInterface allows X2 implementors to cause TheSkyX to call establishLink from the user interface thread.

\ingroup Interface

X2 implementors can implement this interface to have TheSky call establishLink() from the user interface thread instead of from a background thread by default.
This can simplify drivers that use Microsoft COM on Windows in their implementation .
There are no member functions to this interface, simply implemenenting it is sufficient.

This only applies to mount drivers.

\sa LinkInterface
*/

class LinkFromUIThreadInterface 
{
public:

	virtual ~LinkFromUIThreadInterface(){}

public:


};

#endif