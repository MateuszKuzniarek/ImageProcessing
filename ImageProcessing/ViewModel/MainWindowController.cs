using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using ImageProcessingLogic;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.ComponentModel;
using ImageProcessingLogic.Transforms;
using ImageProcessingLogic.Spectra;
using ImageProcessingLogic.Filters;
using System.Collections.Generic;

namespace ImageProcessing
{
    public class MainWindowController : IDataErrorInfo
    {
        public ICommand LoadImageCommand { get; private set; }
        public ICommand SaveImageCommand { get; private set; }
        public ICommand IncreaseBrightnessCommand { get; private set; }
        public ICommand DecreaseBrightnessCommand { get; private set; }
        public ICommand IncreaseContrastCommand { get; private set; }
        public ICommand DecreaseContrastCommand { get; private set; }
        public ICommand NegativeCommand { get; private set; }
        public ICommand ArithmeticFilterCommand { get; private set; }
        public ICommand MedianFilterCommand { get; private set; }
        public ICommand HistogramCommand { get; private set; }
        public ICommand ModifyHistogramCommand { get; private set; }
        public ICommand ChangeMaskCommand { get; private set; }
        public ICommand CustomFilterCommand { get; private set; }
        public ICommand RosenfeldOperatorCommand { get; private set; }
        public ICommand OpenInNewWindowCommand { get; private set; }
        public ICommand FastNorthCommand { get; private set; }
        public ICommand FastNorthEastCommand { get; private set; }
        public ICommand FastEastCommand { get; private set; }
        public ICommand FastSouthEastCommand { get; private set; }
        public ICommand ShowAmplitudeSpectrumCommand { get; private set; }
        public ICommand ShowPhaseSpectrumCommand { get; private set; }
        public ICommand UseFilterCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
        public ICommand SpectrumFilterCommand { get; set; }
        public ICommand SegmentationCommand { get; set; }

        public ImageAbstraction SelectedImage { get; set; }
        public ObservableCollection<ImageAbstraction> Images { get; set; } = new ObservableCollection<ImageAbstraction>();
        public ObservableCollection<ImageAbstraction> OriginalImages { get; set; } = new ObservableCollection<ImageAbstraction>();
        public int BrightnessChange { get; set; } = 1;
        public int ContrastChange { get; set; } = 1;
        public int NegativeChange { get; set; } = 1;
        public int ArithmeticMaskSize { get; set; } = 3;
        public int MedianMaskSize { get; set; } = 3;
        public int GMin { get; set; } = 50;
        public int GMax { get; set; } = 200;
        public int R { get; set; } = 5;
        public string Filter { get; set; } = "F1";
        public Dictionary<string, Filter> filterDictionary = new Dictionary<string, Filter>();
        public double FilterInput1 { get; set; } = 0;
        public double FilterInput2 { get; set; } = 0;
        public double FilterInput3 { get; set; } = 0;
        public double k { get; set; } = 0;
        public double l { get; set; } = 0;

        public string Error { get => null; }
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "GMin":
                        if (GMin < 0 || GMin > 255 || GMin > GMax)
                        {
                            areHistogramModificationValuesValid = false;
                            return "Incorrect data";
                        }
                        else
                        {
                            areHistogramModificationValuesValid = true;
                        }
                        break;

                    case "GMax":
                        if (GMax < 0 || GMax > 255 || GMin > GMax)
                        {
                            areHistogramModificationValuesValid = false;
                            return "Incorrect data";
                        }
                        else
                        {
                            areHistogramModificationValuesValid = true;
                        }
                        break;
                }

                return string.Empty;
            }
        }

        private bool areHistogramModificationValuesValid = true;

        public MainWindowController()
        {
            LoadImageCommand = new RelayCommand(x => LoadImage());
            SaveImageCommand = new RelayCommand(x => SaveImage(), x => SelectedImage != null);
            IncreaseBrightnessCommand = new RelayCommand(x => ChangeBrightness(BrightnessChange), x => SelectedImage != null);
            DecreaseBrightnessCommand = new RelayCommand(x => ChangeBrightness(-BrightnessChange), x => SelectedImage != null);
            IncreaseContrastCommand = new RelayCommand(x => ChangeContrast(ContrastChange, ContrastType.Increase), x => SelectedImage != null);
            DecreaseContrastCommand = new RelayCommand(x => ChangeContrast(ContrastChange, ContrastType.Decrease), x => SelectedImage != null);
            HistogramCommand = new RelayCommand(x => DisplayHistogram(), x => SelectedImage != null);
            NegativeCommand = new RelayCommand(x => ChangeNegative(), x => SelectedImage != null);
            ArithmeticFilterCommand = new RelayCommand(x => ArithmeticFilter(ArithmeticMaskSize), x => SelectedImage != null);
            MedianFilterCommand = new RelayCommand(x => MedianFilter(MedianMaskSize), x => SelectedImage != null);
            ModifyHistogramCommand = new RelayCommand(x => ModifyHistogram(GMin, GMax), x => SelectedImage != null && areHistogramModificationValuesValid);
            ChangeMaskCommand = new RelayCommand(x => ChangeMask(), x => SelectedImage != null);
            RosenfeldOperatorCommand = new RelayCommand(x => RosenfeldOperator(R), x => (SelectedImage != null));
            OpenInNewWindowCommand = new RelayCommand(x => OpenInNewWindow(), x => (SelectedImage != null));
            FastNorthCommand = new RelayCommand(x => FastNorth(), x => (SelectedImage != null));
            FastNorthEastCommand = new RelayCommand(x => FastNorthEast(), x => (SelectedImage != null));
            FastEastCommand = new RelayCommand(x => FastEast(), x => (SelectedImage != null));
            FastSouthEastCommand = new RelayCommand(x => FastSouthEast(), x => (SelectedImage != null));
            ShowAmplitudeSpectrumCommand = new RelayCommand(x => ShowAmplitudeSpectrum(), x => (SelectedImage != null));
            ShowPhaseSpectrumCommand = new RelayCommand(x => ShowPhaseSpectrum(), x => (SelectedImage != null));
            UseFilterCommand = new RelayCommand(x => UseFilter(), x => SelectedImage != null);
            UndoCommand = new RelayCommand(x => Undo(), x => (SelectedImage != null));
            SpectrumFilterCommand = new RelayCommand(x => SpectrumFilter(), x => SelectedImage != null);
            SegmentationCommand = new RelayCommand(x => Segmentation(), x => SelectedImage != null);
        }

        private void Segmentation()
        {
            ImageOperations.Segmentation(SelectedImage.Bitmap);
        }

        private void SpectrumFilter()
        {
            ImageTransformOperations.ShowFilterEffect(SelectedImage.Bitmap, new DecimationInTimeFFT(), new SpectrumFilter(k, l));
        }

        private void CreateFilterDictionary()
        {
            filterDictionary.Clear();
            filterDictionary.Add("F1", new LowPassFilter(FilterInput1));
            filterDictionary.Add("F2", new HighPassFilter(FilterInput1));
            filterDictionary.Add("F3", new BandPassFilter(FilterInput2, FilterInput1));
            filterDictionary.Add("F4", new BandStopFilter(FilterInput3, FilterInput1));
            filterDictionary.Add("F5", new EdgeDetectionFilter(FilterInput1, FilterInput2, FilterInput3));
        }

        private void Undo()
        {
            int originalImageIndex = 0;
            for(int i = 0; i < Images.Count; i++)
            {
                if (Images[i].Equals(SelectedImage))
                {
                    originalImageIndex = i;
                }
            }
            ImageOperations.Undo(SelectedImage.Bitmap, OriginalImages[originalImageIndex].Bitmap);
        }

        private void ShowAmplitudeSpectrum()
        {
            ImageTransformOperations.ShowTransformedImage(SelectedImage.Bitmap, new DecimationInTimeFFT(), new AmplitudeSpectrum());
        }

        private void ShowPhaseSpectrum()
        {
            ImageTransformOperations.ShowTransformedImage(SelectedImage.Bitmap, new DecimationInTimeFFT(), new PhaseSpectrum());
        }

        private void UseFilter()
        {
            CreateFilterDictionary();
            ImageTransformOperations.ShowFilterEffect(SelectedImage.Bitmap, new DecimationInTimeFFT(), filterDictionary[Filter]);
        }

        private void FastNorth()
        {
            FastImageOperations.ApplyNorthFilter(SelectedImage.Bitmap);
        }

        private void FastNorthEast()
        {
            FastImageOperations.ApplyNorthEastFilter(SelectedImage.Bitmap);
        }

        private void FastEast()
        {
            FastImageOperations.ApplyEastFilter(SelectedImage.Bitmap);
        }

        private void FastSouthEast()
        {
            FastImageOperations.ApplySouthEastFilter(SelectedImage.Bitmap);
        }

        private void OpenInNewWindow()
        {
            ImageWindow imageWindow = new ImageWindow(SelectedImage);
            imageWindow.Show();
        }

        private void RosenfeldOperator(int R)
        {
            ImageOperations.RosenfeldOperator(SelectedImage.Bitmap, R);
        }

        private void ChangeMask()
        {
            MaskWindow maskWindow = new MaskWindow(SelectedImage.Bitmap);
            maskWindow.Show();
        }

        private void ModifyHistogram(int gMin, int gMax)
        {
            ImageOperations.ModifyHistogram(SelectedImage.Bitmap, gMin, gMax);
        }

        private void ArithmeticFilter(int maskSize)
        {
            ImageOperations.ArithmeticFilter(SelectedImage.Bitmap, maskSize);
        }

        private void MedianFilter(int maskSize)
        {
            ImageOperations.MedianFilter(SelectedImage.Bitmap, maskSize);
        }

        private void ChangeNegative()
        {
            ImageOperations.ChangeNegative(SelectedImage.Bitmap);
        }

        private void ChangeContrast(int contrastChange, ContrastType contrastType)
        {
            ImageOperations.ChangeContrast(SelectedImage.Bitmap, contrastChange, contrastType);
        }

        private void ChangeBrightness(int brightnessChange)
        {
            ImageOperations.ChangeBrightness(SelectedImage.Bitmap, brightnessChange);
        }

        private void DisplayHistogram()
        {
            HistogramWindow histogramWindow = new HistogramWindow(SelectedImage.Bitmap);
            histogramWindow.Show();
        }

        private void LoadImage()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == false)
            {
                return;
            }

            BitmapImage bitmapImage = new BitmapImage(new Uri(dlg.FileName));
            WriteableBitmap writeableBitmap = new WriteableBitmap(bitmapImage);
            WriteableBitmap originalWriteableBitamap = new WriteableBitmap(bitmapImage);

            if (writeableBitmap.Format != PixelFormats.Bgra32)
            {
                writeableBitmap = new WriteableBitmap(new FormatConvertedBitmap(writeableBitmap, PixelFormats.Bgra32, null, 0));
                originalWriteableBitamap = new WriteableBitmap(new FormatConvertedBitmap(writeableBitmap, PixelFormats.Bgra32, null, 0));
            }
            
            Images.Add(new ImageAbstraction(writeableBitmap, bitmapImage.UriSource.Segments.Last()));
            OriginalImages.Add(new ImageAbstraction(originalWriteableBitamap, bitmapImage.UriSource.Segments.Last()));
        }

        private void SaveImage()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Bitmap Image(.bmp)| *.bmp";
            if (saveFileDialog.ShowDialog() == false)
            {
                return;
            }

            BitmapEncoder encoder = new PngBitmapEncoder();
            SelectedImage.Name = new Uri(saveFileDialog.FileName).Segments.Last();

            encoder.Frames.Add(BitmapFrame.Create(SelectedImage.Bitmap));
            using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

    }
}
