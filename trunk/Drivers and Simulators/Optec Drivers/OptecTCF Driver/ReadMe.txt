Installation Instructions:

Unzip to a temporary or permenant location on your hard drive.

Double Click Setup.exe to begin the install process.



Revision History:

5.1.1.0 - Added functionality for the IsMoving flag
	- Fixed the StepSize property so that it reports the correct stepsize in microns
	- Fixed a multithreading/serial timeout bug that affected older firmware versions
	- Changed the Move method to allow requests beyond the focuser limits. The driver will now
		cause the focuser to move to the limit and stop without throwing an exception.

5.1.0.0 - Initial Release