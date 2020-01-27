using System;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceHub
{
	public static class User32
    {
        [DllImport( "user32.dll" )]
        internal extern static int SetWindowLong( IntPtr hwnd, int index, int value );

        [DllImport( "user32.dll" )]
        internal extern static int GetWindowLong( IntPtr hwnd, int index );

        [DllImport( "user32.dll" )]
        internal static extern bool SetWindowPlacement( IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl );
    }

    // RECT structure required by WINDOWPLACEMENT structure
    [Serializable]
    [StructLayout( LayoutKind.Sequential )]
    public struct Win32Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public Win32Rect( int left, int top, int right, int bottom )
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        public Win32Point UpperLeft { get { return new Win32Point( Left, Top ); } }
        public int Width { get { return Right - Left; } }
        public int Height { get { return Bottom - Top; } }

    }

    // POINT structure required by WINDOWPLACEMENT structure
    [Serializable]
    [StructLayout( LayoutKind.Sequential )]
    public struct Win32Point
    {
        public int X;
        public int Y;

        public Win32Point( int x, int y )
        {
            this.X = x;
            this.Y = y;
        }
    }

    // WINDOWPLACEMENT stores the position, size, and state of a window
    [Serializable]
    [StructLayout( LayoutKind.Sequential )]
    public struct WINDOWPLACEMENT
    {
        public int length;
        public int flags;
        public int showCmd;
        public Win32Point minPosition;
        public Win32Point maxPosition;
        public Win32Rect normalPosition;
    }
}
