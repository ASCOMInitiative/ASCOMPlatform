using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ASCOM.Controls
{
	/// <summary>
	/// A panel for grouping and arranging anunciators.
	/// This control inherits most of its behaviour from the <see cref="FlowLayoutPanel"/>
	/// base class, but provides some default colours.
	/// </summary>
	public class AnunciatorPanel : FlowLayoutPanel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AnunciatorPanel"/> class.
		/// </summary>
		public AnunciatorPanel()
			: base()
		{
			this.BackColor = Color.FromArgb(64, 0, 0);
		}
	}
}
