using ASCOM.Internal;
using Machine.Specifications;
using System.IO;

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
    [Subject(typeof(FileInfoExtensions))]
    public class When_testing_for_telescope_specific_file : With_empty_fileinfo
    {
        Establish context = () => target = new FileInfo("Telescope.blah.cs");
        It should_return_true = () => target.IsDeviceSpecific().ShouldBeTrue();
    }

    // Telescope.cs => should return false
    [Subject(typeof(FileInfoExtensions))]
    public class When_testing_for_telescope_nonspecific_file : With_empty_fileinfo
    {
        Establish context = () => target = new FileInfo("Telescope.cs");
        It should_return_true = () => target.IsDeviceSpecific().ShouldBeFalse();
    }

    // blah.blah.cs => should return false
    [Subject(typeof(FileInfoExtensions))]
    public class When_testing_for_nonspecific_file_with_three_segments : With_empty_fileinfo
    {
        Establish context = () => target = new FileInfo("blah.blah.cs");
        It should_return_true = () => target.IsDeviceSpecific().ShouldBeFalse();
    }

    // blah.cs => should return false
    [Subject(typeof(FileInfoExtensions))]
    public class When_testing_for_nonspecific_file_with_two_segments : With_empty_fileinfo
    {
        Establish context = () => target = new FileInfo("blah.cs");
        It should_return_true = () => target.IsDeviceSpecific().ShouldBeFalse();
    }



}
