#ifndef _BasicStringInterface_H
#define _BasicStringInterface_H

#define BasicStringInterface_Name "com.bisque.TheSkyX.Components.BasicStringInterface/1.0"

/*!
\brief The BasicStringInterface allows a string as an output.

\ingroup Tool

The BasicStringInterface is passed as a parameter when TheSkyX calls various methods 
that need a string as an output parameter from an X2 implementor.  

See the HardwareInfoInterface for methods that pass this interface as a parameter.
*/
class BasicStringInterface
{
public:

	virtual ~BasicStringInterface(){}

public:
	//BasicStringInterface
	virtual BasicStringInterface& operator=(const char*)=0;
	virtual BasicStringInterface& operator+=(const char*)=0;

};

#endif