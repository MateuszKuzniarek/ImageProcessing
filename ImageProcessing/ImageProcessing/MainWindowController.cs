using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ImageProcessing
{
    public class MainWindowController
    {
        public ICommand LoadImageCommand { get; private set; }
        public ICommand SaveImageCommand { get; private set; }

        public BitmapImage SelectedImage { get; set; }
        public ObservableCollection<BitmapImage> Images { get; private set; } = new ObservableCollection<BitmapImage>();

        public MainWindowController()
        {
            LoadImageCommand = new RelayCommand(x => LoadImage());
            SaveImageCommand = new RelayCommand(x => SaveImage());
        }

        private void LoadImage()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == false) return;
            Images.Add(new BitmapImage(new Uri(dlg.FileName)));
        }

        private void SaveImage()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Bitmap Image(.bmp)| *.bmp";
            if (saveFileDialog.ShowDialog() == false) return; BitmapEncoder encoder = new PngBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(SelectedImage));
            using (var fileStream = new System.IO.FileStream(saveFileDialog.FileName, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }
    }
}
