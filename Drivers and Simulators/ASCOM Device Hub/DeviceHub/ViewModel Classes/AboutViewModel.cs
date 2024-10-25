using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace ASCOM.DeviceHub
{
	class AboutViewModel : DeviceHubDialogViewModelBase
	{
		public AboutViewModel()
			: base( "About Device Hub")
		{
            string assemblyFileLocation=Assembly.GetExecutingAssembly().Location;
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assemblyFileLocation);
            ProductVersion = fileVersionInfo.ProductVersion;
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
