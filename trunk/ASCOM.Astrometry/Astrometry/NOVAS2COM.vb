'NOVAS2COM component implementation
'This class is an instantiable version of the NOVAS2 shared component because COM cannot see shared classes
'It is recommended that it only be used from COM clients and that .NET clients use the shared NOVAS2 component

Imports System.Runtime.InteropServices
'Imports ASCOM.Astrometry.Interfaces
Imports ASCOM.Astrometry.NOVASCOM

Namespace NOVAS
    <Guid("C3F04186-CD53-40fb-8B2A-B52BE955956D"), _
    ClassInterface(ClassInterfaceType.None), _
    ComVisible(True)> _
    Public Class NOVAS2COM
        Implements INOVAS2

        Public Function Aberration(ByVal pos() As Double, ByVal vel() As Double, ByVal lighttime As Double, ByRef pos2() As Double) As Short Implements INOVAS2.Aberration
            Return NOVAS2.Aberration(pos, vel, lighttime, pos2)
        End Function

        Public Function AppPlanet(ByVal tjd As Double, ByRef ss_object As BodyDescription, ByRef earth As BodyDescription, ByRef ra As Double, ByRef dec As Double, ByRef dis As Double) As Short Implements INOVAS2.AppPlanet
            Return NOVAS2.AppPlanet(tjd, ss_object, earth, ra, dec, dis)
        End Function

        Public Function AppStar(ByVal tjd As Double, ByRef earth As BodyDescription, ByRef star As CatEntry, ByRef ra As Double, ByRef dec As Double) As Short Implements INOVAS2.AppStar
            Return NOVAS2.AppStar(tjd, earth, star, ra, dec)
        End Function

        Public Function AstroPlanet(ByVal tjd As Double, ByRef ss_object As BodyDescription, ByRef earth As BodyDescription, ByRef ra As Double, ByRef dec As Double, ByRef dis As Double) As Short Implements INOVAS2.AstroPlanet
            Return NOVAS2.AstroPlanet(tjd, ss_object, earth, ra, dec, dis)
        End Function

        Public Function AstroStar(ByVal tjd As Double, ByRef earth As BodyDescription, ByRef star As CatEntry, ByRef ra As Double, ByRef dec As Double) As Short Implements INOVAS2.AstroStar
            NOVAS2.AstroStar(tjd, earth, star, ra, dec)
        End Function

        Public Sub BaryToGeo(ByVal pos() As Double, ByVal earthvector() As Double, ByRef pos2() As Double, ByRef lighttime As Double) Implements INOVAS2.BaryToGeo
            NOVAS2.BaryToGeo(pos, earthvector, pos2, lighttime)
        End Sub

        Public Sub CalDate(ByVal tjd As Double, ByRef year As Short, ByRef month As Short, ByRef day As Short, ByRef hour As Double) Implements INOVAS2.CalDate
            NOVAS2.CalDate(tjd, year, month, day, hour)
        End Sub

        Public Sub CelPole(ByVal del_dpsi As Double, ByVal del_deps As Double) Implements INOVAS2.CelPole
            NOVAS2.CelPole(del_dpsi, del_deps)
        End Sub

        Public Sub EarthTilt(ByVal tjd As Double, ByRef mobl As Double, ByRef tobl As Double, ByRef eq As Double, ByRef dpsi As Double, ByRef deps As Double) Implements INOVAS2.EarthTilt
            NOVAS2.EarthTilt(tjd, mobl, tobl, eq, dpsi, deps)
        End Sub

        Public Function Ephemeris(ByVal tjd As Double, ByRef cel_obj As BodyDescription, ByVal origin As Origin, ByRef pos() As Double, ByRef vel() As Double) As Short Implements INOVAS2.Ephemeris
            Return NOVAS2.Ephemeris(tjd, cel_obj, origin, pos, vel)
        End Function

        Public Sub Equ2Hor(ByVal tjd As Double, ByVal deltat As Double, ByVal x As Double, ByVal y As Double, ByRef location As SiteInfo, ByVal ra As Double, ByVal dec As Double, ByVal ref_option As RefractionOption, ByRef zd As Double, ByRef az As Double, ByRef rar As Double, ByRef decr As Double) Implements INOVAS2.Equ2Hor
            NOVAS2.Equ2Hor(tjd, deltat, x, y, location, ra, dec, ref_option, zd, az, rar, decr)
        End Sub

        Public Sub FundArgs(ByVal t As Double, ByRef a() As Double) Implements INOVAS2.FundArgs
            NOVAS2.FundArgs(t, a)
        End Sub

        Public Function GetEarth(ByVal tjd As Double, ByRef earth As BodyDescription, ByRef tdb As Double, ByRef bary_earthp() As Double, ByRef bary_earthv() As Double, ByRef helio_earthp() As Double, ByRef helio_earthv() As Double) As Short Implements INOVAS2.GetEarth
            Return NOVAS2.GetEarth(tjd, earth, tdb, bary_earthp, bary_earthv, helio_earthp, helio_earthv)
        End Function

        Public Function JulianDate(ByVal year As Short, ByVal month As Short, ByVal day As Short, ByVal hour As Double) As Double Implements INOVAS2.JulianDate
            Return NOVAS2.JulianDate(year, month, day, hour)
        End Function

        Public Function LocalPlanet(ByVal tjd As Double, ByRef ss_object As BodyDescription, ByRef earth As BodyDescription, ByVal deltat As Double, ByRef location As SiteInfo, ByRef ra As Double, ByRef dec As Double, ByRef dis As Double) As Short Implements INOVAS2.LocalPlanet
            Return NOVAS2.LocalPlanet(tjd, ss_object, earth, deltat, location, ra, dec, dis)
        End Function

        Public Function LocalStar(ByVal tjd As Double, ByRef earth As BodyDescription, ByVal deltat As Double, ByRef star As CatEntry, ByRef location As SiteInfo, ByRef ra As Double, ByRef dec As Double) As Short Implements INOVAS2.LocalStar
            Return NOVAS2.LocalStar(tjd, earth, deltat, star, location, ra, dec)
        End Function

        Public Sub MakeCatEntry(ByVal catalog As String, ByVal star_name As String, ByVal star_num As Integer, ByVal ra As Double, ByVal dec As Double, ByVal pm_ra As Double, ByVal pm_dec As Double, ByVal parallax As Double, ByVal rad_vel As Double, ByRef star As CatEntry) Implements INOVAS2.MakeCatEntry
            NOVAS2.MakeCatEntry(catalog, star_name, star_num, ra, dec, pm_ra, pm_dec, parallax, rad_vel, star)
        End Sub

        Public Function MeanStar(ByVal tjd As Double, ByRef earth As BodyDescription, ByVal ra As Double, ByVal dec As Double, ByRef mra As Double, ByRef mdec As Double) As Short Implements INOVAS2.MeanStar
            Return NOVAS2.MeanStar(tjd, earth, ra, dec, mra, mdec)
        End Function

        Public Function Nutate(ByVal tjd As Double, ByVal fn As NutationDirection, ByVal pos() As Double, ByRef pos2() As Double) As Short Implements INOVAS2.Nutate
            Return NOVAS2.Nutate(tjd, fn, pos, pos2)
        End Function

        Public Function NutationAngles(ByVal tdbtime As Double, ByRef longnutation As Double, ByRef obliqnutation As Double) As Short Implements INOVAS2.NutationAngles
            Return NOVAS2.NutationAngles(tdbtime, longnutation, obliqnutation)
        End Function

        Public Sub Pnsw(ByVal tjd As Double, ByVal gast As Double, ByVal x As Double, ByVal y As Double, ByVal vece() As Double, ByRef vecs() As Double) Implements INOVAS2.Pnsw
            NOVAS2.Pnsw(tjd, gast, x, y, vece, vecs)
        End Sub

        Public Sub Precession(ByVal tjd1 As Double, ByVal pos() As Double, ByVal tjd2 As Double, ByRef pos2() As Double) Implements INOVAS2.Precession
            NOVAS2.Precession(tjd1, pos, tjd2, pos2)
        End Sub

        Public Sub ProperMotion(ByVal tjd1 As Double, ByVal pos() As Double, ByVal vel() As Double, ByVal tjd2 As Double, ByRef pos2() As Double) Implements INOVAS2.ProperMotion
            NOVAS2.ProperMotion(tjd1, pos, vel, tjd2, pos2)
        End Sub

        Public Sub RADec2Vector(ByVal ra As Double, ByVal dec As Double, ByVal dist As Double, ByRef pos() As Double) Implements INOVAS2.RADec2Vector
            NOVAS2.RADec2Vector(ra, dec, dist, pos)
        End Sub

        Public Function Refract(ByRef location As SiteInfo, ByVal ref_option As Short, ByVal zd_obs As Double) As Double Implements INOVAS2.Refract
            Return NOVAS2.Refract(location, ref_option, zd_obs)
        End Function

        Public Function SetBody(ByVal type As BodyType, ByVal number As Body, ByVal name As String, ByRef cel_obj As BodyDescription) As Short Implements INOVAS2.SetBody
            Return NOVAS2.SetBody(type, number, name, cel_obj)
        End Function

        Public Sub SiderealTime(ByVal jd_high As Double, ByVal jd_low As Double, ByVal ee As Double, ByRef gst As Double) Implements INOVAS2.SiderealTime
            NOVAS2.SiderealTime(jd_high, jd_low, ee, gst)
        End Sub

        Public Function SolarSystem(ByVal tjd As Double, ByVal body As Body, ByVal origin As Origin, ByRef pos() As Double, ByRef vel() As Double) As Short Implements INOVAS2.SolarSystem
            Return NOVAS2.SolarSystem(tjd, body, origin, pos, vel)
        End Function

        Public Sub Spin(ByVal st As Double, ByVal pos1() As Double, ByRef pos2() As Double) Implements INOVAS2.Spin
            NOVAS2.Spin(st, pos1, pos2)
        End Sub

        Public Sub StarVectors(ByVal star As CatEntry, ByRef pos() As Double, ByRef vel() As Double) Implements INOVAS2.StarVectors
            NOVAS2.StarVectors(star, pos, vel)
        End Sub

        Public Sub SunEph(ByVal jd As Double, ByRef ra As Double, ByRef dec As Double, ByRef dis As Double) Implements INOVAS2.SunEph
            NOVAS2.SunEph(jd, ra, dec, dis)
        End Sub

        Public Function SunField(ByVal pos() As Double, ByVal earthvector() As Double, ByRef pos2() As Double) As Short Implements INOVAS2.SunField
            Return NOVAS2.SunField(pos, earthvector, pos2)
        End Function

        Public Sub Tdb2Tdt(ByVal tdb As Double, ByRef tdtjd As Double, ByRef secdiff As Double) Implements INOVAS2.Tdb2Tdt
            NOVAS2.Tdb2Tdt(tdb, tdtjd, secdiff)
        End Sub

        Public Sub Terra(ByRef locale As SiteInfo, ByVal st As Double, ByRef pos() As Double, ByRef vel() As Double) Implements INOVAS2.Terra
            NOVAS2.Terra(locale, st, pos, vel)
        End Sub

        Public Function TopoPlanet(ByVal tjd As Double, ByRef ss_object As BodyDescription, ByRef earth As BodyDescription, ByVal deltat As Double, ByRef location As SiteInfo, ByRef ra As Double, ByRef dec As Double, ByRef dis As Double) As Short Implements INOVAS2.TopoPlanet
            Return NOVAS2.TopoPlanet(tjd, ss_object, earth, deltat, location, ra, dec, dis)
        End Function

        Public Function TopoStar(ByVal tjd As Double, ByRef earth As BodyDescription, ByVal deltat As Double, ByRef star As CatEntry, ByRef location As SiteInfo, ByRef ra As Double, ByRef dec As Double) As Short Implements INOVAS2.TopoStar
            Return NOVAS2.TopoStar(tjd, earth, deltat, star, location, ra, dec)
        End Function

        Public Sub TransformCat(ByVal [option] As TransformationOption, ByVal date_incat As Double, ByRef incat As CatEntry, ByVal date_newcat As Double, ByRef newcat_id() As Byte, ByRef newcat As CatEntry) Implements INOVAS2.TransformCat
            NOVAS2.TransformCat([option], date_incat, incat, date_newcat, newcat_id, newcat)
        End Sub

        Public Sub TransformHip(ByRef hipparcos As CatEntry, ByRef fk5 As CatEntry) Implements INOVAS2.TransformHip
            NOVAS2.TransformHip(hipparcos, fk5)
        End Sub

        Public Function Vector2RADec(ByVal pos() As Double, ByRef ra As Double, ByRef dec As Double) As Short Implements INOVAS2.Vector2RADec
            Return NOVAS2.Vector2RADec(pos, ra, dec)
        End Function

        Public Function VirtualPlanet(ByVal tjd As Double, ByRef ss_object As BodyDescription, ByRef earth As BodyDescription, ByRef ra As Double, ByRef dec As Double, ByRef dis As Double) As Short Implements INOVAS2.VirtualPlanet
            Return NOVAS2.VirtualPlanet(tjd, ss_object, earth, ra, dec, dis)
        End Function

        Public Function VirtualStar(ByVal tjd As Double, ByRef earth As BodyDescription, ByRef star As CatEntry, ByRef ra As Double, ByRef dec As Double) As Short Implements INOVAS2.VirtualStar
            Return NOVAS2.VirtualStar(tjd, earth, star, ra, dec)
        End Function

        Public Sub Wobble(ByVal x As Double, ByVal y As Double, ByVal pos1() As Double, ByRef pos2() As Double) Implements INOVAS2.Wobble
            NOVAS2.Wobble(x, y, pos1, pos2)
        End Sub
    End Class
End Namespace