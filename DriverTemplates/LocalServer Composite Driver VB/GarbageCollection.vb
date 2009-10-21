
Imports System
Imports System.Threading

Namespace TEMPLATENAMESPACE
	''' <summary>
	''' Summary description for GarbageCollection.
	''' </summary>
	Class GarbageCollection
		Protected m_bContinueThread As Boolean
		Protected m_GCWatchStopped As Boolean
		Protected m_iInterval As Integer
		Protected m_EventThreadEnded As ManualResetEvent

		Public Sub New(iInterval As Integer)
			m_bContinueThread = True
			m_GCWatchStopped = False
			m_iInterval = iInterval
			m_EventThreadEnded = New ManualResetEvent(False)
		End Sub

		Public Sub GCWatch()
			' Pause for a moment to provide a delay to make threads more apparent.
			While ContinueThread()
				GC.Collect()
				Thread.Sleep(m_iInterval)
			End While
			m_EventThreadEnded.[Set]()
		End Sub

		Protected Function ContinueThread() As Boolean
			SyncLock Me
				Return m_bContinueThread
			End SyncLock
		End Function

		Public Sub StopThread()
			SyncLock Me
				m_bContinueThread = False
			End SyncLock
		End Sub

		Public Sub WaitForThreadToStop()
			m_EventThreadEnded.WaitOne()
			m_EventThreadEnded.Reset()
		End Sub
	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Built and maintained by Todd Anglin and Telerik
'=======================================================
