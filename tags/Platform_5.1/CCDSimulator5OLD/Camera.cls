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
Attribute VB_Description = "ASCOM Telescope Driver for Meade Instruments"
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
'---------------------------------------------------------------------

Option Explicit

'-----------------
Implements ICamera      ' Implementation at end, calls public members
'-----------------

Private Const ID As String = "CCDSimulator.Camera"
Private Const DESC As String = "ASCOM CCD camera simulator"

Private m_Util As DriverHelper.Util
Private m_Profile As DriverHelper.Profile

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

Private m_bConnected As Boolean
Private m_bCoolerOn As Boolean
Private m_dSetCCDTemperature As Double
Private m_dLastExposureDuration  As Double
Private m_bTDI As Boolean

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

End Sub

Private Sub Class_Terminate()
    
   On Error Resume Next
   Set m_Util = Nothing
    
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

   If newVal > m_iMaxBinX Then
      Err.Raise SCODE_BINNING_FACTOR_TOO_LARGE, ERR_SOURCE, MSG_BINNING_FACTOR_TOO_LARGE
   End If

   m_iBinX = newVal
   m_iCameraXSizeBinned = m_iCameraXSize / m_iBinX

End Property

Public Property Get BinY() As Integer

   CheckConnected

   BinY = m_iBinY
    
End Property

Public Property Let BinY(ByVal newVal As Integer)

   CheckConnected

   If newVal > m_iMaxBinX Then
      Err.Raise SCODE_BINNING_FACTOR_TOO_LARGE, ERR_SOURCE, MSG_BINNING_FACTOR_TOO_LARGE
   End If

   m_iBinY = newVal
   m_iCameraYSizeBinned = m_iCameraYSize / m_iBinY

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

   CanAbortExposure = False

End Property

Public Property Get CanAsymmetricBin() As Boolean

   CanAsymmetricBin = True

End Property

Public Property Get CanGetCoolerPower() As Boolean

   CanGetCoolerPower = False
    
End Property

Public Property Get CanPulseGuide() As Boolean

   CanPulseGuide = False

End Property


Public Property Get CanSetCCDTemperature() As Boolean

   CanSetCCDTemperature = True

End Property

Public Property Get CanStopExposure() As Boolean

   CanStopExposure = False

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

    Description = "ASCOM CCD camera simulator"
    
End Property

' Returns the gain of the camera in photoelectrons per A/D unit. (Some cameras have
' multiple gain modes; these should be selected via the SetupDialog and thus are
' static during a session.)

Public Property Get ElectronsPerADU() As Double

    ' todo
    ElectronsPerADU = 0

End Property

' Reports the full well capacity of the camera in electrons, at the current camera
' settings (binning, SetupDialog settings, etc.)

Public Property Get FullWellCapacity() As Double

    FullWellCapacity = 300000  ' electrons

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
   ReDim Preserve aImage(0 To m_iNumY - 1, 0 To m_iNumX - 1)

   Randomize

   ' dummy background noise
   
   Const iBackGround As Integer = 4000
   Const iNoiseRange As Integer = 400
   
   Dim X, y As Long
   For X = 0 To m_iNumX - 1
      For y = 0 To m_iNumY - 1
         aImage(y, X) = Int(iNoiseRange * Rnd + iBackGround)
      Next
   Next
   
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
   ReDim Preserve aImage(0 To m_iNumY - 1, 0 To m_iNumX - 1)

   Randomize

   ' dummy background noise
   
   Const iBackGround As Integer = 2800
   Const iNoiseRange As Integer = 300
   Dim pixel As Long

   Dim X, y As Long
   For X = 0 To m_iNumX - 1
      For y = 0 To m_iNumY - 1
         pixel = CLng(iNoiseRange * Rnd + iBackGround)
         aImage(y, X) = pixel
      Next
   Next
   
   ImageArrayVariant = aImage

End Property

' If True, there is an image from the camera available. If False, no image
' is available and attempts to use the ImageArray method will produce an
' exception.

Public Property Get ImageReady() As Boolean

   CheckConnected

   ImageReady = Not m_frmTimer.m_bExposing
    
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

   ' todo
   LastError = ""

End Property

' Reports the actual exposure duration in seconds (i.e. shutter open time).  This
' may differ from the exposure time requested due to shutter latency, camera timing
' precision, etc.

Public Property Get LastExposureDuration() As Double

    LastExposureDuration = m_dLastExposureDuration

End Property

' Reports the actual exposure start in the FITS-standard
' CCYY-MM-DDThh:mm:ss[.sss...] format.

Public Property Get LastExposureStartTime() As String

    ' todo
    ' Reports the actual exposure start in the FITS-standard
    ' CCYY-MM-DDThh:mm:ss[.sss...] format.
   
    LastExposureStartTime = ""

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

   If newVal > m_iCameraXSizeBinned - m_iStartX Then
      Err.Raise SCODE_SUBFRAME_TOO_LARGE, ERR_SOURCE, MSG_SUBFRAME_TOO_LARGE
   End If
     
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

   If newVal > m_iCameraYSizeBinned - m_iStartY Then
      Err.Raise SCODE_SUBFRAME_TOO_LARGE, ERR_SOURCE, MSG_SUBFRAME_TOO_LARGE
   End If
     
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

   m_dSetCCDTemperature = newVal

End Property

' Sets the subframe start position for the X axis (0 based). Also returns the
' current value.  If binning is active, value is in binned pixels.

Public Property Let StartX(ByVal newVal As Long)

   CheckConnected

   If newVal >= m_iCameraXSizeBinned Then
      Err.Raise SCODE_SUBFRAME_TOO_LARGE, ERR_SOURCE, MSG_SUBFRAME_TOO_LARGE
   End If

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

   If newVal >= m_iCameraYSizeBinned Then
      Err.Raise SCODE_SUBFRAME_TOO_LARGE, ERR_SOURCE, MSG_SUBFRAME_TOO_LARGE
   End If

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

End Sub

' Launches a configuration dialog box for the driver.  The call will not return
' until the user clicks OK or cancel manually.

Public Sub SetupDialog()

    '
    ' Must set member vars first, as the lblDriverInfo change
    ' loads for the, executing the OnLoad event, which depends
    ' on the member vars.
    '
    Set frmSetup.m_Profile = m_Profile
    frmSetup.m_DriverID = ID                    ' Form uses Helper.Profile methods
    frmSetup.Show 1
    If Not frmSetup.m_Cancel Then ReadConfig
    Unload frmSetup

End Sub

' Starts an exposure. Use ImageReady to check when the exposure is complete.

Public Sub StartExposure(ByVal Duration As Double, ByVal Light As Boolean)

   CheckConnected
   
   m_frmTimer.m_bExposing = True
   
   m_frmTimer.Timer1.Interval = Duration * 1000
   m_frmTimer.Timer1.Enabled = True
   
   m_dLastExposureDuration = Duration

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
    ICamera_NumX = ICamera_NumX
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
