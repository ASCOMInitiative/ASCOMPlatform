using System;
using System.Windows;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public class ActivityLogService : IActivityLogService
    {
        private bool Initialized { get; set; }
        private ActivityLogViewModel ViewModel { get; set; }
        private Window View { get; set; }
        private Window ParentWindow { get; set; }

        public ActivityLogService( Window parent)
        {
            ParentWindow = parent;
            Initialized = false;
			View = null;
            ViewModel = null;
        }

        public void Show( ActivityLogViewModel vm, double left, double top, double width, double height )
        {
            ViewModel = vm;
            ViewModel.IsActive = true;
			ViewModel.RequestClose += RequestClose;

			View = new ActivityLogView
			{
				Owner = ParentWindow,
				Left = left,
				Top = top,
				Width = width,
				Height = height,
				DataContext = ViewModel
			};
			View.Show();
            View.AdjustWindowPlacement();
        }

        public void RequestClose( object sender, EventArgs e )
        {
			ViewModel.RequestClose -= RequestClose;
			View.DataContext = null;
			View = null;
			ViewModel.Dispose();
			ViewModel = null;

			Messenger.Default.Send( new ActivityLogClosedMessage() );
        }
    }
}
