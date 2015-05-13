using System;

namespace imG.Approx.Mutation
{
    public interface IMutationDescription
    {
        bool Active { get; set; }
        int OccasionsToOccur { get; set; }
        bool CanMutate(Process process, IMutable target);
        void Mutate(Process process, IMutable target);
        Type GetMutationTargetType();
    }
}