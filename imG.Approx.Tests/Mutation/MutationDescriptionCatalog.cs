using System;
using System.Collections;
using System.Linq;
using imG.Approx.Mutation;
using imG.Approx.Tests.Mutation.MutableAndDescription;
using NUnit.Framework;

namespace imG.Approx.Tests.Mutation
{
    public class MutationDescriptionCatalogTest
    {
        public MutationDescriptionCatalog Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new MutationDescriptionCatalog();
        }

        public class DeclareMutation : MutationDescriptionCatalogTest
        {
            [Test]
            public void should_add_description_to_catalog()
            {
                var description = new MutationDescription<Mutable1>("M1", 10, (context, mutable1) => { });
                Assert.That(Target.Descriptions.Count(), Is.EqualTo(0));
                Target.Register(description);
                Assert.That(Target.Descriptions.Count(), Is.EqualTo(1));
                Assert.That(Target.Descriptions[typeof (Mutable1)], Has.Member(description));
            }

            [Test]
            public void should_throw_when_the_same_description_is_declared_twice()
            {
                var description = new MutationDescription<Mutable1>("M1", 10, (context, mutable1) => { });
                Target.Register(description);
                Assert.That(() => Target.Register(description), Throws.InstanceOf<InvalidOperationException>());
            }
        }

        public class For : MutationDescriptionCatalogTest
        {
            [Test]
            public void should_return_empty_list_for_unknown_mutable_type()
            {
                var mutable = new Mutable1();
                Assert.That(Target.For(mutable), Is.Empty);
                Assert.That(Target.For(mutable), Is.Not.Null);
            }

            [Test]
            [TestCaseSource("should_return_list_of_descriptions_for_type_source")]
            public void should_return_list_of_descriptions_for_type(IMutable mutable, IMutationDescription desc1,
                IMutationDescription desc2)
            {
                Target.Register(desc1);
                Target.Register(desc2);
                Assert.That(Target.For(mutable), Is.Not.Empty);
                Assert.That(Target.For(mutable).Count(), Is.EqualTo(1));
                Assert.That(Target.For(mutable), Has.Member(desc1));
            }

            public IEnumerable should_return_list_of_descriptions_for_type_source()
            {
                yield return
                    new object[]
                    {
                        new Mutable1(), new MutationDescription<Mutable1>("M1", 10, (context, mutable) => { }),
                        new MutationDescription<Mutable2>("M2", 10, (context, mutable) => { })
                    };
                yield return
                    new object[]
                    {
                        new Mutable2(), new MutationDescription<Mutable2>("M2", 10, (context, mutable) => { }),
                        new MutationDescription<Mutable3>("M3", 10, (context, mutable) => { })
                    };
                yield return
                    new object[]
                    {
                        new Mutable3(), new MutationDescription<Mutable3>("M3", 10, (context, mutable) => { }),
                        new MutationDescription<Mutable1>("M1", 10, (context, mutable) => { })
                    };
            }
        }

        public class RegisterAllMutations : MutationDescriptionCatalogTest
        {
            [Test]
            public void should_register_all_mutations_declared_by_registrars()
            {
                Target.RegisterAllMutations();
                Assert.That(Target.For(new Mutable1()).Count, Is.EqualTo(1));
                Assert.That(Target.For(new Mutable2()).Count, Is.EqualTo(1));
                Assert.That(Target.For(new Mutable3()).Count, Is.EqualTo(1));
            }
        }
    }
}