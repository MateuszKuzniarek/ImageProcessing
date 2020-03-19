using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace ImageProcessingLogic
{
    public static class ImageOperations
    {
        public static void IncreaseBrightness(Bitmap image)
        {
            for(int i=0; i<image.Width; i++)
            {
                for(int j=0; j<image.Height; j++)
                {
                    Color color = image.GetPixel(i, j);
                }
            }
        }

        public static void DecreaseBrightness(Bitmap image)
        {

        }
    }
}
