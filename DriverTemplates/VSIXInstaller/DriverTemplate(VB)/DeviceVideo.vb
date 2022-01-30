' All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
' Required code must lie within the device implementation region
' The //ENDOFINSERTEDFILE tag must be the last but one line in this file

Imports ASCOM.DeviceInterface
Imports System.Collections
Imports ASCOM
Imports ASCOM.Utilities

Class DeviceVideo
    'Implements IVideo
    Private m_util As New Util()
    Private TL As New TraceLogger()

#Region "IVideo Implementation"
    ' IVideo implementation
#End Region

    '//ENDOFINSERTEDFILE
End Class