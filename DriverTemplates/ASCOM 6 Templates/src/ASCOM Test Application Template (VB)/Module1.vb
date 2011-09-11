Module Module1

    Sub Main()
        Dim id As String = TEMPLATEDEVICECLASS.Choose("")

        Dim driver As TEMPLATEDEVICECLASS = New TEMPLATEDEVICECLASS(id)

        Console.WriteLine("Name " & driver.Name)
        Console.WriteLine("description " + driver.Description)
        Console.WriteLine("DriverInfo " + driver.DriverInfo)
        Console.WriteLine("driverVersion " + driver.DriverVersion)

        ' TODO add more code to test the driver.


        Console.WriteLine("Press Enter to finish")
        Console.ReadLine()

    End Sub

End Module
