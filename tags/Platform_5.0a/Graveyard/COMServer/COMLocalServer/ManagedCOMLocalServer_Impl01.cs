//
// This is the core of a managed COM Local Server, capable of serving
// multiple instances of multiple interfdaces, within a single
// executable. This implementes the equivalent functionality of VB6
// which has been extensively used in ASCOM for drivers that provide
// multiplel interfaces to multiple clients (e.g. Meade Telescope
// and Focuser) as well as hubs (e.g., POTH).
//
// The original idea and code came from the CodeProject article
// "Implementing COM Servers in .NET" by Lim Bio Liong. 
//
// http://www.codeproject.com/useritems/BuildCOMServersInDotNet.asp
//
// Also, this article:
//
// http://www.codeproject.com/com/comintro2.asp
//
// can help yu to understand what's going on.
//
// I modified the code as follows:
//
// (1) All registration takes place via invocation of <exe> /register
//     and all unregistration via <exe> /unregister. I also added the 
//     VB6-like options /regserver and /unregserver.  No separate 
//     invocation of REGASM is needed. the original incorrectly used
//     REGASM, which produced InProcServer32 entries, which prevented
//     the server from correctly auto-starting upon client creation of
//     its served interface.
//
// (2) It uses an interface that originally derives from MIDL, compiled
//     to a type library which is registered for COM, then imported 
//     into an Interop assembly which is put into the GAC and which is 
//     referenced here. This leads to a type library for THIS server that
//     contains only the CoClass, and references the existing registered
//     interface.
//
// (3) Moderate refactoring to simplify it and prepare it to be turned
//     into a VS.NET template. Hiding of kernel32.dll, user32.dll, and 
//     ole32.dll functions in #regions.
//
// (4) Additions of comments in various places to make it easier to 
//     understand the reasons for things.
//
// Note  from the Platform SDK: Automation refers to a class factory as 
// "An object that implements the IClassFactory interface, which allows 
// it to create other objects of a specific class.". It could be other 
// class factory interface (not only the IClassFactory interface) such 
// as the IClassFactory2 or a custom interface. This may be something
// useful to know for the future, but licensing (the objective of 
// IClassFactory2) is not envisioned to be relevant for ASCOM!
//
// Status:
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
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Reflection;
using Microsoft.Win32;
using System.Text;
using System.Threading;

namespace ManagedCOMLocalServer_Impl01
{

	// Note that ManagedCOMLocalServer_Impl01 is NOT declared as public.
    // This is so that it will not be exposed to COM when we call regasm
	// or tlbexp.
	class ManagedCOMLocalServer_Impl01
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

		// We import the POINT structure because it is referenced
		// by the MSG structure.
		[ComVisible(false)]
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;

			public POINT(int x, int y)
			{
				this.X = x;
				this.Y = y;
			}

			public static implicit operator Point(POINT p)
			{
				return new Point(p.X, p.Y);
			}

			public static implicit operator POINT(Point p)
			{
				return new POINT(p.X, p.Y);
			}
		}

		// We import the MSG structure because it is referenced 
		// by the GetMessage(), TranslateMessage() and DispatchMessage()
		// Win32 APIs.
		[ComVisible(false)]
		[StructLayout(LayoutKind.Sequential)]
		public struct MSG
		{
			public IntPtr hwnd;
			public uint message;
			public IntPtr wParam;
			public IntPtr lParam;
			public uint time;
			public POINT pt;
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

		// We will be manually performing a Message Loop within the main thread
		// of this application. Hence we will need to import GetMessage(), 
		// TranslateMessage() and DispatchMessage().
		[DllImport("user32.dll")]
		static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
			uint wMsgFilterMax);

		[DllImport("user32.dll")]
		static extern bool TranslateMessage([In] ref MSG lpMsg);

		[DllImport("user32.dll")]
		static extern IntPtr DispatchMessage([In] ref MSG lpmsg);
		#endregion

		// Define two common GUID objects for public usage.
		public static Guid IID_IUnknown  = new Guid("{00000000-0000-0000-C000-000000000046}");
		public static Guid IID_IDispatch = new Guid("{00020400-0000-0000-C000-000000000046}");

		protected static uint	m_uiMainThreadId;			// Stores the main thread's thread id.
		protected static int	m_iObjsInUse;				// Keeps a count on the total number of objects alive.
		protected static int	m_iServerLocks;				// Keeps a lock count on this application.

		// This property returns the main thread's id.
		public static uint MainThreadId
		{
			get
			{
				return m_uiMainThreadId;
			}
		}

		// This method performs a thread-safe incrementation of the objects count.
		public static int InterlockedIncrementObjectsCount()
		{
			Console.WriteLine("InterlockedIncrementObjectsCount()");
			// Increment the global count of objects.
			return Interlocked.Increment(ref m_iObjsInUse);
		}

		// This method performs a thread-safe decrementation the objects count.
		public static int InterlockedDecrementObjectsCount()
		{
			Console.WriteLine("InterlockedDecrementObjectsCount()");
			// Decrement the global count of objects.
			return Interlocked.Decrement(ref m_iObjsInUse);
		}

		// Returns the total number of objects alive currently.
		public static int ObjectsCount
		{
			get
			{
				lock(typeof(ManagedCOMLocalServer_Impl01))
				{
					return m_iObjsInUse;
				}
			}
		}

		// This method performs a thread-safe incrementation the 
		// server lock count.
		public static int InterlockedIncrementServerLockCount()
		{
			Console.WriteLine("InterlockedIncrementServerLockCount()");
			// Increment the global lock count of this server.
			return Interlocked.Increment(ref m_iServerLocks);
		}

		// This method performs a thread-safe decrementation the 
		// server lock count.
		public static int InterlockedDecrementServerLockCount()
		{
			Console.WriteLine("InterlockedDecrementServerLockCount()");
			// Decrement the global lock count of this server.
			return Interlocked.Decrement(ref m_iServerLocks);
		}

		// Returns the current server lock count.
		public static int ServerLockCount
		{
			get
			{
				lock(typeof(ManagedCOMLocalServer_Impl01))
				{
					return m_iServerLocks;
				}
			}
		}

		// AttemptToTerminateServer() will check to see if 
		// the objects count and the server lock count has
		// both dropped to zero.
		// If so, we post a WM_QUIT message to the main thread's
		// message loop. This will cause the message loop to
		// exit and hence the termination of this application.
		public static void AttemptToTerminateServer()
		{
			lock(typeof(ManagedCOMLocalServer_Impl01))
			{
				Console.WriteLine("AttemptToTerminateServer()");

				// Get the most up-to-date values of these critical data.
				int iObjsInUse = ObjectsCount;
				int iServerLocks = ServerLockCount;

				// Print out these info for debug purposes.
				StringBuilder sb = new StringBuilder("");		  
				sb.AppendFormat("m_iObjsInUse : {0}. m_iServerLocks : {1}", iObjsInUse, iServerLocks);
				Console.WriteLine(sb.ToString());

				if ((iObjsInUse > 0) || (iServerLocks > 0))
				{
					Console.WriteLine("There are still referenced objects or the server lock count is non-zero.");
				}
				else
				{
					UIntPtr wParam = new UIntPtr(0);
					IntPtr lParam = new IntPtr(0);
					Console.WriteLine("PostThreadMessage(WM_QUIT)");
					PostThreadMessage(MainThreadId, 0x0012, wParam, lParam);
				}
			}
		}

		//
		// ProcessArguments() will process the command-line arguments
		// If the return value is true, we carry on and start this application.
		// If the return value is false, we terminate this application immediately.
		//
		// REGISTER/UNREGISTER
		//
		// If we use REGASM to get "most" of the registry entries, there will be problems.
		// First, a useless HKCR\TypeLib entry is made, and worse, it is not removed with
		// regasm -u. Second, all the InprocServer32 are bogus, and prevent us from 
		// being started as LocalServer32. So we just put everything we need into the
		// registry on -register, and remove it all on -unregister. 
		//
		protected static bool ProcessArguments(string[] args)
		{
			bool bRet = true;

			if (args.Length > 0)
			{
				RegistryKey key = null;
				RegistryKey key2 = null;
				RegistryKey key3 = null;

				switch (args[0].ToLower())
				{
					case "-embedding":
						Console.WriteLine("Request to start as out-of-process COM server.");
						break;

					case "-register":
					case "/register":
					case "-regserver":										// Enulate VB6
					case "/regserver":
						//
						// Do everything to register this for COM. Don't require an
						// additional REGASM - and anyway, that would create InProcServer32
						// entries that would prevent proper activation!
						//
						// **TODO** Should this also create AppID/DCOM junk for TheSky?
						// Probably...
						//
						try 
						{
							Assembly assy = Assembly.GetExecutingAssembly();
							Attribute attr = Attribute.GetCustomAttribute(assy, typeof(AssemblyTitleAttribute));
							string assyTitle = ((AssemblyTitleAttribute)attr).Title;
							string clsid = Marshal.GenerateGuidForType(typeof(SimpleCOMObject)).ToString("B");
							string progid = "ManagedCOMLocalServer_Impl01.SimpleCOMObject";
							key = Registry.ClassesRoot.CreateSubKey("CLSID\\" + clsid);
							key.SetValue(null, progid);								// Could be assyTitle, but .NET components show ProgId here
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
							MessageBox.Show("Error while registering the server:\n" + ex.ToString());
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
						bRet = false;
						break;

					case "-unregister":
					case "/unregister":
					case "-unregserver":										// Emulate VB6
					case "/unregserver":
						//
						// Remove all traces of this from the registry. 
						//
						// **TODO** If the above does AppID/DCOM stuff, this would have
						// to remove that stuff too.
						//
						try 
						{
							string clsid = Marshal.GenerateGuidForType(typeof(SimpleCOMObject)).ToString("B");
							string progid = "ManagedCOMLocalServer_Impl01.SimpleCOMObject";
							Registry.ClassesRoot.DeleteSubKey(progid + "\\CLSID");
							Registry.ClassesRoot.DeleteSubKey(progid);
							Registry.ClassesRoot.DeleteSubKey("CLSID\\" + clsid + "\\Implemented Categories\\{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}");
							Registry.ClassesRoot.DeleteSubKey("CLSID\\" + clsid + "\\Implemented Categories");
							Registry.ClassesRoot.DeleteSubKey("CLSID\\" + clsid + "\\ProgId");
							Registry.ClassesRoot.DeleteSubKey("CLSID\\" + clsid + "\\LocalServer32");
							Registry.ClassesRoot.DeleteSubKey("CLSID\\" + clsid);
						} 
						catch (Exception ex)
						{
							MessageBox.Show("Error while unregistering the server:\n"+ex.ToString());
						}
						bRet = false;
						break;

					default:
						Console.WriteLine("Unknown argument: " + args[0] + "\nValid are : -register, -unregister and -embedding");
						break;
				}
			}

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
			if (!ProcessArguments(args))
			{
				return;
			}

			// Initialize critical member variables.
			m_iObjsInUse = 0;
			m_iServerLocks = 0;
			m_uiMainThreadId = GetCurrentThreadId();

			// Register the SimpleCOMObjectClassFactory.
			SimpleCOMObjectClassFactory factory = new SimpleCOMObjectClassFactory();
			factory.ClassContext = (uint)CLSCTX.CLSCTX_LOCAL_SERVER;
			factory.ClassId = Marshal.GenerateGuidForType(typeof(SimpleCOMObject));
			factory.Flags = (uint)REGCLS.REGCLS_MULTIPLEUSE | (uint)REGCLS.REGCLS_SUSPENDED;
			factory.RegisterClassObject();
			ClassFactoryBase.ResumeClassObjects();

			// Start up the garbage collection thread.
			GarbageCollection GarbageCollector = new GarbageCollection(1000);
			Thread GarbageCollectionThread = new Thread(new ThreadStart(GarbageCollector.GCWatch));
			
			// Set the name of the thread object.
			GarbageCollectionThread.Name = "Garbage Collection Thread";
			// Start the thread.
			GarbageCollectionThread.Start();
			//
			// Start the message loop. This serializes incoming calls to our
			// served COM objects, making this act like the VB6 equivalent!
			//
			MSG msg;
			IntPtr null_hwnd = new IntPtr(0);
			while (GetMessage(out msg, null_hwnd, 0, 0) != false) 
			{
				TranslateMessage(ref msg);
				DispatchMessage(ref msg);
			}
			Console.WriteLine("Out of message loop.");

			// Revoke the class factory immediately.
			// Don't wait until the thread has stopped before
			// we perform revokation.
			factory.RevokeClassObject();
			Console.WriteLine("SimpleCOMObjectClassFactory Revoked.");

			// Now stop the Garbage Collector thread.
			GarbageCollector.StopThread();
			GarbageCollector.WaitForThreadToStop();
			Console.WriteLine("GarbageCollector thread stopped.");

			// Just an indication that this COM EXE Server is stopped.
			Console.WriteLine("Press [ENTER] to exit.");
			Console.ReadLine();
		}
	}
}
