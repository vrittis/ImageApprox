using System.Collections.Generic;

namespace imG.Approx.Mutation
{
    public interface IMutationDescriptionRegistrar
    {
        IEnumerable<IMutationDescription> DeclareMutations();
    }
}