using System.Diagnostics.CodeAnalysis;
using imG.Approx.Mutation;

namespace imG.Approx.Tests.Mutation.MutableAndDescription
{
    [ExcludeFromCodeCoverage]
    public class Mutable1 : IMutable
    {
        public IMutable[] MutableComponents
        {
            get { return new IMutable[1] {new Mutable2()}; }
        }
    }

    [ExcludeFromCodeCoverage]
    public class Mutable2 : IMutable
    {
        public IMutable[] MutableComponents
        {
            get { return new IMutable[1] {new Mutable3()}; }
        }
    }

    [ExcludeFromCodeCoverage]
    public class Mutable3 : IMutable
    {
        public IMutable[] MutableComponents
        {
            get { return new IMutable[0]; }
        }
    }
}