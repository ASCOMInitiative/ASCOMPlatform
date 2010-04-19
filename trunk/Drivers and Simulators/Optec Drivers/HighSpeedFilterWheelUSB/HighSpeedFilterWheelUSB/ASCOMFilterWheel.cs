using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OptecHID_FilterWheelAPI;
using ASCOM.Utilities;

namespace ASCOM.HighSpeedFilterWheelUSB
{
    class ASCOMFilterWheel
    {
        internal enum ProfileStrings
        {
            SelectedSerialNumber,
            Test
        }
        private static Utilities.Profile myProfile = new Utilities.Profile();
        internal static OptecHID_FilterWheelAPI.FilterWheel myFilterWheel = null;
        internal static OptecHID_FilterWheelAPI.FilterWheels FilterWheelManager = new FilterWheels();
        private static bool connected = false;

        static ASCOMFilterWheel()
        {
            myProfile.DeviceType = "FilterWheel";
            AssignMyFilterWheel();
        }

        public static bool Connected
        {
            get
            {
                if (myFilterWheel == null)
                {
                    AssignMyFilterWheel();
                    if (myFilterWheel == null)
                    {
                        throw new ASCOM.DriverException("A device has not been selected yet");
                    }
                }
                return (myFilterWheel.IsAttached && connected);
            }
            set
            {
                if (myFilterWheel == null)
                {
                    AssignMyFilterWheel();
                    if (myFilterWheel == null)
                    {
                        throw new ASCOM.DriverException("A device has not been selected yet");
                    }
                }
                if (myFilterWheel.IsAttached) connected = value;
                else throw new ASCOM.NotConnectedException("USB Device Is Not Plugged In");
            }
        }

        private static void AssignMyFilterWheel()
        {
            try
            {
                string SN = myProfile.GetValue(FilterWheel.s_csDriverID, ProfileStrings.SelectedSerialNumber.ToString(), "", "None Selected");
                foreach (OptecHID_FilterWheelAPI.FilterWheel fw in FilterWheelManager.FilterWheelList)
                {
                    if (fw.SerialNumber == SN)
                    {
                        myFilterWheel = fw;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}