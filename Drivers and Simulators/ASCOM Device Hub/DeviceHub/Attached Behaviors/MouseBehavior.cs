using System.Windows;
using System.Windows.Input;

namespace ASCOM.DeviceHub
{
	public class MouseBehavior
	{
		#region MouseDown Command

		public static readonly DependencyProperty MouseDownCommandProperty =
			DependencyProperty.RegisterAttached( "MouseDownCommand", typeof( ICommand ),
												typeof( MouseBehavior ), new FrameworkPropertyMetadata(
												new PropertyChangedCallback( MouseDownCommandChanged ) ) );

		public static DependencyProperty MouseDownCommandParameterProperty =
			   DependencyProperty.RegisterAttached( "MouseDownCommandParameter",
												typeof( object ),
												typeof( MouseBehavior ),
												new UIPropertyMetadata( null ) );

		public static void SetMouseDownCommandParameter( DependencyObject target, object value )
		{
			target.SetValue( MouseDownCommandParameterProperty, value );
		}

		public static object GetMouseDownCommandParameter( DependencyObject target )
		{
			return target.GetValue( MouseDownCommandParameterProperty );
		}

		private static void MouseDownCommandChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			FrameworkElement element = (FrameworkElement)d;

			if ( element != null )
			{
				if ( e.NewValue != null && e.OldValue == null )
				{
					element.PreviewMouseDown += new MouseButtonEventHandler( Element_MouseDown );
				}
				else if ( ( e.NewValue == null ) && ( e.OldValue != null ) )
				{
					element.PreviewMouseDown -= new MouseButtonEventHandler( Element_MouseDown );
				}
			}
		}

		static void Element_MouseDown( object sender, MouseButtonEventArgs e )
		{
			FrameworkElement element = (FrameworkElement)sender;

			ICommand command = GetMouseDownCommand( element );
			object commandParameter = element.GetValue( MouseDownCommandParameterProperty );

			command.Execute( commandParameter );
		}

		public static void SetMouseDownCommand( UIElement element, ICommand value )
		{
			element.SetValue( MouseDownCommandProperty, value );
		}

		public static ICommand GetMouseDownCommand( UIElement element )
		{
			return (ICommand)element.GetValue( MouseDownCommandProperty );
		}

		#endregion MouseDown Command

		#region MouseUp Command

		public static readonly DependencyProperty MouseUpCommandProperty =
			DependencyProperty.RegisterAttached( "MouseUpCommand", typeof( ICommand ),
												typeof( MouseBehavior ), new FrameworkPropertyMetadata(
												new PropertyChangedCallback( MouseUpCommandChanged ) ) );

		public static DependencyProperty MouseUpCommandParameterProperty =
			   DependencyProperty.RegisterAttached( "MouseUpCommandParameter",
												typeof( object ),
												typeof( MouseBehavior ),
												new UIPropertyMetadata( null ) );

		public static void SetMouseUpCommandParameter( DependencyObject target, object value )
		{
			target.SetValue( MouseUpCommandParameterProperty, value );
		}

		public static object GetMouseUpCommandParameter( DependencyObject target )
		{
			return target.GetValue( MouseUpCommandParameterProperty );
		}

		private static void MouseUpCommandChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			FrameworkElement element = (FrameworkElement)d;

			if ( element != null )
			{
				if ( e.NewValue != null && e.OldValue == null )
				{
					element.PreviewMouseUp += new MouseButtonEventHandler( Element_MouseUp );
				}
				else if ( ( e.NewValue == null ) && ( e.OldValue != null ) )
				{
					element.PreviewMouseUp -= new MouseButtonEventHandler( Element_MouseUp );
				}
			}
		}

		static void Element_MouseUp( object sender, MouseButtonEventArgs e )
		{
			FrameworkElement element = (FrameworkElement)sender;

			ICommand command = GetMouseUpCommand( element );
			object commandParameter = element.GetValue( MouseUpCommandParameterProperty );

			command.Execute( commandParameter );
		}

		public static void SetMouseUpCommand( UIElement element, ICommand value )
		{
			element.SetValue( MouseUpCommandProperty, value );
		}

		public static ICommand GetMouseUpCommand( UIElement element )
		{
			return (ICommand)element.GetValue( MouseUpCommandProperty );
		}
	}

	#endregion MouseUp Command
}
