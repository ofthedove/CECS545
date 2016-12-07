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

        private List<City> hasBeenFirst;
        private List<City> hasNotBeenFirst;
        private List<City> hasBeenSecond;
        private List<City> hasNotBeenSecond;

        private List<Edge> edges;

        private bool isComplete = false;

        public bool IsComplete { get { return isComplete; } }

        public Graph(Map map)
        {
            this.map = map;

            hasBeenFirst = new List<City>();
            hasNotBeenFirst = new List<City>(map.GetListOfCities());
            hasBeenSecond = new List<City>();
            hasNotBeenSecond = new List<City>(map.GetListOfCities());

            edges = new List<Edge>();
        }

        // Attempt to add an edge. Don't create sub-cycles or visit a city twice
        internal bool TryAddEdge(City item1, City item2)
        {
            // Check to see if we can use the first city
            if (hasBeenFirst.Contains(item1))
            {
                return false; // Fails b/c city 1 is already used
            }

            if (hasBeenSecond.Contains(item2))
            {
                return false; // Fails b/c city 2 is already used
            }

            // Create the edge and check if it makes a cycle
            Edge edge = new Edge { city1 = item1, city2 = item2 };

            // check for cycle, but only if at least one city exists
            if (edges.Count > 0)
            {
                var checkedCities = new List<City>();

                City firstCity = edges[0].city1;
                City nextCity = edges[0].city2;
                checkedCities.Add(firstCity);
                Edge curEdge = null;
                while (true)
                {
                    foreach (Edge edge_i in edges)
                    {
                        if (edge_i.city1 == nextCity)
                        {
                            curEdge = edge_i;
                            break;
                        }
                    }

                    // If we didn't find the edge, this graph doesn't form any cycle, we're good
                    if (curEdge == null) { break; }

                    if (checkedCities.Contains(nextCity)) { return false; }// throw new ArgumentException("'used' lists out of sync!"); } // Something with the used lists failed big time
                    checkedCities.Add(nextCity);

                    nextCity = curEdge.city2;

                    if (nextCity.CompareTo(firstCity) == 0) // This is only okay if the graph is complete. If the graph were complete we would have failed the 'usedTwice' checks above.
                    { // Therefore, this mean's there's a cycle. Don't add this edge
                        return false;
                    }

                    curEdge = null;
                }
            }

            // No cycle, add the edge
            edges.Add(edge);

            Console.WriteLine("Add edge from {0} to {1}", edge.city1.ID, edge.city2.ID);

            // Update the 'hasBeen' lists
            hasNotBeenFirst.Remove(item1);
            hasBeenFirst.Add(item1);

            hasNotBeenSecond.Remove(item2);
            hasBeenSecond.Add(item2);

            SetIsComplete(); // Check to see if the graph is now complete

            return true;
        }

        // turn the internal list of edges into a list of genes into a chromosome
        internal Chromosome ToChromosome()
        {
            var retVal = new Chromosome();

            // Can't run if it's not a complete graph
            if(!IsComplete) { return null; }

            City firstCity = edges[0].city1;
            City nextCity = edges[0].city2;
            Edge curEdge = null;
            retVal.Genes.Add(new Gene(firstCity));
            while (true)
            {
                foreach (Edge edge in edges)
                {
                    if (edge.city1 == nextCity)
                    {
                        curEdge = edge;
                        break;
                    }
                }

                // If we didn't find the edge, this graph isn't complete, which means there are big problems somewhere
                if (curEdge == null) { throw new ApplicationException("Big problem!");  }

                retVal.Genes.Add(new Gene(nextCity));

                nextCity = curEdge.city2;

                if (nextCity.CompareTo(firstCity) == 0)
                {
                    break;
                }

                curEdge = null;
            }

            return retVal;
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

        // set the is complete variable whenever the graph is changed
        // sets true if the current graph has a path that goes through all cities exactly once.
        private void SetIsComplete()
        {
            if(hasNotBeenFirst.Count == 0 && hasNotBeenSecond.Count == 0)
            {
                Console.WriteLine("IsComplete! hasNotBeenFirst: {0} hasNotBeenSecond: {1}", MyListToString(hasNotBeenFirst), MyListToString(hasNotBeenSecond));
                isComplete = true;
                return;
            }

            isComplete = false;
            return;
        }

        private string MyListToString(List<City> list)
        {
            string str = "";
            foreach(City city in list)
            {
                str += city.ID + ", ";
            }
            return str;
        }

        private class Edge
        {
            public City city1 { get; set; }
            public City city2 { get; set; }
        }
    }
}
