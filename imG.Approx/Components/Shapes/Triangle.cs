using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using imG.Approx.Components.BuildingBlocks;
using imG.Approx.Mutation;
using Color = imG.Approx.Components.BuildingBlocks.Color;

namespace imG.Approx.Components.Shapes
{
    public class Triangle :  AbstractPointShape<Triangle>
    {
        public Triangle()
        { }

        private Triangle(Triangle source)
            : base(source)
        {}

        public override IShape Clone()
        {
            return new Triangle(this);
        }
    }
}