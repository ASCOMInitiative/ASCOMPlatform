using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optec;

namespace TestHarness
{
    class XMLSettings : XmlSettingsManager
    {
        public XMLSettings() : base("TestDevice")
        {
        }

        private enum XMLFileStrings
        {
            ComPortName
        }

        public string ComPortName
        {
            get
            {
                return GetPropertyFromXml(XMLFileStrings.ComPortName.ToString(), "COM1");
            }
            set
            {
                SetPropertyInXml(XMLFileStrings.ComPortName.ToString(), value);
            }
        }

        public void AddFocusOffset(string name, double offset)
        {
            SetGroupProperty("Focus Offsets", name, "Offset", offset.ToString());
        }

        public string PrintFocusOffsets()
        {
            string rtn = "";
            foreach (string x in GetPropertyGroupItemList("Focus Offsets"))
            {
                rtn += Environment.NewLine + "Item: " + x;
                foreach (string y in GetGroupItemPropertyNameList("Focus Offsets", x))
                {
                    rtn += " Property: " + y + " Value: " + GetGroupItemPropertyValue("Focus Offsets", x, y);
                }

            }

            return rtn;
        }

        

    }
}
