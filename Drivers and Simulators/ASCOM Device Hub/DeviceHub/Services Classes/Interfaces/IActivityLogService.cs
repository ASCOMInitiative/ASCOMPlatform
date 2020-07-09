using System;

namespace ASCOM.DeviceHub
{
	public interface IActivityLogService
    {
        void Show( ActivityLogViewModel vm, double left, double top, double width, double height );
        void RequestClose( object sender, EventArgs e );
    }
}
