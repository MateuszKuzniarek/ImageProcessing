using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageProcessing
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FilterList.Items.Add("Low Pass");
            FilterList.Items.Add("High Pass");
            FilterList.Items.Add("Band Pass");
            FilterList.Items.Add("Band Stop");
            FilterList.Items.Add("Edge Detection");
            FilterList.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckFields();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CheckFields()
        {
            if (FilterList.SelectedIndex == 0 || FilterList.SelectedIndex == 1)
            {
                FilterInput2.IsEnabled = false;
                FilterInput3.IsEnabled = false;
                FilterInput2.Text = "--";
                FilterInput3.Text = "--";
                FilterField1.Text = "R: ";
                FilterField2.Text = "";
                FilterField3.Text = "";
            }
            else if (FilterList.SelectedIndex == 2 || FilterList.SelectedIndex == 3)
            {
                FilterInput2.IsEnabled = true;
                FilterInput3.IsEnabled = false;
                FilterInput2.Text = "";
                FilterInput3.Text = "--";
                FilterField1.Text = "R1: ";
                FilterField2.Text = "R2: ";
                FilterField3.Text = "";
            }else if (FilterList.SelectedIndex == 4)
            {
                FilterInput2.IsEnabled = true;
                FilterInput3.IsEnabled = true;
                FilterInput2.Text = "";
                FilterInput3.Text = "";
                FilterField1.Text = "R: ";
                FilterField2.Text = "H: ";
                FilterField3.Text = "A: ";
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MasksList.Items.Clear();
            for(int i = 0; i < Int32.Parse(NumberOfMasks.Text); i++)
            {
                MasksList.Items.Add(i+1);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
