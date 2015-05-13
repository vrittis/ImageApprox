using imG.Approx.Components.BuildingBlocks;
using NSubstitute;
using NUnit.Framework;

namespace imG.Approx.Tests.Components.BuildingBlocks
{
    public class AmountTests : BaseIMutableTests
    {
        public Amount Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new Amount();
        }

        public class Clone : AmountTests
        {
            [Test]
            public void should_return_different_object()
            {
                Assert.That(Target.Clone(), Is.Not.SameAs(Target));
            }

            [Test]
            public void should_return_same_value()
            {
                Assert.That(Target.Clone().Value, Is.EqualTo(Target.Value));
            }
        }

        public class MutableComponents : AmountTests
        {
            [Test]
            public void should_return_empty()
            {
                Assert.That(Target.MutableComponents, Is.Empty);
            }
        }

        public class Nudge : AmountTests
        {
            [Test]
            [TestCase(100, 2, 12)]
            [TestCase(1000, 1000, 321)]
            [TestCase(100, -2, 8)]
            [TestCase(100, -10, 1)]
            [TestCase(100, -100, 1)]
            public void should_change_value_in_range(int amount, int result, int expected)
            {
                Target.Value = 10;
                Process.RandomizationProvider.Next(-amount, amount + 1).Returns(result);
                Target.Nudge(Process, amount);
                Assert.That(Target.Value, Is.EqualTo(expected));
            }
        }

        public class RandomizeValues : AmountTests
        {
            [Test]
            public void should_randomize_value_in_range()
            {
                Process.RandomizationProvider.Next(1, 322).Returns(15);
                Target.RandomizeValues(Process);
                Assert.That(Target.Value, Is.EqualTo(15));
            }
        }
    }
}