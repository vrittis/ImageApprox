using imG.Approx.Mutation;
using NUnit.Framework;

namespace imG.Approx.Tests.Mutation
{
    public class RandomizationProviderTests
    {
        public RandomizationProvider Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new RandomizationProvider(100);
        }

        public class Constructor : RandomizationProviderTests
        {
            [Test]
            public void should_keep_the_seed()
            {
                Assert.That(Target.Seed, Is.EqualTo(100));
            }
        }

        public class Next : RandomizationProviderTests
        {
            [Test]
            public void should_return_integer()
            {
                Assert.That(Target.Next(0, 10), Is.AtLeast(0).And.AtMost(9));
            }
        }
    }
}