using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using imG.Approx.Components.Shapes;
using imG.Approx.Mutation;
using Color = imG.Approx.Components.BuildingBlocks.Color;

namespace imG.Approx.Components
{
    public class Drawing : IMutable<Drawing>
    {
        public Drawing(int width, int height)
        {
            Width = width;
            Height = height;
            BackgroundColor = new Color();
            Shapes = new List<IShape>();
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Color BackgroundColor { get; internal set; }
        public List<IShape> Shapes { get; internal set; }

        public Drawing Clone()
        {
            var clone = new Drawing(Width, Height);
            clone.BackgroundColor = BackgroundColor.Clone();
            for (int shapeCounter = 0; shapeCounter < Shapes.Count; shapeCounter++)
            {
                clone.Shapes.Add(Shapes[shapeCounter].Clone());
            }
            return clone;
        }

        public IMutable[] MutableComponents
        {
            get
            {
                var elements = new IMutable[Shapes.Count() + 1];
                for (int i = 0; i < Shapes.Count(); i++)
                {
                    elements[i] = Shapes[i];
                }
                elements[elements.Length - 1] = BackgroundColor;
                return elements;
            }
        }

        public Bitmap Draw()
        {
            var bmp = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

            using (Graphics graphicsObj = Graphics.FromImage(bmp))
            {
                graphicsObj.SmoothingMode = SmoothingMode.HighQuality;
                graphicsObj.InterpolationMode = InterpolationMode.HighQualityBicubic;
                BackgroundColor.A = 255;
                graphicsObj.Clear(BackgroundColor);
                foreach (IShape shape in Shapes)
                {
                    shape.Draw(graphicsObj);
                }
            }
            return bmp;
        }
    }
}