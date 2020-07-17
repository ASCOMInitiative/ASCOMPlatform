using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ASCOM.DeviceHub
{
    /// <summary>
    /// Interaction logic for ActivityView.xaml
    /// </summary>
    public partial class ActivityLogView : Window
    {
        public ActivityLogView()
        {
            InitializeComponent();
        }

        private void TextBox_TargetUpdated( object sender, DataTransferEventArgs e )
        {
			if ( sender is TextBox tb )
			{
				tb.CaretIndex = tb.Text.Length;
				tb.ScrollToEnd();
			}
			else if (sender is ScrollViewer sv)
			{
				sv.ScrollToEnd();
			}
		}

        private void Window_Loaded( object sender, RoutedEventArgs e )
        {
            if ( DataContext is ActivityLogViewModel vm )
            {
				vm.RequestClose += OnRequestClose;

                this.DisableButtons();
            }
        }

		private void OnRequestClose( object sendeer, DialogCloseEventArgs e )
		{
			//DialogResult = e.DialogResult;
			Close();
		}

		private void Window_LocationChanged( object sender, EventArgs e )
		{
			if ( sender is Window activity )
			{
				Globals.ActivityWindowLeft = activity.Left;
				Globals.ActivityWindowTop = activity.Top;
			}
		}

		private void Window_SizeChanged( object sender, SizeChangedEventArgs e )
		{
			if ( sender is Window activity )
			{
				Globals.ActivityWindowWidth = activity.Width;
				Globals.ActivityWindowHeight = activity.Height;
			}
		}
	}
}
