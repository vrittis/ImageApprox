using System.Drawing;
using imG.Approx.Components.BuildingBlocks;
using imG.Approx.Mutation;
using Color = imG.Approx.Components.BuildingBlocks.Color;

namespace imG.Approx.Components.Shapes
{
    public class Bezier : IShape
    {
        public Bezier()
        {
            Color = new Color();
            PenSize = new PenSize();
            Point1 = new Position();
            Point2 = new Position();
            Point3 = new Position();
            Point4 = new Position();
        }

        private Bezier(Bezier source)
        {
            Color = source.Color.Clone();
            PenSize = source.PenSize.Clone();
            Point1 = source.Point1.Clone();
            Point2 = source.Point2.Clone();
            Point3 = source.Point3.Clone();
            Point4 = source.Point4.Clone();
        }

        public Color Color { get; set; }
        public Position Point1 { get; set; }
        public Position Point2 { get; set; }
        public Position Point3 { get; set; }
        public Position Point4 { get; set; }
        public PenSize PenSize { get; set; }

        public void Draw(Graphics g)
        {
            using (var p = new Pen(Color, PenSize.Size))
            {
                g.DrawBezier(p, new Point(Point1.X, Point1.Y), new Point(Point2.X, Point2.Y),
                    new Point(Point3.X, Point3.Y), new Point(Point4.X, Point4.Y));
            }
        }

        public void InitializeComponents(Process process)
        {
            Color.RandomizeValues(process);
            PenSize.RandomizeValues(process);

            var origin = new Position();
            origin.RandomizeValues(process);

            Point1 = origin.Clone();
            Point1.Nudge(process, 1);
            Point2 = origin.Clone();
            Point2.Nudge(process, 1);
            Point3 = origin.Clone();
            Point3.Nudge(process, 1);
            Point4 = origin.Clone();
            Point4.Nudge(process, 1);
        }

        public IShape Clone()
        {
            return new Bezier(this);
        }

        public IMutable[] MutableComponents
        {
            get { return new IMutable[] {PenSize, Color, Point1, Point2, Point3, Point4}; }
        }
    }
}