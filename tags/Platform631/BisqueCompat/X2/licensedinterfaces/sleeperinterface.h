#ifndef _SleeperInterface_H
#define _SleeperInterface_H

#define SleeperInterface_Name "com.bisque.TheSkyX.Components.SleeperInterface/1.0"

/*!
\brief The SleeperInterface is a cross-platform "sleeper". 

\ingroup Tool

The SleeperInterface provides X2 implementors an operating system agnostic way to enter an efficient sleep state.
Tested and works on Windows, Mac, Ubuntu Linux.
*/
class SleeperInterface
{
public:
	virtual ~SleeperInterface(){};
	/*!Enter an efficient wait state for n milliseconds*/
	virtual void sleep(const int& milliSecondsToSleep) = 0; 
};

#endif
//Linux wants a 'newline' at the end of every source file - don't delete the one after this line
