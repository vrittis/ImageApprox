using System;
using System.Collections;
using imG.Approx.Components.Shapes;
using imG.Approx.Mutation;
using NSubstitute;
using NUnit.Framework;

namespace imG.Approx.Tests.Components.Shapes
{
    public class CircleTests : BaseDrawingTest
    {
        public Circle Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new Circle();
        }


        public class Clone : CircleTests
        {
            [Test]
            [TestCaseSource("should_return_different_object_source")]
            public void should_return_different_object(Func<Circle, object> extractor)
            {
                IShape clone = Target.Clone();

                Assert.That(clone, Is.Not.SameAs(Target));
                Assert.That(extractor(clone as Circle), Is.Not.Null);
                Assert.That(extractor(clone as Circle), Is.Not.SameAs(extractor(Target)));
            }

            public IEnumerable should_return_different_object_source()
            {
                yield return new object[] {new Func<Circle, object>(c => c.Color)};
                yield return new object[] {new Func<Circle, object>(c => c.Size)};
                yield return new object[] {new Func<Circle, object>(c => c.TopLeft)};
            }
        }

        public class Draw : CircleTests
        {
            [Test]
            public void should_draw()
            {
                Target.InitializeComponents(Process);
                DrawTest(g => Target.Draw(g));
            }
        }

        public class InitializeComponents : CircleTests
        {
            [Test]
            public void should_randomize_elements()
            {
                Assert.That(Target.Color.G, Is.EqualTo(0));
                Assert.That(Target.TopLeft.X, Is.EqualTo(0));
                Assert.That(Target.Size.Value, Is.EqualTo(1));
                Process.RandomizationProvider.Next(0, 0).ReturnsForAnyArgs(10);
                Target.InitializeComponents(Process);
                Assert.That(Target.Color.G, Is.EqualTo(10));
                Assert.That(Target.TopLeft.X, Is.EqualTo(10));
                Assert.That(Target.Size.Value, Is.EqualTo(1));
            }
        }

        public class MutableComponents : CircleTests
        {
            [Test]
            [TestCaseSource("should_return_components_source")]
            public void should_return_components(Circle source, IMutable component)
            {
                Assert.That(source.MutableComponents, Contains.Item(component));
            }

            public IEnumerable should_return_components_source()
            {
                var c = new Circle();
                yield return new object[] {c, c.Color};
                yield return new object[] {c, c.Size};
                yield return new object[] {c, c.TopLeft};
            }
        }
    }
}