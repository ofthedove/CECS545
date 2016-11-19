using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace CECS545AI_Project1Combs
{
    public partial class MainForm : Form
    {
        #region Private Member Variables
        /// <summary>
        /// The log that tracks data and diagnostic output for the program.
        /// Log entries are displayed in UI and can be saved to disk
        /// </summary>
        private OutputLog log;

        SolutionView solutionView;
        SettingsView settingsView;
        #endregion Private Member Variables

        #region Constructors
        /// <summary>
        /// MainForm Constructor
        /// Initializes log object
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            solutionView = new SolutionView();
            settingsView = new SettingsView();

            log = new OutputLog();
            log.OnLogUpdate += new OutputLog.UpdateHandler(OutputLog_Update);
        }
        #endregion Constructors

        #region Private Methods
        /// <summary>
        /// Reads an input file from a given path name
        /// Outs the contents of the file as a string
        /// Returns true if successful, false otherwise
        /// </summary>
        /// <param name="filepath">Full path of the file to be read</param>
        /// <param name="inputDataText">Out - the contents of the file as a string</param>
        /// <returns>True on success, false on failure</returns>
        private bool readInputFile(string filepath, out string inputDataText)
        {
            try
            {
                // Create a stream reader to read the file with
                using (StreamReader inFileReader = new StreamReader(filepath))
                {
                    // Read the contents of the file into the out string
                    inputDataText = inFileReader.ReadToEnd();
                }

                // Success! If we haven't thrown an exception, we were successful. Return true
                return true;
            }
            // Handle exceptions dealing with a bad file path
            catch (Exception ex) when (ex is ArgumentException || ex is FileNotFoundException || ex is DirectoryNotFoundException)
            {
                log.writeLogMessage("Error reading input file: file path is invalid");
            }
            // Handle exceptions dealing with a read error
            catch (IOException ex)
            {
                log.writeLogMessage("Error reading input file: unknown IO error: " + ex.Message);
            }
            // Handle exceptions dealing with a memory error, likely caused by abnormally large input file
            catch (OutOfMemoryException)
            {
                log.writeLogMessage("Error reading input file: program out of memory");
            }
            // All other exceptions are unexpected and should not be handled here

            // Failure... Exception was thrown, we failed. Clear out string so we don't send back rubbish and return false
            inputDataText = "";
            return false;
        }

        /// <summary>
        /// Build a graph from the input file
        /// </summary>
        /// <param name="inputDataText">The entire input file as a string</param>
        /// <returns>A graph built from the input file</returns>
        private Graph buildGraph(string inputDataText)
        {
            Map map = buildMap(inputDataText);
            Graph graph = map.CreateGraph();
            return graph;
        }

        /// <summary>
        /// Build a map from the input file
        /// </summary>
        /// <param name="inputDataText">The entire input file as a string</param>
        /// <returns>A map built from the input file</returns>
        private Map buildMap(string inputDataText)
        {
            // Create the graph we'll populate and return
            Map map = new Map();

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
                    throw new ArgumentException("Invalid input data! Line does not contain three values. Bad Line: " + line, "inputDataString");
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
                    throw new ArgumentException("Invalid input data! Line contains bad values. Bad Line: " + line, "inputDataString");
                }

                map.AddCity(newCity);
            }

            // Return the map we created
            return map;
        }

        /// <summary>
        /// Save output data to a specified file
        /// </summary>
        /// <param name="filepath">Path of file to save to</param>
        /// <param name="outputDataText">String of data to be saved</param>
        /// <returns>True on success, False on failure</returns>
        private bool saveOutputFile(string filepath, string outputDataText)
        {
            try
            {
                // Create a stream writer to write to the file with
                using (StreamWriter outFileWriter = new StreamWriter(filepath))
                {
                    // Write the passed in data string to the file
                    outFileWriter.Write(outputDataText);
                }

                // Success! If we haven't thrown an exception, we were successful. Return true
                return true;
            }
            // Handle exceptions dealing with a bad file path
            catch (Exception ex) when (ex is ArgumentException || ex is DirectoryNotFoundException || ex is PathTooLongException)
            {
                log.writeLogMessage("Error reading input file: file path is invalid");
            }
            // Handle exceptions dealing with a write error
            catch (IOException ex)
            {
                log.writeLogMessage("Error reading input file: unknown IO error: " + ex.Message);
            }
            // Handle exceptions dealing a permmisions/security error
            catch (UnauthorizedAccessException)
            {
                log.writeLogMessage("Error reading input file: unauthorized access error");
            }
            // All other exceptions are unexpected and should not be handled here

            // Failure... Exception was thrown, we failed. Return false
            return false;
        }

        /// <summary>
        /// Clears an old log and the log output text box
        /// </summary>
        private void clearLog()
        {
            // Create a new log (old log deleted automatically)
            log = new OutputLog();
            log.OnLogUpdate += new OutputLog.UpdateHandler(OutputLog_Update);

            // Clear the output text area
            outputText.Text = "";
        }
        #endregion Private Methods

        #region Event Handlers
        /// <summary>
        /// When log is updated, update display to show new log entries
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputLog_Update(object sender, EventArgs e)
        {
            // TODO should only read new entries, not all entries
            outputText.Text = log.readCompleteLog();
        }

        /// <summary>
        /// When run button is pressed, run specified algorithms and display output
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runButton_Click(object sender, EventArgs e)
        {
            // Clear the log of data from previous runs
            clearLog();

            // Create strings to hold input data
            string inputData;

            // Only run if input files can be read successfully
            if (readInputFile(inputFilePathTextBox.Text, out inputData))
            {
                try
                {
                    // Build a graph from the input file
                    Graph graph = buildGraph(inputData);

                    // Build a popInfo object from the settings file
                    /// TODO : set up the population properties constructor to accept an input file
                    PopulationProperties popInfo = new PopulationProperties();

                    // Create a new GA
                    Population population = new Population(popInfo);

                    // Inform the user that the calculation is starting
                    log.writeLogMessage("--- Begin Calculation ---");
                    // Supress log updates while the calculation is running, so they don't slow things down
                    log.supressUpdates = true;
                    // Start a stopwatch to time algorithm
                    Stopwatch stopwatch = Stopwatch.StartNew();

                    // Should give log an instance of stopwatch so it can timestamp events

                    // Perform calculation
                    //tsp_ce.CalculateBestRoute();

                    // Should take away stopwatch from log so it doesn't try to use it after it's stopped

                    // Sop stopwatch
                    stopwatch.Stop();
                    // Stop supressing log updates
                    log.supressUpdates = false;
                    // Refresh display of log
                    outputText.Text = log.readCompleteLog();
                    // Log diagnostic information about run
                    //log.writeLogMessage("Best Route Found! Distance: " + tsp_ce.BestRouteLengthString + "  Route: " + tsp_ce.BestRouteString);
                    log.writeLogMessage("Calculation required " + (stopwatch.ElapsedMilliseconds / 1000.00).ToString() + " s");
                    log.writeLogMessage("--- Calculation Complete ---");

                    // Update solution viewer with information about this run
                    solutionView.updateData(log, graph);
                }
                // Thrown if input data strings are invalid
                catch (ArgumentException ex)
                {
                    log.writeLogMessage(ex.Message);
                    log.writeLogMessage("--- Calculation Aborted ---");
                }

            }
        }

        /// <summary>
        /// When save button is pressed, save curent contents of log to file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, EventArgs e)
        {
            // Show file select dialog
            DialogResult outputSaveFileDialogResult = outputSaveFileDialog.ShowDialog();

            // If file was selected (Or new file requested to be created)
            if (outputSaveFileDialogResult == DialogResult.OK)
            {
                // Save the current log contents to the specified file
                saveOutputFile(outputSaveFileDialog.FileName, log.readCompleteLog());
            }
        }

        /// <summary>
        /// When close button is pressed, exit application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// When the clear button is pressed, clear the log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearButton_Click(object sender, EventArgs e)
        {
            clearLog();
        }

        private void displayButton_Click(object sender, EventArgs e)
        {
            if (!solutionView.Visible)
            {
                solutionView.Show(this);
            }
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            if(settingsView.IsDisposed)
            {
                settingsView = new SettingsView();
            }

            settingsView.Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            solutionView.Dispose();

            if (!settingsView.IsDisposed)
            {
                settingsView.Dispose();
            }

            e.Cancel = false;
        }

        #endregion Event Handlers
    }
}
