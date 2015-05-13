using System;
using System.Collections;
using imG.Approx.Components.Shapes;
using imG.Approx.Mutation;
using NSubstitute;
using NUnit.Framework;

namespace imG.Approx.Tests.Components.Shapes
{
    public class RectangleTests : BaseDrawingTest
    {
        public Rectangle Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new Rectangle();
        }

        public class Clone : RectangleTests
        {
            [Test]
            [TestCaseSource("should_return_different_object_source")]
            public void should_return_different_object(Func<Rectangle, object> extractor)
            {
                IShape clone = Target.Clone();

                Assert.That(clone, Is.Not.SameAs(Target));
                Assert.That(extractor(clone as Rectangle), Is.Not.Null);
                Assert.That(extractor(clone as Rectangle), Is.Not.SameAs(extractor(Target)));
            }

            public IEnumerable should_return_different_object_source()
            {
                yield return new object[] {new Func<Rectangle, object>(c => c.Color)};
                yield return new object[] {new Func<Rectangle, object>(c => c.Height)};
                yield return new object[] {new Func<Rectangle, object>(c => c.Width)};
                yield return new object[] {new Func<Rectangle, object>(c => c.TopLeft)};
            }
        }

        public class Draw : RectangleTests
        {
            [Test]
            public void should_draw()
            {
                Target.InitializeComponents(Process);
                DrawTest(g => Target.Draw(g));
            }
        }

        public class InitializeComponents : RectangleTests
        {
            [Test]
            public void should_randomize_elements()
            {
                Assert.That(Target.Color.G, Is.EqualTo(0));
                Assert.That(Target.Height.Value, Is.EqualTo(1));
                Assert.That(Target.Width.Value, Is.EqualTo(1));
                Assert.That(Target.TopLeft.X, Is.EqualTo(0));
                Process.RandomizationProvider.Next(0, 0).ReturnsForAnyArgs(10);
                Target.InitializeComponents(Process);
                Assert.That(Target.Color.G, Is.EqualTo(10));
                Assert.That(Target.Height.Value, Is.EqualTo(1));
                Assert.That(Target.Width.Value, Is.EqualTo(1));
                Assert.That(Target.TopLeft.X, Is.EqualTo(10));
            }
        }


        public class MutableComponents : RectangleTests
        {
            [Test]
            [TestCaseSource("should_return_components_source")]
            public void should_return_components(Rectangle source, IMutable component)
            {
                Assert.That(source.MutableComponents, Contains.Item(component));
            }

            public IEnumerable should_return_components_source()
            {
                var c = new Rectangle();
                yield return new object[] {c, c.Color};
                yield return new object[] {c, c.Height};
                yield return new object[] {c, c.Width};
                yield return new object[] {c, c.TopLeft};
            }
        }
    }
}