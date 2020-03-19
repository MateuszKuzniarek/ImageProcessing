using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageProcessingLogic
{
    public static class ImageOperations
    {
        private const int bytesPerPixel = 4;
        private const int numberOfColors = 3;

        public unsafe static void ChangeBrightness(WriteableBitmap image, int brightnessChange)
        {
            int[] brightnessArray = new int[256];

            for (int i = 0; i < 256; i++)
            {
                brightnessArray[i] = (int)(i + brightnessChange);
            }

            ChangePixelsValue(image, brightnessArray);
        }

        public unsafe static void ArithmeticFilter(WriteableBitmap image, int maskSize)
        {
            image.Lock();
            byte* imagePointer = (byte*)image.BackBuffer;
            int stride = image.BackBufferStride;
            int offset = ((maskSize - 1) / 2);

            for (int i = offset; i < image.PixelHeight - offset; i++)
            {
                for (int j = offset; j < image.PixelWidth - offset; j++)
                {
                    for (int k = 0; k < numberOfColors; k++)
                    {
                        int sum = 0;
                        for(int a = i - offset; a <= i + offset; a++)
                        {
                            for(int b = j - offset; b <= j + offset; b++)
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

        public unsafe static void ChangeNegative(WriteableBitmap image)
        {

            int[] negativeArray = new int[256];

            for (int i = 0; i < 256; i++)
            {
                negativeArray[i] = (int)(255 - i);
            }

            ChangePixelsValue(image, negativeArray);
        }

        public unsafe static void ChangeContrast(WriteableBitmap image, int contrastChange, ContrastType contrastType)
        {
            int[] contrastArray = new int[256];

            if (contrastType.Equals(ContrastType.Decrease))
            {
                for (int i = 0; i < 256; i++)
                {
                    contrastArray[i] = (int)(127 + (i - 127) * (255 - 2 * contrastChange) / 255 + 0.5);
                }
            }
            else
            {
                for (int i = 0; i < 256; i++)
                {
                    contrastArray[i] = (int)(127 + (i - 127) * 255 / (255 - 2 * contrastChange) + 0.5);
                }
            }

            ChangePixelsValue(image, contrastArray);
        }

        public unsafe static void ChangePixelsValue(WriteableBitmap image, int[] mask)
        {
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
