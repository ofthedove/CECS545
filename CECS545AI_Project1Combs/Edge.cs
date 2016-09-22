using System;

namespace CECS545AI_Project1Combs
{
    /// <summary>
    /// Edge from City 1 to City 2.
    /// Calculates distance once and stores
    /// </summary>
    class Edge : IComparable
    {
        /// <summary>
        /// First endpoint of edge
        /// </summary>
        private City city1;
        /// <summary>
        /// Second endpoint of edge
        /// </summary>
        private City city2;
        /// <summary>
        /// Distance between cities; weight of edge
        /// </summary>
        private double distance;

        /// <summary>
        /// Get's first endpoint city
        /// </summary>
        public City City1
        {
            get { return city1;  }
        }
        /// <summary>
        /// Get's second endpoint city
        /// </summary>
        public City City2
        {
            get { return city2; }
        }
        /// <summary>
        /// Get's distance between city endpoints
        /// </summary>
        public double Distance
        {
            get { return distance; }
        }

        /// <summary>
        /// Sets all paramaters for edge, paramaters cannot be changed later
        /// </summary>
        /// <param name="city1In">The first city on the edge</param>
        /// <param name="city2In">The second city on the edge</param>
        public Edge(City city1In, City city2In)
        {
            city1 = city1In;
            city2 = city2In;
            distance = DistanceBetween(city1, city2);
        }

        /// <summary>
        /// Calculates distance between two cities to be used as weight of edge
        /// </summary>
        /// <param name="city1">First city</param>
        /// <param name="city2">Second city</param>
        /// <returns>Distance between input cities</returns>
        private static double DistanceBetween(City city1, City city2)
        {
            return Math.Sqrt(Math.Pow((city2.X - city1.X), 2) + Math.Pow((city2.Y - city1.Y), 2));
        }

        /// <summary>
        /// Two edges are equivalent if they have the same endpoints
        /// </summary>
        /// <param name="obj">The city we're comparing ourselves to</param>
        /// <returns>0 on equivalent, non-zero otherwise</returns>
        public int CompareTo(object obj)
        {
            Edge e = (Edge)obj;
            int sameOri = e.City1.CompareTo(City1) + e.City2.CompareTo(City2);
            int diffOri = e.City1.CompareTo(City2) + e.City2.CompareTo(City1);
            return Math.Min(sameOri, diffOri);
        }
    }
}
