using System.Collections.ObjectModel;

namespace ASCOM.DeviceHub
{
	internal class RightAscensionValuesEntryViewModel : ValuesEntryViewModelBase
	{
		public RightAscensionValuesEntryViewModel()
				: base( "RightAscensionValuesEntry")
		{
			_description = "Enter the target right ascension ";

			_rotarySliderVm = new RotarySliderViewModel
			{
				RotaryValues = new ObservableCollection<RotaryValue>
				{
					new RotaryValue( 0, 0, 24, 'h' ),
					new RotaryValue( 0, 0, 60, 'm' ),
					new RotaryValue( 0, 0, 60, 's' )
				},
				IsValueDefined = new bool[] { true, true, true }
			};
		}

		public override void InitializeValues( int[] values )
		{
			_rotarySliderVm.RotaryValues[0].Value = values[0];
			_rotarySliderVm.RotaryValues[1].Value = values[1];
			_rotarySliderVm.RotaryValues[2].Value = values[2];
			_rotarySliderVm.SetValueIndex( 0 );
		}

		public override int[] GetValues()
		{
			int[] retvals = new int[3];

			retvals[0] = _rotarySliderVm.RotaryValues[0].Value;
			retvals[1] = _rotarySliderVm.RotaryValues[1].Value;
			retvals[2] = _rotarySliderVm.RotaryValues[2].Value;

			return retvals;
		}
	}
}
