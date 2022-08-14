Module Module1

    ' Comment out the "#Const UseChooser = True" line to bypass the code that uses the chooser to select the driver and replace it with code that accesses the driver directly via its ProgId.
#Const UseChooser = True

    Sub Main()

#If UseChooser Then
        ' Choose the device
        Dim id As String = ASCOM.DriverAccess.TEMPLATEDEVICECLASS.Choose("")

        ' Exit if no device was selected
        If (String.IsNullOrEmpty(id)) Then
            Return
        End If

        ' Create the device
        Dim driver As ASCOM.DriverAccess.TEMPLATEDEVICECLASS = New ASCOM.DriverAccess.TEMPLATEDEVICECLASS(id)
#Else
        ' Create the driver class directly.
        Dim driver As ASCOM.DriverAccess.TEMPLATEDEVICECLASS = New ASCOM.DriverAccess.TEMPLATEDEVICECLASS("ASCOM.TEMPLATEDEVICENAME.TEMPLATEDEVICECLASS")
#End If

        ' Disconnect from the device
        driver.Connected = True

        ' Now exercise some calls that are common to all drivers.
        Console.WriteLine($"Name: {driver.Name}")
        Console.WriteLine($"Description: {driver.Description}")
        Console.WriteLine($"DriverInfo: {driver.DriverInfo}")
        Console.WriteLine($"DriverVersion: {driver.DriverVersion}")
        Console.WriteLine($"InterfaceVersion: {driver.InterfaceVersion}")

        '
        ' TODO add more code to test your driver.
        '

        ' Disconnect from the device
        driver.Connected = False

        Console.WriteLine("Press Enter to finish")
        Console.ReadLine()

    End Sub

End Module
