using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageProcessingLogic
{
    public static class ImageOperations
    {

        public unsafe static void ChangeBrightness(WriteableBitmap image, int brightnessChange, int bytesPerPixel)
        {

            int[] brightnessArray = new int[256];

            for (int i = 0; i < 256; i++)
            {
                brightnessArray[i] = (int)(i + brightnessChange);
            }

            ChangePixelsValue(image, bytesPerPixel, brightnessArray);
        }
        public unsafe static void ArithmeticFilter(WriteableBitmap image, int bytesPerPixel, int maskSize)
        {
            int numberOfColors = 1;
            if (bytesPerPixel != 1)
            {
                numberOfColors = 3;
            }
            image.Lock();
            byte* imagePointer = (byte*)image.BackBuffer;
            int stride = image.BackBufferStride;

            for (int i = ((maskSize-1) / 2); i < image.PixelHeight - ((maskSize - 1) / 2); i++)
            {
                for (int j = ((maskSize - 1) / 2); j < image.PixelWidth - ((maskSize - 1) / 2); j++)
                {
                    for (int k = 0; k < numberOfColors; k++)
                    {
                        int sum = 0;
                        for(int a = i - ((maskSize - 1) / 2); a <= i + ((maskSize - 1) / 2); a++)
                        {
                            for(int b = j - ((maskSize - 1) / 2); b <= j + ((maskSize - 1) / 2); b++)
                            {
                                sum += imagePointer[a * stride + b * bytesPerPixel + k];
                            }
                        }
                        int index = i * stride + j * bytesPerPixel + k;
                        int newValue = (int)(sum / (maskSize * maskSize));
                        newValue = Math.Min(newValue, 255);
                        newValue = Math.Max(newValue, 0);
                        imagePointer[index] = (byte)newValue;
                    }
                }
            }

            image.AddDirtyRect(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight));
            image.Unlock();
        }
        public unsafe static void ChangeNegative(WriteableBitmap image, int bytesPerPixel)
        {

            int[] negativeArray = new int[256];

            for (int i = 0; i < 256; i++)
            {
                negativeArray[i] = (int)(255 - i);
            }

            ChangePixelsValue(image, bytesPerPixel, negativeArray);
        }

        public unsafe static void ChangeContrast(WriteableBitmap image, int contrastChange, int bytesPerPixel, String contrastType)
        {
            int[] contrastArray = new int[256];

            if (contrastType == "decrease")
            {
                for (int i = 0; i < 256; i++)
                {
                    contrastArray[i] = (int)(127 + (i - 127) * (255 - 2 * contrastChange) / 255 + 0.5);
                }
            }
            else
            {
                for (int i = 0; i < 255; i++)
                {
                    contrastArray[i] = (int)(127 + (i - 127) * 255 / (255 - 2 * contrastChange) + 0.5);
                }
            }

            ChangePixelsValue(image, bytesPerPixel, contrastArray);
        }

        public unsafe static void ChangePixelsValue(WriteableBitmap image, int bytesPerPixel, int[] mask)
        {
            int numberOfColors = 1;
            if (bytesPerPixel != 1)
            {
                numberOfColors = 3;
            }
            image.Lock();
            byte* imagePointer = (byte*)image.BackBuffer;
            int stride = image.BackBufferStride;

            for (int i = 0; i < image.PixelHeight; i++)
            {
                for (int j = 0; j < image.PixelWidth; j++)
                {
                    for (int k = 0; k < numberOfColors; k++)
                    {
                        int index = i * stride + j * bytesPerPixel + k;
                        int newValue = mask[imagePointer[index]];
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
