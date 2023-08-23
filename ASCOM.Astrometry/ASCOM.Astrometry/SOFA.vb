﻿Imports System.Runtime.InteropServices
Imports System.Environment
Imports ASCOM.Utilities
Imports ASCOM.Utilities.Exceptions
Imports System.Text
Imports System.IO
Imports System.Security.Cryptography

Namespace SOFA
    ''' <summary>
    ''' This class presents a subset of the SOFA (Standards of Fundamental Astronomy) astrometry routines in a form that is easily accessible from both 32bit and 64bit .NET and 
    ''' COM applications.
    ''' </summary>
    ''' <remarks>
    ''' SOFA operates under the auspices of the International Astronomical Union (IAU) to provide algorithms and software for use in astronomical computing. The entire SOFA 
    ''' collection comprises many authoritative routines across a number of areas including:
    ''' <list type="bullet">
    ''' <item><description>Astrometry</description></item>
    ''' <item><description>Calendars</description></item>
    ''' <item><description>Time Scales</description></item>
    ''' <item><description>Earth rotation and sidereal time</description></item>
    ''' <item><description>Ephemerides (medium precision)</description></item>
    ''' <item><description>Geocentric/geodetic transformations</description></item>
    ''' <item><description>Precession, nutation, polar motion</description></item>
    ''' <item><description>Star space motion</description></item>
    ''' <item><description>Star catalogue conversion</description></item>
    ''' </list>
    ''' <para><b>The class's functionality is provided by underlying DLLs compiled from unmodified original C source code distributed by SOFA but the class does not constitute software provided by and/or endorsed by SOFA.
    ''' No change whatsoever has been made to the algorithms implemented by SOFA that realise IAU standards.</b> 
    ''' </para>
    ''' <para>SOFA provides a validation routine to confirm that the compiled library provides expected results. 32 and 64bit versions of this routine (SofaTestXX.exe and SofaTestXX-64.exe, where XX is the issue number) 
    ''' are included in this distribution and can be found in the Common Files\ASCOM\Astrometry directory. To run them open a command prompt in the Astrometry directory and enter the commands SofaTest10 /verbose and SofaTest10-64 /verbose.
    ''' The susbset of these tests that is relevant to the routines presented in this component have also been incorporated in the ASCOM Diagnostics tool and expected operation of the SOFA routnines can be confirmed through this tool.</para>
    ''' <para>Further information on the full library of SOFA routines is available here: http://www.iausofa.org/ </para>
    ''' </remarks>
    <Guid("DF65E97B-ED0E-4F48-BBC9-4A8854C0EF6E"), ClassInterface(ClassInterfaceType.None), ComVisible(True)>
    Public Class SOFA
        Implements ISOFA, IDisposable

        Private Const SOFA32DLL As String = "SOFA.dll" 'Names of SOFA 32 and 64bit DLL files
        Private Const SOFA64DLL As String = "SOFA-64.dll"
        Private Const SOFA_DLL_LOCATION As String = "\ASCOM\Astrometry\" 'This is appended to the Common Files path so that the calling application can dind the SOFA DLLs

        ' Release and revision constants
        Private Const SOFA_RELEASE_NUMBER As Integer = 18
        Private Const SOFA_ISSUE_DATE As String = "2021-05-12"
        Private Const SOFA_REVISION_NUMBER As Integer = 0 ' Not presented in the interface, maintained here for reference
        Private Const SOFA_REVISION_DATE As String = "2021-05-12"

        Private Const MAXIMUM_NUMBER_OF_UPDATED_LEAP_SECOPND_VALUES As Integer = 100

        Private TL As TraceLogger
        Private Utl As Util
        Private Shared SofaDllHandle As IntPtr

        Private Parameters As EarthRotationParameters

        <StructLayout(LayoutKind.Sequential)>
        Friend Structure LeapSecondDataStruct
            Dim Year As Integer
            Dim Month As Integer
            Dim DelAt As Double
        End Structure

        Dim RevisedData(MAXIMUM_NUMBER_OF_UPDATED_LEAP_SECOPND_VALUES - 1) As LeapSecondDataStruct

#Region "New and IDisposable"

        ''' <summary>
        ''' Static initialiser to load the SOFA DLL so that it is available for SOFA static functions such as GetBuiltInLeapSeconds
        ''' </summary>
        Shared Sub New()

            Dim ReturnedPath As New System.Text.StringBuilder(260), SofaDllFile As String, rc As Boolean, LastError As Integer

#If DEBUG Then
            ' In the DEBUG environment load the DLL from the application directory where the latest verion will have been copied.
            ' This assumes that debugging is only undertaken using 32bit applications
            rc = False ' Just to Suppress a compiler warning
            SofaDllFile = String.Format("{0}\..\..\..\..\SOFA\Sofa Library\Win32\Debug\{1}", Environment.CurrentDirectory, SOFA32DLL)
#Else
            'Find the root location of the common files directory containing the ASCOM support files.
            'On a 32bit system this is \Program Files\Common Files
            'On a 64bit system this is \Program Files (x86)\Common Files
            If Is64Bit() Then ' 64bit application so find the 32bit folder location
                rc = SHGetSpecialFolderPath(IntPtr.Zero, ReturnedPath, CSIDL_PROGRAM_FILES_COMMONX86, False)
                SofaDllFile = ReturnedPath.ToString & SOFA_DLL_LOCATION & SOFA64DLL
            Else '32bit application so just go with the .NET returned value
                SofaDllFile = GetFolderPath(SpecialFolder.CommonProgramFiles) & SOFA_DLL_LOCATION & SOFA32DLL
            End If
#End If
            SofaDllHandle = LoadLibrary(SofaDllFile)
            LastError = Marshal.GetLastWin32Error

            If SofaDllHandle <> IntPtr.Zero Then ' Loaded successfully

            Else ' Did not load 
                Throw New HelperException(String.Format("Error code {0} returned from LoadLibrary when loading SOFA library: {1}  ", LastError.ToString("X8"), SofaDllFile))
            End If

        End Sub

        ''' <summary>
        ''' Creates a new instance of the SOFA component
        ''' </summary>
        ''' <exception cref="HelperException">Thrown if the SOFA support library DLL cannot be loaded</exception>
        ''' <remarks></remarks>
        Sub New()
            Dim rc As Boolean, SofaDllFile As String, ReturnedPath As New System.Text.StringBuilder(260), LastError, Count, NumberOfSOFALeapSecondValues As Integer, JulianDateUtc As DateTime

            TL = New TraceLogger("", "SOFA")
            TL.Enabled = GetBool(NOVAS_TRACE, NOVAS_TRACE_DEFAULT) 'Get enabled / disabled state from the user registry

            Utl = New Util

            Dim LeapSecondArray(100) As LeapSecondDataStruct, RecordCount As Integer, HasBeenUpdated As Boolean, UTCNow As DateTime

#If DEBUG Then
            ' In the DEBUG environment load the DLL from the application directory where the latest verion will have been copied.
            ' This assumes that debugging is only undertaken using 32bit applications
            rc = False ' Just to Suppress a compiler warning
            SofaDllFile = String.Format("{0}\..\..\..\..\SOFA\Sofa Library\Win32\Debug\{1}", Environment.CurrentDirectory, SOFA32DLL)
            TL.LogMessage("New", "DEBUG build")
#Else
            'Find the root location of the common files directory containing the ASCOM support files.
            'On a 32bit system this is \Program Files\Common Files
            'On a 64bit system this is \Program Files (x86)\Common Files
            If Is64Bit() Then ' 64bit application so find the 32bit folder location
                rc = SHGetSpecialFolderPath(IntPtr.Zero, ReturnedPath, CSIDL_PROGRAM_FILES_COMMONX86, False)
                SofaDllFile = ReturnedPath.ToString & SOFA_DLL_LOCATION & SOFA64DLL
            Else '32bit application so just go with the .NET returned value
                SofaDllFile = GetFolderPath(SpecialFolder.CommonProgramFiles) & SOFA_DLL_LOCATION & SOFA32DLL
            End If
            TL.LogMessage("New", "PRODUCTION build")
#End If
            If Not File.Exists(SofaDllFile) Then
                TL.LogMessage("New", $"SOFA Initialise - Unable to locate SOFA library DLL: {SofaDllFile}")
                Throw New HelperException($"SOFA Initialise - Unable to locate SOFA library DLL: {SofaDllFile}")
            Else
                TL.LogMessage("New", $"Found SOFA library DLL: {SofaDllFile}")
            End If

            TL.LogMessage("New", "Loading SOFA library DLL: " + SofaDllFile)

            SofaDllHandle = LoadLibrary(SofaDllFile)
            LastError = Marshal.GetLastWin32Error

            If SofaDllHandle <> IntPtr.Zero Then ' Loaded successfully
                TL.LogMessage("New", "Loaded SOFA library OK")

                ' Update the SOFA DLL leap second data with any updated values downloaded from the web
                Try

                    Dim InBuiltLeapSeconds(100) As LeapSecondDataStruct

                    TL.LogMessage("New", String.Format("HasUpdatedData: {0}", UsingUpdatedData().ToString()))

                    Count = 0 ' Counter for the number of leap second values found
                    Using Parameters = New EarthRotationParameters() ' Get the data from the EarthRotationParameters class

                        NumberOfSOFALeapSecondValues = NumberOfBuiltInLeapSecondValues()

                        Select Case Parameters.UpdateType
                            Case UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1
                                TL.LogMessage("New-BuiltIn", "No leap second values available so use SOFA's built-in defaults")

                            Case UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1, UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1
                                TL.LogMessage("New-Manual", "No leap second values available so use SOFA's built-in defaults. UpdateType: " & Parameters.UpdateType)

                                If Not UsingUpdatedData() Then ' First time round so add the manual value at the end of the list
                                    RecordCount = GetLeapSecondData(LeapSecondArray, HasBeenUpdated)
                                    UTCNow = DateTime.UtcNow ' Save the UTC time so that the year month and day values are consistent if SOFA is run across a day/month/year change point

                                    ' Add today's month to the end of the list with the configured number of leap seconds. This will force SOFA to use the configured value for today's calculations
                                    LeapSecondArray(RecordCount).Year = UTCNow.Year
                                    LeapSecondArray(RecordCount).Month = UTCNow.Month
                                    LeapSecondArray(RecordCount).DelAt = Parameters.ManualLeapSeconds

                                    LastError = UpdateLeapSecondData(LeapSecondArray) ' Send the revised data to the SOFA DLL
                                    If LastError = 0 Then ' First time initialisation of the DLL data
                                        TL.LogMessage("New-Manual", "SOFA leap second data initialised. Return code: " & LastError)
                                    ElseIf LastError = 1 Then
                                        TL.LogMessage("New-Manual", "SOFA leap second data already initialised - no action taken. Return code: " & LastError)
                                    ElseIf LastError = 2 Then
                                        TL.LogMessage("New-Manual", "SOFA leap second data rejected because 100 or more records were supplied. Using built-in leap second values. Return code: " & LastError)
                                    Else ' Initialisation has already been made for this instance oif the DLL - no need to do it again
                                        TL.LogMessage("New-Manual", "Unknown return code: " & LastError)
                                    End If

                                End If

                            Case UPDATE_ON_DEMAND_LEAP_SECONDS_AND_DELTAUT1, UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1
                                ' Act according to the number of updated leap second values that are available
                                Select Case Parameters.DownloadedLeapSeconds.Count
                                    Case 0 ' No values have been downloaded so run with built-in SOFA leap second values
                                        TL.LogMessage("New-AutoUpdate", "No leap second values in Parameters.HistoricLeapSeconds - Relying on SOFA's built-in defaults")

                                    Case 1 To NumberOfSOFALeapSecondValues ' The same or less number of values have been downloaded compared to the builtin number in the SOFA DLL so run with the built-in SOFA leap second values
                                        TL.LogMessage("AutoUpdate", String.Format("Parameters.HistoricLeapSeconds has {0} leap second values, which is less or equal to than the number built-in to SOFA ({1}) - Relying on SOFA's built-in defaults", Count, NumberOfSOFALeapSecondValues))

                                    Case NumberOfSOFALeapSecondValues + 1 To MAXIMUM_NUMBER_OF_UPDATED_LEAP_SECOPND_VALUES ' We have sufficient values to warrant overriding the SOFA vaslues
                                        For Each LeapSecond As KeyValuePair(Of Double, Double) In Parameters.DownloadedLeapSeconds ' Retrieve each individual leap second record
                                            JulianDateUtc = DateTime.FromOADate(LeapSecond.Key - OLE_AUTOMATION_JULIAN_DATE_OFFSET) ' Turn the Julian date into a DateTime value
                                            LeapSecondArray(Count).Year = JulianDateUtc.Year ' Save the year and month from the JulianDate DateTime value
                                            LeapSecondArray(Count).Month = JulianDateUtc.Month
                                            LeapSecondArray(Count).DelAt = LeapSecond.Value ' Save the number of leap seconds
                                            Count += 1 ' Increment the count ready for the next leap second value
                                        Next

                                        LastError = UpdateLeapSecondData(LeapSecondArray) ' Send the revised data to the SOFA DLL
                                        If LastError = 0 Then ' First time initialisation of the DLL data
                                            TL.LogMessage("AutoUpdate", "SOFA leap second data initialised. Return code: " & LastError)
                                        ElseIf LastError = 1 Then
                                            TL.LogMessage("AutoUpdate", "SOFA leap second data already initialised - no action taken. Return code: " & LastError)
                                        ElseIf LastError = 2 Then
                                            TL.LogMessage("AutoUpdate", "SOFA leap second data rejected because 100 or more records were supplied. Using built-in leap second values. Return code: " & LastError)
                                        Else ' Initialisation has already been made for this instance oif the DLL - no need to do it again
                                            TL.LogMessage("AutoUpdate", "Unknown return code: " & LastError)
                                        End If

                                    Case Else ' We have more values than can be stored in the allocated capacity within the SOFA DLL so don't try!
                                        TL.LogMessage("AutoUpdate", String.Format("Parameters.HistoricLeapSeconds has {0} leap second values, which is greater than the cpacity of the SOFA's leap second array({1}), data not sent - Relying on SOFA's built-in defaults", Count, NumberOfSOFALeapSecondValues))

                                End Select
                            Case Else
                                Try
                                    MsgBox("SOFA.New - Unknown Parameters.UpdateType value: " & Parameters.UpdateType)
                                Catch
                                End Try
                                Console.WriteLine($"SOFA.New - Unknown Parameters.UpdateType value: {Parameters.UpdateType}")
                                EventLogCode.LogEvent("SOFA.New", $"SOFA.New - Unknown Parameters.UpdateType value: {Parameters.UpdateType}", EventLogEntryType.Error, GlobalConstants.EventLogErrors.Sofa, "")
                        End Select

                    End Using

                    TL.LogMessage("New", String.Format("After HasUpdatedData: {0}", UsingUpdatedData.ToString()))

                Catch ex As Exception
                    TL.LogMessageCrLf("New", "Exception: " & ex.ToString())
                    Throw New HelperException($"SOFA Initialisation Exception - {ex.Message} (See inner exception for details)", ex)
                End Try

            Else ' Did not load 
                TL.LogMessage("New", $"Error code {LastError:X8} returned while loading SOFA library from {SofaDllFile}")
                Throw New HelperException($"SOFA Initialisation - Error code {LastError:X8} returned from LoadLibrary when loading SOFA library: {SofaDllFile}")
            End If

            TL.LogMessage("New", "SOFA Initialised OK")
        End Sub

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' Free other state (managed objects).

                    If Not (Utl Is Nothing) Then
                        Utl.Dispose()
                        Utl = Nothing
                    End If
                    If Not (TL Is Nothing) Then
                        TL.Enabled = False
                        TL.Dispose()
                        TL = Nothing
                    End If
                End If

                ' Free your own state (unmanaged objects) and set large fields to null.
                Try : FreeLibrary(SofaDllHandle) : Catch : End Try ' Free the SOFA library but don't return any error value

            End If
            Me.disposedValue = True
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        ''' <summary>
        ''' Cleans up the SOFA object
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

#Region "Public SOFA Interface - SOFA Members"
        ''' <summary>
        ''' Major number of the SOFA issue currently used by this component.
        ''' </summary>
        ''' <returns>Integer issue number</returns>
        ''' <remarks></remarks>
        Function SofaReleaseNumber() As Integer Implements ISOFA.SofaReleaseNumber
            Return SOFA_RELEASE_NUMBER ' The release number of the issue being used
        End Function

        ''' <summary>
        ''' Release date of the SOFA issue currently used by this component.
        ''' </summary>
        ''' <returns>String date in format yyyy-mm-dd</returns>
        ''' <remarks></remarks>
        Function SofaIssueDate() As String Implements ISOFA.SofaIssueDate
            Return SOFA_ISSUE_DATE ' Release date of the fundamental software issue currently being used 
        End Function

        ''' <summary>
        ''' Release date of the revision to the SOFA Issue that is actually being used by this component.
        ''' </summary>
        ''' <returns>String date in format yyyy-mm-dd</returns>
        ''' <remarks>When a new issue is employed that doesn't yet have a revision, this mehtod will return the SofaIssueDate</remarks>
        Function SofaRevisionDate() As String Implements ISOFA.SofaRevisionDate
            Return SOFA_REVISION_DATE ' Release date of the revision currently being used 
        End Function

        ''' <summary>
        ''' Convert degrees, arcminutes, arcseconds to radians.
        ''' </summary>
        ''' <param name="s">Sign:  '-' = negative, otherwise positive</param>
        ''' <param name="ideg">Degrees</param>
        ''' <param name="iamin">Arcminutes</param>
        ''' <param name="asec">Arcseconds</param>
        ''' <param name="rad">Angle in radian</param>
        ''' <returns>Status:  0 = OK, 1 = ideg outside range 0-359, 2 = iamin outside range 0-59, 3 = asec outside range 0-59.999...</returns>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description>The result is computed even if any of the range checks fail.</description></item>
        ''' <item><description>Negative ideg, iamin and/or asec produce a warning status, but the absolute value is used in the conversion.</description></item>
        ''' <item><description>If there are multiple errors, the status value reflects only the first, the smallest taking precedence.</description></item>
        ''' </list>
        ''' </remarks>
        Public Function Af2a(s As String,
                             ideg As Integer,
                             iamin As Integer,
                             asec As Double,
                             ByRef rad As Double) As Integer Implements ISOFA.Af2a

            Dim RetCode As Short

            If String.IsNullOrEmpty(s) Then s = " " ' Fix any invalid sign values

            If Is64Bit() Then
                Af2a64(s.ToCharArray()(0), Convert.ToInt16(ideg), Convert.ToInt16(iamin), asec, rad)
            Else
                Af2a32(s.ToCharArray()(0), Convert.ToInt16(ideg), Convert.ToInt16(iamin), asec, rad)
            End If

            Return Convert.ToInt32(RetCode)
        End Function

        ''' <summary>
        ''' Normalize angle into the range 0 &lt;= a &lt; 2pi.
        ''' </summary>
        ''' <param name="a">Angle (radians)</param>
        ''' <returns>Angle in range 0-2pi</returns>
        ''' <remarks></remarks>
        Public Function Anp(a As Double) As Double Implements ISOFA.Anp
            Dim RetVal As Double

            If Is64Bit() Then
                RetVal = Anp64(a)
            Else
                RetVal = Anp32(a)
            End If

            Return RetVal
        End Function

        ''' <summary>
        ''' Transform ICRS star data, epoch J2000.0, to CIRS using the SOFA Atci13 function.
        ''' </summary>
        ''' <param name="rc">ICRS right ascension at J2000.0 (radians, Note 1)</param>
        ''' <param name="dc">ICRS declination at J2000.0 (radians, Note 1)</param>
        ''' <param name="pr">RA proper motion (radians/year; Note 2)</param>
        ''' <param name="pd">Dec proper motion (radians/year)</param>
        ''' <param name="px">parallax (arcsec)</param>
        ''' <param name="rv">radial velocity (km/s, +ve if receding)</param>
        ''' <param name="date1">TDB as a 2-part Julian Date (Note 3)</param>
        ''' <param name="date2">TDB as a 2-part Julian Date (Note 3)</param>
        ''' <param name="ri">CIRS geocentric RA (radians)</param>
        ''' <param name="di">CIRS geocentric Dec (radians)</param>
        ''' <param name="eo">equation of the origins (ERA-GST, Note 5)</param>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description>Star data for an epoch other than J2000.0 (for example from the Hipparcos catalog, which has an epoch of J1991.25) will require a preliminary call to iauPmsafe before use.</description></item>
        ''' <item><description>The proper motion in RA is dRA/dt rather than cos(Dec)*dRA/dt.</description></item>
        ''' <item><description> The TDB date date1+date2 is a Julian Date, apportioned in any convenient way between the two arguments.  For example, JD(TDB)=2450123.8g could be expressed in any of these ways, among others:
        ''' <table style="width:340px;" cellspacing="0">
        ''' <col style="width:80px;"></col>
        ''' <col style="width:80px;"></col>
        ''' <col style="width:180px;"></col>
        ''' <tr>
        ''' <td colspan="1" align="center" rowspan="1" style="width: 80px; padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-top-color: #000000; border-top-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid;
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        ''' background-color: #00ffff;" width="110px">
        ''' <b>Date 1</b></td>
        ''' <td colspan="1" rowspan="1" align="center" style="width: 80px; padding-right: 10px; padding-left: 10px; 
        ''' border-top-color: #000000; border-top-style: Solid; 
        ''' border-right-style: Solid; border-right-color: #000000; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        ''' background-color: #00ffff;" width="110px">
        ''' <b>Date 2</b></td>
        ''' <td colspan="1" rowspan="1" align="center" style="width: 180px; padding-right: 10px; padding-left: 10px; 
        ''' border-top-color: #000000; border-top-style: Solid; 
        ''' border-right-style: Solid; border-right-color: #000000; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        ''' background-color: #00ffff;" width="220px">
        ''' <b>Method</b></td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        '''  2450123.8</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 0.0</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' JD method</td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 2451545.0</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' -1421.3</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' J2000 method</td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 2400000.5</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 50123.2</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' MJD method</td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 2450123.5</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 0.2</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' Date and time method</td>
        ''' </tr>
        ''' </table>
        ''' <para>The JD method is the most natural and convenient to use in cases where the loss of several decimal digits of resolution is acceptable.  The J2000 method is best matched to the way the argument is handled internally 
        ''' and will deliver the optimum resolution.  The MJD method and the date and time methods are both good compromises between resolution and convenience.  For most applications of this function the choice will not be at all critical.</para>
        ''' <para>TT can be used instead of TDB without any significant impact on accuracy.</para>
        ''' </description></item>
        ''' <item><description>The available accuracy is better than 1 milliarcsecond, limited mainly by the precession-nutation model that is used, namely IAU 2000A/2006.  Very close to solar system bodies, additional 
        ''' errors of up to several milliarcseconds can occur because of unmodeled light deflection;  however, the Sun's contribution is taken into account, to first order.  The accuracy limitations of 
        ''' the SOFA function iauEpv00 (used to compute Earth position and velocity) can contribute aberration errors of up to 5 microarcseconds.  Light deflection at the Sun's limb is uncertain at the 0.4 mas level.</description></item>
        ''' <item><description>Should the transformation to (equinox based) apparent place be required rather than (CIO based) intermediate place, subtract the equation of the origins from the returned right ascension:
        ''' RA = RI - EO. (The Anp function can then be applied, as required, to keep the result in the conventional 0-2pi range.)</description></item>
        ''' </list>
        ''' </remarks>
        Public Sub CelestialToIntermediate(rc As Double,
                          dc As Double,
                          pr As Double,
                          pd As Double,
                          px As Double,
                          rv As Double,
                          date1 As Double,
                          date2 As Double,
                          ByRef ri As Double,
                          ByRef di As Double,
                          ByRef eo As Double) Implements ISOFA.Atci13

            If Is64Bit() Then
                Atci1364(rc, dc, pr, pd, px, rv, date1, date2, ri, di, eo)
            Else
                Atci1332(rc, dc, pr, pd, px, rv, date1, date2, ri, di, eo)
            End If
        End Sub

        ''' <summary>
        ''' ICRS RA,Dec to observed place using the SOFA Atco13 function.
        ''' </summary>
        ''' <param name="rc">ICRS RA (radians, note 1)</param>
        ''' <param name="dc">ICRS Dec (radians, note 2)</param>
        ''' <param name="pr">RA Proper motion (radians/year)</param>
        ''' <param name="pd">Dec Proper motion (radians/year</param>
        ''' <param name="px">Parallax (arcsec)</param>
        ''' <param name="rv">Radial veolcity (Km/s, +ve if receding</param>
        ''' <param name="utc1">UTC Julian date (part 1, notes 3,4)</param>
        ''' <param name="utc2">UTC Julian date (part 2, notes 3,4)</param>
        ''' <param name="dut1">UT1 - UTC (seonds, note 5)</param>
        ''' <param name="elong">Site longitude (radians, note 6)</param>
        ''' <param name="phi">Site Latitude (radians, note 6)</param>
        ''' <param name="hm">Site Height (meters, notes 6,8)</param>
        ''' <param name="xp">Polar motion co-ordinate (radians, note 7)</param>
        ''' <param name="yp">Polar motion co-ordinate (radians,note 7)</param>
        ''' <param name="phpa">Site Presure (hPa = mB, note 8)</param>
        ''' <param name="tc">Site Temperature (C)</param>
        ''' <param name="rh">Site relative humidity (fraction in the range: 0.0 to 1.0)</param>
        ''' <param name="wl">Observation wavem=length (micrometres, note 9)</param>
        ''' <param name="aob">Observed Azimuth (radians)</param>
        ''' <param name="zob">Observed Zenith distance (radians)</param>
        ''' <param name="hob">Observed Hour Angle (radians)</param>
        ''' <param name="dob">Observed Declination (radians)</param>
        ''' <param name="rob">Observed RA (radians)</param>
        ''' <param name="eo">Equation of the origins (ERA-GST)</param>
        ''' <returns>+1 = dubious year (Note 4), 0 = OK, -1 = unacceptable date</returns>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description>Star data for an epoch other than J2000.0 (for example from the Hipparcos catalog, which has an epoch of J1991.25) will require a preliminary call to iauPmsafe before use.</description></item>
        ''' <item><description>The proper motion in RA is dRA/dt rather than cos(Dec)*dRA/dt.</description></item>
        ''' <item><description>utc1+utc2 is quasi Julian Date (see Note 2), apportioned in any convenient way between the two arguments, for example where utc1 is the Julian Day Number and utc2 is the fraction of a day.
        ''' <para>However, JD cannot unambiguously represent UTC during a leap second unless special measures are taken.  The convention in the present function is that the JD day represents UTC days whether the length is 86399, 86400 or 86401 SI seconds.</para>
        ''' <para>Applications should use the function iauDtf2d to convert from calendar date and time of day into 2-part quasi Julian Date, as it implements the leap-second-ambiguity convention just described.</para></description></item>
        ''' <item><description>The warning status "dubious year" flags UTCs that predate the introduction of the time scale or that are too far in the future to be trusted.  See iauDat for further details.</description></item>
        ''' <item><description>UT1-UTC is tabulated in IERS bulletins.  It increases by exactly one second at the end of each positive UTC leap second, introduced in order to keep UT1-UTC within +/- 0.9s.  n.b. This practice is under review, and in the future UT1-UTC may grow essentially without limit.</description></item>
        ''' <item><description>The geographical coordinates are with respect to the WGS84 reference ellipsoid.  TAKE CARE WITH THE LONGITUDE SIGN:  the longitude required by the present function is east-positive (i.e. right-handed), in accordance with geographical convention.</description></item>
        ''' <item><description>The polar motion xp,yp can be obtained from IERS bulletins.  The values are the coordinates (in radians) of the Celestial Intermediate Pole with respect to the International Terrestrial Reference System (see IERS Conventions 2003), measured along the meridians 0 and 90 deg west respectively.  For many applications, xp and yp can be set to zero.</description></item>
        ''' <item><description>If hm, the height above the ellipsoid of the observing station in meters, is not known but phpa, the pressure in hPa (=mB), is available, an adequate estimate of hm can be obtained from the expression:
        ''' <p style="margin-left:25px;font-family:Lucida Conosle,Monospace"><b>hm = -29.3 * tsl * log ( phpa / 1013.25 );</b></p>
        ''' <para>where tsl is the approximate sea-level air temperature in K (See Astrophysical Quantities, C.W.Allen, 3rd edition, section 52).  Similarly, if the pressure phpa is not known, it can be estimated from the height of the observing station, hm, as follows:</para>
        ''' <p style="margin-left:25px;font-family:Lucida Conosle,Monospace"><b>phpa = 1013.25 * exp ( -hm / ( 29.3 * tsl ) );</b></p>
        ''' <para>Note, however, that the refraction is nearly proportional to the pressure and that an accurate phpa value is important for precise work.</para></description></item>
        ''' <item><description>The argument wl specifies the observing wavelength in micrometers.  The transition from optical to radio is assumed to occur at 100 micrometers (about 3000 GHz).</description></item>
        ''' <item><description>The accuracy of the result is limited by the corrections for refraction, which use a simple A*tan(z) + B*tan^3(z) model. Providing the meteorological parameters are known accurately and there are no gross local effects, the predicted observed coordinates should be within 0.05 arcsec (optical) or 1 arcsec (radio) for a zenith distance of less than 70 degrees, better than 30 arcsec (optical or radio) at 85 degrees and better than 20 arcmin (optical) or 30 arcmin (radio) at the horizon.
        ''' <para>Without refraction, the complementary functions iauAtco13 and iauAtoc13 are self-consistent to better than 1 microarcsecond all over the celestial sphere.  With refraction included, consistency falls off at high zenith distances, but is still better than 0.05 arcsec at 85 degrees.</para></description></item>
        ''' <item><description>"Observed" Az,ZD means the position that would be seen by a perfect geodetically aligned theodolite.  (Zenith distance is used rather than altitude in order to reflect the fact that no allowance is made for depression of the horizon.)  This is related to the observed HA,Dec via the standard rotation, using the geodetic latitude (corrected for polar motion), while the observed HA and RA are related simply through the Earth rotation angle and the site longitude.  "Observed" RA,Dec or HA,Dec thus means the position that would be seen by a perfect equatorial with its polar axis aligned to the Earth's axis of rotation.</description></item>
        ''' <item><description>It is advisable to take great care with units, as even unlikely values of the input parameters are accepted and processed in accordance with the models used.</description></item>
        ''' </list>
        ''' </remarks>
        Public Function CelestialToObserved(rc As Double,
                               dc As Double,
                               pr As Double,
                               pd As Double,
                               px As Double,
                               rv As Double,
                               utc1 As Double,
                               utc2 As Double,
                               dut1 As Double,
                               elong As Double,
                               phi As Double,
                               hm As Double,
                               xp As Double,
                               yp As Double,
                               phpa As Double,
                               tc As Double,
                               rh As Double,
                               wl As Double,
                               ByRef aob As Double,
                               ByRef zob As Double,
                               ByRef hob As Double,
                               ByRef dob As Double,
                               ByRef rob As Double,
                               ByRef eo As Double) As Integer Implements ISOFA.Atco13

            Dim RetCode As Short

            If Is64Bit() Then
                RetCode = Atco1364(rc, dc, pr, pd, px, rv, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, aob, zob, hob, dob, rob, eo)
            Else
                RetCode = Atco1332(rc, dc, pr, pd, px, rv, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, aob, zob, hob, dob, rob, eo)
            End If

            Return Convert.ToInt32(RetCode)
        End Function

        ''' <summary>
        ''' Encode date and time fields into 2-part Julian Date (or in the case of UTC a quasi-JD form that includes special provision for leap seconds).
        ''' </summary>
        ''' <param name="scale">Time scale ID (Note 1)</param>
        ''' <param name="iy">Year in Gregorian calendar (Note 2)</param>
        ''' <param name="im">Month in Gregorian calendar (Note 2)</param>
        ''' <param name="id">Day in Gregorian calendar (Note 2)</param>
        ''' <param name="ihr">Hour</param>
        ''' <param name="imn">Minute</param>
        ''' <param name="sec">Seconds</param>
        ''' <param name="d1">2-part Julian Date (Notes 3, 4)</param>
        ''' <param name="d2">2-part Julian Date (Notes 3, 4)</param>
        ''' <returns>Status: +3 = both of next two, +2 = time is after end of day (Note 5), +1 = dubious year (Note 6), 0 = OK, -1 = bad year, -2 = bad month, -3 = bad day, -4 = bad hour, -5 = bad minute, -6 = bad second (&lt;0)</returns>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description>Scale identifies the time scale.  Only the value "UTC" (in upper case) is significant, and enables handling of leap seconds (see Note 4).</description></item>
        ''' <item><description>For calendar conventions and limitations, see iauCal2jd.</description></item>
        ''' <item><description>The sum of the results, d1+d2, is Julian Date, where normally d1 is the Julian Day Number and d2 is the fraction of a day.  In the case of UTC, where the use of JD is problematical, special conventions apply:  see the next note.</description></item>
        ''' <item><description>JD cannot unambiguously represent UTC during a leap second unless special measures are taken.  The SOFA internal convention is that the quasi-JD day represents UTC days whether the length is 86399,
        ''' 86400 or 86401 SI seconds.  In the 1960-1972 era there were smaller jumps (in either direction) each time the linear UTC(TAI) expression was changed, and these "mini-leaps" are also included in the SOFA convention.</description></item>
        ''' <item><description>The warning status "time is after end of day" usually means that the sec argument is greater than 60.0.  However, in a day ending in a leap second the limit changes to 61.0 (or 59.0 in the case of a negative leap second).</description></item>
        ''' <item><description>The warning status "dubious year" flags UTCs that predate the introduction of the time scale or that are too far in the future to be trusted.  See iauDat for further details.</description></item>
        ''' <item><description>Only in the case of continuous and regular time scales (TAI, TT, TCG, TCB and TDB) is the result d1+d2 a Julian Date, strictly speaking.  In the other cases (UT1 and UTC) the result must be
        ''' used with circumspection;  in particular the difference between two such results cannot be interpreted as a precise time interval.</description></item>
        ''' </list>
        ''' </remarks>
        Public Function Dtf2d(scale As String,
                              iy As Integer,
                              im As Integer,
                              id As Integer,
                              ihr As Integer,
                              imn As Integer,
                              sec As Double,
                              ByRef d1 As Double,
                              ByRef d2 As Double) As Integer Implements ISOFA.Dtf2d

            Dim RetCode As Short

            If Is64Bit() Then
                RetCode = Dtf2d64(scale, iy, im, id, ihr, imn, sec, d1, d2)
            Else
                RetCode = Dtf2d32(scale, iy, im, id, ihr, imn, sec, d1, d2)
            End If

            Return Convert.ToInt32(RetCode)
        End Function

        ''' <summary>
        ''' Equation of the origins, IAU 2006 precession and IAU 2000A nutation.
        ''' </summary>
        ''' <param name="date1">TT as a 2-part Julian Date (Note 1)</param>
        ''' <param name="date2">TT as a 2-part Julian Date (Note 1)</param>
        ''' <returns>Equation of the origins in radians (Note 2)</returns>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description> The TT date date1+date2 is a Julian Date, apportioned in any convenient way between the two arguments.  For example, JD(TT)=2450123.7 could be expressed in any of these ways, among others:
        ''' <table style="width:340px;" cellspacing="0">
        ''' <col style="width:80px;"></col>
        ''' <col style="width:80px;"></col>
        ''' <col style="width:180px;"></col>
        ''' <tr>
        ''' <td colspan="1" align="center" rowspan="1" style="width: 80px; padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-top-color: #000000; border-top-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid;
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        ''' background-color: #00ffff;" width="110px">
        ''' <b>Date 1</b></td>
        ''' <td colspan="1" rowspan="1" align="center" style="width: 80px; padding-right: 10px; padding-left: 10px; 
        ''' border-top-color: #000000; border-top-style: Solid; 
        ''' border-right-style: Solid; border-right-color: #000000; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        ''' background-color: #00ffff;" width="110px">
        ''' <b>Date 2</b></td>
        ''' <td colspan="1" rowspan="1" align="center" style="width: 180px; padding-right: 10px; padding-left: 10px; 
        ''' border-top-color: #000000; border-top-style: Solid; 
        ''' border-right-style: Solid; border-right-color: #000000; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        ''' background-color: #00ffff;" width="220px">
        ''' <b>Method</b></td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        '''  2450123.7</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 0.0</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' JD method</td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 2451545.0</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' -1421.3</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' J2000 method</td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 2400000.5</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 50123.2</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' MJD method</td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 2450123.5</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 0.2</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' Date and time method</td>
        ''' </tr>
        ''' </table>
        ''' <para>The JD method is the most natural and convenient to use in cases where the loss of several decimal digits of resolution is acceptable.  The J2000 method is best matched to the way the argument is handled internally 
        ''' and will deliver the optimum resolution.  The MJD method and the date and time methods are both good compromises between resolution and convenience.  For most applications of this function the choice will not be at all critical.</para>
        ''' </description></item>
        ''' <item><description> The equation of the origins is the distance between the true equinox and the celestial intermediate origin and, equivalently, the difference between Earth rotation angle and Greenwich
        ''' apparent sidereal time (ERA-GST).  It comprises the precession (since J2000.0) in right ascension plus the equation of the equinoxes (including the small correction terms).</description></item>
        ''' </list>
        ''' </remarks>
        Public Function Eo06a(date1 As Double,
                              date2 As Double) As Double Implements ISOFA.Eo06a

            Dim RetVal As Double

            If Is64Bit() Then
                RetVal = Eo06a64(date1, date2)
            Else
                RetVal = Eo06a32(date1, date2)
            End If

            Return RetVal
        End Function

        ''' <summary>
        ''' Transform star RA,Dec from geocentric CIRS to ICRS astrometric using the SOFA Atic13 function.
        ''' </summary>
        ''' <param name="ri">CIRS geocentric RA (radians)</param>
        ''' <param name="di">CIRS geocentric Dec (radians)</param>
        ''' <param name="date1">TDB as a 2-part Julian Date (Note 1)</param>
        ''' <param name="date2">TDB as a 2-part Julian Date (Note 1)</param>
        ''' <param name="rc">ICRS astrometric RA (radians)</param>
        ''' <param name="dc">ICRS astrometric Dec (radians)</param>
        ''' <param name="eo">equation of the origins (ERA-GST, Note 4)</param>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description> The TDB date date1+date2 is a Julian Date, apportioned in any convenient way between the two arguments.  For example, JD(TDB)=2450123.8g could be expressed in any of these ways, among others:
        ''' <table style="width:340px;" cellspacing="0">
        ''' <col style="width:80px;"></col>
        ''' <col style="width:80px;"></col>
        ''' <col style="width:180px;"></col>
        ''' <tr>
        ''' <td colspan="1" align="center" rowspan="1" style="width: 80px; padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-top-color: #000000; border-top-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid;
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        ''' background-color: #00ffff;" width="110px">
        ''' <b>Date 1</b></td>
        ''' <td colspan="1" rowspan="1" align="center" style="width: 80px; padding-right: 10px; padding-left: 10px; 
        ''' border-top-color: #000000; border-top-style: Solid; 
        ''' border-right-style: Solid; border-right-color: #000000; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        ''' background-color: #00ffff;" width="110px">
        ''' <b>Date 2</b></td>
        ''' <td colspan="1" rowspan="1" align="center" style="width: 180px; padding-right: 10px; padding-left: 10px; 
        ''' border-top-color: #000000; border-top-style: Solid; 
        ''' border-right-style: Solid; border-right-color: #000000; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        ''' background-color: #00ffff;" width="220px">
        ''' <b>Method</b></td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        '''  2450123.8</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 0.0</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' JD method</td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 2451545.0</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' -1421.3</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' J2000 method</td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 2400000.5</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 50123.2</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' MJD method</td>
        ''' </tr>
        ''' <tr>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-left-color: #000000; border-left-style: Solid; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 2450123.5</td>
        ''' <td align="right" style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' 0.2</td>
        ''' <td style="padding-right: 10px; padding-left: 10px; 
        ''' border-right-color: #000000; border-right-style: Solid; 
        ''' border-bottom-color: #000000; border-bottom-style: Solid; 
        ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        ''' Date and time method</td>
        ''' </tr>
        ''' </table>
        ''' <para>The JD method is the most natural and convenient to use in cases where the loss of several decimal digits of resolution is acceptable.  The J2000 method is best matched to the way the argument is handled internally 
        ''' and will deliver the optimum resolution.  The MJD method and the date and time methods are both good compromises between resolution and convenience.  For most applications of this function the choice will not be at all critical.</para>
        ''' <para>TT can be used instead of TDB without any significant impact on accuracy.</para>
        ''' </description></item>
        ''' <item><description>Iterative techniques are used for the aberration and light deflection corrections so that the functions Atic13 and Atci13 are accurate inverses; 
        ''' even at the edge of the Sun's disk the discrepancy is only about 1 nanoarcsecond.</description></item>
        ''' <item><description>The available accuracy is better than 1 milliarcsecond, limited mainly by the precession-nutation model that is used, namely IAU 2000A/2006.  Very close to solar system bodies, additional 
        ''' errors of up to several milliarcseconds can occur because of unmodeled light deflection;  however, the Sun's contribution is taken into account, to first order.  The accuracy limitations of 
        ''' the SOFA function iauEpv00 (used to compute Earth position and velocity) can contribute aberration errors of up to 5 microarcseconds.  Light deflection at the Sun's limb is uncertain at the 0.4 mas level.</description></item>
        ''' <item><description>Should the transformation to (equinox based) J2000.0 mean place be required rather than (CIO based) ICRS coordinates, subtract the equation of the origins from the returned right ascension:
        ''' RA = RI - EO.  (The Anp function can then be applied, as required, to keep the result in the conventional 0-2pi range.)</description></item>
        ''' </list>
        ''' </remarks>
        Public Sub IntermediateToCelestial(ri As Double,
                          di As Double,
                          date1 As Double,
                          date2 As Double,
                          ByRef rc As Double,
                          ByRef dc As Double,
                          ByRef eo As Double) Implements ISOFA.Atic13

            If Is64Bit() Then
                Atic1364(ri, di, date1, date2, rc, dc, eo)
            Else
                Atic1332(ri, di, date1, date2, rc, dc, eo)
            End If
        End Sub

        ''' <summary>
        ''' CIRS RA,Dec to observed place using the SOFA Atio13 funciton.
        ''' </summary>
        ''' <param name="ri">CIRS right ascension (CIO-based, radians)</param>
        ''' <param name="di">CIRS declination (radians)</param>
        ''' <param name="utc1">UTC as a 2-part quasi Julian Date (Notes 1,2)</param>
        ''' <param name="utc2">UTC as a 2-part quasi Julian Date (Notes 1,2)</param>
        ''' <param name="dut1">UT1-UTC (seconds, Note 3)</param>
        ''' <param name="elong">longitude (radians, east +ve, Note 4)</param>
        ''' <param name="phi">geodetic latitude (radians, Note 4)</param>
        ''' <param name="hm">height above ellipsoid (m, geodetic Notes 4,6)</param>
        ''' <param name="xp">polar motion coordinates (radians, Note 5)</param>
        ''' <param name="yp">polar motion coordinates (radians, Note 5)</param>
        ''' <param name="phpa">pressure at the observer (hPa = mB, Note 6)</param>
        ''' <param name="tc">ambient temperature at the observer (deg C)</param>
        ''' <param name="rh">relative humidity at the observer (range 0-1)</param>
        ''' <param name="wl">wavelength (micrometers, Note 7)</param>
        ''' <param name="aob">observed azimuth (radians: N=0,E=90)</param>
        ''' <param name="zob">observed zenith distance (radians)</param>
        ''' <param name="hob">observed hour angle (radians)</param>
        ''' <param name="dob">observed declination (radians)</param>
        ''' <param name="rob">observed right ascension (CIO-based, radians)</param>
        ''' <returns> Status: +1 = dubious year (Note 2), 0 = OK, -1 = unacceptable date</returns>
        ''' <remarks>
        ''' <para>Notes:</para>
        ''' <list type="number">
        ''' <item><description>utc1+utc2 is quasi Julian Date (see Note 2), apportioned in any convenient way between the two arguments, for example where utc1 is the Julian Day Number and utc2 is the fraction of a day.
        ''' <para>However, JD cannot unambiguously represent UTC during a leap second unless special measures are taken.  The convention in the present function is that the JD day represents UTC days whether the length is 86399, 86400 or 86401 SI seconds.</para>
        ''' <para>Applications should use the function iauDtf2d to convert from calendar date and time of day into 2-part quasi Julian Date, as it implements the leap-second-ambiguity convention just described.</para></description></item>
        ''' <item><description>The warning status "dubious year" flags UTCs that predate the introduction of the time scale or that are too far in the future to be trusted.  See iauDat for further details.</description></item>
        ''' <item><description>UT1-UTC is tabulated in IERS bulletins.  It increases by exactly one second at the end of each positive UTC leap second, introduced in order to keep UT1-UTC within +/- 0.9s.  n.b. This practice is under review, and in the future UT1-UTC may grow essentially without limit.</description></item>
        ''' <item><description>The geographical coordinates are with respect to the WGS84 reference ellipsoid.  TAKE CARE WITH THE LONGITUDE SIGN:  the longitude required by the present function is east-positive (i.e. right-handed), in accordance with geographical convention.</description></item>
        ''' <item><description>The polar motion xp,yp can be obtained from IERS bulletins.  The values are the coordinates (in radians) of the Celestial Intermediate Pole with respect to the International Terrestrial
        ''' Reference System (see IERS Conventions 2003), measured along the meridians 0 and 90 deg west respectively.  For many applications, xp and yp can be set to zero.</description></item>
        ''' <item><description>If hm, the height above the ellipsoid of the observing station in meters, is not known but phpa, the pressure in hPa (=mB), is available, an adequate estimate of hm can be obtained from the expression:
        ''' <p style="margin-left:25px;font-family:Lucida Conosle,Monospace"><b>hm = -29.3 * tsl * log ( phpa / 1013.25 );</b></p>
        ''' <para>where tsl is the approximate sea-level air temperature in K (See Astrophysical Quantities, C.W.Allen, 3rd edition, section 52).  Similarly, if the pressure phpa is not known, it can be estimated from the height of the observing station, hm, as follows:</para>
        ''' <p style="margin-left:25px;font-family:Lucida Conosle,Monospace"><b>phpa = 1013.25 * exp ( -hm / ( 29.3 * tsl ) );</b></p>
        ''' <para>Note, however, that the refraction is nearly proportional to the pressure and that an accurate phpa value is important for precise work.</para></description></item>
        ''' <item><description>The argument wl specifies the observing wavelength in micrometers.  The transition from optical to radio is assumed to occur at 100 micrometers (about 3000 GHz).</description></item>
        ''' <item><description>"Observed" Az,ZD means the position that would be seen by a perfect geodetically aligned theodolite.  (Zenith distance is used rather than altitude in order to reflect the fact that no
        ''' allowance is made for depression of the horizon.)  This is related to the observed HA,Dec via the standard rotation, using the geodetic latitude (corrected for polar motion), while the observed HA and RA are related simply through the Earth rotation
        ''' angle and the site longitude.  "Observed" RA,Dec or HA,Dec thus means the position that would be seen by a perfect equatorial with its polar axis aligned to the Earth's axis of rotation.</description></item>
        ''' <item><description>The accuracy of the result is limited by the corrections for refraction, which use a simple A*tan(z) + B*tan^3(z) model. Providing the meteorological parameters are known accurately and there are no gross local effects, the predicted astrometric
        ''' coordinates should be within 0.05 arcsec (optical) or 1 arcsec (radio) for a zenith distance of less than 70 degrees, better than 30 arcsec (optical or radio) at 85 degrees and better than 20 arcmin (optical) or 30 arcmin (radio) at the horizon.</description></item>
        ''' <item><description>The complementary functions iauAtio13 and iauAtoi13 are self-consistent to better than 1 microarcsecond all over the celestial sphere.</description></item>
        ''' <item><description>It is advisable to take great care with units, as even unlikely values of the input parameters are accepted and processed in accordance with the models used.</description></item>
        ''' </list>
        ''' </remarks>
        Public Function IntermediateToObserved(ri As Double,
                                         di As Double,
                                         utc1 As Double,
                                         utc2 As Double,
                                         dut1 As Double,
                                         elong As Double,
                                         phi As Double,
                                         hm As Double,
                                         xp As Double,
                                         yp As Double,
                                         phpa As Double,
                                         tc As Double,
                                         rh As Double,
                                         wl As Double,
                                         ByRef aob As Double,
                                         ByRef zob As Double,
                                         ByRef hob As Double,
                                         ByRef dob As Double,
                                         ByRef rob As Double) As Integer Implements ISOFA.Atio13
            Dim RetCode As Short

            If Is64Bit() Then
                RetCode = Atio1364(ri, di, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, aob, zob, hob, dob, rob)
            Else
                RetCode = Atio1332(ri, di, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, aob, zob, hob, dob, rob)
            End If

            Return Convert.ToInt32(RetCode)
        End Function

        ''' <summary>
        ''' Observed place at a groundbased site to to ICRS astrometric RA,Dec using the SOFA Atoc13 function.
        ''' </summary>
        ''' <param name="type">type of coordinates - "R", "H" or "A" (Notes 1,2)</param>
        ''' <param name="ob1">observed Az, HA or RA (radians; Az is N=0,E=90)</param>
        ''' <param name="ob2"> observed ZD or Dec (radians)</param>
        ''' <param name="utc1">UTC as a 2-part quasi Julian Date (Notes 3,4)</param>
        ''' <param name="utc2">UTC as a 2-part quasi Julian Date (Notes 3,4)</param>
        ''' <param name="dut1">UT1-UTC (seconds, Note 5)</param>
        ''' <param name="elong">longitude (radians, east +ve, Note 6)</param>
        ''' <param name="phi">geodetic latitude (radians, Note 6)</param>
        ''' <param name="hm">height above ellipsoid (m, geodetic Notes 6,8)</param>
        ''' <param name="xp">polar motion coordinates (radians, Note 7)</param>
        ''' <param name="yp">polar motion coordinates (radians, Note 7)</param>
        ''' <param name="phpa">pressure at the observer (hPa = mB, Note 8)</param>
        ''' <param name="tc">ambient temperature at the observer (deg C)</param>
        ''' <param name="rh">relative humidity at the observer (range 0-1)</param>
        ''' <param name="wl">wavelength (micrometers, Note 9)</param>
        ''' <param name="rc">ICRS astrometric RA (radians)</param>
        ''' <param name="dc">ICRS astrometric Dec (radians)</param>
        ''' <returns>Status: +1 = dubious year (Note 4), 0 = OK, -1 = unacceptable date</returns>
        ''' <remarks>
        ''' <para>Notes:</para>
        ''' <list type="number">
        ''' <item><description>"Observed" Az,ZD means the position that would be seen by a perfect geodetically aligned theodolite.  (Zenith distance is used rather than altitude in order to reflect the fact that no
        ''' allowance is made for depression of the horizon.)  This is related to the observed HA,Dec via the standard rotation, using the geodetic latitude (corrected for polar motion), while the
        ''' observed HA and RA are related simply through the Earth rotation angle and the site longitude.  "Observed" RA,Dec or HA,Dec thus means the position that would be seen by a perfect equatorial with its polar axis aligned to the Earth's axis of rotation.</description></item>
        ''' <item><description>Only the first character of the type argument is significant. "R" or "r" indicates that ob1 and ob2 are the observed right ascension and declination;  "H" or "h" indicates that they are hour angle (west +ve) and declination;  anything else ("A" or
        ''' "a" is recommended) indicates that ob1 and ob2 are azimuth (north zero, east 90 deg) and zenith distance.</description></item>
        ''' <item><description>utc1+utc2 is quasi Julian Date (see Note 2), apportioned in any convenient way between the two arguments, for example where utc1 is the Julian Day Number and utc2 is the fraction of a day.
        ''' <para>However, JD cannot unambiguously represent UTC during a leap second unless special measures are taken.  The convention in the present function is that the JD day represents UTC days whether the length is 86399, 86400 or 86401 SI seconds.</para>
        ''' <para>Applications should use the function iauDtf2d to convert from calendar date and time of day into 2-part quasi Julian Date, as it implements the leap-second-ambiguity convention just described.</para></description></item>
        ''' <item><description>The warning status "dubious year" flags UTCs that predate the introduction of the time scale or that are too far in the future to be trusted.  See iauDat for further details.</description></item>
        ''' <item><description>UT1-UTC is tabulated in IERS bulletins.  It increases by exactly one second at the end of each positive UTC leap second, introduced in order to keep UT1-UTC within +/- 0.9s.  n.b. This practice is under review, and in the future UT1-UTC may grow essentially without limit.</description></item>
        ''' <item><description>The geographical coordinates are with respect to the WGS84 reference ellipsoid.  TAKE CARE WITH THE LONGITUDE SIGN:  the longitude required by the present function is east-positive (i.e. right-handed), in accordance with geographical convention.</description></item>
        ''' <item><description>The polar motion xp,yp can be obtained from IERS bulletins.  The values are the coordinates (in radians) of the Celestial Intermediate Pole with respect to the International Terrestrial Reference System (see IERS Conventions 2003), measured along the
        ''' meridians 0 and 90 deg west respectively.  For many applications, xp and yp can be set to zero.</description></item>
        ''' <item><description>If hm, the height above the ellipsoid of the observing station in meters, is not known but phpa, the pressure in hPa (=mB), is available, an adequate estimate of hm can be obtained from the expression:
        ''' <p style="margin-left:25px;font-family:Lucida Conosle,Monospace"><b>hm = -29.3 * tsl * log ( phpa / 1013.25 );</b></p>
        ''' <para>where tsl is the approximate sea-level air temperature in K (See Astrophysical Quantities, C.W.Allen, 3rd edition, section 52).  Similarly, if the pressure phpa is not known, it can be estimated from the height of the observing station, hm, as follows:</para>
        ''' <p style="margin-left:25px;font-family:Lucida Conosle,Monospace"><b>phpa = 1013.25 * exp ( -hm / ( 29.3 * tsl ) );</b></p>
        ''' <para>Note, however, that the refraction is nearly proportional to the pressure and that an accurate phpa value is important for precise work.</para></description></item>
        ''' <item><description>The argument wl specifies the observing wavelength in micrometers.  The transition from optical to radio is assumed to occur at 100 micrometers (about 3000 GHz).</description></item>
        ''' <item><description>The accuracy of the result is limited by the corrections for refraction, which use a simple A*tan(z) + B*tan^3(z) model. Providing the meteorological parameters are known accurately and
        ''' there are no gross local effects, the predicted astrometric coordinates should be within 0.05 arcsec (optical) or 1 arcsec (radio) for a zenith distance of less than 70 degrees, better than 30 arcsec (optical or radio) at 85 degrees and better
        ''' than 20 arcmin (optical) or 30 arcmin (radio) at the horizon.
        '''<para>Without refraction, the complementary functions iauAtco13 and iauAtoc13 are self-consistent to better than 1 microarcsecond all over the celestial sphere.  With refraction included, consistency falls off at high zenith distances, but is still better than 0.05 arcsec at 85 degrees.</para></description></item>
        ''' <item><description>It is advisable to take great care with units, as even unlikely values of the input parameters are accepted and processed in accordance with the models used.</description></item>
        ''' </list>
        ''' </remarks>
        Public Function ObservedToCelestial(type As String,
                                         ob1 As Double,
                                         ob2 As Double,
                                         utc1 As Double,
                                         utc2 As Double,
                                         dut1 As Double,
                                         elong As Double,
                                         phi As Double,
                                         hm As Double,
                                         xp As Double,
                                         yp As Double,
                                         phpa As Double,
                                         tc As Double,
                                         rh As Double,
                                         wl As Double,
                                         ByRef rc As Double,
                                         ByRef dc As Double) As Integer Implements ISOFA.Atoc13
            Dim RetCode As Short

            If Is64Bit() Then
                RetCode = Atoc1364(type, ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, rc, dc)
            Else
                RetCode = Atoc1332(type, ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, rc, dc)
            End If

            Return Convert.ToInt32(RetCode)
        End Function

        ''' <summary>
        '''  Observed place to CIRS using the SOFA Atoi13 function.
        ''' </summary>
        ''' <param name="type">type of coordinates - "R", "H" or "A" (Notes 1,2)</param>
        ''' <param name="ob1">observed Az, HA or RA (radians; Az is N=0,E=90)</param>
        ''' <param name="ob2">observed ZD or Dec (radians)</param>
        ''' <param name="utc1">UTC as a 2-part quasi Julian Date (Notes 3,4)</param>
        ''' <param name="utc2">UTC as a 2-part quasi Julian Date (Notes 3,4)</param>
        ''' <param name="dut1">UT1-UTC (seconds, Note 5)</param>
        ''' <param name="elong">longitude (radians, east +ve, Note 6)</param>
        ''' <param name="phi">geodetic latitude (radians, Note 6)</param>
        ''' <param name="hm">height above the ellipsoid (meters, Notes 6,8)</param>
        ''' <param name="xp">polar motion coordinates (radians, Note 7)</param>
        ''' <param name="yp">polar motion coordinates (radians, Note 7)</param>
        ''' <param name="phpa">pressure at the observer (hPa = mB, Note 8)</param>
        ''' <param name="tc">ambient temperature at the observer (deg C)</param>
        ''' <param name="rh">relative humidity at the observer (range 0-1)</param>
        ''' <param name="wl">wavelength (micrometers, Note 9)</param>
        ''' <param name="ri">CIRS right ascension (CIO-based, radians)</param>
        ''' <param name="di">CIRS declination (radians)</param>
        ''' <returns>Status: +1 = dubious year (Note 2), 0 = OK, -1 = unacceptable date</returns>
        ''' <remarks>
        ''' <para>Notes:</para>
        ''' <list type="number">
        ''' <item><description>"Observed" Az,ZD means the position that would be seen by a perfect geodetically aligned theodolite.  (Zenith distance is used rather than altitude in order to reflect the fact that no
        ''' allowance is made for depression of the horizon.)  This is related to the observed HA,Dec via the standard rotation, using the geodetic latitude (corrected for polar motion), while the
        ''' observed HA and RA are related simply through the Earth rotation angle and the site longitude.  "Observed" RA,Dec or HA,Dec thus means the position that would be seen by a perfect equatorial
        ''' with its polar axis aligned to the Earth's axis of rotation.</description></item>
        ''' <item><description>Only the first character of the type argument is significant. "R" or "r" indicates that ob1 and ob2 are the observed right ascension and declination;  "H" or "h" indicates that they are
        ''' hour angle (west +ve) and declination;  anything else ("A" or "a" is recommended) indicates that ob1 and ob2 are azimuth (north zero, east 90 deg) and zenith distance.</description></item>
        ''' <item><description>utc1+utc2 is quasi Julian Date (see Note 2), apportioned in any convenient way between the two arguments, for example where utc1 is the Julian Day Number and utc2 is the fraction of a day.
        ''' <para>However, JD cannot unambiguously represent UTC during a leap second unless special measures are taken.  The convention in the present function is that the JD day represents UTC days whether the length is 86399, 86400 or 86401 SI seconds.</para>
        ''' <para>Applications should use the function iauDtf2d to convert from calendar date and time of day into 2-part quasi Julian Date, as it implements the leap-second-ambiguity convention just described.</para></description></item>
        ''' <item><description>The warning status "dubious year" flags UTCs that predate the introduction of the time scale or that are too far in the future to be trusted.  See iauDat for further details.</description></item>
        ''' <item><description>UT1-UTC is tabulated in IERS bulletins.  It increases by exactly one second at the end of each positive UTC leap second, introduced in order to keep UT1-UTC within +/- 0.9s.  n.b. This
        ''' practice is under review, and in the future UT1-UTC may grow essentially without limit.</description></item>
        ''' <item><description>The geographical coordinates are with respect to the WGS84 reference ellipsoid.  TAKE CARE WITH THE LONGITUDE SIGN:  the longitude required by the present function is east-positive
        ''' (i.e. right-handed), in accordance with geographical convention.</description></item>
        ''' <item><description>The polar motion xp,yp can be obtained from IERS bulletins.  The values are the coordinates (in radians) of the Celestial Intermediate Pole with respect to the International Terrestrial
        ''' Reference System (see IERS Conventions 2003), measured along the meridians 0 and 90 deg west respectively.  For many applications, xp and yp can be set to zero.</description></item>
        ''' <item><description>If hm, the height above the ellipsoid of the observing station in meters, is not known but phpa, the pressure in hPa (=mB), is available, an adequate estimate of hm can be obtained from the expression:
        ''' <p style="margin-left:25px;font-family:Lucida Conosle,Monospace"><b>hm = -29.3 * tsl * log ( phpa / 1013.25 );</b></p>
        ''' <para>where tsl is the approximate sea-level air temperature in K (See Astrophysical Quantities, C.W.Allen, 3rd edition, section 52).  Similarly, if the pressure phpa is not known, it can be estimated from the height of the observing station, hm, as follows:</para>
        ''' <p style="margin-left:25px;font-family:Lucida Conosle,Monospace"><b>phpa = 1013.25 * exp ( -hm / ( 29.3 * tsl ) );</b></p>
        ''' <para>Note, however, that the refraction is nearly proportional to the pressure and that an accurate phpa value is important for precise work.</para></description></item>
        ''' <item><description>The argument wl specifies the observing wavelength in micrometers.  The transition from optical to radio is assumed to occur at 100 micrometers (about 3000 GHz).</description></item>
        ''' <item><description>The accuracy of the result is limited by the corrections for refraction, which use a simple A*tan(z) + B*tan^3(z) model. Providing the meteorological parameters are known accurately and
        ''' there are no gross local effects, the predicted astrometric coordinates should be within 0.05 arcsec (optical) or 1 arcsec (radio) for a zenith distance of less than 70 degrees, better
        ''' than 30 arcsec (optical or radio) at 85 degrees and better than 20 arcmin (optical) or 30 arcmin (radio) at the horizon.
        ''' <para>Without refraction, the complementary functions iauAtio13 and iauAtoi13 are self-consistent to better than 1 microarcsecond all over the celestial sphere.  With refraction included,
        ''' consistency falls off at high zenith distances, but is still better than 0.05 arcsec at 85 degrees.</para></description></item>
        ''' <item><description>It is advisable to take great care with units, as even unlikely values of the input parameters are accepted and processed in accordance with the models used.</description></item>
        ''' </list>
        ''' </remarks>
        Public Function ObservedToIntermediate(type As String,
                                         ob1 As Double,
                                         ob2 As Double,
                                         utc1 As Double,
                                         utc2 As Double,
                                         dut1 As Double,
                                         elong As Double,
                                         phi As Double,
                                         hm As Double,
                                         xp As Double,
                                         yp As Double,
                                         phpa As Double,
                                         tc As Double,
                                         rh As Double,
                                         wl As Double,
                                         ByRef ri As Double,
                                         ByRef di As Double) As Integer Implements ISOFA.Atoi13
            Dim RetCode As Short

            If Is64Bit() Then
                RetCode = Atoi1364(type, ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ri, di)

            Else
                RetCode = Atoi1332(type, ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ri, di)
            End If

            Return Convert.ToInt32(RetCode)
        End Function

        ''' <summary>
        ''' Time scale transformation:  International Atomic Time, TAI, to Coordinated Universal Time, UTC.
        ''' </summary>
        ''' <param name="tai1">TAI as a 2-part Julian Date (Note 1)</param>
        ''' <param name="tai2">TAI as a 2-part Julian Date (Note 1)</param>
        ''' <param name="utc1">UTC as a 2-part quasi Julian Date (Notes 1-3)</param>
        ''' <param name="utc2">UTC as a 2-part quasi Julian Date (Notes 1-3)</param>
        ''' <returns>Status: +1 = dubious year (Note 4), 0 = OK, -1 = unacceptable date</returns>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description>tai1+tai2 is Julian Date, apportioned in any convenient way between the two arguments, for example where tai1 is the Julian Day Number and tai2 is the fraction of a day.  The returned utc1
        ''' and utc2 form an analogous pair, except that a special convention is used, to deal with the problem of leap seconds - see the next note.</description></item>
        ''' <item><description>JD cannot unambiguously represent UTC during a leap second unless special measures are taken.  The convention in the present function is that the JD day represents UTC days whether the
        ''' length is 86399, 86400 or 86401 SI seconds.  In the 1960-1972 era there were smaller jumps (in either direction) each time the linear UTC(TAI) expression was changed, and these "mini-leaps are also included in the SOFA convention.</description></item>
        ''' <item><description>The function iauD2dtf can be used to transform the UTC quasi-JD into calendar date and clock time, including UTC leap second handling.</description></item>
        ''' <item><description>The warning status "dubious year" flags UTCs that predate the introduction of the time scale or that are too far in the future to be trusted.  See iauDat for further details.</description></item>
        ''' </list>
        ''' </remarks>
        Public Function TaiUtc(tai1 As Double, tai2 As Double, ByRef utc1 As Double, ByRef utc2 As Double) As Integer Implements ISOFA.TaiUtc
            Dim RetCode As Short

            If Is64Bit() Then
                RetCode = Taiutc64(tai1, tai2, utc1, utc2)
            Else
                RetCode = Taiutc32(tai1, tai2, utc1, utc2)
            End If

            Return Convert.ToInt32(RetCode)
        End Function

        ''' <summary>
        ''' Time scale transformation:  International Atomic Time, TAI, to Terrestrial Time, TT.
        ''' </summary>
        ''' <param name="tai1">TAI as a 2-part Julian Date</param>
        ''' <param name="tai2">TAI as a 2-part Julian Date</param>
        ''' <param name="tt1">TT as a 2-part Julian Date</param>
        ''' <param name="tt2">TT as a 2-part Julian Date</param>
        ''' <returns>Status:  0 = OK</returns>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description> tai1+tai2 is Julian Date, apportioned in any convenient way between the two arguments, for example where tai1 is the Julian Day Number and tai2 is the fraction of a day.  The returned
        ''' tt1,tt2 follow suit.</description></item>
        ''' </list>
        ''' </remarks>
        Public Function TaiTt(tai1 As Double,
                              tai2 As Double,
                              ByRef tt1 As Double,
                              ByRef tt2 As Double) As Integer Implements ISOFA.TaiTt

            Dim RetCode As Short

            If Is64Bit() Then
                RetCode = Taitt64(tai1, tai2, tt1, tt2)
            Else
                RetCode = Taitt32(tai1, tai2, tt1, tt2)
            End If

            Return Convert.ToInt32(RetCode)
        End Function

        ''' <summary>
        ''' Time scale transformation:  Terrestrial Time, TT, to International Atomic Time, TAI.
        ''' </summary>
        ''' <param name="tt1">TT as a 2-part Julian Date</param>
        ''' <param name="tt2">TT as a 2-part Julian Date</param>
        ''' <param name="tai1">TAI as a 2-part Julian Date</param>
        ''' <param name="tai2">TAI as a 2-part Julian Date</param>
        ''' <returns>Status:  0 = OK</returns>
        ''' <remarks>
        ''' Note
        ''' <list type="number">
        ''' <item><description>tt1+tt2 is Julian Date, apportioned in any convenient way between the two arguments, for example where tt1 is the Julian Day Number and tt2 is the fraction of a day.  The returned tai1,tai2 follow suit.</description></item>
        ''' </list>
        ''' </remarks>
        Public Function TtTai(tt1 As Double, tt2 As Double, ByRef tai1 As Double, ByRef tai2 As Double) As Integer Implements ISOFA.TtTai
            Dim RetCode As Short

            If Is64Bit() Then
                RetCode = Tttai64(tt1, tt2, tai1, tai2)
            Else
                RetCode = Tttai32(tt1, tt2, tai1, tai2)
            End If

            Return Convert.ToInt32(RetCode)
        End Function

        ''' <summary>
        ''' Convert hours, minutes, seconds to radians.
        ''' </summary>
        ''' <param name="s">sign:  '-' = negative, otherwise positive</param>
        ''' <param name="ihour">Hours</param>
        ''' <param name="imin">Minutes</param>
        ''' <param name="sec">Seconds</param>
        ''' <param name="rad">Angle in radians</param>
        ''' <returns>Status:  0 = OK, 1 = ihour outside range 0-23, 2 = imin outside range 0-59, 3 = sec outside range 0-59.999...</returns>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description>The result is computed even if any of the range checks fail.</description></item>
        ''' <item><description>Negative ihour, imin and/or sec produce a warning status, but the absolute value is used in the conversion.</description></item>
        ''' <item><description>If there are multiple errors, the status value reflects only the first, the smallest taking precedence.</description></item>
        ''' </list>
        ''' </remarks>
        Public Function Tf2a(s As String,
                             ihour As Integer,
                             imin As Integer,
                             sec As Double,
                             ByRef rad As Double) As Integer Implements ISOFA.Tf2a

            Dim RetCode As Short

            If String.IsNullOrEmpty(s) Then s = " " ' Fix any invalid sign values

            If Is64Bit() Then
                Tf2a64(s.ToCharArray()(0), Convert.ToInt16(ihour), Convert.ToInt16(imin), sec, rad)
            Else
                Tf2a32(s.ToCharArray()(0), Convert.ToInt16(ihour), Convert.ToInt16(imin), sec, rad)
            End If

            Return Convert.ToInt32(RetCode)
        End Function

        ''' <summary>
        ''' Time scale transformation:  Coordinated Universal Time, UTC, to International Atomic Time, TAI.
        ''' </summary>
        ''' <param name="utc1">UTC as a 2-part quasi Julian Date (Notes 1-4)</param>
        ''' <param name="utc2">UTC as a 2-part quasi Julian Date (Notes 1-4)</param>
        ''' <param name="tai1">TAI as a 2-part Julian Date (Note 5)</param>
        ''' <param name="tai2">TAI as a 2-part Julian Date (Note 5)</param>
        ''' <returns>Status: +1 = dubious year (Note 3) 0 = OK -1 = unacceptable date</returns>
        ''' <remarks>
        ''' Notes:
        ''' <list type="number">
        ''' <item><description>utc1+utc2 is quasi Julian Date (see Note 2), apportioned in any convenient way between the two arguments, for example where utc1 is the Julian Day Number and utc2 is the fraction of a day.</description></item>
        ''' <item><description>JD cannot unambiguously represent UTC during a leap second unless special measures are taken.  The convention in the present function is that the JD day represents UTC days whether the
        ''' length is 86399, 86400 or 86401 SI seconds.  In the 1960-1972 era there were smaller jumps (in either direction) each time the linear UTC(TAI) expression was changed, and these "mini-leaps" are also included in the SOFA convention.</description></item>
        ''' <item><description>The warning status "dubious year" flags UTCs that predate the introduction of the time scale or that are too far in the future to be trusted.  See iauDat for further details.</description></item>
        ''' <item><description>The function iauDtf2d converts from calendar date and time of day into 2-part Julian Date, and in the case of UTC implements the leap-second-ambiguity convention described above.</description></item>
        ''' <item><description>The returned TAI1,TAI2 are such that their sum is the TAI Julian Date.</description></item>
        ''' </list>
        ''' </remarks>
        Public Function UtcTai(utc1 As Double,
                               utc2 As Double,
                               ByRef tai1 As Double,
                               ByRef tai2 As Double) As Integer Implements ISOFA.UtcTai

            Dim RetCode As Short

            If Is64Bit() Then
                Utctai64(utc1, utc2, tai1, tai2)
            Else
                Utctai32(utc1, utc2, tai1, tai2)
            End If

            Return Convert.ToInt32(RetCode)
        End Function

        ''' <summary>
        ''' Transform hour angle and declination to azimuth And altitude.
        ''' </summary>
        ''' <param name="ha">Hour angle (radians)</param>
        ''' <param name="dec">Declination (radians)</param>
        ''' <param name="phi">Latitude (radians)</param>
        ''' <param name="az">Azimuth (radians) - North = 0, East = +pi/2</param>
        ''' <param name="el">Altitude (radians) - Horizon = 0, Zenith = +pi/2</param>
        ''' <remarks>
        ''' Note
        ''' <list type="number">
        ''' <item><description>All the arguments are angles in radians.</description></item>
        ''' <item><description>Azimuth is returned in the range 0−2pi;  north is zero, and east is +pi/2.  Altitude Is returned in the range +/− pi/2.</description></item>
        ''' <item><description>The latitude phi is pi/2 minus the angle between the Earth’s rotation axis And the adopted zenith.  
        ''' In many applications it will be sufficient to use the published geodetic latitude of the site.  
        ''' In very precise (sub−arcsecond) applications, phi can be corrected for polar motion.</description></item>
        ''' <item><description>The returned azimuth az is with respect to the rotational north pole, as opposed to the ITRS pole, 
        ''' and for sub−arcsecond accuracy will need to be adjusted for polar motion if it Is to be with respect to north on a map of the Earth's surface.</description></item>
        ''' <item><description>Should the user wish to work with respect to the astronomical zenith rather than the geodetic zenith, phi will need to be
        ''' adjusted for deflection of the vertical (often tens of arcseconds), and the zero point of the hour angle ha will also be affected.</description></item>
        ''' <item><description>The transformation is the same as Vh = Rz(pi)*Ry(pi/2−phi)*Ve, where Vh And Ve are left-handed unit vectors in the (az,el) 
        ''' and (ha,dec) systems respectively And Ry And Rz are rotations about first the y−axis And then the z−axis.  (n.b. Rz(pi) simply
        '''  reverses the signs of the x And y components.)  For efficiency, the algorithm Is written out rather than calling other utility functions.  
        '''  For applications that require even greater efficiency, additional savings are possible if constant terms such as functions of latitude are computed once And for all.</description></item>
        ''' <item><description>Again for efficiency, no range checking of arguments is carried out.</description></item>
        ''' </list>
        ''' </remarks>
        Public Sub Hd2ae(ha As Double, dec As Double, phi As Double, ByRef az As Double, ByRef el As Double) Implements ISOFA.Hd2ae
            If Is64Bit() Then
                Hd2ae64(ha, dec, phi, az, el)
            Else
                Hd2ae32(ha, dec, phi, az, el)
            End If
        End Sub

        ''' <summary>
        ''' Convert position/velocity from spherical to Cartesian coordinates.
        ''' </summary>
        ''' <param name="theta">Longitude angle (radians)</param>
        ''' <param name="phi">Latitude angle (radians)</param>
        ''' <param name="r">Radial distance</param>
        ''' <param name="td">Rate of change of theta</param>
        ''' <param name="pd">Rate of change of phi</param>
        ''' <param name="rd">Rate of change of r</param>
        ''' <param name="pv">Position/velocity vector</param>

        Public Sub S2pv(theta As Double, phi As Double, r As Double, td As Double, pd As Double, rd As Double, pv As Double(,)) Implements ISOFA.S2pv
            If Is64Bit() Then
                S2pv64(theta, phi, r, td, pd, rd, pv)
            Else
                S2pv32(theta, phi, r, td, pd, rd, pv)
            End If

        End Sub

        ''' <summary>
        ''' Initialize an r−matrix to the identity matrix.
        ''' </summary>
        ''' <param name="r"> r−matrix</param>
        Public Sub Ir(r As Double(,)) Implements ISOFA.Ir
            If Is64Bit() Then
                Ir64(r)
            Else
                Ir32(r)
            End If
        End Sub

        ''' <summary>
        ''' Rotate an r−matrix about the y−axis.
        ''' </summary>
        ''' <param name="theta">Angle (radians)</param>
        ''' <param name="r">r−matrix, rotated</param>
        Public Sub Ry(theta As Double, r As Double(,)) Implements ISOFA.Ry
            If Is64Bit() Then
                Ry64(theta, r)
            Else
                Ry32(theta, r)
            End If
        End Sub

        ''' <summary>
        ''' Multiply a pv−vector by an r−matrix.
        ''' </summary>
        ''' <param name="r">r−matrix</param>
        ''' <param name="pv">pv−vector</param>
        ''' <param name="rpv">r * pv</param>
        ''' <remarks>
        ''' Note
        ''' <list type="number">
        ''' <item><description>The algorithm is for the simple case where the r−matrix r is not a function of time.  The case where r Is a function of time leads
        ''' to an additional velocity component equal to the product of the derivative of r And the position vector.</description></item>
        ''' <item><description>It is permissible for pv and rpv to be the same array.</description></item>
        ''' </list>
        ''' </remarks>
        Public Sub Rxpv(r As Double(,), pv As Double(,), rpv As Double(,)) Implements ISOFA.Rxpv
            If Is64Bit() Then
                Rxpv64(r, pv, rpv)
            Else
                Rxpv32(r, pv, rpv)
            End If
        End Sub

        ''' <summary>
        ''' Convert position/velocity from Cartesian to spherical coordinates.
        ''' </summary>
        ''' <param name="pv">pv-vector</param>
        ''' <param name="theta">Longitude angle (radians)</param>
        ''' <param name="phi">Latitude angle (radians)</param>
        ''' <param name="r">Radial distance</param>
        ''' <param name="td">Rate of change of theta</param>
        ''' <param name="pd">Rate of change of phi</param>
        ''' <param name="rd">Rate of change of r</param>
        ''' <remarks>
        ''' Note
        ''' <list type="number">
        ''' <item><description>If the position part of pv is null, theta, phi, td and pd are indeterminate.  This Is handled by extrapolating the position 
        ''' through unit time by using the velocity part of pv.  This moves the origin without changing the direction of the velocity component.  
        ''' If the position And velocity components of pv are both null, zeroes are returned for all six results.</description></item>
        ''' <item><description>If the position is a pole, theta, td and pd are indeterminate. In such cases zeroes are returned for all three.</description></item>
        ''' </list>
        ''' </remarks>
        Public Sub Pv2s(pv As Double(,), ByRef theta As Double, ByRef phi As Double, ByRef r As Double, ByRef td As Double, ByRef pd As Double, ByRef rd As Double) Implements ISOFA.Pv2s
            If Is64Bit() Then
                Pv2s64(pv, theta, phi, r, td, pd, rd)
            Else
                Pv2s32(pv, theta, phi, r, td, pd, rd)
            End If
        End Sub

#End Region

#Region "Private SOFA DLL access Functions"

        Private Function UpdateLeapSecondData(LeapSecondArray() As LeapSecondDataStruct) As Integer
            Dim RetCode As Short
            If Is64Bit() Then ' Call the appropriate 32 or 64 bit DLL depending on the application bitness.
                RetCode = UpdateLeapSecondData64(LeapSecondArray)
            Else
                RetCode = UpdateLeapSecondData32(LeapSecondArray)
            End If

            Return RetCode
        End Function

        Private Function GetLeapSecondData(LeapSecondArray() As LeapSecondDataStruct, ByRef HasUpdatedData As Boolean) As Integer
            Dim RetCode As Short, UpdatedDataInt As Integer
            If Is64Bit() Then ' Call the appropriate 32 or 64 bit DLL depending on the application bitness.
                RetCode = GetLeapSecondData64(LeapSecondArray, UpdatedDataInt)
            Else
                RetCode = GetLeapSecondData32(LeapSecondArray, UpdatedDataInt)
            End If
            HasUpdatedData = Convert.ToBoolean(UpdatedDataInt)
            Return RetCode
        End Function

        Private Function UsingUpdatedData() As Boolean
            Dim RetCode As Short, BooleanValue As Boolean
            If Is64Bit() Then ' Call the appropriate 32 or 64 bit DLL depending on the application bitness.
                RetCode = UsingUpdatedData64()
            Else
                RetCode = UsingUpdatedData32()
            End If
            BooleanValue = Convert.ToBoolean(RetCode)
            Return BooleanValue
        End Function

        Private Function LeapSeconds(Year As Integer, Month As Integer, Day As Integer, DayFraction As Double) As Integer
            Dim RetCode As Short, LeapSecondValue As Double
            If Is64Bit() Then ' Call the appropriate 32 or 64 bit DLL depending on the application bitness.
                RetCode = LeapSeconds64(Year, Month, Day, DayFraction, LeapSecondValue)
            Else
                RetCode = LeapSeconds32(Year, Month, Day, DayFraction, LeapSecondValue)
            End If

            Return RetCode
        End Function

        Private Function NumberOfBuiltInLeapSecondValues() As Integer
            Dim RetCode As Short

            If Is64Bit() Then
                RetCode = NumberOfBuiltInLeapSecondValues64()
            Else
                RetCode = NumberOfBuiltInLeapSecondValues32()
            End If

            Return RetCode
        End Function

#End Region

#Region "DLL Entry Points SOFA (32bit)"
        <DllImport(SOFA32DLL, EntryPoint:="iauAf2a")>
        Private Shared Function Af2a32(s As Char,
                                      ideg As Short,
                                      iamin As Short,
                                      asec As Double,
                                      ByRef rad As Double) As Short
        End Function

        <DllImport(SOFA32DLL, EntryPoint:="iauAnp")>
        Private Shared Function Anp32(a As Double) As Double
        End Function

        <DllImport(SOFA32DLL, EntryPoint:="iauAtci13")>
        Private Shared Sub Atci1332(rc As Double,
                                   dc As Double,
                                   pr As Double,
                                   pd As Double,
                                   px As Double,
                                   rv As Double,
                                   date1 As Double,
                                   date2 As Double,
                                   ByRef ri As Double,
                                   ByRef di As Double,
                                   ByRef eo As Double)
        End Sub

        <DllImport(SOFA32DLL, EntryPoint:="iauAtco13")>
        Private Shared Function Atco1332(rc As Double,
                                        dc As Double,
                                        pr As Double,
                                        pd As Double,
                                        px As Double,
                                        rv As Double,
                                        utc1 As Double,
                                        utc2 As Double,
                                        dut1 As Double,
                                        elong As Double,
                                        phi As Double,
                                        hm As Double,
                                        xp As Double,
                                        yp As Double,
                                        phpa As Double,
                                        tc As Double,
                                        rh As Double,
                                        wl As Double,
                                        ByRef aob As Double,
                                        ByRef zob As Double,
                                        ByRef hob As Double,
                                        ByRef dob As Double,
                                        ByRef rob As Double,
                                        ByRef eo As Double) As Short
        End Function

        <DllImport(SOFA32DLL, EntryPoint:="iauAtic13")>
        Private Shared Sub Atic1332(ri As Double,
                                   di As Double,
                                   date1 As Double,
                                   date2 As Double,
                                   ByRef rc As Double,
                                   ByRef dc As Double,
                                   ByRef eo As Double)
        End Sub

        <DllImport(SOFA32DLL, EntryPoint:="iauAtoc13")>
        Private Shared Function Atoc1332(type As String,
                                         ob1 As Double,
                                         ob2 As Double,
                                         utc1 As Double,
                                         utc2 As Double,
                                         dut1 As Double,
                                         elong As Double,
                                         phi As Double,
                                         hm As Double,
                                         xp As Double,
                                         yp As Double,
                                         phpa As Double,
                                         tc As Double,
                                         rh As Double,
                                         wl As Double,
                                         ByRef rc As Double,
                                         ByRef dc As Double) As Short
        End Function

        <DllImport(SOFA32DLL, EntryPoint:="iauAtio13")>
        Private Shared Function Atio1332(ri As Double,
                                         di As Double,
                                         utc1 As Double,
                                         utc2 As Double,
                                         dut1 As Double,
                                         elong As Double,
                                         phi As Double,
                                         hm As Double,
                                         xp As Double,
                                         yp As Double,
                                         phpa As Double,
                                         tc As Double,
                                         rh As Double,
                                         wl As Double,
                                         ByRef aob As Double,
                                         ByRef zob As Double,
                                         ByRef hob As Double,
                                         ByRef dob As Double,
                                         ByRef rob As Double) As Short
        End Function

        <DllImport(SOFA32DLL, EntryPoint:="iauAtoi13")>
        Private Shared Function Atoi1332(type As String,
                                         ob1 As Double,
                                         ob2 As Double,
                                         utc1 As Double,
                                         utc2 As Double,
                                         dut1 As Double,
                                         elong As Double,
                                         phi As Double,
                                         hm As Double,
                                         xp As Double,
                                         yp As Double,
                                         phpa As Double,
                                         tc As Double,
                                         rh As Double,
                                         wl As Double,
                                         ByRef ri As Double,
                                         ByRef di As Double) As Short
        End Function

        <DllImport(SOFA32DLL, EntryPoint:="iauDtf2d")>
        Private Shared Function Dtf2d32(scale As String,
                                       iy As Integer,
                                       im As Integer,
                                       id As Integer,
                                       ihr As Integer,
                                       imn As Integer,
                                       sec As Double,
                                       ByRef d1 As Double,
                                       ByRef d2 As Double) As Short
        End Function

        <DllImport(SOFA32DLL, EntryPoint:="iauEo06a")>
        Private Shared Function Eo06a32(date1 As Double,
                                       date2 As Double) As Double
        End Function

        <DllImport(SOFA32DLL, EntryPoint:="iauTaitt")>
        Private Shared Function Taitt32(tai1 As Double,
                                       tai2 As Double,
                                       ByRef tt1 As Double,
                                       ByRef tt2 As Double) As Short
        End Function

        <DllImport(SOFA32DLL, EntryPoint:="iauTttai")>
        Private Shared Function Tttai32(tt1 As Double,
                                       tt2 As Double,
                                       ByRef tai1 As Double,
                                       ByRef tai2 As Double) As Short
        End Function

        <DllImport(SOFA32DLL, EntryPoint:="iauTf2a")>
        Private Shared Function Tf2a32(s As Char,
                                      ihour As Short,
                                      imin As Short,
                                      sec As Double,
                                      ByRef rad As Double) As Short
        End Function

        <DllImport(SOFA32DLL, EntryPoint:="iauUtctai")>
        Private Shared Function Utctai32(utc1 As Double,
                                        utc2 As Double,
                                        ByRef tai1 As Double,
                                        ByRef tai2 As Double) As Short
        End Function

        <DllImport(SOFA32DLL, EntryPoint:="iauTaiutc")>
        Private Shared Function Taiutc32(tai1 As Double,
                                        tai2 As Double,
                                        ByRef utc1 As Double,
                                        ByRef utc2 As Double) As Short
        End Function

        <DllImport(SOFA32DLL, EntryPoint:="UpdateLeapSecondData")>
        Private Shared Function UpdateLeapSecondData32(arr() As LeapSecondDataStruct) As Short
        End Function

        <DllImport(SOFA32DLL, EntryPoint:="GetLeapSecondData")>
        Private Shared Function GetLeapSecondData32(<Out()> arr() As LeapSecondDataStruct, ByRef HasUpdatedData As Integer) As Short
        End Function

        <DllImport(SOFA32DLL, EntryPoint:="GetBuiltInLeapSecondData")>
        Private Shared Function GetLeapSecondData32(<Out()> arr() As LeapSecondDataStruct) As Short
        End Function

        <DllImport(SOFA32DLL, EntryPoint:="UsingUpdatedData")>
        Private Shared Function UsingUpdatedData32() As Short
        End Function

        <DllImport(SOFA32DLL, EntryPoint:="iauDat")> 'int iauDat(int iy, int im, int id, double fd, double *deltat)
        Private Shared Function LeapSeconds32(Year As Integer,
                                              Month As Integer,
                                              Day As Integer,
                                              DayFraction As Double,
                                              ByRef ReturnedLeapSeconds As Double) As Short
        End Function

        <DllImport(SOFA32DLL, EntryPoint:="NumberOfBuiltInLeapSecondValues")>
        Private Shared Function NumberOfBuiltInLeapSecondValues32() As Short
        End Function
        <DllImport(SOFA32DLL, EntryPoint:="iauHd2ae")>
        Private Shared Sub Hd2ae32(ha As Double,
                                   dec As Double,
                                   phi As Double,
                                   ByRef az As Double,
                                   ByRef el As Double)
        End Sub

        <DllImport(SOFA32DLL, EntryPoint:="iauS2pv")>
        Private Shared Sub S2pv32(theta As Double,
                                phi As Double,
                                r As Double,
                                td As Double,
                                pd As Double,
                                rd As Double,
                                pv As Double(,))
        End Sub

        <DllImport(SOFA32DLL, EntryPoint:="iauIr")>
        Private Shared Sub Ir32(r As Double(,))
        End Sub

        <DllImport(SOFA32DLL, EntryPoint:="iauRy")>
        Private Shared Sub Ry32(theta As Double,
                              r As Double(,))
        End Sub

        <DllImport(SOFA32DLL, EntryPoint:="iauRxpv")>
        Private Shared Sub Rxpv32(r As Double(,),
                                pv As Double(,),
                                rpv As Double(,))
        End Sub

        <DllImport(SOFA32DLL, EntryPoint:="iauPv2s")>
        Private Shared Sub Pv2s32(pv As Double(,),
                                ByRef theta As Double,
                                ByRef phi As Double,
                                ByRef r As Double,
                                ByRef td As Double,
                                ByRef pd As Double,
                                ByRef rd As Double)
        End Sub

#End Region

#Region "DLL Entry Points SOFA (64bit)"
        <DllImport(SOFA64DLL, EntryPoint:="iauAf2a")>
        Private Shared Function Af2a64(s As Char,
                                      ideg As Short,
                                      iamin As Short,
                                      asec As Double,
                                      ByRef rad As Double) As Short
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="iauAnp")>
        Private Shared Function Anp64(a As Double) As Double
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="iauAtci13")>
        Private Shared Sub Atci1364(rc As Double,
                                   dc As Double,
                                   pr As Double,
                                   pd As Double,
                                   px As Double,
                                   rv As Double,
                                   date1 As Double,
                                   date2 As Double,
                                   ByRef ri As Double,
                                   ByRef di As Double,
                                   ByRef eo As Double)
        End Sub

        <DllImport(SOFA64DLL, EntryPoint:="iauAtco13")>
        Private Shared Function Atco1364(rc As Double,
                                        dc As Double,
                                        pr As Double,
                                        pd As Double,
                                        px As Double,
                                        rv As Double,
                                        utc1 As Double,
                                        utc2 As Double,
                                        dut1 As Double,
                                        elong As Double,
                                        phi As Double,
                                        hm As Double,
                                        xp As Double,
                                        yp As Double,
                                        phpa As Double,
                                        tc As Double,
                                        rh As Double,
                                        wl As Double,
                                        ByRef aob As Double,
                                        ByRef zob As Double,
                                        ByRef hob As Double,
                                        ByRef dob As Double,
                                        ByRef rob As Double,
                                        ByRef eo As Double) As Short
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="iauAtic13")>
        Private Shared Sub Atic1364(ri As Double,
                                   di As Double,
                                   date1 As Double,
                                   date2 As Double,
                                   ByRef rc As Double,
                                   ByRef dc As Double,
                                   ByRef eo As Double)
        End Sub

        <DllImport(SOFA64DLL, EntryPoint:="iauAtoc13")>
        Private Shared Function Atoc1364(type As String,
                                         ob1 As Double,
                                         ob2 As Double,
                                         utc1 As Double,
                                         utc2 As Double,
                                         dut1 As Double,
                                         elong As Double,
                                         phi As Double,
                                         hm As Double,
                                         xp As Double,
                                         yp As Double,
                                         phpa As Double,
                                         tc As Double,
                                         rh As Double,
                                         wl As Double,
                                         ByRef rc As Double,
                                         ByRef dc As Double) As Short
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="iauAtio13")>
        Private Shared Function Atio1364(ri As Double,
                                         di As Double,
                                         utc1 As Double,
                                         utc2 As Double,
                                         dut1 As Double,
                                         elong As Double,
                                         phi As Double,
                                         hm As Double,
                                         xp As Double,
                                         yp As Double,
                                         phpa As Double,
                                         tc As Double,
                                         rh As Double,
                                         wl As Double,
                                         ByRef aob As Double,
                                         ByRef zob As Double,
                                         ByRef hob As Double,
                                         ByRef dob As Double,
                                         ByRef rob As Double) As Short
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="iauAtoi13")>
        Private Shared Function Atoi1364(type As String,
                                         ob1 As Double,
                                         ob2 As Double,
                                         utc1 As Double,
                                         utc2 As Double,
                                         dut1 As Double,
                                         elong As Double,
                                         phi As Double,
                                         hm As Double,
                                         xp As Double,
                                         yp As Double,
                                         phpa As Double,
                                         tc As Double,
                                         rh As Double,
                                         wl As Double,
                                         ByRef ri As Double,
                                         ByRef di As Double) As Short
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="iauDtf2d")>
        Private Shared Function Dtf2d64(scale As String,
                                       iy As Integer,
                                       im As Integer,
                                       id As Integer,
                                       ihr As Integer,
                                       imn As Integer,
                                       sec As Double,
                                       ByRef d1 As Double,
                                       ByRef d2 As Double) As Short
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="iauEo06a")>
        Private Shared Function Eo06a64(date1 As Double,
                                       date2 As Double) As Double
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="iauTaitt")>
        Private Shared Function Taitt64(tai1 As Double,
                                       tai2 As Double,
                                       ByRef tt1 As Double,
                                       ByRef tt2 As Double) As Short
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="iauTttai")>
        Private Shared Function Tttai64(tt1 As Double,
                                       tt2 As Double,
                                       ByRef tai1 As Double,
                                       ByRef tai2 As Double) As Short
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="iauTf2a")>
        Private Shared Function Tf2a64(s As Char,
                                      ihour As Short,
                                      imin As Short,
                                      sec As Double,
                                      ByRef rad As Double) As Short
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="iauUtctai")>
        Private Shared Function Utctai64(utc1 As Double,
                                        utc2 As Double,
                                        ByRef tai1 As Double,
                                        ByRef tai2 As Double) As Short
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="iauTaiutc")>
        Private Shared Function Taiutc64(tai1 As Double,
                                        tai2 As Double,
                                        ByRef utc1 As Double,
                                        ByRef utc2 As Double) As Short
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="UpdateLeapSecondData")>
        Private Shared Function UpdateLeapSecondData64(arr() As LeapSecondDataStruct) As Short
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="GetLeapSecondData")>
        Private Shared Function GetLeapSecondData64(<Out()> arr() As LeapSecondDataStruct, ByRef HasUpdatedData As Integer) As Short
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="GetBuiltInLeapSecondData")>
        Private Shared Function GetLeapSecondData64(<Out()> arr() As LeapSecondDataStruct) As Short
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="UsingUpdatedData")>
        Private Shared Function UsingUpdatedData64() As Short
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="iauDat")>
        Private Shared Function LeapSeconds64(Year As Integer,
                                              Month As Integer,
                                              Day As Integer,
                                              DayFraction As Double,
                                              ByRef ReturnedLeapSeconds As Double) As Short
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="NumberOfBuiltInLeapSecondValues")>
        Private Shared Function NumberOfBuiltInLeapSecondValues64() As Short
        End Function

        <DllImport(SOFA64DLL, EntryPoint:="iauHd2ae")>
        Private Shared Sub Hd2ae64(ha As Double,
                                   dec As Double,
                                   phi As Double,
                                   ByRef az As Double,
                                   ByRef el As Double)
        End Sub

        <DllImport(SOFA64DLL, EntryPoint:="iauS2pv")>
        Private Shared Sub S2pv64(theta As Double,
                                phi As Double,
                                r As Double,
                                td As Double,
                                pd As Double,
                                rd As Double,
                                pv As Double(,))
        End Sub

        <DllImport(SOFA64DLL, EntryPoint:="iauIr")>
        Private Shared Sub Ir64(r As Double(,))
        End Sub

        <DllImport(SOFA64DLL, EntryPoint:="iauRy")>
        Private Shared Sub Ry64(theta As Double,
                              r As Double(,))
        End Sub

        <DllImport(SOFA64DLL, EntryPoint:="iauRxpv")>
        Private Shared Sub Rxpv64(r As Double(,),
                                pv As Double(,),
                                rpv As Double(,))
        End Sub

        <DllImport(SOFA64DLL, EntryPoint:="iauPv2s")>
        Private Shared Sub Pv2s64(pv As Double(,),
                                ByRef theta As Double,
                                ByRef phi As Double,
                                ByRef r As Double,
                                ByRef td As Double,
                                ByRef pd As Double,
                                ByRef rd As Double)
        End Sub

#End Region

#Region "Private Support Code"
        'Constants for SHGetSpecialFolderPath shell call
        Private Const CSIDL_PROGRAM_FILES As Integer = 38 '0x0026
        Private Const CSIDL_PROGRAM_FILESX86 As Integer = 42 '0x002a,
        Private Const CSIDL_WINDOWS As Integer = 36 ' 0x0024,
        Private Const CSIDL_PROGRAM_FILES_COMMONX86 As Integer = 44 ' 0x002c,

        'DLL to provide the path to Program Files(x86)\Common Files folder location that is not avialable through the .NET framework
        ''' <summary>
        ''' Get path to a system folder
        ''' </summary>
        ''' <param name="hwndOwner">SUpply null / nothing to use "current user"</param>
        ''' <param name="lpszPath">returned string folder path</param>
        ''' <param name="nFolder">Folder Number from CSIDL enumeration e.g. CSIDL_PROGRAM_FILES_COMMONX86 = 44 = 0x2c</param>
        ''' <param name="fCreate">Indicates whether the folder should be created if it does not already exist. If this value is nonzero, 
        ''' the folder is created. If this value is zero, the folder is not created</param>
        ''' <returns>TRUE if successful; otherwise, FALSE.</returns>
        ''' <remarks></remarks>
        <DllImport("shell32.dll")>
        Private Shared Function SHGetSpecialFolderPath(ByVal hwndOwner As IntPtr,
                                               <Out()> ByVal lpszPath As System.Text.StringBuilder,
                                               ByVal nFolder As Integer,
                                               ByVal fCreate As Boolean) As Boolean
        End Function

        ''' <summary>
        ''' Loads a library DLL
        ''' </summary>
        ''' <param name="lpFileName">Full path to the file to load</param>
        ''' <returns>A pointer to the loaded DLL image</returns>
        ''' <remarks>This is a wrapper for the Windows kernel32 function LoadLibraryA</remarks>
        <DllImport("kernel32.dll", SetLastError:=True, EntryPoint:="LoadLibraryA")>
        Private Shared Function LoadLibrary(ByVal lpFileName As String) As IntPtr
        End Function

        ''' <summary>
        ''' Unloads a library DLL
        ''' </summary>
        ''' <param name="hModule">Pointer to the loaded library returned by the LoadLibrary function.</param>
        ''' <returns>True or false depending on whether the library was released.</returns>
        ''' <remarks></remarks>
        <DllImport("kernel32.dll", SetLastError:=True, EntryPoint:="FreeLibrary")>
        Private Shared Function FreeLibrary(ByVal hModule As IntPtr) As Boolean
        End Function

        ''' <summary>
        ''' Indicates whether we are running as a 32bit or 64bit application 
        ''' </summary>
        ''' <returns>True if the application is 64bit, False for 32bit</returns>
        ''' <remarks></remarks>
        Private Shared Function Is64Bit() As Boolean
            If IntPtr.Size = 8 Then 'Check whether we are running on a 32 or 64bit system.
                Return True
            Else
                Return False
            End If
        End Function

#End Region

#Region "Static Methods"

        ' Get the built-in list of leap seconds from the SOFA DLL, which is the master data for the whole Platform
        ' This method is static so that it can be called without having to fully initialise the SOFA DLL which uses an instance of the EarthRotationParameters 
        ' object leading to a circular reference of EarthRotationParameters calling SOFA, which calls EarthRotationParameters and so on ad infinitum.
        Friend Shared Function BuiltInLeapSeconds() As SortedList(Of Double, Double)
            Dim LeapSecondArray(100) As LeapSecondDataStruct, LeapSecondList As SortedList(Of Double, Double)
            Dim NumberOfRecords As Short, UpdatedDataInt As Integer, JulianDate As Double

            LeapSecondList = New SortedList(Of Double, Double)
            Try

                If Is64Bit() Then ' Call the appropriate 32 or 64 bit DLL depending on the application bitness.
                    NumberOfRecords = GetLeapSecondData64(LeapSecondArray, UpdatedDataInt)
                Else
                    NumberOfRecords = GetLeapSecondData32(LeapSecondArray, UpdatedDataInt)
                End If
                Debug.Print("BuiltInLeapSeconds received records: " & NumberOfRecords)

                ' Process the received LeapSecondDataStruct records and save them into a sorted list of juliandate:leapsecond pairs
                For i As Integer = 0 To NumberOfRecords - 1
                    ' Create a Julian date from the supplied year, month, date
                    JulianDate = New DateTime(LeapSecondArray(i).Year, LeapSecondArray(i).Month, 1).ToOADate + OLE_AUTOMATION_JULIAN_DATE_OFFSET
                    LeapSecondList.Add(JulianDate, LeapSecondArray(i).DelAt)
                Next
            Catch ex As Exception
                Debug.Print("")
            End Try

            Return LeapSecondList

        End Function
#End Region

    End Class

End Namespace