// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the ASCOMVIDEOUTILS_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video Simulator - Unmanaged Utils
//
// Description:	Header file 
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// --------------------------------------------------------------------------------
//

// that uses this DLL. This way any other project whose source files include this file see 
// ASCOMVIDEOUTILS_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef ASCOMVIDEOUTILS_EXPORTS
#define ASCOMVIDEOUTILS_API __declspec(dllexport)
#else
#define ASCOMVIDEOUTILS_API __declspec(dllimport)
#endif

#include <windows.h>

HRESULT ApplyGammaBrightness(long width, long height, long bpp, long* pixels, int brightnes);
HRESULT SetGamma(double gamma);
HRESULT InitFrameIntegration(long width, long height);
HRESULT AddFrameForIntegration(long* pixels);
HRESULT GetResultingIntegratedFrame(long* pixels);
HRESULT CreateNewAviFile(LPCTSTR szFileName, long width, long height, int bpp, double fps);
HRESULT AviFileAddFrame(long* pixels);
HRESULT GetLastAviFileError(LPCTSTR szErrorMessage);
HRESULT AviFileClose();