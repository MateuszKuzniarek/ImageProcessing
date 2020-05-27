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
using ImageProcessingLogic.Facades;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace ImageProcessing.ViewModel
{
    public class SoundProcessingMainController : IDataErrorInfo
    {
        public ICommand LoadSoundCommand { get; private set; }

        public string Error => throw new NotImplementedException();

        public string this[string columnName] => throw new NotImplementedException();

        public SoundProcessingMainController()
        {
            LoadSoundCommand = new RelayCommand(x => LoadSound());
        }
        private void LoadSound()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Wave file (*.wav)|*.wav;";
            if (dlg.ShowDialog() == false)
            {
                return;
            }

            
        }
    }
}
