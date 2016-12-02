using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Series;

namespace AIProject4Nes
{
    /// <summary>
    /// Interaction logic for ResultNavigator.xaml
    /// </summary>
    public partial class ResultNavigator : Window
    {
        private Log log;

        private bool isPlotVisible;

        public PlotModel MyModel { get; private set; }

        public ResultNavigator(Log logIn)
        {
            InitializeComponent();
            DataContext = this;

            log = logIn;

            List<string> items = new List<string>();
            for(int i = 0; i < log.Length; i++)
            {
                items.Add(log.ReadShort(i));
            }
            genListBox.ItemsSource = items;

            // Select the last generation in the generation list
            genListBox.SelectedIndex = genListBox.Items.Count - 1;

            // Plot stuff
            var maxSeries = new LineSeries();
            var minSeries = new LineSeries();
            var avgSeries = new LineSeries();

            for (int i = 0; i < log.Length; i++)
            {
                var genData = log.ReadFull(i);
                maxSeries.Points.Add(new DataPoint(genData.GenNum, genData.MaxFitness));
                minSeries.Points.Add(new DataPoint(genData.GenNum, genData.MinFitness));
                avgSeries.Points.Add(new DataPoint(genData.GenNum, genData.AvgFitness));
            }

            MyModel = new PlotModel();
            
            MyModel.Series.Add(maxSeries);
            MyModel.Series.Add(minSeries);
            MyModel.Series.Add(avgSeries);
        }

        private void genListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (genListBox.SelectedItem != null)
            {
                // Hide the plot whenever a generation is selected
                isPlotVisible = false;
                UpdatePlotVisibility();

                // Retrieve the data relating to the selected generation
                int genIndex = genListBox.SelectedIndex;
                Log.GenerationData selectedGen = log.ReadFull(genIndex);

                // Update the general info section
                generationValueLabel.Content = selectedGen.GenNum;
                genTimeValueLabel.Content = String.Format("{0:##0.0}", selectedGen.GenTime);
                wocFitnessValueLabel.Content = String.Format("{0:#####}", selectedGen.WoCFitness);
                maxFitnessValueLabel.Content = String.Format("{0:#####}", selectedGen.MaxFitness);
                minFitnessValueLabel.Content = String.Format("{0:#####}", selectedGen.MinFitness);
                avgFitnessValueLabel.Content = String.Format("{0:#####}", selectedGen.AvgFitness);
                stdDevValueLabel.Content = String.Format("{0:#####}", selectedGen.StdDevFit);

                // ---- Update the board viewer ----
                // Clear the image viewers
                BestFitImage.Source = null;
                LeastFitImage.Source = null;

                // Generate the images
                Bitmap bstImg = Graph.GenerateGraphImage(log.OriginalMap, selectedGen.MostFitSolution);
                Bitmap lstImg = Graph.GenerateGraphImage(log.OriginalMap, selectedGen.LeastFitSolution);

                // Place the best fit image
                BitmapImage bitmapImage = new BitmapImage();
                using (MemoryStream memory = new MemoryStream())
                {
                    bstImg.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                    memory.Position = 0;
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                }
                BestFitImage.Source = bitmapImage;
                
                // Place the lesat fit image
                bitmapImage = new BitmapImage();
                using (MemoryStream memory = new MemoryStream())
                {
                    lstImg.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                    memory.Position = 0;
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                }
                LeastFitImage.Source = bitmapImage;
            }
        }

        private void showPlotButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle plot visibility
            isPlotVisible = !isPlotVisible;

            UpdatePlotVisibility();
        }

        private void UpdatePlotVisibility()
        {
            if (isPlotVisible)
            {
                genDataPlot.Visibility = Visibility.Visible;
                showPlotButton.Content = "Hide Plot";
            }
            else
            {
                genDataPlot.Visibility = Visibility.Hidden;
                showPlotButton.Content = "Show Plot";
            }
        }
    }
}
