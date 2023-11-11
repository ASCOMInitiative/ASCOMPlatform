#ifndef _NOVAS_
#include "novas.h"
#include "eph_manager.h"
#endif

#include <windows.h>
#include <stdio.h>
#include <time.h>

BOOL WINAPI DllMain(
	HINSTANCE hinstDLL,  // handle to DLL module
	DWORD fdwReason,     // reason for calling function
	LPVOID lpReserved)  // reserved
{
	// Perform actions based on the reason for calling.
	switch (fdwReason)
	{
	case DLL_PROCESS_ATTACH:
		// Initialize once for each new process.
		// Return FALSE to fail DLL load.
		break;

	case DLL_THREAD_ATTACH:
		// Do thread-specific initialization.
		break;

	case DLL_THREAD_DETACH:
		// Do thread-specific clean-up.
		break;

	case DLL_PROCESS_DETACH:
		// Perform any necessary clean-up.
		ephem_close();
		break;
	}
	return TRUE;  // Successful DLL_PROCESS_ATTACH.
}