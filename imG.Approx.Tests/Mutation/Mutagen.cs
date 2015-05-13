using System;
using System.Collections;
using System.Collections.Generic;
using imG.Approx.Components.Shapes.Factories;
using imG.Approx.Mutation;
using NSubstitute;
using NUnit.Framework;

namespace imG.Approx.Tests.Mutation
{
    public class MutagenTests
    {
        public Process Process { get; set; }
        public IMutable Mutable { get; set; }
        public IMutationDescriptionCatalog Catalog { get; set; }
        public IRandomizationProvider Randomization { get; set; }

        public Mutagen Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Randomization = Substitute.For<IRandomizationProvider>();
            Mutable = Substitute.For<IMutable>();
            Catalog = Substitute.For<IMutationDescriptionCatalog>();
            Catalog.For(Mutable).Returns(new List<IMutationDescription>());
            Process = new Process(Randomization, Catalog, Substitute.For<ITarget>(),
                Substitute.For<IShapeFactoryCatalog>());

            Target = new Mutagen();
        }


        private static IMutationDescription CreateMutationDescription(bool activeMutation, bool applicableMutation,
            int chancesToActivate)
        {
            var activeMutationDescription = Substitute.For<IMutationDescription>();
            activeMutationDescription.Active.Returns(activeMutation);
            activeMutationDescription.CanMutate(null, null).ReturnsForAnyArgs(applicableMutation);
            activeMutationDescription.OccasionsToOccur.Returns(chancesToActivate);
            return activeMutationDescription;
        }

        public class ChooseMutation : MutagenTests
        {
            [Test]
            public void should_return_null_if_no_mutation_exists()
            {
                var source = new Dictionary<IMutationDescription, List<IMutable>>();
                Assert.That(Target.ChooseMutation(source, Randomization), Is.Null);
            }

            [Test]
            [TestCaseSource("should_return_mutation_description_determined_by_random_provider_source")]
            public void should_return_mutation_description_determined_by_random_provider(
                Dictionary<IMutationDescription, List<IMutable>> source, IMutationDescription expectedDescription,
                IMutable expectedMutable, int val1, int val2)
            {
                var test = Substitute.For<IRandomizationProvider>();
                test.Next(0, 1).ReturnsForAnyArgs(val1, val2);

                Mutagen.SelectedMutation selected = Target.ChooseMutation(source, test);

                Assert.That(selected.Description, Is.EqualTo(expectedDescription));
                Assert.That(selected.Target, Is.EqualTo(expectedMutable));
            }

            public IEnumerable should_return_mutation_description_determined_by_random_provider_source()
            {
                var source = new Dictionary<IMutationDescription, List<IMutable>>();

                var mutationDescription1 = Substitute.For<IMutationDescription>();
                mutationDescription1.OccasionsToOccur.Returns(7);
                var mutable11 = Substitute.For<IMutable>();
                var mutable12 = Substitute.For<IMutable>();
                source.Add(mutationDescription1, new List<IMutable> {mutable11, mutable12});
                var mutationDescription2 = Substitute.For<IMutationDescription>();
                mutationDescription2.OccasionsToOccur.Returns(1);
                var mutable21 = Substitute.For<IMutable>();
                var mutable22 = Substitute.For<IMutable>();
                source.Add(mutationDescription2, new List<IMutable> {mutable21, mutable22});

                yield return
                    new object[] 
                    {source, mutationDescription1, mutable11, 0, 0};

                yield return
                    new object[] 
                    {source, mutationDescription1, mutable11, 6, 0};


                yield return
                    new object[]
                    {source, mutationDescription1, mutable12, 0, 1};

                yield return
                    new object[] 
                    {source, mutationDescription1, mutable12, 6, 1};
                
                yield return
                    new object[]
                    {source, mutationDescription2, mutable21, 7, 0};

                yield return
                    new object[]
                    {source, mutationDescription2, mutable22, 7, 1};
            }
        }

        public class GetMutationsFor : MutagenTests
        {
            [Test]
            public void should_return_empty_if_mutable_is_unknown()
            {
                Assert.That(Target.GetMutationsFor(Process, Mutable), Is.Empty);
            }

            [Test]
            public void should_return_active_and_applicable_and_selectable_mutations()
            {
                IMutationDescription activeMutationDescription = CreateMutationDescription(true, true, 1);
                IMutationDescription inactiveMutationDescription = CreateMutationDescription(false, true, 1);
                IMutationDescription immutableMutationDescription = CreateMutationDescription(true, false, 1);
                IMutationDescription withoutChancesMutationDescription = CreateMutationDescription(true, true, 0);
                Catalog.For(Mutable)
                    .Returns(new List<IMutationDescription>
                    {
                        activeMutationDescription,
                        inactiveMutationDescription,
                        immutableMutationDescription,
                        withoutChancesMutationDescription
                    });

                IDictionary<IMutationDescription, List<IMutable>> result = Target.GetMutationsFor(Process, Mutable);
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result.ContainsKey(activeMutationDescription));
                Assert.That(result[activeMutationDescription].Count, Is.EqualTo(1));
                Assert.That(result[activeMutationDescription], Contains.Item(Mutable));
            }


            [Test]
            public void should_return_mutations_recursively()
            {
                IMutationDescription activeMutationDescription1 = CreateMutationDescription(true, true, 1);
                IMutationDescription activeMutationDescription2 = CreateMutationDescription(true, true, 1);
                var secondMutable = Substitute.For<IMutable>();

                Catalog.For(Mutable).Returns(new List<IMutationDescription> {activeMutationDescription1});
                Mutable.MutableComponents.Returns(new[] {secondMutable});
                Catalog.For(secondMutable).Returns(new List<IMutationDescription> {activeMutationDescription2});

                IDictionary<IMutationDescription, List<IMutable>> result = Target.GetMutationsFor(Process, Mutable);
                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result[activeMutationDescription1].Count, Is.EqualTo(1));
                Assert.That(result[activeMutationDescription1], Contains.Item(Mutable));
                Assert.That(result[activeMutationDescription2].Count, Is.EqualTo(1));
                Assert.That(result[activeMutationDescription2], Contains.Item(secondMutable));
            }
        }

        public class NoOpDescription : MutagenTests
        {
            [Test]
            public void should_be_always_active()
            {
                var desc = new Mutagen.NoOpMutationDescription();
                desc.Active = false;
                Assert.That(desc.Active, Is.True);
            }

            [Test]
            public void should_be_always_able_to_mutate()
            {
                var desc = new Mutagen.NoOpMutationDescription();
                Assert.That(desc.CanMutate(null, null), Is.True);
            }

            [Test]
            public void should_always_have_occasions_to_mutate()
            {
                var desc = new Mutagen.NoOpMutationDescription();
                desc.OccasionsToOccur = 0;
                Assert.That(desc.OccasionsToOccur, Is.GreaterThan(0));
            }

            [Test]
            public void should_always_target_IMutableType()
            {
                var desc = new Mutagen.NoOpMutationDescription();
                Assert.That(desc.GetMutationTargetType(), Is.EqualTo(typeof (IMutable)));
            }

            [Test]
            public void should_always_mutate_without_doing_anything_to_the_target()
            {
                var desc = new Mutagen.NoOpMutationDescription();
                desc.Mutate(null, null);
            }
        }

        public class SelectMutation : MutagenTests
        {
            [Test]
            [TestCaseSource("should_throw_if_any_component_is_null_source")]
            public void should_throw_if_any_component_is_null(Process process, IMutable mutable)
            {
                Assert.That(() => Target.SelectMutation(process, mutable), Throws.InstanceOf<ArgumentNullException>());
            }

            public IEnumerable should_throw_if_any_component_is_null_source()
            {
                yield return new object[] {null, Substitute.For<IMutable>()};
                yield return
                    new object[]
                    {
                        new Process(Substitute.For<IRandomizationProvider>(),
                            Substitute.For<IMutationDescriptionCatalog>(), Substitute.For<ITarget>(),
                            Substitute.For<IShapeFactoryCatalog>()),
                        null
                    };
            }

            [Test]
            public void should_return_a_mutation()
            {
                Assert.That(Target.SelectMutation(Process, Mutable), Is.Not.Null);
            }

            [Test]
            public void should_return_the_default_mutation_if_no_mutation_exists()
            {
                Assert.That(Target.SelectMutation(Process, Mutable).Description,
                    Is.TypeOf<Mutagen.NoOpMutationDescription>());
            }

            [Test]
            public void should_return_matching_selected_mutation()
            {
                IMutationDescription mutationDescription = CreateMutationDescription(true, true, 1);
                Catalog.For(Mutable).Returns(new List<IMutationDescription> {mutationDescription});
                Mutagen.SelectedMutation result = Target.SelectMutation(Process, Mutable);

                Assert.That(result.Target, Is.EqualTo(Mutable));
                Assert.That(result.Description, Is.EqualTo(mutationDescription));
            }
        }
    }
}