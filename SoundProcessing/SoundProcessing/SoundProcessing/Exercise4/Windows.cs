using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundProcessing.Exercise4
{
    public class Windows
    {
        public Windows()
        {

        }

        public List<double> ReactangularWindow(List<double> listOfSamples)
        {
            return listOfSamples;
        }

        public double ReactangularWindowSample(double sample)
        {
            return sample;
        }

        public List<double> HaanWindow(List<double> listOfSamples)
        {
            for (int i = 0; i < listOfSamples.Count; ++i)
            {
                listOfSamples[i] = listOfSamples[i] * (0.5 - 0.5 * Math.Cos((2 * Math.PI * i) / (listOfSamples.Count)));
            }
            return listOfSamples;
        }
        public double HaanWindowSample(double sample, int index, int M)
        {
            return sample * (0.5 - 0.5 * Math.Cos((2 * Math.PI * index) / (M - 1)));
        }
        public List<double> HammingWindow(List<double> listOfSamples)
        {
            for(int i = 0; i < listOfSamples.Count; ++i)
            {
                listOfSamples[i] = listOfSamples[i] * (0.54 - 0.46 * Math.Cos((2 * Math.PI * i) / (listOfSamples.Count - 1)));
            }
            return listOfSamples;
        }

        public double HammingWindowSample(double sample, int index, int M)
        {
            return sample * (0.54 - 0.46 * Math.Cos((2 * Math.PI * index) / (M - 1)));
        }
    }
}
