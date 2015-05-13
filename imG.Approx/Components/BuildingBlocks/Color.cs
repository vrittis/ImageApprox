using imG.Approx.Mutation;

namespace imG.Approx.Components.BuildingBlocks
{
    public class Color : IMutable<Color>
    {
        internal int A;
        internal int B;
        internal int G;
        internal int R;

        public Color()
        {
        }

        public Color(Color source)
        {
            A = source.A;
            R = source.R;
            G = source.G;
            B = source.B;
        }

        public Color Clone()
        {
            return new Color(this);
        }

        public IMutable[] MutableComponents
        {
            get { return new IMutable[0]; }
        }

        public static implicit operator System.Drawing.Color(Color c)
        {
            return System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public void RandomizeValues(Process process)
        {
            A = process.RandomizationProvider.Next(60, 200);
            R = process.RandomizationProvider.Next(0, 256);
            G = process.RandomizationProvider.Next(0, 256);
            B = process.RandomizationProvider.Next(0, 256);
        }

        internal int RandomizeBlue(Process process)
        {
            return B = process.RandomizationProvider.Next(0, 256);
        }

        internal int RandomizeGreen(Process process)
        {
            return G = process.RandomizationProvider.Next(0, 256);
        }

        internal int RandomizeRed(Process process)
        {
            return R = process.RandomizationProvider.Next(0, 256);
        }

        internal int RandomizeAlpha(Process process)
        {
            return A = process.RandomizationProvider.Next(60, 200);
        }
    }
}