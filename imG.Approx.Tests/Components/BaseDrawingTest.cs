using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace imG.Approx.Tests.Components
{
    public class BaseDrawingTest : BaseIMutableTests
    {
        public void DrawTest(Action<Graphics> drawAction)
        {
            var bmp = new Bitmap(Process.Target.Working.Width, Process.Target.Working.Height, PixelFormat.Format24bppRgb);

            using (Graphics graphicsObj = Graphics.FromImage(bmp))
            {
                drawAction(graphicsObj);
            }
        }
    }
}