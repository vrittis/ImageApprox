using System;

namespace imG.Approx.Mutation
{
    public class RandomizationProvider : IRandomizationProvider
    {
        public RandomizationProvider(int seed)
        {
            Seed = seed;
            Random = new Random(seed);
        }

        public int Seed { get; set; }
        private Random Random { get; set; }

        public int Next(int lowerBound, int upperBound)
        {
            return Random.Next(lowerBound, upperBound);
        }
    }
}