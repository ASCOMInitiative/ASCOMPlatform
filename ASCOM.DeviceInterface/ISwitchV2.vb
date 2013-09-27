﻿Imports System.Runtime.InteropServices
'-----------------------------------------------------------------------
' <summary>Defines the ISwitchV2 Interface</summary>
'-----------------------------------------------------------------------
''' <summary>
''' Defines the ISwitchV2 Interface
''' </summary>
<Guid("71A6CA6B-A86B-4EBB-8DA3-6D91705177A3"), ComVisible(True), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
Public Interface ISwitchV2

#Region "Common Methods"
    'IAscomDriver Methods

    ''' <summary>
    ''' Set True to connect to the device. Set False to disconnect from the device.
    ''' You can also read the property to check whether it is connected.
    ''' </summary>
    ''' <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p>Do not use a NotConnectedException here, that exception is for use in other methods that require a connection in order to succeed.</remarks>
    Property Connected() As Boolean

    ''' <summary>
    ''' Returns a description of the device, such as manufacturer and modelnumber. Any ASCII characters may be used. 
    ''' </summary>
    ''' <value>The description.</value>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
    ReadOnly Property Description() As String

    ''' <summary>
    ''' Descriptive and version information about this ASCOM driver.
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented</b></p> This string may contain line endings and may be hundreds to thousands of characters long.
    ''' It is intended to display detailed information on the ASCOM driver, including version and copyright data.
    ''' See the <see cref="Description" /> property for information on the device itself.
    ''' To get the driver version in a parseable string, use the <see cref="DriverVersion" /> property.
    ''' </remarks>
    ReadOnly Property DriverInfo() As String

    ''' <summary>
    ''' A string containing only the major and minor version of the driver.
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p> This must be in the form "n.n".
    ''' It should not to be confused with the <see cref="InterfaceVersion" /> property, which is the version of this specification supported by the 
    ''' driver.
    ''' </remarks>
    ReadOnly Property DriverVersion() As String

    ''' <summary>
    ''' The interface version number that this device supports. Should return 2 for this interface version.
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p> Clients can detect legacy V1 drivers by trying to read ths property.
    ''' If the driver raises an error, it is a V1 driver. V1 did not specify this property. A driver may also return a value of 1. 
    ''' In other words, a raised error or a return value of 1 indicates that the driver is a V1 driver.
    ''' </remarks>
    ReadOnly Property InterfaceVersion() As Short

    ''' <summary>
    ''' The short name of the driver, for display purposes
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
    ReadOnly Property Name() As String

    ''' <summary>
    ''' Launches a configuration dialog box for the driver.  The call will not return
    ''' until the user clicks OK or cancel manually.
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
    Sub SetupDialog()

    'DeviceControl Methods

    ''' <summary>
    ''' Invokes the specified device-specific action.
    ''' </summary>
    ''' <param name="ActionName">
    ''' A well known name agreed by interested parties that represents the action to be carried out. 
    ''' </param>
    ''' <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.
    ''' </param>
    ''' <returns>A string response. The meaning of returned strings is set by the driver author.</returns>
    ''' <exception cref="ASCOM.MethodNotImplementedException">Throws this exception if no actions are suported.</exception>
    ''' <exception cref="ASCOM.ActionNotImplementedException">It is intended that the SupportedActions method will inform clients 
    ''' of driver capabilities, but the driver must still throw an ASCOM.ActionNotImplemented exception if it is asked to 
    ''' perform an action that it does not support.</exception>
    ''' <exception cref="NotConnectedException">If the driver is not connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <example>Suppose filter wheels start to appear with automatic wheel changers; new actions could 
    ''' be “FilterWheel:QueryWheels” and “FilterWheel:SelectWheel”. The former returning a 
    ''' formatted list of wheel names and the second taking a wheel name and making the change, returning appropriate 
    ''' values to indicate success or failure.
    ''' </example>
    ''' <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> 
    ''' This method is intended for use in all current and future device types and to avoid name clashes, management of action names 
    ''' is important from day 1. A two-part naming convention will be adopted - <b>DeviceType:UniqueActionName</b> where:
    ''' <list type="bullet">
    ''' <item><description>DeviceType is the same value as would be used by <see cref="ASCOM.Utilities.Chooser.DeviceType"/> e.g. Telescope, Camera, Switch etc.</description></item>
    ''' <item><description>UniqueActionName is a single word, or multiple words joined by underscore characters, that sensibly describes the action to be performed.</description></item>
    ''' </list>
    ''' <para>
    ''' It is recommended that UniqueActionNames should be a maximum of 16 characters for legibility.
    ''' Should the same function and UniqueActionName be supported by more than one type of device, the reserved DeviceType of 
    ''' “General” will be used. Action names will be case insensitive, so FilterWheel:SelectWheel, filterwheel:selectwheel 
    ''' and FILTERWHEEL:SELECTWHEEL will all refer to the same action.</para>
    ''' <para>The names of all supported actions must bre returned in the <see cref="SupportedActions"/> property.</para>
    ''' </remarks>
    Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String

    ''' <summary>
    ''' Returns the list of action names supported by this driver.
    ''' </summary>
    ''' <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p> This method must return an empty arraylist if no actions are supported. Please do not throw a 
    ''' <see cref="ASCOM.PropertyNotImplementedException" />.
    ''' <para>This is an aid to client authors and testers who would otherwise have to repeatedly poll the driver to determine its capabilities. 
    ''' Returned action names may be in mixed case to enhance presentation but  will be recognised case insensitively in 
    ''' the <see cref="Action">Action</see> method.</para>
    '''<para>An array list collection has been selected as the vehicle for  action names in order to make it easier for clients to
    ''' determine whether a particular action is supported. This is easily done through the Contains method. Since the
    ''' collection is also ennumerable it is easy to use constructs such as For Each ... to operate on members without having to be concerned 
    ''' about hom many members are in the collection. </para>
    ''' <para>Collections have been used in the Telescope specification for a number of years and are known to be compatible with COM. Within .NET
    ''' the ArrayList is the correct implementation to use as the .NET Generic methods are not compatible with COM.</para>
    ''' </remarks>
    ReadOnly Property SupportedActions() As ArrayList

    ''' <summary>
    ''' Transmits an arbitrary string to the device and does not wait for a response.
    ''' Optionally, protocol framing characters may be added to the string before transmission.
    ''' </summary>
    ''' <param name="Command">The literal command string to be transmitted.</param>
    ''' <param name="Raw">
    ''' if set to <c>true</c> the string is transmitted 'as-is'.
    ''' If set to <c>false</c> then protocol framing characters may be added prior to transmission.
    ''' </param>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="NotConnectedException">If the driver is not connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> </remarks>
    Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False)

    ''' <summary>
    ''' Transmits an arbitrary string to the device and waits for a boolean response.
    ''' Optionally, protocol framing characters may be added to the string before transmission.
    ''' </summary>
    ''' <param name="Command">The literal command string to be transmitted.</param>
    ''' <param name="Raw">
    ''' if set to <c>true</c> the string is transmitted 'as-is'.
    ''' If set to <c>false</c> then protocol framing characters may be added prior to transmission.
    ''' </param>
    ''' <returns>
    ''' Returns the interpreted boolean response received from the device.
    ''' </returns>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="NotConnectedException">If the driver is not connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> </remarks>
    Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean

    ''' <summary>
    ''' Transmits an arbitrary string to the device and waits for a string response.
    ''' Optionally, protocol framing characters may be added to the string before transmission.
    ''' </summary>
    ''' <param name="Command">The literal command string to be transmitted.</param>
    ''' <param name="Raw">
    ''' if set to <c>true</c> the string is transmitted 'as-is'.
    ''' If set to <c>false</c> then protocol framing characters may be added prior to transmission.
    ''' </param>
    ''' <returns>
    ''' Returns the string response received from the device.
    ''' </returns>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="NotConnectedException">If the driver is not connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> </remarks>
    Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String

    ''' <summary>
    ''' Dispose the late-bound interface, if needed. Will release it via COM
    ''' if it is a COM object, else if native .NET will just dereference it
    ''' for GC.
    ''' </summary>
    Sub Dispose()

#End Region

#Region "Device Methods"
    ''' <summary>
    ''' The number of switches managed by this driver
    ''' </summary>
    ''' <returns>The number of switches managed by this driver</returns>
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException</b></p> 
    ''' <p>Switches are numbered from 0 to MaxSwitch - 1</p></remarks>
    ReadOnly Property MaxSwitch As Short

    ''' <summary>
    ''' Return the name of switch n.
    ''' </summary>
    ''' <param name="id">The switch number to return</param>
    ''' <returns>The name of the switch</returns>
    ''' <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.MethodNotImplementedException</b></p>
    ''' <para>Switches are numbered from 0 to MaxSwitch - 1</para></remarks>
    Function GetSwitchName(id As Short) As String

    ''' <summary>
    ''' Set a switch's name to a specified value.
    ''' </summary>
    ''' <param name="id">The number of the switch whose name is to be set</param>
    ''' <param name="name">The name of the switch</param>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
    ''' <remarks><p style="color:red"><b>Can throw a not implemented exception if the switch name can not be set by the application.</b></p>
    ''' <para>Switches are numbered from 0 to MaxSwitch - 1</para>
    ''' </remarks>
    Sub SetSwitchName(id As Short, name As String)

    ''' <summary>
    ''' Gets the description of the specified switch. This is to allow a fuller description of
    ''' the switch to be returned, for example for a tool tip.
    ''' </summary>
    ''' <param name="id">The number of the switch whose description is to be returned</param><returns></returns>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.MethodNotImplementedException</b></p>
    ''' <para>Switches are numbered from 0 to MaxSwitch - 1</para>
    ''' </remarks>
    Function GetSwitchDescription(id As Short) As String

    ''' <summary>
    ''' Reports if the specified switch can be written to, default true.
    ''' This is false if the switch cannot be written to, for example a limit switch or a sensor.
    ''' </summary>
    ''' <param name="id">The number of the switch whose write state is to be returned</param>
    ''' <returns>
    '''   <c>true</c> if the switch can be written to, otherwise <c>false</c>.
    ''' </returns>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.MethodNotImplementedException</b></p>
    ''' <para>Switches are numbered from 0 to MaxSwitch - 1</para>
    ''' </remarks>
    Function CanWrite(id As Short) As Boolean

#Region "boolean members"
    ''' <summary>
    ''' Return the state of switch n as a boolean
    ''' </summary>
    ''' <param name="id">The switch number to return</param>
    ''' <returns>True or false</returns>
    ''' <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.MethodNotImplementedException</b></p> 
    ''' <para>An analogue switch will return true if is value is closer to the maximum than the minimum, otherwise false. </para>
    ''' <para>Switches are numbered from 0 to MaxSwitch - 1</para></remarks>
    Function GetSwitch(id As Short) As Boolean

    ''' <summary>
    ''' Sets a switch to the specified state, true or false.
    ''' </summary>
    ''' <param name="id">The number of the switch to set</param>
    ''' <param name="state">The required switch state</param>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
    ''' <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p>
    ''' <para>Setting an analogue switch to true will set it to its maximum value and setting it to false will set it to its minimum value.</para>
    ''' <para>Switches are numbered from 0 to MaxSwitch - 1</para></remarks>
    Sub SetSwitch(id As Short, state As Boolean)
#End Region

#Region "Analogue members"
    ''' <summary>
    ''' Returns the maximum analogue value for this switch
    ''' </summary>
    ''' <param name="id">The switch whose value should be returned</param>
    ''' <returns>The maximum value to which this switch can be set.</returns>
    ''' <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.MethodNotImplementedException</b></p> 
    ''' <para>Boolean switches must return 1.0 as their maximum value. Switches are numbered from 0 to MaxSwitch - 1.</para></remarks>
    Function MaxSwitchValue(id As Short) As Double

    ''' <summary>
    ''' Returns the minimum analogue value for this switch
    ''' </summary>
    ''' <param name="id">The switch whose value should be returned</param>
    ''' <returns>The minimum value to which this switch can be set.</returns>
    ''' <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.MethodNotImplementedException</b></p> 
    ''' <para>Boolean switches must return 0.0 as their minimum value. Switches are numbered from 0 to MaxSwitch - 1.</para></remarks>
    Function MinSwitchValue(id As Short) As Double

    ''' <summary>
    ''' Returns the step size that this switch supports (the difference between successive values of the switch).
    ''' </summary>
    ''' <param name="id">The switch whose value should be returned</param>
    ''' <returns>The step size for this switch.</returns>
    ''' <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.MethodNotImplementedException</b></p>
    ''' <para>Boolean switches must return 1.0, giving two states. Switches are numbered from 0 to MaxSwitch - 1.</para>
    ''' <para>For any switch, the number of steps can be calculated as: ((MaxSwitchValue - MinSwitchValue) / SwitchStep) + 1,</para> </remarks>
    Function SwitchStep(id As Short) As Double

    ''' <summary>
    ''' Returns the analogue switch value for switch id.
    ''' </summary>
    ''' <param name="id">The switch whose value should be returned.</param>
    ''' <returns>The analogue value for this switch.</returns>
    ''' <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.MethodNotImplementedException</b></p>
    ''' <para>Boolean switches must return 0.0 or 1.0, giving two states. Switches are numbered from 0 to MaxSwitch - 1.</para></remarks>
    Function GetSwitchValue(id As Short) As Double

    ''' <summary>
    ''' Set the analogue value for this switch.
    ''' </summary>
    ''' <param name="id">The switch whose value should be set</param>
    ''' <param name="value">Value to be set between MinSwitchValue and MaxSwitchValue</param>
    ''' <exception cref="InvalidValueException">If the value is not between the maximum and minimum.</exception>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
    ''' <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p>
    ''' <para>If the value is not between the maximum and minimum then the method must throw an InvalidValueException. </para>
    ''' <para>The switch's <see cref="GetSwitch"></see> value will be appear as true if the switch value is set closer to the maximum than the minimum, otheriwse false.</para> </remarks>
    Sub SetSwitchValue(id As Short, value As Double)

#End Region
#End Region
End Interface
