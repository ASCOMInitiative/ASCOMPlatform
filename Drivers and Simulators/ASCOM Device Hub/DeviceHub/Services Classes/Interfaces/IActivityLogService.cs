using System;

namespace ASCOM.DeviceHub
{
	public interface IActivityLogService
    {
        void CreateActivityLog( ActivityLogViewModel vm, double left, double top, double width, double height );
        void ShowActivityLog();
        void HideActivityLog();
        void RequestClose( object sender, EventArgs e );
    }
}
