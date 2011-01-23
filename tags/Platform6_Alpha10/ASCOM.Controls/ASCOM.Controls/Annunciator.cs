using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ASCOM.Internal;

namespace ASCOM.Controls
{
    ///<summary>
    ///  <para>
    ///    Wikipedia: An annunciator panel is a group of lights used as a central indicator of status of equipment or systems in an aircraft,
    ///    industrial process, building or other installation. Usually the annunciator panel includes a main warning lamp or audible signal
    ///    to draw the attention of operating personnel to the annunciator panel for abnormal events or conditions.
    ///  </para>
    ///  <para>
    ///    The Anunciator control provides a simple, standard method of displaying a status notification to the user within a Windows Forms application.
    ///    Anunciators are best used with the companion <see cref = "AnnunciatorPanel" /> control, although they can be placed anywhere on a Windows Form.
    ///    The control can be used to provide simple On/Off status displays or can be configured to blink with various levels of urgency so that it can
    ///    represent alarm conditions.
    ///    <example>
    ///      An anunciator may represent the slewing state of a telescope. It would be represented by the word "SLEW". When the telescope is stationary,
    ///      the anunciator remains inactive. When the telescope begins to slew, the anunciator is set to <see cref = "CadencePattern.BlinkFast" /> 
    ///      to alert the user that the equipment is in motion.
    ///    </example>
    ///  </para>
    ///  <para>
    ///    Each anunciator has active and inactive states. When inactive, the control displays in a subdued colour that is readable but does not draw
    ///    attention. When active, the control will display in a stronger, more visible colour and will either have a steady state or will blink in one
    ///    of a number of predefined cadence patterns. The cadence patterns are fixed and not user-definable, so that a standard 'look and feel'
    ///    is promoted accross different applications.
    ///  </para>
    ///  <para>
    ///    Whilst the user is at liberty to choose different colours for both <see cref = "ActiveColor" /> and <see cref = "InactiveColor" />, 
    ///    The default colours have been chosen to look similar to earlier applications that use similar displays and the defaults are highly 
    ///    recommended for most circumstances. The control's background colour is inherited from the parent control (which should normally be 
    ///    an <see cref = "AnnunciatorPanel" />) and is not directly settable by the user.
    ///  </para>
    ///</summary>
    public sealed class Annunciator : Label, ICadencedControl
    {
        /// <summary>
        ///   A flag that records the anunciator's last known state.
        /// </summary>
        private bool lastState;

        /// <summary>
        ///   Stores the mute status for the anunciator.
        /// </summary>
        private bool mute;

        /// <summary>
        /// Tracks whether this object has been disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Annunciator" /> class.
        /// </summary>
        public Annunciator()
        {
            // Default values
            BackColor = Color.FromArgb(64, 0, 0);
            InactiveColor = Color.FromArgb(96, 4, 4);
            ActiveColor = Color.FromArgb(200, 4, 4);
            Font = new Font("Consolas", 10.0F);
            Cadence = CadencePattern.SteadyOn;

            if (Parent != null)
                BackColor = Parent.BackColor; // Inherit background colour from parent.

            ParentChanged += AnunciatorParentChanged;

            lastState = ((uint) Cadence).Bit(CadenceManager.CadenceBitPosition);
            ForeColor = lastState ? ActiveColor : InactiveColor;
            CadenceManager.Instance.Add(this);
        }


        /// <summary>
        ///   Gets or sets the foreground color of the control. There is little point in setting this value
        ///   directly as it will normally be constantly overwritten at runtime.
        /// </summary>
        /// <value></value>
        /// <returns>
        ///   The foreground <see cref = "T:System.Drawing.Color" /> of the control. The default is the value of the <see cref = "P:System.Windows.Forms.Control.DefaultForeColor" /> property.
        /// </returns>
        /// <PermissionSet>
        ///   <IPermission class = "System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version = "1" Unrestricted = "true" />
        /// </PermissionSet>
        [Category("Appearance")]
        [DefaultValue(0xff800404)] // Color.FromArgb(128, 4, 4)
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Description("This property is not normally set directly as it will be overwritten at runtime.")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        /// <summary>
        ///   Gets or sets the color of the anunciator text when inactive.
        /// </summary>
        /// <value></value>
        /// <returns>
        ///   The foreground <see cref = "T:System.Drawing.Color" /> of the control. The default is the value of the <see cref = "P:System.Windows.Forms.Control.DefaultForeColor" /> property.
        /// </returns>
        /// <PermissionSet>
        ///   <IPermission class = "System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version = "1" Unrestricted = "true" />
        /// </PermissionSet>
        [Category("Appearance")]
        [DefaultValue(0xff480404)] // Color.FromArgb(72, 4, 4)
        [Description(
            "The anunciator's inactive colour. This is usually set to a value close to (but not equal) to the background colour. The default value is recommended for most situations."
            )]
        public Color InactiveColor { get; set; }

        /// <summary>
        ///   Gets or sets the color of the anunciator text when active.
        /// </summary>
        /// <value>The color of the anunciator text when active.</value>
        [Category("Appearance"), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(0xff800404)]
        [Description(
            "The anunciators active color. This should be bright and have a high contrast with the control's background. The default value is recommended for most situations."
            )]
        public Color ActiveColor { get; set; }

        /// <summary>
        ///   Gets or sets the background color for the control.
        /// </summary>
        /// <value></value>
        /// <returns>
        ///   A <see cref = "T:System.Drawing.Color" /> that represents the background color of the control. The default is the value of the <see cref = "P:System.Windows.Forms.Control.DefaultBackColor" /> property.
        /// </returns>
        /// <PermissionSet>
        ///   <IPermission class = "System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version = "1" Unrestricted = "true" />
        /// </PermissionSet>
        [Category("Appearance")]
        [DefaultValue(0xff400000)] // Color.FromArgb(64, 0, 0)
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Description("This property is not normally set directly as it will be overwritten at runtime.")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the control can respond to user interaction.
        ///   For an anunciator, this affects how it displays. A disabled anunciator will always display in
        ///   its <see cref = "InactiveColor" /> regardless of other settings and it will not participate in
        ///   cadence updates.
        /// </summary>
        /// <value></value>
        /// <returns><c>true</c> if the control can respond to user interaction; otherwise, <c>false</c>.
        ///   The default is <c>true</c>.
        /// </returns>
        /// <PermissionSet>
        ///   <IPermission class = "System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version = "1" Unrestricted = "true" />
        ///   <IPermission class = "System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version = "1" Unrestricted = "true" />
        ///   <IPermission class = "System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version = "1" Flags = "UnmanagedCode, ControlEvidence" />
        ///   <IPermission class = "System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version = "1" Unrestricted = "true" />
        /// </PermissionSet>
        [Category("Behavior")]
        [DefaultValue(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description(
            "Enables or disables the anunciator. When muted, the anunciator always displays in its InactiveColor.")]
        public bool Mute
        {
            get { return mute; }
            set
            {
                mute = value;
                if (value)
                {
                    StopCadenceUpdates();
                }
                else
                {
                    StartCadenceUpdates();
                }
            }
        }

        #region IDisposable pattern

        /// <summary>
        ///   Unregisters this control from the <see cref="CadenceManager"/> so that it will no longer receive cadence updates.
        /// </summary>
        private void StopCadenceUpdates()
        {
            CadenceManager.Instance.Remove(this);
        }

        /// <summary>
        ///   Registers this control with the <see cref="CadenceManager"/> so that it will receive cadence updates.
        /// </summary>
        private void StartCadenceUpdates()
        {
            CadenceManager.Instance.Add(this);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="T:System.ComponentModel.Component"/>.
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Label"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    StopCadenceUpdates();   // Unregister from CadenceManager.
                }
                disposed = true;
            }
            base.Dispose(disposing);        // Let the underlying control class clean itself up.
        }        #endregion

        #region ICadencedControl Members

        /// <summary>
        ///   Gets or sets the cadence (blink pattern) of the anunciator.
        ///   Different cadence patterns imply different levels of urgency or severity.
        /// </summary>
        /// <value>The cadence pattern.</value>
        [Category("Appearance")]
        [DefaultValue(CadencePattern.SteadyOn)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description(
            "Determines the cadence (blink pattern) for the anunciator. Different cadences imply different levels of severity or urgency."
            )]
        public CadencePattern Cadence { get; set; }


        /// <summary>
        ///   Updates the anunciator's display, if it has changed since the last update.
        /// </summary>
        /// <param name="newState">The new state of the control's appearance ('on' or 'off').</param>
        /// <remarks>
        /// Implements the <see cref="ICadencedControl.CadenceUpdate"/> method.
        /// The <see cref="CadenceManager"/> always calls this method on the GUI thread.
        /// </remarks>
        public void CadenceUpdate(bool newState)
        {
            if (IsDisposed) throw new ObjectDisposedException("Attempt to update an annunciator control after it has been disposed.");
            // Update the control's display, but only if there has been a change of state.
            if (newState != lastState)
            {
                ForeColor = (newState ? ActiveColor : InactiveColor);
                Invalidate();
                Update();
                lastState = newState;
            }
        }

        #endregion

        /// <summary>
        ///   Handles the ParentChanged event of the Anunciator control.
        ///   Changes the control's background colour to blend in with the parent control.
        /// </summary>
        /// <param name = "sender">The source of the event.</param>
        /// <param name = "e">The <see cref = "System.EventArgs" /> instance containing the event data.</param>
        private void AnunciatorParentChanged(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                BackColor = Parent.BackColor;
            }
            else
            {
                BackColor = Color.FromArgb(64, 0, 0);
            }
        }

        #region Nested type: CadenceUpdateDelegate

        /// <summary>
        ///   Defines the signature for the CadenceUpdateDelegate, used in making thread-safe control updates.
        /// </summary>
        private delegate void CadenceUpdateDelegate(bool newState);

        #endregion
    }
}