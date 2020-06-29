using NAudio.Wave;
using SoundProcessing.Exercise4;
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
        public String selectedWindow = "Hamming";
        bool isFile = false;
        int treshhold = 50;
        OpenFileDialog readFile = new OpenFileDialog();
        SaveFileDialog saveFile = new SaveFileDialog();
        List<SoundFragment> generatedSound = new List<SoundFragment>();
        public int M = 2049;
        public int R = 1024;
        public int N = 2560;
        public int L = 1023;
        public int fc = 300;
        public int powerOf2 = 1;
        Manager manager = new Manager();
        Manager managerTime = new Manager();
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
                }
            }
            List<List<double>> listOfFragments = new List<List<double>>();
            List<double> fragment = new List<double>();
            double totalValue = 0.0;
            double average = 0.0;
            for (int i = 0; i < (samples.Count - samples.Count % 2048); ++i)
            {
                totalValue+= samples[i];
            }
            average = Math.Abs(totalValue/samples.Count);
            for (int i = 0; i < (samples.Count - samples.Count%2048); ++i)
            {
                if (Math.Abs(samples[i]) < average*10)
                {
                    fragment.Add(0.00000001);
                }
                else
                {
                    fragment.Add(samples[i]);
                }
                chart3.Series["wave"].Points.Add(samples[i]);
                if (fragment.Count == 2048)
                {
                    listOfFragments.Add(fragment);
                    fragment = new List<double>();
                }
                chart3.Series["wave"].Points.Add(samples[i]);
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
                    if (actualValue < previusValue && actualValue < nextValue)
                    {
                        sampleNumber = j;
                        break;
                    }
                }
                for(int j = 0; j < delayedSamples[i].Count; ++j)
                {
                    if (sampleNumber == 1)
                    {
                        chart5.Series["wave"].Points.Add(0);
                    }
                    else
                    {
                        chart5.Series["wave"].Points.Add(44100 / sampleNumber * 2);
                    }
                    //chart5.Series["wave"].Points.Add(44100/sampleNumber * 2);
                }
                if (sampleNumber == 1)
                {
                    generatedSound.Add(new SoundFragment((int)(0), (2048f / 44100f) * 0.5f));
                }
                else
                {
                    generatedSound.Add(new SoundFragment((int)(44100 / sampleNumber * 2), (2048f / 44100f) * 0.5f));
                }
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

            byte[] buffer = new byte[16384];
            int read = 0;
            List<Complex> samplesComplex = new List<Complex>();
            List<int> zeroFrequency = new List<int>();
            while (wave.Position < wave.Length)
            {
                read = wave.Read(buffer, 0, 16384);
                for (int i = 0; i < read / 4; i++)
                {
                    samples.Add(BitConverter.ToSingle(buffer, i * 4));
                    samplesComplex.Add(new Complex(samples[i], 0));
                }
            }
            samplesComplex = CutSignalSamplesToPowerOfTwo(samplesComplex);
            List<List<Complex>> listOfFragments = new List<List<Complex>>();
            List<Complex> fragment = new List<Complex>();
            double totalValue = 0.0;
            double average = 0.0;
            for (int i = 0; i < (samplesComplex.Count); ++i)
            {
                totalValue += samples[i];
            }
            average = Math.Abs(totalValue / samplesComplex.Count);
            for (int i = 0; i < samplesComplex.Count; i++)
            {
                if (Math.Abs(samples[i]) < average * 10)
                {
                    fragment.Add(new Complex(0.000000000001 * 0.5 * (1 - Math.Cos((2 * Math.PI * i) / (2048 - 1))), 0));
                }
                else
                {
                    fragment.Add(new Complex(samples[i] * 0.5 * (1 - Math.Cos((2 * Math.PI * i) / (2048 - 1))), 0));
                }
                chart3.Series["wave"].Points.Add(samples[i]);
                if (fragment.Count == 2048)
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
                    chart4.Series["wave"].Points.Add(amplitudeSpectrum2[i][k]);
                }
                if (maxIndex == treshhold - 1)
                {
                    chart5.Series["wave"].Points.Add(0);
                    generatedSound.Add(new SoundFragment((int)(0), (2048f / 44100f) * 0.5f));
                }
                else
                {
                    chart5.Series["wave"].Points.Add(44100 / maxIndex);
                    generatedSound.Add(new SoundFragment((int)(44100 / maxIndex), (2048f / 44100f) * 0.5f));
                }
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            treshhold = Int32.Parse(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SoundPlayer player = new SoundPlayer(saveFileName);
            player.PlaySync();
        }

        private void selectOptionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void rectanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedWindow = "Rectangular";
        }

        private void vonHaanWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedWindow = "Haan";
        }

        private void hammingWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedWindow = "Hamming";
        }

        private void MValue_TextChanged(object sender, EventArgs e)
        {
            M = Int32.Parse(MValue.Text);
        }

        private void RValue_TextChanged(object sender, EventArgs e)
        {
            R = Int32.Parse(RValue.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            fc = fc / 2;
            createSeries();
            manager = new Manager();
            managerTime = new Manager();
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
                }
            }
            List<double> lista = new List<double>();
            for(int i = 0; i < 10000; ++i)
            {
                lista.Add(1);
            }
            samples = lista;

            for (int i = 0; i < samples.Count; ++i)
            {
                chart3.Series["wave"].Points.Add(samples[i]);
            }
            N = M + L - 1;
            //time:

            DateTime startTime = DateTime.Now;
            managerTime.CreateWindows(samples, M, N, R, selectedWindow);
            managerTime.LowPassFIlter(L, fc, N, selectedWindow);
            for (int i = 0; i < managerTime.listOfWindows.Count; ++i)
            {
                managerTime.listOfWindows[i] = managerTime.Time(managerTime.listOfWindows[i], L);
            }
            managerTime.AddTime(R, samples.Count);

            for (int i = 0; i < managerTime.result.Count; ++i)
            {
                chart4.Series["wave"].Points.Add(managerTime.result[i]);
            }
            DateTime stopTime = DateTime.Now;
            TimeSpan diff = stopTime - startTime;
            label5.Text = "Time domain duration: " + diff.TotalMilliseconds.ToString() + "ms";

            //frequency:
            DateTime startTime2 = DateTime.Now;
            manager = new Manager();
            manager.CreateWindows(samples, M, N, R, selectedWindow);
            manager.LowPassFIlter(L, fc, N, selectedWindow);
            manager.CreateComplexWindows();
            manager.CreateComplexCoefficient();
            manager.Fill0Coefficients();
            List<Complex> wCoefficients = CalculateWCoefficients(manager.listOfComplexCoefficients.Count, false);
            List<Complex> wCoefficientsWindows = CalculateWCoefficients(manager.listOfComplexWindows[0].Count, false);

            manager.listOfComplexCoefficients =  CalculateFastTransform(manager.listOfComplexCoefficients, wCoefficients, 0);

            for (int i = 0; i < manager.listOfComplexWindows.Count; ++i)
            {
                manager.listOfComplexWindows[i] = CalculateFastTransform(manager.listOfComplexWindows[i], wCoefficientsWindows, 0);
            }
            manager.MultiplySpectrum();


            List<Complex> wReverseCoefficients = CalculateWCoefficients(manager.listOfComplexWindows[0].Count, true);
            for (int i = 0; i < manager.resultComplexWindows.Count; ++i)
            {
                manager.resultComplexWindows[i] = CalculateFastTransform(manager.resultComplexWindows[i], wReverseCoefficients, 0);
                Complex divisor = new Complex(manager.resultComplexWindows[i].Count, 0);
                manager.resultComplexWindows[i] = manager.resultComplexWindows[i].Select(x => x = Complex.Divide(x, divisor)).ToList();
            }

            manager.AddReal(M, R, samples.Count);
            for (int i = 0; i < manager.result.Count; ++i)
            {
                chart5.Series["wave"].Points.Add(manager.result[i]);
                //generatedSound.Add(new SoundFragment((int)(44100 / i * 2), (2048f / 44100f) * 0.5f));
            }
            /*saveFile.Filter = "Wave File (*.wav)|*.wav;";
            if (saveFile.ShowDialog() != DialogResult.OK) return;
            SoundGenerator waveGenerator = new SoundGenerator(generatedSound);
            waveGenerator.Save(saveFile.FileName);
            saveFileName = saveFile.FileName;
            button2.Enabled = true;
            generatedSound = new List<SoundFragment>();*/
            DateTime stopTime2 = DateTime.Now;
            TimeSpan diff2 = stopTime2 - startTime2;
            label6.Text = "Time domain duration: " + diff2.TotalMilliseconds.ToString() + "ms";
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            L = Int32.Parse(textBox4.Text);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            fc = Int32.Parse(textBox5.Text);
        }

        private List<Complex> ExtendSignalSamplesToPowerOfTwo(List<Complex> signal)
        {
            int powerOfTwo = 1;
            while (powerOfTwo <= signal.Count)
            {
                powerOfTwo *= 2;
            }
            List<Complex> result = new List<Complex>();
            for(int i = 0; i < signal.Count; ++i)
            {
                result.Add(signal[i]);
            }
            for(int i = signal.Count; i < powerOfTwo; ++i)
            {
                result.Add(new Complex(0, 0));
            }
            return result;
        }
    }
}
