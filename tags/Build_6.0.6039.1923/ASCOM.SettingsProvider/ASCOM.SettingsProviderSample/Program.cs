using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.SettingsProviderSample
	{
	class Program
		{
		static void Main(string[] args)
			{
			Console.WriteLine("Setting1 value = {0}", Properties.Settings.Default.Setting1);
			Console.ReadLine();
			}
		}
	}
