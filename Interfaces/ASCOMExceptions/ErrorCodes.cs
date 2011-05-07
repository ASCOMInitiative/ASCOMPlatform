namespace ASCOM
{
    /// <summary>
    ///   Error numbers for use by drivers.
    /// </summary>
    /// <remarks>
    ///   The range of permitted values falls within the class FACILTY_ITF as 
    ///   defined by the operating system and COM. These values will never clash with 
    ///   COM, RPC, or OS error codes.
    ///   <para>
    ///     Driver developers may extend this class by making use of the <c>partial</c> keyword.
    ///   </para>
    /// </remarks>
// ReSharper disable PartialTypeWithSinglePart
    public static partial class ErrorCodes
// ReSharper restore PartialTypeWithSinglePart
    {
        /// <summary>
        ///   Reserved error number for property or method not implemented.
        /// </summary>
        /// <remarks>
        ///   See ASCOM.Exception.NotImplementedException.
        /// </remarks>
        public static readonly int NotImplemented = unchecked((int) 0x80040400);

        /// <summary>
        ///   Reserved error number for reporting an invalid value.
        /// </summary>
        /// <remarks>
        ///   See ASCOM.Exception.InvalidValueException.
        /// </remarks>
        public static readonly int InvalidValue = unchecked((int) 0x80040401);

        /// <summary>
        ///   Reserved error number for reporting that a value has not been set.
        /// </summary>
        /// <remarks>
        ///   See ASCOM.Exception.ValueNotSetException.
        /// </remarks>
        public static readonly int ValueNotSet = unchecked((int) 0x80040402);

        /// <summary>
        ///   Error code used to indicate that the communications channel is not connected.
        /// </summary>
        public static readonly int NotConnected = unchecked((int) 0x80040407);

        /// <summary>
        ///   Error code used to indicate that the attempted operation is invalid because the mount
        ///   is currently in a Parked state.
        /// </summary>
        public static readonly int InvalidWhileParked = unchecked((int) 0x80040408);

        /// <summary>
        ///   Error code used to indicate that the attempted operation is invalid because the mount
        ///   is currently in a Slaved state.
        /// </summary>
        public static readonly int InvalidWhileSlaved = unchecked((int) 0x80040409);

        /// <summary>
        ///   An error related to settings.
        /// </summary>
        public static readonly int SettingsProviderError = unchecked((int) 0x8004040A);

        /// <summary>
        ///   Error code to indicate that the requested operation can not be undertaken at this time.
        /// </summary>
        public static readonly int InvalidOperationException = unchecked((int) 0x8004040B);

        /// <summary>
        ///   Error code to indicate that the requested operation can not be undertaken at this time.
        /// </summary>
        public static readonly int ActionNotImplementedException = unchecked((int)0x8004040C);

        /// <summary>
        ///   A 'catch-all' error code used when nothing else was specified.
        /// </summary>
        public static readonly int UnspecifiedError = unchecked((int) 0x800404FF);

        /// <summary>
        ///   The starting value for driver-specific error numbers.
        /// </summary>
        /// <remarks>
        ///   Drivers are free to choose their own numbers starting with
        ///   DriverBase, up to and including DriverMax.
        /// </remarks>
        public static readonly int DriverBase = unchecked((int) 0x80040500);

        /// <summary>
        ///   Maximum value for driver-specific error numbers.
        /// </summary>
        /// <remarks>
        ///   Drivers are free to choose their own numbers starting with
        ///   DriverBase, up to and including DriverMax.
        /// </remarks>
        public static readonly int DriverMax = unchecked((int) 0x80040FFF);
    }
}