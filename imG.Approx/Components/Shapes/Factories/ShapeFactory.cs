using imG.Approx.Mutation;

namespace imG.Approx.Components.Shapes.Factories
{
    public abstract class ShapeFactory<T> : IShapeFactory where T : IShape, new()
    {
        public ShapeFactory()
        {
            IsActive = true;
        }

        public virtual string Name
        {
            get { return typeof (T).Name; }
        }

        public bool IsActive { get; set; }

        public virtual IShape GetShape(Process process)
        {
            var t = new T();
            t.InitializeComponents(process);
            return t;
        }
    }
}