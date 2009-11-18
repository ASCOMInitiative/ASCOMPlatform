'
' My Project/Root Namespace (ASCOM.LocalServerCOM) sets the first part
' of the ProgID, and the class name (VBSample) is the last part. Thus
' we are created as ASCOM.InprocServerCOM.VBSample.
'
' My Project/Assembly Information.../Guid sets the LIBID for our
' type library. In the same place, the Make assembly COM-Visible
' causes the type library to be created and registered along with
' the CoClass
'
' My Project/Assembly Information.../Description sets the title of
' the type library as shown in OLEView, etc.
'
' The Guid attribute sets the CLSID for ASCOM.LocalServerCOM.VBSample
' The ClassInterface/None addribute prevents an empty interface called
' _VBSample from being created and used as the [default] interface
'

<Guid("6F1BE1FE-895B-41e0-8072-299351F3ECF1")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class VBSample
	'==================================
	Inherits ReferenceCountedObjectBase
	Implements IAscomSample
	'==================================

	'
	' COM Error Constants
	'
	Private Const SCODE_X_UNSET As Integer = &H80040001
	Private Const MSG_X_UNSET As String = "The value of X has not been set"
	Private Const SCODE_Y_UNSET As Integer = &H80040002
	Private Const MSG_Y_UNSET As String = "The value of Y has not been set"

	'
	' Class private vars
	'
	Private m_X As Double = Double.NaN							' [sentinel]
	Private m_Y As Double = Double.NaN							' [sentinel]
	Private m_enumVal As SampleEnumType = SampleEnumType.sampleType1

	'
	' Class private functions
	'
	Private Sub checkX()
		If Double.IsNaN(m_X) Then Throw New COMException(MSG_X_UNSET, SCODE_X_UNSET)
	End Sub

	Private Sub checkY()
		If Double.IsNaN(m_Y) Then Throw New COMException(MSG_Y_UNSET, SCODE_Y_UNSET)
	End Sub

	Private Function calcDiag() As Double
		checkX()
		checkY()
		calcDiag = Math.Sqrt(m_X * m_X + m_Y * m_Y)
	End Function

	'
	' PUBLIC COM INTERFACE IAscomSample IMPLEMENTATION
	'
	Property X() As Double Implements IAscomSample.X
		Get
			checkX()
			X = m_X
		End Get
		Set(ByVal value As Double)
			m_X = value
		End Set
	End Property

	Property Y() As Double Implements IAscomSample.Y
		Get
			checkY()
			Y = m_Y
		End Get
		Set(ByVal value As Double)
			m_Y = value
		End Set
	End Property

	ReadOnly Property Diagonal() As Double Implements IAscomSample.Diagonal
		Get
			Diagonal = calcDiag()
		End Get
	End Property

	Function CalculateDiagonal(ByVal X As Double, ByVal Y As Double) As Double Implements IAscomSample.CalculateDiagonal
		m_X = X
		m_Y = Y
		CalculateDiagonal = calcDiag()
	End Function

	Property EnumTest() As SampleEnumType Implements IAscomSample.EnumTest
		Get
			EnumTest = m_enumVal
		End Get
		Set(ByVal value As SampleEnumType)
			m_enumVal = value
		End Set
	End Property
End Class



