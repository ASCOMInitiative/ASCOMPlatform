#if !defined(_CameraDriverInterFace_H_)
#define _CameraDriverInterFace_H_

#ifdef THESKYX_FOLDER_TREE
#include "imagingsystem/hardware/interfaces/licensed/driverrootinterface.h"
#include "imagingsystem/hardware/interfaces/licensed/linkinterface.h"
#include "imagingsystem/hardware/interfaces/licensed/deviceinfointerface.h"
#include "imagingsystem/hardware/interfaces/licensed/driverinfointerface.h"
#include "enumcameraindex.h"
#else//TheSkyX X2 Plug In Build
#include "../../licensedinterfaces/driverrootinterface.h"
#include "../../licensedinterfaces/linkinterface.h"
#include "../../licensedinterfaces/deviceinfointerface.h"
#include "../../licensedinterfaces/driverinfointerface.h"
typedef enum 
{
CI_NONE, 
CI_PLUGIN=10000,
CI_PLUGIN_LAST=CI_PLUGIN+50000,
} enumCameraIndex;
#endif

//TheSkyX's camera driver interface  
#define CAMAPIVERSION 6

typedef enum {CCD_IMAGER, CCD_GUIDER} enumWhichCCD;
typedef enum {PT_UNKNOWN, PT_LIGHT, PT_BIAS, PT_DARK, PT_FLAT, PT_AUTODARK} enumPictureType;

//This is an exact copy of SBIG's enum
typedef enum { zDEV_NONE, zDEV_LPT1, zDEV_LPT2, zDEV_LPT3, zDEV_USB=0x7F00, zDEV_ETH, zDEV_USB1, zDEV_USB2, zDEV_USB3, zDEV_USB4 } enumLPTPort;

/*!
\brief The CameraDriverInterface allows an X2 implementor to a write X2 camera driver.

\ingroup Driver

See the X2Camera for a example.
*/
class CameraDriverInterface : public DriverRootInterface, public HardwareInfoInterface, public DriverInfoInterface 
{
//Base class start copy here

public: 
	inline CameraDriverInterface()
	{  
		m_bLinked = false;
		m_Camera = CI_NONE;
	}
	virtual ~CameraDriverInterface(){};  

	/*!\name DriverRootInterface Implementation
		See DriverRootInterface.*/
	//@{ 
	virtual DeviceType							deviceType(void)							  {return DriverRootInterface::DT_CAMERA;}
	virtual int									queryAbstraction(const char* pszName, void** ppVal)			=0;
	//@} 

	/*!\name DriverInfoInterface Implementation
		See DriverInfoInterface.*/
	//@{ 
	virtual void								driverInfoDetailedInfo(BasicStringInterface& str) const		=0;
	virtual double								driverInfoVersion(void) const								=0;
	//@} 

	/*!\name HardwareInfoInterface Implementation
		See HardwareInfoInterface.*/
	//@{ 
	virtual void deviceInfoNameShort(BasicStringInterface& str) const										=0;
	virtual void deviceInfoNameLong(BasicStringInterface& str) const										=0;
	virtual void deviceInfoDetailedDescription(BasicStringInterface& str) const								=0;
	virtual void deviceInfoFirmwareVersion(BasicStringInterface& str)										=0;
	virtual void deviceInfoModel(BasicStringInterface& str)													=0;
	//@} 

//CameraDriverInterface

	/*! ReadOutMode */
	enum ReadOutMode 
	{
		rm_Line = 0,		/**<  The camera is a line readout device.*/
		rm_Image = 1,		/**<  The camera is a frame readout device.*/
		rm_FitsOnDisk = 2,	/**<  The camera provides the image as a FITS on disk.*/
	};

public://Properties

	/*!Software Bisque only.*/
	virtual enumCameraIndex	cameraId()				{return m_Camera;}
	/*!Software Bisque only.*/
	virtual	void		setCameraId(enumCameraIndex Cam)	{m_Camera = Cam;}
	/*!Return true if the camrea is connected (linked)*/
	virtual bool		isLinked()					{return m_bLinked;}
	/*!Software Bisque only.*/
	virtual void		setLinked(const bool bYes)	{m_bLinked = bYes;}

	/*! Return the version to this interface, X2 implementors do not modify.*/
	virtual int			cameraDriverInterfaceVersion(void)			{return CAMAPIVERSION;}
	/*! Return how this camera reads out the image.*/
	virtual ReadOutMode readoutMode(void)		{return CameraDriverInterface::rm_Line;}
	/*!	This is called to return the path to the corresponding FITS file, only when readoutMode() returns rm_FitsOnDisk. \sa readoutMode().*/
	virtual int			pathTo_rm_FitsOnDisk(char* lpszPath, const int& nPathMaxSize){lpszPath; nPathMaxSize;return 0;}

public://Methods

	/*!	Display a device dependent settings dialog if necessary.*/
	virtual int CCSettings(const enumCameraIndex& Camera, const enumWhichCCD& CCD)=0;

	/*!	Connect or establish a link to the camera.*/
	virtual int CCEstablishLink(enumLPTPort portLPT, const enumWhichCCD& CCD, enumCameraIndex DesiredCamera, enumCameraIndex& CameraFound, const int nDesiredCFW, int& nFoundCFW)=0;
	/*!	Disconnect from the camera.*/
	virtual int CCDisconnect(const bool bShutDownTemp)=0;

	/*!	Return the physical size of the camera's detector.*/
	virtual int CCGetChipSize(const enumCameraIndex& Camera, const enumWhichCCD& CCD, const int& nXBin, const int& nYBin, const bool& bOffChipBinning, int& nW, int& nH, int& nReadOut)=0;
	/*!	Return the number of bin modes this camera supports.*/
	virtual int CCGetNumBins(const enumCameraIndex& Camera, const enumWhichCCD& CCD, int& nNumBins)=0;
	/*!	Return the size x and y bin size for each bin mode this camera supports.*/
	virtual	int CCGetBinSizeFromIndex(const enumCameraIndex& Camera, const enumWhichCCD& CCD, const int& nIndex, long& nBincx, long& nBincy)=0;

	/*!	Set the size of subframe in binned pixels.*/
	virtual int CCSetBinnedSubFrame(const enumCameraIndex& Camera, const enumWhichCCD& CCD, const int& nLeft, const int& nTop, const int& nRight, const int& nBottom)=0;

	/*!	SBIG specific.*/
	virtual void CCMakeExposureState(int* pnState, enumCameraIndex Cam, int nXBin, int nYBin, int abg, bool bRapidReadout)=0;

	/*!	Start the exposure.*/
	virtual int CCStartExposure(const enumCameraIndex& Cam, const enumWhichCCD CCD, const double& dTime, enumPictureType Type, const int& nABGState, const bool& bLeaveShutterAlone)=0;
	/*!	Called to know if the exposure is complete. \param pbComplete Set to true if the exposure is complete, otherwise set to false. \param pStatus is SBIG specific and can be ignored.*/
	virtual int CCIsExposureComplete(const enumCameraIndex& Cam, const enumWhichCCD CCD, bool* pbComplete, unsigned int* pStatus)=0;
	/*! Called once the exposure is complete.  Allows software implementation of downloading since for every CCStartExposure there is a corresponding CCEndExposure.*/
	virtual int CCEndExposure(const enumCameraIndex& Cam, const enumWhichCCD CCD, const bool& bWasAborted, const bool& bLeaveShutterAlone)=0;

	/*!Return one line of the image. /sa readoutMode().*/
	virtual int CCReadoutLine(const enumCameraIndex& Cam, const enumWhichCCD& CCD, const int& pixelStart, const int& pixelLength, const int& nReadoutMode, unsigned char* pMem)=0;
	/*!Dump n lines to speed up download when a subframe is present.*/
	virtual int CCDumpLines(const enumCameraIndex& Cam, const enumWhichCCD& CCD, const int& nReadoutMode, const unsigned int& lines)=0;

	/*!Return the image. /sa readoutMode().*/
	virtual int CCReadoutImage(const enumCameraIndex& Cam, const enumWhichCCD& CCD, const int& nWidth, const int& nHeight, const int& nMemWidth, unsigned char* pMem)=0;

	/*!Turn off or on temperature regulation.*/
	virtual int CCRegulateTemp(const bool& bOn, const double& dTemp)=0;
	/*!Return the temperature and corresponding status.*/
	virtual int CCQueryTemperature(double& dCurTemp, double& dCurPower, char* lpszPower, const int nMaxLen, bool& bCurEnabled, double& dCurSetPoint)=0;
	/*!When possible, return a recommended temperature setpoint. /param dRecSP The recommended temperature setpoint or set to 100 if unable to recommend one.*/
	virtual int	CCGetRecommendedSetpoint(double& dRecSP)=0;
	/*!Turn the fan of off.*/
	virtual int	CCSetFan(const bool& bOn)=0;

	/*!Turn on a camera relay or relays, in 1/100's of a second. Called when autoguiding.*/
	virtual int CCActivateRelays(const int& nXPlus, const int& nXMinus, const int& nYPlus, const int& nYMinus, const bool& bSynchronous, const bool& bAbort, const bool& bEndThread)=0;

	/*!SBIG specific for controlling internal filter wheels.*/
	virtual int CCPulseOut(unsigned int nPulse, bool bAdjust, const enumCameraIndex& Cam)=0;

	/*!Manually control the shutter.*/
	virtual int CCSetShutter(bool bOpen)=0;
	/*!Deprecated.  Called after download to resynchronize the PC clock to the real-time clock.*/
	virtual int CCUpdateClock(void)=0;

	/*!Software Bisque only.*/
	virtual int CCSetImageProps(const enumCameraIndex& Camera, const enumWhichCCD& CCD, const int& nReadOut, void* pImage)=0;	
	/*!Return the camera's full dynamic range, required for @Focus and @Focus2 to work.*/
	virtual int CCGetFullDynamicRange(const enumCameraIndex& Camera, const enumWhichCCD& CCD, unsigned long& dwDynRg)=0;
	
	/*!Called before download.*/
	virtual void CCBeforeDownload(const enumCameraIndex& Cam, const enumWhichCCD& CCD)=0;
	/*!Called after download.*/
	virtual void CCAfterDownload(const enumCameraIndex& Cam, const enumWhichCCD& CCD)=0;

	//Implemenation below here
protected:
	bool		m_bLinked;
	enumCameraIndex	m_Camera;

//Base class End Copy Above here
};


//Added another export to CamAPI
//Not requried so its backward compatible
//Attempted to add a ptr func to it in CamApi.h, but got bad debug delete error 
//(i.e. the CamAPI object header, was different from what CCDSoft allocated.
//This gives TheSkyX a way to talk to a plug in imp outside the context of the above object
//Mostly its for Photometric camera that has NFrames
#ifdef Q_WS_WIN
typedef __declspec(dllexport) int (*pfCamAPIDoCommand)(const int& nCmd, const int& nWhichCCD, int* pnArg1, char* lpszInOutStrArg, const int& nInArgStrArgSize);
#else
typedef int (*pfCamAPIDoCommand)(const int& nCmd, const int& nWhichCCD, int* pnArg1, char* lpszInOutStrArg, const int& nInArgStrArgSize);
#endif

#endif //_CameraDriverInterFace_H_