using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundProcessing.Exercise4
{
    public class Manager
    {
        public List<List<double>> listOfWindows;
        public Windows windows = new Windows();
        public List<double> listOfCoefficients = new List<double>();
        public List<List<Complex>> listOfComplexWindows = new List<List<Complex>>();
        public List<Complex> listOfComplexCoefficients = new List<Complex>();
        public List<List<Complex>> resultComplexWindows = new List<List<Complex>>();
        public List<double> result = new List<double>();
        public Manager ()
        {

        }

        public void Fill0Coefficients()
        {
            for (int i = listOfComplexCoefficients.Count; i < listOfWindows[0].Count; ++i)
            {
                listOfComplexCoefficients.Add(new Complex(0,0));
            }
        }

        public List<double> Time(List<double> window, int L)
        {
            List<double> newWindow = new List<double>();

            for(int i = 0; i < (L-1); ++i)
            {
                newWindow.Add(0);
            }
            for(int i = 0; i < window.Count; ++i)
            {
                newWindow.Add(window[i]);
            }
            for (int i = 0; i < (L - 1); ++i)
            {
                newWindow.Add(0);
            }

            List<double> newResult = new List<double>();
            for (int i = L - 1; i < newWindow.Count; ++i)
            {
                double temp = 0;
                for(int k = 0; k < L; ++k)
                {
                    temp += newWindow[i - k] * listOfCoefficients[k];
                }
                newResult.Add(temp);
            }
            return newResult;
        }

        public int findPower2(int x)
        {
            int result = 1;
            while (true)
            {
                if(result > x)
                {
                    break;
                }
                else
                {
                    result = result * 2;
                }
            }
            return result;
        }

        public void CreateWindows(List<double> samples, int M, int N, int R, string selectedWindow)
        {
            int numberOfSamples = findPower2(N);

            int numberOfWindows = (samples.Count + numberOfSamples - M)/R;
            List<List<double>> listOfFragments = new List<List<double>>();
            for (int i = 0; i < numberOfWindows; ++i)
            {
                List<double> fragment = new List<double>();
                for (int j = 0; j < M; j++)
                {
                    if (i * R + j < samples.Count)
                    {
                        if (selectedWindow == "Rectangular")
                        {
                            fragment.Add(windows.ReactangularWindowSample(samples[i * R + j]));
                        }
                        else if (selectedWindow == "Haan")
                        {
                            fragment.Add(windows.HaanWindowSample(samples[i * R + j], j, M));
                        }
                        else if (selectedWindow == "Hamming")
                        {
                            fragment.Add(windows.HammingWindowSample(samples[i * R + j], j, M));
                        }
                    }
                    else
                    {
                        fragment.Add(0);
                    }
                }
                for (int j = M; j < numberOfSamples; j++)
                {
                    fragment.Add(0);
                }
                listOfFragments.Add(fragment);
            }
            listOfWindows = listOfFragments;
        }

        public void LowPassFIlter(int L, int fc, int N, string window)
        {
            for(int i = 0; i < L; ++i)
            {
                double value = 1.0 * (2 * fc) / 44100.0;
                int center = (L - 1) / 2;
                if (i == center)
                {
                    listOfCoefficients.Add(value);
                }
                else
                {
                    listOfCoefficients.Add((Math.Sin(Math.PI * value * (i - center)) / (Math.PI * (i - center))));
                }
            }

            if (window == "Rectangular")
            {
                listOfCoefficients = windows.ReactangularWindow(listOfCoefficients);
            }
            else if (window == "Haan")
            {
                listOfCoefficients = windows.HaanWindow(listOfCoefficients);
            }
            else if (window == "Hamming")
            {
                listOfCoefficients = windows.HammingWindow(listOfCoefficients);
            }
        }
        
        public void CreateComplexWindows()
        {
            for(int i = 0; i < listOfWindows.Count; ++i)
            {
                List<Complex> window = new List<Complex>();
                for(int j = 0; j < listOfWindows[i].Count; ++j)
                {
                    Complex complex = new Complex(listOfWindows[i][j], 0);
                    window.Add(complex);
                }
                listOfComplexWindows.Add(window);
            }
        }

        public void CreateComplexCoefficient()
        {
            for(int i = 0; i < listOfCoefficients.Count; ++i)
            {
                Complex complex = new Complex(listOfCoefficients[i], 0);
                listOfComplexCoefficients.Add(complex);
            }
        }

        public void MultiplySpectrum()
        {
            for(int i = 0; i < listOfComplexWindows.Count; ++i)
            {
                List<Complex> resultWindow = new List<Complex>();
                for(int j = 0; j < listOfComplexWindows[i].Count; ++j)
                {
                    resultWindow.Add(Complex.Multiply(listOfComplexWindows[i][j], listOfComplexCoefficients[j]));
                }
                resultComplexWindows.Add(resultWindow);
            }
        }

        public void AddReal(int M, int R, int numberOfSamples)
        {
            result.Add(0);

            for (int i = 0; i < resultComplexWindows.Count; i++)
            {
                for (int j = 0; j < resultComplexWindows[i].Count; j++)
                {
                    if (i * R + j < numberOfSamples)
                    {
                        while(result.Count <= (i * R + j))
                        {
                            result.Add(0);
                        }
                        result[i * R + j] += resultComplexWindows[i][j].Real;
                    }
                }
            }
        }

        public void AddTime(int R, int numberOfSamples)
        {
            result.Add(0);

            for (int i = 0; i < listOfWindows.Count; i++)
            {
                for (int j = 0; j < listOfWindows[i].Count; j++)
                {
                    if (i * R + j < numberOfSamples)
                    {
                        while (result.Count <= (i * R + j))
                        {
                            result.Add(0);
                        }
                        result[i * R + j] += listOfWindows[i][j];
                    }
                }
            }
        }
    }
}
