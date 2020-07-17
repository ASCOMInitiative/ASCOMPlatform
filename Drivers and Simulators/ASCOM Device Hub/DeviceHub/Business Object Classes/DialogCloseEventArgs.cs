using System;

namespace ASCOM.DeviceHub
{
	public class DialogCloseEventArgs : EventArgs
	{
		public bool? DialogResult { get; private set; }

		public DialogCloseEventArgs( bool? dialogResult)
		{
			DialogResult = dialogResult;
		}
	}
}
