using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ASCOM.Controls
{
	/// <summary>
	///   A panel control for grouping and arranging <see cref = "Annunciator" /> controls.
	///   This control inherits most of its behaviour from the <see cref = "FlowLayoutPanel" />
	///   base class, but provides some defaults that are appropriate for use with ASCOM.
	/// </summary>
	[Obsolete("An improved version of this class is available as a NuGet package at https://www.nuget.org/packages/TA.WinForms.Controls/")]
	public sealed class AnnunciatorPanel : FlowLayoutPanel
	{
        /// <summary>
        /// Static initialiser called once per AppDomain to log the component name.
        /// </summary>
        static AnnunciatorPanel()
        {
            ASCOM.Utilities.Log.Component(Assembly.GetExecutingAssembly(), "AnnunciatorPanel");
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "AnnunciatorPanel" /> class.
        /// </summary>
        public AnnunciatorPanel()
		{
			BackColor = Color.FromArgb(64, 0, 0);
		}

        /// <summary>
        /// Releases all resources used by the <see cref="T:System.ComponentModel.Component"/>.
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
	}
}