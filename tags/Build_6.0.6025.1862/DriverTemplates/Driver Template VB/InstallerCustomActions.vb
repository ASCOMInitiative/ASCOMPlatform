Imports System.ComponentModel
Imports System.Configuration.Install
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Namespace ASCOM.Setup
    ''' <summary>
    ''' Custom install actions that must be carried out during product installation.
    ''' </summary>
    <RunInstaller(True)> _
    Public Class ComRegistration
        Inherits Installer
        ''' <summary>
        ''' Custom Install Action that regsiters the driver with COM Interop.
        ''' Note that this will in turn trigger an methods with the [ComRegisterFunction()] attribute
        ''' such as those in Driver.cs that perform ASCOM registration.
        ''' </summary>
        ''' <param name="stateSaver">Not used.<see cref="Installer"/></param>
        Public Overloads Overrides Sub Install(ByVal stateSaver As System.Collections.IDictionary)
            Trace.WriteLine("Install custom action - Starting registration for COM Interop")
#If DEBUG Then
            MessageBox.Show("Attach debugger to this process now, if required", "Custom Action Debug", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
#End If
            MyBase.Install(stateSaver)
            Dim regsrv As New RegistrationServices()
            If Not regsrv.RegisterAssembly(Me.GetType().Assembly, AssemblyRegistrationFlags.SetCodeBase) Then
                Trace.WriteLine("COM registration failed")
                Throw New InstallException("Failed To Register driver for COM Interop")
            End If
            Trace.WriteLine("Completed registration for COM Interop")
        End Sub

        ''' <summary>
        ''' Custom Install Action that removes the COM Interop component registrations.
        ''' Note that this will in turn trigger any methods with the [ComUnregisterFunction()] attribute
        ''' such as those in Driver.cs that remove the ASCOM registration.
        ''' </summary>
        ''' <param name="savedState">Not used.<see cref="Installer"/></param>
        Public Overloads Overrides Sub Uninstall(ByVal savedState As System.Collections.IDictionary)
            Trace.WriteLine("Uninstall custom action - unregistering from COM Interop")
            Try
                MyBase.Uninstall(savedState)
                Dim regsrv As New RegistrationServices()
                If Not regsrv.UnregisterAssembly(Me.GetType().Assembly) Then
                    Trace.WriteLine("COM Interop deregistration failed")
                    Throw New InstallException("Failed To Unregister from COM Interop")
                End If
            Finally
                Trace.WriteLine("Completed uninstall custom action")
            End Try
        End Sub
    End Class
End Namespace