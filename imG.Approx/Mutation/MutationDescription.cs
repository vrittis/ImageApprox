using System;

namespace imG.Approx.Mutation
{
    public class MutationDescription<T> : IMutationDescription where T : class, IMutable
    {
        public MutationDescription(string name, int occasionsToOccur, Action<Process, T> action)
            : this(name, occasionsToOccur, action, (context, target) => true)
        {
        }

        public MutationDescription(string name, int occasionsToOccur, Action<Process, T> action,
            Func<Process, T, bool> condition)
        {
            if (occasionsToOccur < 1)
            {
                throw new ArgumentOutOfRangeException("occasionsToOccur", occasionsToOccur,
                    "occasionsToOccur must be positive integer");
            }

            Active = true;
            Name = name;
            OccasionsToOccur = occasionsToOccur;
            Action = action;
            Condition = condition;
        }

        private Action<Process, T> Action { get; set; }
        private Func<Process, T, bool> Condition { get; set; }
        public string Name { get; set; }
        public int OccasionsToOccur { get; set; }
        public bool Active { get; set; }

        public bool CanMutate(Process process, IMutable target)
        {
            return Condition(process, target as T);
        }

        public void Mutate(Process process, IMutable target)
        {
            Action(process, target as T);
        }

        public Type GetMutationTargetType()
        {
            return typeof (T);
        }
    }
}