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
    [Subject(typeof(IntExtensions), "uint register")]
    public class When_UIntBit_LsbSet : With_uint_register
    {
        Establish context = () => register = 0x00000001;
        Because of = () => ex = Catch.Exception(() => register.Bit(32));
        It should_be_true_for_bit_0 = () => register.Bit(0).ShouldBeTrue();
        It should_be_false_for_bit_1 = () => register.Bit(1).ShouldBeFalse();
        It should_be_false_for_bit_31 = () => register.Bit(31).ShouldBeFalse();
        It should_throw_exception_for_bit_32 = () => ex.ShouldBeOfType<ArgumentOutOfRangeException>();
    }
    [Subject(typeof(IntExtensions), "uint register")]
    public class When_UIntBit_MsbSet : With_uint_register
    {
        Establish context = () => register = 0x80000000U;
        Because of = () => ex = Catch.Exception(() => register.Bit(32));
        It should_be_false_for_bit_0 = () => register.Bit(0).ShouldBeFalse();
        It should_be_false_for_bit_1 = () => register.Bit(1).ShouldBeFalse();
        It should_be_true_for_bit_31 = () => register.Bit(31).ShouldBeTrue();
        It should_throw_exception_for_bit_32 = () => ex.ShouldBeOfType<ArgumentOutOfRangeException>();
    }
    [Subject(typeof(IntExtensions), "int register")]
    public class When_IntBit_LsbSet : With_int_register
    {
        Establish context = () => register = 0x00000001;
        Because of = () => ex = Catch.Exception(() => register.Bit(32));
        It should_be_true_for_bit_0 = () => register.Bit(0).ShouldBeTrue();
        It should_be_false_for_bit_1 = () => register.Bit(1).ShouldBeFalse();
        It should_be_false_for_bit_31 = () => register.Bit(31).ShouldBeFalse();
        It should_throw_exception_for_bit_32 = () => ex.ShouldBeOfType<ArgumentOutOfRangeException>();
    }
    [Subject(typeof(IntExtensions), "int register")]
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
