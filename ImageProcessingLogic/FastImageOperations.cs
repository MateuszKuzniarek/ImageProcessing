using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageProcessingLogic
{
    public static class FastImageOperations
    {
        //private static readonly Dictionary<int, int> coefficientsIndexes = new Dictionary<int, int>() { { -2, 0 }, { -1, 1 }, { 1, 2 } };

        //public unsafe static void ApplyNorthFilter2(WriteableBitmap image)
        //{
        //    Stopwatch sw = Stopwatch.StartNew();

        //    int[,] mask = new int[,] { { 1, 1, 1 }, { 1, -2, 1 }, { -1, -1, -1 } };

        //    int[,] lut = new int[3, 256];

        //    for(int i=0; i<256; i++)
        //    {
        //        lut[coefficientsIndexes[-2], i] = -2 * i;
        //        lut[coefficientsIndexes[-1], i] = -i;
        //        lut[coefficientsIndexes[1] , i] = i;
        //    }

        //    image.Lock();
        //    byte* imagePointer = (byte*)image.BackBuffer;
        //    int memorySize = image.BackBufferStride * image.PixelHeight;
        //    byte[] allocatedMemory = new byte[memorySize];
        //    fixed (byte* imageCopyPointer = &allocatedMemory[0])
        //    {
        //        Buffer.MemoryCopy(imagePointer, imageCopyPointer, memorySize, memorySize);
        //        int stride = image.BackBufferStride;
        //        int offset = ((mask.GetLength(0) - 1) / 2);

        //        for (int i = offset; i < image.PixelHeight - offset; i++)
        //        {
        //            for (int j = offset; j < image.PixelWidth - offset; j++)
        //            {
        //                for (int k = 0; k < ImageOperations.numberOfColors; k++)
        //                {
        //                    int sum = 0;
        //                    for (int a = 0; a < mask.GetLength(0); a++)
        //                    {
        //                        for (int b = 0; b < mask.GetLength(0); b++)
        //                        {
        //                            int xCoordinate = i - offset + a;
        //                            int yCoordinate = j - offset + b;
        //                            int pixelValue = imageCopyPointer[xCoordinate * stride + yCoordinate * ImageOperations.bytesPerPixel + k];
        //                            sum += lut[coefficientsIndexes[mask[a,b]], pixelValue];
        //                        }
        //                    }
        //                    int index = i * stride + j * ImageOperations.bytesPerPixel + k;
        //                    int newValue = (int)(sum);
        //                    newValue = Math.Min(newValue, 255);
        //                    newValue = Math.Max(newValue, 0);
        //                    imagePointer[index] = (byte)newValue;
        //                }
        //            }
        //        }
        //    }

        //    image.AddDirtyRect(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight));
        //    image.Unlock();

        //    sw.Stop();
        //    Console.WriteLine("Fast Elapsed={0}", sw.Elapsed);
        //}

        public unsafe static void ApplyNorthFilter(WriteableBitmap image)
        {
            Stopwatch sw = Stopwatch.StartNew();
            int[,] mask = new int[,] { { 1, 1, 1 }, { 1, -2, 1 }, { -1, -1, -1 } };


            image.Lock();
            byte* imagePointer = (byte*)image.BackBuffer;
            int memorySize = image.BackBufferStride * image.PixelHeight;
            byte[] allocatedMemory = new byte[memorySize];
            fixed (byte* imageCopyPointer = &allocatedMemory[0])
            {
                Buffer.MemoryCopy(imagePointer, imageCopyPointer, memorySize, memorySize);
                int stride = image.BackBufferStride;
                int offset = 1;

                for (int k = 0; k < ImageOperations.numberOfColors; k++)
                {
                    for (int i = offset; i < image.PixelHeight - offset; i++)
                    {
                        int[,] lastProducts = PerformFirstIteration(mask, i, offset, imageCopyPointer, imagePointer, stride, k);
                        for (int j = offset + 1; j < image.PixelWidth - offset; j++)
                        {
                            int sum = 0;
                            lastProducts[0, 0] = lastProducts[0, 1];
                            lastProducts[0, 1] = lastProducts[0, 2];
                            lastProducts[2, 0] = lastProducts[2, 1];
                            lastProducts[2, 1] = lastProducts[2, 2];

                            sum += lastProducts[0, 0];
                            sum += lastProducts[0, 1];
                            sum += lastProducts[2, 0];
                            sum += lastProducts[2, 1];
                            int pixelValue = imageCopyPointer[i * stride + j * ImageOperations.bytesPerPixel + k];
                            lastProducts[1, 1] = pixelValue * (-2);
                            sum += lastProducts[1, 1];
                            pixelValue = imageCopyPointer[i * stride + (j-1) * ImageOperations.bytesPerPixel + k];
                            lastProducts[1, 0] = pixelValue;
                            sum += pixelValue; 

                            int xCoordinate = j + 1;
                            for (int a = 0; a < mask.GetLength(0); a++)
                            {
                                int yCoordinate = i - offset + a;
                                pixelValue = imageCopyPointer[yCoordinate * stride + xCoordinate * ImageOperations.bytesPerPixel + k];
                                lastProducts[a, 2] = pixelValue * mask[a, 2];
                                sum += lastProducts[a, 2];
                            }

                            int index = i * stride + j * ImageOperations.bytesPerPixel + k;
                            int newValue = (int)(sum);
                            newValue = Math.Min(newValue, 255);
                            newValue = Math.Max(newValue, 0);
                            imagePointer[index] = (byte)newValue;
                        }
                    }
                }
            }

            image.AddDirtyRect(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight));
            image.Unlock();
            sw.Stop();
            Console.WriteLine("Fast Elapsed={0}", sw.Elapsed);
        }

        private unsafe static int[,] PerformFirstIteration(int[,] mask, int rowNumber, int offset, byte* imageCopyPointer, byte* imagePointer, int stride, int k)
        {
            int[,] lastMaskValues = new int[3, 3];
            int sum = 0;
            for (int a = 0; a < mask.GetLength(0); a++)
            {
                for (int b = 0; b < mask.GetLength(0); b++)
                {
                    int yCoordinate = rowNumber - offset + b;
                    int xCoordinate = a;
                    int pixelValue = imageCopyPointer[yCoordinate * stride + xCoordinate * ImageOperations.bytesPerPixel + k];
                    lastMaskValues[a, b] = pixelValue * mask[a, b];
                    sum += lastMaskValues[a, b];
                }
            }

            int index = rowNumber * stride + k;
            int newValue = (int)(sum);
            newValue = Math.Min(newValue, 255);
            newValue = Math.Max(newValue, 0);
            imagePointer[index] = (byte)newValue;

            return lastMaskValues;
        }
    }
}
