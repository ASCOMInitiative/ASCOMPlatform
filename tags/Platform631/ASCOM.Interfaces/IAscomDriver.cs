//tabs=4
// --------------------------------------------------------------------------------
// <summary>
// ASCOM.Interface ASCOM Driver Common Base Interface
// </summary>
//
// <copyright company="TiGra Astronomy" author="Timothy P. Long">
//	Copyright © 2010 The ASCOM Initiative
// </copyright>
//
// <license>
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </license>
//
//
// Implements:	
// Author:		(TPL) Timothy P. Long <Tim@tigranetworks.co.uk>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 10-Feb-2010	TPL	6.0.*	Initial edit. Based on draft document by Peter Simpson.
// --------------------------------------------------------------------------------
//
namespace ASCOM.Interface
	{
	/// <summary>
	/// Defines the properties and methods that are common to all ASCOM devices.
	/// </summary>
	public interface IAscomDriver
		{
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="IAscomDriver"/> is connected to its physical device.
		/// </summary>
		/// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
		/// <remarks>
		/// The actual meaning of 'connected' varies by device and the type of communications channel used.
		/// For the purposes of ASCOM, a device is said to be connected when it is ready to
		/// accept commands from the Client Application.
		/// </remarks>
		/// <exception cref="ASCOM.NotConnectedException">
		/// Thrown if there is a problem connecting to the device.
		/// </exception>
		bool Connected { get; set; }
		/// <summary>
		/// Gets the driver's description.
		/// </summary>
		/// <value>The description.</value>
		/// <remarks>
		/// This string may contain line endings and may be hundreds of characters long.
		/// It is intended to display detailed information about the telescope itself.
		/// See the <see cref="DriverInfo"/> property for a description of the driver itself.
		/// <alert class="caution">
		///   <para>
		///   This string should not be over 1000 characters in length, as applications may use
		///   popup boxes to display Description. Older versions of Windows have string length
		///   limitations in (e.g.) MessageBox(), which will cause an application failure if the
		///   string is too long. 
		///   </para>
		/// </alert>
		/// </remarks>
		string Description { get; }

		/// <summary>
		/// Gets descriptive and version information about this ASCOM driver.
		/// </summary>
		/// <value>The driver info.</value>
		/// <remarks>
		/// This string may contain line endings and may be hundreds of characters long.
		/// It is intended to display detailed information on the ASCOM driver,
		/// including version and copyright data.
		/// See the <see cref="Description"/> property for a description of the physical device.
		/// <alert class="caution">
		///   <para>
		///   This string should not be over 1000 characters in length, as applications may use
		///   popup boxes to display Description. Older versions of Windows have string length
		///   limitations in (e.g.) MessageBox(), which will cause an application failure if the
		///   string is too long. 
		///   </para>
		/// </alert>
		/// </remarks>
		string DriverInfo { get; }

		/// <summary>
		/// Gets the driver version.
		/// </summary>
		/// <value>A string containing only the major and minor version of the driver. This must be in the form "n.n".</value>
		/// <remarks>
		/// <alert class="caution">
		///   <para>
		///   Avoid the temptation to convert this version string into a number.
		///   Some locales use a decimal seperator other than a period, converting this value
		///   to a number will fail in those locales. If you must convert to a number,
		///   be sure to use <see cref="CultureInfo.InvariantCulture"/>.
		///   </para>
		/// </alert>
		/// </remarks>
		string DriverVersion { get; }

		/// <summary>
		/// Gets the interface version.
		/// </summary>
		/// <value>The interface version as a signed 16-bit integer.</value>
		short InterfaceVersion { get; }

		/// <summary>
		/// Gets the driver short name.
		/// </summary>
		/// <value>The short name of the driver, for display purposes.</value>
		/// <remarks>
		/// This is the name that is displayed in the ASCOM Chooser and potentially
		/// other user interface elements. The name should be a short, one-line description
		/// of the device, typically this will be the make and model of the device.
		/// The name should help to application user to easily select the required driver from
		/// a list of installed drivers.
		/// </remarks>
		string Name { get; }

		/// <summary>
		/// Displays the driver's setup dialog, allowing the user to configure
		/// device-specific settings.
		/// </summary>
		/// <remarks>
		///		<para>
		///		If there are no setup items, a simple popup box with the driver name
		///		and version should be displayed, along with a message that no setup or
		///		configuration is required. Drivers may raise an error if the telescope
		///		is connected when this method is called (as a way of preventing config
		///		changes while the driver is active).
		///		</para>
		///		<para>
		///		The SetupDialog must contain, at a minimum, two graphical elements:
		///		<list type="bullet">
		///			<item>
		///			An ASCOM icon (shown on the right), which is hyperlinked to the
		///			ASCOM Initiative web site. When the mouse hovers over this icon,
		///			it should change to the standard web-link icon . A click on the
		///			icon must open a web browser to the ASCOM Initiative home page
		///			at http://ASCOM-Standards.org/. The image should also display a
		///			hover tool-tip that indicates that the image is a hyperlink to
		///			the ASCOM Initiative web site. 
		///			</item>
		///			<item>
		///			A label that appears as Help. This label must behave as a hyperlink
		///			(as described in the preceding item) to a local HTML document,
		///			included with the driver, and which describes its behavior and other
		///			information that may be useful to an end-user of the driver. 
		///			</item>
		///		</list>
		///		</para>
		///		<alert class="note">
		///			<para>
		///			The setup dialog typically includes selection of the serial (COM) port
		///			or other communications channel to be used. If this is implemented in
		///			the setup dialog, then it should include the ability to dynamically
		///			discover and offer for selection all available devices of the required
		///			type. If dynamic discovery is not included and the communications device
		///			is a COM (serial) port, then at least 32 COM ports should be offered for
		///			selection. This is necessary because most newer systems use USB-to-serial
		///			adapters and these devices may appear on high-numbered COM ports. The USB
		///			bus can support 128 devices so the driver should be able to cope with three
		///			digit port numbers.
		///			Again, dynamic discovery is preferred.
		///			</para>
		///			<para>
		///			It may be useful to support Universal Naming Convention (UNC) when specifying
		///			device names. For example, <c>COM7</c> can be specified in UNC syntax as
		///			<c>\\.\COM7</c>.
		///			</para>
		///		</alert>
		/// </remarks>
		void SetupDialog();
		}
	}
