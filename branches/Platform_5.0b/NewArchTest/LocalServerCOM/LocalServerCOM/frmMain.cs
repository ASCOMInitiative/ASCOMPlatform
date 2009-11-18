using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.LocalServerCOM
{
	public partial class frmMain : Form
	{
		delegate void SetTextCallback(string text);

		public frmMain()
		{
			InitializeComponent();
		}

		//
		// If the calling thread is different from the thread that
		// created the TextBox control, this method creates a
		// SetTextCallback and calls itself asynchronously using the
		// Invoke method.
		//
		// If the calling thread is the same as the thread that created
		// the TextBox control, the Text property is set directly. 
		//
		// The GC thread traces, so this is needed
		//
		public void Trace(string Msg)
		{
			// InvokeRequired required compares the thread ID of the
			// calling thread to the thread ID of the creating thread.
			// If these threads are different, it returns true.
			if (txtTrace.InvokeRequired)
			{
				SetTextCallback d = new SetTextCallback(Trace);
				this.Invoke(d, new object[] { Msg });
			}
			else
			{
				txtTrace.Text += Msg + "\r\n";
				//txtTrace.SelectedText = "";							// HANGS THE SERVER??????? Oh well.
				//txtTrace.ScrollToCaret();								// Works only if serever window has focus!
			}
		}

	}
}