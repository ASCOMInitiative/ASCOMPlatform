Option Strict On
Option Explicit On
Imports ASCOM.Utilities.Interfaces
Imports System.Runtime.InteropServices

''' <summary>
''' The Chooser object provides a way for your application to let your user select the telescope to use.
''' </summary>
''' <remarks>
''' <para>Calling Chooser.Choose() causes a chooser window to appear, with a drop down selector list containing all of the registered Telescope 
''' drivers, listed by the driver's friendly/display name. The user sees a list of telescope types and selects one. 
''' Before the OK button will light up, however, the user must click the Setup button, causing the selected driver's setup dialog to appear 
''' (it calls the driver's Telescope.SetupDialog() method). When the setup dialog is closed, the OK button will light and allow the user 
''' to close the Chooser itself.</para>
''' 
''' <para>The Choose() method returns the DriverID of the selected driver. Choose() allows you to optionally pass the DriverID of a "current" 
''' driver (you probably save this in the registry), and the corresponding telescope type is pre-selected in the Chooser's list. In this case, 
''' the OK button starts out enabled (lit-up); the assumption is that the pre-selected driver has already been configured. </para>
'''</remarks>
<Guid("B7A1F5A0-71B4-44f9-91E9-468697957D6B"), _
ComVisible(True), _
ClassInterface(ClassInterfaceType.None)> _
Public Class Chooser
    Implements IChooser, IChooserExtra, IDisposable
    '   ===========
    '   CHOOSER.CLS
    '   ===========
    '
    ' Implementation of the ASCOM telescope driver Chooser class
    '
    ' Written:  24-Aug-00   Robert B. Denny <rdenny@dc3.com>
    '
    ' Edits:
    '
    ' When      Who     What
    ' --------- ---     --------------------------------------------------
    ' 25-Feb-09 pwgs     5.1.0 - Refactored for Utilities
    '---------------------------------------------------------------------

    Private m_frmChooser As ChooserForm
    Private m_sDeviceType As String = ""

#Region " New and IDisposable Support "

    Private disposedValue As Boolean = False        ' To detect redundant calls

    ''' <summary>
    ''' Creates a new Chooser object
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()
        'MsgBox("CHOOSER.NEW Before New Form")
        Try
            m_frmChooser = New ChooserForm ' Initially hidden
        Catch ex As Exception
            MsgBox("Chooser.New " & ex.ToString)
        End Try
        'MsgBox("CHOOSER.NEW AFter New Form")
        m_sDeviceType = "Telescope" ' Default to Telescope chooser
    End Sub

    ' IDisposable
    ''' <summary>
    ''' Does the work of cleaning up objects used by Chooser
    ''' </summary>
    ''' <param name="disposing">True if called by the user, false if called by the system</param>
    ''' <remarks>You can't call this directly, use Dispose with no arguments instead.</remarks>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
            End If
            If Not (m_frmChooser Is Nothing) Then
                m_frmChooser.Dispose()
                m_frmChooser = Nothing
            End If

        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    ''' <summary>
    ''' Cleans up and disposes objects used by Chooser
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

#End Region

#Region "IChooser Implementation"
    ''' <summary>
    ''' The type of device for which the Chooser will select a driver. (String, default = "Telescope")
    ''' </summary>
    ''' <value>The type of device for which the Chooser will select a driver. (String, default = "Telescope") 
    '''</value>
    ''' <returns>The device type that has been set</returns>
    ''' <exception cref="Exceptions.InvalidValueException">Thrown on setting the device type to empty string</exception>
    ''' <remarks>This property changes the "personality" of the Chooser, allowing it to be used to select a driver for any arbitrary 
    ''' ASCOM device type. The default value for this is "Telescope", but it could be "Focuser", "Camera", etc. 
    ''' <para>This property is independent of the Profile object's DeviceType property. Setting Chooser's DeviceType 
    ''' property doesn't set the DeviceType property in Profile, you must set that also when needed.</para>
    '''</remarks>
    Public Property DeviceType() As String Implements IChooser.DeviceType
        Get
            Return m_sDeviceType
        End Get
        Set(ByVal Value As String)
            If Value = "" Then Throw New Exceptions.InvalidValueException("Chooser:DeviceType - " & MSG_ILLEGAL_DEVTYPE) 'Err.Raise(SCODE_ILLEGAL_DEVTYPE, ERR_SOURCE_PROFILE, MSG_ILLEGAL_DEVTYPE)

            m_sDeviceType = Value
        End Set
    End Property

    ''' <summary>
    ''' Select ASCOM driver to use including pre-selecting one in the dropdown list
    ''' </summary>
    ''' <param name="DriverProgID">Driver to preselect in the chooser dialogue</param>
    ''' <returns>Driver ID of chosen driver</returns>
    ''' <exception cref="Exceptions.InvalidValueException">Thrown if the Chooser.DeviceType property has not been set before Choose is called. 
    ''' It must be set in order for Chooser to know which list of devices to display.</exception>
    ''' <remarks>The supplied driver will be pre-selected in the Chooser's list when the chooser window is first opened.
    ''' </remarks>
    Public Overloads Function Choose(ByVal DriverProgID As String) As String Implements IChooser.Choose
        Dim RetVal As String = ""
        Try

            If String.IsNullOrEmpty(m_sDeviceType) Then Throw New Exceptions.InvalidValueException("Chooser:Choose - Unknown device type, DeviceType property has not been set")

            m_frmChooser.DeviceType = m_sDeviceType
            m_frmChooser.StartSel = DriverProgID
            '   -------------------
            m_frmChooser.ShowDialog() ' -- MODAL --
            '   -------------------
            Try
                RetVal = m_frmChooser.Result
            Catch ex As Exception
                MsgBox("CHOOSER.VB Exception 0" & ex.ToString)
                RetVal = "CHOOSER.VB Exception 0"
            End Try

            Try
                m_frmChooser.Dispose()
            Catch ex As Exception
                MsgBox("CHOOSER.VB Exception 1" & ex.ToString)
                RetVal = "CHOOSER.VB Exception 1"
            End Try
            Try
                m_frmChooser = Nothing
            Catch ex As Exception
                MsgBox("CHOOSER.VB Exception 2" & ex.ToString)
                RetVal = "CHOOSER.VB Exception 2"
            End Try
            Try
                'Form.ActiveForm.BringToFront()
            Catch ex As Exception
                MsgBox("CHOOSER.VB Exception 3" & ex.ToString)
                RetVal = "CHOOSER.VB Exception 3"
            End Try
        Catch ex As Exception
            MsgBox("CHOOSER.VB Exception 4" & ex.ToString)
            RetVal = "CHOOSER.VB Exception 4"
        End Try
        Return RetVal
    End Function
#End Region

#Region "IChooserExtra Implementation"
    ''' <summary>
    ''' Select ASCOM driver to use without pre-selecting in the dropdown list
    ''' </summary>
    ''' <returns>Driver ID of chosen driver</returns>
    ''' <remarks>No driver will be pre-selected in the Chooser's list when the chooser window is first opened. 
    ''' <exception cref="Exceptions.InvalidValueException">Thrown if the Chooser.DeviceType property has not been set before Choose is called. 
    ''' It must be set in order for Chooser to know which list of devices to display.</exception>
    ''' <para>This overload is not available through COM, please use "Choose(ByVal DriverProgID As String)"
    ''' with an empty string parameter to achieve this effect.</para>
    ''' </remarks>
    <ComVisible(False)> _
    Public Overloads Function Choose() As String Implements IChooserExtra.Choose
        Return Me.Choose("")
    End Function
#End Region

End Class