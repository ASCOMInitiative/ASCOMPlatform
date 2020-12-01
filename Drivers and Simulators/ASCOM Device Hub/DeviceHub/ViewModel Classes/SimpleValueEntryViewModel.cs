using System.Collections.ObjectModel;

namespace ASCOM.DeviceHub
{
	internal class SimpleValueEntryViewModel : ValuesEntryViewModelBase
    {
		public SimpleValueEntryViewModel( string sourceID, string description, int maximum, char units )
			:this( sourceID, description, 0, maximum, 0, maximum, units)
		{ }

		public SimpleValueEntryViewModel( string sourceID, string description, int minimumScale, int maximumScale
											, int minimumValue, int maximumValue, char units )
			: base( sourceID )
		{
			_description = description;

			_rotarySliderVm = new RotarySliderViewModel
			{
				RotaryValues = new ObservableCollection<RotaryValue>
				{
					new RotaryValue( minimumValue, minimumScale, maximumScale, minimumValue, maximumValue, units)
				},
				IsValueDefined = new bool[] { true, false, false }
			};

			UsesSignsText = false;
			IsNegative = false;
		}

		public override void InitializeValues( int[] values )
		{
			_rotarySliderVm.RotaryValues[0].Value = values[0];
			_rotarySliderVm.SetValueIndex( 0 );
		}

		public override int[] GetValues()
		{
			int[] retvals = new int[1];

			retvals[0] = _rotarySliderVm.RotaryValues[0].Value;

			return retvals;
		}
	}
}
