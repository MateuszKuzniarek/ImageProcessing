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
        public String selectedOption = "";
        bool isFile = false;
        OpenFileDialog readFile = new OpenFileDialog();
        public SoundProcessing()
        {
            InitializeComponent();
            soundChart.Visible = false;
            chart1.Visible = false;
            chart2.Visible = false;
            button1.Enabled = false;
            button3.Enabled = false;
            createSeries();
        }

        private void createSeries()
        {
            samples = new List<double>();
            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();
            soundChart.Series.Clear();
            soundChart.Series.Add("wave");
            soundChart.Series["wave"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            soundChart.Series["wave"].ChartArea = "ChartArea1";
            chart1.Series.Add("wave");
            chart1.Series["wave"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart1.Series["wave"].ChartArea = "ChartArea1";
            chart2.Series.Add("wave");
            chart2.Series["wave"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart2.Series["wave"].ChartArea = "ChartArea1";
            chart3.Series.Add("wave");
            chart3.Series["wave"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart3.Series["wave"].ChartArea = "ChartArea1";
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
            chart1.Visible = true;
            chart2.Visible = true;
            chart3.Visible = false;
            soundChart.Visible = true;
            selectedOption = "T4";
            if (isFile)
            {
                button3.Enabled = true;
            }
        }

        private void f2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Visible = false;
            chart2.Visible = false;
            soundChart.Visible = false;
            chart3.Visible = true;
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
            NAudio.Wave.WaveChannel32 wave = new NAudio.Wave.WaveChannel32(new NAudio.Wave.WaveFileReader(readFile.FileName));

            byte[] buffer = new byte[65536];
            int read = 0;
            List<TimeSpan> listOftTimes = new List<TimeSpan>();

            while (wave.Position < wave.Length)
            {
                read = wave.Read(buffer, 0, 65536);
                for (int i = 0; i < read / 4; i++)
                {
                    //soundChart.Series["wave"].Points.Add(BitConverter.ToSingle(buffer, i * 4));
                    samples.Add(BitConverter.ToSingle(buffer, i * 4));
                    chart1.Series["wave"].Points.Add(samples[i]);
                    
                }
            }

            List<double> delayedSamples = new List<double>();
            double previusValue;
            double actualValue;
            double nextValue;
            bool isResult = false;
            for (int i = 0; i < samples.Count; ++i)
            {
                double result = 0.0;
                for (int j = 0; j < samples.Count; ++j)
                {
                    var temp = i;
                    if ((j + i) > (samples.Count - 1))
                    {
                        result += Math.Abs(samples[j]);
                    }
                    else
                    {
                        result += Math.Abs(samples[j] - samples[j + i]);
                    }
                }
                delayedSamples.Add(result);
                soundChart.Series["wave"].Points.Add(delayedSamples[i]);
            }
            int sampleNumber = 0;
            List<int> peaks = new List<int>();
            for(int i = 2; i < delayedSamples.Count; ++i)
            {
                previusValue = delayedSamples[i - 2];
                actualValue = delayedSamples[i - 1];
                nextValue = delayedSamples[i];
                if(actualValue > previusValue && actualValue > nextValue)
                {
                    peaks.Add(i);
                    sampleNumber = i;
                }
            }
            int peaksCnt = 0;
            for(int i = 0; i < samples.Count; ++i)
            {
                chart2.Series["wave"].Points.Add(1.0 / (peaks[peaksCnt] * 1.0 / 44100.0));
                if (peaks[peaksCnt] > i && peaksCnt < (peaks.Count -1))
                {
                    ++peaksCnt;
                }
            }
        }
        
        private void F2()
        {
            NAudio.Wave.WaveChannel32 wave = new NAudio.Wave.WaveChannel32(new NAudio.Wave.WaveFileReader(readFile.FileName));

            byte[] buffer = new byte[65536];
            int read = 0;
            List<Complex> samplesComplex = new List<Complex>();
            while (wave.Position < wave.Length)
            {
                read = wave.Read(buffer, 0, 65536);
                for (int i = 0; i < read / 4; i++)
                {
                    //soundChart.Series["wave"].Points.Add(BitConverter.ToSingle(buffer, i * 4));
                    samples.Add(BitConverter.ToSingle(buffer, i * 4));
                    //samplesComplex.Add(new Complex(BitConverter.ToSingle(buffer, i * 4), 0));
                }
            }
            for(int i = 0; i < samples.Count; i++)
            {
                //samplesComplex.Add(new Complex(samples[i], 0));
                samplesComplex.Add(new Complex(samples[i] * 0.5 * (1 - Math.Cos((2 * Math.PI * i) / (2048 - 1))), 0));
            }
            samplesComplex = CutSignalSamplesToPowerOfTwo(samplesComplex);
            List<Complex> wCoefficients = CalculateWCoefficients(samplesComplex.Count, false);
            List<Complex> result = CalculateFastTransform(samplesComplex, wCoefficients, 0);

            List<double> amplitudeSpectrum = new List<double>();
            for (int i = 0; i < result.Count/2; ++i)
            {
                amplitudeSpectrum.Add(Math.Sqrt(result[i].Real * result[i].Real + result[i].Imaginary * result[i].Imaginary));
            }
            List<Complex> spectrumComplex = new List<Complex>();
            for(int i = 0; i < amplitudeSpectrum.Count; ++i)
            {
                amplitudeSpectrum[i] = Math.Log(amplitudeSpectrum[i]);
                spectrumComplex.Add(new Complex(amplitudeSpectrum[i], 0));
            }
            wCoefficients = CalculateWCoefficients(spectrumComplex.Count, false);
            result = CalculateFastTransform(spectrumComplex, wCoefficients, 0);

        
            List<double> amplitudeSpectrum2 = new List<double>();
            for (int i = 0; i < result.Count / 2; ++i)
            {
                amplitudeSpectrum2.Add(Math.Sqrt(result[i].Real * result[i].Real + result[i].Imaginary * result[i].Imaginary));
                chart3.Series["wave"].Points.Add(amplitudeSpectrum2[i]);

                /*if (i > 2)
                {
                    double previusValue = amplitudeSpectrum2[i - 2];
                    double actualValue = amplitudeSpectrum2[i - 1];
                    double nextValue = amplitudeSpectrum2[i];
                    if (actualValue > previusValue && actualValue > nextValue)
                    {
                        return;
                    }
                }*/
            }
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

    }
}
