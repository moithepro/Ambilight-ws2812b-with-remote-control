using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlGuiLed
{
    unsafe class BmpPixelSnoop : IDisposable
    {
        // A reference to the bitmap to be wrapped
        private readonly Bitmap wrappedBitmap;

        // The bitmap's data (once it has been locked)
        private BitmapData data = null;

        // Pointer to the first pixel
        private readonly byte* scan0;

        // Number of bytes per pixel
        private readonly int depth;

        // Number of bytes in an image row
        private readonly int stride;

        // The bitmap's width
        private readonly int width;

        // The bitmap's height
        private readonly int height;

        /// 

        /// Constructs a BmpPixelSnoop object, the bitmap
        /// object to be wraped is passed as a parameter.
        /// 

        /// The bitmap to snoop
        public BmpPixelSnoop(Bitmap bitmap)
        {
            wrappedBitmap = bitmap ?? throw new ArgumentException("Bitmap parameter cannot be null", "bitmap");

            // Currently works only for: PixelFormat.Format32bppArgb
            if (wrappedBitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new System.ArgumentException("Only PixelFormat.Format32bppArgb is supported", "bitmap");

            // Record the width & height
            width = wrappedBitmap.Width;
            height = wrappedBitmap.Height;

            // So now we need to lock the bitmap so that we can gain access
            // to it's raw pixel data.  It will be unlocked when this snoop is 
            // disposed.
            var rect = new Rectangle(0, 0, wrappedBitmap.Width, wrappedBitmap.Height);

            try
            {
                data = wrappedBitmap.LockBits(rect, ImageLockMode.ReadWrite, wrappedBitmap.PixelFormat);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not lock bitmap, is it already being snooped somewhere else?", ex);
            }

            // Calculate number of bytes per pixel
            depth = Bitmap.GetPixelFormatSize(data.PixelFormat) / 8; // bits per channel

            // Get pointer to first pixel
            scan0 = (byte*)data.Scan0.ToPointer();

            // Get the number of bytes in an image row
            // this will be used when determining a pixel's
            // memory address.
            stride = data.Stride;
        }

        /// 

        /// Disposes BmpPixelSnoop object
        /// 

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// 

        /// Disposes BmpPixelSnoop object, we unlock
        /// the wrapped bitmap.
        /// 

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (wrappedBitmap != null)
                    wrappedBitmap.UnlockBits(data);
            }
            // free native resources if there are any.
        }

        /// 

        /// Calculate the pointer to a pixel at (x, x)
        /// 

        /// The pixel's x coordinate
        /// The pixel's y coordinate
        /// A byte* pointer to the pixel's data
        private byte* PixelPointer(int x, int y)
        {
            return scan0 + y * stride + x * depth;
        }

        /// 

        /// Snoop's implemetation of GetPixel() which is similar to
        /// Bitmap's GetPixel() but should be faster.
        /// 

        /// The pixel's x coordinate
        /// The pixel's y coordinate
        /// The pixel's colour
        public System.Drawing.Color GetPixel(int x, int y)
        {
            // Better do the 'decent thing' and bounds check x & y
            if (x < 0 || y < 0 || x >= width || y >= width)
                throw new ArgumentException("x or y coordinate is out of range");

            int a, r, g, b;

            // Get a pointer to this pixel
            byte* p = PixelPointer(x, y);

            // Pull out its colour data
            b = *p++;
            g = *p++;
            r = *p++;
            a = *p;

            // And return a color value for it (this is quite slow
            // but allows us to look like Bitmap.GetPixel())
            return System.Drawing.Color.FromArgb(a, r, g, b);
        }

        /// 

        /// Sets the passed colour to the pixel at (x, y)
        /// 

        /// The pixel's x coordinate
        /// The pixel's y coordinate
        /// The value to be assigned to the pixel
        public void SetPixel(int x, int y, System.Drawing.Color col)
        {
            // Better do the 'decent thing' and bounds check x & y
            if (x < 0 || y < 0 || x >= width || y >= width)
                throw new ArgumentException("x or y coordinate is out of range");

            // Get a pointer to this pixel
            byte* p = PixelPointer(x, y);

            // Set the data
            *p++ = col.B;
            *p++ = col.G;
            *p++ = col.R;
            *p = col.A;
        }

        /// 

        /// The bitmap's width
        /// 

        public int Width { get { return width; } }

        // The bitmap's height
        public int Height { get { return height; } }
    }
}
