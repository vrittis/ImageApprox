using System.Collections.Generic;
using imG.Approx.Components.BuildingBlocks;

namespace imG.Approx.Mutation.Registrars
{
    public class AngleMutationDescriptorRegistrar : IMutationDescriptionRegistrar
    {
        public IEnumerable<IMutationDescription> DeclareMutations()
        {
            return new IMutationDescription[]
            {
                new MutationDescription<Angle>("Full random", 1, (process, angle) => angle.RandomizeValues(process)),
                new MutationDescription<Angle>("Middle move", 1, (process, angle) => angle.Nudge(process, 20)),
                new MutationDescription<Angle>("Small move", 2, (process, angle) => angle.Nudge(process, 3))
            };
        }
    }
}