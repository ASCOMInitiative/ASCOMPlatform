
'
' ================
' Shared Resources
' ================
'
' This class is a container for all shared resources that may be needed
' by the drivers served by the Local Server. 
'
' NOTES:
'
'	* ALL DECLARATIONS MUST BE STATIC HERE!! INSTANCES OF THIS CLASS MUST NEVER BE CREATED!
'
' Written by:	Bob Denny	29-May-2007
'
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports ASCOM.Utilities

Namespace ASCOM.LocalServerName
	Public Class SharedResources
		' Shared serial port
		Private Shared s_z As Integer

		Private Sub New()
		End Sub
		' Prevent creation of instances
		Shared Sub New()
			' Static initialization
            SharedSerial = New Serial()
			s_z = 0
		End Sub

		'
		' Public access to shared resources
		'

        ' Shared serial port
        Private Shared _serial = Nothing   ' Private backing store for the SharedSerial property.
        Public Shared Property SharedSerial() As Serial
            Get
                SharedSerial = _serial
            End Get
            Private Set(ByVal value As Serial)
                _serial = value
            End Set
        End Property
		Public Shared ReadOnly Property z() As Integer
			Get
				Return System.Math.Max(System.Threading.Interlocked.Increment(s_z),s_z - 1)
			End Get
		End Property
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Built and maintained by Todd Anglin and Telerik
'=======================================================
