using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using imG.Approx.Components.Shapes;
using imG.Approx.Components.Shapes.Factories;
using imG.Approx.Mutation;
using NUnit.Framework;

namespace imG.Approx.Tests.Components.Shapes.Factories.ConcreteFactory
{
    public class TestShapeFactory : ShapeFactory<Circle>
    {
        public static IShape TestShape { get; set; }

        public override string Name
        {
            get { return "RegisterAllFactories"; }
        }

        public override IShape GetShape(Process process)
        {
            return TestShape;
        }
    }

    [ExcludeFromCodeCoverage]
    public class SpecialShape : IShape
    {
        public bool Initialized = false;

        public SpecialShape()
        {
            Initialized = false;
        }

        public IShape Clone()
        {
            return new SpecialShape();
        }

        public IMutable[] MutableComponents
        {
            get { return new IMutable[0]; }
        }

        public void Draw(Graphics g)
        {
        }

        public void InitializeComponents(Process process)
        {
            Initialized = true;
        }
    }

    public class SpecialShapeFactory : ShapeFactory<SpecialShape>
    {
    }

    public class ShapeFactoryTests
    {
        public IShapeFactory Target;

        [SetUp]
        public void before_each_test()
        {
            Target = new SpecialShapeFactory();
        }

        public class GetShape : ShapeFactoryTests
        {
            [Test]
            public void should_return_shape()
            {
                IShape result = Target.GetShape(null);
                Assert.That((result as SpecialShape).Initialized, Is.True);
            }
        }

        public class Name : ShapeFactoryTests
        {
            [Test]
            public void should_return_name_by_default()
            {
                Assert.That(Target.Name, Is.EqualTo("SpecialShape"));
            }
        }
    }
}