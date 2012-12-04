Imports ASCOM.Astrometry
Imports ASCOM.Utilities

Public Class EphemerisExamples
    Private TL As ASCOM.Utilities.TraceLogger
    Private AstroUtil As ASCOM.Astrometry.AstroUtils.AstroUtils
    Private Util As ASCOM.Utilities.Util

    Sub Example()
        ' Create a year long almanac for a particular event at a given location.

        ' At latitudes greater than 60N or 60S it is posisble for there to be two rises or sets in a given 24 hour day,
        ' hence results are aggregated into two EventTime lines. If, for a given day, all months have only 0 or 1 event
        ' only the first event times line is included. If any day has two events then the second event times line
        ' is included with the same day number as the first event times line.

        ' For some event types it is possible that no event of that type occurs, in which case the event time is represented 
        ' as a blank space. At times of year when no events occur, because a body is "always risen" or "always set", rise and set times
        ' are shown as "****" and "----" respectively.

        ' The expected output for Moon rising / setting using the parameters: Year=2012, Latitude=75N, Longitude=75W, TimeZone=-5h is given at 
        ' the end of this example.

        Dim EventList As ArrayList ' Returned list of events
        Dim EventTimes1, EventTimes2 As String ' First and second lines of event times
        Dim NumberOfRises, NumberOfSets As Integer ' Number of rise and set events

        ' Set the year and geographical location for this run
        Const Year As Integer = 2012
        Const Latitude As Double = 75.0 ' Positive to the North
        Const Longitude As Double = -75.0 ' Positive to the East
        Const TimeZone As Double = -5.0 ' Positive to the East

        ' Set the event for which the Almanac is required: Sunrise/set, Moonrise/set, Civil, Nautical, Amateur Astronomical or Astronomical twighlight
        ' or planet rise/set
        Const TypeOfEvent As EventType = EventType.MoonRiseMoonSet

        ' Create ASCOM components
        TL = New TraceLogger("", TypeOfEvent.ToString & "_" & Year.ToString)
        TL.Enabled = True
        AstroUtil = New AstroUtils.AstroUtils
        Util = New Util

        'Write the almanac parameters
        TL.LogMessage("Almanac", "Latitude: " & Util.DegreesToDMS(Math.Abs(Latitude), ":", ":", "", 0) & IIf(Latitude >= 0.0, " N", " S") & _
                       ",   Longitude: " & Util.DegreesToDMS(Math.Abs(Longitude), ":", ":", "", 0) & IIf(Longitude >= 0.0, " E", " W") & _
                       ",   Time Zone: " & Math.Abs(TimeZone) & " hours " & IIf(TimeZone <= 0.0, "West", "East") & " of Greenwich" & _
                       ",   Year: " & Year & _
                       ",   Event: " & TypeOfEvent.ToString)
        TL.BlankLine()

        ' Write title lines
        TL.LogMessage("Almanac", "       Jan.       Feb.       Mar.       Apr.       May        June       July       Aug.       Sept.      Oct.       Nov.       Dec.  ")
        TL.LogMessage("Almanac", "Day Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set")

        ' Work through each day number in turn, processing all months having that day.
        ' This approach is taken in order to create output in a similar format to that which comes from the USNO ephemeris data web
        ' site at http://aa.usno.navy.mil/data/docs/RS_OneYear.php
        For Day As Integer = 1 To 31 ' Process 1 day at a time to match the USNO online Almanac
            EventTimes1 = "" ' Initialise event times
            EventTimes2 = ""
            For Mon As Integer = 1 To 12 ' Process each month in turn for this day number
                Try
                    EventList = AstroUtil.EventTimes(TypeOfEvent, Day, Mon, Year, Latitude, Longitude, TimeZone) ' Get the rise and set events list
                Catch ex As ASCOM.InvalidValueException ' Indicates that an invalid day has been specified e.g. 31st of February so ignore it
                    EventList = New ArrayList
                Catch ex As Exception ' Any other unexpected exception
                    TL.LogMessage("Loop", Day & " " & Mon)
                    TL.LogMessageCrLf("Exception", ex.ToString)
                    EventList = New ArrayList
                End Try

                If EventList.Count > 0 Then ' Check whether we have any results

                    NumberOfRises = CInt(EventList(1)) ' Retrieve the number of sunrises
                    NumberOfSets = CInt(EventList(2)) ' Retrieve the number of sunsets

                    If (NumberOfRises > 0) Or (NumberOfSets > 0) Then ' The body either rises or sets this day
                        Select Case NumberOfRises
                            Case 0 ' No rises
                                EventTimes1 += "    "
                                EventTimes2 += "    "
                            Case 1 ' 1 rise so build up the first message line
                                EventTimes1 += RoundHour(EventList(3))
                                EventTimes2 += "    "
                            Case 2 ' 2 rises so build up message lines 1 and 2
                                EventTimes1 += RoundHour(EventList(3))
                                EventTimes2 += RoundHour(EventList(4))
                            Case Else ' Should never happen!
                                EventTimes1 += "????"
                                EventTimes2 += "????"
                        End Select

                        EventTimes1 += " " ' Add spacer between rise and set value
                        EventTimes2 += " "

                        Select Case NumberOfSets
                            Case 0 ' No sets
                                EventTimes1 += "    "
                                EventTimes2 += "    "
                            Case 1 ' 1 set so build up the first message line
                                EventTimes1 += RoundHour(EventList(NumberOfRises + 3))
                                EventTimes2 += "    "
                            Case 2 ' 2 sets so build up message lines 1 and 2
                                EventTimes1 += RoundHour(EventList(NumberOfRises + 3))
                                EventTimes2 += RoundHour(EventList(NumberOfRises + 4))
                            Case Else ' Should never happen!
                                EventTimes1 += "????"
                                EventTimes2 += "????"
                        End Select
                    Else ' Body neither rises nor sets this day so report whether it is above or below the horizon
                        If CBool(EventList(0)) Then ' Body is above horizon
                            EventTimes1 += "**** ****"
                            EventTimes2 += "         "

                        Else ' Body is below the horizon
                            EventTimes1 += "---- ----"
                            EventTimes2 += "         "
                        End If
                    End If
                Else ' No data so print blanks
                    EventTimes1 += "         "
                    EventTimes2 += "         "
                End If

                EventTimes1 += "  " ' Add spacers between months
                EventTimes2 += "  "
            Next

            'Print the whole day line and the line with second events if it has any values
            TL.LogMessage("Almanac", Day.ToString("00") & "  " & EventTimes1)
            If Trim(EventTimes2) <> "" Then TL.LogMessage("Almanac", Day.ToString("00") & "  " & EventTimes2)
        Next

        ' Write the explanetory legend
        TL.BlankLine()
        TL.LogMessage("", "    **** Always risen or above the requested level                    ---- Always set or below the requested level")
        TL.LogMessage("", "         Spaces indicate no event                                          Multiple events in a day are shown as multiple day lines")

        ' Clean up after run
        TL.Enabled = False
        TL.Dispose()
        Util.Dispose()
        AstroUtil.Dispose()
    End Sub

    ''' <summary>
    ''' Rounds an hour value up or down to the nearest minute
    ''' </summary>
    ''' <param name="Moment">Time of day expressed in hours</param>
    ''' <returns>String in HH:mm format rounded to the neaest minute</returns>
    ''' <remarks>.NET rounding, when going from doubles to HH:mm format, always rounds down to the nearest minute e.g. 11:32:58 will be
    ''' returned as 11:58 rather than 11:59. This function adds 30 seconds to the supplied date and rounds that value in order to 
    ''' achieve rounding where XX:YY:00 to XX:YY:29 becomes XX:YY and XX:YY:30 to XX:YY:59 becomes XX:YY+1.
    ''' <para>Rounding is omitted for minute 23:59 in order to prevent the value flipping over into the next day</para></remarks>
    Private Function RoundHour(Moment As Double) As String
        Dim Retval As String = "Unknown"
        Try
            If Moment >= 23.9833333 Then ' If we are at 23:59:XX then don't round the number to prevent it going over into the next day
                Retval = New Date().AddHours(Moment).ToString("HHmm") 'Util.HoursToHM(Moment, "")
            Else ' Round number to nearest minute
                Retval = New Date().AddHours(Moment).AddSeconds(30).ToString("HHmm")
            End If
        Catch ex As Exception
            Retval = "XXXX"
        End Try
        Return Retval
    End Function

End Class

' Example almanac program output for Moon rise/set in 2012 at 75N, 75W with a timezone of -5h

'07:51:02.771 Almanac                   Latitude: 75:00:00 N,   Longitude: 75:00:00 W,   Time Zone: 5 hours West of Greenwich,   Year: 2012,   Event: MoonRiseMoonSet
'07:51:02.771                           
'07:51:02.771 Almanac                          Jan.       Feb.       Mar.       Apr.       May        June       July       Aug.       Sept.      Oct.       Nov.       Dec.  
'07:51:02.771 Almanac                   Day Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set
'07:51:02.832 Almanac                   01  0924 0205  **** ****  **** ****  **** ****  1326 0252  ---- ----  ---- ----  2210       1815 0639  1458 1030  **** ****  **** ****  
'07:51:02.890 Almanac                   02  0829 0430  **** ****  **** ****  1128 0543  1547 0222  ---- ----  ---- ----  2116 0222  1748 0843  **** ****  **** ****  **** ****  
'07:51:02.948 Almanac                   03  **** ****  **** ****  **** ****  1358 0459  1820 0151  ---- ----  ---- ----  2043 0503  1713 1052  **** ****  **** ****  1811 1350  
'07:51:03.006 Almanac                   04  **** ****  **** ****  **** ****  1621 0427  2201 0111  ---- ----  ---- ----  2015 0717  1606 1335  **** ****  **** ****  2032 1305  
'07:51:03.006 Almanac                   04                                                   2330                                                                               
'07:51:03.063 Almanac                   05  **** ****  **** ****  1135 0817  1851 0357  ---- ----  ---- ----  2326       1949 0923  **** ****  **** ****  **** ****  2238 1235  
'07:51:03.121 Almanac                   06  **** ****  **** ****  1431 0710  2145 0322  ---- ----  ---- ----  2245 0535  1919 1128  **** ****  **** ****  2052 1517       1210  
'07:51:03.179 Almanac                   07  **** ****  1456 0927  1700 0632       0226  ---- ----  ---- ----  2215 0757  1834 1346  **** ****  **** ****  2309 1440  0044 1145  
'07:51:03.237 Almanac                   08  **** ****  1733 0839  1926 0600  ---- ----  ---- ----  0145 0555  2149 1005  **** ****  **** ****  **** ****       1412  0257 1115  
'07:51:03.294 Almanac                   09  **** ****  1956 0804  2203 0528  ---- ----  ---- ----  0050 0833  2121 1207  **** ****  **** ****  2101 1747  0118 1347  0533 1029  
'07:51:03.352 Almanac                   10  1449 1207  2222 0733       0445  ---- ----  ---- ----  0017 1043  2046 1415  **** ****  **** ****  2341 1649  0331 1319  ---- ----  
'07:51:03.352 Almanac                   10                                                         2350                                                                         
'07:51:03.409 Almanac                   11  1752 1050       0657  0158 0248  ---- ----  ---- ----  2324 1245  1942 1652  **** ****  **** ****       1616  0558 1243  ---- ----  
'07:51:03.467 Almanac                   12  2017 1010  0108 0603  ---- ----  ---- ----  0304 0855  2254 1446  **** ****  **** ****       1907  0157 1549  0918 1120  ---- ----  
'07:51:03.524 Almanac                   13  2236 0938  ---- ----  ---- ----  ---- ----  0223 1114  2212 1658  **** ****  **** ****  0005 1822  0411 1522  ---- ----  ---- ----  
'07:51:03.581 Almanac                   14       0906  ---- ----  ---- ----  0546 0843  0154 1316  **** ****  **** ****  **** ****  0233 1752  0632 1451  ---- ----  ---- ----  
'07:51:03.639 Almanac                   15  0103 0826  ---- ----  ---- ----  0438 1130  0128 1515  **** ****  **** ****       2151  0450 1725  0918 1402  ---- ----  ---- ----  
'07:51:03.696 Almanac                   16  0407 0713  ---- ----  ---- ----  0402 1340  0101 1717  **** ****  **** ****  0001 2033  0707 1656  ---- ----  ---- ----  1251 1801  
'07:51:03.754 Almanac                   17  ---- ----  ---- ----  ---- ----  0335 1540  0029 1939  **** ****  **** ****  0302 1957  0936 1619  ---- ----  ---- ----  1207 2032  
'07:51:03.754 Almanac                   17                                              2339                                                                                    
'07:51:03.812 Almanac                   18  ---- ----  ---- ----  0710 1125  0309 1739  **** ****  **** ****  **** ****  0522 1928  1256 1454  ---- ----  1524 1803  1139 2242  
'07:51:03.871 Almanac                   19  ---- ----  ---- ----  0616 1356  0240 1945  **** ****  **** ****       2256  0737 1900  ---- ----  ---- ----  1415 2059  1114       
'07:51:03.928 Almanac                   20  ---- ----  0838 1405  0543 1604  0205 2227  **** ****  **** ****  0312 2205  0955 1829  ---- ----  ---- ----  1341 2314  1049 0045  
'07:51:03.986 Almanac                   21  ---- ----  0753 1628  0515 1804  0055       **** ****  **** ****  0545 2132  1230 1743  ---- ----  ---- ----  1315       1020 0248  
'07:51:04.043 Almanac                   22  ---- ----  0721 1835  0448 2006  **** ****  **** ****  **** ****  0759 2104  ---- ----  ---- ----  1639 2101  1251 0116  0938 0505  
'07:51:04.101 Almanac                   23  1124 1355  0653 2037  0418 2219  **** ****  **** ****  0555 0020  1011 2035  ---- ----  ---- ----  1551 2332  1226 0316  **** ****  
'07:51:04.101 Almanac                   23                                                              2339                                                                    
'07:51:04.159 Almanac                   24  1004 1657  0624 2243  0336       **** ****  **** ****  0816 2308  1231 2001  ---- ----  ---- ----  1521       1154 0520  **** ****  
'07:51:04.216 Almanac                   25  0925 1913  0550       **** ****  **** ****  **** ****  1028 2240  1522 1859  ---- ----  1812 2335  1456 0139  1058 0750  **** ****  
'07:51:04.274 Almanac                   26  0855 2117  0453 0112  **** ****  **** ****  0539 0302  1242 2210  ---- ----  ---- ----  1731       1432 0340  **** ****  **** ****  
'07:51:04.332 Almanac                   27  0826 2320  **** ****  **** ****  **** ****  0829 0152  1509 2130  ---- ----  ---- ----  1702 0158  1405 0541  **** ****  **** ****  
'07:51:04.390 Almanac                   28  0755       **** ****  **** ****  **** ****  1046 0115  ---- ----  ---- ----  2112 2300  1637 0404  1329 0750  **** ****  **** ****  
'07:51:04.448 Almanac                   29  0714 0132  **** ****  **** ****  0832 0418  1300 0045  ---- ----  ---- ----  1946       1611 0606  1151 1103  **** ****  **** ****  
'07:51:04.507 Almanac                   30  **** ****             **** ****  1107 0326  1521 0017  ---- ----  ---- ----  1908 0213  1542 0811  **** ****  **** ****  1525 1231  
'07:51:04.507 Almanac                   30                                                   2344                                                                               
'07:51:04.564 Almanac                   31  **** ****             **** ****             1806 2251             ---- ----  1840 0432             **** ****             1801 1131  
'07:51:04.564                           
'07:51:04.564                               **** Always risen or above the requested level                    ---- Always set or below the requested level
'07:51:04.564                                    Spaces indicate no event                                          Multiple events in a day are shown as multiple day lines


' Corresponding output from USNO web site http://aa.usno.navy.mil/data/docs/RS_OneYear.php using Form B - Locations Worldwide

'             o  '    o  '                                   (NO NAME GIVEN)                            Astronomical Applications Dept.
'Location: W075 00, N75 00                         Rise and Set for the Moon for 2012                   U. S. Naval Observatory        
'                                                                                                       Washington, DC  20392-5420     
'                                                      Zone:  5h West of Greenwich                                                     
'
'
'       Jan.       Feb.       Mar.       Apr.       May        June       July       Aug.       Sept.      Oct.       Nov.       Dec.  
'Day Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set  Rise  Set
'     h m  h m   h m  h m   h m  h m   h m  h m   h m  h m   h m  h m   h m  h m   h m  h m   h m  h m   h m  h m   h m  h m   h m  h m
'01  0924 0205  **** ****  **** ****  **** ****  1326 0251  ---- ----  ---- ----  2210       1815 0639  1458 1030  **** ****  **** ****
'02  0829 0430  **** ****  **** ****  1127 0543  1547 0222  ---- ----  ---- ----  2116 0222  1748 0843  **** ****  **** ****  **** ****
'03  **** ****  **** ****  **** ****  1358 0459  1820 0151  ---- ----  ---- ----  2043 0503  1713 1052  **** ****  **** ****  1811 1349
'04  **** ****  **** ****  **** ****  1622 0427  2202 0111  ---- ----  ---- ----  2016 0717  1606 1334  **** ****  **** ****  2032 1305
'04                                                   2330                                                                             
'05  **** ****  **** ****  1135 0817  1851 0356  ---- ----  ---- ----  2326       1949 0922  **** ****  **** ****  **** ****  2239 1235
'06  **** ****  **** ****  1431 0710  2145 0321  ---- ----  ---- ----  2245 0534  1919 1128  **** ****  **** ****  2053 1517       1210
'07  **** ****  1456 0927  1700 0632       0226  ---- ----  ---- ----  2216 0757  1835 1346  **** ****  **** ****  2309 1440  0044 1145
'08  **** ****  1732 0839  1926 0600  ---- ----  ---- ----  0145 0555  2149 1005  **** ****  **** ****  **** ****       1412  0258 1115
'09  **** ****  1956 0804  2203 0527  ---- ----  ---- ----  0050 0833  2121 1207  **** ****  **** ****  2101 1747  0118 1346  0533 1029
'10  1449 1207  2222 0732       0445  ---- ----  ---- ----  0017 1043  2046 1415  **** ****  **** ****  2340 1649  0331 1319  ---- ----
'10                                                         2350                                                                       
'11  1752 1050       0657  0200 0247  ---- ----  ---- ----  2324 1245  1942 1652  **** ****  **** ****       1616  0558 1243  ---- ----
'12  2017 1010  0108 0603  ---- ----  ---- ----  0304 0855  2254 1446  **** ****  **** ****       1906  0157 1549  0919 1120  ---- ----
'13  2237 0938  ---- ----  ---- ----  ---- ----  0223 1113  2213 1658  **** ****  **** ****  0006 1822  0411 1522  ---- ----  ---- ----
'14       0906  ---- ----  ---- ----  0546 0843  0154 1316  **** ****  **** ****  **** ****  0234 1752  0633 1451  ---- ----  ---- ----
'15  0103 0827  ---- ----  ---- ----  0438 1130  0128 1514  **** ****  **** ****       2151  0450 1725  0918 1402  ---- ----  ---- ----
'16  0408 0712  ---- ----  ---- ----  0402 1340  0101 1717  **** ****  **** ****  0001 2034  0708 1656  ---- ----  ---- ----  1251 1801
'17  ---- ----  ---- ----  ---- ----  0335 1540  0029 1938  **** ****  **** ****  0302 1957  0936 1619  ---- ----  ---- ----  1208 2032
'17                                              2339                                                                                  
'18  ---- ----  ---- ----  0710 1124  0309 1739  **** ****  **** ****  **** ****  0522 1928  1257 1454  ---- ----  1524 1803  1139 2242
'19  ---- ----  ---- ----  0616 1356  0241 1945  **** ****  **** ****       2256  0737 1900  ---- ----  ---- ----  1415 2059  1114     
'20  ---- ----  0839 1405  0543 1603  0205 2226  **** ****  **** ****  0312 2205  0955 1829  ---- ----  ---- ----  1341 2313  1049 0045
'21  ---- ----  0753 1629  0515 1804  0055       **** ****  **** ****  0545 2132  1230 1743  ---- ----  ---- ----  1315       1020 0248
'22  ---- ----  0721 1835  0448 2006  **** ****  **** ****  **** ****  0759 2104  ---- ----  ---- ----  1639 2101  1251 0116  0938 0504
'23  1124 1355  0653 2037  0418 2219  **** ****  **** ****  0556 0020  1012 2035  ---- ----  ---- ----  1551 2331  1226 0315  **** ****
'23                                                              2339                                                                  
'24  1004 1657  0624 2243  0336       **** ****  **** ****  0817 2308  1231 2000  ---- ----  ---- ----  1521       1155 0520  **** ****
'25  0925 1912  0550       **** ****  **** ****  **** ****  1028 2240  1522 1859  ---- ----  1812 2335  1456 0139  1059 0749  **** ****
'26  0855 2117  0453 0111  **** ****  **** ****  0539 0302  1242 2210  ---- ----  ---- ----  1731       1432 0340  **** ****  **** ****
'27  0826 2319  **** ****  **** ****  **** ****  0829 0151  1509 2130  ---- ----  ---- ----  1702 0157  1405 0540  **** ****  **** ****
'28  0755       **** ****  **** ****  **** ****  1047 0115  ---- ----  ---- ----  2112 2259  1637 0404  1329 0750  **** ****  **** ****
'29  0714 0132  **** ****  **** ****  0832 0418  1300 0045  ---- ----  ---- ----  1946       1612 0606  1153 1101  **** ****  **** ****
'30  **** ****             **** ****  1107 0325  1521 0017  ---- ----  ---- ----  1909 0213  1542 0811  **** ****  **** ****  1525 1231
'30                                                   2343                                                                             
'31  **** ****             **** ****             1806 2251             ---- ----  1841 0432             **** ****             1801 1131
'
'(**** object continuously above horizon)                                                      (---- object continuously below horizon)
'
'NOTE: BLANK SPACES IN THE TABLE INDICATE THAT A RISING OR A SETTING DID NOT OCCUR DURING THAT 24 HR INTERVAL.