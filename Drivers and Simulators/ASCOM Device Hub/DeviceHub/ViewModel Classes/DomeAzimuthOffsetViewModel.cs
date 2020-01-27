using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.DeviceHub
{
	internal class DomeAzimuthOffsetViewModel : ValuesEntryViewModelBase
	{
		public DomeAzimuthOffsetViewModel()
			: base( "DomeAzimuthAdjustment" )
		{
			_description = String.Format( "Enter the dome azimuth adjustment, in degrees" );

			_rotarySliderVm = new RotarySliderViewModel
			{
				RotaryValues = new System.Collections.ObjectModel.ObservableCollection<RotaryValue>
				{
					new RotaryValue( 0, 0, 11, ' ' )
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
