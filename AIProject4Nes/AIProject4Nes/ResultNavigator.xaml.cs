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

namespace AIProject4Nes
{
    /// <summary>
    /// Interaction logic for ResultNavigator.xaml
    /// </summary>
    public partial class ResultNavigator : Window
    {
        private Log log;
        
        private List<Bitmap> bstImgList;
        private List<Bitmap> lstImgList;

        public ResultNavigator(Log logIn)
        {
            InitializeComponent();

            log = logIn;

            List<string> items = new List<string>();
            for(int i = 0; i < log.Length; i++)
            {
                items.Add(log.ReadShort(i));
            }
            genListBox.ItemsSource = items;

            // Select the last generation in the generation list
            genListBox.SelectedIndex = genListBox.Items.Count - 1;
        }

        private void genListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (genListBox.SelectedItem != null)
            {
                // Retrieve the data relating to the selected generation
                int genIndex = genListBox.SelectedIndex;
                Log.GenerationData selectedGen = log.ReadFull(genIndex);

                // Update the general info section
                generationValueLabel.Content = selectedGen.GenNum;
                maxFitnessValueLabel.Content = String.Format("{0:0.000}", selectedGen.MaxFitness);
                minFitnessValueLabel.Content = String.Format("{0:0.000}", selectedGen.MinFitness);
                avgFitnessValueLabel.Content = String.Format("{0:0.000}", selectedGen.AvgFitness);
                stdDevValueLabel.Content = String.Format("{0:0.000}", selectedGen.StdDevFit);

                // ---- Update the board viewer ----
                // Clear the image viewers
                BestFitImage.Source = null;
                LeastFitImage.Source = null;

                // Generate the images
                //bstImgList = ClickOMania.GenerateImageList(selectedGen.MostFitSolution, log.OriginalBoard.GetBoardAsArray());
                //lstImgList = ClickOMania.GenerateImageList(selectedGen.LeastFitSolution, log.OriginalBoard.GetBoardAsArray());

                // Reset the sliders to 0
                BestFitSlider.Value = 0;
                LeastFitSlider.Value = 0;

                // Update the slider max values
                BestFitSlider.Maximum = bstImgList.Count - 1;
                LeastFitSlider.Maximum = lstImgList.Count - 1;

                // Set all sliders to their max value
                BestFitSlider.Value = BestFitSlider.Maximum;
                LeastFitSlider.Value = LeastFitSlider.Maximum;
            }
        }

        private void BestFitSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            int value = (int)slider.Value;
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream memory = new MemoryStream())
            {
                bstImgList[value].Save(memory, ImageFormat.Png);
                memory.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }
            BestFitImage.Source = bitmapImage;
        }

        private void LeastFitSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            int value = (int)slider.Value;
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream memory = new MemoryStream())
            {
                lstImgList[value].Save(memory, ImageFormat.Png);
                memory.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }
            LeastFitImage.Source = bitmapImage;
        }
    }
}
