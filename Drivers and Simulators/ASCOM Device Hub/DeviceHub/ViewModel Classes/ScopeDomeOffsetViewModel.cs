using System;
using System.Collections.ObjectModel;

namespace ASCOM.DeviceHub
{
	internal class ScopeDomeOffsetViewModel : ValuesEntryViewModelBase
	{
		public ScopeDomeOffsetViewModel( string offsetDirections )
			: base( offsetDirections + "OffsetValuesEntry" )
		{
			_description = String.Format( "Enter the {0} offset distance, in mm", offsetDirections );

			_rotarySliderVm = new RotarySliderViewModel
			{
				RotaryValues = new ObservableCollection<RotaryValue>
				{
					new RotaryValue( 0, 0, +5000, ' ' )
				},
				IsValueDefined = new bool[] { true, false, false }
			};

			UsesSignsText = true;
			NegativeText = "----";
			PositiveText = "++++";
			IsNegative = false; // since RotaryValues[0].Value is not negative
		}

		public override void InitializeValues( int[] values )
		{
			int offset = values[0];

			if ( offset < 0 )
			{
				offset = -offset;
				IsNegative = true;
			}

			_rotarySliderVm.RotaryValues[0].Value = offset;
			_rotarySliderVm.SetValueIndex( 0 );
		}

		public override int[] GetValues()
		{
			int[] retvals = new int[1];

			int offset = _rotarySliderVm.RotaryValues[0].Value;

			if ( IsNegative )
			{
				offset = -offset;
			}

			retvals[0] = offset;

			return retvals;
		}
	}
}
