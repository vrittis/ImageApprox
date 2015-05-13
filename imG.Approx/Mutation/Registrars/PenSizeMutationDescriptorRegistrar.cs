using System.Collections.Generic;
using imG.Approx.Components.BuildingBlocks;

namespace imG.Approx.Mutation.Registrars
{
    public class PenSizeMutationDescriptorRegistrar : IMutationDescriptionRegistrar
    {
        public IEnumerable<IMutationDescription> DeclareMutations()
        {
            return new IMutationDescription[]
            {
                new MutationDescription<PenSize>("Randomize", 1, (process, size) => size.RandomizeValues(process)),
                new MutationDescription<PenSize>("Middle nudge", 1, (process, size) => size.Nudge(process, 20)),
                new MutationDescription<PenSize>("Small nudge", 2, (process, size) => size.Nudge(process, 10))
            };
        }
    }
}