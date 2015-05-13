using System;
using imG.Approx.Components.Shapes.Factories;
using imG.Approx.Mutation;
using NSubstitute;

namespace imG.Approx.Tests.Components
{
    public class BaseIMutableTests
    {
        public BaseIMutableTests()
        {
            Process = new Process(Substitute.For<IRandomizationProvider>(),
                Substitute.For<IMutationDescriptionCatalog>(), Substitute.For<ITarget>(),
                Substitute.For<IShapeFactoryCatalog>());
            Process.Target.Working.Returns(new Target.Dimensions {Width = 321, Height = 654});
            Process.Target.DistanceTo(null, 0).ReturnsForAnyArgs(1000UL);
            Process.RandomizationProvider.Next(0, 0).ReturnsForAnyArgs(info =>
                new Random().Next((int) info[0], (int) info[1]));
        }

        public Process Process { get; set; }
    }
}