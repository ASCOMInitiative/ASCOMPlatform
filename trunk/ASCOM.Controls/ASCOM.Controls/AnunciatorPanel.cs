using System.Drawing;
using System.Windows.Forms;

namespace ASCOM.Controls
{
	/// <summary>
	///   A panel control for grouping and arranging <see cref = "Anunciator" /> controls.
	///   This control inherits most of its behaviour from the <see cref = "FlowLayoutPanel" />
	///   base class, but provides some defaults that are appropriate for use with ASCOM.
	/// </summary>
	public sealed class AnunciatorPanel : FlowLayoutPanel
	{
		/// <summary>
		///   Initializes a new instance of the <see cref = "AnunciatorPanel" /> class.
		/// </summary>
		public AnunciatorPanel()
		{
			BackColor = Color.FromArgb(64, 0, 0);
		}
	}
}