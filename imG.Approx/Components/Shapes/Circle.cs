using System.Drawing;
using imG.Approx.Components.BuildingBlocks;
using imG.Approx.Mutation;
using Color = imG.Approx.Components.BuildingBlocks.Color;

namespace imG.Approx.Components.Shapes
{
    public class Circle : IShape
    {
        public Circle()
        {
            TopLeft = new Position();
            Size = new Amount();
            Color = new Color();
        }

        private Circle(Circle source)
        {
            Color = source.Color.Clone();
            TopLeft = source.TopLeft.Clone();
            Size = source.Size.Clone();
        }

        public Color Color { get; set; }
        public Position TopLeft { get; set; }
        public Amount Size { get; set; }

        public void Draw(Graphics g)
        {
            using (var b = new SolidBrush(Color))
            {
                g.FillEllipse(b, TopLeft.X, TopLeft.Y, Size.Value, Size.Value);
            }
        }

        public void InitializeComponents(Process process)
        {
            TopLeft.RandomizeValues(process);
            Color.RandomizeValues(process);
        }

        public IShape Clone()
        {
            return new Circle(this);
        }

        public IMutable[] MutableComponents
        {
            get { return new IMutable[] {Color, TopLeft, Size}; }
        }
    }
}