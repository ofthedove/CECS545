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
        #endregion Private Member Variables

        #region Constructors
        /// <summary>
        /// MainForm Constructor
        /// Initializes log object
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

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
        /// Reads an connections input file from a given path name
        /// Seperate from readInputFile so log messages can be more descriptive
        /// Outs the contents of the file as a string
        /// Returns true if successful, false otherwise
        /// </summary>
        /// <param name="filepath">Full path of the file to be read</param>
        /// <param name="inputDataText">Out - the contents of the file as a string</param>
        /// <returns>True on success, false on failure</returns>
        private bool readConnectionsFile(string filepath, out string connectionsDataText)
        {
            try
            {
                // Create a stream reader to read the file with
                using (StreamReader inFileReader = new StreamReader(filepath))
                {
                    // Read the contents of the file into the out string
                    connectionsDataText = inFileReader.ReadToEnd();
                }

                // Success! If we haven't thrown an exception, we were successful. Return true
                return true;
            }
            // Handle exceptions dealing with a bad file path
            catch (Exception ex) when (ex is ArgumentException || ex is FileNotFoundException || ex is DirectoryNotFoundException)
            {
                log.writeLogMessage("Error reading connections file: file path is invalid");
            }
            // Handle exceptions dealing with a read error
            catch (IOException ex)
            {
                log.writeLogMessage("Error reading connections file: unknown IO error: " + ex.Message);
            }
            // Handle exceptions dealing with a memory error, likely caused by abnormally large input file
            catch (OutOfMemoryException)
            {
                log.writeLogMessage("Error reading connections file: program out of memory");
            }
            // All other exceptions are unexpected and should not be handled here

            // Failure... Exception was thrown, we failed. Clear out string so we don't send back rubbish and return false
            connectionsDataText = "";
            return false;
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
            // Create strings to hold input data
            string inputData, connectionsData;

            // Only run if input files can be read successfully
            if (readInputFile(inputFilePathTextBox.Text, out inputData)
                && readConnectionsFile(connectionsFilePathTextBox.Text, out connectionsData))
            {
                try
                {
                    // Only run BFS algorithm if it's check box is checked
                    if (runBFSCheckBox.Checked)
                    {
                        // Create a new BFS
                        TSP_BreadthFirst tsp_bfs = new TSP_BreadthFirst(inputData, connectionsData, log);
                        // Inform the user that the calculation is starting
                        log.writeLogMessage("--- Begin BFS Calculation ---");
                        // Supress log updates while the calculation is running, so they don't slow things down
                        log.supressUpdates = true;
                        // Start a stopwatch to time algorithm
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        // Perform calculation
                        tsp_bfs.CalculateBestRoute();
                        // Sop stopwatch
                        stopwatch.Stop();
                        // Stop supressing log updates
                        log.supressUpdates = false;
                        // Refresh display of log
                        outputText.Text = log.readCompleteLog();
                        // Log diagnostic information about run
                        log.writeLogMessage("Best Route Found! Distance: " + tsp_bfs.BestRouteLengthString + "  Route: " + tsp_bfs.BestRouteString);
                        log.writeLogMessage("Calculation required " + (stopwatch.ElapsedMilliseconds / 1000.00).ToString() + " s");
                        log.writeLogMessage("--- BFS Calculation Complete ---");
                    }
                    // Only run DFS algorithm if it's check box is checked
                    if (runDFSCheckBox.Checked)
                    {
                        // Create a new DFS
                        TSP_DepthFirst tsp_dfs = new TSP_DepthFirst(inputData, connectionsData, log);
                        log.writeLogMessage("--- Begin DFS Calculation ---");
                        // Inform the user that the calculation is starting
                        log.supressUpdates = true;
                        // Supress log updates while the calculation is running, so they don't slow things down
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        // Perform calculation
                        tsp_dfs.CalculateBestRoute();
                        // Sop stopwatch
                        stopwatch.Stop();
                        // Stop supressing log updates
                        log.supressUpdates = false;
                        // Refresh display of log
                        outputText.Text = log.readCompleteLog();
                        // Log diagnostic information about run
                        log.writeLogMessage("Best Route Found! Distance: " + tsp_dfs.BestRouteLengthString + "  Route: " + tsp_dfs.BestRouteString);
                        log.writeLogMessage("Calculation required " + (stopwatch.ElapsedMilliseconds / 1000.00).ToString() + " s");
                        log.writeLogMessage("--- DFS Calculation Complete ---");
                    }
                    // If neither check box is checked, inform the user
                    if (!runBFSCheckBox.Checked && !runDFSCheckBox.Checked)
                    {
                        log.writeLogMessage("No Search Pattern Selected! Calculation Aborted");
                    }
                }
                // Thrown by BFS and DFS if input data strings are invalid
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
            Application.Exit();
        }

        /// <summary>
        /// When browse button is pressed, open appropriate file dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inputFileBrowseButton_Click(object sender, EventArgs e)
        {
            // Open dialog
            DialogResult inputFileDialogResult = inputOpenFileDialog.ShowDialog();

            // Handle result. Populate file path text box if file was selected
            if (inputFileDialogResult == DialogResult.OK)
            {
                inputFilePathTextBox.Text = inputOpenFileDialog.FileName;
            }

        }

        /// <summary>
        /// When browse button is pressed, open appropriate file dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectionsFileBrowseButton_Click(object sender, EventArgs e)
        {
            // Open dialog
            DialogResult connectionsFileDialogResult = connectionsOpenFileDialog.ShowDialog();

            // Handle result. Populate file path text box if file was selected
            if (connectionsFileDialogResult == DialogResult.OK)
            {
                connectionsFilePathTextBox.Text = connectionsOpenFileDialog.FileName;
            }
        }

        /// <summary>
        /// When the clear button is pressed, clear the log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearButton_Click(object sender, EventArgs e)
        {
            // Create a new log (old log deleted automatically)
            log = new OutputLog();
            log.OnLogUpdate += new OutputLog.UpdateHandler(OutputLog_Update);

            // Clear the output text area
            outputText.Text = "";
        }
        #endregion Event Handlers
    }
}
