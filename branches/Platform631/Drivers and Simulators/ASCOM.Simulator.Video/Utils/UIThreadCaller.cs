//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video Driver - Simulator
//
// Description:	This file implements a helper class to run delegates on a UI thread with a message loop
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ASCOM.Simulator.Utils
{
	public class UIThreadCaller
	{
		public delegate void CallInUIThreadCallback(IWin32Window applicationWindow, params object[] additionalParams);

		public static void Invoke(CallInUIThreadCallback action, params object[] additionalParams)
		{
			Form appFormWithMessageLoop = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x != null && x.Owner == null);
			if (appFormWithMessageLoop == null) appFormWithMessageLoop = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x != null);

			if (appFormWithMessageLoop != null && (Application.MessageLoop || hasOwnMessageLoop))
			{
				if (appFormWithMessageLoop.InvokeRequired)
				{
					DebugTrace.TraceInfo("Making UIThreadCaller call on the thread of existing top level form.");
					appFormWithMessageLoop.Invoke(action, appFormWithMessageLoop, additionalParams);
				}
				else
				{
					DebugTrace.TraceInfo("Making UIThreadCaller call on the current thread as the found top level form runs on the same thread as us.");
					action.Invoke(appFormWithMessageLoop, additionalParams);
				}
			}
			else if ((!Application.MessageLoop || appFormWithMessageLoop == null) && syncContext == null)
			{
				if (syncContext == null)
				{
					DebugTrace.TraceInfo("UIThreadCaller is creating a MessageLoop thread.");
					ThreadPool.QueueUserWorkItem(RunAppThread);
					while (syncContext == null) Thread.Sleep(10);
				}

				if (syncContext != null)
				{
					DebugTrace.TraceInfo("Making UIThreadCaller call on an existing WindowsFormsSynchronizationContext.");
					bool callFinished = false;
					syncContext.Post(new SendOrPostCallback(delegate(object state)
						{
							action.Invoke(appFormWithMessageLoop != null && !appFormWithMessageLoop.InvokeRequired ? appFormWithMessageLoop : null, additionalParams);
							callFinished = true;
						}), null);
					while (!callFinished) Thread.Sleep(10);
				}
				else
				{
					DebugTrace.TraceInfo("Making UIThreadCaller call directly.");
					action.Invoke(appFormWithMessageLoop != null && !appFormWithMessageLoop.InvokeRequired ? appFormWithMessageLoop : null, additionalParams);
				}
			}
			else
			{
				if (syncContext == null)
				{
					DebugTrace.TraceInfo("UIThreadCaller is creating new WindowsFormsSynchronizationContext on the current thread.");
					syncContext = new WindowsFormsSynchronizationContext();
				}

				if (syncContext != null)
				{
					DebugTrace.TraceInfo("Making UIThreadCaller call on an existing WindowsFormsSynchronizationContext.");
					bool callFinished = false;
					syncContext.Post(new SendOrPostCallback(delegate(object state)
					{
						action.Invoke(appFormWithMessageLoop != null && !appFormWithMessageLoop.InvokeRequired ? appFormWithMessageLoop : null, additionalParams);
						callFinished = true;
					}), null);
					while (!callFinished) Thread.Sleep(10);
				}
				else
				{
					DebugTrace.TraceInfo("Making UIThreadCaller call directly.");
					action.Invoke(appFormWithMessageLoop != null && !appFormWithMessageLoop.InvokeRequired ? appFormWithMessageLoop : null, additionalParams);
				}
			}
		}

		private static WindowsFormsSynchronizationContext syncContext;
		private static bool hasOwnMessageLoop = false;

		private static void RunAppThread(object state)
		{
			var ownMessageLoopMainForm = new Form();
			ownMessageLoopMainForm.ShowInTaskbar = false;
			ownMessageLoopMainForm.Width = 0;
			ownMessageLoopMainForm.Height = 0;
			ownMessageLoopMainForm.Load += ownerForm_Load;
			
			Application.Run(ownMessageLoopMainForm);

			if (syncContext != null)
			{
				syncContext.Dispose();
				syncContext = null;
			}
		}

		static void ownerForm_Load(object sender, EventArgs e)
		{
			Form form = (Form)sender;
			form.Left = -5000;
			form.Top = -5000;
			form.Hide();

			DebugTrace.TraceInfo("UIThreadCaller is creating new WindowsFormsSynchronizationContext on the newly created MessageLoop thread.");
			syncContext = new WindowsFormsSynchronizationContext();

			hasOwnMessageLoop = true;
		}
	}
}
