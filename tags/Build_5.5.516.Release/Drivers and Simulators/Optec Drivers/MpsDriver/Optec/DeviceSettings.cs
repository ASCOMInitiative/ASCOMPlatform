using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ASCOM.Helper;

namespace ASCOM.Optec
{
    [ComVisible(false)]

    class DeviceSettings    // This class contains methods which are used to access 
                            // device specific settings stored by the user. It's used
                            // by the driver constructor as well as SetupDialog().
    {
        private static ASCOM.Helper.Profile ProfileTools = new ASCOM.Helper.Profile();

        static DeviceSettings() {} 

        //
        // [RBD] Grab the stuff from Profile here and convert it to its final type.
        //       Probably should return a default value if nothing exists in the
        //       profile for the given port number.
        //
        public static string Name(int PortNumber)
        {
            string StrName = "Port" + PortNumber.ToString() + "Name";
            string Name;
            //try
            //{
            //    ProfileTools.WriteValue(MultiPortSelector.s_csDriverID, StrName, "Test", "");
            //    Name = ProfileTools.GetValue(MultiPortSelector.s_csDriverID, StrName, "");
            //}
            //catch
            //{
            //    Name = "Port " + PortNumber.ToString();
            //    ProfileTools.WriteValue(MultiPortSelector.s_csDriverID, StrName, Name, "");
            //}
            return "Name" + PortNumber.ToString();
        }

        public static double RightAscensionOffset(int PortNumber)
        {
            return 0.0; 
        }

        public static double DeclinationOffset(int PortNumber)
        {
            return 0.0;
        }

        public static short FocusOffset(int PortNumber)
        {
            return 0;
        }

        public static double RotationOffset(int PortNumber)
        {
            return 0.0;
        }

        //
        // [RBD] You'll need these to save the settings from your SetupDialog
        //

        public static void SetName(int PortNumber, string Name)
        {
            throw new ASCOM.NotImplementedException("Method SetName");
        }

        public static void SetRightAscensionOffset(int PortNumber, double RightAscensionOffset)
        {
            throw new ASCOM.NotImplementedException("Method SetRightAscensionOffset");
        }

        public static void SetDeclinationOffset(int PortNumber, double DeclinationOffset)
        {
            throw new ASCOM.NotImplementedException("Method SetDeclinationOffset");
        }

        public static void SetFocusOffset(int PortNumber, short FocusOffset)
        {
            throw new ASCOM.NotImplementedException("Method SetFocusOffset");
        }

        public static void SetRotationOffset(int PortNumber, double RotationOffset)
        {
            throw new ASCOM.NotImplementedException("Method SetRotationOffset");
        }

    }
}
