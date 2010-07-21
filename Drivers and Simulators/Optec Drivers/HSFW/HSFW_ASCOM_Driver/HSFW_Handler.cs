using System;
using System.Collections.Generic;
using System.Text;
using OptecHID_FilterWheelAPI;
using System.Xml;
using System.Data;
using System.Linq;

namespace ASCOM.HSFW_ASCOM_Driver
{
    class HSFW_Handler
    {
        private static HSFW_Handler myHSFW;
        private Utilities.Profile myProfile;
        private OptecHID_FilterWheelAPI.FilterWheel mydevice;
        private enum ProfileStrings { PreferredSN }
        private FilterWheels fws;
        private string PreferredSN = "0";
        private const string XmlFilename = "DriverSettings.xml";
        private static FocusOffsets FocusOffsetData;

        private HSFW_Handler()
        {
            
       
            // Create instance of Profile
            myProfile = new ASCOM.Utilities.Profile();
            myProfile.DeviceType = "FilterWheel";					//  Requires Helper 5.0.3 or later
            // Check if there is a preferred Serial Number
            PreferredSN = myProfile.GetValue(FilterWheel.s_csDriverID, ProfileStrings.PreferredSN.ToString(), "", "0");
            // Get a list of all available Serial Numbers
            fws = new FilterWheels();

            // If one matches assign it
            foreach( OptecHID_FilterWheelAPI.FilterWheel fw in fws.FilterWheelList)
            {
                if (fw.SerialNumber == PreferredSerialNumber)
                {
                    mydevice = fw;
                    break;
                }
            }

            // Read the focus offsets from the xml file and place them in the dataset
            loadFocusOffsets();

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
            FocusOffsets.FocusOffsetsTableRow  PrevValue = FocusOffsetData.FocusOffsetsTable.SingleOrDefault(
                p => p.SerialNumber == SerialNumber && 
                p.Wheel == WheelID && 
                p.Filter == FilterNum);
            if (PrevValue == null)
            {
                FocusOffsetData.FocusOffsetsTable.AddFocusOffsetsTableRow(SerialNumber,
                    FilterNum, WheelID, Offset);
            }
            else PrevValue.Offset = Offset;

            FocusOffsetData.WriteXml(XmlFilename, XmlWriteMode.IgnoreSchema);
        }

        public static int GetFocusOffset(string SerialNumber, char WheelID, short FilterNum)
        {
            FocusOffsets.FocusOffsetsTableRow x = (from offset in FocusOffsetData.FocusOffsetsTable
                                                   where offset.SerialNumber == SerialNumber
                                                   where offset.Wheel == WheelID
                                                   where offset.Filter == FilterNum
                                                   select offset).SingleOrDefault();
            if (x == null) return 0;
            else return x.Offset;
        }

        private static void loadFocusOffsets()
        {
            try
            {
                FocusOffsetData = new FocusOffsets();
                // Read the xml data into the DataSet
                if (!System.IO.File.Exists(XmlFilename))
                {
                    throw new ApplicationException("DeviceSettings.xml file was not found. Did you move/delete it?");
                }
                FocusOffsetData.ReadXml(XmlFilename, XmlReadMode.IgnoreSchema);
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
    }


}
