// --------------------------------------------------------------------------------
// <summary
// MSpec Specifications for ASCOM.Internal.Extensions.IntExtensions
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
    public class With_uint_register
    {
        protected static uint register;
        protected static Exception ex;
    }

    public class With_int_register
    {
        protected static uint register;
        protected static Exception ex;
    }

    [Subject(typeof (IntExtensions), "uint register")]
    public class When_UIntBit_LsbSet : With_uint_register
    {
        Establish context = () => register = 0x00000001;
        Because of = () => ex = Catch.Exception(() => register.Bit(32));
        It should_be_false_for_bit_1 = () => register.Bit(1).ShouldBeFalse();
        It should_be_false_for_bit_31 = () => register.Bit(31).ShouldBeFalse();
        It should_be_true_for_bit_0 = () => register.Bit(0).ShouldBeTrue();
        It should_throw_exception_for_bit_32 = () => ex.ShouldBeOfType<ArgumentOutOfRangeException>();
    }

    [Subject(typeof (IntExtensions), "uint register")]
    public class When_UIntBit_MsbSet : With_uint_register
    {
        Establish context = () => register = 0x80000000U;
        Because of = () => ex = Catch.Exception(() => register.Bit(32));
        It should_be_false_for_bit_0 = () => register.Bit(0).ShouldBeFalse();
        It should_be_false_for_bit_1 = () => register.Bit(1).ShouldBeFalse();
        It should_be_true_for_bit_31 = () => register.Bit(31).ShouldBeTrue();
        It should_throw_exception_for_bit_32 = () => ex.ShouldBeOfType<ArgumentOutOfRangeException>();
    }

    [Subject(typeof (IntExtensions), "int register")]
    public class When_IntBit_LsbSet : With_int_register
    {
        Establish context = () => register = 0x00000001;
        Because of = () => ex = Catch.Exception(() => register.Bit(32));
        It should_be_false_for_bit_1 = () => register.Bit(1).ShouldBeFalse();
        It should_be_false_for_bit_31 = () => register.Bit(31).ShouldBeFalse();
        It should_be_true_for_bit_0 = () => register.Bit(0).ShouldBeTrue();
        It should_throw_exception_for_bit_32 = () => ex.ShouldBeOfType<ArgumentOutOfRangeException>();
    }

    [Subject(typeof (IntExtensions), "int register")]
    public class When_IntBit_MsbSet : With_int_register
    {
        Establish context = () => register = 0x80000000U;
        Because of = () => ex = Catch.Exception(() => register.Bit(32));
        It should_be_false_for_bit_0 = () => register.Bit(0).ShouldBeFalse();
        It should_be_false_for_bit_1 = () => register.Bit(1).ShouldBeFalse();
        It should_be_true_for_bit_31 = () => register.Bit(31).ShouldBeTrue();
        It should_throw_exception_for_bit_32 = () => ex.ShouldBeOfType<ArgumentOutOfRangeException>();
    }
}