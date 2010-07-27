using System;
using System.Collections.Generic;
using System.Text;
using OptecHID_FilterWheelAPI;
using System.Xml;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Collections;
using System.Reflection;


namespace ASCOM.HSFW_ASCOM_Driver
{
    class HSFW_Handler
    {
        private static HSFW_Handler myHSFW;
        private static Utilities.Profile myProfile;
        private static OptecHID_FilterWheelAPI.FilterWheel mydevice;
        private static FilterWheels fws;
        private enum ProfileStrings { PreferredSN }
        private static XDocument FocusOffsetDocument;

        private static string PreferredSN = "0";
        private const string XmlFilename = "FocusOffsets.xml";
        
        public event EventHandler DeviceListChanged;

        private HSFW_Handler()
        {
            
            // Create instance of Profile
            myProfile = new ASCOM.Utilities.Profile();
            myProfile.DeviceType = "FilterWheel";					//  Requires Helper 5.0.3 or later
            // Check if there is a preferred Serial Number
            PreferredSN = myProfile.GetValue(FilterWheel.s_csDriverID, ProfileStrings.PreferredSN.ToString(), "", "0");
            // Get a list of all available Serial Numbers
            fws = new FilterWheels();
            WaitForHomeToComplete();
            fws.FilterWheelAttached += new EventHandler(fws_DeviceListChanged);
            fws.FilterWheelRemoved += new EventHandler(fws_DeviceListChanged);

            // If one matches assign it
            AssignPreferredDevice();

            // Load the focus offsets from the xml file and place them in the XDocument
            loadFocusOffsets();

        }

        private void WaitForHomeToComplete()
        {
            foreach (OptecHID_FilterWheelAPI.FilterWheel f in fws.FilterWheelList)
            {
                while (f.IsHoming)
                {
                    // just wait for the home to finish
                    System.Threading.Thread.Sleep(250);
                }
            }
        }

        private void AssignPreferredDevice()
        {
            if (fws.AttachedDeviceCount == 0) return;
            else
            {
                foreach (OptecHID_FilterWheelAPI.FilterWheel f in fws.FilterWheelList)
                {
                    if (f.IsAttached && (f.SerialNumber == PreferredSerialNumber))
                    {
                        mydevice = f;
                        return;
                    }
                }
            }
        }

        void fws_DeviceListChanged(object sender, EventArgs e)
        {
            WaitForHomeToComplete();
            AssignPreferredDevice();          
            this.DeviceListChanged(this, EventArgs.Empty);
        }

        public static void DeleteInstance()
        {
            myHSFW = null;
            fws = null;
            myProfile = null;
            FocusOffsetDocument = null;
            mydevice = null;
        }
        
        
        #region Public Methods

        public static HSFW_Handler GetInstance()
        {
            if (myHSFW == null)
            {
                myHSFW = new HSFW_Handler();
                return myHSFW;
            }
            else return myHSFW;
        }

        public bool CheckIfDeviceAttached(string SerialNumber)
        {
            foreach (OptecHID_FilterWheelAPI.FilterWheel fw in fws.FilterWheelList)
            {
                if ((fw.SerialNumber == SerialNumber) && (fw.IsAttached)) return true;
            }
            return false;
        }

        public static void SetFocusOffset(string SerialNumber, char WheelID, short FilterNum, int Offset)
        {

            var xe = from f in FocusOffsetDocument.Descendants("OffsetItem")
                     select new
                     {
                         SerialNumber = f.Element("SerialNumber").Value,
                         Filter = f.Element("Filter").Value,
                         Wheel = f.Element("Wheel").Value,
                         Offset = f.Element("Offset").Value
                     };

            var x = xe.SingleOrDefault(p => (p.SerialNumber == SerialNumber) &&
                (char.Parse(p.Wheel) == WheelID) &&
                (short.Parse(p.Filter) == FilterNum));
          
            if (x == null)
            {
                // Add a new element
                XElement newdatapoint = new XElement("OffsetItem",
                    new XElement("SerialNumber", SerialNumber),
                    new XElement("Filter", FilterNum.ToString()),
                    new XElement("Wheel", WheelID.ToString()),
                    new XElement("Offset", Offset.ToString()));
                FocusOffsetDocument.Root.Add(newdatapoint);
                FocusOffsetDocument.Save("FocusOffsets.xml");
            }
            else
            {
                var y = from s in FocusOffsetDocument.Root.Descendants("OffsetItem")

                        select s;

                foreach (var oi in y)
                {
                    if (oi.Element("SerialNumber").Value == SerialNumber &&
                        int.Parse(oi.Element("Filter").Value) == FilterNum &&
                        oi.Element("Wheel").Value == WheelID.ToString())
                    {
                        oi.Element("Offset").Value = Offset.ToString();
                    }

                }
            }

            //FocusOffsets.FocusOffsetsTableRow  PrevValue = FocusOffsetData.FocusOffsetsTable.SingleOrDefault(
            //    p => p.SerialNumber == SerialNumber && 
            //    p.Wheel == WheelID && 
            //    p.Filter == FilterNum);
            //if (PrevValue == null)
            //{
            //    FocusOffsetData.FocusOffsetsTable.AddFocusOffsetsTableRow(SerialNumber,
            //        FilterNum, WheelID, Offset);
            //}
            //else PrevValue.Offset = Offset;

            //FocusOffsetData.WriteXml(XmlFilename, XmlWriteMode.IgnoreSchema);
        }

        public static int GetFocusOffset(string SerialNumber, char WheelID, short FilterNum)
        {
            var xe = from f in FocusOffsetDocument.Descendants("OffsetItem")
                     select new { 
                         SerialNumber = f.Element("SerialNumber").Value, 
                         Filter = f.Element("Filter").Value,
                         Wheel = f.Element("Wheel").Value,
                         Offset = f.Element("Offset").Value };

            var x = xe.SingleOrDefault(p => (p.SerialNumber == SerialNumber) && 
                (char.Parse(p.Wheel) == WheelID) && 
                (short.Parse(p.Filter) == FilterNum));


            if (x == null) return 0;
            else return int.Parse(x.Offset);
        }

        private static void loadFocusOffsets()
        {
            try
            {
                
                // Read the xml data into the DataSet
                string asmpath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string xpath = System.IO.Path.Combine(asmpath, XmlFilename);
                if (!System.IO.File.Exists(xpath))
                {
                    throw new ApplicationException(XmlFilename + " file was not found. Did you move/delete it?");
                }
                FocusOffsetDocument = XDocument.Load(xpath);



             

                //y.Value = "32";

                //var x = from i in currentdata.Descendants("OffsetItem")
                //        //select new { SerialNumber = i.Attribute("SerialNumber"), Value = i.Attribute("Offset") };
                //        select new { SerialNumber = i.Element("SerialNumber").Value, Value = i.Element("Offset").Value };
                
              
                //var y = x.Single(p => p.SerialNumber == "12345");

                //// try to create a new element

                //XDocument myDoc = XDocument.Load("FocusOffsets.xml");
                //var j = from i in myDoc.Descendants("OffsetItem")
                //         select new { SerialNumber = i.Element("SerialNumber").Value, Value = i.Element("Offset").Value };
             
                //XElement newdatapoint = new XElement("OffsetItem",
                //    new XElement("Offset",
                //        new XElement("SerialNumber", 2468),
                //        new XElement("Offset", 77)));
                //myDoc.Root.Add(newdatapoint);
                //myDoc.Save("FocusOffsets.xml");
                
            }
                
            catch
            {
                throw;
            }
        }

        #endregion


        #region Public Properties

        public int AttachedDeviceCount
        {
            get { return fws.AttachedDeviceCount; }
        }

        public List<string> AttachedDeviceList
        {
            get
            {
                List<string> SNList = new List<string>();
                foreach (OptecHID_FilterWheelAPI.FilterWheel fw in fws.FilterWheelList)
                {
                    if (fw.IsAttached) SNList.Add(fw.SerialNumber);
                }
                return SNList;
            }
        }

        public string PreferredSerialNumber
        {
            get { return PreferredSN; }
        }

        public OptecHID_FilterWheelAPI.FilterWheel myDevice
        {
            get 
            {
                return mydevice;
            }
        }
       

        #endregion

        internal void ChangeMyDevice(string newSN)
        {
            foreach (OptecHID_FilterWheelAPI.FilterWheel f in fws.FilterWheelList)
            {
                if (f.SerialNumber == newSN)
                {
                    mydevice = f;
                    break;
                }
            }
            // If we get here a matching device was not found.

        }
    }


}
