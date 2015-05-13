using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using imG.Approx.Components.BuildingBlocks;
using imG.Approx.Mutation;
using Color = imG.Approx.Components.BuildingBlocks.Color;

namespace imG.Approx.Components.Shapes
{
    public abstract class AbstractPointShape<T> : IShape where T : AbstractPointShape<T>
    {
        internal readonly List<Position> Points;

        public AbstractPointShape()
        {
            Points = new List<Position>();
            for (int i = 0; i < 3; i++)
            {
                Points.Add(new Position());
            }
            Color = new Color();
        }

        protected AbstractPointShape(AbstractPointShape<T> source)
        {
            Color = source.Color.Clone();
            Points = new List<Position>(source.Points.Select(mp => mp.Clone()));
        }

        public Color Color { get; set; }

        public virtual void Draw(Graphics g)
        {
            using (var b = new SolidBrush(Color))
            {
                g.FillPolygon(b, Points.Select(mp => new Point(mp.X, mp.Y)).ToArray());
            }
        }

        public void InitializeComponents(Process process)
        {
            Color.RandomizeValues(process);
            var origin = new Position();
            origin.RandomizeValues(process);
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i] = origin.Clone();
                Points[i].Nudge(process, 1);
            }
        }

        public abstract IShape Clone();

        public IMutable[] MutableComponents
        {
            get
            {
                var mutables = new IMutable[Points.Count + 1];
                mutables[0] = Color;
                Array.Copy(Points.ToArray(), 0, mutables, 1, Points.Count());
                return mutables;
            }
        }
    }
}
