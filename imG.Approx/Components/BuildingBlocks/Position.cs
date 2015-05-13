using imG.Approx.Mutation;
using imG.Approx.Tools;

namespace imG.Approx.Components.BuildingBlocks
{
    public class Position : IMutable<Position>
    {
        public int X;
        public int Y;

        public Position()
        {
        }

        internal Position(Position source)
        {
            X = source.X;
            Y = source.Y;
        }

        public IMutable[] MutableComponents
        {
            get { return new IMutable[0]; }
        }

        public Position Clone()
        {
            return new Position(this);
        }

        internal void RandomizeValues(Process process)
        {
            X = process.RandomizationProvider.Next(0, process.Drawing.Width);
            Y = process.RandomizationProvider.Next(0, process.Drawing.Height);
        }

        internal void Nudge(Process process, int amount)
        {
            X = (X + process.RandomizationProvider.Next(-amount, amount + 1)).Clamp(0, process.Target.Working.Width);
            Y = (Y + process.RandomizationProvider.Next(-amount, amount + 1)).Clamp(0, process.Target.Working.Height);
        }
    }
}