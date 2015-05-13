namespace imG.Approx.Mutation
{
    public interface IMutable
    {
        IMutable[] MutableComponents { get; }
    }

    public interface IMutable<T> : IMutable where T : IMutable<T>
    {
        T Clone();
    }
}