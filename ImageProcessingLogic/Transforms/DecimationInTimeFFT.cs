using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageProcessingLogic.Transforms
{
    public class DecimationInTimeFFT : TransformStrategy
    {
        private List<Complex> wCoefficients = new List<Complex>();

        public override List<Complex> TransformSignal(List<Complex> signal)
        {
            List<Complex> cutSignal = CutSignalSamplesToPowerOfTwo(signal);
            CalculateWCoefficients(cutSignal.Count);
            List<Complex> result = CalculateFastTransform(cutSignal, 0);

            //I think it's not necessary but I'm not sure so I'll leave it commented
            //foreach (Tuple<double, Complex> point in result.Points)
            //{
            //    point.Item2.Real = point.Item2.Real / signal.Points.Count;
            //    point.Item2.Imaginary = point.Item2.Imaginary / signal.Points.Count;
            //}
            return result;
        }

        public override string ToString()
        {
            return "FFT z decymacją w czasie";
        }

        private List<Complex> CalculateFastTransform(List<Complex> signal, int recursionDepth)
        {
            List<Complex> evenElements = new List<Complex>();
            List<Complex> oddElements = new List<Complex>();
            bool isEven = true;
            foreach (Complex number in signal)
            {
                if (isEven) evenElements.Add(number);
                else oddElements.Add(number);
                isEven = !isEven;
            }

            List<Complex> evenElementsTransformed = evenElements;
            List<Complex> oddElementsTransformed = oddElements;
            if (evenElements.Count > 1) evenElementsTransformed = CalculateFastTransform(evenElements, recursionDepth + 1);
            if (oddElements.Count > 1) oddElementsTransformed = CalculateFastTransform(oddElements, recursionDepth + 1);

            List<Complex> result = new List<Complex>();
            result.AddRange(Enumerable.Repeat(Complex.GetZero(), signal.Count));

            int halfOfSampleCount = signal.Count / 2;
            for (int i = 0; i < evenElements.Count; i++)
            {
                Complex product = Complex.Multiply(wCoefficients[(int)(i * Math.Pow(2, recursionDepth))], oddElementsTransformed[i]);
                result[i] = Complex.Add(evenElementsTransformed[i], product);
                result[i + halfOfSampleCount] = Complex.Subtract(evenElementsTransformed[i], product);
            }
            return result;
        }

        private void CalculateWCoefficients(int vectorSize)
        {
            int halfOfVectorSize = vectorSize / 2;
            for (int i = 0; i < halfOfVectorSize; i++)
            {
                wCoefficients.Add(GetWCoefficient(-i, vectorSize));
            }
        }

        private List<Complex> CutSignalSamplesToPowerOfTwo(List<Complex> signal)
        {
            int powerOfTwo = 1;
            while (powerOfTwo <= signal.Count)
            {
                powerOfTwo *= 2;
            }
            powerOfTwo /= 2;
            List<Complex> result = new List<Complex>();
            result.AddRange(signal.GetRange(0, powerOfTwo));
            return result;
        }
    }
}
