Imports System
Imports System.Xml.Serialization
Imports System.Collections.Generic
Imports System.Runtime.InteropServices

''' <summary>
''' Class that represents a whole device Profile and which contains a set of methods for for manipulating its contents
''' </summary>
''' <remarks>
''' This class is used by the Profile.GetProfile and Profile.SetProfile methods, do not confuse it with the Profile Class itself.
''' </remarks>
<ComVisible(True), Guid("43325B3A-8B34-48db-8028-9D8CED9FA9E2"), ClassInterface(ClassInterfaceType.None)> _
Public Class ASCOMProfile
    Implements IXmlSerializable

    Private Subkey As SortedList(Of String, SortedList(Of String, String))

#Region "New and IDisposable"
    Sub New()
        Subkey = New SortedList(Of String, SortedList(Of String, String))
        'Subkey.Add("", New SortedList(Of String, String)) 'Set the default value to unset
        'Subkey("")("") = ""
    End Sub
#End Region

    ''' <summary>
    ''' Add a new subkey
    ''' </summary>
    ''' <param name="SubKeyName">Name of the subkey</param>
    ''' <remarks></remarks>
    Public Sub AddSubkey(ByVal SubKeyName As String)
        Try
            Subkey.Add(SubKeyName, New SortedList(Of String, String))
            SetValue(SubKeyName, "", "") 'Set the default value to uninitialised
        Catch ex As System.ArgumentException 'Ignore this exception which occurs when the subkey has already been added
        End Try
    End Sub

    ''' <summary>
    ''' Clears all contents
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Clear()
        Subkey.Clear()
    End Sub

    ''' <summary>
    ''' Retrieve a registry value
    ''' </summary>
    ''' <param name="Name">Name of the value</param>
    ''' <param name="SubKeyName">"SubKey in which the value is located</param>
    ''' <returns>String value</returns>
    ''' <remarks></remarks>
    Public Overloads Function GetValue(ByVal Name As String, ByVal SubKeyName As String) As String
        Return Subkey(SubKeyName).Item(Name)
    End Function

    ''' <summary>
    ''' Removes a complete subkey
    ''' </summary>
    ''' <param name="SubKeyName">Subkey to be removed</param>
    ''' <remarks></remarks>
    Public Sub RemoveSubKey(ByVal SubKeyName As String)
        Try
            Subkey.Remove(SubKeyName)
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Remove a value
    ''' </summary>
    ''' <param name="ValueName">Name of the value to be removed</param>
    ''' <param name="SubKeyName">"SubKey containing the value</param>
    ''' <remarks></remarks>
    Public Overloads Sub RemoveValue(ByVal ValueName As String, ByVal SubKeyName As String)
        If ValueName <> "" Then
            Try
                Subkey(SubKeyName).Remove(ValueName) 'Catch exception oif they value doesn't exist
            Catch ex As Exception
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Set a value
    ''' </summary>
    ''' <param name="Name">Name of the value to set</param>
    ''' <param name="Value">Value to be set</param>
    ''' <param name="SubKeyName">"Subkey continaining the value</param>
    ''' <remarks>Changing a value with this method does NOT change the underlying profile store, only the value in this class.
    ''' In order to persist the new value, the class should be written back to the profile store through Profile.SetProfile.</remarks>
    Public Overloads Sub SetValue(ByVal Name As String, ByVal Value As String, ByVal SubKeyName As String)

        If Subkey.ContainsKey(SubKeyName) Then
            Subkey(SubKeyName).Item(Name) = Value
        Else
            Subkey.Add(SubKeyName, New SortedList(Of String, String))
            Subkey(SubKeyName).Add("", "") 'Set the default value to unset
            Subkey(SubKeyName)(Name) = Value
        End If
    End Sub

#Region ".NET only methods"
    ''' <summary>
    ''' Get the collection of values in this profile
    ''' </summary>
    ''' <value>All values in the collection</value>
    ''' <returns>SortedList(Of SubKey as String, SortedList(Of ValueName as String, Value as String))</returns>
    ''' <remarks></remarks>
    <ComVisible(False)> _
        Public ReadOnly Property ProfileValues() As SortedList(Of String, SortedList(Of String, String))
        Get
            Return Subkey
        End Get
    End Property

    ''' <summary>
    ''' Retrieve a registry value from the driver top level subkey
    ''' </summary>
    ''' <param name="Name">Name of the value </param>
    ''' <returns>String value</returns>
    ''' <remarks></remarks>
    <ComVisible(False)> _
    Public Overloads Function GetValue(ByVal Name As String) As String
        Return GetValue(Name, "")
    End Function

    ''' <summary>
    ''' Remove a value from the driver top level subkey
    ''' </summary>
    ''' <param name="ValueName">Name of the value to be removed</param>
    ''' <remarks></remarks>
    <ComVisible(False)> _
    Public Overloads Sub RemoveValue(ByVal ValueName As String)
        RemoveValue(ValueName, "")
    End Sub

    ''' <summary>
    ''' Set a value in the driver top level subkey
    ''' </summary>
    ''' <param name="Name">Name of the value to set</param>
    ''' <param name="Value">Value to be set</param>
    ''' <remarks></remarks>
    <ComVisible(False)> _
     Public Overloads Sub SetValue(ByVal Name As String, ByVal Value As String)
        SetValue(Name, Value, "")
    End Sub
#End Region

#Region "IXMLSerialisable Implementation"
    <System.ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)> _
    Public Function GetSchema() As System.Xml.Schema.XmlSchema Implements System.Xml.Serialization.IXmlSerializable.GetSchema
        Return (Nothing)
    End Function

    <System.ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)> _
    Public Sub ReadXml(ByVal reader As System.Xml.XmlReader) Implements System.Xml.Serialization.IXmlSerializable.ReadXml
        Dim CurrentSubKey As String = "", CurrentName As String = ""
        Subkey.Clear() 'Make sure we are starting with an empty collection in case the user has already played with this object
        Do While reader.Read() 'Read the xml stream
            Select Case reader.Name 'Determine what to do based on the element name
                Case XML_SUBKEYNAME_ELEMENTNAME 'This is a subkey element so get itand save for future use when adding values
                    CurrentSubKey = reader.ReadString
                    Subkey.Add(CurrentSubKey, New SortedList(Of String, String)) 'Create a new 
                Case XML_DEFAULTVALUE_ELEMENTNAME ' Default element value so add this to the collection
                    Subkey(CurrentSubKey).Add("", reader.ReadString) 'Set the default value to unset
                Case XML_NAME_ELEMENTNAME
                    CurrentName = reader.ReadString 'This is a value name so save it for when we get a value element
                Case XML_DATA_ELEMENTNAME
                    Subkey(CurrentSubKey).Add(CurrentName, reader.ReadString) 'This is a value element so add it using the saved subkey and name
                Case Else ' Ignore
            End Select
        Loop
    End Sub

    <System.ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)> _
    Public Sub WriteXml(ByVal writer As System.Xml.XmlWriter) Implements System.Xml.Serialization.IXmlSerializable.WriteXml
        For Each key As String In Subkey.Keys
            writer.WriteStartElement(XML_SUBKEY_ELEMENTNAME) 'Start the SubKey element
            writer.WriteElementString(XML_SUBKEYNAME_ELEMENTNAME, key) 'Write the subkey name
            writer.WriteElementString(XML_DEFAULTVALUE_ELEMENTNAME, Subkey(key).Item("")) 'Write the default value
            writer.WriteStartElement(XML_VALUES_ELEMENTNAME) 'Start the values element
            For Each kvp As Generic.KeyValuePair(Of String, String) In Subkey(key) 'Write name values pairs except for default value
                If Not String.IsNullOrEmpty(kvp.Key) Then
                    writer.WriteStartElement(XML_VALUE_ELEMENTNAME)
                    writer.WriteElementString(XML_NAME_ELEMENTNAME, kvp.Key)
                    writer.WriteElementString(XML_DATA_ELEMENTNAME, kvp.Value)
                    writer.WriteEndElement()
                End If
            Next
            writer.WriteEndElement() 'Close the values element
            writer.WriteEndElement() 'Close the subkey element
        Next
    End Sub
#End Region

End Class

