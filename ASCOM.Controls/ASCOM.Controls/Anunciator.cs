using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASCOM.Internal;

namespace ASCOM.Controls
{
    /// <summary>
    /// Annunciator class
    /// </summary>
	public class Anunciator : Label
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Anunciator"/> class.
        /// </summary>
		public Anunciator() : base()
		{
			// Default values
			this.BackColor = Color.FromArgb(64, 0, 0);
			this.InactiveColor = Color.FromArgb(96, 4, 4);
			this.ActiveColor = Color.FromArgb(200, 4, 4);
			this.Font = new Font("Consolas", 10.0F);
			this.Cadence = CadencePattern.SteadyOn;

			if (this.Parent != null)
				this.BackColor = Parent.BackColor;	// Inherit background colour from parent.

			this.ParentChanged += new EventHandler(Anunciator_ParentChanged);

			if (DesignMode)
			{
				Active = true;
				ForeColor = ActiveColor;
			}
			else
			{
				Active = ((uint)Cadence).Bit(cadenceBitPosition);
				this.ForeColor = this.Active ? this.ActiveColor : this.InactiveColor;
				lock (UpdateList)
				{
					UpdateList.Add(this);
					// If this is the first instance, then create and start the timer.
					if (UpdateList.Count == 1 && !DesignMode)
					{
						tmrCadence.Interval = 125;				// 125 millisecond ticks gives 8 cadence updates per second.
						tmrCadence.Tick += cadenceEventHandler;	// Wire up the event handler delegate.
						tmrCadence.Start();						// Let rip.
					}
				}
			}
		}

		/// <summary>
		/// Handles the ParentChanged event of the Anunciator control.
		/// Changes the control's background colour to blend in with the parent control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void Anunciator_ParentChanged(object sender, EventArgs e)
		{
			if (this.Parent != null)
			{
				this.BackColor = Parent.BackColor;
			}
			else
			{
				this.BackColor = Color.FromArgb(64, 0, 0);
			}
		}

		/// <summary>
		/// Handles the Tick event of the tmrCadence control.
		/// Computes the new display status for each anunciator control based on its <see cref="Cadence"/>
		/// property and requests the anunciator update itself with the new value.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		static void tmrCadence_Tick(object sender, EventArgs e)
		{
			// Critical region, prevent addition or removal of anunciator controls during
			// the update cycle.
			lock (UpdateList)	
			{
				if (UpdateList.Count < 1)
				{
					tmrCadence.Stop();							// Stop any further tick events.
					tmrCadence.Tick -= cadenceEventHandler;		// Withdraw the event handler delegate.
					return;										// Nothing more to do.
				}

				// Increment and (if necessary) wrap the cadence bit position index.
				if (++cadenceBitPosition > 31)
				{
					cadenceBitPosition = 0;
				}

				// Visit each anunciator and ask it to update its status.
				foreach (var item in UpdateList)
				{
					try
					{
						uint cadenceMask = (uint)item.Cadence;
						item.CadenceUpdate(cadenceMask.Bit(cadenceBitPosition));
					}
					catch { }	// ToDo: log the exception, maybe?
				}
			}
		}

		/// <summary>
		/// Updates the anunciator's display, if it has changed since the last update.
		/// this is performed in a thread-safe manner.
		/// </summary>
		private void CadenceUpdate(bool newState)
		{
			// If we're on a different thread than the control, we need to marshall
			// execution over onto the correct thread.
			if (this.InvokeRequired)
			{
				this.Invoke(new CadenceUpdateDelegate(CadenceUpdate), new object[]{newState});
				return;
			}

			// OK, we're on the control's thread, now we are completely thread-safe.
			// Update the control's display, but only if there has been a change of state.
			if (newState != this.Active)
			{
				this.ForeColor = (newState ? this.ActiveColor : this.InactiveColor);
				this.Invalidate();
				this.Update();
				this.Active = newState;
			}
		}

		/// <summary>
		/// Defines the signature for the CadenceUpdateDelegate, used in making thread-safe control updates.
		/// </summary>
		private delegate void CadenceUpdateDelegate(bool newState);

		/// <summary>
		/// A timer that triggers updates to anunciators to simulate flashing.
		/// </summary>
		private static Timer tmrCadence = new Timer();

		/// <summary>
		/// The delegate that will be used to handle cadence timer events.
		/// </summary>
		private static readonly EventHandler cadenceEventHandler = new EventHandler(tmrCadence_Tick);

		/// <summary>
		/// A list of all the anunciator controls that have been created which need updating
		/// when the timer ticks.
		/// </summary>
		private static List<Anunciator> UpdateList = new List<Anunciator>();

		/// <summary>
		/// Indicates the current bit position within the cadence register.
		/// </summary>
		private static int cadenceBitPosition = 0;

		/// <summary>
		/// A flag that records the anunciator's last known state.
		/// </summary>
		private bool Active = false;

		/// <summary>
		/// Gets or sets the foreground color of the control. There is little point in setting this value
		/// directly as it will normally be constantly overwritten at runtime.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// The foreground <see cref="T:System.Drawing.Color"/> of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultForeColor"/> property.
		/// </returns>
		/// <PermissionSet>
		/// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// </PermissionSet>
		[Category("Appearance")]
		[DefaultValue(0xff800404)]		// Color.FromArgb(128, 4, 4)
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Description("This property is not normally set directly as it will be overwritten at runtime.")]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}
		/// <summary>
		/// Gets or sets the color of the anunciator text when inactive.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// The foreground <see cref="T:System.Drawing.Color"/> of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultForeColor"/> property.
		/// </returns>
		/// <PermissionSet>
		/// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// </PermissionSet>
		[Category("Appearance")]
		[DefaultValue(0xff480404)]		// Color.FromArgb(72, 4, 4)
		public Color InactiveColor { get; set; }

		private Color _ActiveColor;
		/// <summary>
		/// Gets or sets the color of the anunciator text when active.
		/// </summary>
		/// <value>The color of the anunciator text when active.</value>
		[Category("Appearance")]
		[DefaultValue(0xff800404)]
		public Color ActiveColor
		{
			get
			{
				return _ActiveColor;
			}
			set
			{
				_ActiveColor = value;
				this.ForeColor = value;
			}
		}


		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// A <see cref="T:System.Drawing.Color"/> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"/> property.
		/// </returns>
		/// <PermissionSet>
		/// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// </PermissionSet>
		[Category("Appearance")]
		[DefaultValue(0xff400000)]   // Color.FromArgb(64, 0, 0)
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the cadence.
		/// </summary>
		/// <value>The cadence pattern.</value>
		[Category("Appearance")]
		[DefaultValue(CadencePattern.SteadyOn)]
		public CadencePattern Cadence { get; set; }

		#region IDisposable pattern

		/// <summary>
		/// Stops the timer updates by removing this instance from the <see cref="UpdateList"/>.
		/// </summary>
		private void StopTimerUpdates()
		{
			lock (UpdateList)
			{
				try
				{
					if (UpdateList.Contains(this))
						UpdateList.Remove(this);	// Make sure this object doesn't receive any more timer updates.
				}
				catch { };
			}
		}

        /// <summary>
        /// Releases all resources used by the <see cref="T:System.ComponentModel.Component"/>.
        /// </summary>
		public new void Dispose()
		{
			this.StopTimerUpdates();
			this.Dispose(true);	// Allow disposal of both managed and unmanaged resources.
		}
		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="Anunciator"/> is reclaimed by garbage collection.
		/// </summary>
		~Anunciator()
		{
			this.Dispose(false); // Allow disposal of only unmanaged resources.
		}
		#endregion
	}
}
