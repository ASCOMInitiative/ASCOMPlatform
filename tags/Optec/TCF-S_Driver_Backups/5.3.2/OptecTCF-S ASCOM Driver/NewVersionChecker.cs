using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace Optec
{

    public static class NewVersionChecker
    {
        private static string _url = "";
        private static Version _LatestVersionNumber = new Version();

        private static bool _NewerVersionAvailable = false;


        public enum ProductType { TCF_S }

        public static bool NewerVersionAvailable
        {
            get { return _NewerVersionAvailable; }
        }

        public static string NewerVersionURL
        {
            get { return _url; }
        }
        public static string NewerVersionNumber
        {
            get { return _LatestVersionNumber.ToString(); }
        }

        public static bool CheckLatestVerisonNumber(ProductType Product)
        {
            try
            {
                string xmlURL = "http://www.optecinc.com/astronomy/software/download/ascom/CurrentAppVersion.xml";
                XDocument xdoc = XDocument.Load(xmlURL);
                XElement v = xdoc.Descendants("versionGetter").Descendants("TCF_S").Descendants().Single(i => i.Name == "version");
                XElement u = xdoc.Descendants("versionGetter").Descendants("TCF_S").Descendants().Single(i => i.Name == "url");
                _LatestVersionNumber = new Version(v.Value);
                _url = v.Value;
                return true;
            }
            catch
            {
                return false;
            }

        }



        internal static void CompareToLatestVersion(Version CurrentVersionNumber)
        {
            if (CurrentVersionNumber.CompareTo(_LatestVersionNumber) < 0)
            {
                _NewerVersionAvailable = true;
            }
        }
    }
}
