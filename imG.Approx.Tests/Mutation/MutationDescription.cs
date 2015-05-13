using System;
using imG.Approx.Components.Shapes.Factories;
using imG.Approx.Mutation;
using imG.Approx.Tests.Mutation.MutableAndDescription;
using NSubstitute;
using NUnit.Framework;

namespace imG.Approx.Tests.Mutation
{
    public class MutationDescriptionTests
    {
        private bool CheckedWhetherMutationCanOccur;
        protected bool MutationCanOccur;
        private bool MutationOccured;
        public IMutationDescription Target { get; set; }
        public Process Process { get; set; }

        [SetUp]
        public void before_each_test()
        {
            var randomization = Substitute.For<IRandomizationProvider>();
            randomization.Next(Arg.Any<int>(), 10).Returns(0);
            Process = new Process(randomization, Substitute.For<IMutationDescriptionCatalog>(),
                Substitute.For<ITarget>(), Substitute.For<IShapeFactoryCatalog>());

            MutationOccured = false;
            CheckedWhetherMutationCanOccur = false;
            Target = new MutationDescription<Mutable1>("M1", 10, (context, mutable1) => { MutationOccured = true; },
                (process, mutable1) =>
                {
                    CheckedWhetherMutationCanOccur = true;
                    return MutationCanOccur;
                });
        }

        public class CanMutate : MutationDescriptionTests
        {
            [Test]
            public void lambda_is_called_when_checking()
            {
                Assert.That(CheckedWhetherMutationCanOccur, Is.False);
                bool result = Target.CanMutate(Process, Substitute.For<IMutable>());
                Assert.That(CheckedWhetherMutationCanOccur, Is.True);
                Assert.That(result, Is.EqualTo(MutationCanOccur));
            }
        }

        public class Constructor : MutationDescriptionTests
        {
            [Test]
            [TestCase(0)]
            [TestCase(-1)]
            [TestCase(-1000)]
            public void should_refuse_odds_that_are_not_positive(int odds)
            {
                Assert.That(() => new MutationDescription<Mutable1>("M1", odds, (context, mutable1) => { }),
                    Throws.InstanceOf<ArgumentOutOfRangeException>());
            }
        }

        public class GetMutationTargetType : MutationDescriptionTests
        {
            [Test]
            public void should_return_type_of_generic()
            {
                Assert.That(Target.GetMutationTargetType(), Is.EqualTo(typeof (Mutable1)));
            }
        }

        public class Mutate : MutationDescriptionTests
        {
            [Test]
            public void lambda_is_called_when_mutating()
            {
                Assert.That(MutationOccured, Is.False);
                Target.Mutate(Process, Substitute.For<IMutable>());
                Assert.That(MutationOccured, Is.True);
            }
        }
    }
}