using System;
using System.Collections;
using imG.Approx.Components.Shapes;
using imG.Approx.Mutation;
using NSubstitute;
using NUnit.Framework;

namespace imG.Approx.Tests.Components.Shapes
{
    public class LineTests : BaseDrawingTest
    {
        public Line Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new Line();
        }


        public class Clone : LineTests
        {
            [Test]
            [TestCaseSource("should_return_different_object_source")]
            public void should_return_different_object(Func<Line, object> extractor)
            {
                IShape clone = Target.Clone();

                Assert.That(clone, Is.Not.SameAs(Target));
                Assert.That(extractor(clone as Line), Is.Not.Null);
                Assert.That(extractor(clone as Line), Is.Not.SameAs(extractor(Target)));
            }

            public IEnumerable should_return_different_object_source()
            {
                yield return new object[] {new Func<Line, object>(c => c.Color)};
                yield return new object[] {new Func<Line, object>(c => c.PenSize)};
                yield return new object[] {new Func<Line, object>(c => c.Point1)};
                yield return new object[] {new Func<Line, object>(c => c.Point2)};
            }
        }

        public class Draw : LineTests
        {
            [Test]
            public void should_draw()
            {
                Target.InitializeComponents(Process);
                DrawTest(g => Target.Draw(g));
            }
        }

        public class InitializeComponents : LineTests
        {
            [Test]
            public void should_randomize_elements()
            {
                Assert.That(Target.Color.G, Is.EqualTo(0));
                Assert.That(Target.Point1.X, Is.EqualTo(0));
                Assert.That(Target.Point2.X, Is.EqualTo(0));
                Assert.That(Target.PenSize.Size, Is.EqualTo(1));
                Process.RandomizationProvider.Next(0, 0).ReturnsForAnyArgs(10);
                Target.InitializeComponents(Process);
                Assert.That(Target.Color.G, Is.EqualTo(10));
                Assert.That(Target.Point1.X, Is.EqualTo(20));
                Assert.That(Target.Point2.X, Is.EqualTo(20));
                Assert.That(Target.PenSize.Size, Is.EqualTo(1));
            }
        }

        public class MutableComponents : LineTests
        {
            [Test]
            [TestCaseSource("should_return_components_source")]
            public void should_return_components(Line source, IMutable component)
            {
                Assert.That(source.MutableComponents, Contains.Item(component));
            }

            public IEnumerable should_return_components_source()
            {
                var c = new Line();
                yield return new object[] {c, c.Color};
                yield return new object[] {c, c.PenSize};
                yield return new object[] {c, c.Point1};
                yield return new object[] {c, c.Point2};
            }
        }
    }
}