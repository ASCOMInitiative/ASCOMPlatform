using System;

namespace ASCOM.Interfaces
{
	/// <summary>
	/// This interface is intended for use in any current or future device type and to avoid name
	/// clashes, management of action names is important from day  1. A two-part naming
	/// convention will be adopted - <example>DeviceType:UniqueActionName</example> where: 
	/// <list type="bullet">
	/// <item>
	/// DeviceType is the same value as would be used by Chooser.DeviceType e.g. 
	/// Telescope, Camera, Switch etc. 
	/// </item>
	/// <item>
	/// UniqueActionName is a single word, or multiple words joined by underscore 
	/// characters, that sensibly describes the action to be performed. 
	/// </item>
	/// </list>
	/// It is recommended that UniqueActionNames should be a maximum of 16 characters for 
	/// legibility. Should the same function and UniqueActionName be supported by more than
	/// one type of device, a DeviceType of “General” will be used. Action names will be
	/// case insensitive, so FilterWheel:SelectWheel, filterwheel:selectwheel 
	/// and FILTERWHEEL:SELECTWHEEL will all refer to the same action. 
	/// </summary>
	public interface IDeviceControl
	{
		/// <summary>
		/// Invokes the specified device-specific action.
		/// </summary>
		/// <param name="ActionName">
		/// A well known name agreed by interested parties that represents the action
		/// to be carried out. 
		/// <example>suppose filter wheels start to appear with automatic wheel changers; new actions could 
		/// be “FilterWheel:QueryWheels” and “FilterWheel:SelectWheel”. The former returning a 
		/// formatted list of wheel names and the second taking a wheel name and making the change.
		/// </example>
		/// </param>
		/// <param name="ActionParameters">
		/// List of required parameters or <see cref="String.Empty"/>  if none are required.
		/// </param>
		/// <returns>A string response and sets the <c>IDeviceControl.LastResult</c> property.</returns>
		string Action(string ActionName, string ActionParameters);

		/// <summary>
		/// Gets the supported actions.
		/// </summary>
		/// <value>The supported actions.</value>
		string[] SupportedActions { get; }

		/// <summary>
		/// Transmits an arbitrary string to the device and does not wait for a response.
		/// Optionally, protocol framing characters may be added to the string before transmission.
		/// </summary>
		/// <param name="Command">The literal command string to be transmitted.</param>
		/// <param name="Raw">
		/// if set to <c>true</c> the string is transmitted 'as-is'.
		/// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
		/// </param>
		void CommandBlind(string Command, bool Raw);

		/// <summary>
		/// Transmits an arbitrary string to the device and waits for a boolean response.
		/// Optionally, protocol framing characters may be added to the string before transmission.
		/// </summary>
		/// <param name="Command">The literal command string to be transmitted.</param>
		/// <param name="Raw">
		/// if set to <c>true</c> the string is transmitted 'as-is'.
		/// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
		/// </param>
		/// <returns>
		/// Returns the interpreted boolean response received from the device.
		/// </returns>
		bool CommandBool(string Command, bool Raw);

		/// <summary>
		/// Transmits an arbitrary string to the device and waits for a string response.
		/// Optionally, protocol framing characters may be added to the string before transmission.
		/// </summary>
		/// <param name="Command">The literal command string to be transmitted.</param>
		/// <param name="Raw">
		/// if set to <c>true</c> the string is transmitted 'as-is'.
		/// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
		/// </param>
		/// <returns>
		/// Returns the string response received from the device.
		/// </returns>
		string CommandString(string Command, bool Raw);
	}
}
