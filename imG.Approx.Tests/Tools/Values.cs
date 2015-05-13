using System;
using imG.Approx.Tools;
using NUnit.Framework;

namespace imG.Approx.Tests.Tools
{
    public class TestValues
    {
        public class Clamp : TestValues
        {
            [Test]
            public void should_throw_if_min_is_above_max()
            {
                Assert.That(() => 1.Clamp(1, 0), Throws.InstanceOf<ArgumentOutOfRangeException>());
            }

            [Test]
            [TestCase(1, 0, 1)]
            [TestCase(1, 10, 10)]
            [TestCase(1, 1, 1)]
            public void should_return_max_value_between_original_and_min_value(int value, int min, int expected)
            {
                Assert.That(value.Clamp(min, min + 100), Is.EqualTo(expected));
            }

            [Test]
            [TestCase(1, 0, 0)]
            [TestCase(1, 10, 1)]
            [TestCase(1, 1, 1)]
            public void should_return_min_value_between_original_and_max_value(int value, int max, int expected)
            {
                Assert.That(value.Clamp(max - 100, max), Is.EqualTo(expected));
            }
        }

        public class Wrap : TestValues
        {
            [Test]
            public void should_throw_if_min_is_above_max()
            {
                Assert.That(() => 1.Wrap(360, 0), Throws.InstanceOf<ArgumentOutOfRangeException>());
            }

            [Test]
            [TestCase(16, 10, 25, 16)]
            [TestCase(10, 10, 25, 10)]
            [TestCase(25, 10, 25, 10)]
            [TestCase(101, 10, 20, 11)]
            [TestCase(-101, 10, 20, 19)]
            public void should_wrap_back_to_range(int value, int min, int max, int expected)
            {
                Assert.That((value).Wrap(min, max), Is.EqualTo(expected));
            }
        }
    }
}