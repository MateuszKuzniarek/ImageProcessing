using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageProcessingLogic
{
    public static class ImageOperations
    {
        public const int numberOfColors = 3;
        public const int bytesPerPixel = 4;

        public unsafe static void ChangeBrightness(WriteableBitmap image, int brightnessChange)
        {
            image.Lock();
            byte* imagePointer = (byte*) image.BackBuffer;
            int stride = image.BackBufferStride;

            for (int i = 0; i < image.PixelHeight; i++)
            {
                for (int j = 0; j < image.PixelWidth; j++)
                {
                    for(int k = 0; k < numberOfColors; k++)
                    {
                        int index = i * stride + j * bytesPerPixel + k;
                        int newValue = imagePointer[index] + brightnessChange;
                        newValue = Math.Min(newValue, 255);
                        newValue = Math.Max(newValue, 0);
                        imagePointer[index] = (byte)newValue;
                    }
                }
            }

            image.AddDirtyRect(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight));
            image.Unlock();
        }
    }
}
