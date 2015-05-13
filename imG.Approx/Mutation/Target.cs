using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using imG.Approx.Components;

namespace imG.Approx.Mutation
{
    public class Target : ITarget
    {
        private static readonly ulong[,] Errors;

        static Target()
        {
            Errors = new ulong[256, 256];

            for (ulong r1 = 0; r1 < 256; r1++)
            {
                for (ulong r2 = 0; r2 < 256; r2++)
                {
                    ulong rError = (r1 - r2);
                    Errors[r1, r2] = rError*rError;
                }
            }
        }

        public Target(string file, int maxDimension)
        {
            File = file;
            MaxDimension = maxDimension;
            LoadImageData();
        }

        public string Name {
            get { return System.IO.Path.GetFileName(File); }
        }

        public int MaxDimension { get; private set; }
        public string File { get; private set; }
        internal Pixel[] ImageData { get; private set; }

        public ulong DistanceTo(Drawing drawing, ulong stopAt)
        {
            if (drawing.Width != Working.Width || drawing.Height != Working.Height)
            {
                throw new InvalidOperationException(
                    string.Format("Dimensions mismatched drawing / target : {0}-{1} / {2}-{3}", drawing.Width,
                        drawing.Height, Working.Width, Working.Height));
            }

            return GetDrawingFitness(drawing, ImageData, stopAt);
        }

        public Dimensions Original { get; internal set; }
        public Dimensions Working { get; internal set; }

        internal void LoadImageData()
        {
            using (var bmp = new Bitmap(File))
            {
                ComputeDimensions(bmp);

                //TODO: constructeur plus efficace pour ce point, utilise directement l'image
                var resizedImage = new Bitmap(Working.Width, Working.Height, PixelFormat.Format24bppRgb);
                resizedImage.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);

                Graphics g = Graphics.FromImage(resizedImage);
                g.InterpolationMode = InterpolationMode.Default;

                g.DrawImage(bmp, new Rectangle(0, 0, Working.Width, Working.Height),
                    new Rectangle(0, 0, Original.Width, Original.Height), GraphicsUnit.Pixel);
                g.Dispose();

                ImageData = SetupSourceColorMatrix(resizedImage);
            }
        }

        public struct Pixel
        {
            public byte B; // important: #1 field
            public byte G; // important: #2 field
            public byte R; // important: #3 field
            public byte A; // important: #4 field
        }

        public Pixel[] SetupSourceColorMatrix(Bitmap sourceImage)
        {
            BitmapData bd = sourceImage.LockBits(
            new Rectangle(0, 0, Working.Width , Working.Height),
            ImageLockMode.ReadOnly,
            PixelFormat.Format32bppArgb);
            Pixel[] sourcePixels = new Pixel[Working.Width * Working.Height];
            unsafe
            {
                fixed (Pixel* psourcePixels = sourcePixels)
                {
                    Pixel* pSrc = (Pixel*)bd.Scan0.ToPointer();
                    Pixel* pDst = psourcePixels;
                    for (int i = sourcePixels.Length; i > 0; i--)
                        *(pDst++) = *(pSrc++);
                }
            }
            sourceImage.UnlockBits(bd);

            return sourcePixels;
        }

        private void ComputeDimensions(Bitmap bmp)
        {
            Original = new Dimensions();
            Working = new Dimensions();

            Original.Width = bmp.Width;
            Original.Height = bmp.Height;

            int currentMaxDimension = Math.Max(Original.Width, Original.Height);
            double ratio = currentMaxDimension > MaxDimension ? (MaxDimension/(float) currentMaxDimension) : 1.0;

            Working.Width = (int) (ratio*Original.Width);
            Working.Height = (int) (ratio*Original.Height);
        }

        private  ulong GetDrawingFitness(Drawing drawing, Pixel[] sourcePixels, ulong stopAt)
        {
            ulong error = 0;

            using (Bitmap bmp = drawing.Draw())
            {
                BitmapData bd = bmp.LockBits(
                    new Rectangle(0, 0, Working.Width, Working.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb);

                unchecked
                {
                    unsafe
                    {
                        fixed (Pixel* psourcePixels = sourcePixels)
                        {
                            Pixel* p1 = (Pixel*) bd.Scan0.ToPointer();
                            Pixel* p2 = psourcePixels;
                            for (int i = sourcePixels.Length; i > 0 ; i--, p1++, p2++)
                            {
                                error += Errors[p1->R, p2->R]+ Errors[p1->G, p2->G] + Errors[p1->B, p2->B];
                            }
                        }
                        }
                }
                bmp.UnlockBits(bd);
            }
            //return error;

            return (ulong)((error * (decimal)(drawing.Shapes.Count + 100)) / (decimal)(drawing.Shapes.Count + 101));
    

        }

        public class Dimensions
        {
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
}