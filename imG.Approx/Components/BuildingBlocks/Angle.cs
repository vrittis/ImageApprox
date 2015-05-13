using imG.Approx.Mutation;
using imG.Approx.Tools;

namespace imG.Approx.Components.BuildingBlocks
{
    public class Angle : IMutable<Angle>
    {
        public int Value;

        public Angle()
        {
        }

        internal Angle(Angle source)
        {
            Value = source.Value;
        }

        public IMutable[] MutableComponents
        {
            get { return new IMutable[0]; }
        }

        public Angle Clone()
        {
            return new Angle(this);
        }

        internal void RandomizeValues(Process process)
        {
            Value = process.RandomizationProvider.Next(0, 360);
        }

        internal void Nudge(Process process, int amount)
        {
            int nudgeAmount = process.RandomizationProvider.Next(1, amount + 1);
            bool positiveDirection = process.RandomizationProvider.Next(0, 2) == 1;
            Value = (Value + nudgeAmount*(positiveDirection ? 1 : -1)).Wrap(0, 360);
        }
    }
}