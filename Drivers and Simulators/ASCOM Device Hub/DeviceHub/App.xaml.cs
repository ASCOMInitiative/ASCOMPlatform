using System;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Windows;

namespace ASCOM.DeviceHub
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup( StartupEventArgs e )
		{
			base.OnStartup( e );

			try
			{
				// Before we proceed to start up, make sure that there is not another instance of DeviceHub already running.

				string mutexName = "Global\\" + GetProductGuid();
				bool createdNew;

				MutexAccessRule allowEveryoneRule = new MutexAccessRule( new SecurityIdentifier( WellKnownSidType.WorldSid, null)
																							   , MutexRights.FullControl
																							   , AccessControlType.Allow );
				MutexSecurity securitySettings = new MutexSecurity();
				securitySettings.AddAccessRule( allowEveryoneRule );

				using ( Mutex mutex = new Mutex( false, mutexName, out createdNew, securitySettings ) )
				{
					bool hasHandle = false;

					try
					{
						try
						{
							hasHandle = mutex.WaitOne( 0, false );

							if ( !hasHandle )
							{
								MessageBox.Show( "Another instance of the Device Hub is already running...Exiting!", "Second Instance Error" );
								App.Current.Shutdown();
							}
						}
						catch ( AbandonedMutexException )
						{ }

						// Launch the ASCOM Local Server

						Server.Startup( e.Args );
					}
					finally
					{
						if ( hasHandle )
						{
							mutex.ReleaseMutex();
						}
					}
				}
			}
			catch ( Exception xcp )
			{
				string msg = String.Format( "DeviceHub caught a fatal exception during startup\r\n{0}", xcp.Message );
				MessageBox.Show( msg, "DeviceHub", MessageBoxButton.OK, MessageBoxImage.Stop );

				if ( Application.Current != null )
				{
					Application.Current.Shutdown();
				}
			}
		}

		private static string GetProductGuid()
		{
			string retval = "";

			AssemblyGuidAttribute attribute = GetAssemblyAttribute<AssemblyGuidAttribute>(false);

			if ( attribute != null )
			{
				retval = attribute.Guid;
			}

			return retval;
		}

		private static T GetAssemblyAttribute<T>( bool inherit )
		{
			Assembly assembly = Assembly.GetEntryAssembly();

			return (T)assembly.GetCustomAttributes( typeof( T ), inherit ).First();
		}


	}
}
