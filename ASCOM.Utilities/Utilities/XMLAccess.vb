'Class to read and write profile values in an XML format

Imports System.Xml
Imports System.IO
Imports System.Text
Imports Microsoft.Win32
Imports System.Collections
Imports ASCOM.Utilities.Interfaces
Imports ASCOM.Utilities.Exceptions

Friend Class XMLAccess
    Implements IAccess, IDisposable

    Private Const COLLECTION_DEFAULT_VALUE_NAME As String = "***** DefaultValueName *****" 'Name identifier label
    Private Const COLLECTION_DEFAULT_UNSET_VALUE As String = "===== ***** UnsetValue ***** =====" 'Value identifier label
    Private Const VALUES_FILENAME As String = "Profile.xml" 'Name of file to contain profile xml information
    Private Const VALUES_FILENAME_ORIGINAL As String = "ProfileOriginal.xml" 'Name of file to contain original profile xml information
    Private Const VALUES_FILENAME_NEW As String = "ProfileNew.xml" 'Name of file to contain original profile xml information
    Private Const PROFILE_NAME As String = "Profile" 'Name of top level XML element

    Private Const DEFAULT_ELEMENT_NAME As String = "DefaultElement" 'Default value label
    Private Const VALUE_ELEMENT_NAME As String = "Element" 'Profile value element name
    Private Const NAME_ATTRIBUTE_NAME As String = "Name" 'Profile value name attribute
    Private Const VALUE_ATTRIBUTE_NAME As String = "Value" 'Profile element value attribute

    Private Const ROOT_KEY_NAME As String = "Software\ASCOM" 'Location of ASCOM profile in HKLM registry hive

    Private FileStore As IFileStoreProvider 'File store containing the new ASCOM XML profile 
    Private disposedValue As Boolean = False        ' To detect redundant calls to IDisposable

    Private KeyCache As Generic.Dictionary(Of String, Generic.SortedList(Of String, String))
    Private TL As TraceLogger

    Private sw, swSupport As Stopwatch

#Region "New and IDisposable Support"
    Public Sub New()
        Dim PlatformVersion As String
        Dim FromKey As RegistryKey
        KeyCache = New Generic.Dictionary(Of String, Generic.SortedList(Of String, String))

        TL = New TraceLogger("", "XMLAccess") 'Create a new trace logger
        TL.Enabled = GetBool(TRACE_XMLACCESS, TRACE_XMLACCESS_DEFAULT) 'Get enabled / disabled state from the user registry
        RunningVersions(TL) ' Include version information

        sw = New Stopwatch 'Create the stowatch instances
        swSupport = New Stopwatch

        FileStore = New AllUsersFileSystemProvider
        'FileStore = New IsolatedStorageFileStoreProvider
        Try
            If Not FileStore.Exists("\" & VALUES_FILENAME) Then Throw New Exceptions.ProfileNotFoundException("Utilities Error Base key does not exist")
            PlatformVersion = GetProfile("\", "PlatformVersion")
            'OK, no exception so assume that we are initialised
        Catch ex As ProfileNotFoundException
            MsgBox("Migrating ASCOM Profiles from registry to file system" & vbCrLf & vbCrLf & "You should only see this message once, if not, please post on Yahoo group ASCOM-Talk", , "ASCOM Utilities")
            'It doesn't exist so migrate
            TL.LogMessage("New", "ProfileNotFoundException - migrating keys")
            'Create the root directory if it doesn't already exist
            If Not FileStore.Exists("\" & VALUES_FILENAME) Then
                FileStore.CreateDirectory("\")
                CreateKey("\") 'Create the root key
            End If

            FromKey = Registry.LocalMachine.OpenSubKey(ROOT_KEY_NAME) 'Source to copy from 
            MigrateKey(FromKey, "") 'Use recursion to copy contents to new tree
            FromKey.Close()
        Catch ex As Exception
            TL.LogMessage("XMLAccess.New Unexpected exception:", ex.ToString)
            MsgBox("XMLAccess.New Unexpected exception: " & ex.ToString)
        End Try
    End Sub

    Sub New(ByVal p_CallingComponent As String)
        Me.New()
    End Sub

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            Try
                FileStore = Nothing 'Clean up the filestore and keycache
                KeyCache = Nothing
                TL.Enabled = False 'Clean up the logger
                TL.Dispose()
                TL = Nothing
                sw.Stop() 'Clean up the stopwatches
                sw = Nothing
                swSupport.Stop()
                swSupport = Nothing

            Catch ex As Exception
                MsgBox("XMLAccess:Dispose Exception - " & ex.ToString)
            End Try
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

#End Region

#Region "IAccess Implementation"

    Friend Sub CreateKey(ByVal p_SubKeyName As String) Implements IAccess.CreateKey
        'Create a key
        Dim InitalValues As New Generic.SortedList(Of String, String)
        Dim SubKeys(), SubKey As String
        Dim i, j As Integer

        sw.Reset() : sw.Start() 'Start timing this call
        TL.LogMessage("CreateKey", "SubKey: """ & p_SubKeyName & """")

        p_SubKeyName = Trim(p_SubKeyName) 'Normalise the string:
        SubKeys = Split(p_SubKeyName, "\", , Microsoft.VisualBasic.CompareMethod.Text) 'Parse p_SubKeyName into its elements
        Select Case p_SubKeyName
            Case ""
                'Null path so do nothing
            Case "\"
                'Root node so just create this
                'Test whether the key already exists
                If Not FileStore.Exists("\" & VALUES_FILENAME) Then
                    TL.LogMessage("  CreateKey", "  Creating root key ""\""")
                    InitalValues.Clear() 'Now add the file containing the contents of the key
                    InitalValues.Add(COLLECTION_DEFAULT_VALUE_NAME, COLLECTION_DEFAULT_UNSET_VALUE)
                    WriteValues("\", InitalValues, False) 'Write the profile file, don't check if it already exists
                Else
                    TL.LogMessage("  CreateKey", "  Root key alread exists")
                End If
            Case Else
                'Create the directory and its intermediate directories
                For i = 0 To SubKeys.Length - 1
                    SubKey = ""
                    For j = 0 To i
                        SubKey = SubKey & "\" & SubKeys(j)
                    Next
                    'Logger.'LogMsg("    CreateKey", "    SubKey " & i.ToString & " " & SubKey)
                    'Test whether the key already exists
                    If Not FileStore.Exists(SubKey & "\" & VALUES_FILENAME) Then
                        '   Logger.'LogMsg("  CreateKey", "  Creating key """ & SubKey & """")
                        FileStore.CreateDirectory(SubKey)  'It doesn't exist so create it
                        InitalValues.Clear() 'Now add the file containing the contents of the key
                        InitalValues.Add(COLLECTION_DEFAULT_VALUE_NAME, COLLECTION_DEFAULT_UNSET_VALUE)
                        WriteValues(SubKey, InitalValues, False) 'Write the profile file
                    Else
                        '  Logger.'LogMsg("  CreateKey", "  Key exists   """ & SubKey & """")
                    End If
                Next
        End Select
        sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")

    End Sub

    Friend Sub DeleteKey(ByVal p_SubKeyName As String) Implements IAccess.DeleteKey
        'Delete a key

        sw.Reset() : sw.Start() 'Start timing this call
        TL.LogMessage("DeleteKey", "SubKey: """ & p_SubKeyName & """")

        Try : FileStore.DeleteDirectory(p_SubKeyName) : Catch ex As Exception : End Try
        Try : KeyCache.Remove(p_SubKeyName) : Catch : End Try
        'MsgBox("Removed key " & p_SubKeyName)
        sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")
    End Sub

    Sub RenameKey(ByVal CurrentSubKeyName As String, ByVal NewSubKeyName As String) Implements IAccess.RenameKey
        sw.Reset() : sw.Start() 'Start timing this call
        TL.LogMessage("RenameKey", "Current SubKey: """ & CurrentSubKeyName & """" & " New SubKey: """ & NewSubKeyName & """")
        FileStore.RenameDirectory(CurrentSubKeyName, NewSubKeyName)
        Try : KeyCache.Remove(CurrentSubKeyName) : Catch : End Try
        'MsgBox("Removed key " & p_SubKeyName)
        sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")
    End Sub


    Friend Sub DeleteProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String) Implements IAccess.DeleteProfile
        'Delete a value from a key
        Dim Values As Generic.SortedList(Of String, String)

        sw.Reset() : sw.Start() 'Start timing this call
        TL.LogMessage("DeleteProfile", "SubKey: """ & p_SubKeyName & """ Name: """ & p_ValueName & """")

        Values = ReadValues(p_SubKeyName) 'Read current contents of key
        Try 'Remove value if it exists
            If p_ValueName = "" Then 'Just set the default name to the unset value
                Values(COLLECTION_DEFAULT_VALUE_NAME) = COLLECTION_DEFAULT_UNSET_VALUE
                TL.LogMessage("DeleteProfile", "  Default name was changed to unset value")
            Else 'Actually delete the value
                Values.Remove(p_ValueName)
                TL.LogMessage("DeleteProfile", "  Value was deleted")
            End If
        Catch
            TL.LogMessage("DeleteProfile", "  Value did not exist")
        End Try
        WriteValues(p_SubKeyName, Values) 'Write the new list of values back to the key
        Values = Nothing
        sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")
    End Sub

    Friend Function EnumKeys(ByVal p_SubKeyName As String) As Collections.Generic.SortedList(Of String, String) Implements IAccess.EnumKeys
        'Return a sorted list of subkeys
        Dim Values As Generic.SortedList(Of String, String)
        Dim RetValues As New Generic.SortedList(Of String, String)
        Dim Directories(), DefaultValue As String

        sw.Reset() : sw.Start() 'Start timing this call
        TL.LogMessage("EnumKeys", "SubKey: """ & p_SubKeyName & """")

        Directories = FileStore.GetDirectoryNames(p_SubKeyName) 'Get a list of the keys
        For Each Directory As String In Directories 'Process each key in trun
            Try 'If there is an error reading the data don't include in the returned list
                Values = ReadValues(p_SubKeyName & "\" & Directory) 'Read the values of this key to find the default value
                DefaultValue = Values.Item(COLLECTION_DEFAULT_VALUE_NAME) 'Save the default value
                If DefaultValue = COLLECTION_DEFAULT_UNSET_VALUE Then DefaultValue = ""
                RetValues.Add(Directory, DefaultValue) 'Add the directory name and default value to the hashtable
            Catch
            End Try
            Values = Nothing
        Next
        sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")
        Return RetValues
    End Function

    Friend Function EnumProfile(ByVal p_SubKeyName As String) As Generic.SortedList(Of String, String) Implements IAccess.EnumProfile
        'Returns a sorted list of key values
        Dim Values As Generic.SortedList(Of String, String)
        Dim RetValues As New Generic.SortedList(Of String, String)

        sw.Reset() : sw.Start() 'Start timing this call
        TL.LogMessage("EnumProfile", "SubKey: """ & p_SubKeyName & """")

        Values = ReadValues(p_SubKeyName) 'Read values from profile XML file
        For Each kvp As Generic.KeyValuePair(Of String, String) In Values 'Retrieve each key/value  pair in turn
            If kvp.Key = COLLECTION_DEFAULT_VALUE_NAME Then
                If kvp.Value = COLLECTION_DEFAULT_UNSET_VALUE Then
                    'Do nothing if the value is unset
                Else
                    RetValues.Add("", kvp.Value) 'Add any other value to the return value
                End If
            Else
                RetValues.Add(kvp.Key, kvp.Value)
            End If
        Next
        Values = Nothing
        sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")
        Return RetValues
    End Function

    Friend Function GetProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String) As String Implements IAccess.GetProfile
        'Read a single value from a key
        Dim Values As Generic.SortedList(Of String, String), RetVal As String

        sw.Reset() : sw.Start() 'Start timing this call
        TL.LogMessage("GetProfile", "SubKey: """ & p_SubKeyName & """ Name: """ & p_ValueName & """")

        RetVal = "" 'Initialise return value to null string
        Values = ReadValues(p_SubKeyName) 'Read in the key values
        Try
            If p_ValueName = "" Then 'Requested the default value
                RetVal = Values.Item(COLLECTION_DEFAULT_VALUE_NAME)
            Else 'Requested a particular value
                RetVal = Values.Item(p_ValueName)
            End If
        Catch : End Try 'Missing value generates an exception and should return a null string

        Values = Nothing
        sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")
        Return RetVal
    End Function

    Friend Sub WriteProfile(ByVal p_SubKeyName As String, ByVal p_ValueName As String, ByVal p_ValueData As String) Implements IAccess.WriteProfile
        'Write a single value to a key
        Dim Values As Generic.SortedList(Of String, String)

        sw.Reset() : sw.Start() 'Start timing this call
        TL.LogMessage("WriteProfile", "SubKey: """ & p_SubKeyName & """ Name: """ & p_ValueName & """ Value: """ & p_ValueData & """")

        'Check if the directory exists
        If Not FileStore.Exists(p_SubKeyName & "\" & VALUES_FILENAME) Then CreateKey(p_SubKeyName) 'Create the subkey if it doesn't already exist
        Values = ReadValues(p_SubKeyName) 'Read the key values

        If p_ValueName = "" Then 'Write the deault value
            If Values.ContainsKey(COLLECTION_DEFAULT_VALUE_NAME) Then 'Does exist so update it
                '        Logger.'LogMsg("  WriteProfile", "  Updating existing default value to #" & p_ValueData & "#")
                Values.Item(COLLECTION_DEFAULT_VALUE_NAME) = p_ValueData 'Update the existing value
            Else 'Doesn't exist so add it
                '       Logger.'LogMsg("  WriteProfile", "  Adding new default value")
                Values.Add(COLLECTION_DEFAULT_VALUE_NAME, p_ValueData) 'Add the new value
            End If
            '  Logger.'LogMsg("  WriteProfile", "  Writing updated values")

            WriteValues(p_SubKeyName, Values) 'Write the values back to the XML profile
        Else 'Write a named value
            ' Logger.'LogMsg("  WriteProfile", "  Updating existing value")
            If Values.ContainsKey(p_ValueName) Then Values.Remove(p_ValueName) 'Remove old value if it exists
            Values.Add(p_ValueName, p_ValueData) 'Add the new value
            WriteValues(p_SubKeyName, Values) 'Write the values back to the XML profile
        End If
        Values = Nothing
        sw.Stop() : TL.LogMessage("  ElapsedTime", "  " & sw.ElapsedMilliseconds & " milliseconds")
    End Sub
#End Region

#Region "Support Functions"

    Private Function ReadValues(ByVal p_SubKeyName As String) As Generic.SortedList(Of String, String)
        'Read all values in a key
        'SubKey has to be absolute from the profile store root
        Dim Retval As New Generic.SortedList(Of String, String)
        Dim ReaderSettings As XmlReaderSettings
        Dim LastElementName As String = ""
        Dim NextName As String = ""
        Dim ValueName As String = ""

        swSupport.Reset() : swSupport.Start() 'Start timing this call
        If Left(p_SubKeyName, 1) <> "\" Then p_SubKeyName = "\" & p_SubKeyName 'Condition to have leading \

        If KeyCache.ContainsKey(p_SubKeyName) Then
            swSupport.Stop()
            TL.LogMessage("  ReadValues", "  Found in cache  " & p_SubKeyName & " - " & swSupport.ElapsedMilliseconds & " milliseconds")
            Return KeyCache.Item(p_SubKeyName)
        Else
            TL.LogMessage("  ReadValues", "  Cache miss      " & p_SubKeyName)
        End If

        Try
            ReaderSettings = New XmlReaderSettings
            ReaderSettings.IgnoreWhitespace = True
            Using Reader As XmlReader = XmlReader.Create(FileStore.FullPath(p_SubKeyName & "\" & VALUES_FILENAME), ReaderSettings)
                Reader.Read() 'Get rid of the version string
                Reader.Read() 'Read in the Profile name tag

                'Start reading profile strings
                While Reader.Read()
                    Select Case Reader.NodeType
                        Case XmlNodeType.Element
                            Select Case Reader.Name
                                Case DEFAULT_ELEMENT_NAME 'Found default value
                                    Retval.Add(COLLECTION_DEFAULT_VALUE_NAME, Reader.GetAttribute(VALUE_ATTRIBUTE_NAME))
                                    TL.LogMessage("    ReadValues", "    found " & COLLECTION_DEFAULT_VALUE_NAME & " = " & Retval.Item(COLLECTION_DEFAULT_VALUE_NAME))
                                Case VALUE_ELEMENT_NAME 'Fount an element name
                                    ValueName = Reader.GetAttribute(NAME_ATTRIBUTE_NAME)
                                    Retval.Add(ValueName, Reader.GetAttribute(VALUE_ATTRIBUTE_NAME))
                                    TL.LogMessage("    ReadValues", "    found " & ValueName & " = " & Retval.Item(ValueName))
                                Case Else 'Do nothing
                                    TL.LogMessage("    ReadValues", "    ## Found unexpected Reader.Name: " & Reader.Name.ToString)
                            End Select
                        Case Else 'Do nothing
                    End Select
                End While

                Reader.Close() 'Close the IO readers
            End Using
            KeyCache.Add(p_SubKeyName, Retval) 'Add the new value to the cache
            swSupport.Stop()
            TL.LogMessage("  ReadValues", "  added to cache - " & swSupport.ElapsedMilliseconds & " milliseconds")
        Catch ex As Exception
            'MsgBox("XMLAccess:ReadValues " & p_SubKeyName & " " & ex.ToString)
            TL.LogMessage("  ReadValues", " exception: " & ex.ToString)
            Throw
        End Try
        Return Retval
    End Function

    Private Overloads Sub WriteValues(ByVal p_SubKeyName As String, ByRef p_KeyValuePairs As Generic.SortedList(Of String, String))
        'Make the general case check for existence of a current Profile.xml file. Most cases need this
        'The exception is the CreateKey where the Profile.xmldefinitlkey won't exist as we are about to create it for the first time
        WriteValues(p_SubKeyName, p_KeyValuePairs, True)
    End Sub

    Private Overloads Sub WriteValues(ByVal p_SubKeyName As String, ByRef p_KeyValuePairs As Generic.SortedList(Of String, String), ByVal p_CheckForCurrentProfileStore As Boolean)
        'Write  all key values to an XML file
        'SubKey has to be absolute from the profile store root
        Dim WriterSettings As XmlWriterSettings
        Dim FName As String
        Dim Ct As Integer

        swSupport.Reset() : swSupport.Start() 'Start timing this call
        TL.LogMessage("  WriteValues", "  SubKeyName: " & p_SubKeyName)
        If Left(p_SubKeyName, 1) <> "\" Then p_SubKeyName = "\" & p_SubKeyName

        Try
            Ct = 0
            For Each kvp As Generic.KeyValuePair(Of String, String) In p_KeyValuePairs
                Ct += 1
                TL.LogMessage("  WriteValues List", "  " & Ct.ToString & " " & kvp.Key & " = " & kvp.Value)
            Next

            WriterSettings = New XmlWriterSettings
            WriterSettings.Indent = True
            FName = FileStore.FullPath(p_SubKeyName & "\" & VALUES_FILENAME_NEW)
            Dim Writer As XmlWriter
            Writer = XmlWriter.Create(FName, WriterSettings)
            Using Writer
                Writer.WriteStartDocument()
                Writer.WriteStartElement(PROFILE_NAME) 'Write the profile element
                Writer.WriteStartElement(DEFAULT_ELEMENT_NAME) 'Write the default element
                Writer.WriteAttributeString(VALUE_ATTRIBUTE_NAME, p_KeyValuePairs.Item(COLLECTION_DEFAULT_VALUE_NAME)) 'Write the default value
                Writer.WriteEndElement()
                Ct = 0
                For Each kvp As Generic.KeyValuePair(Of String, String) In p_KeyValuePairs 'Write each named value in turn
                    Ct += 1
                    TL.LogMessage("  Writing Value", "  " & Ct.ToString & " " & kvp.Key & " = " & kvp.Value)
                    If kvp.Value Is Nothing Then TL.LogMessage("  Writing Value", "  WARNING - Suppplied Value is Nothing not empty string")
                    Select Case kvp.Key
                        Case COLLECTION_DEFAULT_VALUE_NAME ' Ignore the default value entry
                        Case Else 'Write everything else to the file
                            Writer.WriteStartElement(VALUE_ELEMENT_NAME) 'Write the element name
                            Writer.WriteAttributeString(NAME_ATTRIBUTE_NAME, kvp.Key) 'Write the name attribute
                            Writer.WriteAttributeString(VALUE_ATTRIBUTE_NAME, kvp.Value) 'Write the value attribute
                            Writer.WriteEndElement() 'Close this element
                    End Select
                Next
                Writer.WriteEndElement()

                'Flush and close the writer object to complete writing of the XML file. 
                Writer.Close() 'Actualy write the XML to a file
            End Using
            Writer = Nothing

            Try 'New file successfully created so now rename the current file to original and rename the new file to current
                If p_CheckForCurrentProfileStore Then 'Check for existence for current profile store if required
                    FileStore.Rename(p_SubKeyName & "\" & VALUES_FILENAME, p_SubKeyName & "\" & VALUES_FILENAME_ORIGINAL)
                End If
                Try
                    FileStore.Rename(p_SubKeyName & "\" & VALUES_FILENAME_NEW, p_SubKeyName & "\" & VALUES_FILENAME)
                Catch ex2 As Exception
                    'Attempt to rename new file as current failed so try and restore the original file
                    TL.LogMessage("XMLAccess:WriteValues", "Unable to rename new profile file to current - " & p_SubKeyName & "\" & VALUES_FILENAME_NEW & "to " & p_SubKeyName & "\" & VALUES_FILENAME & " " & ex2.ToString)
                    Try
                        FileStore.Rename(p_SubKeyName & "\" & VALUES_FILENAME_ORIGINAL, p_SubKeyName & "\" & VALUES_FILENAME)
                    Catch ex3 As Exception
                        'Restoration also failed so no clear recovery from this point
                        TL.LogMessage("XMLAccess:WriteValues", "Unable to rename original profile file to current - " & p_SubKeyName & "\" & VALUES_FILENAME_ORIGINAL & "to " & p_SubKeyName & "\" & VALUES_FILENAME & " " & ex3.ToString)
                    End Try
                End Try
            Catch ex1 As Exception
                'No clear remedial action as the current file rename failed so just leave as is
                TL.LogMessage("XMLAccess:WriteValues", "Unable to rename current profile file to original - " & p_SubKeyName & "\" & VALUES_FILENAME & "to " & p_SubKeyName & "\" & VALUES_FILENAME_ORIGINAL & " " & ex1.ToString)
            End Try

            WriterSettings = Nothing

            If KeyCache.ContainsKey(p_SubKeyName) Then
                KeyCache.Item(p_SubKeyName) = p_KeyValuePairs 'Update the local cache
                swSupport.Stop()
                TL.LogMessage("  WriteValues", "  Updated cache entry " & p_SubKeyName & " - " & swSupport.ElapsedMilliseconds & " milliseconds")
            Else
                KeyCache.Add(p_SubKeyName, p_KeyValuePairs) 'Add the new value
                swSupport.Stop()
                TL.LogMessage("  WriteValues", "  Created cache entry " & p_SubKeyName & " - " & swSupport.ElapsedMilliseconds & " milliseconds")
            End If
        Catch ex As Exception
            TL.LogMessage("  WriteValues", "  Exception " & p_SubKeyName & " " & ex.ToString)
            MsgBox("XMLAccess:Writevalues " & p_SubKeyName & " " & ex.ToString)
        End Try
    End Sub

    Private Sub MigrateKey(ByVal p_FromKey As RegistryKey, ByVal p_ToDir As String)
        'Subroutine used for one off copy of registry profile to new XML profile
        Dim ValueNames(), SubKeyNames() As String
        Dim FromKey As RegistryKey
        Dim Values As New Generic.SortedList(Of String, String)
        'Recusively copy contents from one key to the other
        Dim swLocal As Stopwatch

        Static RecurseDepth As Integer

        RecurseDepth += 1 'Increment the recursion depth indicator

        swLocal = Stopwatch.StartNew
        TL.LogMessage("MigrateKeys " & RecurseDepth.ToString, "Start")

        'First copy values from the from key to the to key
        ValueNames = p_FromKey.GetValueNames
        Values.Add(COLLECTION_DEFAULT_VALUE_NAME, COLLECTION_DEFAULT_UNSET_VALUE)
        For Each ValueName As String In ValueNames
            If ValueName = "" Then 'Deal with case where the customer has specified a value for the deault value
                Values.Remove(COLLECTION_DEFAULT_VALUE_NAME) 'Remove the default unset value and replace with actual value
                Values.Add(COLLECTION_DEFAULT_VALUE_NAME, p_FromKey.GetValue(ValueName).ToString)
            Else
                Values.Add(ValueName, p_FromKey.GetValue(ValueName).ToString)
            End If

        Next
        WriteValues(p_ToDir, Values) 'Write values to XML file

        'Now process the keys
        SubKeyNames = p_FromKey.GetSubKeyNames
        For Each SubKeyName As String In SubKeyNames
            FromKey = p_FromKey.OpenSubKey(SubKeyName) 'Point at the source to copy to it
            CreateKey(p_ToDir & "\" & SubKeyName)
            MigrateKey(FromKey, p_ToDir & "\" & SubKeyName) 'Recursively process each key
            FromKey.Close()
        Next
        swLocal.Stop() : TL.LogMessage("  ElapsedTime " & RecurseDepth.ToString, "  " & swLocal.ElapsedMilliseconds & " milliseconds")
        RecurseDepth -= 1 'Decrement the recursion depth counter
        swLocal = Nothing
    End Sub

#End Region

End Class