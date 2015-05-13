using System.Drawing;
using imG.Approx.Components.BuildingBlocks;
using imG.Approx.Mutation;
using Color = imG.Approx.Components.BuildingBlocks.Color;

namespace imG.Approx.Components.Shapes
{
    public class Line : IShape
    {
        public Line()
        {
            Color = new Color();
            Point1 = new Position();
            Point2 = new Position();
            PenSize = new PenSize();
        }

        private Line(Line source)
        {
            Color = source.Color.Clone();
            Point1 = source.Point1.Clone();
            Point2 = source.Point2.Clone();
            PenSize = source.PenSize.Clone();
        }

        public Position Point1 { get; set; }
        public Position Point2 { get; set; }
        public Color Color { get; set; }
        public PenSize PenSize { get; set; }

        public IMutable[] MutableComponents
        {
            get { return new IMutable[] {Point1, Point2, Color, PenSize}; }
        }

        public void Draw(Graphics g)
        {
            using (var p = new Pen(Color, PenSize.Size))
            {
                g.DrawLine(p, new Point(Point1.X, Point1.Y), new Point(Point2.X, Point2.Y));
            }
        }

        public void InitializeComponents(Process process)
        {
            var origin = new Position();
            origin.RandomizeValues(process);
            Point1 = origin.Clone();
            Point1.Nudge(process, 1);
            Point2 = origin.Clone();
            Point2.Nudge(process, 1);

            Color.RandomizeValues(process);
        }

        public IShape Clone()
        {
            return new Line(this);
        }
    }
}