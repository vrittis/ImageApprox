using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace imG.Approx.Mutation
{
    public class MutationDescriptionCatalog : IMutationDescriptionCatalog
    {
        public MutationDescriptionCatalog()
        {
            Descriptions = new Dictionary<Type, List<IMutationDescription>>();
        }

        internal Dictionary<Type, List<IMutationDescription>> Descriptions { get; set; }

        public void Register(IMutationDescription mutationDescription)
        {
            Type typeTargetedByMutation = mutationDescription.GetMutationTargetType();
            if (!Descriptions.ContainsKey(typeTargetedByMutation))
            {
                Descriptions.Add(typeTargetedByMutation, new List<IMutationDescription>());
            }

            List<IMutationDescription> descriptionsForType = Descriptions[typeTargetedByMutation];
            if (descriptionsForType.Contains(mutationDescription))
            {
                throw new InvalidOperationException("Description already present");
            }
            Descriptions[typeTargetedByMutation].Add(mutationDescription);
        }

        public List<IMutationDescription> For(IMutable mutable)
        {
            Type targetType = mutable.GetType();
            if (Descriptions.ContainsKey(targetType))
            {
                return Descriptions[targetType];
            }
            return new List<IMutationDescription>();
        }

        public void RegisterAllMutations()
        {
            Assembly[] allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            IEnumerable<Type> types = allAssemblies.SelectMany(a => a.GetTypes()
                .Where(
                    t =>
                        t.GetInterfaces().Contains(typeof (IMutationDescriptionRegistrar)) &&
                        t.GetConstructor(Type.EmptyTypes) != null));
            IEnumerable<IMutationDescriptionRegistrar> mutationsDescriptionRegistrars =
                types.Select(t => Activator.CreateInstance(t) as IMutationDescriptionRegistrar);


            foreach (IMutationDescriptionRegistrar mutationDescriptionRegistrar in mutationsDescriptionRegistrars)
            {
                foreach (IMutationDescription mutationDescription in mutationDescriptionRegistrar.DeclareMutations())
                {
                    Register(mutationDescription);
                }
            }
        }
    }
}