using imG.Approx.Components.BuildingBlocks;
using NSubstitute;
using NUnit.Framework;

namespace imG.Approx.Tests.Components.BuildingBlocks
{
    public class PositionTests : BaseIMutableTests
    {
        public Position Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new Position();
        }

        public class Clone : PositionTests
        {
            [Test]
            public void should_return_different_object()
            {
                Assert.That(Target.Clone(), Is.Not.SameAs(Target));
            }

            [Test]
            public void should_return_same_value()
            {
                Position clone = Target.Clone();
                Assert.That(clone.X, Is.EqualTo(Target.X));
                Assert.That(clone.Y, Is.EqualTo(Target.Y));
            }
        }

        public class MutableComponents : PositionTests
        {
            [Test]
            public void should_return_empty()
            {
                Assert.That(Target.MutableComponents, Is.Empty);
            }
        }

        public class Nudge : PositionTests
        {
            [Test]
            [TestCase(10, 10, 1, true, 11, 11)]
            [TestCase(10, 10, 1, false, 9, 9)]
            [TestCase(10, 10, 1000, true, 321, 654)]
            [TestCase(10, 10, 100, false, 0, 0)]
            public void should_change_value_in_range(int startX, int startY, int amount, bool positive, int expectedX,
                int expectedY)
            {
                Target.X = startX;
                Target.Y = startY;
                Process.RandomizationProvider.Next(-amount, amount + 1).Returns(amount*(positive ? 1 : -1));
                Target.Nudge(Process, amount);
                Assert.That(Target.X, Is.EqualTo(expectedX));
                Assert.That(Target.Y, Is.EqualTo(expectedY));
            }
        }

        public class RandomizeValues : PositionTests
        {
            [Test]
            public void should_return_value_inside_target_limits()
            {
                Process.RandomizationProvider.Next(0, 321).Returns(200);
                Process.RandomizationProvider.Next(0, 654).Returns(100);
                Target.RandomizeValues(Process);
                Process.RandomizationProvider.Received(1).Next(0, 321);
                Process.RandomizationProvider.Received(1).Next(0, 654);
                Assert.That(Target.X, Is.EqualTo(200));
                Assert.That(Target.Y, Is.EqualTo(100));
            }
        }
    }
}