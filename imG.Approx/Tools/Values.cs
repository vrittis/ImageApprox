using System;

namespace imG.Approx.Tools
{
    public static class Values
    {
        public static int Clamp(this int value, int min, int max)
        {
            return InnerClamp(value, min, max);
        }

        private static int InnerClamp(int value, int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException();
            }
            return Math.Min(max, Math.Max(value, min));
        }


        public static int Wrap(this int value, int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException();
            }

            while (value < min)
            {
                value = value + max - min;
            }
            while (value >= max)
            {
                value = value - max + min;
            }
            return value;
        }
    }
}