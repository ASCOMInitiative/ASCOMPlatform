//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Rotator driver for RotatorSimulator
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Rotator interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Rotator Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using ASCOM.Interface;
using System.Reflection;

namespace ASCOM.Simulator
{
	//
	// Your driver's ID is ASCOM.Simulator.Rotator
	//
	// The Guid attribute sets the CLSID for ASCOM.Simulator.Rotator
	// The ClassInterface/None addribute prevents an empty interface called
	// _Rotator from being created and used as the [default] interface
	//
	[Guid("e3244961-cb52-437d-aa9b-e5324db8f388")]
	[ClassInterface(ClassInterfaceType.None)]
	public class Rotator : 
		ReferenceCountedObjectBase,
		IRotator
	{

		//
		// Constructor - Must be public for COM registration!
		//
		public Rotator() { }

		//
		// PUBLIC COM INTERFACE IRotator IMPLEMENTATION
		//

		#region IRotator Members

		/// <summary>
		///   Gets a value indicating whether this driver supports the <see cref = "IRotator.Reverse" /> property.
		/// </summary>
		/// <value>
		///   <c>true</c> if <see cref = "IRotator.Reverse" /> is supported; otherwise, <c>false</c>.
		/// </value>
		public bool CanReverse
		{
			get { return RotatorHardware.CanReverse; }
		}

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
		public bool Connected
		{
			get { return RotatorHardware.Connected; }
			set { RotatorHardware.Connected = value; }
		}

		/// <summary>
		/// Gets the driver's description.
		/// </summary>
		/// <value>The description.</value>
		/// <remarks>
		/// This string may contain line endings and may be hundreds of characters long.
		/// It is intended to display detailed information about the telescope itself.
		/// See the <see cref="IAscomDriver.DriverInfo"/> property for a description of the driver itself.
		/// <alert class="caution">
		///   <para>
		///   This string should not be over 1000 characters in length, as applications may use
		///   popup boxes to display Description. Older versions of Windows have string length
		///   limitations in (e.g.) MessageBox(), which will cause an application failure if the
		///   string is too long. 
		///   </para>
		/// </alert>
		/// </remarks>
		public string Description
		{
			get { return "Rotator Simulator"; }
		}

		/// <summary>
		/// Gets descriptive and version information about this ASCOM driver.
		/// </summary>
		/// <value>The driver info.</value>
		/// <remarks>
		/// This string may contain line endings and may be hundreds of characters long.
		/// It is intended to display detailed information on the ASCOM driver,
		/// including version and copyright data.
		/// See the <see cref="IAscomDriver.Description"/> property for a description of the physical device.
		/// <alert class="caution">
		///   <para>
		///   This string should not be over 1000 characters in length, as applications may use
		///   popup boxes to display Description. Older versions of Windows have string length
		///   limitations in (e.g.) MessageBox(), which will cause an application failure if the
		///   string is too long. 
		///   </para>
		/// </alert>
		/// </remarks>
		public string DriverInfo
		{
			get { return "ASCOM Platform Rotator Simulator"; }
		}

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
		public string DriverVersion
		{
			get
			{
				var assy = Assembly.GetExecutingAssembly();
				var name = assy.GetName();
				var version = name.Version;
				return string.Format("{0}.{1}", version.Major, version.Minor);
			}
		}

		/// <summary>
		/// Gets the interface version.
		/// </summary>
		/// <value>The interface version as a signed 16-bit integer.</value>
		public short InterfaceVersion
		{
			get { return 1; }
		}

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
		public string Name
		{
			get { return "Rotator Simulator"; }
		}

		/// <summary>
		///   Immediately stops any rotation due to a previous <see cref = "IRotator.Move" /> or <see cref = "IRotator.MoveAbsolute" /> call.
		/// </summary>
		/// <remarks>
		///   Optional. Should raise <see cref = "ASCOM.NotImplementedException" /> if not supported.
		/// </remarks>
		/// <exception cref = "ASCOM.NotImplementedException">
		///   Must be thrown if the device cannot support this method.
		/// </exception>
		public void Halt()
		{
			RotatorHardware.Halt();
		}

		/// <summary>
		///   Gets a value indicating whether the rotator is moving.
		/// </summary>
		/// <value><c>true</c> if the rotator is moving; otherwise, <c>false</c>.</value>
		/// <remarks>
		///   Rotation is asynchronous, that is, when the <see cref = "IRotator.Move" /> is called, it starts the rotation,
		///   then returns immediately. During rotation, <see cref = "IRotator.IsMoving" /> must be <c>true</c>,
		///   else it must be <c>false</c>.
		/// </remarks>
		public bool IsMoving
		{
			get { return RotatorHardware.Moving; }
		}

		/// <summary>
		///   Moves to the specified position, relative to the current position angle.
		/// </summary>
		/// <param name = "Position">Relative position to move in degrees from current position.</param>
		/// <remarks>
		///   Calling this method causes the <see cref = "IRotator.TargetPosition" /> property to change to the sum of the current angular position and the
		///   value of the <see cref = "IRotator.Position" /> parameter (modulo 360 degrees), then starts rotation to <see cref = "IRotator.TargetPosition" />.
		/// </remarks>
		public void Move(float Position)
		{
			RotatorHardware.Move(Position);
		}

		/// <summary>
		///   Changes the rotator position angle to the given angular position.
		/// </summary>
		/// <param name = "Position">The absolute position angle in degrees to which the rotator will be moved.</param>
		/// <remarks>
		///   Calling this method causes the <see cref = "IRotator.TargetPosition" /> property to change to the value of the
		///   <see cref = "IRotator.Position" /> parameter, then starts rotation to <see cref = "IRotator.TargetPosition" />.
		/// </remarks>
		/// <exception cref = "ASCOM.InvalidValueException">
		///   Should be thrown if the supplied <see cref = "IRotator.Position" /> is not in required range.
		/// </exception>
		public void MoveAbsolute(float Position)
		{
			RotatorHardware.MoveAbsolute(Position);
		}

		/// <summary>
		///   Gets the current instantaneous position angle of the rotator (deg. See remarks).
		/// </summary>
		/// <value>The current instantaneous position angle of the rotator.</value>
		/// <remarks>
		///   <para>
		///     The position is expressed as an angle from 0 up to but not including 360 degrees, counter-clockwise against the sky.
		///     This is the standard definition of Position Angle. However, the rotator does not need to (and in general will not)
		///     report the true Equatorial Position Angle, as the attached imager may not be precisely aligned with the rotator's
		///     indexing. It is up to the client to determine any offset between mechanical rotator position angle and the true
		///     Equatorial Position Angle of the imager, and compensate for any difference.
		///   </para>
		///   <para>
		///     The optional Reverse property is provided in order to manage rotators being used on optics with odd or even number
		///     of reflections. With the Reverse switch in the correct position for the optics, the reported position angle must
		///     be counter-clockwise against the sky. 
		///   </para>
		/// </remarks>
		public float Position
		{
			get { return RotatorHardware.Position; }
		}

		/// <summary>
		///   Gets or sets a value indicating whether this <see cref = "IRotator" /> should use reversed rotation and angular direction.
		///   <seealso cref = "IRotator.Position" />.
		/// </summary>
		/// <value><c>true</c> if rotation and angular direction are reversed; otherwise, <c>false</c>.</value>
		public bool Reverse
		{
			get { return RotatorHardware.Reverse; }
			set { RotatorHardware.Reverse = value; }
		}

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
		public void SetupDialog()
		{
			if(RotatorHardware.Connected)
				throw new DriverException("The rotator is connected, cannot do SetupDialog()",
									unchecked(ErrorCodes.DriverBase + 4));
			RotatorSimulator.m_MainForm.DoSetupDialog();			// Kinda sleazy
		}

		/// <summary>
		///   Gets the rotator's step size.
		/// </summary>
		/// <value>The angular size of each step in position angle, in degrees.</value>
		public float StepSize
		{
			get { return RotatorHardware.StepSize; }
		}

		/// <summary>
		///   Gets the target position angle for <see cref = "IRotator.Move" /> and <see cref = "IRotator.MoveAbsolute" />.
		/// </summary>
		/// <value>The target position.</value>
		/// <remarks>
		///   Upon calling <see cref = "IRotator.Move" /> or <see cref = "IRotator.MoveAbsolute" />, this property immediately changes to the position angle to which the
		///   rotator is moving. The value is retained until a subsequent call to <see cref = "IRotator.Move" /> or <see cref = "IRotator.MoveAbsolute" />.
		/// </remarks>
		public float TargetPosition
		{
			get { return RotatorHardware.TargetPosition; }
		}

		#endregion
	}
}
