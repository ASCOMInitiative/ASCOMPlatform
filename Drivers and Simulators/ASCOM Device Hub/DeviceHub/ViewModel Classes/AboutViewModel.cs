using System.Reflection;

namespace ASCOM.DeviceHub
{
	class AboutViewModel : DeviceHubDialogViewModelBase
	{
		public AboutViewModel()
			: base( "About Device Hub")
		{
			string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

			ProductVersion = version;
		}

		private string _productVersion;

		public string ProductVersion
		{
			get { return _productVersion; }
			set
			{
				if ( value != _productVersion )
				{
					_productVersion = value;
					OnPropertyChanged();
				}
			}
		}
	}
}
