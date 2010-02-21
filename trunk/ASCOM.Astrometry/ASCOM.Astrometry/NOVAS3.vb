Imports System.Runtime.InteropServices
Imports System.Environment
Imports ASCOM.Utilities.Exceptions

Namespace NOVAS

    ''' <summary>
    ''' NOVAS3: Class presenting the contents of the USNO NOVAS 3 library published December 2009. 
    ''' NOVAS was developed by the Astronomical Applications department of the United States Naval 
    ''' Observatory.
    ''' </summary>
    ''' <remarks>If you wish to explore or utilise NOVAS3 please see the extensive help document 
    ''' (NOVAS C3.0 Guide.pdf) on the USNO web site at
    ''' <href>http://www.usno.navy.mil/USNO/astronomical-applications/software-products/novas</href>
    ''' in the "C Edition of NOVAS" link. 
    ''' <para>If you use NOVAS, please send an e-mail through this page:
    ''' <href>http://www.usno.navy.mil/help/astronomy-help</href> as this provides evidence to USNO that justifies further 
    ''' improvements and developments of NOVAS capabilities.
    ''' </para>
    '''  </remarks>
    <Guid("74F604BD-6106-40ac-A821-B32F80BF3FED"), _
        ClassInterface(ClassInterfaceType.None), _
        ComVisible(True)> _
        Public Class NOVAS3
        Implements INOVAS3, IDisposable

        Private Const NOVAS32Dll As String = "NOVAS3.dll" 'Names of NOVAS 32 and 64bit DLL files
        Private Const NOVAS64Dll As String = "NOVAS3-64.dll"

        Private Const JPL_EPHEM_FILE_NAME As String = "JPLEPH" 'Name of JPL ephemeredes file
        Private Const JPL_EPHEM_START_DATE As Double = 2305424.5 'First date of data in the ephemeredes file
        Private Const JPL_EPHEM_END_DATE As Double = 2525008.5 'Last date of data in the ephemeredes file

        Private Const NOVAS_DLL_LOCATION As String = "\ASCOM\.net" 'This is appended to the Common Files path
        Private Const RACIO_FILE As String = "\cio_ra.bin" 'Name of the RA of CIO binary data file

#Region "New and IDisposable"
        Sub New()
            Dim rc As Boolean, rc1 As Short

            'Add the ASCOM\.net directory to the DLL search path so that the NOVAS C 32 and 64bit DLLs can be found
            rc = SetDllDirectory(GetFolderPath(SpecialFolder.CommonProgramFiles) & NOVAS_DLL_LOCATION)

            'Establish the location of the file of CIO RAs
            SetRACIOFile(GetFolderPath(SpecialFolder.CommonProgramFiles) & NOVAS_DLL_LOCATION & RACIO_FILE)

            ' Open the ephemerides file and set its applicable date range
            rc1 = Ephem_Open(GetFolderPath(SpecialFolder.CommonProgramFiles) & NOVAS_DLL_LOCATION & "\" & JPL_EPHEM_FILE_NAME, JPL_EPHEM_START_DATE, JPL_EPHEM_END_DATE)
            If rc1 > 0 Then Throw New HelperException("Unable to open ephemeris file: " & GetFolderPath(SpecialFolder.CommonProgramFiles) & NOVAS_DLL_LOCATION & "\" & JPL_EPHEM_FILE_NAME & ", RC: " & rc1)
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

#Region "Private Ephemeris Routines"
        Private Function Ephem_Open(ByVal Ephem_Name As String, _
                                    ByRef JD_Begin As Double, _
                                    ByRef JD_End As Double) As Short

            Dim rc As Short
            If Is64Bit() Then
                rc = Ephem_Open64(Ephem_Name, JD_Begin, JD_End)
            Else
                rc = Ephem_Open32(Ephem_Name, JD_Begin, JD_End)
            End If
            Return rc
        End Function

        Private Function Ephem_Close() As Short
            If Is64Bit() Then
                Return Ephem_Close64()
            Else
                Return Ephem_Close32()
            End If
        End Function
#End Region

#Region "Public Ephemeris Interface"

        Public Function PlanetEphemeris(ByRef Tjd() As Double, _
                                        ByVal Target As Target, _
                                        ByVal Center As Target, _
                                        ByRef Position() As Double, _
                                        ByRef Velocity() As Double) As Short Implements INOVAS3.PlanetEphemeris

            Dim JdHp As New JDHighPrecision, VPos As New PosVector, VVel As New VelVector, rc As Short

            JdHp.JDPart1 = Tjd(0)
            JdHp.JDPart2 = Tjd(1)
            If Is64Bit() Then
                rc = PlanetEphemeris64(JdHp, Target, Center, VPos, VVel)
            Else
                rc = PlanetEphemeris32(JdHp, Target, Center, VPos, VVel)
            End If

            PosVecToArr(VPos, Position)
            VelVecToArr(VVel, Velocity)
            Return rc
        End Function

        Public Function ReadEph(ByVal Mp As Integer, _
                                ByVal Name As String, _
                                ByVal Jd As Double, _
                                ByRef Err As Integer) As Double() Implements INOVAS3.ReadEph

            Const DOUBLE_LENGTH As Integer = 8
            Const NUM_RETURN_VALUES As Integer = 6

            Dim PosVec(NUM_RETURN_VALUES - 1) As Double
            Dim EphPtr As IntPtr
            Dim Bytes(NUM_RETURN_VALUES * DOUBLE_LENGTH) As Byte

            If Is64Bit() Then
                EphPtr = ReadEph64(Mp, Name, Jd, Err)
            Else
                EphPtr = ReadEph32(Mp, Name, Jd, Err)
            End If
            '// Safely marshal unmanaged buffer to byte[] 
            Marshal.Copy(EphPtr, Bytes, 0, NUM_RETURN_VALUES * DOUBLE_LENGTH)

            '// Converter to double[] 
            For i As Integer = 0 To NUM_RETURN_VALUES - 1
                PosVec(i) = BitConverter.ToDouble(Bytes, i * DOUBLE_LENGTH)
            Next

            Return PosVec
        End Function

        Public Function SolarSystem(ByVal Tjd As Double, _
                                    ByVal Body As Body, _
                                    ByVal Origin As Origin, _
                                    ByRef Pos() As Double, _
                                    ByRef Vel() As Double) As Short Implements INOVAS3.SolarSystem

            Dim VPos As New PosVector, VVel As New VelVector, rc As Short

            If Is64Bit() Then
                rc = SolarSystem64(Tjd, CShort(Body), CShort(Origin), VPos, VVel)
            Else
                rc = SolarSystem32(Tjd, CShort(Body), CShort(Origin), VPos, VVel)
            End If

            PosVecToArr(VPos, Pos)
            VelVecToArr(VVel, Vel)
            Return rc
        End Function

        Public Function State(ByRef Jed() As Double, _
                              ByVal Target As Target, _
                              ByRef TargetPos() As Double, _
                              ByRef TargetVel() As Double) As Short Implements INOVAS3.State

            Dim JdHp As New JDHighPrecision, VPos As New PosVector, VVel As New VelVector, rc As Short

            JdHp.JDPart1 = Jed(0)
            JdHp.JDPart2 = Jed(1)
            If Is64Bit() Then
                rc = State64(JdHp, Target, VPos, VVel)
            Else
                rc = State32(JdHp, Target, VPos, VVel)
            End If

            PosVecToArr(VPos, TargetPos)
            VelVecToArr(VVel, TargetVel)
            Return rc
        End Function
#End Region

#Region "Public NOVAS Interface"
        Public Sub Aberration(ByVal Pos() As Double, _
                              ByVal Vel() As Double, _
                              ByVal LightTime As Double, _
                              ByRef Pos2() As Double) Implements INOVAS3.Aberration
            Dim VPos2 As PosVector
            If Is64Bit() Then
                Aberration64(ArrToPosVec(Pos), ArrToVelVec(Vel), LightTime, VPos2)
            Else
                Aberration32(ArrToPosVec(Pos), ArrToVelVec(Vel), LightTime, VPos2)
            End If
            PosVecToArr(VPos2, Pos2)

        End Sub

        Public Function AppPlanet(ByVal JdTt As Double, _
                                  ByVal SsBody As Object3, _
                                  ByVal Accuracy As Accuracy, _
                                  ByRef Ra As Double, _
                                  ByRef Dec As Double, _
                                  ByRef Dis As Double) As Short Implements INOVAS3.AppPlanet
            If Is64Bit() Then
                Return AppPlanet64(JdTt, O3IFromObject3(SsBody), Accuracy, Ra, Dec, Dis)
            Else
                Return AppPlanet32(JdTt, O3IFromObject3(SsBody), Accuracy, Ra, Dec, Dis)
            End If

        End Function

        Public Function AppStar(ByVal JdTt As Double, _
                                ByVal Star As CatEntry3, _
                                ByVal Accuracy As Accuracy, _
                                ByRef Ra As Double, _
                                ByRef Dec As Double) As Short Implements INOVAS3.AppStar
            If Is64Bit() Then
                Return AppStar64(JdTt, Star, Accuracy, Ra, Dec)
            Else
                Return AppStar32(JdTt, Star, Accuracy, Ra, Dec)
            End If
        End Function

        Public Function AstroPlanet(ByVal JdTt As Double, _
                                    ByVal SsBody As Object3, _
                                    ByVal Accuracy As Accuracy, _
                                    ByRef Ra As Double, _
                                    ByRef Dec As Double, _
                                    ByRef Dis As Double) As Short Implements INOVAS3.AstroPlanet
            If Is64Bit() Then
                Return AstroPlanet64(JdTt, O3IFromObject3(SsBody), Accuracy, Ra, Dec, Dis)
            Else
                Return AstroPlanet32(JdTt, O3IFromObject3(SsBody), Accuracy, Ra, Dec, Dis)
            End If
        End Function

        Public Function AstroStar(ByVal JdTt As Double, _
                                  ByVal Star As CatEntry3, _
                                  ByVal Accuracy As Accuracy, _
                                  ByRef Ra As Double, _
                                  ByRef Dec As Double) As Short Implements INOVAS3.AstroStar

            If Is64Bit() Then
                Return AstroStar64(JdTt, Star, Accuracy, Ra, Dec)
            Else
                Return AstroStar32(JdTt, Star, Accuracy, Ra, Dec)
            End If
        End Function

        Public Sub Bary2Obs(ByVal Pos() As Double, _
                            ByVal PosObs() As Double, _
                            ByRef Pos2() As Double, _
                            ByRef Lighttime As Double) Implements INOVAS3.Bary2Obs
            Dim PosV As New PosVector
            If Is64Bit() Then
                Bary2Obs64(ArrToPosVec(Pos), ArrToPosVec(PosObs), PosV, Lighttime)
                PosVecToArr(PosV, Pos2)
            Else
                Bary2Obs32(ArrToPosVec(Pos), ArrToPosVec(PosObs), PosV, Lighttime)
                PosVecToArr(PosV, Pos2)
            End If
        End Sub

        Public Sub CalDate(ByVal Tjd As Double, _
                           ByRef Year As Short, _
                           ByRef Month As Short, _
                           ByRef Day As Short, _
                           ByRef Hour As Double) Implements INOVAS3.CalDate
            If Is64Bit() Then
                CalDate64(Tjd, Year, Month, Day, Hour)
            Else
                CalDate32(Tjd, Year, Month, Day, Hour)
            End If
        End Sub

        Public Function CelPole(ByVal Tjd As Double, _
                                ByVal Type As PoleOffsetCorrectionType, _
                                ByVal Dpole1 As Double, _
                                ByVal Dpole2 As Double) As Short Implements INOVAS3.CelPole
            If Is64Bit() Then
                Return CelPole64(Tjd, Type, Dpole1, Dpole2)
            Else
                Return CelPole32(Tjd, Type, Dpole1, Dpole2)
            End If
        End Function

        Public Function CioRa(ByVal JdTt As Double, _
                              ByVal Accuracy As Accuracy, _
                              ByRef RaCio As Double) As Short Implements INOVAS3.CioRa
            If Is64Bit() Then
                Return CioRa64(JdTt, Accuracy, RaCio)
            Else
                Return CioRa32(JdTt, Accuracy, RaCio)
            End If
        End Function

        Public Function CioArray(ByVal JdTdb As Double, _
                                 ByVal NPts As Integer, _
                                 ByRef Cio As ArrayList) As Short Implements INOVAS3.CioArray
            Dim CioStruct As New RAOfCioArray, rc As Short

            CioStruct.Initialise() 'Set internal default values so we can see which elements are changed by the NOVAS DLL.
            If Is64Bit() Then
                rc = CioArray64(JdTdb, NPts, CioStruct)
            Else
                rc = CioArray32(JdTdb, NPts, CioStruct)
            End If

            RACioArrayStructureToArr(CioStruct, Cio) 'Copy data from the CioStruct structure to the returning arraylist
            Return rc
        End Function

        Public Function CioBasis(ByVal JdTdbEquionx As Double, _
                                 ByVal RaCioEquionx As Double, _
                                 ByVal RefSys As ReferenceSystem, _
                                 ByVal Accuracy As Accuracy, _
                                 ByRef x As Double, _
                                 ByRef y As Double, _
                                 ByRef z As Double) As Short Implements INOVAS3.CioBasis
            If Is64Bit() Then
                Return CioBasis64(JdTdbEquionx, RaCioEquionx, RefSys, Accuracy, x, y, z)
            Else
                Return CioBasis32(JdTdbEquionx, RaCioEquionx, RefSys, Accuracy, x, y, z)
            End If
        End Function

        Public Function CioLocation(ByVal JdTdb As Double, _
                                    ByVal Accuracy As Accuracy, _
                                    ByRef RaCio As Double, _
                                    ByRef RefSys As ReferenceSystem) As Short Implements INOVAS3.CioLocation
            If Is64Bit() Then
                Return CioLocation64(JdTdb, Accuracy, RaCio, RefSys)
            Else
                Return CioLocation32(JdTdb, Accuracy, RaCio, RefSys)
            End If
        End Function

        Public Function DLight(ByVal Pos1() As Double, _
                               ByVal PosObs() As Double) As Double Implements INOVAS3.DLight
            If Is64Bit() Then
                Return DLight64(ArrToPosVec(Pos1), ArrToPosVec(PosObs))
            Else
                Return DLight32(ArrToPosVec(Pos1), ArrToPosVec(PosObs))
            End If
        End Function

        Public Function Ecl2EquVec(ByVal JdTt As Double, _
                                   ByVal CoordSys As CoordSys, _
                                   ByVal Accuracy As Accuracy, _
                                   ByVal Pos1() As Double, _
                                   ByRef Pos2() As Double) As Short Implements INOVAS3.Ecl2EquVec
            Dim VPos2 As New PosVector, rc As Short
            If Is64Bit() Then
                rc = Ecl2EquVec64(JdTt, CoordSys, Accuracy, ArrToPosVec(Pos1), VPos2)
            Else
                rc = Ecl2EquVec32(JdTt, CoordSys, Accuracy, ArrToPosVec(Pos1), VPos2)
            End If

            PosVecToArr(VPos2, Pos2)
            Return rc
        End Function

        Public Function EeCt(ByVal JdHigh As Double, _
                             ByVal JdLow As Double, _
                             ByVal Accuracy As Accuracy) As Double Implements INOVAS3.EeCt
            If Is64Bit() Then
                Return EeCt64(JdHigh, JdLow, Accuracy)
            Else
                Return EeCt32(JdHigh, JdLow, Accuracy)
            End If
        End Function

        Public Function Ephemeris(ByVal Jd() As Double, _
                                  ByVal CelObj As Object3, _
                                  ByVal Origin As Origin, _
                                  ByVal Accuracy As Accuracy, _
                                  ByRef Pos() As Double, _
                                  ByRef Vel() As Double) As Short Implements INOVAS3.Ephemeris
            Dim VPos As New PosVector, VVel As New VelVector, JdHp As JDHighPrecision, rc As Short
            JdHp.JDPart1 = Jd(0)
            JdHp.JDPart2 = Jd(1)
            If Is64Bit() Then
                rc = Ephemeris64(JdHp, O3IFromObject3(CelObj), Origin, Accuracy, VPos, VVel)
            Else
                rc = Ephemeris32(JdHp, O3IFromObject3(CelObj), Origin, Accuracy, VPos, VVel)
            End If

            PosVecToArr(VPos, Pos)
            VelVecToArr(VVel, Vel)
            Return rc
        End Function

        Public Function Equ2Ecl(ByVal JdTt As Double, _
                                ByVal CoordSys As CoordSys, _
                                ByVal Accuracy As Accuracy, _
                                ByVal Ra As Double, _
                                ByVal Dec As Double, _
                                ByRef ELon As Double, _
                                ByRef ELat As Double) As Short Implements INOVAS3.Equ2Ecl
            If Is64Bit() Then
                Return Equ2Ecl64(JdTt, CoordSys, Accuracy, Ra, Dec, ELon, ELat)
            Else
                Return Equ2Ecl32(JdTt, CoordSys, Accuracy, Ra, Dec, ELon, ELat)
            End If
        End Function

        Public Function Equ2EclVec(ByVal JdTt As Double, _
                                   ByVal CoordSys As CoordSys, _
                                   ByVal Accuracy As Accuracy, _
                                   ByVal Pos1() As Double, _
                                   ByRef Pos2() As Double) As Short Implements INOVAS3.Equ2EclVec
            Dim VPos2 As New PosVector, rc As Short
            If Is64Bit() Then
                rc = Equ2EclVec64(JdTt, CoordSys, Accuracy, ArrToPosVec(Pos1), VPos2)
            Else
                rc = Equ2EclVec32(JdTt, CoordSys, Accuracy, ArrToPosVec(Pos1), VPos2)
            End If

            PosVecToArr(VPos2, Pos2)
            Return rc
        End Function

        Public Sub Equ2Gal(ByVal RaI As Double, _
                           ByVal DecI As Double, _
                           ByRef GLon As Double, _
                           ByRef GLat As Double) Implements INOVAS3.Equ2Gal
            If Is64Bit() Then
                Equ2Gal64(RaI, DecI, GLon, GLat)
            Else
                Equ2Gal32(RaI, DecI, GLon, GLat)
            End If
        End Sub

        Public Sub Equ2Hor(ByVal Jd_Ut1 As Double, _
                           ByVal DeltT As Double, _
                           ByVal Accuracy As Accuracy, _
                           ByVal x As Double, _
                           ByVal y As Double, _
                           ByVal Location As OnSurface, _
                           ByVal Ra As Double, _
                           ByVal Dec As Double, _
                           ByVal RefOption As RefractionOption, _
                           ByRef Zd As Double, _
                           ByRef Az As Double, _
                           ByRef RaR As Double, _
                           ByRef DecR As Double) Implements INOVAS3.Equ2Hor
            If Is64Bit() Then
                Equ2Hor64(Jd_Ut1, DeltT, Accuracy, x, y, Location, Ra, Dec, RefOption, Zd, Az, RaR, DecR)
            Else
                Equ2Hor32(Jd_Ut1, DeltT, Accuracy, x, y, Location, Ra, Dec, RefOption, Zd, Az, RaR, DecR)
            End If
        End Sub

        Public Function Era(ByVal JdHigh As Double, _
                            ByVal JdLow As Double) As Double Implements INOVAS3.Era
            If Is64Bit() Then
                Return Era64(JdHigh, JdLow)
            Else
                Return Era32(JdHigh, JdLow)
            End If
        End Function

        Public Sub ETilt(ByVal JdTdb As Double, _
                         ByVal Accuracy As Accuracy, _
                         ByRef Mobl As Double, _
                         ByRef Tobl As Double, _
                         ByRef Ee As Double, _
                         ByRef Dpsi As Double, _
                         ByRef Deps As Double) Implements INOVAS3.ETilt
            If Is64Bit() Then
                ETilt64(JdTdb, Accuracy, Mobl, Tobl, Ee, Dpsi, Deps)
            Else
                ETilt32(JdTdb, Accuracy, Mobl, Tobl, Ee, Dpsi, Deps)
            End If
        End Sub

        Public Sub FrameTie(ByVal Pos1() As Double, _
                            ByVal Direction As FrameConversionDirection, _
                            ByRef Pos2() As Double) Implements INOVAS3.FrameTie
            Dim VPos2 As New PosVector

            If Is64Bit() Then
                FrameTie64(ArrToPosVec(Pos1), Direction, VPos2)
            Else
                FrameTie32(ArrToPosVec(Pos1), Direction, VPos2)
            End If
            PosVecToArr(VPos2, Pos2)
        End Sub

        Public Sub FundArgs(ByVal t As Double, _
                            ByRef a() As Double) Implements INOVAS3.FundArgs
            Dim va As New FundamentalArgs

            If Is64Bit() Then
                FundArgs64(t, va)
            Else
                FundArgs32(t, va)
            End If

            a(0) = va.l
            a(1) = va.ldash
            a(2) = va.F
            a(3) = va.D
            a(4) = va.Omega

        End Sub

        Public Function Gcrs2Equ(ByVal JdTt As Double, _
                                 ByVal CoordSys As CoordSys, _
                                 ByVal Accuracy As Accuracy, _
                                 ByVal RaG As Double, _
                                 ByVal DecG As Double, _
                                 ByRef Ra As Double, _
                                 ByRef Dec As Double) As Short Implements INOVAS3.Gcrs2Equ
            If Is64Bit() Then
                Return Gcrs2Equ64(JdTt, CoordSys, Accuracy, RaG, DecG, Ra, Dec)
            Else
                Return Gcrs2Equ32(JdTt, CoordSys, Accuracy, RaG, DecG, Ra, Dec)
            End If
        End Function

        Public Function GeoPosVel(ByVal JdTt As Double, _
                                  ByVal DeltaT As Double, _
                                  ByVal Accuracy As Accuracy, _
                                  ByVal Obs As Observer, _
                                  ByRef Pos() As Double, _
                                  ByRef Vel() As Double) As Short Implements INOVAS3.GeoPosVel
            Dim VPos As New PosVector, VVel As New VelVector, rc As Short

            If Is64Bit() Then
                rc = GeoPosVel64(JdTt, DeltaT, Accuracy, Obs, VPos, VVel)
            Else
                rc = GeoPosVel32(JdTt, DeltaT, Accuracy, Obs, VPos, VVel)
            End If

            PosVecToArr(VPos, Pos)
            VelVecToArr(VVel, Vel)
            Return rc
        End Function

        Public Function GravDef(ByVal JdTdb As Double, _
                                ByVal LocCode As EarthDeflection, _
                                ByVal Accuracy As Accuracy, _
                                ByVal Pos1() As Double, _
                                ByVal PosObs() As Double, _
                                ByRef Pos2() As Double) As Short Implements INOVAS3.GravDef
            Dim VPos2 As New PosVector, rc As Short

            If Is64Bit() Then
                rc = GravDef64(JdTdb, LocCode, Accuracy, ArrToPosVec(Pos1), ArrToPosVec(PosObs), VPos2)
            Else
                rc = GravDef32(JdTdb, LocCode, Accuracy, ArrToPosVec(Pos1), ArrToPosVec(PosObs), VPos2)
            End If

            PosVecToArr(VPos2, Pos2)
            Return rc
        End Function

        Public Sub GravVec(ByVal Pos1() As Double, _
                           ByVal PosObs() As Double, _
                           ByVal PosBody() As Double, _
                           ByVal RMass As Double, _
                           ByRef Pos2() As Double) Implements INOVAS3.GravVec
            Dim VPos2 As New PosVector

            If Is64Bit() Then
                GravVec64(ArrToPosVec(Pos1), ArrToPosVec(PosObs), ArrToPosVec(PosBody), RMass, VPos2)
            Else
                GravVec32(ArrToPosVec(Pos1), ArrToPosVec(PosObs), ArrToPosVec(PosBody), RMass, VPos2)
            End If

            PosVecToArr(VPos2, Pos2)
        End Sub

        Public Function IraEquinox(ByVal JdTdb As Double, _
                                   ByVal Equinox As EquinoxType, _
                                   ByVal Accuracy As Accuracy) As Double Implements INOVAS3.IraEquinox
            If Is64Bit() Then
                Return IraEquinox64(JdTdb, Equinox, Accuracy)
            Else
                Return IraEquinox32(JdTdb, Equinox, Accuracy)
            End If
        End Function

        Public Function JulianDate(ByVal year As Short, _
                                   ByVal month As Short, _
                                   ByVal day As Short, _
                                   ByVal hour As Double) As Double Implements INOVAS3.JulianDate
            If Is64Bit() Then
                Return JulianDate64(year, month, day, hour)
            Else
                Return JulianDate32(year, month, day, hour)
            End If
        End Function

        Public Function LightTime(ByVal JdTdb As Double, _
                                  ByVal SsObject As Object3, _
                                  ByVal PosObs() As Double, _
                                  ByVal TLight0 As Double, _
                                  ByVal Accuracy As Accuracy, _
                                  ByRef Pos() As Double, _
                                  ByRef TLight As Double) As Short Implements INOVAS3.LightTime
            Dim VPos As New PosVector, rc As Short
            If Is64Bit() Then
                rc = LightTime64(JdTdb, O3IFromObject3(SsObject), ArrToPosVec(PosObs), TLight0, Accuracy, VPos, TLight)
            Else
                rc = LightTime32(JdTdb, O3IFromObject3(SsObject), ArrToPosVec(PosObs), TLight0, Accuracy, VPos, TLight)
            End If

            PosVecToArr(VPos, Pos)
            Return rc
        End Function

        Public Sub LimbAngle(ByVal PosObj() As Double, _
                             ByVal PosObs() As Double, _
                             ByRef LimbAng As Double, _
                             ByRef NadirAng As Double) Implements INOVAS3.LimbAngle
            If Is64Bit() Then
                LimbAngle64(ArrToPosVec(PosObj), ArrToPosVec(PosObs), LimbAng, NadirAng)
            Else
                LimbAngle32(ArrToPosVec(PosObj), ArrToPosVec(PosObs), LimbAng, NadirAng)
            End If
        End Sub

        Public Function LocalPlanet(ByVal JdTt As Double, _
                                    ByVal SsBody As Object3, _
                                    ByVal DeltaT As Double, _
                                    ByVal Position As OnSurface, _
                                    ByVal Accuracy As Accuracy, _
                                    ByRef Ra As Double, _
                                    ByRef Dec As Double, _
                                    ByRef Dis As Double) As Short Implements INOVAS3.LocalPlanet
            If Is64Bit() Then
                Return LocalPlanet64(JdTt, O3IFromObject3(SsBody), DeltaT, Position, Accuracy, Ra, Dec, Dis)
            Else
                Return LocalPlanet32(JdTt, O3IFromObject3(SsBody), DeltaT, Position, Accuracy, Ra, Dec, Dis)
            End If
        End Function

        Public Function LocalStar(ByVal JdTt As Double, _
                                  ByVal DeltaT As Double, _
                                  ByVal Star As CatEntry3, _
                                  ByVal Position As OnSurface, _
                                  ByVal Accuracy As Accuracy, _
                                  ByRef Ra As Double, _
                                  ByRef Dec As Double) As Short Implements INOVAS3.LocalStar
            If Is64Bit() Then
                Return LocalStar64(JdTt, DeltaT, Star, Position, Accuracy, Ra, Dec)
            Else
                Return LocalStar32(JdTt, DeltaT, Star, Position, Accuracy, Ra, Dec)
            End If
        End Function

        Public Sub MakeCatEntry(ByVal StarName As String, _
                                ByVal Catalog As String, _
                                ByVal StarNum As Integer, _
                                ByVal Ra As Double, _
                                ByVal Dec As Double, _
                                ByVal PmRa As Double, _
                                ByVal PmDec As Double, _
                                ByVal Parallax As Double, _
                                ByVal RadVel As Double, _
                                ByRef Star As CatEntry3) Implements INOVAS3.MakeCatEntry
            If Is64Bit() Then
                MakeCatEntry64(StarName, Catalog, StarNum, Ra, Dec, PmRa, PmDec, Parallax, RadVel, Star)
            Else
                MakeCatEntry32(StarName, Catalog, StarNum, Ra, Dec, PmRa, PmDec, Parallax, RadVel, Star)
            End If
        End Sub

        Public Sub MakeInSpace(ByVal ScPos() As Double, _
                               ByVal ScVel() As Double, _
                               ByRef ObsSpace As InSpace) Implements INOVAS3.MakeInSpace
            If Is64Bit() Then
                MakeInSpace64(ArrToPosVec(ScPos), ArrToVelVec(ScVel), ObsSpace)
            Else
                MakeInSpace32(ArrToPosVec(ScPos), ArrToVelVec(ScVel), ObsSpace)
            End If
        End Sub

        Public Function MakeObject(ByVal Type As ObjectType, _
                                   ByVal Number As Short, _
                                   ByVal Name As String, _
                                   ByVal StarData As CatEntry3, _
                                   ByRef CelObj As Object3) As Short Implements INOVAS3.MakeObject
            Dim O3I As New Object3Internal, rc As Short

            If Is64Bit() Then
                rc = MakeObject64(Type, Number, Name, StarData, O3I)
            Else
                rc = MakeObject32(Type, Number, Name, StarData, O3I)
            End If
            O3FromO3Internal(O3I, CelObj)
            Return rc
        End Function

        Public Function MakeObserver(ByVal Where As ObserverLocation, _
                                     ByVal ObsSurface As OnSurface, _
                                     ByVal ObsSpace As InSpace, _
                                     ByRef Obs As Observer) As Short Implements INOVAS3.MakeObserver
            If Is64Bit() Then
                Return MakeObserver64(Where, ObsSurface, ObsSpace, Obs)
            Else
                Return MakeObserver32(Where, ObsSurface, ObsSpace, Obs)
            End If
        End Function

        Public Sub MakeObserverAtGeocenter(ByRef ObsAtGeocenter As Observer) Implements INOVAS3.MakeObserverAtGeocenter
            If Is64Bit() Then
                MakeObserverAtGeocenter64(ObsAtGeocenter)
            Else
                MakeObserverAtGeocenter32(ObsAtGeocenter)
            End If
        End Sub

        Public Sub MakeObserverInSpace(ByVal ScPos() As Double, _
                                       ByVal ScVel() As Double, _
                                       ByRef ObsInSpace As Observer) Implements INOVAS3.MakeObserverInSpace
            If Is64Bit() Then
                MakeObserverInSpace64(ArrToPosVec(ScPos), ArrToVelVec(ScVel), ObsInSpace)
            Else
                MakeObserverInSpace32(ArrToPosVec(ScPos), ArrToVelVec(ScVel), ObsInSpace)
            End If
        End Sub

        Public Sub MakeObserverOnSurface(ByVal Latitude As Double, _
                                         ByVal Longitude As Double, _
                                         ByVal Height As Double, _
                                         ByVal Temperature As Double, _
                                         ByVal Pressure As Double, _
                                         ByRef ObsOnSurface As Observer) Implements INOVAS3.MakeObserverOnSurface
            If Is64Bit() Then
                MakeObserverOnSurface64(Latitude, Longitude, Height, Temperature, Pressure, ObsOnSurface)
            Else
                MakeObserverOnSurface32(Latitude, Longitude, Height, Temperature, Pressure, ObsOnSurface)
            End If
        End Sub

        Public Sub MakeOnSurface(ByVal Latitude As Double, _
                                 ByVal Longitude As Double, _
                                 ByVal Height As Double, _
                                 ByVal Temperature As Double, _
                                 ByVal Pressure As Double, _
                                 ByRef ObsSurface As OnSurface) Implements INOVAS3.MakeOnSurface
            If Is64Bit() Then
                MakeOnSurface64(Latitude, Longitude, Height, Temperature, Pressure, ObsSurface)
            Else
                MakeOnSurface32(Latitude, Longitude, Height, Temperature, Pressure, ObsSurface)
            End If
        End Sub

        Public Function MeanObliq(ByVal JdTdb As Double) As Double Implements INOVAS3.MeanObliq
            If Is64Bit() Then
                Return MeanObliq64(JdTdb)
            Else
                Return MeanObliq32(JdTdb)
            End If

        End Function

        Public Function MeanStar(ByVal JdTt As Double, _
                                 ByVal Ra As Double, _
                                 ByVal Dec As Double, _
                                 ByVal Accuracy As Accuracy, _
                                 ByRef IRa As Double, _
                                 ByRef IDec As Double) As Short Implements INOVAS3.MeanStar
            If Is64Bit() Then
                Return MeanStar64(JdTt, Ra, Dec, Accuracy, IRa, IDec)
            Else
                Return MeanStar32(JdTt, Ra, Dec, Accuracy, IRa, IDec)
            End If
        End Function

        Public Function NormAng(ByVal Angle As Double) As Double Implements INOVAS3.NormAng
            If Is64Bit() Then
                Return NormAng64(Angle)
            Else
                Return NormAng32(Angle)
            End If
        End Function

        Public Sub Nutation(ByVal JdTdb As Double, _
                            ByVal Direction As NutationDirection, _
                            ByVal Accuracy As Accuracy, _
                            ByVal Pos() As Double, _
                            ByRef Pos2() As Double) Implements INOVAS3.Nutation
            Dim VPOs2 As New PosVector

            If Is64Bit() Then
                Nutation64(JdTdb, Direction, Accuracy, ArrToPosVec(Pos), VPOs2)
            Else
                Nutation32(JdTdb, Direction, Accuracy, ArrToPosVec(Pos), VPOs2)
            End If
            PosVecToArr(VPOs2, Pos2)
        End Sub

        Public Sub NutationAngles(ByVal t As Double, _
                                  ByVal Accuracy As Accuracy, _
                                  ByRef DPsi As Double, _
                                  ByRef DEps As Double) Implements INOVAS3.NutationAngles
            If Is64Bit() Then
                NutationAngles64(t, Accuracy, DPsi, DEps)
            Else
                NutationAngles32(t, Accuracy, DPsi, DEps)
            End If
        End Sub

        Public Function Place(ByVal JdTt As Double, _
                           ByVal CelObject As Object3, _
                           ByVal Location As Observer, _
                           ByVal DeltaT As Double, _
                           ByVal CoordSys As CoordSys, _
                           ByVal Accuracy As Accuracy, _
                           ByRef Output As SkyPos) As Short Implements INOVAS3.Place
            If Is64Bit() Then
                Return Place64(JdTt, O3IFromObject3(CelObject), Location, DeltaT, CoordSys, Accuracy, Output)
            Else
                Return Place32(JdTt, O3IFromObject3(CelObject), Location, DeltaT, CoordSys, Accuracy, Output)
            End If
        End Function

        Public Function Precession(ByVal JdTdb1 As Double, _
                                   ByVal Pos1() As Double, _
                                   ByVal JdTdb2 As Double, _
                                   ByRef Pos2() As Double) As Short Implements INOVAS3.Precession

            Dim VPos2 As New PosVector, rc As Short

            If Is64Bit() Then
                rc = Precession64(JdTdb1, ArrToPosVec(Pos1), JdTdb2, VPos2)
            Else
                rc = Precession32(JdTdb1, ArrToPosVec(Pos1), JdTdb2, VPos2)
            End If
            PosVecToArr(VPos2, Pos2)
            Return rc
        End Function

        Public Sub ProperMotion(ByVal JdTdb1 As Double, _
                                ByVal Pos() As Double, _
                                ByVal Vel() As Double, _
                                ByVal JdTdb2 As Double, _
                                ByRef Pos2() As Double) Implements INOVAS3.ProperMotion
            Dim VPos2 As New PosVector

            If Is64Bit() Then
                ProperMotion64(JdTdb1, ArrToPosVec(Pos), ArrToVelVec(Vel), JdTdb2, VPos2)
            Else
                ProperMotion32(JdTdb1, ArrToPosVec(Pos), ArrToVelVec(Vel), JdTdb2, VPos2)
            End If

            PosVecToArr(VPos2, Pos2)
        End Sub

        Public Sub RaDec2Vector(ByVal Ra As Double, _
                                ByVal Dec As Double, _
                                ByVal Dist As Double, _
                                ByRef Vector() As Double) Implements INOVAS3.RaDec2Vector
            Dim VVector As New PosVector

            If Is64Bit() Then
                RaDec2Vector64(Ra, Dec, Dist, VVector)
            Else
                RaDec2Vector32(Ra, Dec, Dist, VVector)
            End If
            PosVecToArr(VVector, Vector)
        End Sub

        Public Sub RadVel(ByVal CelObject As Object3, _
                          ByVal Pos() As Double, _
                          ByVal Vel() As Double, _
                          ByVal VelObs() As Double, _
                          ByVal DObsGeo As Double, _
                          ByVal DObsSun As Double, _
                          ByVal DObjSun As Double, _
                          ByRef Rv As Double) Implements INOVAS3.RadVel
            If Is64Bit() Then
                RadVel64(O3IFromObject3(CelObject), ArrToPosVec(Pos), ArrToVelVec(Vel), ArrToVelVec(VelObs), DObsGeo, DObsSun, DObjSun, Rv)
            Else
                RadVel32(O3IFromObject3(CelObject), ArrToPosVec(Pos), ArrToVelVec(Vel), ArrToVelVec(VelObs), DObsGeo, DObsSun, DObjSun, Rv)
            End If
        End Sub

        Public Function Refract(ByVal Location As OnSurface, _
                                ByVal RefOption As RefractionOption, _
                                ByVal ZdObs As Double) As Double Implements INOVAS3.Refract
            If Is64Bit() Then
                Return Refract64(Location, RefOption, ZdObs)
            Else
                Return Refract32(Location, RefOption, ZdObs)
            End If
        End Function

        Public Function SiderealTime(ByVal jd_high As Double, _
                                    ByVal jd_low As Double, _
                                    ByVal delta_t As Double, _
                                    ByVal gst_type As GstType, _
                                    ByVal method As Method, _
                                    ByVal accuracy As Accuracy, _
                                    ByRef gst As Double) As Short Implements INOVAS3.SiderealTime

            If Is64Bit() Then
                Return SiderealTime64(jd_high, jd_low, delta_t, gst_type, method, accuracy, gst)
            Else
                Return SiderealTime32(jd_high, jd_low, delta_t, gst_type, method, accuracy, gst)
            End If
        End Function

        Public Sub Spin(ByVal Angle As Double, _
                        ByVal Pos1() As Double, _
                        ByRef Pos2() As Double) Implements INOVAS3.Spin
            Dim VPOs2 As New PosVector
            If Is64Bit() Then
                Spin64(Angle, ArrToPosVec(Pos1), VPOs2)
            Else
                Spin32(Angle, ArrToPosVec(Pos1), VPOs2)
            End If

            PosVecToArr(VPOs2, Pos2)
        End Sub

        Public Sub StarVectors(ByVal Star As CatEntry3, _
                               ByRef Pos() As Double, _
                               ByRef Vel() As Double) Implements INOVAS3.StarVectors
            Dim VPos As New PosVector, VVel As New VelVector
            If Is64Bit() Then
                StarVectors64(Star, VPos, VVel)
            Else
                StarVectors32(Star, VPos, VVel)
            End If

            PosVecToArr(VPos, Pos)
            VelVecToArr(VVel, Vel)
        End Sub

        Public Sub Tdb2Tt(ByVal TdbJd As Double, _
                          ByRef TtJd As Double, _
                          ByRef SecDiff As Double) Implements INOVAS3.Tdb2Tt
            If Is64Bit() Then
                Tdb2Tt64(TdbJd, TtJd, SecDiff)
            Else
                Tdb2Tt32(TdbJd, TtJd, SecDiff)
            End If
        End Sub

        Public Function Ter2Cel(ByVal JdHigh As Double, _
                                ByVal JdLow As Double, _
                                ByVal DeltaT As Double, _
                                ByVal Method As Method, _
                                ByVal Accuracy As Accuracy, _
                                ByVal OutputOption As OutputVectorOption, _
                                ByVal x As Double, _
                                ByVal y As Double, _
                                ByVal VecT() As Double, _
                                ByRef VecC() As Double) As Short Implements INOVAS3.Ter2Cel
            Dim VVecC As New PosVector, rc As Short
            If Is64Bit() Then
                rc = Ter2Cel64(JdHigh, JdLow, DeltaT, Method, Accuracy, OutputOption, x, y, ArrToPosVec(VecT), VVecC)
            Else
                rc = Ter2Cel32(JdHigh, JdLow, DeltaT, Method, Accuracy, OutputOption, x, y, ArrToPosVec(VecT), VVecC)
            End If

            PosVecToArr(VVecC, VecC)
            Return rc
        End Function

        Public Sub Terra(ByVal Location As OnSurface, _
                         ByVal St As Double, _
                         ByRef Pos() As Double, _
                         ByRef Vel() As Double) Implements INOVAS3.Terra
            Dim VPos As New PosVector, VVel As New VelVector
            If Is64Bit() Then
                Terra64(Location, St, VPos, VVel)
            Else
                Terra32(Location, St, VPos, VVel)
            End If

            PosVecToArr(VPos, Pos)
            VelVecToArr(VVel, Vel)
        End Sub

        Public Function TopoPlanet(ByVal JdTt As Double, _
                                   ByVal SsBody As Object3, _
                                   ByVal DeltaT As Double, _
                                   ByVal Position As OnSurface, _
                                   ByVal Accuracy As Accuracy, _
                                   ByRef Ra As Double, _
                                   ByRef Dec As Double, _
                                   ByRef Dis As Double) As Short Implements INOVAS3.TopoPlanet
            If Is64Bit() Then
                Return TopoPlanet64(JdTt, O3IFromObject3(SsBody), DeltaT, Position, Accuracy, Ra, Dec, Dis)
            Else
                Return TopoPlanet32(JdTt, O3IFromObject3(SsBody), DeltaT, Position, Accuracy, Ra, Dec, Dis)
            End If
        End Function

        Public Function TopoStar(ByVal JdTt As Double, _
                                ByVal DeltaT As Double, _
                                ByVal Star As CatEntry3, _
                                ByVal Position As OnSurface, _
                                ByVal Accuracy As Accuracy, _
                                ByRef Ra As Double, _
                                ByRef Dec As Double) As Short Implements INOVAS3.TopoStar
            If Is64Bit() Then
                Return TopoStar64(JdTt, DeltaT, Star, Position, Accuracy, Ra, Dec)
            Else
                Return TopoStar32(JdTt, DeltaT, Star, Position, Accuracy, Ra, Dec)
            End If
        End Function

        Public Function TransformCat(ByVal TransformOption As TransformationOption3, _
                                     ByVal DateInCat As Double, _
                                     ByVal InCat As CatEntry3, _
                                     ByVal DateNewCat As Double, _
                                     ByVal NewCatId As String, _
                                     ByRef NewCat As CatEntry3) As Short Implements INOVAS3.TransformCat
            If Is64Bit() Then
                Return TransformCat64(TransformOption, DateInCat, InCat, DateNewCat, NewCatId, NewCat)
            Else
                Return TransformCat32(TransformOption, DateInCat, InCat, DateNewCat, NewCatId, NewCat)
            End If
        End Function

        Public Sub TransformHip(ByVal Hipparcos As CatEntry3, _
                                ByRef Hip2000 As CatEntry3) Implements INOVAS3.TransformHip
            If Is64Bit() Then
                TransformHip64(Hipparcos, Hip2000)
            Else
                TransformHip32(Hipparcos, Hip2000)
            End If
        End Sub

        Public Function Vector2RaDec(ByVal Pos() As Double, _
                                     ByRef Ra As Double, _
                                     ByRef Dec As Double) As Short Implements INOVAS3.Vector2RaDec
            If Is64Bit() Then
                Return Vector2RaDec64(ArrToPosVec(Pos), Ra, Dec)
            Else
                Return Vector2RaDec32(ArrToPosVec(Pos), Ra, Dec)
            End If
        End Function

        Public Function VirtualPlanet(ByVal JdTt As Double, _
                                      ByVal SsBody As Object3, _
                                      ByVal Accuracy As Accuracy, _
                                      ByRef Ra As Double, _
                                      ByRef Dec As Double, _
                                      ByRef Dis As Double) As Short Implements INOVAS3.VirtualPlanet
            If Is64Bit() Then
                Return VirtualPlanet64(JdTt, O3IFromObject3(SsBody), Accuracy, Ra, Dec, Dis)
            Else
                Return VirtualPlanet32(JdTt, O3IFromObject3(SsBody), Accuracy, Ra, Dec, Dis)
            End If
        End Function

        Public Function VirtualStar(ByVal JdTt As Double, _
                                    ByVal Star As CatEntry3, _
                                    ByVal Accuracy As Accuracy, _
                                    ByRef Ra As Double, _
                                    ByRef Dec As Double) As Short Implements INOVAS3.VirtualStar
            If Is64Bit() Then
                Return VirtualStar64(JdTt, Star, Accuracy, Ra, Dec)
            Else
                Return VirtualStar32(JdTt, Star, Accuracy, Ra, Dec)
            End If
        End Function

        Public Sub Wobble(ByVal Tjd As Double, _
                          ByVal x As Double, _
                          ByVal y As Double, _
                          ByVal Pos1() As Double, _
                          ByRef Pos2() As Double) Implements INOVAS3.Wobble
            Dim VPos2 As New PosVector

            If Is64Bit() Then
                Wobble64(Tjd, x, y, ArrToPosVec(Pos1), VPos2)
            Else
                Wobble32(Tjd, x, y, ArrToPosVec(Pos1), VPos2)
            End If

            PosVecToArr(VPos2, Pos2)
        End Sub


#End Region

#Region "DLL Entry Points Ephemeris (32bit)"
        <DllImportAttribute(NOVAS32Dll, EntryPoint:="Ephem_Close")> _
        Private Shared Function Ephem_Close32() As Short
        End Function

        <DllImportAttribute(NOVAS32Dll, EntryPoint:="Ephem_Open")> _
        Private Shared Function Ephem_Open32(<MarshalAs(UnmanagedType.LPStr)> ByVal Ephem_Name As String, _
                                                                              ByRef JD_Begin As Double, _
                                                                              ByRef JD_End As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="Planet_Ephemeris")> _
        Private Shared Function PlanetEphemeris32(ByRef Tjd As JDHighPrecision, _
                                                  ByVal Target As Target, _
                                                  ByVal Center As Target, _
                                                  ByRef Position As PosVector, _
                                                  ByRef Velocity As VelVector) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="readeph")> _
        Private Shared Function ReadEph32(ByVal Mp As Integer, _
                                          <MarshalAs(UnmanagedType.LPStr)> ByVal Name As String, _
                                          ByVal Jd As Double, _
                                          ByRef Err As Integer) As System.IntPtr
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="solarsystem")> _
        Private Shared Function SolarSystem32(ByVal tjd As Double, _
                                              ByVal body As Short, _
                                              ByVal origin As Short, _
                                              ByRef pos As PosVector, _
                                              ByRef vel As VelVector) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="State")> _
         Private Shared Function State32(ByRef Jed As JDHighPrecision, _
                                         ByVal Target As Target, _
                                         ByRef TargetPos As PosVector, _
                                         ByRef TargetVel As VelVector) As Short
        End Function
#End Region

#Region "DLL Entry Points NOVAS (32bit)"
        <DllImport(NOVAS32Dll, EntryPoint:="aberration")> _
        Private Shared Sub Aberration32(ByRef Pos As PosVector, _
                                        ByRef Vel As VelVector, _
                                        ByVal LightTime As Double, _
                                        ByRef Pos2 As PosVector)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="app_planet")> _
        Private Shared Function AppPlanet32(ByVal JdTt As Double, _
                                            ByRef SsBody As Object3Internal, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef Ra As Double, _
                                            ByRef Dec As Double, _
                                            ByRef Dis As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="app_star")> _
           Private Shared Function AppStar32(ByVal JdTt As Double, _
                                             ByRef Star As CatEntry3, _
                                             ByVal Accuracy As Accuracy, _
                                             ByRef Ra As Double, _
                                             ByRef Dec As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="astro_planet")> _
        Private Shared Function AstroPlanet32(ByVal JdTt As Double, _
                                              ByRef SsBody As Object3Internal, _
                                              ByVal Accuracy As Accuracy, _
                                              ByRef Ra As Double, _
                                              ByRef Dec As Double, _
                                              ByRef Dis As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="astro_star")> _
        Private Shared Function AstroStar32(ByVal JdTt As Double, _
                                            ByRef Star As CatEntry3, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef Ra As Double, _
                                            ByRef Dec As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="bary2obs")> _
        Private Shared Sub Bary2Obs32(ByRef Pos As PosVector, _
                                      ByRef PosObs As PosVector, _
                                      ByRef Pos2 As PosVector, _
                                      ByRef Lighttime As Double)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="cal_date")> _
        Private Shared Sub CalDate32(ByVal Tjd As Double, _
                                     ByRef Year As Short, _
                                     ByRef Month As Short, _
                                     ByRef Day As Short, _
                                     ByRef Hour As Double)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="cel_pole")> _
        Private Shared Function CelPole32(ByVal Tjd As Double, _
                                          ByVal Type As PoleOffsetCorrectionType, _
                                          ByVal Dpole1 As Double, _
                                          ByVal Dpole2 As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="cio_array")> _
        Private Shared Function CioArray32(ByVal JdTdb As Double, _
                                           ByVal NPts As Integer, _
                                           ByRef Cio As RAOfCioArray) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="cio_basis")> _
        Private Shared Function CioBasis32(ByVal JdTdbEquionx As Double, _
                                           ByVal RaCioEquionx As Double, _
                                           ByVal RefSys As ReferenceSystem, _
                                           ByVal Accuracy As Accuracy, _
                                           ByRef x As Double, _
                                           ByRef y As Double, _
                                           ByRef z As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="cio_location")> _
        Private Shared Function CioLocation32(ByVal JdTdb As Double, _
                                              ByVal Accuracy As Accuracy, _
                                              ByRef RaCio As Double, _
                                              ByRef RefSys As ReferenceSystem) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="cio_ra")> _
        Private Shared Function CioRa32(ByVal JdTt As Double, _
                                        ByVal Accuracy As Accuracy, _
                                        ByRef RaCio As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="d_light")> _
        Private Shared Function DLight32(ByRef Pos1 As PosVector, _
                                         ByRef PosObs As PosVector) As Double
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="e_tilt")> _
        Private Shared Sub ETilt32(ByVal JdTdb As Double, _
                                   ByVal Accuracy As Accuracy, _
                                   ByRef Mobl As Double, _
                                   ByRef Tobl As Double, _
                                   ByRef Ee As Double, _
                                   ByRef Dpsi As Double, _
                                   ByRef Deps As Double)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="ecl2equ_vec")> _
        Private Shared Function Ecl2EquVec32(ByVal JdTt As Double, _
                                             ByVal CoordSys As CoordSys, _
                                             ByVal Accuracy As Accuracy, _
                                             ByRef Pos1 As PosVector, _
                                             ByRef Pos2 As PosVector) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="ee_ct")> _
        Private Shared Function EeCt32(ByVal JdHigh As Double, _
                                       ByVal JdLow As Double, _
                                       ByVal Accuracy As Accuracy) As Double
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="ephemeris")> _
        Private Shared Function Ephemeris32(ByRef Jd As JDHighPrecision, _
                                            ByRef CelObj As Object3Internal, _
                                            ByVal Origin As Origin, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef Pos As PosVector, _
                                            ByRef Vel As VelVector) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="equ2ecl")> _
        Private Shared Function Equ2Ecl32(ByVal JdTt As Double, _
                                          ByVal CoordSys As CoordSys, _
                                          ByVal Accuracy As Accuracy, _
                                          ByVal Ra As Double, _
                                          ByVal Dec As Double, _
                                          ByRef ELon As Double, _
                                          ByRef ELat As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="equ2ecl_vec")> _
        Private Shared Function Equ2EclVec32(ByVal JdTt As Double, _
                                             ByVal CoordSys As CoordSys, _
                                             ByVal Accuracy As Accuracy, _
                                             ByRef Pos1 As PosVector, _
                                             ByRef Pos2 As PosVector) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="equ2gal")> _
        Private Shared Sub Equ2Gal32(ByVal RaI As Double, _
                                     ByVal DecI As Double, _
                                     ByRef GLon As Double, _
                                     ByRef GLat As Double)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="equ2hor")> _
        Private Shared Sub Equ2Hor32(ByVal Jd_Ut1 As Double, _
                                     ByVal DeltT As Double, _
                                     ByVal Accuracy As Accuracy, _
                                     ByVal x As Double, _
                                     ByVal y As Double, _
                                     ByRef Location As OnSurface, _
                                     ByVal Ra As Double, _
                                     ByVal Dec As Double, _
                                     ByVal RefOption As RefractionOption, _
                                     ByRef Zd As Double, _
                                     ByRef Az As Double, _
                                     ByRef RaR As Double, _
                                     ByRef DecR As Double)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="era")> _
        Private Shared Function Era32(ByVal JdHigh As Double, _
                                      ByVal JdLow As Double) As Double
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="frame_tie")> _
        Private Shared Sub FrameTie32(ByRef Pos1 As PosVector, _
                                      ByVal Direction As FrameConversionDirection, _
                                      ByRef Pos2 As PosVector)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="fund_args")> _
        Private Shared Sub FundArgs32(ByVal t As Double, _
                                      ByRef a As FundamentalArgs)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="gcrs2equ")> _
         Private Shared Function Gcrs2Equ32(ByVal JdTt As Double, _
                                            ByVal CoordSys As CoordSys, _
                                            ByVal Accuracy As Accuracy, _
                                            ByVal RaG As Double, _
                                            ByVal DecG As Double, _
                                            ByRef Ra As Double, _
                                            ByRef Dec As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="geo_posvel")> _
        Private Shared Function GeoPosVel32(ByVal JdTt As Double, _
                                            ByVal DeltaT As Double, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef Obs As Observer, _
                                            ByRef Pos As PosVector, _
                                            ByRef Vel As VelVector) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="grav_def")> _
        Private Shared Function GravDef32(ByVal JdTdb As Double, _
                                          ByVal LocCode As EarthDeflection, _
                                          ByVal Accuracy As Accuracy, _
                                          ByRef Pos1 As PosVector, _
                                          ByRef PosObs As PosVector, _
                                          ByRef Pos2 As PosVector) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="grav_vec")> _
        Private Shared Sub GravVec32(ByRef Pos1 As PosVector, _
                                     ByRef PosObs As PosVector, _
                                     ByRef PosBody As PosVector, _
                                     ByVal RMass As Double, _
                                     ByRef Pos2 As PosVector)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="ira_equinox")> _
        Private Shared Function IraEquinox32(ByVal JdTdb As Double, _
                                             ByVal Equinox As EquinoxType, _
                                             ByVal Accuracy As Accuracy) As Double
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="julian_date")> _
        Private Shared Function JulianDate32(ByVal Year As Short, _
                                             ByVal Month As Short, _
                                             ByVal Day As Short, _
                                             ByVal Hour As Double) As Double
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="light_time")> _
        Private Shared Function LightTime32(ByVal JdTdb As Double, _
                                            ByRef SsObject As Object3Internal, _
                                            ByRef PosObs As PosVector, _
                                            ByVal TLight0 As Double, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef Pos As PosVector, _
                                            ByRef TLight As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="limb_angle")> _
        Private Shared Sub LimbAngle32(ByRef PosObj As PosVector, _
                                       ByRef PosObs As PosVector, _
                                       ByRef LimbAng As Double, _
                                       ByRef NadirAng As Double)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="local_planet")> _
         Private Shared Function LocalPlanet32(ByVal JdTt As Double, _
                                               ByRef SsBody As Object3Internal, _
                                               ByVal DeltaT As Double, _
                                               ByRef Position As OnSurface, _
                                               ByVal Accuracy As Accuracy, _
                                               ByRef Ra As Double, _
                                               ByRef Dec As Double, _
                                               ByRef Dis As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="local_star")> _
        Private Shared Function LocalStar32(ByVal JdTt As Double, _
                                            ByVal DeltaT As Double, _
                                            ByRef Star As CatEntry3, _
                                            ByRef Position As OnSurface, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef Ra As Double, _
                                            ByRef Dec As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="make_cat_entry")> _
        Private Shared Sub MakeCatEntry32(<MarshalAs(UnmanagedType.LPStr)> ByVal StarName As String, _
                                          <MarshalAs(UnmanagedType.LPStr)> ByVal Catalog As String, _
                                                                           ByVal StarNum As Integer, _
                                                                           ByVal Ra As Double, _
                                                                           ByVal Dec As Double, _
                                                                           ByVal PmRa As Double, _
                                                                           ByVal PmDec As Double, _
                                                                           ByVal Parallax As Double, _
                                                                           ByVal RadVel As Double, _
                                                                           ByRef Star As CatEntry3)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="make_in_space")> _
        Private Shared Sub MakeInSpace32(ByRef ScPos As PosVector, _
                                         ByRef ScVel As VelVector, _
                                         ByRef ObsSpace As InSpace)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="make_object")> _
        Private Shared Function MakeObject32(ByVal Type As ObjectType, _
                                             ByVal Number As Short, _
                                             <MarshalAs(UnmanagedType.LPStr)> ByVal Name As String, _
                                             ByRef StarData As CatEntry3, _
                                             ByRef CelObj As Object3Internal) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="make_observer")> _
        Private Shared Function MakeObserver32(ByVal Where As ObserverLocation, _
                                               ByRef ObsSurface As OnSurface, _
                                               ByRef ObsSpace As InSpace, _
                                               ByRef Obs As Observer) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="make_observer_at_geocenter")> _
        Private Shared Sub MakeObserverAtGeocenter32(ByRef ObsAtGeocenter As Observer)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="make_observer_in_space")> _
        Private Shared Sub MakeObserverInSpace32(ByRef ScPos As PosVector, _
                                                 ByRef ScVel As VelVector, _
                                                 ByRef ObsInSpace As Observer)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="make_observer_on_surface")> _
        Private Shared Sub MakeObserverOnSurface32(ByVal Latitude As Double, _
                                                   ByVal Longitude As Double, _
                                                   ByVal Height As Double, _
                                                   ByVal Temperature As Double, _
                                                   ByVal Pressure As Double, _
                                                   ByRef ObsOnSurface As Observer)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="make_on_surface")> _
        Private Shared Sub MakeOnSurface32(ByVal Latitude As Double, _
                                           ByVal Longitude As Double, _
                                           ByVal Height As Double, _
                                           ByVal Temperature As Double, _
                                           ByVal Pressure As Double, _
                                           ByRef ObsSurface As OnSurface)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="mean_obliq")> _
        Private Shared Function MeanObliq32(ByVal JdTdb As Double) As Double
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="mean_star")> _
        Private Shared Function MeanStar32(ByVal JdTt As Double, _
                                           ByVal Ra As Double, _
                                           ByVal Dec As Double, _
                                           ByVal Accuracy As Accuracy, _
                                           ByRef IRa As Double, _
                                           ByRef IDec As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="norm_ang")> _
        Private Shared Function NormAng32(ByVal Angle As Double) As Double
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="nutation")> _
        Private Shared Sub Nutation32(ByVal JdTdb As Double, _
                                      ByVal Direction As NutationDirection, _
                                      ByVal Accuracy As Accuracy, _
                                      ByRef Pos As PosVector, _
                                      ByRef Pos2 As PosVector)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="nutation_angles")> _
        Private Shared Sub NutationAngles32(ByVal t As Double, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef DPsi As Double, _
                                            ByRef DEps As Double)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="place")> _
        Private Shared Function Place32(ByVal JdTt As Double, _
                                        ByRef CelObject As Object3Internal, _
                                        ByRef Location As Observer, _
                                        ByVal DeltaT As Double, _
                                        ByVal CoordSys As CoordSys, _
                                        ByVal Accuracy As Accuracy, _
                                        ByRef Output As SkyPos) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="precession")> _
        Private Shared Function Precession32(ByVal JdTdb1 As Double, _
                                             ByRef Pos1 As PosVector, _
                                             ByVal JdTdb2 As Double, _
                                             ByRef Pos2 As PosVector) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="proper_motion")> _
        Private Shared Sub ProperMotion32(ByVal JdTdb1 As Double, _
                                          ByRef Pos As PosVector, _
                                          ByRef Vel As VelVector, _
                                          ByVal JdTdb2 As Double, _
                                          ByRef Pos2 As PosVector)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="rad_vel")> _
        Private Shared Sub RadVel32(ByRef CelObject As Object3Internal, _
                                    ByRef Pos As PosVector, _
                                    ByRef Vel As VelVector, _
                                    ByRef VelObs As VelVector, _
                                    ByVal DObsGeo As Double, _
                                    ByVal DObsSun As Double, _
                                    ByVal DObjSun As Double, _
                                    ByRef Rv As Double)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="radec2vector")> _
        Private Shared Sub RaDec2Vector32(ByVal Ra As Double, _
                                          ByVal Dec As Double, _
                                          ByVal Dist As Double, _
                                          ByRef Vector As PosVector)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="refract")> _
        Private Shared Function Refract32(ByRef Location As OnSurface, _
                                          ByVal RefOption As RefractionOption, _
                                          ByVal ZdObs As Double) As Double
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="sidereal_time")> _
        Private Shared Function SiderealTime32(ByVal JdHigh As Double, _
                                               ByVal JdLow As Double, _
                                               ByVal DeltaT As Double, _
                                               ByVal GstType As GstType, _
                                               ByVal Method As Method, _
                                               ByVal Accuracy As Accuracy, _
                                               ByRef Gst As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="spin")> _
        Private Shared Sub Spin32(ByVal Angle As Double, _
                                  ByRef Pos1 As PosVector, _
                                  ByRef Pos2 As PosVector)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="starvectors")> _
        Private Shared Sub StarVectors32(ByRef Star As CatEntry3, _
                                         ByRef Pos As PosVector, _
                                         ByRef Vel As VelVector)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="tdb2tt")> _
        Private Shared Sub Tdb2Tt32(ByVal TdbJd As Double, _
                                    ByRef TtJd As Double, _
                                    ByRef SecDiff As Double)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="ter2cel")> _
        Private Shared Function Ter2Cel32(ByVal JdHigh As Double, _
                                          ByVal JdLow As Double, _
                                          ByVal DeltaT As Double, _
                                          ByVal Method As Method, _
                                          ByVal Accuracy As Accuracy, _
                                          ByVal OutputOption As OutputVectorOption, _
                                          ByVal x As Double, _
                                          ByVal y As Double, _
                                          ByRef VecT As PosVector, _
                                          ByRef VecC As PosVector) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="terra")> _
        Private Shared Sub Terra32(ByRef Location As OnSurface, _
                                   ByVal St As Double, _
                                   ByRef Pos As PosVector, _
                                   ByRef Vel As VelVector)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="topo_planet")> _
        Private Shared Function TopoPlanet32(ByVal JdTt As Double, _
                                             ByRef SsBody As Object3Internal, _
                                             ByVal DeltaT As Double, _
                                             ByRef Position As OnSurface, _
                                             ByVal Accuracy As Accuracy, _
                                             ByRef Ra As Double, _
                                             ByRef Dec As Double, _
                                             ByRef Dis As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="topo_star")> _
        Private Shared Function TopoStar32(ByVal JdTt As Double, _
                                           ByVal DeltaT As Double, _
                                           ByRef Star As CatEntry3, _
                                           ByRef Position As OnSurface, _
                                           ByVal Accuracy As Accuracy, _
                                           ByRef Ra As Double, _
                                           ByRef Dec As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="transform_cat")> _
        Private Shared Function TransformCat32(ByVal TransformOption As TransformationOption3, _
                                               ByVal DateInCat As Double, _
                                               ByRef InCat As CatEntry3, _
                                               ByVal DateNewCat As Double, _
                                               <MarshalAs(UnmanagedType.LPStr)> ByVal NewCatId As String, _
                                               ByRef NewCat As CatEntry3) As Short

        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="transform_hip")> _
        Private Shared Sub TransformHip32(ByRef Hipparcos As CatEntry3, _
                                          ByRef Hip2000 As CatEntry3)
        End Sub

        <DllImport(NOVAS32Dll, EntryPoint:="vector2radec")> _
        Private Shared Function Vector2RaDec32(ByRef Pos As PosVector, _
                                               ByRef Ra As Double, _
                                               ByRef Dec As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="virtual_planet")> _
        Private Shared Function VirtualPlanet32(ByVal JdTt As Double, _
                                                ByRef SsBody As Object3Internal, _
                                                ByVal Accuracy As Accuracy, _
                                                ByRef Ra As Double, _
                                                ByRef Dec As Double, _
                                                ByRef Dis As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="virtual_star")> _
        Private Shared Function VirtualStar32(ByVal JdTt As Double, _
                                              ByRef Star As CatEntry3, _
                                              ByVal Accuracy As Accuracy, _
                                              ByRef Ra As Double, _
                                              ByRef Dec As Double) As Short
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="wobble")> _
        Private Shared Sub Wobble32(ByVal Tjd As Double, _
                                    ByVal x As Double, _
                                    ByVal y As Double, _
                                    ByRef Pos1 As PosVector, _
                                    ByRef Pos2 As PosVector)
        End Sub
#End Region

#Region "DLL Entry Points Ephemeris (64bit)"
        <DllImportAttribute(NOVAS64Dll, EntryPoint:="Ephem_Close")> _
        Private Shared Function Ephem_Close64() As Short
        End Function

        <DllImportAttribute(NOVAS64Dll, EntryPoint:="Ephem_Open")> _
        Private Shared Function Ephem_Open64(<MarshalAs(UnmanagedType.LPStr)> ByVal Ephem_Name As String, _
                                                                              ByRef JD_Begin As Double, _
                                                                              ByRef JD_End As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="Planet_Ephemeris")> _
        Private Shared Function PlanetEphemeris64(ByRef Tjd As JDHighPrecision, _
                                                  ByVal Target As Target, _
                                                  ByVal Center As Target, _
                                                  ByRef Position As PosVector, _
                                                  ByRef Velocity As VelVector) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="readeph")> _
        Private Shared Function ReadEph64(ByVal Mp As Integer, _
                                          <MarshalAs(UnmanagedType.LPStr)> ByVal Name As String, _
                                          ByVal Jd As Double, _
                                          ByRef Err As Integer) As System.IntPtr
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="solarsystem")> _
        Private Shared Function SolarSystem64(ByVal tjd As Double, _
                                              ByVal body As Short, _
                                              ByVal origin As Short, _
                                              ByRef pos As PosVector, _
                                              ByRef vel As VelVector) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="State")> _
         Private Shared Function State64(ByRef Jed As JDHighPrecision, _
                                         ByVal Target As Target, _
                                         ByRef TargetPos As PosVector, _
                                         ByRef TargetVel As VelVector) As Short
        End Function
#End Region

#Region "DLL Entry Points NOVAS (64bit)"
        <DllImport(NOVAS64Dll, EntryPoint:="aberration")> _
        Private Shared Sub Aberration64(ByRef Pos As PosVector, _
                                        ByRef Vel As VelVector, _
                                        ByVal LightTime As Double, _
                                        ByRef Pos2 As PosVector)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="app_planet")> _
        Private Shared Function AppPlanet64(ByVal JdTt As Double, _
                                            ByRef SsBody As Object3Internal, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef Ra As Double, _
                                            ByRef Dec As Double, _
                                            ByRef Dis As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="app_star")> _
           Private Shared Function AppStar64(ByVal JdTt As Double, _
                                             ByRef Star As CatEntry3, _
                                             ByVal Accuracy As Accuracy, _
                                             ByRef Ra As Double, _
                                             ByRef Dec As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="astro_planet")> _
        Private Shared Function AstroPlanet64(ByVal JdTt As Double, _
                                              ByRef SsBody As Object3Internal, _
                                              ByVal Accuracy As Accuracy, _
                                              ByRef Ra As Double, _
                                              ByRef Dec As Double, _
                                              ByRef Dis As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="astro_star")> _
        Private Shared Function AstroStar64(ByVal JdTt As Double, _
                                            ByRef Star As CatEntry3, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef Ra As Double, _
                                            ByRef Dec As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="bary2obs")> _
        Private Shared Sub Bary2Obs64(ByRef Pos As PosVector, _
                                      ByRef PosObs As PosVector, _
                                      ByRef Pos2 As PosVector, _
                                      ByRef Lighttime As Double)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="cal_date")> _
        Private Shared Sub CalDate64(ByVal Tjd As Double, _
                                     ByRef Year As Short, _
                                     ByRef Month As Short, _
                                     ByRef Day As Short, _
                                     ByRef Hour As Double)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="cel_pole")> _
        Private Shared Function CelPole64(ByVal Tjd As Double, _
                                          ByVal Type As PoleOffsetCorrectionType, _
                                          ByVal Dpole1 As Double, _
                                          ByVal Dpole2 As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="cio_array")> _
        Private Shared Function CioArray64(ByVal JdTdb As Double, _
                                           ByVal NPts As Integer, _
                                           ByRef Cio As RAOfCioArray) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="cio_basis")> _
        Private Shared Function CioBasis64(ByVal JdTdbEquionx As Double, _
                                           ByVal RaCioEquionx As Double, _
                                           ByVal RefSys As ReferenceSystem, _
                                           ByVal Accuracy As Accuracy, _
                                           ByRef x As Double, _
                                           ByRef y As Double, _
                                           ByRef z As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="cio_location")> _
        Private Shared Function CioLocation64(ByVal JdTdb As Double, _
                                              ByVal Accuracy As Accuracy, _
                                              ByRef RaCio As Double, _
                                              ByRef RefSys As ReferenceSystem) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="cio_ra")> _
        Private Shared Function CioRa64(ByVal JdTt As Double, _
                                        ByVal Accuracy As Accuracy, _
                                        ByRef RaCio As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="d_light")> _
        Private Shared Function DLight64(ByRef Pos1 As PosVector, _
                                         ByRef PosObs As PosVector) As Double
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="e_tilt")> _
        Private Shared Sub ETilt64(ByVal JdTdb As Double, _
                                   ByVal Accuracy As Accuracy, _
                                   ByRef Mobl As Double, _
                                   ByRef Tobl As Double, _
                                   ByRef Ee As Double, _
                                   ByRef Dpsi As Double, _
                                   ByRef Deps As Double)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="ecl2equ_vec")> _
        Private Shared Function Ecl2EquVec64(ByVal JdTt As Double, _
                                             ByVal CoordSys As CoordSys, _
                                             ByVal Accuracy As Accuracy, _
                                             ByRef Pos1 As PosVector, _
                                             ByRef Pos2 As PosVector) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="ee_ct")> _
        Private Shared Function EeCt64(ByVal JdHigh As Double, _
                                       ByVal JdLow As Double, _
                                       ByVal Accuracy As Accuracy) As Double
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="ephemeris")> _
        Private Shared Function Ephemeris64(ByRef Jd As JDHighPrecision, _
                                            ByRef CelObj As Object3Internal, _
                                            ByVal Origin As Origin, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef Pos As PosVector, _
                                            ByRef Vel As VelVector) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="equ2ecl")> _
        Private Shared Function Equ2Ecl64(ByVal JdTt As Double, _
                                          ByVal CoordSys As CoordSys, _
                                          ByVal Accuracy As Accuracy, _
                                          ByVal Ra As Double, _
                                          ByVal Dec As Double, _
                                          ByRef ELon As Double, _
                                          ByRef ELat As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="equ2ecl_vec")> _
        Private Shared Function Equ2EclVec64(ByVal JdTt As Double, _
                                             ByVal CoordSys As CoordSys, _
                                             ByVal Accuracy As Accuracy, _
                                             ByRef Pos1 As PosVector, _
                                             ByRef Pos2 As PosVector) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="equ2gal")> _
        Private Shared Sub Equ2Gal64(ByVal RaI As Double, _
                                     ByVal DecI As Double, _
                                     ByRef GLon As Double, _
                                     ByRef GLat As Double)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="equ2hor")> _
        Private Shared Sub Equ2Hor64(ByVal Jd_Ut1 As Double, _
                                     ByVal DeltT As Double, _
                                     ByVal Accuracy As Accuracy, _
                                     ByVal x As Double, _
                                     ByVal y As Double, _
                                     ByRef Location As OnSurface, _
                                     ByVal Ra As Double, _
                                     ByVal Dec As Double, _
                                     ByVal RefOption As RefractionOption, _
                                     ByRef Zd As Double, _
                                     ByRef Az As Double, _
                                     ByRef RaR As Double, _
                                     ByRef DecR As Double)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="era")> _
        Private Shared Function Era64(ByVal JdHigh As Double, _
                                      ByVal JdLow As Double) As Double
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="frame_tie")> _
        Private Shared Sub FrameTie64(ByRef Pos1 As PosVector, _
                                      ByVal Direction As FrameConversionDirection, _
                                      ByRef Pos2 As PosVector)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="fund_args")> _
        Private Shared Sub FundArgs64(ByVal t As Double, _
                                      ByRef a As FundamentalArgs)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="gcrs2equ")> _
         Private Shared Function Gcrs2Equ64(ByVal JdTt As Double, _
                                            ByVal CoordSys As CoordSys, _
                                            ByVal Accuracy As Accuracy, _
                                            ByVal RaG As Double, _
                                            ByVal DecG As Double, _
                                            ByRef Ra As Double, _
                                            ByRef Dec As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="geo_posvel")> _
        Private Shared Function GeoPosVel64(ByVal JdTt As Double, _
                                            ByVal DeltaT As Double, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef Obs As Observer, _
                                            ByRef Pos As PosVector, _
                                            ByRef Vel As VelVector) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="grav_def")> _
        Private Shared Function GravDef64(ByVal JdTdb As Double, _
                                          ByVal LocCode As EarthDeflection, _
                                          ByVal Accuracy As Accuracy, _
                                          ByRef Pos1 As PosVector, _
                                          ByRef PosObs As PosVector, _
                                          ByRef Pos2 As PosVector) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="grav_vec")> _
        Private Shared Sub GravVec64(ByRef Pos1 As PosVector, _
                                     ByRef PosObs As PosVector, _
                                     ByRef PosBody As PosVector, _
                                     ByVal RMass As Double, _
                                     ByRef Pos2 As PosVector)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="ira_equinox")> _
        Private Shared Function IraEquinox64(ByVal JdTdb As Double, _
                                             ByVal Equinox As EquinoxType, _
                                             ByVal Accuracy As Accuracy) As Double
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="julian_date")> _
        Private Shared Function JulianDate64(ByVal Year As Short, _
                                             ByVal Month As Short, _
                                             ByVal Day As Short, _
                                             ByVal Hour As Double) As Double
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="light_time")> _
        Private Shared Function LightTime64(ByVal JdTdb As Double, _
                                            ByRef SsObject As Object3Internal, _
                                            ByRef PosObs As PosVector, _
                                            ByVal TLight0 As Double, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef Pos As PosVector, _
                                            ByRef TLight As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="limb_angle")> _
        Private Shared Sub LimbAngle64(ByRef PosObj As PosVector, _
                                       ByRef PosObs As PosVector, _
                                       ByRef LimbAng As Double, _
                                       ByRef NadirAng As Double)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="local_planet")> _
         Private Shared Function LocalPlanet64(ByVal JdTt As Double, _
                                               ByRef SsBody As Object3Internal, _
                                               ByVal DeltaT As Double, _
                                               ByRef Position As OnSurface, _
                                               ByVal Accuracy As Accuracy, _
                                               ByRef Ra As Double, _
                                               ByRef Dec As Double, _
                                               ByRef Dis As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="local_star")> _
        Private Shared Function LocalStar64(ByVal JdTt As Double, _
                                            ByVal DeltaT As Double, _
                                            ByRef Star As CatEntry3, _
                                            ByRef Position As OnSurface, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef Ra As Double, _
                                            ByRef Dec As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="make_cat_entry")> _
        Private Shared Sub MakeCatEntry64(<MarshalAs(UnmanagedType.LPStr)> ByVal StarName As String, _
                                          <MarshalAs(UnmanagedType.LPStr)> ByVal Catalog As String, _
                                                                           ByVal StarNum As Integer, _
                                                                           ByVal Ra As Double, _
                                                                           ByVal Dec As Double, _
                                                                           ByVal PmRa As Double, _
                                                                           ByVal PmDec As Double, _
                                                                           ByVal Parallax As Double, _
                                                                           ByVal RadVel As Double, _
                                                                           ByRef Star As CatEntry3)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="make_in_space")> _
        Private Shared Sub MakeInSpace64(ByRef ScPos As PosVector, _
                                         ByRef ScVel As VelVector, _
                                         ByRef ObsSpace As InSpace)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="make_object")> _
        Private Shared Function MakeObject64(ByVal Type As ObjectType, _
                                             ByVal Number As Short, _
                                             <MarshalAs(UnmanagedType.LPStr)> ByVal Name As String, _
                                             ByRef StarData As CatEntry3, _
                                             ByRef CelObj As Object3Internal) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="make_observer")> _
        Private Shared Function MakeObserver64(ByVal Where As ObserverLocation, _
                                               ByRef ObsSurface As OnSurface, _
                                               ByRef ObsSpace As InSpace, _
                                               ByRef Obs As Observer) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="make_observer_at_geocenter")> _
        Private Shared Sub MakeObserverAtGeocenter64(ByRef ObsAtGeocenter As Observer)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="make_observer_in_space")> _
        Private Shared Sub MakeObserverInSpace64(ByRef ScPos As PosVector, _
                                                 ByRef ScVel As VelVector, _
                                                 ByRef ObsInSpace As Observer)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="make_observer_on_surface")> _
        Private Shared Sub MakeObserverOnSurface64(ByVal Latitude As Double, _
                                                   ByVal Longitude As Double, _
                                                   ByVal Height As Double, _
                                                   ByVal Temperature As Double, _
                                                   ByVal Pressure As Double, _
                                                   ByRef ObsOnSurface As Observer)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="make_on_surface")> _
        Private Shared Sub MakeOnSurface64(ByVal Latitude As Double, _
                                           ByVal Longitude As Double, _
                                           ByVal Height As Double, _
                                           ByVal Temperature As Double, _
                                           ByVal Pressure As Double, _
                                           ByRef ObsSurface As OnSurface)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="mean_obliq")> _
        Private Shared Function MeanObliq64(ByVal JdTdb As Double) As Double
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="mean_star")> _
        Private Shared Function MeanStar64(ByVal JdTt As Double, _
                                           ByVal Ra As Double, _
                                           ByVal Dec As Double, _
                                           ByVal Accuracy As Accuracy, _
                                           ByRef IRa As Double, _
                                           ByRef IDec As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="norm_ang")> _
        Private Shared Function NormAng64(ByVal Angle As Double) As Double
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="nutation")> _
        Private Shared Sub Nutation64(ByVal JdTdb As Double, _
                                      ByVal Direction As NutationDirection, _
                                      ByVal Accuracy As Accuracy, _
                                      ByRef Pos As PosVector, _
                                      ByRef Pos2 As PosVector)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="nutation_angles")> _
        Private Shared Sub NutationAngles64(ByVal t As Double, _
                                            ByVal Accuracy As Accuracy, _
                                            ByRef DPsi As Double, _
                                            ByRef DEps As Double)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="place")> _
        Private Shared Function Place64(ByVal JdTt As Double, _
                                        ByRef CelObject As Object3Internal, _
                                        ByRef Location As Observer, _
                                        ByVal DeltaT As Double, _
                                        ByVal CoordSys As CoordSys, _
                                        ByVal Accuracy As Accuracy, _
                                        ByRef Output As SkyPos) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="precession")> _
        Private Shared Function Precession64(ByVal JdTdb1 As Double, _
                                             ByRef Pos1 As PosVector, _
                                             ByVal JdTdb2 As Double, _
                                             ByRef Pos2 As PosVector) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="proper_motion")> _
        Private Shared Sub ProperMotion64(ByVal JdTdb1 As Double, _
                                          ByRef Pos As PosVector, _
                                          ByRef Vel As VelVector, _
                                          ByVal JdTdb2 As Double, _
                                          ByRef Pos2 As PosVector)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="rad_vel")> _
        Private Shared Sub RadVel64(ByRef CelObject As Object3Internal, _
                                    ByRef Pos As PosVector, _
                                    ByRef Vel As VelVector, _
                                    ByRef VelObs As VelVector, _
                                    ByVal DObsGeo As Double, _
                                    ByVal DObsSun As Double, _
                                    ByVal DObjSun As Double, _
                                    ByRef Rv As Double)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="radec2vector")> _
        Private Shared Sub RaDec2Vector64(ByVal Ra As Double, _
                                          ByVal Dec As Double, _
                                          ByVal Dist As Double, _
                                          ByRef Vector As PosVector)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="refract")> _
        Private Shared Function Refract64(ByRef Location As OnSurface, _
                                          ByVal RefOption As RefractionOption, _
                                          ByVal ZdObs As Double) As Double
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="sidereal_time")> _
        Private Shared Function SiderealTime64(ByVal JdHigh As Double, _
                                               ByVal JdLow As Double, _
                                               ByVal DeltaT As Double, _
                                               ByVal GstType As GstType, _
                                               ByVal Method As Method, _
                                               ByVal Accuracy As Accuracy, _
                                               ByRef Gst As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="spin")> _
        Private Shared Sub Spin64(ByVal Angle As Double, _
                                  ByRef Pos1 As PosVector, _
                                  ByRef Pos2 As PosVector)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="starvectors")> _
        Private Shared Sub StarVectors64(ByRef Star As CatEntry3, _
                                         ByRef Pos As PosVector, _
                                         ByRef Vel As VelVector)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="tdb2tt")> _
        Private Shared Sub Tdb2Tt64(ByVal TdbJd As Double, _
                                    ByRef TtJd As Double, _
                                    ByRef SecDiff As Double)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="ter2cel")> _
        Private Shared Function Ter2Cel64(ByVal JdHigh As Double, _
                                          ByVal JdLow As Double, _
                                          ByVal DeltaT As Double, _
                                          ByVal Method As Method, _
                                          ByVal Accuracy As Accuracy, _
                                          ByVal OutputOption As OutputVectorOption, _
                                          ByVal x As Double, _
                                          ByVal y As Double, _
                                          ByRef VecT As PosVector, _
                                          ByRef VecC As PosVector) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="terra")> _
        Private Shared Sub Terra64(ByRef Location As OnSurface, _
                                   ByVal St As Double, _
                                   ByRef Pos As PosVector, _
                                   ByRef Vel As VelVector)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="topo_planet")> _
        Private Shared Function TopoPlanet64(ByVal JdTt As Double, _
                                             ByRef SsBody As Object3Internal, _
                                             ByVal DeltaT As Double, _
                                             ByRef Position As OnSurface, _
                                             ByVal Accuracy As Accuracy, _
                                             ByRef Ra As Double, _
                                             ByRef Dec As Double, _
                                             ByRef Dis As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="topo_star")> _
        Private Shared Function TopoStar64(ByVal JdTt As Double, _
                                           ByVal DeltaT As Double, _
                                           ByRef Star As CatEntry3, _
                                           ByRef Position As OnSurface, _
                                           ByVal Accuracy As Accuracy, _
                                           ByRef Ra As Double, _
                                           ByRef Dec As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="transform_cat")> _
        Private Shared Function TransformCat64(ByVal TransformOption As TransformationOption3, _
                                               ByVal DateInCat As Double, _
                                               ByRef InCat As CatEntry3, _
                                               ByVal DateNewCat As Double, _
                                               <MarshalAs(UnmanagedType.LPStr)> ByVal NewCatId As String, _
                                               ByRef NewCat As CatEntry3) As Short

        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="transform_hip")> _
        Private Shared Sub TransformHip64(ByRef Hipparcos As CatEntry3, _
                                          ByRef Hip2000 As CatEntry3)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="vector2radec")> _
        Private Shared Function Vector2RaDec64(ByRef Pos As PosVector, _
                                               ByRef Ra As Double, _
                                               ByRef Dec As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="virtual_planet")> _
        Private Shared Function VirtualPlanet64(ByVal JdTt As Double, _
                                                ByRef SsBody As Object3Internal, _
                                                ByVal Accuracy As Accuracy, _
                                                ByRef Ra As Double, _
                                                ByRef Dec As Double, _
                                                ByRef Dis As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="virtual_star")> _
        Private Shared Function VirtualStar64(ByVal JdTt As Double, _
                                              ByRef Star As CatEntry3, _
                                              ByVal Accuracy As Accuracy, _
                                              ByRef Ra As Double, _
                                              ByRef Dec As Double) As Short
        End Function

        <DllImport(NOVAS64Dll, EntryPoint:="wobble")> _
        Private Shared Sub Wobble64(ByVal Tjd As Double, _
                                    ByVal x As Double, _
                                    ByVal y As Double, _
                                    ByRef Pos1 As PosVector, _
                                    ByRef Pos2 As PosVector)
        End Sub
#End Region

#Region "Support Code"
        'Declare the api call that sets the additional DLL search directory
        <DllImport("kernel32.dll", SetLastError:=False)> _
        Private Shared Function SetDllDirectory(ByVal lpPathName As String) As Boolean
        End Function

        <DllImport(NOVAS32Dll, EntryPoint:="set_racio_file")> _
       Private Shared Sub SetRACIOFile32(<MarshalAs(UnmanagedType.LPStr)> ByVal FName As String)
        End Sub

        <DllImport(NOVAS64Dll, EntryPoint:="set_racio_file")> _
        Private Shared Sub SetRACIOFile64(<MarshalAs(UnmanagedType.LPStr)> ByVal Name As String)
        End Sub

        Private Shared Function Is64Bit() As Boolean
            If IntPtr.Size = 8 Then 'Check whether we are running on a 32 or 64bit system.
                Return True
            Else
                Return False
            End If
        End Function

        Private Shared Function ArrToPosVec(ByVal Arr As Double()) As PosVector
            'Create a new vector having the values in the supplied double array
            Dim V As New PosVector
            V.x = Arr(0)
            V.y = Arr(1)
            V.z = Arr(2)
            Return V
        End Function

        Private Shared Sub PosVecToArr(ByVal V As PosVector, ByRef Ar As Double())
            'Copy a vector structure to a returned double array
            Ar(0) = V.x
            Ar(1) = V.y
            Ar(2) = V.z
        End Sub
        Private Shared Function ArrToVelVec(ByVal Arr As Double()) As VelVector
            'Create a new vector having the values in the supplied double array
            Dim V As New VelVector
            V.x = Arr(0)
            V.y = Arr(1)
            V.z = Arr(2)
            Return V
        End Function

        Private Shared Sub VelVecToArr(ByVal V As VelVector, ByRef Ar As Double())
            'Copy a vector structure to a returned double array
            Ar(0) = V.x
            Ar(1) = V.y
            Ar(2) = V.z
        End Sub

        Private Shared Sub RACioArrayStructureToArr(ByVal C As RAOfCioArray, ByRef Ar As ArrayList)
            'Transfer all RACio values that have actually been set by the NOVAS DLL to the arraylist for return to the client
            If C.Value1.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value1)
            If C.Value2.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value2)
            If C.Value3.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value3)
            If C.Value4.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value4)
            If C.Value5.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value5)
            If C.Value6.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value6)
            If C.Value7.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value7)
            If C.Value8.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value8)
            If C.Value9.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value9)
            If C.Value10.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value10)
            If C.Value11.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value11)
            If C.Value12.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value12)
            If C.Value13.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value13)
            If C.Value14.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value14)
            If C.Value15.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value15)
            If C.Value16.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value16)
            If C.Value17.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value17)
            If C.Value18.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value18)
            If C.Value19.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value19)
            If C.Value20.RACio <> RACIO_DEFAULT_VALUE Then Ar.Add(C.Value20)

        End Sub

        Private Sub O3FromO3Internal(ByVal O3I As Object3Internal, ByRef O3 As Object3)
            O3.Name = O3I.Name
            O3.Number = CType(O3I.Number, Body)
            O3.Star = O3I.Star
            O3.Type = O3I.Type
        End Sub
        Private Function O3IFromObject3(ByVal O3 As Object3) As Object3Internal
            Dim O3I As New Object3Internal
            O3I.Name = O3.Name
            O3I.Number = CShort(O3.Number)
            O3I.Star = O3.Star
            O3I.Type = O3.Type
            Return O3I
        End Function

        Private Sub SetRACIOFile(ByVal FName As String)
            If Is64Bit() Then
                SetRACIOFile64(FName)
            Else
                SetRACIOFile32(FName)
            End If
        End Sub


#End Region

    End Class

End Namespace
