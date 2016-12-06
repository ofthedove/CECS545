using GAF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIProject4Nes
{
    class Graph
    {
        private Map map;

        private List<City> unused;
        private List<City> usedOnce;
        private List<City> usedTwice;

        private List<Edge> edges;

        private bool isComplete = false;

        public bool IsComplete
        {
            get
            {
                return isComplete;
            }
        }

        public Graph(Map map)
        {
            this.map = map;

            unused = map.GetListOfCities();
            usedOnce = new List<City>();
            usedTwice = new List<City>();

            edges = new List<Edge>();
            // need to add edge list and crap too (probably want used and unused cities
        }

        internal void TryAddEdge(City item1, City item2)
        {
            // Attempt to add an edge. Don't create sub-cycles or visit a city twice
            throw new NotImplementedException();

            // Eventually create and add the edge
            edges.Add(new Edge { city1 = item1, city2 = item2 });
            SetIsComplete();
        }

        internal Chromosome ToChromosome()
        {
            // turn the internal list of edges into a list of genes into a chromosome
            throw new NotImplementedException();
        }

        public static double CalculateRouteLength(Map map, Chromosome solution)
        {
            if (solution.Count < 2)
            {
                // We should never have fewer than four cities, so this shouldn't happen. If it does
                // there are probably bigger problems that need to be addressed
                throw new ApplicationException("Too few elements in solution! Can't calculate route length");
            }

            double runningDistance = 0;

            int i = 0;
            City city1 = (City)solution.Genes[i++].ObjectValue;
            City city2 = (City)solution.Genes[i++].ObjectValue;
            if (city1 == null || city2 == null)
            {
                throw new ApplicationException("Fatal error: solution contains non-existant city");
            }
            while (i <= solution.Count + 1)
            {
                runningDistance += city1.DistanceTo(city2);

                city1 = city2;
                city2 = (City)solution.Genes[i++ % solution.Count].ObjectValue;

                if(city2 == null)
                {
                    throw new ApplicationException("Fatal error: solution contains non-existant city");
                }
            }

            return runningDistance;
        }

        public static Bitmap GenerateGraphImage(Map map, Chromosome solution)
        {
            Bitmap bmp = new Bitmap(1000, 1000);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                // Draw Edges
                DrawEdges(map, solution, g, bmp.Width, bmp.Height, 10);

                // Draw Nodes
                map.DrawCities(g, bmp.Width, bmp.Height, 10);
            }
            return bmp;
        }

        public static void DrawEdges(Map map, Chromosome solution, Graphics g, double width, double height, int margin)
        {
            SolidBrush b = new SolidBrush(Color.Red);
            Pen p = new Pen(b, 2);

            double xCoordFactor = (width - margin - margin) / 100D;
            double xCoordOffset = margin;
            double yCoordFactor = (height - margin - margin) / 100D;
            double yCoordOffset = margin;

            if(solution.Count < 2)
            {
                return; // need at least two for this to work
            }

            int i = 0;
            City city1 = (City)solution.Genes[i++].ObjectValue;
            City city2 = (City)solution.Genes[i++].ObjectValue;
            while (i <= solution.Count + 1)
            {
                int startXCoord = (int)((city1.X * xCoordFactor) + xCoordOffset);
                int startYCoord = (int)((city1.Y * yCoordFactor) + yCoordOffset);
                int endXCoord = (int)((city2.X * xCoordFactor) + xCoordOffset);
                int endYCoord = (int)((city2.Y * yCoordFactor) + yCoordOffset);

                g.DrawLine(p, startXCoord, startYCoord, endXCoord, endYCoord);

                city1 = city2;
                city2 = (City)solution.Genes[i++ % solution.Count].ObjectValue;
            }
        }

        private void SetIsComplete()
        {
            if(unused.Count != 0 || usedOnce.Count != 0)
            {
                isComplete = false;
                return;
            }

            City firstCity = edges[0].city1;
            City nextCity = edges[0].city2;
            Edge curEdge;
            while (true)
            {
                foreach(Edge edge in edges)
                {
                    if(edge.city1 == nextCity)
                    {
                        curEdge = edge;
                        break;
                    }
                }
            }
            // returns true if the current graph has a path that goes through all cities exactly once.
            // Probably want an internal class variable for this and set that when adding or removing edges
            throw new NotImplementedException();
            // set the is complete variable whenever the graph is changed
        }

        private class Edge
        {
            public City city1 { get; set; }
            public City city2 { get; set; }
        }
    }
}
