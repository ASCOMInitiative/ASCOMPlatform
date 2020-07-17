using System.Windows;

namespace ASCOM.DeviceHub
{
	public class RotaryValue : DependencyObject
	{
		public RotaryValue()
		{}

		public RotaryValue( int value, int minimum, int maximum, char units )
		{
			Value = value;
			Minimum = minimum;
			Maximum = maximum;
			Units = units;
		}

		#region Minimum Dependency Property

		public int Minimum
		{
			get { return (int)GetValue( MinimumProperty ); }
			set { SetValue( MinimumProperty, value ); }
		}

		public static readonly DependencyProperty MinimumProperty =
			DependencyProperty.Register( "Minimum", typeof( int ), typeof( RotaryValue )
				, new PropertyMetadata( 0, null, CoerceMinimum ) );

		private static object CoerceMinimum( DependencyObject d, object baseValue )
		{
			RotaryValue rotary = (RotaryValue)d;

			int newValue = (int)baseValue;

			if ( newValue > rotary.Maximum )
			{
				newValue = rotary.Maximum;
			}

			return newValue;
		}

		#endregion Minimum Dependency Property

		#region Maximum Dependency Property

		public int Maximum
		{
			get { return (int)GetValue( MaximumProperty ); }
			set { SetValue( MaximumProperty, value ); }
		}

		public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register( "Maximum", typeof( int ), typeof( RotaryValue )
				, new PropertyMetadata( 0, null, CoerceMaximum ) );

		private static object CoerceMaximum( DependencyObject d, object baseValue )
		{
			RotaryValue rotary = (RotaryValue)d;

			int newValue = (int)baseValue;

			if ( newValue < rotary.Minimum )
			{
				newValue = rotary.Minimum;
			}

			return newValue;
		}

		#endregion Maximum Dependency Property

		#region Units Dependency Property
			   
		public char Units
		{
			get { return (char)GetValue( UnitsProperty ); }
			set { SetValue( UnitsProperty, value ); }
		}

		public static readonly DependencyProperty UnitsProperty =
			DependencyProperty.Register( "Units", typeof( char ), typeof( RotaryValue ), new PropertyMetadata( ' ' ) );

		#endregion Units Dependency Property

		#region Value Dependency Property

		public int Value
		{
			get { return (int)GetValue( ValueProperty ); }
			set { SetValue( ValueProperty, value ); }
		}

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register( "Value", typeof( int ), typeof( RotaryValue )
				, new PropertyMetadata( 0, null, CoerceValue ) );

		private static object CoerceValue( DependencyObject d, object baseValue )
		{
			RotaryValue rotary = (RotaryValue)d;
			int min = rotary.Minimum;
			int max = rotary.Maximum;

			int newValue = (int)baseValue;

			if ( newValue < min )
			{
				newValue = min;
			}
			else if ( newValue > max )
			{
				newValue = max;
			}

			return newValue;
		}

		#endregion Value Dependency Property
	}
}
