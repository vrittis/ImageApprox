using System;
using System.Collections;
using imG.Approx.Components.BuildingBlocks;
using imG.Approx.Components.Shapes;
using imG.Approx.Mutation;
using NSubstitute;
using NUnit.Framework;

namespace imG.Approx.Tests.Components.Shapes
{
    public class BlobTests : BaseDrawingTest
    {
        public Blob Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new Blob();
        }

        public class Clone : BlobTests
        {
            [Test]
            [TestCaseSource("should_return_different_object_source")]
            public void should_return_different_object(Func<Blob, object> extractor)
            {
                Target.Points.Add(new Position());
                IShape clone = Target.Clone();

                Assert.That(clone, Is.Not.SameAs(Target));
                Assert.That(extractor(clone as Blob), Is.Not.Null);
                Assert.That(extractor(clone as Blob), Is.Not.SameAs(extractor(Target)));
            }

            public IEnumerable should_return_different_object_source()
            {
                yield return new object[] {new Func<Blob, object>(c => c.Color)};
                yield return new object[] {new Func<Blob, object>(c => c.Points[0])};
            }
        }

        public class Draw : BlobTests
        {
            [Test]
            public void should_draw()
            {
                Target.InitializeComponents(Process);
                DrawTest(g => Target.Draw(g));
            }
        }

        public class InitializeComponents : BlobTests
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

        public class MutableComponents : BlobTests
        {
            [Test]
            [TestCaseSource("should_return_components_source")]
            public void should_return_components(Blob source, IMutable component)
            {
                Assert.That(source.MutableComponents, Contains.Item(component));
            }

            public IEnumerable should_return_components_source()
            {
                var c = new Blob();
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