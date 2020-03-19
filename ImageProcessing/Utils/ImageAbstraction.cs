using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace ImageProcessing
{
    public class ImageAbstraction : INotifyPropertyChanged
    {
        private string name;

        public WriteableBitmap Bitmap { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }


        public ImageAbstraction(WriteableBitmap bitmap, string name)
        {
            Bitmap = bitmap;
            Name = name;
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
