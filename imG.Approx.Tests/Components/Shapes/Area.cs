using System;
using System.Collections;
using imG.Approx.Components.Shapes;
using imG.Approx.Mutation;
using NSubstitute;
using NUnit.Framework;

namespace imG.Approx.Tests.Components.Shapes
{
    public class AreaTests : BaseDrawingTest
    {
        public Area Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new Area();
        }

        public class Clone : AreaTests
        {
            [Test]
            [TestCaseSource("should_return_different_object_source")]
            public void should_return_different_object(Func<Area, object> extractor)
            {
                IShape clone = Target.Clone();

                Assert.That(clone, Is.Not.SameAs(Target));
                Assert.That(extractor(clone as Area), Is.Not.Null);
                Assert.That(extractor(clone as Area), Is.Not.SameAs(extractor(Target)));
            }

            public IEnumerable should_return_different_object_source()
            {
                yield return new object[] {new Func<Area, object>(c => c.Color)};
                yield return new object[] {new Func<Area, object>(c => c.Angle)};
                yield return new object[] {new Func<Area, object>(c => c.Center)};
            }
        }

        public class Draw : AreaTests
        {
            [Test]
            public void should_draw()
            {
                Target.InitializeComponents(Process);
                DrawTest(g => Target.Draw(g));
            }
        }

        public class InitializeComponents : AreaTests
        {
            [Test]
            public void should_randomize_elements()
            {
                Assert.That(Target.Color.G, Is.EqualTo(0));
                Assert.That(Target.Angle.Value, Is.EqualTo(0));
                Assert.That(Target.Center.X, Is.EqualTo(0));
                Process.RandomizationProvider.Next(0, 0).ReturnsForAnyArgs(10);
                Target.InitializeComponents(Process);
                Assert.That(Target.Color.G, Is.EqualTo(10));
                Assert.That(Target.Angle.Value, Is.EqualTo(10));
                Assert.That(Target.Center.X, Is.EqualTo(10));
            }
        }

        public class MutableComponents : AreaTests
        {
            [Test]
            [TestCaseSource("should_return_components_source")]
            public void should_return_components(Area source, IMutable component)
            {
                Assert.That(source.MutableComponents, Contains.Item(component));
            }

            public IEnumerable should_return_components_source()
            {
                var c = new Area();
                yield return new object[] {c, c.Color};
                yield return new object[] {c, c.Angle};
                yield return new object[] {c, c.Center};
            }
        }
    }
}