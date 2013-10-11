//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM.Native - Unmanaged implementation
//
// Description:	Unmanaged methods to create bitmaps from various ImageArray data
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// --------------------------------------------------------------------------------
//

// BitmapUtils.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "BitmapUtils.h"
#include <stdlib.h>
#include <math.h>

void CopyBitmapHeaders(long width, long height, bool flipVertically, BYTE* bitmapPixels)
{
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
	bih.biHeight = flipVertically ? -height : height; // bitmap height 

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
}

HRESULT GetBitmapPixels(long width, long height, long bpp, long flipMode, long* pixels, BYTE* bitmapPixels)
{
	bool flipHorizontally = flipMode == 1 || flipMode == 3;
	bool flipVertically = flipMode == 2 || flipMode == 3;

	CopyBitmapHeaders(width, height, flipVertically, bitmapPixels);

	long x_sp = 3 * width;
	long x_nrc = -6 * width;
	long x_inc = 3;

	if (flipHorizontally)
	{
		x_sp = 0;
		x_nrc = 0;
		x_inc = -3;
	}

	long currLinePos = 0;
	int length = width * height;
	bitmapPixels+=54 + 3 * length + x_sp;

	int shiftVal = bpp == 12 ? 4 : 8;

	int total = width * height;
	while(total--)
	{
		if (currLinePos == 0) 
		{
			currLinePos = width;
			bitmapPixels+= x_nrc;
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
		bitmapPixels+=x_inc;

		currLinePos--;
	}

	return S_OK;
}

HRESULT GetColourBitmapPixels(long width, long height, long bpp, long flipMode, long* pixels, BYTE* bitmapPixels)
{
	bool flipHorizontally = flipMode == 1 || flipMode == 3;
	bool flipVertically = flipMode == 2 || flipMode == 3;

	CopyBitmapHeaders(width, height, flipVertically, bitmapPixels);
	bitmapPixels+= 54;

	long x_sp = 3 * width;
	long x_nrc = -6 * width;
	long x_inc = 3;

	if (flipHorizontally)
	{
		x_sp = 0;
		x_nrc = 0;
		x_inc = -3;
	}

	long currLinePos = 0;
	int length = width * height;
	int twoLengths = 2 * length;
	bitmapPixels+=3 * length + x_sp;

	int shiftVal = bpp == 12 ? 4 : 8;

	int total = width * height;
	while(total--)
	{
		if (currLinePos == 0) 
		{
			currLinePos = width;
			bitmapPixels+=x_nrc;
		};

		unsigned int valR = *pixels;
		unsigned int valG = *(pixels + length);
		unsigned int valB = *(pixels + twoLengths);

		pixels++;

		unsigned int dblValR;
		unsigned int dblValG;
		unsigned int dblValB;

		if (bpp == 8)
		{
			dblValR = valR;
			dblValG = valG;
			dblValB = valB;
		}
		else
		{
			dblValR = valR >> shiftVal;
			dblValG = valG >> shiftVal;
			dblValB = valB >> shiftVal;
		}
		
		*bitmapPixels = (BYTE)(dblValB & 0xFF);
		*(bitmapPixels + 1) = (BYTE)(dblValG & 0xFF);
		*(bitmapPixels + 2) = (BYTE)(dblValR & 0xFF);

		bitmapPixels+=x_inc;

		currLinePos--;
	}

	return S_OK;
}

HRESULT GetRGGBBayerBitmapPixels(long width, long height, long bpp, long* pixels, BYTE* bitmapPixels)
{
	// RGGB Format:
	// 
	//           X = 1      X = 2  
	// Y = 1       R          G
	// Y = 2       G          B

	// Possible conversion from the BIAS open source library
	http://www.mip.informatik.uni-kiel.de/~wwwadmin/Software/Doc/BIAS/html/d5/d41/ImageConvert_8cpp_source.html

	return E_NOTIMPL;
}

HRESULT GetMonochromePixelsFromBitmap(long width, long height, long bpp, long flipMode, HBITMAP* bitmap, long* pixels, BYTE* bitmapPixels, int mode)
{
	BITMAP bmp;
	GetObject(bitmap, sizeof(bmp), &bmp);

	unsigned char* buf = reinterpret_cast<unsigned char*>(bmp.bmBits);

	CopyBitmapHeaders(width, height, false, bitmapPixels);
	
	unsigned char* ptrBuf = buf + ((width * height) - 1) * 4;
	bitmapPixels += 54 + ((width * height) - 1) * 3;

	for (int y=0; y < height; y++)
    {
		for (int x=0; x < width; x++)
		{
			long pixVal = 0;
			if (mode == 0)
				pixVal = *(ptrBuf + 2); //R
			else if (mode == 1)
				pixVal = *(ptrBuf + 1); //G
			else if (mode == 2)
				pixVal = *(ptrBuf); //B
			else if (mode == 3)
			{
				// YUV Conversion (PAL & NTSC)
				// Luma = 0.299 R + 0.587 G + 0.114 B
				double luma = 0.299* *(ptrBuf) + 0.587* *(ptrBuf + 1) + 0.114* *(ptrBuf + 2);
				pixVal = (long)luma;
			}

			BYTE* currBitmapPixel;
			if (flipMode == 0)
			{
				*(pixels + (width - 1 - x) + width * y) = pixVal;
				currBitmapPixel = bitmapPixels - 3 * (x + width * y);
			}
			else if (flipMode == 1) /* Flip Horizontally */
			{
				*(pixels + x + width * y) = pixVal;
				currBitmapPixel = bitmapPixels - 3 * ((width - 1 - x) + width * y);
			}
			else if (flipMode == 2) /* Flip Vertically */
			{
				*(pixels + (width - 1 - x) + width * (height - 1 - y)) = pixVal;
				currBitmapPixel = bitmapPixels - 3 * (x +  width * (height - 1 - y));
			}
			else if (flipMode == 3) /* Flip Horizontally & Vertically */
			{
				*(pixels + x + width * (height - 1 - y)) = pixVal;
				currBitmapPixel = bitmapPixels - 3 * ((width - 1 - x) + width * (height - 1 - y));
			}

			*(currBitmapPixel) = pixVal;
			*(currBitmapPixel + 1) = pixVal;
			*(currBitmapPixel + 2) = pixVal;

			ptrBuf-=4;
		}
	}

	return S_OK;
}

HRESULT GetColourPixelsFromBitmap(long width, long height, long bpp, long flipMode, HBITMAP* bitmap, long* pixels, BYTE* bitmapPixels)
{
	BITMAP bmp;
	GetObject(bitmap, sizeof(bmp), &bmp);

	long* ptrPixelsR = pixels;
	long* ptrPixelsG = pixels + (width * height);
	long* ptrPixelsB = pixels + 2 * (width * height);
	unsigned char* buf = reinterpret_cast<unsigned char*>(bmp.bmBits);

	CopyBitmapHeaders(width, height, false, bitmapPixels);

	unsigned char* ptrBuf = buf + ((width * height) - 1) * 4;

	bitmapPixels += 54 + ((width * height) - 1) * 3;

	for (int y=0; y < height; y++)
    {
		for (int x=0; x < width; x++)
		{
			BYTE* currBitmapPixel;

			if (flipMode == 0)
			{
				*(ptrPixelsR + (width - 1 - x) + width * y ) = *(ptrBuf + 2);
				*(ptrPixelsG + (width - 1 - x) + width * y ) = *(ptrBuf + 1);
				*(ptrPixelsB + (width - 1 - x) + width * y ) = *(ptrBuf);

				currBitmapPixel = bitmapPixels - 3 * (x + width * y);
			}
			else if (flipMode == 1) /* Flip Horizontally */
			{

				*(ptrPixelsR + x + width * y ) = *(ptrBuf + 2);
				*(ptrPixelsG + x + width * y ) = *(ptrBuf + 1);
				*(ptrPixelsB + x + width * y ) = *(ptrBuf);

				currBitmapPixel = bitmapPixels - 3 * ((width - 1 - x) + width * y);
			}
			else if (flipMode == 2) /* Flip Vertically */
			{
				*(ptrPixelsR + (width - 1 - x) + width * (height - 1 - y) ) = *(ptrBuf + 2);
				*(ptrPixelsG + (width - 1 - x) + width * (height - 1 - y) ) = *(ptrBuf + 1);
				*(ptrPixelsB + (width - 1 - x) + width * (height - 1 - y) ) = *(ptrBuf);

				currBitmapPixel = bitmapPixels - 3 * (x +  width * (height - 1 - y));
			}
			else if (flipMode == 3) /* Flip Horizontally & Vertically */
			{
				*(ptrPixelsR + x + width * (height - 1 - y) ) = *(ptrBuf + 2);
				*(ptrPixelsG + x + width * (height - 1 - y) ) = *(ptrBuf + 1);
				*(ptrPixelsB + x + width * (height - 1 - y) ) = *(ptrBuf);

				currBitmapPixel = bitmapPixels - 3 * ((width - 1 - x) + width * (height - 1 - y));
			}

			*(currBitmapPixel) = *(ptrBuf);
			*(currBitmapPixel + 1) = *(ptrBuf + 1);
			*(currBitmapPixel + 2) = *(ptrBuf + 2);

			ptrBuf-=4;
		}
	}

	return S_OK;

}

HRESULT GetBitmapBytes(long width, long height, HBITMAP* bitmap, BYTE* bitmapPixels)
{
	BITMAP bmp;
	GetObject(bitmap, sizeof(bmp), &bmp);

	unsigned char* buf = reinterpret_cast<unsigned char*>(bmp.bmBits);

	CopyBitmapHeaders(width, height, false, bitmapPixels);
	memcpy(bitmapPixels + 53, buf, width * height * 3);

	unsigned char* ptrBuf = buf + ((width * height) - 1) * 4;
	bitmapPixels += 54 + ((width * height) - 1) * 3;

	for (int y=0; y < height; y++)
    {
		for (int x=0; x < width; x++)
		{
			*(bitmapPixels) = *(ptrBuf);
			*(bitmapPixels + 1) = *(ptrBuf + 1);
			*(bitmapPixels + 2) = *(ptrBuf + 2);

			bitmapPixels-=3;
			ptrBuf-=4;
		}
	}
	return S_OK;
}

HRESULT GetRGGBBayerPixelsFromBitmap(long width, long height, long bpp, HBITMAP* bitmap, long* pixels)
{
	// RGGB Format:
	// 
	//           X = 1      X = 2  
	// Y = 1       R          G
	// Y = 2       G          B

	// 

	return E_NOTIMPL;
}


