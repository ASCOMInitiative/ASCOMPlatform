Imports System.Runtime.InteropServices
'Imports ASCOM.Astrometry.Interfaces
Imports ASCOM.Astrometry.NOVASCOM
Namespace NOVAS

    <Guid("74F604BD-6106-40ac-A821-B32F80BF3FED"), _
    ClassInterface(ClassInterfaceType.None), _
    ComVisible(True)> _
    Public Class NOVAS3
        Implements INOVAS3, IDisposable


        Private Const NOVAS32Dll As String = "NOVAS3.dll"
        Private Const NOVAS64Dll As String = "NOVAS3-64.dll"
        Private Const JPL_EPHEMERIDES As String = "JPLEPH"
        Private Const NOVAS_DLL_LOCATION As String = "\ASCOM\.net" 'This is appended to the Common Files path

#Region "New and IDisposable"
        Sub New()
            Dim rc As Boolean, rc1 As Short

            'Add the ASCOM\.net directory to the DLL search path so that the NOVAS C 32 and 64bit DLLs can be found
            rc = SetDllDirectory(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) & NOVAS_DLL_LOCATION)

            rc1 = Ephem_Open(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) & NOVAS_DLL_LOCATION & "\" & JPL_EPHEMERIDES, 2305424.5, 2525008.5)
            If rc1 > 0 Then MsgBox("Ephem open RC: " & rc1)
        End Sub

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' Free other state (managed objects).
                End If
                ' Free your own state (unmanaged objects) and set large fields to null.
                Try : Ephem_Close() : Catch : End Try ' Close the ephemeris file if its open
            End If
            Me.disposedValue = True
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region


#Region "Public Interface"




        Public Function place(ByVal jd_tt As Double, _
                              ByRef cel_object As Object3, _
                              ByRef location As Observer, _
                              ByVal delta_t As Double, _
                              ByVal coord_sys As CoordSys, _
                              ByVal accuracy As AccuracyValue, _
                              ByRef output As sky_pos) As Short Implements INOVAS3.place
            If Is64Bit() Then
                Return place64(jd_tt, cel_object, location, delta_t, coord_sys, accuracy, output)
            Else
                Return place32(jd_tt, cel_object, location, delta_t, coord_sys, accuracy, output)
            End If

        End Function

        Public Function AstroStar(ByVal JDTT As Double, _
                                  ByRef Star As CatEntry3, _
                                  ByVal Accuracy As AccuracyValue, _
                                  ByRef RA As Double, _
                                  ByRef Dec As Double) As Short Implements INOVAS3.AstroStar
            If Is64Bit() Then
                Return astro_star64(JDTT, Star, Accuracy, RA, Dec)
            Else
                Return astro_star32(JDTT, Star, Accuracy, RA, Dec)
            End If
        End Function

        Public Function TopoStar(ByVal JDTT As Double, _
                                 ByVal DeltaT As Double, _
                                 ByRef Star As CatEntry3, _
                                 ByRef position As OnSurface, _
                                 ByVal Accuracy As AccuracyValue, _
                                 ByRef RA As Double, _
                                 ByRef Dec As Double) As Short Implements INOVAS3.TopoStar
            If Is64Bit() Then
                Return topo_star64(JDTT, DeltaT, Star, position, Accuracy, RA, Dec)
            Else
                Return topo_star32(JDTT, DeltaT, Star, position, Accuracy, RA, Dec)
            End If

        End Function
        Public Function JulianDate(ByVal year As Short, _
                                           ByVal month As Short, _
                                           ByVal day As Short, _
                                           ByVal hour As Double) As Double Implements INOVAS3.JulianDate
            If Is64Bit() Then
                Return julian_date64(year, month, day, hour)
            Else
                Return julian_date32(year, month, day, hour)
            End If
        End Function

        Public Function sidereal_time(ByVal jd_high As Double, _
                                      ByVal jd_low As Double, _
                                      ByVal delta_t As Double, _
                                      ByVal gst_type As GstType, _
                                      ByVal method As Method, _
                                      ByVal accuracy As AccuracyValue, _
                                      ByRef gst As Double) As Short Implements INOVAS3.sidereal_time

            If Is64Bit() Then
                sidereal_time64(jd_high, jd_low, delta_t, gst_type, method, accuracy, gst)
            Else
                sidereal_time32(jd_high, jd_low, delta_t, gst_type, method, accuracy, gst)
            End If

        End Function
#End Region
#Region "DLL Entry Points (32bit)"
        <DllImportAttribute(NOVAS32Dll, EntryPoint:="Ephem_Open")> _
                Private Shared Function Ephem_Open32(<MarshalAs(UnmanagedType.LPStr)> ByVal Ephem_Name As String, _
                                                  ByRef JD_Begin As Double, _
                                                  ByRef JD_End As Double) As Short
        End Function

        <DllImportAttribute(NOVAS32Dll, EntryPoint:="Ephem_Close")> Private Shared Function Ephem_Close32() As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="place")> Private Shared Function place32(ByVal jd_tt As Double, _
                                                                                     ByRef cel_object As Object3, _
                                                                                     ByRef location As Observer, _
                                                                                     ByVal delta_t As Double, _
                                                                                     ByVal coord_sys As Short, _
                                                                                     ByVal accuracy As Short, _
                                                                                     ByRef output As sky_pos) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="astro_star")> Private Shared Function astro_star32(ByVal JDTT As Double, _
                                                                                       ByRef star As CatEntry3, _
                                                                                       ByVal accuracy As Short, _
                                                                                       ByRef ra As Double, _
                                                                                       ByRef dec As Double) As Short

        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="topo_star")> Private Shared Function topo_star32(ByVal JDTT As Double, _
                                                                                     ByVal deltat As Double, _
                                                                                     ByRef star As CatEntry3, _
                                                                                     ByRef position As OnSurface, _
                                                                                     ByVal accuracy As Short, _
                                                                                     ByRef ra As Double, _
                                                                                     ByRef dec As Double) As Short
        End Function
        <DllImport(NOVAS32Dll, EntryPoint:="julian_date")> Private Shared Function julian_date32(ByVal year As Short, _
                                                                                                 ByVal month As Short, _
                                                                                                 ByVal day As Short, _
                                                                                                 ByVal hour As Double) As Double
        End Function
        <DllImport(NOVAS32Dll, EntryPoint:="sidereal_time")> Private Shared Function sidereal_time32(ByVal jd_high As Double, _
                                                                                                     ByVal jd_low As Double, _
                                                                                                     ByVal delta_t As Double, _
                                                                                                     ByVal gst_type As Short, _
                                                                                                     ByVal method As Short, _
                                                                                                     ByVal accuracy As Short, _
                                                                                                     ByRef gst As Double) As Short
        End Function

#End Region

#Region "DLL Entry Points (64bit)"
        <DllImportAttribute(NOVAS64Dll, EntryPoint:="Ephem_Open")> _
                Private Shared Function Ephem_Open64(<MarshalAs(UnmanagedType.LPStr)> ByVal Ephem_Name As String, _
                                                  ByRef JD_Begin As Double, _
                                                  ByRef JD_End As Double) As Short
        End Function

        <DllImportAttribute(NOVAS64Dll, EntryPoint:="Ephem_Close")> Private Shared Function Ephem_Close64() As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="place")> Private Shared Function place64(ByVal jd_tt As Double, _
                                                                                     ByRef cel_object As Object3, _
                                                                                     ByRef location As Observer, _
                                                                                     ByVal delta_t As Double, _
                                                                                     ByVal coord_sys As Short, _
                                                                                     ByVal accuracy As Short, _
                                                                                     ByRef output As sky_pos) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="astro_star")> Private Shared Function astro_star64(ByVal JDTT As Double, _
                                                                                       ByRef star As CatEntry3, _
                                                                                       ByVal accuracy As Short, _
                                                                                       ByRef ra As Double, _
                                                                                       ByRef dec As Double) As Short
        End Function
        <DllImport(NOVAS64Dll, EntryPoint:="topo_star")> Private Shared Function topo_star64(ByVal JDTT As Double, _
                                                                                     ByVal deltat As Double, _
                                                                                     ByRef star As CatEntry3, _
                                                                                     ByRef position As OnSurface, _
                                                                                     ByVal accuracy As Short, _
                                                                                     ByRef ra As Double, _
                                                                                     ByRef dec As Double) As Short
        End Function
        <DllImport(NOVAS64Dll, EntryPoint:="julian_date")> _
        Private Shared Function julian_date64(ByVal year As Short, _
                                              ByVal month As Short, _
                                              ByVal day As Short, _
                                              ByVal hour As Double) As Double
        End Function
        <DllImport(NOVAS64Dll, EntryPoint:="sidereal_time")> _
        Private Shared Function sidereal_time64(ByVal jd_high As Double, _
                                                ByVal jd_low As Double, _
                                                ByVal delta_t As Double, _
                                                ByVal gst_type As Short, _
                                                ByVal method As Short, _
                                                ByVal accuracy As Short, _
                                                ByRef gst As Double) As Short
        End Function
#End Region
#Region "Support Code"
        'Declare the api call that sets the additional DLL search directory
        <DllImport("kernel32.dll", SetLastError:=False)> _
        Private Shared Function SetDllDirectory(ByVal lpPathName As String) As Boolean
        End Function

        Private Shared Function Is64Bit() As Boolean

            If IntPtr.Size = 8 Then 'Check whether we are running on a 32 or 64bit system.
                Return True
            Else
                Return False
            End If
        End Function

        Private Shared Function Ephem_Open(ByVal Ephem_Name As String, _
                                  ByRef JD_Begin As Double, _
                                  ByRef JD_End As Double) As Short
            If Is64Bit() Then
                Return Ephem_Open64(Ephem_Name, JD_Begin, JD_End)
            Else
                Return Ephem_Open32(Ephem_Name, JD_Begin, JD_End)
            End If

        End Function

        Private Shared Function Ephem_Close() As Short
            If Is64Bit() Then
                Return Ephem_Close64()
            Else
                Return Ephem_Close32()
            End If
        End Function





#End Region


    End Class

End Namespace
