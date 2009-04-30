using System;
using System.Runtime.InteropServices;

namespace ManagedCOMLocalServer_Impl01
{
	class ClassFactoryBase : IClassFactory
	{

		#region Access to ole32.dll functions for class factories

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

		//
		// CoRegisterClassObject() is used to register a Class Factory
		// into COM's internal table of Class Factories.
		//
		[DllImport("ole32.dll")]
		static extern int CoRegisterClassObject(
			[In] ref Guid rclsid,
			[MarshalAs(UnmanagedType.IUnknown)] object pUnk, 
			uint dwClsContext,
			uint flags, 
			out uint lpdwRegister);
		//
		// Called by an COM EXE Server that can register multiple class objects 
		// to inform COM about all registered classes, and permits activation 
		// requests for those class objects. 
		// This function causes OLE to inform the SCM about all the registered 
		// classes, and begins letting activation requests into the server process.
		//
		[DllImport("ole32.dll")]
		static extern int CoResumeClassObjects();
		//
		// CoRevokeClassObject() is used to unregister a Class Factory
		// from COM's internal table of Class Factories.
		//
		[DllImport("ole32.dll")]
		static extern int CoRevokeClassObject(uint dwRegister);
		#endregion

		public ClassFactoryBase()
		{
		}

		protected UInt32	m_locked = 0;
		protected uint		m_ClassContext = (uint)CLSCTX.CLSCTX_LOCAL_SERVER;
		protected Guid		m_ClassId;
		protected uint		m_Flags;
		protected uint		m_Cookie;

		//
		// Inheriting real ClassFactories must override at least this. Each type
		// of object served by this server must have its own ClassFactory which
		// inherits this and overrides virtual_CreateInstance().
		//
		public virtual void virtual_CreateInstance(IntPtr pUnkOuter, ref Guid riid, out IntPtr ppvObject)
		{
			IntPtr nullPtr = new IntPtr(0);
			ppvObject = nullPtr;
		}

		public uint ClassContext
		{
			get
			{
				return m_ClassContext;
			}
			set
			{
				m_ClassContext = value;
			}
		}

		public Guid ClassId
		{
			get
			{
				return m_ClassId;
			}
			set
			{
				m_ClassId = value;
			}
		}

		public uint Flags
		{
			get
			{
				return m_Flags;
			}
			set
			{
				m_Flags = value;
			}
		}

		public bool RegisterClassObject()
		{
			// Register the class factory
			int i = CoRegisterClassObject
				(
				ref m_ClassId, 
				this, 
				ClassContext, 
				Flags,
				out m_Cookie
				);

			if (i == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool RevokeClassObject()
		{
			int i = CoRevokeClassObject(m_Cookie);

			if (i == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool ResumeClassObjects()
		{
			int i = CoResumeClassObjects();

			if (i == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		#region IClassFactory Implementations
		public void CreateInstance(IntPtr pUnkOuter, ref Guid riid, out IntPtr ppvObject)
		{
			virtual_CreateInstance(pUnkOuter, ref riid, out ppvObject);
		}

		public void LockServer(bool bLock)
		{
			if (bLock)
			{
				ManagedCOMLocalServer_Impl01.InterlockedIncrementServerLockCount();
			}
			else
			{
				ManagedCOMLocalServer_Impl01.InterlockedDecrementServerLockCount();
			}

			// Always attempt to see if we need to shutdown this server application.
			ManagedCOMLocalServer_Impl01.AttemptToTerminateServer();
		}
		#endregion
	}
}
