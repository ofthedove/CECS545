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
        private OutputLog log;

        public MainForm()
        {
            InitializeComponent();

            log = new OutputLog();
            log.OnLogUpdate += new OutputLog.UpdateHandler(OutputLog_Update);
        }

        private bool readInputFile(string filepath, out string inputDataText)
        {
            try
            {
                using (StreamReader inFileReader = new StreamReader(filepath))
                {
                    inputDataText = inFileReader.ReadToEnd();
                }

                // Success!
                return true;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is FileNotFoundException || ex is DirectoryNotFoundException)
            {
                log.writeLogMessage("Error reading input file: file path is invalid");
            }
            catch (IOException ex)
            {
                log.writeLogMessage("Error reading input file: unknown IO error: " + ex.Message);
            }
            catch (OutOfMemoryException)
            {
                log.writeLogMessage("Error reading input file: program out of memory");
            }

            // Failure...
            inputDataText = "";
            return false;
        }

        private bool saveOutputFile(string filepath, string outputDataText)
        {
            try
            {
                using (StreamWriter outFileWriter = new StreamWriter(filepath))
                {
                    outFileWriter.Write(outputDataText);
                }

                // Success!
                return true;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is DirectoryNotFoundException || ex is PathTooLongException)
            {
                log.writeLogMessage("Error reading input file: file path is invalid");
            }
            catch (IOException ex)
            {
                log.writeLogMessage("Error reading input file: unknown IO error: " + ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                log.writeLogMessage("Error reading input file: unauthorized access error");
            }

            // Failure...
            return false;
        }

        private void OutputLog_Update(object sender, EventArgs e)
        {
            // TODO should only read new entries
            outputText.Text = log.readCompleteLog();
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            string inputData;
            if (readInputFile(inputFilePathTextBox.Text, out inputData))
            {
                try
                {
                    TSP_BruteForce tsp_bf = new TSP_BruteForce(inputData, log);
                    log.writeLogMessage("--- Begin Calculation ---");
                    log.supressUpdates = true;
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    tsp_bf.CalculateBestRoute();
                    stopwatch.Stop();
                    log.supressUpdates = false;
                    outputText.Text = log.readCompleteLog();
                    log.writeLogMessage("Best Route Found! Distance: " + tsp_bf.BestRouteLengthString + "  Route: " + tsp_bf.BestRouteString);
                    log.writeLogMessage("Calculation required " + (stopwatch.ElapsedMilliseconds / 1000.00).ToString() + " s");
                    log.writeLogMessage("--- Calculation Complete ---");
                }
                catch (ArgumentException ex)
                {
                    log.writeLogMessage(ex.Message);
                    log.writeLogMessage("--- Calculation Aborted ---");
                }

            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            DialogResult outputSaveFileDialogResult = outputSaveFileDialog.ShowDialog();
            if (outputSaveFileDialogResult == DialogResult.OK)
            {
                saveOutputFile(outputSaveFileDialog.FileName, log.readCompleteLog());
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void inputFileBrowseButton_Click(object sender, EventArgs e)
        {
            DialogResult inputFileDialogResult = inputOpenFileDialog.ShowDialog();
            if (inputFileDialogResult == DialogResult.OK)
            {
                inputFilePathTextBox.Text = inputOpenFileDialog.FileName;
            }

        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            log = new OutputLog();
            log.OnLogUpdate += new OutputLog.UpdateHandler(OutputLog_Update);
            outputText.Text = "";
        }
    }
}
