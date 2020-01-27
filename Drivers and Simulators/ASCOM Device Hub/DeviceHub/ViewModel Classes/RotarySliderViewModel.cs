using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ASCOM.DeviceHub
{
	public class RotarySliderViewModel : DeviceHubViewModelBase
    {
		public RotarySliderViewModel()
		{
			SelectedIndex = -1;
			SelectedValue = null;
		}

		private ObservableCollection<RotaryValue> _rotaryValues;

		public ObservableCollection<RotaryValue> RotaryValues
		{
			get { return _rotaryValues; }
			set
			{
				if ( value != _rotaryValues )
				{
					_rotaryValues = value;
					OnPropertyChanged();
				}
			}
		}

		protected bool[] _isValueDefined;

		public bool[] IsValueDefined
		{
			get { return _isValueDefined; }
			set
			{
				if ( value != _isValueDefined )
				{
					_isValueDefined = value;
					OnPropertyChanged();
				}
			}
		}

		private int _selectedIndex;

		public int SelectedIndex
		{
			get { return _selectedIndex; }
			set
			{
				if ( value != _selectedIndex )
				{
					_selectedIndex = value;
					OnPropertyChanged();
				}
			}
		}

		private RotaryValue _selectedValue;

		public RotaryValue SelectedValue
		{
			get { return _selectedValue; }
			set
			{
				if ( value != _selectedValue )
				{
					_selectedValue = value;
					OnPropertyChanged();
				}
			}
		}

		public void SetValueIndex( int index )
		{
			SelectedValue = RotaryValues[index];
			SelectedIndex = index;
		}


		#region AdjustValueCommand

		private ICommand _adjustValueCommand;

		public ICommand AdjustValueCommand
		{
			get
			{
				if ( _adjustValueCommand == null )
				{
					_adjustValueCommand = new RelayCommand(
						param => this.AdjustValue( param ) );
				}

				return _adjustValueCommand;
			}
		}

		private void AdjustValue( object param )
		{
			int adjValue = Int32.Parse( param.ToString() );

			int newValue = SelectedValue.Value + adjValue;

			if ( newValue > SelectedValue.Maximum )
			{
				newValue -= SelectedValue.Maximum;
				newValue += SelectedValue.Minimum;
			}
			else if ( newValue < SelectedValue.Minimum )
			{
				newValue -= SelectedValue.Minimum;
				newValue += SelectedValue.Maximum;
			}

			SelectedValue.Value = newValue;
		}

		#endregion AdjustValueCommand
	}
}
