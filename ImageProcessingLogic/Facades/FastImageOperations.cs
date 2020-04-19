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

                for (int k = 0; k < ImageConstants.numberOfColors; k++)
                {
                    for (int i = offset; i < image.PixelHeight - offset; i++)
                    {
                        int[,] lastProducts = PerformFirstIteration(mask, i, offset, imageCopyPointer, imagePointer, stride, k);
                        int secondColumnPartialSum = lastProducts[0, 1] + lastProducts[2, 1];
                        int thirdColumnPartialSum = lastProducts[0, 2] + lastProducts[2, 2];
                        for (int j = offset + 1; j < image.PixelWidth - offset; j++)
                        {
                            int sum = 0;
                            sum += secondColumnPartialSum + thirdColumnPartialSum;
                            secondColumnPartialSum = thirdColumnPartialSum;
                            thirdColumnPartialSum = 0;

                            int pixelValue = imageCopyPointer[i * stride + j * ImageConstants.bytesPerPixel + k];
                            sum += pixelValue * (-2);
                            pixelValue = imageCopyPointer[i * stride + (j - 1) * ImageConstants.bytesPerPixel + k];
                            sum += pixelValue;

                            pixelValue = imageCopyPointer[(i - 1) * stride + (j + 1) * ImageConstants.bytesPerPixel + k];
                            thirdColumnPartialSum += pixelValue;
                            sum += pixelValue;
                            pixelValue = imageCopyPointer[i * stride + (j + 1) * ImageConstants.bytesPerPixel + k];
                            sum += pixelValue;
                            pixelValue = imageCopyPointer[(i + 1) * stride + (j + 1) * ImageConstants.bytesPerPixel + k];
                            thirdColumnPartialSum += -pixelValue;
                            sum += -pixelValue;

                            int index = i * stride + j * ImageConstants.bytesPerPixel + k;
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

        public unsafe static void ApplyNorthEastFilter(WriteableBitmap image)
        {
            Stopwatch sw = Stopwatch.StartNew();
            int[,] mask = new int[,] { { 1, 1, 1 }, { -1, -2, 1 }, { -1, -1, 1 } };


            image.Lock();
            byte* imagePointer = (byte*)image.BackBuffer;
            int memorySize = image.BackBufferStride * image.PixelHeight;
            byte[] allocatedMemory = new byte[memorySize];
            fixed (byte* imageCopyPointer = &allocatedMemory[0])
            {
                Buffer.MemoryCopy(imagePointer, imageCopyPointer, memorySize, memorySize);
                int stride = image.BackBufferStride;
                int offset = 1;

                for (int k = 0; k < ImageConstants.numberOfColors; k++)
                {
                    for (int i = offset; i < image.PixelHeight - offset; i++)
                    {
                        int[,] lastProducts = PerformFirstIteration(mask, i, offset, imageCopyPointer, imagePointer, stride, k);
                        int secondColumnPartialSum = lastProducts[0, 1] + lastProducts[2, 1];
                        for (int j = offset + 1; j < image.PixelWidth - offset; j++)
                        {
                            int sum = 0;
                            sum += secondColumnPartialSum;
                            secondColumnPartialSum = 0;

                            int pixelValue = imageCopyPointer[i * stride + j * ImageConstants.bytesPerPixel + k];
                            sum += pixelValue * (-2);
                            pixelValue = imageCopyPointer[(i - 1) * stride + j * ImageConstants.bytesPerPixel + k];
                            secondColumnPartialSum += pixelValue;
                            sum += pixelValue;
                            pixelValue = imageCopyPointer[(i + 1) * stride + j * ImageConstants.bytesPerPixel + k];
                            secondColumnPartialSum += -pixelValue;
                            sum += -pixelValue;

                            pixelValue = imageCopyPointer[(i - 1) * stride + (j + 1) * ImageConstants.bytesPerPixel + k];
                            sum += pixelValue;
                            pixelValue = imageCopyPointer[i * stride + (j + 1) * ImageConstants.bytesPerPixel + k];
                            sum += pixelValue;
                            pixelValue = imageCopyPointer[(i + 1) * stride + (j + 1) * ImageConstants.bytesPerPixel + k];
                            sum += pixelValue;

                            pixelValue = imageCopyPointer[i * stride + (j - 1) * ImageConstants.bytesPerPixel + k];
                            sum += -pixelValue;

                            int index = i * stride + j * ImageConstants.bytesPerPixel + k;
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

        public unsafe static void ApplyEastFilter(WriteableBitmap image)
        {
            Stopwatch sw = Stopwatch.StartNew();
            int[,] mask = new int[,] { { -1, 1, 1 }, { -1, -2, 1 }, { -1, 1, 1 } };


            image.Lock();
            byte* imagePointer = (byte*)image.BackBuffer;
            int memorySize = image.BackBufferStride * image.PixelHeight;
            byte[] allocatedMemory = new byte[memorySize];
            fixed (byte* imageCopyPointer = &allocatedMemory[0])
            {
                Buffer.MemoryCopy(imagePointer, imageCopyPointer, memorySize, memorySize);
                int stride = image.BackBufferStride;
                int offset = 1;

                for (int k = 0; k < ImageConstants.numberOfColors; k++)
                {
                    for (int i = offset; i < image.PixelHeight - offset; i++)
                    {
                        int[,] lastProducts = PerformFirstIteration(mask, i, offset, imageCopyPointer, imagePointer, stride, k);
                        int secondColumnPartialSum = lastProducts[0, 1] + lastProducts[2, 1];
                        int thirdColumnPartialSum = lastProducts[0, 2] + lastProducts[2, 2];
                        for (int j = offset + 1; j < image.PixelWidth - offset; j++)
                        {
                            int sum = 0;
                            sum += -secondColumnPartialSum + thirdColumnPartialSum;
                            secondColumnPartialSum = thirdColumnPartialSum;
                            thirdColumnPartialSum = 0;

                            int pixelValue = imageCopyPointer[i * stride + j * ImageConstants.bytesPerPixel + k];
                            sum += pixelValue * (-2);
                            pixelValue = imageCopyPointer[i * stride + (j - 1) * ImageConstants.bytesPerPixel + k];
                            sum += -pixelValue;

                            pixelValue = imageCopyPointer[(i - 1) * stride + (j + 1) * ImageConstants.bytesPerPixel + k];
                            thirdColumnPartialSum += pixelValue;
                            sum += pixelValue;
                            pixelValue = imageCopyPointer[i * stride + (j + 1) * ImageConstants.bytesPerPixel + k];
                            sum += pixelValue;
                            pixelValue = imageCopyPointer[(i + 1) * stride + (j + 1) * ImageConstants.bytesPerPixel + k];
                            thirdColumnPartialSum += pixelValue;
                            sum += pixelValue;

                            int index = i * stride + j * ImageConstants.bytesPerPixel + k;
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

        public unsafe static void ApplySouthEastFilter(WriteableBitmap image)
        {
            Stopwatch sw = Stopwatch.StartNew();
            int[,] mask = new int[,] { { -1, -1, 1 }, { -1, -2, 1 }, { 1, 1, 1 } };


            image.Lock();
            byte* imagePointer = (byte*)image.BackBuffer;
            int memorySize = image.BackBufferStride * image.PixelHeight;
            byte[] allocatedMemory = new byte[memorySize];
            fixed (byte* imageCopyPointer = &allocatedMemory[0])
            {
                Buffer.MemoryCopy(imagePointer, imageCopyPointer, memorySize, memorySize);
                int stride = image.BackBufferStride;
                int offset = 1;

                for (int k = 0; k < ImageConstants.numberOfColors; k++)
                {
                    for (int i = offset; i < image.PixelHeight - offset; i++)
                    {
                        int[,] lastProducts = PerformFirstIteration(mask, i, offset, imageCopyPointer, imagePointer, stride, k);
                        int secondColumnPartialSum = lastProducts[0, 1] + lastProducts[2, 1];
                        for (int j = offset + 1; j < image.PixelWidth - offset; j++)
                        {
                            int sum = 0;
                            sum += secondColumnPartialSum;
                            secondColumnPartialSum = 0;

                            int pixelValue = imageCopyPointer[i * stride + j * ImageConstants.bytesPerPixel + k];
                            sum += pixelValue * (-2);
                            pixelValue = imageCopyPointer[(i - 1) * stride + j * ImageConstants.bytesPerPixel + k];
                            secondColumnPartialSum += -pixelValue;
                            sum += -pixelValue;
                            pixelValue = imageCopyPointer[(i + 1) * stride + j * ImageConstants.bytesPerPixel + k];
                            secondColumnPartialSum += pixelValue;
                            sum += pixelValue;

                            pixelValue = imageCopyPointer[(i - 1) * stride + (j + 1) * ImageConstants.bytesPerPixel + k];
                            sum += pixelValue;
                            pixelValue = imageCopyPointer[i * stride + (j + 1) * ImageConstants.bytesPerPixel + k];
                            sum += pixelValue;
                            pixelValue = imageCopyPointer[(i + 1) * stride + (j + 1) * ImageConstants.bytesPerPixel + k];
                            sum += pixelValue;

                            pixelValue = imageCopyPointer[i * stride + (j - 1) * ImageConstants.bytesPerPixel + k];
                            sum += -pixelValue;

                            int index = i * stride + j * ImageConstants.bytesPerPixel + k;
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
            int[,] productValues = new int[3, 3];
            int sum = 0;
            for (int a = 0; a < mask.GetLength(0); a++)
            {
                for (int b = 0; b < mask.GetLength(0); b++)
                {
                    int yCoordinate = rowNumber - offset + b;
                    int xCoordinate = a;
                    int pixelValue = imageCopyPointer[yCoordinate * stride + xCoordinate * ImageConstants.bytesPerPixel + k];
                    productValues[a, b] = pixelValue * mask[a, b];
                    sum += productValues[a, b];
                }
            }

            int index = rowNumber * stride + k;
            int newValue = (int)(sum);
            newValue = Math.Min(newValue, 255);
            newValue = Math.Max(newValue, 0);
            imagePointer[index] = (byte)newValue;

            return productValues;
        }
    }
}
