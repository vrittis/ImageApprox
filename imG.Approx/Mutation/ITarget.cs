using imG.Approx.Components;

namespace imG.Approx.Mutation
{
    public interface ITarget
    {
        string Name { get; }
        Target.Dimensions Original { get; }
        Target.Dimensions Working { get; }
        ulong DistanceTo(Drawing drawing, ulong currentDistanceToTarget);
    }
}