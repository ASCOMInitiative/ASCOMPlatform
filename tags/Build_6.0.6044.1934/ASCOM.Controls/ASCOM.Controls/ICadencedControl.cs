namespace ASCOM.Controls
{
    /// <summary>
    ///   Defines the members necessary for a control to register and be managed by the
    ///   <see cref = "CadenceManager" /> singleton.
    /// </summary>
    public interface ICadencedControl
    {
        /// <summary>
        ///   Gets or sets the cadence (blink pattern) of the control.
        ///   Different cadence patterns imply different levels of urgency or severity.
        /// </summary>
        /// <value>The cadence pattern.</value>
        /// <remarks>
        ///   <see cref = "CadencePattern" /> is based on a 64-bit long integer but
        ///   only 32-bits are used. This is necessary to achieve CLS compliance, because
        ///   32-bit uints are not CLS compliant.
        /// </remarks>
        CadencePattern Cadence { get; set; }

        /// <summary>
        ///   Updates the control's display.
        ///   <see cref = "CadenceManager" /> always calls this method on the GUI thread so that control updates are thread-safe.
        /// </summary>
        void CadenceUpdate(bool newState);
    }
}