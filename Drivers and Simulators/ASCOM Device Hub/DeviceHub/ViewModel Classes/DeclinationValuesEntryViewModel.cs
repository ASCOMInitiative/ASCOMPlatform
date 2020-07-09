using System.Collections.ObjectModel;

namespace ASCOM.DeviceHub
{
	internal class DeclinationValuesEntryViewModel : ValuesEntryViewModelBase
	{
		public DeclinationValuesEntryViewModel()
			: base( "DeclinationValuesEntry" )
		{
			_description = "Enter the target declination";

			_rotarySliderVm = new RotarySliderViewModel
			{
				RotaryValues = new ObservableCollection<RotaryValue>
				{
					new RotaryValue( 0, 0, 90, '°' ),
					new RotaryValue( 0, 0, 60, '\'' ),
					new RotaryValue( 0, 0, 60, '"' )
				},
				IsValueDefined = new bool[] { true, true, true }
			};

			UsesSignsText = true;
			NegativeText = "South";
			PositiveText = "North";
			IsNegative = false; // since RotaryValues[0].Value is not negative
		}

		public override void InitializeValues( int[] values )
		{
			int degrees = values[0];

			if ( degrees < 0 )
			{
				degrees = -degrees;
				IsNegative = true;
			}

			_rotarySliderVm.RotaryValues[0].Value = degrees;
			_rotarySliderVm.RotaryValues[1].Value = values[1];
			_rotarySliderVm.RotaryValues[2].Value = values[2];
			_rotarySliderVm.SetValueIndex( 0 );
		}

		public override int[] GetValues()
		{
			int[] retvals = new int[3];

			int degrees = _rotarySliderVm.RotaryValues[0].Value;

			if ( IsNegative )
			{
				degrees = -degrees;
			}

			retvals[0] = degrees;
			retvals[1] = _rotarySliderVm.RotaryValues[1].Value;
			retvals[2] = _rotarySliderVm.RotaryValues[2].Value;

			return retvals;
		}
	}
}
