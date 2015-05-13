using System;
using System.Collections;
using System.Drawing.Drawing2D;
using imG.Approx.Components;
using imG.Approx.Components.BuildingBlocks;
using imG.Approx.Mutation;
using NUnit.Framework;

namespace imG.Approx.Tests.Mutation
{
    public class TargetTests
    {
        public Target Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new Target("data\\red.png", 25);
        }

        public class Name : TargetTests
        {
            [Test]
            public void should_return_filename()
            {
                Assert.That(Target.Name, Is.EqualTo("red.png"));
            }
        }

        public class Constructor : TargetTests
        {
            [Test]
            [TestCaseSource("should_keep_initialized_data_source")]
            public void should_keep_initialized_data(Func<Target, object> evaluator, object result)
            {
                Assert.That(evaluator(Target), Is.EqualTo(result));
            }

            public IEnumerable should_keep_initialized_data_source()
            {
                yield return new object[] {new Func<Target, object>(fc => fc.MaxDimension), 25};
                yield return new object[] {new Func<Target, object>(fc => fc.File), "data\\red.png"};
            }
        }

        public class DistanceTo : TargetTests
        {
            [Test]
            [TestCaseSource("should_throw_if_dimensions_are_different_source")]
            public void should_throw_if_dimensions_are_different(Drawing drawing)
            {
                Assert.That(() => Target.DistanceTo(drawing, 0), Throws.InstanceOf<InvalidOperationException>());
            }

            public IEnumerable should_throw_if_dimensions_are_different_source()
            {
                yield return new Drawing(10, 10);
                yield return new Drawing(25, 10);
                yield return new Drawing(10, 5);
            }

            [Test]
            public void should_not_throw_if_dimensions_are_identical()
            {
                Target.DistanceTo(new Drawing(25, 5), 0);
            }

            //[Test]
            //[TestCaseSource("should_compute_distance_to_drawing_source")]
            //public void should_compute_distance_to_drawing(string file, Drawing drawing, double expectedDistance)
            //{
            //    Target = new Target(file, 25);
            //    Assert.That(Target.DistanceTo(drawing, (ulong) (expectedDistance + 1)), Is.EqualTo(expectedDistance));
            //}

            //public IEnumerable should_compute_distance_to_drawing_source()
            //{
            //    yield return new object[] {"data\\red.png", new Drawing(25, 5) {BackgroundColor = new Color {R = 255, A = 255}}, 0};
            //    yield return
            //        new object[] {"data\\red.png", new Drawing(25, 5) {BackgroundColor = new Color {R = 255, A = 255, B = 1}}, 25*5};
            //    yield return
            //        new object[] {"data\\red.png",  new Drawing(25, 5) { BackgroundColor = new Color { R = 255, A = 255, G = 1 } }, 25 * 5 };
            //    yield return
            //        new object[] {"data\\red.png", new Drawing(25, 5) { BackgroundColor = new Color { R = 255, A = 255, G = 1, B=1 } }, 25 * 5 * 2};


            //    yield return new object[] { "data\\green.png", new Drawing(25, 5) { BackgroundColor = new Color { G = 255, A = 255 } }, 0 };
            //    yield return
            //        new object[] { "data\\green.png", new Drawing(25, 5) { BackgroundColor = new Color { G = 255, A = 255, B = 1 } }, 25 * 5 };
            //    yield return
            //        new object[] { "data\\green.png", new Drawing(25, 5) { BackgroundColor = new Color { G = 255, A = 255, R = 1 } }, 25 * 5 };
            //    yield return
            //        new object[] { "data\\green.png", new Drawing(25, 5) { BackgroundColor = new Color { G = 255, A = 255, R = 1, B = 1 } }, 25 * 5 * 2 };


            //    yield return new object[] { "data\\blue.png", new Drawing(25, 5) { BackgroundColor = new Color { B = 255, A = 255 } }, 0 };
            //    yield return
            //        new object[] { "data\\blue.png", new Drawing(25, 5) { BackgroundColor = new Color { B = 255, A = 255, G = 1 } }, 25 * 5 };
            //    yield return
            //        new object[] { "data\\blue.png", new Drawing(25, 5) { BackgroundColor = new Color { B = 255, A = 255, R = 1 } }, 25 * 5 };
            //    yield return
            //        new object[] { "data\\blue.png", new Drawing(25, 5) { BackgroundColor = new Color { B = 255, A = 255, R = 1, G = 1 } }, 25 * 5 * 2 };
            //}
        }

        public class LoadImageData : TargetTests
        {
            [Test]
            public void should_load_image_data()
            {
                for (int width = 0; width < 25; width++)
                {
                    for (int height = 0; height < 5; height++)
                    {
                        int index = height*width + width;
                        Assert.That(Target.ImageData[index].R, Is.EqualTo(255));
                        Assert.That(Target.ImageData[index].B, Is.EqualTo(0));
                        Assert.That(Target.ImageData[index].G, Is.EqualTo(0));
                    }
                }
            }

            [Test]
            public void should_load_dimensions_from_image()
            {
                AssertOriginalValues();
            }

            private void AssertOriginalValues()
            {
                Assert.That(Target.Original.Height, Is.EqualTo(10));
                Assert.That(Target.Original.Width, Is.EqualTo(50));
            }


            [Test]
            public void should_resize_if_image_dimensions_are_over_maxDimension()
            {
                AssertOriginalValues();
                AssertComputedDimensions(5, 25);
            }

            private void AssertComputedDimensions(int expectedHeight, int expectedWidth)
            {
                Assert.That(Target.Working.Height, Is.EqualTo(expectedHeight));
                Assert.That(Target.Working.Width, Is.EqualTo(expectedWidth));
            }

            [Test]
            [TestCase(50)]
            [TestCase(100)]
            public void should_not_resize_if_image_dimensions_are_over_or_equal_to_maxDimension(int maxDimension)
            {
                Target = new Target("data\\red.png", maxDimension);
                AssertOriginalValues();
                AssertComputedDimensions(10, 50);
            }

            [Test]
            [TestCase(99, 1.0f)]
            [TestCase(50, 1.0f)]
            [TestCase(25, 0.5f)]
            [TestCase(10, 0.2f)]
            public void should_set_ratio_to_correct_value_when_loading(int maxDimension, float expectedRatio)
            {
                Target = new Target("data\\red.png", maxDimension);
                Assert.That(Target.Working.Width/(float) Target.Original.Width, Is.EqualTo(expectedRatio));
                Assert.That(Target.Working.Height/(float) Target.Original.Height, Is.EqualTo(expectedRatio));
            }
        }
    }
}