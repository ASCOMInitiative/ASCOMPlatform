using System;
using System.Runtime.InteropServices;

namespace ASCOM.Remote
{
    internal static class NativeMethods
    {

        // CoRegisterClassObject() is used to register a Class Factory
        // into COM's internal table of Class Factories.
        [DllImport("ole32.dll")]
        public static extern int CoRegisterClassObject(
            [In] ref Guid rclsid,
            [MarshalAs(UnmanagedType.IUnknown)] object pUnk,
            uint dwClsContext,
            uint flags,
            out uint lpdwRegister);

        // Called by a COM EXE Server that can register multiple class objects 
        // to inform COM about all registered classes, and permits activation 
        // requests for those class objects. 
        // This function causes OLE to inform the SCM about all the registered 
        // classes, and begins letting activation requests into the server process.
        [DllImport("ole32.dll")]
        public static extern int CoResumeClassObjects();

        // Prevents any new activation requests from the SCM on all class objects
        // registered within the process. Even though a process may call this API, 
        // the process still must call CoRevokeClassObject for each CLSID it has 
        // registered, in the apartment it registered in.
        [DllImport("ole32.dll")]
        public static extern int CoSuspendClassObjects();

        // CoRevokeClassObject() is used to unregister a Class Factory
        // from COM's internal table of Class Factories.
        [DllImport("ole32.dll")]
        public static extern int CoRevokeClassObject(uint dwRegister);

        // CoInitializeEx() can be used to set the apartment model
        // of individual threads.
        [DllImport("ole32.dll")]
        public static extern int CoInitializeEx(IntPtr pvReserved, uint dwCoInit);

        // CoUninitialize() is used to uninitialize a COM thread.
        [DllImport("ole32.dll")]
        public static extern void CoUninitialize();

        // PostThreadMessage() allows us to post a Windows Message to
        // a specific thread (identified by its thread id).
        // We will need this API to post a WM_QUIT message to the main 
        // thread in order to terminate this application.
        [DllImport("user32.dll")]
        public static extern bool PostThreadMessage(uint idThread, uint Msg, UIntPtr wParam,
            IntPtr lParam);

        // GetCurrentThreadId() allows us to obtain the thread id of the
        // calling thread. This allows us to post the WM_QUIT message to
        // the main thread.
        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();
    }
}
