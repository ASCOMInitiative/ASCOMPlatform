Imports System.Runtime.InteropServices

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

        Public Function Place(ByVal JdTt As Double, _
                            ByVal CelObject As Object3, _
                            ByVal Location As Observer, _
                            ByVal DeltaT As Double, _
                            ByVal CoordSys As CoordSys, _
                            ByVal Accuracy As Accuracy, _
                            ByRef Output As SkyPos) As Short Implements INOVAS3.Place
            If Is64Bit() Then
                Return place64(JdTt, CelObject, Location, DeltaT, CoordSys, Accuracy, Output)
            Else
                Return place32(JdTt, CelObject, Location, DeltaT, CoordSys, Accuracy, Output)
            End If

        End Function

        Function AstroStar(ByVal JdTt As Double, _
                               ByVal Star As CatEntry3, _
                               ByVal Accuracy As Accuracy, _
                               ByRef Ra As Double, _
                               ByRef Dec As Double) As Short Implements INOVAS3.AstroStar
            If Is64Bit() Then
                Return astro_star64(JdTt, Star, Accuracy, Ra, Dec)
            Else
                Return astro_star32(JdTt, Star, Accuracy, Ra, Dec)
            End If
        End Function

        Function TopoStar(ByVal JdTt As Double, _
                              ByVal DeltaT As Double, _
                              ByVal Star As CatEntry3, _
                              ByVal Position As OnSurface, _
                              ByVal Accuracy As Accuracy, _
                              ByRef Ra As Double, _
                              ByRef Dec As Double) As Short Implements INOVAS3.TopoStar
            If Is64Bit() Then
                Return topo_star64(JdTt, DeltaT, Star, Position, Accuracy, Ra, Dec)
            Else
                Return topo_star32(JdTt, DeltaT, Star, Position, Accuracy, Ra, Dec)
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
                                      ByVal accuracy As Accuracy, _
                                      ByRef gst As Double) As Short Implements INOVAS3.SiderealTime

            If Is64Bit() Then
                sidereal_time64(jd_high, jd_low, delta_t, gst_type, method, accuracy, gst)
            Else
                sidereal_time32(jd_high, jd_low, delta_t, gst_type, method, accuracy, gst)
            End If

        End Function

        Public Sub Aberration(ByVal Pos() As Double, ByVal Vel() As Double, ByVal LightTime As Double, ByRef Pos2() As Double) Implements INOVAS3.Aberration

        End Sub

        Public Function AppPlanet(ByVal JdTt As Double, ByVal SsBody As Object3, ByVal Accuracy As Accuracy, ByRef Ra As Double, ByRef Dec As Double, ByRef Dis As Double) As Short Implements INOVAS3.AppPlanet

        End Function

        Public Function AppStar(ByVal JdTt As Double, ByVal Star As CatEntry3, ByVal Accuracy As Accuracy, ByRef Ra As Double, ByRef Dec As Double) As Short Implements INOVAS3.AppStar

        End Function

        Public Function AstroPlanet(ByVal JdTt As Double, ByVal SsBody As Object3, ByVal Accuracy As Accuracy, ByRef Ra As Double, ByRef Dec As Double, ByRef Dis As Double) As Short Implements INOVAS3.AstroPlanet

        End Function


        Public Sub Bary2Obs(ByVal Pos() As Double, ByVal PosObs() As Double, ByRef Pos2() As Double, ByRef Lighttime As Double) Implements INOVAS3.Bary2Obs

        End Sub

        Public Sub CalDate(ByVal Tjd As Double, ByRef Year As Short, ByRef Month As Short, ByRef Day As Short, ByRef Hour As Double) Implements INOVAS3.CalDate

        End Sub

        Public Function CelPole(ByVal Tjd As Double, ByVal Type As PoleOffsetCorrectionType, ByVal Dpole1 As Double, ByVal Dpole2 As Double) As Short Implements INOVAS3.CelPole

        End Function

        Public Function Cio_Ra(ByVal JdTt As Double, ByVal Accuracy As Accuracy, ByRef RaCio As Double) As Short Implements INOVAS3.Cio_Ra

        End Function

        Public Function CioArray(ByVal JdTdb As Double, ByVal NPts As Integer, ByRef Cio() As RAOfCio) As Short Implements INOVAS3.CioArray

        End Function

        Public Function CioBasis(ByVal JdTdbEquionx As Double, ByVal RaCioEquionx As Double, ByVal RefSys As ReferenceSystem, ByVal Accuracy As Accuracy, ByRef x As Double, ByRef y As Double, ByRef z As Double) As Short Implements INOVAS3.CioBasis

        End Function

        Public Function CioLocation(ByVal JdTdb As Double, ByVal Accuracy As Accuracy, ByRef RaCio As Double, ByRef RefSys As ReferenceSystem) As Short Implements INOVAS3.CioLocation

        End Function

        Public Function DLight(ByVal Pos1() As Double, ByVal PosObs() As Double) As Double Implements INOVAS3.DLight

        End Function

        Public Function Ecl2EquVec(ByVal JdTt As Double, ByVal CoordSys As CoordSys, ByVal Accuracy As Accuracy, ByVal Pos1() As Double, ByRef Pos2() As Double) As Short Implements INOVAS3.Ecl2EquVec

        End Function

        Public Function EeCt(ByVal JdHigh As Double, ByVal JdLow As Double, ByVal Accuracy As Accuracy) As Double Implements INOVAS3.EeCt

        End Function

        Public Function Ephemeris(ByVal Jd() As Double, ByVal CelObj As Object3, ByVal Origin As Origin, ByVal Accuracy As Accuracy, ByRef Pos() As Double, ByVal Vel() As Double) As Short Implements INOVAS3.Ephemeris

        End Function

        Public Function Equ2Ecl(ByVal JdTt As Double, ByVal CoordSys As CoordSys, ByVal Accuracy As Accuracy, ByVal Ra As Double, ByVal Dec As Double, ByRef ELon As Double, ByRef ELat As Double) As Short Implements INOVAS3.Equ2Ecl

        End Function

        Public Function Equ2EclVec(ByVal JdTt As Double, ByVal CoordSys As CoordSys, ByVal Accuracy As Accuracy, ByVal Pos1() As Double, ByRef Pos2() As Double) As Short Implements INOVAS3.Equ2EclVec

        End Function

        Public Sub Equ2Gal(ByVal RaI As Double, ByVal DecI As Double, ByRef GLon As Double, ByRef GLat As Double) Implements INOVAS3.Equ2Gal

        End Sub

        Public Sub Equ2Hor(ByVal Jd_Ut1 As Double, ByVal DeltT As Double, ByVal Accuracy As Accuracy, ByVal x As Double, ByVal y As Double, ByVal Location As OnSurface, ByVal Ra As Double, ByVal Dec As Double, ByVal RefOption As RefractionOption, ByRef Zd As Double, ByRef Az As Double, ByRef RaR As Double, ByRef DecR As Double) Implements INOVAS3.Equ2Hor

        End Sub

        Public Function Era(ByVal JdHigh As Double, ByVal JdLow As Double) As Double Implements INOVAS3.Era

        End Function

        Public Sub ETilt(ByVal JdTdb As Double, ByVal Accuracy As Accuracy, ByRef Mobl As Double, ByRef Tobl As Double, ByRef Ee As Double, ByRef Dpsi As Double, ByRef Deps As Double) Implements INOVAS3.ETilt

        End Sub

        Public Sub FrameTie(ByVal Pos1() As Double, ByVal Direction As FrameConversionDirection, ByRef Pos2() As Double) Implements INOVAS3.FrameTie

        End Sub

        Public Sub FundArgs(ByVal t As Double, ByRef a() As Double) Implements INOVAS3.FundArgs

        End Sub

        Public Function Gcrs2Equ(ByVal JdTt As Double, ByVal CoordSys As CoordSys, ByVal Accuracy As Accuracy, ByVal RaG As Double, ByVal DecG As Double, ByRef Ra As Double, ByRef Dec As Double) As Short Implements INOVAS3.Gcrs2Equ

        End Function

        Public Function GeoPosVel(ByVal JdTt As Double, ByVal DeltaT As Double, ByVal Accuracy As Accuracy, ByVal Obs As ObserverLocation, ByRef Pos() As Double, ByRef Vel() As Double) As Short Implements INOVAS3.GeoPosVel

        End Function

        Public Function GravDef(ByVal JdTdb As Double, ByVal LocCode As EarthDeflection, ByVal Accuracy As Accuracy, ByVal Pos1() As Double, ByVal PosObs() As Double, ByRef Pos2() As Double) As Short Implements INOVAS3.GravDef

        End Function

        Public Sub GravVec(ByVal Pos1() As Double, ByVal PosObs() As Double, ByVal PosBody() As Double, ByVal RMass As Double, ByRef Pos2() As Double) Implements INOVAS3.GravVec

        End Sub

        Public Function IraEquinox(ByVal JdTdb As Double, ByVal Equinox As EquinoxType, ByVal Accuracy As Accuracy) As Double Implements INOVAS3.IraEquinox

        End Function

        Public Function LightTime(ByVal JdTdb As Double, ByVal SsObject As Object3, ByVal PosObs() As Double, ByVal TLight0 As Double, ByVal Accuracy As Accuracy, ByRef Pos() As Double, ByRef TLight As Double) As Short Implements INOVAS3.LightTime

        End Function

        Public Sub LimbAngle(ByVal PosObj() As Double, ByVal PosObs() As Double, ByRef LimbAng As Double, ByRef NadirAng As Double) Implements INOVAS3.LimbAngle

        End Sub

        Public Function LocalPlanet(ByVal JdTt As Double, ByVal SsBody As Object3, ByVal DeltaT As Double, ByVal Position As OnSurface, ByVal Accuracy As Accuracy, ByRef Ra As Double, ByRef Dec As Double, ByRef Dis As Double) As Short Implements INOVAS3.LocalPlanet

        End Function

        Public Function LocalStar(ByVal JdTt As Double, ByVal DeltaT As Double, ByVal Star As CatEntry3, ByVal Position As OnSurface, ByVal Accuracy As Accuracy, ByRef Ra As Double, ByRef Dec As Double) As Short Implements INOVAS3.LocalStar

        End Function

        Public Sub MakeCatEntry(ByVal StarName As String, ByVal Catalog As String, ByVal StarNum As Integer, ByVal Ra As Double, ByVal Dec As Double, ByVal PmRa As Double, ByVal PmDec As Double, ByVal Parallax As Double, ByVal RadVel As Double, ByRef Star As CatEntry3) Implements INOVAS3.MakeCatEntry

        End Sub

        Public Sub MakeInSpace(ByVal ScPos() As Double, ByVal ScVel() As Double, ByRef ObsSpace As InSpace) Implements INOVAS3.MakeInSpace

        End Sub

        Public Function MakeObject(ByVal Type As ObjectType, ByVal Number As Short, ByVal Name As String, ByVal StarData As CatEntry3, ByRef CelObj As Object3) As Short Implements INOVAS3.MakeObject

        End Function

        Public Function MakeObserver(ByVal Where As ObserverLocation, ByVal ObsSurface As OnSurface, ByVal ObsSpace As InSpace, ByRef Obs As Observer) As Short Implements INOVAS3.MakeObserver

        End Function

        Public Sub MakeObserverAtGeocenter(ByRef ObsAtGeocenter As Observer) Implements INOVAS3.MakeObserverAtGeocenter

        End Sub

        Public Sub MakeObserverInSpace(ByVal ScPos() As Double, ByVal ScVel() As Double, ByRef ObsInSpace As Observer) Implements INOVAS3.MakeObserverInSpace

        End Sub

        Public Sub MakeObserverOnSurface(ByVal Latitude As Double, ByVal Longitude As Double, ByVal Height As Double, ByVal Temperature As Double, ByVal Pressure As Double, ByRef ObsOnSurface As Observer) Implements INOVAS3.MakeObserverOnSurface

        End Sub

        Public Sub MakeOnSurface(ByVal Latitude As Double, ByVal Longitude As Double, ByVal Height As Double, ByVal Temperature As Double, ByVal Pressure As Double, ByRef ObsSurface As OnSurface) Implements INOVAS3.MakeOnSurface

        End Sub

        Public Function MeanObliq(ByVal JdTdb As Double) As Double Implements INOVAS3.MeanObliq

        End Function

        Public Function MeanStar(ByVal JdTt As Double, ByVal Ra As Double, ByVal Dec As Double, ByVal Accuracy As Accuracy, ByRef IRa As Double, ByRef IDec As Double) As Short Implements INOVAS3.MeanStar

        End Function

        Public Function NormAng(ByVal Angle As Double) As Double Implements INOVAS3.NormAng

        End Function

        Public Sub Nutation(ByVal JdTdb As Double, ByVal Direction As NutationDirection, ByVal Accuracy As Accuracy, ByVal Pos() As Double, ByRef Pos2() As Double) Implements INOVAS3.Nutation

        End Sub

        Public Sub NutationAngles(ByVal t As Double, ByVal Accuracy As Accuracy, ByRef DPsi As Double, ByRef DEps As Double) Implements INOVAS3.NutationAngles

        End Sub

        Public Function Precession(ByVal JdTdb1 As Double, ByVal Pos1() As Double, ByVal JdTdb2 As Double, ByRef Pos2() As Double) As Short Implements INOVAS3.Precession

        End Function

        Public Sub ProperMotion(ByVal JdTdb1 As Double, ByVal Pos() As Double, ByVal Vel() As Double, ByVal JdTdb2 As Double, ByRef Pos2() As Double) Implements INOVAS3.ProperMotion

        End Sub

        Public Sub RaDec2Vector(ByVal Ra As Double, ByVal Dec As Double, ByVal Dist As Double, ByRef Vector() As Double) Implements INOVAS3.RaDec2Vector

        End Sub

        Public Sub RadVel(ByVal CelObject As Object3, ByVal Pos() As Double, ByVal Vel() As Double, ByVal VelObs() As Double, ByVal DObsGeo As Double, ByVal DObsSun As Double, ByVal DObjSun As Double, ByRef Rv As Double) Implements INOVAS3.RadVel

        End Sub

        Public Function ReadEph(ByVal Mp As Integer, ByVal Name As String, ByVal Jd As Double, ByRef Err As Integer) As Double() Implements INOVAS3.ReadEph
            Return New Double() {0.0}
        End Function

        Public Function Refract(ByVal Location As OnSurface, ByVal RefOption As RefractionOption, ByVal ZdObs As Double) As Double Implements INOVAS3.Refract

        End Function

        Public Sub Spin(ByVal Angle As Double, ByVal Pos1() As Double, ByRef Pos2() As Double) Implements INOVAS3.Spin

        End Sub

        Public Sub StarVectors(ByVal Star As CatEntry3, ByRef Pos() As Double, ByRef Vel() As Double) Implements INOVAS3.StarVectors

        End Sub

        Public Sub Tdb2Tt(ByVal TdbJd As Double, ByRef TtJd As Double, ByRef SecDiff As Double) Implements INOVAS3.Tdb2Tt

        End Sub

        Public Function Ter2Cel(ByVal JdHigh As Double, ByVal JdLow As Double, ByVal DeltaT As Double, ByVal Method As Method, ByVal Accuracy As Accuracy, ByVal OutputOption As OutputVectorOption, ByVal x As Double, ByVal y As Double, ByVal VecT() As Double, ByRef VecC() As Double) As Short Implements INOVAS3.Ter2Cel

        End Function

        Public Sub Terra(ByVal Location As OnSurface, ByVal St As Double, ByRef Pos() As Double, ByRef Vel() As Double) Implements INOVAS3.Terra

        End Sub

        Public Function TopoPlanet(ByVal JdTt As Double, ByVal SsBody As Object3, ByVal DeltaT As Double, ByVal Position As OnSurface, ByVal Accuracy As Accuracy, ByRef Ra As Double, ByRef Dec As Double, ByRef Dis As Double) As Short Implements INOVAS3.TopoPlanet

        End Function

        Public Function TransformCat(ByVal TransformOption As TransformationOption3, ByVal DateInCat As Double, ByVal InCat As CatEntry3, ByVal DateNewCat As Double, ByVal NewCatId As String, ByRef NewCat As CatEntry3) As Short Implements INOVAS3.TransformCat

        End Function

        Public Sub TransformHip(ByVal Hipparcos As CatEntry3, ByRef Hip2000 As CatEntry3) Implements INOVAS3.TransformHip

        End Sub

        Public Function Vector2RaDec(ByVal Pos() As Double, ByRef Ra As Double, ByRef Dec As Double) As Short Implements INOVAS3.Vector2RaDec

        End Function

        Public Function VirtualPlanet(ByVal JdTt As Double, ByVal SsBody As Object3, ByVal Accuracy As Accuracy, ByRef Ra As Double, ByRef Dec As Double, ByRef Dis As Double) As Short Implements INOVAS3.VirtualPlanet

        End Function

        Public Function VirtualStar(ByVal JdTt As Double, ByVal Star As CatEntry3, ByVal Accuracy As Accuracy, ByRef Ra As Double, ByRef Dec As Double) As Short Implements INOVAS3.VirtualStar

        End Function

        Public Sub Wobble(ByVal Tjd As Double, ByVal x As Double, ByVal y As Double, ByVal Pos1() As Double, ByRef Pos2() As Double) Implements INOVAS3.Wobble

        End Sub

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
                                                                                     ByRef output As SkyPos) As Short
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
                                                                                     ByRef output As SkyPos) As Short
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
