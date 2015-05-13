using System.Collections.Generic;

namespace imG.Approx.Components.Shapes.Factories
{
    public interface IShapeFactoryCatalog
    {
        void Register(IShapeFactory factory);
        IList<IShapeFactory> ActiveFactories();
        void EnableAll();
        void DisableAll();
        void Disable(params string[] names);
        void Enable(params string[] names);
    }
}