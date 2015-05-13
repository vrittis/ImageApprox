using System.Collections.Generic;
using imG.Approx.Mutation;
using imG.Approx.Tests.Mutation.MutableAndDescription;

namespace imG.Approx.Tests.Mutation.Registrars
{
    public class MutationDescriptionRegistrar : IMutationDescriptionRegistrar
    {
        public IEnumerable<IMutationDescription> DeclareMutations()
        {
            return new IMutationDescription[]
            {
                new MutationDescription<Mutable1>("M1", 10, (context, mutable1) => { })
                , new MutationDescription<Mutable2>("M2", 10, (context, mutable1) => { })
                , new MutationDescription<Mutable3>("M3", 10, (context, mutable1) => { })
            };
        }
    }
}