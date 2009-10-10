using System;
using System.Runtime.InteropServices;
using System.Text;

using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Security.Permissions;

namespace Microsoft.Samples.WinForms.Extras
{
	internal class Win32API
	{
		// C# representation of the IMalloc interface
		[InterfaceType ( ComInterfaceType.InterfaceIsIUnknown ),
			Guid ( "00000002-0000-0000-C000-000000000046" )]
			public interface IMalloc
		{
			[PreserveSig] IntPtr Alloc ( [In] int cb );
			[PreserveSig] IntPtr Realloc ( [In] IntPtr pv, [In] int cb );
			[PreserveSig] void   Free ( [In] IntPtr pv );
			[PreserveSig] int    GetSize ( [In] IntPtr pv );
			[PreserveSig] int    DidAlloc ( IntPtr pv );
			[PreserveSig] void   HeapMinimize ( );
		}

		[DllImport("User32.DLL")]
		public static extern IntPtr GetActiveWindow ( );

		public class Shell32
		{
			// Styles used in the BROWSEINFO.ulFlags field
			[Flags]    
				public enum BffStyles 
			{
				RestrictToFilesystem = 0x0001, // BIF_RETURNONLYFSDIRS
				RestrictToDomain =     0x0002, // BIF_DONTGOBELOWDOMAIN
				RestrictToSubfolders = 0x0008, // BIF_RETURNFSANCESTORS
				ShowTextBox =          0x0010, // BIF_EDITBOX
				ValidateSelection =    0x0020, // BIF_VALIDATE
				NewDialogStyle =       0x0040, // BIF_NEWDIALOGSTYLE
				BrowseForComputer =    0x1000, // BIF_BROWSEFORCOMPUTER
				BrowseForPrinter =     0x2000, // BIF_BROWSEFORPRINTER
				BrowseForEverything =  0x4000, // BIF_BROWSEINCLUDEFILES
			}

			// Delegate type used in BROWSEINFO.lpfn field
			public delegate int BFFCALLBACK ( IntPtr hwnd, uint uMsg, IntPtr lParam, IntPtr lpData );

			[StructLayout ( LayoutKind.Sequential, Pack=8 )]
				public struct BROWSEINFO
			{
				public IntPtr       hwndOwner;
				public IntPtr       pidlRoot;
				public IntPtr       pszDisplayName;
				[MarshalAs ( UnmanagedType.LPTStr )]
				public string       lpszTitle;
				public int          ulFlags;
				[MarshalAs ( UnmanagedType.FunctionPtr )]
				public BFFCALLBACK  lpfn;
				public IntPtr       lParam;
				public int          iImage;
			}

			[DllImport ( "Shell32.DLL" )]
			public static extern int SHGetMalloc ( out IMalloc ppMalloc );

			[DllImport ( "Shell32.DLL" )]
			public static extern int SHGetSpecialFolderLocation ( 
				IntPtr hwndOwner, int nFolder, out IntPtr ppidl );

			[DllImport ( "Shell32.DLL" )]
			public static extern int SHGetPathFromIDList ( 
				IntPtr pidl, StringBuilder Path );

			[DllImport ( "Shell32.DLL", CharSet=CharSet.Auto )]
			public static extern IntPtr SHBrowseForFolder ( ref BROWSEINFO bi );
		}
	}
	/// <summary>
	/// Component wrapping access to the Browse For Folder common dialog.
	/// Call the ShowDialog() method to bring the dialog up.
	/// </summary>
	[ToolboxBitmap(typeof(FolderBrowser))]
	public sealed class FolderBrowser : Component
	{
		private static readonly int MAX_PATH = 260;
    
		// Root node of the tree view.
		private FolderID startLocation = FolderID.Desktop;

		// Browse info options
		private int publicOptions = (int) Win32API.Shell32.BffStyles.RestrictToFilesystem |
			(int) Win32API.Shell32.BffStyles.RestrictToDomain;
		private int privateOptions = (int) Win32API.Shell32.BffStyles.NewDialogStyle;

		// Description text to show.
		private string descriptionText = "Please select a folder below:";

		// Folder picked by the user.
		private string directoryPath = String.Empty;

		/// <summary>
		/// Helper function used to set / reset bits in the publicOptions bitfield
		/// </summary>
		private void SetOptionField ( int mask, bool turnOn )
		{
			if (turnOn)
				publicOptions |= mask;
			else
				publicOptions &= ~mask;
		}

		/// <summary>
		/// Only return file system directories. If the user selects folders
		/// that are not part of the file system, the OK button is grayed.
		/// </summary>
		[Category ( "Navigation" )]
		[Description ( "Only return file system directories. If the user selects folders " +
			 "that are not part of the file system, the OK button is grayed." )]
		[DefaultValue ( true )]
		public bool OnlyFilesystem
		{
			get 
			{
				return (publicOptions & (int) Win32API.Shell32.BffStyles.RestrictToFilesystem) != 0;
			}
			set 
			{
				SetOptionField ( (int) Win32API.Shell32.BffStyles.RestrictToFilesystem, value );
			}
		}

		/// <summary>
		/// Include network folders below the domain level in the
		/// dialog box's tree view control.
		/// </summary>
		[Category ( "Navigation" )]
		[Description ( "Include network folders below the domain level in the " + 
			 "dialog box's tree view control." )]
		[DefaultValue ( false )]
		public bool ShowNetworkFolders
		{
			get
			{
				// note the == 0 below instead of != 0
				return (publicOptions & (int) Win32API.Shell32.BffStyles.RestrictToDomain) == 0;
			}
			set
			{
				// note the !value below
				SetOptionField ( (int) Win32API.Shell32.BffStyles.RestrictToDomain, !value );
			}
		}

		/// <summary>
		/// Only return file system ancestors. An ancestor is a subfolder that
		/// is beneath the root folder in the namespace hierarchy. If the user
		/// selects an ancestor of the root folder that is not part of the file system,
		/// the OK button is grayed.
		/// </summary>
		[Category ( "Navigation" )]
		[Description ( "Only return file system ancestors. An ancestor is a subfolder that " +
			 "is beneath the root folder in the namespace hierarchy. If the user " +
			 "selects an ancestor of the root folder that is not part of the file system, " +
			 "the OK button is grayed." )]
		[DefaultValue ( false )]
		public bool OnlySubfolders
		{
			get 
			{
				return (publicOptions & (int) Win32API.Shell32.BffStyles.RestrictToSubfolders) != 0;
			}
			set 
			{
				SetOptionField ( (int) Win32API.Shell32.BffStyles.RestrictToSubfolders, value );
			}
		}

		/// <summary>
		/// Include an edit control in the browse dialog box that allows the user
		/// to type the name of an item.
		/// </summary>
		[Description ( "Include an edit control in the browse dialog box that allows the user " +
			 "to type the name of an item." )]
		[DefaultValue ( false )]
		public bool ShowTextBox
		{
			get 
			{
				return (publicOptions & (int) Win32API.Shell32.BffStyles.ShowTextBox) != 0;
			}
			set 
			{
				SetOptionField ( (int) Win32API.Shell32.BffStyles.ShowTextBox, value );
			}
		}

		/// <summary>
		/// If the user types an invalid name into the edit box, the browse dialog box
		/// will call the application's BrowseCallbackProc with the BFFM_VALIDATEFAILED
		/// message. This flag is ignored if ShowTextBox is false.
		/// </summary>
		[Description ( "If the user types an invalid name into the edit box, the browse dialog box " +
			 "will call the application's BrowseCallbackProc with the BFFM_VALIDATEFAILED " +
			 "message. This flag is ignored if ShowTextBox is false." )]
		[DefaultValue ( false )]
		public bool ValidateUserInput
		{
			get 
			{
				return (publicOptions & (int) Win32API.Shell32.BffStyles.ValidateSelection) != 0;
			}
			set 
			{
				SetOptionField ( (int) Win32API.Shell32.BffStyles.ValidateSelection, value );
			}
		}

		/// <summary>
		/// Only return computers. If the user selects anything other than a computer,
		/// the OK button is grayed.
		/// </summary>
		[Category ( "Selection Target" )]
		[Description ( "Only return computers. If the user selects anything other than a computer, " +
			 "the OK button is grayed." )]
		[DefaultValue ( false )]
		public bool SelectComputer
		{
			get 
			{
				return (publicOptions & (int) Win32API.Shell32.BffStyles.BrowseForComputer) != 0;
			}
			set 
			{
				SetOptionField ( (int) Win32API.Shell32.BffStyles.BrowseForComputer, value );
			}
		}

		/// <summary>
		/// Only return printers. If the user selects anything other than a printer,
		/// the OK button is grayed.
		/// </summary>
		[Category ( "Selection Target" )]
		[Description ( "Only return printers. If the user selects anything other than a printer, " +
			 "the OK button is grayed." )]
		[DefaultValue ( false )]
		public bool SelectPrinter
		{
			get 
			{
				return (publicOptions & (int) Win32API.Shell32.BffStyles.BrowseForPrinter) != 0;
			}
			set 
			{
				SetOptionField ( (int) Win32API.Shell32.BffStyles.BrowseForPrinter, value );
			}
		}

		/// <summary>
		/// The browse dialog box will display files as well as folders.
		/// </summary>
		[Category ( "Selection Target" )]
		[Description ( "The browse dialog box will display files as well as folders." )]
		[DefaultValue ( false )]
		public bool SelectFiles
		{
			get 
			{
				return (publicOptions & (int) Win32API.Shell32.BffStyles.BrowseForEverything) != 0;
			}
			set 
			{
				SetOptionField ( (int) Win32API.Shell32.BffStyles.BrowseForEverything, value );
			}
		}

		/// <summary>
		/// Gets the directory path of the folder the user picked.
		/// </summary>
		[Description ( "Contains the directory path of the folder the user picked." )]
		public string DirectoryPath 
		{
			get 
			{
				new FileIOPermission ( FileIOPermissionAccess.PathDiscovery, directoryPath ).Demand ( );
				return directoryPath;
			}
		}
    
		/// <summary>
		/// Location of the root folder from which to start browsing. Only the specified
		/// folder and any subfolders beneath it in the namespace hierarchy will appear
		/// in the dialog box.
		/// </summary>
		[Category ( "Navigation" )]
		[Description ( "Location of the root folder from which to start browsing. Only the specified " +
			 "folder and any subfolders beneath it in the namespace hierarchy will appear " +
			 "in the dialog box." )]
		[DefaultValue ( typeof(FolderID), "0")]
		public FolderID StartLocation 
		{
			get 
			{
				return startLocation;
			}
			set 
			{
				new UIPermission ( UIPermissionWindow.AllWindows ).Demand ( );
				startLocation = value;
			}
		}

		/// <summary>
		/// Description to be displayed above the folders tree view. It can provide 
		/// instructions for selecting a folder.
		/// </summary>
		[Description ( "Description to be displayed above the folders tree view. It can provide " +
			 "instructions for selecting a folder." )]
		[DefaultValue ( "Please select a folder below:")]
		public string Description
		{
			get
			{
				return descriptionText;
			}
			set
			{
				new UIPermission ( UIPermissionWindow.AllWindows ).Demand ( );
				descriptionText = ( value == null ) ? String.Empty: value;
			}
		}

		/// <summary>
		/// Helper function that returns the IMalloc interface used by the shell.
		/// </summary>
		private static Win32API.IMalloc GetSHMalloc ( )
		{
			Win32API.IMalloc malloc;
			Win32API.Shell32.SHGetMalloc ( out malloc );
			return malloc;
		}

		/// <summary>
		/// Shows the folder browser dialog.
		/// </summary>
		public DialogResult ShowDialog ( ) 
		{
			return ShowDialog ( null );
		}

		/// <summary>
		/// Shows the folder browser dialog with the specified owner window.
		/// </summary>
		public DialogResult ShowDialog ( IWin32Window owner ) 
		{
			IntPtr pidlRoot = IntPtr.Zero;

			// Get/find an owner HWND for this dialog
			IntPtr hWndOwner;

			if ( owner != null ) 
			{
				hWndOwner = owner.Handle;
			}
			else 
			{
				hWndOwner = Win32API.GetActiveWindow ( );
			}

			// Get the IDL for the specific startLocation
			Win32API.Shell32.SHGetSpecialFolderLocation ( hWndOwner, (int) startLocation, out pidlRoot );

			if (pidlRoot == IntPtr.Zero) 
			{
				return DialogResult.Cancel;
			}

			int mergedOptions = (int)publicOptions | (int)privateOptions;

			if ( ( mergedOptions & (int)Win32API.Shell32.BffStyles.NewDialogStyle ) != 0 ) 
			{
				Application.OleRequired ( );
			}

			IntPtr pidlRet = IntPtr.Zero;

			try 
			{
				// Construct a BROWSEINFO
				Win32API.Shell32.BROWSEINFO bi = new Win32API.Shell32.BROWSEINFO ( );
				IntPtr buffer = Marshal.AllocHGlobal ( MAX_PATH);

				bi.pidlRoot = pidlRoot;
				bi.hwndOwner = hWndOwner;
				bi.pszDisplayName = buffer;
				bi.lpszTitle = descriptionText;
				bi.ulFlags = mergedOptions;
				// Rest of the fields are initialized to zero by constructor
				// bi.lpfn = null;  bi.lParam = IntPtr.Zero;    bi.iImage = 0;

				// And show the dialog
				pidlRet = Win32API.Shell32.SHBrowseForFolder ( ref bi );

				// Free the buffer we've allocated on the global heap
				Marshal.FreeHGlobal ( buffer );

				if ( pidlRet == IntPtr.Zero ) 
				{
					// User pressed Cancel
					return DialogResult.Cancel;
				}

				// Then retrieve the path from the IDList
				StringBuilder sb = new StringBuilder ( MAX_PATH );
				if ( 0 == Win32API.Shell32.SHGetPathFromIDList ( pidlRet, sb ) )
				{
					return DialogResult.Cancel;
				}

				// Convert to a string
				directoryPath = sb.ToString ( );
			}
			finally 
			{
				Win32API.IMalloc malloc = GetSHMalloc ( );
				malloc.Free ( pidlRoot );

				if ( pidlRet != IntPtr.Zero ) 
				{
					malloc.Free ( pidlRet );
				}
			}

			return DialogResult.OK;
		} 

		/// <summary>
		/// Enum of CSIDLs identifying standard shell folders.
		/// </summary>
		public enum FolderID 
		{
			Desktop                   = 0x0000,
			Printers                  = 0x0004,
			MyDocuments               = 0x0005,
			Favorites                 = 0x0006,
			Recent                    = 0x0008,
			SendTo                    = 0x0009,
			StartMenu                 = 0x000b,
			MyComputer                = 0x0011,
			NetworkNeighborhood       = 0x0012,
			Templates                 = 0x0015,
			MyPictures                = 0x0027,
			NetAndDialUpConnections   = 0x0031,
		}
	} 

}
