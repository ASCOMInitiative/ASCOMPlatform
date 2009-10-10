#include "stdafx.h"

//
// This mess is a result of (1) Helper using the Dictionary object
// of the Scripting Runtime, and the Scripting Runtime containing
// these symbols which conflict with Win32 API. Just consider it
// to be magic.
//
#import "scrrun.dll" \
	rename("DeleteFile","DeleteFileX") \
	rename("CopyFile","CopyFileX") \
	rename("MoveFile","MoveFileX") \
	rename("TotalSize","TotalSizeX") \
	rename("FreeSpace","FreeSpaceX")
//
// There's no point in including the fat resulting from importing
// Helper-seerved classes we don't use. Plus a few of them also
// contain symbols that conflict with Win32 API.
//
#import "c:\\Program Files\\Common files\\ASCOM\\Helper.dll" \
	exclude("_Util","_Chooser","_Serial","_Timer","__Timer")
using namespace DriverHelper;


int _tmain(int argc, _TCHAR* argv[])
{
	const char *progID = "Fake.Rotator";

	CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);	// If you haven't already

	_ProfilePtr P = NULL;
	P.CreateInstance("DriverHelper.Profile");
	if(P == NULL) {
		// Do someting to indicate registration failed
		return 0;
	}
	_bstr_t dt("Rotator");						// Messy - needed for Automation/VB properties
	BSTR bdt = dt.copy();
	P->put_DeviceType(&bdt);
	//
	// Each time this is run, it either registers or unregisters
	// the fake rotator entries.
	//
	if(!P->IsRegistered(progID)) {
		P->Register(progID, "Fake Rotator for Testing");
		printf("Registered %s\n", progID);
	} else {
		P->Unregister(progID);
		printf("Unregistered %s\n", progID);
	}
	P.Release();
	printf("Press enter to quit: "); fflush(stdout);
	getchar();\
	return 0;
}

