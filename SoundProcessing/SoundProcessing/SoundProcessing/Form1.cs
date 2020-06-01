using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoundProcessing
{
    public partial class SoundProcessing : Form
    {
        public List<double> samples = new List<double>();
        public String soundName = "";
        public String saveFileName = "";
        public String selectedOption = "";
        bool isFile = false;
        int treshhold = 50;
        OpenFileDialog readFile = new OpenFileDialog();
        SaveFileDialog saveFile = new SaveFileDialog();
        List<SoundFragment> generatedSound = new List<SoundFragment>();
        public SoundProcessing()
        {
            InitializeComponent();
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            textBox1.Enabled = false;
            textBox1.Text = "50";
            createSeries();
        }

        private void createSeries()
        {
            samples = new List<double>();
            chart3.Series.Clear();
            chart4.Series.Clear();
            chart5.Series.Clear();
            chart3.Series.Add("wave");
            chart3.Series["wave"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart3.Series["wave"].ChartArea = "ChartArea1";
            chart4.Series.Add("wave");
            chart4.Series["wave"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart4.Series["wave"].ChartArea = "ChartArea1";
            chart5.Series.Add("wave");
            chart5.Series["wave"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart5.Series["wave"].ChartArea = "ChartArea1";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            readFile.Filter = "Wave File (*.wav)|*.wav;";
            if (readFile.ShowDialog() != DialogResult.OK) return;
            

            soundName = readFile.FileName;
            string[] path = soundName.Split('\\');
            label1.Text="File name: " + path[path.Length - 1];
            button1.Enabled = true;
            isFile = true;
            if(selectedOption != "")
            {
                button3.Enabled = true;
            }
        }

        private void t4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            selectedOption = "T4";
            if (isFile)
            {
                button3.Enabled = true;
            }
        }

        private void f2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            selectedOption = "F2";
            if (isFile)
            {
                button3.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SoundPlayer player = new SoundPlayer(soundName);
            player.PlaySync();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void T4()
        {
            saveFileName = "";
            NAudio.Wave.WaveChannel32 wave = new NAudio.Wave.WaveChannel32(new NAudio.Wave.WaveFileReader(readFile.FileName));

            byte[] buffer = new byte[44100];
            int read = 0;

            while (wave.Position < wave.Length)
            {
                read = wave.Read(buffer, 0, 44100);
                for (int i = 0; i < read / 4; i++)
                {
                    samples.Add(BitConverter.ToSingle(buffer, i * 4));
                    chart3.Series["wave"].Points.Add(samples[i]);
                }
            }
            double value = samples[samples.Count - 10];
            List<List<double>> listOfFragments = new List<List<double>>();
            List<double> fragment = new List<double>(); 
            for(int i = 0; i < (samples.Count - samples.Count%2048); ++i)
            {
                fragment.Add(samples[i]);
                if(fragment.Count == 2048)
                {
                    listOfFragments.Add(fragment);
                    fragment = new List<double>();
                }
            }

            List<List<double>> delayedSamples = new List<List<double>>();
            double previusValue;
            double actualValue;
            double nextValue;
            for(int i = 0; i < listOfFragments.Count; ++i)
            {
                List<double> delayed = new List<double>();
                for(int j = 0; j < listOfFragments[i].Count; ++j)
                {
                    double result = 0.0;
                    for(int k = 0; k < listOfFragments[i].Count; ++k)
                    {
                        if((k + j) > (listOfFragments[i].Count - 1))
                        {
                            result += Math.Abs(listOfFragments[i][k]);
                        }
                        else
                        {
                            result += Math.Abs(listOfFragments[i][k] - listOfFragments[i][k + j]);
                        }
                    }
                    delayed.Add(result);
                    chart4.Series["wave"].Points.Add(delayed[j]);
                }
                delayedSamples.Add(delayed);
            }
            for(int i = 0; i < delayedSamples.Count; ++i)
            {
                int sampleNumber = 1;
                for (int j = 2; j < delayedSamples[i].Count; ++j)
                {
                    previusValue = delayedSamples[i][j - 2];
                    actualValue = delayedSamples[i][j - 1];
                    nextValue = delayedSamples[i][j];
                    if (actualValue > previusValue && actualValue > nextValue)
                    {
                        sampleNumber = j;
                        break;
                    }
                }
                for(int j = 0; j < delayedSamples[i].Count; ++j)
                {
                    chart5.Series["wave"].Points.Add(1.0 / (sampleNumber * 1.0 / 44100.0));
                }
                generatedSound.Add(new SoundFragment((int)(1.0 / (sampleNumber * 1.0 / 44100.0)), 2048f / 44100f));
            }
            saveFile.Filter = "Wave File (*.wav)|*.wav;";
            if (saveFile.ShowDialog() != DialogResult.OK) return;
            SoundGenerator waveGenerator = new SoundGenerator(generatedSound);
            waveGenerator.Save(saveFile.FileName);
            saveFileName = saveFile.FileName;
            button2.Enabled = true;
            generatedSound = new List<SoundFragment>();
        }
        
        private void F2()
        {
            saveFileName = "";
            NAudio.Wave.WaveChannel32 wave = new NAudio.Wave.WaveChannel32(new NAudio.Wave.WaveFileReader(readFile.FileName));

            byte[] buffer = new byte[44100];
            int read = 0;
            List<Complex> samplesComplex = new List<Complex>();
            while (wave.Position < wave.Length)
            {
                read = wave.Read(buffer, 0, 44100);
                for (int i = 0; i < read / 4; i++)
                {
                    samples.Add(BitConverter.ToSingle(buffer, i * 4));
                    samplesComplex.Add(new Complex(samples[i], 0));
                    chart3.Series["wave"].Points.Add(samples[i]);
                }
            }
            samplesComplex = CutSignalSamplesToPowerOfTwo(samplesComplex);
            for (int i = 0; i < samplesComplex.Count; ++i)
            {
                //chart3.Series["wave"].Points.Add(samples[i]);
            }
            List<List<Complex>> listOfFragments = new List<List<Complex>>();
            List<Complex> fragment = new List<Complex>();
            for (int i = 0; i < samplesComplex.Count; i++)
            {
                fragment.Add(new Complex(samples[i] * 0.5 * (1 - Math.Cos((2 * Math.PI * i) / (2048 - 1))), 0));
                if(fragment.Count == 2048)
                {
                    listOfFragments.Add(fragment);
                    fragment = new List<Complex>();
                }
            }
            List<Complex> wCoefficients = CalculateWCoefficients(2048, false);
            for(int i = 0; i < listOfFragments.Count; ++i)
            {
                listOfFragments[i] = CalculateFastTransform(listOfFragments[i], wCoefficients, 0);
            }

            List<List<double>> amplitudeSpectrum = new List<List<double>>();
            for(int i = 0; i < listOfFragments.Count; ++i)
            {
                List<double> amplitude = new List<double>();
                for (int j = 0; j < listOfFragments[i].Count / 2; ++j)
                {
                    amplitude.Add(Math.Sqrt(listOfFragments[i][j].Real * listOfFragments[i][j].Real + listOfFragments[i][j].Imaginary * listOfFragments[i][j].Imaginary));
                }
                amplitudeSpectrum.Add(amplitude);
            }
            List<List<Complex>> spectrumComplex = new List<List<Complex>>();
            for(int i = 0; i < listOfFragments.Count; ++i)
            {
                List<Complex> spectrum = new List<Complex>();
                for (int j = 0; j < amplitudeSpectrum[i].Count; ++j)
                {
                    amplitudeSpectrum[i][j] = Math.Log(amplitudeSpectrum[i][j]);
                    spectrum.Add(new Complex(amplitudeSpectrum[i][j], 0));
                }
                spectrumComplex.Add(spectrum);
            }
            wCoefficients = CalculateWCoefficients(spectrumComplex[0].Count, false);
            for (int i = 0; i < listOfFragments.Count; ++i)
            {
                listOfFragments[i] = CalculateFastTransform(spectrumComplex[i], wCoefficients, 0);
            }
            List<List<double>> amplitudeSpectrum2 = new List<List<double>>();
            for(int i = 0; i < listOfFragments.Count; ++i)
            {
                List<double> spectrum = new List<double>();
                for (int j = 0; j < listOfFragments[i].Count / 2; ++j)
                {
                    spectrum.Add(Math.Sqrt(listOfFragments[i][j].Real * listOfFragments[i][j].Real + listOfFragments[i][j].Imaginary * listOfFragments[i][j].Imaginary));
                }
                amplitudeSpectrum2.Add(spectrum);
            }
            for (int i = 0; i < listOfFragments.Count; ++i)
            {
                double maxValue = amplitudeSpectrum2[i][treshhold-1];
                int maxIndex = treshhold-1;
                for (int j = treshhold; j < amplitudeSpectrum2[i].Count; ++j)
                {
                    if(amplitudeSpectrum2[i][j] > maxValue)
                    {
                        maxValue = amplitudeSpectrum2[i][j];
                        maxIndex = j;
                    }
                }
                for(int k = 0; k < amplitudeSpectrum2[i].Count; ++k)
                {
                    chart5.Series["wave"].Points.Add(44100/maxIndex);
                    chart4.Series["wave"].Points.Add(amplitudeSpectrum2[i][k]);
                }
                generatedSound.Add(new SoundFragment((int)(44100 / maxIndex), 2048f / 44100f));
            }
            saveFile.Filter = "Wave File (*.wav)|*.wav;";
            if (saveFile.ShowDialog() != DialogResult.OK) return;
            SoundGenerator waveGenerator = new SoundGenerator(generatedSound);
            waveGenerator.Save(saveFile.FileName);
            saveFileName = saveFile.FileName;
            button2.Enabled = true;
            generatedSound = new List<SoundFragment>();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            createSeries();
            if (selectedOption == "T4")
            {
                T4();
            }
            else if (selectedOption == "F2")
            {
                F2();
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
        private List<Complex> CalculateWCoefficients(int vectorSize, bool forReverseTransform)
        {
            List<Complex> result = new List<Complex>();
            int multiplier = forReverseTransform ? -1 : 1;
            int halfOfVectorSize = vectorSize / 2;
            for (int i = 0; i < halfOfVectorSize; i++)
            {
                result.Add(GetWCoefficient(-i * multiplier, vectorSize, true));
            }

            return result;
        }

        private Complex GetWCoefficient(double upperCoefficient, double lowerCoefficient, bool isNegativeExponent)
        {
            Complex result = new Complex();
            double exponent = (2.0 * Math.PI * upperCoefficient) / lowerCoefficient;
            result.Real = Math.Cos(exponent);
            result.Imaginary = Math.Sin(exponent);
            return result;
        }

        private List<Complex> CalculateFastTransform(List<Complex> signal, List<Complex> wCoefficients, int recursionDepth)
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
            if (evenElements.Count > 1) evenElementsTransformed = CalculateFastTransform(evenElements, wCoefficients, recursionDepth + 1);
            if (oddElements.Count > 1) oddElementsTransformed = CalculateFastTransform(oddElements, wCoefficients, recursionDepth + 1);

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

        private List<Complex> ReverseSignalTransform(List<Complex> signal)
        {
            List<Complex> cutSignal = CutSignalSamplesToPowerOfTwo(signal);
            List<Complex> wCoefficients = CalculateWCoefficients(cutSignal.Count, true);
            List<Complex> result = CalculateFastTransform(cutSignal, wCoefficients, 0);
            Complex divisor = new Complex(result.Count, 0);
            result = result.Select(x => x = Complex.Divide(x, divisor)).ToList();
            return result;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            treshhold = Int32.Parse(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SoundPlayer player = new SoundPlayer(saveFileName);
            player.PlaySync();
        }
    }
}
