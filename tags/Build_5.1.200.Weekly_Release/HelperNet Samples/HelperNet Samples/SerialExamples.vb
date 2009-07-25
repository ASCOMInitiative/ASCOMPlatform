Public Class SerialExamples
    'Define the serial port variable
    Private SerPort As ASCOM.HelperNET.Interfaces.ISerial

    Sub BasicExample() 'This example shows a simple use of the ASCOM serial port
        'Define variables for strings to be transmitted and received
        Dim TXString, RXString As String

        'Serial port defaults are:  Speed 9600 baud
        '                           Timeout 5 seconds
        '                           Format 8 data bits, no parity, 1 stop bit
        '                           DTR enabled
        '                           No handshaking
        Try
            'Create the serial port
            SerPort = New ASCOM.HelperNET.Serial

            'Configure the port, if any defaults are not suitable, set the relevant SerPort properties here
            SerPort.Port = 1 'Change the 1 to the serial port to which your device is connected

            'Connect to the serial port
            SerPort.Connected = True

            'Send a command to the connected device
            TXString = "DeviceCommand" 'Change this to a command to your device that will return a response
            SerPort.Transmit(TXString)

            'Get the response from the device and display it
            RXString = SerPort.Receive
            MsgBox("Sent: " & TXString & " Received back: " & RXString)

            SerPort.Connected = False 'Disconnect and clean up
            SerPort.Dispose()
            SerPort = Nothing

        Catch ex As Exception 'Catch any exceptions and display
            MsgBox("Threw exception: " & ex.ToString)
            SerPort.Dispose()
            SerPort = Nothing
        End Try
    End Sub

    Sub FullExample()
        'Create variables for this example
        Dim ASCIIEncoding As New System.Text.ASCIIEncoding() 'Create an encoding to enable us to convert a string to a byte array
        Dim RxString As String, RxByte, RxBytes(), TxBytes(), TerminatorBytes() As Byte
        Dim AvailablePorts() As String

        'Create a new serial port
        SerPort = New ASCOM.HelperNET.Serial

        'Set up the basic port parameters
        'You only need to use one of the Port or PortName options, its your choice as appropriate
        SerPort.Port = 1 'Set to port COM1 - please choose an appropriate value for your setup
        SerPort.PortName = "COM1" 'Achieves same as above but gives flexibility where port name is not of form COMn
        SerPort.Speed = ASCOM.HelperNET.Serial.PortSpeed.ps9600 'Set to baud rate 9600, choose an appropriate value for your system
        'You only need to use one of the ReceiveTimeout or ReceiveTimeoutMs options, its your choice as appropriate
        SerPort.ReceiveTimeout = 5 'Set the time (in seconds) that the receive commands will wait before returning a timeout exception
        SerPort.ReceiveTimeoutMs = 5000 'Set the time (in milli-seconds) that the receive commands will wait before returning a timeout exception

        'Set serial format and serial line parameters as required
        'Following values are the defaults so you don't need to set them if they work OK with your device
        SerPort.DataBits = 8 ' Set 8 data bits
        SerPort.Parity = IO.Ports.Parity.None 'Set no parity bit
        SerPort.StopBits = IO.Ports.StopBits.One 'Set one stop bit
        SerPort.DTREnable = True 'Set the DTR line high
        SerPort.Handshake = IO.Ports.Handshake.None 'Don't use hardware or software flow control on the serial line

        'Create the serial port and ready it for use
        SerPort.Connected = True

        'Transmit examples
        SerPort.Transmit("Hello World") 'Send a string message to the device through the serial port
        TxBytes = ASCIIEncoding.GetBytes("Hello World As Byte Array")
        SerPort.TransmitBinary(TxBytes) 'Send a message held in a byte array to the device'

        'Receive examples
        'These will fail if no chacraters have been previously sent to the port
        RxString = SerPort.Receive() '"Get all characters that have been received by the serial port as a atring
        RxByte = SerPort.ReceiveByte() 'Get one character as a byte from the serial port
        RxString = SerPort.ReceiveCounted(5) 'Get 5 characters from the serial port as a string
        RxBytes = SerPort.ReceiveCountedBinary(5) 'get 5 characters from the serial port as a byte array
        RxString = SerPort.ReceiveTerminated("#") 'Get all characters up to and including the # character
        TerminatorBytes = ASCIIEncoding.GetBytes("#") 'Create the terminator byte array from a string value
        RxBytes = SerPort.ReceiveTerminatedBinary(TerminatorBytes) REM Get all characters up to and including the specified terminator bytes as a byte array

        'Utility examples
        AvailablePorts = SerPort.AvailableComPorts 'Get a list of installed COM ports
        SerPort.LogMessage("MyID", "My message") 'Add a custom message to the serial log file
        SerPort.ClearBuffers() 'Use this wherever you want to be sure that the serial port is ready for the next command

        'Clean up and release the serial port
        SerPort.Connected = False 'Disconnect the serial port
        SerPort.Dispose() 'Release serial port resources
        SerPort = Nothing

    End Sub

End Class