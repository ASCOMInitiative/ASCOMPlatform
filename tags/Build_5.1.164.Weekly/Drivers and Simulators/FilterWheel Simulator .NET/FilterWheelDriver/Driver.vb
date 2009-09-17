'tabs=4
' --------------------------------------------------------------------------------
'
' ASCOM FilterWheel driver for FilterWheelSimulator
'
' Description:	A port of the VB6 ASCOM Filterwheel simulator to VB.Net.
'               Converted and built in Visual Studio 2008.
'               The port leaves some messy code - it could really do with
'               a ground up re-write!
'
' Implements:	ASCOM FilterWheel interface version: 5.1.0
' Author:		Mark Crossley <mark@markcrossley.co.uk>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' 06-Jun-2009	mpc	1.0.0	Initial edit, from FilterWheel template
' --------------------------------------------------------------------------------
'
' Your driver's ID is ASCOM.FilterWheelSim.FilterWheel
'
' The Guid attribute sets the CLSID for ASCOM.FilterWheelSim.FilterWheel
' The ClassInterface/None addribute prevents an empty interface called
' _FilterWheel from being created and used as the [default] interface
'
<Assembly: ServedClassName("Filter Wheel Simulator")>   '[TPL] Mark this assembly as something that LocalServer should be interested in.

<Guid("F9043C88-F6F2-101A-A3C9-08002B2F49FC")> _
<ClassInterface(ClassInterfaceType.None)> Public Class FilterWheel
    '	==========
    Inherits ReferenceCountedObjectBase
    Implements IFilterWheel ' Early-bind interface implemented by this driver
    Implements IDisposable  ' Clean-up code
    '	==========

    Private m_handBox As HandboxForm                     ' Hand box
    Private disposed As Boolean = False


    '
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()

        m_handBox = New HandboxForm

        ' Show the handbox now
        m_handBox.Show()
        m_handBox.Activate()
        ' And start the Timer
        m_handBox.Timer.Enabled = True

    End Sub

    ' Implement IDisposable.
    ' Do not make this method virtual.
    ' A derived class should not be able to override this method.
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        ' This object will be cleaned up by the Dispose method.
        ' Therefore, you should call GC.SupressFinalize to
        ' take this object off the finalization queue 
        ' and prevent finalization code for this object
        ' from executing a second time.
        GC.SuppressFinalize(Me)
    End Sub

    ' Dispose(bool disposing) executes in two distinct scenarios.
    ' If disposing equals true, the method has been called directly
    ' or indirectly by a user's code. Managed and unmanaged resources
    ' can be disposed.
    ' If disposing equals false, the method has been called by the 
    ' runtime from inside the finalizer and you should not reference 
    ' other objects. Only unmanaged resources can be disposed.
    Private Overloads Sub Dispose(ByVal disposing As Boolean)
        ' Check to see if Dispose has already been called.
        If Not Me.disposed Then
            ' If disposing equals true, dispose all managed 
            ' and unmanaged resources.
            If disposing Then
                ' Dispose managed resources.
                m_handBox.Close()
                m_handBox.Dispose()
                m_handBox = Nothing
            End If

            ' Call the appropriate methods to clean up 
            ' unmanaged resources here.
            ' If disposing is false, 
            ' only the following code is executed.
        End If
        disposed = True
    End Sub



    '
    ' PUBLIC COM INTERFACE IFilterWheel IMPLEMENTATION
    '

#Region "IFilterWheel Members"

    Public Property Connected() As Boolean Implements IFilterWheel.Connected
        Get

            Connected = m_handBox.Connected

        End Get
        Set(ByVal value As Boolean)

            m_handBox.Connected = value

        End Set
    End Property

    Public Property Position() As Short Implements IFilterWheel.Position
        Get

            check_connected()

            Position = m_handBox.Position

        End Get
        Set(ByVal value As Short)

            check_connected()

            m_handBox.Position = value

        End Set
    End Property

    Public ReadOnly Property FocusOffsets() As Integer() Implements IFilterWheel.FocusOffsets
        Get
            check_connected()

            FocusOffsets = m_handBox.FocusOffsets

        End Get
    End Property

    Public ReadOnly Property Names() As String() Implements IFilterWheel.Names
        Get
            check_connected()

            Names = m_handBox.FilterNames

        End Get
    End Property

    Public Sub SetupDialog() Implements IFilterWheel.SetupDialog

        m_handBox.DoSetup()

    End Sub
#End Region


    '---------------------------------------------------------------------
    '
    ' check_connected() - Raise an error if the focuser is not connected
    '
    '---------------------------------------------------------------------
    Private Sub check_connected()

        If Not m_handBox.Connected Then _
        Throw New DriverException(MSG_NOT_CONNECTED, SCODE_NOT_CONNECTED)

    End Sub

End Class
