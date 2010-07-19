using System;
using System.Collections.Generic;
using System.Text;
using OptecHID_FilterWheelAPI;

namespace ASCOM.HSFW_ASCOM_Driver
{
    class HSFW_Communicator
    {
        private static HSFW_Communicator myHSFW;
        private Utilities.Profile myProfile;
        private enum ProfileStrings { PreferredSN }
        private FilterWheels fws;
        private string PreferredSN = "0";

        private HSFW_Communicator()
        {
       
            // Create instance of Profile
            myProfile = new ASCOM.Utilities.Profile(true);
            myProfile.DeviceType = "FilterWheel";					//  Requires Helper 5.0.3 or later
            // Check if there is a preferred Serial Number
            PreferredSN = myProfile.GetValue(FilterWheel.s_csDriverID, ProfileStrings.PreferredSN.ToString(), "", "0");
            // Get a list of all available Serial Numbers
            fws = new FilterWheels();

        }

        #region Public Methods

        public static HSFW_Communicator GetInstance()
        {
            if (myHSFW == null)
            {
                myHSFW = new HSFW_Communicator();
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



        
        #endregion
    }

}
