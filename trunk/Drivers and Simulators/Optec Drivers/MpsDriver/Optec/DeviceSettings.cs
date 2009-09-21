using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ASCOM.Helper;
using System.Windows.Forms;

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
            string Name;
            switch (PortNumber)
            {
                case 1:
                    Name = Properties.Defaults.Default.Port1Name;
                    break;
                case 2:
                    Name = Properties.Defaults.Default.Port2Name;
                    break;
                case 3:
                    Name = Properties.Defaults.Default.Port3Name;
                    break;
                case 4:
                    Name = Properties.Defaults.Default.Port4Name;
                    break;
                default:
                    MessageBox.Show("Invalid Port Number Request to Name \"Property\"");
                    Name = "";
                    break;
            }
            return Name;
        }

        public static double RightAscensionOffset(int PortNumber)
        {
            double RA_Offset;
            switch (PortNumber)
            {
                case 1:
                    RA_Offset = Properties.Defaults.Default.P1RAOffset;
                    break;
                case 2:
                    RA_Offset = Properties.Defaults.Default.P2RAOffset;
                    break;
                case 3:
                    RA_Offset = Properties.Defaults.Default.P3RAOffset;
                    break;
                case 4:
                    RA_Offset = Properties.Defaults.Default.P4RAOffset;
                    break;
                default:
                    MessageBox.Show("Invalid Port Number Request to RA_Offset \"Property\"");
                    RA_Offset = 0.0;
                    break;
            }
            return RA_Offset; 
        }

        public static double DeclinationOffset(int PortNumber)
        {
            double Dec_Offset;
            switch (PortNumber)
            {
                case 1:
                    Dec_Offset = Properties.Defaults.Default.P1DecOffset;
                    break;
                case 2:
                    Dec_Offset = Properties.Defaults.Default.P2DecOffset;
                    break;
                case 3:
                    Dec_Offset = Properties.Defaults.Default.P3DecOffset;
                    break;
                case 4:
                    Dec_Offset = Properties.Defaults.Default.P4DecOffset;
                    break;
                default:
                    MessageBox.Show("Invalid Port Number Request to Dec_Offset \"Property\"");
                    Dec_Offset = 0.0;
                    break;
            }
            return Dec_Offset; 
        }

        public static short FocusOffset(int PortNumber)
        {
            short Focus_Offset;
            switch (PortNumber)
            {
                case 1:
                    Focus_Offset = Properties.Defaults.Default.P1FocusOffset;
                    break;
                case 2:
                    Focus_Offset = Properties.Defaults.Default.P2FocusOffset;
                    break;
                case 3:
                    Focus_Offset = Properties.Defaults.Default.P3FocusOffset;
                    break;
                case 4:
                    Focus_Offset = Properties.Defaults.Default.P4FocusOffset;
                    break;
                default:
                    MessageBox.Show("Invalid Port Number Request to Focus_Offset \"Property\"");
                    Focus_Offset = 0;
                    break;
            }
            return Focus_Offset; 
        }

        public static double RotationOffset(int PortNumber)
        {
            double Rot_Offset;
            switch (PortNumber)
            {
                case 1:
                    Rot_Offset = Properties.Defaults.Default.P1RotationOffset;
                    break;
                case 2:
                    Rot_Offset = Properties.Defaults.Default.P2RotationOffset;
                    break;
                case 3:
                    Rot_Offset = Properties.Defaults.Default.P3RotationOffset;
                    break;
                case 4:
                    Rot_Offset = Properties.Defaults.Default.P4RotationOffset;
                    break;
                default:
                    MessageBox.Show("Invalid Port Number Request to Rotation_Offset \"Property\"");
                    Rot_Offset = 0.0;
                    break;
            }
            return Rot_Offset;
        }

        //
        // [RBD] You'll need these to save the settings from your SetupDialog
        //

        public static void SetName(int PortNumber, string Name)
        {
            switch (PortNumber)
            {
                case 1:
                    Properties.Defaults.Default.Port1Name = Name;
                    break;
                case 2:
                    Properties.Defaults.Default.Port2Name = Name;
                    break;
                case 3:
                    Properties.Defaults.Default.Port3Name = Name;
                    break;
                case 4:
                    Properties.Defaults.Default.Port4Name = Name;
                    break;
                default:
                    MessageBox.Show("Invalid Port Number Request to SetName \"Property\"");
                    break;
            }
            Properties.Defaults.Default.Save();
        }

        public static void SetRightAscensionOffset(int PortNumber, double RightAscensionOffset)
        {
            switch (PortNumber)
            {
                case 1:
                    Properties.Defaults.Default.P1RAOffset = RightAscensionOffset;
                    break;
                case 2:
                    Properties.Defaults.Default.P2RAOffset = RightAscensionOffset;
                    break;
                case 3:
                    Properties.Defaults.Default.P3RAOffset = RightAscensionOffset;
                    break;
                case 4:
                    Properties.Defaults.Default.P4RAOffset = RightAscensionOffset;
                    break;
                default:
                    MessageBox.Show("Invalid Port Number Request to SetRAOffset \"Property\"");
                    break;
            }
            Properties.Defaults.Default.Save();
        }

        public static void SetDeclinationOffset(int PortNumber, double DeclinationOffset)
        {
            switch (PortNumber)
            {
                case 1:
                    Properties.Defaults.Default.P1DecOffset = DeclinationOffset;
                    break;
                case 2:
                    Properties.Defaults.Default.P2DecOffset = DeclinationOffset;
                    break;
                case 3:
                    Properties.Defaults.Default.P3DecOffset = DeclinationOffset;
                    break;
                case 4:
                    Properties.Defaults.Default.P4DecOffset = DeclinationOffset;
                    break;
                default:
                    MessageBox.Show("Invalid Port Number Request to SetDecOffset \"Property\"");
                    break;
            }
            Properties.Defaults.Default.Save();
        }

        public static void SetFocusOffset(int PortNumber, short FocusOffset)
        {
            switch (PortNumber)
            {
                case 1:
                    Properties.Defaults.Default.P1FocusOffset = FocusOffset;
                    break;
                case 2:
                    Properties.Defaults.Default.P2FocusOffset = FocusOffset;
                    break;
                case 3:
                    Properties.Defaults.Default.P3FocusOffset = FocusOffset;
                    break;
                case 4:
                    Properties.Defaults.Default.P4FocusOffset = FocusOffset;
                    break;
                default:
                    MessageBox.Show("Invalid Port Number Request to SetFocusOffset \"Property\"");
                    break;
            }
            Properties.Defaults.Default.Save();
        }

        public static void SetRotationOffset(int PortNumber, double RotationOffset)
        {
            switch (PortNumber)
            {
                case 1:
                    Properties.Defaults.Default.P1RotationOffset = RotationOffset;
                    break;
                case 2:
                    Properties.Defaults.Default.P2RotationOffset = RotationOffset;
                    break;
                case 3:
                    Properties.Defaults.Default.P3RotationOffset = RotationOffset;
                    break;
                case 4:
                    Properties.Defaults.Default.P4RotationOffset = RotationOffset;
                    break;
                default:
                    MessageBox.Show("Invalid Port Number Request to SetRotationOffset \"Property\"");
                    break;
            }
            Properties.Defaults.Default.Save();
        }

    }
}
