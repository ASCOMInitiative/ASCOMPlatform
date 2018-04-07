''' <summary>
''' Static class containing one member to return the value of Delta T for a given Julian date.
''' </summary>
''' <remarks>See the DeltaT member for further information</remarks>
Public Class DeltaT
    ''' <summary>
    ''' Return the value of DeltaT for the given Julian date
    ''' </summary>
    ''' <param name="Tjd">Julian date for which the delta T value is required</param>
    ''' <returns>Double value of DeltaT (seconds)</returns>
    ''' <remarks>Valid between the years 1650 and 2050. Based on the polynomial given at http://sunearth.gsfc.nasa.gov/eclipse/SEcat5/deltatpoly.html as retrtieved on 11-Jan-2009.
    ''' <para>The Astronomical Almanac table is corrected by adding the expression: -0.000091 (ndot + 26)(year-1955)^2 seconds 
    ''' to entries prior to 1955 (AA page K8), where ndot is the secular tidal term in the mean motion of the Moon.</para>
    ''' <para>Entries after 1955 are referred to atomic time standards and are not affected by errors in Lunar or planetary theory.</para>
    ''' <para>See also: Morrison, L. and Stephenson, F. R., "Historical Values of the Earth's Clock Error ΔT and the Calculation of Eclipses", J. Hist. Astron., Vol. 35 Part 3, August 2004, No. 120, pp 327-336 (2004).
    ''' Note, Stephenson and Morrison's table starts at the year 1630.The actual accuracy decreases rapidly prior to 1780.</para>
    '''</remarks>
    Public Shared Function DeltaT(ByVal Tjd As Double) As Double
        Return ASCOM.Astrometry.DeltaTCalc(Tjd)
    End Function

End Class
