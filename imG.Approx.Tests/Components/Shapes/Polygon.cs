using System;
using System.Collections;
using imG.Approx.Components.BuildingBlocks;
using imG.Approx.Components.Shapes;
using imG.Approx.Mutation;
using NSubstitute;
using NUnit.Framework;

namespace imG.Approx.Tests.Components.Shapes
{
    public class PolygonTests : BaseDrawingTest
    {
        public Polygon Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new Polygon();
        }

        public class Clone : PolygonTests
        {
            [Test]
            [TestCaseSource("should_return_different_object_source")]
            public void should_return_different_object(Func<Polygon, object> extractor)
            {
                Target.Points.Add(new Position());
                IShape clone = Target.Clone();

                Assert.That(clone, Is.Not.SameAs(Target));
                Assert.That(extractor(clone as Polygon), Is.Not.Null);
                Assert.That(extractor(clone as Polygon), Is.Not.SameAs(extractor(Target)));
            }

            public IEnumerable should_return_different_object_source()
            {
                yield return new object[] {new Func<Polygon, object>(c => c.Color)};
                yield return new object[] {new Func<Polygon, object>(c => c.Points[0])};
            }
        }

        public class Draw : PolygonTests
        {
            [Test]
            public void should_draw()
            {
                Target.InitializeComponents(Process);
                DrawTest(g => Target.Draw(g));
            }
        }

        public class InitializeComponents : PolygonTests
        {
            [Test]
            public void should_randomize_elements()
            {
                Assert.That(Target.Color.G, Is.EqualTo(0));
                Assert.That(Target.Points[0].X, Is.EqualTo(0));
                Assert.That(Target.Points[1].X, Is.EqualTo(0));
                Assert.That(Target.Points[2].X, Is.EqualTo(0));
                Process.RandomizationProvider.Next(0, 0).ReturnsForAnyArgs(10);
                Target.InitializeComponents(Process);
                Assert.That(Target.Color.G, Is.EqualTo(10));
                Assert.That(Target.Points[0].X, Is.EqualTo(20));
                Assert.That(Target.Points[1].X, Is.EqualTo(20));
                Assert.That(Target.Points[2].X, Is.EqualTo(20));
            }
        }

        public class MutableComponents : PolygonTests
        {
            [Test]
            [TestCaseSource("should_return_components_source")]
            public void should_return_components(Polygon source, IMutable component)
            {
                Assert.That(source.MutableComponents, Contains.Item(component));
            }

            public IEnumerable should_return_components_source()
            {
                var c = new Polygon();
                c.Points.Add(new Position());
                yield return new object[] {c, c.Color};
                foreach (Position point in c.Points)
                {
                    yield return new object[] {c, point};
                }
            }
        }
    }
}