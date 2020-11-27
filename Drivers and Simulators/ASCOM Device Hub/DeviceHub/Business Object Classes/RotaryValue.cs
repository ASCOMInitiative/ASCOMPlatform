using System.Windows;

namespace ASCOM.DeviceHub
{
	public class RotaryValue : DependencyObject
	{
		public RotaryValue()
		{}

		public RotaryValue( int value, int minimumScale, int maximumScale, char units )
			: this( value, minimumScale, maximumScale, minimumScale, maximumScale, units )
		{ 
			// This constructor is used when the scale and the value range are the same.
		}

		public RotaryValue(int value, int minimumScale, int maximumScale, int minimumValue, int maximumValue, char units )
		{
			// Because of Conercion of the Dependency Properties, the order of the assignments
			// is critical. The Maximum must be set before the Minimum and the scale must be set
			// before the value rante. Then it is followed by the Value.

			MaximumScale = maximumScale;
			MinimumScale = minimumScale;
			MaximumValue = maximumValue;
			MinimumValue = minimumValue;
			Value = value;
			Units = units;
		}

		#region MinimumScale Dependency Property

		public int MinimumScale
		{
			get { return (int)GetValue( MinimumScaleProperty ); }
			set { SetValue( MinimumScaleProperty, value ); }
		}

		public static readonly DependencyProperty MinimumScaleProperty =
			DependencyProperty.Register( "Minimum", typeof( int ), typeof( RotaryValue )
				, new PropertyMetadata( 0, null, CoerceMinimumScale ) );

		private static object CoerceMinimumScale( DependencyObject d, object baseValue )
		{
			RotaryValue rotary = (RotaryValue)d;

			int newValue = (int)baseValue;

			if ( newValue > rotary.MaximumScale )
			{
				newValue = rotary.MaximumScale;
			}

			return newValue;
		}

		#endregion MinimumScale Dependency Property

		#region MaximumScale Dependency Property

		public int MaximumScale
		{
			get { return (int)GetValue( MaximumScaleProperty ); }
			set { SetValue( MaximumScaleProperty, value ); }
		}

		public static readonly DependencyProperty MaximumScaleProperty =
			DependencyProperty.Register( "Maximum", typeof( int ), typeof( RotaryValue )
				, new PropertyMetadata( 0, null, CoerceMaximumScale ) );

		private static object CoerceMaximumScale( DependencyObject d, object baseValue )
		{
			RotaryValue rotary = (RotaryValue)d;

			int newValue = (int)baseValue;

			if ( newValue < rotary.MinimumScale )
			{
				newValue = rotary.MinimumScale;
			}

			return newValue;
		}

		#endregion MaximumScale Dependency Property

		#region MinimumValue Dependency Property

		public int MinimumValue
		{
			get { return (int)GetValue( MinimumValueProperty ); }
			set { SetValue( MinimumValueProperty, value ); }
		}

		// Using a DependencyProperty as the backing store for MinimumValue.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty MinimumValueProperty =
			DependencyProperty.Register("MinimumValue", typeof(int), typeof(RotaryValue)
				, new PropertyMetadata(0, null, CoerceMinimumValue ) );

		private static object CoerceMinimumValue( DependencyObject d, object baseValue )
		{
			RotaryValue rotary = (RotaryValue)d;

			int newValue = (int)baseValue;

			if ( newValue < rotary.MinimumScale )
			{
				newValue = rotary.MinimumScale;
			}

			return newValue;
		}

		#endregion MinimumValue Dependency Property

		#region MaximumValue Dependency Property

		public int MaximumValue
		{
			get { return (int)GetValue( MaximumValueProperty ); }
			set { SetValue( MaximumValueProperty, value ); }
		}

		// Using a DependencyProperty as the backing store for MaximumValue.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty MaximumValueProperty =
			DependencyProperty.Register("MaximumValue", typeof(int), typeof(RotaryValue)
				, new PropertyMetadata(0, null, CoerceMaximumValue ) );

		private static object CoerceMaximumValue( DependencyObject d, object baseValue )
		{
			RotaryValue rotary = (RotaryValue)d;

			int newValue = (int)baseValue;

			if ( newValue > rotary.MaximumScale )
			{
				newValue = rotary.MaximumScale;
			}
			else if ( newValue < rotary.MinimumScale )
			{
				newValue = rotary.MinimumScale;
			}

			return newValue;
		}

		#endregion MaximumValue Dependency Property

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

			int newValue = (int)baseValue;

			if ( newValue < rotary.MinimumValue )
			{
				newValue = rotary.MinimumValue;
			}
			else if ( newValue > rotary.MaximumValue )
			{
				newValue = rotary.MaximumValue;
			}

			return newValue;
		}

		#endregion Value Dependency Property
	}
}
