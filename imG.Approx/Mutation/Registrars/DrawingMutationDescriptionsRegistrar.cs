using System.Collections.Generic;
using System.Linq;
using imG.Approx.Components;
using imG.Approx.Components.Shapes;
using imG.Approx.Components.Shapes.Factories;

namespace imG.Approx.Mutation.Registrars
{
    public class DrawingMutationDescriptionsRegistrar : IMutationDescriptionRegistrar
    {
        public IEnumerable<IMutationDescription> DeclareMutations()
        {
            return new IMutationDescription[]
            {
                new MutationDescription<Drawing>("Add shape", 1, (process, drawing) =>
                {
                    IShapeFactory factory =
                        process.ShapeFactoryCatalog.ActiveFactories()[
                            process.RandomizationProvider.Next(0, process.ShapeFactoryCatalog.ActiveFactories().Count)];
                    drawing.Shapes.Add(factory.GetShape(process));
                },
                (process, drawing) => { return drawing.Shapes.Count < 60; }),
                new MutationDescription<Drawing>("Remove shape", 1,
                    (process, drawing) =>
                        drawing.Shapes.RemoveAt(process.RandomizationProvider.Next(0, drawing.Shapes.Count)),
                    (process, drawing) => drawing.Shapes.Any()),
                new MutationDescription<Drawing>("Move shapes", 1, (process, drawing) =>
                {
                    int from = process.RandomizationProvider.Next(0, drawing.Shapes.Count - 1);
                    int to = process.RandomizationProvider.Next(from + 1, drawing.Shapes.Count);
                    IShape movingShape = drawing.Shapes[from];
                    drawing.Shapes[from] = drawing.Shapes[to];
                    drawing.Shapes[to] = movingShape;
                }, (process, drawing) => drawing.Shapes.Count > 1)
            };
        }
    }
}