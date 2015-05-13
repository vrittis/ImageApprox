using System.Collections.Generic;
using imG.Approx.Components.BuildingBlocks;

namespace imG.Approx.Mutation.Registrars
{
    public class AmountMutationDescriptionsRegistrar : IMutationDescriptionRegistrar
    {
        public IEnumerable<IMutationDescription> DeclareMutations()
        {
            return new IMutationDescription[]
            {
                new MutationDescription<Amount>("Full random", 1, (process, amount) => amount.RandomizeValues(process)),
                new MutationDescription<Amount>("Middle move", 1, (process, amount) => amount.Nudge(process, 20)),
                new MutationDescription<Amount>("Small move", 2, (process, amount) => amount.Nudge(process, 3))
            };
        }
    }
}