#ifndef _MutexInterface_h
#define _MutexInterface_h

#define MutexInterface_Name "com.bisque.TheSkyX.Components.MutexInterface/1.0"

/*!
\brief The MutexInterface provides a cross-platform mutex.

\ingroup Tool

The MutexInterface is a cross-platform mutex interface passed to X2 implementors.
Provides X2 implementors an operating system agnostic mutex.
Tested and works on Windows, Mac, Ubuntu Linux.
*/
class MutexInterface 
{
public:
	virtual ~MutexInterface(){};
public:
	/*!Locks the mutex.*/
	virtual void lock()=0; 
	/*!Unlocks the mutex.*/
	virtual void unlock()=0;
};

/*!
\brief The X2MutexLocker provides a cross-platform utility to lock and unlock a MutexInterface.

\ingroup Tool

A convienent helper ensures perfect mutex unlock for every lock, typically used to serialize device io calls.
Simply declare a local instance of this object which automatically locks the mutex and unlocks the mutex when
it goes out of scope.
*/
class X2MutexLocker
{
public:
	/*! The constructor that automatically locks the MutexInterface passed in.*/
	X2MutexLocker(MutexInterface* pIOMutex)
	{
		m_pIOMutex = pIOMutex;

		if (m_pIOMutex)
			m_pIOMutex->lock();
	}

	/*! The destructor that automatically unlocks the MutexInterface.*/
	~X2MutexLocker()
	{
		if (m_pIOMutex)
			m_pIOMutex->unlock();
	}
private:

	MutexInterface* m_pIOMutex;
	
};
#endif

//Linux wants a 'newline' at the end of every source file - don't delete the one after this line
