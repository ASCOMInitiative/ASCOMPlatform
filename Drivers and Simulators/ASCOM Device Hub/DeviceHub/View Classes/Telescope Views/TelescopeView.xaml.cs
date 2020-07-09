using System;
using System.Windows.Controls;

namespace ASCOM.DeviceHub
{
	/// <summary>
	/// Interaction logic for TelescopeView.xaml
	/// </summary>
	public partial class TelescopeView : UserControl
	{
		public TelescopeView()
		{
			InitializeComponent();
		}

		private void TabControl_SelectionChanged( object sender, SelectionChangedEventArgs e )
		{
			if ( sender is TabControl ctrl )
			{
				if ( ctrl.SelectedItem is TabItem item )
				{
					string label = item.Header.ToString();
					GetViewModel().ChangeActiveFunction( label );
				}
			}
		}

		private TelescopeViewModel GetViewModel()
		{
			return this.DataContext as TelescopeViewModel;
		}
	}
}
