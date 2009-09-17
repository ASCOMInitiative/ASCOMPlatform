using System;
using System.Text;
using System.EnterpriseServices.Internal;

// See http://www.belshe.com/2004/03/29/gacutil-gac-install/

namespace GACInstaller
{
	class Program
	{
		static void Main(string[] args)
		{
			string path = args[0].Replace("\"", "");
			Publish P = new Publish();
			P.GacInstall(path);
		}
	}
}
