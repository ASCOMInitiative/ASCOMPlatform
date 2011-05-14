using System.Runtime.InteropServices;

namespace ASCOM.Interface
{
	[Guid("8915DF3D-B055-4195-8D23-AAD7F58FDF3B")]
	public enum ShutterState
	{
		shutterOpen,
		shutterClosed,
		shutterOpening,
		shutterClosing,
		shutterError
	}
}