using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using imG.Approx.Components.BuildingBlocks;
using imG.Approx.Components.Shapes;

namespace imG.Approx.Mutation.Registrars
{
    public class BlobMutationDescriptionsRegistrar : IMutationDescriptionRegistrar
    {
        public IEnumerable<IMutationDescription> DeclareMutations()
        {
            return new IMutationDescription[]
            {
                new MutationDescription<Blob>("Remove point", 1,
                    (process, blob) =>
                        blob.Points.RemoveAt(process.RandomizationProvider.Next(0, blob.Points.Count)),
                    (process, blob) => blob.Points.Count > 3),
                new MutationDescription<Blob>("Add point", 1, (process, blob) =>
                {
                    int insertAfterIndex = process.RandomizationProvider.Next(0, blob.Points.Count - 1);

                    Position previousPoint = blob.Points[insertAfterIndex];
                    Position nextPoint = blob.Points[insertAfterIndex + 1];

                    var newPoint = new Position
                    {
                        X = (previousPoint.X + nextPoint.X)/2,
                        Y = (previousPoint.Y + nextPoint.Y)/2
                    };

                    blob.Points.Insert(insertAfterIndex + 1, newPoint);
                },
                (process, blob) => blob.Points.Count < 6)
            };
        }
    }
}
