namespace ASCOM.DeviceHub
{
	public enum DeviceTypeEnum { Unknown, Telescope, Dome, Focuser };
	public enum ParkingStateEnum { Unparked, ParkInProgress, IsAtPark, ParkFailed };
	public enum HomingStateEnum { NotAtHome, HomingInProgress, IsAtHome, HomeFailed };
	public enum MoveDirections { None = -1, North, South, East, West, Up, Down, Left, Right, Clockwise, CounterClockwise };
	public enum ScopeMoveEnum { Variable, Fixed };
	public enum FocuserMoveCompletionState { Success, Timeout, Cancelled };
}
