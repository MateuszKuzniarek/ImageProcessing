using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageProcessingLogic.Facades
{
    public static class SegmentationOperations
    {
        public unsafe static List<WriteableBitmap> Segmentation(WriteableBitmap image, int numberOfMasks, int treshhold)
        {
            image.Lock();
            byte* imagePointer = (byte*)image.BackBuffer;
            int stride = image.BackBufferStride;

            Random random = new Random();
            List<int> allFoundPixels = new List<int>();
            List<List<int>> regions = new List<List<int>>();
            for(int i=0; i<numberOfMasks; i++)
            {
                int startI = random.Next(0, image.PixelHeight);
                int startJ = random.Next(0, image.PixelWidth);
                int startIndex = startI * stride + startJ * ImageConstants.bytesPerPixel;
                while (allFoundPixels.Contains(startIndex))
                {
                    startI = random.Next(0, image.PixelHeight);
                    startJ = random.Next(0, image.PixelWidth);
                    startIndex = startI * stride + startJ * ImageConstants.bytesPerPixel;
                }

                List<int> region = FindRegion(startI, startJ, treshhold, image, allFoundPixels);
                allFoundPixels.AddRange(region);
                regions.Add(region);
            }

            image.Unlock();
            return ConvertToWriteableBitmaps(regions, image.PixelWidth, image.PixelWidth);
        }

        private unsafe static List<int> FindRegion(int startI, int startJ, int treshhold, WriteableBitmap image, List<int> allFoundPoints)
        {
            byte* imagePointer = (byte*)image.BackBuffer;
            int stride = image.BackBufferStride;
            int startIndex = startI * stride + startJ * ImageConstants.bytesPerPixel;
            List<int> segmentationValue = GetColorValues(imagePointer, startIndex);
            List<int> foundPixels = new List<int>();

            List<int> visitedPoints = new List<int>();
            visitedPoints.AddRange(allFoundPoints);
            Stack<Tuple<int, int>> stackPoints = new Stack<Tuple<int, int>>();
            stackPoints.Push(new Tuple<int, int>(startI, startJ));
            visitedPoints.Add(startIndex);
            while (stackPoints.Count > 0)
            {
                int actualI = stackPoints.Peek().Item1;
                int actualJ = stackPoints.Peek().Item2;
                int actualIndex = actualI * stride + actualJ * ImageConstants.bytesPerPixel;
                List<int> actualValue = GetColorValues(imagePointer, actualIndex);
                double distance = GetPixelColorDistance(segmentationValue, actualValue);
                stackPoints.Pop();
                if (distance < treshhold)
                {
                    foundPixels.Add(actualIndex);
                    //lewy sąsiad
                    if ((actualJ - 1) >= 0)
                    {
                        int left = actualI * stride + (actualJ - 1) * ImageConstants.bytesPerPixel;
                        if (!visitedPoints.Contains(left))
                        {
                            stackPoints.Push(new Tuple<int, int>(actualI, actualJ - 1));
                            visitedPoints.Add(left);
                        }
                    }
                    //prawy sąsiad
                    if ((actualJ + 1) < image.PixelWidth)
                    {
                        int right = actualI * stride + (actualJ + 1) * ImageConstants.bytesPerPixel;
                        if (!visitedPoints.Contains(right))
                        {
                            stackPoints.Push(new Tuple<int, int>(actualI, actualJ + 1));
                            visitedPoints.Add(right);
                        }
                    }
                    //górny sąsiad
                    if ((actualI - 1) >= 0)
                    {
                        int up = (actualI - 1) * stride + actualJ * ImageConstants.bytesPerPixel;
                        if (!visitedPoints.Contains(up))
                        {
                            stackPoints.Push(new Tuple<int, int>(actualI - 1, actualJ));
                            visitedPoints.Add(up);
                        }
                    }
                    //dolny sąsiad
                    if ((actualI + 1) < image.PixelHeight)
                    {
                        int down = (actualI + 1) * stride + actualJ * ImageConstants.bytesPerPixel;
                        if (!visitedPoints.Contains(down))
                        {
                            stackPoints.Push(new Tuple<int, int>(actualI + 1, actualJ));
                            visitedPoints.Add(down);
                        }
                    }
                }
            }

            return foundPixels;
        }

        private unsafe static List<int> GetColorValues(byte* imagePointer, int startIndex)
        {
            List<int> colorValues = new List<int>();
            colorValues.Add(imagePointer[startIndex]);
            colorValues.Add(imagePointer[startIndex + 1]);
            colorValues.Add(imagePointer[startIndex + 2]);
            return colorValues;
        }

        private static double GetPixelColorDistance(List<int> pixel1, List<int> pixel2)
        {
            double sum = 0;
            for(int i=0; i<ImageConstants.numberOfColors; i++)
            {
                double difference = pixel1[i] - pixel2[i];
                sum += difference * difference;
            }

            return Math.Sqrt(sum);
        }

        private unsafe static void DisplayRegions(WriteableBitmap image, List<int> foundPixels)
        {
            byte* imagePointer = (byte*)image.BackBuffer;
            int stride = image.BackBufferStride;

            for (int i = 0; i < image.PixelHeight; i++)
            {
                for (int j = 0; j < image.PixelWidth; j++)
                {
                    foreach (int colorChannel in ColorChannel.All)
                    {
                        int index = i * stride + j * ImageConstants.bytesPerPixel + colorChannel;
                        imagePointer[index] = (byte)0;
                    }
                }
            }

            foreach (int index in foundPixels)
            {
                imagePointer[index] = (byte)255;
                imagePointer[index + 1] = (byte)255;
                imagePointer[index + 2] = (byte)255;
            }
        }

        private unsafe static List<WriteableBitmap> ConvertToWriteableBitmaps(List<List<int>> regions, int width, int height)
        {
            List<WriteableBitmap> result = new List<WriteableBitmap>();
            foreach(List<int> region in regions)
            {
                WriteableBitmap writeableBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
                writeableBitmap.Lock();
                DisplayRegions(writeableBitmap, region);

                writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight));
                writeableBitmap.Unlock();
                result.Add(writeableBitmap);
            }

            return result;
        }

        public unsafe static void ShowMask(WriteableBitmap image, WriteableBitmap mask)
        {
            image.Lock();
            byte* imagePointer = (byte*)image.BackBuffer;
            int stride = image.BackBufferStride;
            byte* maskPointer = (byte*)mask.BackBuffer;

            for (int i = 0; i < image.PixelHeight; i++)
            {
                for (int j = 0; j < image.PixelWidth; j++)
                {
                    foreach (int colorChannel in ColorChannel.All)
                    {
                        int index = i * stride + j * ImageConstants.bytesPerPixel + colorChannel;
                        if(maskPointer[index] != 0)
                        {
                            imagePointer[index] = (byte)255;
                        }
                    }
                }
            }

            image.AddDirtyRect(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight));
            image.Unlock();
        }
    }
}
