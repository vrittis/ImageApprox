using System.Collections.Generic;
using System.Linq;
using imG.Approx.Components.Shapes;
using imG.Approx.Components.Shapes.Factories;
using imG.Approx.Tests.Components.Shapes.Factories.ConcreteFactory;
using NSubstitute;
using NUnit.Framework;

namespace imG.Approx.Tests.Components.Shapes.Factories
{
    public class ShapeFactoryCatalogTests
    {
        public ShapeFactoryCatalog Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new ShapeFactoryCatalog();
        }

        protected void RegisterManyFactories(bool active)
        {
            for (int i = 1; i < 10; i++)
            {
                var factory = Substitute.For<IShapeFactory>();
                factory.IsActive.Returns(active);
                factory.Name.Returns("factory" + i);
                Target.Register(factory);
            }
        }

        public class ActiveFactories : ShapeFactoryCatalogTests
        {
            [Test]
            public void should_return_only_active_factories()
            {
                RegisterManyFactories(false);
                Target.Factories[3].IsActive.Returns(true);
                IList<IShapeFactory> activeFactories = Target.ActiveFactories();
                Assert.That(activeFactories, Has.Member(Target.Factories[3]));
                Assert.That(activeFactories, Has.No.Member(Target.Factories[2]));
            }
        }

        public class Disable : ShapeFactoryCatalogTests
        {
            [Test]
            public void should_enable_factories_named()
            {
                RegisterManyFactories(true);
                var disableFactories = new[] {"factory1", "factory2"};
                IEnumerable<string> enabledfactories =
                    Target.Factories.Select(f => f.Name).Where(s => !disableFactories.Contains(s));
                Target.Disable(disableFactories);
                Assert.That(Target.Factories.Where(f => !f.IsActive).Select(f => f.Name),
                    Is.EquivalentTo(disableFactories));
                Assert.That(Target.Factories.Where(f => f.IsActive).Select(f => f.Name),
                    Is.EquivalentTo(enabledfactories));
            }
        }

        public class DisableAll : ShapeFactoryCatalogTests
        {
            [Test]
            public void should_disable_all_factories()
            {
                RegisterManyFactories(true);
                Target.DisableAll();
                Assert.That(Target.Factories.TrueForAll(f => !f.IsActive));
            }
        }

        public class Enable : ShapeFactoryCatalogTests
        {
            [Test]
            public void should_enable_factories_named()
            {
                RegisterManyFactories(false);
                var enableFactories = new[] {"factory1", "factory2"};
                IEnumerable<string> disabledfactories =
                    Target.Factories.Select(f => f.Name).Where(s => !enableFactories.Contains(s));
                Target.Enable(enableFactories);
                Assert.That(Target.Factories.Where(f => f.IsActive).Select(f => f.Name),
                    Is.EquivalentTo(enableFactories));
                Assert.That(Target.Factories.Where(f => !f.IsActive).Select(f => f.Name),
                    Is.EquivalentTo(disabledfactories));
            }
        }

        public class EnableAll : ShapeFactoryCatalogTests
        {
            [Test]
            public void should_enable_all_factories()
            {
                RegisterManyFactories(false);
                Target.EnableAll();
                Assert.That(Target.Factories.TrueForAll(f => f.IsActive));
            }
        }

        public class Register : ShapeFactoryCatalogTests
        {
            [Test]
            public void should_add_factory()
            {
                Assert.That(Target.Factories, Is.Empty);
                Target.Register(Substitute.For<IShapeFactory>());
                Assert.That(Target.Factories.Count, Is.EqualTo(1));
            }
        }

        public class RegisterAllFactories : ShapeFactoryCatalogTests
        {
            [Test]
            public void should_register_all_factories()
            {
                var SubstShape = Substitute.For<IShape>();
                TestShapeFactory.TestShape = SubstShape;
                Target.RegisterAllFactories();
                Assert.That(Target.Factories.Count, Is.GreaterThan(0));
                IShapeFactory factory = Target.Factories.FirstOrDefault(f => f.Name == "RegisterAllFactories");
                Assert.That(factory.GetShape(null), Is.EqualTo(SubstShape));
            }
        }
    }
}