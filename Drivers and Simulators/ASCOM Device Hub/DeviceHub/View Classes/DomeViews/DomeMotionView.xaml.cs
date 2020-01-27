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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ASCOM.DeviceHub
{
    /// <summary>
    /// Interaction logic for DomeMotionView.xaml
    /// </summary>
    public partial class DomeMotionView : UserControl
    {
		private List<Expander> _expanders;

        public DomeMotionView()
        {
            InitializeComponent();

			_expanders = new List<Expander>
			{
				_motionExpander,
				_otherActionsExpander
			};
		}

		private void Expander_Expanded( object sender, RoutedEventArgs e )
		{
			if ( !( sender is Expander sendingExpander ) || _expanders == null )
			{
				return;
			}

			foreach ( Expander expander in _expanders )
			{
				if ( expander != sendingExpander && expander.IsExpanded )
				{
					expander.IsExpanded = false;
				}
			}
		}
	}
}
