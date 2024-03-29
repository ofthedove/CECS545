﻿using GAF;
using GAF.Extensions;
using GAF.Operators;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AIProject4Nes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Program Constants
        /// <summary>
        /// The maximum amount two floating point values can be different and still be considered equal.
        /// Used for fitness comparisons in the Terminate function
        /// </summary>
        const double MAX_FLOAT_DIFF = 0.000000001;
        const double MAX_ROUTE_LENGTH = 10000;
        const double MIN_ROUTE_LENGTH = 0;
        #endregion

        Random rand = new Random();
        Stopwatch stopwatch;
        BackgroundWorker b;

        #region GA Parameters
        double crossoverProbability = 0.85;
        double mutationProbability = 0.08;
        int elitismPercentage = 5;
        int initialPopulationSize = 50;
        int maxGenerations = 1000;
        int expertPercentage = 20;
        #endregion

        Map map;

        Queue<double> plateauDetectorQueue;
        int plateauDetectorSize = 5;
        Log log;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region UI Helper Methods
        /// <summary>
        /// Reads an input file from a given path name
        /// Outs the contents of the file as a string
        /// Returns true if successful, false otherwise
        /// </summary>
        /// <param name="filepath">Full path of the file to be read</param>
        /// <param name="inputDataText">Out - the contents of the file as a string</param>
        /// <returns>True on success, false on failure</returns>
        private string readInputFile()
        {
            string filepath = inputFilePath.Text;
            string inputDataText = "";

            try
            {
                // Create a stream reader to read the file with
                using (StreamReader inFileReader = new StreamReader(filepath))
                {
                    // Read the contents of the file into the out string
                    inputDataText = inFileReader.ReadToEnd();
                }

                // Success! If we haven't thrown an exception, we were successful. Return true
                return inputDataText;
            }
            // Handle exceptions dealing with a bad file path
            catch (Exception ex) when (ex is ArgumentException || ex is FileNotFoundException || ex is DirectoryNotFoundException)
            {
                throw new ApplicationException("Error: input path is invalid");
            }
            // Handle exceptions dealing with a read error
            catch (IOException ex)
            {
                throw new ApplicationException("Error reading input file: unknown IO error: " + ex.Message);
            }
            // Handle exceptions dealing with a memory error, likely caused by abnormally large input file
            catch (OutOfMemoryException)
            {
                throw new ApplicationException("Error reading input file: program out of memory");
            }
            // All other exceptions are unexpected and should not be handled here
        }

        /// <summary>
        /// Build a Map from the input file
        /// </summary>
        /// <param name="inputDataText">The entire input file as a string</param>
        /// <returns>A Map built from the input file</returns>
        private Map buildMap(string inputDataText)
        {
            // Create the graph we'll populate and return
            MapBuilder mapB = new MapBuilder();

            // Clean up input data
            inputDataText = inputDataText.Substring(inputDataText.IndexOf("NODE_COORD_SECTION") + "NODE_COORD_SECTION".Length);
            inputDataText = inputDataText.Trim();

            // Build the list of cities
            foreach (string line in inputDataText.Split('\n'))
            {
                // Split the line representing our city into individual values
                // The data on the line looks like "cityID xCoord yCoord"
                string[] values = line.Trim().Split(' ');

                // Make sure we actually have three pieces of data. If we don't it's a bad input file, we have to quit
                if (values.Length != 3)
                {
                    throw new ApplicationException("Invalid input data! Line does not contain three values. Bad Line: " + line);
                }

                // Create a city object from the city data we got from the input line
                City newCity;
                try
                {
                    newCity = new City(Convert.ToInt32(values[0]), Convert.ToDouble(values[1]), Convert.ToDouble(values[2]));
                }
                // If the id isn't a number, or a coordinate isn't a double, it's a bad input file, we have to quit
                catch (FormatException)
                {
                    throw new ApplicationException("Invalid input data! Line contains bad values. Bad Line: " + line);
                }

                mapB.AddCity(newCity);
            }

            // Return the graph we created
            return mapB.ToMap();
        }
        #endregion

        #region Background Worker Thread Methods
        private void ShowResultNavigator()
        {
            ResultNavigator rn = new ResultNavigator(log);
            rn.Show();
        }

        private void bw_ProgressChanged(object o, ProgressChangedEventArgs args)
        {
            var gs = args.UserState as GenerationState;
            generationValueLabel.Content = gs.genNum;
            fitnessValueLabel.Content = String.Format("{0,5:0.000}", gs.maxFit);
        }

        private void bw_RunWorkerComplete(object o, RunWorkerCompletedEventArgs args)
        {
            stopwatch.Stop();
            double elapsedTime = (double)stopwatch.ElapsedMilliseconds / 1000.0;

            statusLabel.Content = String.Format("Done in {0:0.000} sec", elapsedTime);
            startButton.IsEnabled = true;

            //log.SaveBrief(@"C:\Users\Andrew\Desktop\output.txt");

            ShowResultNavigator();
        }
        #endregion

        #region UI Event Handlers
        private void fileInputBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                inputFilePath.Text = openFileDialog.FileName;
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            startButton.IsEnabled = false;
            statusLabel.Content = "Running...";

            // Attempt to load map from input file
            try
            {
                string inputFileContents = readInputFile();
                map = buildMap(inputFileContents);
            }
            catch (ApplicationException ex)
            { // If loading fails, display error message and don't attempt to run
                statusLabel.Content = ex.Message;
                startButton.IsEnabled = true;
                return;
            }

            log = new Log(map);

            crossoverProbability = crossoverProbabilitySlider.Value / 100;
            mutationProbability = mutationProbabilitySlider.Value / 100;
            elitismPercentage = Convert.ToInt32(elitismPercentageSlider.Value);
            initialPopulationSize = Convert.ToInt32(populationSizeSlider.Value);
            maxGenerations = Convert.ToInt32(maxGenerationsSlider.Value);
            plateauDetectorSize = Convert.ToInt32(plateauSizeSlider.Value);
            expertPercentage = Convert.ToInt32(expertPercentageSlider.Value);

            // Confirm that expert percentage is high enough to generate at least one expert
            if ((expertPercentage / 100D) * initialPopulationSize < 1)
            { // Won't be able to do WoC, abort the run and report why
                statusLabel.Content = "Expert percentage too low!";
                startButton.IsEnabled = true;
                return;
            }

            plateauDetectorQueue = new Queue<double>();

            var population = GenerateInitialPopulation(initialPopulationSize);

            //create the genetic operators 
            var elite = new Elite(elitismPercentage);

            var crossover = new Crossover(crossoverProbability)
            {
                CrossoverType = CrossoverType.DoublePointOrdered
            };
            crossover.ReplacementMethod = ReplacementMethod.DeleteLast;

            var mutation = new SwapMutate(mutationProbability);

            var ga = new GeneticAlgorithm(population, CalculateFitness);

            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutation);

            ga.OnGenerationComplete += ga_OnGenerationComplete;

            // Timing
            stopwatch = Stopwatch.StartNew();

            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    b = o as BackgroundWorker;
                    ga.Run(TerminateFunction);
                });
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerComplete);
            bw.RunWorkerAsync();
        }
        #endregion

        #region GA Methods
        private Population GenerateInitialPopulation(int initialPopulationSize)
        {
            var population = new Population();
            var cities = map.GetListOfCities();

            for (int i = 0; i < initialPopulationSize; i++)
            {
                var chromosome = new Chromosome();
                foreach(City city in cities)
                {
                    chromosome.Genes.Add(new Gene(city));
                }

                var rnd = GAF.Threading.RandomProvider.GetThreadRandom();
                chromosome.Genes.ShuffleFast(rnd);

                chromosome.Evaluate(CalculateFitness);
                population.Solutions.Add(chromosome);
            }

            return population;
        }

        private void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {
            // Generate the WoC individual for this generation
            var wocSolution = GenerateWoCSolution(e.Population);

            // Add this generation's data to our log
            log.Write(Log.GenerationData.GenDataFromPopulation(e.Generation, stopwatch.Elapsed.TotalSeconds, e.Population, wocSolution));

            // Report the current state of execution back to the main UI thread
            GenerationState gs = new GenerationState() { genNum = e.Generation, maxFit = e.Population.MaximumFitness };
            b.ReportProgress(-1, gs);

            // Maintain lastFiveGens queue
            // Put this generations fitness onto the queue
            plateauDetectorQueue.Enqueue(e.Population.MaximumFitness);
            // Pull 6th last generation from queue, if it exists
            if (plateauDetectorQueue.Count > plateauDetectorSize)
            {
                plateauDetectorQueue.Dequeue();
            }
        }

        private Chromosome GenerateWoCSolution(Population population)
        {
            List<Chromosome> Experts = new List<Chromosome>(population.GetTopPercent(expertPercentage));

            var popularities = new Dictionary<Tuple<City, City>, int>();
            for(int i = 1; i <= map.GetListOfCities().Count; i++)
            {
                for (int j = 1; j <= map.GetListOfCities().Count; j++)
                {
                    if (!(j == i))
                        popularities.Add(new Tuple<City, City>(map.GetCityByID(i), map.GetCityByID(j)), 0);
                }
            }

            var popularities2 = new Dictionary<Tuple<City, City>, int>();

            foreach (Chromosome expert in Experts)
            {
                Gene gene1 = expert.Genes[0];
                Gene gene2;
                for(int i = 0; i < expert.Genes.Count; i++)
                {
                    gene2 = expert.Genes[(i+1)%expert.Genes.Count];
                    
                    var city1 = (City)(gene1.ObjectValue);
                    var city2 = (City)(gene2.ObjectValue);
                    var key = new Tuple<City, City>(city1, city2);

                    if (popularities.ContainsKey(key))
                    {
                        popularities[key]++;
                    }
                    else {
                        popularities.Add(key, 1);
                    }

                    gene1 = gene2;
                }
            }

            Graph graph = new Graph(map);
            while(!graph.IsComplete)
            {
                Tuple<City, City> nextPair = GetNextMostPopular(ref popularities, ref popularities2);
                graph.TryAddEdge(nextPair.Item1, nextPair.Item2);
            }

            if (!graph.IsComplete)
                throw new ApplicationException("I hope this doesn't happen... I'm sorry, it's after 1 am.");

            var wocChromosome = graph.ToChromosome();
            // Doesn't calculate chromosomes fitness, must do it here.
            wocChromosome.Evaluate(CalculateFitness);
            return wocChromosome;
        }

        // Iterate through all entries looking for most popular
        // Once found most popular, delete it from the dictionary and return it
        private Tuple<City, City> GetNextMostPopular(ref Dictionary<Tuple<City, City>, int> popularities, ref Dictionary<Tuple<City, City>, int> popularities2)
        {
            if (popularities.Count == 0) { var temp = popularities; popularities = popularities2; popularities2 = temp; }

            KeyValuePair<Tuple<City, City>, int> currentMostPopular = popularities.ElementAt(0);

            foreach (KeyValuePair<Tuple<City, City>, int> entry in popularities)
            {
                if (entry.Value > currentMostPopular.Value)
                { // If the current entr is most popular, make it current most popular
                    currentMostPopular = entry;
                }
                else if (entry.Value.CompareTo(currentMostPopular.Value) == 0)
                { // If they're tied, choose randomly whether to replace it.
                    // This is not the same as randomly choosing one! If 5 are equal the last one checked
                    //    has a much higher chance of beign chosen than the first. This shouldn't cause problems
                    if(rand.NextDouble() > 0.5) { currentMostPopular = entry; }
                }
            }

            popularities.Remove(currentMostPopular.Key);
            popularities2.Add(currentMostPopular.Key, currentMostPopular.Value);

            return currentMostPopular.Key;
        }

        /// <summary>
        /// Fitness Function
        /// </summary>
        /// <returns>Between 0 and 1 with 1 being being most fit</returns>
        internal double CalculateFitness(Chromosome chromosome)
        {
            double fitnessValue = -1, pathLength = -1;
            if (chromosome != null)
            {
                pathLength = Graph.CalculateRouteLength(map, chromosome);

                fitnessValue = 1 - ((pathLength - MIN_ROUTE_LENGTH) / (MAX_ROUTE_LENGTH - MIN_ROUTE_LENGTH));
            }
            else
            {
                //chromosome is null
                throw new ArgumentNullException("chromosome", "The specified Chromosome is null.");
            }

            // Set the chromosome's tag to be the actual path length, so we can use it later
            chromosome.Tag = pathLength;
            return fitnessValue;
        }

        /// <summary>
        /// End condition checker
        /// </summary>
        /// <returns>True to stop genetic algorithm</returns>
        private bool TerminateFunction(Population population,
            int currentGeneration,
            long currentEvaluation)
        {
            // If we passed the max number of generations, terminate
            if (currentGeneration >= maxGenerations)
                return true;

            // If we found the max possible fitness, terminate
            if (System.Math.Abs(population.MaximumFitness - 1) < MAX_FLOAT_DIFF)
                return true;

            // Only run plateau detector if the size is 2 or bigger. 0 or 1 means don't run
            if (plateauDetectorSize > 1)
            {
                // If we got the same fitness for the last five generations, terminate
                if (plateauDetectorQueue.Count >= plateauDetectorSize) // Only run if we have enough generations worth of data to check
                {
                    double value = plateauDetectorQueue.ElementAt(0); // Get the value of the oldest run
                    bool flag = false; // This flag goes true if we have different values and need to continue
                    for (int i = 1; i < plateauDetectorQueue.Count; i++) // Iterate through the queue
                    {
                        if (System.Math.Abs((double)plateauDetectorQueue.ElementAt(i) - (double)value) > MAX_FLOAT_DIFF)
                        { // If the values are sufficeintly different
                            flag = true; // Flag that we need to keep running
                            break; // No point continuing, break
                        }
                    }
                    return !flag; // If flag is true we need to keep going, if we got here and flag is false we need to terminate
                }
            }

            // We aren't past maxGenerations, we haven't solved the problem, and we don't have enough data to detect a plateu
            // Keep going
            return false;
        }
        #endregion

        private class GenerationState
        {
            public int genNum;
            public double maxFit;
        }
    }
}