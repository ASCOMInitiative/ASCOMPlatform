using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.Controls
{
	public partial class Anunciator : UserControl
	{
		public Anunciator()
		{
			InitializeComponent();
			lock (UpdateList)
			{
				UpdateList.Add(this);
			}
		}
		/// <summary>
		/// A timer that triggers updates to anunciators to simulate flashing.
		/// </summary>
		private static Timer tmrCadence = new Timer();
		/// <summary>
		/// A list of all the anunciator controls that have been created which need updating
		/// when the timer ticks.
		/// </summary>
		private static List<Anunciator> UpdateList = new List<Anunciator>();
		/// <summary>
		/// Gets or sets the foreground color of the control.
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
		[DefaultValue(CadencePattern.SteadyOff)]
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
		public void Dispose()
		{
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
