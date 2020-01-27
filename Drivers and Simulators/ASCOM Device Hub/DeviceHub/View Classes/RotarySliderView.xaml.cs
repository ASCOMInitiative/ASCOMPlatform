using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ASCOM.DeviceHub
{
	/// <summary>
	/// Interaction logic for RotarySlider.xaml
	/// </summary>
	public partial class RotarySliderView : UserControl
	{
		private bool _isPressed = false;
		private Canvas _templateCanvas = null;
		private UIElement _capturingElement = null;

		public RotarySliderView()
		{
			InitializeComponent();

			//HideValues();
		}

		#region Dependency Properties

		public Brush RotaryFace
		{
			get { return (Brush)GetValue( RotaryFaceProperty ); }
			set { SetValue( RotaryFaceProperty, value ); }
		}

		public static readonly DependencyProperty RotaryFaceProperty =
			DependencyProperty.RegisterAttached( "RotaryFace", typeof( Brush ), typeof( RotarySliderView )
				, new PropertyMetadata( Brushes.LightBlue ) );

		public Brush RotaryHand
		{
			get { return (Brush)GetValue( RotaryHandProperty ); }
			set { SetValue( RotaryHandProperty, value ); }
		}

		public static readonly DependencyProperty RotaryHandProperty =
			DependencyProperty.RegisterAttached( "RotaryHand", typeof( Brush ), typeof( RotarySliderView )
				, new PropertyMetadata( Brushes.Red ) );

		public Brush SelectedValueBrush
		{
			get { return (Brush)GetValue( SelectedValueBrushProperty ); }
			set { SetValue( SelectedValueBrushProperty, value ); }
		}

		public static readonly DependencyProperty SelectedValueBrushProperty =
			DependencyProperty.RegisterAttached( "SelectedValueBrush", typeof( Brush ), typeof( RotarySliderView )
				, new PropertyMetadata( Brushes.DarkBlue ) );

		#endregion Dependency Properties

		#region U/I Event Handlers

		private void Ellipse_MouseLeftButtonDown( object sender, MouseButtonEventArgs e )
		{
			// Mouse movement causes the value to change.

			_isPressed = true;

			// Let mouse movement control the knob pointer, even when the mouse is outside of the control.

			_capturingElement = (UIElement)_knob.Template.FindName( "_bigCircle", _knob );
			_capturingElement.CaptureMouse();
		}

		private void Ellipse_MouseLeftButtonUp( object sender, MouseButtonEventArgs e )
		{
			// Mouse movement does not cause the value to change.

			if ( _capturingElement != null )
			{
				_capturingElement.ReleaseMouseCapture();
				_capturingElement = null;
			}

			_isPressed = false;
		}

		private void _bigCircle_MouseMove( object sender, MouseEventArgs e )
		{
			double radius = 150;

			if ( _isPressed )
			{
				//Find the parent canvas.

				if ( _templateCanvas == null )
				{
					_templateCanvas = FindParentCanvas( e.Source as Ellipse );

					if ( _templateCanvas == null )
					{
						return;
					}
				}

				// Calculate the current rotation angle and set the value.

				Point newPos = e.GetPosition( _templateCanvas );
				double angle = GetRotationAngle( newPos, radius );
				double angleDeg = angle * 360.0 / ( 2 * Math.PI );
				double newValue = ( _knob.Maximum - _knob.Minimum ) * angle / ( 2 * Math.PI );
				newValue = Math.Round( newValue / _knob.TickFrequency ) * _knob.TickFrequency;

				if ( newValue >= _knob.Maximum )
				{
					newValue = _knob.Minimum;
				}

				bool isOver = _knob.IsMouseOver;
				_knob.Value = newValue;
			}
		}

		private void _value0_GotFocus( object sender, RoutedEventArgs e )
		{
			_value0.Foreground = this.SelectedValueBrush;
			_value1.Foreground = this.Foreground;
			_value2.Foreground = this.Foreground;

			RotarySliderViewModel vm = (RotarySliderViewModel)DataContext;
			vm.SetValueIndex( 0 );
		}

		private void _value1_GotFocus( object sender, RoutedEventArgs e )
		{
			_value0.Foreground = this.Foreground;
			_value1.Foreground = this.SelectedValueBrush;
			_value2.Foreground = this.Foreground;

			RotarySliderViewModel vm = (RotarySliderViewModel)DataContext;
			vm.SetValueIndex( 1 );
		}

		private void _value2_GotFocus( object sender, RoutedEventArgs e )
		{
			_value0.Foreground = this.Foreground;
			_value1.Foreground = this.Foreground;
			_value2.Foreground = this.SelectedValueBrush;

			RotarySliderViewModel vm = (RotarySliderViewModel)DataContext;
			vm.SetValueIndex( 2 );
		}

		#endregion U/I Event Handlers

		#region Helper Methods

		private Canvas FindParentCanvas( Ellipse source )
		{
			FrameworkElement current = source;

			do
			{
				current = VisualTreeHelper.GetParent( current ) as FrameworkElement;

				if ( current is Canvas )
				{
					return (Canvas)current;
				}
			}
			while ( current != null );

			return null;
		}

		//Get the rotation angle from the position of the mouse

		private double GetRotationAngle( Point pos, double radius )
		{
			//Calculate out the distance (r) between the center and the position

			Point center = new Point( radius, radius );

			double xDiff = center.X - pos.X;
			double yDiff = center.Y - pos.Y;
			double r = Math.Sqrt( xDiff * xDiff + yDiff * yDiff );

			//Calculate the angle

			double angle = Math.Acos( ( center.Y - pos.Y ) / r );

			// If to the left of center, subtract the angle from 360.0

			if ( xDiff > 0.0 )
			{
				angle = 2 * Math.PI - angle;
			}

			angle = Double.IsNaN( angle ) ? 0.0 : angle;

			if ( Math.Abs( angle - 2 * Math.PI ) < 1E-05 )
			{
				angle = 0.0;
			}

			return angle;
		}

		private void HideValues()
		{
			_value0.Visibility = Visibility.Collapsed;
			_units0.Visibility = Visibility.Collapsed;
			_value1.Visibility = Visibility.Collapsed;
			_units1.Visibility = Visibility.Collapsed;
			_value2.Visibility = Visibility.Collapsed;
			_units2.Visibility = Visibility.Collapsed;
		}

		#endregion Helper Methods
	}
}
