using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Win32;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public static class Server
	{
		#region Private Data

		private static int _objsInUse;                      // Keeps a count on the total number of objects alive.
		private static int _scopesInUse;                    // Keeps a count on the total number of telescopes alive.
		private static int _domesInUse;                     // Keeps a count on the total number of domes alive.
		private static int _focusersInUse;                  // Keeps a count on the total number of focusers alive.
		private static int _serverLocks;                    // Keeps a lock count on this application.
		private static List<Assembly> _comObjectAssys;      // Dynamically loaded assemblies containing served COM objects
		private static List<Type> _comObjectTypes;          // Served COM object types
		private static List<ClassFactory> _classFactories;  // Served COM object class factories
		private static readonly string _appId = "{4f90ea04-044f-444e-963e-b52db2a87575}";   // Our AppId
		private static readonly Object _lockObject = new object();
		private const string _uacRegistryKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
		private const string _uacRegistryValue = "EnableLUA";


		private static MainWindow MainWindow { get; set; }          // Reference to the main view
		private static MainWindowViewModel ViewModel { get; set; }  // Reference to the main view model

		#endregion Private Data

		#region Private Properties

		private static Task GCTask { get; set; } // The garbage collection task

		// Used to end periodic garbage collection.

		private static CancellationTokenSource GCTokenSource { get; set; }

		#endregion Private Properties

		#region Public Properties

		// Used to tell if started by COM or manually

		public static bool StartedByCOM { get; private set; }   // True if server started by COM (-embedding)
		public static int ScopesInUse => _scopesInUse;
		public static int DomesInUse => _domesInUse;
		public static int FocusersInUse => _focusersInUse;

		#endregion Public Properties

		#region Startup Method

		internal static void Startup( string[] args )
		{
			// Uncomment the following lines to allow the Visual Studio Debugger to be 
			// attached to the server for debugging.

			//int procId = Process.GetCurrentProcess().Id;
			//string msg = String.Format( "Attach the debugger to process #{0} now.", procId );
			//MessageBox.Show( msg );

			Globals.UISyncContext = TaskScheduler.FromCurrentSynchronizationContext();

			LoadComObjectAssemblies();   // Load served COM class assemblies, get types

			if ( !ProcessArguments( args ) )    // Register/Unregister
			{
				App.Current.Shutdown();

				return;
			}

			// Now that we are past the registration code, make sure that we are NOT
			// running As Administrator.

			if ( IsElevated && IsUacEnabled )
			{
				MessageBox.Show( "You cannot run this application with elevated access...Terminating!" );
				App.Current.Shutdown();

				return;
			}

			// Initialize critical member variables.

			_objsInUse = 0;
			_scopesInUse = 0;
			_domesInUse = 0;
			_focusersInUse = 0;
			_serverLocks = 0;

			// Application Startup

			// Initialize our non-U/I services.

			ServiceInjector.InjectServices();

			// Create Device Hub App Settings and seed with default values.

			AppSettingsManager.CreateInitialAppSettings();

			// Load the saved position of the main window. Do this before the
			// window object is created so that the code-behind can pick up
			// the location.			

			AppSettingsManager.LoadMainWindowSettings();

			// Create the View and ViewModel

			ViewModel = new MainWindowViewModel();
			MainWindow = new MainWindow();

			// Create the U/I services.

			ServiceInjector.InjectUIServices( MainWindow );

			MainWindow.DataContext = ViewModel;
			MainWindow.Closing += MainWindow_Closing;

			// Load the saved settings to ensure that everyone up-to-date. Be sure to do this
			// after the main window is created so we can set its location.

			AppSettingsManager.LoadAppSettings();

			LoadDeviceSettings();

			// Register the class factories of the served objects

			RegisterClassFactories();

			StartGarbageCollection( 10000 );    // Collect garbage every 10 seconds.

			try
			{
				ShowMainWindow();
			}	
			finally
			{
				AppSettingsManager.SaveAppSettings();

				// Update the dome settings to save the azimuth adjustment.

				SaveDomeSettings();

				// Revoke the class factories immediately.
				// Don't wait until the thread has stopped before
				// we perform revocation!!!

				RevokeClassFactories();

				MainWindow.DataContext = null;
				MainWindow = null;
				ViewModel.Dispose();
				ViewModel = null;

				ServiceContainer.Instance.ClearAllServices();

				// Now stop the Garbage Collector task.

				StopGarbageCollection();
			}
		}

		#endregion  Startup Method

		#region Server Lock, Object Counting, and AutoQuit on COM startup

		// Returns the total number of objects alive currently.

		public static int ObjectsCount
		{
			get
			{
				lock ( _lockObject )
				{
					return _objsInUse;
				}
			}
		}

		// This method performs a thread-safe incrementation of the objects count.

		public static int CountObject( DeviceTypeEnum devType )
		{
			int retval;

			// Increment the count of objects for the specified device type.

			if ( devType == DeviceTypeEnum.Telescope )
			{
				Interlocked.Increment( ref _scopesInUse );
			}
			else if ( devType == DeviceTypeEnum.Dome )
			{
				Interlocked.Increment( ref _domesInUse );
			}
			else if ( devType == DeviceTypeEnum.Focuser )
			{
				Interlocked.Increment( ref _focusersInUse );
			}

			// Increment the global count of objects.

			retval = Interlocked.Increment( ref _objsInUse );

			Messenger.Default.Send( new ObjectCountMessage( _scopesInUse, _domesInUse, _focusersInUse ) );

			return retval;
		}

		// This method performs a thread-safe decrementation the objects count.

		public static int UncountObject( DeviceTypeEnum devType )
		{
			int retval;

			// Decrement the count of objects for the specified device type.

			if ( devType == DeviceTypeEnum.Telescope )
			{
				Interlocked.Decrement( ref _scopesInUse );
			}
			else if ( devType == DeviceTypeEnum.Dome )
			{
				Interlocked.Decrement( ref _domesInUse );
			}
			else if ( devType == DeviceTypeEnum.Focuser )
			{
				Interlocked.Decrement( ref _focusersInUse );
			}

			// Decrement the global count of objects.

			retval = Interlocked.Decrement( ref _objsInUse );

			Messenger.Default.Send( new ObjectCountMessage( _scopesInUse, _domesInUse, _focusersInUse ) );

			return retval;
		}

		// Returns the current server lock count.

		public static int ServerLockCount
		{
			get
			{
				lock ( _lockObject )
				{
					return _serverLocks;
				}
			}
		}

		// This method performs a thread-safe incrementation the 
		// server lock count.

		public static int CountLock()
		{
			// Increment the global lock count of this server.

			return Interlocked.Increment( ref _serverLocks );
		}

		// This method performs a thread-safe decrementation the 
		// server lock count.

		public static int UncountLock()
		{
			// Decrement the global lock count of this server.

			return Interlocked.Decrement( ref _serverLocks );
		}

		// ExitIf() will check to see if the objects count and the server 
		// lock count have both dropped to zero.
		//
		// If so, and if we were started by COM, we fire a Shutdown event to terminate
		// the app.

		public static void ExitIf()
		{
			lock ( _lockObject )
			{
				if ( ( ObjectsCount <= 0 ) && ( ServerLockCount <= 0 ) )
				{
					if ( StartedByCOM )
					{
						Task.Factory.StartNew( () =>
						{
							App.Current.Shutdown();
						}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
					}
				}
			}
		}

		public static void DisconnectTelescopeIf()
		{
			Task discon = new Task( () =>
			{
				if ( _scopesInUse == 1 )
				{
					TelescopeManager.Instance.Disconnect();
				}
			} );

			discon.Start( Globals.UISyncContext );
			discon.Wait();
		}

		public static void DisconnectDomeIf()
		{
			Task discon = new Task( () =>
			{
				if ( _domesInUse == 1 )
				{
					DomeManager.Instance.Disconnect();
				}
			} );

			discon.Start( Globals.UISyncContext );
			discon.Wait();
		}

		public static void DisconnectFocuserIf()
		{
			Task discon = new Task( () =>
			{
				if ( _focusersInUse == 1 )
				{
					FocuserManager.Instance.Disconnect();
				}
			} );

			discon.Start( Globals.UISyncContext );
			discon.Wait();
		}

		#endregion Server Lock, Object Counting, and AutoQuit on COM startup

		#region Helper Methods

		private static void LoadDeviceSettings()
		{
			TelescopeSettings scopeSettings = TelescopeSettings.FromProfile();
			TelescopeManager.SetTelescopeID( scopeSettings.TelescopeID );

			DomeSettings domeSettings = DomeSettings.FromProfile();
			DomeManager.SetDomeID( domeSettings.DomeID );
			Globals.DomeLayout = domeSettings.DomeLayout;
			Globals.DomeAzimuthAdjustment = domeSettings.AzimuthAdjustment;
			Globals.UsePOTHDomeSlaveCalculation = domeSettings.UsePOTHDomeSlaveCalculation;

			FocuserSettings focuserSettings = FocuserSettings.FromProfile();
			FocuserManager.SetFocuserID( focuserSettings.FocuserID );
			Globals.FocuserTemperatureOffset = focuserSettings.TemperatureOffset;
		}

		private static void SaveDomeSettings()
		{
			DomeSettings domeSettings = DomeSettings.FromProfile();
			domeSettings.AzimuthAdjustment = Globals.DomeAzimuthAdjustment;
			domeSettings.UsePOTHDomeSlaveCalculation = Globals.UsePOTHDomeSlaveCalculation;
			domeSettings.ToProfile();
		}

		private static void MainWindow_Closing( object sender, System.ComponentModel.CancelEventArgs e )
		{
			bool cancelClosing = false;

			IMessageBoxService msgSvc = ServiceContainer.Instance.GetService<IMessageBoxService>();

			// Check for connected client apps.

			if ( ObjectsCount > 0 )
			{
				string text = String.Format( "Device Hub has {0} client connection{1}. Are you sure that you want to shut down?"
											, ObjectsCount, ( ObjectsCount > 1 ) ? "s" : "" );
				string title = "Confirm Forced Shutdown";

				MessageBoxResult result = msgSvc.Show( text, title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.None );

				cancelClosing = ( result != MessageBoxResult.Yes );
			}
			else if ( !StartedByCOM ) // Check for device connections
			{
				bool deviceConnections = TelescopeManager.Instance.Connected
										|| DomeManager.Instance.Connected
										|| FocuserManager.Instance.Connected;

				if ( deviceConnections )
				{
					string text = String.Format( "Device Hub is still connected to one or more devices. Are you sure that you want to shut down?", Server.ObjectsCount );
					string title = "Confirm Forced Shutdown";

					MessageBoxResult result = msgSvc.Show( text, title, MessageBoxButton.YesNo, MessageBoxImage.Question, System.Windows.MessageBoxResult.No, System.Windows.MessageBoxOptions.None );

					cancelClosing = ( result != MessageBoxResult.Yes );
				}
			}
			
			e.Cancel = cancelClosing;
		}

		private static void ShowMainWindow()
		{
			Messenger.Default.Send( new ObjectCountMessage( _scopesInUse, _domesInUse, _focusersInUse ) );
			MainWindow.AdjustWindowPlacement();
			MainWindow.ShowDialog();
		}

		private static void StartGarbageCollection( int interval )
		{
			// Start up the garbage collection thread.

			GarbageCollection garbageCollector = new GarbageCollection( interval );

			GCTokenSource = new CancellationTokenSource();
			GCTask = Task.Factory.StartNew( () => garbageCollector.GCWatch( GCTokenSource.Token ),
											GCTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default );
		}

		private static void StopGarbageCollection()
		{
			GCTokenSource.Cancel();
			GCTask.Wait();
			GCTask = null;
			GCTokenSource.Dispose();
			GCTokenSource = null;
		}

		private static void WaitForGarbageCollectionToStop()
		{
			Task task = Task.Run( () =>
			{
				while ( GCTask != null && GCTask.IsCanceled )
				{
					Thread.Sleep( 250 );
				}
			} );
		}

        #endregion Helper Methods

        #region Dynamic Driver Assembly Loader

        private static void LoadComObjectAssemblies()
		{
			_comObjectAssys = new List<Assembly>();
			_comObjectTypes = new List<Type>();

			// Put everything into one folder, the same as the server.

			string rootPath = Assembly.GetEntryAssembly().Location;
			rootPath = Path.GetDirectoryName( rootPath );

			DirectoryInfo d = new DirectoryInfo( rootPath );
			FileInfo[] files = d.GetFiles( "*.dll" );

			foreach ( FileInfo file in files )
			{
				string assemblyPath = file.FullName;

				// First try to load the assembly and get the types for
				// the class and the class factory. If this doesn't work ????

				try
				{
					Assembly assembly = Assembly.LoadFrom( assemblyPath );

					// Get the types in the assembly

					Type[] types = assembly.GetTypes();

					foreach ( Type type in types )
					{
						// Check to see if the type has the ServedClassName attribute, only use it if it does.

						object[] attributes = type.GetCustomAttributes( typeof( ServedClassNameAttribute ), false );

						if ( attributes.Length > 0 )
						{
							_comObjectTypes.Add( type );
							_comObjectAssys.Add( assembly );
						}
					}
				}
				catch ( BadImageFormatException )
				{
					// Probably an attempt to load a Win32 DLL (i.e. not a .net assembly)
					// Just swallow the exception and continue to the next item.

					continue;
				}
				catch ( Exception e )
				{
					string msg = String.Format( "Failed to load served COM class assembly {0} - {1}", file.Name, e.Message );
					throw new Exception( msg, e );
				}
			}
		}

		#endregion Dynamic Driver Assembly Loader

		#region COM Registration and Unregistration

		// Test if UAC is enabled.

		public static bool IsUacEnabled
		{
			get
			{
				using ( RegistryKey uacKey = Registry.LocalMachine.OpenSubKey( _uacRegistryKey, false ) )
				{
					bool result = uacKey.GetValue( _uacRegistryValue ).Equals( 1 );
					return result;
				}
			}
		}

		// Test if running elevated

		private static bool IsElevated
		{
			get
			{
				WindowsIdentity i = WindowsIdentity.GetCurrent();
				WindowsPrincipal p = new WindowsPrincipal( i );

				return p.IsInRole( WindowsBuiltInRole.Administrator );
			}
		}

		// Elevate by re-running ourselves with elevation dialog

		private static void ElevateSelf( string arg )
		{
			ProcessStartInfo si = new ProcessStartInfo
			{
				Arguments = arg,
				WorkingDirectory = Environment.CurrentDirectory,
				FileName = GetExecutablePath(),
				Verb = "runas"
			};

			try { Process.Start( si ); }

			catch ( System.ComponentModel.Win32Exception )
			{
				MessageBox.Show( "DeviceHub was not " + ( arg == "/register" ? "registered" : "unregistered" ) +
					" because you did not allow it.", "DeviceHub", MessageBoxButton.OK, MessageBoxImage.Warning );
			}
			catch ( Exception ex )
			{
				MessageBox.Show( ex.ToString(), "DeviceHub", MessageBoxButton.OK, MessageBoxImage.Stop );
			}

			return;
		}

		private static string GetExecutablePath()
		{
			string path = Assembly.GetEntryAssembly().Location;

			return path;
		}

		private static string GetExecutableFileName()
		{
			string path = GetExecutablePath();
			string name = Path.GetFileName( path );

			return name;
		}

		// Do everything to register this for COM. Never use REGASM on
		// this exe assembly! It would create InProcServer32 entries 
		// which would prevent proper activation!

		// Using the list of COM object types generated during dynamic
		// assembly loading, it registers each one for COM as served by our
		// exe/local server, as well as registering it for ASCOM. It also
		// adds DCOM info for the local server itself, so it can be activated
		// via an outbound connection from TheSky.

		private static void RegisterObjects( bool rerunningAsElevated = false )
		{
			if ( rerunningAsElevated )
			{
				if ( !IsElevated )
				{
					MessageBox.Show( "Unable to gain elevated privileges to register the Device Hub.\n\n"
						+ "Possible fixes are to enable User Account Control and/or add your user account\n"
						+ "to the local Administrators group. Then re-install the Device Hub",
						"DeviceHub", MessageBoxButton.OK, MessageBoxImage.Stop );

					return;
				}
			}
			else
			{
				if ( !IsElevated )
				{
					// Here we want to elevate a copy of ourselves, unless we are running under the VS Debugger.

					RerunAsAdministrator( "/regElevated" );

					return;
				}
			}

			// If reached here, we're running elevated

			Assembly assy = Assembly.GetExecutingAssembly();
			Attribute attr = Attribute.GetCustomAttribute( assy, typeof( AssemblyTitleAttribute ) );
			string assyTitle = ( (AssemblyTitleAttribute)attr ).Title;
			attr = Attribute.GetCustomAttribute( assy, typeof( AssemblyDescriptionAttribute ) );
			string assyDescription = ( (AssemblyDescriptionAttribute)attr ).Description;

			// Local server's DCOM/AppID information

			try
			{
				// HKCR\APPID\appid

				using ( RegistryKey key = Registry.ClassesRoot.CreateSubKey( "APPID\\" + _appId ) )
				{
					key.SetValue( null, assyDescription );
					key.SetValue( "AppID", _appId );
					key.SetValue( "AuthenticationLevel", 1, RegistryValueKind.DWord );
					key.SetValue( "RunAs", "Interactive User", RegistryValueKind.String ); // Added to ensure that only one copy of the local server ...
				}

					// HKCR\APPID\exename.ext

				using ( RegistryKey key = Registry.ClassesRoot.CreateSubKey( string.Format( "APPID\\{0}",
						GetExecutableFileName() ) ) )
				{
					key.SetValue( "AppID", _appId );
				}
			}
			catch ( Exception ex )
			{
				MessageBox.Show( "Error while registering the server:\n" + ex.ToString(),
						"DeviceHub", MessageBoxButton.OK, MessageBoxImage.Stop );

				return;
			}

			// For each of the driver assemblies

			foreach ( Type type in _comObjectTypes )
			{
				bool bFail = false;

				try
				{
					// HKCR\CLSID\clsid

					string clsid = Marshal.GenerateGuidForType( type ).ToString( "B" );
					string progid = Marshal.GenerateProgIdForType( type );

					//PWGS Generate device type from the Class name

					string deviceType = type.Name;

					using ( RegistryKey key = Registry.ClassesRoot.CreateSubKey( string.Format( "CLSID\\{0}", clsid ) ) )
					{
						key.SetValue( null, progid );                     // Could be assyTitle/Desc??, but .NET components show ProgId here
						key.SetValue( "AppId", _appId );

						using ( RegistryKey key2 = key.CreateSubKey( "Implemented Categories" ) )
						{
							key2.CreateSubKey( "{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}" );
						}

						using ( RegistryKey key2 = key.CreateSubKey( "ProgId" ) )
						{
							key2.SetValue( null, progid );
						}

						key.CreateSubKey( "Programmable" );

						using ( RegistryKey key2 = key.CreateSubKey( "LocalServer32" ) )
						{
							string path = GetExecutablePath();
							key2.SetValue( null, path );
						}
					}

					// HKCR\CLSID\progid

					using ( RegistryKey key = Registry.ClassesRoot.CreateSubKey( progid ) )
					{
						key.SetValue( null, assyTitle );
						using ( RegistryKey key2 = key.CreateSubKey( "CLSID" ) )
						{
							key2.SetValue( null, clsid );
						}
					}

					// ASCOM 

					assy = type.Assembly;

					// Pull the display name from the ServedClassName attribute.

					attr = Attribute.GetCustomAttribute( type, typeof( ServedClassNameAttribute ) ); //PWGS Changed to search type for attribute rather than assembly
					string chooserName = ( (ServedClassNameAttribute)attr ).DisplayName ?? "MultiServer";

					using ( var P = new ASCOM.Utilities.Profile() )
					{
						P.DeviceType = deviceType;
						P.Register( progid, chooserName );
					}
				}
				catch ( Exception ex )
				{
					MessageBox.Show( "Error while registering the server:\n" + ex.ToString(),
							"DeviceHub", MessageBoxButton.OK, MessageBoxImage.Stop );
					bFail = true;
				}
				finally
				{ }

				if ( bFail )
				{
					break;
				}
			}
		}

		private static void RerunAsAdministrator( string args )
		{
			if ( Debugger.IsAttached )
			{
				MessageBox.Show( "You must run Visual Studio As Administrator to debug an ASCOM driver.",
								"Run As Administrator", MessageBoxButton.OK, MessageBoxImage.Stop );
			}
			else
			{
				ElevateSelf( args );
			}
		}

		// Remove all traces of this from the registry. 

		private static void UnregisterObjects( bool rerunningAsElevated = false )
		{
			if ( rerunningAsElevated )
			{
				if ( !IsElevated )
				{
					MessageBox.Show( "Unable to gain elevated privileges to unregister the Device Hub.\n\n"
						+ "If User Account Control is disabled, please enable it before uninstalling the Device Hub",
						"DeviceHub", MessageBoxButton.OK, MessageBoxImage.Stop );

					return;
				}
			}
			else
			{
				if ( !IsElevated )
				{
					// Here we want to elevate a copy of ourselves, unless we are running under the VS Debugger.

					RerunAsAdministrator( "/unregElevated" );

					return;
				}
			}

			// Local server's DCOM/AppID information

			Registry.ClassesRoot.DeleteSubKey( String.Format( "APPID\\{0}", _appId ), false );
			Registry.ClassesRoot.DeleteSubKey( String.Format( "APPID\\{0}", GetExecutableFileName() ), false );

			// For each of the driver assemblies

			foreach ( Type type in _comObjectTypes )
			{
				string clsid = Marshal.GenerateGuidForType( type ).ToString( "B" );
				string progid = Marshal.GenerateProgIdForType( type );
				string deviceType = type.Name;

				// Best efforts

				// HKCR\progid

				Registry.ClassesRoot.DeleteSubKey( String.Format( "{0}\\CLSID", progid ), false );
				Registry.ClassesRoot.DeleteSubKey( progid, false );

				// HKCR\CLSID\clsid

				Registry.ClassesRoot.DeleteSubKey( String.Format( "CLSID\\{0}\\Implemented Categories\\{{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}}", clsid ), false );
				Registry.ClassesRoot.DeleteSubKey( String.Format( "CLSID\\{0}\\Implemented Categories", clsid ), false );
				Registry.ClassesRoot.DeleteSubKey( String.Format( "CLSID\\{0}\\ProgId", clsid ), false );
				Registry.ClassesRoot.DeleteSubKey( String.Format( "CLSID\\{0}\\LocalServer32", clsid ), false );
				Registry.ClassesRoot.DeleteSubKey( String.Format( "CLSID\\{0}\\Programmable", clsid ), false );
				Registry.ClassesRoot.DeleteSubKey( String.Format( "CLSID\\{0}", clsid ), false );

				try
				{
					// ASCOM

					using ( var P = new ASCOM.Utilities.Profile() )
					{
						P.DeviceType = deviceType;
						P.Unregister( progid );
					}
				}
				catch ( Exception ) { }
			}
		}

		#endregion  COM Registration and Unregistration

		#region Class Factory Support

		// On startup, we register the class factories of the COM objects
		// that we serve. This requires the class factory name to be
		// equal to the served class name + "ClassFactory".

		private static bool RegisterClassFactories()
		{
			_classFactories = new List<ClassFactory>();

			foreach ( Type type in _comObjectTypes )
			{
				ClassFactory factory = new ClassFactory( type );                  // Use default context & flags
				_classFactories.Add( factory );

				if ( !factory.RegisterClassObject() )
				{
					MessageBox.Show( "Failed to register class factory for " + type.Name,
						"DeviceHub", MessageBoxButton.OK, MessageBoxImage.Stop );

					return false;
				}
			}

			ClassFactory.ResumeClassObjects();                                  // Served objects now go live

			return true;
		}

		private static void RevokeClassFactories()
		{
			ClassFactory.SuspendClassObjects();                                 // Prevent race conditions

			foreach ( ClassFactory factory in _classFactories )
			{
				factory.RevokeClassObject();
			}
		}

		#endregion Class Factory Support

		#region Command Line Arguments

		// ProcessArguments() will process the command-line arguments
		// If the return value is true, we carry on and start this application.
		// If the return value is false, we terminate this application immediately.

		private static bool ProcessArguments( string[] args )
		{
			bool retval = true;

			if ( args.Length > 0 )
			{
				switch ( args[0].ToLower() )
				{
					case "-embedding":
						StartedByCOM = true;                                        // Indicate COM started us

						break;

					case "-register":
					case @"/register":
					case "-regserver":                                          // Emulate VB6
					case @"/regserver":
						RegisterObjects();                                      // Register each served object
						retval = false;

						break;

					case "-unregister":
					case @"/unregister":
					case "-unregserver":                                        // Emulate VB6
					case @"/unregserver":
						UnregisterObjects();                                    //Unregister each served object
						retval = false;

						break;

					case "-regElevated":
						RegisterObjects( true );

						break;

					case "-unregElevated":
						UnregisterObjects( true );

						break;
					default:
						MessageBox.Show( "Unknown argument: " + args[0] + "\nValid are : -register, -unregister and -embedding",
							"DeviceHub", MessageBoxButton.OK, MessageBoxImage.Exclamation );

						break;
				}
			}
			else
			{
				StartedByCOM = false;
			}

			return retval;
		}

		#endregion Command Line Arguments
	}
}
