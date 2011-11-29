#ifndef _X2GUIInterface_H
#define _X2GUIInterface_H

#define X2GUIInterface_Name "com.bisque.TheSkyX.X2GUIInterface/1.0"

#ifdef THESKYX_FOLDER_TREE
#include "driverrootinterface.h"
#include "components/theskyxfacadefordrivers/theskyxfacadefordriversinterface.h"
#else
#include "../../licensedinterfaces\driverrootinterface.h"
#include "../../licensedinterfaces\theskyxfacadefordriversinterface.h"
#endif

/*!   
\ingroup GUI

\brief The X2GUIExchangeInterface provides the X2 developer the means to get and set data from a graphical user interface (X2GUIInterface)

The X2GUIInterface returns this interface so X2 developer can set/get data from a X2GUIInterface. 

*/
class X2GUIExchangeInterface 
{
public:

	virtual ~X2GUIExchangeInterface (){}

public:
	/*! Call this to set text user interface control values*/
	virtual void setText(const char* pszObjectName, const char* pszValue) = 0;
	/*! Retreive the text values from user interface controls*/
	virtual void text(const char* pszObjectName, char* pszOut, const int& nOutMaxSize) = 0;

	/*! Enable the user interface control*/
	virtual void setEnabled(const char* pszObjectName, const bool& bEnabled) = 0;
	/*! See if the user interface control is enabled*/
	virtual bool isEnabled(const char* pszObjectName) = 0;

	/*! Set the current index on list type user interface controls, like a combo box.*/
	virtual void setCurrentIndex(const char* pszObjectName, const int & nValue) = 0;
	/*! Get the current index on list type user interface controls, like a combo box.*/
	virtual int currentIndex(const char* pszObjectName) = 0;

	/*! Check a user interface controls, like a radio button.*/
	virtual void setChecked(const char* pszObjectName, const int & nValue) = 0;
	/*! Get if a user interface controls is checked, like a radio button.*/
	virtual int isChecked(const char* pszObjectName) = 0;

	/*! Append a string to a combo box list.*/
	virtual void comboBoxAppendString(const char* pszControlName, const char* pszValue) = 0;

	/*! Set the text of a table widget.*/
	virtual void tableWidgetSetItem(const char* pszControlName, const int& nRow, const int& nCol, const char* pszValue) = 0;
	/*! Get the text of a table widget.*/
	virtual void tableWidgetGetItem(const char* pszControlName, const int& nRow, const int& nCol, char* pszValue, const int& nOutMaxSize) = 0;
	/*! Get the current row of a table widget.*/
	virtual void tableWidgetCurrentRow(const char* pszControlName, int& nRow) = 0;
	/*! Get the current column of a table widget.*/
	virtual void tableWidgetCurrentCol(const char* pszControlName, int& nCol) = 0;
	/*! Remove a row of a table widge.t*/
	virtual void tableWidgetRemoveRow(const char* pszControlName, const int& nRow) = 0;
	/*! Remove a column of a table widget.*/
	virtual void tableWidgetRemoveCol(const char* pszControlName, const int& nCol) = 0;

	/*! Display a message box.*/
	virtual void messageBox(const char* pszTitle, const char* pszMessage) = 0;

	/*! Call an arbitrary method (signal or slot, not properties) with no argument or one argument (double, int or string)*/
	virtual bool invokeMethod(const char* pszObjectName, const char* pszMethodName, 
								char* pszReturn=NULL, const int& nReturnMaxSize=0, 
								const char* pszArg1=NULL)=0;

	/*! Set a text property of a user inteface control.*/
	virtual void setPropertyString(const char* pszObjectName, const char* pszPropertyName, const char* pszValue) = 0;
	/*! Get a text property of a user inteface control.*/
	virtual void propertyString(const char* pszObjectName, const char* pszPropertyName, char* pszOut, const int& nOutMaxSize) = 0;

	/*! Set an integer property of a user inteface control.*/
	virtual void setPropertyInt(const char* pszObjectName, const char* pszPropertyName, const int & nValue) = 0;
	/*! Get an integer property of a user inteface control.*/
	virtual void propertyInt(const char* pszObjectName, const char* pszPropertyName, int& nValue) = 0;

	/*! Set an double property of a user inteface control.*/
	virtual void setPropertyDouble(const char* pszObjectName, const char* pszPropertyName, const double& dValue) = 0;
	/*! Get an double property of a user inteface control.*/
	virtual void propertyDouble(const char* pszObjectName, const char* pszPropertyName, double& dValue) = 0;
};

#define X2GUIEventInterface_Name "com.bisque.TheSkyX.X2GUIEventInterface/1.0"
/*!   
\ingroup GUI

\brief The X2UIEventsInterface notifies X2 implementors when user interface events happen.

X2 implementors can implement this interface to be notified when user interface events happen.
*/
class X2GUIEventInterface
{
public:
	
	/*! Take what ever action is necessary when a user interface event happens, for example, the user pressed a button. \param pszEvent The name of the event that occured following the convention "on_<object name>_<signal name>".*/
	virtual void uiEvent(X2GUIExchangeInterface* uiex, const char* pszEvent)=0;

};

/*!   
\ingroup GUI

\brief The X2GUIInterface allows X2 implementors to display a customized, cross platform, graphical user interface.

TheSkyX Build 4174 or later is required for all X2GUI type interfaces.

When making a graphical user interface associated with a cross platform device driver, the developer is faced with basically two options.

Option A: the developer could write and maintain native GUI code specific to each operating system.  While this is a perfectly valid
solution, the code is difficult to maintain and native GUI expertise/experience is required on all supported operating systems.  

Option B: the developer could use a cross platform graphical user interface library, for 
example qt or wxWidgets, to make their graphical user interface.  Again a perfectly valid solution, but then the distribution of 
any associated GUI libraries falls on the driver developer and if not done carefully, dll $#&& will result (especially in a plug in
architecture).

Option C: The X2 standard offers a third option when a driver developer is faced with the problem of creating custom graphical user interface
associated with the hardware, that works on multiple operating systems.
X2 developer can use the X2GUIInterface to have TheSky display their own, modal, custom, graphical user interface that is cross platform.  
The X2GUIInterface is windowing library agnostic, it does not expose or depend on any cross platform GUI library or windowing code 
native to any operating system.  The consequence is that X2 drivers using the X2GUIInterface are more or less encapsulated 
as far as the GUI goes.  So development, distribution and maintenance are greatly simplified.  There is of course some overhead in learning the 
X2GUIInterface, but the code samples show how to do it.

The X2GUIInterface requires creating the graphical user interface file with qt's Designer (an open source copy of
Designer (designer.exe) is included in the X2 samples in the footer).  The X2 developer distributes 
the .ui created by Designer and TheSkyX loads this user interface dynamically at run time.  Graphical user interface events are 
supported through the X2GUIEventInterface. Qt's Designer is only required at <b>design time</b> by the X2 developer for the creation 
of the X2 user interface.  The X2 developer does not need to worry about distribution of any qt binaries at run time because
X2 is not dependent upon qt.  Please note that the .ui created in this  way is considered open source and since the 
.ui is visible in TheSkyX anyway there isn't much intellectual property disclosed.

There are a few rules when using qt's Designer to create a .ui file compatible with X2GUIInterface:

-# All controls must be placed within a QFrame, promoted to an X2Form via the x2form.h and named X2Form (code samples do this for you).
-# If you need access to GUI events through the X2GUIEventInterface, keep the default object name that qt Designer creates when dropping user interface 
controls inside the X2Form (for example, the first radio button is named "radioButton", the second is "radioButton_2", etc).

Please note, that not every control and not every event from every control is supported.  Never-the-less, the most common ones are supported.
Between the X2GUIExchangeInterface and using qt's Designer to set user interface control properties, a nice GUI can be created with
a fairly broad range of capabilities.

Declare a local instance of the X2ModalUIUtil class to obtain this interface.  See the X2Camera for a complete end to end example of
creating a graphical user interface, setting control values, responding to GUI events and retrieving control values.

\sa X2ModalUIUtil 
*/

class X2GUIInterface 
{
public:

	virtual ~X2GUIInterface(){}

public:
	//X2GUIInterface

	/*! Set the name of the Qt user interface file (.ui) that defines your custom user interface.  This must be called before calling exec(). The .ui file goes into the same folder as the binary (shared library) of the driver.*/
	virtual int loadUserInterface(const char* pszFileName, const int& dt, const int& nISIndex)=0;

	/*! Returns the X2GUIExchangeInterface associated with this user-interface. */
	virtual X2GUIExchangeInterface* X2DX()=0;

	/*! Display the user interface.*/
	virtual int	exec(bool& bPressedOK)=0;

};

/*!   
\ingroup GUI

\brief The X2ModalUIUtil class supplies the X2 developer with the X2GUIInterface interface.

Declare a local instance of this class to obtain a X2GUIInterface.  See the X2Camera for an example.

The implementation of this class merely assures proper intialization and cleanup of the X2GUIInterface and should not be changed.

\sa X2Camera
*/
class X2ModalUIUtil
{

public:
	/*! Constructor */
	X2ModalUIUtil(DriverRootInterface* pCaller, TheSkyXFacadeForDriversInterface* pTheSkyX)
	{
		m_pTheSkyX = pTheSkyX;
		m_pX2UI = NULL;
		m_pX2UIEvent = NULL;
		if (pCaller)
			pCaller->queryAbstraction(X2GUIEventInterface_Name, (void**)&m_pX2UIEvent);
		X2UI();
	}

	/*! Obtain the X2GUIInterface*/
	X2GUIInterface* X2UI()
	{
		int nErr;

		if (NULL == m_pX2UI && m_pTheSkyX)
		{	
			if (nErr = m_pTheSkyX->doCommand(TheSkyXFacadeForDriversInterface::GET_X2UI, m_pX2UIEvent, &m_pX2UI))
				return NULL;
		}
		return m_pX2UI;
	}

	/*! Obtain the X2GUIExchangeInterface*/
	X2GUIExchangeInterface* X2DX()
	{
		if (NULL != m_pX2UI)
		{	
			return m_pX2UI->X2DX();
		}
		return NULL;
	}

	~X2ModalUIUtil()
	{
		if (m_pTheSkyX)
		{
			if (m_pX2UI)
				m_pTheSkyX->doCommand(TheSkyXFacadeForDriversInterface::UNGET_X2UI, NULL, &m_pX2UI);
		}
	}

private:
	TheSkyXFacadeForDriversInterface*	m_pTheSkyX;
	X2GUIInterface*					m_pX2UI;
	X2GUIEventInterface*				m_pX2UIEvent;			
};

#endif