using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace imG.Approx.Components.Shapes.Factories
{
    public class ShapeFactoryCatalog : IShapeFactoryCatalog
    {
        public ShapeFactoryCatalog()
        {
            Factories = new List<IShapeFactory>();
        }

        internal List<IShapeFactory> Factories { get; set; }

        public void Register(IShapeFactory factory)
        {
            Factories.Add(factory);
        }

        public IList<IShapeFactory> ActiveFactories()
        {
            return Factories.Where(f => f.IsActive).ToList();
        }

        public void EnableAll()
        {
            Factories.ForEach(f => f.IsActive = true);
        }

        public void DisableAll()
        {
            Factories.ForEach(f => f.IsActive = false);
        }

        public void Disable(params string[] names)
        {
            ForNamedFactory(names, factory => factory.IsActive = false);
        }

        public void Enable(params string[] names)
        {
            ForNamedFactory(names, factory => factory.IsActive = true);
        }

        public void RegisterAllFactories()
        {
            Assembly[] allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            IEnumerable<Type> types = allAssemblies.SelectMany(a => a.GetTypes()
                .Where(
                    t =>
                        t.GetInterfaces().Contains(typeof (IShapeFactory)) && t.GetConstructor(Type.EmptyTypes) != null &&
                        !t.IsAbstract));
            IEnumerable<IShapeFactory> factories =
                types.Select(t => Activator.CreateInstance(t) as IShapeFactory);

            foreach (IShapeFactory factory in factories)
            {
                Register(factory);
            }
        }

        private void ForNamedFactory(string[] names, Action<IShapeFactory> factoryChange)
        {
            foreach (string name in names)
            {
                string name1 = name;
                foreach (
                    IShapeFactory factory in
                        Factories.Where(f => f.Name.Equals(name1, StringComparison.InvariantCultureIgnoreCase)))
                {
                    factoryChange(factory);
                }
            }
        }
    }
}