VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Camera"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Description = "ASCOM Camera driver for CCD Simulator"
'---------------------------------------------------------------------
'   ==========
'   CAMERA.CLS
'   ==========
'
' Implementation of ASCOM CCD simulator driver
'
' Written:  06-Feb-02   Matthias Busch <Matthias.Busch@easysky.de>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 06-Feb-02 mab     Initial edit
' 20-Jul-03 mab     Conforms to http://ascom-standards.org/CamProp3.html
' 25-Feb-07 mab     Adheres to the adopted Camera Driver Interface V1
' 14-Oct-07 rbd     5.0.1 - Added Platform 5 early binding interface
'                   Fix ImageArray to return Variant array of Long
'                   instead of Integer. This was causing MaxIm to fail
'                   with a runtime error. Change enum constants to
'                   match ICamera, remove enums here.
' 08-May-08 pwgs    5.0.2 Changed ImageArray and ImageArrayVariant to return in format Arr(x,y)
'                   Added full well apcity constant
'                   Electrons per adu now calculated from full well capacity
'                   Added check so that imageready is false until first image has been taken
'                   Added check for binning set to less than 1
'                   Fixed last exposure start time to return a value rather than a null string
'                   Added check for image size < 1
'                   Added LastError support
'                   Introduced limits (-200,+50)to allowable ccd cooler temperature set points
'                   Added code to throw an error if LastExposureDuration or StartTime are called before
'                     an image is taken
'                   Added throw a not implemented exception when canpulseguide is false
'                   Corrected Get ICamera_NumX() to correctly call NumX property
'                   Fixed LastExposureTime to report UTC time per FITS specification
'                   Fixed BinX and BinY Let to use integer division to calculate effective number of binned pixels
'                     binning * size for bin > 1 should always be <= size at bin = 1 otherwise you have created pixels
'                   Set CanGetCoolerPower to true as driver can return CoolerPower
'                   Moved StartXY and NumXY parameter value checks into the startexpopsure method
' 27-May-08 pwgs    5.0.3 Restored BinXY limit checks when parameters are set
'                   Added version number to description field
' 28-May-08 pwgs    5.0.4 Sent source to Larry Weber
' 20-Jul-08 lfw     5.0.5 Added stars to image with subs GenerateStars and GenerateBackgroundNoise
' 09-Aug-08 pwgs    5.0.6 Fixed issue with malformed self generated stars
'---------------------------------------------------------------------

Option Explicit

'-----------------
Implements ICamera      ' Implementation at end, calls public members
'-----------------

Private Const ID As String = "CCDSimulator.Camera"
Private Const DESC As String = "ASCOM CCD camera simulator"

Private Const FULL_WELL_CAPACITY As Double = 300000# 'pwgs CCD full well capacity
Private Const MAX_COOLER_TEMPERATURE As Double = 50# 'pwgs Highest allowable ccd cooler set point
Private Const MIN_COOLER_TEMPERATURE As Double = -200# 'pwgs Lowest allowable cooler set point
Private Const PI = 3.14159265358979    'lfw1

Private m_Util As DriverHelper.Util
Private m_Profile As DriverHelper.Profile
Private m_Util2 As DriverHelper2.Util 'pwgs access to helper2 to get UTC time

Private m_iBinX As Integer
Private m_iBinY As Integer
Private m_iCameraXSize As Long
Private m_iCameraYSize As Long
Private m_iCameraXSizeBinned As Long
Private m_iCameraYSizeBinned As Long
Private m_dPixelSizeX As Double
Private m_dPixelSizeY As Double
Private m_iStartX As Long
Private m_iStartY As Long
Private m_iNumX As Long
Private m_iNumY As Long
Private m_iMaxBinX As Integer
Private m_iMaxBinY As Integer
Private m_iStarImage() As Long   'lfw1

Private m_bConnected As Boolean
Private m_bCoolerOn As Boolean
Private m_bLightFrameEnabled As Boolean  'lfw1
Private m_dSetCCDTemperature As Double
Private m_dLastExposureDuration  As Double
Private m_bTDI As Boolean
Private m_bFirstImageTaken As Boolean 'pwgs Added to record whether first exposure has occured
Private m_sLastExposureStartTime As String 'pwgs Added to hold last exposure start time
Private m_sLastError As String 'pwgs Added to hold last error status
Private m_bShowStars  'True if stars are to be shown  'lfw1

Private m_frmTimer As frmTimer


'Public Enum CameraStates
'    stateIdle = 0      ' At idle state, available to start exposure
'    stateWaiting = 1   ' Exposure started but waiting (for shutter, trigger, filter wheel, etc.)
'    stateExposing = 2  ' Exposure currently in progress
'    stateReading = 3   ' CCD array is being read out (digitized)
'    stateDownload = 4  ' Downloading data to PC
'    stateError = 5     ' Camera error condition serious enough to prevent
'End Enum
'
'
'' Note: directions are nominal and may depend on exact mount wiring.  guideNorth
'' must be opposite guideSouth, and guideEast must be opposite guideWest.
'
'Public Enum GuideDirections
'    guideNorth = 0     ' North (+ declination/elevation)
'    guideSouth = 1     ' South (- declination/elevation)
'    guideEast = 2      ' East (+ right ascension/azimuth)
'    guideWest = 3      ' West (+ right ascension/azimuth)
'End Enum


Private Sub Class_Initialize()

   Set m_Util = New DriverHelper.Util
   Set m_Util2 = New DriverHelper2.Util 'pwgs Create helper2 object
   Set m_Profile = New DriverHelper.Profile
   m_Profile.DeviceType = "Camera"      ' We're a Camera driver
   m_Profile.Register ID, DESC
  
   Set m_frmTimer = New frmTimer

   ReadConfig
    
   ' set default values

   m_iBinX = 1
   m_iBinY = 1
   
   m_iMaxBinX = 4
   m_iMaxBinY = 4

   m_iStartX = 0
   m_iStartY = 0
   
   m_iNumX = m_iCameraXSize
   m_iNumY = m_iCameraYSize

   m_iCameraXSizeBinned = m_iCameraXSize / m_iBinX
   m_iCameraYSizeBinned = m_iCameraYSize / m_iBinY

   m_bConnected = False
   m_bCoolerOn = False
    
   m_dSetCCDTemperature = 0
   m_dLastExposureDuration = 0

   m_bTDI = False
   m_frmTimer.m_bExposing = False
   m_bFirstImageTaken = False
   
   
End Sub

Private Sub Class_Terminate()
    
   On Error Resume Next
   Set m_Util = Nothing
   Set m_Util2 = Nothing 'pwgs Destroy helper2 object
    
   Set m_frmTimer = Nothing
    
End Sub
'
' CheckConnected() - Raise an error if the scope is not connected
'
Private Sub CheckConnected()

    If Not m_bConnected Then
        Err.Raise SCODE_NOT_CONNECTED, _
                    ERR_SOURCE, _
                    MSG_NOT_CONNECTED
    End If
    
End Sub

Private Sub ReadConfig()
    Dim buf As String
    
    buf = m_Profile.GetValue(ID, "DetectorWidth")
    If buf = "" Then
        m_iCameraXSize = 512
    Else
        m_iCameraXSize = CInt(buf)
    End If
    
    buf = m_Profile.GetValue(ID, "DetectorHeight")
    If buf = "" Then
        m_iCameraYSize = 512
    Else
        m_iCameraYSize = CInt(buf)
    End If

    buf = m_Profile.GetValue(ID, "PixelWidth")
    If buf = "" Then
        m_dPixelSizeX = 24
    Else
        m_dPixelSizeX = CInt(buf)
    End If
    
    buf = m_Profile.GetValue(ID, "PixelHeight")
    If buf = "" Then
        m_dPixelSizeY = 24
    Else
        m_dPixelSizeY = CInt(buf)
    End If
    
    buf = m_Profile.GetValue(ID, "ShowStars")                   'lfw1
    If buf = "" Then                                            'pwgs fix first time usage
        buf = "0"                                               'pwgs
        m_Profile.WriteValue ID, "ShowStars", "0"               'pwgs
    End If                                                      'pwgs
    If buf = 1 Then                                             'lfw1
        m_bShowStars = True                                     'lfw1
    Else                                                        'lfw1
        m_bShowStars = False                                    'lfw1
    End If                                                      'lfw1
   
End Sub

' ----------------------------
'  properties
' ----------------------------

Public Property Get BinX() As Integer

   CheckConnected
   
   BinX = m_iBinX

End Property

Public Property Let BinX(ByVal newVal As Integer)

   CheckConnected

   'pwgs added test for binning < 1
   If (newVal > m_iMaxBinX) Or (newVal < 1) Then
      Err.Raise SCODE_BINNING_FACTOR_INAPPROPRIATE, ERR_SOURCE, MSG_BINNING_FACTOR_INAPPROPRIATE
   End If
   
   m_iBinX = newVal

End Property

Public Property Get BinY() As Integer

   CheckConnected

   BinY = m_iBinY
    
End Property

Public Property Let BinY(ByVal newVal As Integer)

   CheckConnected

   'pwgs added test for binning < 1
   If (newVal > m_iMaxBinY) Or (newVal < 1) Then
      Err.Raise SCODE_BINNING_FACTOR_INAPPROPRIATE, ERR_SOURCE, MSG_BINNING_FACTOR_INAPPROPRIATE
   End If
   
   m_iBinY = newVal
 
End Property

Public Property Get CameraState() As CameraStates

    If m_frmTimer.m_bExposing Then
       CameraState = cameraExposing
    Else
       CameraState = cameraIdle
    End If

End Property

Public Property Get CameraXSize() As Long

   CameraXSize = m_iCameraXSize
    
End Property

Public Property Get CameraYSize() As Long

   CameraYSize = m_iCameraYSize

End Property

Public Property Get CanAbortExposure() As Boolean

   CanAbortExposure = True

End Property

Public Property Get CanAsymmetricBin() As Boolean

   CanAsymmetricBin = True

End Property

Public Property Get CanGetCoolerPower() As Boolean

   CanGetCoolerPower = True
    
End Property

Public Property Get CanPulseGuide() As Boolean

   CanPulseGuide = False

End Property


Public Property Get CanSetCCDTemperature() As Boolean

   CanSetCCDTemperature = True

End Property

Public Property Get CanStopExposure() As Boolean

   CanStopExposure = True

End Property

Public Property Get CCDTemperature() As Double
   On Error GoTo errhandler
   CheckConnected

   CCDTemperature = m_dSetCCDTemperature + (2 * Rnd) - 1

   Exit Property
errhandler:
End Property

Public Property Let Connected(ByVal newVal As Boolean)
   
   m_bConnected = newVal

End Property

Public Property Get Connected() As Boolean

   Connected = m_bConnected

End Property

Public Property Let CoolerOn(ByVal newVal As Boolean)

   CheckConnected
   
   m_bCoolerOn = newVal
    
End Property

Public Property Get CoolerOn() As Boolean

   CheckConnected
   
   CoolerOn = m_bCoolerOn

End Property

' Returns the present cooler power level, in percent.
' Returns zero if CoolerOn is False.

Public Property Get CoolerPower() As Double

   CheckConnected
   
   If m_bCoolerOn Then
      CoolerPower = 100
   Else
      CoolerPower = 0
   End If

End Property

' Returns a description of the camera model, such as manufacturer and model
' number. Any ASCII characters may be used. The string shall not exceed 68
' characters (for compatibility with FITS headers).

Public Property Get Description() As String

    Description = "ASCOM CCD camera simulator - Version " & _
                App.Major & "." & App.Minor & "." & App.Revision
    
End Property

' Returns the gain of the camera in photoelectrons per A/D unit. (Some cameras have
' multiple gain modes; these should be selected via the SetupDialog and thus are
' static during a session.)

Public Property Get ElectronsPerADU() As Double

    'pwgs Changed from value of 0 to value derrived from full well capacity
    ElectronsPerADU = FULL_WELL_CAPACITY / 65535#

End Property

' Reports the full well capacity of the camera in electrons, at the current camera
' settings (binning, SetupDialog settings, etc.)

Public Property Get FullWellCapacity() As Double

    'pwgs Changed to FULL_WELL_CAPACITY constant
    FullWellCapacity = FULL_WELL_CAPACITY ' electrons

End Property

' If True, the camera has a mechanical shutter. If False, the camera does not have
' a shutter.  If there is no shutter, the StartExposure command will ignore the
' Light parameter.

Public Property Get HasShutter() As Boolean

   HasShutter = True
    
End Property

' Returns the current heat sink temperature (called "ambient temperature" by some
' manufacturers) in degrees Celsius. Only valid if CanControlTemperature is True.

Public Property Get HeatSinkTemperature() As Double

    ' todo
    HeatSinkTemperature = -4.6

End Property

' Returns a safearray of Long of size NumX * NumY containing the pixel values from
' the last exposure. The application must inspect the Safearray parameters to
' determine the dimensions. Note: if NumX or NumY is changed after a call to
' StartExposure it will have no effect on the size of this array. This is the
' preferred method for programs (not scripts) to download iamges since it requires
' much less memory.
'
' For color or multispectral cameras, will produce an array of NumX * NumY *
' NumPlanes.  If the application cannot handle multispectral images, it should use
' just the first plane.

Public Property Get ImageArray() As Variant

   CheckConnected
   
   Dim aImage() As Long
   'pwgs changed next line ReDim Preserve aImage(0 To m_iNumY - 1, 0 To m_iNumX - 1)
   ReDim Preserve aImage(0 To m_iNumX - 1, 0 To m_iNumY - 1)
   
   'pwgs Added check for loading image array before first image taken
   If Not m_bFirstImageTaken Then
        Err.Raise SCODE_IMAGE_NOT_TAKEN, ERR_SOURCE, MSG_IMAGE_NOT_TAKEN
   End If
   
   Randomize

   ' dummy background noise
   
   Const iBackGround As Integer = 4000
   Const iNoiseRange As Integer = 400
   
   Dim X, y As Long
   
   If m_bShowStars = True Then   'Show stars                                'lfw1
     GenerateStars m_dLastExposureDuration, m_iBinX, m_iBinY                'lfw1
        For X = 0 To m_iNumX - 1                                            'lfw1
            For y = 0 To m_iNumY - 1                                        'lfw1
                aImage(X, y) = m_iStarImage(X + m_iStartX, y + m_iStartY)   'lfw1
            Next                                                            'lfw1
        Next                                                                'lfw1
   
   Else    'Just show random noise                                          'lfw1
        For X = 0 To m_iNumX - 1
            For y = 0 To m_iNumY - 1
      'pwgs Changed next line to swap x and y axes
                aImage(X, y) = Int(iNoiseRange * Rnd + iBackGround)
            Next
        Next
   End If                                                                   'lfw1
   
   ImageArray = aImage

End Property

' Returns a safearray of Variant of size NumX * NumY containing the pixel values
' from the last exposure. The application must inspect the Safearray parameters to
' determine the dimensions. Note: if NumX or NumY is changed after a call to
' StartExposure it will have no effect on the size of this array. This property
' should only be used from scripts due to the extremely high memory utilization on
' large image arrays (26 bytes per pixel). Pixels values should be in Short, Long,
' or Double format.
'
' For color or multispectral cameras, will produce an array of NumX * NumY *
' NumPlanes.  If the application cannot handle multispectral images, it should use
' just the first plane.

Public Property Get ImageArrayVariant() As Variant

   CheckConnected
   
   Dim aImage() As Variant
   ReDim Preserve aImage(0 To m_iNumX - 1, 0 To m_iNumY - 1)

   Randomize

   ' dummy background noise
   
   Const iBackGround As Integer = 2800
   Const iNoiseRange As Integer = 300


   Dim X, y As Long
   
   If m_bShowStars = True Then   'Show stars                                'lfw1
     GenerateStars m_dLastExposureDuration, m_iBinX, m_iBinY                'lfw1
        For X = 0 To m_iNumX - 1                                            'lfw1
            For y = 0 To m_iNumY - 1                                        'lfw1
                aImage(X, y) = m_iStarImage(X + m_iStartX, y + m_iStartY)   'lfw1
            Next                                                            'lfw1
        Next                                                                'lfw1
   
   Else    'Just show random noise                                          'lfw1
        For X = 0 To m_iNumX - 1
            For y = 0 To m_iNumY - 1
                aImage(X, y) = CLng(iNoiseRange * Rnd + iBackGround)
            Next
        Next

   End If                                                                   'lfw1
   
   ImageArrayVariant = aImage

End Property

' If True, there is an image from the camera available. If False, no image
' is available and attempts to use the ImageArray method will produce an
' exception.

Public Property Get ImageReady() As Boolean

   CheckConnected

   ImageReady = (Not m_frmTimer.m_bExposing) And m_bFirstImageTaken
    
End Property

' If True, pulse guiding is in progress. Required if the PulseGuide() method
' (which is non-blocking) is implemented. See the PulseGuide() method.

Public Property Get IsPulseGuiding() As Boolean

   CheckConnected

   IsPulseGuiding = False

End Property

' Reports the last error condition reported by the camera hardware or communications
' link.  The string may contain a text message or simply an error code.  The error
' value is cleared the next time any method is called.

Public Property Get LastError() As String

   ' pwgs Added last error support
   If m_sLastError <> "" Then ' An error has occured so report it
        LastError = m_sLastError
   Else ' No error so throw an exception
        Err.Raise SCODE_NO_ERROR, ERR_SOURCE, MSG_NO_ERROR
   End If

End Property

' Reports the actual exposure duration in seconds (i.e. shutter open time).  This
' may differ from the exposure time requested due to shutter latency, camera timing
' precision, etc.

Public Property Get LastExposureDuration() As Double

    'pwgs Added code to throw an error if called before first image is taken
    If m_bFirstImageTaken Then 'An image has been taken so return value
         LastExposureDuration = m_dLastExposureDuration
    Else 'No image taken yet so throw an error
         Err.Raise SCODE_IMAGE_NOT_TAKEN, ERR_SOURCE, MSG_IMAGE_NOT_TAKEN
    End If

End Property

' Reports the actual exposure start in the FITS-standard
' CCYY-MM-DDThh:mm:ss[.sss...] format.

Public Property Get LastExposureStartTime() As String

    ' Reports the actual exposure start in the FITS-standard
    ' CCYY-MM-DDThh:mm:ss[.sss...] format.
    
        'pwgs Added code to throw an error if called before first image is taken
    If m_bFirstImageTaken Then 'An image has been taken so return value
        'pwgs Updated to read variable not return a null string
        LastExposureStartTime = m_sLastExposureStartTime
    Else 'No image taken yet so throw an error
        Err.Raise SCODE_IMAGE_NOT_TAKEN, ERR_SOURCE, MSG_IMAGE_NOT_TAKEN
    End If

End Property

' Reports the maximum ADU value the camera can produce.

Public Property Get MaxADU() As Long

    MaxADU = 65535

End Property

' If AsymmetricBinning = False, returns the maximum allowed binning factor. If
' AsymmetricBinning = True, returns the maximum allowed binning factor for the X
' axis.

Public Property Get MaxBinX() As Integer

   MaxBinX = m_iMaxBinX

End Property

' If AsymmetricBinning = False, equals MaxBinX. If AsymmetricBinning = True,
' returns the maximum allowed binning factor for the Y axis.

Public Property Get MaxBinY() As Integer

   MaxBinY = m_iMaxBinY

End Property

' Sets the subframe width. Also returns the current value.  If binning is active,
' value is in binned pixels.  No error check is performed when the value is set.
' Should default to CameraXSize.

Public Property Let NumX(ByVal newVal As Long)

   CheckConnected

   m_iNumX = newVal

End Property

Public Property Get NumX() As Long

   CheckConnected

   NumX = m_iNumX
    
End Property

' Sets the subframe height. Also returns the current value.  If binning is active,
' value is in binned pixels.  No error check is performed when the value is set.
' Should default to CameraYSize.

Public Property Let NumY(ByVal newVal As Long)

   CheckConnected

    m_iNumY = newVal

End Property

Public Property Get NumY() As Long

   CheckConnected

   NumY = m_iNumY
    
End Property

' Returns the width of the CCD chip pixels in microns, as provided by the camera
' driver.

Public Property Get PixelSizeX() As Double

   PixelSizeX = m_dPixelSizeX

End Property

' Returns the height of the CCD chip pixels in microns, as provided by the camera
' driver.

Public Property Get PixelSizeY() As Double

   PixelSizeY = m_dPixelSizeY

End Property

' Sets the camera cooler setpoint in degrees Celsius, and returns the current
' setpoint.
'
' Note:  camera hardware and/or driver should perform cooler ramping, to prevent
' thermal shock and potential damage to the CCD array or cooler stack.

Public Property Get SetCCDTemperature() As Double

   CheckConnected

   SetCCDTemperature = m_dSetCCDTemperature
    
End Property

Public Property Let SetCCDTemperature(ByVal newVal As Double)

   CheckConnected

   'pwgs Added checks for temperature within allowable limits
   If (newVal < MIN_COOLER_TEMPERATURE) Or (newVal > MAX_COOLER_TEMPERATURE) Then
      m_sLastError = MSG_SETCCDTEMPERATURE_INAPPROPRIATE & " " & newVal
      Err.Raise SCODE_SETCCDTEMPERATURE_INAPPROPRIATE, ERR_SOURCE, MSG_SETCCDTEMPERATURE_INAPPROPRIATE & " " & newVal
   End If
   
   m_dSetCCDTemperature = newVal

End Property

' Sets the subframe start position for the X axis (0 based). Also returns the
' current value.  If binning is active, value is in binned pixels.

Public Property Let StartX(ByVal newVal As Long)

   CheckConnected

   m_iStartX = newVal

End Property

Public Property Get StartX() As Long

   CheckConnected

   StartX = m_iStartX
    
End Property

' Sets the subframe start position for the Y axis (0 based). Also returns the
' current value.  If binning is active, value is in binned pixels.

Public Property Let StartY(ByVal newVal As Long)

   CheckConnected

   m_iStartY = newVal

End Property

Public Property Get StartY() As Long

   CheckConnected

   StartY = m_iStartY

End Property

' ----------------------------
'  methods
' ----------------------------

' Aborts the current exposure, if any, and returns the camera to Idle state.

Public Sub AbortExposure()
   
   StopExposure         ' Same  as StopExposure in this simulator

End Sub

' This method may return immediately after the move has started, in which case
' back-to-back dual axis pulse-guiding can be supported. Use the IsPulseGuiding
' property to detect when all moves have completed.

Public Sub PulseGuide(ByVal Direction As GuideDirections, ByVal Duration As Long)
    
    ' todo
    'pwgs reset LastError
    m_sLastError = ""
    
    'pwgs Added code to throw an exception as pulseguide isn't supported
    If CanPulseGuide Then 'Do it!
        'pwgs Not implemented
    Else ' Throw an error
        m_sLastError = MSG_NOT_IMPLEMENTED
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, MSG_NOT_IMPLEMENTED
    End If

End Sub

' Launches a configuration dialog box for the driver.  The call will not return
' until the user clicks OK or cancel manually.

Public Sub SetupDialog()

    '
    ' Must set member vars first, as the lblDriverInfo change
    ' loads for the, executing the OnLoad event, which depends
    ' on the member vars.
    '
    
    'pwgs reset LastError
    m_sLastError = ""
    
    Set frmSetup.m_Profile = m_Profile
    frmSetup.m_DriverID = ID                    ' Form uses Helper.Profile methods
    frmSetup.Show 1
    If Not frmSetup.m_Cancel Then ReadConfig
    Unload frmSetup

End Sub

' Starts an exposure. Use ImageReady to check when the exposure is complete.

Public Sub StartExposure(ByVal Duration As Double, ByVal Light As Boolean)

   'pwgs reset LastError
   m_sLastError = ""
    
   CheckConnected
   
   'pwgs Added test for negative duration
   If Duration < 0# Then
        m_sLastError = MSG_INVALID_DURATION
        Err.Raise SCODE_INVALID_DURATION, ERR_SOURCE, MSG_INVALID_DURATION
   End If
   
   'pwgs added  binning tests
   If (m_iBinX > m_iMaxBinX) Or (m_iBinX < 1) Then
      m_sLastError = MSG_BINNING_FACTOR_INAPPROPRIATE & " " & m_iBinX
      Err.Raise SCODE_BINNING_FACTOR_INAPPROPRIATE, ERR_SOURCE, MSG_BINNING_FACTOR_INAPPROPRIATE & " " & m_iBinX
   End If
   If (m_iBinY > m_iMaxBinY) Or (m_iBinY < 1) Then
      m_sLastError = MSG_BINNING_FACTOR_INAPPROPRIATE & " " & m_iBinY
      Err.Raise SCODE_BINNING_FACTOR_INAPPROPRIATE, ERR_SOURCE, MSG_BINNING_FACTOR_INAPPROPRIATE & " " & m_iBinY
   End If
   
   'pwgs Moved next two tests here from property let
   'pwgs changed / to \ in next line for integer division
   'size of binning * size for bin > 1 should always be <= size at bin = 1
   m_iCameraXSizeBinned = m_iCameraXSize \ m_iBinX
  'pwgs changed / to \ in next line for integer division
   'size of binning * size for bin > 1 should always be <= size at bin = 1
   m_iCameraYSizeBinned = m_iCameraYSize \ m_iBinY

   
   'pwgs Moved these checks here from let properties
   
   'pwgs Added check for size < 1
   If (m_iNumX > (m_iCameraXSizeBinned - m_iStartX)) Or (m_iNumX < 1) Then
      m_sLastError = MSG_SUBFRAME_INAPPROPRIATE & " " & m_iNumX
      Err.Raise SCODE_SUBFRAME_INAPPROPRIATE, ERR_SOURCE, MSG_SUBFRAME_INAPPROPRIATE & " " & m_iNumX
   End If
   
     'pwgs Added check for size < 1
   If (m_iNumY > (m_iCameraYSizeBinned - m_iStartY)) Or (m_iNumY < 1) Then
      m_sLastError = MSG_SUBFRAME_INAPPROPRIATE & " " & m_iNumY
      Err.Raise SCODE_SUBFRAME_INAPPROPRIATE, ERR_SOURCE, MSG_SUBFRAME_INAPPROPRIATE & " " & m_iNumY
   End If

      'pwgs Added check for start < 0
   If (m_iStartX >= m_iCameraXSizeBinned) Or (m_iStartX < 0) Then
      m_sLastError = MSG_SUBFRAME_INAPPROPRIATE & " " & m_iStartX
      Err.Raise SCODE_SUBFRAME_INAPPROPRIATE, ERR_SOURCE, MSG_SUBFRAME_INAPPROPRIATE & " " & m_iStartX
   End If

      'pwgs Added check for start < 0
   If (m_iStartY >= m_iCameraYSizeBinned) Or (m_iStartY < 0) Then
      m_sLastError = MSG_SUBFRAME_INAPPROPRIATE & " " & m_iStartY
      Err.Raise SCODE_SUBFRAME_INAPPROPRIATE, ERR_SOURCE, MSG_SUBFRAME_INAPPROPRIATE & " " & m_iStartY
   End If


   m_bLightFrameEnabled = Light         'lfw1
   m_frmTimer.m_bExposing = True
   
   m_frmTimer.Timer1.Interval = Duration * 1000
   m_frmTimer.Timer1.Enabled = True
   
   m_dLastExposureDuration = Duration
   'pwgs exposure start time formatting added
   m_sLastExposureStartTime = Format(m_Util2.UTCDate, "yyyy-mm-ddThh:mm:ss")
   m_bFirstImageTaken = True
   
End Sub

' Stops the current exposure, if any.  If an exposure is in progress, the readout
' process is initiated.  Ignored if readout is already in process.

Public Sub StopExposure()

   CheckConnected
   
   m_frmTimer.m_bExposing = False
   m_frmTimer.Timer1.Enabled = False

End Sub

'-----------------------------------------------------------
' properties and methods
' not mentioned in http://ascom-standards.org/CamProp3.html
'-----------------------------------------------------------

Public Property Get DriverInfo() As String
    '
    ' Use the Project/Properties sheet, Make tab, to set these
    ' items. That way they will show in the Version tab of the
    ' Explorer property sheet, and the exact same data will
    ' show in Camera.DriverInfo.
    '
    DriverInfo = App.FileDescription & " " & _
                App.Major & "." & App.Minor & "." & App.Revision
    If App.CompanyName <> "" Then _
        DriverInfo = DriverInfo & vbCrLf & App.CompanyName
    If App.LegalCopyright <> "" Then _
        DriverInfo = DriverInfo & vbCrLf & App.LegalCopyright
    If App.Comments <> "" Then _
        DriverInfo = DriverInfo & vbCrLf & App.Comments
    
End Property

Public Property Get Name() As String
        
   Name = "ASCOM CCD camera simulator"

End Property


Private Sub GenerateStars(ExposureInterval As Double, BinX As Integer, BinY As Integer)       'A new sub   'lfw1
'This generates an artificial field of 5 gausian stars of varying magnitude
'INPUTS:  ExposureInterval in seconds, BinX, BinY
'OUTPUT: m_iStarImage(x,y) array of size x = 0 to Int(m_iCameraXSize/BinX)-1, y = 0 to Int(m_iCameraYSize/BinY)-1
    
' ------ USER CONFIGURATION SECTION -----
    Const limmag = 17#                                  ' limiting mag
    Const lmexp = 120                                   ' exposure interval to reach limiting mag
    Const loflux = 300#                                 ' flux at limiting mag (arbitrary!)
    Const fwhm = 5#                                     ' FWHM arcsec
    Const ArcsecPerPixel = 1.5                          ' Arc seconds per pixel
    Const BiasLevel = 1000                              ' Bias Level in ADU units
    Const NoiseSigma = 15                               ' Noise standard deviation in ADU units
' ------ END CONFIGURATION SECTION -----
        
    Const starX = 1
    Const starY = 2
    Const Magnitude = 3
    
    Dim R1 As Double
    Dim R2 As Double
    Dim i0 As Double
    Dim intensity As Double
    Dim X0 As Double
    Dim X As Double
    Dim Y0 As Double
    Dim y As Double
    Dim s6 As Long
    Dim dx As Long
    Dim dy As Long
    Dim pix As Double
    Dim oldpix As Double
    Dim newpix As Double
    Dim ImageRows As Long
    Dim ImageColumns As Long
    Dim BinnedXIndex As Long
    Dim BinnedYIndex As Long
    Dim i As Long
    Dim star(1 To 5, 1 To 3) As Integer 'pwgs Changed from double to integer to fix rounding issues in coorodinate calculation
    
' Generate field of 5 artificial stars
    star(1, starX) = m_iCameraXSize / 2 - 1
    star(1, starY) = m_iCameraYSize / 2 - 1
    star(1, Magnitude) = 4
    star(2, starX) = m_iCameraXSize / 4 - 1
    star(2, starY) = m_iCameraYSize / 4 - 1
    star(2, Magnitude) = 5
    star(3, starX) = 3 * m_iCameraXSize / 4 - 1
    star(3, starY) = 3 * m_iCameraYSize / 4 - 1
    star(3, Magnitude) = 3
    star(4, starX) = 3 * m_iCameraXSize / 4 - 1
    star(4, starY) = m_iCameraYSize / 4 - 1
    star(4, Magnitude) = 2
    star(5, starX) = m_iCameraXSize / 4 - 1
    star(5, starY) = 3 * m_iCameraYSize / 4 - 1
    star(5, Magnitude) = 6

    GenerateBackgroundNoise BiasLevel, NoiseSigma, BinX, BinY
    
    If m_bLightFrameEnabled = True Then   ' Add Gaussian stars to image
        R1 = (fwhm / ArcsecPerPixel) / 2.3548       ' Sigma of Gaussian star profile, pix
        i0 = (ExposureInterval * loflux) / ((2# * PI) ^ 0.5 * R1 * lmexp) ' Intensity of center pixel
        
        For i = 1 To 5
            X0 = star(i, starX)
            Y0 = star(i, starY)
            intensity = i0 * 10 ^ (0.4 * (limmag - star(i, Magnitude)))
            s6 = Round(6# * R1)                            ' Integrate out to at least 6 sigma
            For dx = -s6 To s6
                For dy = -s6 To s6
                    ImageRows = m_iCameraYSize
                    ImageColumns = m_iCameraXSize
                    X = Round(X0 + dx)
                    y = Round(Y0 + dy)
                    If (X > s6 And X < (ImageColumns - s6) And y > s6 And y < (ImageRows - s6)) Then
                        R2 = ((X - X0) * (X - X0) + (y - Y0) * (y - Y0)) ^ 0.5
                        pix = (intensity / (6.28 * R1 * R1)) * Exp(-0.5 * (R2 / R1) * (R2 / R1))
                        BinnedXIndex = Int((ImageColumns - 1 - X) / BinX)
                        BinnedYIndex = Int((ImageRows - 1 - y) / BinY)
                        oldpix = m_iStarImage(BinnedXIndex, BinnedYIndex)
                        newpix = oldpix + pix
                        If (newpix > 65534) Then newpix = 65534         ' saturation limit
                        If (newpix < 0) Then newpix = 0
                        m_iStarImage(BinnedXIndex, BinnedYIndex) = newpix
                    End If
                Next dy
            Next dx
        Next i
    End If

End Sub


Sub GenerateBackgroundNoise(Offset As Single, Sigma As Single, BinX As Integer, BinY As Integer)   'A new sub   'lfw1
'This generates Gausian Noise for the image background with an Offest and a Standard Deviation: Sigma
'INPUTS:  Offset, Sigma, BinX, BinY
'OUTPUT: m_iStarImage(x,y) array of size x = 0 to Int(m_iCameraXSize/BinX)-1, y = 0 to Int(m_iCameraYSize/BinY)-1

' This uses the very powerful freeware Universal Non-Uniform Random number generator: UNU.RAN developed and maintained by:
' Josef Leydold, Wolfgang Hörmann, Erich Janka, Günter Tirler and Roman Karawatzki
' http://statistik.wu-wien.ac.at/unuran/index.html

    Dim i As Long
    Dim j As Long
    Dim ReturnInt As Integer
    
' Setup UNURAN1 Component for Normal distribution of Mean: Offest and Standard Deviation: Sigma
    ReturnInt = unuran_init("normal(" & CStr(Offset) & "," & CStr(Sigma) & ")")
    ' Generate Gaussian Noise for image
    ReDim m_iStarImage(Int(m_iCameraXSize / BinX) - 1, Int(m_iCameraYSize / BinY) - 1)
    For i = 0 To Int(m_iCameraXSize / BinX) - 1
        For j = 0 To Int(m_iCameraYSize / BinY) - 1
            m_iStarImage(i, j) = unuran_sample
        Next j
    Next i
    
    
End Sub



' ========================================
' Implemenentation of ITelescope interface
' ========================================

Private Sub ICamera_AbortExposure()
    AbortExposure
End Sub

Private Property Let ICamera_BinX(ByVal RHS As Integer)
    BinX = RHS
End Property

Private Property Get ICamera_BinX() As Integer
    ICamera_BinX = BinX
End Property

Private Property Let ICamera_BinY(ByVal RHS As Integer)
    BinY = RHS
End Property

Private Property Get ICamera_BinY() As Integer
    ICamera_BinY = BinY
End Property

Private Property Get ICamera_CameraState() As AscomInterfacesLib.CameraStates
    ICamera_CameraState = CameraState
End Property

Private Property Get ICamera_CameraXSize() As Long
    ICamera_CameraXSize = CameraXSize
End Property

Private Property Get ICamera_CameraYSize() As Long
    ICamera_CameraYSize = CameraYSize
End Property

Private Property Get ICamera_CanAbortExposure() As Boolean
    ICamera_CanAbortExposure = CanAbortExposure
End Property

Private Property Get ICamera_CanAsymmetricBin() As Boolean
    ICamera_CanAsymmetricBin = CanAsymmetricBin
End Property

Private Property Get ICamera_CanGetCoolerPower() As Boolean
    ICamera_CanGetCoolerPower = CanGetCoolerPower
End Property

Private Property Get ICamera_CanPulseGuide() As Boolean
    ICamera_CanPulseGuide = CanPulseGuide
End Property

Private Property Get ICamera_CanSetCCDTemperature() As Boolean
    ICamera_CanSetCCDTemperature = CanSetCCDTemperature
End Property

Private Property Get ICamera_CanStopExposure() As Boolean
    ICamera_CanStopExposure = CanStopExposure
End Property

Private Property Get ICamera_CCDTemperature() As Double
    ICamera_CCDTemperature = CCDTemperature
End Property

Private Property Let ICamera_Connected(ByVal RHS As Boolean)
    Connected = RHS
End Property

Private Property Get ICamera_Connected() As Boolean
    ICamera_Connected = Connected
End Property

Private Property Let ICamera_CoolerOn(ByVal RHS As Boolean)
    CoolerOn = RHS
End Property

Private Property Get ICamera_CoolerOn() As Boolean
    ICamera_CoolerOn = CoolerOn
End Property

Private Property Get ICamera_CoolerPower() As Double
    ICamera_CoolerPower = CoolerPower
End Property

Private Property Get ICamera_Description() As String
    ICamera_Description = Description
End Property

Private Property Get ICamera_ElectronsPerADU() As Double
    ICamera_ElectronsPerADU = ElectronsPerADU
End Property

Private Property Get ICamera_FullWellCapacity() As Double
    ICamera_FullWellCapacity = FullWellCapacity
End Property

Private Property Get ICamera_HasShutter() As Boolean
    ICamera_HasShutter = HasShutter
End Property

Private Property Get ICamera_HeatSinkTemperature() As Double
    ICamera_HeatSinkTemperature = HeatSinkTemperature
End Property

Private Property Get ICamera_ImageArray() As Variant
    ICamera_ImageArray = ImageArray
End Property

Private Property Get ICamera_ImageArrayVariant() As Variant
    ICamera_ImageArrayVariant = ImageArrayVariant
End Property

Private Property Get ICamera_ImageReady() As Boolean
    ICamera_ImageReady = ImageReady
End Property

Private Property Get ICamera_IsPulseGuiding() As Boolean
    ICamera_IsPulseGuiding = IsPulseGuiding
End Property

Private Property Get ICamera_LastError() As String
    ICamera_LastError = LastError
End Property

Private Property Get ICamera_LastExposureDuration() As Double
    ICamera_LastExposureDuration = LastExposureDuration
End Property

Private Property Get ICamera_LastExposureStartTime() As String
    ICamera_LastExposureStartTime = LastExposureStartTime
End Property

Private Property Get ICamera_MaxADU() As Long
    ICamera_MaxADU = MaxADU
End Property

Private Property Get ICamera_MaxBinX() As Integer
    ICamera_MaxBinX = MaxBinX
End Property

Private Property Get ICamera_MaxBinY() As Integer
    ICamera_MaxBinY = MaxBinY
End Property

Private Property Let ICamera_NumX(ByVal RHS As Long)
    NumX = RHS
End Property

Private Property Get ICamera_NumX() As Long
'pwgs Corrected next line
    'ICamera_NumX = ICamera_NumX
    ICamera_NumX = NumX
End Property

Private Property Let ICamera_NumY(ByVal RHS As Long)
    NumY = RHS
End Property

Private Property Get ICamera_NumY() As Long
    ICamera_NumY = NumY
End Property

Private Property Get ICamera_PixelSizeX() As Double
    ICamera_PixelSizeX = PixelSizeX
End Property

Private Property Get ICamera_PixelSizeY() As Double
    ICamera_PixelSizeY = PixelSizeY
End Property

Private Sub ICamera_PulseGuide(ByVal Direction As AscomInterfacesLib.GuideDirections, ByVal Duration As Long)
    PulseGuide Direction, Duration
End Sub

Private Property Let ICamera_SetCCDTemperature(ByVal RHS As Double)
    SetCCDTemperature = RHS
End Property

Private Property Get ICamera_SetCCDTemperature() As Double
    ICamera_SetCCDTemperature = SetCCDTemperature
End Property

Private Sub ICamera_SetupDialog()
    SetupDialog
End Sub

Private Sub ICamera_StartExposure(ByVal Duration As Double, ByVal Light As Boolean)
    StartExposure Duration, Light
End Sub

Private Property Let ICamera_StartX(ByVal RHS As Long)
    StartX = RHS
End Property

Private Property Get ICamera_StartX() As Long
    ICamera_StartX = StartX
End Property

Private Property Let ICamera_StartY(ByVal RHS As Long)
    StartY = RHS
End Property

Private Property Get ICamera_StartY() As Long
    ICamera_StartY = StartY
End Property

Private Sub ICamera_StopExposure()
    StopExposure
End Sub
