using System.Windows.Media.Imaging;
using ImageProcessingLogic;
using OxyPlot;
using OxyPlot.Series;
using System.ComponentModel;
using OxyPlot.Axes;

namespace ImageProcessing
{
    public class HistogramWindowController : INotifyPropertyChanged
    {
        public PlotModel RedHistogramPlotModel { get; set; } = new PlotModel();
        public PlotModel GreenHistogramPlotModel { get; set; } = new PlotModel();
        public PlotModel BlueHistogramPlotModel { get; set; } = new PlotModel();

        public event PropertyChangedEventHandler PropertyChanged;

        public HistogramWindowController(WriteableBitmap writeableBitmap)
        {
            HistogramData histogramData = ImageOperations.GenerateHistogramData(writeableBitmap);

            ColumnSeries RedHistogramSeries = new ColumnSeries();
            ColumnSeries GreenHistogramSeries = new ColumnSeries();
            ColumnSeries BlueHistogramSeries = new ColumnSeries();

            RedHistogramPlotModel.Series.Add(RedHistogramSeries);
            GreenHistogramPlotModel.Series.Add(GreenHistogramSeries);
            BlueHistogramPlotModel.Series.Add(BlueHistogramSeries);

            for (int i = 0; i < 256; i++)
            {
                RedHistogramSeries.Items.Add(new ColumnItem(histogramData.RedData[i]));
                GreenHistogramSeries.Items.Add(new ColumnItem(histogramData.GreenData[i]));
                BlueHistogramSeries.Items.Add(new ColumnItem(histogramData.BlueData[i]));
            }

            SetupPlots();
        }

        private void SetupPlots()
        {
            RedHistogramPlotModel.InvalidatePlot(true);
            GreenHistogramPlotModel.InvalidatePlot(true);
            BlueHistogramPlotModel.InvalidatePlot(true);

            SetAxes(RedHistogramPlotModel);
            SetAxes(GreenHistogramPlotModel);
            SetAxes(BlueHistogramPlotModel);

            OnPropertyChanged("BlueHistogramPlotModel");
            OnPropertyChanged("GreenHistogramPlotModel");
            OnPropertyChanged("RedHistogramPlotModel");
        }

        private void SetAxes(PlotModel plotModel)
        {
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left
            });
            CategoryAxis xAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = 255,
                MajorStep = 10
            };

            for (int i=0; i<256; i++)
            {
                xAxis.ActualLabels.Add(i.ToString());
            }
            plotModel.Axes.Add(xAxis);
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
