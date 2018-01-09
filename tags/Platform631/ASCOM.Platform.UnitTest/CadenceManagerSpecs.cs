// --------------------------------------------------------------------------------
// <summary
// MSpec Specifications for ASCOM.Controls.CadenceManager class
// </summary>
//
// <copyright company="TiGra Astronomy">
//	Copyright © 2009-2010 Timothy P. Long, .
//	http://www.tigranetworks.co.uk/Astronomy
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

using ASCOM.Controls;
using Machine.Specifications;

namespace ASCOM.Platform.UnitTest
{
    [Subject(typeof (CadenceManager), "Duplicate detection")]
    public class when_the_same_control_is_added_twice : with_clean_cadencemanager
    {
        Because of = () =>
            {
                CadenceManager.Instance.Add(testControl);
                CadenceManager.Instance.Add(testControl);
            };

        It should_only_add_one_instance = () => CadenceManager.UpdateList.Count.ShouldEqual(1);
    }


    public class with_clean_cadencemanager
    {
        protected static ICadencedControl testControl;
        protected static CadenceManager manager;

        Establish context = () =>
            {
                testControl = new Annunciator();
                manager = CadenceManager.Instance;
                CadenceManager.UpdateList.Clear();
            };
    }
}