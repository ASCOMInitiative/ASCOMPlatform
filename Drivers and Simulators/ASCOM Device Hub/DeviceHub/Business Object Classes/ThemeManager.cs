using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ASCOM.DeviceHub
{
	public class ThemeManager
	{
		static ThemeManager()
		{
			ThemeList = null;
		}

		public static void InitializeThemes( ICommand selectCommand )
		{
			ThemeList = new List<Theme>();

			ResourceDictionary res = new ResourceDictionary();
			res.MergedDictionaries.Add( new ResourceDictionary { Source = new Uri( "Themes/generic.xaml", UriKind.Relative ) } );
			ThemeList.Add( new Theme( "Standard", res, selectCommand, true ) );

			res = new ResourceDictionary();
			res.MergedDictionaries.Add( new ResourceDictionary { Source = new Uri( "Themes/DevHubMono.xaml", UriKind.Relative ) } );
			ThemeList.Add( new Theme( "Custom", res, selectCommand ) );
		}


		public static List<Theme> ThemeList { get; private set; }

		public static string[] ThemeNames
		{
			get
			{
				return ThemeList.Select( t => t.Name ).ToArray();
			}
		}
	}
}
