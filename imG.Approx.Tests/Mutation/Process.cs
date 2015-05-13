using System;
using System.Collections;
using System.Collections.Generic;
using imG.Approx.Components;
using imG.Approx.Components.Shapes.Factories;
using imG.Approx.Mutation;
using NSubstitute;
using NUnit.Framework;

namespace imG.Approx.Tests.Mutation
{
    public class ProcessTests
    {
        public Process Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            var drawingTarget = Substitute.For<ITarget>();
            drawingTarget.Working.Returns(new Target.Dimensions {Width = 1000, Height = 456});
            drawingTarget.DistanceTo(Arg.Any<Drawing>(), Arg.Any<ulong>()).Returns(105UL);

            Target = new Process(Substitute.For<IRandomizationProvider>(), Substitute.For<IMutationDescriptionCatalog>(),
                drawingTarget, Substitute.For<IShapeFactoryCatalog>());
        }

        public class Constructor : ProcessTests
        {
            [Test]
            [TestCaseSource("should_throw_if_any_argument_is_null_source")]
            public void should_throw_if_any_argument_is_null(IRandomizationProvider randomProvider,
                IMutationDescriptionCatalog catalog, ITarget target, IShapeFactoryCatalog shapeFactoryCatalog)
            {
                Assert.That(() => { Target = new Process(randomProvider, catalog, target, shapeFactoryCatalog); },
                    Throws.InstanceOf<ArgumentNullException>());
            }

            public IEnumerable should_throw_if_any_argument_is_null_source()
            {
                yield return
                    new object[]
                    {
                        null, Substitute.For<IMutationDescriptionCatalog>(), Substitute.For<ITarget>(),
                        Substitute.For<IShapeFactoryCatalog>()
                    };
                yield return
                    new object[]
                    {
                        Substitute.For<IRandomizationProvider>(), null, Substitute.For<ITarget>(),
                        Substitute.For<IShapeFactoryCatalog>()
                    };
                yield return
                    new object[]
                    {
                        Substitute.For<IRandomizationProvider>(), Substitute.For<IMutationDescriptionCatalog>(), null,
                        Substitute.For<IShapeFactoryCatalog>()
                    };
                yield return
                    new object[]
                    {
                        Substitute.For<IRandomizationProvider>(), Substitute.For<IMutationDescriptionCatalog>(),
                        Substitute.For<ITarget>(), null
                    };
            }
        }

        public class Mutate : ProcessTests
        {
            private bool MutationOccured;
            private bool NewGenerationOccured;

            [Test]
            [TestCase(true, Description = "Evolution positive")]
            [TestCase(false, Description = "Evolution négative")]
            public void should_always_keep_best_drawing_according_to_distance(bool secondDrawingIsBetter)
            {
                Target.Target.DistanceTo(Arg.Any<Drawing>(), Arg.Any<ulong>()).Returns(1000UL, secondDrawingIsBetter ? 500UL : 2000UL);
                Target.MutationDescriptionCatalog.For(null).ReturnsForAnyArgs(new List<IMutationDescription>());
                Drawing firstDrawing = Target.Drawing;
                Target.Mutate();
                Assert.That(firstDrawing, (secondDrawingIsBetter ? Is.Not : Is.Not.Not).SameAs(Target.Drawing));
            }


            private void run_mutation(ulong secondValue)
            {
                MutationOccured = false;
                NewGenerationOccured = false;
                Target.Target.DistanceTo(Arg.Any<Drawing>(), Arg.Any<ulong>()).Returns(1000UL, secondValue);
                Target.MutationDescriptionCatalog.For(null).ReturnsForAnyArgs(new List<IMutationDescription>());
                Target.BetterFitnessFound += p => MutationOccured = true;
                Target.NewGenerationOccured += process => NewGenerationOccured = true;
                Target.Mutate();
            }

            [Test]
            public void should_trigger_event_when_drawing_is_better()
            {
                run_mutation(500);
                Assert.That(MutationOccured, Is.True);
                Assert.That(NewGenerationOccured, Is.True);
            }


            [Test]
            public void should_trigger_event_when_drawing_is_worse()
            {
                run_mutation(5000);
                Assert.That(MutationOccured, Is.False);
                Assert.That(NewGenerationOccured, Is.True);
            }

            [Test]
            public void should_increase_generation_number()
            {
                Target.Target.DistanceTo(Arg.Any<Drawing>(), Arg.Any<ulong>()).Returns(1000UL, 500UL);
                Target.MutationDescriptionCatalog.For(null).ReturnsForAnyArgs(new List<IMutationDescription>());
                Target.Mutate();
                Assert.That(Target.Generation, Is.EqualTo(1));
            }

            [Test]
            public void should_increase_evolutions_when_drawing_is_better()
            {
                Target.Evolutions = 0;
                run_mutation(500);
                Assert.That(Target.Evolutions, Is.EqualTo(1));
            }
        }

        public class SetupDrawing : ProcessTests
        {
            [Test]
            public void should_create_drawing_based_on_target()
            {
                Target.InitializeDrawingAndDistance();
                Assert.That(Target.Drawing, Is.Not.Null);
                Assert.That(Target.Drawing.Width, Is.EqualTo(Target.Target.Working.Width));
                Assert.That(Target.Drawing.Height, Is.EqualTo(Target.Target.Working.Height));
                Assert.That(Target.DistanceToTarget, Is.EqualTo(105));
            }

            [Test]
            public void should_create_the_drawing_only_the_first_time()
            {
                Drawing firstDrawing = Target.Drawing;
                Target.InitializeDrawingAndDistance();
                Assert.That(Target.Drawing, Is.SameAs(firstDrawing));
            }


            [Test]
            public void should_compute_the_distance_only_the_first_time()
            {
                ulong firstDistance = Target.DistanceToTarget;
                Target.InitializeDrawingAndDistance();
                Assert.That(Target.DistanceToTarget, Is.EqualTo(firstDistance));
            }
        }
    }
}