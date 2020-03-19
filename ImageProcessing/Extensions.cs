using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace ImageProcessing
{
    public static class Extensions
    {
        public static Bitmap ToBitmap(this BitmapImage bitmapImage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(memoryStream);
                Bitmap bitmap = new Bitmap(memoryStream);

                return new Bitmap(bitmap);
            }
        }

        public static void UpdateFromBitmap(this BitmapImage bitmapImage, Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                
                bitmapImage.StreamSource = memory;
                bitmapImage.Freeze();
            }
        }
    }
}
