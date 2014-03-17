// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the ASCOMNATIVE_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM.Native - Unmanaged implementation
//
// Description:	Header file 
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// --------------------------------------------------------------------------------
//

// that uses this DLL. This way any other project whose source files include this file see 
// ASCOMNATIVE_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef ASCOMVIDEOUTILS_EXPORTS
#define ASCOMNATIVE_API __declspec(dllexport)
#else
#define ASCOMNATIVE_API __declspec(dllimport)
#endif

#include <windows.h>

HRESULT GetBitmapPixels(long width, long height, long bpp, long flipMode, long* pixels, BYTE* bitmapPixels);
HRESULT GetColourBitmapPixels(long width, long height, long bpp, long flipMode, long* pixels, BYTE* bitmapPixels);
HRESULT GetRGGBBayerBitmapPixels(long width, long height, long bpp, long* pixels, BYTE* bitmapPixels);
HRESULT GetMonochromePixelsFromBitmap(long width, long height, long bpp, long flipMode, HBITMAP* bitmap, long* pixels, BYTE* bitmapPixels, int mode);
HRESULT GetColourPixelsFromBitmap(long width, long height, long bpp, long flipMode, HBITMAP* bitmap, long* pixels, BYTE* bitmapPixels);
HRESULT GetRGGBBayerPixelsFromBitmap(long width, long height, long bpp, HBITMAP* bitmap, long* pixels);
HRESULT GetBitmapBytes(long width, long height, HBITMAP* bitmap, BYTE* bitmapPixels);