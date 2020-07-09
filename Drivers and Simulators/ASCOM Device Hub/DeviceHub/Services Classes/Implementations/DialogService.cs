using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ASCOM.DeviceHub
{
	class DialogService : IDialogService
	{
		private Window ParentWindow { get; set; }
		private IEnumerable<DialogContents> Contents { get; set; }

		public DialogService( IEnumerable<DialogContents> contents )
			: this( null, contents )
		{ }

		public DialogService( Window parentWindow, IEnumerable<DialogContents> contents )
		{
			ParentWindow = parentWindow;
			Contents = contents;
		}

		public void Show( DeviceHubDialogViewModelBase viewModel, EventHandler closedHandler )
		{
			LaunchDialog( viewModel, false, closedHandler );
		}

		public bool? ShowDialog( DeviceHubDialogViewModelBase viewModel )
		{
			return LaunchDialog( viewModel, true, null );
		}

		private bool? LaunchDialog( DeviceHubDialogViewModelBase viewModel, bool isModal, EventHandler closedHandler )
		{
			bool? retval = null;

			DeviceHubDialogView view = GetDialogView( viewModel );
			view.DataContext = viewModel;
			view.Owner = ParentWindow;

			if ( isModal )
			{
				viewModel.RequestClose += view.OnRequestClose;
				retval = view.ShowDialog();

				viewModel.RequestClose -= view.OnRequestClose;
				view.DataContext = null;
				view = null;
			}
			else
			{
				if ( closedHandler != null )
				{
					viewModel.NotifyClosed += closedHandler;
				}

				view.Closed += ( o, a ) => { viewModel.NotifyClosedSubscribers(); };
				view.Show();
				viewModel.Activate();
			}

			return retval;
		}

		private DeviceHubDialogView GetDialogView( DeviceHubDialogViewModelBase viewModel )
		{
			if ( Contents == null )
			{
				throw new Exception( "Dialog Service contents are null!" );
			}

			string viewModelClassname = viewModel.GetType().FullName;
			DialogContents item = Contents.Where( c => c.ViewModelClassname == viewModelClassname ).FirstOrDefault();

			if ( item == null )
			{
				throw new Exception( String.Format( "Invalid classname, {0), passed to DialogService.LaunchDialog().", viewModelClassname ) );
			}

			Type viewType = Type.GetType( item.ViewClassname );
			DeviceHubDialogView view = (DeviceHubDialogView)Activator.CreateInstance( viewType );
			view.Owner = ParentWindow;

			return view;
		}
	}
}
