Module Module1

    Sub Main()
        ' Uncomment the code that's required
        ' choose the device
        Dim id As String = ASCOM.DriverAccess.TEMPLATEDEVICECLASS.Choose("")
        If (String.IsNullOrEmpty(id)) Then
            Return
        End If

        ' create this device
        Dim driver As ASCOM.DriverAccess.TEMPLATEDEVICECLASS = New ASCOM.DriverAccess.TEMPLATEDEVICECLASS(id)

        ' this can be replaced by this code, it avoids the chooser and creates the driver class directly.
        ' Dim device As ASCOM.DriverAccess.TEMPLATEDEVICECLASS = New ASCOM.DriverAccess.TEMPLATEDEVICECLASS("ASCOM.TEMPLATEDEVICENAME.TEMPLATEDEVICECLASS")

        ' now run some tests, adding code to your driver so that the tests will pass.
        ' these first tests are common to all drivers.
        Console.WriteLine("Name " & driver.Name)
        Console.WriteLine("description " + driver.Description)
        Console.WriteLine("DriverInfo " + driver.DriverInfo)
        Console.WriteLine("driverVersion " + driver.DriverVersion)

        ' TODO add more code to test the driver.


        Console.WriteLine("Press Enter to finish")
        Console.ReadLine()

    End Sub

End Module
