using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CECS545AI_Project1Combs
{
    partial class SolutionView : Form
    {
        private OutputLog log = null;
        private Graph graph = null;
        private int selectedStepIndex;
        private int curGraphStepIndex;

        public SolutionView()
        {
            InitializeComponent();

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
            }
            pictureBox1.Image = bmp;
        }

        public void updateData(OutputLog logIn, Graph graphIn)
        {
            log = logIn;
            graph = graphIn;

            curGraphStepIndex = graph.NumEdges - 1; // Graph is always given to us complete
            UpdateLogView();
            DrawGraph();
        }

        private void DrawGraph()
        {
            if (log == null || graph == null)
            {
                return;
            }

            // Update graph to match current selected step
            UpdateGraph();

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                SolidBrush b = new SolidBrush(Color.Black);
                Pen p = new Pen(b, 2);
                Font f = new Font(FontFamily.GenericMonospace, 10);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;

                // Draw text header
                Rectangle headerRect = new Rectangle(0, 5, bmp.Width, 30);
                g.DrawString(logListBox.Items[selectedStepIndex].ToString(), f, b, headerRect, sf);
                //headerRect.Y += 15;
                //headerRect.Height -= 15;
                //g.DrawString("Total Distance : " + curGraphTotalDistance, f, b, headerRect, sf);

                // Draw Edges
                graph.DrawEdges(g, bmp.Width, bmp.Height, (headerRect.Y + headerRect.Height), 10);

                // Draw Nodes
                graph.DrawCities(g, bmp.Width, bmp.Height, (headerRect.Y + headerRect.Height), 10);
            }
            pictureBox1.Image = bmp;
        }

        private void UpdateGraph()
        {
            if(curGraphStepIndex == selectedStepIndex)
            {
                return;
            }

            while (curGraphStepIndex > selectedStepIndex)
            {
                string curStep = logListBox.Items[curGraphStepIndex].ToString().Trim();

                if (curStep[0] == 'A')
                {
                    int city1ID = logEntryGetCity1ID(curStep);
                    Edge edge = graph.GetEdgeByStartingCity(graph.GetCityByID(city1ID));
                    graph.RemoveEdge(edge);
                }
                else if (curStep[0] == 'B')
                {
                    int city1ID = logEntryGetCity1ID(curStep);
                    int newcityID = logEntryGetInsertedCityID(curStep);
                    Edge edge1 = graph.GetEdgeByStartingCity(graph.GetCityByID(city1ID));
                    Edge edge2 = graph.GetEdgeByStartingCity(graph.GetCityByID(newcityID));
                    graph.RemoveCityFromEdges(edge1, edge2);
                }

                curGraphStepIndex--;
            }
            
            while (curGraphStepIndex < selectedStepIndex)
            {
                string curStep = logListBox.Items[curGraphStepIndex + 1].ToString().Trim();

                if (curStep[0] == 'A')
                {
                    int city1ID = logEntryGetCity1ID(curStep);
                    int city2ID = logEntryGetCity2ID(curStep);
                    graph.AddEdge(new Edge(graph.GetCityByID(city1ID), graph.GetCityByID(city2ID)));
                }
                else if (curStep[0] == 'B')
                {
                    int city1ID = logEntryGetCity1ID(curStep);
                    int newcityID = logEntryGetInsertedCityID(curStep);
                    Edge edge = graph.GetEdgeByStartingCity(graph.GetCityByID(city1ID));
                    City city = graph.GetCityByID(newcityID);
                    graph.BreakCityIntoEdge(edge, city);
                }

                curGraphStepIndex++;
            }
        }

        private void UpdateLogView()
        {
            if(log == null || graph == null)
            {
                return;
            }

            logListBox.BeginUpdate();
            logListBox.Items.Clear();
            foreach (string item in log.readResultDataItems())
            {
                logListBox.Items.Add(item);
            }
            selectedStepIndex = logListBox.Items.Count - 1;
            logListBox.SetSelected(selectedStepIndex, true);
            logListBox.EndUpdate();
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            if (selectedStepIndex <= 0)
            {
                return;
            }
            logListBox.SelectedIndex = --selectedStepIndex;
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            if(selectedStepIndex >= logListBox.Items.Count - 1)
            {
                return;
            }
            logListBox.SelectedIndex = ++selectedStepIndex;
        }

        private void SolutionView_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void SolutionView_Shown(object sender, EventArgs e)
        {
            // To Do: something
        }

        private void logListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            selectedStepIndex = logListBox.SelectedIndex;
            DrawGraph();
        }

        private static int logEntryGetCity1ID(string logEntry)
        {
            string textAndFirstID = logEntry.Split('-')[0];
            return Convert.ToInt32(textAndFirstID.Substring(textAndFirstID.Length - 3));
        }

        private static int logEntryGetCity2ID(string logEntry)
        {
            return Convert.ToInt32(logEntry.Split('-')[1].Substring(0,3));
        }

        private static int logEntryGetInsertedCityID(string logEntry)
        {
            return Convert.ToInt32(logEntry.Substring(10, 4).Trim());
        }
    }
}
