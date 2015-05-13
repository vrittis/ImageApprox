using System;
using imG.Approx.Components.Shapes.Factories;
using imG.Approx.Mutation;

namespace imG.Approx
{
    public class ProcessFactory
    {
        public ProcessFactory(string filePath)
            : this(filePath, new Random().Next())
        {
        }

        public ProcessFactory(string filePath, int seed)
        {
            TargetFactory = () => new Target(filePath, 100);
            MutationDescriptionCatalogFactory = () =>
            {
                var catalog = new MutationDescriptionCatalog();
                catalog.RegisterAllMutations();
                return catalog;
            };
            ShapeCatalogFactory = () =>
            {
                var catalog = new ShapeFactoryCatalog();
                catalog.RegisterAllFactories();
                return catalog;
            };
            RandomizationProviderFactory = () => new RandomizationProvider(seed);
        }

        public Func<ITarget> TargetFactory { get; set; }
        public Func<IMutationDescriptionCatalog> MutationDescriptionCatalogFactory { get; set; }
        public Func<IShapeFactoryCatalog> ShapeCatalogFactory { get; set; }
        public Func<IRandomizationProvider> RandomizationProviderFactory { get; set; }

        public Process Build()
        {
            return new Process(RandomizationProviderFactory(), MutationDescriptionCatalogFactory(), TargetFactory(),
                ShapeCatalogFactory());
        }
    }
}