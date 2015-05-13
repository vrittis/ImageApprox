using System;
using System.Collections;
using imG.Approx.Components.BuildingBlocks;
using imG.Approx.Components.Shapes;
using imG.Approx.Mutation;
using NSubstitute;
using NUnit.Framework;

namespace imG.Approx.Tests.Components.Shapes
{
    public class TriangleTests : BaseDrawingTest
    {
        public Triangle Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new Triangle();
        }

        public class Clone : TriangleTests
        {
            [Test]
            [TestCaseSource("should_return_different_object_source")]
            public void should_return_different_object(Func<Triangle, object> extractor)
            {
                IShape clone = Target.Clone();

                Assert.That(clone, Is.Not.SameAs(Target));
                Assert.That(extractor(clone as Triangle), Is.Not.Null);
                Assert.That(extractor(clone as Triangle), Is.Not.SameAs(extractor(Target)));
            }

            public IEnumerable should_return_different_object_source()
            {
                yield return new object[] {new Func<Triangle, object>(c => c.Color)};
                yield return new object[] {new Func<Triangle, object>(c => c.Points[0])};
                yield return new object[] {new Func<Triangle, object>(c => c.Points[1])};
                yield return new object[] {new Func<Triangle, object>(c => c.Points[2])};
            }
        }

        public class Draw : TriangleTests
        {
            [Test]
            public void should_draw()
            {
                Target.InitializeComponents(Process);
                DrawTest(g => Target.Draw(g));
            }
        }

        public class InitializeComponents : TriangleTests
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


        public class MutableComponents : TriangleTests
        {
            [Test]
            [TestCaseSource("should_return_components_source")]
            public void should_return_components(Triangle source, IMutable component)
            {
                Assert.That(source.MutableComponents, Contains.Item(component));
            }

            public IEnumerable should_return_components_source()
            {
                var c = new Triangle();
                yield return new object[] {c, c.Color};
                foreach (Position point in c.Points)
                {
                    yield return new object[] {c, point};
                }
            }
        }
    }
}