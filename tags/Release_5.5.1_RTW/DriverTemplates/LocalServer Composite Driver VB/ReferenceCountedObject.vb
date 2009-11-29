
Imports System
Imports System.Runtime.InteropServices

Namespace TEMPLATENAMESPACE
	<ComVisible(False)> _
	Public Class ReferenceCountedObjectBase
		Public Sub New()
			' We increment the global count of objects.
			TEMPLATEDEVICENAME.CountObject()
		End Sub
		Protected Overrides Sub Finalize()
			Try

				' We decrement the global count of objects.
				TEMPLATEDEVICENAME.UncountObject()
				' We then immediately test to see if we the conditions
				' are right to attempt to terminate this server application.
				TEMPLATEDEVICENAME.ExitIf()
			Finally
				MyBase.Finalize()
			End Try
		End Sub
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Built and maintained by Todd Anglin and Telerik
'=======================================================
