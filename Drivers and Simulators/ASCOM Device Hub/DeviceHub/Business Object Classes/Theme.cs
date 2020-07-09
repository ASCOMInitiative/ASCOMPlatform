using System.Windows;
using System.Windows.Input;

using ASCOM.DeviceHub.Resources;

namespace ASCOM.DeviceHub
{
	public class Theme
	{
		public string Name { get; protected set; }
		public ResourceDictionary Resource { get; protected set; }
		public bool IsDefault { get; protected set; }
		public bool IsSelected { get; set; }
		public ICommand SelectCommand { get; protected set; }

		public string DisplayName
		{
			get
			{
				string stringName = "ThemeName" + Name;

				return Strings.ResourceManager.GetString( stringName, Strings.Culture );
			}
		}

		public Theme( string name, ResourceDictionary resource, ICommand selectCommand )
			: this( name, resource, selectCommand, false )
		{ }

		public Theme( string name, ResourceDictionary resource, ICommand selectCommand, bool isDefault )
		{
			Name = name;
			Resource = resource;
			SelectCommand = selectCommand;
			IsDefault = isDefault;
		}
	}
}
