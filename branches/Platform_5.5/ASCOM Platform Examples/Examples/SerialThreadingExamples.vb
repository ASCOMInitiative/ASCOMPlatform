'This example is advanced and demonstrates in the SerWorker class how to use a Mutex to protect
'a serial transmit - receive transaction. Sub ThreadingExample is the main controller that starts the threads

'To run this example you will need to customise the serial portname for the device that you are talking to
'and the commands and expected responses. The example commands and responses are for the Gemini level 4 controller

'Main thread class
Public Class SerialThreadingExamples

    'This example starts three threads t1 to 3 each running an instanace of the SerWorker class, SerWorker1 to 3
    'Each SerWork instance has different values set for the command to be sent, the expected result and its instance number
    'A common serial port SerPort is created along with a common Mutex to synchronise access to the serial port
    'so that the threads will access the serial port one at a time.
    Sub ThreadingExample()
        'Define the threads and SerWorker instances
        Dim t1, t2, t3 As System.Threading.Thread
        Dim SerWorker1, SerWorker2, SerWorker3 As New SerialWorker

        'Create the Mutex, Serial Port and a trace logger instance to record the main thread output.
        Dim Mutex As New System.Threading.Mutex
        Dim SerPort As New ASCOM.Utilities.Serial
        Dim TLMain As New ASCOM.Utilities.TraceLogger("", "SerialMainThread")

        Try
            'Set up the  serial port and tracelogger
            SerPort.PortName = "COM1"
            SerPort.Connected = True
            TLMain.Enabled = True

            'Create the threads and point them at the three serworker instances
            t1 = New System.Threading.Thread(AddressOf SerWorker1.SerialWork)
            t2 = New System.Threading.Thread(AddressOf SerWorker2.SerialWork)
            t3 = New System.Threading.Thread(AddressOf SerWorker3.SerialWork)

            'Set up the commands and responses for the three serworker instances
            SerWorker1.Command = ":GVP#"
            SerWorker1.Response = "Losmandy Gemini#"
            SerWorker1.SerPort = SerPort
            SerWorker1.Instance = 1
            SerWorker1.SerMutex = Mutex

            SerWorker2.Command = ":GVN#"
            SerWorker2.Response = "4.10#"
            SerWorker2.SerPort = SerPort
            SerWorker2.Instance = 2
            SerWorker2.SerMutex = Mutex

            SerWorker3.Command = ":GV#"
            SerWorker3.Response = "410#"
            SerWorker3.SerPort = SerPort
            SerWorker3.Instance = 3
            SerWorker3.SerMutex = Mutex

            'Start the threads and wait a quarter of a second between each start
            t1.Start()
            System.Threading.Thread.Sleep(250)
            t2.Start()
            System.Threading.Thread.Sleep(250)
            t3.Start()

            'Now send this main thread to sleep for 5 seconds while the three worker threads do their work.
            'The 500 commands per worker take about 4 seconds in total to process so are finished before
            'the main thread wakes up from its 5 second sleep
            TLMain.LogMessage("SerialThread", "Starting sleep")
            System.Threading.Thread.Sleep(5000)
            TLMain.LogMessage("SerialThread", "Finished sleep")

            'Clean up the serial port and trace logger
            SerPort.Connected = False
            SerPort.Dispose()
            SerPort = Nothing
            TLMain.Enabled = False
            TLMain.Dispose()
            TLMain = Nothing
        Catch ex As Exception
            TLMain.LogMessage("SerialThread", "Exception " & ex.ToString)
        End Try
    End Sub
End Class

'Worker thread class
Public Class SerialWorker
    Public Command As String
    Public Response As String
    Public Instance As Integer
    Public SerPort As ASCOM.Utilities.Serial 'Serial port to use
    Public SerMutex As System.Threading.Mutex 'Semaphore to use 
    'Worker class that will run on a thread and undertake 500 transmit - receive transactions
    'checking each time that the received response is the expected response.
    Public Sub SerialWork()
        'Create a trace logger to log information
        Dim TL As New ASCOM.Utilities.TraceLogger("", "SerialThread " & Instance.ToString)
        Dim SerResponse As String

        Try
            'Enable the trace logger and report the given parameters
            TL.Enabled = True
            TL.LogMessage("Command", Command)
            TL.LogMessage("Response", Response)
            TL.LogMessage("Instance", Instance)

            For i = 1 To 500
                TL.LogStart("SerialThread " & Instance, "Loop " & i.ToString & " ")

                'Start the transaction by waiting until the mutex is granted
                SerMutex.WaitOne()
                'Now we have the mutex so send the command and wait for its response
                SerPort.Transmit(Command)
                SerResponse = SerPort.ReceiveTerminated("#")
                'Release the mutex so the next thread can start
                SerMutex.ReleaseMutex()

                'Log the response and compare it to what is expected.
                TL.LogFinish(SerResponse)
                If SerResponse <> Response Then
                    TL.LogMessage("*****", "Response not what was expected: " & SerResponse & " " & Response)
                End If

            Next
        Catch ex As Exception
            TL.LogFinish(" " & ex.ToString)
        End Try

        'Clean up the trace logger
        Try : TL.Enabled = False : Catch : End Try
        Try : TL.Dispose() : Catch : End Try
        TL = Nothing

    End Sub
End Class
