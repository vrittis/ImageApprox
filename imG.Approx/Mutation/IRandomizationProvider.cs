namespace imG.Approx.Mutation
{
    public interface IRandomizationProvider
    {
        int Seed { get; }
        int Next(int lowerBound, int upperBound);
    }
}