using System;
using System.Buffers;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Pfim;

namespace Rocksmith2014PsarcLib.Psarc.Asset
{
    public class DdsAsset : PsarcAsset
    {
        /// <summary>
        /// Uses ArrayPool to rent byte arrays to Pfim, by default Pfim creates a new byte array each time
        /// </summary>
        private class ArrayPoolAllocator : IImageAllocator
        {
            // Use the shared byte array pool
            private readonly ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            
            public byte[] Rent(int size)
            {
                return pool.Rent(size);
            }

            public void Return(byte[] data)
            {
                pool.Return(data);
            }
        }
        
        /// <summary>
        /// Config used by Pfim to parse dds assets
        /// </summary>
        private readonly PfimConfig config = new PfimConfig(allocator: new ArrayPoolAllocator());
        
        public Bitmap Bitmap { get; private set; }

        public override void ReadFrom(MemoryStream stream)
        {
            base.ReadFrom(stream);

            using var image = Pfimage.FromStream(stream, config);
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
                
                using var bm = new Bitmap(image.Width, image.Height, image.Stride, format, data);
                Bitmap = new Bitmap(bm);
            }
            finally
            {
                handle.Free();
            }
        }
    }
}
