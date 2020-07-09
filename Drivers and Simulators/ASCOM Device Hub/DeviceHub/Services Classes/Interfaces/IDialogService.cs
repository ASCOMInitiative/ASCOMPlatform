using System;

namespace ASCOM.DeviceHub
{
	interface IDialogService
	{
		void Show( DeviceHubDialogViewModelBase viewModel, EventHandler closedHandler );
		bool? ShowDialog( DeviceHubDialogViewModelBase viewModel );
	}
}
