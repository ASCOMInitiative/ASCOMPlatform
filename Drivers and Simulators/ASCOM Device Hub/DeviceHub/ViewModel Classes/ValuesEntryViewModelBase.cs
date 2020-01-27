using System.Windows.Input;

namespace ASCOM.DeviceHub
{
	internal class ValuesEntryViewModelBase : DeviceHubDialogViewModelBase
	{
		protected ValuesEntryViewModelBase( string sourceID )
			: base( sourceID )
		{
			UsesSignsText = false;
			NegativeText = "---";
			PositiveText = "+++";
			IsNegative = false; // since RotaryValues[0].Value is not negative
		}

		#region Change Notification Properties

		protected string _description;

		public string Description
		{
			get { return _description; }
			set
			{
				if ( value != _description )
				{
					_description = value;
					OnPropertyChanged();
				}
			}
		}

		protected RotarySliderViewModel _rotarySliderVm;

		public RotarySliderViewModel RotarySliderVm
		{
			get { return _rotarySliderVm; }
			set
			{
				if ( value != _rotarySliderVm )
				{
					_rotarySliderVm = value;
					OnPropertyChanged();
				}
			}
		}

		protected bool _usesSignsText;

		public bool UsesSignsText
		{
			get { return _usesSignsText; }
			set
			{
				if ( value != _usesSignsText )
				{
					_usesSignsText = value;
					OnPropertyChanged();
				}
			}
		}

		protected string _negativeText;

		public string NegativeText
		{
			get { return _negativeText; }
			set
			{
				if ( value != _negativeText )
				{
					_negativeText = value;
					OnPropertyChanged();
				}
			}
		}

		protected string _positiveText;

		public string PositiveText
		{
			get { return _positiveText; }
			set
			{
				if ( value != _positiveText )
				{
					_positiveText = value;
					OnPropertyChanged();
				}
			}
		}

		protected bool _isNegative;

		public bool IsNegative
		{
			get { return _isNegative; }
			set
			{
				if ( value != _isNegative )
				{
					_isNegative = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		public virtual void InitializeValues( int[] values )
		{}

		public virtual int[] GetValues()
		{
			return null;
		}

		#region Relay Commands

		#region NegativeCommand

		private ICommand _negativeCommand;

		public ICommand NegativeCommand
		{
			get
			{
				if ( _negativeCommand == null )
				{
					_negativeCommand = new RelayCommand(
						param => this.Negative() );
				}

				return _negativeCommand;
			}
		}

		private void Negative()
		{
			IsNegative = true;
		}

		#endregion NegativeCommand

		#region PositiveCommand

		private ICommand _positiveCommand;

		public ICommand PositiveCommand
		{
			get
			{
				if ( _positiveCommand == null )
				{
					_positiveCommand = new RelayCommand(
						param => this.Positive() );
				}

				return _positiveCommand;
			}
		}

		private void Positive()
		{
			IsNegative = false;
		}

		#endregion PositiveCommand

		#endregion Relay Commands
	}
}
