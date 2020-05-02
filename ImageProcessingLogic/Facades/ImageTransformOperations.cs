using ImageProcessingLogic.Filters;
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
            SwapQuadrants(transform);
            ShowSpectrum(image, transform, spectrum);


            //SwapQuadrants(transform);
            //ReverseTransform(transform, transformStrategy);
            //SwapImage(image, transform);
        }

        public unsafe static void ShowFilterEffect(WriteableBitmap image, TransformStrategy transformStrategy, Filter filter)
        {
            List<List<List<Complex>>> transform = TransformImage(image, transformStrategy);
            SwapQuadrants(transform);
            //int[][] mask = new int[][];
            transform.ForEach(x => filter.ApplyFilter(x));
            //

            //transform.ForEach(x => new LowPassFilter(40).ApplyFilter(x));


            //ShowSpectrum(image, transform, new AmplitudeSpectrum());
            //transform.ForEach(x => filter.CreateMask(x, mask));
            SwapQuadrants(transform);
            ReverseTransform(transform, transformStrategy);
            SwapImage(image, transform);
        }

        private unsafe static void ReverseTransform(List<List<List<Complex>>> transform, TransformStrategy transformStrategy)
        {
            for (int c = 0; c < ImageConstants.numberOfColors; c++)
            {
                List<List<Complex>> colorPlane = transform[c];

                for (int i = 0; i < colorPlane.Count; i++)
                {
                    colorPlane[i] = transformStrategy.ReverseSignalTransform(colorPlane[i]);
                }

                for (int i = 0; i < colorPlane[0].Count; i++)
                {
                    List<Complex> column = Enumerable.Range(0, colorPlane.Count).Select(x => colorPlane[x][i]).ToList();
                    List<Complex> transformedColumn = transformStrategy.ReverseSignalTransform(column);
                    for (int j = 0; j < colorPlane.Count; j++)
                    {
                        colorPlane[j][i] = transformedColumn[j];
                    }
                }
            }
        }

        private unsafe static void SwapImage(WriteableBitmap image, List<List<List<Complex>>> newImage)
        {
            image.Lock();
            byte* imagePointer = (byte*)image.BackBuffer;
            int stride = image.BackBufferStride;

            for (int c = 0; c < ImageConstants.numberOfColors; c++)
            {
                List<List<Complex>> colorPlane = newImage[c];

                for (int i = 0; i < image.PixelHeight; i++)
                {
                    for (int j = 0; j < image.PixelWidth; j++)
                    {
                        byte value = (byte) colorPlane[i][j].Real;
                        int index = i * stride + j * ImageConstants.bytesPerPixel + c;
                        imagePointer[index] = value;
                    }
                }
            }

            image.AddDirtyRect(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight));
            image.Unlock();
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
            double xx = imagePointer[10000];

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
                        if(index == 10000)
                        {

                        }
                    }
                }
            }

            image.AddDirtyRect(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight));
            image.Unlock();
        }

        private unsafe static void SwapQuadrants(List<List<List<Complex>>> transform)
        {
            int Height = transform[0].Count;
            int Width = transform[0][0].Count;
            int halfOfHeight = Height / 2;
            int halfOfWidth = Width / 2;

            for (int i = 0; i < halfOfHeight; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    int iToSwapWith = i + halfOfHeight;
                    if (iToSwapWith >= Height)
                    {
                        iToSwapWith -= Height;
                    }

                    int jToSwapWith = j + halfOfWidth;
                    if (jToSwapWith >= Width)
                    {
                        jToSwapWith -= Width;
                    }

                    for (int k = 0; k < ImageConstants.numberOfColors; k++)
                    {
                        Complex swappedElement = transform[k][i][j];
                        transform[k][i][j] = transform[k][iToSwapWith][jToSwapWith];
                        transform[k][iToSwapWith][jToSwapWith] = swappedElement;
                    }
                }
            }
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
