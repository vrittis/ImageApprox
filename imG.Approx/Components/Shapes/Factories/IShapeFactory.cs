using imG.Approx.Mutation;

namespace imG.Approx.Components.Shapes.Factories
{
    public interface IShapeFactory
    {
        string Name { get; }
        bool IsActive { get; set; }

        IShape GetShape(Process process);
    }
}