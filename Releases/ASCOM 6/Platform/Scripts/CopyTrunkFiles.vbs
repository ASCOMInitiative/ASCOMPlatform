Option Explicit
DIM WshShell, btn, FSO, startTxt, arrFiles(4), destDir, objFile 

startTxt = "Run this script to copy all the needed ASCOM 6 release files from the main trunk " _
& "when complete run the installer project.  The installer project will use all the " _ 
& "copied files to create the release. Before running make sure you have buit version " _
& "6 with the release\Any CPU options. "

set WshShell = WScript.CreateObject("WScript.Shell")
Set FSO = CreateObject("Scripting.FileSystemObject")

btn = WshShell.Popup(startTxt , 0, "Question:", &H4 + &H20)

Select Case btn
    ' Yes button pressed.
    case 6
        

        Copy5Assemblies()
        Copy6Assemblies()
		'If FSO.FileExists(HOSTS_DIR & "hosts" & strCopyFromExtension) Then    'Back it up first    FSO.CopyFile HOSTS_DIR & "hosts", HOSTS_DIR & "hosts" & strCopyFromExtension & ".BAK", True        'Then copy over    FSO.CopyFile HOSTS_DIR & "hosts" & strCopyFromExtension, HOSTS_DIR & "hosts", True        MsgBox "File " & "hosts" & strCopyFromExtension & _        " copied over the hosts file, which was back up as " & _        HOSTS_DIR & "hosts" & strCopyFromExtension & ".BAK" & ".", vbInformationElse    MsgBox "Could not find " & HOSTS_DIR & "hosts" & strCopyFromExtension & ".", vbExclamation    WScript.Quit 1End If

        WScript.Echo "Copy complete"




         

    ' No button pressed.
    case 7
        WScript.Echo "Good choice, try again later."
    ' Timed out.
End Select
WScript.Quit 1
Set FSO = Nothing
Set WshShell = Nothing


Function Copy6Assemblies
        Dim arrFiles(8), destDir
        destDir = "C:\ASCOM Trunk\Releases\ASCOM 6\Platform\Assemblies\v6\"

        FSO.DeleteFile(destDir + "*.*"), True

        arrFiles(0) = "C:\ASCOM Trunk\ASCOM.DeviceInterface\bin\Release\ASCOM.DeviceInterfaces.dll"
        arrFiles(1) = "C:\ASCOM Trunk\ASCOM.Astrometry\ASCOM.Astrometry\bin\Release\ASCOM.Astrometry.dll"
        arrFiles(2) = "C:\ASCOM Trunk\ASCOM.Attributes\bin\Release\ASCOM.Attributes.dll"
        arrFiles(3) = "C:\ASCOM Trunk\ASCOM.DriverAccess\bin\Release\ASCOM.DriverAccess.dll"
        arrFiles(4) = "C:\ASCOM Trunk\ASCOM.IConform\ASCOM.IConform\bin\Release\ASCOM.IConform.dll"
        arrFiles(5) = "C:\ASCOM Trunk\ASCOM.Utilities\ASCOM.Utilities\bin\Release\ASCOM.Utilities.dll"
        arrFiles(6) = "C:\ASCOM Trunk\Interfaces\ASCOMExceptions\bin\Release\ASCOM.Exceptions.dll"
        arrFiles(7) = "C:\ASCOM Trunk\Interfaces\HelperPIAs\ASCOM.Helper.dll"
        arrFiles(8) = "C:\ASCOM Trunk\Interfaces\HelperPIAs\ASCOM.Helper2.dll"

        For Each objFile In arrFiles
            FSO.CopyFile objFile, destDir , True
        Next
End Function



Function Copy5Assemblies
        Dim arrFiles(0), destDir
        destDir = "C:\ASCOM Trunk\Releases\ASCOM 6\Platform\Assemblies\"

        FSO.DeleteFile(destDir + "*.*"), True

        arrFiles(0) = "C:\ASCOM Trunk\ASCOM.DriverAccess.Platform5\bin\Release\ASCOM.DriverAccess.dll"
                
        For Each objFile In arrFiles
            FSO.CopyFile objFile, destDir , True
        Next
End Function


