//
// This is the core of a managed COM Local Server, capable of serving
// multiple instances of multiple interfdaces, within a single
// executable. This implementes the equivalent functionality of VB6
// which has been extensively used in ASCOM for drivers that provide
// multiple interfaces to multiple clients (e.g. Meade Telescope
// and Focuser) as well as hubs (e.g., POTH).
//
// It seems like this is working as advertised, but it needs to be 
// tested and verified for behavior like VB6 -- namely, that it funnels
// incoming calls from clients through a single thread (the message loop).
// On the face of it, it must - but who knows what gremlins live in the
// .NET framework and COM Interop that could become a gotcha.
//
//
//
using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
using Microsoft.Win32;
using System.Text;
using System.Threading;

namespace ASCOM.LocalServerCOM
{
	//
	// Note that class LocalServerCOM is NOT declared as public.
	// This prevents it from being registered for COM by REGASM
	// and TLBEXP. These tools shoould NEVER be used on this 
	// server anyway, but...
	//
	class LocalServerCOM
	{

		#region Access to kernel32.dll, user32.dll, and ole32.dll functions
		[Flags]
		enum CLSCTX : uint
		{
			CLSCTX_INPROC_SERVER = 0x1,
			CLSCTX_INPROC_HANDLER = 0x2,
			CLSCTX_LOCAL_SERVER = 0x4,
			CLSCTX_INPROC_SERVER16 = 0x8,
			CLSCTX_REMOTE_SERVER = 0x10,
			CLSCTX_INPROC_HANDLER16 = 0x20,
			CLSCTX_RESERVED1 = 0x40,
			CLSCTX_RESERVED2 = 0x80,
			CLSCTX_RESERVED3 = 0x100,
			CLSCTX_RESERVED4 = 0x200,
			CLSCTX_NO_CODE_DOWNLOAD = 0x400,
			CLSCTX_RESERVED5 = 0x800,
			CLSCTX_NO_CUSTOM_MARSHAL = 0x1000,
			CLSCTX_ENABLE_CODE_DOWNLOAD = 0x2000,
			CLSCTX_NO_FAILURE_LOG = 0x4000,
			CLSCTX_DISABLE_AAA = 0x8000,
			CLSCTX_ENABLE_AAA = 0x10000,
			CLSCTX_FROM_DEFAULT_CONTEXT = 0x20000,
			CLSCTX_INPROC = CLSCTX_INPROC_SERVER | CLSCTX_INPROC_HANDLER,
			CLSCTX_SERVER = CLSCTX_INPROC_SERVER | CLSCTX_LOCAL_SERVER | CLSCTX_REMOTE_SERVER,
			CLSCTX_ALL = CLSCTX_SERVER | CLSCTX_INPROC_HANDLER
		}

		[Flags]
		enum COINIT : uint
		{
			/// Initializes the thread for multi-threaded object concurrency.
			COINIT_MULTITHREADED = 0x0,
			/// Initializes the thread for apartment-threaded object concurrency. 
			COINIT_APARTMENTTHREADED = 0x2,
			/// Disables DDE for Ole1 support.
			COINIT_DISABLE_OLE1DDE = 0x4,
			/// Trades memory for speed.
			COINIT_SPEED_OVER_MEMORY = 0x8
		}

		[Flags]
		enum REGCLS : uint
		{
			REGCLS_SINGLEUSE = 0,
			REGCLS_MULTIPLEUSE = 1,
			REGCLS_MULTI_SEPARATE = 2,
			REGCLS_SUSPENDED = 4,
			REGCLS_SURROGATE = 8
		}


		// CoInitializeEx() can be used to set the apartment model
		// of individual threads.
		[DllImport("ole32.dll")]
		static extern int CoInitializeEx(IntPtr pvReserved, uint dwCoInit);

		// CoUninitialize() is used to uninitialize a COM thread.
		[DllImport("ole32.dll")]
		static extern void CoUninitialize();

		// PostThreadMessage() allows us to post a Windows Message to
		// a specific thread (identified by its thread id).
		// We will need this API to post a WM_QUIT message to the main 
		// thread in order to terminate this application.
		[DllImport("user32.dll")]
		static extern bool PostThreadMessage(uint idThread, uint Msg, UIntPtr wParam,
			IntPtr lParam);

		// GetCurrentThreadId() allows us to obtain the thread id of the
		// calling thread. This allows us to post the WM_QUIT message to
		// the main thread.
		[DllImport("kernel32.dll")]
		static extern uint GetCurrentThreadId();
		#endregion

		protected static uint		m_uiMainThreadId;			// Stores the main thread's thread id.
		protected static int		m_iObjsInUse;				// Keeps a count on the total number of objects alive.
		protected static int		m_iServerLocks;				// Keeps a lock count on this application.
		protected static bool		m_bComStart;				// True if server started by COM (-embedding)
		protected static frmMain	m_MainForm = null;			// Reference to our main form
		protected static ArrayList	m_ComObjectAssys;			// Dynamically loaded assemblies containing served COM objects
		protected static ArrayList	m_ComObjectTypes;			// Served COM object types
		protected static ArrayList  m_ClassFactories;			// Served COM object class factories

		// This property returns the main thread's id.
		public static uint MainThreadId { get { return m_uiMainThreadId;  }	}

		// Used to tell if started by COM or manually
		public static bool StartedByCOM { get { return m_bComStart; } }

		#region Server Lock, Object Counting, and AutoQuit on COM startup
		// Returns the total number of objects alive currently.
		public static int ObjectsCount
		{
			get
			{
				lock(typeof(LocalServerCOM))
				{
					return m_iObjsInUse;
				}
			}
		}

		// This method performs a thread-safe incrementation of the objects count.
		public static int CountObject()
		{
			Trace("CountObject()");
			// Increment the global count of objects.
			return Interlocked.Increment(ref m_iObjsInUse);
		}

		// This method performs a thread-safe decrementation the objects count.
		public static int UncountObject()
		{
			Trace("UncountObject()");
			// Decrement the global count of objects.
			return Interlocked.Decrement(ref m_iObjsInUse);
		}

		// Returns the current server lock count.
		public static int ServerLockCount
		{
			get
			{
				lock(typeof(LocalServerCOM))
				{
					return m_iServerLocks;
				}
			}
		}

		// This method performs a thread-safe incrementation the 
		// server lock count.
		public static int CountLock()
		{
			Trace("CountLock()");
			// Increment the global lock count of this server.
			return Interlocked.Increment(ref m_iServerLocks);
		}

		// This method performs a thread-safe decrementation the 
		// server lock count.
		public static int UncountLock()
		{
			Trace("UncountLock()");
			// Decrement the global lock count of this server.
			return Interlocked.Decrement(ref m_iServerLocks);
		}

		// AttemptToTerminateServer() will check to see if the objects count and the server 
		// lock count have both dropped to zero.
		//
		// If so, and if we were started by COM, we post a WM_QUIT message to the main thread's
		// message loop. This will cause the message loop to exit and hence the termination 
		// of this application. If hand-started, then just trace that it WOULD exit now.
		//
		public static void ExitIf()
		{
			lock(typeof(LocalServerCOM))
			{
				Trace("ExitIf()");

				// Get the most up-to-date values of these critical data.
				int iObjsInUse = ObjectsCount;
				int iServerLocks = ServerLockCount;

				// Print out these info for debug purposes.
				StringBuilder sb = new StringBuilder("");		  
				sb.AppendFormat("m_iObjsInUse : {0}. m_iServerLocks : {1}", iObjsInUse, iServerLocks);
				Trace(sb.ToString());

				if ((iObjsInUse > 0) || (iServerLocks > 0))
				{
					Trace("There are still referenced objects or the server lock count is non-zero.");
				}
				else
				{
					if (m_bComStart)
					{
						UIntPtr wParam = new UIntPtr(0);
						IntPtr lParam = new IntPtr(0);
						Trace("Com started - PostThreadMessage(WM_QUIT)");
						PostThreadMessage(MainThreadId, 0x0012, wParam, lParam);
					}
					else
						Trace("Server would exit now if started by COM");
				}
			}
		}
		#endregion

		//
		// Trace activity to textbox on form. This is really cheezy, but
		// heck, this is a sample anyway. Normally, the main form would
		// have controls or something.
		//
		public static void Trace(string Msg)
		{
			// **TODO** Queue Trace() messages before m_MainForm is real
			if (m_MainForm != null) m_MainForm.Trace(Msg);
			// MessageBox.Show(Msg);
		}

		// -----------------
		// PRIVATE FUNCTIONS
		// -----------------

		//
		// Load the assemblies that contain the classes that we will serve
		// via COM. These will be located in the subfolder ServedClasses
		// below our executable. The code below takes care of the situation
		// where we're running in the VS.NET IDE, allowing the ServedClasses
		// folder to be in the solution folder, while we are executing in
		// the LocalServerCOM\bin\Debug subfolder.
		//
		protected static bool LoadComObjectAssemblies()
		{
			m_ComObjectAssys = new ArrayList();
			m_ComObjectTypes = new ArrayList();

			string assyPath = Assembly.GetEntryAssembly().Location;
			int i = assyPath.LastIndexOf(@"\LocalServerCOM\bin\");					// Look for us running in IDE
			if (i == -1) i = assyPath.LastIndexOf('\\');
			assyPath = assyPath.Remove(i, assyPath.Length - i) + "\\ServedClasses";

			DirectoryInfo d = new DirectoryInfo(assyPath);
			foreach (FileInfo fi in d.GetFiles("*.dll"))
			{
				string aPath = fi.FullName;
				string fqClassName = "ASCOM.LocalServerCOM." + fi.Name.Replace(fi.Extension, "");	// COM class FQN
				//
				// First try to load the assembly and get the types for
				// the class and the class facctory. If this doesn't work ????
				//
				try
				{
					Assembly so = Assembly.LoadFrom(aPath);
					m_ComObjectTypes.Add(so.GetType(fqClassName, true));
					m_ComObjectAssys.Add(so);
				}
				catch(Exception e)
				{
					MessageBox.Show("Failed to load served COM class assembly " + fi.Name + " - " + e.Message,
						"LocalServerCOM", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					return false;
				}

			}
			return true;
		}
		//
		// Do everything to register this for COM. Never use REGASM on
		// this exe assembly! It would create InProcServer32 entries 
		// which would prevent proper activation!
		//
		// This finds all of the types that are subclasses of 
		// ReferenceCountedObjectBase, namely those types that are the
		// served COM objects. It registers each one as served by our
		// exe/local server.
		//
		// **TODO** Should this also create AppID/DCOM junk for TheSky?
		// Probably...
		//
		protected static void RegisterObjects()
		{
			RegistryKey key = null;
			RegistryKey key2 = null;
			RegistryKey key3 = null;

			Assembly assy = Assembly.GetExecutingAssembly();
			Attribute attr = Attribute.GetCustomAttribute(assy, typeof(AssemblyTitleAttribute));
			string assyTitle = ((AssemblyTitleAttribute)attr).Title;

			foreach (Type type in m_ComObjectTypes)
			{
				bool bFail = false;
				try
				{
					string clsid = Marshal.GenerateGuidForType(type).ToString("B");
					string progid = Marshal.GenerateProgIdForType(type);
					key = Registry.ClassesRoot.CreateSubKey("CLSID\\" + clsid);
					key.SetValue(null, progid);										// Could be assyTitle/Desc??, but .NET components show ProgId here
					key2 = key.CreateSubKey("Implemented Categories");
					key3 = key2.CreateSubKey("{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}");
					key3.Close();
					key3 = null;
					key2.Close();
					key2 = null;
					key2 = key.CreateSubKey("ProgId");
					key2.SetValue(null, progid);
					key2.Close();
					key2 = null;
					key2 = key.CreateSubKey("LocalServer32");
					key2.SetValue(null, Application.ExecutablePath);
					key2.Close();
					key2 = null;
					key.Close();
					key = null;
					key = Registry.ClassesRoot.CreateSubKey(progid);
					key.SetValue(null, assyTitle);
					key2 = key.CreateSubKey("CLSID");
					key2.SetValue(null, clsid);
					key2.Close();
					key2 = null;
					key.Close();
					key = null;
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error while registering the server:\n" + ex.ToString(),
							"LocalServerCOM", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					bFail = true;
				}
				finally
				{
					if (key != null)
						key.Close();
					if (key2 != null)
						key2.Close();
					if (key3 != null)
						key3.Close();
				}
				if(bFail) break;
			}
		}

		//
		// Remove all traces of this from the registry. 
		//
		// **TODO** If the above does AppID/DCOM stuff, this would have
		// to remove that stuff too.
		//
		protected static void UnregisterObjects()
		{
			foreach (Type type in m_ComObjectTypes)
			{
				string clsid = Marshal.GenerateGuidForType(type).ToString("B");
				string progid = Marshal.GenerateProgIdForType(type);
				//
				// Best efforts: Try for each of the two places separately
				//
				try
				{
					Registry.ClassesRoot.DeleteSubKey(progid + "\\CLSID");
					Registry.ClassesRoot.DeleteSubKey(progid);
				}
				catch (Exception) 
				{
					break;
				}
				try
				{
					Registry.ClassesRoot.DeleteSubKey("CLSID\\" + clsid + "\\Implemented Categories\\{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}");
					Registry.ClassesRoot.DeleteSubKey("CLSID\\" + clsid + "\\Implemented Categories");
					Registry.ClassesRoot.DeleteSubKey("CLSID\\" + clsid + "\\ProgId");
					Registry.ClassesRoot.DeleteSubKey("CLSID\\" + clsid + "\\LocalServer32");
					Registry.ClassesRoot.DeleteSubKey("CLSID\\" + clsid);
				}
				catch (Exception) 
				{
					break;
				}
			}
		}

		//
		// On startup, we register the class factories of the COM objects
		// that we serve. This requires the class facgtory name to be
		// equal to the served class name + "ClassFactory".
		//
		protected static bool RegisterClassFactories()
		{
			m_ClassFactories = new ArrayList();
			foreach (Type type in m_ComObjectTypes)
			{
				ClassFactory factory = new ClassFactory(type);					// Use default context & flags
				m_ClassFactories.Add(factory);
				if (!factory.RegisterClassObject())
				{
					MessageBox.Show("Failed to register class factory for " + type.Name,
						"LocalServerCOM", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					return false;
				}
			}
			ClassFactory.ResumeClassObjects();									// Served objects now go live
			return true;
		}

		protected static void RevokeClassFactories()
		{
			ClassFactory.SuspendClassObjects();									// Prevent race conditions
			foreach (ClassFactory factory in m_ClassFactories)
				factory.RevokeClassObject();
		}
		//
		// ProcessArguments() will process the command-line arguments
		// If the return value is true, we carry on and start this application.
		// If the return value is false, we terminate this application immediately.
		//
		protected static bool ProcessArguments(string[] args)
		{
			bool bRet = true;

			//
			//**TODO** -Embedding is "ActiveX start". Prohibit non_AX starting?
			//
			if (args.Length > 0)
			{

				switch (args[0].ToLower())
				{
					case "-embedding":
						m_bComStart = true;										// Indicate COM started us
						break;

					case "-register":
					case "/register":
					case "-regserver":											// Emulate VB6
					case "/regserver":
						RegisterObjects();										// Register each served object
						bRet = false;
						break;

					case "-unregister":
					case "/unregister":
					case "-unregserver":										// Emulate VB6
					case "/unregserver":
						UnregisterObjects();									//Unregister each served object
						bRet = false;
						break;

					default:
						MessageBox.Show("Unknown argument: " + args[0] + "\nValid are : -register, -unregister and -embedding",
							"LocalServerCOM", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						break;
				}
			}
			else
				m_bComStart = false;

			return bRet;
		}

		//
		// ==================
		// SERVER ENTRY POINT
		// ==================
		//
		[STAThread]
		static void Main(string[] args)
		{
			if(!LoadComObjectAssemblies()) return;						// Load served COM class assemblies, get types

			if (!ProcessArguments(args)) return;						// Register/Unregister

			// Initialize critical member variables.
			m_iObjsInUse = 0;
			m_iServerLocks = 0;
			m_uiMainThreadId = GetCurrentThreadId();
			Thread.CurrentThread.Name = "Main Thread";

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			m_MainForm = new frmMain();
			if (m_bComStart) m_MainForm.WindowState = FormWindowState.Minimized;

			// Register the class factories of the served objects
			RegisterClassFactories();

			// Start up the garbage collection thread.
			GarbageCollection GarbageCollector = new GarbageCollection(1000);
			Thread GCThread = new Thread(new ThreadStart(GarbageCollector.GCWatch));
			GCThread.Name = "Garbage Collection Thread";
			GCThread.Start();

			//
			// Start the message loop. This serializes incoming calls to our
			// served COM objects, making this act like the VB6 equivalent!
			//
			Application.Run(m_MainForm);

			// Revoke the class factories immediately.
			// Don't wait until the thread has stopped before
			// we perform revocation!!!
			RevokeClassFactories();

			// Now stop the Garbage Collector thread.
			GarbageCollector.StopThread();
			GarbageCollector.WaitForThreadToStop();
		}
	}
}
