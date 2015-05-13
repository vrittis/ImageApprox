using imG.Approx.Mutation;
using imG.Approx.Tools;

namespace imG.Approx.Components.BuildingBlocks
{
    public class Amount : IMutable<Amount>
    {
        public int Value;

        internal Amount()
        {
            Value = 1;
        }

        public IMutable[] MutableComponents
        {
            get { return new IMutable[0]; }
        }

        public Amount Clone()
        {
            return new Amount {Value = Value};
        }

        internal void RandomizeValues(Process process)
        {
            Value = process.RandomizationProvider.Next(1, process.Target.Working.Width + 1);
        }

        internal void Nudge(Process process, int amount)
        {
            Value = (Value + process.RandomizationProvider.Next(-amount, amount + 1)).Clamp(1,
                process.Target.Working.Width);
        }
    }
}