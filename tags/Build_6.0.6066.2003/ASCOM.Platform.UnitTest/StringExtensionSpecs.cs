// --------------------------------------------------------------------------------
// <summary
// MSpec Specifications for ASCOM.Internal.Extensions.StringExtensions
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

using System;
using ASCOM.Internal;
using Machine.Specifications;

namespace ASCOM.Platform.UnitTest
{

    #region Test contexts

    public class with_head_tail
    {
        protected static string sample = "head tail";
        protected static string actual;

        Establish context = () => { };
    }

    public class with_head
    {
        protected static string sample = "head";
        protected static string actual;

        Establish context = () => { };
    }

    public class with_empty_sample
    {
        protected static string sample = string.Empty;
        protected static string actual;

        Establish context = () => { };
    }

    public class with_null_sample
    {
        protected static string sample;
        protected static string actual;

        Establish context = () => { };
    }

    #endregion

    [Subject(typeof (StringExtensions), "Head")]
    public class When_getting_head_size_4 : with_head_tail
    {
        Because of = () => actual = sample.Head(4);

        It should_return_head = () => actual.ShouldEqual("head");
    }

    [Subject(typeof (StringExtensions), "Head")]
    public class When_getting_head_size_0 : with_head_tail
    {
        Because of = () => actual = sample.Head(0);

        It should_return_empty_string = () => actual.ShouldBeEmpty();
    }

    [Subject(typeof (StringExtensions), "Head")]
    public class When_requested_head_length_is_greater_than_sample_length : with_empty_sample
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => { actual = sample.Head(1); });

        It should_fail = () => exception.ShouldNotBeNull();
        It should_throw_argument_out_of_range = () => exception.ShouldBeOfType<ArgumentOutOfRangeException>();
    }

    [Subject(typeof (StringExtensions), "Tail")]
    public class When_tail_length_is_4 : with_head_tail
    {
        Because of = () => actual = sample.Tail(4);
        It should_return_tail = () => actual.ShouldEqual("tail");
    }

    [Subject(typeof (StringExtensions), "Tail")]
    public class When_tail_length_is_zero : with_head_tail
    {
        Because of = () => actual = sample.Tail(0);
        It should_return_empty_string = () => actual.ShouldBeEmpty();
    }

    [Subject(typeof (StringExtensions), "Tail")]
    public class When_tail_length_exceeds_sample_length : with_empty_sample
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => { actual = sample.Tail(1); });
        It should_fail = () => exception.ShouldNotBeNull();
        It should_throw_argument_out_of_range = () => exception.ShouldBeOfType<ArgumentOutOfRangeException>();
    }

    [Subject(typeof (StringExtensions), "Clean")]
    public class When_cleaning_spaces : with_head_tail
    {
        Because of = () => actual = sample.Clean(" ");

        It should_return_space = () => actual.ShouldEqual(" ");
    }

    [Subject(typeof (StringExtensions), "Clean")]
    public class When_cleaning_causes_all_characters_to_be_removed : with_head_tail
    {
        Because of = () => actual = sample.Clean("$");

        It should_return_empty_string = () => actual.ShouldBeEmpty();
    }

    [Subject(typeof (StringExtensions), "Clean")]
    public class When_cleaning_empty_source_string : with_empty_sample
    {
        Because of = () => actual = sample.Clean("$");

        It should_return_empty_string = () => actual.ShouldBeEmpty();
    }

    [Subject(typeof (StringExtensions), "Clean")]
    public class When_cleaning_a_null_sample : with_null_sample
    {
        Because of = () => actual = sample.Clean("$");

        It should_return_empty_string = () => actual.ShouldBeEmpty();
    }

    [Subject(typeof (StringExtensions), "RemoveHead")]
    public class When_removing_head_length_5 : with_head_tail
    {
        Because of = () => actual = sample.RemoveHead(5);

        It should_return_tail = () => actual.ShouldEqual("tail");
    }

    [Subject(typeof (StringExtensions), "RemoveTail")]
    public class When_removing_tail_length_5 : with_head_tail
    {
        Because of = () => actual = sample.RemoveTail(5);

        It should_return_head = () => actual.ShouldEqual("head");
    }

    [Subject(typeof (StringExtensions), "RemoveHead")]
    public class When_removing_head_length_greater_than_sample_length : with_head
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => { actual = sample.RemoveHead(5); });
        It should_fail = () => exception.ShouldNotBeNull();
        It should_throw_argument_out_of_range = () => exception.ShouldBeOfType<ArgumentOutOfRangeException>();
    }

    [Subject(typeof (StringExtensions), "RemoveTail")]
    public class when_removing_tail_length_greater_than_sample_length : with_head
    {
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => { actual = sample.RemoveTail(5); });
        It should_fail = () => exception.ShouldNotBeNull();
        It should_throw_argument_out_of_range = () => exception.ShouldBeOfType<ArgumentOutOfRangeException>();
    }
}