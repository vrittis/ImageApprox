using System.Collections.Generic;
using imG.Approx.Components.BuildingBlocks;
using imG.Approx.Components.Shapes;

namespace imG.Approx.Mutation.Registrars
{
    public class PolygonMutationDescriptionRegistrar : IMutationDescriptionRegistrar
    {
        public IEnumerable<IMutationDescription> DeclareMutations()
        {
            return new IMutationDescription[]
            {
                new MutationDescription<Polygon>("Remove point", 1,
                    (process, polygon) =>
                        polygon.Points.RemoveAt(process.RandomizationProvider.Next(0, polygon.Points.Count)),
                    (process, polygon) => polygon.Points.Count > 3),
                new MutationDescription<Polygon>("Add point", 1, (process, polygon) =>
                {
                    int insertAfterIndex = process.RandomizationProvider.Next(0, polygon.Points.Count - 1);

                    Position previousPoint = polygon.Points[insertAfterIndex];
                    Position nextPoint = polygon.Points[insertAfterIndex + 1];

                    var newPoint = new Position
                    {
                        X = (previousPoint.X + nextPoint.X)/2,
                        Y = (previousPoint.Y + nextPoint.Y)/2
                    };

                    polygon.Points.Insert(insertAfterIndex + 1, newPoint);
                },
                (process, polygon) => polygon.Points.Count < 30)
            };
        }
    }
}