
'
' TEMPLATEDEVICENAME Local COM Server
'
' This is the core of a managed COM Local Server, capable of serving
' multiple instances of multiple interfdaces, within a single
' executable. This implementes the equivalent functionality of VB6
' which has been extensively used in ASCOM for drivers that provide
' multiple interfaces to multiple clients (e.g. Meade Telescope
' and Focuser) as well as hubs (e.g., POTH).
'
' Written by: Robert B. Denny (Version 1.0.1, 29-May-2007)
'
'
Imports System
Imports System.IO
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Collections
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports Microsoft.Win32
Imports System.Text
Imports System.Threading
Imports ASCOM
Imports ASCOM.Utilities

Namespace TEMPLATENAMESPACE
	Public Class TEMPLATEDEVICENAME

#Region "Access to kernel32.dll, user32.dll, and ole32.dll functions"
        <Flags()> _
        Enum CLSCTX As UInteger
            CLSCTX_INPROC_SERVER = &H1
            CLSCTX_INPROC_HANDLER = &H2
            CLSCTX_LOCAL_SERVER = &H4
            CLSCTX_INPROC_SERVER16 = &H8
            CLSCTX_REMOTE_SERVER = &H10
            CLSCTX_INPROC_HANDLER16 = &H20
            CLSCTX_RESERVED1 = &H40
            CLSCTX_RESERVED2 = &H80
            CLSCTX_RESERVED3 = &H100
            CLSCTX_RESERVED4 = &H200
            CLSCTX_NO_CODE_DOWNLOAD = &H400
            CLSCTX_RESERVED5 = &H800
            CLSCTX_NO_CUSTOM_MARSHAL = &H1000
            CLSCTX_ENABLE_CODE_DOWNLOAD = &H2000
            CLSCTX_NO_FAILURE_LOG = &H4000
            CLSCTX_DISABLE_AAA = &H8000
            CLSCTX_ENABLE_AAA = &H10000
            CLSCTX_FROM_DEFAULT_CONTEXT = &H20000
            CLSCTX_INPROC = CLSCTX_INPROC_SERVER Or CLSCTX_INPROC_HANDLER
            CLSCTX_SERVER = CLSCTX_INPROC_SERVER Or CLSCTX_LOCAL_SERVER Or CLSCTX_REMOTE_SERVER
            CLSCTX_ALL = CLSCTX_SERVER Or CLSCTX_INPROC_HANDLER
        End Enum

        <Flags()> _
        Enum COINIT As UInteger
            ''' Initializes the thread for multi-threaded object concurrency.
            COINIT_MULTITHREADED = &H0
            ''' Initializes the thread for apartment-threaded object concurrency. 
            COINIT_APARTMENTTHREADED = &H2
            ''' Disables DDE for Ole1 support.
            COINIT_DISABLE_OLE1DDE = &H4
            ''' Trades memory for speed.
            COINIT_SPEED_OVER_MEMORY = &H8
        End Enum

        <Flags()> _
        Enum REGCLS As UInteger
            REGCLS_SINGLEUSE = 0
            REGCLS_MULTIPLEUSE = 1
            REGCLS_MULTI_SEPARATE = 2
            REGCLS_SUSPENDED = 4
            REGCLS_SURROGATE = 8
        End Enum


        ' CoInitializeEx() can be used to set the apartment model
        ' of individual threads.
        <DllImport("ole32.dll")> _
        Shared Function CoInitializeEx(ByVal pvReserved As IntPtr, ByVal dwCoInit As UInteger) As Integer
        End Function

        ' CoUninitialize() is used to uninitialize a COM thread.
        <DllImport("ole32.dll")> _
        Shared Sub CoUninitialize()
        End Sub

        ' PostThreadMessage() allows us to post a Windows Message to
        ' a specific thread (identified by its thread id).
        ' We will need this API to post a WM_QUIT message to the main 
        ' thread in order to terminate this application.
        <DllImport("user32.dll")> _
        Shared Function PostThreadMessage(ByVal idThread As UInteger, ByVal Msg As UInteger, ByVal wParam As UIntPtr, ByVal lParam As IntPtr) As Boolean
        End Function

        ' GetCurrentThreadId() allows us to obtain the thread id of the
        ' calling thread. This allows us to post the WM_QUIT message to
        ' the main thread.
        <DllImport("kernel32.dll")> _
        Shared Function GetCurrentThreadId() As UInteger
        End Function
#End Region

#Region "Private Data"
        ' Stores the main thread's thread id.
        Private Shared m_iObjsInUse As Integer
        ' Keeps a count on the total number of objects alive.
        Private Shared m_iServerLocks As Integer
        ' Keeps a lock count on this application.
        ' True if server started by COM (-embedding)
        Private Shared m_MainForm As frmMain = Nothing
        ' Reference to our main form
        Private Shared m_ComObjectAssys As ArrayList
        ' Dynamically loaded assemblies containing served COM objects
        Private Shared m_ComObjectTypes As ArrayList
        ' Served COM object types
        Private Shared m_ClassFactories As ArrayList
        ' Served COM object class factories
        Private Shared m_sAppId As String = "{$guid1$}"
        ' Our AppId
#End Region
        ' This property returns the main thread's id.
        Public Shared Property MainThreadId() As UInteger
            Get
            End Get
            Private Set(ByVal value As UInteger)
            End Set
        End Property

        ' Used to tell if started by COM or manually
        Public Shared Property StartedByCOM() As Boolean
            Get
            End Get
            Private Set(ByVal value As Boolean)
            End Set
        End Property


#Region "Server Lock, Object Counting, and AutoQuit on COM startup"
        ' Returns the total number of objects alive currently.
        Public Shared ReadOnly Property ObjectsCount() As Integer
            Get
                SyncLock GetType(TEMPLATEDEVICENAME)
                    Return m_iObjsInUse
                End SyncLock
            End Get
        End Property

        ' This method performs a thread-safe incrementation of the objects count.
        Public Shared Function CountObject() As Integer
            ' Increment the global count of objects.
            Return Interlocked.Increment(m_iObjsInUse)
        End Function

        ' This method performs a thread-safe decrementation the objects count.
        Public Shared Function UncountObject() As Integer
            ' Decrement the global count of objects.
            Return Interlocked.Decrement(m_iObjsInUse)
        End Function

        ' Returns the current server lock count.
        Public Shared ReadOnly Property ServerLockCount() As Integer
            Get
                SyncLock GetType(TEMPLATEDEVICENAME)
                    Return m_iServerLocks
                End SyncLock
            End Get
        End Property

        ' This method performs a thread-safe incrementation the 
        ' server lock count.
        Public Shared Function CountLock() As Integer
            ' Increment the global lock count of this server.
            Return Interlocked.Increment(m_iServerLocks)
        End Function

        ' This method performs a thread-safe decrementation the 
        ' server lock count.
        Public Shared Function UncountLock() As Integer
            ' Decrement the global lock count of this server.
            Return Interlocked.Decrement(m_iServerLocks)
        End Function

        ' AttemptToTerminateServer() will check to see if the objects count and the server 
        ' lock count have both dropped to zero.
        '
        ' If so, and if we were started by COM, we post a WM_QUIT message to the main thread's
        ' message loop. This will cause the message loop to exit and hence the termination 
        ' of this application. If hand-started, then just trace that it WOULD exit now.
        '
        Public Shared Sub ExitIf()
            SyncLock GetType(TEMPLATEDEVICENAME)
                If (ObjectsCount <= 0) AndAlso (ServerLockCount <= 0) Then
                    If StartedByCOM Then
                        Dim wParam As New UIntPtr(0)
                        Dim lParam As New IntPtr(0)
                        PostThreadMessage(MainThreadId, &H12, wParam, lParam)
                    End If
                End If
            End SyncLock
        End Sub
#End Region

        ' -----------------
        ' PRIVATE FUNCTIONS
        ' -----------------

#Region "Dynamic Driver Assembly Loader"
        '
        ' Load the assemblies that contain the classes that we will serve
        ' via COM. These will be located in the subfolder ServedClasses
        ' below our executable. The code below takes care of the situation
        ' where we're running in the VS.NET IDE, allowing the ServedClasses
        ' folder to be in the solution folder, while we are executing in
        ' the TEMPLATEDEVICENAME\bin\Debug subfolder.
        '
        Private Shared Function LoadComObjectAssemblies() As Boolean
            m_ComObjectAssys = New ArrayList()
            m_ComObjectTypes = New ArrayList()

            Dim assyPath As String = Assembly.GetEntryAssembly().Location
            Dim i As Integer = assyPath.LastIndexOf("\TEMPLATEDEVICENAME\bin\")
            ' Look for us running in IDE
            If i = -1 Then
                i = assyPath.LastIndexOf("\"c)
            End If
            assyPath = [String].Format("{0}\TEMPLATEDEVICENAMEServedClasses", assyPath.Remove(i, assyPath.Length - i))

            Dim d As New DirectoryInfo(assyPath)
            For Each fi As FileInfo In d.GetFiles("*.dll")
                Dim aPath As String = fi.FullName
                Dim fqClassName As String = fi.Name.Replace(fi.Extension, "")
                ' COM class FQN
                '
                ' First try to load the assembly and get the types for
                ' the class and the class facctory. If this doesn't work ????
                '
                Try
                    Dim so As Assembly = Assembly.LoadFrom(aPath)
                    ' Check to see if the assembly has the ServedClassName attribute, only use it if it does.
                    Dim attrbutes As Object() = so.GetCustomAttributes(GetType(ServedClassNameAttribute), False)
                    If attrbutes.Length > 0 Then
                        m_ComObjectTypes.Add(so.[GetType](fqClassName, True))
                        m_ComObjectAssys.Add(so)
                    End If
                Catch e As Exception
                    MessageBox.Show([String].Format("Failed to load served COM class assembly {0} - {1}", fi.Name, e.Message), "TEMPLATEDEVICENAME", MessageBoxButtons.OK, MessageBoxIcon.[Stop])
                    Return False

                End Try
            Next
            Return True
        End Function
        Private Shared Function GetDeviceClass(ByVal progid As String) As String
            Return progid.Substring(progid.LastIndexOf("."c) + 1)
        End Function
#End Region

#Region "COM Registration and Unregistration"
        '
        ' Do everything to register this for COM. Never use REGASM on
        ' this exe assembly! It would create InProcServer32 entries 
        ' which would prevent proper activation!
        '
        ' Using the list of COM object types generated during dynamic
        ' assembly loading, it registers each one for COM as served by our
        ' exe/local server, as well as registering it for ASCOM. It also
        ' adds DCOM info for the local server itself, so it can be activated
        ' via an outboiud connection from TheSky.
        '
        Private Shared Sub RegisterObjects()
            Dim assy As Assembly = Assembly.GetExecutingAssembly()
            Dim attr As Attribute = Attribute.GetCustomAttribute(assy, GetType(AssemblyTitleAttribute))
            Dim assyTitle As String = (DirectCast(attr, AssemblyTitleAttribute)).Title
            attr = Attribute.GetCustomAttribute(assy, GetType(AssemblyDescriptionAttribute))
            Dim assyDescription As String = (DirectCast(attr, AssemblyDescriptionAttribute)).Description

            '
            ' Local server's DCOM/AppID information
            '
            Try
                '
                ' HKCR\APPID\appid
                '
                Using key As RegistryKey = Registry.ClassesRoot.CreateSubKey("APPID\" + m_sAppId)

                    key.SetValue(Nothing, assyDescription)
                    key.SetValue("AppID", m_sAppId)
                    key.SetValue("AuthenticationLevel", 1, RegistryValueKind.DWord)
                    key.Close()
                End Using
                '
                ' HKCR\APPID\exename.ext
                '
                Using key As RegistryKey = Registry.ClassesRoot.CreateSubKey("APPID\" + Application.ExecutablePath.Substring(Application.ExecutablePath.LastIndexOf("\"c) + 1))
                    key.SetValue("AppID", m_sAppId)
                    key.Close()
                End Using
            Catch ex As Exception
                MessageBox.Show("Error while registering the server:" & vbLf + ex.ToString(), "TEMPLATEDEVICENAME", MessageBoxButtons.OK, MessageBoxIcon.[Stop])
                Return
            End Try

            '
            ' For each of the driver assemblies
            '
            For Each type As Type In m_ComObjectTypes
                Dim bFail As Boolean = False
                Try
                    '
                    ' HKCR\CLSID\clsid
                    '
                    Dim clsid As String = Marshal.GenerateGuidForType(type).ToString("B")
                    Dim progid As String = Marshal.GenerateProgIdForType(type)
                    Using key As RegistryKey = Registry.ClassesRoot.CreateSubKey("CLSID\" + clsid)
                        key.SetValue(Nothing, progid)
                        ' Could be assyTitle/Desc??, but .NET components show ProgId here
                        key.SetValue("AppId", m_sAppId)
                        Using key2 As RegistryKey = key.CreateSubKey("Implemented Categories")
                            Using key3 As RegistryKey = key2.CreateSubKey("{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}")
                                key3.Close()
                            End Using
                            key2.Close()
                        End Using
                        Using key2 As RegistryKey = key.CreateSubKey("ProgId")
                            key2.SetValue(Nothing, progid)
                            key2.Close()
                        End Using
                        Using key2 As RegistryKey = key.CreateSubKey("Programmable")
                            key2.Close()
                        End Using
                        Using key2 As RegistryKey = key.CreateSubKey("LocalServer32")
                            key2.SetValue(Nothing, Application.ExecutablePath)
                            key2.Close()
                        End Using
                    End Using
                    '
                    ' HKCR\CLSID\progid
                    '
                    Using key As RegistryKey = Registry.ClassesRoot.CreateSubKey(progid)
                        key.SetValue(Nothing, assyTitle)
                        Using key2 As RegistryKey = key.CreateSubKey("CLSID")
                            key2.SetValue(Nothing, clsid)
                            key2.Close()
                        End Using
                        key.Close()
                    End Using
                    '
                    ' ASCOM 
                    '
                    assy = type.Assembly
                    'attr = Attribute.GetCustomAttribute(assy, typeof(AssemblyProductAttribute));
                    'string chooserName = ((AssemblyProductAttribute)attr).Product;

                    ' Pull the display name from the ServedClassName attribute.
                    attr = Attribute.GetCustomAttribute(assy, GetType(ServedClassNameAttribute))
                    Dim chooserName As String = If((DirectCast(attr, ServedClassNameAttribute)).DisplayName, "TEMPLATEDEVICENAME")
                    Using P As Profile = New Profile()
                        P.Register(progid, chooserName)
                        Try
                            ' In case Helper becomes native .NET
                            Marshal.ReleaseComObject(P)
                        Catch generatedExceptionName As Exception
                        End Try
                    End Using
                Catch ex As Exception
                    MessageBox.Show([String].Format("Error while registering the server:" & vbLf & "{0}", ex), "TEMPLATEDEVICENAME", MessageBoxButtons.OK, MessageBoxIcon.[Stop])
                    bFail = True
                End Try
                If bFail Then
                    Exit For
                End If
            Next
        End Sub

        '
        ' Remove all traces of this from the registry. 
        '
        ' **TODO** If the above does AppID/DCOM stuff, this would have
        ' to remove that stuff too.
        '
        Private Shared Sub UnregisterObjects()
            '
            ' Local server's DCOM/AppID information
            '
            Registry.ClassesRoot.DeleteSubKey([String].Format("APPID\{0}", m_sAppId), False)
            Registry.ClassesRoot.DeleteSubKey([String].Format("APPID\{0}", Application.ExecutablePath.Substring(Application.ExecutablePath.LastIndexOf("\"c) + 1)), False)

            '
            ' For each of the driver assemblies
            '
            For Each type As Type In m_ComObjectTypes
                Dim clsid As String = Marshal.GenerateGuidForType(type).ToString("B")
                Dim progid As String = Marshal.GenerateProgIdForType(type)
                '
                ' Best efforts
                '
                '
                ' HKCR\progid
                '
                Registry.ClassesRoot.DeleteSubKey([String].Format("{0}\CLSID", progid), False)
                Registry.ClassesRoot.DeleteSubKey(progid, False)
                '
                ' HKCR\CLSID\clsid
                '
                Registry.ClassesRoot.DeleteSubKey([String].Format("CLSID\{0}\Implemented Categories\{{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}}", clsid), False)
                Registry.ClassesRoot.DeleteSubKey([String].Format("CLSID\{0}\Implemented Categories", clsid), False)
                Registry.ClassesRoot.DeleteSubKey([String].Format("CLSID\{0}\ProgId", clsid), False)
                Registry.ClassesRoot.DeleteSubKey([String].Format("CLSID\{0}\LocalServer32", clsid), False)
                Registry.ClassesRoot.DeleteSubKey([String].Format("CLSID\{0}\Programmable", clsid), False)
                Registry.ClassesRoot.DeleteSubKey([String].Format("CLSID\{0}", clsid), False)
                Try
                    '
                    ' ASCOM
                    '
                    Using P As New Profile()
                        P.Unregister(progid)
                        Try
                            ' In case Helper becomes native .NET
                            Marshal.ReleaseComObject(P)
                        Catch generatedExceptionName As Exception
                        End Try
                    End Using
                Catch generatedExceptionName As Exception
                End Try
            Next
        End Sub
#End Region

#Region "Class Factory Support"
        '
        ' On startup, we register the class factories of the COM objects
        ' that we serve. This requires the class facgtory name to be
        ' equal to the served class name + "ClassFactory".
        '
        Private Shared Function RegisterClassFactories() As Boolean
            m_ClassFactories = New ArrayList()
            For Each type As Type In m_ComObjectTypes
                Dim factory As New ClassFactory(type)
                ' Use default context & flags
                m_ClassFactories.Add(factory)
                If Not factory.RegisterClassObject() Then
                    MessageBox.Show("Failed to register class factory for " + type.Name, "TEMPLATEDEVICENAME", MessageBoxButtons.OK, MessageBoxIcon.[Stop])
                    Return False
                End If
            Next
            ClassFactory.ResumeClassObjects()
            ' Served objects now go live
            Return True
        End Function

        Private Shared Sub RevokeClassFactories()
            ClassFactory.SuspendClassObjects()
            ' Prevent race conditions
            For Each factory As ClassFactory In m_ClassFactories
                factory.RevokeClassObject()
            Next
        End Sub
#End Region

#Region "Command Line Arguments"
        '
        ' ProcessArguments() will process the command-line arguments
        ' If the return value is true, we carry on and start this application.
        ' If the return value is false, we terminate this application immediately.
        '
        Private Shared Function ProcessArguments(ByVal args As String()) As Boolean
            Dim bRet As Boolean = True

            '
            '**TODO** -Embedding is "ActiveX start". Prohibit non_AX starting?
            '
            If args.Length > 0 Then

                Select Case args(0).ToLower()
                    Case "-embedding"
                        StartedByCOM = True
                        ' Indicate COM started us
                        Exit Select

                        ' Emulate VB6
                    Case "-register", "/register", "-regserver", "/regserver"
                        RegisterObjects()
                        ' Register each served object
                        bRet = False
                        Exit Select

                        ' Emulate VB6
                    Case "-unregister", "/unregister", "-unregserver", "/unregserver"
                        UnregisterObjects()
                        'Unregister each served object
                        bRet = False
                        Exit Select
                    Case Else

                        MessageBox.Show("Unknown argument: " + args(0) + vbLf & "Valid are : -register, -unregister and -embedding", "TEMPLATEDEVICENAME", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Select
                End Select
            Else
                StartedByCOM = False
            End If

            Return bRet
        End Function
#End Region

#Region "SERVER ENTRY POINT (main)"
        '
        ' ==================
        ' SERVER ENTRY POINT
        ' ==================
        '
        <STAThread()> _
        Shared Sub Main(ByVal args As String())
            If Not LoadComObjectAssemblies() Then
                Return
            End If
            ' Load served COM class assemblies, get types
            If Not ProcessArguments(args) Then
                Return
            End If
            ' Register/Unregister
            ' Initialize critical member variables.
            m_iObjsInUse = 0
            m_iServerLocks = 0
            MainThreadId = GetCurrentThreadId()
            Thread.CurrentThread.Name = "Main Thread"

            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            m_MainForm = New frmMain()
            If StartedByCOM Then
                m_MainForm.WindowState = FormWindowState.Minimized
            End If

            ' Register the class factories of the served objects
            RegisterClassFactories()

            ' Start up the garbage collection thread.
            Dim GarbageCollector As New GarbageCollection(1000)
            Dim GCThread As New Thread(AddressOf GarbageCollector.GCWatch)
            GCThread.Name = "Garbage Collection Thread"
            GCThread.Start()

            '
            ' Start the message loop. This serializes incoming calls to our
            ' served COM objects, making this act like the VB6 equivalent!
            '
            Application.Run(m_MainForm)

            ' Revoke the class factories immediately.
            ' Don't wait until the thread has stopped before
            ' we perform revocation!!!
            RevokeClassFactories()

            ' Now stop the Garbage Collector thread.
            GarbageCollector.StopThread()
            GarbageCollector.WaitForThreadToStop()
        End Sub
#End Region
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Built and maintained by Todd Anglin and Telerik
'=======================================================
