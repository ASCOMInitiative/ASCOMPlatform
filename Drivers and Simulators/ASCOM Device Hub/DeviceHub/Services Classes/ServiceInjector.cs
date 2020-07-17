using System.Collections.Generic;
using System.Windows;

namespace ASCOM.DeviceHub
{
	public static class ServiceInjector
    {
		private static List<DialogContents> Dialogs { get; set; }

		static ServiceInjector()
		{
			Dialogs = new List<DialogContents>
			{
				new DialogContents( "ASCOM.DeviceHub.SetupView"
										, "ASCOM.DeviceHub.SetupViewModel" )
				, new DialogContents( "ASCOM.DeviceHub.ValuesEntryView"
										, "ASCOM.DeviceHub.DeclinationValuesEntryViewModel" )
				, new DialogContents( "ASCOM.DeviceHub.ValuesEntryView"
										, "ASCOM.DeviceHub.RightAscensionValuesEntryViewModel" )
				, new DialogContents( "ASCOM.DeviceHub.ValuesEntryView"
										, "ASCOM.DeviceHub.ScopeDomeOffsetViewModel" )
				, new DialogContents( "ASCOM.DeviceHub.ValuesEntryView"
										, "ASCOM.DeviceHub.SimpleValueEntryViewModel" )
				, new DialogContents( "ASCOM.DeviceHub.ValuesEntryView"
										, "ASCOM.DeviceHub.AzimuthValuesEntryViewModel" )
				, new DialogContents( "ASCOM.DeviceHub.ValuesEntryView"
										, "ASCOM.DeviceHub.AltitudeValuesEntryViewModel" )
				, new DialogContents( "ASCOM.DeviceHub.ValuesEntryView"
										, "ASCOM.DeviceHub.DomeAzimuthOffsetViewModel" )
				, new DialogContents( "ASCOM.DeviceHub.AboutView"
										, "ASCOM.DeviceHub.AboutViewModel" )
			};
		}

		// Loads service objects into the ServiceContainer on startup.

		public static void InjectServices()
		{
			// The ServiceContainer allows new concrete service instances to be
			// created and returned. To always return the same instance, pass that instance when
			// registering the service. To return a different instance each time one is requested,
			// pass the type of the concrete implementation to AddService.

			//Examples:

			// Singleton( the same instance is returned each time a service reference is requested.
			// ServiceContainer.Instance.AddService<ISpeechService>( new SpeechService() );

			// Unique( a different, unique instance is returned each time a service reference is requested.
			// ServiceContainer.Instance.AddService<IVCurvePointService>( typeof(DummyFocusPointGenerator) );

            ServiceContainer.Instance.AddService<ITelescopeService>( typeof( TelescopeService ) );
			ServiceContainer.Instance.AddService<IDomeService>( typeof( DomeService ) );
			ServiceContainer.Instance.AddService<IFocuserService>( typeof( FocuserService ) );
			ServiceContainer.Instance.AddService( AppSettingsService.Instance );
        }

        public static void InjectUIServices( Window parentWindow )
		{
			ServiceContainer.Instance.AddService<IDialogService>( new DialogService( parentWindow, Dialogs ) );
			ServiceContainer.Instance.AddService<IMessageBoxService>( new MessageBoxService( parentWindow ) );
            ServiceContainer.Instance.AddService<IActivityLogService>( new ActivityLogService( parentWindow ) );
		}

		public static void Clear()
		{
			ServiceContainer.Instance.ClearAllServices();
		}
	}
}
