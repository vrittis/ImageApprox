using System.Drawing;
using System.Linq;
using imG.Approx.Components;
using imG.Approx.Components.Shapes;
using NSubstitute;
using NUnit.Framework;
using Color = imG.Approx.Components.BuildingBlocks.Color;

namespace imG.Approx.Tests.Components
{
    public class DrawingTest
    {
        public Drawing Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new Drawing(100, 150);
            var shape = Substitute.For<IShape>();
            shape.Clone().Returns(Substitute.For<IShape>());
            Target.Shapes.Add(shape);
        }

        public class Clone : DrawingTest
        {
            [Test]
            public void should_create_clone_of_target()
            {
                Assert.That(Target.Clone(), Is.Not.SameAs(Target));
            }

            [Test]
            public void should_copy_properties()
            {
                Drawing clone = Target.Clone();
                Assert.That(clone.Width, Is.EqualTo(Target.Width));
                Assert.That(clone.Height, Is.EqualTo(Target.Height));
            }

            [Test]
            public void should_clone_inner_components()
            {
                Assert.That(Target.Clone().BackgroundColor, Is.Not.SameAs(Target.BackgroundColor));
            }

            [Test]
            public void should_clone_all_shapes()
            {
                Drawing clone = Target.Clone();
                Assert.That(clone.Shapes.Count, Is.EqualTo(Target.Shapes.Count));

                for (int shapeCounter = 0; shapeCounter < clone.Shapes.Count; shapeCounter++)
                {
                    Assert.That(clone.Shapes[shapeCounter], Is.Not.SameAs(Target.Shapes[shapeCounter]));
                    Assert.That(clone.Shapes[shapeCounter].GetType(), Is.EqualTo(Target.Shapes[shapeCounter].GetType()));
                }
            }
        }

        public class Constructor : DrawingTest
        {
            [Test]
            public void should_keep_data()
            {
                Assert.That(Target.Width, Is.EqualTo(100));
                Assert.That(Target.Height, Is.EqualTo(150));
            }
        }

        public class Draw : DrawingTest
        {
            [Test]
            public void should_return_correct_size_image()
            {
                Bitmap bmp = Target.Draw();
                Assert.That(bmp.Width, Is.EqualTo(100));
                Assert.That(bmp.Height, Is.EqualTo(150));
            }

            [Test]
            public void should_fill_image_with_background_color()
            {
                Target.BackgroundColor = new Color {A = 255, R = 100, G = 100, B = 100};
                Bitmap bmp = Target.Draw();
                Assert.That(bmp.GetPixel(0, 0).R, Is.EqualTo(100));
                Assert.That(bmp.GetPixel(0, 0).G, Is.EqualTo(100));
                Assert.That(bmp.GetPixel(0, 0).B, Is.EqualTo(100));
            }

            [Test]
            public void should_draw_all_shapes()
            {
                Target.Shapes.Add(Substitute.For<IShape>());
                Target.Draw();
                Assert.That(Target.Shapes.All(s =>
                {
                    s.ReceivedWithAnyArgs(1).Draw(null);
                    return true;
                }));
            }
        }

        public class MutableComponents : DrawingTest
        {
            [Test]
            public void should_contain_color()
            {
                Assert.That(Target.MutableComponents, Contains.Item(Target.BackgroundColor));
            }

            [Test]
            public void should_contain_all_shapes()
            {
                foreach (IShape shape in Target.Shapes)
                {
                    Assert.That(Target.MutableComponents, Contains.Item(shape));
                }
            }
        }
    }
}