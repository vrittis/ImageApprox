using imG.Approx.Components.BuildingBlocks;
using NSubstitute;
using NUnit.Framework;

namespace imG.Approx.Tests.Components.BuildingBlocks
{
    public class ColorTests : BaseIMutableTests
    {
        public Color Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new Color();
        }

        public class Clone : ColorTests
        {
            [Test]
            public void should_clone_component()
            {
                Assert.That(Target.Clone(), Is.Not.SameAs(Target));
            }

            [Test]
            public void should_copy_values()
            {
                Color clone = Target.Clone();
                Assert.That(clone.R, Is.EqualTo(Target.R));
                Assert.That(clone.G, Is.EqualTo(Target.G));
                Assert.That(clone.B, Is.EqualTo(Target.B));
                Assert.That(clone.A, Is.EqualTo(Target.A));
            }
        }

        public class Constructor : ColorTests
        {
            [Test]
            public void should_initialize_data()
            {
                Assert.That(Target.R, Is.EqualTo(0));
                Assert.That(Target.G, Is.EqualTo(0));
                Assert.That(Target.B, Is.EqualTo(0));
            }
        }

        public class ImplicitConversionToDrawingColor : ColorTests
        {
            [Test]
            public void should_convert_to_drawing_color()
            {
                Target.R = 100;
                Target.G = 101;
                Target.B = 102;
                Target.A = 103;

                System.Drawing.Color color = Target;
                Assert.That(color.R, Is.EqualTo(Target.R));
                Assert.That(color.G, Is.EqualTo(Target.G));
                Assert.That(color.B, Is.EqualTo(Target.B));
                Assert.That(color.A, Is.EqualTo(Target.A));
            }
        }

        public class MutableComponents : ColorTests
        {
            [Test]
            public void should_not_contain_anything()
            {
                Assert.That(Target.MutableComponents, Is.Empty);
            }
        }

        public class RandomizeAlpha : ColorTests
        {
            [Test]
            public void should_randomize_alpha_in_range()
            {
                Process.RandomizationProvider.Next(60, 200).Returns(66);
                Target.RandomizeAlpha(Process);
                Assert.That(Target.A, Is.EqualTo(66));
            }
        }

        public class RandomizeBlue : ColorTests
        {
            [Test]
            public void should_randomize_alpha_in_range()
            {
                Process.RandomizationProvider.Next(0, 256).Returns(66);
                Target.RandomizeBlue(Process);
                Assert.That(Target.B, Is.EqualTo(66));
            }
        }

        public class RandomizeGreen : ColorTests
        {
            [Test]
            public void should_randomize_alpha_in_range()
            {
                Process.RandomizationProvider.Next(0, 256).Returns(66);
                Target.RandomizeGreen(Process);
                Assert.That(Target.G, Is.EqualTo(66));
            }
        }

        public class RandomizeRed : ColorTests
        {
            [Test]
            public void should_randomize_alpha_in_range()
            {
                Process.RandomizationProvider.Next(0, 256).Returns(66);
                Target.RandomizeRed(Process);
                Assert.That(Target.R, Is.EqualTo(66));
            }
        }

        public class RandomizeValues : ColorTests
        {
            [Test]
            public void should_randomize_colors_in_the_range()
            {
                Process.RandomizationProvider.Next(0, 256).Returns(20, 30, 40);
                Process.RandomizationProvider.Next(60, 200).Returns(10);
                Target.RandomizeValues(Process);
                Assert.That(Target.A, Is.EqualTo(10));
                Assert.That(Target.R, Is.EqualTo(20));
                Assert.That(Target.G, Is.EqualTo(30));
                Assert.That(Target.B, Is.EqualTo(40));
            }
        }
    }
}