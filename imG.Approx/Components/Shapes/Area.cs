using System;
using System.Drawing;
using imG.Approx.Components.BuildingBlocks;
using imG.Approx.Mutation;
using Color = imG.Approx.Components.BuildingBlocks.Color;

namespace imG.Approx.Components.Shapes
{
    public class Area : IShape
    {
        public Area()
        {
            Color = new Color();
            Center = new Position();
            Angle = new Angle();
        }

        private Area(Area source)
        {
            Center = source.Center.Clone();
            Angle = source.Angle.Clone();
            Color = source.Color.Clone();
        }

        public Color Color { get; set; }
        public Position Center { get; set; }
        public Angle Angle { get; set; }

        public IMutable[] MutableComponents
        {
            get { return new IMutable[] {Color, Center, Angle}; }
        }

        public void Draw(Graphics g)
        {
            using (var b = new SolidBrush(Color))
            {
                float r = Math.Max(g.VisibleClipBounds.Width, g.VisibleClipBounds.Height);

                int X = Center.X;
                int Y = Center.Y;

                g.FillPie(b, X - r*2, Y - r*2, r*4, r*4, Angle.Value, 180);
            }
        }

        public void InitializeComponents(Process process)
        {
            Color.RandomizeValues(process);
            Center.RandomizeValues(process);
            Angle.RandomizeValues(process);
        }

        public IShape Clone()
        {
            return new Area(this);
        }
    }
}