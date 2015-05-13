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
    public class Blob  :  AbstractPointShape<Blob>
    {
        public Blob()
        { }

        private Blob(Blob source)
            : base(source)
        {}

        public override IShape Clone()
        {
            return new Blob(this);
        }

        public override void Draw(Graphics g)
        {
            using (var b = new SolidBrush(Color))
            {
                g.FillClosedCurve(b, Points.Select(mp => new Point(mp.X, mp.Y)).ToArray());
            }
        }
    }
}
