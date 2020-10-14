using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Rocksmith2014PsarcLib.Psarc.Asset
{
    public class DdsAsset : PsarcAsset
    {
        public Bitmap Bitmap { get; private set; }

        public override void ReadFrom(MemoryStream stream)
        {
            base.ReadFrom(stream);

            using (var image = Pfim.Pfim.FromStream(stream))
            {
                PixelFormat format;

                switch (image.Format)
                {
                    case Pfim.ImageFormat.Rgba32:
                        format = PixelFormat.Format32bppArgb;
                        break;
                    case Pfim.ImageFormat.Rgb24:
                        format = PixelFormat.Format24bppRgb;
                        break;
                    default:
                        // see the sample for more details
                        throw new NotImplementedException();
                }

                var handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
                try
                {
                    var data = Marshal.UnsafeAddrOfPinnedArrayElement(image.Data, 0);
                    using (var bm = new Bitmap(image.Width, image.Height, image.Stride, format, data))
                    {
                        Bitmap = new Bitmap(bm);
                    }
                }
                finally
                {
                    handle.Free();
                }
            }


            /*
            //Create a Pfim image from memory stream
            Pfim.Dds img = Pfim.Dds.Create(stream, new Pfim.PfimConfig());

            //Create bitmap
            Bitmap = new Bitmap(img.Width, img.Height);

            //Convert Pfim image to bitmap
            int bytesPerPixel = img.BytesPerPixel;
            for (int i = 0; i < img.Data.Length; i += bytesPerPixel)
            {
                //Calculate pixel X and Y coordinates
                int x = (i / bytesPerPixel) % img.Width;
                int y = (i / bytesPerPixel) / img.Width;

                //Get color from the Pfim image data array
                Color c = Color.FromArgb(255, img.Data[i + 2], img.Data[i + 1], img.Data[i]);

                //Set pixel in bitmap
                Bitmap.SetPixel(x, y, c);
            }
            */
        }
    }
}
