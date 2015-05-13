using System.Collections.Generic;

namespace imG.Approx.Mutation
{
    public interface IMutationDescriptionCatalog
    {
        void Register(IMutationDescription mutationDescription);
        List<IMutationDescription> For(IMutable mutable);
    }
}