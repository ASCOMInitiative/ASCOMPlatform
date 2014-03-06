//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video Driver - Unmanaged Utils
//
// Description:	A class that creates a Bitmap structure from an array of integer pixel values
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// --------------------------------------------------------------------------------
//

// VideoUtils.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "VideoUtils.h"
#include <stdlib.h>
#include <math.h>
#include "Avi.h"
#include "Gdiplus.h"

double s_CurrentGamma = 1.0;
int s_GammaMap256[256];
int s_GammaMap4096[4096]; 

HRESULT SetGamma(double gamma)
{
	if (s_CurrentGamma != gamma)
	{
		double normGammaValue255 = pow(255, gamma);
		double normGammaValue4095 = pow(4095, gamma);

		for (int i = 0; i < 256; i++)
		{
			s_GammaMap256[i] = (long)(1.0 * 255 * pow(i, gamma) / normGammaValue255);
			s_GammaMap4096[i] = (long)(1.0 * 4095 * pow(i, gamma) / normGammaValue4095);
		}

		for (int i = 256; i < 4096; i++)
		{
			s_GammaMap4096[i] = (long)(1.0 * 4095 * pow(i, gamma) / normGammaValue4095);
		}

		s_CurrentGamma = gamma;
	}

	return S_OK;
}

long s_WhiteBalance = 255;

HRESULT SetWhiteBalance(long whiteBalance)
{
	s_WhiteBalance = whiteBalance;

	return S_OK;
}

void GetMinMaxValuesForBpp(int bpp, int* minValue, int* maxValue)
{
	*minValue = 0;
	*maxValue = 0;

	if (bpp == 8)
		*maxValue = 0xFF;
	else if (bpp == 12)
		*maxValue = 0xFFF;
	else if (bpp == 14)
		*maxValue = 0x3FFF;
	else if (bpp == 16)
		*maxValue = 0xFFFF;
}

HRESULT ApplyGammaBrightness(long width, long height, long bpp, long* pixelsIn, long* pixelsOut, short brightness)
{
	int minValue, maxValue;
	GetMinMaxValuesForBpp(bpp, &minValue, &maxValue);
	
	if (brightness < -255) brightness = -255;
	if (brightness > 255) brightness = 255;

	double bppBrightness = brightness;
	if (bpp == 12) bppBrightness *= 0xF;
	if (bpp == 16) bppBrightness *= 0xFF;

	long totalPixels = width * height;

	memcpy(&pixelsOut[0], &pixelsIn[0], totalPixels * sizeof(long));

	long* pPixels = pixelsOut;

	double normGammaValue = s_CurrentGamma != 1.0 
		? pow(maxValue, s_CurrentGamma)
		: 0;

	while(totalPixels--)
	{
		double pixel = *pPixels;

		if (s_CurrentGamma != 1.0)
		{
			if (bpp == 8)
			{
				pixel = s_GammaMap256[*pPixels];
			}
			else if (bpp == 12)
			{
				pixel = s_GammaMap4096[*pPixels];
			}
			else
				pixel = (long)(1.0 * maxValue * pow(pixel, s_CurrentGamma) / normGammaValue);
		
			if (pixel < minValue) pixel = minValue;
			if (pixel > maxValue) pixel = maxValue;	
		}

		pixel = pixel + bppBrightness;
		if (pixel < minValue) pixel = minValue;
		if (pixel > maxValue) pixel = maxValue;
		
		if (pixel > s_WhiteBalance)
			pixel = maxValue;

		*pPixels = (long)pixel;

 		pPixels++;
	}

	return S_OK;
}

long s_Width;
long s_Height;
long s_NumPixels;
long s_AddedFrames;
double* s_Pixels = NULL;
int s_MaxPixelVal;
int s_MinPixelVal;

HRESULT InitFrameIntegration(long width, long height, long bpp)
{
	s_Width = width;
	s_Height = height;
	s_NumPixels = width * height;
	s_AddedFrames = 0;

	GetMinMaxValuesForBpp(bpp, &s_MinPixelVal, &s_MaxPixelVal);

	if (NULL != s_Pixels)
	{
		delete s_Pixels;
		s_Pixels = NULL;
	}

	s_Pixels = (double*) malloc(sizeof(double) * s_NumPixels);
	::ZeroMemory(s_Pixels, sizeof(double) * s_NumPixels);

	return S_OK;
}

HRESULT AddFrameForIntegration(long* pixels)
{
	long* itt = pixels;
	double* destItt = s_Pixels;

	for (int i = 0; i < s_NumPixels; i++)
	{
		*destItt += *itt;

		destItt++;
		itt++;
	}

	s_AddedFrames++;

	return S_OK;
}

HRESULT GetResultingIntegratedFrame(long* pixels)
{
	if (s_AddedFrames > 0)
	{
		long* itt = pixels;
		double* destItt = s_Pixels;

		double brightnessCoeff = 1;
		if (s_AddedFrames <= 2)
			brightnessCoeff = 1;
		if (s_AddedFrames > 2 && s_AddedFrames <= 4)
			brightnessCoeff = 0.9;
		else if (s_AddedFrames > 4 && s_AddedFrames <= 8)
			brightnessCoeff = 0.8;
		else if (s_AddedFrames > 8 && s_AddedFrames <= 16)
			brightnessCoeff = 0.7;
		else if (s_AddedFrames > 16 && s_AddedFrames <= 32)
			brightnessCoeff = 0.6;
		else if (s_AddedFrames > 32 && s_AddedFrames <= 64)
			brightnessCoeff = 0.3;
		else if (s_AddedFrames > 64 && s_AddedFrames <= 128)
			brightnessCoeff = 0.15;
		else if (s_AddedFrames > 128)
			brightnessCoeff = 0.075;

		for (int i = 0; i < s_NumPixels; i++)
		{
			long integratedVal = (long)(*destItt * brightnessCoeff); 

			if (integratedVal < s_MinPixelVal)
				integratedVal = s_MinPixelVal;

			if (integratedVal > s_MaxPixelVal)
				integratedVal = s_MaxPixelVal;
			
			*itt = integratedVal;

			destItt++;
			itt++;
		}

		return S_OK;
	}
	else
		return S_FALSE;
}


Avi* s_AviFile = NULL;
IStream* s_pStream = NULL;

#define ERROR_BUFFER_SIZE 200
char s_LastAviErrorMessage[ERROR_BUFFER_SIZE];

long s_AviFrameNo = 0;
long s_AviFrameWidth = 0;
long s_AviFrameHeight = 0;
long s_AviFrameBpp = 8;
bool s_ShowCompressionDialog = false;
unsigned long s_UsedCompression = 0;

void EnsureAviFileClosed()
{
	if (NULL != s_pStream)
	{
		s_pStream->Release();
		s_pStream = NULL;
	}

	if (NULL != s_AviFile)
	{
		s_AviFile->Close();
		delete s_AviFile;
		s_AviFile = NULL;
	}
}

unsigned long GetUsedAviCompression()
{
	return s_UsedCompression;
}

HRESULT CreateNewAviFile(LPCTSTR szFileName, long width, long height, long bpp, double fps, bool showCompressionDialog)
{
	HRESULT rv = S_OK;

	EnsureAviFileClosed();
	
	s_AviFile = new Avi(szFileName, static_cast<int>(1000.0/fps), 0);

	s_AviFrameNo = 0;
	s_AviFrameWidth = width;
	s_AviFrameHeight = height;
	s_AviFrameBpp = bpp;
	s_ShowCompressionDialog = showCompressionDialog;
	s_UsedCompression = 0;

	::CreateStreamOnHGlobal(NULL, TRUE, &s_pStream);

	s_AddedFrames = 0;

	return rv;
}

HRESULT SetAviFileCompression(HBITMAP* bmp)
{
	AVICOMPRESSOPTIONS opts; 
	ZeroMemory(&opts,sizeof(opts));
	// Use uncompressed by default
	opts.fccHandler= mmioFOURCC('D','I','B',' '); //0x20424944
	opts.dwFlags = AVICOMPRESSF_VALID;

	HRESULT rv = s_AviFile->compression(*bmp, &opts, s_ShowCompressionDialog, 0);
	
	if (rv != S_OK) 
	{
		rv = s_AviFile->compression(*bmp, 0, false, 0);
		s_UsedCompression = 0;
	}
	else
	{
		s_UsedCompression = opts.fccHandler;
	}

	if (rv != S_OK) 
		FormatAviMessage(rv, s_LastAviErrorMessage, ERROR_BUFFER_SIZE);

	return rv;
}

HRESULT GetLastAviFileError(char* szErrorMessage)
{
	strncpy(szErrorMessage, s_LastAviErrorMessage, ERROR_BUFFER_SIZE);

	return S_OK;
}


BYTE* BuildBitmap(long width, long height, long bpp, long* pixels)
{
	BYTE* bitmapPixels = (BYTE*)malloc(sizeof(BYTE) * ((width * height * 3) + 40 + 14 + 1));
	BYTE* bitmapPixelsStartPtr = bitmapPixels;

	// define the bitmap information header 
	BITMAPINFOHEADER bih;
	bih.biSize = sizeof(BITMAPINFOHEADER); 
	bih.biPlanes = 1; 
	bih.biBitCount = 24;                          // 24-bit 
	bih.biCompression = BI_RGB;                   // no compression 
	bih.biSizeImage = width * abs(height) * 3;    // width * height * (RGB bytes) 
	bih.biXPelsPerMeter = 0; 
	bih.biYPelsPerMeter = 0; 
	bih.biClrUsed = 0; 
	bih.biClrImportant = 0; 
	bih.biWidth = width;                          // bitmap width 
	bih.biHeight = height;                        // bitmap height 

	// and BitmapInfo variable-length UDT
	BYTE memBitmapInfo[40];
	RtlMoveMemory(memBitmapInfo, &bih, sizeof(bih));

	BITMAPFILEHEADER bfh;
	bfh.bfType=19778;    //BM header
	bfh.bfSize=55 + bih.biSizeImage;
	bfh.bfReserved1=0;
	bfh.bfReserved2=0;
	bfh.bfOffBits=sizeof(BITMAPINFOHEADER) + sizeof(BITMAPFILEHEADER); //54

	// Copy the display bitmap including the header
	RtlMoveMemory(bitmapPixels, &bfh, sizeof(bfh));
	RtlMoveMemory(bitmapPixels + sizeof(bfh), &memBitmapInfo, sizeof(memBitmapInfo));

	bitmapPixels = bitmapPixels + sizeof(bfh) + sizeof(memBitmapInfo);

	long currLinePos = 0;
	int length = width * height;
	bitmapPixels+=3 * (length + width);

	int shiftVal = bpp == 12 ? 4 : 8;

	int total = width * height;
	while(total--)
	{
		if (currLinePos == 0) 
		{
			currLinePos = width;
			bitmapPixels-=6*width;
		};

		unsigned int val = *pixels;
		pixels++;

		unsigned int dblVal;
		if (bpp == 8)
		{
			dblVal = val;
		}
		else
		{
			dblVal = val >> shiftVal;
		}
		 

		BYTE btVal = (BYTE)(dblVal & 0xFF);
		
		*bitmapPixels = btVal;
		*(bitmapPixels + 1) = btVal;
		*(bitmapPixels + 2) = btVal;
		bitmapPixels+=3;

		currLinePos--;
	}

	return bitmapPixelsStartPtr;
}

HRESULT AviFileAddFrame(long* pixels)
{
	HRESULT rv = S_OK;

	BYTE* bitmapPixels = BuildBitmap(s_AviFrameWidth, s_AviFrameHeight, s_AviFrameBpp, pixels);
	
	if(s_pStream)
	{
		s_pStream->Revert();

		rv = s_pStream->Write(&bitmapPixels[0], ULONG(sizeof(BYTE) * ((s_AviFrameWidth * s_AviFrameHeight * 3) + 40 + 14 + 1)), NULL);
		if(rv == S_OK)
		{
			HBITMAP hbmp = NULL;
			Gdiplus::Bitmap* pBitmap = Gdiplus::Bitmap::FromStream(s_pStream);
			if (pBitmap->GetHBITMAP(Gdiplus::Color(255, 0, 0, 0), &hbmp) == Gdiplus::Ok)
			{
				// NOTE: No AVI compression supported in the Simulator
				//if (s_AddedFrames == 0) SetAviFileCompression(&hbmp);

				rv = s_AviFile->add_frame(hbmp);

				s_AddedFrames++;

				if (rv != S_OK)
				  FormatAviMessage(rv, s_LastAviErrorMessage, ERROR_BUFFER_SIZE);

				::DeleteObject(pBitmap);
			}

			::DeleteObject(hbmp);
		}
	}

	delete bitmapPixels;

	return rv;
}


HRESULT AviFileClose()
{
	EnsureAviFileClosed();

	return S_OK;
}