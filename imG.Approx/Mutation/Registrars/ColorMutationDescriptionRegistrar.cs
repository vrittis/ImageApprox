using System;
using System.Collections.Generic;
using imG.Approx.Components.BuildingBlocks;

namespace imG.Approx.Mutation.Registrars
{
    public class ColorMutationDescriptionRegistrar : IMutationDescriptionRegistrar
    {
        public IEnumerable<IMutationDescription> DeclareMutations()
        {
            return new IMutationDescription[]
            {
                new MutationDescription<Color>("Randomize all", 1,
                    (process, color) => color.RandomizeValues(process)),
                new MutationDescription<Color>("Randomize alpha", 1,
                    (process, color) => color.RandomizeAlpha(process)),
                new MutationDescription<Color>("Randomize red", 2,
                    (process, color) => color.RandomizeRed(process)),
                new MutationDescription<Color>("Randomize green", 2,
                    (process, color) => color.RandomizeGreen(process)),
                new MutationDescription<Color>("Randomize blue", 2,
                    (process, color) => color.RandomizeBlue(process)),
                new MutationDescription<Color>("Lighten", 1, (process, color) =>
                {
                    color.R = Math.Min(255, color.R + 10);
                    color.G = Math.Min(255, color.G + 10);
                    color.B = Math.Min(255, color.B + 10);
                }),
                new MutationDescription<Color>("Darken", 1, (process, color) =>
                {
                    color.R = Math.Max(0, color.R - 10);
                    color.G = Math.Max(0, color.G - 10);
                    color.B = Math.Max(0, color.B - 10);
                })
            };
        }
    }
}