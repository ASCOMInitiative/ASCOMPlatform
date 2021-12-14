using System;

namespace ASCOM.DeviceHub
{
	public enum DeviceTypeEnum 
    { 
        [Display(Name = "Unknown")]
        Unknown,
        [Display(Name = "Telescope")]
        Telescope,
        [Display(Name = "Dome")]
        Dome,
        [Display(Name = "Focuser")]
        Focuser
    };
	public enum ParkingStateEnum { Unparked, ParkInProgress, IsAtPark };
	public enum HomingStateEnum { NotAtHome, HomingInProgress, IsAtHome, HomeFailed };
	public enum MoveDirections { None = -1, North, South, East, West, Up, Down, Left, Right, Clockwise, CounterClockwise };
	public enum ScopeMoveEnum { Variable, Fixed };
	public enum FocuserMoveCompletionState { Success, Timeout, Cancelled };

	public static class EnumExtensions
	{
        public static string GetDisplayName( this Enum GenericEnum )
        {

            Type genericEnumType = GenericEnum.GetType();
            System.Reflection.MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());

            if ( ( memberInfo != null && memberInfo.Length > 0 ) )
            {
                dynamic _Attribs = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);

                if ( ( _Attribs != null && _Attribs.Length > 0 ) )
                {
                    return ( (DisplayAttribute)_Attribs[0] ).Name;
                }
            }

            return GenericEnum.ToString();
        }
    }
}
