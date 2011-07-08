// This file contains the code elements that allow the standard NOVAS3 code to access a file containing the varying cio ra over time
// The original NOVAS3 code assumed that the file was in the current directory of the application over which we, as Platform 
// providers, have no control. Instead, we place the file in a standard position and the invoking Platform code 
// makes a call to set_racio_file to inform the NOVAS dll where the file is located.

// The value is stored in the global variable RACIO_FILE_NAME and the NOVAS3 code has been changed to use this value
// rather than the original default value.

// Consequently, if new versions of the NOVAS3 code are published by USNO, edits must be made to the new code to re-instate
// the ability to access the file in other than the application current directory.

// A default value is set for the RACIO_FILE_NAME variable so that behaviour will be identical to the unmodified DLL if
// set_racio_fie is not called.

// The following two subroutines in novas.c contain 1 changed line each: cio_location and cio_array
// An extern statement for RACIO_FILE_NAME has been added to the top of novas.c

#ifndef _NOVAS_
   #include "novas.h"
#endif

#ifndef __ASCOM__
   #include "ascom.h"
#endif

char RACIO_FILE_NAME[255]  = {'c','i','o','_','r','a','.','b','i','n','\0'}; //cio_ra.bin
 
void set_racio_file (char file_name[255])
{
strcpy (RACIO_FILE_NAME, file_name);
}
