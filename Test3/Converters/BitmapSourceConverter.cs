using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Test3.Converters
{
    public static class BitmapSourceConverter
    {
        public static BitmapSource ToBitmapSource(Mat image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));

            var pixelFormat = image.Channels() switch
            {
                1 => PixelFormats.Gray8,
                3 => PixelFormats.Bgr24,
                4 => PixelFormats.Bgr32,
                _ => throw new ArgumentException("Number of channels must be 1, 3 or 4.")
            };

            var bitmap = new WriteableBitmap(image.Width, image.Height, 96, 96, pixelFormat, null);
            bitmap.Lock();

            var sourceData = image.Data;
            var bufferSize = image.Rows * image.Cols * image.Channels();
            var buffer = new byte[bufferSize];
            Marshal.Copy(sourceData, buffer, 0, bufferSize);

            Marshal.Copy(buffer, 0, bitmap.BackBuffer, bufferSize);
            bitmap.AddDirtyRect(new System.Windows.Int32Rect(0, 0, image.Width, image.Height));
            bitmap.Unlock();

            return bitmap;
        }
    }
}
