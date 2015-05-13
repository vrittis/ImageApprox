using System.Drawing;
using imG.Approx.Components.BuildingBlocks;
using imG.Approx.Mutation;
using Color = imG.Approx.Components.BuildingBlocks.Color;

namespace imG.Approx.Components.Shapes
{
    public class Rectangle : IShape
    {
        public Rectangle()
        {
            TopLeft = new Position();
            Width = new Amount();
            Height = new Amount();
            Color = new Color();
        }

        private Rectangle(Rectangle source)
        {
            Color = source.Color.Clone();
            TopLeft = source.TopLeft.Clone();
            Width = source.Width.Clone();
            Height = source.Height.Clone();
        }

        public Position TopLeft { get; set; }
        public Amount Width { get; set; }
        public Amount Height { get; set; }
        public Color Color { get; set; }

        public void Draw(Graphics g)
        {
            using (var b = new SolidBrush(Color))
            {
                g.FillRectangle(b, TopLeft.X, TopLeft.Y, Width.Value, Height.Value);
            }
        }

        public void InitializeComponents(Process process)
        {
            TopLeft.RandomizeValues(process);
            //Width.RandomizeValues(process);
            //Height.RandomizeValues(process);
            Color.RandomizeValues(process);
        }

        public IShape Clone()
        {
            return new Rectangle(this);
        }

        public IMutable[] MutableComponents
        {
            get { return new IMutable[] {Color, TopLeft, Width, Height}; }
        }
    }
}