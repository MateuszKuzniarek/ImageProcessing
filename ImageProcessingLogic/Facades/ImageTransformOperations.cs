using ImageProcessingLogic.Spectra;
using ImageProcessingLogic.Transforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageProcessingLogic
{
    public static class ImageTransformOperations
    {
        public unsafe static void ShowTransformedImage(WriteableBitmap image, TransformStrategy transformStrategy, Spectrum spectrum)
        {
            List<List<List<Complex>>> transform = TransformImage(image, transformStrategy);
            ShowSpectrum(image, transform, spectrum);
            SwapQuadrants(image);
        }

        private unsafe static List<List<List<Complex>>> TransformImage(WriteableBitmap image, TransformStrategy transformStrategy)
        {
            image.Lock();
            byte* imagePointer = (byte*)image.BackBuffer;
            int stride = image.BackBufferStride;

            List<List<List<Complex>>> transformedImage = new List<List<List<Complex>>>();
            for (int c = 0; c < ImageConstants.numberOfColors; c++)
            {
                List<List<Complex>> colorPlane = new List<List<Complex>>();

                for (int i = 0; i < image.PixelHeight; i++)
                {
                    List<Complex> row = new List<Complex>();

                    for (int j = 0; j < image.PixelWidth; j++)
                    {
                        int index = i * stride + j * ImageConstants.bytesPerPixel + c;
                        row.Add(new Complex(imagePointer[index], 0));
                    }

                    colorPlane.Add(transformStrategy.TransformSignal(row));
                }

                transformedImage.Add(colorPlane);
            }

            for (int c = 0; c < ImageConstants.numberOfColors; c++)
            {
                List<List<Complex>> colorPlane = transformedImage[c];
                for (int i = 0; i < image.PixelWidth; i++)
                {
                    List<Complex> column = Enumerable.Range(0, colorPlane.Count).Select(x => colorPlane[x][i]).ToList();
                    List<Complex> transformedColumn = transformStrategy.TransformSignal(column);
                    for (int j = 0; j < colorPlane.Count; j++)
                    {
                        colorPlane[j][i] = transformedColumn[j];
                    }
                }

            }

            image.Unlock();

            return transformedImage;
        }

        private unsafe static void ShowSpectrum(WriteableBitmap image, List<List<List<Complex>>> transform, Spectrum spectrum)
        {
            image.Lock();
            byte* imagePointer = (byte*)image.BackBuffer;
            int stride = image.BackBufferStride;

            for (int c = 0; c < ImageConstants.numberOfColors; c++)
            {
                List<List<Complex>> colorPlane = transform[c];
                double maxTransformValue = colorPlane.SelectMany(x => x).Select(y => spectrum.GetValueForSpectrum(y)).Max();
                double minTransformValue = colorPlane.SelectMany(x => x).Select(y => spectrum.GetValueForSpectrum(y)).Min();

                for (int i = 0; i < image.PixelHeight; i++)
                {
                    for (int j = 0; j < image.PixelWidth; j++)
                    {
                        byte value = (byte)NormalizeToPixelValueUsingLog(spectrum.GetValueForSpectrum(colorPlane[i][j]), minTransformValue, maxTransformValue);
                        int index = i * stride + j * ImageConstants.bytesPerPixel + c;
                        imagePointer[index] = value;
                    }
                }
            }

            image.AddDirtyRect(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight));
            image.Unlock();
        }

        private unsafe static void SwapQuadrants(WriteableBitmap image)
        {
            image.Lock();
            byte* imagePointer = (byte*)image.BackBuffer;
            int stride = image.BackBufferStride;

            int halfOfHeight = image.PixelHeight / 2;
            int halfOfWidth = image.PixelWidth / 2;

            for (int i = 0; i < halfOfHeight; i++)
            {
                for (int j = 0; j < image.PixelWidth; j++)
                {
                    int iToSwapWith = i + halfOfHeight;
                    if (iToSwapWith >= image.PixelHeight)
                    {
                        iToSwapWith -= image.PixelHeight;
                    }

                    int jToSwapWith = j + halfOfWidth;
                    if (jToSwapWith >= image.PixelWidth)
                    {
                        jToSwapWith -= image.PixelWidth;
                    }

                    for (int k = 0; k < ImageConstants.numberOfColors; k++)
                    {
                        int index = i * stride + j * ImageConstants.bytesPerPixel + k;
                        int indexToSwapWith = iToSwapWith * stride + jToSwapWith * ImageConstants.bytesPerPixel + k;
                        byte swappedByte = imagePointer[index];
                        imagePointer[index] = imagePointer[indexToSwapWith];
                        imagePointer[indexToSwapWith] = swappedByte;
                    }
                }
            }

            image.AddDirtyRect(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight));
            image.Unlock();
        }

        private static int NormalizeToPixelValueUsingLog(double value, double min, double max)
        {
            double normalizedMin = 0;
            double normalizedMax = 255;

            double logCoeficient = (Math.Log(1 + (value - min), 2) / (Math.Log(1 + (max - min), 2)));
            int result = (int)(logCoeficient * (normalizedMax - normalizedMin) + normalizedMin);
            return result;
        }
    }
}
