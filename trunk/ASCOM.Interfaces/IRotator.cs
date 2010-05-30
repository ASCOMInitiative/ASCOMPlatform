using System.Runtime.InteropServices;

namespace ASCOM.Interface
{
	//[ComImport, TypeLibType((short)0x10c0), Guid("49003324-8DE2-4986-BC7D-4D85E1C4CF6B")]
	//public interface IRotator
	//{
	//    [DispId(0x65)]
	//    bool CanReverse { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x65)] get; }
	//    [DispId(0x66)]
	//    bool Connected { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x66)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x66)] set; }
	//    [DispId(0x67)]
	//    bool IsMoving { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x67)] get; }
	//    [DispId(0x68)]
	//    float Position { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x68)] get; }
	//    [DispId(0x69)]
	//    bool Reverse { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x69)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x69)] set; }
	//    [DispId(0x6a)]
	//    float StepSize { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6a)] get; }
	//    [DispId(0x6b)]
	//    float TargetPosition { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6b)] get; }
	//    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x191)]
	//    void Halt();
	//    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x192)]
	//    void Move(float Position);
	//    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x193)]
	//    void MoveAbsolute(float Position);
	//    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x194)]
	//    void SetupDialog();
	//}

	[Guid("49003324-8DE2-4986-BC7D-4D85E1C4CF6B")]
	public interface IRotator
	{
		/// <summary>
		///   Gets a value indicating whether this driver supports the <see cref = "Reverse" /> property.
		/// </summary>
		/// <value>
		///   <c>true</c> if <see cref = "Reverse" /> is supported; otherwise, <c>false</c>.
		/// </value>
		bool CanReverse { get; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="IRotator"/> is connected.
		/// </summary>
		/// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
		bool Connected { get; set; }

		/// <summary>
		///   Gets a value indicating whether the rotator is moving.
		/// </summary>
		/// <value><c>true</c> if the rotator is moving; otherwise, <c>false</c>.</value>
		/// <remarks>
		///   Rotation is asynchronous, that is, when the <see cref = "Move" /> is called, it starts the rotation,
		///   then returns immediately. During rotation, <see cref = "IsMoving" /> must be <c>true</c>,
		///   else it must be <c>false</c>.
		/// </remarks>
		bool IsMoving { get; }

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
		float Position { get; }

		/// <summary>
		///   Gets or sets a value indicating whether this <see cref = "IRotator" /> should use reversed rotation and angular direction.
		///   <seealso cref = "Position" />.
		/// </summary>
		/// <value><c>true</c> if rotation and angular direction are reversed; otherwise, <c>false</c>.</value>
		bool Reverse { get; set; }

		/// <summary>
		///   Gets the rotator's step size.
		/// </summary>
		/// <value>The angular size of each step in position angle, in degrees.</value>
		float StepSize { get; }

		/// <summary>
		///   Gets the target position angle for <see cref = "Move" /> and <see cref = "MoveAbsolute" />.
		/// </summary>
		/// <value>The target position.</value>
		/// <remarks>
		///   Upon calling <see cref = "Move" /> or <see cref = "MoveAbsolute" />, this property immediately changes to the position angle to which the
		///   rotator is moving. The value is retained until a subsequent call to <see cref = "Move" /> or <see cref = "MoveAbsolute" />.
		/// </remarks>
		float TargetPosition { get; }

		/// <summary>
		///   Immediately stops any rotation due to a previous <see cref = "Move" /> or <see cref = "MoveAbsolute" /> call.
		/// </summary>
		/// <remarks>
		///   Optional. Should raise <see cref = "ASCOM.NotImplementedException" /> if not supported.
		/// </remarks>
		/// <exception cref = "ASCOM.NotImplementedException">
		///   Must be thrown if the device cannot support this method.
		/// </exception>
		void Halt();

		/// <summary>
		///   Moves to the specified position, relative to the current position angle.
		/// </summary>
		/// <param name = "Position">Relative position to move in degrees from current position.</param>
		/// <remarks>
		///   Calling this method causes the <see cref = "TargetPosition" /> property to change to the sum of the current angular position and the
		///   value of the <see cref = "Position" /> parameter (modulo 360 degrees), then starts rotation to <see cref = "TargetPosition" />.
		/// </remarks>
		void Move(float Position);

		/// <summary>
		///   Changes the rotator position angle to the given angular position.
		/// </summary>
		/// <param name = "Position">The absolute position angle in degrees to which the rotator will be moved.</param>
		/// <remarks>
		///   Calling this method causes the <see cref = "TargetPosition" /> property to change to the value of the
		///   <see cref = "Position" /> parameter, then starts rotation to <see cref = "TargetPosition" />.
		/// </remarks>
		/// <exception cref = "ASCOM.InvalidValueException">
		///   Should be thrown if the supplied <see cref = "Position" /> is not in required range.
		/// </exception>
		void MoveAbsolute(float Position);

		/// <summary>
		/// Displays the ASCOM driver's setup dialog.
		/// </summary>
		void SetupDialog();
	}
}