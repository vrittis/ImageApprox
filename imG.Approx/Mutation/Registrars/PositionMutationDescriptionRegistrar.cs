using System.Collections.Generic;
using imG.Approx.Components.BuildingBlocks;

namespace imG.Approx.Mutation.Registrars
{
    public class PositionMutationDescriptionRegistrar : IMutationDescriptionRegistrar
    {
        public IEnumerable<IMutationDescription> DeclareMutations()
        {
            return new IMutationDescription[]
            {
                new MutationDescription<Position>("Randomisation", 1,
                    (process, position) => position.RandomizeValues(process)),
                new MutationDescription<Position>("Déplacement moyen", 1,
                    (process, position) => position.Nudge(process, 20)),
                new MutationDescription<Position>("Déplacement léger", 2,
                    (process, position) => position.Nudge(process, 3))
            };
        }
    }
}