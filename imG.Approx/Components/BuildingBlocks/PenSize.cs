using imG.Approx.Mutation;
using imG.Approx.Tools;

namespace imG.Approx.Components.BuildingBlocks
{
    public class PenSize : IMutable<PenSize>
    {
        internal int Size;

        internal PenSize()
        {
            Size = 1;
        }

        public IMutable[] MutableComponents
        {
            get { return new IMutable[0]; }
        }

        public PenSize Clone()
        {
            return new PenSize {Size = Size};
        }

        public void RandomizeValues(Process process)
        {
            Size = process.RandomizationProvider.Next(1, 20);
        }

        public void Nudge(Process process, int nudgeUppedLimit)
        {
            int nudgeAmount = process.RandomizationProvider.Next(-nudgeUppedLimit, nudgeUppedLimit + 1);
            Size = (Size + nudgeAmount).Clamp(1, 16);
        }
    }
}