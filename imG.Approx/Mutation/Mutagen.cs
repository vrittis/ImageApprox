using System;
using System.Collections.Generic;
using System.Linq;

namespace imG.Approx.Mutation
{
    public class Mutagen
    {
        public SelectedMutation SelectMutation(Process process, IMutable mutable)
        {
            CheckArguments(process, mutable);

            IDictionary<IMutationDescription, List<IMutable>> possibleMutations = GetMutationsFor(process, mutable);
            SelectedMutation selectedMutation = ChooseMutation(possibleMutations, process.RandomizationProvider);
            return selectedMutation ??
                   new SelectedMutation {Description = new NoOpMutationDescription(), Target = mutable};
        }

        internal IDictionary<IMutationDescription, List<IMutable>> GetMutationsFor(Process process, IMutable mutable)
        {
            var dictionary = new Dictionary<IMutationDescription, List<IMutable>>();
            AddMutationsFor(process, mutable, dictionary);
            return dictionary;
        }

        private void AddMutationsFor(Process process, IMutable mutable,
            IDictionary<IMutationDescription, List<IMutable>> mutations)
        {
            foreach (
                IMutationDescription mutationDescription in
                    process.MutationDescriptionCatalog.For(mutable)
                        .Where(md => md.Active && md.OccasionsToOccur > 0 && md.CanMutate(process, mutable)))
            {
                if (!mutations.ContainsKey(mutationDescription))
                {
                    mutations.Add(mutationDescription, new List<IMutable>());
                }
                mutations[mutationDescription].Add(mutable);
            }

            foreach (IMutable childMutable in mutable.MutableComponents)
            {
                AddMutationsFor(process, childMutable, mutations);
            }
        }

        private static void CheckArguments(Process process, IMutable mutable)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (mutable == null)
                throw new ArgumentNullException("mutable");
        }

        internal SelectedMutation ChooseMutation(IDictionary<IMutationDescription, List<IMutable>> source,
            IRandomizationProvider randomization)
        {
            var mutationWithMutables = source.Where(kvp => kvp.Value.Count > 0).ToList();
            int selectedSlot = randomization.Next(0, mutationWithMutables.Sum(kvp => kvp.Key.OccasionsToOccur));

            int currentSlot = 0;
            foreach (var kvp in mutationWithMutables)
            {
                int mutationsSlots = kvp.Key.OccasionsToOccur;
                if (selectedSlot < currentSlot + mutationsSlots)
                {
                    return new SelectedMutation
                    {
                        Description = kvp.Key,
                        Target = kvp.Value[randomization.Next(0, kvp.Value.Count)]
                    };
                }
                currentSlot += mutationsSlots;
            }

            return null;
        }

        public class NoOpMutationDescription : IMutationDescription
        {
            public bool Active
            {
                get { return true; }
                set { }
            }

            public int OccasionsToOccur
            {
                get { return 1; }
                set { }
            }

            public bool CanMutate(Process process, IMutable target)
            {
                return true;
            }

            public void Mutate(Process process, IMutable target)
            {
            }

            public Type GetMutationTargetType()
            {
                return typeof (IMutable);
            }
        }

        public class SelectedMutation
        {
            public IMutationDescription Description { get; set; }
            public IMutable Target { get; set; }
        }
    }
}