using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
    public class ActivityLogService : IActivityLogService
    {
        private ActivityLogViewModel ViewModel { get; set; }
        private Window View { get; set; }
        private Window ParentWindow { get; set; }

        public ActivityLogService(Window parent)
        {
            ParentWindow = parent;
            View = null;
            ViewModel = null;
        }

        /// <summary>
        /// Create the activity log dialogue
        /// </summary>
        /// <param name="vm">Activity log view model</param>
        /// <param name="left">X coordinate of the activity log dialogue top left corner</param>
        /// <param name="top">Y coordinate of the activity log dialogue top left corner</param>
        /// <param name="width">Activity log dialogue width</param>
        /// <param name="height">Activity log dialogue height</param>
        public void CreateActivityLog(ActivityLogViewModel vm, double left, double top, double width, double height)
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

            View.Hide();
            View.AdjustWindowPlacement();
        }

        /// <summary>
        /// Make the activity log dialogue visible
        /// </summary>
        public void ShowActivityLog()
        {
            Task.Factory.StartNew(() =>
            {
                View.Show();
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Make the activity log dialogue invisible
        /// </summary>
        public void HideActivityLog()
        {
            Task.Factory.StartNew(() =>
            {
                View.Hide();
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Clean up when the activity log closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RequestClose(object sender, EventArgs e)
        {
            ViewModel.RequestClose -= RequestClose;
            View.DataContext = null;
            View = null;
            ViewModel.Dispose();
            ViewModel = null;

            Messenger.Default.Send(new ActivityLogClosedMessage());
        }
    }
}
