using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ASCOM.DeviceHub
{
	public class RelayCommand : ICommand
	{
		#region Static Members

		private static bool InvokeRequired
		{
			get
			{
				bool retval = false;

				// NOTE:  the application will be null if this is called from a ViewModel that is running
				//        under a unit test.

				if ( Application.Current != null )
				{
					retval = Application.Current.Dispatcher.Thread.ManagedThreadId != Thread.CurrentThread.ManagedThreadId;
				}

				return retval;
			}
		}
		
		public static void RaiseCanExecuteChanged()
		{
			if ( InvokeRequired )
			{
				Application.Current.Dispatcher.Invoke( DispatcherPriority.Normal, new Action( RaiseCanExecuteChanged ) );
			}
			else
			{
				CommandManager.InvalidateRequerySuggested();
			}
		}

		#endregion Static Members

		#region Fields

		private Action<object> _execute;
		private Predicate<object> _canExecute;

		#endregion Fields

		#region Constructors

		public RelayCommand( Action<object> execute )
			: this( execute, null )
		{}

		public RelayCommand( Action<object> execute, Predicate<object> canExecute )
		{
			_execute = execute ?? throw new ArgumentNullException( "execute" );
			_canExecute = canExecute;
		}

		#endregion Constructors

		public void Revoke()
		{
			_canExecute = null;
			_execute = null;
		}

		#region ICommand Members

		[DebuggerStepThrough]
		public bool CanExecute( object parameters )
		{
			return _canExecute == null ? true : _canExecute( parameters );
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void Execute( object parameters )
		{
			_execute( parameters );
		}

		#endregion ICommand Members
	}
}
