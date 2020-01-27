using System;

namespace ASCOM.DeviceHub
{
	public class AscomFocuserStatus : DevicePropertiesBase
	{
		#region Instance Constructor

		public AscomFocuserStatus()
		{}

		public AscomFocuserStatus( FocuserManager mgr)
		{
			Connected = mgr.Connected;
			IsMoving = mgr.IsMoving;
			Link = mgr.Link;
			bool IsAbsolute = mgr.Parameters.Absolute;
			Position = IsAbsolute ? mgr.Position : Int32.MinValue;
			TempComp = mgr.TempComp;
			Temperature = mgr.Temperature;
		}

		#endregion Instance Constructor

		#region Change Notification Properties

		private bool _connected;

		public bool Connected
		{
			get { return _connected; }
			set
			{
				if ( value != _connected )
				{
					_connected = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isMoving;

		public bool IsMoving
		{
			get 
			{
				return _isMoving;
			}
			set
			{
				if ( value != _isMoving )
				{
					_isMoving = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _link;

		public bool Link
		{
			get { return _link; }
			set
			{
				if ( value != _link )
				{
					_link = value;
					OnPropertyChanged();
				}
			}
		}

		private int _position;

		public int Position
		{
			get { return _position; }
			set
			{
				if ( value != _position )
				{
					_position = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _tempComp;

		public bool TempComp
		{
			get { return _tempComp; }
			set
			{
				if ( value != _tempComp )
				{
					_tempComp = value;
					OnPropertyChanged();
				}
			}
		}

		private double _temperature;

		public double Temperature
		{
			get { return _temperature; }
			set
			{
				if ( value != _temperature )
				{
					_temperature = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region Protected Methods

		protected virtual void Clean()
		{
			Connected = false;
			IsMoving = false;
			Link = false;
			Position = Int32.MinValue;
			TempComp = false;
			Temperature = Double.NaN;
		}

		#endregion Protected Methods
	}
}
