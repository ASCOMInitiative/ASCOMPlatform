using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.DeviceHub
{
	internal class AzimuthValuesEntryViewModel : ValuesEntryViewModelBase
	{
		public AzimuthValuesEntryViewModel()
			: base( "AzimuthValuesEntry" )
		{
			_description = "Enter the target azimuth";

			_rotarySliderVm = new RotarySliderViewModel
			{
				RotaryValues = new ObservableCollection<RotaryValue>
				{
					new RotaryValue( 0, 0, 360, '°' ),
					new RotaryValue( 0, 0, 60, '\'' ),
					new RotaryValue( 0, 0, 60, '"' )
				},
				IsValueDefined = new bool[] { true, true, true }
			};

			UsesSignsText = false;
			IsNegative = false; // since RotaryValues[0].Value is not negative
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
