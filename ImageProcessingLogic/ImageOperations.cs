using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageProcessingLogic
{
    public static class ImageOperations
    {
        public const int bytesPerPixel = 4;
        public const int numberOfColors = 3;
        
        public unsafe static void Undo(WriteableBitmap selectedImage, WriteableBitmap originalImage)
        {
            selectedImage.Lock();
            byte* imagePointer = (byte*)selectedImage.BackBuffer;
            byte* originalImagePointer = (byte*)originalImage.BackBuffer;
            int stride = selectedImage.BackBufferStride;

            for (int i = 0; i < selectedImage.PixelHeight; i++)
            {
                for (int j = 0; j < selectedImage.PixelWidth; j++)
                {
                    foreach (int colorChannel in ColorChannel.All)
                    {
                        int index = i * stride + j * bytesPerPixel + colorChannel;
                        imagePointer[index] = originalImagePointer[index];
                    }
                }
            }

            selectedImage.AddDirtyRect(new Int32Rect(0, 0, selectedImage.PixelWidth, selectedImage.PixelHeight));
            selectedImage.Unlock();
        }

        public unsafe static void RosenfeldOperator(WriteableBitmap image, int R)
        {
            image.Lock();
            byte* imagePointer = (byte*)image.BackBuffer;
            int memorySize = image.BackBufferStride * image.PixelHeight;
            byte[] allocatedMemory = new byte[memorySize];
            fixed (byte* imageCopyPointer = &allocatedMemory[0])
            {
                Buffer.MemoryCopy(imagePointer, imageCopyPointer, memorySize, memorySize);
                int stride = image.BackBufferStride;

                for (int i = 0; i < image.PixelHeight; i++)
                {
                    for (int j = 0; j < image.PixelWidth - (R - 1); j++)
                    {
                        for (int k = 0; k < numberOfColors; k++)
                        {
                            int sum1 = 0;
                            for (int a = 1; a <= R; a++)
                            {
                                int xCoordinate = i;
                                int yCoordinate = j + a -1;
                                int pixelValue = imageCopyPointer[xCoordinate * stride + yCoordinate * bytesPerPixel + k];
                                sum1 += pixelValue;
                            }

                            int sum2 = 0;
                            for (int a = 1; a <= R; a++)
                            {
                                int xCoordinate = i;
                                int yCoordinate = j - a;
                                int pixelValue = imageCopyPointer[xCoordinate * stride + yCoordinate * bytesPerPixel + k];
                                sum2 += pixelValue;
                            }

                            int index = i * stride + j * bytesPerPixel + k;
                            int newValue = (int)(1d/R * (sum1 - sum2));
                            newValue = Math.Min(newValue, 255);
                            newValue = Math.Max(newValue, 0);
                            imagePointer[index] = (byte)newValue;
                        }
                    }
                }
            }

            image.AddDirtyRect(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight));
            image.Unlock();
        }
        
        public unsafe static void CustomFilter(WriteableBitmap image, int[,] maskArray)
        {
            ApplyMask(image, maskArray);
        }

        public unsafe static void ChangeBrightness(WriteableBitmap image, int brightnessChange)
        {
            int[] brightnessArray = new int[256];

            for (int i = 0; i < 256; i++)
            {
                brightnessArray[i] = (int)(i + brightnessChange);
            }
            ChangePixelsValue(image, brightnessArray, ColorChannel.All);
        }

        public unsafe static void ArithmeticFilter(WriteableBitmap image, int maskSize)
        {
            int[,] mask = new int[maskSize, maskSize];
            for(int i=0; i<maskSize; i++)
            {
                for(int j=0; j<maskSize; j++)
                {
                    mask[i,j] = 1;
                }
            }

            double factor = 1d / (maskSize * maskSize);
            ApplyMask(image, mask, factor);
        }

        public unsafe static void MedianFilter(WriteableBitmap image, int maskSize)
        {
            image.Lock();
            byte* imagePointer = (byte*)image.BackBuffer;
            int memorySize = image.BackBufferStride * image.PixelHeight;
            byte[] allocatedMemory = new byte[memorySize];
            fixed(byte* imageCopyPointer = &allocatedMemory[0])
            {
                Buffer.MemoryCopy(imagePointer, imageCopyPointer, memorySize, memorySize);
                int stride = image.BackBufferStride;
                int offset = ((maskSize - 1) / 2);

                for (int i = offset; i < image.PixelHeight - offset; i++)
                {
                    for (int j = offset; j < image.PixelWidth - offset; j++)
                    {
                        for (int k = 0; k < numberOfColors; k++)
                        {
                            List<int> pixelList = new List<int>();
                            for (int a = i - offset; a <= i + offset; a++)
                            {
                                for (int b = j - offset; b <= j + offset; b++)
                                {
                                    pixelList.Add(imageCopyPointer[a * stride + b * bytesPerPixel + k]);
                                }
                            }
                            int index = i * stride + j * bytesPerPixel + k;
                            pixelList.Sort();
                            int middle = pixelList.Count / 2;
                            int median = pixelList.Count % 2 != 0 ? pixelList[middle] : (pixelList[middle] + pixelList[middle - 1]) / 2;
                            imagePointer[index] = (byte)median;
                        }
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

            ChangePixelsValue(image, negativeArray, ColorChannel.All);
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

            ChangePixelsValue(image, contrastArray, ColorChannel.All);
        }

        public unsafe static HistogramData GenerateHistogramData(WriteableBitmap image)
        {
            HistogramData result = new HistogramData();

            byte* imagePointer = (byte*)image.BackBuffer;
            int stride = image.BackBufferStride;

            for (int i = 0; i < image.PixelHeight; i++)
            {
                for (int j = 0; j < image.PixelWidth; j++)
                {
                    int index = i * stride + j * bytesPerPixel;
                    result.BlueData[imagePointer[index]]++;

                    index++;
                    result.GreenData[imagePointer[index]]++;

                    index++;
                    result.RedData[imagePointer[index]]++;
                }
            }

            return result;
        }

        public unsafe static void ModifyHistogram(WriteableBitmap image, int gMin, int gMax)
        {
            int[] redLookUpTable = new int[256];
            int[] greenLookUpTable = new int[256];
            int[] blueLookUpTable = new int[256];

            double valueBase = gMax / gMin;
            int sum = image.PixelHeight * image.PixelWidth;
            HistogramData histogramData = GenerateHistogramData(image);
            for(int i=0; i<256; i++)
            {
                double bluePower = GetPowerForHistogramModification(histogramData.BlueData, i, sum);
                blueLookUpTable[i] = (int) (Math.Pow(valueBase, bluePower) * gMin);
                double redPower = GetPowerForHistogramModification(histogramData.RedData, i, sum);
                redLookUpTable[i] = (int) (Math.Pow(valueBase, redPower) * gMin);
                double greenPower = GetPowerForHistogramModification(histogramData.GreenData, i, sum);
                greenLookUpTable[i] = (int) (Math.Pow(valueBase, greenPower) * gMin);
            }

            ChangePixelsValue(image, redLookUpTable, new List<int>() { ColorChannel.Red });
            ChangePixelsValue(image, blueLookUpTable, new List<int>() { ColorChannel.Blue });
            ChangePixelsValue(image, greenLookUpTable, new List<int>() { ColorChannel.Green });
        }

        private static double GetPowerForHistogramModification(int[] colorHistogramData, int value, int sum)
        {
            double result = 0;
            for(int i=0; i<value; i++)
            {
                result += colorHistogramData[i];
            }
            result /= sum;
            return result;
        }

        public unsafe static void ChangePixelsValue(WriteableBitmap image, int[] lookUpTable, List<int> colorChannels)
        {
            image.Lock();
            byte* imagePointer = (byte*)image.BackBuffer;
            int stride = image.BackBufferStride;

            for (int i = 0; i < image.PixelHeight; i++)
            {
                for (int j = 0; j < image.PixelWidth; j++)
                {
                    foreach(int colorChannel in colorChannels)
                    {
                        int index = i * stride + j * bytesPerPixel + colorChannel;
                        int newValue = lookUpTable[imagePointer[index]];
                        newValue = Math.Min(newValue, 255);
                        newValue = Math.Max(newValue, 0);
                        imagePointer[index] = (byte)newValue;
                    }
                }
            }

            image.AddDirtyRect(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight));
            image.Unlock();
        }

        public unsafe static void ApplyMask(WriteableBitmap image, int[,] mask, double factor = 1)
        {
            Stopwatch sw = Stopwatch.StartNew();

            image.Lock();
            byte* imagePointer = (byte*)image.BackBuffer;
            int memorySize = image.BackBufferStride * image.PixelHeight;
            byte[] allocatedMemory = new byte[memorySize];
            fixed (byte* imageCopyPointer = &allocatedMemory[0])
            {
                Buffer.MemoryCopy(imagePointer, imageCopyPointer, memorySize, memorySize);
                int stride = image.BackBufferStride;
                int offset = ((mask.GetLength(0) - 1) / 2);

                for (int i = offset; i < image.PixelHeight - offset; i++)
                {
                    for (int j = offset; j < image.PixelWidth - offset; j++)
                    {
                        for (int k = 0; k < numberOfColors; k++)
                        {
                            int sum = 0;
                            for (int a = 0; a < mask.GetLength(0); a++)
                            {
                                for (int b = 0; b < mask.GetLength(0); b++)
                                {
                                    int xCoordinate = i - offset + a;
                                    int yCoordinate = j - offset + b;
                                    int pixelValue = imageCopyPointer[xCoordinate * stride + yCoordinate * bytesPerPixel + k];
                                    sum += pixelValue * mask[a, b];
                                }
                            }
                            int index = i * stride + j * bytesPerPixel + k;
                            int newValue = (int)(sum * factor);
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
            Console.WriteLine("Normal Elapsed={0}", sw.Elapsed);
        }
    }
}
