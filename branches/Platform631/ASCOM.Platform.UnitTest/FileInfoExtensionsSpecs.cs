// --------------------------------------------------------------------------------
// <summary
// MSpec Specifications for ASCOM.Internal.Extensions.FileInfoExtensions
// </summary>
// 
// <copyright company="TiGra Astronomy">
// 	Copyright © 2009-2010 Timothy P. Long, .
// 	http://www.tigranetworks.co.uk/Astronomy
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
// Original Author:		[TPL] Timothy P. Long <Tim@tigranetworks.co.uk>

using System.IO;
using ASCOM.Internal;
using Machine.Specifications;

namespace ASCOM.Platform.UnitTest
{
    /***
     * FileInfoExtensions
     * 
     * IsDeviceSpecific extension
     * --------------------------
     * Takes a FileInfo instance and examines the Name property
     * Returns true if the name has three parts (separated by '.') and the first part matches one of a set of known strings.
     * When name is 'Telescope.blah.cs' should return true.
     * When name is 'Telescope.cs' should return false.
     * When name is 'blah.blah.cs' should return false
     * When name is 'blah.cs' should return false.
     */

    public class With_empty_fileinfo
    {
        protected static FileInfo target;
    }

    // Telescope.blah.cs => should return true
    [Subject(typeof (FileInfoExtensions))]
    public class When_testing_for_telescope_specific_file : With_empty_fileinfo
    {
        Establish context = () => target = new FileInfo("Telescope.blah.cs");
        It should_return_true = () => target.IsDeviceSpecific().ShouldBeTrue();
    }

    // Telescope.cs => should return false
    [Subject(typeof (FileInfoExtensions))]
    public class When_testing_for_telescope_nonspecific_file : With_empty_fileinfo
    {
        Establish context = () => target = new FileInfo("Telescope.cs");
        It should_return_true = () => target.IsDeviceSpecific().ShouldBeFalse();
    }

    // blah.blah.cs => should return false
    [Subject(typeof (FileInfoExtensions))]
    public class When_testing_for_nonspecific_file_with_three_segments : With_empty_fileinfo
    {
        Establish context = () => target = new FileInfo("blah.blah.cs");
        It should_return_true = () => target.IsDeviceSpecific().ShouldBeFalse();
    }

    // blah.cs => should return false
    [Subject(typeof (FileInfoExtensions))]
    public class When_testing_for_nonspecific_file_with_two_segments : With_empty_fileinfo
    {
        Establish context = () => target = new FileInfo("blah.cs");
        It should_return_true = () => target.IsDeviceSpecific().ShouldBeFalse();
    }
}