//tabs=4
// --------------------------------------------------------------------------------
// <summary>
// ASCOM.Internal.FileInfoExtensions
// </summary>
//
// <copyright company="The ASCOM Initiative" author="Timothy P. Long">
//	Copyright © 2010 TiGra Astronomy
// </copyright>
//
// <license>
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </license>
//
// Author:		(TPL) Timothy P. Long <Tim@tigranetworks.co.uk>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 11-Mar-2010	TPL	6.0.*	Initial edit.
// --------------------------------------------------------------------------------


namespace ASCOM.Internal
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.IO;

	/// <summary>
	/// Extension methods relating to <see cref="FileInfo"/>.
	/// </summary>
	public static class FileInfoExtensions
	{
		/// <summary>
		/// The recognized ASCOM device classes.
		/// </summary>
		/// <remarks>
		/// This list is specifically intended to support the Template Project Wizard
		/// and contains a list of the device types for which the Wizard can expand
		/// a project template.
		/// </remarks>
		private static readonly List<string> deviceClasses = new List<string>()
		{
			"Telescope",
			"Focuser",
			"Camera",
			"FilterWheel",
			"Switch",
			"Dome",
			"Rotator",
			"MultiPortSelector"
		};

		/// <summary>
		/// Determines whether the specified file is device specific.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <returns>
		/// 	<c>true</c> if it is device specific; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>
		/// A file is considered 'device specific' if it meets the following criteria:
		/// <list type="number">
		///		<item>
		///			The filename contains at least two dots ('.');
		///		</item>
		///		<item>
		///			The first dot-delimited segment of the filename matches one of the
		///			recognized interface types contained in
		///		</item>
		/// </list>
		/// </remarks>
		public static bool IsDeviceSpecific(this FileInfo file)
		{
			// Split the file name into dot-seperated segments.
			string[] parts = file.Name.Split('.');
			if (parts.Count() < 3)
				return false;
			if (String.IsNullOrEmpty(parts[0]))
				return false;
			if (deviceClasses.Contains(parts[0]))
				return true;
			return false;
		}
	}
}
