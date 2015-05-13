using imG.Approx.Components.BuildingBlocks;
using NSubstitute;
using NUnit.Framework;

namespace imG.Approx.Tests.Components.BuildingBlocks
{
    public class AngleTests : BaseIMutableTests
    {
        public Angle Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new Angle();
        }

        public class Clone : AngleTests
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

        public class MutableComponents : AngleTests
        {
            [Test]
            public void should_return_empty()
            {
                Assert.That(Target.MutableComponents, Is.Empty);
            }
        }

        public class Nudge : AngleTests
        {
            [Test]
            [TestCase(10, 40, true, 50)]
            [TestCase(10, 40, false, 330)]
            [TestCase(10, 6, false, 4)]
            [TestCase(10, 355, true, 5)]
            public void should_wrap_by_a_value_between_1_and_max_value_in_both_directions(int startValue, int amount,
                bool positive, int expected)
            {
                Target.Value = startValue;
                Process.RandomizationProvider.Next(1, amount + 1).Returns(amount);
                Process.RandomizationProvider.Next(0, 2).Returns(positive ? 1 : 0);
                Target.Nudge(Process, amount);
                Assert.That(Target.Value, Is.EqualTo(expected));
            }
        }

        public class RandomizeValue : AngleTests
        {
            [Test]
            public void should_select_a_value_between_0_and_360()
            {
                Process.RandomizationProvider.Next(0, 360).Returns(100);
                Target.RandomizeValues(Process);
                Assert.That(Target.Value, Is.EqualTo(100));
            }
        }
    }
}