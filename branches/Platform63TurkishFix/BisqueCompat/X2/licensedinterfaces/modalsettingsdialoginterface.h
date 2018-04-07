#ifndef _ModalSettingsDialogInterface_H
#define _ModalSettingsDialogInterface_H

#define ModalSettingsDialogInterface_Name "com.bisque.TheSkyX.ModalSettingsDialogInterface/1.0"

/*!   
\brief The ModalSettingsDialogInterface allows X2 implementors to display a customized settings user interface.

\ingroup Interface

X2 implementors can implement this interface to have TheSky display their own, modal, settings user interface.  

Warning, if this interface is implemented without a using X2GUIInterace, the resulting X2 driver will 
require either some kind of cross platform windowing library or windowing code native to each operating system.  
Instead, X2 implementors are encouraged to use the X2GUIInterace in their implementation of this interface 
(see the X2Camera for a complete example) to keep their driver more maintainable and portable across operating systems
and to simplify their driver distribution.

\sa SerialPortParams2Interface
*/

class ModalSettingsDialogInterface 
{
public:

	virtual ~ModalSettingsDialogInterface(){}

public:
	//ModalSettingsDialogInterface
	/*! Initialize the modal settings dialog.*/
	virtual int								initModalSettingsDialog(void) = 0;
	/*! Execute and display the modal settings dialog.*/
	virtual int								execModalSettingsDialog(void) = 0;

};

#endif