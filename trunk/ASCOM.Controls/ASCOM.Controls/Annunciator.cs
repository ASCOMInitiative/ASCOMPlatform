using System;
using System.Collections.Generic;
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
    ///      the anunciator remains inactive. When teh telescope begins to slew, the anunciator is set to <see cref = "CadencePattern.BlinkFast" /> 
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
    public sealed class Annunciator : Label
    {
        /// <summary>
        ///   A timer that triggers updates to anunciators to simulate flashing.
        /// </summary>
        private static readonly Timer CadenceTimer = new Timer();

        /// <summary>
        ///   The delegate that will be used to handle cadence timer events.
        /// </summary>
        private static readonly EventHandler CadenceEventHandler = tmrCadence_Tick;

        /// <summary>
        ///   A list of all the anunciator controls that have been created which need updating
        ///   when the timer ticks.
        /// </summary>
        private static readonly List<Annunciator> UpdateList = new List<Annunciator>();

        /// <summary>
        ///   Indicates the current bit position within the cadence register.
        /// </summary>
        private static int cadenceBitPosition;

        /// <summary>
        ///   A flag that records the anunciator's last known state.
        /// </summary>
        private bool active;

        /// <summary>
        ///   Stores the mute status for the anunciator.
        /// </summary>
        private bool mute;

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

            ParentChanged += Anunciator_ParentChanged;

            active = ((uint) Cadence).Bit(cadenceBitPosition);
            ForeColor = active ? ActiveColor : InactiveColor;
            lock (UpdateList)
            {
                UpdateList.Add(this);
                // If this is the first instance, then create and start the timer.
                if (UpdateList.Count == 1)
                {
                    CadenceTimer.Interval = 125; // 125 millisecond ticks gives 8 cadence updates per second.
                    CadenceTimer.Tick += CadenceEventHandler; // Wire up the event handler delegate.
                    CadenceTimer.Start(); // Let rip.
                }
            }
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
                    StopTimerUpdates();
                    CadenceUpdate(false); // Make the display state Inactive.
                }
                else
                {
                    lock (this)
                    {
                        bool activeState = ((uint) Cadence).Bit(cadenceBitPosition);
                        CadenceUpdate(activeState);
                        StartTimerUpdates();
                    }
                }
            }
        }

        #region IDisposable pattern

        /// <summary>
        ///   Stops the timer updates by removing this instance from the <see cref = "UpdateList" />.
        /// </summary>
        private void StopTimerUpdates()
        {
            lock (UpdateList)
            {
                if (UpdateList.Contains(this))
                    UpdateList.Remove(this); // Make sure this object doesn't receive any more timer updates.
            }
        }

        /// <summary>
        ///   Registers the anunciator control for timed cadence updates by adding it to <see cref = "UpdateList" />.
        /// </summary>
        private void StartTimerUpdates()
        {
            lock (UpdateList)
            {
                if (!UpdateList.Contains(this))
                    UpdateList.Add(this);
            }
        }

        /// <summary>
        ///   Releases all resources used by the <see cref = "T:System.ComponentModel.Component" />.
        /// </summary>
        public new void Dispose()
        {
            StopTimerUpdates();
            Dispose(true); // Allow disposal of both managed and unmanaged resources.
        }

        /// <summary>
        ///   Releases unmanaged resources and performs other cleanup operations before the
        ///   <see cref = "Annunciator" /> is reclaimed by garbage collection.
        /// </summary>
        ~Annunciator()
        {
            Dispose(false); // Allow disposal of only unmanaged resources.
        }

        #endregion

        /// <summary>
        ///   Handles the ParentChanged event of the Anunciator control.
        ///   Changes the control's background colour to blend in with the parent control.
        /// </summary>
        /// <param name = "sender">The source of the event.</param>
        /// <param name = "e">The <see cref = "System.EventArgs" /> instance containing the event data.</param>
        private void Anunciator_ParentChanged(object sender, EventArgs e)
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

        /// <summary>
        ///   Handles the Tick event of the tmrCadence control.
        ///   Computes the new display status for each anunciator control based on its <see cref = "Cadence" />
        ///   property and requests the anunciator update itself with the new value.
        /// </summary>
        /// <param name = "sender">The source of the event.</param>
        /// <param name = "e">The <see cref = "System.EventArgs" /> instance containing the event data.</param>
        private static void tmrCadence_Tick(object sender, EventArgs e)
        {
            // Critical region, prevent addition or removal of anunciator controls during
            // the update cycle.
            lock (UpdateList)
            {
                if (UpdateList.Count < 1)
                {
                    CadenceTimer.Stop(); // Stop any further tick events.
                    CadenceTimer.Tick -= CadenceEventHandler; // Withdraw the event handler delegate.
                    return; // Nothing more to do.
                }

                // Increment and (if necessary) wrap the cadence bit position index.
                if (++cadenceBitPosition > 31)
                {
                    cadenceBitPosition = 0;
                }

                // Visit each anunciator and ask it to update its status.
                foreach (Annunciator item in UpdateList)
                {
                    try
                    {
                        uint cadenceMask = (uint) item.Cadence;
                        item.CadenceUpdate(cadenceMask.Bit(cadenceBitPosition));
                    }
                    catch
                    {
                    } // ToDo: log the exception, maybe?
                }
            }
        }

        /// <summary>
        ///   Updates the anunciator's display, if it has changed since the last update.
        ///   this is performed in a thread-safe manner.
        /// </summary>
        private void CadenceUpdate(bool newState)
        {
            // If we're on a different thread than the control, we need to marshall
            // execution over onto the correct thread.
            if (InvokeRequired)
            {
                Invoke(new CadenceUpdateDelegate(CadenceUpdate), new object[] {newState});
                return;
            }

            // OK, we're on the control's thread, now we are completely thread-safe.
            // Update the control's display, but only if there has been a change of state.
            if (newState != active)
            {
                ForeColor = (newState ? ActiveColor : InactiveColor);
                Invalidate();
                Update();
                active = newState;
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