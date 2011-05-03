#ifndef _FocuserGotoInterface2_H
#define _FocuserGotoInterface2_H

#define FocuserGotoInterface2_Name "com.bisque.TheSkyX.FocuserGotoInterface2/1.0"

/*!
\brief The FocuserGotoInterface2 allows a focuser to perform a goto operation.

\ingroup Interface

The nature of a focuser is that it moves relative from where it is, in(-) and out(+).
This justifies why startFocGoto() is relative rather than absolute.  An absolute goto can 
be accomplished by the more primitive, indigenous startFocGoto.

The gotos can be asynchronous, but some focuser hardware precludes that.  In such, cases
startFocGoto should be synchronous and isCompleteFocGoto would always return true so drivers
can essentially "fake" the the asynchronousness.

*/

class FocuserGotoInterface2
{
public:

	virtual ~FocuserGotoInterface2(){}

public:

	/*! Return the position of the focuser.  If the hardware doesn't have a digital read out, return a number that roughly corresponds to whatever units the focuser moves in (time, lenght, etc.)*/
	virtual int	focPosition(int& nPosition) 					=0;
	/*! Return the focusers minimum limit.*/
	virtual int	focMinimumLimit(int& nMinLimit) 				=0;
	/*! Return the focusers maximum limit.*/
	virtual int	focMaximumLimit(int& nMaxLimit)					=0; 
	/*! Abort an operation in progress.*/
	virtual int	focAbort()										=0;

	/*! Initiate the focus goto operation.*/
	virtual int	startFocGoto(const int& nRelativeOffset)		= 0;
	/*! Return if the goto is complete.*/
	virtual int	isCompleteFocGoto(bool& bComplete) const		= 0;
	/*! Called after the goto is complete.  This is called once for every corresponding startFocGoto() allowing software implementations of focuser gotos.*/
	virtual int	endFocGoto(void)								= 0;

	/*! Return the number (count) of avaiable focuser gotos.*/
	virtual int	amountCountFocGoto(void) const					= 0;
	/*! Return a string along with the amount or size of the corresponding focuser goto.*/
	virtual int	amountNameFromIndexFocGoto(const int& nZeroBasedIndex, BasicStringInterface& strDisplayName, int& nAmount)=0;
	/*! Return the current index of focuser goto selection. */
	virtual int	amountIndexFocGoto(void)						=0;

	/*! Coming soon to TheSkyX, a mount having an embedded focuser, via x2. */
	virtual void embeddedFocuserInit(const char* psFilterWheelSelection){psFilterWheelSelection;}

};

#endif