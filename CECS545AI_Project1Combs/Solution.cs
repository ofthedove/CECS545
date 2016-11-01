using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECS545AI_Project1Combs
{
    class Solution
    {
        private List<SolutionItem> path;

        private Solution()
        {
            path = new List<SolutionItem>();
        }

        /// <summary>
        /// Create a solution from a single strand of genes where value represents the city and position represents the location in the path
        /// </summary>
        /// <param name="strand1"></param>
        /// <returns></returns>
        public static Solution CreateSolutionFromCityStrand(Strand strand1)
        {
            Solution solution = new Solution();
            strand1.Sort();
            for (int i = 0; i < strand1.Length; i++)
            {
                SolutionItem item = new SolutionItem(i, strand1[i].Value);
                solution.AddItem(item);
            }
            return solution;
        }

        /// <summary>
        /// Create a solution from a single strand of genes where value is a sort order and position represents the city id
        /// </summary>
        /// <param name="strand1"></param>
        /// <returns></returns>
        public static Solution CreateSolutionFromPositionStrand(Strand strand1)
        {
            Solution solution = new Solution();
            strand1.Sort(Gene.sortValueAscending()); // Sort by value ascending
            for (int i = 0; i < strand1.Length; i++)
            { // The value is the sort order. The origonal position is the city id
                SolutionItem item = new SolutionItem(i, strand1[i].Position);
                solution.AddItem(item);
            }
            return solution;
        }

        private void AddItem(SolutionItem item)
        {
            path.Add(item);
        }

        public Graph BuildGraph(Map map)
        {
            Graph graph = map.CreateGraph();
            path.Sort();
            for(int i = 0; i < path.Count; i++)
            {
                SolutionItem item = path[i];
                SolutionItem nextItem = path[(i + 1) % path.Count];
                City city1 = map.GetCityByID(item.CityID);
                City city2 = map.GetCityByID(nextItem.CityID);
                Edge edge = new Edge(city1, city2);
                graph.AddEdge(edge);
            }
            return graph;
        }
        private class SolutionItem : IComparable
        {
            private int position;
            private int cityID;
            
            public int CityID
            {
                get { return cityID; }
            }
            
            public int Position
            {
                get { return position; }
            }

            public SolutionItem(int positionIn, int cityIDIn)
            {
                position = positionIn;
                cityID = cityIDIn;
            }

            public int CompareTo(object obj)
            {
                if (obj is SolutionItem)
                {
                    return position.CompareTo((obj as SolutionItem).Position);
                }
                else
                {
                    return -1;
                }
            }
        }
    }
}
