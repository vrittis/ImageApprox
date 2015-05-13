using System.Drawing;
using imG.Approx.Mutation;

namespace imG.Approx.Components.Shapes
{
    public interface IShape : IMutable<IShape>
    {
        void Draw(Graphics g);
        void InitializeComponents(Process process);
    }
}