#ifndef _OpenLoopMoveInterface_H
#define _OpenLoopMoveInterface_H

#ifdef THESKYX_FOLDER_TREE
#include "imagingsystem/hardware/mountbridge/implementor/mountdriverinterface/mountdriverinterface.h"//For MoveDir
#else//TheSkyX X2 Plug In Build
#include "../mountdriverinterface.h"//For MoveDir
#endif

//This might not be common - move to underneath MountBridge if specific to mounts

#define OpenLoopMoveInterface_Name "com.bisque.TheSkyX.OpenLoopMoveInterface/1.0"

/*!
\brief The OpenLoopMoveInterface allows a mount to move at a given rate for a open-ended amount of time.

\ingroup Interface

This interface is typically used by TheSkyX to allow a user-interface button to move the mount, where the mount
moves while the button is down, and the mount stops moving when the button is released.
*/
class OpenLoopMoveInterface 
{
public:

	virtual ~OpenLoopMoveInterface(){}

public:
	/*!Start the open-loop move.*/
	virtual int								startOpenLoopMove(const MountDriverInterface::MoveDir& Dir, const int& nRateIndex) = 0;
	/*!End the open-loop move.  This function is always called for every corresponding startOpenLoopMove(), allowing software implementations of the move.*/
	virtual int								endOpenLoopMove(void)							= 0;

	/*!Return true if the mount can be commanded to move in more than one perpendicular axis at the same time, otherwise return false.*/
	virtual bool							allowDiagonalMoves() { return false;}

	//OpenLoopMove specifics	

	/*! Return the number (count) of avaiable moves.*/
	virtual int								rateCountOpenLoopMove(void) const = 0;
	/*! Return a string along with the amount or size of the corresponding move.*/
	virtual int								rateNameFromIndexOpenLoopMove(const int& nZeroBasedIndex, char* pszOut, const int& nOutMaxSize)=0;
	/*! Return the current index of move selection. */
	virtual int								rateIndexOpenLoopMove(void)=0;
		
};

#endif