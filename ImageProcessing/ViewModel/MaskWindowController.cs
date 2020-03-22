using ImageProcessingLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ImageProcessing
{
    public class MaskWindowController
    {
        public int MaskSize { get; set; } = 3;
        public int cellSize = 25;
        public ICommand CreateMaskCommand { get; private set; }
        public ICommand SaveMaskCommand { get; private set; }
        public DataGrid MaskTable { get; private set; }
        public MaskWindow Window;

        public MaskWindowController(DataGrid maskTable, MaskWindow maskWindow)
        {
            this.MaskTable = maskTable;
            this.Window = maskWindow;
            CreateMaskCommand = new RelayCommand(x => CreateMask(MaskSize));
            SaveMaskCommand = new RelayCommand(x => SaveMask());
            CreateMask(3);
        }

        private void SaveMask()
        {
            var maskArray = (ObservableCollection<Object>)MaskTable.ItemsSource;
            ImageOperations.maskArray = new int[maskArray.Count, maskArray.Count];
            for(int i = 0; i < maskArray.Count; i++)
            {
                dynamic row = maskArray[i];
                IDictionary<string, object> dictionary = (IDictionary<string, object>)row;
                for (int j = 1; j <= maskArray.Count; j++)
                {
                    ImageOperations.maskArray[i, j-1] = Int32.Parse(dictionary["Col" + j.ToString()].ToString());
                }
            }
            Window.Close();
        }

        private void CreateMask(int maskSize)
        {
            var list = new ObservableCollection<Object>();
            MaskTable.Columns.Clear();
            for (int i = 1; i <= maskSize; i++)
            {
                var anonymousType = new { Col1 = 5, Col2 = 10, Col3 = 11 };
                DataGridTextColumn column = new DataGridTextColumn();
                column.Width = cellSize;
                column.Binding = new Binding("Col" + i.ToString());
                MaskTable.Columns.Add(column);
            }
            for(int i = 0; i < maskSize; i++)
            {
                dynamic data = new ExpandoObject();
                IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
                for(int j = 1; j <= maskSize; j++)
                {
                    dictionary.Add("Col" + j.ToString(), 1);
                }
                list.Add(data);
            }
            MaskTable.HeadersVisibility = 0;
            MaskTable.RowHeight = cellSize;
            MaskTable.ItemsSource = list;
        }
    }
}
