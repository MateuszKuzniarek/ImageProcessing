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

namespace ImageProcessing
{
    public class ImageWindowController
    {
        public WriteableBitmap Image { get; private set; }

        public ImageWindowController(WriteableBitmap image)
        {
            Image = image;
        }
    }
}
