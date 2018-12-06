﻿Imports ASCOM.DeviceInterface

'
' The Rate class implements IRate, and is used to hold values
' for AxisRates. You do not need to change this class.
'
' The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.Rate
' The ClassInterface/None addribute prevents an empty interface called
' _Rate from being created and used as the [default] interface
'
<Guid("AD6248B3-3F51-4FFF-B62B-E3E942DD817E")>
<ClassInterface(ClassInterfaceType.None)>
<ComVisible(True)>
Public Class Rate
    '		================
    Implements IRate
    '		================

    Private m_dMaximum As Double = 0
    Private m_dMinimum As Double = 0

    '
    ' Default constructor - Internal prevents public creation
    ' of instances. These are values for AxisRates.
    '
    Friend Sub New(ByVal Minimum As Double, ByVal Maximum As Double)
        m_dMaximum = Maximum
        m_dMinimum = Minimum
    End Sub

#Region "IRate Members"

    Public Sub Dispose() Implements IRate.Dispose
        ' TODO Add any required object cleanup here
    End Sub

    Public Property Maximum() As Double Implements IRate.Maximum
        Get
            Return m_dMaximum
        End Get
        Set(ByVal Value As Double)
            m_dMaximum = Value
        End Set
    End Property

    Public Property Minimum() As Double Implements IRate.Minimum
        Get
            Return m_dMinimum
        End Get
        Set(ByVal Value As Double)
            m_dMinimum = Value
        End Set
    End Property

#End Region


End Class

'
' AxisRates is a strongly-typed collection that must be enumerable by
' both COM and .NET. The IAxisRates and IEnumerable interfaces provide
' this polymorphism. 
'
' The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.AxisRates
' The ClassInterface/None addribute prevents an empty interface called
' _AxisRates from being created and used as the [default] interface
'
<Guid("99DB28A6-0132-43BF-91C0-D723124813C8")>
<ClassInterface(ClassInterfaceType.None)>
<ComVisible(True)>
Public Class AxisRates
    '		======================
    Implements IAxisRates
    Implements IEnumerable
    '		======================

    Private m_Axis As TelescopeAxes
    Private m_Rates(-1) As Rate         ' Empty array, but an array nonetheless

    '
    ' Constructor - Friend prevents public creation
    ' of instances. Returned by Telescope.AxisRates.
    '
    Friend Sub New(ByVal Axis As TelescopeAxes)
        m_Axis = Axis
        '
        ' This collection must hold zero or more Rate objects describing the 
        ' rates of motion ranges for the Telescope.MoveAxis() method
        ' that are supported by your driver. It is OK to leave this 
        ' array empty, indicating that MoveAxis() is not supported.
        '
        ' Note that we are constructing a rate array for the axis passed
        ' to the constructor. Thus we switch() below, and each case should 
        ' initialize the array for the rate for the selected axis.
        '
        Select Case Axis
            Case TelescopeAxes.axisPrimary
                ' TODO Initialize this array with any Primary axis rates that your driver may provide
                ' Example: ReDim m_Rates(2)
                '          m_Rates(0) = New Rate(10.0, 35.0)
                '          m_Rates(1) = New Rate(60.1, 120.0)
                Exit Sub
            Case TelescopeAxes.axisSecondary
                ' TODO Initialize this array with any Secondary axis rates that your driver may provide
                Exit Sub
            Case TelescopeAxes.axisTertiary
                ' TODO Initialize this array with any Tertiary axis rates that your driver may provide
                Exit Sub
        End Select
    End Sub

#Region "IAxisRates Members"

    Public ReadOnly Property Count() As Integer Implements IAxisRates.Count
        Get
            Return m_Rates.Length
        End Get
    End Property

    Public Sub Dispose() Implements IAxisRates.Dispose
        ' TODO Add any required object cleanup here
    End Sub

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator, IAxisRates.GetEnumerator
        Return m_Rates.GetEnumerator()
    End Function

    Default Public ReadOnly Property Item(ByVal Index As Integer) As IRate Implements IAxisRates.Item
        Get
            Return CType(m_Rates(Index - 1), IRate)    ' 1-based
        End Get
    End Property

#End Region

End Class

'
' TrackingRates is a strongly-typed collection that must be enumerable by
' both COM and .NET. The ITrackingRates and IEnumerable interfaces provide
' this polymorphism. 
'
' The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.TrackingRates
' The ClassInterface/None addribute prevents an empty interface called
' _TrackingRates from being created and used as the [default] interface
'
<Guid("49A4CA43-46B2-4D66-B9D3-FBE3ABE13DEB")>
<ClassInterface(ClassInterfaceType.None)>
<ComVisible(True)>
Public Class TrackingRates
    '		=========================
    Implements ITrackingRates
    Implements IEnumerable
    Implements IEnumerator
    '		=========================

    Private m_TrackingRates(-1) As DriveRates           ' Empty array, but an array nonetheless
    ' This should make the enumerator thread safe.
    Private ReadOnly pos As Threading.ThreadLocal(Of Integer) = New Threading.ThreadLocal(Of Integer)(
            Function()
                Return 0
            End Function)

    Private ReadOnly lockObj As Object = New Object

    '
    ' Default constructor - Friend prevents public creation
    ' of instances. Returned by Telescope.AxisRates.
    '
    Friend Sub New()
        '
        ' This array must hold ONE or more DriveRates values, indicating
        ' the tracking rates supported by your telescope. The one value
        ' (tracking rate) that MUST be supported is driveSidereal!
        '
        ' TODO Initialize this array with any additional tracking rates that your driver may provide
        ' Example: ReDim m_TrackingRates(1)
        '          m_TrackingRates(0) = DriveRates.driveSidereal
        '          m_TrackingRates(1) = DriveRates.driveLunar
        '
        ReDim m_TrackingRates(0)
        m_TrackingRates(0) = DriveRates.driveSidereal

    End Sub

#Region "ITrackingRates Members"

    Public ReadOnly Property Count() As Integer Implements ITrackingRates.Count
        Get
            Return m_TrackingRates.Length
        End Get
    End Property

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator, ITrackingRates.GetEnumerator
        pos.Value = -1
        Return DirectCast(Me, IEnumerator)
    End Function

    Public Sub Dispose() Implements ITrackingRates.Dispose
        pos.Dispose()
    End Sub

    Default Public ReadOnly Property Item(ByVal Index As Integer) As DriveRates Implements ITrackingRates.Item
        Get
            Return m_TrackingRates(Index - 1)  ' 1-based
        End Get
    End Property

#End Region

#Region "IEnumerator members"
    Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
        SyncLock lockObj
            If Not pos.IsValueCreated Then
                pos.Value = -1
            End If
            pos.Value += 1
            If pos.Value >= m_TrackingRates.Length Then
                Return False
            End If
            Return True
        End SyncLock
    End Function

    Public ReadOnly Property Current As Object Implements IEnumerator.Current
        Get
            SyncLock lockObj
                If Not pos.IsValueCreated Then
                    pos.Value = -1
                End If
                If pos.Value < 0 Or pos.Value >= m_TrackingRates.Length Then
                    Throw New InvalidOperationException()
                End If
                Return m_TrackingRates(pos.Value)
            End SyncLock
        End Get
    End Property

    Public Sub Reset() Implements IEnumerator.Reset
        pos.Value = -1
    End Sub
#End Region

End Class
