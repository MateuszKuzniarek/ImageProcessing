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

namespace ImageProcessing
{
    public class MainWindowController
    {
        public ICommand LoadImageCommand { get; private set; }
        public ICommand SaveImageCommand { get; private set; }
        public ICommand IncreaseBrightnessCommand { get; private set; }
        public ICommand DecreaseBrightnessCommand { get; private set; }
        public ICommand IncreaseContrastCommand { get; private set; }
        public ICommand DecreaseContrastCommand { get; private set; }
        public ICommand NegativeCommand { get; private set; }
        public ICommand ArithmeticFilterCommand { get; private set; }
        public ICommand HistogramCommand { get; private set; }

        public ImageAbstraction SelectedImage { get; set; }
        public ObservableCollection<ImageAbstraction> Images { get; set; } = new ObservableCollection<ImageAbstraction>();
        public int BrightnessChange { get; set; } = 1;
        public int ContrastChange { get; set; } = 1;
        public int NegativeChange { get; set; } = 1;
        public int MaskSize { get; set; } = 3;

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
            ArithmeticFilterCommand = new RelayCommand(x => ArithmeticFilter(MaskSize), x => SelectedImage != null);
        }

        private void ArithmeticFilter(int maskSize)
        {
            ImageOperations.ArithmeticFilter(SelectedImage.Bitmap, maskSize);
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

            if (writeableBitmap.Format != PixelFormats.Bgra32)
            {
                writeableBitmap = new WriteableBitmap(new FormatConvertedBitmap(writeableBitmap, PixelFormats.Bgra32, null, 0));
            }

            Images.Add(new ImageAbstraction(writeableBitmap, bitmapImage.UriSource.Segments.Last()));
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
