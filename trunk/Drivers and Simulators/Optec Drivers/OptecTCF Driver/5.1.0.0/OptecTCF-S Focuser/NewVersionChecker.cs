using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ASCOM.OptecTCF_S
{
    
    public static class NewVersionChecker
    {
        private static string _url = "";
        private static Version _LatestVersionNumber = new Version();

        private static bool _NewerVersionAvailable = false;
        

        public enum ProductType {TCF_S}

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
            XmlTextReader reader = null;
            
            try
            {
                // provide the XmlTextReader with the URL of  
                // our xml document  
                string xmlURL = "http://www.optecinc.com/astronomy/software/download/ascom/CurrentAppVersion.xml";
                bool FoundProduct = false;
                reader = new XmlTextReader(xmlURL);
                // simply (and easily) skip the junk at the beginning  
                reader.MoveToContent();
                // internal - as the XmlTextReader moves only  
                // forward, we save current xml element name  
                // in elementName variable. When we parse a  
                // text node, we refer to elementName to check  
                // what was the node name  
                string elementName = "";
                // we check if the xml starts with a proper  
                // "ourfancyapp" element node  
                if ((reader.NodeType == XmlNodeType.Element) &&
                    (reader.Name == "versionGetter"))
                {
                    // Search for the right product

                    while(reader.Read())
                    {
                        if ((reader.NodeType == XmlNodeType.Element) &&
                            (reader.Name == Product.ToString()))
                        {
                            FoundProduct = true;
                            break;
                        }
                    }
                    if (FoundProduct)
                    {
                        while (reader.Read())
                        {
                            // when we find an element node,  
                            // we remember its name  
                            if (reader.NodeType == XmlNodeType.Element)
                                elementName = reader.Name;
                            else
                            {
                                // for text nodes...  
                                if ((reader.NodeType == XmlNodeType.Text) &&
                                    (reader.HasValue))
                                {
                                    // we check what the name of the node was  
                                    switch (elementName)
                                    {
                                        case "version":
                                            // thats why we keep the version info  
                                            // in xxx.xxx.xxx.xxx format  
                                            // the Version class does the  
                                            // parsing for us  
                                            _LatestVersionNumber = new Version(reader.Value);
                                            break;
                                        case "url":
                                            _url = reader.Value;
                                            break;
                                    }
                                }
                            }
                        }

                        if ((_LatestVersionNumber != null) && (_url != ""))
                        {
                            return true;
                        }
                        else return false;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return false;
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
